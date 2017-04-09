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
using ServiceStack.Text;

namespace TheSoul.DataManager.DBClass
{
    public class BossRaidDetail_Info
    {
        public BossRaidCreation CreaterInfo;
        public List<BossRaidJoiner> JoinerInfo_List;
    }


    public class ActiveBossRaid_Info
    {
        public DateTime CurrentDate { get; set; }
        public List<BossRaidCreation> BossList { get; set; }
    }

    public class BossRaidCreation
    {
        public long BossRaidID { get; set; }
        public long DungeonID { get; set; }
        public long NpcID { get; set; }
        public int BossLevel { get; set; }
        public long HP { get; set; }
        public long? RemainHP { get; set; }
        public long CreaterAID { get; set; }
        public string CreaterNick { get; set; }
        public long? KillerAID { get; set; }
        public string KillerNick { get; set; }
        public int PublicChnnel { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? PublicDate { get; set; }
        public string Status { get; set; }
        public DateTime? BossDeadDate { get; set; }
        public string DoReward { get; set; }
        public int RemainSec { get; set; }
    }

    public class BossRaidCreationInfo : BossRaidCreation
    {
        public long MaxDamageAID { get; set; }
        public int MaxDamage { get; set; }
    }

    public class BossRaidJoinerList
    {
        public long bossraidid { get; set; }
        public long dungeonid { get; set; }
        public long maxhp { get; set; }
        public long remainhp { get; set; }
        public long createraid { get; set; }
        public string creaternick { get; set; }
        public int remainsec { get; set; }
        public string status { get; set; }
        public string getreward { get; set; }
        public string doreward { get; set; }
    }

    public class BossRaidJoiner
    {
        public long BossRaidID { get; set; }
        public long DungeonID { get; set; }
        public long MaxHP { get; set; }
        public long RemainHP { get; set; }
        public long JoinerAID { get; set; }
        public string JoinerNick { get; set; }
        public int JoinerClass { get; set; }
        public int JoinerLevel { get; set; }
        public long CreaterAID { get; set; }
        public string CreaterNick { get; set; }
        public long KillerAID { get; set; }
        public string KillerNick { get; set; }
        public int RemainSec { get; set; }
        public int RaidJoinTime { get; set; }
        public int RaidJoinedCnt { get; set; }
        public int Damage { get; set; }
        public int PublicChnnel { get; set; }
        public string Status { get; set; }
        public string GetReward { get; set; }
        public string DoReward { get; set; }
    }

    public class System_BOSS_RAID
    {
        public int DungeonID { get; set; }
        public string Description { get; set; }
        public int BossID { get; set; }
        public string Dungeon_Scene_name { get; set; }
        public string Prefab_name { get; set; }
        public byte Condition_PlayCoin { get; set; }
        public int Base_Reward_EXP { get; set; }
        public int Base_RandBox_DropBoxGroupId { get; set; }
        public int Add_RandBox_DropBoxGroupId { get; set; }
        public int Best1_Reward_Item_PC1 { get; set; }
        public int Item1_Grade_PC1 { get; set; }
        public int Best1_Reward_Item_PC2 { get; set; }
        public int Item1_Grade_PC2 { get; set; }
        public int Best1_Reward_Item_PC3 { get; set; }
        public int Item1_Grade_PC3 { get; set; }
        public int Best2_Reward_Item_PC1 { get; set; }
        public int Item2_Grade_PC1 { get; set; }
        public int Best2_Reward_Item_PC2 { get; set; }
        public int Item2_Grade_PC2 { get; set; }
        public int Best2_Reward_Item_PC3 { get; set; }
        public int Item2_Grade_PC3 { get; set; }
        public int Best3_Reward_Item_PC1 { get; set; }
        public int Item3_Grade_PC1 { get; set; }
        public int Best3_Reward_Item_PC2 { get; set; }
        public int Item3_Grade_PC2 { get; set; }
        public int Best3_Reward_Item_PC3 { get; set; }
        public int Item3_Grade_PC3 { get; set; }
        public string Boss_Atlas { get; set; }
        public string Boss_Image { get; set; }
        public byte Booster_Group_ID { get; set; }
    }
}

namespace TheSoul.DataManager
{
    public static class BossRaid_Define
    {
        public const int SetActiveAutoExpireTime_Sec = 180;
        public const int SetActiveAutoListOutTime_Hour = -24;

        public const string BossRaid_DB = "common";
        public const string BossRaidInfo_DB = "sharding";
        public const int BossRaid_WorldID_Min = 2;
        public const int BossRaid_Stage_Min = 10;

