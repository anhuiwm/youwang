using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager.DBClass;
using System.Globalization;

namespace TheSoul.DataManager
{
    public static partial class TriggerManager
    {
        public static string GetRediskey_System_EventGroup_Admin()
        {
            return string.Format("{0}_{1}", Trigger_Define.Trigger_Prefix, Trigger_Define.System_EventGroup_Admin_TableName);
        }

        public static List<System_EventGroup_Admin> GetSystem_EventGroup_Admin(ref TxnBlock TB, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = GetRediskey_System_EventGroup_Admin();
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  ORDER BY Order_Index ASC, Event_Index ASC", Trigger_Define.System_EventGroup_Admin_TableName);

            return GenericFetch.FetchFromRedis_MultipleRow<System_EventGroup_Admin>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
        }

        public static System_Event GetSystem_Event(ref TxnBlock TB, long Event_ID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            return GetSystem_Event_List(ref TB).Find(ev => ev.Event_ID == Event_ID);
        }

        public static List<System_Event> GetSystem_Event_All_List(ref TxnBlock TB, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)", Trigger_Define.System_Event_TableName);
            return GenericFetch.FetchFromDB_MultipleRow<System_Event>(ref TB, setQuery, dbkey);
        }
        
        public static List<System_Event> GetSystem_Event_List(ref TxnBlock TB, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = string.Format("{0}_{1}", Trigger_Define.Trigger_Prefix, Trigger_Define.System_Event_TableName);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK, INDEX(IDX_System_Event)) WHERE ActiveType > 0 AND (GETDATE() BETWEEN Event_StartTime AND Event_EndTime)", Trigger_Define.System_Event_TableName);

