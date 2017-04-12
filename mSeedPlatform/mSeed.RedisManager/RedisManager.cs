using System;
using System.Text;
using System.Collections.Generic;
using ServiceStack.Text;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System.Diagnostics;
using ServiceStack.Text.Json;

namespace mSeed.RedisManager
{
    public partial class mJsonSerializer
    {
        public static string RemoveJson(string json, string key)
        {
            JsonObject setJson = JsonObject.Parse(json);
            
            if (setJson == null)
                setJson = new JsonObject();

            if (setJson.ContainsKey(key))
                setJson.Remove(key);

            return setJson.ToJson();
        }

        public static string RemoveJsonAll(string json, string key)
        {
            JsonObject setJson = JsonObject.Parse(json);

            if (setJson == null)
                setJson = new JsonObject();

            var keys = setJson.Values;
            foreach (string jsonkey in keys)
            {
                if(JsonUtils.IsJsArray(setJson[jsonkey]))
                {
                    setJson[jsonkey] = RemoveJsonAll(setJson[jsonkey], key);
                }else if(jsonkey.Equals(key))
                {
                    setJson.Remove(key);
                }
            }

            return setJson.ToJson();
        }

        public static string GetJsonValue(string json, string key, int arrayindex = 0)
        {
            if (JsonUtils.IsJsArray(json))
            {
                JsonArrayObjects setJsonArray = JsonArrayObjects.Parse(json);
                if (setJsonArray == null)
                    return string.Empty;

                return setJsonArray[arrayindex].Get(key);
            }
            else if (JsonUtils.IsJsObject(json))
            {
                JsonObject setJson = JsonObject.Parse(json);
                
                if (setJson == null)
                    return string.Empty;

                return setJson.Get(key);
            }

            return string.Empty;
        }

        public static string AddJsonArray(string baseArray, string json)
        {
            JsonArrayObjects setJsonArray = JsonArrayObjects.Parse(baseArray);

            if (setJsonArray == null)
                setJsonArray = new JsonArrayObjects();

            JsonObject setJson = JsonObject.Parse(json);

            if (setJson == null)
                setJson = new JsonObject();

            setJsonArray.Add(setJson);
            return setJsonArray.ToJson();
        }

        public static string MergeJson(string json_first, string json_second)
        {
            JsonObject first = JsonObject.Parse(json_first);
            JsonObject that = JsonObject.Parse(json_second);
            foreach (var entry in that)
            {
                var exists = first.ContainsKey(entry.Key);
                if (exists)
                {
                    var otherThis = JsonObject.Parse(first.GetUnescaped(entry.Key));
                    var otherThat = JsonObject.Parse(that.GetUnescaped(entry.Key));
                    first[entry.Key] = MergeJson(otherThis.ToJson(), otherThat.ToJson()).ToJson();
                }
                else
                {
                    first[entry.Key] = entry.Value;
                }
            }
            return first.ToJson();
        }

        public static string ToJsonString(object obj)
        {
            return JsonSerializer.SerializeToString(obj);
        }
                
        public static T JsonToObject<T>(string json)
        {
            try
            {
                T retObj = JsonSerializer.DeserializeFromString<T>(json);
                return retObj;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return default(T);
            }
        }

        public static JsonObject AddJson(JsonObject json, string key, string value)
        {
            if (json == null)
                json = new JsonObject();
            json.Add(key, value);
            return json;
        }

        public static string AddJson(string json, string key, string value)
        {
            while (json.Contains("\\\\"))
            {
                json = json.Replace("\\\\", "\\");
            }            
            JsonObject setJson = JsonObject.Parse(json);
            
            if (setJson == null)
                setJson = new JsonObject();
            setJson.Add(key, value);
            return setJson.ToJson();
        }

        public static Dictionary<string, string> JsonToDictionary(string json)
        {
            JsonObject setJson = JsonObject.Parse(json);
            return setJson.ToDictionary();
        }

