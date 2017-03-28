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
using System.Globalization;

namespace TheSoul.DataManager
{
    public partial class PvPManager
    {
        public static User_PVP_Overlord_Ranking GetUser_Overlord_Ranking_Dummy_ByAID(ref TxnBlock TB, long AID, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE AID = {1}", PvP_Define.PvP_System_PvP_Overlord_Ranking_Dummy_TableName, AID);
            User_PVP_Overlord_Ranking retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<User_PVP_Overlord_Ranking>(ref TB, setQuery, dbkey);
            return retObj == null ? new User_PVP_Overlord_Ranking() : retObj;
        }

        public static User_PVP_Overlord_Ranking GetUser_Overlord_Ranking_ByAID(ref TxnBlock TB, long AID, byte isNPC = 0, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE AID = {1} AND isNPC = {2}", PvP_Define.PvP_User_PVP_Overlord_Ranking_TableName, AID, isNPC);
            User_PVP_Overlord_Ranking retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<User_PVP_Overlord_Ranking>(ref TB, setQuery, dbkey);
            if (retObj == null)
                retObj = new User_PVP_Overlord_Ranking();

            if (retObj.update_date.AddMinutes(PvP_Define.Overlord_Play_Time_Min) < DateTime.Now)
                retObj.Flag = (int)PvP_Define.ePvPPlayFlag.None;

            return retObj;
        }

        public static User_PVP_Overlord_Ranking InsertPvP_OverlordRanking(ref TxnBlock TB, long AID, byte isNPC = 0, string dbkey = PvP_Define.PvP_Info_DB)
        {
            User_PVP_Overlord_Ranking retObj = new User_PVP_Overlord_Ranking();

            SqlCommand commandUserOverlordRanking = new SqlCommand();
            commandUserOverlordRanking.CommandText = "GetUser_PvP_OverlordRanking";
            commandUserOverlordRanking.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            commandUserOverlordRanking.Parameters.Add("@isNPC", SqlDbType.TinyInt).Value = isNPC;

            SqlDataReader getDr = null;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandUserOverlordRanking, ref getDr))
            {
                if (getDr != null)
                {
                    var r = SQLtoJson.Serialize(ref getDr);
                    string json = mJsonSerializer.ToJsonString(r);
                    getDr.Dispose(); getDr.Close();
                    commandUserOverlordRanking.Dispose();

                    User_PVP_Overlord_Ranking[] retSet = mJsonSerializer.JsonToObject<User_PVP_Overlord_Ranking[]>(json);

                    if (retSet.Length > 0)
                        retObj = retSet[0];
                }
            }
            else
            {
                commandUserOverlordRanking.Dispose();
            }

            if (retObj.update_date.AddMinutes(PvP_Define.Overlord_Play_Time_Min) < DateTime.Now)
                retObj.Flag = (int) PvP_Define.ePvPPlayFlag.None;
            
