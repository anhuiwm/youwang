using System;
using System.Collections.Generic;
using ServiceStack.Text;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System.Diagnostics;

namespace mSeed.RedisManager
{
    public partial class mRedis : IDisposable
    {
        private string PrefixSets = "Sets";

        /// <summary>
        /// set prefix tag for redis Sets 
        /// </summary>
        public void SetPrefixSet(string prefix)
        {
            PrefixSets = prefix;
        }
        /// <summary>
        /// get prefix tag for redis Sets 
        /// </summary>
        private string GetPrefixSet(string key)
        {
            return string.Format("{0}:{1}_{2}", PrefixTag, PrefixSets, key);
        }

        /// <summary>set member to set (with key, member name) in redis connection</summary>
        /// <param name="key">stored redis key</param>
        /// <param name="member">member key in set</param>
        public bool SetAdd(string key, string member) { return SetAdd(string.Empty, key, member); }
        /// <param name="serverKey">alias key in redis connection</param>
        public bool SetAdd(string serverKey, string key, string member)
        {
            string[] setArray = { member };
            return SetAdds(serverKey, key, setArray);
        }

        ///<summary> add objects range to redis Set at redis connection (object serialize) </summary>
        ///<param name="key">stored redis key</param>
        ///<param name="value">member string array</param>
        public bool SetAdds(string key, string[] values) { return SetAdds(string.Empty, key, values); }
        ///<param name="serverKey">alias key in redis connection</param>
        public bool SetAdds(string serverKey, string key, string[] values)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        List<string> setValues = new List<string>(values);
                        redis.AddRangeToSet(GetPrefixSet(key), setValues);
                        return true;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to set Sets object exception msg = " + e.Message);
                return false;
            }
        }


        /// <summary>Set expire timer set </summary>
        /// <param name="key">stored redis key</param>
        public bool SetExpireTimeSet(string key) { return SetExpireTimeSet(string.Empty, key, DefaultExpireTime); }
        ///<param name="second">set expire time (second)</param>
        public bool SetExpireTimeSet(string key, int second) { return SetExpireTimeSet(string.Empty, key, new TimeSpan(0, 0, second)); }
        ///<param name="exprie">set expire time </param>
        public bool SetExpireTimeSet(string key, TimeSpan expire) { return SetExpireTimeSet(string.Empty, key, expire); }
        /// <param name="serverKey">alias key in redis connection</param>
        public bool SetExpireTimeSet(string serverKey, string key, TimeSpan expire)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        expire = expire.TotalSeconds <= 0 ? new TimeSpan(365, 24, 60, 60) : expire;

                        redis.ExpireEntryIn(GetPrefixSet(key), expire);
                        return true;
                    }
                }, DefaultRetryCount, DefaultRetryTime);
                throw new Exception("redis client not connected");
            }
            catch (Exception e)
            {
                ErrorLog("fail to set Sets object exception msg = " + e.Message);
                return false;
            }
        }

        /// <summary>Get the number of members in a set ( default redis connection )</summary>
        /// <param name="key">stored redis key</param>
        /// <returns></returns>
        public long GetSetCount(string key) { return GetSetCount(string.Empty, key); }
        /// <param name="serverKey">alias key in redis connection</param>
        public long GetSetCount(string serverKey, string key)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        return redis.GetSetCount(GetPrefixSet(key));
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to get Sets object  object exception msg = " + e.Message);
                return 0;
            }
        }

        /// <summary>check contains member in set (with key, member name) </summary>
        /// <param name="key">stored redis key</param>
        /// <param name="member">member key in set</param>
        public bool SetContainsItem(string key, string member) { return SetContainsItem(string.Empty, key, member); }
        /// <param name="serverKey">alias key in redis connection</param>
        public bool SetContainsItem(string serverKey, string key, string member)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        return redis.SetContainsItem(GetPrefixSet(key), member);
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to set Sets object exception msg = " + e.Message);
                return false;
            }
        }

        /// <summary>remove member in sets</summary>
        ///<param name="key">stored redis key</param>
        ///<param name="member">member key in set</param>
        public bool RemoveSetItem(string key, string member) { return RemoveSetItem(string.Empty, key, member); }
        ///<param name="serverKey">alias key in redis connection</param>
        public bool RemoveSetItem(string serverKey, string key, string member)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        redis.RemoveItemFromSet(GetPrefixSet(key), member);
                        return true;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to set Sets object exception msg = " + e.Message);
                return false;
            }
        }

        ///<summary> remove Set in redis (default redis connection)</summary>
        ///<param name="key">stored redis key</param>
        public bool RemoveSet(string key) { return RemoveSet(string.Empty, key); }
        ///<param name="serverKey">alias key in redis connection</param>
        public bool RemoveSet(string serverKey, string key)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        redis.Remove(GetPrefixSet(key));
                        return true;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to set Sets object exception msg = " + e.Message);
                return false;
            }
        }

        ///<summary> get all member string in set </summary>
        ///<param name="key">stored redis key</param>
        public string[] GetSetsAll_Item(string key) { return GetSetsAll_Item(string.Empty, key); }
        ///<param name="serverKey">alias key in redis connection</param>
        public string[] GetSetsAll_Item(string serverKey, string key)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        HashSet<string> getItems = new HashSet<string>();
                        getItems = redis.GetAllItemsFromSet(GetPrefixSet(key));
                        if (getItems.Count == 0)
                            return new string[] { };

                        string[] json_texts = new string[getItems.Count];

                        getItems.CopyTo(json_texts);

                        return json_texts;
                    }
                    //else
                    //    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to get Sets object  object exception msg = " + e.Message);
                return new string[] { };
            }
        }

        ///<summary> Returns the members of the set resulting from the intersection of all the given sets </summary>
        ///<param name="setKeys">check set keys</param>
        public HashSet<string> SetsIntersaction(string[] setKeys) { return SetsIntersaction(string.Empty, setKeys); }
        ///<param name="serverKey">alias key in redis connection</param>
        public HashSet<string> SetsIntersaction(string serverKey, string[] setKeys)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        if (setKeys.Length == 0)
                            throw new Exception("setKeys Empty or Null");

                        string[] setArray = new string[setKeys.Length];

                        for (int i = 0; i < setKeys.Length; i++)
                        {
                            setArray[i] = GetPrefixSet(setKeys[i]);
                        }

                        return redis.GetIntersectFromSets(setArray);
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to set Sets object exception msg = " + e.Message);
                return new HashSet<string>();
            }
        }

        ///<summary> Returns the members of the set resulting from the union of all the given sets </summary>
        ///<param name="setKeys">check set keys</param>
        public HashSet<string> SetsUnion(string[] setKeys) { return SetsUnion(string.Empty, setKeys); }
        ///<param name="serverKey">alias key in redis connection</param>
        public HashSet<string> SetsUnion(string serverKey, string[] setKeys)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        if (setKeys.Length == 0)
                            throw new Exception("setKeys Empty or Null");

                        string[] setArray = new string[setKeys.Length];

                        for (int i = 0; i < setKeys.Length; i++)
                        {
                            setArray[i] = GetPrefixSet(setKeys[i]);
                        }

                        return redis.GetUnionFromSets(setArray);
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to set Sets object exception msg = " + e.Message);
                return new HashSet<string>();
            }
        }

        ///<summary> Returns the members of the set resulting from the difference between the first set and all the successive sets. </summary>
        ///<param name="baseSet">base set for check</param>
        ///<param name="setKeys">check set keys</param>
        public HashSet<string> SetsDifference(string baseSet, string[] setKeys) { return SetsDifference(string.Empty, baseSet, setKeys); }
        ///<param name="serverKey">alias key in redis connection</param>
        public HashSet<string> SetsDifference(string serverKey, string baseSet, string[] setKeys)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        if (setKeys.Length == 0)
                            throw new Exception("setKeys Empty or Null");

                        string[] setArray = new string[setKeys.Length];

                        for (int i = 0; i < setKeys.Length; i++)
                        {
                            setArray[i] = GetPrefixSet(setKeys[i]);
                        }

                        return redis.GetDifferencesFromSet(GetPrefixSet(baseSet), setArray);
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to set Sets object exception msg = " + e.Message);
                return new HashSet<string>();
            }
        }

        ///<summary> Returns the random member in sets. </summary>
        ///<param name="setKeys">check set keys</param>
        public string SetsGetRandomMember(string key) { return SetsGetRandomMember(string.Empty, key); }
        ///<param name="serverKey">alias key in redis connection</param>
        public string SetsGetRandomMember(string serverKey, string key)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        return redis.GetRandomItemFromSet(GetPrefixSet(key));
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to set Sets object exception msg = " + e.Message);
                return string.Empty;
            }
        }
    }
}
