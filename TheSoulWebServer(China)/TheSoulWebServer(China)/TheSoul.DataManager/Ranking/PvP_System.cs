using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;

namespace TheSoul.DataManager
{
    public partial class PvPManager
    {
        public static List<RetBattleReward> GetSystem_Battle_Reward_List(ref TxnBlock TB, string dbkey = PvP_Define.PvP_Info_DB, bool Flush = false)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)", PvP_Define.System_Battle_Reward_TableName);

            List<System_Battle_Reward> retList = GenericFetch.FetchFromDB_MultipleRow<System_Battle_Reward>(ref TB, setQuery, dbkey);
            List<RetBattleReward> retReward = new List<RetBattleReward>();

            retList.ForEach(setItem =>
            {
                retReward.Add(new RetBattleReward(setItem));
            }
            );

            return retReward;
        }

        public static List<PVP_Record> GetUser_PvP_Reward_List(ref TxnBlock TB, long AID)
        {
            List<PVP_Record> retList = new List<PVP_Record>();
            foreach(KeyValuePair<PvP_Define.ePvPType, List<PvP_Define.ePvPRewardRepeatType>> setRepaetList in PvP_Define.PvPReward_CheckList)
            {
                foreach (PvP_Define.ePvPRewardRepeatType checkType in setRepaetList.Value)
                {
                    int setSeperater = 0;                    
                    switch(checkType)
                    {
                        case PvP_Define.ePvPRewardRepeatType.Daily: setSeperater = PvPManager.GetSeperater_Day(ref TB); break;
                        case PvP_Define.ePvPRewardRepeatType.Monthly: setSeperater = PvPManager.GetSeperater_Month(ref TB); break;
                        case PvP_Define.ePvPRewardRepeatType.Weekly: setSeperater = PvPManager.GetSeperater_Week(ref TB); break;
                        case PvP_Define.ePvPRewardRepeatType.Season: setSeperater = PvPManager.GetSeperater_Season(ref TB); break;                            
                    }

                    if (setSeperater > 0)
                    {
                        string setDBTable = setRepaetList.Key == PvP_Define.ePvPType.MATCH_GUILD_3VS3 ?
                                            (checkType == PvP_Define.ePvPRewardRepeatType.Daily ? PvP_Define.PvP_GuildUser_Daily_TableName :
                                            checkType == PvP_Define.ePvPRewardRepeatType.Weekly ? PvP_Define.PvP_GuildUser_Weekly_TableName :
                                            checkType == PvP_Define.ePvPRewardRepeatType.Monthly ? PvP_Define.PvP_GuildUser_Monthly_TableName : string.Empty) :
                                            (checkType == PvP_Define.ePvPRewardRepeatType.Daily ? PvP_Define.PvP_PvP_Daily_TableName :
                                            checkType == PvP_Define.ePvPRewardRepeatType.Weekly ? PvP_Define.PvP_PvP_Weekly_TableName :
                                            checkType == PvP_Define.ePvPRewardRepeatType.Monthly ? PvP_Define.PvP_PvP_Monthly_TableName :
                                            checkType == PvP_Define.ePvPRewardRepeatType.Season ? PvP_Define.PvP_PvP_Season_TableName : string.Empty);
                        int CheckSeperater = checkType == PvP_Define.ePvPRewardRepeatType.Daily ? (Mail_Define.Mail_Close_Min / 60 / 24) : 1;

                        if (setRepaetList.Key == PvP_Define.ePvPType.MATCH_GUILD_3VS3)
                        {
                        }
                        else
                        {
                            for (int i = 0; i < CheckSeperater; i++)
                            {
                                PVP_Record userPvPInfo = PvPManager.GetUser_PvP_Record(ref TB, AID, setSeperater - (i + 1), setRepaetList.Key, setDBTable);
                                if (userPvPInfo != null)
                                {
                                    if (userPvPInfo.dailycashgetyn.Equals("N"))
                                        retList.Add(userPvPInfo);
                                }
                            }
                        }
                    }
                }
            }

            foreach (PVP_Record setRewardItem in retList)
            {
                
            }

            return retList;
        }

        public static List<PVP_Record> GetUser_PvP_Record_All(ref TxnBlock TB, long AID, int seperaterWeekOrSeason = 0, PvP_Define.ePvPType PvPType = PvP_Define.ePvPType.MATCH_FREE, string dbkey = PvP_Define.PvP_Info_DB, bool Flush = false)
        {
            if (seperaterWeekOrSeason < 1)
                seperaterWeekOrSeason = PvPManager.GetSeperater(ref TB, PvPType);

            string setDBTable = PvPType == PvP_Define.ePvPType.MATCH_1VS1 ? PvP_Define.PvP_PvP_Season_TableName :
                                            PvPType == PvP_Define.ePvPType.MATCH_FREE ? PvP_Define.PvP_PvP_Weekly_TableName :
                                            PvPType == PvP_Define.ePvPType.MATCH_GUILD_3VS3 ? PvP_Define.PvP_PvP_Monthly_TableName : PvP_Define.PvP_PvP_Weekly_TableName;
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND seperater = {2}", setDBTable, AID, seperaterWeekOrSeason);
            List<PVP_Record> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<PVP_Record>(ref TB, setQuery, dbkey);

            return retObj;
        }

        public static void RemoveUser_PvP_Record(long AID, string setDBTable, int PvPType, int seperater)
        {
            string setKey = string.Format("{0}_{1}_{2}_{3}", setDBTable, AID, PvPType, seperater);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_Ranking, setKey);
        }
        
        public static PVP_Record GetUser_PvP_Record(ref TxnBlock TB, long AID, int seperaterWeekOrSeason = 0, PvP_Define.ePvPType PvPType = PvP_Define.ePvPType.MATCH_FREE, string setDBTable = "", bool Flush = false, string dbkey = PvP_Define.PvP_Info_DB)
        {
            if (string.IsNullOrEmpty(setDBTable))
            {
                setDBTable = PvPType == PvP_Define.ePvPType.MATCH_1VS1 ? PvP_Define.PvP_PvP_Season_TableName :
                                            PvPType == PvP_Define.ePvPType.MATCH_FREE ? PvP_Define.PvP_PvP_Weekly_TableName :
                                            PvPType == PvP_Define.ePvPType.MATCH_GUILD_3VS3 ? PvP_Define.PvP_PvP_Monthly_TableName : PvP_Define.PvP_PvP_Weekly_TableName;
            }
            else if (seperaterWeekOrSeason == 0) 
            {
                if (setDBTable == PvP_Define.PvP_PvP_Season_TableName)
                    seperaterWeekOrSeason = PvPManager.GetSeperater_Season(ref TB);
                else if (setDBTable == PvP_Define.PvP_PvP_Monthly_TableName)
                    seperaterWeekOrSeason = PvPManager.GetSeperater_Month(ref TB);
                else if (setDBTable == PvP_Define.PvP_PvP_Weekly_TableName)
                    seperaterWeekOrSeason = PvPManager.GetSeperater_Week(ref TB);
                else if (setDBTable == PvP_Define.PvP_PvP_Daily_TableName)
                    seperaterWeekOrSeason = PvPManager.GetSeperater_Day(ref TB);
            }

            if (seperaterWeekOrSeason < 1)
                seperaterWeekOrSeason = PvPManager.GetSeperater(ref TB, PvPType);

            string setKey = string.Format("{0}_{1}_{2}_{3}", setDBTable, AID, (int)PvPType, seperaterWeekOrSeason);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND pvp_type = {2} AND seperater = {3}", setDBTable, AID, (int)PvPType, seperaterWeekOrSeason);
            PVP_Record retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<PVP_Record>(ref TB, DataManager_Define.RedisServerAlias_Ranking, setKey, setQuery, dbkey, Flush);
            if (retObj == null)
            {
                retObj = new PVP_Record(AID, seperaterWeekOrSeason);
                Account_Simple userAccount = AccountManager.GetSimpleAccountInfo(ref TB, AID);
                retObj.user_nick = userAccount.username;
                retObj.pvp_type = (byte)PvPType;
            }

            if (retObj.pvp_type == (byte)PvP_Define.ePvPType.MATCH_1VS1
                && retObj.totalhonorpoint == 0
                && retObj.totalkill == 0
                && retObj.totaldeath == 0)
            {
                retObj.totalhonorpoint = PvP_Define.BasePvPPoint;
            }

            return retObj;
        }

        public static Result_Define.eResult SetUser_PvP_Record(ref TxnBlock TB, PVP_Record setInfo)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            setInfo.updateTimeRedisValue = (int)GenericFetch.ConvertToMSeedTime();
            int baseSeperator = 0;
            if (setInfo.pvp_type == (int)PvP_Define.ePvPType.MATCH_1VS1) // Do season, daily, weekly table update
            {
                baseSeperator = setInfo.seperater = PvPManager.GetSeperater_Season(ref TB);                
                retError = SetUser_PvP_RecordToDB(ref TB, setInfo, PvP_Define.PvP_PvP_Season_TableName);
                if (retError == Result_Define.eResult.SUCCESS)
                {
                    setInfo.seperater = PvPManager.GetSeperater_Week(ref TB);
                    retError = SetUser_PvP_RecordToDB(ref TB, setInfo, PvP_Define.PvP_PvP_Weekly_TableName);
                }              
            }
            else if (setInfo.pvp_type == (int)PvP_Define.ePvPType.MATCH_FREE)
            {
                baseSeperator = setInfo.seperater = PvPManager.GetSeperater_Week(ref TB);
                retError = SetUser_PvP_RecordToDB(ref TB, setInfo, PvP_Define.PvP_PvP_Weekly_TableName);
            }
            else if (setInfo.pvp_type == (int)PvP_Define.ePvPType.MATCH_GUILD_3VS3)
            {
                baseSeperator = setInfo.seperater = PvPManager.GetSeperater_Month(ref TB);
                retError = SetUser_PvP_RecordToDB(ref TB, setInfo, PvP_Define.PvP_PvP_Monthly_TableName);
                if (retError == Result_Define.eResult.SUCCESS)
                {
                    setInfo.seperater = PvPManager.GetSeperater_Week(ref TB);
                    retError = SetUser_PvP_RecordToDB(ref TB, setInfo, PvP_Define.PvP_PvP_Weekly_TableName);
                }
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                setInfo.seperater = PvPManager.GetSeperater_Day(ref TB);
                retError = SetUser_PvP_RecordToDB(ref TB, setInfo, PvP_Define.PvP_PvP_Daily_TableName);
            }

            if (retError == Result_Define.eResult.SUCCESS)
                retError = PvPManager.SetUser_PvP_Point(setInfo, baseSeperator);
            
            return retError;
        }

        private static Result_Define.eResult SetUser_PvP_RecordToDB(ref TxnBlock TB, PVP_Record setInfo, string setTable, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON aid = @aid AND pvp_type = @pvptype AND seperater = @curseperater
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                    totalhonorpoint = @currWeekRankPoint, 	
	                                                totalkill = @currWeekKill, 
	                                                totaldeath = @currWeekDeath,
	                                                straightwin = CASE WHEN @currStraightWin > 0 THEN straightwin + 1 ELSE 0 End ,
	                                                straightlose = CASE WHEN  @currStraightLose > 0 THEN straightlose + 1 ELSE 0 End ,
	                                                update_date = getdate(),
	                                                updateTimeRedisValue = @updateTimeRedisValue
                                                WHEN NOT MATCHED THEN
                                                   INSERT (
                                                    aid, user_nick, 
                                                    totalkill, totaldeath, totalhonorpoint,
                                                    weekkill, weekdeath, weekhonorpoint, 
                                                    creation_date, update_date, dailycashgetyn,
                                                    pvp_type, straightwin, straightlose, 
                                                    seperater, updateTimeRedisValue ) 
                                                  VALUES (
                                                    @aid, @user_nick, 
                                                    @currWeekKill, @currWeekDeath, @currWeekRankPoint, 
                                                    0, 0, 0, 
                                                    getdate(), getdate(), 'N',
                                                    @pvptype, @baseStraightWin, @baseStraightLose, 
                                                    @curseperater, @updateTimeRedisValue );
                                    ", setTable);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = setQuery;
            cmd.Parameters.AddWithValue("@aid", setInfo.aid);
            cmd.Parameters.AddWithValue("@user_nick", setInfo.user_nick);
            cmd.Parameters.AddWithValue("@currWeekKill", setInfo.totalkill);
            cmd.Parameters.AddWithValue("@currWeekDeath", setInfo.totaldeath);
            cmd.Parameters.AddWithValue("@currStraightWin", setInfo.straightwin);
            cmd.Parameters.AddWithValue("@currStraightLose", setInfo.straightlose);
            cmd.Parameters.AddWithValue("@currWeekRankPoint", setInfo.totalhonorpoint);
            cmd.Parameters.AddWithValue("@baseStraightWin", setInfo.straightwin > 0 ? 1 : 0);
            cmd.Parameters.AddWithValue("@baseStraightLose", setInfo.straightlose > 0 ? 1: 0);
            cmd.Parameters.AddWithValue("@pvptype", setInfo.pvp_type);
            cmd.Parameters.AddWithValue("@curseperater", setInfo.seperater);
            cmd.Parameters.AddWithValue("@updateTimeRedisValue", setInfo.updateTimeRedisValue);

            RemoveUser_PvP_Record(setInfo.aid, setTable, setInfo.pvp_type, setInfo.seperater);
            return TB.ExcuteSqlCommand(dbkey, ref cmd) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }


        public static Result_Define.eResult AddUser_PvP_CountToDB(ref TxnBlock TB, long AID, PvP_Define.ePvPType PvPType = PvP_Define.ePvPType.MATCH_FREE, int map_index = 1, int add_count = 1, string dbkey = PvP_Define.PvP_Info_DB)
        {
            List<User_PvP_Play_Info> getPvPCountList = PvPManager.GetUser_PvPInfo_List(ref TB, AID, PvPType);
            var pvpInfo = getPvPCountList.Find(item => item.map_index == map_index);

            if (pvpInfo != null)
            {
                string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON aid = @aid AND pvp_type = @pvptype AND map_index = @map_index
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                    play_count = play_count + @add_count, 	
	                                                regdate = getdate()
                                                WHEN NOT MATCHED THEN
                                                   INSERT ([aid], [pvp_type], [play_count], [regdate], [map_index])
                                                   VALUES (@aid, @pvptype, @add_count, getdate(), @map_index);
                                    ", PvP_Define.PvP_PlayInfo_TableName);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = setQuery;
                cmd.Parameters.AddWithValue("@aid", AID);
                cmd.Parameters.AddWithValue("@pvptype", (int)PvPType);
                cmd.Parameters.AddWithValue("@add_count", add_count);
                cmd.Parameters.AddWithValue("@map_index", map_index);
                Result_Define.eResult retError = TB.ExcuteSqlCommand(dbkey, ref cmd) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                if(retError == Result_Define.eResult.SUCCESS) 
                    GetUser_PvPInfo(ref TB, AID, PvPType, true);
                return retError;
            }
            else
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            
        }

        public static Result_Define.eResult UpdateHighestGrade(ref TxnBlock TB, long AID, int Grade = 1, int map_index = 1, PvP_Define.ePvPType PvPType = PvP_Define.ePvPType.MATCH_FREE, string dbkey = PvP_Define.PvP_Info_DB)
        {
            SqlCommand commandUser_PvPInfo = new SqlCommand();
            commandUser_PvPInfo.CommandText = "PVP_SetPVPPlayLastHighGrade";
            commandUser_PvPInfo.Parameters.Add("@aid", SqlDbType.BigInt).Value = AID;
            commandUser_PvPInfo.Parameters.Add("@pvpType", SqlDbType.TinyInt).Value = (byte)PvPType;
            commandUser_PvPInfo.Parameters.Add("@newHighGrade", SqlDbType.Int).Value = Grade;
            commandUser_PvPInfo.Parameters.Add("@map_index", SqlDbType.Int).Value = map_index;

            return TB.ExcuteSqlStoredProcedure(dbkey, ref commandUser_PvPInfo) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static void RemoveGuildPvP_Record(ref TxnBlock TB, long GID)
        {
            int seperater = PvPManager.GetSeperater(ref TB, PvP_Define.ePvPType.MATCH_GUILD_3VS3);
            string setKey = string.Format("{0}_{1}_{2}", PvP_Define.PvP_GuildPvP_Monthly_TableName, GID, seperater);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_Ranking, setKey);
        }

        public static Guild_PVP_Record GetGuild_PvP_Record(ref TxnBlock TB, long GID, int seperater = -1, bool Flush = false, string dbkey = PvP_Define.PvP_Guild_Info_DB)
        {
            if(seperater < 0)
                seperater = PvPManager.GetSeperater(ref TB, PvP_Define.ePvPType.MATCH_GUILD_3VS3);
            string setKey = string.Format("{0}_{1}_{2}", PvP_Define.PvP_GuildPvP_Monthly_TableName, GID, seperater);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE gid = {1} AND seperater = {2}", PvP_Define.PvP_GuildPvP_Monthly_TableName, GID, seperater);
            Guild_PVP_Record retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<Guild_PVP_Record>(ref TB, DataManager_Define.RedisServerAlias_Ranking, setKey, setQuery, dbkey, Flush);

            if (retObj == null)
                retObj = new Guild_PVP_Record(GID, seperater);

            if (retObj.win == 0
                && retObj.lose == 0
                && retObj.rankpoint == 0)
            {
                retObj.rankpoint = PvP_Define.BaseGuildPvPPoint;
            }

            return retObj;
        }

        private static string GetRedisKey_Guild_User_PvP_Record(long AID, string setTable, int seperater)
        {
            return string.Format("{0}_{1}_{2}_{3}", PvP_Define.PvP_Guild_User_Prefix, setTable, AID, seperater);
        }

        public static void RemoveGuild_User_PvP_Record(ref TxnBlock TB, long AID, string setTable)
        {
            int seperater = PvPManager.GetSeperater(ref TB, PvP_Define.ePvPType.MATCH_GUILD_3VS3);
            string setKey = GetRedisKey_Guild_User_PvP_Record(AID, setTable, seperater);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_Ranking, setKey);
        }

        public static Guild_User_PVP_Record GetGuild_User_PvP_Record(ref TxnBlock TB, long AID, long GID, string setTable, int seperater = 0, bool Flush = false, string dbkey = PvP_Define.PvP_Guild_Info_DB)
        {

            if (setTable == PvP_Define.PvP_GuildUser_Monthly_TableName)
                seperater = seperater < 1 ? PvPManager.GetSeperater_Month(ref TB) : seperater;
            else if (setTable == PvP_Define.PvP_GuildUser_Weekly_TableName)
                seperater = seperater < 1 ? PvPManager.GetSeperater_Week(ref TB) : seperater;
            else if (setTable == PvP_Define.PvP_GuildUser_Daily_TableName)
                seperater = seperater < 1 ? PvPManager.GetSeperater_Day(ref TB) : seperater;
            else if (setTable == PvP_Define.PvP_GuildPvP_Monthly_TableName)
                seperater = seperater < 1 ? PvPManager.GetSeperater_Month(ref TB) : seperater;
            else
            {
                setTable = PvP_Define.PvP_GuildUser_Monthly_TableName;
                seperater = seperater < 1 ? PvPManager.GetSeperater_Month(ref TB) : seperater;
            }

            string setKey = GetRedisKey_Guild_User_PvP_Record(AID, setTable, seperater);
            string setQuery = string.Format(@"SELECT 
                                                    aid, gid, user_nick, 
                                                    totalkill, totaldeath, totalhonorpoint,
                                                    totalwin, totallose, rating_point,
                                                    creation_date, update_date,
                                                    (SELECT TOP 1 lastjoin_date FROM Guild_User_PVP_Record_Monthly WITH(NOLOCK)  WHERE aid = {1} AND seperater = {2} ORDER BY lastjoin_date DESC) as lastjoin_date,
                                                    dailycashgetyn,
                                                    straightwin, straightlose, 
                                                    seperater, updateTimeRedisValue
                                            FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND seperater = {2} AND gid = {3}"
                                            , setTable, AID, seperater, GID);
            Guild_User_PVP_Record retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<Guild_User_PVP_Record>(ref TB, DataManager_Define.RedisServerAlias_Ranking, setKey, setQuery, dbkey, Flush);
            if (retObj == null)
            {
                retObj = new Guild_User_PVP_Record(AID, GID, seperater);
                setQuery = string.Format("SELECT TOP 1 lastjoin_date FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND seperater = {2} ORDER BY lastjoin_date", setTable, AID, seperater);
                GuildPvP_DateTime getDBTime = TheSoul.DataManager.GenericFetch.FetchFromDB<GuildPvP_DateTime>(ref TB, setQuery, dbkey);
                DateTime setLastTime = getDBTime == null ? DateTime.Now.AddDays(-1) : getDBTime.lastjoin_date;
                retObj.lastjoin_date = setLastTime;
                Account_Simple userAccount = AccountManager.GetSimpleAccountInfo(ref TB, AID);
                retObj.user_nick = userAccount.username;
                int maxLevel = CharacterManager.GetCharacterMaxLevel_FromDB(ref TB, AID);

                foreach(KeyValuePair<KeyValuePair<int, int>, PvP_Define.ePvPConstDef> checkKey in PvP_Define.PvPGuildRating_BasePointList)
                {
                    if (checkKey.Key.Key <= maxLevel && maxLevel <= checkKey.Key.Value)
                    {
                        retObj.rating_point = SystemData.GetConstValueInt(ref TB, PvP_Define.PvP_Const_Def_Key_List[checkKey.Value]);
                        break;
                    }
                }
                if (retObj.rating_point == 0)
                    retObj.rating_point = SystemData.GetConstValueInt(ref TB, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_GUILD_G3VS3_MATCHINGINIT1]);
            }
            return retObj;
        }

        public static Result_Define.eResult DeleteGuild_PvP_Record(ref TxnBlock TB, long aid, long gid, string dbkey = PvP_Define.PvP_Guild_Info_DB)
        {
            string setTable = PvP_Define.PvP_GuildUser_Monthly_TableName;
            string setQuery = string.Format("DELETE FROM {0} WHERE aid = {1} and (gid = {2} OR gid = 0)", setTable, aid, gid);
            Result_Define.eResult retError = (TB.ExcuteSqlCommand(dbkey, setQuery)) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            if (retError == Result_Define.eResult.SUCCESS)
            {
                setTable = PvP_Define.PvP_GuildUser_Weekly_TableName;
                setQuery = string.Format("DELETE FROM {0} WHERE aid = {1} and (gid = {2} OR gid = 0)", setTable, aid, gid);
                retError = (TB.ExcuteSqlCommand(dbkey, setQuery)) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }
            if (retError == Result_Define.eResult.SUCCESS)
            {
                setTable = PvP_Define.PvP_GuildUser_Daily_TableName;
                setQuery = string.Format("DELETE FROM {0} WHERE aid = {1} and (gid = {2} OR gid = 0)", setTable, aid, gid);
                retError = (TB.ExcuteSqlCommand(dbkey, setQuery)) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }
            return retError;
        }

        public static Result_Define.eResult SetGuild_PvP_Record(ref TxnBlock TB, Guild_PVP_Record setInfo, string dbkey = PvP_Define.PvP_Guild_Info_DB)
        {
            setInfo.updateTimeRedisValue = (int)GenericFetch.ConvertToMSeedTime();
            int baseSeperator = PvPManager.GetSeperater_Month(ref TB);
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON gid = @gid AND seperater = @curseperater
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                    rankpoint = @currRankPoint, 	
	                                                win = @currWin, 
	                                                lose = @currLose,
	                                                straightwin = @currStraightWin,
	                                                straightlose = @currStraightLose,
	                                                update_date = getdate(),
	                                                updateTimeRedisValue = @updateTimeRedisValue
                                                WHEN NOT MATCHED THEN
                                                   INSERT (
                                                    gid, win, lose, rankpoint,
                                                    straightwin, straightlose, 
                                                    seperater, updateTimeRedisValue,
                                                    creation_date, update_date
                                                    ) 
                                                  VALUES (
                                                    @gid, @currWin, @currLose, @currRankPoint, 
                                                    @currStraightWin, @currStraightLose, 
                                                    @curseperater, @updateTimeRedisValue,
                                                    getdate(), getdate() );
                                    ", PvP_Define.PvP_GuildPvP_Monthly_TableName);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = setQuery;
            cmd.Parameters.AddWithValue("@gid", setInfo.gid);
            cmd.Parameters.AddWithValue("@currRankPoint", setInfo.rankpoint);
            cmd.Parameters.AddWithValue("@currWin", setInfo.win);
            cmd.Parameters.AddWithValue("@currLose", setInfo.lose);
            cmd.Parameters.AddWithValue("@currStraightWin", setInfo.straightwin);
            cmd.Parameters.AddWithValue("@currStraightLose", setInfo.straightlose);
            cmd.Parameters.AddWithValue("@curseperater", setInfo.seperater);
            cmd.Parameters.AddWithValue("@updateTimeRedisValue", setInfo.updateTimeRedisValue);
            Result_Define.eResult retError = TB.ExcuteSqlCommand(dbkey, ref cmd) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR; 

            if (retError == Result_Define.eResult.SUCCESS)
            {
                RemoveGuildPvP_Record(ref TB, setInfo.gid);
                retError = PvPManager.SetGuildPvP_GuildWar_Rank_Info(ref TB, setInfo.gid);
            }
            return retError;
        }


        public static Result_Define.eResult SetGuild_User_PvP_Record(ref TxnBlock TB, Guild_User_PVP_Record setInfo, long GID, string dbkey = PvP_Define.PvP_Guild_Info_DB)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            setInfo.updateTimeRedisValue = (int)GenericFetch.ConvertToMSeedTime();
            int baseSeperator = PvPManager.GetSeperater_Month(ref TB);
            retError = SetGuild_User_PvP_RecordToDB(ref TB, setInfo, PvP_Define.PvP_GuildUser_Monthly_TableName, GID, dbkey);
            if (retError == Result_Define.eResult.SUCCESS)
            {
                setInfo.seperater = PvPManager.GetSeperater_Week(ref TB);
                retError = SetGuild_User_PvP_RecordToDB(ref TB, setInfo, PvP_Define.PvP_GuildUser_Weekly_TableName, GID, dbkey);
            }
            if (retError == Result_Define.eResult.SUCCESS)
            {
                setInfo.seperater = PvPManager.GetSeperater_Day(ref TB);
                retError = SetGuild_User_PvP_RecordToDB(ref TB, setInfo, PvP_Define.PvP_GuildUser_Daily_TableName, GID, dbkey);
            }

            return retError;
        }

        private static Result_Define.eResult SetGuild_User_PvP_RecordToDB(ref TxnBlock TB, Guild_User_PVP_Record setInfo, string setTable, long GID, string dbkey = PvP_Define.PvP_Guild_Info_DB)
        {
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON aid = @aid AND gid = @gid AND seperater = @curseperater
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                    totalhonorpoint = @currWeekRankPoint, 	
	                                                totalkill = @currTotalKill, 
	                                                totaldeath = @currTotalDeath,
	                                                totalwin = @currTotalWin, 
	                                                totallose= @currTotalLose,
	                                                straightwin = CASE WHEN @currStraightWin > 0 THEN straightwin + 1 ELSE 0 End ,
	                                                straightlose = CASE WHEN  @currStraightLose > 0 THEN straightlose + 1 ELSE 0 End ,
	                                                rating_point = @currRating,
	                                                update_date = getdate(),
	                                                updateTimeRedisValue = @updateTimeRedisValue
                                                WHEN NOT MATCHED THEN
                                                   INSERT (
                                                    aid, gid, user_nick, 
                                                    totalkill, totaldeath, totalhonorpoint,
                                                    totalwin, totallose, rating_point,
                                                    creation_date, update_date, lastjoin_date, dailycashgetyn,
                                                    straightwin, straightlose, 
                                                    seperater, updateTimeRedisValue ) 
                                                  VALUES (
                                                    @aid, @gid, @user_nick, 
                                                    @currTotalKill, @currTotalDeath, @currWeekRankPoint,
                                                    @currTotalWin, @currTotalLose, @currRating,
                                                    getdate(), getdate(), getdate(), 'N',
                                                    @baseStraightWin, @baseStraightLose, 
                                                    @curseperater, @updateTimeRedisValue );
                                    ", setTable);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = setQuery;
            cmd.Parameters.AddWithValue("@aid", setInfo.aid);
            cmd.Parameters.AddWithValue("@gid", GID);
            cmd.Parameters.AddWithValue("@user_nick", setInfo.user_nick);
            cmd.Parameters.AddWithValue("@currTotalKill", setInfo.totalkill);
            cmd.Parameters.AddWithValue("@currTotalDeath", setInfo.totaldeath);
            cmd.Parameters.AddWithValue("@currTotalWin", setInfo.totalwin);
            cmd.Parameters.AddWithValue("@currTotalLose", setInfo.totallose);
            cmd.Parameters.AddWithValue("@currStraightWin", setInfo.straightwin);
            cmd.Parameters.AddWithValue("@currStraightLose", setInfo.straightlose);
            cmd.Parameters.AddWithValue("@currWeekRankPoint", setInfo.totalhonorpoint);
            cmd.Parameters.AddWithValue("@currRating", setInfo.rating_point);
            cmd.Parameters.AddWithValue("@baseStraightWin", setInfo.straightwin > 0 ? 1 : 0);
            cmd.Parameters.AddWithValue("@baseStraightLose", setInfo.straightlose > 0 ? 1 : 0);

            cmd.Parameters.AddWithValue("@curseperater", setInfo.seperater);
            cmd.Parameters.AddWithValue("@updateTimeRedisValue", setInfo.updateTimeRedisValue);

            Result_Define.eResult retError = TB.ExcuteSqlCommand(dbkey, ref cmd) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            if (retError == Result_Define.eResult.SUCCESS)
            {
                RemoveGuild_User_PvP_Record(ref TB, setInfo.aid, setTable);
            }
            return retError;
        }

        public static Result_Define.eResult SetGuild_User_PvP_Record_JoinReset(ref TxnBlock TB, Guild_User_PVP_Record setInfo, string setTable, int playTime, string dbkey = PvP_Define.PvP_Guild_Info_DB)
        {
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON aid = @aid AND seperater = @curseperater
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                    lastjoin_date = DATEADD(MINUTE, @playtime, getdate())
                                                WHEN NOT MATCHED THEN
                                                   INSERT (
                                                    aid, gid, user_nick, 
                                                    totalkill, totaldeath, totalhonorpoint,
                                                    totalwin, totallose, rating_point,
                                                    creation_date, update_date, lastjoin_date, dailycashgetyn,
                                                    straightwin, straightlose, 
                                                    seperater, updateTimeRedisValue ) 
                                                  VALUES (
                                                    @aid, @gid, @user_nick, 
                                                    0, 0, 0,
                                                    0, 0, 0,
                                                    getdate(), getdate(), DATEADD(MINUTE, @playtime, getdate()), 'N',
                                                    0, 0, 
                                                    @curseperater, 0 );
                                    ", setTable);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = setQuery;
            cmd.Parameters.AddWithValue("@aid", setInfo.aid);
            cmd.Parameters.AddWithValue("@gid", setInfo.gid);
            cmd.Parameters.AddWithValue("@user_nick", setInfo.user_nick);
            cmd.Parameters.AddWithValue("@playtime", playTime * -1);
            cmd.Parameters.AddWithValue("@curseperater", setInfo.seperater);
            Result_Define.eResult retError = TB.ExcuteSqlCommand(dbkey, ref cmd) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            if (retError == Result_Define.eResult.SUCCESS)
                RemoveGuild_User_PvP_Record(ref TB, setInfo.aid, setTable);

            return retError;
        }

        public static Result_Define.eResult SetGuild_User_PvP_Record_JoinSet(ref TxnBlock TB, Guild_User_PVP_Record setInfo, string setTable, int playTime, string dbkey = PvP_Define.PvP_Guild_Info_DB)
        {
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON aid = @aid AND seperater = @curseperater
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                    lastjoin_date = getdate()
                                                WHEN NOT MATCHED THEN
                                                   INSERT (
                                                    aid, gid, user_nick, 
                                                    totalkill, totaldeath, totalhonorpoint,
                                                    totalwin, totallose, rating_point,
                                                    creation_date, update_date, lastjoin_date, dailycashgetyn,
                                                    straightwin, straightlose, 
                                                    seperater, updateTimeRedisValue ) 
                                                  VALUES (
                                                    @aid, @gid, @user_nick, 
                                                    0, 0, 0,
                                                    0, 0, 0,
                                                    getdate(), getdate(), getdate(), 'N',
                                                    0, 0, 
                                                    @curseperater, 0 );
                                    ", setTable);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = setQuery;
            cmd.Parameters.AddWithValue("@aid", setInfo.aid);
            cmd.Parameters.AddWithValue("@gid", setInfo.gid);
            cmd.Parameters.AddWithValue("@user_nick", setInfo.user_nick);
            cmd.Parameters.AddWithValue("@curseperater", setInfo.seperater);
            Result_Define.eResult retError = TB.ExcuteSqlCommand(dbkey, ref cmd) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            if (retError == Result_Define.eResult.SUCCESS)
                RemoveGuild_User_PvP_Record(ref TB, setInfo.aid, setTable);

            return retError;
        }

        public static byte CheckPvPAchiveReward(ref TxnBlock TB, long AID, PvP_Define.ePvPType setPvPType)
        {
            List<User_Event_Data> userAchiveList = TriggerManager.Check_Achieve_PvP_Data_List(ref TB, AID);

            List<User_Event_Data> checkList = userAchiveList.FindAll(findEvent => findEvent.PVP_Type == (int)setPvPType);

            List<Character> userCharacter = CharacterManager.GetCharacterList(ref TB, AID);
            List<RetMissionRank> userMission = Dungeon_Manager.GetUser_All_MissionRank(ref TB, AID);
            List<User_GuerrillaDungeon_Play> userGuerillaMission = Dungeon_Manager.GetUser_All_GuerrillaDungeonRank(ref TB, AID);
            List<RetEliteDungeonRank> userElistMission = Dungeon_Manager.GetUser_All_EliteDungeonRank(ref TB, AID);

            foreach (User_Event_Data checkEvent in checkList)
            {
                if (checkEvent.RewardFlag.Equals("N"))
                {
                    bool isClear = false;
                    int maxValue1 = 0;
                    int maxValue2 = 0;
                    TriggerManager.CheckClear(ref TB, AID, checkEvent, out isClear, out maxValue1, out maxValue2, userCharacter, userMission, userGuerillaMission, userElistMission);

                    if (isClear)
                        return 1;
                }
            }

            return 0;
        }

        public static System_Party_Dungeon GetSystemPartyDungeonInfo(ref TxnBlock TB, int mapindex, bool Flush = false, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setKey = string.Format("{0}", PvP_Define.PvP_System_Party_Dungeon_TableName);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE PartyDungeonID = {1}", PvP_Define.PvP_System_Party_Dungeon_TableName, mapindex);
            System_Party_Dungeon retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Party_Dungeon>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, mapindex.ToString(), setQuery, dbkey, Flush);
            return retObj == null ? new System_Party_Dungeon() : retObj;
        }

        public static void RemoveUserPartyDungeonClear(long AID)
        {
            string setKey = string.Format("{0}_{1}", PvP_Define.PvP_User_PartyDungeon_Clear_TableName, AID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }

        public static List<User_PartyDungeon_Clear> GetUserPartyDungeonClearList(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setKey = string.Format("{0}_{1}", PvP_Define.PvP_User_PartyDungeon_Clear_TableName, AID);
            string setQuery = string.Format("SELECT map_index, clear FROM {0} WITH(NOLOCK)  WHERE aid = {1}", PvP_Define.PvP_User_PartyDungeon_Clear_TableName, AID);
            return TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow<User_PartyDungeon_Clear>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
        }

        public static Result_Define.eResult SetUserPartyDungeonClear(ref TxnBlock TB, long AID, int mapindex, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON aid = @aid and map_index = @map
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                    clear = 1
                                                WHEN NOT MATCHED THEN
                                                   INSERT (
                                                    aid, map_index, clear, regdate ) 
                                                  VALUES ( @aid, @map, 1, getdate() );
                                    ", PvP_Define.PvP_User_PartyDungeon_Clear_TableName);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = setQuery;
            cmd.Parameters.AddWithValue("@aid", AID);
            cmd.Parameters.AddWithValue("@map", mapindex);
            Result_Define.eResult retError = TB.ExcuteSqlCommand(dbkey, ref cmd) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            if (retError == Result_Define.eResult.SUCCESS)
                RemoveUserPartyDungeonClear(AID);
            return retError;
        }

        public static bool CheckPvPOpenTime (ref TxnBlock TB, PvP_Define.ePvPType pvpType, out bool isBonus)
        {
            bool isOpen = false;
            isBonus = false;
            long currentTime = (long)(DateTime.Now - DateTime.Parse(DateTime.Now.ToShortDateString())).TotalSeconds;
            int openStart = 0;
            int openEnd = 0;
            if (PvP_Define.PvP_OpenTime_1st_Const_List.ContainsKey(pvpType))
            {
                PvP_Define.PvPOpenTimeKey setTime = PvP_Define.PvP_OpenTime_1st_Const_List[pvpType];
                openStart = SystemData.AdminConstValueFetchFromRedis(ref TB, PvP_Define.PvP_Const_Def_Key_List[setTime.StartTime]);
                openEnd = SystemData.AdminConstValueFetchFromRedis(ref TB, PvP_Define.PvP_Const_Def_Key_List[setTime.EndTime]);

                if (openStart < openEnd)
                    isOpen = currentTime > openStart && currentTime < openEnd;
                else if (openStart > openEnd)
                    isOpen = currentTime > openStart || currentTime < openEnd;
                else
                    isOpen = true;
            }

            if (!isOpen || pvpType == PvP_Define.ePvPType.MATCH_GUILD_3VS3 || pvpType == PvP_Define.ePvPType.MATCH_1VS1)
            {
                if (PvP_Define.PvP_OpenTime_2nd_Const_List.ContainsKey(pvpType))
                {
                    PvP_Define.PvPOpenTimeKey setTime = PvP_Define.PvP_OpenTime_2nd_Const_List[pvpType];
                    openStart = SystemData.AdminConstValueFetchFromRedis(ref TB, PvP_Define.PvP_Const_Def_Key_List[setTime.StartTime]);
                    openEnd = SystemData.AdminConstValueFetchFromRedis(ref TB, PvP_Define.PvP_Const_Def_Key_List[setTime.EndTime]);

                    if (!isOpen)
                    {
                        if (openStart < openEnd)
                            isOpen = currentTime > openStart && currentTime < openEnd;
                        else if (openStart > openEnd)
                            isOpen = currentTime > openStart || currentTime < openEnd;
                        else
                            isOpen = true;
                    }

                    if (pvpType == PvP_Define.ePvPType.MATCH_GUILD_3VS3 || pvpType == PvP_Define.ePvPType.MATCH_1VS1)
                    {
                        if (openStart < openEnd)
                            isBonus = currentTime > openStart && currentTime < openEnd;
                        else if (openStart > openEnd)
                            isBonus = currentTime > openStart || currentTime < openEnd;
                        else
                            isBonus = true;
                    }
                }
            }

            return isOpen;
        }
    }
}
