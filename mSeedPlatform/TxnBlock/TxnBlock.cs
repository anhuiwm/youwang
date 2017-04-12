using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace mSeed.mDBTxnBlock
{
    public delegate void LOGFUNC(string e);

    public class TxnBlock :IDisposable
    {        
        //// singleton instance
        //private static TxnBlock _instance = new TxnBlock();;

        ///// <summary> get TxnBlock Instance </summary>
        //public static TxnBlock Instance
        //{
        //    get {
        //        return _instance;
        //    }
        //}
        
        //private TxnBlock() {}

        public Dictionary<string, string> SystemLogData = new Dictionary<string, string>();

        public void SetLogData(string key, object value = null)
        {
            if (!string.IsNullOrEmpty(key))
            {
                if (value == null)
                    SystemLogData[key] = "1";
                else
                    SystemLogData[key] = value.ToString();
            }
        }

        public string GetLogData(string key)
        {
            return SystemLogData.ContainsKey(key) ? SystemLogData[key] : string.Empty;
        }

        private IsolationLevel isoLevel = IsolationLevel.ReadUncommitted;

        public IsolationLevel IsoLevel
        {
            get { return isoLevel; }
            set
            {
                isoLevel = value;
                foreach (KeyValuePair<string, SqlDBClient> conn in DBClient)
                {
                    conn.Value.IsoLevel = value;
                }
            }
        }

        // SqlDBconnection list
        private Dictionary<string, SqlDBClient> DBClient = new Dictionary<string, SqlDBClient>();
        private string defaultkey = "mainDB";
        private LOGFUNC elog = null;

        public LOGFUNC Elog
        {
            get { return elog; }
            set {
                elog = value;
                foreach (KeyValuePair<string, SqlDBClient> conn in DBClient)
                {
                    conn.Value.Elog = value;
                }
            }
        }

        private void ErrorLog(string error)
        {
            if (elog != null)
                elog(error);
        }


        ~TxnBlock()
        {
            Dispose();
        }

        public string GetDBKey(ref string DBKey)
        {
            if (string.IsNullOrEmpty(DBKey))
            {
                DBKey = DBClient.Count > 0 ? DBClient.First().Key : defaultkey;
            }
            return DBKey;
        }

        /// <summary> create new SqlDB connection </summary>
        /// <paparam name="key">set alias connection name</paparam>
        /// <paparam name="connEndPoint">connection structure </paparam>
        public void DBConn(DBEndpoint connEndPoint, string DBKey = "", bool Debug = false)
        {
            try
            {
                if (DBClient.ContainsKey(GetDBKey(ref DBKey)))
                    return;

                DBClient[DBKey] = new SqlDBClient(connEndPoint, DBKey, Debug);
                if (Elog != null)
                    Elog = elog;
            }
            catch (Exception e)
            {
                ErrorLog("StackTrace" + e.StackTrace);
                ErrorLog(e.Message);                    
            }
        }

        /// <summary> create new SqlDB connection </summary>
        /// <paparam name="key">set alias connection name</paparam>
        /// <paparam name="host">connection string (SqlConnectionString) </paparam>
        public void DBConn(string connString, string DBKey = "", bool Debug = false)
        {
            try
            {
                if (DBClient.ContainsKey(GetDBKey(ref DBKey)))
                    return;

                DBClient[DBKey] = new SqlDBClient(connString, DBKey, Debug);
                DBClient[DBKey].IsoLevel = this.isoLevel;
                if (Elog != null)
                    Elog = elog;
            }
            catch (Exception e)
            {
                ErrorLog("StackTrace" + e.StackTrace);
                ErrorLog(e.Message);                    
            }
        }

        /// <summary> get first connection in SqlDB connection list </summary>
        public SqlDBClient GetClient()
        {
            return GetClient(string.Empty);
            //foreach (KeyValuePair<string, SqlDBClient> conn in DBClient)
            //{
            //    return conn.Value;
            //}
            //return null;
        }

        /// <summary> get connections such as Key in SqlDB connection list</summary>
        public SqlDBClient GetClient(string DBKey)
        {
            if (DBClient.ContainsKey(GetDBKey(ref DBKey)))
                return DBClient[DBKey];
            return null;
        }

        /// <summary> close all connection in SqlDB connection list</summary>
        public void Dispose()
        {
            foreach (KeyValuePair<string, SqlDBClient> conn in DBClient)
            {
                conn.Value.Dispose();
            }

            DBClient.Clear();
        }

        /// <summary> close connections such as Key in SqlDB connection list</summary>
        public void Dispose(string DBKey)
        {
            try
            {
                if (DBClient.ContainsKey(GetDBKey(ref DBKey)))
                {
                    DBClient[DBKey].Dispose();
                    DBClient[DBKey] = null;
                    DBClient.Remove(DBKey);
                }
            }
            catch
            {
                System.Data.SqlClient.SqlConnection.ClearAllPools();
            }
        }

        /// <summary>
        /// This will add an array of parameters to a SqlCommand. This is used for an IN statement.
        /// Use the returned value for the IN part of your SQL call. (i.e. SELECT * FROM table WHERE field IN ({paramNameRoot}))
        /// </summary>
        /// <param name="cmd">The SqlCommand object to add parameters to.</param>
        /// <param name="values">The array of strings that need to be added as parameters.</param>
        /// <param name="paramNameRoot">What the parameter should be named followed by a unique value for each value. This value surrounded by {} in the CommandText will be replaced.</param>
        /// <param name="start">The beginning number to append to the end of paramNameRoot for each value.</param>
        /// <param name="separator">The string that separates the parameter names in the sql command.</param>
        public static SqlParameter[] AddArrayParameters<T>(ref SqlCommand cmd, IEnumerable<T> values, string paramNameRoot, int start = 1, string separator = ", ")
        {
            /* An array cannot be simply added as a parameter to a SqlCommand so we need to loop through things and add it manually. 
             * Each item in the array will end up being it's own SqlParameter so the return value for this must be used as part of the
             * IN statement in the CommandText.
             */
            var parameters = new List<SqlParameter>();
            var parameterNames = new List<string>();
            var paramNbr = start;
            foreach (var value in values)
            {
                var paramName = string.Format("@{0}{1}", paramNameRoot, paramNbr++);
                parameterNames.Add(paramName);
                parameters.Add(cmd.Parameters.AddWithValue(paramName, value));
            }

            cmd.CommandText = cmd.CommandText.Replace("{" + paramNameRoot + "}", string.Join(separator, parameterNames));

            return parameters.ToArray();
        }

        public bool ExcuteSqlStoredProcedure(ref SqlCommand command, ref SqlDataReader reader, bool txnFlag = true) { return ExcuteSqlStoredProcedure(string.Empty, ref command, ref reader, txnFlag); }
        public bool ExcuteSqlStoredProcedure(string DBKey, ref SqlCommand command, ref SqlDataReader reader, bool txnFlag = true)
        {
            command.CommandType = CommandType.StoredProcedure;
            return ExcuteSqlCommand(DBKey, ref command, ref reader, txnFlag);
        }

        public bool ExcuteSqlStoredProcedure(ref SqlCommand command, bool txnFlag = true) { return ExcuteSqlStoredProcedure(string.Empty, ref command, txnFlag); }
        public bool ExcuteSqlStoredProcedure(string DBKey, ref SqlCommand command, bool txnFlag = true)
        {
            command.CommandType = CommandType.StoredProcedure;
            return ExcuteSqlCommand(DBKey, ref command, txnFlag);
        }

        public bool ExcuteSqlCommand(ref SqlCommand command, ref SqlDataReader reader, bool txnFlag = true) { return ExcuteSqlCommand(string.Empty, ref command, ref reader, txnFlag); }
        public bool ExcuteSqlCommand(string DBKey, ref SqlCommand command, ref SqlDataReader reader, bool txnFlag = true)
        {
            if (DBClient.ContainsKey(GetDBKey(ref DBKey)))
            {
                return DBClient[DBKey].ExcuteSqlCommand(ref command, ref reader, txnFlag);
            }
            else
            {
                ErrorLog("DBClient not found : " + DBKey);
                return false;
            }
        }


        public bool ExcuteSqlCommand(ref SqlCommand command, bool txnFlag = true) { return ExcuteSqlCommand(string.Empty, ref command, txnFlag); }
        public bool ExcuteSqlCommand(string DBKey, ref SqlCommand command, bool txnFlag = true)
        {
            if (DBClient.ContainsKey(GetDBKey(ref DBKey)))
            {
                return DBClient[DBKey].ExcuteSqlCommand(ref command, txnFlag);
            }
            else
            {
                ErrorLog("DBClient not found : " + DBKey);
                return false;
            }
        }

        public bool ExcuteSqlCommand(string command) { return ExcuteSqlCommand(string.Empty, command); }
        public bool ExcuteSqlCommand(string DBKey, string command)
        {
            if (DBClient.ContainsKey(GetDBKey(ref DBKey)))
            {
                return DBClient[DBKey].ExcuteSqlCommand(command);
            }
            else
            {
                ErrorLog("DBClient not found : " + DBKey);
                return false;
            }
        }

        public bool ExcuteSqlCommand(string command, ref SqlDataReader retSqlReader) { return ExcuteSqlCommand(string.Empty, command, ref retSqlReader); }
        public bool ExcuteSqlCommand(string DBKey, string command, ref SqlDataReader retSqlReader)
        {
            if (DBClient.ContainsKey(GetDBKey(ref DBKey)))
            {
                return DBClient[DBKey].ExcuteSqlCommand(command, ref retSqlReader);
            }
            else
            {
                ErrorLog("DBClient not found : " + DBKey);
                return false;
            }
        }

        public bool ExcuteSqlCommand(string command, ref DataSet retSqlDataSet) { return ExcuteSqlCommand(string.Empty, command, ref retSqlDataSet); }
        public bool ExcuteSqlCommand(string DBKey, string command, ref DataSet retSqlDataSet)
        {
            if (DBClient.ContainsKey(GetDBKey(ref DBKey)))
            {
                return DBClient[DBKey].ExcuteSqlCommand(command, ref retSqlDataSet);
            }
            else
            {
                ErrorLog("DBClient not found : " + DBKey);
                return false;
            }
        }

        public bool RollBack()
        {
            return EndTransaction(false);
        }

        public bool EndTransaction(bool success = true)
        {
            if (success)
            {
                foreach (KeyValuePair<string, SqlDBClient> conn in DBClient)
                {
                    if (conn.Value.QueryFailed)
                    {
                        success = false;
                        break;
                    }
                    else
                        success = true;
                }
            }

            bool globalTxnCommit = false;

            foreach (KeyValuePair<string, SqlDBClient> conn in DBClient)
            {
                bool localTxnCommit = false;
                if (success)
                    localTxnCommit = conn.Value.EndTransaction();
                else
                    localTxnCommit = conn.Value.RollbackTransaction();

                globalTxnCommit = globalTxnCommit ? true : localTxnCommit;
            }

            return globalTxnCommit;
        }
    }
}
