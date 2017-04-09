using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Tools;
using TheSoul.DataManager.Global;
using TheSoulGMTool.DBClass;

namespace TheSoulGMTool.lib
{
    public partial class GMCheatManager
    {
        public static Result_Define.eResult SetGuildSkillTime(ref TxnBlock TB, long gid, string dbkey = GMData_Define.CommonDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            string setQuery = string.Format(@"UPDATE A SET A.YesterdayAttendCheck = B.cntUser FROM {0} A Join 
                                                                (Select COUNT(*) as cntUser, GuildID  From dbo.{1} Where GuildID = {2} Group by GuildID) as B
                                                                ON A.GuildID = B.GuildID", GuildManager.GuildCreationDBTableName, GuildManager.GuildJoinerDBTableName, gid);
            retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            if (retError == Result_Define.eResult.SUCCESS)
            {
                string setQuery2 = string.Format("Update {0} Set YesterdayAttendDate = DATEADD(DD,-1,TodayAttendDate), TodayAttendDate = NULL WHERE GuildID={1}", GuildManager.GuildJoinerDBTableName, gid);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery2) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            }

            return retError;
        }

        public static Result_Define.eResult SetGuildNoticeTime(ref TxnBlock TB, long gid, string dbkey = GMData_Define.CommonDBName)
        {
            string setQuery = string.Format("Update {0} Set GuildIntroduceModifyDate = NULL, GuildNoticeModifyDate = NULL WHERE GuildID={1}", GuildManager.GuildCreationDBTableName, gid);            
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static Result_Define.eResult SetGuildEntrustTime(ref TxnBlock TB, long gid, string dbkey = GMData_Define.CommonDBName)
        {
            string setQuery = string.Format("Update {0} Set EntrustAskDate = NULL WHERE GuildID={1}", GuildManager.GuildCreationDBTableName, gid);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static Result_Define.eResult SetGuildJoinTime(ref TxnBlock TB, long AID, string dbkey = GMData_Define.CommonDBName)
        {
            string setQuery = string.Format("UPDATE {0} SET JoinerDeleteDate=NULL, JoinerbanishDate=NULL WHERE JoinerAID={1}", GuildManager.GuildJoinerDBTableName, AID);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static Result_Define.eResult SetGuildTodayAttend(ref TxnBlock TB, long gid, int count, string dbkey = GMData_Define.CommonDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            int AttendCount = 0;
            List<GuildJoiner> joinerList = GuildManager.GetGuildJoinerInfoList(ref TB, gid);
            foreach (GuildJoiner joiner in joinerList)
            {
                if (AttendCount < count)
                {
                    DateTime dt1 = DateTime.Now;
                    if (joiner.TodayAttendDate == null)
                    {
                        TimeSpan oneDay = new TimeSpan(1, 0, 0, 0);
                        DateTime tomorrow = dt1 + oneDay;

                        joiner.TodayAttendDate = tomorrow;
                    }
                    if (joiner.JoinerState == "S" && joiner.TodayAttendDate > dt1)
                    {
                        string setQuery = string.Format("UPDATE {0} SET TodayAttendDate=GETDATE() WHERE GuildID={1} AND JoinerState='S' AND JoinerAID={2}", GuildManager.GuildJoinerDBTableName, gid, joiner.JoinerAID);
                        retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retError == Result_Define.eResult.SUCCESS)
                            AttendCount = AttendCount + 1;
                        else
                            break;
                    }
                }
            }
            return retError;
        }

        public static Result_Define.eResult SetGuildYesterdayAttend(ref TxnBlock TB, long gid, int count, string dbkey = GMData_Define.CommonDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            int AttendCount = 0;
            List<GuildJoiner> joinerList = GuildManager.GetGuildJoinerInfoList(ref TB, gid);
            foreach (GuildJoiner joiner in joinerList)
            {
                if (AttendCount < count)
                {
                    if (joiner.JoinerState == "S")
                    {
                        string setQuery = string.Format("UPDATE {0} SET TodayAttendDate = DateAdd(D, -1, GETDATE()) WHERE GuildID={1} AND JoinerState='S' AND JoinerAID={2}", GuildManager.GuildJoinerDBTableName, gid, joiner.JoinerAID);

                        retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retError == Result_Define.eResult.SUCCESS)
                            AttendCount = AttendCount + 1;
                        else
                            break;
                    }
                }
            }
            return retError;
        }

        public static Result_Define.eResult SetGuildLevel(ref TxnBlock TB, long gid, int level, string dbkey = GMData_Define.CommonDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            long GuildLVUpExp = 0;
            for (int i = 1; i <= (level + 1); i++)
            {
                GuildLVUpExp = GuildLVUpExp + GuildManager.GetSystemGuildData(ref TB, i).NeedExp;
            }
            if (GuildLVUpExp > 0)
                GuildLVUpExp = GuildLVUpExp - 200;
            string setQuery = string.Format("Update {0} Set GuildLevel = {2}, GuildExp = {3}, GuildWithdrawExp = 0 Where GuildID={1}", GuildManager.GuildCreationDBTableName, gid, level, GuildLVUpExp);
            retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            if (retError == Result_Define.eResult.SUCCESS)
            {
                GuildManager.GetGuilData(ref TB, gid, true);
            }
            return retError;
        }

        public static Result_Define.eResult InitPvPResetCount(ref TxnBlock TB, long AID, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            string setQuery = string.Format("Update {0} Set play_resetcount = 0 Where Aid={1}", PvP_Define.PvP_PlayInfo_TableName, AID);
            retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            if (retError == Result_Define.eResult.SUCCESS)
            {
                foreach (PvP_Define.ePvPType item in Enum.GetValues(typeof(PvP_Define.ePvPType)))
                {
                    PvPManager.GetUser_PvPInfo(ref TB, AID, item, true);
                }
            }
            return retError;
        }

        public static Result_Define.eResult InitPvPPlayCount(ref TxnBlock TB, long AID, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            string setQuery = string.Format("Update {0} Set play_count = 0 Where Aid={1}", PvP_Define.PvP_PlayInfo_TableName, AID);
            retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            if (retError == Result_Define.eResult.SUCCESS)
            {
                foreach (PvP_Define.ePvPType item in Enum.GetValues(typeof(PvP_Define.ePvPType)))
                {
                    PvPManager.GetUser_PvPInfo(ref TB, AID, item, true);
                }
            }
            return retError;
        }

        public static Result_Define.eResult InitMission(ref TxnBlock TB, long AID, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            string setQuery = string.Format("Delete From {0} Where AID = {1}", Dungeon_Define.Mission_Play_TableName, AID); 
            retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            if (retError == Result_Define.eResult.SUCCESS)
            {
                setQuery = string.Format("Delete From {0} Where AID = {1}", Dungeon_Define.World_Rank_Reward_TableName, AID);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            }
            return retError;
        }

        //public static Result_Define.eResult InitPvPPlayCount(ref TxnBlock TB, long AID, string dbkey = GMData_Define.ShardingDBName)
        //{
        //    Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
        //    string setQuery = string.Format("Update {0} Set challengereset = 0 Where Aid={1}", Dungeon_Define.Dark_Passage_Play_TableName, AID);
        //    retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        //    if (retError == Result_Define.eResult.SUCCESS)
        //    {
        //        List<User_GuerrillaDungeon_Play> list = Dungeon_Manager.GetUser_All_GuerrillaDungeonRank(ref TB, AID);
        //        foreach (User_GuerrillaDungeon_Play data in list)
        //        {
        //            Dungeon_Manager.GetUser_DarkPassagePlayInfo(ref TB, ref retError, AID, data.dungeonid, true);
        //        }
        //    }
        //    return retError;
        //}
    }
}