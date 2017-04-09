using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager.DBClass;

namespace TheSoul.DataManager
{
    public static class SnailLogManager
    {
        const string LogDBName = "log";
        const string LogRotateRuleTableName = "log_rotate_rule";

        public static string SnailLogTable(ref TxnBlock TB, string baseTableName, string dbkey = DataManager_Define.LogDB)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE ObjectName = '{1}'", LogRotateRuleTableName, baseTableName);
            log_rotate_rule retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<log_rotate_rule>(ref TB, DataManager_Define.RedisServerAlias_System, LogRotateRuleTableName, baseTableName, setQuery, dbkey);
            string retString = baseTableName;
            if (retObj != null)
            {
                if (retObj.RuleType == (byte)SnailLog_Define.Log_Rotate_Type.Day)
                    retString = string.Format("{0}_{1}", baseTableName, DateTime.Now.ToString("yyyyMMdd"));
                else if (retObj.RuleType == (byte)SnailLog_Define.Log_Rotate_Type.Week)
                    retString = string.Format("{0}_{1}", baseTableName, GenericFetch.StartOfWeek(DateTime.Now, DayOfWeek.Monday).ToString("yyyyMMdd"));
                else if (retObj.RuleType == (byte)SnailLog_Define.Log_Rotate_Type.Month)
                    retString = string.Format("{0}_{1}", baseTableName, DateTime.Now.ToString("yyyyMM01"));
            }
            return retString;
        }

