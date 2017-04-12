using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using System.Text;
using mSeed.Common;
using mSeed.mDBTxnBlock;
using System.Data;
using System.Data.SqlClient;
using mSeed.Platform.Cache;
using mSeed.RedisManager;
using mSeed.Common.BackgroundWorker;

namespace mSeed.Platform.PushNotification
{
    public partial class PushManager
    {
        private const string log_tag = "push";

        public static bool SetPushToken(long service_access_id, string service_key, long user_idx, string push_token, mSeed.Common.ePushType push_type, bool serverOff)
        {
            game_access_auth authInfo = Server_Cache.GetSystemCacheInstance().GetCacheAuthInfo(service_access_id, service_key);
            bool isSuccess = false;

            if (authInfo.game_service_id > 0)
            {
                PlatformLogger logger = new PlatformLogger(log_tag);
                TxnBlock TB = new TxnBlock();
                {
                    TB.DBConn(SystemConfig.GetSystemConfigInstance().platformDB, SystemConfig.GetSystemConfigInstance().platformDB.SetDBAlias);
                    TB.IsoLevel = IsolationLevel.ReadUncommitted;
                    TB.Elog = logger.DBLog;

                    user_push_token tokenInfo = GetUserTokenInfo(ref TB, authInfo.game_service_id, push_token);
                    SqlCommand cmd = new SqlCommand();
                    string paramlog ;

                    if (tokenInfo.token_idx > 0)
                    {
                        string setQuery = string.Format("UPDATE {0} SET user_id = @p1, service_type = @p3, push_status = @p5, reg_date = GETDATE() WHERE token_idx = @p2", DB_Define.DBTables[DB_Define.eDBTables.user_push_token]);
                        cmd.CommandText = setQuery;
                        cmd.Parameters.AddWithValue("@p1", user_idx);
                        cmd.Parameters.AddWithValue("@p2", tokenInfo.token_idx);
                        cmd.Parameters.AddWithValue("@p3", (int)push_type);
                        cmd.Parameters.AddWithValue("@p5", serverOff ? 1 : 0);
                        paramlog = string.Format("setUpdate - p1 = {0}, p2 = {1}, p3 = {2}, p4 = {3}", user_idx, tokenInfo.token_idx, (int)push_type, serverOff ? 1 : 0);
                    }
                    else
                    {
                        string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON user_id = @p1 AND game_service_id = @p2 AND service_type = @p3
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                    push_token = @p4,
                                                    push_status = @p5,
                                                    reg_date = GETDATE()
                                                WHEN NOT MATCHED THEN
                                                   INSERT (user_id, game_service_id, service_type, push_token, push_status, reg_date)
                                                   VALUES (@p1, @p2, @p3, @p4, @p5, GETDATE());
                                    ", DB_Define.DBTables[DB_Define.eDBTables.user_push_token]);
                        cmd.CommandText = setQuery;
                        cmd.Parameters.AddWithValue("@p1", user_idx);
                        cmd.Parameters.AddWithValue("@p2", authInfo.game_service_id);
                        cmd.Parameters.AddWithValue("@p3", (int)push_type);
                        cmd.Parameters.AddWithValue("@p4", push_token);
                        cmd.Parameters.AddWithValue("@p5", serverOff ? 1 : 0);
                        paramlog = string.Format("setMERGE - p1 = {0}, p2 = {1}, p3 = {2}, p4 = {3}, p5 = {4}", user_idx, authInfo.game_service_id, (int)push_type, push_token, serverOff ? 1 : 0);
                    }
                    isSuccess = TB.ExcuteSqlCommand(ref cmd);
                    cmd.Dispose();
                    logger.DBLog(paramlog);                    
                    TB.EndTransaction();
                    TB.Dispose();
                    logger.Dispose();
                }
            }

            return isSuccess;
        }

        public static bool InsertPushService(system_push_service setData)
        {
            bool isResult = false;
            TxnBlock TB = new TxnBlock();
            {
                PlatformLogger logger = new PlatformLogger(log_tag);
                TB.DBConn(SystemConfig.GetSystemConfigInstance().platformDB, SystemConfig.GetSystemConfigInstance().platformDB.SetDBAlias);
                TB.IsoLevel = IsolationLevel.ReadUncommitted;
                TB.Elog = logger.DBLog;

                string setQuery = string.Format(@"Insert Into {0} (game_service_id, title, message, push_status, push_reason, send_reserv_date, register, reg_date, push_type)
                                                        Values ({1}, N'{2}', N'{3}', {4}, N'{5}', N'{6}', N'{7}', getdate(), {8})"
                                                    , DB_Define.DBTables[DB_Define.eDBTables.system_push_service]
                                                    , setData.game_service_id, setData.title, setData.message, setData.push_status, setData.push_reason
                                                    , setData.send_reserv_date.ToString("yyyy-MM-dd HH:mm:ss"), setData.register, setData.push_type);
                isResult = TB.ExcuteSqlCommand(setQuery);

                TB.EndTransaction();
                TB.Dispose();
                logger.Dispose();
            }
            return isResult;
        }