        // for tutorial
        public const short BossRaid_Tutorial_Stage_Max = 20;
        public const int BossRaid_Tutorial_Check_Step = 21100;

        // BossRaid System info
        public const string BossRaid_DBName = "sharding";

        public const string BossRaid_TableName = "System_BOSS_RAID";
        public const string BossRaid_System_CreationTable = "System_BossRaidCreation";
        public const string BossRaid_System_JoinerTableName = "System_BossRaidJoiner";

        public const string BossRaid_Prefix = "System_Boss_Raid";
        public const string BossRaid_Surfix = "Info";
        public const string BossRaid_ActiveList_Surfix = "Active_List";
        public const string BossRaid_PublicChannel_List = "BossRaid_PublicChannel_List";

        // BossRaid System DB Procedure name
        public const string BossRaid_DB_SP_Create_BossRaid = "System_Create_BossRaid";
        public const string BossRaid_DB_SP_Update_Joiner = "System_Update_Joiner";
        public const string BossRaid_DB_SP_Reward_Calc = "System_Reward_Calc";
        public const string BossRaid_DB_SP_Check_Public_Fail = "System_BossRaid_Public_Open";

        public enum eRaidConst
        {
            BOSSRAID_APPEAR_PROBABILITY = 1,
            BOSSRAID_MAX_TRYTIME,
            BOSSRAID_MAX_REWARDTIME,
            BOSSRAID_PUBLICTIME_MINUTE,
            DEF_BOSSRAID_FINDER_FISRTENTER,
            DEF_BOSSRAID_FINDER_ENTER,
            DEF_BOSSRAID_USER,
        }

        public static readonly Dictionary<eRaidConst, string> BossRaidConstKey = new Dictionary<eRaidConst, string>()
        {
            { eRaidConst.BOSSRAID_APPEAR_PROBABILITY, "BOSSRAID_APPEAR_PROBABILITY" },
            { eRaidConst.BOSSRAID_MAX_TRYTIME, "BOSSRAID_MAX_TRYTIME" },
            { eRaidConst.BOSSRAID_MAX_REWARDTIME, "BOSSRAID_MAX_REWARDTIME" },
            { eRaidConst.BOSSRAID_PUBLICTIME_MINUTE, "BOSSRAID_PUBLICTIME_MINUTE" },
            { eRaidConst.DEF_BOSSRAID_FINDER_FISRTENTER, "DEF_BOSSRAID_FINDER_FISRTENTER" },
            { eRaidConst.DEF_BOSSRAID_FINDER_ENTER, "DEF_BOSSRAID_FINDER_ENTER" },
            { eRaidConst.DEF_BOSSRAID_USER, "DEF_BOSSRAID_USER" },
        };

        public enum eRaidStatus
        {
            Active = 1,
            Clear,
            Fail,
            Error,
        };

        public static readonly Dictionary<eRaidStatus, string> BossRaidStatus = new Dictionary<eRaidStatus, string>()
        {
            { eRaidStatus.Active,   "I" },
            { eRaidStatus.Clear,    "C" },
            { eRaidStatus.Fail,     "F" },
            { eRaidStatus.Error,     "E" },
        };

        public enum eRaidReturnKeys
        {
            DungeonID = 1,
            RetJoinersCount,
            SNO_List,
            ActiveCount,
            BossList,
            BossRewardCount,
            BossRaidID,
            CreaterNick,
            Status,
            MaxHP,
            RemainHP,
            RemainSec,
            JoinerAID,
            JoinerNick,
            JoinerClass,
            JoinerLevel,
            RaidJoinTime,
            RaidJoinCount,
            JoinerDamage,
            RaidID,
            DetailInfo,
            RetKeyFillMax,
            RetHonorPoint,
            RetGold,
            PopupAchiveID,
            PopupAchiveList,
            EnterCost,
        }

