using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace mSeed.mDBTxnBlock
{
    public partial class SqlDBClient
    {
        private LOGFUNC elog = null;    // for log delgate

        public LOGFUNC Elog
        {
            get { return elog; }
            set { elog = value; }
        }

        private void ErrorLog(string error)
        {
            if (elog != null)
                elog(error);
        }

        private IsolationLevel isoLevel = IsolationLevel.ReadUncommitted;

        public IsolationLevel IsoLevel
        {
            get { return isoLevel; }
            set { isoLevel = value; }
        }

        private SqlConnection dbConn = null;
        public SqlConnection DbConn
        {
            get { return dbConn; }
        }

        private SqlTransaction dbTxn = null;
        private SqlCommand setCommand = null;

        public bool debug = false;                            // for detail log
        private bool failed = false;                            // At least one query has failed. No further query will be run
        
        public bool QueryFailed
        {
            get { return failed; }
        }

        private bool recommit_on_deadlock = false;              // Transaction deadlock retry flag
        private bool started = false;                           // Transaction was triggered by a query
        private List<string> querys = new List<string>();       // Executed queries. For history
        private string TxnBlockKey = string.Empty;
        List<DetailLogStruct> setDetailLogs = new List<DetailLogStruct>();
        private string setConnString = string.Empty;

        private void reset_internal() 
        {
            started = false;
            failed = false;
            querys.Clear();
            setDetailLogs.Clear();
            time_end = time_bgn =  txn_time_end = txn_time_bgn = System.DateTime.Now.Ticks;
        }

        public SqlDBClient(DBEndpoint conn, string TxnKey, bool debugFlag = true)
        {
            reset_internal();

            setConnString = string.Format("Server={0};Database={1};UID={2};PWD={3}", conn.Host, conn.Database, conn.UserID, conn.UserPW);
            //DetailQueryLog(setConnString);
            debug = debugFlag;
            TxnBlockKey = TxnKey;
        }

        public SqlDBClient(string conn, string TxnKey, bool debugFlag = true)
        {
            reset_internal();
            debug = debugFlag;

            conn.IndexOf("Server", StringComparison.OrdinalIgnoreCase);
            if (
                !string.IsNullOrEmpty(conn)
                && (conn.IndexOf("Server", StringComparison.OrdinalIgnoreCase) != -1
                    || conn.IndexOf("Data Source", StringComparison.OrdinalIgnoreCase) != -1
                    || conn.IndexOf("Address", StringComparison.OrdinalIgnoreCase) != -1
                    || conn.IndexOf("Addr", StringComparison.OrdinalIgnoreCase) != -1
                    || conn.IndexOf("Network Address", StringComparison.OrdinalIgnoreCase) != -1)
                && (conn.IndexOf("Database", StringComparison.OrdinalIgnoreCase) != -1
                    || conn.IndexOf("Initial Catalog", StringComparison.OrdinalIgnoreCase) != -1)
                && (conn.IndexOf("UID", StringComparison.OrdinalIgnoreCase) != -1
                    || conn.IndexOf("User ID", StringComparison.OrdinalIgnoreCase) != -1)
                && (conn.IndexOf("PWD", StringComparison.OrdinalIgnoreCase) != -1
                    || conn.IndexOf("Password", StringComparison.OrdinalIgnoreCase) != -1)
             )
            {
                setConnString = conn;
                TxnBlockKey = TxnKey;
            }
        }

        ~SqlDBClient()
        {
            Dispose();
        }

        private bool MakeDBConnection()
        {
            if (string.IsNullOrEmpty(setConnString))
                return false;

            try
            {
                dbConn = new SqlConnection(setConnString);
                dbConn.Open();
                setCommand = dbConn.CreateCommand();
                return true;
            }
            catch (Exception e)
            {
                if (elog != null)
                {
                    ErrorLog("StackTrace" + e.StackTrace);
                    ErrorLog("DBClinet Connection Exception Type:" + e.GetType());
                    ErrorLog("DBClinet Connection Exception Message:" + e.Message);
                }
                return false;
            }
        }

        public void Dispose()
        {
            try
            {
                if (sqlDR != null)
                    sqlDR.Dispose();

                if (sqlDS != null)
                    sqlDS.Dispose();

                if (dbTxn != null)
                {
                    RollbackTransaction();
                    dbTxn.Dispose();
                }

                if (setCommand != null)
                    setCommand.Dispose();

                if (dbConn != null)
                {
                    dbConn.Dispose();
                    dbConn.Close();
                }

                dbTxn = null;
                setCommand = null;
                dbConn = null;
            }
            catch (Exception e)
            {
                if (elog != null)
                {
                    ErrorLog("StackTrace" + e.StackTrace);
                    ErrorLog("Dispose Exception CallStack:" + e.StackTrace);
                    ErrorLog("Dispose Exception Type:" + e.GetType());
                    ErrorLog("Dispose Exception Message:" + e.Message);
                }
            }
        }

        private void TxnStart(string TxnKey)
        {
            if (dbTxn != null)
                dbTxn.Dispose();

            dbTxn = dbConn.BeginTransaction(isoLevel, TxnKey);
            started = true;
        }

        private void setSqlCommand(string command, bool isSP = false)
        {
            try
            {
                if (setCommand != null)
                    setCommand.Dispose();

                setCommand = new SqlCommand(command, dbConn, dbTxn);
                if (isSP)
                    setCommand.CommandType = CommandType.StoredProcedure;
            }
            catch (Exception e)
            {
                if (elog != null)
                {
                    ErrorLog("Command : " + command + "StackTrace" + e.StackTrace);
                    ErrorLog("SQL Query Exception Type:" + e.GetType());
                    ErrorLog("SQL Query Exception Message:" + e.Message);
                }
            }   
        }
        private long txn_time_bgn = 0;
        private long time_bgn = 0;
        private long time_end = 0;
        private long txn_time_end = 0;
        private const double tick_to_ms = 10000.0f;
        private const double tick_to_sec = 10000000.0f;
        private const double tick_to_ns = 100.0f;

        private double timeGap(bool txnflag = false, double tick_to = tick_to_sec)
        {
            return txnflag ? (txn_time_end - txn_time_bgn) / tick_to : (time_end - time_bgn) / tick_to;
        }

        private bool CheckQueryFailed(bool CheckTxn = false)
        {
            if (dbConn == null)
            {
                if (!MakeDBConnection())
                    return false;
            }

            if(sqlDR != null)
                sqlDR.Dispose();
            if (sqlDS != null)
                sqlDS.Dispose();

            if (CheckTxn)
            {
                //原版
                if (failed || dbConn == null)
                    return true;
                //修改
                 //if (dbConn == null)
                   // return true;
                else
                {
                    if (!started)
                        TxnStart(TxnBlockKey);

                    return false;
                }
            }
            else
                return false;
        }

        private class DetailLogStruct
        {
            public long exec_times_bgn { get; set; }
            public long exec_times_end { get; set; }
            public double exec_times { get; set; }
            public string exec_query { get; set; }
            public string error_info { get; set; }
        }

        private void DetailCommandLog(ref SqlCommand command, string Error = "")
        {
            string cmdlog = command.CommandType == CommandType.StoredProcedure ? SqlCommandDumper.GetCommandText(command) : command.CommandText;

            DetailQueryLog(command.CommandType == CommandType.StoredProcedure ?
                                    string.Format("StoredProcedure - {0}", cmdlog) :
                                    string.Format("Sql Query - {0}", cmdlog),
                            Error);
        }


        private void DetailQueryLog(string Query, string Error = "")
        {
            time_end = System.DateTime.Now.Ticks;
            DetailLogStruct setLog = new DetailLogStruct();

            setLog.exec_times_bgn = time_bgn;
            setLog.exec_times_end = time_end;
            setLog.exec_times = timeGap();
            setLog.exec_query = string.Format("Sql Query - {0}", Query); ;
            setLog.error_info = Error;

            setDetailLogs.Add(setLog);
        }

        private void dump_query_stat()
        {
            if (elog != null)
            {
                int i = 0;
                foreach (DetailLogStruct log in setDetailLogs)
                {
                    ErrorLog(string.Format("query[{0}][{1}] : {2}  (error_info : {3})", i, log.exec_times, log.exec_query, string.IsNullOrEmpty(log.error_info) ? "success" : log.error_info));
                    i++;
                }
            }
        }

        public bool ExcuteSqlCommand(ref SqlCommand command, bool txnFlag = true)
        {
            if (CheckQueryFailed(txnFlag))
                return false;

            try
            {
                command.Connection = dbConn;
                command.Transaction = dbTxn;
                command.ExecuteNonQuery();

                if(debug)
                    DetailCommandLog(ref command);

                querys.Add(command.CommandText);

                return true;
            }
            catch (Exception e)
            {
                ErrorLog("Command : " + command.CommandText + "StackTrace" + e.StackTrace);
                failed = true;

                if (debug)
                    DetailCommandLog(ref command, "SQL Query Exception Type:" + e.GetType() + "SQL Query Exception Message:" + e.Message);
                //ErrorLog("ExcuteCommand Fail - Try Clear Connection Pool " + TxnBlockKey);                
                //System.Data.SqlClient.SqlConnection.ClearAllPools();
                
                querys.Add(command.CommandText);

                return false;
            }   
        }

        public bool ExcuteSqlCommand(ref SqlCommand command, ref SqlDataReader reader, bool txnFlag = true)
        {
            if (CheckQueryFailed(txnFlag))
                return false;

            try
            {
                command.Connection = dbConn;
                command.Transaction = dbTxn;
                reader = command.ExecuteReader();

                if (debug)
                    DetailCommandLog(ref command);

                querys.Add(command.CommandText);
                sqlDR = reader;

                return true;
            }
            catch (Exception e)
            {
                ErrorLog("Command : " + command.CommandText + "StackTrace" + e.StackTrace);
                failed = true;

                if (debug)
                    DetailCommandLog(ref command, "SQL Query Exception Type:" + e.GetType() + "SQL Query Exception Message:" + e.Message);
                //ErrorLog("ExcuteCommand Fail - Try Clear Connection Pool " + TxnBlockKey);
                //System.Data.SqlClient.SqlConnection.ClearAllPools();

                querys.Add(command.CommandText);
                sqlDR = reader;

                return false;
            }
        }

        public bool ExcuteSqlCommand(string command)
        {
            string checkCommand = command.ToLower();
            if (CheckQueryFailed(checkCommand.Contains("update ") || checkCommand.Contains("insert ") || checkCommand.Contains("merge")))
                return false;
            try
            {
                setSqlCommand(command);
                setCommand.ExecuteNonQuery();

                if (debug)
                    DetailQueryLog(command);

                querys.Add(command);

                return true;
            }
            catch (Exception e)
            {
                ErrorLog("Command : " + command + "StackTrace" + e.StackTrace);
                failed = true;

                if (debug)
                    DetailQueryLog(command, "SQL Query Exception Type:" + e.GetType() + "SQL Query Exception Message:" + e.Message);
                //ErrorLog("ExcuteCommand Fail - Try Clear Connection Pool " + TxnBlockKey);
                //System.Data.SqlClient.SqlConnection.ClearAllPools();
                
                querys.Add(command);

                return false;
            }
        }

        SqlDataReader sqlDR = null;

        public bool ExcuteSqlCommand(string command, ref SqlDataReader retSqlReader)
        {
            string checkCommand = command.ToLower();
            if (CheckQueryFailed(checkCommand.Contains("update ") || checkCommand.Contains("insert ") || checkCommand.Contains("merge")))
                return false;

            retSqlReader = null;

            try
            {
                setSqlCommand(command);
                retSqlReader = setCommand.ExecuteReader();

                if (debug)
                    DetailQueryLog(command);

                querys.Add(command);
                sqlDR = retSqlReader;
                return true;
            }
            catch (Exception e)
            {


                ErrorLog("Command : " + command + "StackTrace" + e.StackTrace);
                failed = true;

                if (debug)
                    DetailQueryLog(command, "SQL Query Exception Type:" + e.GetType() + "SQL Query Exception Message:" + e.Message);
                //ErrorLog("ExcuteCommand Fail - Try Clear Connection Pool " + TxnBlockKey);
                //System.Data.SqlClient.SqlConnection.ClearAllPools();

                querys.Add(command);
                sqlDR = retSqlReader;

                return false;
            }
        }

        DataSet sqlDS = null;

        public bool ExcuteSqlCommand(string command, ref DataSet retSqlDataSet)
        {
            string checkCommand = command.ToLower();
            if (CheckQueryFailed(checkCommand.Contains("update ") || checkCommand.Contains("insert ") || checkCommand.Contains("merge")))
                return false;

            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                setSqlCommand(command);
                adapter.SelectCommand = setCommand;
                adapter.Fill(retSqlDataSet);

                if (debug)
                    DetailQueryLog(command);                

                querys.Add(command);
                sqlDS = retSqlDataSet;
                return true;
            }
            catch (Exception e)
            {
                ErrorLog("Command : " + command + "StackTrace" + e.StackTrace);
                failed = true;

                if (debug)
                    DetailQueryLog(command, "SQL Query Exception Type:" + e.GetType() + "SQL Query Exception Message:" + e.Message);
                //ErrorLog("ExcuteCommand Fail - Try Clear Connection Pool " + TxnBlockKey);
                //System.Data.SqlClient.SqlConnection.ClearAllPools();

                querys.Add(command);
                sqlDS = retSqlDataSet;
                return false;
            }
        }

        public bool RollbackTransaction()
        {
            if (querys.Count == 0)
                return false;
            if (debug)
                dump_query_stat();

            if (started)
            {

                if (dbTxn != null)
                {
                    try
                    {
                        dbTxn.Rollback();
                    }
                    catch (Exception e)
                    {
                        ErrorLog("StackTrace" + e.StackTrace);
                        ErrorLog("TxnBlock rollback failed Type:" + e.GetType());
                        ErrorLog("TxnBlock rollback failed Message:" + e.Message);
                        return false;
                    }
                }
                else
                {
                    ErrorLog("SqlTransaction Null Exception");
                }

                time_end = System.DateTime.Now.Ticks;

                ErrorLog(string.Format("TXN [rollback] => [{0}, {1}]", TxnBlockKey, timeGap()));
            }
            bool retTxn = true;

            reset_internal();

            return retTxn;
        }

        public bool EndTransaction()
        {
            if (querys.Count == 0)
                return false;

            bool success = false;
            if (debug)
                dump_query_stat();

            if (started)
            {
                int retried = 0;
                if (failed)
                {
                    if (dbTxn != null)
                    {
                        try
                        {
                            dbTxn.Rollback();
                        }
                        catch (Exception e)
                        {
                            ErrorLog("StackTrace" + e.StackTrace);
                            ErrorLog("TxnBlock rollback failed Type:" + e.GetType());
                            ErrorLog("TxnBlock rollback failed Message:" + e.Message);
                        }
                    }
                    else
                        ErrorLog("SqlTransaction Null Exception");

                }
                else
                {
                    while (retried < 5)
                    {
                        if (dbTxn != null)
                        {
                            try
                            {
                                dbTxn.Commit();
                                success = true;
                            }
                            catch (Exception e)
                            {
                                success = false;
                                try
                                {
                                    if (dbTxn != null)
                                    {
                                        dbTxn.Rollback();
                                    }
                                }
                                catch (Exception ex2)
                                {
                                    // This catch block will handle any errors that may have occurred 
                                    // on the server that would cause the rollback to fail, such as 
                                    // a closed connection.
                                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                                    Console.WriteLine("  Message: {0}", ex2.Message);
                                }

                                e.Message.ToLower();
                                ErrorLog("StackTrace" + e.StackTrace);

                                if (e.Message.Contains("deadlock") || e.Message.Contains("dead lock"))
                                {
                                    if (recommit_on_deadlock)
                                    {
                                        retried++;
                                        ErrorLog("recommits on deadlock with retried : try count " + retried);
                                    }
                                }
                                else
                                {
                                    ErrorLog("TxnBlock commit failed Type:" + e.GetType());
                                    ErrorLog("TxnBlock commit failed Message:" + e.Message);
                                }
                            }
                            finally
                            {
                                dbTxn.Dispose();
                            }
                            break;
                        }
                    }
                }
                txn_time_end = System.DateTime.Now.Ticks;

                if(retried > 0)
                    ErrorLog("commit was eventually succeeded with {retried: " + retried + ", success: " + success + "}");

                if (failed && !success)
                    ErrorLog(string.Format("TXN [rollback] => [{0}, {1}]", TxnBlockKey, timeGap(true)));
                else
                    ErrorLog(string.Format("TXN [commit] => [{0}, {1}]", TxnBlockKey, timeGap(true)));
            }

            bool retTxn = started;

            reset_internal();

            return retTxn;

        }
    }
}
