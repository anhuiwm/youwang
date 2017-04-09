using System;
using System.Text;
using System.Collections.Generic;
using ServiceStack.Text;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System.Diagnostics;
using System.Linq;

namespace mSeed.RedisManager
{
    // callback for list
    public delegate T[] LISTCALLBACK<T>();

    public partial class mRedis : IDisposable
    {
        private string PrefixList = "List";

        /// <summary>
        /// set prefix tag for redis lists 
        /// </summary>
        public void SetPrefixList(string prefix)
        {
            PrefixList = prefix;
        }

        /// <summary>
        /// get prefix tag for redis lists 
        /// </summary>
        private string GetPrefixList(string key)
        {
            return string.Format("{0}:{1}_{2}", PrefixTag, PrefixList, key);
        }

        ///<summary> add object to redis list at redis connection (object serialize) </summary>
        ///<param name="key">stored redis key</param>
        ///<param name="value">stored redis object array</param>
        public bool ListAdd(string key, object value) { object[] setArray = { value }; return ListAdds<object>(string.Empty, key, setArray); }
        ///<param name="serverKey">alias key in redis connection</param>
        public bool ListAdd(string serverKey, string key, object value) { object[] setArray = { value }; return ListAdds<object>(serverKey, key, setArray); }

        ///<summary> add objects range to redis list at redis connection (object serialize) </summary>
        ///<param name="key">stored redis key</param>
        ///<param name="value">stored redis object array</param>
        public bool ListAdds<T>(string key, T[] values) { return ListAdds(string.Empty, key, values); }
        ///<param name="serverKey">alias key in redis connection</param>
        public bool ListAdds<T>(string serverKey, string key, T[] values)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {                        
                        List<string> setValues = new List<string>();
                        foreach (T item in values)
                        {
                            var serialized = JsonSerializer.SerializeToString(item);
                            string json_text = serialized.ToString();

                            byte[] bytes = Encoding.UTF8.GetBytes(json_text);
                            json_text = Convert.ToBase64String(bytes);

                            setValues.Add(json_text);
                        }

                        redis.AddRangeToList(GetPrefixList(key), setValues);
                        //redis.ExpireEntryIn(GetPrefixList(key), new TimeSpan(365, 24, 60, 60));
                        return true;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime); 
            }
            catch (Exception e)
            {
                ErrorLog(key + " set fail to ListAdds object exception msg = " + e.Message);
                return false;
            }
        }

        ///<summary> add objects range to redis list at redis connection (object serialize) </summary>
        ///<param name="key">stored redis key</param>
        ///<param name="value">stored redis object array</param>
        public bool ListAdds(string key, List<string> values) { return ListAdds(string.Empty, key, values); }
        ///<param name="serverKey">alias key in redis connection</param>
        public bool ListAdds(string serverKey, string key, List<string> values)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        redis.AddRangeToList(GetPrefixList(key), values);
                        //redis.ExpireEntryIn(GetPrefixList(key), new TimeSpan(365, 24, 60, 60));
                        return true;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog(key + " set fail to ListAdds object exception msg = " + e.Message);
                return false;
            }
        }

        /// <summary>Lists expire timer set </summary>
        /// <param name="key">stored redis key</param>
        public bool ListExpireTimeSet(string key) { return ListExpireTimeSet(string.Empty, key, DefaultExpireTime); }
        ///<param name="second">set expire time (second)</param>
        public bool ListExpireTimeSet(string key, int second) { return ListExpireTimeSet(string.Empty, key, new TimeSpan(0, 0, second)); }
        ///<param name="exprie">set expire time </param>
        public bool ListExpireTimeSet(string key, TimeSpan expire) { return ListExpireTimeSet(string.Empty, key, expire); }
        /// <param name="serverKey">alias key in redis connection</param>
        public bool ListExpireTimeSet(string serverKey, string key) { return ListExpireTimeSet(serverKey, key, DefaultExpireTime); }
        /// <param name="serverKey">alias key in redis connection</param>
        public bool ListExpireTimeSet(string serverKey, string key, TimeSpan expire)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        expire = expire.TotalSeconds <= 0 ? new TimeSpan(365, 24, 60, 60) : expire;
                        redis.ExpireEntryIn(GetPrefixList(key), expire);
                        return true;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog(key + " set fail to ListExpireTimeSet object exception msg = " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// remove item in list by target index value. (default redis connection);
        /// Note :  All Item is the same value will be deleted at the same time. Check that the value of such Item is in List.
        /// </summary>
        ///<param name="key">stored redis key</param>
        ///<param name="index">value index of list</param>
        public bool RemoveListItemByIndex(string key, int index) { return RemoveListItemByIndex(string.Empty, key, index); }
        ///<param name="serverKey">alias key in redis connection</param>
        public bool RemoveListItemByIndex(string serverKey, string key, int index)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        string value = redis.GetItemFromList(GetPrefixList(key), index);
                        redis.RemoveItemFromList(GetPrefixList(key), value);
                        return true;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog(key + " set fail to RemoveListItemByIndex object exception msg = " + e.Message);
                return false;
            }
        }

        ///<summary> remove list in redis (default redis connection)</summary>
        ///<param name="key">stored redis key</param>
        public bool RemoveList(string key) { return RemoveList(string.Empty, key); }
        ///<param name="serverKey">alias key in redis connection</param>
        public bool RemoveList(string serverKey, string key)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        redis.Remove(GetPrefixList(key));
                        return true;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog(key + " set fail to RemoveList object exception msg = " + e.Message);
                return false;
            }
        }

        ///<summary> get object string format in list from index</summary>
        ///<param name="serverKey">alias key in redis connection</param>
        ///<param name="key">stored redis key</param>
        ///<param name="index">index of list</param>
        private string GetListIndex(string serverKey, string key, int index)
        {            
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        string json_text = string.Empty;
                        json_text = redis.GetItemFromList(GetPrefixList(key), index);

                        if (string.IsNullOrEmpty(json_text))
                            json_text = string.Empty;

                        byte[] bytes = Convert.FromBase64String(json_text);
                        json_text = Encoding.UTF8.GetString(bytes);

                        return json_text;
                    }
                }, DefaultRetryCount, DefaultRetryTime);
                //else
                //    throw new Exception("redis client not connected");
            }
            catch (Exception e)
            {
                ErrorLog(key + " set fail to GetListIndex object exception msg = " + e.Message);
                return string.Empty;
            }
        }

        ///<summary> get all object string format in list from index</summary>
        ///<param name="serverKey">alias key in redis connection</param>
        ///<param name="key">stored redis key</param>
        private string[] GetListAll_Item(string serverKey, string key)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        List<string> json_texts = new List<string>();
                        List<string> json_texts_utf8 = new List<string>();

                        json_texts_utf8 = redis.GetAllItemsFromList(GetPrefixList(key));

                        foreach (string json_text in json_texts_utf8)
                        {
                            byte[] bytes = Convert.FromBase64String(json_text);
                            json_texts.Add(Encoding.UTF8.GetString(bytes));
                        }

                        if (json_texts == null || json_texts.Count == 0)
                            return new string[] { };

                        return json_texts.ToArray();
                    }
                }, DefaultRetryCount, DefaultRetryTime);
                //else
                //    throw new Exception("redis client not connected");
            }
            catch (Exception e)
            {
                ErrorLog(key + " set fail to GetListAll_Item object exception msg = " + e.Message);
                return new string[] { };
            }
        }

        ///<summary>return object from index of list (cast templete T then deserialize)</summary>
        ///<param name="key">stored redis key</param>
        ///<param name="index">index of list</param>
        public T GetListObj<T>(string key, int index) { return GetListObj<T>(string.Empty, key, index); }
        ///<param name="serverKey">alias key in redis connection</param>
        public T GetListObj<T>(string serverKey, string key, int index)
        {
            string json_text = GetListIndex(serverKey, key, index);
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
                    ErrorLog(key + " set fail to GetListObj object exception msg = " + e.Message);
                }
            }

            return retObj;
        }

        ///<summary>return json string from index of list (cast templete T then deserialize)</summary>
        ///<param name="key">stored redis key</param>
        ///<param name="index">index of list</param>
        public string GetListString(string key, int index) { return GetListString(string.Empty, key, index); }
        ///<param name="serverKey">alias key in redis connection</param>
        public string GetListString(string serverKey, string key, int index)
        {
            string retObj = GetListObj<string>(serverKey, key, index);
            if (string.IsNullOrEmpty(retObj))
                retObj = string.Empty;
            return retObj;
        }

        ///<summary>return object from index of list (cast templete T then deserialize)</summary>
        ///<param name="key">stored redis key</param>
        ///<param name="index">index of list</param>
        public List<T> GetAllItemFromList<T>(string key) { return GetAllItemFromList<T>(string.Empty, key); }
        ///<param name="serverKey">alias key in redis connection</param>
        public List<T> GetAllItemFromList<T>(string serverKey, string key)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    string[] json_texts = GetListAll_Item(serverKey, key);

                    List<T> retObjs = new List<T>();
                    foreach (string json_text in json_texts)
                    {
                        retObjs.Add(JsonSerializer.DeserializeFromString<T>(json_text));
                    }

                    return retObjs;
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog(key + " set fail GetAllItemFromList object exception msg = " + e.Message);
                return new List<T>();
            }
        }

        ///<summary>return object from index of list with callback method for set call (cast templete T then deserialize)</summary>
        ///<param name="key">stored redis key</param>
        ///<param name="cbfunc">call back for get fail</param>        
        public T[] GetAllItemFromList<T>(string key, LISTCALLBACK<T> cbfunc) { return GetAllItemFromList<T>(string.Empty, key, cbfunc); }
        ///<param name="serverKey">alias key in redis connection</param>
        public T[] GetAllItemFromList<T>(string serverKey, string key, LISTCALLBACK<T> cbfunc)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    string[] json_texts = GetListAll_Item(serverKey, key);

                    List<T> retObjs = new List<T>();
                    if (json_texts.Length > 0)
                    {
                        foreach (string json_text in json_texts)
                        {
                            retObjs.Add(JsonSerializer.DeserializeFromString<T>(json_text));
                        }
                    }
                    else
                    {
                        T[] retFuncObjs = cbfunc();
                        foreach (T setObj in retFuncObjs)
                        {
                            retObjs.Add(setObj);
                        }
                        ListAdd(key, retObjs.ToArray());
                    }

                    return retObjs.ToArray();
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog(key + " set fail GetAllItemFromList object exception msg = " + e.Message);
                return new T[] { };
            }
        }

        /// <summary>Get the number of members in list ( default redis connection )</summary>
        /// <param name="key"></param>
        public long GetListCount(string key) { return GetListCount(string.Empty, key); }
        /// <param name="serverKey"></param>
        public long GetListCount(string serverKey, string key)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        return redis.GetListCount(GetPrefixList(key));
                    }
                    //else
                    //    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);

            }
            catch (Exception e)
            {
                ErrorLog(key + " set fail to GetListCount object exception msg = " + e.Message);
                return 0;
            }
        }

        ///<summary> pop object string format in list from index (FILO)</summary>
        ///<param name="serverKey">alias key in redis connection</param>
        ///<param name="key">stored redis key</param>
        private string PopList(string serverKey, string key)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        string json_text = string.Empty;
                        json_text = redis.PopItemFromList(GetPrefixList(key));

                        byte[] bytes = Convert.FromBase64String(json_text);
                        json_text = Encoding.UTF8.GetString(bytes);
                        
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
                ErrorLog(key + " set fail to PopList object exception msg = " + e.Message);
                return string.Empty;
            }
        }

        ///<summary> pop object from list (cast templete T then deserialize) (FILO)</summary>
        ///<param name="key">stored redis key</param>
        public T PopListObj<T>(string key) { return PopListObj<T>(string.Empty, key); }
        ///<param name="serverKey">alias key in redis connection</param>
        public T PopListObj<T>(string serverKey, string key)
        {
            string json_text = PopList(serverKey, key);
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
                    ErrorLog(key + " set fail to PopListObj object exception msg = " + e.Message);
                }
            }

            return retObj;
        }

        ///<summary>return json string from index of list (FILO)</summary>
        ///<param name="key">stored redis key</param>
        public string PopListString(string key) { return PopListString(string.Empty, key); }
        ///<param name="serverKey">alias key in redis connection</param>
        public string PopListString(string serverKey, string key)
        {
            string retObj = PopListObj<string>(serverKey, key);
            if (string.IsNullOrEmpty(retObj))
                retObj = string.Empty;
            return retObj;
        }


        ///<summary> push object to redis list at redis connection (object serialize) </summary>
        ///<param name="key">stored redis key</param>
        ///<param name="value">stored redis object array</param>
        public bool PushList(string key, object value) { object[] setArray = { value }; return PushLists<object>(string.Empty, key, setArray); }
        ///<param name="serverKey">alias key in redis connection</param>
        public bool PushList(string serverKey, string key, object value) { object[] setArray = { value }; return PushLists<object>(serverKey, key, setArray); }

        ///<summary> push objects range to redis list at redis connection (object serialize) </summary>
        ///<param name="key">stored redis key</param>
        ///<param name="value">stored redis object array</param>
        public bool PushLists<T>(string key, T[] values) { return PushLists(string.Empty, key, values); }
        ///<param name="serverKey">alias key in redis connection</param>
        public bool PushLists<T>(string serverKey, string key, T[] values)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        List<string> setValues = new List<string>();
                        foreach (T item in values)
                        {
                            var serialized = JsonSerializer.SerializeToString(item);
                            string json_text = serialized.ToString();

                            byte[] bytes = Encoding.UTF8.GetBytes(json_text);
                            json_text = Convert.ToBase64String(bytes);

                            setValues.Add(json_text);
                        }

                        redis.AddRangeToList(GetPrefixList(key), setValues);
                        //setValues.ForEach(setValue => { redis.PushItemToList(GetPrefixList(key), setValue); });
                        return true;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog(key + " set fail to PushLists object exception msg = " + e.Message);
                return false;
            }
        }

        ///<summary> enqueue object to redis list at redis connection (object serialize) </summary>
        ///<param name="key">stored redis key</param>
        ///<param name="value">stored redis object array</param>
        public bool EnqueueList(string key, object value) { object[] setArray = { value }; return EnqueueLists<object>(string.Empty, key, setArray); }
        ///<param name="serverKey">alias key in redis connection</param>
        public bool EnqueueList(string serverKey, string key, object value) { object[] setArray = { value }; return EnqueueLists<object>(serverKey, key, setArray); }

        ///<summary> enqueue objects range to redis list at redis connection (object serialize) </summary>
        ///<param name="key">stored redis key</param>
        ///<param name="value">stored redis object array</param>
        public bool EnqueueLists<T>(string key, T[] values) { return EnqueueLists(string.Empty, key, values); }
        ///<param name="serverKey">alias key in redis connection</param>
        public bool EnqueueLists<T>(string serverKey, string key, T[] values)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        List<string> setValues = new List<string>();
                        foreach (T item in values)
                        {
                            var serialized = JsonSerializer.SerializeToString(item);
                            string json_text = serialized.ToString();

                            byte[] bytes = Encoding.UTF8.GetBytes(json_text);
                            json_text = Convert.ToBase64String(bytes);
                            
                            setValues.Add(json_text);
                        }

                        setValues.ForEach(setValue => { redis.EnqueueItemOnList(GetPrefixList(key), setValue); });
                        return true;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog(key + " set fail to EnqueueLists object exception msg = " + e.Message);
                return false;
            }
        }

        ///<summary> Dequeue object string format in list from index (FIFO)</summary>
        ///<param name="serverKey">alias key in redis connection</param>
        ///<param name="key">stored redis key</param>
        private string DequeueList(string serverKey, string key)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        string json_text = string.Empty;
                        json_text = redis.DequeueItemFromList(GetPrefixList(key));

                        byte[] bytes = Convert.FromBase64String(json_text);
                        json_text = Encoding.UTF8.GetString(bytes);                        

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
                ErrorLog(key + " set fail to DequeueList object exception msg = " + e.Message);
                return string.Empty;
            }
        }

        ///<summary>return Dequeue object from list (cast templete T then deserialize) (FIFO)</summary>
        ///<param name="key">stored redis key</param>
        public T DequeueListObj<T>(string key) { return DequeueListObj<T>(string.Empty, key); }
        ///<param name="serverKey">alias key in redis connection</param>
        public T DequeueListObj<T>(string serverKey, string key)
        {
            string json_text = DequeueList(serverKey, key);
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
                    ErrorLog(key + " set fail to DequeueListObj object exception msg = " + e.Message);
                }
            }

            return retObj;
        }

        ///<summary>return json string from index of list (FIFO)</summary>
        ///<param name="key">stored redis key</param>
        public string DequeueListString(string key) { return DequeueListString(string.Empty, key); }
        ///<param name="serverKey">alias key in redis connection</param>
        public string DequeueListString(string serverKey, string key)
        {
            string retObj = DequeueListObj<string>(serverKey, key);
            if (string.IsNullOrEmpty(retObj))
                retObj = string.Empty;
            return retObj;
        }

        /// <summary>Get the number of random members in list ( default redis connection )</summary>
        /// <param name="key"></param>
        public List<T> GetRandomList<T>(string key, int getcount = 1) { return GetRandomList<T>(string.Empty, key, getcount); }
        /// <param name="serverKey"></param>
        public List<T> GetRandomList<T>(string serverKey, string key, int getcount = 1)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        int[] getIndexPos = new int[getcount];
                        Random rand = new Random((int)DateTime.Now.Ticks);
                        int Max = (int)redis.GetListCount(GetPrefixList(key));
                        int[] list = Enumerable.Range(0, Max).ToArray();
                        int idx, old;
                        for (int i = 0; i < Max; i++)
                        {
                            idx = rand.Next(Max);
                            old = list[i];
                            list[i] = list[idx];
                            list[idx] = old;
                        }

                        if (list.Count() > getcount)
                            Array.Copy(list, 0, getIndexPos, 0, getcount);
                        else
                            getIndexPos = list;

                        List<T> retObj = new List<T>();
                        foreach (int setIndex in getIndexPos)
                        {
                            string json_text = GetListIndex(serverKey, key, setIndex);
                            if(!string.IsNullOrEmpty(json_text))
                                retObj.Add(JsonSerializer.DeserializeFromString<T>(json_text));
                        }

                        return retObj;
                    }
                    //else
                    //    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);

            }
            catch (Exception e)
            {
                ErrorLog(key + " set fail to GetRandomList object exception msg = " + e.Message);
                return new List<T>();
            }
        }
    }
}