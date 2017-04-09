using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using mSeed.mDBTxnBlock;
using mSeed.RedisManager;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Global;
using TheSoulWebServer.Tools;
using TheSoulCheatServer.lib;
using System.Net.Json;

namespace TheSoulCheatServer
{
    public partial class cheat_openTime : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam();
            string retJson = "";
            queryFetcher.SetDebugMode = true;
            string bossraid = queryFetcher.QueryParam_Fetch("bossraid", "");
            string matching = queryFetcher.QueryParam_Fetch("matching", "");
            string pvp_start_hour1 = queryFetcher.QueryParam_Fetch("pvp_start_hour1", "");
            string pvp_start_min1 = queryFetcher.QueryParam_Fetch("pvp_start_min1", "");
            string pvp_start_hour2 = queryFetcher.QueryParam_Fetch("pvp_start_hour2", "");
            string pvp_start_min2 = queryFetcher.QueryParam_Fetch("pvp_start_min2", "");
            string rubyPVP_start_hour1 = queryFetcher.QueryParam_Fetch("rubyPVP_start_hour1", "");
            string rubyPVP_start_min1 = queryFetcher.QueryParam_Fetch("rubyPVP_start_min1", "");
            string rubyPVP_start_hour2 = queryFetcher.QueryParam_Fetch("rubyPVP_start_hour2", "");
            string rubyPVP_start_min2 = queryFetcher.QueryParam_Fetch("rubyPVP_start_min2", "");
            string PVP1vs1_start_hour1 = queryFetcher.QueryParam_Fetch("1vs1PVP_start_hour1", "");
            string PVP1vs1_start_min1 = queryFetcher.QueryParam_Fetch("1vs1PVP_start_min1", "");
            string PVP1vs1_start_hour2 = queryFetcher.QueryParam_Fetch("1vs1PVP_start_hour2", "");
            string PVP1vs1_start_min2 = queryFetcher.QueryParam_Fetch("1vs1PVP_start_min2", "");
            string pvp_end_hour1 = queryFetcher.QueryParam_Fetch("pvp_end_hour1", "");
            string pvp_end_min1 = queryFetcher.QueryParam_Fetch("pvp_end_min1", "");
            string pvp_end_hour2 = queryFetcher.QueryParam_Fetch("pvp_end_hour2", "");
            string pvp_end_min2 = queryFetcher.QueryParam_Fetch("pvp_end_min2", "");
            string rubyPVP_end_hour1 = queryFetcher.QueryParam_Fetch("rubyPVP_end_hour1", "");
            string rubyPVP_end_min1 = queryFetcher.QueryParam_Fetch("rubyPVP_end_min1", "");
            string rubyPVP_end_hour2 = queryFetcher.QueryParam_Fetch("rubyPVP_end_hour2", "");
            string rubyPVP_end_min2 = queryFetcher.QueryParam_Fetch("rubyPVP_end_min2", "");
            string PVP1vs1_end_hour1 = queryFetcher.QueryParam_Fetch("1vs1PVP_end_hour1", "");
            string PVP1vs1_end_min1 = queryFetcher.QueryParam_Fetch("1vs1PVP_end_min1", "");
            string PVP1vs1_end_hour2 = queryFetcher.QueryParam_Fetch("1vs1PVP_end_hour2", "");
            string PVP1vs1_end_min2 = queryFetcher.QueryParam_Fetch("1vs1PVP_end_min2", "");
            string guild_start_hour1 = queryFetcher.QueryParam_Fetch("guild_start_hour1", "");
            string guild_start_min1 = queryFetcher.QueryParam_Fetch("guild_start_min1", "");
            string guild_start_hour2 = queryFetcher.QueryParam_Fetch("guild_start_hour2", "");
            string guild_start_min2 = queryFetcher.QueryParam_Fetch("guild_start_min2", "");
            string guild_end_hour1 = queryFetcher.QueryParam_Fetch("guild_end_hour1", "");
            string guild_end_min1 = queryFetcher.QueryParam_Fetch("guild_end_min1", "");
            string guild_end_hour2 = queryFetcher.QueryParam_Fetch("guild_end_hour2", "");
            string guild_end_min2 = queryFetcher.QueryParam_Fetch("guild_end_min2", "");

