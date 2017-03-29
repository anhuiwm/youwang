using System;
using System.Text;
using System.Collections.Generic;
using ServiceStack.Text;
using ServiceStack.Redis;
using ServiceStack.CacheAccess;
using ServiceStack.Redis.Generic;
using System.Diagnostics;
using System.Threading;

namespace mSeed.RedisManager
{
    /// <summary> reids connection pool manager </summary>
    public class CustomRedisPooledClient : IDisposable
    {
        private class RedisClientController : IDisposable
        {
            const int defaultMakeClientCount = 1;
            //int MakeClientCount = defaultMakeClientCount;

            PooledRedisClientManager innerClient = null;
            //int CurrentClientPos = 0;

            //TODO : lastConnstr => dictionary <key, connstr> Pair 
            string lastConnStr;

            public bool CreateSuccessFlag = false;

            public void ResetRedisFailOver()
            {
                if (innerClient != null)
                {
                    innerClient.Dispose();
                    CreateSuccessFlag = InitConnection(lastConnStr, defaultMakeClientCount, connectTimeout);
                }
            }

            //public RedisClientController(string connStr)
            //{
            //    CreateSuccessFlag = InitConnection(connStr, defaultMakeClientCount);
            //}
            public RedisClientController(string connStr, int timeOut = 0, int makeCount = defaultMakeClientCount)
            {
                CreateSuccessFlag = InitConnection(connStr, makeCount, timeOut);
            }

            public void resetClient()
            {
                Debug.WriteLine("ResetClientController : " + lastConnStr);

                innerClient.Dispose();
                CreateSuccessFlag = InitConnection(lastConnStr, defaultMakeClientCount);
            }

            private bool InitConnection(string connStr, int makeCount, int setTimeOut = 0)
            {
                lastConnStr = connStr;
                connectTimeout = setTimeOut;
                try{
                    if (innerClient != null)
                        innerClient.Dispose();
                    
                    innerClient = new PooledRedisClientManager(100, 100, connStr);
                    innerClient.PoolTimeout = 
                        innerClient.SocketReceiveTimeout = 
                        innerClient.SocketSendTimeout = 
                        innerClient.ConnectTimeout = setTimeOut < this.connectTimeout ? this.connectTimeout : setTimeOut;
                    int port = innerClient.GetClient().Port;
                    innerClient.GetClient().SetEntry("check_ping_pong_mseed_redis", "1");
                    
                    return true;
                    //List<string> connStrList = new List<string>();

                    //for(int makePos = 0; makePos < MakeClientCount; makePos++)
                    //{
                    //    connStrList.Add(connStr);
                    //}
                    //innerClient = new PooledRedisClientManager(connStrList.ToArray());
                }catch(Exception e)
                {
                    innerClient.Dispose();
                    Debug.WriteLine("RedisClientController exception : " + e.Message);
                    return false;
                }
            }

            int connectTimeout = 100;

            public int ConnectTimeout
            {
                get { return connectTimeout; }
                set { 
                    connectTimeout = value;
                    if (innerClient != null)
                        innerClient.ConnectTimeout = connectTimeout;
                }
            }

            public IRedisClient GetClient()
            {
                if (innerClient != null) {
                    return innerClient.GetClient();
                }
                else
                    return null;
                //CurrentClientPos = CurrentClientPos == MakeClientCount ? 0 : (CurrentClientPos+1);
                ////innerClient[CurrentClientPos].ResetSendBuffer();
                //return innerClient[CurrentClientPos];
            }

            public void Dispose()
            {
                if (innerClient != null)
                    innerClient.Dispose();

                innerClient = null;
            }
        }

        private LOGFUNC elog = null;    // for log delgate

        public LOGFUNC Elog
        {
            get { return elog; }
            set { elog = value; }
        }

        private void ErrorLog(string error)
        {
            Debug.WriteLine(error);
        }

        public bool redisConnActive
        {
            get { return redisClients.Count > 0; }
        }

        public void resetPooledClient()
        {
            foreach (KeyValuePair<string, RedisClientController> conn in redisClients)
            {
                try
                {
                    if(conn.Value != null)
                        conn.Value.Dispose();
                }
                catch
                {
                    // do something? what?
                }
            }
            redisClients.Clear();

            foreach (KeyValuePair<string, string> conn in redisClientHosts)
            {
                redisClients[conn.Key] = new RedisClientController(conn.Value, defTimeOutms);
            }
        }

        // singleton instance
        private static CustomRedisPooledClient _instance = new CustomRedisPooledClient();
        /// <summary> get CustomRedisPooledClient Instance </summary>
        public static CustomRedisPooledClient GetPoolManager()
        {
            return _instance;
        }