        public static readonly Dictionary<eRaidReturnKeys, string> BossRaid_Ret_KeyList = new Dictionary<eRaidReturnKeys, string>()
        {
            { eRaidReturnKeys.DungeonID,            "dungeonid"         },
            { eRaidReturnKeys.RetJoinersCount,      "retjoinerscnt"     },
            { eRaidReturnKeys.SNO_List,             "snolist"           },
            { eRaidReturnKeys.ActiveCount,          "activecnt"         },
            { eRaidReturnKeys.BossList,             "bosslist"          },
            { eRaidReturnKeys.BossRewardCount,      "bossreward"        },
            { eRaidReturnKeys.BossRaidID,           "bossraidid"        },
            { eRaidReturnKeys.CreaterNick,          "creaternick"       },
            { eRaidReturnKeys.Status,               "status"            },
            { eRaidReturnKeys.MaxHP,                "maxhp"             },
            { eRaidReturnKeys.RemainHP,             "remainhp"          },
            { eRaidReturnKeys.RemainSec,            "remaintime"        },
            { eRaidReturnKeys.JoinerAID,            "joineraid"         },
            { eRaidReturnKeys.JoinerNick,           "joinernick"        },
            { eRaidReturnKeys.JoinerClass,          "joinerclass"       },
            { eRaidReturnKeys.JoinerLevel,          "joinerlevel"       },
            { eRaidReturnKeys.RaidJoinTime,         "raidjointime"      },
            { eRaidReturnKeys.RaidJoinCount,        "raidjoinedcnt"     },
            { eRaidReturnKeys.JoinerDamage,         "damage"            },
            { eRaidReturnKeys.RaidID,               "raidid"            },
            { eRaidReturnKeys.DetailInfo,           "raiddetail"        },
            { eRaidReturnKeys.RetKeyFillMax,        "retkeyfillmax"     },
            { eRaidReturnKeys.RetHonorPoint,        "honorpoint"        },
            { eRaidReturnKeys.RetGold,              "retgold"           },
            { eRaidReturnKeys.PopupAchiveID,        "popupachieveid"    },
            { eRaidReturnKeys.PopupAchiveList,      "popupachieveidlist"},            
            { eRaidReturnKeys.EnterCost,            "entercost"         },            
        };
    }

    public class BossRaid
    {
        public Result_Define.eResult ErrorCode = Result_Define.eResult.SUCCESS;
        public long setDungeonID = 0;

        private static string GetRediskey_ChatChannel()
        {
            //return string.Format("{0}", BossRaid_Define.BossRaid_PublicChannel_List);
            return BossRaid_Define.BossRaid_PublicChannel_List;
        }

        public static Result_Define.eResult SetChatChannel(List<int> chatlist)
        {
            string setKey = GetRediskey_ChatChannel();
            return (RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_System, setKey, chatlist)) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.REDIS_COMMAND_FAIL;
        }

        public static List<int> GetChatChannel()
        {
            string setKey = GetRediskey_ChatChannel();
            List<int> retList = GenericFetch.FetchFromOnly_Redis<List<int>>(DataManager_Define.RedisServerAlias_System, setKey);
            return retList == null? new List<int>() : retList;
        }

        public static BossRaidCreation GetBossRaidInfo(ref TxnBlock TB, long RaidID, bool Flush = false, string dbkey = BossRaid_Define.BossRaid_DB)
        {
            ActiveBossRaid_Info ActiveBossList = GetActiveBossRaid(ref TB, Flush);

            foreach (BossRaidCreation setItem in ActiveBossList.BossList)
            {
                if (setItem.BossRaidID == RaidID)
                    return setItem;
            }

            return new BossRaidCreation();
        }

        public static BossRaidCreation GetBossRaidInfoFromDB(ref TxnBlock TB, long RaidID, string dbkey = BossRaid_Define.BossRaid_DB)
        {
            string setQuery = string.Format("SELECT *, DATEDIFF(SS,GETDATE(), ExpireDate) AS RemainSec FROM {0} WITH(NOLOCK) WHERE BossRaidID = {1} ", BossRaid_Define.BossRaid_System_CreationTable, RaidID);
            return GenericFetch.FetchFromDB<BossRaidCreation>(ref TB, setQuery, dbkey);
        }

        public static BossRaidCreationInfo GetBossRaidInfoWithDamage(ref TxnBlock TB, long RaidID, string dbkey = BossRaid_Define.BossRaid_DB)
        {
            string setQuery = string.Format("SELECT TOP 1 A.*, DATEDIFF(SS,GETDATE(), ExpireDate) AS RemainSec, B.JoinerAID as MaxDamageAID, B.Damage as MaxDamage FROM {0} as A WITH(NOLOCK) JOIN {1} as B WITH(NOLOCK)  ON  A.BossRaidID = B.BossRaidID WHERE A.BossRaidID = {2} ORDER BY B.Damage DESC", BossRaid_Define.BossRaid_System_CreationTable, BossRaid_Define.BossRaid_System_JoinerTableName, RaidID);
            return GenericFetch.FetchFromDB<BossRaidCreationInfo>(ref TB, setQuery, dbkey);
        }