        public static void SetOperationTo_Snail_SID(ref TxnBlock TB, string operation)
        {
            DataManager_Define.eCountryCode serviceArea = SystemData.GetServiceArea(ref TB);
            if (serviceArea == DataManager_Define.eCountryCode.China || serviceArea == DataManager_Define.eCountryCode.Taiwan)
            {
                if (SnailLog_Define.GetOperationSID.ContainsKey(operation) && string.IsNullOrEmpty(TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id])))
                {
                    TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.GetOperationSID[operation] + SnailLog_Define.Snail_s_id_Seperator_Operator);
                }
            }
            else
            {
                TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], operation);
            }
        }

        public static void SnailLog_CurrentUser_Log(ref TxnBlock TB, long userCount, string LogDB = DataManager_Define.LogDB)
        {
            string query = string.Format(@"INSERT INTO {0} (user_count, regdate)
                                                        VALUES ( @p1, GETDATE())", SnailLogTable(ref TB, SnailLog_Define._CurrentUser_Log_tablename));
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@p1", userCount);
            TB.ExcuteSqlCommand(LogDB, ref cmd);
            cmd.Dispose();
        }

        public static void SnailLog_write_game_player_action_log(ref TxnBlock TB, long AID, string LogDB = DataManager_Define.LogDB)
        {
            _snail_game_player_action_log snaillog = new _snail_game_player_action_log();
            snaillog.aid = AID;
            snaillog.s_account = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_account]);

            if (string.IsNullOrEmpty(snaillog.s_account))
                snaillog.s_account = AccountManager.GetAccountData(ref TB, AID).SNO.ToString();

            snaillog.s_role_name = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_role]);
            snaillog.d_create = DateTime.Now;
            snaillog.s_act_id = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id]);
            int nType = 0;
            snaillog.n_type = int.TryParse(TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_act_type]), out nType) ? nType : 0;
            snaillog.s_act_value = string.Empty;

            string query = string.Format(@"INSERT INTO {0} (aid, s_account, s_role_name, d_create, s_act_id, n_type, s_act_value)
                                                        VALUES ( @p1, @p2, @p3, @p4, @p5, @p6, @p7)", SnailLogTable(ref TB, SnailLog_Define.game_player_action_log_tablename));
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@p1", snaillog.aid);
            cmd.Parameters.AddWithValue("@p2", snaillog.s_account);
            cmd.Parameters.AddWithValue("@p3", snaillog.s_role_name);
            cmd.Parameters.AddWithValue("@p4", snaillog.d_create.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@p5", snaillog.s_act_id);
            cmd.Parameters.AddWithValue("@p6", snaillog.n_type);
            cmd.Parameters.AddWithValue("@p7", snaillog.s_act_value);

            TB.ExcuteSqlCommand(LogDB, ref cmd);
            cmd.Dispose();
        }

        public static void SnailLog_write_role_log(ref TxnBlock TB, long AID, string LogDB = DataManager_Define.LogDB)
        {
            _snail_role_log snaillog = new _snail_role_log();
            snaillog.aid = AID;
            snaillog.s_account = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_account]);
            if (string.IsNullOrEmpty(snaillog.s_account))
                snaillog.s_account = AccountManager.GetAccountData(ref TB, AID).SNO.ToString();

            snaillog.s_role = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_role]);
            snaillog.s_ip = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_ip]);
            snaillog.d_logintime = System.Convert.ToDateTime(TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.d_logintime]));
            snaillog.d_logouttime = System.Convert.ToDateTime(TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.d_logouttime]));
            snaillog.s_mac = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_mac]);
            snaillog.s_os_type = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_os_type]);
            snaillog.s_comment = string.Empty;

            string query = string.Format(@"INSERT INTO {0} (aid, n_id, s_account, s_role, s_ip, d_logintime, d_logouttime, s_mac, s_os_type, s_comment)
                                                        VALUES ( @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)", SnailLogTable(ref TB, SnailLog_Define.role_log_tablename));
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@p1", snaillog.aid);
            cmd.Parameters.AddWithValue("@p2", snaillog.n_id);
            cmd.Parameters.AddWithValue("@p3", snaillog.s_account);
            cmd.Parameters.AddWithValue("@p4", snaillog.s_role);
            cmd.Parameters.AddWithValue("@p5", snaillog.s_ip);
            cmd.Parameters.AddWithValue("@p6", snaillog.d_logintime.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@p7", snaillog.d_logouttime.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@p8", snaillog.s_mac);
            cmd.Parameters.AddWithValue("@p9", snaillog.s_os_type);
            cmd.Parameters.AddWithValue("@p10", snaillog.s_comment);

            TB.ExcuteSqlCommand(LogDB, ref cmd);

            cmd.Dispose();
        }

        public static void SnailLog_update_role_log(ref TxnBlock TB, long AID, string LogDB = DataManager_Define.LogDB)
        {
            string query = string.Format(@"SELECT TOP 1 log_idx FROM {0} WHERE aid = @p1 ORDER BY log_idx DESC", SnailLogTable(ref TB, SnailLog_Define.role_log_tablename));
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@p1", AID);

            SqlDataReader getDr = null;
            if (TB.ExcuteSqlCommand(LogDB, ref cmd, ref getDr))
            {
                if (getDr != null)
                {
                    if (getDr.Read())
                    {
                        long logIdx = getDr.GetInt64(0);
                        getDr.Dispose();
                        if (logIdx > 0)
                        {
                            DateTime logoutTime = DateTime.Now;
                            query = string.Format(@"UPDATE {0} SET d_logouttime = @p1 WHERE log_idx = {1}", SnailLogTable(ref TB, SnailLog_Define.role_log_tablename), logIdx);
                            SqlCommand cmdUpdate = new SqlCommand();
                            cmdUpdate.CommandText = query;
                            cmdUpdate.Parameters.AddWithValue("@p1", logoutTime.ToString("yyyy-MM-dd HH:mm:ss"));

                            TB.ExcuteSqlCommand(LogDB, ref cmdUpdate);
                            cmdUpdate.Dispose();
                        }
                    }
                    else
                        getDr.Dispose();
                }
            }
            cmd.Dispose();

            SnailLog_update_role_additional_log(ref TB, AID);
        }

        public static void SnailLog_renew_role_log(ref TxnBlock TB, long AID, string LogDB = DataManager_Define.LogDB)
        {
            string query = string.Format(@"SELECT TOP 1 * FROM {0} WHERE aid = {1} ORDER BY log_idx DESC", SnailLogTable(ref TB, SnailLog_Define.role_log_tablename), AID);
            _snail_role_log snaillog = GenericFetch.FetchFromDB<_snail_role_log>(ref TB, query, LogDB);

            if (snaillog != null)
            {
                query = string.Format(@"INSERT INTO {0} (aid, n_id, s_account, s_role, s_ip, d_logintime, d_logouttime, s_mac, s_os_type, s_comment)
                                                        VALUES ( @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)", SnailLogTable(ref TB, SnailLog_Define.role_log_tablename));
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@p1", snaillog.aid);
                cmd.Parameters.AddWithValue("@p2", snaillog.n_id);
                cmd.Parameters.AddWithValue("@p3", snaillog.s_account);
                cmd.Parameters.AddWithValue("@p4", snaillog.s_role);
                cmd.Parameters.AddWithValue("@p5", snaillog.s_ip);
                cmd.Parameters.AddWithValue("@p6", DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                cmd.Parameters.AddWithValue("@p7", snaillog.d_logouttime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@p8", snaillog.s_mac);
                cmd.Parameters.AddWithValue("@p9", snaillog.s_os_type);
                cmd.Parameters.AddWithValue("@p10", snaillog.s_comment);
                TB.ExcuteSqlCommand(LogDB, ref cmd);
                cmd.Dispose();
            }

            SnailLog_update_role_additional_log(ref TB, AID);
        }

        public static void SnailLog_update_role_additional_log(ref TxnBlock TB, long AID, string LogDB = DataManager_Define.LogDB)
        {
            _snail_role_additional_log snaillog = new _snail_role_additional_log();
            snaillog.aid = AID;
            snaillog.s_role_name = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_role]);
            snaillog.s_uid = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_account]);
            snaillog.s_create_acc = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_create_acc]);
            //snaillog.n_vip_level
            if (string.IsNullOrEmpty(snaillog.s_create_acc) || string.IsNullOrEmpty(snaillog.s_uid))
            {
                Account userInfo = AccountManager.GetAccountData(ref TB, AID);
                snaillog.s_uid = userInfo.SNO.ToString();
                snaillog.s_create_acc = userInfo.UserID;
            }

            snaillog.n_role_num = CharacterManager.GetCharacterCount_FromDB(ref TB, AID);
            snaillog.n_role_level = CharacterManager.GetCharacterMaxLevel_FromDB(ref TB, AID);

            string s_vip_level = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_vip_level]);
            if (string.IsNullOrEmpty(s_vip_level))
                snaillog.n_vip_level = VipManager.GetUser_VIPInfo(ref TB, AID).viplevel;
            else
            {
                int n_vip_level = 0;
                snaillog.n_vip_level = int.TryParse(s_vip_level, out n_vip_level) ? n_vip_level : 0;
            }

            int n_total_sec = 0;
            snaillog.n_total_sec = (int.TryParse(TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.update_role_log]), out n_total_sec) ? n_total_sec : 0) * 60;
            snaillog.n_role_effectiveness = AccountManager.GetUserWarPoint(ref TB, AID).WAR_POINT;

            string query = string.Format(@"MERGE {0} USING (select 'X' as DUAL) AS B
                                            ON aid = @p1
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                        s_uid = @p3,
                                                        s_create_acc = @p4,
                                                        n_total_sec = n_total_sec + @p8,
                                                        d_save = GETDATE(),
                                                        d_insert = GETDATE(),
                                                        n_role_num = @p11,
                                                        n_role_level = @p12,
                                                        n_vip_level = @p13,
                                                        n_role_effectiveness = @p14
                                                WHEN NOT MATCHED THEN
                                                   INSERT (
                                                        aid, s_role_name, s_uid, s_create_acc, d_create,
                                                        d_delete, n_deleted, n_total_sec, d_save, d_insert,
                                                        n_role_num, n_role_level, n_vip_level, n_role_effectiveness
                                                        ) 
                                                    VALUES ( @p1, @p2, @p3, @p4, GETDATE()
                                                            , NULL, @p7, @p8, GETDATE(), GETDATE()
                                                            , @p11, @p12, @p13, @p14
                                                            );
                                                        ", SnailLogTable(ref TB, SnailLog_Define.role_additional_log_tablename));
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@p1", snaillog.aid);
            cmd.Parameters.AddWithValue("@p2", snaillog.s_role_name);
            cmd.Parameters.AddWithValue("@p3", snaillog.s_uid);
            cmd.Parameters.AddWithValue("@p4", snaillog.s_create_acc);
            //cmd.Parameters.AddWithValue("@p5", snaillog.d_create);        // GETDATE()
            //cmd.Parameters.AddWithValue("@p6", snaillog.d_delete);        // NULL
            snaillog.n_deleted = 0;
            cmd.Parameters.AddWithValue("@p7", snaillog.n_deleted);       // Always 0
            cmd.Parameters.AddWithValue("@p8", snaillog.n_total_sec);
            //cmd.Parameters.AddWithValue("@p9", snaillog.d_save);          // GETDATE()
            //cmd.Parameters.AddWithValue("@p10", snaillog.d_insert);       // GETDATE()
            cmd.Parameters.AddWithValue("@p11", snaillog.n_role_num);
            cmd.Parameters.AddWithValue("@p12", snaillog.n_role_level);
            cmd.Parameters.AddWithValue("@p13", snaillog.n_vip_level);
            cmd.Parameters.AddWithValue("@p14", snaillog.n_role_effectiveness);
            TB.ExcuteSqlCommand(LogDB, ref cmd);
            cmd.Dispose();

        }


        public static void SnailLog_write_task_log(ref TxnBlock TB, long AID, ref List<TaskLogInfo> setLogList, string LogDB = DataManager_Define.LogDB)
        {
            string s_account = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_account]);
            if (string.IsNullOrEmpty(s_account))
                s_account = AccountManager.GetAccountData(ref TB, AID).SNO.ToString();

            string s_role_name = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_role]);

            foreach (TaskLogInfo logInfo in setLogList)
            {
                _snail_task_log snaillog = new _snail_task_log();
                snaillog.aid = AID;
                snaillog.s_account = s_account;
                snaillog.s_role_name = s_role_name;
                snaillog.s_task_id = logInfo.taskid.ToString();
                snaillog.n_act_type = logInfo.act_type;
                
                string query = string.Format(@"INSERT INTO {0} (aid, s_account, s_role_name, s_task_id, n_act_type, d_create)
                                                        VALUES ( @p1, @p2, @p3, @p4, @p5, @p6)", SnailLogTable(ref TB, SnailLog_Define.task_log_tablename));
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@p1", snaillog.aid);
                cmd.Parameters.AddWithValue("@p2", snaillog.s_account);
                cmd.Parameters.AddWithValue("@p3", snaillog.s_role_name);
                cmd.Parameters.AddWithValue("@p4", snaillog.s_task_id);
                cmd.Parameters.AddWithValue("@p5", snaillog.n_act_type);
                cmd.Parameters.AddWithValue("@p6", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                TB.ExcuteSqlCommand(LogDB, ref cmd);

                cmd.Dispose();
            }
        }


        public static void SnailLog_write_money_log(ref TxnBlock TB, long AID, ref List<MoneyLogInfo> setLogList, string LogDB = DataManager_Define.LogDB)
        {
            string s_account = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_account]);
            if (string.IsNullOrEmpty(s_account))
                s_account = AccountManager.GetAccountData(ref TB, AID).SNO.ToString();

            string s_role_name = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_role]);
            string s_guild_id = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_guild_id]);
            string s_event_id = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id]);
            string s_item_id = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_item_id]);
            string s_scene_id = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_scene_id]);

            int n_count = 0;
            string t_count = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_count]);
            if (!string.IsNullOrEmpty(t_count))
                n_count = System.Convert.ToInt32(t_count);

            string n_role_level = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_role_level]);
            if (string.IsNullOrEmpty(n_role_level))
                n_role_level = CharacterManager.GetCharacterMaxLevel_FromDB(ref TB, AID).ToString();

            foreach (MoneyLogInfo logInfo in setLogList)
            {
                _snail_money_log snaillog = new _snail_money_log();
                snaillog.aid = AID;
                snaillog.s_buy_account = s_account;
                snaillog.s_buy_role = s_role_name;
                snaillog.d_create = DateTime.Parse(logInfo.d_create);
                snaillog.n_money_type = logInfo.money_type;
                snaillog.s_event_id = s_event_id;
                snaillog.n_event_type = logInfo.event_type;
                snaillog.s_item_id = s_item_id;
                snaillog.s_instance_id = string.Empty;  // what mean!?, item_base id?
                snaillog.n_count = n_count;
                snaillog.n_money = logInfo.n_money;
                snaillog.s_comment = snaillog.s_recv_role = snaillog.s_recv_account = string.Empty;
                snaillog.s_recv_role = s_event_id;
                snaillog.s_scene_id = s_scene_id;
                snaillog.s_guild_id = s_guild_id;
                snaillog.s_state = "1";
                snaillog.n_role_level = string.IsNullOrEmpty(n_role_level) ? 0 : System.Convert.ToInt32(n_role_level);
                snaillog.n_total_logtime = 0;
                snaillog.n_before = logInfo.n_before;
                snaillog.n_after = logInfo.n_after;

                string query = string.Format(@"INSERT INTO {0} ( aid ,s_buy_account ,s_buy_role ,d_create ,n_money_type
                                                                ,s_event_id ,n_event_type ,s_item_id ,s_instance_id ,n_count
                                                                ,n_money ,s_recv_account ,s_recv_role ,s_scene_id ,s_guild_id
                                                                ,s_state ,s_comment ,n_role_level ,n_total_logtime ,n_before
                                                                ,n_after
                                                                )
                                                        VALUES ( @p1, @p2, @p3, @p4, @p5
                                                                , @p6, @p7, @p8, @p9, @p10
                                                                , @p11, @p12, @p13, @p14, @p15
                                                                , @p16, @p17, @p18, @p19, @p20
                                                                , @p21
                                                                )", SnailLogTable(ref TB, SnailLog_Define.money_log_tablename));
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@p1", snaillog.aid);
                cmd.Parameters.AddWithValue("@p2", snaillog.s_buy_account);
                cmd.Parameters.AddWithValue("@p3", snaillog.s_buy_role);
                cmd.Parameters.AddWithValue("@p4", snaillog.d_create.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@p5", snaillog.n_money_type);
                cmd.Parameters.AddWithValue("@p6", snaillog.s_event_id);
                cmd.Parameters.AddWithValue("@p7", snaillog.n_event_type);
                cmd.Parameters.AddWithValue("@p8", snaillog.s_item_id);
                cmd.Parameters.AddWithValue("@p9", snaillog.s_instance_id);
                cmd.Parameters.AddWithValue("@p10", snaillog.n_count);
                cmd.Parameters.AddWithValue("@p11", snaillog.n_money);
                cmd.Parameters.AddWithValue("@p12", snaillog.s_recv_account);
                cmd.Parameters.AddWithValue("@p13", snaillog.s_recv_role);
                cmd.Parameters.AddWithValue("@p14", snaillog.s_scene_id);
                cmd.Parameters.AddWithValue("@p15", snaillog.s_guild_id);
                cmd.Parameters.AddWithValue("@p16", snaillog.s_state);
                cmd.Parameters.AddWithValue("@p17", snaillog.s_comment);
                cmd.Parameters.AddWithValue("@p18", snaillog.n_role_level);
                cmd.Parameters.AddWithValue("@p19", snaillog.n_total_logtime);
                cmd.Parameters.AddWithValue("@p20", snaillog.n_before);
                cmd.Parameters.AddWithValue("@p21", snaillog.n_after);

                TB.ExcuteSqlCommand(LogDB, ref cmd);

                cmd.Dispose();
            }
        }

        public static void SnailLog_write_item_log(ref TxnBlock TB, long AID, ref List<ItemLogInfo> setLogList, string LogDB = DataManager_Define.LogDB)
        {
            string s_account = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_account]);
            if (string.IsNullOrEmpty(s_account))
                s_account = AccountManager.GetAccountData(ref TB, AID).SNO.ToString();

            string s_role_name = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_role]);
            string s_guild_id = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_guild_id]);
            string s_event_id = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id]);
            string s_item_id = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_item_id]);
            string s_scene_id = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_scene_id]);

            int n_count = 0;
            string t_count = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_count]);
            if (!string.IsNullOrEmpty(t_count))
                n_count = System.Convert.ToInt32(t_count);

            foreach (ItemLogInfo logInfo in setLogList)
            {
                _snail_item_log snaillog = new _snail_item_log();
                snaillog.aid = AID;
                snaillog.s_account = s_account;
                snaillog.s_role_name = s_role_name;
                snaillog.d_create = DateTime.Parse(logInfo.d_create);
                snaillog.s_event_id = s_event_id;
                snaillog.n_event_type = logInfo.event_type;
                snaillog.s_item_id = logInfo.item_id.ToString();
                snaillog.n_count = logInfo.n_count;
                snaillog.s_scene_id = s_scene_id;
                snaillog.s_guild_id = s_guild_id;
                snaillog.s_state = "1";
                snaillog.s_comment = string.Empty;
                snaillog.n_before = logInfo.n_before;
                snaillog.n_after = logInfo.n_after;

                string query = string.Format(@"INSERT INTO {0} ( aid, s_account ,s_role_name ,d_create ,s_event_id
                                                                ,n_event_type ,s_item_id ,n_count ,s_scene_id ,s_guild_id
                                                                ,s_state ,s_comment ,n_before ,n_after
                                                                )
                                                        VALUES ( @p1, @p2, @p3, @p4, @p5
                                                                , @p6, @p7, @p8, @p9, @p10
                                                                , @p11, @p12, @p13, @p14
                                                                )", SnailLogTable(ref TB, SnailLog_Define.item_log_tablename));
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@p1", snaillog.aid);
                cmd.Parameters.AddWithValue("@p2", snaillog.s_account);
                cmd.Parameters.AddWithValue("@p3", snaillog.s_role_name);
                cmd.Parameters.AddWithValue("@p4", snaillog.d_create.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@p5", snaillog.s_event_id);
                cmd.Parameters.AddWithValue("@p6", snaillog.n_event_type);
                cmd.Parameters.AddWithValue("@p7", snaillog.s_item_id);
                cmd.Parameters.AddWithValue("@p8", snaillog.n_count);
                cmd.Parameters.AddWithValue("@p9", snaillog.s_scene_id);
                cmd.Parameters.AddWithValue("@p10", snaillog.s_guild_id);
                cmd.Parameters.AddWithValue("@p11", snaillog.s_state);
                cmd.Parameters.AddWithValue("@p12", snaillog.s_comment);
                cmd.Parameters.AddWithValue("@p13", snaillog.n_before);
                cmd.Parameters.AddWithValue("@p14", snaillog.n_after);

                TB.ExcuteSqlCommand(LogDB, ref cmd);

                cmd.Dispose();
            }
        }


        public static void MseedLog_mSeed_item_log(ref TxnBlock TB, long AID, ref List<ItemLogInfo> setLogList, string LogDB = DataManager_Define.LogDB)
        {
            foreach (ItemLogInfo logInfo in setLogList)
            {
                int n_count = logInfo.n_before - logInfo.n_after;
                if (n_count > 0)
                {
                    mseed_item_log mseedlog = new mseed_item_log();
                    mseedlog.aid = AID;
                    mseedlog.class_type = logInfo.class_type;
                    mseedlog.inven_seq = logInfo.inven_seq;
                    mseedlog.item_id = logInfo.item_id;
                    mseedlog.grade = logInfo.grade;
                    mseedlog.level = logInfo.level;
                    mseedlog.equipposition = logInfo.equipposition;
                    mseedlog.deleted_ea = n_count;

                    string query = string.Format(@"INSERT INTO {0} ( aid, class_type ,inven_seq ,item_id ,grade
                                                                ,level ,equipposition ,deleted_ea, status, reg_date
                                                                )
                                                        VALUES ( @p1, @p2, @p3, @p4, @p5
                                                                , @p6, @p7, @p8, @p9, GETDATE()
                                                                )", SnailLogTable(ref TB, SnailLog_Define.mseed_item_log_tablename));
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@p1", mseedlog.aid);
                    cmd.Parameters.AddWithValue("@p2", mseedlog.class_type);
                    cmd.Parameters.AddWithValue("@p3", mseedlog.inven_seq);
                    cmd.Parameters.AddWithValue("@p4", mseedlog.item_id);
                    cmd.Parameters.AddWithValue("@p5", mseedlog.grade);
                    cmd.Parameters.AddWithValue("@p6", mseedlog.level);
                    cmd.Parameters.AddWithValue("@p7", mseedlog.equipposition);
                    cmd.Parameters.AddWithValue("@p8", mseedlog.deleted_ea);
                    cmd.Parameters.AddWithValue("@p9", 0);
                    TB.ExcuteSqlCommand(LogDB, ref cmd);
                    cmd.Dispose();
                }
            }
        }

        public static void SnailLog_write_role_upgrade_log(ref TxnBlock TB, long AID, string LogDB = DataManager_Define.LogDB)
        {
            _snail_role_upgrade_log snaillog = new _snail_role_upgrade_log();
            snaillog.aid = AID;
            snaillog.d_time = DateTime.Now;
            snaillog.s_account = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_account]);
            if (string.IsNullOrEmpty(snaillog.s_account))
                snaillog.s_account = AccountManager.GetAccountData(ref TB, AID).SNO.ToString();

            snaillog.s_role = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_role]);
            snaillog.n_logtype = System.Convert.ToInt32(TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_logtype]));
            snaillog.n_level_before = System.Convert.ToInt32(TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_level_before]));
            snaillog.n_level_after = System.Convert.ToInt32(TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_level_after]));
            snaillog.s_skill_id = string.Empty;

            string query = string.Format(@"INSERT INTO {0} (aid, n_id, d_time, s_account, s_role,
                                                            n_logtype, n_level_before, n_level_after, s_skill_id)
                                                        VALUES ( @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9)", SnailLogTable(ref TB, SnailLog_Define.role_upgrade_log_tablename));
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@p1", snaillog.aid);
            cmd.Parameters.AddWithValue("@p2", snaillog.n_id);
            cmd.Parameters.AddWithValue("@p3", snaillog.d_time.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@p4", snaillog.s_account);
            cmd.Parameters.AddWithValue("@p5", snaillog.s_role);
            cmd.Parameters.AddWithValue("@p6", snaillog.n_logtype);
            cmd.Parameters.AddWithValue("@p7", snaillog.n_level_before);
            cmd.Parameters.AddWithValue("@p8", snaillog.n_level_after);
            cmd.Parameters.AddWithValue("@p9", snaillog.s_skill_id);

            TB.ExcuteSqlCommand(LogDB, ref cmd);

            cmd.Dispose();
        }


        public static void SnailLog_write_instance_log(ref TxnBlock TB, long AID, string LogDB = DataManager_Define.LogDB)
        {
            _snail_instance_log snaillog = new _snail_instance_log();
            snaillog.aid = AID;
            snaillog.s_instancetypeid = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_scene_id]);
            snaillog.s_instanceid = (System.Convert.ToInt64(snaillog.s_instancetypeid) % 10000).ToString();
            string t_duration = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_duration]);
            if (string.IsNullOrEmpty(t_duration))
                snaillog.n_duration = 0;
            else
                snaillog.n_duration = System.Convert.ToInt32(t_duration);
            snaillog.d_endtime = DateTime.Now;
            snaillog.d_starttime = snaillog.d_endtime.AddSeconds(snaillog.n_duration * -1);
            snaillog.n_troop_count1 = snaillog.n_troop_count2 = 1;

            string query = string.Format(@"INSERT INTO {0} (aid, n_id, s_instanceid, s_instancetypeid, d_starttime
                                                            ,d_endtime ,n_duration ,n_troop_count1 ,n_troop_count2)
                                                        VALUES ( @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9)", SnailLogTable(ref TB, SnailLog_Define.instance_log_tablename));
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@p1", snaillog.aid);
            cmd.Parameters.AddWithValue("@p2", snaillog.n_id);
            cmd.Parameters.AddWithValue("@p3", snaillog.s_instanceid);
            cmd.Parameters.AddWithValue("@p4", snaillog.s_instancetypeid);
            cmd.Parameters.AddWithValue("@p5", snaillog.d_starttime);
            cmd.Parameters.AddWithValue("@p6", snaillog.d_endtime);
            cmd.Parameters.AddWithValue("@p7", snaillog.n_duration);
            cmd.Parameters.AddWithValue("@p8", snaillog.n_troop_count1);
            cmd.Parameters.AddWithValue("@p9", snaillog.n_troop_count2);

            TB.ExcuteSqlCommand(LogDB, ref cmd);

            cmd.Dispose();
        }
        
        public static void SnailLog_write_scene_log(ref TxnBlock TB, long AID, string LogDB = DataManager_Define.LogDB)
        {
            _snail_scene_log snaillog = new _snail_scene_log();
            snaillog.aid = AID;
            snaillog.n_id = 0;
            snaillog.s_sceneid = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_scene_id]);
            snaillog.d_date = DateTime.Now;
            snaillog.n_enter = 1;
            string t_duration = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_duration]); 
            if (string.IsNullOrEmpty(t_duration))
                snaillog.n_totaltime = 0;
            else
                snaillog.n_totaltime = System.Convert.ToInt32(t_duration);

            string query = string.Format(@"MERGE {0} USING (select 'X' as DUAL) AS B
                                            ON aid = @p1 AND s_sceneid = @p3 AND d_date = @p4
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                        n_enter = n_enter + @p5,
                                                        n_totaltime = n_totaltime + @p6
                                                WHEN NOT MATCHED THEN
                                                   INSERT (aid, n_id, s_sceneid, d_date, n_enter, n_totaltime) 
                                                    VALUES ( @p1, @p2, @p3, @p4, @p5, @p6);
                                                        ", SnailLogTable(ref TB, SnailLog_Define.scene_log_tablename));
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@p1", snaillog.aid);
            cmd.Parameters.AddWithValue("@p2", snaillog.n_id);
            cmd.Parameters.AddWithValue("@p3", snaillog.s_sceneid);
            cmd.Parameters.AddWithValue("@p4", snaillog.d_date.ToShortDateString());
            cmd.Parameters.AddWithValue("@p5", snaillog.n_enter);
            cmd.Parameters.AddWithValue("@p6", snaillog.n_totaltime);

            TB.ExcuteSqlCommand(LogDB, ref cmd);

            cmd.Dispose();
        }

        public static void SnailLog_write_program_log(ref TxnBlock TB, string GM, ref System_Event setLog, string LogDB = DataManager_Define.LogDB)
        {

            _snail_program_log snaillog = new _snail_program_log();
            snaillog.gm_id = GM;
            snaillog.s_programid = setLog.Event_ID.ToString();
            snaillog.s_programname = setLog.Event_Dev_Name;
            snaillog.d_opentime = setLog.Event_StartTime;
            snaillog.d_endtime = setLog.Event_EndTime;
            string query = string.Format(@"INSERT INTO {0} (gm_id, s_programid, s_programname, d_opentime, d_endtime)
                                                Values ( @p1, @p2, @p3, @p4, @p5);", SnailLogTable(ref TB, SnailLog_Define.program_log_tablename));
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@p1", snaillog.gm_id);
            cmd.Parameters.AddWithValue("@p2", snaillog.s_programid);
            cmd.Parameters.AddWithValue("@p3", snaillog.s_programname);
            cmd.Parameters.AddWithValue("@p4", snaillog.d_opentime.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@p5", snaillog.d_endtime.ToString("yyyy-MM-dd HH:mm:ss"));

            TB.ExcuteSqlCommand(LogDB, ref cmd);

            cmd.Dispose();
        }

        public static long GetSnailLog_program_log(ref TxnBlock TB, string LogDB = DataManager_Define.LogDB)
        {
            string query = string.Format(@"Select Count(*) as count From {0} WITH(NOLOCK)", SnailLogTable(ref TB, SnailLog_Define.program_log_tablename));
            SnailLogCount retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<SnailLogCount>(ref TB, query, LogDB);
            if (retObj == null)
                retObj = new SnailLogCount();
            return retObj.count;
        }
    }
}