            TxnBlock tb = new TxnBlock();
            {
                long AID = 0;
                try
                {
                    SystemData.GetConstValue(ref tb, "DEF_PVP_GLADIATOR_START_TIME_1st", Account_Define.AccountShardingDB, true);
                    

queryFetcher.TxnBlockInit(ref tb, ref AID);
                    queryFetcher.GlobalDBOpen(ref tb);

                    Result_Define.eResult retErr = Result_Define.eResult.POST_DATA_ERROR;
                    bossRate.Text = SystemData.AdminConstValueFetchFromRedis(ref tb, "BOSSRAID_APPEAR_PROBABILITY").ToString();
                    
                    if (!string.IsNullOrEmpty(bossraid))
                    {
                        string setKey = "BOSSRAID_APPEAR_PROBABILITY";
                        int changeData = System.Convert.ToInt32(bossraid);
                        if (changeData < 101)
                        {
                            string setQuery = string.Format("Update System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                            retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                            setQuery = string.Format("Update Admin_System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                            retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                SystemData.GetConstValue(ref tb, setKey, Account_Define.AccountShardingDB, true);
                                SystemData.AdminConstValueFetchFromRedis(ref tb, setKey, SystemData_Define.AdminConstTableName, true);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(matching))
                    {
                        string setKey = "DEF_EXPEDITION_MATCHING_VALUE";
                        int changeData = System.Convert.ToInt32(matching);
                        string setQuery = string.Format("Update System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retErr == Result_Define.eResult.SUCCESS)
                        {
                            SystemData.GetConstValue(ref tb, setKey, Account_Define.AccountShardingDB, true);
                        }
                    }

                    if (!string.IsNullOrEmpty(pvp_start_hour1) && !string.IsNullOrEmpty(pvp_start_min1))
                    {
                        string setKey = "DEF_BATTLE_FREEFORALL_START_TIME_1st";
                        long changeData = (System.Convert.ToInt64(pvp_start_hour1)*3600) + (System.Convert.ToInt64(pvp_start_min1)*60);
                        string setQuery = string.Format("Update System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        setQuery = string.Format("Update Admin_System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retErr == Result_Define.eResult.SUCCESS)
                        {
                            SystemData.GetConstValue(ref tb, setKey, Account_Define.AccountShardingDB, true);
                            SystemData.AdminConstValueFetchFromRedis(ref tb, setKey, SystemData_Define.AdminConstTableName, true);
                        }
                    }
                    if (!string.IsNullOrEmpty(pvp_end_hour1) && !string.IsNullOrEmpty(pvp_end_min1))
                    {
                        string setKey = "DEF_BATTLE_FREEFORALL_END_TIME_1st";
                        long changeData = (System.Convert.ToInt64(pvp_end_hour1) * 3600) + (System.Convert.ToInt64(pvp_end_min1) * 60);
                        string setQuery = string.Format("Update System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        setQuery = string.Format("Update Admin_System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retErr == Result_Define.eResult.SUCCESS)
                        {
                            SystemData.GetConstValue(ref tb, setKey, Account_Define.AccountShardingDB, true);
                            SystemData.AdminConstValueFetchFromRedis(ref tb, setKey, SystemData_Define.AdminConstTableName, true);
                        }
                    }
                    if (!string.IsNullOrEmpty(pvp_start_hour2) && !string.IsNullOrEmpty(pvp_start_min2))
                    {
                        string setKey = "DEF_BATTLE_FREEFORALL_START_TIME_2nd";
                        long changeData = (System.Convert.ToInt64(pvp_start_hour2) * 3600) + (System.Convert.ToInt64(pvp_start_min2) * 60);
                        string setQuery = string.Format("Update System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        setQuery = string.Format("Update Admin_System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retErr == Result_Define.eResult.SUCCESS)
                        {
                            SystemData.GetConstValue(ref tb, setKey, Account_Define.AccountShardingDB, true);
                            SystemData.AdminConstValueFetchFromRedis(ref tb, setKey, SystemData_Define.AdminConstTableName, true);
                        }
                    }
                    if (!string.IsNullOrEmpty(pvp_end_hour2) && !string.IsNullOrEmpty(pvp_end_min2))
                    {
                        string setKey = "DEF_BATTLE_FREEFORALL_END_TIME_2nd";
                        long changeData = (System.Convert.ToInt64(pvp_end_hour2) * 3600) + (System.Convert.ToInt64(pvp_end_min2) * 60);
                        string setQuery = string.Format("Update System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        setQuery = string.Format("Update Admin_System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retErr == Result_Define.eResult.SUCCESS)
                        {
                            SystemData.GetConstValue(ref tb, setKey, Account_Define.AccountShardingDB, true);
                            SystemData.AdminConstValueFetchFromRedis(ref tb, setKey, SystemData_Define.AdminConstTableName, true);
                        }
                    }
                    if (!string.IsNullOrEmpty(rubyPVP_start_hour1) && !string.IsNullOrEmpty(rubyPVP_start_min1))
                    {
                        string setKey = "DEF_PVP_GLADIATOR_START_TIME_1st";
                        long changeData = (System.Convert.ToInt64(rubyPVP_start_hour1) * 3600) + (System.Convert.ToInt64(rubyPVP_start_min1) * 60);
                        string setQuery = string.Format("Update System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        setQuery = string.Format("Update Admin_System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retErr == Result_Define.eResult.SUCCESS)
                        {
                            SystemData.GetConstValue(ref tb, setKey, Account_Define.AccountShardingDB, true);
                            SystemData.AdminConstValueFetchFromRedis(ref tb, setKey, SystemData_Define.AdminConstTableName, true);
                        }
                    }
                    if (!string.IsNullOrEmpty(rubyPVP_end_hour1) && !string.IsNullOrEmpty(rubyPVP_end_min1))
                    {
                        string setKey = "DEF_PVP_GLADIATOR_END_TIME_1st";
                        long changeData = (System.Convert.ToInt64(rubyPVP_end_hour1) * 3600) + (System.Convert.ToInt64(rubyPVP_end_min1) * 60);
                        string setQuery = string.Format("Update System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        setQuery = string.Format("Update Admin_System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retErr == Result_Define.eResult.SUCCESS)
                        {
                            SystemData.GetConstValue(ref tb, setKey, Account_Define.AccountShardingDB, true);
                            SystemData.AdminConstValueFetchFromRedis(ref tb, setKey, SystemData_Define.AdminConstTableName, true);
                        }
                    }
                    if (!string.IsNullOrEmpty(rubyPVP_start_hour2) && !string.IsNullOrEmpty(rubyPVP_start_min2))
                    {
                        string setKey = "DEF_PVP_GLADIATOR_START_TIME_2nd";
                        long changeData = (System.Convert.ToInt64(rubyPVP_start_hour2) * 3600) + (System.Convert.ToInt64(rubyPVP_start_min2) * 60);
                        string setQuery = string.Format("Update System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        setQuery = string.Format("Update Admin_System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retErr == Result_Define.eResult.SUCCESS)
                        {
                            SystemData.GetConstValue(ref tb, setKey, Account_Define.AccountShardingDB, true);
                            SystemData.AdminConstValueFetchFromRedis(ref tb, setKey, SystemData_Define.AdminConstTableName, true);
                        }
                    }
                    if (!string.IsNullOrEmpty(rubyPVP_end_hour2) && !string.IsNullOrEmpty(rubyPVP_end_min2))
                    {
                        string setKey = "DEF_PVP_GLADIATOR_END_TIME_2nd";
                        long changeData = (System.Convert.ToInt64(rubyPVP_end_hour2) * 3600) + (System.Convert.ToInt64(rubyPVP_end_min2) * 60);
                        string setQuery = string.Format("Update System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        setQuery = string.Format("Update Admin_System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retErr == Result_Define.eResult.SUCCESS)
                        {
                            SystemData.GetConstValue(ref tb, setKey, Account_Define.AccountShardingDB, true);
                            SystemData.AdminConstValueFetchFromRedis(ref tb, setKey, SystemData_Define.AdminConstTableName, true);
                        }
                    }
                    if (!string.IsNullOrEmpty(PVP1vs1_start_hour1) && !string.IsNullOrEmpty(PVP1vs1_start_min1))
                    {
                        string setKey = "DEF_1VS1REAL_START_TIME";
                        long changeData = (System.Convert.ToInt64(PVP1vs1_start_hour1) * 3600) + (System.Convert.ToInt64(PVP1vs1_start_hour1) * 60);
                        string setQuery = string.Format("Update System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        setQuery = string.Format("Update Admin_System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retErr == Result_Define.eResult.SUCCESS)
                        {
                            SystemData.GetConstValue(ref tb, setKey, Account_Define.AccountShardingDB, true);
                            SystemData.AdminConstValueFetchFromRedis(ref tb, setKey, SystemData_Define.AdminConstTableName, true);
                        }
                    }
                    if (!string.IsNullOrEmpty(PVP1vs1_end_hour1) && !string.IsNullOrEmpty(PVP1vs1_end_min1))
                    {
                        string setKey = "DEF_1VS1REAL_END_TIME";
                        long changeData = (System.Convert.ToInt64(PVP1vs1_end_hour1) * 3600) + (System.Convert.ToInt64(PVP1vs1_end_hour1) * 60);
                        string setQuery = string.Format("Update System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        setQuery = string.Format("Update Admin_System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retErr == Result_Define.eResult.SUCCESS)
                        {
                            SystemData.GetConstValue(ref tb, setKey, Account_Define.AccountShardingDB, true);
                            SystemData.AdminConstValueFetchFromRedis(ref tb, setKey, SystemData_Define.AdminConstTableName, true);
                        }
                    }
                    if (!string.IsNullOrEmpty(PVP1vs1_start_hour2) && !string.IsNullOrEmpty(PVP1vs1_start_min2))
                    {
                        string setKey = "DEF_1VS1REAL_START_TIME_BONUS";
                        long changeData = (System.Convert.ToInt64(PVP1vs1_start_hour2) * 3600) + (System.Convert.ToInt64(PVP1vs1_start_hour2) * 60);
                        string setQuery = string.Format("Update System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        setQuery = string.Format("Update Admin_System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retErr == Result_Define.eResult.SUCCESS)
                        {
                            SystemData.GetConstValue(ref tb, setKey, Account_Define.AccountShardingDB, true);
                            SystemData.AdminConstValueFetchFromRedis(ref tb, setKey, SystemData_Define.AdminConstTableName, true);
                        }
                    }
                    if (!string.IsNullOrEmpty(PVP1vs1_end_hour2) && !string.IsNullOrEmpty(PVP1vs1_end_min2))
                    {
                        string setKey = "DEF_1VS1REAL_END_TIME_BOUNS";
                        long changeData = (System.Convert.ToInt64(PVP1vs1_end_hour2) * 3600) + (System.Convert.ToInt64(PVP1vs1_end_min2) * 60);
                        string setQuery = string.Format("Update System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        setQuery = string.Format("Update Admin_System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retErr == Result_Define.eResult.SUCCESS)
                        {
                            SystemData.GetConstValue(ref tb, setKey, Account_Define.AccountShardingDB, true);
                            SystemData.AdminConstValueFetchFromRedis(ref tb, setKey, SystemData_Define.AdminConstTableName, true);
                        }
                    }
                    if (!string.IsNullOrEmpty(guild_start_hour1) && !string.IsNullOrEmpty(guild_start_min1))
                    {
                        string setKey = "DEF_BATTLE_GUILD_G3VS3_START_TIME_1st";
                        long changeData = (System.Convert.ToInt64(guild_start_hour1) * 3600) + (System.Convert.ToInt64(guild_start_min1) * 60);
                        string setQuery = string.Format("Update System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        setQuery = string.Format("Update Admin_System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retErr == Result_Define.eResult.SUCCESS)
                        {
                            SystemData.GetConstValue(ref tb, setKey, Account_Define.AccountShardingDB, true);
                            SystemData.AdminConstValueFetchFromRedis(ref tb, setKey, SystemData_Define.AdminConstTableName, true);
                        }
                    }
                    if (!string.IsNullOrEmpty(guild_end_hour1) && !string.IsNullOrEmpty(guild_end_min1))
                    {
                        string setKey = "DEF_BATTLE_GUILD_G3VS3_END_TIME_1st";
                        long changeData = (System.Convert.ToInt64(guild_end_hour1) * 3600) + (System.Convert.ToInt64(guild_end_min1) * 60);
                        string setQuery = string.Format("Update System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        setQuery = string.Format("Update Admin_System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retErr == Result_Define.eResult.SUCCESS)
                        {
                            SystemData.GetConstValue(ref tb, setKey, Account_Define.AccountShardingDB, true);
                            SystemData.AdminConstValueFetchFromRedis(ref tb, setKey, SystemData_Define.AdminConstTableName, true);
                        }
                    }
                    if (!string.IsNullOrEmpty(guild_start_hour2) && !string.IsNullOrEmpty(guild_start_min2))
                    {
                        string setKey = "DEF_BATTLE_GUILD_G3VS3_START_TIME_2nd";
                        long changeData = (System.Convert.ToInt64(guild_start_hour2) * 3600) + (System.Convert.ToInt64(guild_start_min2) * 60);
                        string setQuery = string.Format("Update System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        setQuery = string.Format("Update Admin_System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retErr == Result_Define.eResult.SUCCESS)
                        {
                            SystemData.GetConstValue(ref tb, setKey, Account_Define.AccountShardingDB, true);
                            SystemData.AdminConstValueFetchFromRedis(ref tb, setKey, SystemData_Define.AdminConstTableName, true);
                        }
                    }
                    if (!string.IsNullOrEmpty(guild_end_hour2) && !string.IsNullOrEmpty(guild_end_min2))
                    {
                        string setKey = "DEF_BATTLE_GUILD_G3VS3_END_TIME_2nd";
                        long changeData = (System.Convert.ToInt64(guild_end_hour2) * 3600) + (System.Convert.ToInt64(guild_end_min2) * 60);
                        string setQuery = string.Format("Update System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        setQuery = string.Format("Update Admin_System_Const Set value={0} Where ConstDefine='{1}'", changeData, setKey);
                        retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retErr == Result_Define.eResult.SUCCESS)
                        {
                            SystemData.GetConstValue(ref tb, setKey, Account_Define.AccountShardingDB, true);
                            SystemData.AdminConstValueFetchFromRedis(ref tb, setKey, SystemData_Define.AdminConstTableName, true);
                        }
                    }

                    retJson = queryFetcher.Render("", retErr);

                }
                catch (Exception errorEx)
                {
                    retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(errorEx.Message), Result_Define.eResult.System_Exception);
                }
                tb.EndTransaction(queryFetcher.Render_errorFlag);
                tb.Dispose();
            }

        }
    }
}