        public static List<ret_push_msg> GetPushServiceData(ePushStatus status)
        {
            List<ret_push_msg> retObj = new List<ret_push_msg>();
            TxnBlock TB = new TxnBlock();
            {
                PlatformLogger logger = new PlatformLogger(log_tag);
                TB.DBConn(SystemConfig.GetSystemConfigInstance().platformDB, SystemConfig.GetSystemConfigInstance().platformDB.SetDBAlias);
                TB.IsoLevel = IsolationLevel.ReadUncommitted;
                TB.Elog = logger.DBLog;

                string setQuery = string.Format(@"SELECT push_id, game_service_id, title, message, push_type
                                                    From {0} WITH(NOLOCK) WHERE send_reserv_date < GETDATE() AND push_status = {1}"
                                                    , DB_Define.DBTables[DB_Define.eDBTables.system_push_service], (int)status);
                List<ret_push_msg> setObj = GenericFetch.FetchFromDB_MultipleRow<ret_push_msg>(ref TB, setQuery);
                foreach (ret_push_msg msg in setObj)
                {
                    msg.token_count = GetTargetTokenCount(ref TB, msg.game_service_id);
                    msg.fcm_key = GameServiceManager.GetFCMPushServiceInfo(msg.game_service_id).service_secret;
                    msg.apns_cert = GameServiceManager.GetAPNSPushServiceInfo(msg.game_service_id).service_secret;
                    retObj.Add(msg);
                }
                TB.EndTransaction();
                TB.Dispose();
                logger.Dispose();
            }
            return retObj;
        }

        public static system_push_service GetPushServiceData(long game_id, long push_id)
        {
            system_push_service retObj = null;
            TxnBlock TB = new TxnBlock();
            {
                PlatformLogger logger = new PlatformLogger(log_tag);
                TB.DBConn(SystemConfig.GetSystemConfigInstance().platformDB, SystemConfig.GetSystemConfigInstance().platformDB.SetDBAlias);
                TB.IsoLevel = IsolationLevel.ReadUncommitted;
                TB.Elog = logger.DBLog;

                string setQuery = string.Format(@"Select * From {0} WITH(NOLOCK) Where push_id = {1} And game_service_id = {2}"
                                                    , DB_Define.DBTables[DB_Define.eDBTables.system_push_service], push_id, game_id);
                retObj = GenericFetch.FetchFromDB<system_push_service>(ref TB, setQuery);

                TB.EndTransaction();
                TB.Dispose();
                logger.Dispose();
            }
            return retObj == null ? new system_push_service() : retObj;
        }

        public static bool SetPushStatus(long game_id, long push_id, ePushStatus pushStatus)
        {
            bool isResult = false;
            TxnBlock TB = new TxnBlock();
            {
                PlatformLogger logger = new PlatformLogger(log_tag);
                TB.DBConn(SystemConfig.GetSystemConfigInstance().platformDB, SystemConfig.GetSystemConfigInstance().platformDB.SetDBAlias);
                TB.IsoLevel = IsolationLevel.ReadUncommitted;
                TB.Elog = logger.DBLog;

                string setQuery = string.Format(@"Update {0} Set push_status = {3} Where push_id = {1} And game_service_id = {2}"
                                                    , DB_Define.DBTables[DB_Define.eDBTables.system_push_service], push_id, game_id, (int)pushStatus);
                isResult = TB.ExcuteSqlCommand(setQuery);

                TB.EndTransaction();
                TB.Dispose();
                logger.Dispose();
            }
            return isResult;
        }

        public const int pagelimit = 10000;
        public static Dictionary<long, Dictionary<int, List<ret_push_token>>> checkPushSetCount = new Dictionary<long, Dictionary<int, List<ret_push_token>>>();

        //static int worker = 0;

        public static long GetTargetTokenCount(ref TxnBlock TB, long game_service_id)
        {
            List<ret_push_token> pushlist = new List<ret_push_token>();

            string query = string.Format(@"SELECT COUNT(*) AS chk_count FROM {0} 
                                                    WITH(INDEX([IX_game_service]), NOLOCK)
                                                        WHERE  game_service_id = {1} AND push_status = 0
                                                    "
                                        , DB_Define.DBTables[DB_Define.eDBTables.user_push_token], game_service_id);
            return GenericFetch.FetchFromSingleLong(ref TB, query);
        }

