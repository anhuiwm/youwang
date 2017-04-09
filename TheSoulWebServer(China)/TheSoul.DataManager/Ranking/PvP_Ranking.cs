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
        private const int StartSeasonMonth = 3;
        private static DateTime baseTime = DateTime.Parse("2015-03-01");

        public static int GetTodayTotalSeconds()
        {
            DateTime baseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            return (int)((DateTime.Now - baseTime).TotalSeconds);
        }

        public static DateTime GetBaseTime()
        {
            return baseTime;
        }

        public static int GetSeperater_Day()
        {
            return (int)DateTime.Today.Subtract(GetBaseTime()).TotalDays;
        }
        
        public static int DayOfWeekNumber()
        {
            var cal = new GregorianCalendar();
            return (int)cal.GetDayOfWeek(DateTime.Now);
        }

        public static int DayOfMonthNumber()
        {
            var cal = new GregorianCalendar();
            return (int)cal.GetDayOfMonth(DateTime.Now);            
        }

        public static int GetSeperater_Week()
        {
            return GetWeeks(GetBaseTime(), DateTime.Now);
        }

        public static int GetSeperater_Week(DateTime checkDay)
        {
            return GetWeeks(GetBaseTime(), checkDay);
        }

        /// <summary>
        /// Returns the number of weeks between two datetimes
        /// </summary>
        /// <param name="cal"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int GetWeeks( DateTime startDate, DateTime endDate, CalendarWeekRule weekRule = CalendarWeekRule.FirstDay,
                                        DayOfWeek dayofWeek = DayOfWeek.Monday)
        {
            var cal = new GregorianCalendar();

            int startWeekNumber = 0;
            int endWeekNumber = 0;
            int numberOfWeeksInRange = 0;

            if (startDate.Year == endDate.Year)
            {
                startWeekNumber = 0;
                endWeekNumber = 0;
                startWeekNumber = cal.GetWeekOfYear(startDate,
                                                    weekRule,
                                                    dayofWeek);

                endWeekNumber = cal.GetWeekOfYear(endDate,
                                                  weekRule,
                                                  dayofWeek);

                numberOfWeeksInRange = endWeekNumber - startWeekNumber;

            }
            else    // calculate per year, inclusive
            {
                for (int year = startDate.Year; year <= endDate.Year; year++)
                {
                    startWeekNumber = 0;
                    endWeekNumber = 0;

                    if (year == startDate.Year)
                    { // start date through the end of the year
                        startWeekNumber = cal.GetWeekOfYear(startDate, weekRule, dayofWeek);

                        endWeekNumber = cal.GetWeekOfYear((new DateTime(year + 1, 1, 1).AddDays(-1)),
                            weekRule, dayofWeek);
                    }
                    else if (year == endDate.Year)
                    { // start of the given year through the end date
                        startWeekNumber = cal.GetWeekOfYear((new DateTime(year, 1, 1)),
                            weekRule, dayofWeek);

                        endWeekNumber = cal.GetWeekOfYear(endDate,
                            weekRule, dayofWeek);
                    }
                    else
                    { // calculate the total number of weeks in this year                
                        startWeekNumber = cal.GetWeekOfYear(new DateTime(year, 1, 1),
                            weekRule, dayofWeek);

                        endWeekNumber = cal.GetWeekOfYear((new DateTime(year + 1, 1, 1).AddDays(-1)),
                            weekRule, dayofWeek);
                    }
                    numberOfWeeksInRange += endWeekNumber - startWeekNumber;
                }
            }

            return numberOfWeeksInRange;
        }

        public static int GetSeperater_Month()
        {
            return GetMonthsBetween(DateTime.Now, GetBaseTime());
        }

        private static int GetMonthsBetween(DateTime from, DateTime to)
        {
            if (from > to) return GetMonthsBetween(to, from);

            var monthDiff = System.Math.Abs((to.Year * 12 + (to.Month - 1)) - (from.Year * 12 + (from.Month - 1)));

            if (from.AddMonths(monthDiff) > to || to.Day < from.Day)
            {
                return monthDiff - 1;
            }
            else
            {
                return monthDiff;
            }
        }

        public static int GetSeperater_Season()
        {
            int getMonth = GetMonthsBetween(GetBaseTime().AddMonths(StartSeasonMonth), DateTime.Now) + 1;
            return (getMonth / 3) + 1;
        }

        public static int GetSeperater(PvP_Define.ePvPType PvPType)
        {
            if (PvPType == PvP_Define.ePvPType.MATCH_1VS1)
                return GetSeperater_Season();
            if (PvPType == PvP_Define.ePvPType.MATCH_GUILD_3VS3)
                return GetSeperater_Month();
            else
                return GetSeperater_Week();
        }

        public static int GetSeperater(ref TxnBlock TB, PvP_Define.ePvPType PvPType, string dbkey = PvP_Define.PvP_Info_DB)
        {
            if (PvPType == PvP_Define.ePvPType.MATCH_1VS1)
                return GetSeperater_Season(ref TB, dbkey);
            if (PvPType == PvP_Define.ePvPType.MATCH_GUILD_3VS3)
                return GetSeperater_Month(ref TB, dbkey);
            else
                return GetSeperater_Week(ref TB, dbkey);
        }

        public static int GetSeperater_Day(ref TxnBlock TB, string dbkey = PvP_Define.PvP_Info_DB)
        {
            SqlCommand commandUser_PvPInfo = new SqlCommand();
            commandUser_PvPInfo.CommandText = "PVP_GetSeperater_Daily";
            var outputResult = new SqlParameter("@RESULT", SqlDbType.Int) { Direction = ParameterDirection.Output };
            commandUser_PvPInfo.Parameters.Add(outputResult);
            int retValue = 0;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandUser_PvPInfo))
                retValue = System.Convert.ToInt32(outputResult.Value);

            commandUser_PvPInfo.Dispose();
            return retValue;
        }

        public static int GetSeperater_Week(ref TxnBlock TB, string dbkey = PvP_Define.PvP_Info_DB)
        {
            SqlCommand commandUser_PvPInfo = new SqlCommand();
            commandUser_PvPInfo.CommandText = "PVP_GetSeperater_Week";
            var outputResult = new SqlParameter("@RESULT", SqlDbType.Int) { Direction = ParameterDirection.Output };
            commandUser_PvPInfo.Parameters.Add(outputResult);
            int retValue = 0;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandUser_PvPInfo))
                retValue = System.Convert.ToInt32(outputResult.Value);

            commandUser_PvPInfo.Dispose();
            return retValue;
        }

        public static int GetSeperater_Month(ref TxnBlock TB, string dbkey = PvP_Define.PvP_Info_DB)
        {
            SqlCommand commandUser_PvPInfo = new SqlCommand();
            commandUser_PvPInfo.CommandText = "PVP_GetSeperater_Month";
            var outputResult = new SqlParameter("@RESULT", SqlDbType.Int) { Direction = ParameterDirection.Output };
            commandUser_PvPInfo.Parameters.Add(outputResult);
            int retValue = 0;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandUser_PvPInfo))
                retValue = System.Convert.ToInt32(outputResult.Value);

            commandUser_PvPInfo.Dispose();
            return retValue;
        }
        
        public static int GetSeperater_Season(ref TxnBlock TB, string dbkey = PvP_Define.PvP_Info_DB)
        {
            SqlCommand commandUser_PvPInfo = new SqlCommand();
            commandUser_PvPInfo.CommandText = "PVP_GetSeperater_Season";
            var outputResult = new SqlParameter("@RESULT", SqlDbType.Int) { Direction = ParameterDirection.Output };
            commandUser_PvPInfo.Parameters.Add(outputResult);
            int retValue = 0;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandUser_PvPInfo))
                retValue = System.Convert.ToInt32(outputResult.Value);

            commandUser_PvPInfo.Dispose();
            return retValue;
        }

        public static int Get1vs1PvPGrade(int checkPoint)
        {
            int checkBefore = 0;
            foreach (KeyValuePair<PvP_Define.ePvPGrade_1vs1, int> checkSet in PvP_Define.PvP_1vs1_Grade_Check_PointList)
            {
                if(checkSet.Key == PvP_Define.ePvPGrade_1vs1.Platinum_5)
                    return (int)checkSet.Key;
                else if (checkPoint >= checkBefore && checkPoint < checkSet.Value)
                    return (int)checkSet.Key;
                else
                    checkBefore = checkSet.Value;
            }

            return (int)PvP_Define.ePvPGrade_1vs1.Bronze_1;
        }
        
        public static int GetFreePvPGrade(long checkRank, long totalPlayer)
        {
            foreach (KeyValuePair<PvP_Define.ePvPGrade_Free, KeyValuePair<byte, int>> checkSet in PvP_Define.PvP_Free_Grade_Check_PointList)
            {
                if (checkSet.Value.Key == PvP_Define.PvPFree_GradeCheckType_Rank_Position && checkRank < checkSet.Value.Value)
                    return (int)checkSet.Key;
                if (checkSet.Value.Key == PvP_Define.PvPFree_GradeCheckType_Rank_Rate && (checkRank / totalPlayer * 100.0f) < checkSet.Value.Value)
                    return (int)checkSet.Key;
            }

            return (int)PvP_Define.ePvPGrade_Free.Bronze;
        }

        public static User_PvP_Play_Info GetUser_PvPInfo(ref TxnBlock TB, long AID, PvP_Define.ePvPType PvPType, bool Flush = false, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}_{3}", PvP_Define.PvP_RedisKey_List[PvPType], PvP_Define.PvP_PlayInfo_TableName, AID, (int)PvPType);

            User_PvP_Play_Info userPlayInfo = Flush ? null : GenericFetch.FetchFromOnly_Redis<User_PvP_Play_Info>(DataManager_Define.RedisServerAlias_User, setKey);
            if (userPlayInfo == null)
            {
                SqlCommand commandUser_PvPInfo = new SqlCommand();
                commandUser_PvPInfo.CommandText = "PVP_GetDailyPlayCount";
                commandUser_PvPInfo.Parameters.Add("@aid", SqlDbType.BigInt).Value = AID;
                commandUser_PvPInfo.Parameters.Add("@pvpType", SqlDbType.NVarChar, 128).Value = (int)PvPType;

                SqlDataReader getDr = null;
                if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandUser_PvPInfo, ref getDr))
                {
                    if (getDr != null)
                    {
                        var r = SQLtoJson.Serialize(ref getDr);
                        string json = mJsonSerializer.ToJsonString(r);
                        getDr.Dispose(); getDr.Close();

                        User_PvP_Play_Info[] retSet = mJsonSerializer.JsonToObject<User_PvP_Play_Info[]>(json);

                        if (retSet.Length > 0)
                        {
                            userPlayInfo = retSet[0];
                            RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_User, setKey, userPlayInfo);
                        }
                        else
                            userPlayInfo = new User_PvP_Play_Info();
                    }
                }
                else
                    userPlayInfo = new User_PvP_Play_Info();
                commandUser_PvPInfo.Dispose();
            }

            return userPlayInfo;
        }

        public static List<User_PvP_Play_Info> GetUser_PvPInfo_List(ref TxnBlock TB, long AID, PvP_Define.ePvPType PvPType, string dbkey = PvP_Define.PvP_Info_DB)
        {
            List<User_PvP_Play_Info> userPlayInfo = new List<User_PvP_Play_Info>();
            SqlCommand commandUser_PvPInfo = new SqlCommand();
            commandUser_PvPInfo.CommandText = "PVP_GetDailyPlayCount";
            commandUser_PvPInfo.Parameters.Add("@aid", SqlDbType.BigInt).Value = AID;
            commandUser_PvPInfo.Parameters.Add("@pvpType", SqlDbType.NVarChar, 128).Value = (int)PvPType;

            SqlDataReader getDr = null;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandUser_PvPInfo, ref getDr, false))
            {
                if (getDr != null)
                {
                    var r = SQLtoJson.Serialize(ref getDr);
                    string json = mJsonSerializer.ToJsonString(r);
                    getDr.Dispose(); getDr.Close();

                    userPlayInfo = mJsonSerializer.JsonToObject<List<User_PvP_Play_Info>>(json);
                    userPlayInfo.ForEach(item => item.pvp_type = (int)PvPType);
                }
            }
            commandUser_PvPInfo.Dispose();
            return userPlayInfo;
        }

        public static Result_Define.eResult SetUserPvP_Rank_Info(ref TxnBlock TB, long AID, int seperaterWeekOrSeason = 0, PvP_Define.ePvPType PvPType = PvP_Define.ePvPType.MATCH_FREE, string dbkey = PvP_Define.PvP_Info_DB)
        {
            PVP_Record setObj = PvPManager.GetUser_PvP_Record(ref TB, AID, seperaterWeekOrSeason, PvPType);
            if (setObj.totalkill > 0 || setObj.totaldeath > 0)
                return SetUser_PvP_Point(setObj, seperaterWeekOrSeason);
            return Result_Define.eResult.SUCCESS;
        }


        public static Result_Define.eResult SetUser_PvP_Point(Guild_PVP_Record setObj, long seperaterWeekOrSeason)
        {
            return SetUser_PvP_Point(setObj.gid, setObj.rankpoint, setObj.win, setObj.lose, seperaterWeekOrSeason, PvP_Define.ePvPType.MATCH_GUILD_3VS3);
        }


        public static Result_Define.eResult SetUser_PvP_Point(PVP_Record setObj, long seperaterWeekOrSeason)
        {
            return SetUser_PvP_Point(setObj.aid, setObj.totalhonorpoint, setObj.totalkill, setObj.totaldeath, seperaterWeekOrSeason, (PvP_Define.ePvPType)setObj.pvp_type);
        }

        public static Result_Define.eResult SetUser_PvP_Point(long AID, int rankPoint, int firstValue, int secondValue, long seperaterWeekOrSeason, PvP_Define.ePvPType PvPType)
        {
            string setKey = GetPvP_RankKey(seperaterWeekOrSeason, PvPType);

            //long setTime = updateTimeRedisValue;
            //int Seperater = setTime.ToString().Length;
            //double SetPoint = rankPoint + (setTime / (System.Math.Pow(10f, Seperater)));
            double SetPoint = rankPoint + (firstValue / System.Math.Pow(10f, 4)) + ((PvP_Define.PvP_RANKING_SORT_SEP - secondValue) / (System.Math.Pow(10f, 8)));
            bool bSetPoint = RedisConst.GetRedisInstance().SortedSetAdd(DataManager_Define.RedisServerAlias_Ranking, setKey, AID.ToString(), SetPoint);

            return bSetPoint ? Result_Define.eResult.SUCCESS : Result_Define.eResult.REDIS_COMMAND_FAIL;
        }

        public static long GetTotal_PvP_Rank_Player(ref TxnBlock TB, long seperaterWeekOrSeason = 0, PvP_Define.ePvPType PvPType = PvP_Define.ePvPType.MATCH_FREE)
        {
            if (seperaterWeekOrSeason < 1)
                seperaterWeekOrSeason = GetSeperater(ref TB, PvPType);

            string setKey = GetPvP_RankKey(seperaterWeekOrSeason, PvPType);
            return RedisConst.GetRedisInstance().GetSortedSetCount(DataManager_Define.RedisServerAlias_Ranking, setKey);
        }

        public static Result_Define.eResult RemoveUser_PvP_point(ref TxnBlock TB, long AID, long seperaterWeekOrSeason, PvP_Define.ePvPType PvPType = PvP_Define.ePvPType.MATCH_FREE, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setDBTable = PvPType == PvP_Define.ePvPType.MATCH_1VS1 ? PvP_Define.PvP_PvP_Season_TableName : PvP_Define.PvP_PvP_Weekly_TableName;
            string setQuery = string.Format("DELETE FROM {0} WHERE pvp_type = {1} AND seperater = {2} AND aid = {3} ", PvP_Define.PvP_Table_List[PvPType], (int)PvPType, seperaterWeekOrSeason, AID);

            Result_Define.eResult retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;

            if (retError == Result_Define.eResult.SUCCESS && PvPType == PvP_Define.ePvPType.MATCH_1VS1)
            {
                setQuery = string.Format("DELETE FROM {0} WHERE pvp_type = {1} AND seperater = {2} AND aid = {3} ", PvP_Define.PvP_PvP_Weekly_TableName, (int)PvPType, seperaterWeekOrSeason, AID);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                string setKey = GetPvP_RankKey(seperaterWeekOrSeason, PvPType);
                RedisConst.GetRedisInstance().RemoveSortedSet(DataManager_Define.RedisServerAlias_Ranking, setKey, AID.ToString());
            }

            return retError;
        }

        public static long GetUser_PvP_Rank(ref TxnBlock TB, long AID, int seperaterWeekOrSeason = 0, PvP_Define.ePvPType PvPType = PvP_Define.ePvPType.MATCH_FREE, string dbkey = PvP_Define.PvP_Info_DB)
        {
            if (seperaterWeekOrSeason < 1)
                seperaterWeekOrSeason = GetSeperater(ref TB, PvPType);

            string setKey = GetPvP_RankKey(seperaterWeekOrSeason, PvPType);

            long setRank = RedisConst.GetRedisInstance().GetRank(DataManager_Define.RedisServerAlias_Ranking, setKey, AID.ToString());

            return setRank;
        }

        public static Ret_PvP GetUser_PvP_Rank_Info(ref TxnBlock TB, long AID, int seperaterWeekOrSeason = 0, PvP_Define.ePvPType PvPType = PvP_Define.ePvPType.MATCH_FREE, string dbkey = PvP_Define.PvP_Info_DB)
        {
            if (seperaterWeekOrSeason < 1)
                seperaterWeekOrSeason = GetSeperater(ref TB, PvPType);

            CheckSetPvPRankList(ref TB, seperaterWeekOrSeason, PvPType, dbkey);

            string setKey = GetPvP_RankKey(seperaterWeekOrSeason, PvPType);
            Account_Simple_With_Character userSimpleInfo = AccountManager.GetSimpleAccountCharacterInfo(ref TB, AID, dbkey);
            PVP_Record setObj = PvPManager.GetUser_PvP_Record(ref TB, AID, seperaterWeekOrSeason, PvPType);
            long setRank = 0;
            double setPoint = 0;
            //double setInfoPoint = 0;
            try
            {
                setRank = RedisConst.GetRedisInstance().GetRank(DataManager_Define.RedisServerAlias_Ranking, setKey, AID.ToString());
                setPoint = RedisConst.GetRedisInstance().GetScore(DataManager_Define.RedisServerAlias_Ranking, setKey, AID.ToString());
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
                double getScore = 0;
                //double getInfoPoint = 0;
                //SetUserPvP_Rank_Info(ref TB, AID, seperaterWeekOrSeason, PvPType);
                //setRank = GetTotal_PvP_Rank_Player(ref TB, seperaterWeekOrSeason, PvPType) + 1;
                setPoint = getScore;
                //setInfoPoint = getInfoPoint;
            }

            Ret_PvP retObj = CalcValuePoint(ref TB, AID, setRank, seperaterWeekOrSeason, PvPType);

            retObj.aid = userSimpleInfo.aid;
            retObj.username = userSimpleInfo.username;
            retObj.Class = userSimpleInfo.charinfo.Class;
            retObj.level = userSimpleInfo.charinfo.level;

            return retObj;
        }

        public static Ret_PvP CalcValuePoint(ref TxnBlock TB, long setAID, long setRank, int week, PvP_Define.ePvPType PvPType, string dbkey = PvP_Define.PvP_Info_DB)
        {
            Ret_PvP retObj = new Ret_PvP();
            PVP_Record setObj = PvPManager.GetUser_PvP_Record(ref TB, setAID, week, PvPType);
            retObj.first_value = setObj.totalkill;
            retObj.second_value = setObj.totaldeath;
            retObj.point = setObj.totalhonorpoint;
            retObj.rank = setRank;
            return retObj;
        }

        public static string GetPvP_RankKey(long seperaterWeekOrSeason, PvP_Define.ePvPType PvPType = PvP_Define.ePvPType.MATCH_FREE)
        {
            string setKey = string.Format("{0}_{1}_{2}", PvP_Define.PvP_RedisKey_List[PvPType], PvP_Define.PvP_Table_List[PvPType], seperaterWeekOrSeason);
            return setKey;
        }

        public static void CheckSetPvPRankList(ref TxnBlock TB, int seperaterWeekOrSeason = 0, PvP_Define.ePvPType PvPType = PvP_Define.ePvPType.MATCH_FREE, string dbkey = GuildManager.GuildcommonDBName)
        {
            string setKey = GetPvP_RankKey(seperaterWeekOrSeason, PvPType);

            long getCount = RedisConst.GetRedisInstance().GetSortedSetCount(DataManager_Define.RedisServerAlias_Ranking, setKey);
            Dictionary<string, double> getRedis_SortedSet_Obj = new Dictionary<string, double>();

            string setDBTable = PvPType == PvP_Define.ePvPType.MATCH_1VS1 ? PvP_Define.PvP_PvP_Season_TableName : PvP_Define.PvP_PvP_Weekly_TableName;
            string setQuery = string.Format("SELECT COUNT(*) as count FROM {0} WITH(NOLOCK)  WHERE pvp_type = {1} AND seperater = {2}", setDBTable, (int)PvPType, seperaterWeekOrSeason);
            Rank_Count pvpCount = TheSoul.DataManager.GenericFetch.FetchFromDB<Rank_Count>(ref TB, setQuery, dbkey);

            if (getCount < pvpCount.count)
            {
                setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE pvp_type = {1} AND seperater = {2}", setDBTable, (int)PvPType, seperaterWeekOrSeason);

                List<PVP_Record> setObjList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<PVP_Record>(ref TB, setQuery, dbkey);
                getRedis_SortedSet_Obj = new Dictionary<string, double>();

                setObjList.ForEach(setObj => { SetUser_PvP_Point(setObj, seperaterWeekOrSeason); });
            }
        }

        public static List<Ret_PvP> GetUser_PvP_Rank_List(ref TxnBlock TB, long AID, int seperaterWeekOrSeason = 0, PvP_Define.ePvPType PvPType = PvP_Define.ePvPType.MATCH_FREE, int rank_start = 0, int rank_to = PvP_Define.PvP_DefaultTopCount, string dbkey = PvP_Define.PvP_Info_DB)
        {
            if (seperaterWeekOrSeason < 1)
                seperaterWeekOrSeason = GetSeperater(ref TB, PvPType);

            CheckSetPvPRankList(ref TB, seperaterWeekOrSeason, PvPType, dbkey);
            string setKey = GetPvP_RankKey(seperaterWeekOrSeason, PvPType);
            Dictionary<string, double> getRedis_SortedSet_Obj = RedisConst.GetRedisInstance().GetSortedSetRangeByPosWithScore(DataManager_Define.RedisServerAlias_Ranking, setKey, rank_start, rank_to);

            if (getRedis_SortedSet_Obj == null)
                getRedis_SortedSet_Obj = new Dictionary<string, double>();

            List<Ret_PvP> retObj = new List<Ret_PvP>();
            long setRank = rank_start;
            //Ret_PvP beforeRank = new Ret_PvP();
            foreach (KeyValuePair<string, double> setitem in getRedis_SortedSet_Obj)
            {
                long setAID = Int64.Parse(setitem.Key);
                if (setAID > 0)
                {
                    Account_Simple_With_Character userSimpleInfo = AccountManager.GetSimpleAccountCharacterInfo(ref TB, setAID, 0, false, dbkey);

                    if (userSimpleInfo != null)
                    {
                        if (userSimpleInfo.charinfo != null)
                        {
                            setRank++;

                            Ret_PvP setObj = CalcValuePoint(ref TB, setAID, setRank, seperaterWeekOrSeason, PvPType);
                            //if (beforeRank.point == setObj.point && beforeRank.first_value == setObj.first_value && beforeRank.second_value == setObj.second_value)
                            //    setObj.rank = beforeRank.rank;
                            //else
                            setObj.rank = setRank;
                            //beforeRank = setObj;

                            setObj.aid = userSimpleInfo.aid;
                            setObj.username = userSimpleInfo.username;
                            setObj.Class = userSimpleInfo.charinfo.Class;
                            setObj.level = userSimpleInfo.charinfo.level;

                            retObj.Add(setObj);
                        }
                    }
                }
            }

            return retObj;
        }

        // character ranking 
        public static Result_Define.eResult SetUser_PvP_Warpoint(ref PvP_WarPoint setObj)
        {
            return SetUser_PvP_Warpoint(setObj.aid, setObj.cid, setObj.warpoint);
        }

        public static Result_Define.eResult SetUser_PvP_Warpoint(long aid, long cid, long setpoint)
        {
            if (aid > 0 && cid > 0)
            {
                PvP_Define.eCharacterRankType PvPType = PvP_Define.eCharacterRankType.WARPOINT;
                string setKey = string.Format("{0}_{1}", PvP_Define.CharacterRank_RedisKey_List[PvPType], PvP_Define.CharacterRank_Table_List[PvPType]);
                //string setInfoKey = string.Format("{0}_{1}_{2}", PvP_Define.CharacterRank_RedisKey_List[PvPType], PvP_Define.CharacterRank_Table_List[PvPType], PvP_Define.PvP_Info_Surfix);

                bool bSetPoint = RedisConst.GetRedisInstance().SortedSetAdd(DataManager_Define.RedisServerAlias_Ranking, setKey, cid.ToString(), setpoint);
                bool bSetInfoPoint = true;
                //bool bSetInfoPoint = RedisConst.GetRedisInstance().SortedSetAdd(setInfoKey, cid.ToString(), aid);

                return (bSetPoint && bSetInfoPoint) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.REDIS_COMMAND_FAIL;
            }
            return Result_Define.eResult.SUCCESS;
        }

        public static Ret_PvP GetUser_PvP_Warpoint_Rank_Info(ref TxnBlock TB, ref PvP_WarPoint setObj, long AID, ref long TotalPlayerCount, string dbkey = PvP_Define.PvP_Info_DB)
        {
            PvP_Define.eCharacterRankType PvPType = PvP_Define.eCharacterRankType.WARPOINT;
            string setKey = string.Format("{0}_{1}", PvP_Define.CharacterRank_RedisKey_List[PvPType], PvP_Define.CharacterRank_Table_List[PvPType]);

            long setRank = 0;
            double setPoint = 0;
            try
            {
                setRank = RedisConst.GetRedisInstance().GetRank(DataManager_Define.RedisServerAlias_Ranking, setKey, setObj.cid.ToString());
                setPoint = RedisConst.GetRedisInstance().GetScore(DataManager_Define.RedisServerAlias_Ranking, setKey, setObj.cid.ToString());
                TotalPlayerCount = RedisConst.GetRedisInstance().GetSortedSetCount(DataManager_Define.RedisServerAlias_Ranking, setKey);
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
                double getScore = 0;

                SetUser_PvP_Warpoint(setObj.aid, setObj.cid, setObj.warpoint);
                TotalPlayerCount = RedisConst.GetRedisInstance().GetSortedSetCount(DataManager_Define.RedisServerAlias_Ranking, setKey);

                setRank = TotalPlayerCount;
                setPoint = getScore;
            }
            Account_Simple userSimpleInfo = AccountManager.GetSimpleAccountInfo(ref TB, AID);

            Ret_PvP retObj = new Ret_PvP();
            Character_Simple charinfo = new Character_Simple(CharacterManager.GetCharacter_FromDB(ref TB, AID, setObj.cid));

            retObj.aid = userSimpleInfo.aid;
            retObj.username = userSimpleInfo.username;
            retObj.Class = charinfo.Class;
            retObj.level = charinfo.level;
            retObj.point = setPoint;
            retObj.rank = setRank;

            return retObj;
        }

        public static string GetUser_PvP_Warpoint_Rank_key()
        {
            PvP_Define.eCharacterRankType PvPType = PvP_Define.eCharacterRankType.WARPOINT;
            string setKey = string.Format("{0}_{1}", PvP_Define.CharacterRank_RedisKey_List[PvPType], PvP_Define.CharacterRank_Table_List[PvPType]);
            return setKey;
        }

        public static List<Ret_PvP> GetUser_PvP_Warpoint_Rank_List(ref TxnBlock TB, int rank_start = 0, int rank_to = PvP_Define.PvP_DefaultTopCount, string dbkey = PvP_Define.PvP_Info_DB)
        {
            PvP_Define.eCharacterRankType PvPType = PvP_Define.eCharacterRankType.WARPOINT;
            string setKey = string.Format("{0}_{1}", PvP_Define.CharacterRank_RedisKey_List[PvPType], PvP_Define.CharacterRank_Table_List[PvPType]);

            string setQuery = string.Format("SELECT COUNT(*) as count FROM {0} WITH(NOLOCK) ", Character_Define.CharacterTableName);
            Rank_Count charCount = TheSoul.DataManager.GenericFetch.FetchFromDB<Rank_Count>(ref TB, setQuery, dbkey);

            long getCount = RedisConst.GetRedisInstance().GetSortedSetCount(DataManager_Define.RedisServerAlias_Ranking, setKey);
            Dictionary<string, double> getRedis_SortedSet_Obj = RedisConst.GetRedisInstance().GetSortedSetRangeByPosWithScore(DataManager_Define.RedisServerAlias_Ranking, setKey, rank_start, rank_to);

            if (getRedis_SortedSet_Obj == null)
                getRedis_SortedSet_Obj = new Dictionary<string, double>();

            if (getCount < charCount.count)
            {
                setQuery = string.Format("SELECT aid, cid, warpoint FROM {0} WITH(NOLOCK) ", Character_Define.CharacterTableName);

                List<PvP_WarPoint> setObjList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<PvP_WarPoint>(ref TB, setQuery, dbkey);
                getRedis_SortedSet_Obj = new Dictionary<string, double>();
                setObjList.ForEach(setObj => { SetUser_PvP_Warpoint(setObj.aid, setObj.cid, setObj.warpoint); });
                getRedis_SortedSet_Obj = RedisConst.GetRedisInstance().GetSortedSetRangeByPosWithScore(DataManager_Define.RedisServerAlias_Ranking, setKey, rank_start, rank_to);
            }


            if (getRedis_SortedSet_Obj == null)
                getRedis_SortedSet_Obj = new Dictionary<string, double>();

            List<Ret_PvP> retObj = new List<Ret_PvP>();
            long setRank = rank_start;
            Ret_PvP beforeRank = new Ret_PvP();
            foreach (KeyValuePair<string, double> setitem in getRedis_SortedSet_Obj)
            {
                long setCID = Int64.Parse(setitem.Key);
                Account_Simple_With_Character userSimpleInfo = CharacterManager.GetSimpleAccountCharacterInfo_ByCharacterManager(ref TB, setCID, false, dbkey);
                if (userSimpleInfo != null)
                {
                    if (userSimpleInfo.charinfo != null)
                    {
                        setRank++;
                        Ret_PvP setObj = new Ret_PvP();
                        setObj.point = setitem.Value;
                        if (beforeRank.point == setObj.point)
                            setObj.rank = beforeRank.rank;
                        else
                            setObj.rank = setRank;
                        beforeRank = setObj;

                        setObj.aid = userSimpleInfo.aid;
                        setObj.username = userSimpleInfo.username;
                        setObj.Class = userSimpleInfo.charinfo.Class;
                        setObj.level = userSimpleInfo.charinfo.level;
                        setObj.point = setitem.Value;
                        beforeRank = setObj;

                        retObj.Add(setObj);
                    }
                }
            }

            return retObj;
        }


        //////////////////////////////////////////////////////////////////////////////////
        // Guild war (PVP) ranking 
        //////////////////////////////////////////////////////////////////////////////////
        public static Ret_GuildWarPvP CalcValueGuildWarPoint(ref TxnBlock TB, long guildIDOrAID, long setRank, int seperaterWeek, string dbkey = GuildManager.GuildcommonDBName)
        {
            Ret_GuildWarPvP retObj = new Ret_GuildWarPvP();
            Guild_PVP_Record setObj = PvPManager.GetGuild_PvP_Record(ref TB, guildIDOrAID, seperaterWeek);
            retObj.first_value = setObj.win;
            retObj.second_value = setObj.lose;
            retObj.point = setObj.rankpoint;
            retObj.rank = setRank;
            return retObj;
        }

        public static Ret_GuildWarJoinerPvP GetUser_GuildWar_Rank_Info(ref TxnBlock TB, long AID, long GID, int seperaterWeek = 0, string dbkey = GuildManager.GuildcommonDBName)
        {
            if (seperaterWeek < 1)
                seperaterWeek = GetSeperater(ref TB, PvP_Define.ePvPType.MATCH_GUILD_3VS3);

            Guild_User_PVP_Record getInfo = PvPManager.GetGuild_User_PvP_Record(ref TB, AID, GID, PvP_Define.PvP_GuildUser_Monthly_TableName, seperaterWeek);
            return new Ret_GuildWarJoinerPvP(getInfo);
        }

        public static Result_Define.eResult SetGuildPvP_GuildWar_Rank_Info(ref TxnBlock TB, long GID, out double RankPoint, string dbkey = GuildManager.GuildcommonDBName)
        {
            RankPoint = 0;
            if (GID <= 0)
                return Result_Define.eResult.GUILD_NOEXIST_INFO;
            int seperater = GetSeperater(ref TB, PvP_Define.ePvPType.MATCH_GUILD_3VS3);
            Guild_PVP_Record getMyGuildInfo = PvPManager.GetGuild_PvP_Record(ref TB, GID, seperater, true);
            RankPoint = getMyGuildInfo.rankpoint;
            return SetGuildPvP_GuildWar_Rank_Info(GID, getMyGuildInfo.rankpoint, getMyGuildInfo.win, getMyGuildInfo.lose, getMyGuildInfo.updateTimeRedisValue, seperater);
        }


        public static Result_Define.eResult SetGuildPvP_GuildWar_Rank_Info(ref TxnBlock TB, long GID, string dbkey = GuildManager.GuildcommonDBName)
        {
            if (GID <= 0)
                return Result_Define.eResult.GUILD_NOEXIST_INFO;
            int seperater = GetSeperater(ref TB, PvP_Define.ePvPType.MATCH_GUILD_3VS3);
            Guild_PVP_Record getMyGuildInfo = PvPManager.GetGuild_PvP_Record(ref TB, GID, seperater, true);
            if (getMyGuildInfo.win > 0 || getMyGuildInfo.lose > 0)
                return SetGuildPvP_GuildWar_Rank_Info(GID, getMyGuildInfo.rankpoint, getMyGuildInfo.win, getMyGuildInfo.lose, getMyGuildInfo.updateTimeRedisValue, seperater);
            return Result_Define.eResult.SUCCESS;
        }


        public static Result_Define.eResult SetGuildPvP_GuildWar_Rank_Info(long GID, int rankPoint, int win, int lose, int updateTimeRedisValue, long seperaterWeekOrSeason)
        {
            string setKey = GetPvP_RankKey(seperaterWeekOrSeason, PvP_Define.ePvPType.MATCH_GUILD_3VS3);

            //long setTime = updateTimeRedisValue;
            //int Seperater = setTime.ToString().Length;
            //double SetPoint = rankPoint + (setTime / (System.Math.Pow(10f, Seperater)));
            double SetPoint = rankPoint + (win / System.Math.Pow(10f, 4)) + (lose / (System.Math.Pow(10f, 8)));
            bool bSetPoint = RedisConst.GetRedisInstance().SortedSetAdd(DataManager_Define.RedisServerAlias_Ranking, setKey, GID.ToString(), SetPoint);

            return bSetPoint ? Result_Define.eResult.SUCCESS : Result_Define.eResult.REDIS_COMMAND_FAIL;
        }

        public static long GetGuildPvP_Rank(ref TxnBlock TB, long GID, int seperaterWeek = 0, string dbkey = GuildManager.GuildcommonDBName)
        {
            if (seperaterWeek < 1)
                seperaterWeek = GetSeperater(ref TB, PvP_Define.ePvPType.MATCH_GUILD_3VS3);

            string setKey = GetPvP_RankKey(seperaterWeek, PvP_Define.ePvPType.MATCH_GUILD_3VS3);
            long setRank = RedisConst.GetRedisInstance().GetRank(DataManager_Define.RedisServerAlias_Ranking, setKey, GID.ToString());
            return setRank;
        }

        public static Ret_GuildWarPvP GetGuildPvP_GuildWar_Rank_Info(ref TxnBlock TB, long gid, ref long TotalPlayerCount, int seperaterWeek = 0, string dbkey = GuildManager.GuildcommonDBName)
        {
            if (seperaterWeek < 1)
                seperaterWeek = GetSeperater(ref TB, PvP_Define.ePvPType.MATCH_GUILD_3VS3);

            string setKey = GetPvP_RankKey(seperaterWeek, PvP_Define.ePvPType.MATCH_GUILD_3VS3);
            string setLastWeekKey = GetPvP_RankKey(seperaterWeek - 1, PvP_Define.ePvPType.MATCH_GUILD_3VS3);
            Guild_GuildCreation setObj = GuildManager.GetGuilData(ref TB, gid);

            if (setObj == null)
                setObj = new Guild_GuildCreation();

            long setRank = 0;
            double setPoint = 0;
            double lastPoint = 0;
            try
            {
                setRank = RedisConst.GetRedisInstance().GetRank(DataManager_Define.RedisServerAlias_Ranking, setKey, setObj.GuildID.ToString());
                setPoint = RedisConst.GetRedisInstance().GetScore(DataManager_Define.RedisServerAlias_Ranking, setKey, setObj.GuildID.ToString());
                lastPoint = RedisConst.GetRedisInstance().GetScore(DataManager_Define.RedisServerAlias_Ranking, setLastWeekKey, setObj.GuildID.ToString());
                TotalPlayerCount = RedisConst.GetRedisInstance().GetSortedSetCount(DataManager_Define.RedisServerAlias_Ranking, setKey);
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
                double getScore = 0;

                //SetGuildPvP_GuildWar_Rank_Info(ref TB, setObj.GuildID);
                //TotalPlayerCount = RedisConst.GetRedisInstance().GetSortedSetCount(DataManager_Define.RedisServerAlias_Ranking, setKey);
                setRank = 0;
                setPoint = getScore;
            }

            Ret_GuildWarPvP retObj = CalcValueGuildWarPoint( ref TB, gid, setRank, seperaterWeek );

            retObj.aid = setObj.GuildCreateAID;
            retObj.username = setObj.GuildCreateUserName;
            retObj.point = (int)setPoint;
            retObj.guild_mark = setObj.GuildMark;
            retObj.rank = setRank;
            retObj.gid = setObj.GuildID;
            retObj.guild_level = setObj.GuildLevel;
            retObj.guild_name = setObj.GuildName;

            return retObj;
        }

        public static Result_Define.eResult SetGuildPvP_GuildWarRanking_Point(PVP_GuildWarRecord setObj, long seperaterWeek, ref double SetPoint)
        {
            PvP_Define.eGuildRankType PvPType = PvP_Define.eGuildRankType.GUILDWARRANK;
            string setKey = string.Format("{0}_{1}_{2}", PvP_Define.GuildRank_RedisKey_List[PvPType], PvP_Define.GuildRank_Table_List[PvPType], seperaterWeek);
            //string setInfoKey = string.Format("{0}_{1}_{2}_{3}", PvP_Define.PvP_RedisKey_List[PvPType], PvP_Define.PvP_Table_List[PvPType], PvP_Define.PvP_Info_Surfix, week);

            long setTime = setObj.updateTimeRedisValue;
            int SeperaterLen = setTime.ToString().Length;
            SetPoint = setObj.weekpoint + (setTime / (System.Math.Pow(10f, SeperaterLen)));

            bool bSetPoint = RedisConst.GetRedisInstance().SortedSetAdd(DataManager_Define.RedisServerAlias_Ranking, setKey, setObj.guildid.ToString(), SetPoint);

            return bSetPoint ? Result_Define.eResult.SUCCESS : Result_Define.eResult.REDIS_COMMAND_FAIL;
        }

        public static string GetGuildPvP_GuildWar_Rank_Key(ref TxnBlock TB, long seperaterWeek = 0)
        {
            if (seperaterWeek < 0)
                seperaterWeek = GetSeperater_Week(ref TB);

            PvP_Define.eGuildRankType PvPType = PvP_Define.eGuildRankType.GUILDWARRANK;
            string setKey = string.Format("{0}_{1}_{2}", PvP_Define.GuildRank_RedisKey_List[PvPType], PvP_Define.GuildRank_Table_List[PvPType], seperaterWeek);
            return setKey;
        }

        public static List<Ret_GuildWarPvP> GetGuildPvP_GuildWar_Rank_List(ref TxnBlock TB, int rank_start = 0, int rank_to = PvP_Define.PvP_DefaultTopCount, int seperaterWeekOrSeason = -1, string dbkey = GuildManager.GuildcommonDBName)
        {
            if (seperaterWeekOrSeason < 0)
                seperaterWeekOrSeason = GetSeperater(ref TB, PvP_Define.ePvPType.MATCH_GUILD_3VS3);

            string setKey = GetPvP_RankKey(seperaterWeekOrSeason, PvP_Define.ePvPType.MATCH_GUILD_3VS3);
            string setQuery = string.Format("SELECT COUNT(*) as count FROM {0} WITH(NOLOCK)  WHERE seperater = {1}", PvP_Define.PvP_Table_List[PvP_Define.ePvPType.MATCH_GUILD_3VS3], seperaterWeekOrSeason);
            Rank_Count guildCount = TheSoul.DataManager.GenericFetch.FetchFromDB<Rank_Count>(ref TB, setQuery, dbkey);
            if (guildCount == null)
                guildCount = new Rank_Count();

            long getCount = RedisConst.GetRedisInstance().GetSortedSetCount(DataManager_Define.RedisServerAlias_Ranking, setKey);
            Dictionary<string, double> getRedis_SortedSet_Obj = new Dictionary<string, double>();

            if (getCount < guildCount.count)
            {
                setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE seperater = {1}", PvP_Define.PvP_Table_List[PvP_Define.ePvPType.MATCH_GUILD_3VS3], seperaterWeekOrSeason);

                List<Guild_PVP_Record> setObjList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<Guild_PVP_Record>(ref TB, setQuery, dbkey);
                setObjList.ForEach(setObj => { 
                    if(setObj.win > 0 || setObj.lose > 0)
                        SetGuildPvP_GuildWar_Rank_Info(setObj.gid, setObj.rankpoint, setObj.win, setObj.lose, setObj.updateTimeRedisValue, seperaterWeekOrSeason);
                });
                getRedis_SortedSet_Obj = RedisConst.GetRedisInstance().GetSortedSetRangeByPosWithScore(DataManager_Define.RedisServerAlias_Ranking, setKey, rank_start, rank_to);
            }else
                getRedis_SortedSet_Obj = RedisConst.GetRedisInstance().GetSortedSetRangeByPosWithScore(DataManager_Define.RedisServerAlias_Ranking, setKey, rank_start, rank_to);

            if (getRedis_SortedSet_Obj == null)
                getRedis_SortedSet_Obj = new Dictionary<string, double>();

            List<Ret_GuildWarPvP> retObj = new List<Ret_GuildWarPvP>();
            long setRank = rank_start;
            //long lastPoint = 0;
            string setLastWeekKey = string.Empty;
            Ret_GuildWarPvP beforeRank = new Ret_GuildWarPvP();
            foreach (KeyValuePair<string, double> setitem in getRedis_SortedSet_Obj)
            {
                long setGuildID = Int64.Parse(setitem.Key);
                if (0 < setGuildID)
                {
                    Guild_GuildCreation setGuildInfo = GuildManager.GetGuilData(ref TB, setGuildID, false, dbkey);
                    setRank++;
                    Guild_PVP_Record getMyGuildInfo = PvPManager.GetGuild_PvP_Record(ref TB, setGuildID, seperaterWeekOrSeason);

                    Ret_GuildWarPvP setObj = new Ret_GuildWarPvP();
                    setObj.point = getMyGuildInfo.rankpoint;
                    setObj.first_value = getMyGuildInfo.win;
                    setObj.second_value = getMyGuildInfo.lose;

                    //if (beforeRank.point == setObj.point && beforeRank.first_value == setObj.first_value && beforeRank.second_value == setObj.second_value)
                    //    setObj.rank = beforeRank.rank;
                    //else
                        setObj.rank = setRank;

                    beforeRank = setObj;

                    setObj.aid = setGuildInfo.GuildCreateAID;
                    setObj.username = setGuildInfo.GuildCreateUserName;

                    setObj.gid = setGuildInfo.GuildID;
                    setObj.guild_name = setGuildInfo.GuildName;
                    setObj.guild_level = setGuildInfo.GuildLevel;
                    setObj.guild_mark = setGuildInfo.GuildMark;

                    retObj.Add(setObj);
                }
            }

            return retObj;
        }

        //////////////////////////////////////////////////////////////////////////////////
        // Guild ranking 
        public static Result_Define.eResult SetUser_GuildRanking_Point(ref TxnBlock TB, ref System_GuildRanking_Data setObj, long week = 0)
        {
            return SetUser_GuildRanking_Point(ref TB, setObj.gid, setObj.weekGuildRankPoint, week);
        }

        public static Result_Define.eResult SetUser_GuildRanking_Point(ref TxnBlock TB, long gid, long setpoint, long week = 0)
        {
            if (week < 1)
                week = GetSeperater_Week(ref TB);

            if (gid > 0)
            {
                PvP_Define.eGuildRankType PvPType = PvP_Define.eGuildRankType.GUILDPOINT;
                string setKey = string.Format("{0}_{1}_{2}", PvP_Define.GuildRank_RedisKey_List[PvPType], PvP_Define.GuildRank_Table_List[PvPType], week);
                //string setInfoKey = string.Format("{0}_{1}_{2}", PvP_Define.CharacterRank_RedisKey_List[PvPType], PvP_Define.CharacterRank_Table_List[PvPType], PvP_Define.PvP_Info_Surfix);

                bool bSetPoint = RedisConst.GetRedisInstance().SortedSetAdd(DataManager_Define.RedisServerAlias_Ranking, setKey, gid.ToString(), setpoint);
                bool bSetInfoPoint = true;
                //bool bSetInfoPoint = RedisConst.GetRedisInstance().SortedSetAdd(setInfoKey, cid.ToString(), aid);

                return (bSetPoint && bSetInfoPoint) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.REDIS_COMMAND_FAIL;
            }
            return Result_Define.eResult.SUCCESS;
        }

        // The guild ranking information using the ranking list last week. However, the use of score marks week. 
        public static Ret_GuildPvP GetUser_PvP_Guild_Rank_Info(ref TxnBlock TB, long gid, ref long TotalPlayerCount, long week = 0, string dbkey = GuildManager.GuildcommonDBName)
        {
            if (week < 1)
                week = GetSeperater_Week(ref TB);
            long lastweek = week - 1;

            CheckSetGuildRankList(ref TB, lastweek, dbkey);

            PvP_Define.eGuildRankType PvPType = PvP_Define.eGuildRankType.GUILDPOINT;
            string setKey = string.Format("{0}_{1}_{2}", PvP_Define.GuildRank_RedisKey_List[PvPType], PvP_Define.GuildRank_Table_List[PvPType], lastweek);
            string setCurrentWeekKey = string.Format("{0}_{1}_{2}", PvP_Define.GuildRank_RedisKey_List[PvPType], PvP_Define.GuildRank_Table_List[PvPType], week);
            Guild_GuildCreation setObj = GuildManager.GetGuilData(ref TB, gid);

            if (setObj == null)
                setObj = new Guild_GuildCreation();

            long setRank = 0;
            double setPoint = 0;
            double lastPoint = 0;
            try
            {
                setRank = RedisConst.GetRedisInstance().GetRank(DataManager_Define.RedisServerAlias_Ranking, setKey, setObj.GuildID.ToString());
                setPoint = RedisConst.GetRedisInstance().GetScore(DataManager_Define.RedisServerAlias_Ranking, setCurrentWeekKey, setObj.GuildID.ToString());
                lastPoint = RedisConst.GetRedisInstance().GetScore(DataManager_Define.RedisServerAlias_Ranking, setKey, setObj.GuildID.ToString());
                TotalPlayerCount = RedisConst.GetRedisInstance().GetSortedSetCount(DataManager_Define.RedisServerAlias_Ranking, setKey);
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
                double getScore = 0;

                //SetUser_GuildRanking_Point(ref TB, setObj.GuildID, setObj.GuildRankingPoint, week);
                TotalPlayerCount = RedisConst.GetRedisInstance().GetSortedSetCount(DataManager_Define.RedisServerAlias_Ranking, setKey);

                setRank = TotalPlayerCount;
                setPoint = getScore;
            }

            Ret_GuildPvP retObj = new Ret_GuildPvP();

            retObj.aid = setObj.GuildCreateAID;
            retObj.username = setObj.GuildCreateUserName;
            retObj.point = setPoint;
            retObj.rank = setRank;
            retObj.last_point = lastPoint;
            retObj.gid = setObj.GuildID;
            retObj.guild_level = setObj.GuildLevel;
            retObj.guild_name = setObj.GuildName;

            return retObj;
        }

        public static void CheckSetGuildRankList(ref TxnBlock TB, long week = 0, string dbkey = GuildManager.GuildcommonDBName)
        {
            PvP_Define.eGuildRankType PvPType = PvP_Define.eGuildRankType.GUILDPOINT;

            string setQuery = string.Format("SELECT COUNT(*) as count FROM {0} WITH(NOLOCK)  WHERE seperater = {1}", PvP_Define.GuildRank_Table_List[PvPType], week);
            Rank_Count guildCount = TheSoul.DataManager.GenericFetch.FetchFromDB<Rank_Count>(ref TB, setQuery, dbkey);

            string setKey = string.Format("{0}_{1}_{2}", PvP_Define.GuildRank_RedisKey_List[PvPType], PvP_Define.GuildRank_Table_List[PvPType], week);
            Dictionary<string, double> getRedis_SortedSet_Obj = new Dictionary<string, double>();
            long getCount = RedisConst.GetRedisInstance().GetSortedSetCount(DataManager_Define.RedisServerAlias_Ranking, setKey);

            if (getCount < guildCount.count)
            {
                setQuery = string.Format("SELECT gid as GuildID, weekGuildRankPoint as GuildRankingPoint FROM {0} WITH(NOLOCK)  WHERE seperater = {1}", PvP_Define.GuildRank_Table_List[PvPType], week);

                List<PvP_GuildPoint> setObjList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<PvP_GuildPoint>(ref TB, setQuery, dbkey);

                foreach (PvP_GuildPoint setObj in setObjList)
                {
                    SetUser_GuildRanking_Point(ref TB, setObj.GuildID, setObj.GuildRankingPoint, week);
                }
            }
        }

        public static string GetPvP_Guild_RankKey(ref TxnBlock TB, long week = 0, PvP_Define.eGuildRankType PvPType = PvP_Define.eGuildRankType.GUILDPOINT, string dbkey = PvP_Define.PvP_Info_DB)
        {
            if (week < 1)
                week = GetSeperater_Week(ref TB, dbkey);
            long lastweek = week - 1;

            string setKey = string.Format("{0}_{1}_{2}", PvP_Define.GuildRank_RedisKey_List[PvPType], PvP_Define.GuildRank_Table_List[PvPType], lastweek);
            return setKey;
        }

        public static List<Ret_GuildPvP> GetUser_PvP_Guild_Rank_List(ref TxnBlock TB, int rank_start = 0, int rank_to = PvP_Define.PvP_DefaultTopCount, long week = 0, string dbkey = GuildManager.GuildcommonDBName)
        {
            if (week < 1)
                week = GetSeperater_Week(ref TB);
            long lastweek = week - 1;

            CheckSetGuildRankList(ref TB, lastweek, dbkey);
            PvP_Define.eGuildRankType PvPType = PvP_Define.eGuildRankType.GUILDPOINT;
            string setKey = GetPvP_Guild_RankKey(ref TB, week, PvPType);
            string setCurrentWeekKey = string.Format("{0}_{1}_{2}", PvP_Define.GuildRank_RedisKey_List[PvPType], PvP_Define.GuildRank_Table_List[PvPType], week);
            
            Dictionary<string, double> getRedis_SortedSet_Obj = RedisConst.GetRedisInstance().GetSortedSetRangeByPosWithScore(DataManager_Define.RedisServerAlias_Ranking, setKey, rank_start, rank_to);

            if (getRedis_SortedSet_Obj == null)
                getRedis_SortedSet_Obj = new Dictionary<string, double>();

            List<Ret_GuildPvP> retObj = new List<Ret_GuildPvP>();
            long setRank = rank_start;
            long lastPoint = 0;

            getRedis_SortedSet_Obj = getRedis_SortedSet_Obj.OrderByDescending(item => item.Value).ThenBy(item => item.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (KeyValuePair<string, double> setitem in getRedis_SortedSet_Obj)
            {
                long setGuildID = Int64.Parse(setitem.Key);
                //setLastWeekKey = string.Format("{0}_{1}_{2}", PvP_Define.GuildRank_RedisKey_List[PvPType], PvP_Define.GuildRank_Table_List[PvPType], week - 1);
                lastPoint = System.Convert.ToInt64(RedisConst.GetRedisInstance().GetScore(DataManager_Define.RedisServerAlias_Ranking, setKey, setGuildID.ToString()));

                Guild_GuildCreation setGuildInfo = GuildManager.GetGuilData(ref TB, setGuildID, false, dbkey);
                setRank++;

                Ret_GuildPvP setObj = new Ret_GuildPvP();
                setObj.guild_mark = setGuildInfo.GuildMark;
                setObj.aid = setGuildInfo.GuildCreateAID;
                setObj.username = setGuildInfo.GuildCreateUserName;
                setObj.point = GuildManager.GetGuildRankPoint(ref TB, setGuildID).weekGuildRankPoint;
                setObj.rank = setRank;
                setObj.last_point = lastPoint;
                setObj.gid = setGuildInfo.GuildID;
                setObj.guild_level = setGuildInfo.GuildLevel;
                setObj.guild_name = setGuildInfo.GuildName;

                retObj.Add(setObj);
            }

            return retObj;
        }

        public static Result_Define.eResult SetPvPDailyCount(ref TxnBlock TB, long AID, int PvPType, int SetCount, int MapIndex = 1, string dbkey = PvP_Define.PvP_Info_DB)
        {
            SqlCommand commandUser_PvPInfo = new SqlCommand();
            commandUser_PvPInfo.CommandText = "PVP_SetDailyPlayCount";
            var outputResult = new SqlParameter("@ret_result", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            commandUser_PvPInfo.Parameters.Add("@aid", SqlDbType.BigInt).Value = AID;
            commandUser_PvPInfo.Parameters.Add("@pvpType", SqlDbType.Int).Value = PvPType;
            commandUser_PvPInfo.Parameters.Add("@newDailyPlayCnt", SqlDbType.Int).Value = SetCount;
            commandUser_PvPInfo.Parameters.Add("@map_index", SqlDbType.Int).Value = MapIndex;
            commandUser_PvPInfo.Parameters.Add("@add_reset_count", SqlDbType.Int).Value = 1;
            commandUser_PvPInfo.Parameters.Add(outputResult);
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandUser_PvPInfo))
            {
                int result = System.Convert.ToInt32(outputResult.Value.ToString());
                if (result != 0)
                    retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }
            else
            {
                retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }
            commandUser_PvPInfo.Dispose();

            return retError;
        }

        public static User_PvP_Play GetUser_PvP_Play_Info_DB(ref TxnBlock TB, long AID, int PvPType, int MapIndex = 1, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setKey = string.Format("{0}_{1}", PvP_Define.PvP_PlayInfo_TableName, AID);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND pvp_type = {2} AND map_index = {3}", PvP_Define.PvP_PlayInfo_TableName, AID, PvPType, MapIndex);

            User_PvP_Play retObj = GenericFetch.FetchFromDB<User_PvP_Play>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new User_PvP_Play();
        }

        public static int GetUser_PvP_High_Grade(ref TxnBlock TB, long AID, int PvPType, int MapIndex = 1, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setQuery = string.Format(@"SELECT [high_grade] FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND pvp_type = {2} AND map_index = {3}", PvP_Define.PvP_PlayInfo_TableName, AID, PvPType, MapIndex);

            PvP_Grade retObj = GenericFetch.FetchFromDB<PvP_Grade>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj.high_grade : 0;
        }
        
        public static int GetUser_Free_PvP_Grade(ref TxnBlock TB, int Level, int PvPType = 1, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setQuery = string.Format(@"SELECT ModeIndex as [high_grade] FROM {0} WITH(NOLOCK)  WHERE MatchingType = {1} AND ({2} BETWEEN  StartLevel AND EndLevel)", PvP_Define.PvP_System_Matching_Level_TableName, PvPType, Level);

            PvP_Grade retObj = GenericFetch.FetchFromDB<PvP_Grade>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj.high_grade : 1;
        }

        public static Result_Define.eResult SetUser_PvP_High_Grade(ref TxnBlock TB, long AID, long setGrade, int PvPType, int MapIndex = 1, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET [high_grade] = {1}, [high_grade_regdate] = GETDATE() WHERE aid = {2} AND pvp_type = {3} AND map_index = {4}", PvP_Define.PvP_PlayInfo_TableName, setGrade, AID, PvPType, MapIndex);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static int GetUser_PvP_High_Point(ref TxnBlock TB, long AID, int PvPType, int MapIndex = 1, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setQuery = string.Format(@"SELECT [high_point] as high_grade FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND pvp_type = {2} AND map_index = {3}", PvP_Define.PvP_PlayInfo_TableName, AID, PvPType, MapIndex);

            PvP_Grade retObj = GenericFetch.FetchFromDB<PvP_Grade>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj.high_grade : 0;
        }

        public static Result_Define.eResult SetUser_PvP_High_Point(ref TxnBlock TB, long AID, long setPoint, int PvPType, int MapIndex = 1, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET [high_point] = {1}, [high_point_regdate] = GETDATE() WHERE aid = {2} AND pvp_type = {3} AND map_index = {4}", PvP_Define.PvP_PlayInfo_TableName, setPoint, AID, PvPType, MapIndex);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static List<System_Rankup_Reward> GetSystem_Rankup_Reward(ref TxnBlock TB, bool Flush = false, string dbkey = PvP_Define.PvP_Info_DB)
        {
            string setKey = string.Format("{0}_{1}", PvP_Define.PvP_Overlord_Prefix, PvP_Define.PvP_System_Rankup_Reward_TableName);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)", PvP_Define.PvP_System_Rankup_Reward_TableName);
            List<System_Rankup_Reward> retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow<System_Rankup_Reward>(ref TB, DataManager_Define.RedisServerAlias_Ranking, setKey, setQuery, dbkey, Flush);
            return (retObj != null) ? retObj : new List<System_Rankup_Reward>();
        }

        public static long CaclHighGradeReward(ref TxnBlock TB, long beforeRank, long afterRank)
        {
            long rewardHighGrade = 0;

            if (afterRank >= PvP_Define.Overlord_HighGradeReward_Min)
                return rewardHighGrade;

            List<System_Rankup_Reward> sysRewardInfo = PvPManager.GetSystem_Rankup_Reward(ref TB);
            beforeRank = beforeRank < 1 || beforeRank > PvP_Define.Overlord_HighGradeReward_Min ? PvP_Define.Overlord_HighGradeReward_Min-1 : beforeRank;
            System_Rankup_Reward findReward = sysRewardInfo.Find(rewardInfo => rewardInfo.StartRank <= beforeRank && rewardInfo.EndRank >= beforeRank);
            while (findReward != null && (beforeRank > afterRank || beforeRank == 1)) 
            {
                int setReward = (int)((afterRank <= findReward.StartRank ? ((beforeRank - findReward.StartRank) +1) : (beforeRank - afterRank)) * findReward.Reward_Num);
                rewardHighGrade += setReward;
                beforeRank = afterRank < findReward.StartRank || beforeRank <= 1 ? (findReward.StartRank - 1) : afterRank;
                findReward = sysRewardInfo.Find(rewardInfo => rewardInfo.StartRank <= beforeRank && rewardInfo.EndRank >= beforeRank);
            }
            return rewardHighGrade;
        }
    }
}