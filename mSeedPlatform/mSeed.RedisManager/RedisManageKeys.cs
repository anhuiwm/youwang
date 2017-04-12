using System;
using System.Text;
using System.Collections.Generic;
using ServiceStack.Text;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System.Diagnostics;

namespace mSeed.RedisManager
{
    // callback for keys
    public delegate T CALLBACK<T>();
    public delegate T CALLBACK<T, ArgType1>(ArgType1 v1);
    public delegate T CALLBACK<T, ArgType1, ArgType2>(ArgType1 v1, ArgType2 v2);
    public delegate T CALLBACK<T, ArgType1, ArgType2, ArgType3>(ArgType1 v1, ArgType2 v2, ArgType3 v3);
    public delegate T CALLBACK<T, ArgType1, ArgType2, ArgType3, ArgType4>(ArgType1 v1, ArgType2 v2, ArgType3 v3, ArgType4 v4);
    public delegate T CALLBACK<T, ArgType1, ArgType2, ArgType3, ArgType4, ArgType5>(ArgType1 v1, ArgType2 v2, ArgType3 v3, ArgType4 v4, ArgType5 v5);

    public partial class mRedis : IDisposable
    {
        private string PrefixKeys = "Key";

        /// <summary>
        /// set prefix tag for redis keys 
        /// </summary>
        public void SetPrefixKey(string prefix)
        {
            PrefixKeys = prefix;
        }
        /// <summary>
        /// get prefix tag for redis keys 
        /// </summary>
        private string GetPrefixKey(string key)
        {
            return string.Format("{0}:{1}_{2}", PrefixTag, PrefixKeys, key);
        }

        ///<summary> get redis object string format</summary>
        ///<param name="serverKey">alias key in redis connection</param>
        ///<param name="key">stored redis key</param>
        private string GetEntryKey(string serverKey, string key)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    string json_text = string.Empty;
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        string getKey = GetPrefixKey(key);
                        json_text = redis.GetValue(getKey);
                        //json_text = redis.Get<string>(getKey);

                        //byte[] bytes = Encoding.UTF8.GetBytes(json_text);
                        //json_text = Encoding.Default.GetString(bytes);

                        //json_text = redis.Get<string>(GetPrefixKey(key));
                        //json_text = redis.GetEntry(GetPrefixKey(key));
                    }
                     

                    if (string.IsNullOrEmpty(json_text))
                        json_text = string.Empty;