        public static user_push_token GetUserTokenInfo(ref TxnBlock TB, long game_service_id, string pushtoken)
        {
            string setQuery = string.Format(@"SELECT * From {0} WITH(INDEX([IDX_Unique_push_token_with_game_service_id]), NOLOCK)
                                                                        WHERE game_service_id = {1} AND push_token = N'{2}'"
                                    , DB_Define.DBTables[DB_Define.eDBTables.user_push_token], game_service_id, pushtoken);
            user_push_token retObj = GenericFetch.FetchFromDB<user_push_token>(ref TB, setQuery);
            return retObj == null ? new user_push_token() : retObj;
        }

        #region sync_outofmemory
        //public static List<ret_push_token> CreatePushList(long game_service_id, out long chkCount)
        //{
        //    //PlatformLogger logger = new PlatformLogger(log_tag);
        //    List<ret_push_token> chkList = new List<ret_push_token>();
        //    int totalpageCount = 0;
        //    int currentpageCount = 0;
        //    checkPushSetCount[game_service_id] = new Dictionary<int, List<ret_push_token>>();
        //    List<int> pageCount = new List<int>();

        //    TxnBlock TB = new TxnBlock();
        //    {
        //        TB.DBConn(SystemConfig.GetSystemConfigInstance().platformDB, SystemConfig.GetSystemConfigInstance().platformDB.SetDBAlias);
        //        TB.IsoLevel = IsolationLevel.ReadUncommitted;
        //        //TB.Elog = logger.DBLog;
        //        chkCount = GetTargetTokenCount(ref TB, game_service_id);

        //        totalpageCount = (int)Math.Floor((double)(chkCount / pagelimit));

        //        while (chkCount > 1 && totalpageCount >= currentpageCount)
        //        {
        //            pageCount.Add(currentpageCount);
        //            currentpageCount++;
        //        }
        //        foreach (int page in pageCount)
        //        {
        //            SetPushTokenList(ref TB, game_service_id, page, totalpageCount);
        //        }
        //        TB.EndTransaction();
        //        TB.Dispose();
        //    }

        //    chkCount = totalpageCount;
        //    return chkList;
        //}

        //public static void SetPushTokenList(ref TxnBlock TB, long game_service_id, int page_count, int totalpage)
        //{
        //    int outWork = 0;
        //    int outCompletion = 0;
        //    ThreadPool.GetAvailableThreads(out outWork, out outCompletion);
        //    mSeed.Common.mLogger.mLogger.Debug(string.Format("call page = {0}, worker = {1}, I/O = {2} ", page_count, outWork, outCompletion), "call page");
        //    mSeed.Common.mLogger.mLogger.GetLoggerInstance().FlushLog();
        //    string setkey = string.Format("push_list_count_{0}", game_service_id);
        //    SqlCommand cmd = new SqlCommand("Get_PushTokenList");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add("@game_service_id", SqlDbType.BigInt).Value = game_service_id;
        //    cmd.Parameters.Add("@pageCount", SqlDbType.Int).Value = page_count;
        //    cmd.Parameters.Add("@getCount", SqlDbType.Int).Value = pagelimit;

        //    List<ret_push_token> pushlist = GenericFetch.FetchFromDB_MultipleRow<ret_push_token>(ref TB, cmd);
        //    checkPushSetCount[game_service_id][page_count] = pushlist;
        //}

        #endregion

        #region async_max_connection_pool
        const int maxWorker = 6;
        static long time_bgn = DateTime.Now.Ticks;
        static long time_end = DateTime.Now.Ticks;
        const double tick_to_sec = 10000000.0f;

        public static List<ret_push_token> CreatePushList(long game_service_id, out long chkCount)
        {
            List<ret_push_token> chkList = new List<ret_push_token>();
            int totalpageCount = 0;

            TxnBlock TB = new TxnBlock();
            {
                TB.DBConn(SystemConfig.GetSystemConfigInstance().platformDB, SystemConfig.GetSystemConfigInstance().platformDB.SetDBAlias);
                TB.IsoLevel = IsolationLevel.ReadUncommitted;
                //TB.Elog = logger.DBLog;
                chkCount = GetTargetTokenCount(ref TB, game_service_id);

                totalpageCount = (int)Math.Floor((double)(chkCount / pagelimit));
                TB.EndTransaction();
                TB.Dispose();
            }
            RemoveTokenList(game_service_id);
            //checkPushSetCount[game_service_id] = new Dictionary<int, List<ret_push_token>>();

            List<int> workerCount = new List<int>();

            for (int i = 0; i < maxWorker; i++)
            {
                workerCount.Add(i);
            }

            workerCount.ForEach(setWorker =>
            {
                Thread.Sleep(setWorker * 1000);
                mSeed.Common.BackgroundWorker.BackgroundTaskRunner.FireAndForgetTask(() =>
                {
                    SetPushTokenList(game_service_id, setWorker, totalpageCount);
                });
            });
            
            chkCount = totalpageCount;
            return chkList;
        }