            List<System_Event> retObj = GenericFetch.FetchFromRedis_MultipleRow<System_Event>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);

            var overcheck = retObj.Find(eventTime => eventTime.Event_EndTime < DateTime.Now);
            if (overcheck != null)
            {
                Flush = true;
                retObj = GenericFetch.FetchFromRedis_MultipleRow<System_Event>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
            }
            return retObj;
            //return GenericFetch.FetchFromDB_MultipleRow<System_Event>(ref TB, setQuery, dbkey);
        }
        
        public static List<System_Event_Reward_Box> GetSystem_Event_Reward_Box_List(ref TxnBlock TB, long EventBoxID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = string.Format("{0}_{1}", Trigger_Define.Trigger_Prefix, Trigger_Define.System_Event_Reward_Box_TableName);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE EventBoxID = {1}", Trigger_Define.System_Event_Reward_Box_TableName, EventBoxID);

            return GenericFetch.FetchFromRedis_MultipleRow_Hash<System_Event_Reward_Box>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, EventBoxID.ToString(), setQuery, dbkey, Flush);
        }

        public static string GetRediskey_System_Event_Daily_List()
        {
            return string.Format("{0}_{1}", Trigger_Define.Trigger_Prefix, Trigger_Define.System_Event_Daily_TableName);
        }

        public static List<System_Event_Daily> GetSystem_Event_Daily_List(ref TxnBlock TB, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = GetRediskey_System_Event_Daily_List();
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) ", Trigger_Define.System_Event_Daily_TableName);

            return GenericFetch.FetchFromRedis_MultipleRow<System_Event_Daily>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
        }

        public static System_Event_Daily GetSystem_Event_Daily(ref TxnBlock TB, long EventID, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  Where Event_Daily_ID = {1}", Trigger_Define.System_Event_Daily_TableName, EventID);
            System_Event_Daily retObj = GenericFetch.FetchFromDB<System_Event_Daily>(ref TB, setQuery, dbkey);
            if (retObj == null)
                retObj = new System_Event_Daily();
            return retObj;
        }

        public static string GetRediskey_System_Event_First_Payment()
        {
            return string.Format("{0}_{1}", Trigger_Define.Trigger_Prefix, Trigger_Define.System_Event_First_Payment_TableName);
        }

        public static System_Event_First_Payment GetSystem_Event_First_Payment(ref TxnBlock TB, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = GetRediskey_System_Event_First_Payment();
            string setQuery = string.Format(@"SELECT TOP 1 * FROM {0} WITH(NOLOCK) ", Trigger_Define.System_Event_First_Payment_TableName);

            return GenericFetch.FetchFromRedis<System_Event_First_Payment>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
        }

        /// 获取首冲和累计充值的奖励信息
        public static List<System_Event_First_Payment> GetSystem_Event_First_ACCU_Payment(ref TxnBlock TB, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = GetRediskey_System_Event_First_Payment();
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) ", Trigger_Define.System_Event_First_Payment_TableName);

            return GenericFetch.FetchFromRedis_MultipleRow<System_Event_First_Payment>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
        }

        // Normal Achieve
        public static System_Achieve GetSystem_Achieve(ref TxnBlock TB, long AchieveID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            return GetSystem_Achieve_List(ref TB).Find(ev => ev.AchieveID == AchieveID);
        }

        public static List<System_Achieve> GetSystem_Achieve_List(ref TxnBlock TB, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = string.Format("{0}_{1}", Trigger_Define.Trigger_Prefix, Trigger_Define.System_Achieve_TableName);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) ", Trigger_Define.System_Achieve_TableName);

            return GenericFetch.FetchFromRedis_MultipleRow<System_Achieve>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
            //return GenericFetch.FetchFromDB_MultipleRow<System_Achieve>(ref TB, setQuery, dbkey);
        }

        public static List<System_Achieve_RewardBox> GetSystem_Achieve_Reward_Box_List(ref TxnBlock TB, long EventBoxID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = string.Format("{0}_{1}", Trigger_Define.Trigger_Prefix, Trigger_Define.System_Achieve_Reward_Box_TableName);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE EventBoxID = {1}", Trigger_Define.System_Achieve_Reward_Box_TableName, EventBoxID);

            return GenericFetch.FetchFromRedis_MultipleRow_Hash<System_Achieve_RewardBox>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, EventBoxID.ToString(), setQuery, dbkey, Flush);
        }

        // PvP Achieve
        public static System_Achieve_PvP GetSystem_Achieve_PvP(ref TxnBlock TB, long AchieveID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            return GetSystem_Achieve_PvP_List(ref TB).Find(ev => ev.AchieveID == AchieveID);
        }

        public static List<System_Achieve_PvP> GetSystem_Achieve_PvP_List(ref TxnBlock TB, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = string.Format("{0}_{1}", Trigger_Define.Trigger_Prefix, Trigger_Define.System_Achieve_PvP_TableName);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)", Trigger_Define.System_Achieve_PvP_TableName);

            return GenericFetch.FetchFromRedis_MultipleRow<System_Achieve_PvP>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
            //return GenericFetch.FetchFromDB_MultipleRow<System_Achieve>(ref TB, setQuery, dbkey);
        }

        public static List<System_Achieve_PvP_RewardBox> GetSystem_Achieve_PvP_Reward_Box_List(ref TxnBlock TB, long EventBoxID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = string.Format("{0}_{1}", Trigger_Define.Trigger_Prefix, Trigger_Define.System_Achieve_PvP_Reward_Box_TableName);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE EventBoxID = {1}", Trigger_Define.System_Achieve_PvP_Reward_Box_TableName, EventBoxID);

            return GenericFetch.FetchFromRedis_MultipleRow_Hash<System_Achieve_PvP_RewardBox>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, EventBoxID.ToString(), setQuery, dbkey, Flush);
        }

        public static void RemoveSystem_RewardBox_RedisKey(string type, string dbkey = DataManager_Define.RedisServerAlias_System)
        {
            string setKey = "";
            if(type == "event")
                setKey = string.Format("{0}_{1}", Trigger_Define.Trigger_Prefix, Trigger_Define.System_Event_Reward_Box_TableName);
            else
                setKey = string.Format("{0}_{1}", Trigger_Define.Trigger_Prefix, Trigger_Define.System_Achieve_Reward_Box_TableName);

            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveHash(dbkey, setKey);
        }

        // User Event Data
        public static User_Event_Data GetUser_Event_Data(ref TxnBlock TB, long AID, long userEventID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            List<User_Event_Data> getEvent = GetUser_Event_Data_List(ref TB, AID, Flush);
            User_Event_Data findObj = getEvent.Find(item => item.User_Event_ID == userEventID);
            if (findObj == null)
                findObj = new User_Event_Data();

            return findObj;
        }

        public static List<User_Event_Data> GetUser_Event_Data_List(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}", Trigger_Define.User_Event_Prefix, Trigger_Define.User_Event_Data_TableName, AID);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK, INDEX(IDX_User_Event_Data))  WHERE AID = {1} AND (GETDATE() BETWEEN StartTime AND EndTime)", Trigger_Define.User_Event_Data_TableName, AID);
            List<User_Event_Data> retObj = GenericFetch.FetchFromRedis_MultipleRow<User_Event_Data>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);

            if(retObj.FindAll(eventInfo => eventInfo.EndTime < DateTime.Now).Count > 0)
                retObj = GenericFetch.FetchFromRedis_MultipleRow<User_Event_Data>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, true);

            return retObj;
        }

        public static string GetRediskey_User_Event_Check_Data(long AID)
        {
            return string.Format("{0}_{1}_{2}", Trigger_Define.User_Event_Prefix, Trigger_Define.User_Event_Check_Data_TableName, AID);
        }

        public static User_Event_Check_Data GetUser_Event_Check_Data(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = GetRediskey_User_Event_Check_Data(AID);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE AID = {1}", Trigger_Define.User_Event_Check_Data_TableName, AID);
            User_Event_Check_Data retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<User_Event_Check_Data>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
            return retObj == null ? new User_Event_Check_Data(AID) : retObj;

            //User_Event_Check_Data retObj = Flush ? null : GenericFetch.FetchFromOnly_Redis<User_Event_Check_Data>(DataManager_Define.RedisServerAlias_User, setKey);

            //if (retObj == null)
            //{
            //    SqlCommand commandUser_EventInfo = new SqlCommand();
            //    commandUser_EventInfo.CommandText = "System_Get_User_Event_Check_Data";
            //    var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            //    commandUser_EventInfo.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            //    commandUser_EventInfo.Parameters.Add(result);

            //    SqlDataReader getDr = null;
            //    if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandUser_EventInfo, ref getDr))
            //    {
            //        if (getDr != null)
            //        {
            //            var r = SQLtoJson.Serialize(ref getDr);
            //            string json = mJsonSerializer.ToJsonString(r);
            //            getDr.Dispose(); getDr.Close();
            //            commandUser_EventInfo.Dispose();
            //            int checkValue = System.Convert.ToInt32(result.Value);

            //            if (checkValue < 0)
            //                return retObj;

            //            User_Event_Check_Data[] retSet = mJsonSerializer.JsonToObject<User_Event_Check_Data[]>(json);

            //            if (retSet.Length > 0)
            //                retObj = retSet[0];

            //            RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_User, setKey, retObj);
            //        }
            //    }
            //}
            //return retObj;
        }

        // Normal Achieve
        public static User_Event_Data GetUser_Achieve_Data(ref TxnBlock TB, long AID, long userEventID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            var getList = GetUser_Achieve_Data_List(ref TB, AID, Flush);
            User_Event_Data findObj = getList.Find(item => item.User_Event_ID == userEventID);
            if (findObj == null)
                findObj = new User_Event_Data();

            return findObj;
        }

        public static List<User_Event_Data> GetUser_Achieve_Data_List(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = GetRedisKey_UserAchive(AID, false);
            string setQuery = string.Format(@"SELECT *, '' AS Event_Type FROM {0} WITH(NOLOCK, INDEX(IDX_User_Achieve_Data)) WHERE AID = {1} AND (GETDATE() BETWEEN StartTime AND EndTime)", Trigger_Define.User_Achieve_Data_TableName, AID);
            
            return GenericFetch.FetchFromRedis_MultipleRow<User_Event_Data>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
        }

        // PvP Achieve
        public static User_Event_Data GetUser_Achieve_PvP_Data(ref TxnBlock TB, long AID, long userEventID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            var getList = GetUser_Achieve_PvP_Data_List(ref TB, AID, Flush);
            User_Event_Data findObj = getList.Find(item => item.User_Event_ID == userEventID);
            if (findObj == null)
                findObj = new User_Event_Data();

            return findObj;
        }

        public static List<User_Event_Data> GetUser_Achieve_PvP_Data_List(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = GetRedisKey_UserAchive(AID, true);
            string setQuery = string.Format(@"SELECT *, '' AS Event_Type FROM {0} WITH(NOLOCK, INDEX(IDX_User_Achieve_PvP_Data)) WHERE AID = {1} AND (GETDATE() BETWEEN StartTime AND EndTime)", Trigger_Define.User_Achieve_Data_PvP_TableName, AID);

            return GenericFetch.FetchFromRedis_MultipleRow<User_Event_Data>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
        }

        // for Daily Check 
        public static User_Event_Check_Data Check_User_Daily_Event(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            User_Event_Check_Data userEvent = GetUser_Event_Check_Data(ref TB, AID, Flush);

            DateTime startDate = DateTime.Parse(userEvent.RegDate.ToShortDateString());
            DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());

            bool bFlush = false;
            if(userEvent.CheckCount == 0)
            {
                UpdateEvent_Check_Data(ref TB, AID, 1, 0, 0, userEvent.VIPRewardList, userEvent.FirstPaymentFlag);
                bFlush = true;
            }
            else if (startDate.Year != dbDate.Year || startDate.Month != dbDate.Month)
            {
                UpdateEvent_Check_Data(ref TB, AID, 1, 0, 0, userEvent.VIPRewardList, userEvent.FirstPaymentFlag);
                bFlush = true;
            }
            else
            {
                TimeSpan TS = dbDate - startDate;

                if (TS.Days != 0 && userEvent.CheckCount == userEvent.RewardCount)
                {
                    UpdateEvent_Check_Data(ref TB, AID, userEvent.CheckCount + 1, userEvent.RewardCount, userEvent.AddCount, userEvent.VIPRewardList, userEvent.FirstPaymentFlag);
                    bFlush = true;
                }
            }

            if(bFlush)
                userEvent = GetUser_Event_Check_Data(ref TB, AID, true);

            return userEvent;
        }
        
        public static Ret_Daily_Event_List GetDailyEvent_RewardList(ref TxnBlock TB, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            int checkMonth = DateTime.Now.Month;
            string setKey = string.Format("{0}_{1}_{2}_{3}", Trigger_Define.User_Event_Prefix, Trigger_Define.System_Event_Daily_TableName, checkMonth, Trigger_Define.User_Event_Daily_Surfix);

            Ret_Daily_Event_List retObj = Flush ? null : GenericFetch.FetchFromOnly_Redis<Ret_Daily_Event_List>(DataManager_Define.RedisServerAlias_User, setKey);

            if (retObj == null)
            {
                retObj = new Ret_Daily_Event_List();
                retObj.everyday = new List<Ret_Reward_Item>();
                retObj.count_list = new Dictionary<long, List<Ret_Reward_Item>>();
                retObj.over_list = new Dictionary<long, List<Ret_Reward_Item>>();

                List<System_Event_Daily> DailyEventList = GetSystem_Event_Daily_List(ref TB);

                bool checkEven = (checkMonth % 2) == 0;
                var CountList = DailyEventList.Where(
                                    ev => ev.ActiveType_Event > 0
                                    && (ev.Event_Daily_Type.Equals(Trigger_Define.DailyType[Trigger_Define.eEventDailyType.Count]))
                                    && (ev.Event_LoopType == (int)(checkEven ? Trigger_Define.eEventLoopType.Even_Month : Trigger_Define.eEventLoopType.Odd_Month))
                                    //|| ev.Event_Daily_Type.Equals(Trigger_Define.DailyType[Trigger_Define.eEventDailyType.Count]))
                                    ).OrderBy(ev => ev.Daily_Count);

                if (CountList.Count() < 1)
                {
                    CountList = DailyEventList.Where(
                                    ev => ev.ActiveType_Event > 0
                                    && (ev.Event_Daily_Type.Equals(Trigger_Define.DailyType[Trigger_Define.eEventDailyType.Count]))
                                    && (ev.Event_LoopType == (int)(Trigger_Define.eEventLoopType.Month))
                        //|| ev.Event_Daily_Type.Equals(Trigger_Define.DailyType[Trigger_Define.eEventDailyType.Count]))
                                    ).OrderBy(ev => ev.Daily_Count);
                }
                
                
                foreach (System_Event_Daily setEv in CountList)
                {
                    List< System_Event_Reward_Box> setOpenBoxList = new List<System_Event_Reward_Box>();
                    List<Ret_Reward_Item> reward_item = new List<Ret_Reward_Item>();

                    if (setEv.Reward_Box1ID > 0)
                        setOpenBoxList.AddRange(TriggerManager.GetSystem_Event_Reward_Box_List(ref TB, setEv.Reward_Box1ID));

                    foreach (System_Event_Reward_Box setBox in setOpenBoxList)
                    {
                        if(setBox.EventItem_ID > 0)
                            reward_item.Add(new Ret_Reward_Item(setBox));
                    }

                    if (!retObj.count_list.ContainsKey(setEv.Daily_Count))
                        retObj.count_list[setEv.Daily_Count] = reward_item;
                }

                var OverList = DailyEventList.Where(
                                    ev => ev.ActiveType_Event > 0
                                    && (ev.Event_Daily_Type.Equals(Trigger_Define.DailyType[Trigger_Define.eEventDailyType.Over]))
                                    ).OrderBy(ev => ev.Daily_Count);


                foreach (System_Event_Daily setEv in OverList)
                {
                    List<System_Event_Reward_Box> setOpenBoxList = new List<System_Event_Reward_Box>();
                    List<Ret_Reward_Item> reward_item = new List<Ret_Reward_Item>();

                    if (setEv.Reward_Box1ID > 0)
                        setOpenBoxList.AddRange(TriggerManager.GetSystem_Event_Reward_Box_List(ref TB, setEv.Reward_Box1ID));

                    foreach (System_Event_Reward_Box setBox in setOpenBoxList)
                    {
                        if (setBox.EventItem_ID > 0)
                            reward_item.Add(new Ret_Reward_Item(setBox));
                    }

                    if (!retObj.over_list.ContainsKey(setEv.Daily_Count))
                        retObj.over_list[setEv.Daily_Count] = reward_item;
                }

                var EveryDay = DailyEventList.Find(
                                    ev => ev.ActiveType_Event > 0
                                    && (ev.Event_Daily_Type.Equals(Trigger_Define.DailyType[Trigger_Define.eEventDailyType.Over]))
                                    );

                if (EveryDay != null)
                {
                    List<System_Event_Reward_Box> setOpenBoxList = new List<System_Event_Reward_Box>();
                    List<Ret_Reward_Item> reward_item = new List<Ret_Reward_Item>();

                    if (EveryDay.Reward_Box1ID > 0)
                        setOpenBoxList.AddRange(TriggerManager.GetSystem_Event_Reward_Box_List(ref TB, EveryDay.Reward_Box1ID));

                    foreach (System_Event_Reward_Box setBox in setOpenBoxList)
                    {
                        if (setBox.EventItem_ID > 0)
                            reward_item.Add(new Ret_Reward_Item(setBox));
                    }

                    retObj.everyday = reward_item;
                }

                RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_User, setKey, retObj);
            }

            return retObj;
        }

        public static Result_Define.eResult EventRewardMailSend(ref TxnBlock TB, long AID, int GetCount, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            User_Event_Check_Data userDailyInfo = TriggerManager.Check_User_Daily_Event(ref TB, AID);
            return EventRewardMailSend(ref TB, ref userDailyInfo, AID, GetCount, dbkey);
        }
        public static Result_Define.eResult EventRewardMailSend(ref TxnBlock TB, ref User_Event_Check_Data userDailyInfo, long AID, int GetCount, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            if (userDailyInfo.CheckCount < userDailyInfo.RewardCount + GetCount)
                return Result_Define.eResult.TRIGGER_EVENT_DAILY_REWARD_COUNT_OVER;

            int checkMonth = DateTime.Now.Month;
            //Ret_Daily_Event_List DailyEventList = GetDailyEvent_RewardList(ref TB);

            List<System_Event_Daily> DailyEventList = GetSystem_Event_Daily_List(ref TB);

            bool checkEven = (checkMonth % 2) == 0;
            var CountList = DailyEventList.Where(
                                ev => ev.ActiveType_Event > 0
                                && (ev.Event_Daily_Type.Equals(Trigger_Define.DailyType[Trigger_Define.eEventDailyType.Count]))
                                && (ev.Event_LoopType == (int)(checkEven ? Trigger_Define.eEventLoopType.Even_Month : Trigger_Define.eEventLoopType.Odd_Month))
                //|| ev.Event_Daily_Type.Equals(Trigger_Define.DailyType[Trigger_Define.eEventDailyType.Count]))
                                ).OrderBy(ev => ev.Daily_Count);

            if (CountList.Count() < 1)
            {
                CountList = DailyEventList.Where(
                                ev => ev.ActiveType_Event > 0
                                && (ev.Event_Daily_Type.Equals(Trigger_Define.DailyType[Trigger_Define.eEventDailyType.Count]))
                                && (ev.Event_LoopType == (int)(Trigger_Define.eEventLoopType.Month))
                    //|| ev.Event_Daily_Type.Equals(Trigger_Define.DailyType[Trigger_Define.eEventDailyType.Count]))
                                ).OrderBy(ev => ev.Daily_Count);
            }

            for (int dayCount = userDailyInfo.RewardCount+1; dayCount <= userDailyInfo.RewardCount + GetCount; dayCount++)
            {
                System_Event_Daily setEv = CountList.ToList().Find(
                                    ev => ev.ActiveType_Event > 0
                                    && (ev.Event_Daily_Type.Equals(Trigger_Define.DailyType[Trigger_Define.eEventDailyType.Count]))
                                    && ev.Daily_Count == dayCount);

                List<System_Event_Reward_Box> setOpenBoxList = new List<System_Event_Reward_Box>();
                List<Ret_Reward_Item> reward_item = new List<Ret_Reward_Item>();

                if (setEv.Reward_Box1ID > 0)
                    setOpenBoxList.AddRange(TriggerManager.GetSystem_Event_Reward_Box_List(ref TB, setEv.Reward_Box1ID));

                foreach (System_Event_Reward_Box setBox in setOpenBoxList)
                {
                    if (setBox.EventItem_ID > 0)
                        reward_item.Add(new Ret_Reward_Item(setBox));
                }

                //if (DailyEventList.count_list.ContainsKey(dayCount))
                //    reward_item = DailyEventList.count_list[dayCount];

                for (int i = reward_item.Count; i < Mail_Define.Mail_MaxItem; i++)
                {
                    reward_item.Add(new Ret_Reward_Item());
                }
                Result_Define.eResult retError = MailManager.SendMail(ref TB, AID, Mail_Define.Mail_System_Sender_AID, Mail_Define.Mail_System_Sender_Name, setEv.Reward_Mail_Text_CN, setEv.Reward_Mail_Subject_CN,
                    reward_item[0].itemid, reward_item[0].itemea, reward_item[0].itemgrade, reward_item[0].itemlevel,
                    reward_item[1].itemid, reward_item[1].itemea, reward_item[1].itemgrade, reward_item[1].itemlevel,
                    reward_item[2].itemid, reward_item[2].itemea, reward_item[2].itemgrade, reward_item[2].itemlevel,
                    reward_item[3].itemid, reward_item[3].itemea, reward_item[3].itemgrade, reward_item[3].itemlevel,
                    reward_item[4].itemid, reward_item[4].itemea, reward_item[4].itemgrade, reward_item[4].itemlevel
                    );
                if (retError != Result_Define.eResult.SUCCESS)
                    return retError;
            }

            MailManager.RemoveMailCache(AID);

            return UpdateEvent_Check_Data(ref TB, AID, userDailyInfo.CheckCount, userDailyInfo.RewardCount + GetCount, userDailyInfo.AddCount, userDailyInfo.VIPRewardList, userDailyInfo.FirstPaymentFlag);
        }


        public static Result_Define.eResult EventDailyRewardSend(ref TxnBlock TB, ref User_Event_Check_Data userDailyInfo, ref List<User_Inven> makeRealItem, long AID, long CID, int GetCount, bool isBuy = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            if (userDailyInfo.CheckCount < userDailyInfo.RewardCount + GetCount && !isBuy)
                return Result_Define.eResult.TRIGGER_EVENT_DAILY_REWARD_COUNT_OVER;

            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            //List<System_Event_Daily> DailyEventList = GetSystem_Event_Daily_List(ref TB);
            Ret_Daily_Event_List DailyEventList = GetDailyEvent_RewardList(ref TB);

            for (int dayCount = userDailyInfo.RewardCount + 1; (dayCount <= userDailyInfo.RewardCount + GetCount) && dayCount <= DailyEventList.count_list.Count; dayCount++)
            {
                //System_Event_Daily setEv = DailyEventList.Find(
                //                    ev => ev.ActiveType_Event > 0
                //                    && (ev.Event_Daily_Type.Equals(Trigger_Define.DailyType[Trigger_Define.eEventDailyType.Count]))
                //                    && ev.Daily_Count == dayCount);

                //if (setEv != null)
                if(DailyEventList.count_list.ContainsKey(dayCount))
                {
                    //List<System_Event_Reward_Box> setOpenBoxList = new List<System_Event_Reward_Box>();
                    List<Ret_Reward_Item> reward_item = DailyEventList.count_list[dayCount];

                    //if (setEv.Reward_Box1ID > 0)
                    //    setOpenBoxList.AddRange(TriggerManager.GetSystem_Event_Reward_Box_List(ref TB, setEv.Reward_Box1ID));

                    User_VIP UserVIP = VipManager.GetUser_VIPInfo(ref TB, AID);

                    //foreach (System_Event_Reward_Box setReward in setOpenBoxList)
                    //{
                    //    bool bMake = false;
                    //    List<User_Inven> makeItem = new List<User_Inven>();

                    //    int rewardCount = (UserVIP.viplevel >= setReward.VIP_Level && setReward.VIP_Level > 0) ? setReward.EventItem_Num * 2 : setReward.EventItem_Num;
                    //    //if (AID == 103 || AID == 28894)
                    //    //    setReward.EventItem_ID = 303040007;
                    //    retError = ItemManager.MakeItem(ref TB, ref makeItem, AID, setReward.EventItem_ID, rewardCount, CID, setReward.EventItem_Level, setReward.EventItem_Grade);
                    //    if (retError != Result_Define.eResult.SUCCESS)
                    //        return retError;
                    //    else
                    //        makeItem.ForEach(item => item.itemea = rewardCount);

                    //    makeRealItem.AddRange(makeItem);

                    //    if (makeRealItem.Count < 1 && bMake)
                    //        return Result_Define.eResult.TRIGGER_REWARD_EMPTY;
                    //}


                    foreach (Ret_Reward_Item setReward in reward_item)
                    {
                        List<User_Inven> makeItem = new List<User_Inven>();
                        int rewardCount = (UserVIP.viplevel >= setReward.viplevel && setReward.viplevel > 0) ? setReward.itemea * 2 : setReward.itemea;
                        retError = ItemManager.MakeItem(ref TB, ref makeItem, AID, setReward.itemid, rewardCount, CID, setReward.itemlevel, setReward.itemgrade);

                        if (retError != Result_Define.eResult.SUCCESS)
                            return retError;
                        else
                            makeItem.ForEach(item => item.itemea = rewardCount);

                        makeRealItem.AddRange(makeItem);
                    }

                    if (makeRealItem.Count < 1)
                        return Result_Define.eResult.TRIGGER_REWARD_EMPTY;
                }
            }

            int AddCount = isBuy ? GetCount : 0;
            //MailManager.RemoveMailCache(AID);

            //if (AID == 103 || AID == 28894)
            //    return Result_Define.eResult.SUCCESS;
            //else
                return UpdateEvent_Check_Data(ref TB, AID, userDailyInfo.RewardCount + GetCount, userDailyInfo.RewardCount + GetCount, userDailyInfo.AddCount + AddCount, userDailyInfo.VIPRewardList, userDailyInfo.FirstPaymentFlag);
        }
        

        public static Result_Define.eResult EventFirstPaymentRewardSend(ref TxnBlock TB, ref List<User_Inven> makeRealItem, long AID, long CID, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            System_Event_First_Payment getRewardInfo = TriggerManager.GetSystem_Event_First_Payment(ref TB);

            List<System_Event_Reward_Box> setOpenBoxList = new List<System_Event_Reward_Box>();
            //List<Ret_Reward_Item> reward_item = new List<Ret_Reward_Item>();
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            if (getRewardInfo.Reward_Box1ID > 0)
                setOpenBoxList.AddRange(TriggerManager.GetSystem_Event_Reward_Box_List(ref TB, getRewardInfo.Reward_Box1ID));

            List<Set_Mail_Item> mailItem = new List<Set_Mail_Item>();
            setOpenBoxList.ForEach(setItem => 
            {
                mailItem.Add(new Set_Mail_Item(setItem.EventItem_ID, setItem.EventItem_Num, setItem.EventItem_Grade, setItem.EventItem_Level));
            }
            );
            
            int sendItemCount = 0;
            List<Set_Mail_Item> setMailItem = new List<Set_Mail_Item>();
            foreach (Set_Mail_Item setItem in mailItem)
            {
                if (setItem.itemid > 0 && setItem.itemea > 0)
                {
                    sendItemCount++;
                    setMailItem.Add(setItem);
                }

                if (sendItemCount >= Mail_Define.Mail_MaxItem)
                {
                    retError = MailManager.SendMail(ref TB, ref setMailItem, AID, 0, "", getRewardInfo.Reward_Mail_Subject_CN, getRewardInfo.Reward_Mail_Text_CN, Mail_Define.Mail_VIP_CloseTime_Min);
                    if (retError == Result_Define.eResult.SUCCESS)
                    {
                        setMailItem.Clear();
                        sendItemCount = 0;
                    }
                    else
                        break;
                }
            }

            if (retError == Result_Define.eResult.SUCCESS && sendItemCount > 0)
            {
                retError = MailManager.SendMail(ref TB, ref setMailItem, AID, 0, "", getRewardInfo.Reward_Mail_Subject_CN, getRewardInfo.Reward_Mail_Text_CN);
            }

            //foreach (System_Event_Reward_Box setReward in setOpenBoxList)
            //{
            //    List<User_Inven> makeItem = new List<User_Inven>();
            //    retError = ItemManager.MakeItem(ref TB, ref makeItem, AID, setReward.EventItem_ID, setReward.EventItem_Num, CID, setReward.EventItem_Level, setReward.EventItem_Grade);
            //    if (retError != Result_Define.eResult.SUCCESS)
            //        return retError;

            //    makeRealItem.AddRange(makeItem);

            //}

            //if (makeRealItem.Count == 0)
            //    return Result_Define.eResult.TRIGGER_REWARD_EMPTY;

            User_Event_Check_Data userDailyInfo = TriggerManager.Check_User_Daily_Event(ref TB, AID);

            return UpdateEvent_Check_Data(ref TB, AID, userDailyInfo.CheckCount, userDailyInfo.RewardCount, userDailyInfo.AddCount, userDailyInfo.VIPRewardList, "Y");
        }

        public static Result_Define.eResult UpdateEvent_Check_Data(ref TxnBlock TB, long AID, int CheckCount, int RewardCount, int addCount, string vipRewardList, string RewardFlag, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON AID = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                    RegDate = GETDATE(),
                                                    CheckCount = '{2}',
                                                    RewardCount = '{3}',
                                                    AddCount = '{4}',
                                                    VIPRewardList = '{5}',
                                                    FirstPaymentFlag = '{6}'
                                                WHEN NOT MATCHED THEN
                                                   INSERT (AID, RegDate, CheckCount, RewardCount, AddCount, VIPRewardList, FirstPaymentFlag)
                                                   VALUES ('{1}', GETDATE(), '{2}', '{3}', '{4}', '{5}', '{6}');
                                    ", Trigger_Define.User_Event_Check_Data_TableName
                                     , AID, CheckCount, RewardCount, addCount, vipRewardList, RewardFlag);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        // for Event List Control
        private static User_Event_Data InsertUser_Event_Data(ref TxnBlock TB, long AID, System_Event setEvent, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            User_Event_Data retObj = new User_Event_Data();

            SqlCommand commandUser_EventInfo = new SqlCommand();
            commandUser_EventInfo.CommandText = "System_Get_User_Event_Data";
            var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            commandUser_EventInfo.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            commandUser_EventInfo.Parameters.Add("@Event_ID", SqlDbType.NVarChar, 128).Value = setEvent.Event_ID;
            commandUser_EventInfo.Parameters.Add(result);

            SqlDataReader getDr = null;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandUser_EventInfo, ref getDr))
            {
                if (getDr != null)
                {
                    var r = SQLtoJson.Serialize(ref getDr);
                    string json = mJsonSerializer.ToJsonString(r);
                    getDr.Dispose(); getDr.Close();
                    int checkValue = System.Convert.ToInt32(result.Value);
                    commandUser_EventInfo.Dispose();

                    if (checkValue < 0)
                        return retObj;

                    User_Event_Data[] retSet = mJsonSerializer.JsonToObject<User_Event_Data[]>(json);

                    if (retSet.Length > 0)
                        retObj = retSet[0];
                    else
                        return retObj;

                    retObj.isActive = true;
                    retObj.isStart = true;
                    retObj.isFail = false;

                    string setTaskLogJson = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.trigger_id_list]);
                    TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_task_log]);
                    TaskLogInfo setLog = new TaskLogInfo(retObj.Event_ID, (int)SnailLog_Define.Snail_Task_Act_type.start, Trigger_Define.eEventListType.Event);
                    setTaskLogJson = mJsonSerializer.AddJsonArray(setTaskLogJson, mJsonSerializer.ToJsonString(setLog));
                    TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.trigger_id_list], setTaskLogJson);
                }
            }
            else
            {
                commandUser_EventInfo.Dispose();
            }
            return retObj;
        }

        public static User_Event_Data Check_Event_Data_Info(ref TxnBlock TB, long AID, long EventIdx, bool Flush = false)
        {
            List<User_Event_Data> getEventList = Check_Event_Data_List(ref TB, AID, Flush);
            User_Event_Data getEvent = getEventList.Find(eventinfo => eventinfo.User_Event_ID == EventIdx);

            return getEvent == null ? new User_Event_Data() : getEvent;
        }

        public static List<User_Event_Data> Check_Event_Data_List(ref TxnBlock TB, long AID, bool Flush = false, List<Character> userCharacter = null, List<RetMissionRank> userMission = null, List<User_GuerrillaDungeon_Play> userGuerillaMission = null, List<RetEliteDungeonRank> userElistMission = null)
        {
            List<User_Event_Data> userEventInfo = GetUser_Event_Data_List(ref TB, AID, Flush);
            List<System_Event> systemEventInfo = GetSystem_Event_List(ref TB);            

            List<User_Event_Data> setEventList = new List<User_Event_Data>();
            userCharacter = userCharacter == null ? CharacterManager.GetCharacterList(ref TB, AID) : userCharacter;
            userMission = userMission == null ? Dungeon_Manager.GetUser_All_MissionRank(ref TB, AID) : userMission;
            userGuerillaMission = userGuerillaMission == null ? Dungeon_Manager.GetUser_All_GuerrillaDungeonRank(ref TB, AID) : userGuerillaMission;
            userElistMission = userElistMission == null ? Dungeon_Manager.GetUser_All_EliteDungeonRank(ref TB, AID) : userElistMission;

            bool isFlush = Flush;

            foreach (System_Event setItem in systemEventInfo)
            {
                User_Event_Data findObj = userEventInfo.Find(item => item.Event_ID == setItem.Event_ID);
                bool isUpdated = false;
                bool isDataChange = false;
                bool checkActive1 = true;
                bool checkActive2 = true;
                if (findObj != null && findObj.RewardFlag.Equals("Y"))
                {
                    checkActive1 = checkActive2 = true;
                }else
                {                 
                    checkActive1 = TriggerManager.CheckClearTrigger(ref TB, AID, Trigger_Define.TriggerType[setItem.ActiveTriggerType1],
                                                    setItem.ActiveTriggerType1_Value1, setItem.ActiveTriggerType1_Value2, setItem.ActiveTriggerType1_Value3,
                                                    userCharacter, userMission, userGuerillaMission, userElistMission);
                    checkActive2 = TriggerManager.CheckClearTrigger(ref TB, AID, Trigger_Define.TriggerType[setItem.ActiveTriggerType2],
                                                    setItem.ActiveTriggerType2_Value1, setItem.ActiveTriggerType2_Value2, setItem.ActiveTriggerType2_Value3,
                                                    userCharacter, userMission, userGuerillaMission, userElistMission);
                }
                                
                if (findObj == null && checkActive1 && checkActive2)
                {
                    findObj = InsertUser_Event_Data(ref TB, AID, setItem);
                    if (findObj != null)
                    {
                        if (findObj.ClearTriggerType1.Equals(Trigger_Define.TriggerString[Trigger_Define.eTriggerType.Game_Access])
                            || findObj.ClearTriggerType2.Equals(Trigger_Define.TriggerString[Trigger_Define.eTriggerType.Game_Access]))
                        {
                            findObj.StartTime = findObj.StartTime.AddDays(-1);
                            if (UpdateUserEvent(ref TB, findObj, isUpdated, Trigger_Define.eEventListType.Event, false) == Result_Define.eResult.SUCCESS)
                                isFlush = true;
                        }
                        isFlush = true;
                    }
                }
                
                if (findObj != null)
                {
                    isDataChange =
                            findObj.Event_Type != setItem.Event_Type
                            || findObj.EndTime.ToString("yyyy-MM-dd HH:mm:ss") != setItem.Event_EndTime.ToString("yyyy-MM-dd HH:mm:ss")
                            || findObj.Event_LoopType != setItem.Event_LoopType
                            || findObj.ClearTriggerType1 != setItem.ClearTriggerType1
                            || findObj.ClearTriggerType1_Value1 != setItem.ClearTriggerType1_Value1
                            || findObj.ClearTriggerType1_Value2 != setItem.ClearTriggerType1_Value2
                            || findObj.ClearTriggerType1_Value3 != setItem.ClearTriggerType1_Value3
                            || findObj.ClearTriggerType2 != setItem.ClearTriggerType2
                            || findObj.ClearTriggerType2_Value1 != setItem.ClearTriggerType2_Value1
                            || findObj.ClearTriggerType2_Value2 != setItem.ClearTriggerType2_Value2
                            || findObj.ClearTriggerType2_Value3 != setItem.ClearTriggerType2_Value3
                            ;
                    if (isDataChange)
                    {
                        findObj.Event_Type = setItem.Event_Type;
                        findObj.Event_LoopType = setItem.Event_LoopType;
                        findObj.ClearTriggerType1 = setItem.ClearTriggerType1;
                        findObj.ClearTriggerType1_Value1 = setItem.ClearTriggerType1_Value1;
                        findObj.ClearTriggerType1_Value2 = setItem.ClearTriggerType1_Value2;
                        findObj.ClearTriggerType1_Value3 = setItem.ClearTriggerType1_Value3;
                        findObj.ClearTriggerType2 = setItem.ClearTriggerType2;
                        findObj.ClearTriggerType2_Value1 = setItem.ClearTriggerType2_Value1;
                        findObj.ClearTriggerType2_Value2 = setItem.ClearTriggerType2_Value2;
                        findObj.ClearTriggerType2_Value3 = setItem.ClearTriggerType2_Value3;
                        findObj.CurrentValue1 = 0;
                        findObj.CurrentValue2 = 0;
                        findObj.EndTime = setItem.Event_EndTime;
                        findObj.RewardFlag = "N";
                        findObj.ClearFlag = "P";
                        isUpdated = true;
                    }
                }

                if (findObj != null && (setItem.Event_EndTime != findObj.EndTime || isDataChange))
                {
                    if (UpdateUserEventEndTime(ref TB, findObj.User_Event_ID, setItem.Event_EndTime, true) == Result_Define.eResult.SUCCESS)
                        isFlush = true;
                    isUpdated = true;
                }

                if (findObj != null && checkActive1 && checkActive2)
                {
                    if (CheckLoopTime(ref findObj) && findObj.RewardFlag.Equals("Y"))
                    {
                        findObj.RewardFlag = "N";
                        findObj.ClearFlag = "P";
                        findObj.StartTime = DateTime.Now;
                        findObj.CurrentValue1 = 0;
                        findObj.CurrentValue2 = 0;
                        isUpdated = true;
                    }
                    findObj.isActive = true;
                }else if(findObj != null)
                    findObj.isActive = false;

                if (findObj != null)
                {
                    if (isUpdated || isDataChange)
                    {
                        if (UpdateUserEvent(ref TB, findObj, isUpdated, Trigger_Define.eEventListType.Event, isDataChange) == Result_Define.eResult.SUCCESS)
                            isFlush = true;
                    }

                    if (findObj.isActive)
                        setEventList.Add(findObj);
                }
            }

            var deactiveList = userEventInfo.Where(item => !item.isActive);

            foreach (User_Event_Data deactiveItem in deactiveList)
            {
                if (UpdateUserEventEndTime(ref TB, deactiveItem.User_Event_ID, DateTime.Now.AddSeconds(-1)) == Result_Define.eResult.SUCCESS)
                    isFlush = true;
                //if (UpdateEvent_Data(ref TB, AID, deactiveItem.User_Event_ID, Trigger_Define.eClearType.End, deactiveItem.RewardFlag, true) == Result_Define.eResult.SUCCESS)
                //    isFlush = true;
            }

            if (isFlush || Flush)
                GetUser_Event_Data_List(ref TB, AID, true);

            return setEventList;
        }

        public static void RemoveEventDataFromRedis(long AID, bool isSystem = false)
        {
            string setKey = "";
            if (isSystem)
            {
                setKey = string.Format("{0}_{1}", Trigger_Define.Trigger_Prefix, Trigger_Define.System_Event_TableName);
                RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_System, setKey);
            }
            setKey = string.Format("{0}_{1}_{2}", Trigger_Define.User_Event_Prefix, Trigger_Define.User_Event_Data_TableName, AID);
            RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }


        // For Achieve List Control
        private static User_Event_Data InsertUser_Achieve_Data(ref TxnBlock TB, long AID, System_Achieve setAchieve, Trigger_Define.eEventListType eventType, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            User_Event_Data retObj = new User_Event_Data();

            SqlCommand commandUser_AchieveInfo = new SqlCommand();
            commandUser_AchieveInfo.CommandText = eventType == Trigger_Define.eEventListType.Achive ? "System_Get_User_Achieve_Data" :
                                                    eventType == Trigger_Define.eEventListType.PvP_Achive ? "System_Get_User_Achieve_PvP_Data" :
                                                        string.Empty;
            if (string.IsNullOrEmpty(commandUser_AchieveInfo.CommandText))
                return retObj;
            var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            commandUser_AchieveInfo.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            commandUser_AchieveInfo.Parameters.Add("@Event_ID", SqlDbType.NVarChar, 128).Value = setAchieve.AchieveID;
            commandUser_AchieveInfo.Parameters.Add(result);

            SqlDataReader getDr = null;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandUser_AchieveInfo, ref getDr))
            {
                if (getDr != null)
                {
                    var r = SQLtoJson.Serialize(ref getDr);
                    string json = mJsonSerializer.ToJsonString(r);
                    getDr.Dispose(); getDr.Close();
                    int checkValue = System.Convert.ToInt32(result.Value);
                    commandUser_AchieveInfo.Dispose();
                    if (checkValue < 0)
                        return retObj;

                    User_Event_Data[] retSet = mJsonSerializer.JsonToObject<User_Event_Data[]>(json);

                    if (retSet.Length > 0)
                        retObj = retSet[0];
                    else
                        return retObj;

                    retObj.isActive = true;
                    retObj.isStart = true;
                    retObj.isFail = false;

                    string setTaskLogJson = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.trigger_id_list]);
                    TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_task_log]);
                    TaskLogInfo setLog = new TaskLogInfo(retObj.Event_ID, (int)SnailLog_Define.Snail_Task_Act_type.start, eventType);
                    setTaskLogJson = mJsonSerializer.AddJsonArray(setTaskLogJson, mJsonSerializer.ToJsonString(setLog));
                    TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.trigger_id_list], setTaskLogJson);
                }
            }
            else
            {
                commandUser_AchieveInfo.Dispose();
            }
            return retObj;
        }