                    return json_text;
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to get Key object exception msg = " + e.Message);
                return string.Empty;
            }
        }

        ///<summary> set object to redis (object serialize) </summary>
        ///<param name="key">stored redis key</param>
        ///<param name="value">stored redis value (use json string)</param>
        public bool SetObj(string key, object value) { return SetObj(string.Empty, key, value, DefaultExpireTime); }
        ///<param name="exprie">set expire time </param>
        public bool SetObj(string key, object value, TimeSpan expire) { return SetObj(string.Empty, key, value, expire); }
        ///<param name="serverKey">alias key in redis connection</param>
        public bool SetObj(string serverKey, string key, object value) { return SetObj(serverKey, key, value, DefaultExpireTime); }
        ///<param name="exprie">set expire time </param>
        public bool SetObj(string serverKey, string key, object value, TimeSpan expire)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        var serialized = JsonSerializer.SerializeToString(value);
                        string json_text = serialized.ToString();

                        //byte[] bytes = Encoding.Default.GetBytes(json_text);
                        //json_text = Encoding.UTF8.GetString(bytes);

                        expire = expire.TotalSeconds <= 0 ? new TimeSpan(365, 24, 60, 60) : expire;
                        
                        redis.SetEntry(GetPrefixKey(key), json_text, expire);
                        return true;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to set key object exception msg = " +  e.Message);
                return false;
            }
        }

        /// <summary>Key expire timer set </summary>
        /// <param name="key">stored redis key</param>
        ///<param name="second">set expire time (second)</param>
        public bool KeyExpireTimeSet(string key, int second) { return KeyExpireTimeSet(string.Empty, key, new TimeSpan(0, 0, second)); }
        /// <param name="serverKey">alias key in redis connection</param>
        public bool KeyExpireTimeSet(string serverKey, string key, int second) { return KeyExpireTimeSet(serverKey, key, new TimeSpan(0, 0, second)); }
        ///<param name="exprie">set expire time </param>
        public bool KeyExpireTimeSet(string key, TimeSpan expire) { return KeyExpireTimeSet(string.Empty, key, expire); }
        /// <param name="serverKey">alias key in redis connection</param>
        public bool KeyExpireTimeSet(string serverKey, string key, TimeSpan expire)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using(IRedisClient redis = GetRedisClient(serverKey))
                    {
                        expire = expire.TotalSeconds <= 0 ? new TimeSpan(365, 24, 60, 60) : expire;

                        redis.ExpireEntryIn(GetPrefixKey(key), expire);
                        return true;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to set key object exception msg = " + e.Message);
                return false;
            }
        }

        ///<summary> remove object to redis </summary>
        ///<param name="key">stored redis key</param>
        public bool RemoveObj(string key) { return RemoveObj(string.Empty, key); }
        ///<param name="serverKey">alias key in redis connection</param>
        public bool RemoveObj(string serverKey, string key)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        redis.Remove(GetPrefixKey(key));
                        return true;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to remove object exception msg = " + e.Message);
                return false;
            }
        }

        ///<summary>return object from redis (cast templete T then deserialize)</summary>
        public T GetObj<T>(string key) { return GetObj<T>(string.Empty, key); }
        ///<param name="serverKey">alias key in redis connection</param>
        public T GetObj<T>(string serverKey, string key)
        {
            string json_text = GetEntryKey(serverKey, key);
            T retObj = default(T);
            if (!string.IsNullOrEmpty(json_text))
            {
                try
                {
                    return Retry.RetryMethod(() =>
                    {
                        retObj = JsonSerializer.DeserializeFromString<T>(json_text);
                        return retObj;
                    }, DefaultRetryCount, DefaultRetryTime);                    
                }
                catch (Exception e)
                {
                    ErrorLog("fail to set key object exception msg = " + e.Message);
                    return retObj;
                }
            }
            return retObj;
        }

        ///<summary>return object from redis with callback method for set call (cast templete T then deserialize)</summary>
        public T GetObj<T>(string key, CALLBACK<T> cbfunc) { return GetObj<T>(string.Empty, key, cbfunc); }
        ///<param name="serverKey">alias key in redis connection</param>
        public T GetObj<T>(string serverKey, string key, CALLBACK<T> cbfunc)
        {
            string json_text = GetEntryKey(serverKey, key);
            T retObj = default(T);
            if (!string.IsNullOrEmpty(json_text))
            {
                try
                {
                    return Retry.RetryMethod(() =>
                    {
                       return retObj = JsonSerializer.DeserializeFromString<T>(json_text);
                    }, DefaultRetryCount, DefaultRetryTime);
                }
                catch (Exception e)
                {
                    ErrorLog("fail to set key object exception msg = " + e.Message);
                }
            }
            else
            {
                try
                {
                    retObj = cbfunc();
                    SetObj(key, retObj);
                }
                catch (Exception e)
                {
                    ErrorLog("callback function fail exception msg = " + e.Message);
                }
            }
            return retObj;
        }

        ///<summary>return object from redis with callback(arg) method for set call (cast templete T then deserialize)</summary>
        public T GetObj<T, ArgType1>(string key, CALLBACK<T, ArgType1> cbfunc, ArgType1 v1) { return GetObj<T, ArgType1>(string.Empty, key, cbfunc, v1); }
        ///<param name="serverKey">alias key in redis connection</param>
        public T GetObj<T, ArgType1>(string serverKey, string key, CALLBACK<T, ArgType1> cbfunc, ArgType1 v1)
        {
            string json_text = GetEntryKey(serverKey, key);
            T retObj = default(T);
            if (!string.IsNullOrEmpty(json_text))
            {
                try
                {
                    return Retry.RetryMethod(() =>
                    {
                        return retObj = JsonSerializer.DeserializeFromString<T>(json_text);
                    }, DefaultRetryCount, DefaultRetryTime); 
                    
                }
                catch (Exception e)
                {
                    ErrorLog("fail to set key object exception msg = " + e.Message);
                }
            }
            else
            {
                try
                {
                    retObj = (T)cbfunc(v1);
                    SetObj(key, retObj);
                }
                catch (Exception e)
                {
                    ErrorLog("callback function fail exception msg = " + e.Message);
                }
            }
            return retObj;
        }

        ///<summary>return object from redis with callback(2 arg) method for set call (cast templete T then deserialize)</summary>
        public T GetObj<T, ArgType1, ArgType2>(string key, CALLBACK<T, ArgType1, ArgType2> cbfunc, ArgType1 v1, ArgType2 v2) { return GetObj<T, ArgType1, ArgType2>(string.Empty, key, cbfunc, v1, v2);  }
        ///<param name="serverKey">alias key in redis connection</param>
        public T GetObj<T, ArgType1, ArgType2>(string serverKey, string key, CALLBACK<T, ArgType1, ArgType2> cbfunc, ArgType1 v1, ArgType2 v2)
        {
            string json_text = GetEntryKey(serverKey, key);
            T retObj = default(T);
            if (!string.IsNullOrEmpty(json_text))
            {
                try
                {
                    return Retry.RetryMethod(() =>
                    {
                        return retObj = JsonSerializer.DeserializeFromString<T>(json_text);
                    }, DefaultRetryCount, DefaultRetryTime);
                }
                catch (Exception e)
                {
                    ErrorLog("fail to set key object exception msg = " + e.Message);
                }
            }
            else
            {
                try
                {
                    retObj = (T)cbfunc(v1, v2);
                    SetObj(key, retObj);
                }
                catch (Exception e)
                {
                    ErrorLog("callback function fail exception msg = " + e.Message);
                }
            }
            return retObj;
        }
        ///<summary>return object from redis with callback(3 arg) method for set call (cast templete T then deserialize)</summary>
        public T GetObj<T, ArgType1, ArgType2, ArgType3>(string key, CALLBACK<T, ArgType1, ArgType2, ArgType3> cbfunc, ArgType1 v1, ArgType2 v2, ArgType3 v3) { return GetObj<T, ArgType1, ArgType2, ArgType3>(string.Empty, key, cbfunc, v1, v2, v3);  }
        ///<param name="serverKey">alias key in redis connection</param>
        public T GetObj<T, ArgType1, ArgType2, ArgType3>(string serverKey, string key, CALLBACK<T, ArgType1, ArgType2, ArgType3> cbfunc, ArgType1 v1, ArgType2 v2, ArgType3 v3)
        {
            string json_text = GetEntryKey(serverKey, key);
            T retObj = default(T);
            if (!string.IsNullOrEmpty(json_text))
            {
                try
                {
                    return Retry.RetryMethod(() =>
                    {
                        return retObj = JsonSerializer.DeserializeFromString<T>(json_text);
                    }, DefaultRetryCount, DefaultRetryTime);
                }
                catch (Exception e)
                {
                    ErrorLog("fail to set key object exception msg = " + e.Message);
                }
            }
            else
            {
                try
                {
                    retObj = (T)cbfunc(v1, v2, v3);
                    SetObj(key, retObj);
                }
                catch (Exception e)
                {
                    ErrorLog("callback function fail exception msg = " + e.Message);
                }
            }
            return retObj;
        }

        ///<summary>return object from redis with callback(4 arg) method for set call (cast templete T then deserialize)</summary>
        public T GetObj<T, ArgType1, ArgType2, ArgType3, ArgType4>(string key, CALLBACK<T, ArgType1, ArgType2, ArgType3, ArgType4> cbfunc, ArgType1 v1, ArgType2 v2, ArgType3 v3, ArgType4 v4) { return GetObj<T, ArgType1, ArgType2, ArgType3, ArgType4>(string.Empty, key, cbfunc, v1, v2, v3, v4);  }
        ///<param name="serverKey">alias key in redis connection</param>
        public T GetObj<T, ArgType1, ArgType2, ArgType3, ArgType4>(string serverKey, string key, CALLBACK<T, ArgType1, ArgType2, ArgType3, ArgType4> cbfunc, ArgType1 v1, ArgType2 v2, ArgType3 v3, ArgType4 v4)
        {
            string json_text = GetEntryKey(serverKey, key);
            T retObj = default(T);
            if (!string.IsNullOrEmpty(json_text))
            {
                try
                {
                    return Retry.RetryMethod(() =>
                    {
                        return retObj = JsonSerializer.DeserializeFromString<T>(json_text);
                    }, DefaultRetryCount, DefaultRetryTime);
                }
                catch (Exception e)
                {
                    ErrorLog("fail to set key object exception msg = " + e.Message);
                }
            }
            else
            {
                try
                {
                    retObj = (T)cbfunc(v1, v2, v3, v4);
                    SetObj(key, retObj);
                }
                catch (Exception e)
                {
                    ErrorLog("callback function fail exception msg = " + e.Message);
                }
            }
            return retObj;
        }
        ///<summary>return object from redis with callback(5 arg) method for set call (cast templete T then deserialize)</summary>
        public T GetObj<T, ArgType1, ArgType2, ArgType3, ArgType4, ArgType5>(string key, CALLBACK<T, ArgType1, ArgType2, ArgType3, ArgType4, ArgType5> cbfunc, ArgType1 v1, ArgType2 v2, ArgType3 v3, ArgType4 v4, ArgType5 v5) { return GetObj<T, ArgType1, ArgType2, ArgType3, ArgType4, ArgType5>(string.Empty, key, cbfunc, v1, v2, v3, v4, v5); }
        ///<param name="serverKey">alias key in redis connection</param>
        public T GetObj<T, ArgType1, ArgType2, ArgType3, ArgType4, ArgType5>(string serverKey, string key, CALLBACK<T, ArgType1, ArgType2, ArgType3, ArgType4, ArgType5> cbfunc, ArgType1 v1, ArgType2 v2, ArgType3 v3, ArgType4 v4, ArgType5 v5)
        {
            string json_text = GetEntryKey(serverKey, key);
            T retObj = default(T);
            if (!string.IsNullOrEmpty(json_text))
            {
                try
                {
                    return Retry.RetryMethod(() =>
                    {
                        return retObj = JsonSerializer.DeserializeFromString<T>(json_text);
                    }, DefaultRetryCount, DefaultRetryTime);
                }
                catch (Exception e)
                {
                    ErrorLog("fail to set key object exception msg = " + e.Message);
                }
            }
            else
            {
                try
                {
                    retObj = (T)cbfunc(v1, v2, v3, v4, v5);
                    SetObj(key, retObj);
                }
                catch (Exception e)
                {
                    ErrorLog("callback function fail exception msg = " + e.Message);
                }
            }
            return retObj;
        }

        ///<summary>overide JsonObject to redis</summary>
        public void SetJson(string key, JsonObject jsonObj) { SetJson(string.Empty, key, jsonObj); }
        ///<param name="serverKey">alias key in redis connection</param>
        public void SetJson(string serverKey, string key, JsonObject jsonObj)
        {
            SetObj(serverKey, key, jsonObj);
        }

        ///<summary>overide GetObj for JsonObject</summary>
        public JsonObject GetJson(string key) { return GetJson(string.Empty, key); }
        ///<param name="serverKey">alias key in redis connection</param>
        public JsonObject GetJson(string serverKey, string key)
        {
            JsonObject retObj = GetObj<JsonObject>(serverKey, key);
            return retObj;
        }

        ///<summary>overide string to redis</summary>
        public void SetString(string key, string value) { SetString(string.Empty, key, value); }
        ///<param name="serverKey">alias key in redis connection</param>
        public void SetString(string serverKey, string key, string value)
        {
            SetObj(serverKey, key, value);
        }

        ///<summary>overide GetObj for string </summary>
        public string GetString(string key) { return GetString(string.Empty, key); }
        ///<param name="serverKey">alias key in redis connection</param>
        public string GetString(string serverKey, string key)
        {
            string retObj = GetEntryKey(serverKey, key);
            if (string.IsNullOrEmpty(retObj))
                retObj = string.Empty;
            return retObj;
        }
    }
}