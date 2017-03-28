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
    public partial class Dungeon_Manager
    {
        public static List<System_Mission_World> GetSystem_MissionWorldList(ref TxnBlock TB, string dbkey = Dungeon_Define.Dungeon_Info_DB)
        {
            string setKey = string.Format("{0}", Dungeon_Define.Mission_World_TableName);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) ", Dungeon_Define.Mission_World_TableName);

            return GenericFetch.FetchFromRedis_MultipleRow<System_Mission_World>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey);
        }

        public static System_Mission_World GetSystem_MissionWorldInfo(ref TxnBlock TB, long WorldID)
        {
            string setKey = string.Format("{0}_{1}", Dungeon_Define.Mission_World_TableName, Dungeon_Define.Mission_World_Surfix);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE WorldID = {1}", Dungeon_Define.Mission_World_TableName, WorldID);

            System_Mission_World retObj = GenericFetch.FetchFromRedis_Hash<System_Mission_World>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, WorldID.ToString(), setQuery, Dungeon_Define.Dungeon_Info_DB);
            return (retObj != null) ? retObj : new System_Mission_World();
        }

        public static System_Guerilla_Dungeon GetSystem_DarkPassageInfo(ref TxnBlock TB, int DungeonID)
        {
            string setKey = string.Format("{0}_{1}", Dungeon_Define.Dark_Passage_TableName, Dungeon_Define.Mission_World_Surfix);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE DungeonID = {1}", Dungeon_Define.Dark_Passage_TableName, DungeonID);

            System_Guerilla_Dungeon retObj = GenericFetch.FetchFromRedis_Hash<System_Guerilla_Dungeon>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, DungeonID.ToString(), setQuery, Dungeon_Define.Dungeon_Info_DB);
            return (retObj != null) ? retObj : new System_Guerilla_Dungeon();
        }

        public static System_Guerrilla_Soul GetSystem_DarkPassageSoulInfo(ref TxnBlock TB)
        {
            string setQuery = string.Format(@"SELECT TOP 1 * FROM {0} WITH(NOLOCK) ORDER BY NEWID()", Dungeon_Define.Dark_Passage_Soul_TableName);
            System_Guerrilla_Soul retObj = GenericFetch.FetchFromDB<System_Guerrilla_Soul>(ref TB, setQuery, Dungeon_Define.Dungeon_Info_DB);
            return (retObj != null) ? retObj : new System_Guerrilla_Soul();
        }

        public static System_Elite_Dungeon GetSystem_EliteDungeonInfo(ref TxnBlock TB, int DungeonID)
        {
            string setKey = string.Format("{0}_{1}", Dungeon_Define.Eliete_Dungeon_TableName, Dungeon_Define.Mission_World_Surfix);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE DungeonID = {1}", Dungeon_Define.Eliete_Dungeon_TableName, DungeonID);

            System_Elite_Dungeon retObj = GenericFetch.FetchFromRedis_Hash<System_Elite_Dungeon>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, DungeonID.ToString(), setQuery, Dungeon_Define.Dungeon_Info_DB);
            return (retObj != null) ? retObj : new System_Elite_Dungeon();
        }

        public static System_Mission_Stage GetSystem_MissionStageInfo(ref TxnBlock TB, int StageID, string dbkey = Dungeon_Define.Dungeon_Info_DB)
        {
            string setKey = string.Format("{0}_{1}", Dungeon_Define.Mission_Stage_TableName, Dungeon_Define.Mission_Stage_Surfix);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE StageID = {1}", Dungeon_Define.Mission_Stage_TableName, StageID);

            System_Mission_Stage retObj = GenericFetch.FetchFromRedis_Hash<System_Mission_Stage>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, StageID.ToString(), setQuery, dbkey);
            return (retObj != null) ? retObj : new System_Mission_Stage();
        }

        public static System_Booster_Group GetSystem_BoosterGroup(ref TxnBlock TB, int BoostGroupID)
        {
            string setKey = string.Format("{0}_{1}", Dungeon_Define.System_Booster_Group_TableName, Dungeon_Define.Mission_World_Surfix);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE Booster_Group_ID = {1}", Dungeon_Define.System_Booster_Group_TableName, BoostGroupID);
            System_Booster_Group retObj = GenericFetch.FetchFromRedis_Hash<System_Booster_Group>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, BoostGroupID.ToString(), setQuery, Dungeon_Define.Dungeon_Info_DB);
            return (retObj != null) ? retObj : new System_Booster_Group();
        }

        public static int GetUser_Mission_LastStage(ref TxnBlock TB, long AID, string dbkey = Dungeon_Define.Dungeon_Info_DB)
        {
            string setQuery = string.Format(@"SELECT MAX(stageid) as stageid FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND [rank] > 0", Dungeon_Define.Mission_Play_TableName, AID);
            User_Mission_Last_Stage retObj = GenericFetch.FetchFromDB<User_Mission_Last_Stage>(ref TB, setQuery, dbkey);
            if(retObj == null) 
                retObj = new User_Mission_Last_Stage();
            return retObj.stageid;
        }


        public static User_Mission_Play GetUser_MissionInfo(ref TxnBlock TB, ref Result_Define.eResult retError, long AID, long WorldID, long StageID, bool Flush = false, string dbkey = Dungeon_Define.Dungeon_Info_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}_{3}_{4}", Dungeon_Define.Mission_Play_TableName, AID, WorldID, StageID, Dungeon_Define.Mission_Play_Surfix);
            retError = Result_Define.eResult.SUCCESS;
            User_Mission_Play userPlayInfo = Flush ? null: GenericFetch.FetchFromOnly_Redis<User_Mission_Play>(DataManager_Define.RedisServerAlias_User, setKey);
            if (userPlayInfo == null)
            {
                SqlCommand commandUser_MissionInfo = new SqlCommand();
                commandUser_MissionInfo.CommandText = "System_Get_UserMissionPlayInfo";
                var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                commandUser_MissionInfo.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                commandUser_MissionInfo.Parameters.Add("@WorldID", SqlDbType.NVarChar, 128).Value = WorldID;
                commandUser_MissionInfo.Parameters.Add("@StageID", SqlDbType.NVarChar, 32).Value = StageID;
                commandUser_MissionInfo.Parameters.Add(result);

                SqlDataReader getDr = null;
                if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandUser_MissionInfo, ref getDr))
                {
                    if (getDr != null)
                    {
                        var r = SQLtoJson.Serialize(ref getDr);
                        string json = mJsonSerializer.ToJsonString(r);
                        getDr.Dispose(); getDr.Close();
                        int checkValue = System.Convert.ToInt32(result.Value);
                        commandUser_MissionInfo.Dispose();
                        if (checkValue < 0)
                        {
                            retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                            return new User_Mission_Play();
                        }

                        User_Mission_Play[] retSet = mJsonSerializer.JsonToObject<User_Mission_Play[]>(json);

                        if (retSet.Length > 0)
                            userPlayInfo = retSet[0];
                        else
                        {
                            retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                            return new User_Mission_Play();
                        }

                        RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_User, setKey, userPlayInfo);
                    }
                }
                else
                {
                    commandUser_MissionInfo.Dispose();
                    retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                    return new User_Mission_Play();
                }
            }
            return userPlayInfo;
        }

        public static User_GuerrillaDungeon_Play GetUser_DarkPassagePlayInfo(ref TxnBlock TB, ref Result_Define.eResult retError, long AID, int dungeonid, bool Flush = false, string dbkey = Dungeon_Define.Dungeon_Info_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}_{3}", Dungeon_Define.Dark_Passage_Play_TableName, AID, dungeonid, Dungeon_Define.Mission_Play_Surfix);
            retError = Result_Define.eResult.SUCCESS;

            User_GuerrillaDungeon_Play userPlayInfo = Flush ? null : GenericFetch.FetchFromOnly_Redis<User_GuerrillaDungeon_Play>(DataManager_Define.RedisServerAlias_User, setKey);
            if (userPlayInfo == null)
            {
                SqlCommand commandUser_MissionInfo = new SqlCommand();
                commandUser_MissionInfo.CommandText = "System_Get_UserGuerrillaDungeonPlayInfo";
                var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                commandUser_MissionInfo.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                commandUser_MissionInfo.Parameters.Add("@dungeonid", SqlDbType.NVarChar, 128).Value = dungeonid;
                commandUser_MissionInfo.Parameters.Add(result);

                SqlDataReader getDr = null;
                if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandUser_MissionInfo, ref getDr))
                {
                    if (getDr != null)
                    {
                        var r = SQLtoJson.Serialize(ref getDr);
                        string json = mJsonSerializer.ToJsonString(r);
                        getDr.Dispose(); getDr.Close(); 
                        int checkValue = System.Convert.ToInt32(result.Value);
                        commandUser_MissionInfo.Dispose();

                        if (checkValue < 0)
                        {
                            retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                            return new User_GuerrillaDungeon_Play();
                        }

                        User_GuerrillaDungeon_Play[] retSet = mJsonSerializer.JsonToObject<User_GuerrillaDungeon_Play[]>(json);

                        if (retSet.Length > 0)
                            userPlayInfo = retSet[0];
                        else
                        {
                            retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                            return new User_GuerrillaDungeon_Play();
                        }

                        RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_User, setKey, userPlayInfo);
                    }
                }
                else
                {
                    commandUser_MissionInfo.Dispose();
                    retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                    return new User_GuerrillaDungeon_Play();
                }
            }
            return userPlayInfo;
        }

        public static RetUserEliteDungeon_Play GetUser_EliteDungeonPlayInfo(ref TxnBlock TB, ref Result_Define.eResult retError, long AID, int dungeonid, bool Flush = false, string dbkey = Dungeon_Define.Dungeon_Info_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}_{3}", Dungeon_Define.EliteDungeon_Play_TableName, AID, dungeonid, Dungeon_Define.Mission_Play_Surfix);

            RetUserEliteDungeon_Play userPlayInfo = Flush ? null : GenericFetch.FetchFromOnly_Redis<RetUserEliteDungeon_Play>(DataManager_Define.RedisServerAlias_User, setKey);
            retError = Result_Define.eResult.SUCCESS;
            if (userPlayInfo == null)
            {
                long laststage = 0;
                long lasttry = 0;
                List<User_EliteDungeon_Play> retList = new List<User_EliteDungeon_Play>();
                SqlCommand commandUser_MissionInfo = new SqlCommand();
                commandUser_MissionInfo.CommandText = "System_Get_UserEliteDungeonPlayInfo";
                var retlasttry = new SqlParameter("@lasttry", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                var retlaststage = new SqlParameter("@laststage", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                commandUser_MissionInfo.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                commandUser_MissionInfo.Parameters.Add("@dungeonid", SqlDbType.NVarChar, 128).Value = dungeonid;
                commandUser_MissionInfo.Parameters.Add(retlasttry);
                commandUser_MissionInfo.Parameters.Add(retlaststage);
                commandUser_MissionInfo.Parameters.Add(result);

                SqlDataReader getDr = null;
                if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandUser_MissionInfo, ref getDr))
                {
                    if (getDr != null)
                    {
                        var r = SQLtoJson.Serialize(ref getDr);
                        string json = mJsonSerializer.ToJsonString(r);
                        int checkValue = System.Convert.ToInt32(result.Value);
                        getDr.Dispose(); getDr.Close();

                        if (checkValue < 0)
                        {
                            retList = new List<User_EliteDungeon_Play>();
                            userPlayInfo = new RetUserEliteDungeon_Play();
                            userPlayInfo.retList = retList;
                            retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                            return userPlayInfo;
                        }
                        else
                        {
                            laststage = System.Convert.ToInt64(retlaststage.Value);
                            lasttry = System.Convert.ToInt64(retlasttry.Value);
                            retList = mJsonSerializer.JsonToObject<List<User_EliteDungeon_Play>>(json);
                        }
                    }
                }
                commandUser_MissionInfo.Dispose();
                userPlayInfo = new RetUserEliteDungeon_Play();
                userPlayInfo.retList = retList;
                userPlayInfo.laststage = laststage;
                userPlayInfo.selectedidx = lasttry;

                RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_User, setKey, userPlayInfo);

                if (dungeonid != 0)
                {
                    string removeKey = string.Format("{0}_{1}_{2}_{3}", Dungeon_Define.EliteDungeon_Play_TableName, AID, 0, Dungeon_Define.Mission_Play_Surfix);
                    RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, removeKey);
                }
            }
            return userPlayInfo;
        }

        public static List<RetMissionRank> GetUser_All_MissionRank(ref TxnBlock TB, long AID, long WorldID = 0, string dbkey = Dungeon_Define.Dungeon_Info_DB)
        {
            string setQuery = WorldID > 0 ?
                string.Format(@"SELECT worldid, stageid, rank FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND worldid = {2}", Dungeon_Define.Mission_Play_TableName, AID, WorldID) :
                string.Format(@"SELECT worldid, stageid, rank FROM {0} WITH(NOLOCK)  WHERE aid = {1}", Dungeon_Define.Mission_Play_TableName, AID) ;

            return GenericFetch.FetchFromDB_MultipleRow<RetMissionRank>(ref TB, setQuery, Dungeon_Define.Dungeon_Info_DB);
        }

        public static List<User_Mission_Play> GetUser_All_MissionInfo(ref TxnBlock TB, long AID, long WorldID, string dbkey = Dungeon_Define.Dungeon_Info_DB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1}", Dungeon_Define.Mission_Play_TableName, AID);
            //string setQuery = WorldID > 0 ?
            //    string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND worldid = {2}", Dungeon_Define.Mission_Play_TableName, AID, WorldID) :
            //    string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1}", Dungeon_Define.Mission_Play_TableName, AID);

            return GenericFetch.FetchFromDB_MultipleRow<User_Mission_Play>(ref TB, setQuery, Dungeon_Define.Dungeon_Info_DB);
        }



        public static List<RetWorldRank> GetUser_All_WorldReward(ref TxnBlock TB, long AID, string dbkey = Dungeon_Define.Dungeon_Info_DB)
        {
            string setQuery = string.Format(@"SELECT worldid, 0 as rank, reward1, reward2 FROM {0} WITH(NOLOCK)  WHERE aid = {1}", Dungeon_Define.World_Rank_Reward_TableName, AID);
            return GenericFetch.FetchFromDB_MultipleRow<RetWorldRank>(ref TB, setQuery, Dungeon_Define.Dungeon_Info_DB);
        }

        public static RetWorldRank GetUser_WorldReward(ref TxnBlock TB, long AID, long WorldID, bool Flush = false, string dbkey = Dungeon_Define.Dungeon_Info_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}_{3}", Dungeon_Define.World_Rank_Reward_TableName, AID, WorldID, Dungeon_Define.Mission_Play_Surfix);
            RetWorldRank retObj = Flush ? null : GenericFetch.FetchFromOnly_Redis<RetWorldRank>(DataManager_Define.RedisServerAlias_User, setKey);
            if (retObj == null)
            {
                SqlCommand commandUser_WorldReawrdInfo = new SqlCommand();
                commandUser_WorldReawrdInfo.CommandText = "System_Get_UserMissionWorldRankReward";
                var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                commandUser_WorldReawrdInfo.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                commandUser_WorldReawrdInfo.Parameters.Add("@WorldID", SqlDbType.NVarChar, 128).Value = WorldID;
                commandUser_WorldReawrdInfo.Parameters.Add(result);

                SqlDataReader getDr = null;
                if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandUser_WorldReawrdInfo, ref getDr))
                {
                    if (getDr != null)
                    {
                        var r = SQLtoJson.Serialize(ref getDr);
                        string json = mJsonSerializer.ToJsonString(r);
                        getDr.Dispose(); getDr.Close();
                        int checkValue = System.Convert.ToInt32(result.Value);
                        commandUser_WorldReawrdInfo.Dispose();
                        if (checkValue < 0)
                            return null;

                        RetWorldRank[] retSet = mJsonSerializer.JsonToObject<RetWorldRank[]>(json);

                        if (retSet.Length > 0)
                            retObj = retSet[0];
                        else
                            return null;

                        RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_User, setKey, retObj);
                    }
                }
                else
                {
                    commandUser_WorldReawrdInfo.Dispose();
                    return null;
                }

            }
            return retObj;
        }

        const string setWorldRewardFirstName = "reward";

        public static Result_Define.eResult UpdateWorldRankReward(ref TxnBlock TB, long AID, long WorldID, int GetType, bool isGet = true, bool Flush = false, string dbkey = Dungeon_Define.Dungeon_Info_DB)
        {
            // hard coding for field name set

            string setField = string.Format("{0}{1}", setWorldRewardFirstName, GetType);

            string setQuery = string.Format(@"UPDATE {0} SET 
                                                    {1} = {2}
                                                WHERE aid = {3} AND worldid = {4} ",
                                                    Dungeon_Define.World_Rank_Reward_TableName
                                                    , setField
                                                    , (isGet ? 1 : 0)
                                                    , AID
                                                    , WorldID
                                                    );
            if (TB.ExcuteSqlCommand(dbkey, setQuery))
                return Result_Define.eResult.SUCCESS;
            else
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static List<User_GuerrillaDungeon_Play> GetUser_All_GuerrillaDungeonRank(ref TxnBlock TB, long AID, string dbkey = Dungeon_Define.Dungeon_Info_DB)
        {
            //string setQuery = string.Format(@"SELECT dungeonid, rank, challengecount FROM {0} WITH(NOLOCK) WHERE aid = {1}", Dungeon_Define.Dark_Passage_Play_TableName, AID);
            string setQuery = string.Format(@"SELECT    C.Worldid as worldid , A.DungeonID as dungeonid, B.rank, 
                                                        B.challengecount, B.challengereset, A.Try_Value as maxtrycount, B.regdate
                                                  FROM {0} AS A WITH(NOLOCK),
                                                        {1} AS B WITH(NOLOCK),
                                                        {2} AS C WITH(NOLOCK)
                                                      WHERE A.DungeonID = B.dungeonid AND A.DungeonID = C.Dark_DungeonID AND B.aid = {3} 
                                                      ORDER BY  A.DungeonID ASC
                                                    ", Dungeon_Define.Dark_Passage_TableName, Dungeon_Define.Dark_Passage_Play_TableName
                                                     , Dungeon_Define.Mission_World_TableName, AID );

            return GenericFetch.FetchFromDB_MultipleRow<User_GuerrillaDungeon_Play>(ref TB, setQuery, Dungeon_Define.Dungeon_Info_DB);            
        }

        public static List<User_EliteDungeon_Play> GetUser_All_EliteDungeonInfo(ref TxnBlock TB, long AID, string dbkey = Dungeon_Define.Dungeon_Info_DB)
        {
            string setQuery = string.Format(@"SELECT B.idx, ISNULL(B.aid, {2}) as aid, A.DungeonID as dungeonid, B.rank, B.clearcount, B.maxcount, B.regdate, B.lastdate 
                                                FROM {0} AS A WITH(NOLOCK) LEFT OUTER JOIN {1} AS B WITH(NOLOCK, INDEX(IDX_User_EliteDungeon_Play_AID_DungeonID)) ON A.DungeonID = B.dungeonid AND B.aid = {2} ", Dungeon_Define.Eliete_Dungeon_TableName, Dungeon_Define.EliteDungeon_Play_TableName, AID);
            return GenericFetch.FetchFromDB_MultipleRow<User_EliteDungeon_Play>(ref TB, setQuery, Dungeon_Define.Dungeon_Info_DB);
        }

        public static List<RetEliteDungeonRank> GetUser_All_EliteDungeonRank(ref TxnBlock TB, long AID, string dbkey = Dungeon_Define.Dungeon_Info_DB)
        {
            string setQuery = string.Format(@"SELECT B.Worldid as worldid , A.dungeonid, A.rank FROM {0} AS A WITH(NOLOCK, INDEX(IDX_User_EliteDungeon_Play_AID_DungeonID)) INNER JOIN {1} AS B WITH(NOLOCK) ON A.dungeonid = B.Elite_DungeonID1 WHERE A.aid = {2} ", Dungeon_Define.EliteDungeon_Play_TableName, Dungeon_Define.Mission_World_TableName, AID);
            List<RetEliteDungeonRank> retObj = GenericFetch.FetchFromDB_MultipleRow<RetEliteDungeonRank>(ref TB, setQuery, Dungeon_Define.Dungeon_Info_DB);
            setQuery = string.Format(@"SELECT B.Worldid as worldid , A.dungeonid, A.rank FROM {0} AS A WITH(NOLOCK, INDEX(IDX_User_EliteDungeon_Play_AID_DungeonID)) INNER JOIN {1} AS B WITH(NOLOCK) ON A.dungeonid = B.Elite_DungeonID2 WHERE A.aid = {2} ", Dungeon_Define.EliteDungeon_Play_TableName, Dungeon_Define.Mission_World_TableName, AID);
            List<RetEliteDungeonRank> retObj2 = GenericFetch.FetchFromDB_MultipleRow<RetEliteDungeonRank>(ref TB, setQuery, Dungeon_Define.Dungeon_Info_DB);

            retObj.AddRange(retObj2);
            return retObj;
        }


        public static System_Mission_Stage_Task GetSystem_Mission_Task_Info(ref TxnBlock TB, int TaskID)
        {
            if (TaskID > 0)
            {
                string setKey = string.Format("{0}_{1}", Dungeon_Define.Mission_Task_TableName, Dungeon_Define.Mission_Task_Surfix);
                string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE TaskID = {1}", Dungeon_Define.Mission_Task_TableName, TaskID);

                System_Mission_Stage_Task retObj = GenericFetch.FetchFromRedis_Hash<System_Mission_Stage_Task>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, TaskID.ToString(), setQuery, Dungeon_Define.Dungeon_Info_DB);
                return (retObj != null) ? retObj : new System_Mission_Stage_Task();
            }

            return new System_Mission_Stage_Task();
        }

        public static RetTaskResult CheckTaskStatus(ref TxnBlock TB, int TaskID, int beforeTaskValue, int UserTaskValue, string isClear)
        {
            RetTaskResult retObj = new RetTaskResult(isClear, beforeTaskValue);

            if (isClear.Equals("Y"))
                return retObj;

            System_Mission_Stage_Task taskInfo = Dungeon_Manager.GetSystem_Mission_Task_Info(ref TB, TaskID);
            Dungeon_Define.eTaskCheckType taskType = Dungeon_Define.Dungeon_Task_TypeList[taskInfo.TaskType];
            int checkValue = taskInfo.value3;

            switch (taskType)
            {
                case Dungeon_Define.eTaskCheckType.Combo:
                case Dungeon_Define.eTaskCheckType.HP1:
                    {
                        retObj.taskValue = checkValue > UserTaskValue ? UserTaskValue : checkValue;
                        retObj.taskClear = checkValue > UserTaskValue ? "N" : "Y";
                    }
                    break;
                case Dungeon_Define.eTaskCheckType.Die:
                case Dungeon_Define.eTaskCheckType.HP2:
                case Dungeon_Define.eTaskCheckType.Time1:
                case Dungeon_Define.eTaskCheckType.Time2:
                    {
                        retObj.taskValue = checkValue < UserTaskValue ? UserTaskValue : checkValue;
                        retObj.taskClear = checkValue < UserTaskValue ? "N" : "Y";
                    }
                    break;
                case Dungeon_Define.eTaskCheckType.Item:
                    {
                        checkValue = 0;
                        retObj.taskValue = checkValue < UserTaskValue ? UserTaskValue : checkValue;
                        retObj.taskClear = checkValue < UserTaskValue ? "Y" : "N";
                    }
                    break;
                case Dungeon_Define.eTaskCheckType.Kill:
                    {
                        UserTaskValue = UserTaskValue + beforeTaskValue;
                        retObj.taskValue = checkValue > UserTaskValue ? UserTaskValue : checkValue;
                        retObj.taskClear = checkValue > UserTaskValue ? "N" : "Y";
                    }
                    break;
                case Dungeon_Define.eTaskCheckType.Skill2:
                    {
                        retObj.taskValue = 0;
                        retObj.taskClear = checkValue < UserTaskValue ? "N" : "Y";
                    }
                    break;
            }

            if (retObj.taskClear.ToUpper().Equals("Y"))
            {
                retObj.taskGold = (taskInfo.Rewardtype.ToLower().Equals("gold")) ? taskInfo.Rewardvalue : 0;
                retObj.taskExp = (taskInfo.Rewardtype.ToLower().Equals("exp")) ? taskInfo.Rewardvalue : 0;
                retObj.taskRuby = (taskInfo.Rewardtype.ToLower().Equals("cash")) ? taskInfo.Rewardvalue : 0;
            }

            return retObj;

        }

        public static Result_Define.eResult UpdateMissionTask(ref TxnBlock TB, ref User_Mission_Play userMissionInfo, int clearCount = 1, string dbkey = Dungeon_Define.Dungeon_Info_DB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET 
                                                    [rank] = {3},
                                                    task1 = '{4}',
                                                    task1value = {5},
                                                    task2 = '{6}',
                                                    task2value = {7},
                                                    task3 = '{8}',
                                                    task3value = {9},
                                                    ClearTime = {10},
                                                    ChallengeCnt = CASE WHEN regdate != CONVERT(varchar(10),getdate(),21) THEN {11} ELSE ChallengeCnt + {11} END,
                                                    ChallengeReset = CASE WHEN regdate != CONVERT(varchar(10),getdate(),21) THEN 0 ELSE ChallengeReset END,
                                                    regdate = GETDATE()
                                                WHERE aid = {1} AND stageid = {2} ",
                                                    Dungeon_Define.Mission_Play_TableName
                                                    , userMissionInfo.aid
                                                    , userMissionInfo.stageid
                                                    , userMissionInfo.rank
                                                    , userMissionInfo.task1
                                                    , userMissionInfo.task1value
                                                    , userMissionInfo.task2
                                                    , userMissionInfo.task2value
                                                    , userMissionInfo.task3
                                                    , userMissionInfo.task3value
                                                    , userMissionInfo.ClearTime
                                                    , clearCount
                                                    );
            if (TB.ExcuteSqlCommand(dbkey, setQuery))
                return Result_Define.eResult.SUCCESS;
            else
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult UpdateDarkPassge(ref TxnBlock TB, ref User_GuerrillaDungeon_Play userPlayInfo, int clearCount = 1, string dbkey = Dungeon_Define.Dungeon_Info_DB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET 
                                                    [rank] = {3},
                                                    cleartime = {4},
                                                    challengecount = CASE WHEN regdate != CONVERT(varchar(10),getdate(),21) THEN {5} ELSE challengecount + {5} END,
                                                    challengereset = CASE WHEN regdate != CONVERT(varchar(10),getdate(),21) THEN 0 ELSE challengereset END,
                                                    regdate = GETDATE()
                                                WHERE aid = {1} AND dungeonid = {2} ",
                                                    Dungeon_Define.Dark_Passage_Play_TableName
                                                    , userPlayInfo.aid
                                                    , userPlayInfo.dungeonid
                                                    , userPlayInfo.rank
                                                    , userPlayInfo.cleartime
                                                    , clearCount
                                                    );
            if (TB.ExcuteSqlCommand(dbkey, setQuery))
                return Result_Define.eResult.SUCCESS;
            else
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult UpdateEliteDungeon(ref TxnBlock TB, ref User_EliteDungeon_Play userPlayInfo, int clearCount = 1, string dbkey = Dungeon_Define.Dungeon_Info_DB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET 
                                                    [rank] = {3},
                                                    clearcount = CASE WHEN regdate != CONVERT(varchar(10),getdate(),21) THEN {5} ELSE clearcount + {5} END,
                                                    resetcount = CASE WHEN regdate != CONVERT(varchar(10),getdate(),21) THEN 0 ELSE resetcount END,
                                                    regdate = GETDATE(),
                                                    lastdate = GETDATE()
                                                WHERE idx = {4} AND aid = {1} AND dungeonid = {2} ",
                                                    Dungeon_Define.EliteDungeon_Play_TableName
                                                    , userPlayInfo.aid
                                                    , userPlayInfo.dungeonid
                                                    , userPlayInfo.rank
                                                    , userPlayInfo.idx
                                                    , clearCount
                                                    //, userPlayInfo.resetcount     // not use yet
                                                    );
            if (TB.ExcuteSqlCommand(dbkey, setQuery))
                return Result_Define.eResult.SUCCESS;
            else
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult ResetPvEPlayCount(ref TxnBlock TB, long AID, Dungeon_Define.eDungeonBoostType pveType, long worldid = 0, long stageid = 0, long dungeonid = 0, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setQuery = string.Empty;
            if (pveType == Dungeon_Define.eDungeonBoostType.ContentType_Senario)
            {
                setQuery = string.Format(@"
                                                UPDATE {0} SET 
                                                    ChallengeCnt = 0,
                                                    ChallengeReset = CASE WHEN regdate != CONVERT(varchar(10),getdate(),21) THEN 1 ELSE ChallengeReset + 1 END,
                                                    regdate = GETDATE()
                                                WHERE aid = {1} AND stageid = {2}
                                            ", Dungeon_Define.Mission_Play_TableName,
                                                    AID,
                                                    stageid
                                                 );
            }
            else if (pveType == Dungeon_Define.eDungeonBoostType.ContentType_Guerilla)
            {
                setQuery = string.Format(@"
                                                UPDATE {0} SET 
                                                    challengecount = 0,
                                                    challengereset = CASE WHEN regdate != CONVERT(varchar(10),getdate(),21) THEN 1 ELSE challengereset + 1 END,
                                                    regdate = GETDATE()
                                                WHERE aid = {1} AND dungeonid = {2}
                                            ", Dungeon_Define.Dark_Passage_Play_TableName,
                                                    AID,
                                                    dungeonid
                                                 );
            }
            else if (pveType == Dungeon_Define.eDungeonBoostType.ContentType_Elite)
            {
                setQuery = string.Format(@"
                                                UPDATE {0} SET 
                                                    clearcount = 0,
                                                    resetcount = CASE WHEN regdate != CONVERT(varchar(10),getdate(),21) THEN 1 ELSE resetcount + 1 END,
                                                    regdate = GETDATE()
                                                WHERE aid = {1} AND dungeonid = {2}
                                            ", Dungeon_Define.EliteDungeon_Play_TableName,
                                                    AID,
                                                    dungeonid
                                                 );
            }
            else
                return Result_Define.eResult.VIP_DUNGEON_RESET_TYPE_INVALIDE;

            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

    }
}