        public static ActiveBossRaid_Info GetActiveBossRaid(ref TxnBlock TB, bool Flush = false, string dbkey = BossRaid_Define.BossRaid_DB)
        {
            string setQuery = string.Format("SELECT *, DATEDIFF(SS,GETDATE(), ExpireDate) AS RemainSec FROM {0} WITH(NOLOCK, INDEX(IDX_System_BossRaidCreation_Active)) WHERE RemainHP > 0 AND ExpireDate > DATEADD(HOUR, {1}, GETDATE())"
                                                                    , BossRaid_Define.BossRaid_System_CreationTable, BossRaid_Define.SetActiveAutoListOutTime_Hour);
            ActiveBossRaid_Info ActiveBossList = new ActiveBossRaid_Info();
            ActiveBossList.BossList = GenericFetch.FetchFromDB_MultipleRow<BossRaidCreation>(ref TB, setQuery, dbkey);
            ActiveBossList.CurrentDate = DateTime.Now;
            return ActiveBossList;
        }

        public static BossRaidJoiner GetBossRaidJoinerInfo(ref TxnBlock TB, long AID, long raidid, bool Flush = false, string dbkey = BossRaid_Define.BossRaid_DB)
        {
            List<BossRaidJoiner> getRewardList = GetJoinerBossRaid(ref TB, AID, BossRaid_Define.eRaidStatus.Clear, Flush, dbkey);
            BossRaidJoiner getJoiner = getRewardList.Find(delegate(BossRaidJoiner tmp) { return (tmp.JoinerAID == AID && tmp.BossRaidID == raidid); });
            if (getJoiner == null)
                getJoiner = new BossRaidJoiner();

            return getJoiner;
        }

        public static long GetBossRaidCount(ref TxnBlock TB, long AID, BossRaid_Define.eRaidStatus StatusType, bool checkGetReward, bool Flush = false, string dbkey = BossRaid_Define.BossRaid_DB)
        {
            string setQuery = string.Format(@"
                                        SELECT COUNT(*) as count
 	                                        FROM 	{0} AS A WITH(NOLOCK) 
 	                                        JOIN  	{1} AS B WITH(NOLOCK) 
 	                                        ON	A.BossRaidID = B.BossRaidID 
                                        WHERE A.Status = '{2}' AND ExpireDate > GETDATE() AND B.JoinerAID = {3} AND B.GetReward = '{4}'",
                                                                                                                                        BossRaid_Define.BossRaid_System_CreationTable, BossRaid_Define.BossRaid_System_JoinerTableName, BossRaid_Define.BossRaidStatus[StatusType], AID, checkGetReward ? "N" : "Y");
            Rank_Count retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<Rank_Count>(ref TB, setQuery, dbkey);
            return (retObj == null) ? 0 : retObj.count;
        }


        public static List<BossRaidJoiner> GetRewardBossRaid(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = BossRaid_Define.BossRaid_DB)
        {
            return GetJoinerBossRaid(ref TB, AID, BossRaid_Define.eRaidStatus.Clear, Flush, dbkey);
        }

        public static List<BossRaidJoiner> GetFailBossRaid(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = BossRaid_Define.BossRaid_DB)
        {
            return GetJoinerBossRaid(ref TB, AID, BossRaid_Define.eRaidStatus.Fail, Flush, dbkey);
        }

        private static List<BossRaidJoiner> GetJoinerBossRaid(ref TxnBlock TB, long AID, BossRaid_Define.eRaidStatus StatusType, bool Flush = false, string dbkey = BossRaid_Define.BossRaid_DB)
        {
            string setQuery = string.Format(@"
                                        SELECT		B.BossRaidID, A.DungeonID, A.HP as MaxHP, A.RemainHP, B.JoinerAID, B.JoinerNick, B.JoinerClass, B.JoinerLevel, A.CreaterAID, A.CreaterNick, A.KillerAID, A.KillerNick, B.RaidJoinTIme, B.RaidJoinedCnt, B.Damage, A.PublicChnnel,
    		                                        DATEDIFF(SS,GETDATE(), A.ExpireDate) AS RemainSec, A.Status, B.GetReward, ISNULL(A.DoReward, 'N') AS DoReward
 	                                        FROM 	{0} AS A WITH(NOLOCK) 
 	                                        JOIN  	{1} AS B WITH(NOLOCK) 
 	                                        ON	A.BossRaidID = B.BossRaidID 
                                        WHERE A.Status = '{2}' AND ExpireDate > GETDATE() AND B.JoinerAID = {3};",
                                            BossRaid_Define.BossRaid_System_CreationTable, BossRaid_Define.BossRaid_System_JoinerTableName, BossRaid_Define.BossRaidStatus[StatusType], AID);
            //string setKey = string.Format("{0}_{1}_{2}_{3}", 
            //                                BossRaid_Define.BossRaid_Prefix, BossRaid_Define.BossRaid_ActiveList_Surfix,
            //                                BossRaid_Define.BossRaidStatus[StatusType], AID
            //                                );
            //List<BossRaidJoiner> JoinerList = GenericFetch.FetchFromRedis_MultipleRow<BossRaidJoiner>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush, BossRaid_Define.SetActiveAutoExpireTime_Sec);
            List<BossRaidJoiner> JoinerList = GenericFetch.FetchFromDB_MultipleRow<BossRaidJoiner>(ref TB, setQuery, dbkey);
            return JoinerList;
        }