        public static string SetJsonKeyLow(string json)
        {
            if (!(ServiceStack.Text.Json.JsonUtils.IsJsObject(json) || ServiceStack.Text.Json.JsonUtils.IsJsArray(json)))
                return json;

            Dictionary<string, string> jsonData = JsonToDictionary(json);
            foreach (KeyValuePair<string, string> data in jsonData)
            {
                if (!ServiceStack.Text.Json.JsonUtils.IsJsArray(data.Value))
                {
                    json = json.Replace(data.Key, data.Key.ToLower());
                }
                else
                {
                    string[] arrData = data.Value.Substring(1, data.Value.Length - 2).Split('}');
                    json = json.Replace(data.Key, data.Key.ToLower());
                    foreach (string set in arrData)
                    {
                        if (!string.IsNullOrEmpty(set))
                        {
                            string setData = set.Replace(",{", "{") + "}";
                            json = json.Replace(setData, SetJsonKeyLow(setData));
                        }
                    }
                }
            }
            return json;
        }

        // do not use yet ... has some bugs :(
        private static string JsonKeyToLower(string json)
        {
            if(!(ServiceStack.Text.Json.JsonUtils.IsJsObject(json) || ServiceStack.Text.Json.JsonUtils.IsJsArray(json)))
                return json;
            if (ServiceStack.Text.Json.JsonUtils.IsJsArray(json))
            {
                JsonArrayObjects jsonarr = JsonArrayObjects.Parse(json);
                JsonArrayObjects setArrObj = new JsonArrayObjects();
                foreach (JsonObject setJson in jsonarr)
                {
                    JsonObject that = JsonObject.Parse(json);
                    var keys = that.Keys;
                    int n = keys.Count;
                    string lowKey;
                    foreach (var set in keys)
                    {
                        var key = set;
                        if (key.Equals((lowKey = key.ToLower())))
                        {
                            that[key] = JsonKeyToLower(that[key].ToJson());
                            continue;
                        }
                        that[lowKey] = JsonKeyToLower(that[key].ToJson());
                        that.Remove(key);
                    }
                    setArrObj.Add(that);
                }
                return setArrObj.ToJson();
            }
            else
            {
                JsonObject that = JsonObject.Parse(json);
                var keys = that.Keys;
                int n = keys.Count;
                string lowKey;
                foreach (var set in keys)
                {
                    var key = set;
                    if (key.Equals((lowKey = key.ToLower())))
                    {
                        that[key] = JsonKeyToLower(that[key].ToJson());
                        continue;
                    }
                    that[lowKey] = JsonKeyToLower(that[key].ToJson());
                    that.Remove(key);
                }
                return that.ToJson();
            }

            //if (ServiceStack.Text.Json.JsonUtils.IsJsObject(json))
            //{
            //    JsonObject that = JsonObject.Parse(json);
            //    JsonObject first = new JsonObject();

            //    foreach (var entry in that)
            //    {
            //        if (ServiceStack.Text.Json.JsonUtils.IsJsObject(that.GetUnescaped(entry.Key))
            //            || ServiceStack.Text.Json.JsonUtils.IsJsArray(that.GetUnescaped(entry.Key)))
            //            first[entry.Key.ToLower()] = JsonKeyToLower(that.GetUnescaped(entry.Key));
            //        else
            //            first[entry.Key.ToLower()] = first.GetUnescaped(entry.Key);
            //    }
            //    return first.ToJson();
            //}
            //else if (ServiceStack.Text.Json.JsonUtils.IsJsArray(json))
            //{
            //    JsonArrayObjects setJson = JsonArrayObjects.Parse(json);
            //    JsonObject[] arrObj = setJson.ToArray();
            //    JsonObject[] setArrObj = new JsonObject[arrObj.Length];
            //    int idxCount = 0;
            //    foreach (JsonObject setArr in arrObj)
            //    {
            //        JsonObject first = new JsonObject();

            //        if (ServiceStack.Text.Json.JsonUtils.IsJsObject(first.GetUnescaped(setArr.ToJson()))
            //            || ServiceStack.Text.Json.JsonUtils.IsJsArray(first.GetUnescaped(setArr.ToJson())))
            //            first = JsonObject.Parse(JsonKeyToLower(first.GetUnescaped(setArr.ToJson())).ToJson());
            //        else
            //            first = JsonObject.Parse(first.GetUnescaped((setArr.ToJson().ToJson())));

            //        setArrObj[idxCount] = first;
            //    }
            //    return setArrObj.ToJson();
            //}
            //else
            //    return json;
        }
    }
    public delegate void LOGFUNC(string e);

