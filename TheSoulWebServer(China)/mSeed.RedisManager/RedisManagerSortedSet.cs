using System;
using System.Collections.Generic;
using ServiceStack.Text;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System.Diagnostics;
using System.Linq;

namespace mSeed.RedisManager
{
    public partial class mRedis : IDisposable
    {
        [Flags]
        public enum SortedSet_Proc_Separator
        {
            None = Default,
            Highest_Rank = 0x1,
            Lowest_Rank = 0x2,
            //WithScore = 0x4,
            //No_WithScore = 0x8,
            //Max_Below = 0x4,
            //Max_MoreThan = 0x8,
            //Min_Below = 0x10,
            //Min_MoreThan = 0x20,
            //Default = Min_Below | Max_Below | Highest_Rank,
            Default = Highest_Rank,
        };

        private string PrefixSortedSet = "SortedSet";

        /// <summary>
        /// set prefix tag for redis lists 
        /// </summary>
        public void SetPrefixSortedSet(string prefix)
        {
            PrefixSortedSet = prefix;
        }

        /// <summary>
        /// get prefix tag for redis lists 
        /// </summary>
        private string GetPrefixSortedSet(string key)
        {
            return string.Format("{0}:{1}_{2}", PrefixTag, PrefixSortedSet, key);
        }