        public static System_BOSS_RAID GetBossInfo(ref TxnBlock TB, int worldID, string dbkey = BossRaid_Define.BossRaidInfo_DB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", BossRaid_Define.BossRaid_Prefix, BossRaid_Define.BossRaid_Surfix);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE DungeonID = {1}", BossRaid_Define.BossRaid_TableName, worldID);
            System_BOSS_RAID retObj = GenericFetch.FetchFromRedis_Hash<System_BOSS_RAID>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, worldID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_BOSS_RAID();
            return retObj;
        }

        public void CreateBossRaid(ref TxnBlock tb, long AID, int StageID, string dbkey = BossRaid_Define.BossRaid_DB)
        {
            BossRaidCreation setBoss = new BossRaidCreation();
            CheckActiveBossRaid(ref tb, AID, StageID);
            if (ErrorCode == Result_Define.eResult.SUCCESS)
            {
                Account accInfo = AccountManager.GetAccountData(ref tb, AID, ref ErrorCode);
                int WorldID = GetMission_WordID(ref tb, StageID);

                if (WorldID < 1)
                {
                    ErrorCode = Result_Define.eResult.BOSSRAID_INVALID_STAGE;
                    return;
                }

                System_BOSS_RAID bossRaidInfo = BossRaid.GetBossInfo(ref tb, WorldID);

                if (bossRaidInfo.BossID > 0)
                {
                    System_NPC bossInfo = NPC_Manager.GetNPCInfo(ref tb, bossRaidInfo.BossID);
                    if (bossInfo.NPCID > 0)
                    {
                        Random rnd = new Random();
                        var channelList = BossRaid.GetChatChannel();
                        int setOpenChannel = channelList.Count() > 0 ? channelList.OrderBy(x => rnd.Next()).FirstOrDefault() : 1;
                        SqlCommand Cmd = new SqlCommand();
                        Cmd.CommandText = BossRaid_Define.BossRaid_DB_SP_Create_BossRaid;
                        Cmd.Parameters.Add("@creater_aid", SqlDbType.BigInt).Value = accInfo.AID;
                        Cmd.Parameters.Add("@creater_nickname", SqlDbType.NVarChar, 32).Value = accInfo.UserName;
                        Cmd.Parameters.Add("@openchannel", SqlDbType.Int).Value = setOpenChannel;
                        Cmd.Parameters.Add("@dungeon_id", SqlDbType.Int).Value = bossRaidInfo.DungeonID;
                        Cmd.Parameters.Add("@npc_id", SqlDbType.Int).Value = bossInfo.NPCID;
                        Cmd.Parameters.Add("@npc_level", SqlDbType.Int).Value = bossInfo.Level;
                        Cmd.Parameters.Add("@npc_hp", SqlDbType.BigInt).Value = bossInfo.HP;
                        float timeSet = (float)SystemData.GetConstValue(ref tb, BossRaid_Define.BossRaidConstKey[BossRaid_Define.eRaidConst.BOSSRAID_MAX_TRYTIME]);
                        timeSet = timeSet * 60; // Hour to Minute
                        Cmd.Parameters.Add("@expire_time", SqlDbType.Int).Value = System.Convert.ToInt32(timeSet);
                        timeSet = (float)SystemData.GetConstValue(ref tb, BossRaid_Define.BossRaidConstKey[BossRaid_Define.eRaidConst.BOSSRAID_PUBLICTIME_MINUTE]);
                        Cmd.Parameters.Add("@public_time", SqlDbType.Int).Value = System.Convert.ToInt32(timeSet);
                        Cmd.Parameters.Add("@status", SqlDbType.Char).Value = BossRaid_Define.BossRaidStatus[BossRaid_Define.eRaidStatus.Active];

                        SqlDataReader getDr = null;
                        tb.ExcuteSqlStoredProcedure(dbkey, ref Cmd, ref getDr);

                        if (getDr != null)
                        {
                            var r = SQLtoJson.Serialize(ref getDr);
                            string json = mJsonSerializer.ToJsonString(r);

                            getDr.Dispose(); getDr.Close();
                            BossRaidCreation[] BossList = mJsonSerializer.JsonToObject<BossRaidCreation[]>(json);

                            if (BossList.Length > 0)
                                setBoss = BossList[0];
                            else
                                ErrorCode = Result_Define.eResult.DB_ERROR;
                        }
                        else
                            ErrorCode = Result_Define.eResult.DB_ERROR;

                        Cmd.Dispose();
                    }
                    else
                        ErrorCode = Result_Define.eResult.BOSSRAID_ID_NOT_FOUND;
                }
                else
                    ErrorCode = Result_Define.eResult.BOSSRAID_ID_NOT_FOUND;
            }

            setDungeonID = setBoss.DungeonID;

            if (setBoss.BossRaidID == 0 && ErrorCode == Result_Define.eResult.SUCCESS)
                ErrorCode = Result_Define.eResult.BOSSRAID_ID_NOT_FOUND;
            else if (ErrorCode == Result_Define.eResult.SUCCESS)
            {
                BossRaid.CheckPublicRaid(ref tb);
                GetActiveBossRaid(ref tb, true);
            }
        }