            return retObj;
        }

        public static Result_Define.eResult SetUser_Overlord_Ranking(ref TxnBlock TB, User_PVP_Overlord_Ranking setInfo, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET 
                                                    AID = {2},
                                                    Flag = {3},
                                                    isNPC = {4},
                                                    update_date = GETDATE()
                                                WHERE Ranking = {1}",
                                        PvP_Define.PvP_User_PVP_Overlord_Ranking_TableName
                                        , setInfo.Ranking
                                        , setInfo.AID
                                        , setInfo.Flag
                                        , setInfo.isNPC
                                        );
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        private static User_PVP_Overlord_ReadRecord GetUser_Overlord_ReadRecord(ref TxnBlock TB, long AID, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE AID = {1}", PvP_Define.PvP_User_PVP_Overlord_ReadRecord_TableName, AID);
            User_PVP_Overlord_ReadRecord retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<User_PVP_Overlord_ReadRecord>(ref TB, setQuery, dbkey);
            if (retObj == null)
                retObj = new User_PVP_Overlord_ReadRecord();
            return retObj;
        }


        private static Result_Define.eResult SetUser_Overlord_ReadRecord(ref TxnBlock TB, long AID, long setIndex, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON aid = @aid
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                    rec_idx = @recidx
                                                WHEN NOT MATCHED THEN
                                                   INSERT ([aid], [rec_idx])
                                                   VALUES (@aid, @recidx);
                                    ", PvP_Define.PvP_User_PVP_Overlord_ReadRecord_TableName);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = setQuery;
            cmd.Parameters.AddWithValue("@aid", AID);
            cmd.Parameters.AddWithValue("@recidx", setIndex);
            return TB.ExcuteSqlCommand(dbkey, ref cmd) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

        }

        public static List<User_PVP_Overlord_Record> GetUser_PVP_Overlord_Record_Info(ref TxnBlock TB, long AID, ref List<User_PVP_Overlord_Record> retList, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE AID = {1}", PvP_Define.PvP_User_PVP_Overlord_Record_TableName, AID);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_PVP_Overlord_Record>(ref TB, setQuery, dbkey);
        }

        public static Result_Define.eResult GetUser_PVP_Overlord_Record(ref TxnBlock TB, long AID, ref List<User_PVP_Overlord_Record> retList, string dbkey = PvP_Define.PvP_Info_DB)
        {
            User_PVP_Overlord_ReadRecord readInfo = PvPManager.GetUser_Overlord_ReadRecord(ref TB, AID);

            string setQuery = string.Format("SELECT TOP {3} * FROM {0} WITH(NOLOCK)  WHERE AID = {1} AND rec_idx > {2} ORDER BY rec_idx DESC", 
                                            PvP_Define.PvP_User_PVP_Overlord_Record_TableName, AID, readInfo.rec_idx, PvP_Define.Overlord_Play_List_Max);
            List<User_PVP_Overlord_Record> setList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_PVP_Overlord_Record>(ref TB, setQuery, dbkey);
            if (setList.Count > 0)
            {
                foreach (User_PVP_Overlord_Record setInfo in setList)
                {
                    User_PVP_Overlord_Ranking setUserRanking = PvPManager.GetUser_Overlord_Ranking_ByAID(ref TB, setInfo.challengerAID);
                    Account_Simple_With_Character setUserInfo = AccountManager.GetSimpleAccountCharacterInfo(ref TB, setInfo.challengerAID);
                    User_PVP_Overlord_Record setObj = new User_PVP_Overlord_Record();
                    setObj.AID = setInfo.AID;
                    setObj.rec_idx = setInfo.rec_idx;
                    setObj.win = setInfo.win;
                    setObj.beforeRank = setInfo.beforeRank;
                    setObj.afterRank = setInfo.afterRank;
                    setObj.challengerAID = setUserInfo.aid;
                    setObj.challengerUsername = setUserInfo.username;
                    setObj.challengerRank = setUserRanking.Ranking;
                    setObj.challengerClasstype = setUserInfo.charinfo.Class;
                    setObj.challengerLevel = setUserInfo.charinfo.level;
                    retList.Add(setObj);
                }

                retList = retList.OrderByDescending(setItem => setItem.rec_idx).ToList();

                return PvPManager.SetUser_Overlord_ReadRecord(ref TB, AID, retList.Max(recinfo => recinfo.rec_idx));
            }else
                return Result_Define.eResult.SUCCESS; ;
        }

        public static User_PVP_Overlord_Ranking GetUser_Overlord_Ranking(ref TxnBlock TB, long Ranking, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Ranking = {1}", PvP_Define.PvP_User_PVP_Overlord_Ranking_TableName, Ranking);
            User_PVP_Overlord_Ranking retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<User_PVP_Overlord_Ranking>(ref TB, setQuery, dbkey);
            if (retObj == null)
                retObj = new User_PVP_Overlord_Ranking();

            if (retObj.update_date.AddMinutes(PvP_Define.Overlord_Play_Time_Min) < DateTime.Now)
                retObj.Flag = (int)PvP_Define.ePvPPlayFlag.None;

            return retObj;
        }

        public static User_PVP_Overlord_Ranking GetSystem_PvP_Overlord_Ranking_Dummy(ref TxnBlock TB, long Ranking, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Ranking = {1}", PvP_Define.PvP_System_PvP_Overlord_Ranking_Dummy_TableName, Ranking);
            User_PVP_Overlord_Ranking retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<User_PVP_Overlord_Ranking>(ref TB, setQuery, dbkey);
            if (retObj == null)
                retObj = new User_PVP_Overlord_Ranking();
            return retObj == null ? new User_PVP_Overlord_Ranking() : retObj;
        }


        public static List<Ret_OverLord> GetUser_Overlord_Top_Ranking(ref TxnBlock TB, long Ranking = 100, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Ranking <= {1}", PvP_Define.PvP_User_PVP_Overlord_Ranking_TableName, Ranking);
            List<User_PVP_Overlord_Ranking> retList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_PVP_Overlord_Ranking>(ref TB, setQuery, dbkey);

            List<Ret_OverLord> RankInfoList = new List<Ret_OverLord>();
            foreach (User_PVP_Overlord_Ranking getRankingInfo in retList)
            {
                Ret_OverLord setObj = new Ret_OverLord();

                getRankingInfo.AID = getRankingInfo.isNPC > 0 ? getRankingInfo.AID % PvP_Define.Overlord_NPC_Seperator : getRankingInfo.AID;
                Account_Simple_With_Character userInfo = getRankingInfo.isNPC > 0 ? DummyManager.GetSimpleAccountCharacterInfo(ref TB, getRankingInfo.AID) : AccountManager.GetSimpleAccountCharacterInfo(ref TB, getRankingInfo.AID);
                User_WarPoint userWarpoint = getRankingInfo.isNPC > 0 ? DummyManager.GetUserWarPoint(ref TB, userInfo.aid) : AccountManager.GetUserWarPoint(ref TB, userInfo.aid);
                setObj.aid = userInfo.aid;
                setObj.username = userInfo.username;
                setObj.classtype = userInfo.charinfo.Class;
                setObj.level = userInfo.charinfo.level;
                setObj.isnpc = getRankingInfo.isNPC;
                setObj.rank = getRankingInfo.Ranking;
                setObj.warpoint = userWarpoint.WAR_POINT;
                setObj.playflag = getRankingInfo.Flag;
                RankInfoList.Add(setObj);
            }

            return RankInfoList;
        }

        public static long GetUser_Overlord_Ranking_TotalPlayer(ref TxnBlock TB, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setQuery = string.Format("SELECT COUNT(*) as count FROM {0}", PvP_Define.PvP_User_PVP_Overlord_Ranking_TableName);
            Rank_Count retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<Rank_Count>(ref TB, setQuery, dbkey);
            return (retObj == null) ? 0 : retObj.count;
        }

        public static Result_Define.eResult MakeMatchingOverlordEnemyInfo(ref TxnBlock TB, long AID, long UserRank, ref List<Ret_OverLord> RankInfoList, out long totalPlayer, string dbkey = PvP_Define.PvP_Info_DB)
        {
            totalPlayer = PvPManager.GetUser_Overlord_Ranking_TotalPlayer(ref TB);
            double baseMatch = 0; // (matchingVal / GoldExpedition_Define.PercentageDivede) + 1) - GoldExpedition
            //List<int> RankList = new List<int>();
            int findNext = 0;
            int totalEnemyCount = PvP_Define.GetEnemyRangeList.Sum(enemyInfo => enemyInfo.getCount);

            bool trySorted = true;
            int ListTryCount = 0;
            long RankJumpRate = UserRank < PvP_Define.MinimumJumpRank ? PvP_Define.MinimumJumpRank : UserRank;
            List<int> RankList = new List<int>();

            if (UserRank == 0)  // first overlord play
            {
                int tryCount = 0;
                int findCount = 0;
                int findTotalCount = PvP_Define.GetEnemyRangeList.Sum(targetInfo => targetInfo.getCount);
                while (findCount < findTotalCount && tryCount < 100)
                {
                    int findRank = TheSoul.DataManager.Math.GetRandomInt(1, PvP_Define.PvP_Overlord_Ranking_Dummy_Count);
                    if (!RankList.Contains(findRank) && findRank != UserRank)
                    {
                        RankList.Add(findRank);
                        findCount++;
                        tryCount = 0;
                    }
                    if (RankList.Count >= totalEnemyCount)
                        break;
                    tryCount++;
                }

            }
            else if (UserRank > 20)
            {
                while (RankList.Count < totalEnemyCount && ListTryCount < 4)
                {
                    List<PvP_Define.FindEnemyRange> setList = trySorted ? PvP_Define.GetEnemyRangeList : PvP_Define.GetEnemyRangeReverseList;

                    foreach (PvP_Define.FindEnemyRange setEnemyRange in setList)
                    {
                        double setMin = ((baseMatch + (setEnemyRange.minRate / GoldExpedition_Define.PercentageDivede)) * RankJumpRate);
                        double setMax = ((baseMatch + (setEnemyRange.maxRate / GoldExpedition_Define.PercentageDivede)) * RankJumpRate);
                        setMin += UserRank;
                        setMax += UserRank;
                        setMin = setMin < 1 ? 1 : setMin;
                        setMax = setMax > totalPlayer ? totalPlayer : setMax;

                        bool bDoFindNext = (setMin < PvP_Define.RankingTop1 || setMax < PvP_Define.RankingTop1) || (setMin > totalPlayer || setMax > totalPlayer);
                        // System.Math.Abs(setMax - setMin) < setEnemyRange.getCount
                        if (!bDoFindNext)
                        {
                            int tryCount = 0;
                            int findCount = 0;
                            while (findCount < setEnemyRange.getCount + findNext && tryCount < 100)
                            {
                                int findRank = TheSoul.DataManager.Math.GetRandomInt((int)setMin, (int)setMax);
                                if (!RankList.Contains(findRank) && findRank != UserRank)
                                {
                                    RankList.Add(findRank);
                                    findCount++;
                                    tryCount = 0;
                                }
                                if (RankList.Count >= totalEnemyCount)
                                    break;
                                tryCount++;
                            }
                            findNext += (setEnemyRange.getCount - findCount);
                        }
                        else
                            findNext += setEnemyRange.getCount;

                        if (RankList.Count >= totalEnemyCount)
                            break;
                    }
                    trySorted = !trySorted;
                    ListTryCount++;
                }
            }
            else            // high ranker exception progress
            {
                long UpperCount = PvP_Define.GetEnemyRangeList.FindAll(ranknum => ranknum.minRate < 0 && ranknum.maxRate < 0).Sum(item => item.getCount);
                long LowerCount = PvP_Define.GetEnemyRangeList.FindAll(ranknum => ranknum.minRate >= 0 && ranknum.maxRate >= 0).Sum(item => item.getCount);
                if (UpperCount > UserRank)
                {
                    LowerCount += (UpperCount - UserRank - 1);
                    UpperCount = UserRank - 1;
                }

                if (UpperCount + LowerCount < totalEnemyCount)
                    LowerCount = totalEnemyCount - UpperCount;

                long LowerRank = (long)((UserRank + LowerCount) * 1.5);

                while (RankList.Count < totalEnemyCount && ListTryCount < 4)
                {
                    int tryCount = 0;
                    int findCount = 0;
                    while (findCount < UpperCount && tryCount < 100)
                    {
                        int findRank = TheSoul.DataManager.Math.GetRandomInt((int)PvP_Define.RankingTop1, (int)UserRank);
                        if (!RankList.Contains(findRank) && findRank != UserRank)
                        {
                            RankList.Add(findRank);
                            findCount++;
                            tryCount = 0;
                        }
                        if (RankList.Count >= UpperCount)
                            break;
                        tryCount++;
                    }

                    tryCount = 0;
                    while (findCount < LowerCount && tryCount < 100)
                    {
                        int findRank = TheSoul.DataManager.Math.GetRandomInt((int)UserRank + 1, (int)LowerRank);
                        if (!RankList.Contains(findRank) && findRank != UserRank)
                        {
                            RankList.Add(findRank);
                            findCount++;
                            tryCount = 0;
                        }
                        if (RankList.Count >= totalEnemyCount)
                            break;
                        tryCount++;
                    }

                    ListTryCount++;
                }
            }

            RankList = RankList.OrderBy(rankInfo => rankInfo).ToList();

            if (UserRank == 0)  // first overlord play
            {
                foreach (long enemy_rank in RankList)
                {
                    Ret_OverLord setObj = new Ret_OverLord();
                    User_PVP_Overlord_Ranking getRankingInfo = PvPManager.GetSystem_PvP_Overlord_Ranking_Dummy(ref TB, enemy_rank);
                    if (getRankingInfo.AID < 1)
                        continue;
                    else
                    {
                        long getAID = getRankingInfo.isNPC > 0 ? getRankingInfo.AID % PvP_Define.Overlord_NPC_Seperator : getRankingInfo.AID;
                        Account_Simple_With_Character userInfo = getRankingInfo.isNPC > 0 ? DummyManager.GetSimpleAccountCharacterInfo(ref TB, getAID) : AccountManager.GetSimpleAccountCharacterInfo(ref TB, getAID);
                        if ((userInfo.aid == getAID && userInfo.cid > 0) || getRankingInfo.isNPC == 1)
                        {
                            User_WarPoint userWarpoint = getRankingInfo.isNPC > 0 ? DummyManager.GetUserWarPoint(ref TB, userInfo.aid) : AccountManager.GetUserWarPoint(ref TB, userInfo.aid);
                            setObj.aid = getRankingInfo.AID;
                            setObj.username = userInfo.username;
                            setObj.classtype = userInfo.charinfo.Class;
                            setObj.level = userInfo.charinfo.level;
                            setObj.isnpc = getRankingInfo.isNPC;
                            setObj.rank = enemy_rank;
                            setObj.warpoint = userWarpoint.WAR_POINT;
                            setObj.playflag = getRankingInfo.Flag;
                            setObj.rank += (totalPlayer + 1);
                            RankInfoList.Add(setObj);
                        }
                    }
                }
            }
            else
            {
                foreach (long enemy_rank in RankList)
                {
                    Ret_OverLord setObj = new Ret_OverLord();
                    User_PVP_Overlord_Ranking getRankingInfo = PvPManager.GetUser_Overlord_Ranking(ref TB, enemy_rank);
                    if (getRankingInfo.Ranking < 1 || getRankingInfo.AID < 1)
                        continue;
                    else
                    {
                        long getAID = getRankingInfo.isNPC > 0 ? getRankingInfo.AID % PvP_Define.Overlord_NPC_Seperator : getRankingInfo.AID;
                        Account_Simple_With_Character userInfo = getRankingInfo.isNPC > 0 ? DummyManager.GetSimpleAccountCharacterInfo(ref TB, getAID) : AccountManager.GetSimpleAccountCharacterInfo(ref TB, getAID);
                        if ((userInfo.aid == getAID && userInfo.cid > 0) || getRankingInfo.isNPC == 1)
                        {
                            User_WarPoint userWarpoint = getRankingInfo.isNPC > 0 ? DummyManager.GetUserWarPoint(ref TB, userInfo.aid) : AccountManager.GetUserWarPoint(ref TB, userInfo.aid);
                            setObj.aid = getRankingInfo.AID;
                            setObj.username = userInfo.username;
                            setObj.classtype = userInfo.charinfo.Class;
                            setObj.level = userInfo.charinfo.level;
                            setObj.isnpc = getRankingInfo.isNPC;
                            setObj.rank = enemy_rank;
                            setObj.warpoint = userWarpoint.WAR_POINT;
                            setObj.playflag = getRankingInfo.Flag;
                            RankInfoList.Add(setObj);
                        }
                    }
                }
            }

            return Result_Define.eResult.SUCCESS;
        }

        public static Result_Define.eResult InsertUser_PvP_Overlord_PlayRecord(ref TxnBlock TB, long AID, long beforeRank, long afterRank, long enemyAID, bool bWin, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setQuery = string.Format(@"INSERT INTO {0}    (AID, win, beforeRank, afterRank, challengerAID, updateDate) 
                                                        VALUES  (@AID,@win,@beforeRank,@afterRank,@challengerAID, GETDATE()) ", PvP_Define.PvP_User_PVP_Overlord_Record_TableName);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = setQuery;
            cmd.Parameters.AddWithValue("@AID", AID);
            cmd.Parameters.AddWithValue("@win", bWin ? 1 : 0);
            cmd.Parameters.AddWithValue("@beforeRank", beforeRank);
            cmd.Parameters.AddWithValue("@afterRank", afterRank);
            cmd.Parameters.AddWithValue("@challengerAID", enemyAID);
            return TB.ExcuteSqlCommand(dbkey, ref cmd) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }
    }
}