    /// <summary>
    /// Redis control class - connection pool, get/set obj
    /// </summary>
    public partial class mRedis : IDisposable
    {
        private LOGFUNC elog = null;    // for log delgate

        public LOGFUNC Elog
        {
            get { return elog; }
            set { elog = value; }
        }

        private void ErrorLog(string error)
        {
            //if (error.Contains("exception"))
            //{
            //    redisManager.resetPooledClient();
            //}

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("{0} +-> backtrace \n", error));

            if (DebugTrace)
            {
                StackTrace st = new StackTrace(true);
                string stackIndent = "";
                for (int i = 1; i < st.FrameCount; i++)
                {
                    // Note that at this level, there are four
                    // stack frames, one for each method invocation.
                    StackFrame sf = st.GetFrame(i);
                    sb.Append(string.Format(stackIndent + " Method: {0}\n", sf.GetMethod()));
                    sb.Append(string.Format(stackIndent + " File: {0}\n", sf.GetFileName()));
                    sb.Append(string.Format(stackIndent + " Line Number: {0}\n", sf.GetFileLineNumber()));
                    stackIndent += "  ";
                }
            }

            if (elog != null)
                elog(sb.ToString());
            Debug.WriteLine(sb.ToString());
        }

        int DefaultRetryCount = 0;

        public int SetRetryCount
        {
            set { DefaultRetryCount = value; }
        }
        static float DefaultRetryTime = 0.3f;
        bool DebugTrace = false;
        // default redis ip - for test
        string InitHost= "localhost";
        // default redis port
        string Initport = "6379";
        public const int defaultTimeOut = 1500;    // connection timeout ms

        // PooledRedisClientManager was always new connection (bug?). so CustomRedisPooledClient instead.
        //PooledRedisClientManager redisManager = new PooledRedisClientManager();

        // custum redis pool manager
        CustomRedisPooledClient redisManager = null;

        public bool redisConnActive
        {
            get { return redisManager.redisConnActive; }
        }

        // RedisClient Interface;
        //public IRedisClient redis = null;

        /// <summary> destructor - close all redis connection </summary>
        ~mRedis()
        {
            Dispose();
        }

        /// <summary> constructor - create CustomRedisPooledClient </summary>
        public mRedis()
        {
            elog = ErrorLog;
            //Debug.Indent();
            JsConfig.IncludeNullValues = true;
            redisManager = GetRedisManager();
        }

        /// <summary> Init class and create default connection </summary>
        public void Init(string host) { Init(host, Initport); }
        public void Init(string host, string port)
        {
            InitHost = host;
            Initport = port;
            RedisConn(host, port);
        }

        private string PrefixTag= "TAG";

        /// <summary>
        /// set prefix tag for redis 
        /// </summary>
        public void SetPrefixTag(string prefix)
        {
            PrefixTag = prefix;
        }

        // default time = 600 sec;
        private TimeSpan DefaultExpireTime = new TimeSpan(0, 0, 300);

        /// <summary>
        /// set default redis expire time
        /// </summary>
        /// <param name="second">set second</param>
        public void SetExprireTime(int second)
        {
            DefaultExpireTime = new TimeSpan(0, 0, second);
        }