        public int GetMission_WordID(ref TxnBlock tb, int StageID)
        {
            List<System_Mission_World> WorldList = Dungeon_Manager.GetSystem_MissionWorldList(ref tb);

            foreach (System_Mission_World checkWorld in WorldList)
            {
                List<int> checkDungeonIDList = new List<int>()
                {
                    checkWorld.Normal_DungeonID1,
                    checkWorld.Normal_DungeonID2,
                    checkWorld.Normal_DungeonID3,
                    checkWorld.Normal_DungeonID4,
                    checkWorld.Subboss_DungeonID,
                    checkWorld.Normal_DungeonID6,
                    checkWorld.Normal_DungeonID7,
                    checkWorld.Normal_DungeonID8,
                    checkWorld.Normal_DungeonID9,
                    checkWorld.Boss_DungeonID,
                    checkWorld.Dark_DungeonID,
                    checkWorld.Elite_DungeonID1,
                    checkWorld.Elite_DungeonID2,
                };

                if (checkDungeonIDList.Contains(StageID))
                    return checkWorld.WorldID;
            }

            return 0;
        }

        public void CheckActiveBossRaid(ref TxnBlock tb, long AID, int StageID)
        {
            string setQuery = string.Format(@"SELECT *, DATEDIFF(SS,GETDATE(), ExpireDate) AS RemainSec FROM {0} WITH(NOLOCK, INDEX(IDX_System_BossRaidCreation_AID_Date_Status)) 
		                                    	    WHERE CreaterAID = {1} AND [Status] = '{2}' AND RemainHP > 0
				                                            AND ExpireDate > GETDATE()",
                                                BossRaid_Define.BossRaid_System_CreationTable, AID,
                                                BossRaid_Define.BossRaidStatus[BossRaid_Define.eRaidStatus.Active]);

            BossRaidCreation bossRaidinfo = GenericFetch.FetchFromDB<BossRaidCreation>(ref tb, setQuery, BossRaid_Define.BossRaid_DB);

            if (bossRaidinfo != null)
                ErrorCode = Result_Define.eResult.BOSSRAID_ALREADY_OPEN;
            else
            {
                if (StageID > BossRaid_Define.BossRaid_Stage_Min && StageID <= BossRaid_Define.BossRaid_Tutorial_Stage_Max)
                {
                    Tutorial_Step userTutorialStep = AccountManager.Get_User_Tutorial(ref tb, AID);
                    if (!userTutorialStep.conditional_tutorial.Contains(BossRaid_Define.BossRaid_Tutorial_Check_Step))
                    {
                        if (AccountManager.GetAccountData(ref tb, AID, ref ErrorCode).Tutorial == 0)
                            return;
                    }
                }

                float checkRate = (float)SystemData.AdminConstValueFetchFromRedis(ref tb, BossRaid_Define.BossRaidConstKey[BossRaid_Define.eRaidConst.BOSSRAID_APPEAR_PROBABILITY]);
                if (Math.GetRandomDouble(0.0f, 100.0f) > checkRate)
                    ErrorCode = Result_Define.eResult.BOSSRAID_CREATE_RATE_CHECK_FAIL;
            }
        }

