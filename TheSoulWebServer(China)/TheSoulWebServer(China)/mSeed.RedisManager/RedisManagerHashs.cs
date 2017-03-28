using System;
using System.Text;
using System.Collections.Generic;
using ServiceStack.Text;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System.Diagnostics;

namespace mSeed.RedisManager
{
    public partial class mRedis : IDisposable
    {
        private string PrefixHashs = "Hashs";

        /// <summary>
        /// set prefix tag for redis Hashs 
        /// </summary>
        public void SetPrefixHash(string prefix)
        {
            PrefixHashs = prefix;
        }
        /// <summary>
        /// get prefix tag for redis Hashs 
        /// </summary>
        private string GetPrefixHash(string key)
        {
            return string.Format("{0}:{1}_{2}", PrefixTag, PrefixHashs, key);
        }

        /// <summary>set Field to set (with key, Field name) in redis connection</summary>
        /// <param name="key">stored redis key</param>
        /// <param name="Field">Field key in set</param>
        public bool SetHashField(string key, string field, object value) { return SetHashField(string.Empty, key, field, value); }
        /// <param name="serverKey">alias key in redis connection</param>
        public bool SetHashField(string serverKey, string key, string field, object value)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        var serialized = JsonSerializer.SerializeToString(value);
                        string json_text = serialized.ToString();

