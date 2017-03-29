using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;

namespace mSeed.Common
{
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

    public static class GenericFetch
    {
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
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

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


        private const double tick_to_ms = 10000.0f;
        private const double tick_to_sec = 10000000.0f;
        private const double tick_to_ns = 100.0f;

        public static double timeGap(long time_bgn, long time_end, double tick_to = tick_to_sec)
        {
            return (time_end - time_bgn) / tick_to;
        }

        public static long FetchFromSingleLong(ref TxnBlock TB, string setQuery, string dbKey = "")
        {
            GenericFetch.fromWhere = FetchFrom.MSSQL;

            SqlDataReader getDr = null;
            TB.ExcuteSqlCommand(dbKey, setQuery, ref getDr);
            long retvalue = 0;
            if (getDr != null)
            {
                getDr.Read();
                var r = getDr.GetValue(0);
                getDr.Dispose(); getDr.Close();
                try
                {
                    retvalue = System.Convert.ToInt64(r);
                }
                catch (Exception ex)
                {
                    TB.Elog(ex.Message);
                }
            }
            return retvalue;
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
        
        public static T FetchFromDB<T>(ref TxnBlock TB, SqlCommand setCommand, string dbKey = "")
        {
            GenericFetch.fromWhere = FetchFrom.MSSQL;

            SqlDataReader getDr = null;
            TB.ExcuteSqlCommand(dbKey, ref setCommand, ref getDr);

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


        public static List<T> FetchFromDB_MultipleRow<T>(ref TxnBlock TB, SqlCommand setCommand, string dbKey = "")
        {
            GenericFetch.fromWhere = FetchFrom.MSSQL;

            SqlDataReader getDr = null;
            TB.ExcuteSqlCommand(dbKey, ref setCommand, ref getDr);

            if (getDr != null)
            {
                var r = SQLtoJson.Serialize(ref getDr);
                string json = mJsonSerializer.ToJsonString(r);
                getDr.Dispose(); getDr.Close();
                List<T> retList = mJsonSerializer.JsonToObject<List<T>>(json);
                return retList;
            }else
                return new List<T>();
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
    }

}