//        public static Result_Define.eResult UpdateAchieve_Data(ref TxnBlock TB, long AID, long User_Achieve_ID, string dbkey = Trigger_Define.Trigger_Info_DB)
//        {
//            // hard coding for field name set
//            string setQuery = string.Format(@"UPDATE {0} SET 
//                                                    ClearFlag = '{1}'
//                                                WHERE AID = {2} AND User_Achieve_ID = {3} ",
//                                                    Trigger_Define.User_Achieve_Data_TableName
//                                                    , Trigger_Define.ClearFlag[Trigger_Define.eClearType.End]
//                                                    , AID
//                                                    , User_Achieve_ID
//                                                    );
//            if (TB.ExcuteSqlCommand(dbkey, setQuery))
//                return Result_Define.eResult.SUCCESS;
//            else
//                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
//        }

        public static User_Event_Data Check_Achieve_PvP_Data_Info(ref TxnBlock TB, long AID, long EventIdx, bool Flush)
        {
            List<User_Event_Data> getEventList = Check_Achieve_PvP_Data_List(ref TB, AID, Flush);
            User_Event_Data getEvent = getEventList.Find(eventinfo => eventinfo.User_Event_ID == EventIdx);

            return getEvent == null ? new User_Event_Data() : getEvent;
        }

        public static List<User_Event_Data> Check_Achieve_PvP_Data_List(ref TxnBlock TB, long AID, bool Flush = false, List<Character> userCharacter = null, List<RetMissionRank> userMission = null, List<User_GuerrillaDungeon_Play> userGuerillaMission = null, List<RetEliteDungeonRank> userElistMission = null)
        {
            return Check_Achieve_Data_List(ref TB, AID, true, Flush, userCharacter, userMission, userGuerillaMission, userElistMission);
        }

        public static User_Event_Data Check_Achieve_Data_Info(ref TxnBlock TB, long AID, long EventIdx, bool Flush)
        {
            List<User_Event_Data> getEventList = Check_Achieve_Data_List(ref TB, AID, Flush);
            User_Event_Data getEvent = getEventList.Find(eventinfo => eventinfo.User_Event_ID == EventIdx);

            return getEvent == null ? new User_Event_Data() : getEvent;
        }

        public static List<User_Event_Data> Check_Achieve_Data_List(ref TxnBlock TB, long AID, bool Flush = false, List<Character> userCharacter = null, List<RetMissionRank> userMission = null, List<User_GuerrillaDungeon_Play> userGuerillaMission = null, List<RetEliteDungeonRank> userElistMission = null)
        {
            return Check_Achieve_Data_List(ref TB, AID, false, Flush, userCharacter, userMission, userGuerillaMission, userElistMission);
        }

        public static List<User_Event_Data> Check_Achieve_Data_List(ref TxnBlock TB, long AID, bool isPvPAchive, bool Flush, List<Character> userCharacter = null, List<RetMissionRank> userMission = null, List<User_GuerrillaDungeon_Play> userGuerillaMission = null, List<RetEliteDungeonRank> userElistMission = null)
        {
            List<User_Event_Data> userAchieveInfo = isPvPAchive ? GetUser_Achieve_PvP_Data_List(ref TB, AID, Flush) : GetUser_Achieve_Data_List(ref TB, AID, Flush);
            List<System_Achieve_PvP> systemAchieveInfo = isPvPAchive ? GetSystem_Achieve_PvP_List(ref TB) : GetSystem_Achieve_List(ref TB).ConvertAll<System_Achieve_PvP>(achive => System_Achieve_PvP.CastToSystem_Achieve_PvP(achive));

            List<User_Event_Data> setAchieveList = new List<User_Event_Data>();
            userCharacter = userCharacter == null ? CharacterManager.GetCharacterList(ref TB, AID) : userCharacter;
            userMission = userMission == null ? Dungeon_Manager.GetUser_All_MissionRank(ref TB, AID) : userMission;
            userGuerillaMission = userGuerillaMission == null ? Dungeon_Manager.GetUser_All_GuerrillaDungeonRank(ref TB, AID) : userGuerillaMission;
            userElistMission = userElistMission == null ? Dungeon_Manager.GetUser_All_EliteDungeonRank(ref TB, AID) : userElistMission;            

            bool isFlush = Flush;
            Trigger_Define.eEventListType eventType = isPvPAchive ? Trigger_Define.eEventListType.PvP_Achive : Trigger_Define.eEventListType.Achive;
            foreach (System_Achieve_PvP setItem in systemAchieveInfo)
            {
                User_Event_Data findObj = userAchieveInfo.Find(item => item.Event_ID == setItem.AchieveID);
                bool isUpdated = false;
                bool isDataChange = false;
                bool bFindPre = (setItem.Require_AchieveID == 0);

                if (!bFindPre)
                {
                    User_Event_Data findBefore = userAchieveInfo.Find(item =>
                        (item.Event_ID == setItem.Require_AchieveID
                        && item.ClearFlag.Equals(Trigger_Define.ClearFlag[Trigger_Define.eClearType.Clear])
                        && item.RewardFlag.Equals("Y")
                        ));
                    bFindPre = (findBefore != null);
                }

                bool checkActive1 = true;
                bool checkActive2 = true;
                if (findObj != null && findObj.RewardFlag.Equals("Y"))
                {
                    checkActive1 = checkActive2 = (setItem.Event_LoopType != (int)Trigger_Define.eEventLoopType.Continue);
                }
                else
                {
                    checkActive1 = TriggerManager.CheckClearTrigger(ref TB, AID, Trigger_Define.TriggerType[setItem.ActiveTriggerType1],
                                                    setItem.ActiveTriggerType1_Value1, setItem.ActiveTriggerType1_Value2, setItem.ActiveTriggerType1_Value3, 
                                                    userCharacter, userMission, userGuerillaMission, userElistMission);
                    checkActive2 = TriggerManager.CheckClearTrigger(ref TB, AID, Trigger_Define.TriggerType[setItem.ActiveTriggerType2],
                                                    setItem.ActiveTriggerType2_Value1, setItem.ActiveTriggerType2_Value2, setItem.ActiveTriggerType2_Value3,
                                                    userCharacter, userMission, userGuerillaMission, userElistMission);
                }

                if (findObj == null && bFindPre && checkActive1 && checkActive2)
                {
                    findObj = InsertUser_Achieve_Data(ref TB, AID, setItem, eventType);
                    if (findObj != null)
                    {
                        if (findObj.ClearTriggerType1.Equals(Trigger_Define.TriggerString[Trigger_Define.eTriggerType.Game_Access])
                        || findObj.ClearTriggerType2.Equals(Trigger_Define.TriggerString[Trigger_Define.eTriggerType.Game_Access]))
                        {
                            findObj.StartTime = findObj.StartTime.AddDays(-1);
                            if (UpdateUserEvent(ref TB, findObj, isUpdated, eventType, false) == Result_Define.eResult.SUCCESS)
                                isFlush = true;
                        }
                        isFlush = true;
                    }
                }

                if (findObj != null)
                {
                    isDataChange = findObj.ClearTriggerType1 != setItem.ClearTriggerType1
                            || findObj.ClearTriggerType1_Value1 != setItem.ClearTriggerType1_Value1
                            || findObj.ClearTriggerType1_Value2 != setItem.ClearTriggerType1_Value2
                            || findObj.ClearTriggerType1_Value3 != setItem.ClearTriggerType1_Value3
                            || findObj.ClearTriggerType2 != setItem.ClearTriggerType2
                            || findObj.ClearTriggerType2_Value1 != setItem.ClearTriggerType2_Value1
                            || findObj.ClearTriggerType2_Value2 != setItem.ClearTriggerType2_Value2
                            || findObj.ClearTriggerType2_Value3 != setItem.ClearTriggerType2_Value3
                            ;

                    if (isDataChange)
                    {
                        findObj.ClearTriggerType1 = setItem.ClearTriggerType1;
                        findObj.ClearTriggerType1_Value1 = setItem.ClearTriggerType1_Value1;
                        findObj.ClearTriggerType1_Value2 = setItem.ClearTriggerType1_Value2;
                        findObj.ClearTriggerType1_Value3 = setItem.ClearTriggerType1_Value3;
                        findObj.ClearTriggerType2 = setItem.ClearTriggerType2;
                        findObj.ClearTriggerType2_Value1 = setItem.ClearTriggerType2_Value1;
                        findObj.ClearTriggerType2_Value2 = setItem.ClearTriggerType2_Value2;
                        findObj.ClearTriggerType2_Value3 = setItem.ClearTriggerType2_Value3;
                    }
                }

                if (findObj != null && checkActive1 && checkActive2)
                {
                    if (findObj != null)
                    {
                        if (CheckLoopTime(ref findObj) && findObj.RewardFlag.Equals("Y"))
                        {
                            findObj.RewardFlag = "N";
                            findObj.StartTime = DateTime.Now;
                            isUpdated = true;
                        }
                    }
                    findObj.isActive = true;
                }
                else if (findObj != null)
                    findObj.isActive = false;
                
                if (bFindPre && checkActive1 && checkActive2 && findObj != null)
                {
                    findObj.isActive = true;

                    if (setItem.Require_AchieveID > 0 && setItem.Event_LoopType == (int)Trigger_Define.eEventLoopType.Continue)
                        setAchieveList.Remove(setAchieveList.Find(item => item.Event_ID == setItem.Require_AchieveID));
                }
                else if(findObj != null)
                    findObj.isActive = false;

                if (findObj != null)
                {
                    if (isUpdated || isDataChange)
                    {
                        if (UpdateUserEvent(ref TB, findObj, isUpdated, eventType, isDataChange) == Result_Define.eResult.SUCCESS)
                            isFlush = true;
                    }

                    if (findObj.isActive)
                    {
                        if (isPvPAchive)
                        {
                            findObj.PVP_Type = setItem.PVP_Type;
                            findObj.Ranking_List_Type = setItem.Ranking_List_Type;
                        }

                        setAchieveList.Add(findObj);
                    }
                }
            }

            //var deactiveList = userAchieveInfo.Where(item => !item.isActive);

            //foreach (User_Event_Data deactiveItem in deactiveList)
            //{
            //    deactiveItem.isFail = true;
            //    deactiveItem.RewardFlag = "N";
            //    if (UpdateEvent_Data(ref TB, AID, deactiveItem.User_Event_ID, Trigger_Define.eClearType.End, deactiveItem.RewardFlag, false) == Result_Define.eResult.SUCCESS)
            //        isFlush = true;
            //}

            if (isFlush || Flush)
            {
                RemoveAchieveDataFromRedis(AID, isPvPAchive);
            }                

            return setAchieveList;
        }

        public static void RemoveAchieveDataFromRedis(long AID, bool isPvPAchive)
        {
            string setKey = GetRedisKey_UserAchive(AID, isPvPAchive);
            RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }

        public static string GetRedisKey_UserAchive(long AID, bool isPvPAchieve)
        {
            return string.Format("{0}_{1}_{2}", Trigger_Define.User_Event_Prefix, isPvPAchieve ? Trigger_Define.User_Achieve_Data_PvP_TableName : Trigger_Define.User_Achieve_Data_TableName, AID);
        }

        public static Result_Define.eResult UpdateUserEventEndTime(ref TxnBlock TB, long UserEventIDX, DateTime SetEndTime, bool isEvent = true, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET EndTime = '{2}' WHERE User_Event_ID = {1}",
                                                isEvent ?  Trigger_Define.User_Event_Data_TableName : Trigger_Define.User_Achieve_Data_TableName,
                                                UserEventIDX,
                                                SetEndTime.ToString("yyyy-MM-dd HH:mm:ss")
                                                );

            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult UpdateUserEvent(ref TxnBlock TB, User_Event_Data SetEvent, bool timeUpdate, Trigger_Define.eEventListType isEvent, bool isDataChange, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string settimeUpdate = timeUpdate ? string.Format(", StartTime = GETDATE(), EndTime = '{1}', ClearFlag = '{0}', RewardFlag = 'N'", Trigger_Define.ClearFlag[Trigger_Define.eClearType.Playing], SetEvent.EndTime.ToString("yyyy-MM-dd HH:mm:ss")) : string.Empty;
            string setDataUpdate = isDataChange ? string.Format(@", ClearTriggerType1 = '{0}'
                                                                , ClearTriggerType1_Value1 = {1}
                                                                , ClearTriggerType1_Value2 = {2}
                                                                , ClearTriggerType1_Value3 = {3}
                                                                , ClearTriggerType2 = '{4}'
                                                                , ClearTriggerType2_Value1 = {5}
                                                                , ClearTriggerType2_Value2 = {6}
                                                                , ClearTriggerType2_Value3 = {7}
                                                                , Event_LoopType = {8}
                                                                {9}
                                                                "
                                                                , SetEvent.ClearTriggerType1
                                                                , SetEvent.ClearTriggerType1_Value1
                                                                , SetEvent.ClearTriggerType1_Value2
                                                                , SetEvent.ClearTriggerType1_Value3
                                                                , SetEvent.ClearTriggerType2
                                                                , SetEvent.ClearTriggerType2_Value1
                                                                , SetEvent.ClearTriggerType2_Value2
                                                                , SetEvent.ClearTriggerType2_Value3
                                                                , SetEvent.Event_LoopType
                                                                , isEvent == Trigger_Define.eEventListType.Event ? string.Format(", Event_Type = '{0}'", SetEvent.Event_Type) : string.Empty
                                                                )
                                                                 : string.Empty;

            if (SetEvent.ClearTriggerType1.Equals(Trigger_Define.TriggerString[Trigger_Define.eTriggerType.Game_Access])
                || SetEvent.ClearTriggerType2.Equals(Trigger_Define.TriggerString[Trigger_Define.eTriggerType.Game_Access]))
            {
                settimeUpdate = timeUpdate ? string.Format(", StartTime = '{0}', ClearFlag = '{1}', RewardFlag = 'N'", DateTime.Parse(SetEvent.StartTime.ToShortDateString()).ToString("yyyy-MM-dd HH:mm:ss"), Trigger_Define.ClearFlag[Trigger_Define.eClearType.Playing])
                    : string.Format(", StartTime = '{0}'", DateTime.Parse(SetEvent.StartTime.ToShortDateString()).ToString("yyyy-MM-dd HH:mm:ss"));
            }
            string setTable = isEvent == Trigger_Define.eEventListType.Event ? Trigger_Define.User_Event_Data_TableName :
                                    isEvent == Trigger_Define.eEventListType.Achive ? Trigger_Define.User_Achieve_Data_TableName :
                                    isEvent == Trigger_Define.eEventListType.PvP_Achive ? Trigger_Define.User_Achieve_Data_PvP_TableName : string.Empty;

            if (string.IsNullOrEmpty(setTable))
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            string setQuery = string.Format(@"UPDATE {0} SET CurrentValue1 = {1}, CurrentValue2 = {2} {3} {4} WHERE User_Event_ID = {5}",
                                                setTable,
                                                SetEvent.CurrentValue1, SetEvent.CurrentValue2,
                                                settimeUpdate,
                                                setDataUpdate,
                                                SetEvent.User_Event_ID
                                                );
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            if (TB.ExcuteSqlCommand(dbkey, setQuery))
                retError = Result_Define.eResult.SUCCESS;
            else
                retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            string setTaskLogJson = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.trigger_id_list]);

            if (SetEvent.isStart)
            {
                TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_task_log]);
                TaskLogInfo setLog = new TaskLogInfo(SetEvent.Event_ID, (int)SnailLog_Define.Snail_Task_Act_type.start, isEvent);
                setTaskLogJson = mJsonSerializer.AddJsonArray(setTaskLogJson, mJsonSerializer.ToJsonString(setLog));
            }
            if (SetEvent.isFail)
            {
                TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_task_log]);
                TaskLogInfo setLog = new TaskLogInfo(SetEvent.Event_ID, (int)SnailLog_Define.Snail_Task_Act_type.fail, isEvent);
                setTaskLogJson = mJsonSerializer.AddJsonArray(setTaskLogJson, mJsonSerializer.ToJsonString(setLog));
            }
            TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.trigger_id_list], setTaskLogJson);

            return retError;
        }

        public static Result_Define.eResult UpdateEvent_Data(ref TxnBlock TB, long AID, long Event_ID, long User_Event_ID, Trigger_Define.eClearType eventFlag, string RewardFlag, Trigger_Define.eEventListType isEvent = Trigger_Define.eEventListType.Event, Trigger_Define.eEventLoopType checkLoop = Trigger_Define.eEventLoopType.None, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setQuery;
            string setTaskLogJson = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.trigger_id_list]);
            string setTable = isEvent == Trigger_Define.eEventListType.Event ? Trigger_Define.User_Event_Data_TableName :
                                    isEvent == Trigger_Define.eEventListType.Achive ? Trigger_Define.User_Achieve_Data_TableName :
                                    isEvent == Trigger_Define.eEventListType.PvP_Achive ? Trigger_Define.User_Achieve_Data_PvP_TableName : string.Empty;

            if (string.IsNullOrEmpty(setTable))
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            if (checkLoop == Trigger_Define.eEventLoopType.Repeat && eventFlag == Trigger_Define.eClearType.Clear)
            {
                setQuery = string.Format(@"UPDATE {0} SET 
                                                    ClearFlag = '{1}',
                                                    RewardFlag = '{2}',
                                                    CurrentValue1 = 0,
                                                    CurrentValue2 = 0
                                                WHERE AID = {3} AND User_Event_ID = {4} ",
                                        setTable
                                        , Trigger_Define.ClearFlag[Trigger_Define.eClearType.Playing]
                                        , "N"
                                        , AID
                                        , User_Event_ID
                                        );

                // task log success , start
                TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_task_log]);
                TaskLogInfo setLog = new TaskLogInfo(Event_ID, (int)SnailLog_Define.Snail_Task_Act_type.success, isEvent);
                setTaskLogJson = mJsonSerializer.AddJsonArray(setTaskLogJson, mJsonSerializer.ToJsonString(setLog));
                setLog.act_type = (int)SnailLog_Define.Snail_Task_Act_type.start;
                setTaskLogJson = mJsonSerializer.AddJsonArray(setTaskLogJson, mJsonSerializer.ToJsonString(setLog));
            }
            else if (checkLoop == Trigger_Define.eEventLoopType.None || checkLoop == Trigger_Define.eEventLoopType.Continue)
            {
                // hard coding for field name set
                setQuery = string.Format(@"UPDATE {0} SET 
                                                    ClearFlag = '{1}',
                                                    RewardFlag = '{2}'
                                                WHERE AID = {3} AND User_Event_ID = {4} ",
                                                        setTable
                                                        , Trigger_Define.ClearFlag[eventFlag]
                                                        , RewardFlag
                                                        , AID
                                                        , User_Event_ID
                                                        );

                if (eventFlag == Trigger_Define.eClearType.Clear)
                {
                    TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_task_log]);
                    TaskLogInfo setLog = new TaskLogInfo(Event_ID, (int)SnailLog_Define.Snail_Task_Act_type.success, isEvent);
                    setTaskLogJson = mJsonSerializer.AddJsonArray(setTaskLogJson, mJsonSerializer.ToJsonString(setLog));
                }
            }
            else
            {
                // hard coding for field name set
                setQuery = string.Format(@"UPDATE {0} SET 
                                                    StartTime = GETDATE(),
                                                    ClearFlag = '{1}',
                                                    RewardFlag = '{2}'
                                                WHERE AID = {3} AND User_Event_ID = {4} ",
                                                        setTable
                                                        , Trigger_Define.ClearFlag[eventFlag]
                                                        , RewardFlag
                                                        , AID
                                                        , User_Event_ID
                                                        );

                if (eventFlag == Trigger_Define.eClearType.Clear)
                {
                    TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_task_log]);
                    TaskLogInfo setLog = new TaskLogInfo(Event_ID, (int)SnailLog_Define.Snail_Task_Act_type.success, isEvent);
                    setTaskLogJson = mJsonSerializer.AddJsonArray(setTaskLogJson, mJsonSerializer.ToJsonString(setLog));
                }
            }
            TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.trigger_id_list], setTaskLogJson);

            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            if (TB.ExcuteSqlCommand(dbkey, setQuery))
                retError = Result_Define.eResult.SUCCESS;
            else
                retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            return retError;
        }

        public static bool CheckLoopTime(ref User_Event_Data setEvent)
        {
            Trigger_Define.eEventLoopType checkLoop = (Trigger_Define.eEventLoopType)setEvent.Event_LoopType;
            bool bLoop = false;
            switch (checkLoop)
            {
                case Trigger_Define.eEventLoopType.None:
                case Trigger_Define.eEventLoopType.Continue:
                    break;
                case Trigger_Define.eEventLoopType.Day:
                case Trigger_Define.eEventLoopType.Week:
                case Trigger_Define.eEventLoopType.Month:
                {
                    DateTime startDate = DateTime.Parse(setEvent.StartTime.ToShortDateString());
                    DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                                                
                    if (checkLoop == Trigger_Define.eEventLoopType.Day)
                    {
                        TimeSpan TS = dbDate - startDate;
                        if (TS.Days != 0)
                            bLoop = true;
                    }
                    else if (checkLoop == Trigger_Define.eEventLoopType.Week)
                    {
                        int startWeek = PvPManager.GetSeperater_Week(startDate);
                        int currentWeek = PvPManager.GetSeperater_Week(dbDate);
                        
                        //DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                        //Calendar cal = dfi.Calendar;

                        //int startWeek = cal.GetWeekOfYear(startDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
                        //int currentWeek = cal.GetWeekOfYear(dbDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
                        // firstweek set monday;
                        //int startWeek = cal.GetWeekOfYear(startDate, dfi.CalendarWeekRule, DayOfWeek.Monday);
                        //int currentWeek = cal.GetWeekOfYear(dbDate, dfi.CalendarWeekRule, DayOfWeek.Monday);

                        // TODO : check over year check
                        if (startWeek != currentWeek)
                            bLoop = true;
                    }
                    else if (checkLoop == Trigger_Define.eEventLoopType.Month)
                    {
                        if (startDate.Year != dbDate.Year || startDate.Month != dbDate.Month)
                            bLoop = true;
                    }
                    break;
                }
            }

            if (bLoop)
            {
                setEvent.CurrentValue1 = 0;
                setEvent.CurrentValue2 = 0;
                setEvent.isFail = !setEvent.ClearFlag.Equals(Trigger_Define.ClearFlag[Trigger_Define.eClearType.Clear]);
                setEvent.isStart = true;
            }

            return bLoop;
        }

        public static Result_Define.eResult ProgressTrigger(ref TxnBlock TB, long AID, Trigger_Define.eTriggerType TriggerType, long UserValue1 = 0, long UserValue2 = 0, long UserValue3 = 1)
        {
            List<Character> userCharacter = CharacterManager.GetCharacterList(ref TB, AID);
            List<RetMissionRank> userMission = Dungeon_Manager.GetUser_All_MissionRank(ref TB, AID);
            List<User_GuerrillaDungeon_Play> userGuerillaMission = Dungeon_Manager.GetUser_All_GuerrillaDungeonRank(ref TB, AID);
            List<RetEliteDungeonRank> userElistMission = Dungeon_Manager.GetUser_All_EliteDungeonRank(ref TB, AID);

            List<User_Event_Data> userEventList = TriggerManager.Check_Event_Data_List(ref TB, AID, false, userCharacter, userMission, userGuerillaMission, userElistMission);
            List<User_Event_Data> userAchieveList = TriggerManager.Check_Achieve_Data_List(ref TB, AID, false, userCharacter, userMission, userGuerillaMission, userElistMission);
            List<User_Event_Data> userAchievePvPList = TriggerManager.Check_Achieve_PvP_Data_List(ref TB, AID, false, userCharacter, userMission, userGuerillaMission, userElistMission);
            List<TriggerProgressData> setDataList = new List<TriggerProgressData>();
            setDataList.Add(new TriggerProgressData(TriggerType, UserValue1, UserValue2, UserValue3));
            return TriggerManager.ProgressTrigger(ref TB, ref userEventList, ref userAchieveList, ref userAchievePvPList, AID, setDataList, userCharacter, userMission, userGuerillaMission, userElistMission);
        }

        public static Result_Define.eResult ProgressTrigger(ref TxnBlock TB, long AID, List<TriggerProgressData> setDataList)
        {
            List<Character> userCharacter = CharacterManager.GetCharacterList(ref TB, AID);
            List<RetMissionRank> userMission = Dungeon_Manager.GetUser_All_MissionRank(ref TB, AID);
            List<User_GuerrillaDungeon_Play> userGuerillaMission = Dungeon_Manager.GetUser_All_GuerrillaDungeonRank(ref TB, AID);
            List<RetEliteDungeonRank> userElistMission = Dungeon_Manager.GetUser_All_EliteDungeonRank(ref TB, AID);

            List<User_Event_Data> userEventList = TriggerManager.Check_Event_Data_List(ref TB, AID, false, userCharacter, userMission, userGuerillaMission, userElistMission);
            List<User_Event_Data> userAchieveList = TriggerManager.Check_Achieve_Data_List(ref TB, AID, false, userCharacter, userMission, userGuerillaMission, userElistMission);
            List<User_Event_Data> userAchievePvPList = TriggerManager.Check_Achieve_PvP_Data_List(ref TB, AID, false, userCharacter, userMission, userGuerillaMission, userElistMission);

            return TriggerManager.ProgressTrigger(ref TB, ref userEventList, ref userAchieveList, ref userAchievePvPList, AID, setDataList, userCharacter, userMission, userGuerillaMission, userElistMission);
        }

        public static Result_Define.eResult ProgressTrigger(ref TxnBlock TB, ref List<User_Event_Data> userEventList, ref List<User_Event_Data> userAchieveList, ref List<User_Event_Data> userAchievePvPList, long AID, List<TriggerProgressData> setDataList, List<Character> userCharacter = null, List<RetMissionRank> userMission = null, List<User_GuerrillaDungeon_Play> userGuerillaMission = null, List<RetEliteDungeonRank> userElistMission = null)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            bool bEventFlush = false;
            bool bAchiveFlush = false;
            bool bAchivePvPFlush = false;
            userCharacter = userCharacter == null ? CharacterManager.GetCharacterList(ref TB, AID) : userCharacter;
            userMission = userMission == null ? Dungeon_Manager.GetUser_All_MissionRank(ref TB, AID) : userMission;
            userGuerillaMission = userGuerillaMission == null ? Dungeon_Manager.GetUser_All_GuerrillaDungeonRank(ref TB, AID) : userGuerillaMission;
            userElistMission = userElistMission == null ? Dungeon_Manager.GetUser_All_EliteDungeonRank(ref TB, AID) : userElistMission;      

            foreach (TriggerProgressData setData in setDataList)
            {
                Trigger_Define.eTriggerType TriggerType = setData.Trigger_type;
                long UserValue1 = setData.CheckValue1;
                long UserValue2 = setData.CheckValue2;
                long UserValue3 = setData.CheckValue3;
                string Check_Trigger = Trigger_Define.TriggerString[TriggerType];

                var targetEventList = userEventList.Where(ev => ev.ClearTriggerType1.Equals(Check_Trigger) || ev.ClearTriggerType2.Equals(Check_Trigger));
                var targetAchieveList = userAchieveList.Where(ev => ev.ClearTriggerType1.Equals(Check_Trigger) || ev.ClearTriggerType2.Equals(Check_Trigger));
                var targetAchievePvPList = userAchievePvPList.Where(ev => ev.ClearTriggerType1.Equals(Check_Trigger) || ev.ClearTriggerType2.Equals(Check_Trigger));

                User_Event_Data retItem;
                foreach (User_Event_Data tItem in targetEventList)
                {
                    if (tItem.ClearFlag.Equals(Trigger_Define.ClearFlag[Trigger_Define.eClearType.Clear]) || tItem.ClearFlag.Equals(Trigger_Define.ClearFlag[Trigger_Define.eClearType.End]))
                        continue;

                    bool isUpdated = false;
                    bool timeUpdate = false;
                    long Trigger_Value = 0;
                    long updateValue = 0;

                    retItem = tItem;
                    timeUpdate = TriggerManager.CheckLoopTime(ref retItem);
                    tItem.CurrentValue1 = retItem.CurrentValue1;
                    tItem.CurrentValue2 = retItem.CurrentValue2;

                    if (tItem.ClearTriggerType1.Equals(Check_Trigger))
                    {
                        Trigger_Value = updateValue = tItem.CurrentValue1;

                        TriggerManager.CheckTrigger(ref TB, AID, TriggerType, tItem.StartTime,
                                                        tItem.ClearTriggerType1_Value1, tItem.ClearTriggerType1_Value2, tItem.ClearTriggerType1_Value3,
                                                    ref updateValue, UserValue1, UserValue2, UserValue3, userCharacter, userMission, userGuerillaMission, userElistMission);
                        if (updateValue > tItem.ClearTriggerType1_Value3)
                            updateValue = tItem.ClearTriggerType1_Value3;
                        if (Trigger_Value != updateValue)
                        {
                            if (TriggerType == Trigger_Define.eTriggerType.Game_Access)
                                tItem.StartTime = DateTime.Now;
                            tItem.CurrentValue1 = updateValue;
                            //User_Event_Data retItem = tItem;
                            //timeUpdate = TriggerManager.CheckLoopTime(ref retItem);
                            //tItem.CurrentValue1 = timeUpdate ?
                            //                            System.Convert.ToInt32(Trigger_Value) :
                            //                            System.Convert.ToInt32(retItem.CurrentValue1);
                            isUpdated = true;
                        }
                    }

                    if (tItem.ClearTriggerType2.Equals(Check_Trigger))
                    {
                        Trigger_Value = updateValue = tItem.CurrentValue2;

                        TriggerManager.CheckTrigger(ref TB, AID, TriggerType, tItem.StartTime,
                                                        tItem.ClearTriggerType2_Value1, tItem.ClearTriggerType2_Value2, tItem.ClearTriggerType2_Value3,
                                                    ref updateValue, UserValue1, UserValue2, UserValue3, userCharacter, userMission, userGuerillaMission, userElistMission);
                        if (updateValue > tItem.ClearTriggerType2_Value3)
                            updateValue = tItem.ClearTriggerType2_Value3;
                        if (Trigger_Value != updateValue)
                        {
                            tItem.CurrentValue2 = updateValue;
                            //User_Event_Data retItem = tItem;
                            //timeUpdate = TriggerManager.CheckLoopTime(ref retItem);           
                            //tItem.CurrentValue2 = timeUpdate ?
                            //                            System.Convert.ToInt32(Trigger_Value) :
                            //                            System.Convert.ToInt32(retItem.CurrentValue2);
                            isUpdated = true;
                        }
                    }

                    if (isUpdated)
                    {
                        bEventFlush = true;
                        retError = UpdateUserEvent(ref TB, tItem, timeUpdate, Trigger_Define.eEventListType.Event, false);
                    }

                    if (retError != Result_Define.eResult.SUCCESS)
                        return retError;
                }

                foreach (User_Event_Data tItem in targetAchieveList)
                {
                    bool isUpdated = false;
                    bool timeUpdate = false;
                    long Trigger_Value = 0;
                    long updateValue = 0;
                    retItem = tItem;
                    timeUpdate = TriggerManager.CheckLoopTime(ref retItem);
                    tItem.CurrentValue1 = retItem.CurrentValue1;
                    tItem.CurrentValue2 = retItem.CurrentValue2;
                    if (tItem.ClearTriggerType1.Equals(Check_Trigger))
                    {
                        Trigger_Value = updateValue = tItem.CurrentValue1;

                        TriggerManager.CheckTrigger(ref TB, AID, TriggerType, tItem.StartTime,
                                                        tItem.ClearTriggerType1_Value1, tItem.ClearTriggerType1_Value2, tItem.ClearTriggerType1_Value3,
                                                    ref updateValue, UserValue1, UserValue2, UserValue3, userCharacter, userMission, userGuerillaMission, userElistMission);
                        if (updateValue > tItem.ClearTriggerType1_Value3)
                            updateValue = tItem.ClearTriggerType1_Value3;
                        if (Trigger_Value != updateValue)
                        {
                            if (TriggerType == Trigger_Define.eTriggerType.Game_Access)
                                tItem.StartTime = DateTime.Now;

                            tItem.CurrentValue1 = updateValue;
                            //User_Event_Data retItem = tItem;
                            //timeUpdate = TriggerManager.CheckLoopTime(ref retItem);
                            //tItem.CurrentValue1 = timeUpdate ?
                            //                            System.Convert.ToInt32(Trigger_Value) :
                            //                            System.Convert.ToInt32(retItem.CurrentValue1); 
                            isUpdated = true;
                        }
                    }

                    if (tItem.ClearTriggerType2.Equals(Check_Trigger))
                    {
                        Trigger_Value = updateValue = tItem.CurrentValue2;

                        TriggerManager.CheckTrigger(ref TB, AID, TriggerType, tItem.StartTime,
                                                        tItem.ClearTriggerType2_Value1, tItem.ClearTriggerType2_Value2, tItem.ClearTriggerType2_Value3,
                                                    ref updateValue, UserValue1, UserValue2, UserValue3, userCharacter, userMission, userGuerillaMission, userElistMission);
                        if (updateValue > tItem.ClearTriggerType2_Value3)
                            updateValue = tItem.ClearTriggerType2_Value3;
                        if (Trigger_Value != updateValue)
                        {
                            tItem.CurrentValue2 = updateValue;
                            //User_Event_Data retItem = tItem;
                            //timeUpdate = TriggerManager.CheckLoopTime(ref retItem);
                            //tItem.CurrentValue2 = timeUpdate ?
                            //                            System.Convert.ToInt32(Trigger_Value) :
                            //                            System.Convert.ToInt32(retItem.CurrentValue2);
                            isUpdated = true;
                        }
                    }

                    if (isUpdated)
                    {
                        bAchiveFlush = true;
                        retError = UpdateUserEvent(ref TB, tItem, timeUpdate, Trigger_Define.eEventListType.Achive, false);
                    }

                    if (retError != Result_Define.eResult.SUCCESS)
                        return retError;
                }


                foreach (User_Event_Data tItem in targetAchievePvPList)
                {
                    bool isUpdated = false;
                    bool timeUpdate = false;
                    long Trigger_Value = 0;
                    long updateValue = 0;
                    retItem = tItem;
                    timeUpdate = TriggerManager.CheckLoopTime(ref retItem);
                    tItem.CurrentValue1 = retItem.CurrentValue1;
                    tItem.CurrentValue2 = retItem.CurrentValue2;
                    if (tItem.ClearTriggerType1.Equals(Check_Trigger))
                    {
                        Trigger_Value = updateValue = tItem.CurrentValue1;

                        TriggerManager.CheckTrigger(ref TB, AID, TriggerType, tItem.StartTime,
                                                        tItem.ClearTriggerType1_Value1, tItem.ClearTriggerType1_Value2, tItem.ClearTriggerType1_Value3,
                                                    ref updateValue, UserValue1, UserValue2, UserValue3, userCharacter, userMission, userGuerillaMission, userElistMission);
                        if (updateValue > tItem.ClearTriggerType1_Value3)
                            updateValue = tItem.ClearTriggerType1_Value3;
                        if (Trigger_Value != updateValue)
                        {
                            if (TriggerType == Trigger_Define.eTriggerType.Game_Access)
                                tItem.StartTime = DateTime.Now;

                            tItem.CurrentValue1 = updateValue;
                            //User_Event_Data retItem = tItem;
                            //timeUpdate = TriggerManager.CheckLoopTime(ref retItem);
                            //tItem.CurrentValue1 = timeUpdate ?
                            //                            System.Convert.ToInt32(Trigger_Value) :
                            //                            System.Convert.ToInt32(retItem.CurrentValue1); 
                            isUpdated = true;
                        }
                    }

                    if (tItem.ClearTriggerType2.Equals(Check_Trigger))
                    {
                        Trigger_Value = updateValue = tItem.CurrentValue2;

                        TriggerManager.CheckTrigger(ref TB, AID, TriggerType, tItem.StartTime,
                                                        tItem.ClearTriggerType2_Value1, tItem.ClearTriggerType2_Value2, tItem.ClearTriggerType2_Value3,
                                                    ref updateValue, UserValue1, UserValue2, UserValue3, userCharacter, userMission, userGuerillaMission, userElistMission);
                        if (updateValue > tItem.ClearTriggerType2_Value3)
                            updateValue = tItem.ClearTriggerType2_Value3;
                        if (Trigger_Value != updateValue)
                        {
                            tItem.CurrentValue2 = updateValue;
                            //User_Event_Data retItem = tItem;
                            //timeUpdate = TriggerManager.CheckLoopTime(ref retItem);
                            //tItem.CurrentValue2 = timeUpdate ?
                            //                            System.Convert.ToInt32(Trigger_Value) :
                            //                            System.Convert.ToInt32(retItem.CurrentValue2);
                            isUpdated = true;
                        }
                    }

                    if (isUpdated)
                    {
                        bAchivePvPFlush = true;
                        retError = UpdateUserEvent(ref TB, tItem, timeUpdate, Trigger_Define.eEventListType.PvP_Achive, false);
                    }

                    if (retError != Result_Define.eResult.SUCCESS)
                        return retError;
                }
            }

            if (bEventFlush && retError == Result_Define.eResult.SUCCESS)
                RemoveEventDataFromRedis(AID);

            if (bAchiveFlush && retError == Result_Define.eResult.SUCCESS)
                RemoveAchieveDataFromRedis(AID, false);

            if (bAchivePvPFlush && retError == Result_Define.eResult.SUCCESS)
                RemoveAchieveDataFromRedis(AID, true);

            return retError;
        }

        public static User_Event_Data CheckClear(ref TxnBlock TB, long AID, long UserEventID, bool isEvent, out bool isClear, out int maxValue1, out int maxValue2,
                                                    List<Character> userCharacter = null, List<RetMissionRank> userMission = null, List<User_GuerrillaDungeon_Play> userGuerillaMission = null, List<RetEliteDungeonRank> userElistMission = null)
        {
            List<User_Event_Data> userEventList = isEvent ? TriggerManager.Check_Event_Data_List(ref TB, AID) : TriggerManager.Check_Achieve_Data_List(ref TB, AID);
            User_Event_Data targetEvent = userEventList.Find(ev => ev.User_Event_ID == UserEventID);

            long current = targetEvent.CurrentValue1;
            bool ClearFlag1 = TriggerManager.CheckClearTrigger(ref TB, AID, Trigger_Define.TriggerType[targetEvent.ClearTriggerType1], targetEvent.StartTime, targetEvent.ClearTriggerType1_Value1, targetEvent.ClearTriggerType1_Value2, targetEvent.ClearTriggerType1_Value3, ref current, out maxValue1, userCharacter);
            targetEvent.CurrentValue1 = current > maxValue1 ? maxValue1 : current;

            current = targetEvent.CurrentValue2;
            bool ClearFlag2 = TriggerManager.CheckClearTrigger(ref TB, AID, Trigger_Define.TriggerType[targetEvent.ClearTriggerType2], targetEvent.StartTime, targetEvent.ClearTriggerType2_Value1, targetEvent.ClearTriggerType2_Value2, targetEvent.ClearTriggerType2_Value3, ref current, out maxValue2, userCharacter);
            targetEvent.CurrentValue2 = current > maxValue2 ? maxValue2 : current;

            isClear = ClearFlag1 && ClearFlag2;
            return targetEvent;
        }

        public static User_Event_Data CheckClear(ref TxnBlock TB, long AID,  User_Event_Data targetEvent, out bool isClear, out int maxValue1, out int maxValue2,
                                                    List<Character> userCharacter = null, List<RetMissionRank> userMission = null, List<User_GuerrillaDungeon_Play> userGuerillaMission = null, List<RetEliteDungeonRank> userElistMission = null)
        {
            long current = targetEvent.CurrentValue1;
            bool ClearFlag1 = TriggerManager.CheckClearTrigger(ref TB, AID, Trigger_Define.TriggerType[targetEvent.ClearTriggerType1], targetEvent.StartTime, targetEvent.ClearTriggerType1_Value1, targetEvent.ClearTriggerType1_Value2, targetEvent.ClearTriggerType1_Value3
                                                                , ref current, out maxValue1, userCharacter, userMission, userGuerillaMission, userElistMission);
            targetEvent.CurrentValue1 = current > maxValue1 ? maxValue1 : current;
            current = targetEvent.CurrentValue2;
            bool ClearFlag2 = TriggerManager.CheckClearTrigger(ref TB, AID, Trigger_Define.TriggerType[targetEvent.ClearTriggerType2], targetEvent.StartTime, targetEvent.ClearTriggerType2_Value1, targetEvent.ClearTriggerType2_Value2, targetEvent.ClearTriggerType2_Value3
                                                                , ref current, out maxValue2, userCharacter, userMission, userGuerillaMission, userElistMission);
            targetEvent.CurrentValue2 = current > maxValue2 ? maxValue2 : current;

            isClear = ClearFlag1 && ClearFlag2;
            return targetEvent;
        }

        public static bool CheckClearTrigger(ref TxnBlock TB, long AID, Trigger_Define.eTriggerType TriggerType, int TriggerValue1, int TriggerValue2, int TriggerValue3,
                                List<Character> userCharacter = null, List<RetMissionRank> userMission = null, List<User_GuerrillaDungeon_Play> userGuerillaMission = null, List<RetEliteDungeonRank> userElistMission = null)
        {
            long refItem = 0;
            int maxItem = 0;
            return CheckClearTrigger(ref TB, AID, TriggerType, DateTime.Now, TriggerValue1, TriggerValue2, TriggerValue3, ref refItem, out maxItem, userCharacter, userMission, userGuerillaMission, userElistMission);
        }

        public static bool CheckClearTrigger(ref TxnBlock TB, long AID, Trigger_Define.eTriggerType TriggerType, DateTime StartTime, int TriggerValue1, int TriggerValue2, int TriggerValue3, ref long CurrentValue, out int MaxValue,
                                                List<Character> userCharacter = null, List<RetMissionRank> userMission = null, List<User_GuerrillaDungeon_Play> userGuerillaMission = null, List<RetEliteDungeonRank> userElistMission = null)
        {
            MaxValue = TriggerValue3;

            {
                switch (TriggerType)
                {
                    case Trigger_Define.eTriggerType.None:
                        return true;
                    case Trigger_Define.eTriggerType.Level:
                    case Trigger_Define.eTriggerType.Class:
                    case Trigger_Define.eTriggerType.CHARGE_FIXED:
                    case Trigger_Define.eTriggerType.CHARGE_FIXED_WEEK:
                    case Trigger_Define.eTriggerType.Charge_First:
                    case Trigger_Define.eTriggerType.Charge_Price:
                    case Trigger_Define.eTriggerType.VIP_Point:
                    case Trigger_Define.eTriggerType.Goods_Purchase:
                    case Trigger_Define.eTriggerType.Friend_register:
                    case Trigger_Define.eTriggerType.Guild_Level:
                    case Trigger_Define.eTriggerType.VIP_Level:
                    case Trigger_Define.eTriggerType.VIP_Level_First:
                    case Trigger_Define.eTriggerType.Time:
                    case Trigger_Define.eTriggerType.Rank_1vs1Real_First:
                    case Trigger_Define.eTriggerType.Rank_Freeforall_First:
                    case Trigger_Define.eTriggerType.Play_Scenario_First:
                    case Trigger_Define.eTriggerType.Play_Guerilla_First:
                    case Trigger_Define.eTriggerType.Play_Elite_First:
                    case Trigger_Define.eTriggerType.Clear_Scenario_First:
                    case Trigger_Define.eTriggerType.Clear_Guerilla_First:
                    case Trigger_Define.eTriggerType.Clear_Elite_First:
                    case Trigger_Define.eTriggerType.Clear_Mission:
                    case Trigger_Define.eTriggerType.Grade_PVP:
                    case Trigger_Define.eTriggerType.Clear_Event:
                    case Trigger_Define.eTriggerType.Soul_Acquire:
                    case Trigger_Define.eTriggerType.Soul_Lv:
                    case Trigger_Define.eTriggerType.AttackPower:
                    case Trigger_Define.eTriggerType.DivineWeapon:
                    case Trigger_Define.eTriggerType.Game_Access:
                    case Trigger_Define.eTriggerType.FaceBook_FriendCount:
                        return CheckTrigger(ref TB, AID, TriggerType, StartTime, TriggerValue1, TriggerValue2, TriggerValue3, ref CurrentValue, 0, 0, 0, userCharacter, userMission, userGuerillaMission, userElistMission);

                    default:
                        return (CurrentValue >= TriggerValue3);
                }
            }
        }
        
        // Check Triger Success
        public static bool CheckTrigger(ref TxnBlock TB, long AID, Trigger_Define.eTriggerType TriggerType, DateTime StartTime, long TriggerValue1, long TriggerValue2, long TriggerValue3, ref long current, long UserValue1 = 0, long UserValue2 = 0, long UserValue3 = 0,
                                                List<Character> userCharacter = null, List<RetMissionRank> userMission = null, List<User_GuerrillaDungeon_Play> userGuerillaMission = null, List<RetEliteDungeonRank> userElistMission = null)
        {
            switch (TriggerType)
            {
                case Trigger_Define.eTriggerType.None:
                    break;

                // not use yet : duplicated function Trigger_Define.eTriggerType.Class
                case Trigger_Define.eTriggerType.Level:
                    {
                        if(userCharacter == null)
                            userCharacter = CharacterManager.GetCharacterList(ref TB, AID);

                        Character findCharacter = userCharacter.Find(item => item.level >= TriggerValue3);

                        if (findCharacter == null)
                        {
                            current = userCharacter.FirstOrDefault().level;
                            return false;
                        }
                        else
                        {
                            current = findCharacter.level;
                            if (current < TriggerValue3)
                                return false;
                        }

                        break;
                    }

                case Trigger_Define.eTriggerType.Class:
                    {
                        if (userCharacter == null)
                            userCharacter = CharacterManager.GetCharacterList(ref TB, AID);

                        if (TriggerValue1 == (int)Trigger_Define.eClassCheckType.MaxLevel)
                        {
                            Character findCharacter = userCharacter.Find(item => item.level >= TriggerValue3);

                            if (findCharacter == null)
                            {
                                if (userCharacter.Count > 0)
                                    current = userCharacter.FirstOrDefault().level;
                                return false;
                            }
                            else
                            {
                                current = findCharacter.level;
                                if (current < TriggerValue3)
                                    return false;
                            }
                        }
                        else if (TriggerValue1 == (int)Trigger_Define.eClassCheckType.CharacterLevel)
                        {
                            Character findCharacter = userCharacter.Find(item => (item.level >= TriggerValue3) && (item.Class == TriggerValue2));

                            if (findCharacter == null)
                            {
                                //current = userCharacter.FirstOrDefault().level;
                                return false;
                            }
                            else
                            {
                                current = findCharacter.level;
                                if (current < TriggerValue3)
                                    return false;
                            }
                        }
                        break;
                    }
                // Shop Subscription Item Check
                case Trigger_Define.eTriggerType.CHARGE_FIXED:
                    {
                        RetShopSubscription subscriptionInfo = ShopManager.GetUserSubscriptionLeftDay(ref TB, AID, false);
                        //if (TriggerValue1 != subscriptionInfo.shop_item_id || subscriptionInfo.left_day < 1)
                        if(subscriptionInfo.left_day < 1)
                            return false;
                        break;
                    }

                // Shop Subscription Item Check
                case Trigger_Define.eTriggerType.CHARGE_FIXED_WEEK:
                    {
                        RetShopSubscription subscriptionInfo = ShopManager.GetUserSubscriptionLeftDay(ref TB, AID, true);
                        //if (TriggerValue1 != subscriptionInfo.shop_item_id || subscriptionInfo.left_day < 1)
                        if (subscriptionInfo.left_day < 1)
                            return false;
                        break;
                    }

                // Shop Item Charge Trigger First
                case Trigger_Define.eTriggerType.Charge_First:
                    {
                        User_Shop_Buy userBuy = ShopManager.GetUserBuyItemCount(ref TB, AID, TriggerValue1);
                        current = userBuy.TotalBuy_Count;
                        if (userBuy.TotalBuy_Count != 1)
                            return false;
                        break;
                    }
                case Trigger_Define.eTriggerType.Charge:
                case Trigger_Define.eTriggerType.Charge_Billing:
                    {
                        if (TriggerValue1 == UserValue1 || TriggerValue1 == 0)
                        {
                            current += UserValue3;

                            if (current < TriggerValue3)
                                return false;
                        }
                        break;
                    }

                // Real Cash Charge Trigger
                case Trigger_Define.eTriggerType.Charge_Price:
                case Trigger_Define.eTriggerType.VIP_Point:
                    {
                        if (TriggerValue1 == (int)Trigger_Define.eRubyUseType.Single)
                        {
                            if(current < TriggerValue3)
                                current = UserValue3 < TriggerValue3 ? UserValue3 : TriggerValue3;

                            if (current < TriggerValue3)
                                return false;
                        }
                        else if (TriggerValue1 == (int)Trigger_Define.eRubyUseType.Accumulate)
                        {
                            current += UserValue3;

                            if (current < TriggerValue3)
                                return false;
                        }
                        break;
                    }

                // 투자계획구매 ?? i don't know yet
                case Trigger_Define.eTriggerType.Goods_Purchase:
                    {
                        if (UserValue1 != TriggerValue3 && current != TriggerValue3)
                        {
                            current = UserValue3;
                            return false;
                        }
                        else
                            current = TriggerValue3;

                        break;
                    }

                // check User VIP Level
                case Trigger_Define.eTriggerType.VIP_Level:
                case Trigger_Define.eTriggerType.VIP_Level_First:
                    {
                        current = VipManager.GetUser_VIPInfo(ref TB, AID).viplevel;
                        if (current < TriggerValue3)
                            return false;
                        break;
                    }

                case Trigger_Define.eTriggerType.Game_Access:
                    {
                        if (TriggerValue1 == UserValue1 || TriggerValue1 == (int)Trigger_Define.eGameAccessType.AccumulateLoginDay)
                        {
                            if (TriggerValue1 == (int)Trigger_Define.eGameAccessType.TotalLoginCount)
                            {
                                current += UserValue3;
                            }
                            else if (TriggerValue1 == (int)Trigger_Define.eGameAccessType.AccumulateLoginDay)
                            {
                                int loginCount = 0;
                                AccountManager.GetUserLoginCount(ref TB, AID, out loginCount);
                                current = loginCount;
                                //DateTime CheckDate = DateTime.Parse(StartTime.ToShortDateString());
                                //DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                                //TimeSpan TS = dbDate - CheckDate;

                                //if (TS.Days != 0)
                                //    current += UserValue3;
                            }
                            else if (TriggerValue1 == (int)Trigger_Define.eGameAccessType.AccumulateLoginTime)
                            {
                                current += UserValue3;
                            }
                            else if (TriggerValue1 == (int)Trigger_Define.eGameAccessType.CountinueLoginDay)
                            {
                                DateTime CheckDate = DateTime.Parse(StartTime.ToShortDateString());
                                DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                                TimeSpan TS = dbDate - CheckDate;

                                if (TS.Days == 1)
                                    current += UserValue3;
                                else if (TS.Days > 1)
                                    current = 1;
                            }
                            else if (TriggerValue1 == (int)Trigger_Define.eGameAccessType.CountLoginDay)
                            {
                                DateTime CheckDate = DateTime.Parse(StartTime.ToShortDateString());
                                DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                                TimeSpan TS = dbDate - CheckDate;

                                if (TS.Days > 0)
                                    current += 1;
                            }
                        }

                        if (current < TriggerValue3)
                            return false;
                        break;
                    }
                // Friend 
                case Trigger_Define.eTriggerType.Friend_register:
                    {
                        var FriendCnt = FriendManager.GetMyFriendCount(ref TB, AID).count;
                        current = FriendCnt;
                        if (FriendCnt < TriggerValue3)
                            return false;
                        break;
                    }

                // only use CN or TW 
                case Trigger_Define.eTriggerType.Wechat_Share:
                case Trigger_Define.eTriggerType.FaceBook_Open:
                    current += UserValue3;
                    if (current < TriggerValue3)
                        return false;
                    break;

                case Trigger_Define.eTriggerType.FaceBook_FriendCount:
                    current = FriendManager.GetFaceBookFriendsCount(ref TB, AID);
                    if (current < TriggerValue3)
                        return false;
                    break;

                case Trigger_Define.eTriggerType.Ruby_Use:
                    {
                        if (TriggerValue1 == UserValue1)
                        {
                            if (TriggerValue1 == (int)Trigger_Define.eRubyUseType.Single)
                            {
                                if (UserValue3 < TriggerValue3 && current < TriggerValue3)
                                {
                                    current = UserValue3;
                                    return false;
                                }
                            }
                            else if (TriggerValue1 == (int)Trigger_Define.eRubyUseType.Accumulate)
                            {
                                current += UserValue3;

                                if (current < TriggerValue3)
                                    return false;
                            }
                        }
                        break;
                    }

                // don't support yet
                case Trigger_Define.eTriggerType.Town_Enter:
                    break;

                // check value1, value2 equal, then check over value3
                case Trigger_Define.eTriggerType.Kill_NPC_Appoint:
                case Trigger_Define.eTriggerType.Kill_NPC_Appear:
                // for Achieve
                case Trigger_Define.eTriggerType.Clear_PVE:
                case Trigger_Define.eTriggerType.Clear_Scenario:
                case Trigger_Define.eTriggerType.Clear_Guerilla:
                case Trigger_Define.eTriggerType.Clear_Elite:
                case Trigger_Define.eTriggerType.Play_Scenario:
                case Trigger_Define.eTriggerType.Play_Guerilla:
                case Trigger_Define.eTriggerType.Play_Elite:
                    {
                        if (TriggerValue1 == UserValue1 || TriggerValue1 == 0)
                        {
                            if (TriggerValue2 == UserValue2 || TriggerValue2 == 0)
                            {
                                current += UserValue3;

                                if (current < TriggerValue3)
                                    return false;
                            }
                            else
                                return false;
                        }
                        else
                            return false;
                        break;
                    }

                // check value1, value2 equal, then check over value3, current value only 1
                case Trigger_Define.eTriggerType.Kill_NPC_First:
                case Trigger_Define.eTriggerType.Play_Scenario_First:
                case Trigger_Define.eTriggerType.Play_Guerilla_First:
                case Trigger_Define.eTriggerType.Play_Elite_First:
                    {
                        if (TriggerValue1 == UserValue1 || TriggerValue1 == 0)
                        {
                            if (TriggerValue2 == UserValue2 || TriggerValue2 == 0)
                            {
                                current = 1;

                                if (current < TriggerValue3)
                                    return false;
                            }
                            else
                                return false;
                        }
                        else
                            return false;
                        break;
                    }
                case Trigger_Define.eTriggerType.Clear_Scenario_First:
                    {
                        if (current >= TriggerValue3)
                            return true;

                        if (userMission == null)
                            userMission = Dungeon_Manager.GetUser_All_MissionRank(ref TB, AID);

                        var getMission = userMission.Find(mission => mission.stageid == TriggerValue1);

                        if (getMission != null)
                        {
                            if (getMission.rank > 0)
                                current++;

                            if (current < TriggerValue3)
                                return false;
                        }
                        else
                            return false;
                        break;
                    }

                case Trigger_Define.eTriggerType.Clear_Guerilla_First:
                    {
                        if (current >= TriggerValue3)
                            return true;

                        if (userGuerillaMission == null)
                            userGuerillaMission = Dungeon_Manager.GetUser_All_GuerrillaDungeonRank(ref TB, AID);

                        var getMission = userGuerillaMission.Find(mission => mission.dungeonid == TriggerValue1);

                        if (getMission != null)
                        {
                            if (getMission.rank > 0)
                                current++;

                            if (current < TriggerValue3)
                                return false;
                        }
                        else
                            return false;
            
                    }
                    break;
                case Trigger_Define.eTriggerType.Clear_Elite_First:
                    {
                        if (current >= TriggerValue3)
                            return true;

                        if (userElistMission == null)    
                            userElistMission = Dungeon_Manager.GetUser_All_EliteDungeonRank(ref TB, AID);

                        var getMission = userElistMission.Find(mission => mission.dungeonid == TriggerValue1);

                        if (getMission != null)
                        {
                            if (getMission.rank > 0)
                                current++;

                            if (current < TriggerValue3)
                                return false;
                        }
                        else
                            return false;
             
                    }
                    break;

                // check value1 equal, then check over value3
                case Trigger_Define.eTriggerType.Clear_Party:
                case Trigger_Define.eTriggerType.Clear_Party_First:
                case Trigger_Define.eTriggerType.Play_Bossraid:
                case Trigger_Define.eTriggerType.Play_Party:
                case Trigger_Define.eTriggerType.Play_Party_First:
                // item and soul
                case Trigger_Define.eTriggerType.Equip_Acquire:
                case Trigger_Define.eTriggerType.Soul_LvUp:
                case Trigger_Define.eTriggerType.Weapon_LvUp:
                case Trigger_Define.eTriggerType.WEAPON_REFINING:
                case Trigger_Define.eTriggerType.ARMOR_METALWORK:
                case Trigger_Define.eTriggerType.ACCESSORY_REMODELING:
                case Trigger_Define.eTriggerType.PASSIVESOUL_ACTION:
                case Trigger_Define.eTriggerType.Soul_Piece_Acquire:
                    {
                        if (TriggerValue1 == UserValue1 || TriggerValue1 == 0)
                        {
                            if (TriggerType == Trigger_Define.eTriggerType.Clear_Party_First || TriggerType == Trigger_Define.eTriggerType.Play_Party_First)
                                current = 1;
                            else
                                current += UserValue3;

                            if (current < TriggerValue3)
                                return false;
                        }
                        else
                            return false;
                        break;
                    }                    

                // check only value3 count. not check over
                case Trigger_Define.eTriggerType.Weapon_Lv:
                    if (UserValue3 < TriggerValue3)
                        return false;
                    break;

                case Trigger_Define.eTriggerType.Soul_Lv:
                    {
                        List<User_ActiveSoul> userActiveSoul = SoulManager.GetUser_ActiveSoul(ref TB, AID);
                        if (userActiveSoul.Count < 1)
                            return false;

                        bool bFind = (  TriggerValue1 == (int)Trigger_Define.eSoul_LvUpType.All ? userActiveSoul.Count(findSoul => findSoul.level > 0 && findSoul.grade > 0 && findSoul.starlevel > 0) :
                                        TriggerValue1 == (int)Trigger_Define.eSoul_LvUpType.Level ? userActiveSoul.Count(findSoul => findSoul.level >= TriggerValue3 && findSoul.grade > 0 && findSoul.starlevel > 0) :
                                        TriggerValue1 == (int)Trigger_Define.eSoul_LvUpType.Grade ? userActiveSoul.Count(findSoul => findSoul.level > 0 && findSoul.grade >= TriggerValue3 && findSoul.starlevel > 0) :
                                        TriggerValue1 == (int)Trigger_Define.eSoul_LvUpType.StarLevel ? userActiveSoul.Count(findSoul => findSoul.level > 0 && findSoul.grade > 0 && findSoul.starlevel >= TriggerValue3) :
                                    0) > 0;

                        if (bFind)
                            current = TriggerValue3;
                        else
                            current = 
                                    (
                                        TriggerValue1 == (int)Trigger_Define.eSoul_LvUpType.All ? userActiveSoul.Count(findSoul => findSoul.level > 0 && findSoul.grade > 0 && findSoul.starlevel > 0) :
                                        TriggerValue1 == (int)Trigger_Define.eSoul_LvUpType.Level ? userActiveSoul.Max(findSoul => findSoul.level) :
                                        TriggerValue1 == (int)Trigger_Define.eSoul_LvUpType.Grade ? userActiveSoul.Max(findSoul => findSoul.grade) :
                                        TriggerValue1 == (int)Trigger_Define.eSoul_LvUpType.StarLevel ? userActiveSoul.Max(findSoul => findSoul.starlevel) : 
                                    0);

                        if (!bFind)
                            return false;
                        break;
                    }

                // check value1 mask, then check over value3
                case Trigger_Define.eTriggerType.Clear_Fail_PVE:
                case Trigger_Define.eTriggerType.Kill_NPC:
                case Trigger_Define.eTriggerType.Play_PVE:
                case Trigger_Define.eTriggerType.Play_PVP:
                case Trigger_Define.eTriggerType.Win_PVP:
                case Trigger_Define.eTriggerType.Energy_Use:
                case Trigger_Define.eTriggerType.Gacha:
                    {
                        if (IsSetMask(TriggerValue1, UserValue1))
                        {
                            if (TriggerValue2 == (int)Trigger_Define.eGachaCheckType.TryCount)
                            {
                                current += UserValue3;

                                if (current < TriggerValue3)
                                    return false;
                            }
                            else if (TriggerValue2 == (int)Trigger_Define.eGachaCheckType.GachaGetCount)
                            {
                                if (IsSetMask(UserValue1, (int)(Trigger_Define.eGachaType.NORMAL_TRY_TEN | Trigger_Define.eGachaType.PREMIUM_TRY_TEN | Trigger_Define.eGachaType.BEST_TRY_TEN)))
                                    UserValue3 = Trigger_Define.DrawTenGachaCount;
                                else
                                    UserValue3 = Trigger_Define.DrawOneGachaCount;

                                current += UserValue3;

                                if (current < TriggerValue3)
                                    return false;
                            }
                        }
                        else
                            return false;
                        break;
                    }

                // check only value3 count
                case Trigger_Define.eTriggerType.Reward_Acquire_Bossraid:
                case Trigger_Define.eTriggerType.Autoclear_Use:
                case Trigger_Define.eTriggerType.Kill_Freeforall:
                case Trigger_Define.eTriggerType.Gold:
                case Trigger_Define.eTriggerType.Destroy_Object:
                // item and soul
                case Trigger_Define.eTriggerType.Armor_LvUp:
                case Trigger_Define.eTriggerType.Armor_GradeUp:
                case Trigger_Define.eTriggerType.Accerary_GradeUp:
                // guild and friend
                case Trigger_Define.eTriggerType.Friend_Key:
                case Trigger_Define.eTriggerType.Guild_Donation:
                case Trigger_Define.eTriggerType.Guild_Attendance:
                case Trigger_Define.eTriggerType.Guild_User_EXP:
                // gacha shop
                case Trigger_Define.eTriggerType.GACHASHOP:
                case Trigger_Define.eTriggerType.GACHASHOP_SPECIAL:

                // account regist
                case Trigger_Define.eTriggerType.ACCOUNT_REGIST:
                    {
                        current += UserValue3;
                        /// 如果当前的值小于填写的第三个参数，则为失败，否则则通过过滤
                        if (current < TriggerValue3)
                            return false;
                        break;
                    }

                    /// 角色战力信息 warpoint
                case Trigger_Define.eTriggerType.AttackPower:
                    AccountManager.RemoveUser_UserWarPoint(AID);
                    /// 战力规则，客户端的战力是除以10的显示
                    current = AccountManager.GetUserWarPoint(ref TB, AID).WAR_POINT / 10;
                    return TriggerValue3 <= current;
                    break;
                case Trigger_Define.eTriggerType.DivineWeapon:
                    List<User_Ultimate_Inven> setObj = new List<User_Ultimate_Inven>();
                    setObj = ItemManager.GetMaxUser_Ultimate_Inven(ref TB, AID);
                    current = 0;
                    if (setObj.Count > 0)
                    {
                        current = setObj[0].level;
                    }
                    return TriggerValue3 <= current;
                    break;
                case Trigger_Define.eTriggerType.Soul_Acquire:
                    {
                        List<User_ActiveSoul> userActiveSoul = SoulManager.GetUser_ActiveSoul(ref TB, AID);
                        current = TriggerValue1 == (int)Trigger_Define.eSoul_LvUpType.All ? userActiveSoul.Count(findSoul => findSoul.level > 0 && findSoul.grade > 0 && findSoul.starlevel > 0) :
                                    TriggerValue1 == (int)Trigger_Define.eSoul_LvUpType.Level ? userActiveSoul.Count(findSoul => findSoul.level >= TriggerValue2 && findSoul.grade > 0 && findSoul.starlevel > 0) :
                                    TriggerValue1 == (int)Trigger_Define.eSoul_LvUpType.Grade ? userActiveSoul.Count(findSoul => findSoul.level > 0 && findSoul.grade >= TriggerValue2 && findSoul.starlevel > 0) :
                                    TriggerValue1 == (int)Trigger_Define.eSoul_LvUpType.StarLevel ? userActiveSoul.Count(findSoul => findSoul.level > 0 && findSoul.grade > 0 && findSoul.starlevel >= TriggerValue2) :
                                    0;
                        if (current < TriggerValue3)
                            return false;
                        break;   
                    }

                // PVP
                case Trigger_Define.eTriggerType.Rank_1vs1Real_First:
                case Trigger_Define.eTriggerType.Rank_Freeforall_First:
                    {
                        int pvpType = TriggerType == Trigger_Define.eTriggerType.Rank_1vs1Real_First ? (int)PvP_Define.ePvPType.MATCH_1VS1 :
                            (TriggerType == Trigger_Define.eTriggerType.Rank_Freeforall_First ? (int)PvP_Define.ePvPType.MATCH_FREE : 0);
                        
                        int pvpGrade = PvPManager.GetUser_PvP_High_Grade(ref TB, AID, pvpType);
                        if (pvpGrade < TriggerValue1)
                            return false;
                        break;
                    }

                // PVP Kill Streak
                case Trigger_Define.eTriggerType.Kill_Count:
                    {
                        //if (IsSetMask(TriggerValue1, UserValue1))
                        //{
                        //    current = current < UserValue3 ? UserValue3 : current;

                        //    if (current < TriggerValue3)
                        //        return false;
                        //}
                        //break;

                        if (IsSetMask(TriggerValue1, UserValue1) && (TriggerValue2 == UserValue2))
                        {
                            if (TriggerValue2 == (int)Trigger_Define.eKillCountType.Accumulate)
                            {
                                current += UserValue3;

                                if (current < TriggerValue3)
                                    return false;
                            }
                            else if (TriggerValue2 == (int)Trigger_Define.eKillCountType.KillStreak)
                            {
                                if (current < UserValue3)
                                    current = UserValue3;

                                if (current < TriggerValue3)
                                    return false;
                            }
                        }
                        else
                            return false;
                        break;
                    }
                // PVP Win Streak
                case Trigger_Define.eTriggerType.Straight_PVP:
                    {
                        if (IsSetMask(TriggerValue1, UserValue1))
                        {
                            string setDBTable = TriggerValue2 == (int)Trigger_Define.ePvPTableType.Daily ? PvP_Define.PvP_PvP_Daily_TableName :
                                TriggerValue2 == (int)Trigger_Define.ePvPTableType.Weekly ? PvP_Define.PvP_PvP_Weekly_TableName :
                                TriggerValue2 == (int)Trigger_Define.ePvPTableType.Monthly ? PvP_Define.PvP_PvP_Monthly_TableName:
                                TriggerValue2 == (int)Trigger_Define.ePvPTableType.Season ? PvP_Define.PvP_PvP_Season_TableName:
                                TriggerValue2 == (int)Trigger_Define.ePvPTableType.Guild_Daily ? PvP_Define.PvP_GuildUser_Daily_TableName :
                                TriggerValue2 == (int)Trigger_Define.ePvPTableType.Guild_Weekly ? PvP_Define.PvP_GuildUser_Weekly_TableName :
                                TriggerValue2 == (int)Trigger_Define.ePvPTableType.Guild_Monthly ? PvP_Define.PvP_GuildUser_Monthly_TableName : string.Empty;

                            if (string.IsNullOrEmpty(setDBTable))
                                return false;

                            PvP_Define.ePvPType PvPType = UserValue1 == (int)Trigger_Define.ePvPType.MATCH_1VS1 ? PvP_Define.ePvPType.MATCH_1VS1 :
                                UserValue1 == (int)Trigger_Define.ePvPType.MATCH_FREE ? PvP_Define.ePvPType.MATCH_FREE :
                                UserValue1 == (int)Trigger_Define.ePvPType.MATCH_GUILD_WAR ? PvP_Define.ePvPType.MATCH_GUILD_3VS3 :
                                UserValue1 == (int)Trigger_Define.ePvPType.MATCH_OVERLORD ? PvP_Define.ePvPType.MATCH_OVERLORD :
                                UserValue1 == (int)Trigger_Define.ePvPType.MATCH_RUBY_PVP ? PvP_Define.ePvPType.MATCH_RUBY_PVP : PvP_Define.ePvPType.MATCH_NONE;

                            if (PvPType == PvP_Define.ePvPType.MATCH_NONE)
                                return false;

                            int checkValue = PvPManager.GetUser_PvP_Record(ref TB, AID, 0, PvPType, setDBTable).straightwin;
                            current = current < checkValue ? checkValue : current;

                            if (current < TriggerValue3)
                                return false;
                        }

                        break;
                    }
                // PVP Grade
                case Trigger_Define.eTriggerType.Grade_PVP:
                    {
                        //if (IsSetMask(TriggerValue1, UserValue1))
                        {
                            Trigger_Define.ePvPType tPvPType = (Trigger_Define.ePvPType)TriggerValue1;
                            PvP_Define.ePvPType PvPType = Trigger_Define.PvPType_Define_List[tPvPType];

                            if (TriggerValue2 == (int)Trigger_Define.eGradePvPType.Point)
                            {
                                int highPoint = PvPManager.GetUser_PvP_High_Point(ref TB, AID, (int)PvPType);
                                current = current < highPoint ? highPoint : current;
                            }
                            else if (TriggerValue2 == (int)Trigger_Define.eGradePvPType.Grade)
                            {
                                int highGrade = PvPManager.GetUser_PvP_High_Grade(ref TB, AID, (int)PvPType);
                                current = current < highGrade ? highGrade : current;
                            }

                            if (current < TriggerValue3)
                                return false;
                        }
                        break;
                    }
                // PVP Match Rank
                case Trigger_Define.eTriggerType.PVP_Match_Rank:
                    {
                        if (IsSetMask(TriggerValue1, UserValue1))
                        {
                            if (TriggerValue2 >= UserValue2)
                            {
                                current += UserValue3;

                                if (current < TriggerValue3)
                                    return false;
                            }
                        }
                        else
                            return false;
                        break;
                    }

                // PVE Play Combo (by client data)
                case Trigger_Define.eTriggerType.Combo:
                    {
                        if ((TriggerValue1 == 0 || IsSetMask(TriggerValue1, UserValue1))
                            && (TriggerValue2 == UserValue2))
                        {
                            if (TriggerValue2 == (int)Trigger_Define.eComboType.Max)
                            {
                                current = current < UserValue3 ? UserValue3 : current;

                                if (current < TriggerValue3)
                                    return false;
                            }
                            else if (TriggerValue2 == (int)Trigger_Define.eComboType.Accumulate)
                            {
                                current += UserValue3;

                                if (current < TriggerValue3)
                                    return false;
                            }
                        }
                        break;
                    }
                case Trigger_Define.eTriggerType.Guild_Level:
                    {
                        current = GuildManager.GetGuildLV(ref TB, AID);
                        if (current < TriggerValue3)
                            return false;
                        break;
                    }

                case Trigger_Define.eTriggerType.Time:
                    {
                        DateTime CurrentDate = DateTime.Now;
                        DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                        TimeSpan TS = CurrentDate - dbDate;
                        if (TS.TotalMinutes >= TriggerValue1 && TS.TotalMinutes <= TriggerValue2)
                        {
                            current = 1;
                            return true;
                        }
                        else
                        {
                            current = 0;
                            return false;
                        }
                    }

                case Trigger_Define.eTriggerType.Clear_Elite_Perfect:
                    {
                        if (TriggerValue1 == UserValue1 || TriggerValue1 == 0)
                        {
                            if (TriggerValue2 == UserValue2 || TriggerValue2 == 0)
                            {
                                List<RetEliteDungeonRank> userEliteList = Dungeon_Manager.GetUser_All_EliteDungeonRank(ref TB, AID);
                                var rankInfo = userEliteList.Find(item => (item.worldid == TriggerValue1 || TriggerValue1 == 0) && (item.dungeonid == TriggerValue2 || TriggerValue2 == 0) && item.rank >= Dungeon_Define.Rank3Star);

                                if (rankInfo != null)
                                {
                                    current += UserValue3;

                                    if (current < TriggerValue3)
                                        return false;
                                }
                            }
                            else
                                return false;
                        }
                        else
                            return false;
                        break;
                    }

                case Trigger_Define.eTriggerType.Clear_Mission:
                    {
                        if (current >= TriggerValue3)
                            return true;

                        long WorldID = TriggerValue1;
                        List<KeyValuePair<long, long>> dungeonIDList = new List<KeyValuePair<long, long>>();
                        List<System_Mission_World> setWorldList = new List<System_Mission_World>();
                        if (WorldID > 0)
                        {
                            System_Mission_World worldInfo = Dungeon_Manager.GetSystem_MissionWorldInfo(ref TB, WorldID);
                            setWorldList.Add(worldInfo);
                        }
                        else
                            setWorldList = Dungeon_Manager.GetSystem_MissionWorldList(ref TB);

                        if (setWorldList != null)
                        {
                            setWorldList.ForEach(checkWorld =>
                            {
                                if (checkWorld.Boss_DungeonID > 0) dungeonIDList.Add(new KeyValuePair<long,long>(checkWorld.WorldID, checkWorld.Boss_DungeonID));
                                if (checkWorld.Normal_DungeonID1 > 0) dungeonIDList.Add(new KeyValuePair<long,long>(checkWorld.WorldID, checkWorld.Normal_DungeonID1));
                                if (checkWorld.Normal_DungeonID2 > 0) dungeonIDList.Add(new KeyValuePair<long,long>(checkWorld.WorldID, checkWorld.Normal_DungeonID2));
                                if (checkWorld.Normal_DungeonID3 > 0) dungeonIDList.Add(new KeyValuePair<long,long>(checkWorld.WorldID, checkWorld.Normal_DungeonID3));
                                if (checkWorld.Normal_DungeonID4 > 0) dungeonIDList.Add(new KeyValuePair<long,long>(checkWorld.WorldID, checkWorld.Normal_DungeonID4));
                                if (checkWorld.Subboss_DungeonID > 0) dungeonIDList.Add(new KeyValuePair<long, long>(checkWorld.WorldID, checkWorld.Subboss_DungeonID));
                                if (checkWorld.Normal_DungeonID6 > 0) dungeonIDList.Add(new KeyValuePair<long, long>(checkWorld.WorldID, checkWorld.Normal_DungeonID6));
                                if (checkWorld.Normal_DungeonID7 > 0) dungeonIDList.Add(new KeyValuePair<long, long>(checkWorld.WorldID, checkWorld.Normal_DungeonID7));
                                if (checkWorld.Normal_DungeonID8 > 0) dungeonIDList.Add(new KeyValuePair<long, long>(checkWorld.WorldID, checkWorld.Normal_DungeonID8));
                                if (checkWorld.Normal_DungeonID9 > 0) dungeonIDList.Add(new KeyValuePair<long, long>(checkWorld.WorldID, checkWorld.Normal_DungeonID9));
                                if (checkWorld.Boss_DungeonID > 0) dungeonIDList.Add(new KeyValuePair<long, long>(checkWorld.WorldID, checkWorld.Boss_DungeonID));
                            });
                        }

                        foreach (KeyValuePair<long, long> setID in dungeonIDList)
                        {
                            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
                            User_Mission_Play getMissionInfo = Dungeon_Manager.GetUser_MissionInfo(ref TB, ref retError, AID, setID.Key, setID.Value);
                            if (!(getMissionInfo.task1.Equals("Y")
                                && getMissionInfo.task1.Equals("Y")
                                && getMissionInfo.task1.Equals("Y")
                                ))
                                return false;
                        }

                        current++;
                        if (current < TriggerValue3)
                            return false;
                        break;
                    }
                case Trigger_Define.eTriggerType.Clear_Event:
                    {
                        if (UserValue1 == TriggerValue1 || current > 0)
                        {
                            current = 1;
                            return true;
                        }
                        else
                            return false;
                    }

                // not use yet (for tutorial)
                //case Trigger_Define.eTriggerType.Clear_Perfect_First:
                //    {
                //        List<RetEliteDungeonRank> userEliteList = Dungeon_Manager.GetUser_All_EliteDungeonRank(ref TB, AID);
                //        var rankInfo = userEliteList.Find(item => (item.worldid == TriggerValue1 || TriggerValue1 == 0) && (item.dungeonid == TriggerValue2 || TriggerValue2 == 0));

                //        if (rankInfo != null)
                //        {
                //            if (rankInfo.rank >= Dungeon_Define.Rank3Star)
                //                current += UserValue3;

                //            if (current < TriggerValue3)
                //                return false;
                //        }
                //    }
                //    break;
                // not work yet
                case Trigger_Define.eTriggerType.Clear_Scenario_Time:
                case Trigger_Define.eTriggerType.Clear_Scenario_Restrict:
                case Trigger_Define.eTriggerType.Clear_Scenario_HP:

                // do not use yet
                case Trigger_Define.eTriggerType.Achieve_Daily_All:
                case Trigger_Define.eTriggerType.Achieve_Daily:
                    return false;
            }
            return true;
        }

        public static bool IsSetMask(long mask, long type)
        {
            return (mask & type) != 0;

            //int index = 0;

            //foreach(int flag in Enum.GetValues(typeof(Trigger_Define.ePvEType)).Cast<int>())
            //{
            //    index += 1;
            //    if ((type & flag) == flag)
            //        return true;
            //}
            //return false;
        }
    }
}