        public static BossRaidDetail_Info GetBossRaidDetail(ref TxnBlock tb, long RaidID)
        {
            BossRaidDetail_Info setDetail = new BossRaidDetail_Info();
            //string setQuery = string.Format("SELECT CreaterAID, CreaterNick, Status, HP, RemainHP, DATEDIFF(SS,GETDATE(), ExpireDate) as RemainSec, DungeonID FROM {0} WITH(NOLOCK) WHERE BossRaidID = {1}",
            //                                            BossRaid_Define.BossRaid_System_CreationTable, RaidID);
            //setDetail.CreaterInfo = GenericFetch.FetchFromDB<BossRaidCreation>(ref tb, setQuery, BossRaid_Define.BossRaid_DB);
            setDetail.CreaterInfo = GetBossRaidInfoFromDB(ref tb, RaidID);

            string setJoinerQuery = string.Format("SELECT JoinerAID, JoinerNick, JoinerClass, JoinerLevel, RaidJoinTime, RaidJoinedCnt, Damage	FROM {0} WITH(NOLOCK, INDEX(IDX_System_BossRaidJoiner_RaidID)) WHERE BossRaidID = {1}",
                                                                BossRaid_Define.BossRaid_System_JoinerTableName, RaidID);
            setDetail.JoinerInfo_List = GenericFetch.FetchFromDB_MultipleRow<BossRaidJoiner>(ref tb, setJoinerQuery, BossRaid_Define.BossRaid_DB);

            if (setDetail.CreaterInfo == null)
                setDetail.CreaterInfo = new BossRaidCreation();

            return setDetail;
        }

        public static string SetBossRaidResult(ref TxnBlock tb, BossRaidJoiner currentResult, string dbkey = BossRaid_Define.BossRaid_DB)
        {
            if (currentResult.BossRaidID > 0 && currentResult.JoinerAID > 0
                    && currentResult.JoinerClass > 0 && currentResult.JoinerLevel > 0
                /* && currentResult.RaidJoinTime > 0 && currentResult.Damage > 0 */
                    && !string.IsNullOrEmpty(currentResult.JoinerNick)
                )
            {
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = BossRaid_Define.BossRaid_DB_SP_Update_Joiner;
                Cmd.Parameters.Add("@BossRaidID", SqlDbType.BigInt).Value = currentResult.BossRaidID;
                Cmd.Parameters.Add("@JoinerAID", SqlDbType.BigInt).Value = currentResult.JoinerAID;
                Cmd.Parameters.Add("@JoinerNick", SqlDbType.NVarChar).Value = currentResult.JoinerNick;
                Cmd.Parameters.Add("@JoinerClass", SqlDbType.Int).Value = currentResult.JoinerClass;
                Cmd.Parameters.Add("@JoinerLevel", SqlDbType.Int).Value = currentResult.JoinerLevel;
                Cmd.Parameters.Add("@JoinTime", SqlDbType.Int).Value = currentResult.RaidJoinTime;
                Cmd.Parameters.Add("@Damage", SqlDbType.Int).Value = currentResult.Damage;
                var ResultBossFlag = new SqlParameter("@ResultBossFlag", SqlDbType.Char, 1) { Direction = ParameterDirection.Output };
                Cmd.Parameters.Add(ResultBossFlag);

                tb.ExcuteSqlStoredProcedure(dbkey, ref Cmd);
                Cmd.Dispose();
                return System.Convert.ToString(ResultBossFlag.Value);
            }
            else
                return BossRaid_Define.BossRaidStatus[BossRaid_Define.eRaidStatus.Error];
        }