        public static void RemoveTokenList(long game_service_id)
        {
            for (int worker = 0; worker < maxWorker; worker++)
            {
                string setkey = GetPushTokenKey(game_service_id, worker);
                RedisManager.GetRedisInstance().GetRedisController().RemoveList(setkey);
            }
        }

        public static long GetRemainTokenCount(long game_service_id)
        {
            long retCount = 0;
            string setkey;
            for (int worker = 0; worker < maxWorker; worker++)
            {
                setkey = GetPushTokenKey(game_service_id, worker);
                long chkCount = RedisManager.GetRedisInstance().GetRedisController().GetListCount(setkey);
                time_end = DateTime.Now.Ticks;
                mSeed.Common.mLogger.mLogger.Debug(string.Format("{0} - {2} itemcount : {1}", ((time_end - time_bgn) / tick_to_sec)
                    , chkCount
                    , setkey
                    ));
                retCount += chkCount;
                mSeed.Common.mLogger.mLogger.GetLoggerInstance().FlushLog();
            }
            return retCount;
        }

        public static string GetPushTokenKey(long game_service_id, int worker)
        {
            return string.Format("push_list_count_{0}_{1}", game_service_id, worker);
        }

        public static void SetPushTokenList(long game_service_id, int worker, int totalpage)
        {
            string setkey = GetPushTokenKey(game_service_id, worker);

            int trycount = 0;
            int page_count = trycount * maxWorker + worker;

            for (; page_count < totalpage; page_count = trycount * maxWorker + worker)
            {
                SetPushTokenWorker(game_service_id, totalpage, trycount, page_count, worker);
                trycount++;
            }        
        }

        public static List<ret_push_token> GetPushTokenList(long game_service_id, int page_count, int getCount = pagelimit)
        {
            List<ret_push_token> retObj = new List<ret_push_token>();
            TxnBlock TB = new TxnBlock();
            {
                TB.DBConn(SystemConfig.GetSystemConfigInstance().platformDB, SystemConfig.GetSystemConfigInstance().platformDB.SetDBAlias);
                TB.IsoLevel = IsolationLevel.ReadUncommitted;
                SqlCommand cmd = new SqlCommand("Get_PushTokenList");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@game_service_id", SqlDbType.BigInt).Value = game_service_id;
                cmd.Parameters.Add("@pageCount", SqlDbType.Int).Value = page_count;
                cmd.Parameters.Add("@getCount", SqlDbType.Int).Value = getCount;
                SqlDataReader getDr = null;
                TB.ExcuteSqlCommand(ref cmd, ref getDr);
                if (getDr != null)
                {
                    while (getDr.Read())
                    {
                        ret_push_token setObj = new ret_push_token();
                        setObj.token_idx = long.Parse(getDr["token_idx"].ToString());
                        setObj.user_id = long.Parse(getDr["user_id"].ToString());
                        setObj.service_type = int.Parse(getDr["service_type"].ToString());
                        setObj.push_token = getDr["push_token"].ToString();
                        retObj.Add(setObj);
                    }
                    getDr.Dispose(); getDr.Close();
                }
                cmd.Dispose();
            }
            TB.EndTransaction(false);
            TB.Dispose();
            return retObj;
        }

        private static void SetPushTokenWorker(long game_service_id, int totalpage, int trycount, int page_count, int worker)
        {
            string setkey = GetPushTokenKey(game_service_id, worker);

            mSeed.Common.mLogger.mLogger.Debug(string.Format("call page = {0} / {1}, worker = {2}", page_count, totalpage, worker), "call page");
            mSeed.Common.mLogger.mLogger.GetLoggerInstance().FlushLog();
            List<ret_push_token> retObj = GetPushTokenList(game_service_id, page_count);
            RedisManager.GetRedisInstance().GetRedisController().ListAdds(setkey, retObj.Select(item => item.push_token).ToList());
            retObj.Clear();
            Thread.Sleep(100);
        }
        #endregion

        public static Result_Define.eResult DeleteTokens(ref List<long> tokens)
        {
            bool isResult = false;
            TxnBlock TB = new TxnBlock();
            {
                TB.DBConn(SystemConfig.GetSystemConfigInstance().platformDB, SystemConfig.GetSystemConfigInstance().platformDB.SetDBAlias);
                TB.IsoLevel = IsolationLevel.ReadUncommitted;
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format(@"DELETE FROM {0} WHERE token_idx IN "
                                                    , DB_Define.DBTables[DB_Define.eDBTables.user_push_token]));
                sb.Append("({tokens})");
                SqlCommand cmd = new SqlCommand(sb.ToString());
                TxnBlock.AddArrayParameters(ref cmd, tokens, "tokens");
                isResult = TB.ExcuteSqlCommand(ref cmd);
                cmd.Dispose();
            }
            TB.EndTransaction(isResult);
            TB.Dispose();

            return isResult ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }
    }
}
