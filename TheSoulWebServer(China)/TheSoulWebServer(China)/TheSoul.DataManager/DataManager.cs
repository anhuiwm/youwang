using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace TheSoul.DataManager
{
    // logging test function
    //public class SnailLogger
    //{
    //    public Dictionary<string, string> LogParams = new Dictionary<string, string>();
        
    //    public static string GetStackTrace()
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        StackTrace st = new StackTrace(true);
    //        StackFrame[] frames = st.GetFrames();

    //        foreach (StackFrame frame in frames)
    //        {
    //            MethodBase method = frame.GetMethod();

    //            sb.AppendFormat("{0} - {1}", method.DeclaringType, method.Name);
    //            ParameterInfo[] paramaters = method.GetParameters();
    //            foreach (ParameterInfo paramater in paramaters)
    //            {
    //                sb.AppendFormat(" {0}: {1}", paramater.Name, paramater.ToString());
    //            }
    //            sb.AppendLine();
    //        }
    //        return sb.ToString();
    //    }
    //}

    // The casts to object in the below code are an unfortunate necessity due to
    // C#'s restriction against a where T : Enum constraint. (There are ways around
    // this, but they're outside the scope of this simple illustration.)
    public static class FlagsHelper
    {
        public static bool IsSet<T>(T flags, T flag) where T : struct
        {
            int flagsValue = (int)(object)flags;
            int flagValue = (int)(object)flag;

            return (flagsValue & flagValue) != 0;
        }

        public static void Set<T>(ref T flags, T flag) where T : struct
        {
            int flagsValue = (int)(object)flags;
            int flagValue = (int)(object)flag;

            flags = (T)(object)(flagsValue | flagValue);
        }
         
        public static void Unset<T>(ref T flags, T flag) where T : struct
        {
            int flagsValue = (int)(object)flags;
            int flagValue = (int)(object)flag;

            flags = (T)(object)(flagsValue & (~flagValue));
        }
    }

    public static class Math
    {
        static Random baseRandom = new Random();
        public static double GetRandomDouble(Random random, double min, double max)
        {
            if (min == max)
                return max;
            if (min > max)
            {
                double temp = min;
                min = max;
                max = temp;
            }
            return min + (random.NextDouble() * (max - min));
        }

        public static double GetRandomDouble(double min, double max)
        {
            if (min == max)
                return max;
            if (min > max)
            {
                double temp = min;
                min = max;
                max = temp;
            }
            return min + (baseRandom.NextDouble() * (max - min));
        }

        public static int GetRandomInt(Random random, int min, int max)
        {
            if (min == max)
                return max;
            if (min > max)
            {
                int temp = min;
                min = max;
                max = temp;
            }
            return random.Next(min, max + 1);
        }

        public static int GetRandomInt(int min, int max)
        {
            if (min == max)
                return max;
            if (min > max)
            {
                int temp = min;
                min = max;
                max = temp;
            }
            return baseRandom.Next(min, max + 1);
        }
    }


    public static class SQLtoJson
    {
        public static IEnumerable<Dictionary<string, object>> Serialize(ref SqlDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));

            while (reader.Read())
                results.Add(SerializeRow(cols, ref reader));

            return results;
        }
        private static Dictionary<string, object> SerializeRow(IEnumerable<string> cols, ref SqlDataReader reader)
        {
            var result = new Dictionary<string, object>();
            int diff_count = 0;
            foreach (var col in cols)
            {
                var t = reader[col];
                if (t == DBNull.Value)
                    t = null;

                if (result.ContainsKey(col))
                {
                    result.Add(col + diff_count, t);
                    diff_count++;
                }
                else
                {
                    result.Add(col, t);
                }
            }
            return result;
        }
    }

    //public static class RedisLogManager
    //{
    //    public static void ErrorLog(string e)
    //    {
    //        RedisConst.RedisConstErrorLog(e);
    //    }
    //}
    internal static class Extensions
    {
        public static IList<T> CloneList<T>(this IList<T> list) where T : ICloneable
        {
            return list.Select(item => (T)item.Clone()).ToList();
        }
    }

    public static class GenericFetch
    {
        const string formatString = "yyyy-MM-dd HH:mm:ss";
        public static string GetServerTimeString()
        {
            return DateTime.Now.ToString(formatString);
        }

        public static DateTime GetTimeStringToDateTime(string timeset)
        {
            try
            {
                return DateTime.ParseExact(timeset, formatString, null);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        //                            ,  ,   
        const long baseMaxSecond = 999999999;
        public static double ConvertToMSeedTime()
        {
            DateTime datetime = DateTime.Now;
            DateTime sTime = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            double ret = (datetime - sTime).TotalSeconds;
            ret = baseMaxSecond - ret;
            return ret;
        }

        /// <summary>
        /// Convert a date time object to Unix time representation.
        /// </summary>
        /// <param name="datetime">The datetime object to convert to Unix time stamp.</param>
        /// <returns>Returns a numerical representation (Unix time) of the DateTime object.</returns>
        public static long ConvertToUnixTime(DateTime datetime)
        {
            DateTime sTime = new DateTime(1970, 1, 1,0,0,0,DateTimeKind.Utc);
 
            return (long)(datetime - sTime).TotalSeconds;
        }

        /// <summary>
        /// Convert Unix time value to a DateTime object.
        /// </summary>
        /// <param name="unixtime">The Unix time stamp you want to convert to DateTime.</param>
        /// <returns>Returns a DateTime object that represents value of the Unix time.</returns>
        public static DateTime UnixTimeToDateTime(long unixtime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return sTime.AddSeconds(unixtime);
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }

        public enum FetchFrom
        {
            REDIS = 0x1,
            MSSQL = 0x2,
        };

        public static FetchFrom fromWhere = FetchFrom.REDIS;
        
        //private static long time_bgn = 0;
        //public static void Time_bgn() { GenericFetch.time_bgn = System.DateTime.Now.Ticks; }

        //private static long time_end = 0;
        //public static void Time_end() { GenericFetch.time_end = System.DateTime.Now.Ticks; }

        private const double tick_to_ms = 10000.0f;
        private const double tick_to_sec = 10000000.0f;
        private const double tick_to_ns = 100.0f;

        public static double timeGap(long time_bgn, long time_end, double tick_to = tick_to_sec)
        {
            return (time_end - time_bgn) / tick_to;
        }

        public static T FetchFromDB<T>(ref TxnBlock TB, string setQuery, string dbKey = "")
        {
            GenericFetch.fromWhere = FetchFrom.MSSQL;

            SqlDataReader getDr = null;
            TB.ExcuteSqlCommand(dbKey, setQuery, ref getDr);

            if (getDr != null)
            {
                var r = SQLtoJson.Serialize(ref getDr);
                string json = mJsonSerializer.ToJsonString(r);
                getDr.Dispose(); getDr.Close();
                T[] retSet = mJsonSerializer.JsonToObject<T[]>(json);

                if (retSet.Length > 0)
                    return retSet[0];
                else
                    return default(T);
            }

            return default(T);
        }

        public static T FetchFromRedis<T>(ref TxnBlock TB, string redisServerKey, string Key, string setQuery, string dbKey = "", bool Flush = false, int ExprieTimeSec = 0)
        {
            GenericFetch.fromWhere = FetchFrom.REDIS;
            T getInfo = default(T);
            if (!Flush)
                getInfo = RedisConst.GetRedisInstance().GetObj<T>(redisServerKey, Key);

            if (!(getInfo != null && !Flush))
            {
                getInfo = FetchFromDB<T>(ref TB, setQuery, dbKey);
                if (getInfo != null)
                {
                    if (ExprieTimeSec > 0)
                        RedisConst.GetRedisInstance().SetObj(redisServerKey, Key, getInfo, new TimeSpan(0, 0, ExprieTimeSec));
                    else
                        RedisConst.GetRedisInstance().SetObj(redisServerKey, Key, getInfo);
                }
            }

            if (getInfo == null)
                return default(T);

            return getInfo;
        }

        public static T FetchFromOnly_Redis<T>(string redisServerKey, string Key)
        {
            GenericFetch.fromWhere = FetchFrom.REDIS;
            T getInfo = RedisConst.GetRedisInstance().GetObj<T>(redisServerKey, Key);

            if (getInfo == null)
                return default(T);

            return getInfo;
        }


        public static List<T> FetchFromOnly_Redis_MultipleRow<T>(string redisServerKey, string Key)
        {
            GenericFetch.fromWhere = FetchFrom.REDIS;
            List<T> getInfo = RedisConst.GetRedisInstance().GetObj<List<T>>(redisServerKey, Key);

            if (getInfo == null)
                return new List<T>();

            return getInfo;
        }

        public static bool SetToRedis(string redisServerKey, string Key, object setObj)
        {
            GenericFetch.fromWhere = FetchFrom.REDIS;
            if (setObj != null)
            {
                RedisConst.GetRedisInstance().SetObj(redisServerKey, Key, setObj);
                return true;
            }

            return false;
        }


        public static List<T> FetchFromDB_MultipleRow<T>(ref TxnBlock TB, string setQuery, string dbKey = "")
        {
            GenericFetch.fromWhere = FetchFrom.MSSQL;

            SqlDataReader getDr = null;
            TB.ExcuteSqlCommand(dbKey, setQuery, ref getDr);

            if (getDr != null)
            {
                var r = SQLtoJson.Serialize(ref getDr);
                string json = mJsonSerializer.ToJsonString(r);
                getDr.Dispose(); getDr.Close();
                List<T> retList = mJsonSerializer.JsonToObject<List<T>>(json);
                return retList;
            }

            return new List<T>();
        }

        public static List<T> FetchFromRedis_MultipleRow<T>(ref TxnBlock TB, string redisServerKey, string Key, string setQuery, string dbKey = "", bool Flush = false, int ExprieTimeSec = 0)
        {
            GenericFetch.fromWhere = FetchFrom.REDIS;
            List<T> retList = new List<T>();
            if (!Flush)
                retList = RedisConst.GetRedisInstance().GetObj<List<T>>(redisServerKey, Key);

            if (retList == null || Flush || retList.Count == 0)
            {
                retList = FetchFromDB_MultipleRow<T>(ref TB, setQuery, dbKey);
                if (retList.Count == 0)
                    return retList;

                if (ExprieTimeSec > 0)
                    RedisConst.GetRedisInstance().SetObj(redisServerKey, Key, retList, new TimeSpan(0, 0, ExprieTimeSec));
                else
                    RedisConst.GetRedisInstance().SetObj(redisServerKey, Key, retList);
            }

            return retList;
        }

        public static string FetchFromDB_JsonString(ref TxnBlock TB, string setQuery, string dbKey = "")
        {
            GenericFetch.fromWhere = FetchFrom.MSSQL;

            SqlDataReader getDr = null;
            TB.ExcuteSqlCommand(dbKey, setQuery, ref getDr);

            if (getDr != null)
            {
                var r = SQLtoJson.Serialize(ref getDr);
                string json = mJsonSerializer.ToJsonString(r);
                getDr.Dispose(); getDr.Close();
                string[] retSet = mJsonSerializer.JsonToObject<string[]>(json);

                if (retSet.Length > 0)
                    return retSet[0];
                else
                    return default(string);
            }

            return default(string);
        }

        public static string FetchFromRedis_JsonString(ref TxnBlock TB, string redisServerKey, string Key, string setQuery, string dbKey = "", bool Flush = false, int ExprieTimeSec = 0)
        {
            GenericFetch.fromWhere = FetchFrom.REDIS;
            string getInfo = RedisConst.GetRedisInstance().GetString(redisServerKey, Key);

            if (!(getInfo != null && !Flush))
            {
                getInfo = FetchFromDB_JsonString(ref TB, setQuery, dbKey);
                if (ExprieTimeSec > 0)
                    RedisConst.GetRedisInstance().SetObj(redisServerKey, Key, getInfo, new TimeSpan(0, 0, ExprieTimeSec));
                else
                    RedisConst.GetRedisInstance().SetObj(redisServerKey, Key, getInfo);
            }

            return getInfo;
        }


        // for hash field
        public static List<T> FetchFromOnly_Redis_Hash_All<T>(string redisServerKey, string Key)
        {
            GenericFetch.fromWhere = FetchFrom.REDIS;
            List<string> getInfo = RedisConst.GetRedisInstance().GetHashsAll_Value(redisServerKey, Key);
            List<T> retObj = new List<T>();
            foreach (string json in getInfo)
            {
                retObj.Add(mJsonSerializer.JsonToObject<T>(json));
            }
            return retObj;
        }


        // for hash field
        public static T FetchFromOnly_Redis_Hash_Field<T>(string redisServerKey, string Key, string Field)
        {
            GenericFetch.fromWhere = FetchFrom.REDIS;

            T getInfo = default(T);
            getInfo = RedisConst.GetRedisInstance().GetHashFieldObj<T>(redisServerKey, Key, Field);
            if (getInfo == null)
                getInfo = default(T);

            return getInfo;
        }

        public static T FetchFromRedis_Hash<T>(ref TxnBlock TB, string redisServerKey, string Key, string Field, string setQuery, string dbKey = "", bool Flush = false)
        {
            GenericFetch.fromWhere = FetchFrom.REDIS;

            T getInfo = default(T);
            if (!Flush)
                getInfo = RedisConst.GetRedisInstance().GetHashFieldObj<T>(redisServerKey, Key, Field);

            if (!(getInfo != null && !Flush))
            {
                getInfo = FetchFromDB<T>(ref TB, setQuery, dbKey);
                if (getInfo != null)
                {
                    RedisConst.GetRedisInstance().SetHashField(redisServerKey, Key, Field, getInfo);
                    RedisConst.GetRedisInstance().SetExpireTimeHash(redisServerKey, Key);
                }
            }

            if (getInfo == null)
                return default(T);

            return getInfo;
        }

        public static List<T> FetchFromRedis_MultipleRow_Hash<T>(ref TxnBlock TB, string redisServerKey, string Key, string Field, string setQuery, string dbKey = "", bool Flush = false)
        {
            GenericFetch.fromWhere = FetchFrom.REDIS;
            List<T> retList = new List<T>();
            if (!Flush)
                retList = RedisConst.GetRedisInstance().GetHashFieldObj<List<T>>(redisServerKey, Key, Field);

            if (retList == null || Flush || retList.Count == 0)
            {
                retList = FetchFromDB_MultipleRow<T>(ref TB, setQuery, dbKey);
                if (retList.Count == 0)
                    return retList;
                RedisConst.GetRedisInstance().SetHashField(redisServerKey, Key, Field, retList);
                RedisConst.GetRedisInstance().SetExpireTimeHash(redisServerKey, Key);
            }

            return retList;
        }

        public static string FetchFromRedis_JsonString_Hash(ref TxnBlock TB, string redisServerKey, string Key, string Field, string setQuery, string dbKey = "", bool Flush = false)
        {
            GenericFetch.fromWhere = FetchFrom.REDIS;
            string getInfo = RedisConst.GetRedisInstance().GetHashFieldString(redisServerKey, Key, Field);

            if (!(getInfo != null && !Flush))
            {
                getInfo = FetchFromDB_JsonString(ref TB, setQuery, dbKey);
                RedisConst.GetRedisInstance().SetHashField(redisServerKey, Key, Field, getInfo);
                RedisConst.GetRedisInstance().SetExpireTimeHash(redisServerKey, Key);
            }

            return getInfo;
        }
    }


    public static class RedisConst
    {
        public static void RedisConstErrorLog(string e)
        {

            return;

            //if (RedisController != null)
            //{
            //    if (RedisController.redisConnActive)
            //    {
            //        string logkey = string.Format("ErrorLog_{0}", DateTime.Now.ToString("yyyy-MM-dd"));
            //        e = string.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss.ffff"), e);
            //        RedisController.ListAdd(logkey, e);
            //    }
            //}
        }

        //TODO : redis multiple connection controller;
        private static mRedis RedisController = null;
        private static object syncLock = new object();
        private static int DefaultRetryCount = 3;
        private static int DefaultRetryTime = 1;
        private static bool forceInit = false;

        public static bool ForceInit
        {
            get { return RedisConst.forceInit; }
        }

        public static void SetRedisInstance()
        {
            mRedis RedisInstance = GetRedisInstance();
            RedisInstance.Dispose();
            RedisInstance.RedisClose();

            foreach (RedisEndpoint setServer in DataManager_Define.RedisServerList)
            {
                RedisInstance.RedisConn(setServer.Host, setServer.Port, setServer.Alias);
            }
            if (RedisInstance.redisConnActive)
            {
                InitFlag = true;
                forceInit = true;
            }
        }

        public static mRedis GetRedisInstance()
        {
            if (RedisController == null)
            {
                lock (syncLock)
                {
                    if (RedisController == null)
                    {
                        RedisController = new mRedis();
                        RedisController.Elog = RedisConstErrorLog;
                    }
                }
            }
            return RedisController;
        }

        private static bool InitFlag = false;

        public static void RedisInit()
        {
            InitFlag = false;
            if (!InitFlag)
            {
                Retry.RetryVoidMethod(() =>
                {
                    // redis init add
                    //List<RedisEndpoint> RedisServerList = new List<RedisEndpoint>();
                    //RedisServerList.Add(new RedisEndpoint("210.122.11.223"));
                    //RedisServerList.Add(new RedisEndpoint("192.168.11.204"));
                    //RedisServerList.Add(new RedisEndpoint("192.168.11.243", "6379", "main"));
                    //RedisServerList.Add(new RedisEndpoint("210.122.11.223", "6379", "sub"));

                    mRedis RedisInstance = GetRedisInstance();
                    RedisInstance.Dispose();
                    foreach (RedisEndpoint setServer in DataManager_Define.RedisServerList)
                    {
                        RedisInstance.RedisConn(setServer.Host, setServer.Port, setServer.Alias);
                    }
                    if (RedisInstance.redisConnActive)
                        InitFlag = true;
                }, DefaultRetryCount, DefaultRetryTime);
            }
        }
    }
}