        public static JsonArrayObjects GetBossRaidDetailJsonObj(ref TxnBlock TB, long AID, int RaidID, out int entercost)
        {
            BossRaidDetail_Info DetailInfo = BossRaid.GetBossRaidDetail(ref TB, RaidID);
            entercost = 0;

            if (SystemData.GetServiceArea(ref TB) != DataManager_Define.eCountryCode.China)
            {
                List<long> MyFriendAID_List = FriendManager.GetFriend_AID_List(ref TB, AID);
                MyFriendAID_List.Add(AID);

                if (DetailInfo.CreaterInfo.CreaterAID == AID)
                {
                    if (DetailInfo.JoinerInfo_List.Find(joiner => joiner.RaidJoinTime > 0 && AID == joiner.JoinerAID) != null)
                        entercost = SystemData.GetConstValueInt(ref TB, BossRaid_Define.BossRaidConstKey[BossRaid_Define.eRaidConst.DEF_BOSSRAID_FINDER_ENTER]);
                    else
                        entercost = SystemData.GetConstValueInt(ref TB, BossRaid_Define.BossRaidConstKey[BossRaid_Define.eRaidConst.DEF_BOSSRAID_FINDER_FISRTENTER]);
                }
                else
                    entercost = SystemData.GetConstValueInt(ref TB, BossRaid_Define.BossRaidConstKey[BossRaid_Define.eRaidConst.DEF_BOSSRAID_USER]);
            }
            // make json for client
            JsonArrayObjects retJson = new JsonArrayObjects();
            JsonObject createrInfo = new JsonObject();
            createrInfo = mJsonSerializer.AddJson(createrInfo, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.RaidID], RaidID.ToString());
            createrInfo = mJsonSerializer.AddJson(createrInfo, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.CreaterNick], DetailInfo.CreaterInfo.CreaterNick);
            createrInfo = mJsonSerializer.AddJson(createrInfo, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.Status], DetailInfo.CreaterInfo.Status);
            createrInfo = mJsonSerializer.AddJson(createrInfo, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.MaxHP], DetailInfo.CreaterInfo.HP.ToString());
            createrInfo = mJsonSerializer.AddJson(createrInfo, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.RemainHP], DetailInfo.CreaterInfo.RemainHP.ToString());
            createrInfo = mJsonSerializer.AddJson(createrInfo, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.RemainSec], DetailInfo.CreaterInfo.RemainSec.ToString());
            createrInfo = mJsonSerializer.AddJson(createrInfo, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.DungeonID], DetailInfo.CreaterInfo.DungeonID.ToString());

            retJson = mJsonSerializer.AddJsonArray(retJson, createrInfo);

            var vDamageSort = DetailInfo.JoinerInfo_List.OrderByDescending(item => item.Damage);

            foreach (BossRaidJoiner setJoiner in vDamageSort)
            {
                JsonObject joinerinfo = new JsonObject();
                joinerinfo = mJsonSerializer.AddJson(joinerinfo, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.JoinerAID], setJoiner.JoinerAID.ToString());
                joinerinfo = mJsonSerializer.AddJson(joinerinfo, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.JoinerNick], setJoiner.JoinerNick);
                joinerinfo = mJsonSerializer.AddJson(joinerinfo, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.JoinerClass], setJoiner.JoinerClass.ToString());
                joinerinfo = mJsonSerializer.AddJson(joinerinfo, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.JoinerLevel], setJoiner.JoinerLevel.ToString());
                joinerinfo = mJsonSerializer.AddJson(joinerinfo, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.RaidJoinTime], setJoiner.RaidJoinTime.ToString());
                joinerinfo = mJsonSerializer.AddJson(joinerinfo, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.RaidJoinCount], setJoiner.RaidJoinedCnt.ToString());
                joinerinfo = mJsonSerializer.AddJson(joinerinfo, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.JoinerDamage], setJoiner.Damage.ToString());
                retJson = mJsonSerializer.AddJsonArray(retJson, joinerinfo);
            }
            return retJson;
        }

        public static bool CheckPublicRaid(ref TxnBlock TB, string dbkey = BossRaid_Define.BossRaid_DB)
        {
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = BossRaid_Define.BossRaid_DB_SP_Check_Public_Fail;
            var ResultFlush = new SqlParameter("@ResultFlush", SqlDbType.Char, 1) { Direction = ParameterDirection.Output };
            Cmd.Parameters.Add(ResultFlush);

            TB.ExcuteSqlStoredProcedure(dbkey, ref Cmd, false);
            string retFlush = System.Convert.ToString(ResultFlush.Value);
            Cmd.Dispose();
            if (retFlush.Equals("Y"))
                return true;
            else
                return false;
        }

        public static Result_Define.eResult UpdateBossRaidReward(ref TxnBlock TB, long AID, long BossRaidID, string dbkey = BossRaid_Define.BossRaid_DB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET GetReward = 'Y' WHERE JoinerAID = {1} AND BossRaidID = {2} ",
                                                    BossRaid_Define.BossRaid_System_JoinerTableName, AID, BossRaidID);
            if (TB.ExcuteSqlCommand(dbkey, setQuery))
                return Result_Define.eResult.SUCCESS;
            else
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

        }
    }
}