        /// <summary> get CustomRedisPooledClient instance; </summary>
        private CustomRedisPooledClient GetRedisManager()
        {
            //if (redisManager == null)
            //    redisManager = new CustomRedisPooledClient();

            redisManager = CustomRedisPooledClient.GetPoolManager();
            return redisManager;
        }

        /// <summary>
        /// make redisConnection by InitHost, InitPort
        /// </summary>
        public void RedisConn()
        {
            //if (redis != null)
            //    return;

            RedisConn(InitHost, Initport, string.Empty, defaultTimeOut);
        }

        /// <summary>
        ///  make redis connection by setIP, setPort
        /// </summary>
        /// <param name="setIP">redis server ip</param>
        /// <param name="setPort">redis server port</param>
        public void RedisConn(string setIP, string setPort, int defTimeOut = defaultTimeOut ) { RedisConn(setIP, setPort, string.Empty, defTimeOut); }
        /// <summary>
        ///  make redis connection by setIP, setPort, alias serverKey
        /// </summary>
        /// <param name="setIP">redis server ip</param>
        /// <param name="setPort">redis server port</param>
        /// <param name="serverKey">redis server alias in redis connection list</param>
        public void RedisConn(string setIP, string setPort, string serverKey, int defTimeOut = defaultTimeOut )
        {
            try
            {
                if (string.IsNullOrEmpty(setIP))
                {
                    throw new Exception("setIP is empty!");
                }

                string SetHosts = string.Format("{0}:{1}", setIP, setPort);
                redisManager = null;

                redisManager = GetRedisManager();
                if (string.IsNullOrEmpty(serverKey))
                    serverKey = "main";

                redisManager.CreateClient(serverKey, SetHosts, defTimeOut);
            }
            catch (Exception e)
            {
                ErrorLog(e.Message);
            }
        }

        /// <summary> get first connection in redis mananger </summary>
        private IRedisClient GetRedisClient() { return GetRedisClient(string.Empty); }
        /// <summary> get connections such as Key in redis mananger </summary>
        private IRedisClient GetRedisClient(string key)
        {
            try {
                if (string.IsNullOrEmpty(key))
                {
                    return redisManager.GetClient();
                }
                else
                {
                    return redisManager.GetClient(key);
                }

                //if (redis == null)
                //    throw new Exception("redis client not connected");

                //return null; 
            }
            catch (Exception e)
            {
                //redis = null;
                ErrorLog(e.Message);
                return null;
            }
        }

        /// <summary> close all connection in redis connection list</summary>
        public void Dispose()
        {
            EndTransaction();
            //redisManager.Dispose();
        }
        
        /// <summary> close all connection in redis connection list</summary>
        public void RedisClose()
        {
            //redisManager.Dispose();
        }

        /// <summary> close connections such as Key in redis connection list</summary>
        public void Dispose(string serverKey)
        {
            redisManager.Dispose(serverKey);
        }

        /// <summary> 
        /// (not surrport yet) use direct command to redis server
        /// </summary>
        private void Command(string command)
        {
        }

        Dictionary<string, IRedisTransaction> callTransaction = new Dictionary<string, IRedisTransaction>();
        /// <summary> 
        /// use transaction for command
        /// </summary>
        private void CreateTransaction(string serverKey)
        {
            using (IRedisClient redis = GetRedisClient(serverKey))
            {
                if (redis != null)
                    callTransaction.Add(serverKey, redis.CreateTransaction());
            }
        }

        public void RedisFlush(string serverKey)
        {
            using (IRedisClient redis = GetRedisClient(serverKey))
            {
                if (redis != null)
                    redis.FlushAll();
            }
        }

        /// <summary> 
        /// transaction commit
        /// </summary>
        private void EndTransaction()
        {
            foreach (KeyValuePair<string, IRedisTransaction> trans in callTransaction)
            {
                trans.Value.Commit();
                trans.Value.Dispose();
            }

            callTransaction.Clear();
        }
    }
}