        private CustomRedisPooledClient()
        {
            Elog = ErrorLog;
            redisClientHosts.Clear();
            redisClients.Clear();            
        }


        ~CustomRedisPooledClient()
        {
            Dispose();
        }

        private int defTimeOutms = 1500;

        /// <summary> create new redis connection </summary>
        /// <paparam name="key">set alias connection name</paparam>
        /// <paparam name="host">connection string " string.Format("{0}:{1}", host, port) "</paparam>
        public void CreateClient(string key, string host, int defTimeOut = 1500)
        {
            if (string.IsNullOrEmpty(key))
                return;

            defTimeOutms = defTimeOut;

            try            
            {
                if (redisClientHosts.ContainsKey(key))
                {
                    if (!redisClients.ContainsKey(key))
                        redisClients[key].Dispose();

                    RedisClientController setClient = new RedisClientController(host, defTimeOutms);
                    if (setClient.CreateSuccessFlag)
                    {
                        redisClients[key] = setClient;
                        redisClientHosts[key] = host;
                        Elog("contains redisClientHosts : " + key);
                    }
                    else
                    {
                        throw new Exception("redis client not connected");
                    }
                }
                else
                {
                    if (redisClients.ContainsKey(key))
                    {
                        string SetHosts = string.Format("{0}:{1}", redisClients[key].GetClient().Host, redisClients[key].GetClient().Port);
                        if (redisClientHosts[key] != SetHosts)
                        {
                            redisClients[key].Dispose();

                            RedisClientController setClient = new RedisClientController(host, defTimeOutms);
                            if (setClient.CreateSuccessFlag)
                            {
                                redisClients[key] = setClient;
                                redisClientHosts[key] = host;
                                Elog("create other redisClients : " + key);
                            }
                            else
                            {
                                throw new Exception("redis client not connected");
                            }
                        }else
                            Elog("already connected redisClients : " + key);
                    }
                    else
                    {
                        RedisClientController setClient = new RedisClientController(host, defTimeOutms);
                        if (setClient.CreateSuccessFlag)
                        {
                            redisClients[key] = setClient;
                            redisClientHosts[key] = host;
                            Elog("create new redisClients : " + key);
                        }
                        else
                        {
                            throw new Exception("redis client not connected");
                        }
                    }
                }

                redisClients[key].ConnectTimeout = defTimeOut;
                Elog("success to redis connect : " + host + ", current connect : " + redisClients.Count);
            }
            catch (Exception e)
            {
                if (redisClients.ContainsKey(key))
                {
                    redisClients.Remove(key);
                    redisClientHosts.Remove(key);
                }
                
                ErrorLog(e.Message);
                Elog("fail to redis connect : " + host + ", current connect : " + redisClients.Count);
            }
        }

        // redis connection list
        //private Dictionary<string, RedisClient> redisClient = new Dictionary<string, RedisClient>();
        private Dictionary<string, RedisClientController> redisClients = new Dictionary<string, RedisClientController>();
        private Dictionary<string, string> redisClientHosts = new Dictionary<string, string>();

        /// <summary> get first connection in redis connection list </summary>
        public IRedisClient GetClient()
        {
            foreach (KeyValuePair<string, RedisClientController> conn in redisClients)
            {
                return conn.Value.GetClient();
            }
            return null;
        }

        /// <summary> get connections such as Key in redis connection list</summary>
        public IRedisClient GetClient(string key)
        {
            if (redisClients.ContainsKey(key))
                return redisClients[key].GetClient();
            else
                return GetClient();
        }

        public void ClientFailOverReset(string key)
        {
            if (redisClients.ContainsKey(key))
                redisClients[key].ResetRedisFailOver();
        }

        /// <summary> close all connection in redis connection list</summary>
        public void Dispose()
        {
            try
            {
                foreach (KeyValuePair<string, RedisClientController> conn in redisClients)
                {
                    if (elog != null)
                        Elog("dispose redis connect - Key : " + conn.Key + ", value : " + conn.Value.GetClient().Host);
                    //Elog("dispose redis connect - Key : " + conn.Key + ", value : " + conn.Value.RedisClientFactory.Dump());
                    conn.Value.Dispose();
                }
            }
            catch {
                Dispose();
            }
        }

        /// <summary> close connections such as Key in redis connection list</summary>
        public void Dispose(string key)
        {
            if (redisClients.ContainsKey(key))
            {
                if (elog != null)
                    elog("dispose redis connect - Key : " + key + ", value : " + redisClients[key].GetClient().Host);
                    //elog("dispose redis connect - Key : " + key + ", value : " + redisClient[key].RedisClientFactory.Dump());
                redisClients[key].Dispose();

                redisClientHosts.Remove(key);
                redisClients.Remove(key);
            }
        }
    }
}