        /// <summary>set score value to sorted set (with key, member name) in redis connection</summary>
        /// <param name="key">stored redis key</param>
        /// <param name="member">stored member key in sorted set</param>
        /// <param name="scoreValue">set value (double) </param>
        public bool SortedSetAdd(string key, string member, double scoreValue) { return SortedSetAdd(string.Empty, key, member, scoreValue); }
        /// <param name="serverKey">alias key in redis connection</param>
        public bool SortedSetAdd(string serverKey, string key, string member, double scoreValue)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        redis.AddItemToSortedSet(GetPrefixSortedSet(key), member, scoreValue);
                        //redis.ExpireEntryIn(GetPrefixSortedSet(key), new TimeSpan(365, 24, 60, 60));
                        return true;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to set SortedSet object exception msg = " + e.Message);
                return false;
            }
        }

        /// <summary>SortedSet expire timer set </summary>
        /// <param name="key">stored redis key</param>
        public bool SortedSetExpireTimeSet(string key) { return SortedSetExpireTimeSet(string.Empty, key, DefaultExpireTime); }
        ///<param name="second">set expire time (second)</param>
        public bool SortedSetExpireTimeSet(string key, int second) { return SortedSetExpireTimeSet(string.Empty, key, new TimeSpan(0, 0, second)); }        
        ///<param name="exprie">set expire time </param>
        public bool SortedSetExpireTimeSet(string key, TimeSpan expire) { return SortedSetExpireTimeSet(string.Empty, key, expire); }
        /// <param name="serverKey">alias key in redis connection</param>
        public bool SortedSetExpireTimeSet(string serverKey, string key, TimeSpan expire)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        expire = expire.TotalSeconds <= 0 ? new TimeSpan(365, 24, 60, 60) : expire;

                        redis.ExpireEntryIn(GetPrefixSortedSet(key), expire);
                        return true;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to set SortedSet object exception msg = " + e.Message);
                return false;
            }
        }

        /// <summary>Get the number of members in a sorted set ( default redis connection )</summary>
        /// <param name="key">stored redis key</param>
        /// <returns></returns>
        public long GetSortedSetCount(string key) { return GetSortedSetCount(string.Empty, key); }
        /// <param name="serverKey">alias key in redis connection</param>
        public long GetSortedSetCount(string serverKey, string key)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        return redis.GetSortedSetCount(GetPrefixSortedSet(key));
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);                
            }
            catch (Exception e)
            {
                ErrorLog("fail to get SortedSet object  object exception msg = " + e.Message);
                return 0;
            }            
        }
        /// <summary>Returns the number of elements in the sorted set at key with a score between min and max.</summary>
        /// <param name="key">stored redis key</param>
        /// <param name="fromScore">min score</param>
        /// <param name="toScore">max score</param>
        public long GetSortedSetRangeCount(string key, long fromScore, long toScore) { return GetSortedSetRangeCount(string.Empty, key, fromScore, toScore); }
        /// <param name="serverKey">alias key in redis connection</param>
        public long GetSortedSetRangeCount(string serverKey, string key, long fromScore, long toScore)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        return redis.GetSortedSetCount(GetPrefixSortedSet(key), fromScore, toScore);
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to get SortedSet object  object exception msg = " + e.Message);
                return 0;
            }
        }
        /// <param name="serverKey">alias key in redis connection</param>
        public long GetSortedSetRangeCount(string key, double fromScore, double toScore) { return GetSortedSetRangeCount(string.Empty, key, fromScore, toScore); }
        public long GetSortedSetRangeCount(string serverKey, string key, double fromScore, double toScore)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        return redis.GetSortedSetCount(GetPrefixSortedSet(key), fromScore, toScore);
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to get SortedSet object  object exception msg = " + e.Message);
                return 0;
            }
        }

        /// <summary>
        /// Returns the specified range of elements in the sorted set stored at key. The elements are considered to be ordered from the lowest to the highest score. 
        /// </summary>
        public List<string> GetSortedSetRange(string key, double fromScore, double toScore) { return GetSortedSetRange(string.Empty, key, fromScore, toScore, null, null, SortedSet_Proc_Separator.Highest_Rank); }
        /// <param name="eProc">Highest_Rank or Lowest_Rank</param>
        public List<string> GetSortedSetRange(string key, double fromScore, double toScore, SortedSet_Proc_Separator eProc) { return GetSortedSetRange(string.Empty, key, fromScore, toScore, null, null, eProc); }
        /// <param name="serverKey">alias key in redis connection</param>
        public List<string> GetSortedSetRange(string serverKey, string key, double fromScore, double toScore) { return GetSortedSetRange(serverKey, key, fromScore, toScore, null, null, SortedSet_Proc_Separator.Highest_Rank); }
        public List<string> GetSortedSetRange(string serverKey, string key, double fromScore, double toScore, SortedSet_Proc_Separator eProc) { return GetSortedSetRange(serverKey, key, fromScore, toScore, null, null, eProc); }
        /// <param name="serverKey">alias key in redis connection</param>
        /// <param name="key">stored redis key</param>
        /// <param name="fromScore">min score</param>
        /// <param name="toScore">max score</param>
        /// <param name="skip">start position</param>
        /// <param name="take">stop position</param>
        public List<string> GetSortedSetRange(string serverKey, string key, double fromScore, double toScore, int? skip, int? take, SortedSet_Proc_Separator eProc)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        List<string> retObj;
                        if ((eProc & SortedSet_Proc_Separator.Highest_Rank) != 0)
                            retObj = redis.GetRangeFromSortedSetByLowestScore(GetPrefixSortedSet(key), fromScore, toScore, skip, take);
                        else if ((eProc & SortedSet_Proc_Separator.Lowest_Rank) != 0)
                            retObj = redis.GetRangeFromSortedSetByHighestScore(GetPrefixSortedSet(key), fromScore, toScore, skip, take);
                        else
                            throw new Exception("invalid SortedSet_Proc_Separator");

                        return retObj;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to get SortedSet object  object exception msg = " + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Returns the specified range of elements in the sorted set stored at key. The elements are considered to be ordered from the lowest to the highest score. 
        /// </summary>
        public List<string> GetSortedSetRange(string key, long fromScore, long toScore) { return GetSortedSetRange(string.Empty, key, fromScore, toScore, null, null, SortedSet_Proc_Separator.Highest_Rank); }
        /// <param name="eProc">Highest_Rank or Lowest_Rank</param>
        public List<string> GetSortedSetRange(string key, long fromScore, long toScore, SortedSet_Proc_Separator eProc) { return GetSortedSetRange(string.Empty, key, fromScore, toScore, null, null, eProc); }
        /// <param name="serverKey">alias key in redis connection</param>
        public List<string> GetSortedSetRange(string serverKey, string key, long fromScore, long toScore) { return GetSortedSetRange(serverKey, key, fromScore, toScore, null, null, SortedSet_Proc_Separator.Highest_Rank); }
        public List<string> GetSortedSetRange(string serverKey, string key, long fromScore, long toScore, SortedSet_Proc_Separator eProc) { return GetSortedSetRange(serverKey, key, fromScore, toScore, null, null, eProc); }
        /// <param name="serverKey">alias key in redis connection</param>
        /// <param name="key">stored redis key</param>
        /// <param name="fromScore">min score</param>
        /// <param name="toScore">max score</param>
        /// <param name="skip">start position</param>
        /// <param name="take">stop position</param>
        public List<string> GetSortedSetRange(string serverKey, string key, long fromScore, long toScore, int? skip, int? take, SortedSet_Proc_Separator eProc)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        List<string> retObj;
                        if ((eProc & SortedSet_Proc_Separator.Highest_Rank) != 0)
                            retObj = redis.GetRangeFromSortedSetByLowestScore(GetPrefixSortedSet(key), fromScore, toScore, skip, take);
                        else if ((eProc & SortedSet_Proc_Separator.Lowest_Rank) != 0)
                            retObj = redis.GetRangeFromSortedSetByHighestScore(GetPrefixSortedSet(key), fromScore, toScore, skip, take);
                        else
                            throw new Exception("invalid SortedSet_Proc_Separator");

                        return retObj;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to get SortedSet object  object exception msg = " + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Returns the specified range of elements with score (string, double pair) in the sorted set stored at key. The elements are considered to be ordered from the lowest to the highest score. 
        /// </summary>
        public Dictionary<string, double> GetSortedSetRangeWithScore(string key, double fromScore, double toScore) { return GetSortedSetRangeWithScore(string.Empty, key, fromScore, toScore, null, null, SortedSet_Proc_Separator.Highest_Rank); }
        /// <param name="eProc">Highest_Rank or Lowest_Rank</param>
        public Dictionary<string, double> GetSortedSetRangeWithScore(string key, double fromScore, double toScore, SortedSet_Proc_Separator eProc) { return GetSortedSetRangeWithScore(string.Empty, key, fromScore, toScore, null, null, eProc); }
        /// <param name="serverKey">alias key in redis connection</param>
        public Dictionary<string, double> GetSortedSetRangeWithScore(string serverKey, string key, double fromScore, double toScore) { return GetSortedSetRangeWithScore(serverKey, key, fromScore, toScore, null, null, SortedSet_Proc_Separator.Highest_Rank); }
        public Dictionary<string, double> GetSortedSetRangeWithScore(string serverKey, string key, double fromScore, double toScore, SortedSet_Proc_Separator eProc) { return GetSortedSetRangeWithScore(serverKey, key, fromScore, toScore, null, null, eProc); }
        /// <param name="serverKey">alias key in redis connection</param>
        /// <param name="key">stored redis key</param>
        /// <param name="fromScore">min score</param>
        /// <param name="toScore">max score</param>
        /// <param name="skip">start position</param>
        /// <param name="take">stop position</param>
        public Dictionary<string, double> GetSortedSetRangeWithScore(string serverKey, string key, double fromScore, double toScore, int? skip, int? take, SortedSet_Proc_Separator eProc)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        Dictionary<string, double> retObj;
                        if ((eProc & SortedSet_Proc_Separator.Highest_Rank) != 0)
                            retObj = redis.GetRangeWithScoresFromSortedSetByHighestScore(GetPrefixSortedSet(key), fromScore, toScore, skip, take).ToDictionary(k => k.Key, v => v.Value);
                        else if ((eProc & SortedSet_Proc_Separator.Lowest_Rank) != 0)
                            retObj = redis.GetRangeWithScoresFromSortedSetByLowestScore(GetPrefixSortedSet(key), fromScore, toScore, skip, take).ToDictionary(k => k.Key, v => v.Value);
                        else
                            throw new Exception("invalid SortedSet_Proc_Separator");

                        return retObj;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to get SortedSet object  object exception msg = " + e.Message);
                return null;
            }
        }

        public Dictionary<string, double> GetSortedSetRangeWithScore(string key, long fromScore, long toScore) { return GetSortedSetRangeWithScore(string.Empty, key, fromScore, toScore, null, null, SortedSet_Proc_Separator.Highest_Rank); }
        /// <param name="eProc">Highest_Rank or Lowest_Rank</param>
        public Dictionary<string, double> GetSortedSetRangeWithScore(string key, long fromScore, long toScore, SortedSet_Proc_Separator eProc) { return GetSortedSetRangeWithScore(string.Empty, key, fromScore, toScore, null, null, eProc); }
        /// <param name="serverKey">alias key in redis connection</param>
        public Dictionary<string, double> GetSortedSetRangeWithScore(string serverKey, string key, long fromScore, long toScore) { return GetSortedSetRangeWithScore(serverKey, key, fromScore, toScore, null, null, SortedSet_Proc_Separator.Highest_Rank); }
        public Dictionary<string, double> GetSortedSetRangeWithScore(string serverKey, string key, long fromScore, long toScore, SortedSet_Proc_Separator eProc) { return GetSortedSetRangeWithScore(serverKey, key, fromScore, toScore, null, null, eProc); }
        /// <param name="serverKey">alias key in redis connection</param>
        /// <param name="key">stored redis key</param>
        /// <param name="fromScore">min score</param>
        /// <param name="toScore">max score</param>
        /// <param name="skip">start position</param>
        /// <param name="take">stop position</param>
        public Dictionary<string, double> GetSortedSetRangeWithScore(string serverKey, string key, long fromScore, long toScore, int? skip, int? take, SortedSet_Proc_Separator eProc)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {                        
                        Dictionary<string, double> retObj;
                        if ((eProc & SortedSet_Proc_Separator.Highest_Rank) != 0)
                            retObj = redis.GetRangeWithScoresFromSortedSetByHighestScore(GetPrefixSortedSet(key), fromScore, toScore, skip, take).ToDictionary(k => k.Key, v => v.Value);
                        else if ((eProc & SortedSet_Proc_Separator.Lowest_Rank) != 0)
                            retObj = redis.GetRangeWithScoresFromSortedSetByLowestScore(GetPrefixSortedSet(key), fromScore, toScore, skip, take).ToDictionary(k => k.Key, v => v.Value);
                        else
                            throw new Exception("invalid SortedSet_Proc_Separator");

                        return retObj;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime); 
            }
            catch (Exception e)
            {
                ErrorLog("fail to get SortedSet object  object exception msg = " + e.Message);
                return null;
            }
        }


        public Dictionary<string, double> GetSortedSetRangeByPosWithScore(string key, int fromPos, int toPos) { return GetSortedSetRangeByPosWithScore(string.Empty, key, fromPos, toPos, null, null, SortedSet_Proc_Separator.Highest_Rank); }
        /// <param name="eProc">Highest_Rank or Lowest_Rank</param>
        public Dictionary<string, double> GetSortedSetRangeByPosWithScore(string key, int fromPos, int toPos, SortedSet_Proc_Separator eProc) { return GetSortedSetRangeByPosWithScore(string.Empty, key, fromPos, toPos, null, null, eProc); }
        /// <param name="serverKey">alias key in redis connection</param>
        public Dictionary<string, double> GetSortedSetRangeByPosWithScore(string serverKey, string key, int fromPos, int toPos) { return GetSortedSetRangeByPosWithScore(serverKey, key, fromPos, toPos, null, null, SortedSet_Proc_Separator.Highest_Rank); }
        public Dictionary<string, double> GetSortedSetRangeByPosWithScore(string serverKey, string key, int fromPos, int toPos, SortedSet_Proc_Separator eProc) { return GetSortedSetRangeByPosWithScore(serverKey, key, fromPos, toPos, null, null, eProc); }
        /// <param name="serverKey">alias key in redis connection</param>
        /// <param name="key">stored redis key</param>
        /// <param name="fromScore">min score</param>
        /// <param name="toScore">max score</param>
        /// <param name="skip">start position</param>
        /// <param name="take">stop position</param>
        public Dictionary<string, double> GetSortedSetRangeByPosWithScore(string serverKey, string key, int fromPos, int toPos, int? skip, int? take, SortedSet_Proc_Separator eProc)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        Dictionary<string, double> retObj;

                        if ((eProc & SortedSet_Proc_Separator.Highest_Rank) != 0)
                            retObj = redis.GetRangeWithScoresFromSortedSetDesc(GetPrefixSortedSet(key), fromPos, toPos).ToDictionary(k => k.Key, v => v.Value);
                        else if ((eProc & SortedSet_Proc_Separator.Lowest_Rank) != 0)
                            retObj = redis.GetRangeWithScoresFromSortedSet(GetPrefixSortedSet(key), fromPos, toPos).ToDictionary(k => k.Key, v => v.Value);
                        else
                            throw new Exception("invalid SortedSet_Proc_Separator");

                        return retObj;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to get SortedSet object  object exception msg = " + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Returns the score of member in the sorted set at key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        public double GetScore(string key, string member) { return GetScore(string.Empty, key, member); }
        /// <param name="serverKey"></param>
        public double GetScore(string serverKey, string key, string member)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        double member_score = redis.GetItemScoreInSortedSet(GetPrefixSortedSet(key), member);
                        if (double.IsNaN(member_score))
                            member_score = 0;

                        return member_score;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to get SortedSet object  object exception msg = " + e.Message);
                return 0;
            }
        }

        /// <summary>
        /// Returns the rank of member in the sorted set stored at key, with the scores ordered (default high to low) 
        /// </summary>
        /// <param name="key">stored redis key</param>
        /// <param name="member">stored member key in sorted set</param>
        public long GetRank(string key, string member) { return GetRank(string.Empty, key, member, SortedSet_Proc_Separator.Highest_Rank); }
        /// <param name="eProc">Highest_Rank or Lowest_Rank</param>
        public long GetRank(string key, string member, SortedSet_Proc_Separator eProc) { return GetRank(string.Empty, key, member, eProc); }
        /// <param name="serverKey">alias key in redis connection</param>
        public long GetRank(string serverKey, string key, string member) { return GetRank(serverKey, key, member, SortedSet_Proc_Separator.Highest_Rank); }
        public long GetRank(string serverKey, string key, string member, SortedSet_Proc_Separator eProc)
        {
            try
            {
                return Retry.RetryMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        long retRank = 0;
                        double member_score = redis.GetItemScoreInSortedSet(GetPrefixSortedSet(key), member);
                        if (double.IsNaN(member_score))
                            return retRank;

                        List<string> rankObj;
                        rankObj = redis.GetRangeFromSortedSetByHighestScore(GetPrefixSortedSet(key), member_score, member_score);

                        //if (rankObj.Count < 1)                        
                        //    throw new Exception(string.Format("SortedSet ZRANGEBYSCORE command Fail = {0}, {1}, {2}", key, member, member_score));

                        long total_ItemCount = redis.GetSortedSetCount(GetPrefixSortedSet(key));

                        if (rankObj.Count < 1)
                            return total_ItemCount + 1;

                        if ((eProc & SortedSet_Proc_Separator.Highest_Rank) != 0)
                        {
                            retRank = total_ItemCount - redis.GetItemIndexInSortedSet(GetPrefixSortedSet(key), rankObj.FirstOrDefault());
                        }
                        else if ((eProc & SortedSet_Proc_Separator.Lowest_Rank) != 0)
                        {
                            retRank = total_ItemCount - redis.GetItemIndexInSortedSet(GetPrefixSortedSet(key), rankObj.LastOrDefault());
                        }
                        else
                            throw new Exception("invalid SortedSet_Proc_Separator");

                        return retRank;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to get SortedSet object  object exception msg = " + e.Message);
                return 0;
            }
        }

        ///<summary> remove member in sorted set (default redis connection)</summary>
        ///<param name="key">stored redis key</param>
        /// <param name="member">stored member key in sorted set</param>
        public void RemoveMember(string key, string member) { RemoveMember(string.Empty, key, member); }
        /// <param name="serverKey">alias key in redis connection</param>
        public void RemoveMember(string serverKey, string key, string member)
        {
            try
            {
                Retry.RetryVoidMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        redis.RemoveItemFromSortedSet(GetPrefixSortedSet(key), member);
                        return;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to get SortedSet object  object exception msg = " + e.Message);
                return ;
            }
        }

        ///<summary> remove sorted set in redis (default redis connection)</summary>
        ///<param name="key">stored redis key</param>
        public void RemoveSortedSet(string key, string member) { RemoveSortedSet(string.Empty, key, member); }
        /// <param name="serverKey">alias key in redis connection</param>
        public void RemoveSortedSet(string serverKey, string key, string member)
        {
            try
            {
                Retry.RetryVoidMethod(() =>
                {
                    using (IRedisClient redis = GetRedisClient(serverKey))
                    {
                        redis.RemoveItemFromSortedSet(GetPrefixSortedSet(key), member);
                        return;
                    }
                    throw new Exception("redis client not connected");
                }, DefaultRetryCount, DefaultRetryTime);
            }
            catch (Exception e)
            {
                ErrorLog("fail to get SortedSet object  object exception msg = " + e.Message);
                return;
            }
        }
    }
}