                        redis.SetEntryInHash(GetPrefixHash(key), field, json_text);
                        SetExpireTimeHash(serverKey, key);
                        return true;
                    }
                    //else
                    //    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to set Hashs object exception msg = " + e.Message);
                return false;
            }
        }

        ///<summary> remove hash in redis </summary>
        ///<param name="key">stored redis key</param>
        public bool RemoveHash(string key) { return RemoveHash(string.Empty, key); }
        ///<param name="serverKey">alias key in redis connection</param>
        public bool RemoveHash(string serverKey, string key)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        redis.Remove(GetPrefixHash(key));
                        return true;
                    }
                    //else
                    //    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to remove object exception msg = " + e.Message);
                return false;
            }
        }

        /// <summary>Get the number of field in hash( default redis connection )</summary>
        /// <param name="key"></param>
        public long GetHashCount(string key) { return GetHashCount(string.Empty, key); }
        /// <param name="serverKey"></param>
        public long GetHashCount(string serverKey, string key)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        return redis.GetHashCount(GetPrefixHash(key));
                    }
                    //else
                    //    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to get Hashs object ListCount exception msg = " + e.Message);
                return 0;
            }
        }


        ///<summary> get hash Field value object to string format</summary>
        ///<param name="serverKey">alias key in redis connection</param>
        ///<param name="key">stored redis key</param>
        private string GetEntryField(string serverKey, string key, string field)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        string json_text = string.Empty;
                        json_text = redis.GetValueFromHash(GetPrefixHash(key), field);
                        if (string.IsNullOrEmpty(json_text))
                            json_text = string.Empty;

                        return json_text;
                    }
                    //else
                    //    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to get Hashs object  object exception msg = " + e.Message);
                return string.Empty;
            }
        }

        /// <summary>Set expire timer set </summary>
        /// <param name="key">stored redis key</param>
        public bool SetExpireTimeHash(string key) { return SetExpireTimeHash(string.Empty, key, DefaultExpireTime); }
        ///<param name="second">set expire time (second)</param>
        public bool SetExpireTimeHash(string key, int second) { return SetExpireTimeHash(string.Empty, key, new TimeSpan(0, 0, second)); }
        ///<param name="exprie">set expire time </param>
        public bool SetExpireTimeHash(string key, TimeSpan expire) { return SetExpireTimeHash(string.Empty, key, expire); }
        public bool SetExpireTimeHash(string serverKey, string key) { return SetExpireTimeHash(serverKey, key, DefaultExpireTime); }
        /// <param name="serverKey">alias key in redis connection</param>
        public bool SetExpireTimeHash(string serverKey, string key, TimeSpan expire)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        expire = expire.TotalSeconds <= 0 ? new TimeSpan(365, 24, 60, 60) : expire;

                        redis.ExpireEntryIn(GetPrefixHash(key), expire);
                        return true;
                    }
                    //else
                    //    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to set Hashs object exception msg = " + e.Message);
                return false;
            }
        }

        ///<summary>return object from redis (cast templete T then deserialize)</summary>
        public string GetHashFieldString(string key, string field) { return GetHashFieldString(string.Empty, key, field); }
        ///<param name="serverKey">alias key in redis connection</param>
        public string GetHashFieldString(string serverKey, string key, string field)
        {
            return GetEntryField(serverKey, key, field);
        }

        ///<summary>return object from redis (cast templete T then deserialize)</summary>
        public T GetHashFieldObj<T>(string key, string field, CALLBACK<T> cbfunc = null) { return GetHashFieldObj<T>(string.Empty, key, field, cbfunc); }
        ///<param name="serverKey">alias key in redis connection</param>
        public T GetHashFieldObj<T>(string serverKey, string key, string field, CALLBACK<T> cbfunc = null)
        {
            string json_text = GetEntryField(serverKey, key, field);
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
                    ErrorLog("fail to set Hashs object exception msg = " + e.Message);
                }
            }
            else if (cbfunc != null)
            {
                try
                {
                    return Retry.RetryMethod(() =>
                    {
                        retObj = (T)cbfunc();
                        SetHashField(serverKey, key, field, retObj);
                        return retObj;
                    }, DefaultRetryCount, DefaultRetryTime);
                }
                catch (Exception e)
                {
                    ErrorLog("callback function fail exception msg = " + e.Message);
                }

            }

            return retObj;
        }

        ///<summary>return object from redis (cast templete T then deserialize)</summary>
        public T GetHashFieldObj<T, ArgType1>(string key, string field, CALLBACK<T, ArgType1> cbfunc = null, ArgType1 v1 = default(ArgType1)) { return GetHashFieldObj<T, ArgType1>(string.Empty, key, field, cbfunc, v1); }
        ///<param name="serverKey">alias key in redis connection</param>
        public T GetHashFieldObj<T, ArgType1>(string serverKey, string key, string field, CALLBACK<T, ArgType1> cbfunc = null, ArgType1 v1 = default(ArgType1))
        {
            string json_text = GetEntryField(serverKey, key, field);
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
                    ErrorLog("fail to set Hashs object exception msg = " + e.Message);
                }
            }
            else if(cbfunc != null)
            {
                try
                {
                    return Retry.RetryMethod(() =>
                    {
                        retObj = (T)cbfunc(v1);
                        SetHashField(serverKey, key, field, retObj);
                        return retObj;
                    }, DefaultRetryCount, DefaultRetryTime);
                }
                catch (Exception e)
                {
                    ErrorLog("callback function fail exception msg = " + e.Message);
                }

            }

            return retObj;
        }

        ///<summary>return object from redis (cast templete T then deserialize)</summary>
        public T GetHashFieldObj<T, ArgType1, ArgType2>(string key, string field, CALLBACK<T, ArgType1, ArgType2> cbfunc = null, ArgType1 v1 = default(ArgType1), ArgType2 v2 = default(ArgType2)) { return GetHashFieldObj<T, ArgType1, ArgType2>(string.Empty, key, field, cbfunc, v1, v2); }
        ///<param name="serverKey">alias key in redis connection</param>
        public T GetHashFieldObj<T, ArgType1, ArgType2>(string serverKey, string key, string field, CALLBACK<T, ArgType1, ArgType2> cbfunc = null, ArgType1 v1 = default(ArgType1), ArgType2 v2 = default(ArgType2))
        {
            string json_text = GetEntryField(serverKey, key, field);
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
                    ErrorLog("fail to set Hashs object exception msg = " + e.Message);
                }
            }
            else if (cbfunc != null)
            {
                try
                {
                    return Retry.RetryMethod(() =>
                    {
                        retObj = (T)cbfunc(v1, v2);
                        SetHashField(serverKey, key, field, retObj);
                        return retObj;
                    }, DefaultRetryCount, DefaultRetryTime);
                }
                catch (Exception e)
                {
                    ErrorLog("callback function fail exception msg = " + e.Message);
                }
            }

            return retObj;
        }

        ///<summary>return object from redis (cast templete T then deserialize)</summary>
        public T GetHashFieldObj<T, ArgType1, ArgType2, ArgType3>(string key, string field, CALLBACK<T, ArgType1, ArgType2, ArgType3> cbfunc = null, ArgType1 v1 = default(ArgType1), ArgType2 v2 = default(ArgType2), ArgType3 v3 = default(ArgType3)) { return GetHashFieldObj<T, ArgType1, ArgType2, ArgType3>(string.Empty, key, field, cbfunc, v1, v2, v3); }
        ///<param name="serverKey">alias key in redis connection</param>
        public T GetHashFieldObj<T, ArgType1, ArgType2, ArgType3>(string serverKey, string key, string field, CALLBACK<T, ArgType1, ArgType2, ArgType3> cbfunc = null, ArgType1 v1 = default(ArgType1), ArgType2 v2 = default(ArgType2), ArgType3 v3 = default(ArgType3))
        {
            string json_text = GetEntryField(serverKey, key, field);
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
                    ErrorLog("fail to set Hashs object exception msg = " + e.Message);
                }
            }
            else if (cbfunc != null)
            {
                try
                {
                    return Retry.RetryMethod(() =>
                    {
                        retObj = (T)cbfunc(v1, v2, v3);
                        SetHashField(serverKey, key, field, retObj);
                        return retObj;
                    }, DefaultRetryCount, DefaultRetryTime);
                }
                catch (Exception e)
                {
                    ErrorLog("callback function fail exception msg = " + e.Message);
                }
            }

            return retObj;
        }
        /// <summary>check contains Field in set (with key, Field name) </summary>
        /// <param name="key">stored redis key</param>
        /// <param name="Field">Field key in set</param>
        public bool HashContainsField(string key, string field) { return HashContainsField(string.Empty, key, field); }
        /// <param name="serverKey">alias key in redis connection</param>
        public bool HashContainsField(string serverKey, string key, string field)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        return redis.HashContainsEntry(GetPrefixHash(key), field);
                    }
                    //else
                    //    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to set Hashs object exception msg = " + e.Message);
                return false;
            }
        }

        /// <summary>remove Field in Hashs</summary>
        ///<param name="key">stored redis key</param>
        ///<param name="Field">Field key in set</param>
        public bool RemoveHashItem(string key, string field) { return RemoveHashItem(string.Empty, key, field); }
        ///<param name="serverKey">alias key in redis connection</param>
        public bool RemoveHashItem(string serverKey, string key, string field)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        redis.RemoveEntryFromHash(GetPrefixHash(key), field);
                        return true;
                    }
                    //else
                    //    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to set Hashs object exception msg = " + e.Message);
                return false;
            }
        }

        ///<summary> get all Field & value in Hashs </summary>
        ///<param name="key">stored redis key</param>
        public Dictionary<string, string> GetHashsAll_Item(string key) { return GetHashsAll_Item(string.Empty, key); }
        ///<param name="serverKey">alias key in redis connection</param>
        public Dictionary<string, string> GetHashsAll_Item(string serverKey, string key)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        return redis.GetAllEntriesFromHash(GetPrefixHash(key));
                    }
                    //else
                    //    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to get Hashs object  object exception msg = " + e.Message);
                return new Dictionary<string, string>();
            }
        }

        ///<summary> get all Field key in Hashs </summary>
        ///<param name="key">stored redis key</param>
        public List<string> GetHashsAll_Key(string key) { return GetHashsAll_Key(string.Empty, key); }
        ///<param name="serverKey">alias key in redis connection</param>
        public List<string> GetHashsAll_Key(string serverKey, string key)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        return redis.GetHashKeys(GetPrefixHash(key));
                    }
                    //else
                    //    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to get Hashs object  object exception msg = " + e.Message);
                return new List<string>();
            }
        }

        ///<summary> get all Field value in Hashs </summary>
        ///<param name="key">stored redis key</param>
        public List<string> GetHashsAll_Value(string key) { return GetHashsAll_Value(string.Empty, key); }
        ///<param name="serverKey">alias key in redis connection</param>
        public List<string> GetHashsAll_Value(string serverKey, string key)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        return redis.GetHashValues(GetPrefixHash(key));
                    }
                    //else
                    //    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to get Hashs object  object exception msg = " + e.Message);
                return new List<string>();
            }
        }

        ///<summary> Increments the number stored at field in the hash stored at key by increment.  </summary>
        ///<param name="key">stored redis key</param>
        public long HashIncrementField(string key, string field, int value) { return HashIncrementField(string.Empty, key, field, value); }
        ///<param name="serverKey">alias key in redis connection</param>
        public long HashIncrementField(string serverKey, string key, string field, int value)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        return redis.IncrementValueInHash(GetPrefixHash(key), field, value);
                    }
                    //else
                    //    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to get Hashs object  object exception msg = " + e.Message);
                return 0;
            }
        }
    }
}
