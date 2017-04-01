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
        public static int Check7DayEvent(ref TxnBlock TB, long AID, DateTime creationdate)
        {
            int admin_7Day_flag = SystemData.AdminConstValueFetchFromRedis(ref TB, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.ADMIN_7DAY_EVENT_ON_OFF]);

            if (admin_7Day_flag > 0)
            {
                TimeSpan EventTS = DateTime.Now - creationdate;
                int Event_Day_Limit = SystemData.GetConstValueInt(ref TB, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.DEF_7DAY_EVENT_LIMIT_DAY]);
                if (EventTS.Days <= Event_Day_Limit)
                    admin_7Day_flag = 1;
                else
                    admin_7Day_flag = 0;
            }

            return admin_7Day_flag;
        }

        public static Result_Define.eResult SetUser_7Day_Event_Info(ref TxnBlock TB, long AID, User_Event_7Day_Data setInfo, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            long setID = setInfo.Event_ID > 0 ? setInfo.Event_ID : setInfo.ShopGoodsID;
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON AID = {1} AND {7} = {2}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                ClearFlag = '{5}',
	                                                RewardFlag = '{6}'
                                                WHEN NOT MATCHED THEN
                                                   INSERT (AID, Event_ID, ShopGoodsID, ClearFlag, RewardFlag) VALUES ({1}, {3}, {4}, '{5}', '{6}');
                                    ", Trigger_Define.User_Event_7Day_Data_TableName
                         , AID
                         , setID
                         , setInfo.Event_ID
                         , setInfo.ShopGoodsID
                         , setInfo.ClearFlag
                         , setInfo.RewardFlag
                         , setInfo.Event_ID > 0 ? "Event_ID" : "ShopGoodsID"
                         );
            RemoveUser_7Day_Event_InfoCache(AID, setID, setInfo.Event_ID > 0);

            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        private static string GetUser_7Day_EventKey(long AID)
        {
            return string.Format("{0}_{1}_{2}", Trigger_Define.User_7DayEvent_Prefix, Trigger_Define.User_Event_7Day_Data_TableName, AID);
        }

        private static void RemoveUser_7Day_Event_InfoCache(long AID, long EventID, bool isEvent)
        {
            string setKey = "";
            if (isEvent)
                setKey = GetUser_7Day_EventKey(AID);
            else
                setKey = GetUser_7Day_PackageKey(AID);

            RedisConst.GetRedisInstance().RemoveHashItem(DataManager_Define.RedisServerAlias_User, setKey, EventID.ToString());
        }

        public static User_Event_7Day_Data GetUser_7Day_Event_Info(ref TxnBlock TB, long AID, long EventID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = GetUser_7Day_EventKey(AID);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) WHERE AID = {1} AND Event_ID = {2}", Trigger_Define.User_Event_7Day_Data_TableName, AID, EventID);

            User_Event_7Day_Data retObj = GenericFetch.FetchFromRedis_Hash<User_Event_7Day_Data>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, EventID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
            {
                retObj = new User_Event_7Day_Data();
                retObj.Event_ID = EventID;
                Result_Define.eResult retError = SetUser_7Day_Event_Info(ref TB, AID, retObj);
                if (retError == Result_Define.eResult.SUCCESS)
                    retObj = GenericFetch.FetchFromDB<User_Event_7Day_Data>(ref TB, setQuery, dbkey);
            }

            return retObj == null ? new User_Event_7Day_Data() : retObj;
        }


        private static string GetUser_7Day_PackageKey(long AID)
        {
            return string.Format("{0}_{1}_{2}", Trigger_Define.User_7DayPackage_Prefix, Trigger_Define.User_Event_7Day_Data_TableName, AID);
        }


        public static User_Event_7Day_Data GetUser_7Day_Package_Info(ref TxnBlock TB, long AID, long PackageID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = GetUser_7Day_PackageKey(AID);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) WHERE AID = {1} AND ShopGoodsID = {2}", Trigger_Define.User_Event_7Day_Data_TableName, AID, PackageID);

            User_Event_7Day_Data retObj = GenericFetch.FetchFromRedis_Hash<User_Event_7Day_Data>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, PackageID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
            {
                retObj = new User_Event_7Day_Data();
                retObj.ShopGoodsID = PackageID;
                Result_Define.eResult retError = SetUser_7Day_Event_Info(ref TB, AID, retObj);
                if (retError == Result_Define.eResult.SUCCESS)
                    retObj = GenericFetch.FetchFromDB<User_Event_7Day_Data>(ref TB, setQuery, dbkey);
            }

            return retObj == null ? new User_Event_7Day_Data() : retObj;
        }        

        public static System_Event_7Day GetSystem_7Day_Event_Info(ref TxnBlock TB, long EventID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}", Trigger_Define.Trigger_Prefix, Trigger_Define.System_7Day_Event_TableName, EventID);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) WHERE Event_ID = {1} ", Trigger_Define.System_7Day_Event_TableName, EventID);

            return GenericFetch.FetchFromDB<System_Event_7Day>(ref TB, setQuery, dbkey);
        }

        public static string GetRediskey_System_7Day_Event_List()
        {
            return string.Format("{0}_{1}", Trigger_Define.Trigger_Prefix, Trigger_Define.System_7Day_Event_TableName);
        }

        public static List<System_Event_7Day> GetSystem_7Day_Event_List(ref TxnBlock TB, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = GetRediskey_System_7Day_Event_List();
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)", Trigger_Define.System_7Day_Event_TableName);

            return GenericFetch.FetchFromRedis_MultipleRow<System_Event_7Day>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
        }

        public static System_Event_7Day_Package_List GetSystem_7Day_Event_Package_Info(ref TxnBlock TB, long PackageID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}", Trigger_Define.Trigger_Prefix, Trigger_Define.System_7Day_Event_Package_TableName, PackageID);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) WHERE Package_ID = {1} ", Trigger_Define.System_7Day_Event_Package_TableName, PackageID);

            return GenericFetch.FetchFromDB<System_Event_7Day_Package_List>(ref TB, setQuery, dbkey);
        }

        public static string GetRediskey_System_7Day_Event_Package_List()
        {
            return string.Format("{0}_{1}", Trigger_Define.Trigger_Prefix, Trigger_Define.System_7Day_Event_Package_TableName);
        }

        public static List<System_Event_7Day_Package_List> GetSystem_7Day_Event_Package_List(ref TxnBlock TB, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = GetRediskey_System_7Day_Event_Package_List();
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) ", Trigger_Define.System_7Day_Event_Package_TableName);

            return GenericFetch.FetchFromRedis_MultipleRow<System_Event_7Day_Package_List>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
        }

        public static string GetRediskey_System_7Day_Event_Package_RewardBox()
        {
            return string.Format("{0}_{1}", Trigger_Define.Trigger_Prefix, Trigger_Define.System_7Day_Event_Reward_TableName);
        }

        public static List<System_Event_7Day_Reward> GetSystem_7Day_Event_Package_RewardBox(ref TxnBlock TB, long RewardBoxID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = GetRediskey_System_7Day_Event_Package_RewardBox();
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE RewardBoxID = {1}", Trigger_Define.System_7Day_Event_Reward_TableName, RewardBoxID);

            return GenericFetch.FetchFromRedis_MultipleRow_Hash<System_Event_7Day_Reward>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, RewardBoxID.ToString(), setQuery, dbkey, Flush);
        }

        public static List<Ret7DayReward> Get7Day_Reward_List(ref TxnBlock TB, long AID, long CID, long RewardBoxID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            List<Ret7DayReward> retObj = new List<Ret7DayReward>();
            List<System_Event_7Day_Reward> rewardList = GetSystem_7Day_Event_Package_RewardBox(ref TB, RewardBoxID);

            rewardList.ForEach(setItem =>
            {
                retObj.Add(new Ret7DayReward(setItem));
            }
            );

            return retObj;
        }

        /// UserValue 仅仅是用户的天数？
        public static bool Check7Day_Clear_Trigger(ref TxnBlock TB, long AID, out long Current, Trigger_Define.eTriggerType TriggerType, 
            int TriggerValue1, int TriggerValue2, int TriggerValue3, int UserValue = 0)
        {
            switch (TriggerType)
            {
                case Trigger_Define.eTriggerType.Event7Day:
                    Current = UserValue;
                    return TriggerValue3 <= UserValue;
                case Trigger_Define.eTriggerType.Charge_Price:
                    Current = ShopManager.GetUserBillingCash(ref TB, AID);
                    return TriggerValue3 <= Current;
                case Trigger_Define.eTriggerType.VIP_Point:
                    Current = VipManager.GetUser_VIPInfo(ref TB, AID).totalvippoint;
                    return TriggerValue3 <= Current;
                case Trigger_Define.eTriggerType.AttackPower:
                    AccountManager.RemoveUser_UserWarPoint(AID);
                    /// 战力规则，客户端的战力是除以10的显示
                    Current = AccountManager.GetUserWarPoint(ref TB, AID).WAR_POINT / 10;
                    return TriggerValue3 <= Current;
                case Trigger_Define.eTriggerType.DivineWeapon:
                    List<User_Ultimate_Inven> setObj = new List<User_Ultimate_Inven>();
                    setObj = ItemManager.GetMaxUser_Ultimate_Inven(ref TB, AID);
                    Current = 0;
                    if (setObj.Count > 0)
                    {
                        Current = setObj[0].level;
                    }
                    return TriggerValue3 <= Current;
                case Trigger_Define.eTriggerType.None:
                    Current = 0;
                    return true;
                //case Trigger_Define.eTriggerType.Armor_GradeUp:
                //    Current = 1;
                //    return true;
                //case Trigger_Define.eTriggerType.Soul_LvUp:
                //    Current = 2;
                //    return true;
                //case Trigger_Define.eTriggerType.Weapon_LvUp:
                //    Current = 3;
                //    return true;
                //case Trigger_Define.eTriggerType.Play_PVP:
                //    Current = 4;
                //    return true;
                //case Trigger_Define.eTriggerType.Kill_Count:
                //    Current = 5;
                //    return true;
                //default:
                //    {
                //        Current = UserValue;
                //        /// 如果当前的值小于填写的第三个参数，则为失败，否则则通过过滤
                //        if (Current < TriggerValue3)
                //            return false;
                //        break;
                //    }
                //    return true;
            }
            Current = 0;
            return false;
        }

        public static Dictionary<long, List<Ret_Event_7Day_Data>> Check_7Day_Data_List(ref TxnBlock TB, long AID, long CID, int loginCount = 0, bool Flush = false)
        {
            List<System_Event_7Day> system7DayInfo = GetSystem_7Day_Event_List(ref TB);

            List<Ret_Event_7Day_Data> setEventList = new List<Ret_Event_7Day_Data>();
            Character charInfo = CharacterManager.GetCharacter(ref TB, AID, CID);
            Dictionary<long, List<Ret_Event_7Day_Data>> retEventList = new Dictionary<long, List<Ret_Event_7Day_Data>>();

            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            foreach (System_Event_7Day setItem in system7DayInfo)
            {
                User_Event_7Day_Data userEventInfo = GetUser_7Day_Event_Info(ref TB, AID, setItem.Event_ID);
                Ret_Event_7Day_Data setEventInfo = new Ret_Event_7Day_Data(setItem, AID);
                bool isUpdated = false;
                long setDayKey = 0;

                if (!userEventInfo.ClearFlag.Equals("Y"))
                {
                    long current = 0;
                    bool ClearFlag1 = TriggerManager.Check7Day_Clear_Trigger(ref TB, AID, out current, Trigger_Define.TriggerType[setItem.ClearTriggerType1], setItem.ClearTriggerType1_Value1, setItem.ClearTriggerType1_Value2, setItem.ClearTriggerType1_Value3, loginCount);
                    setEventInfo.currentvalue1 = current > setItem.ClearTriggerType1_Value3 ? setItem.ClearTriggerType1_Value3 : current;
                    setEventInfo.max_value1 = setItem.ClearTriggerType1_Value3;

                    bool ClearFlag2 = TriggerManager.Check7Day_Clear_Trigger(ref TB, AID, out current, Trigger_Define.TriggerType[setItem.ClearTriggerType2], setItem.ClearTriggerType2_Value1, setItem.ClearTriggerType2_Value2, setItem.ClearTriggerType2_Value3, loginCount);
                    setEventInfo.currentvalue2 = current > setItem.ClearTriggerType2_Value3 ? setItem.ClearTriggerType2_Value3 : current;
                    setEventInfo.max_value2 = setItem.ClearTriggerType2_Value3;

                    setEventInfo.clearflag = ClearFlag1 && ClearFlag2 ? "Y" : "N";
                    setEventInfo.rewardflag = "N";

                    isUpdated = (userEventInfo.ClearFlag != setEventInfo.clearflag)
                        || (userEventInfo.RewardFlag != setEventInfo.rewardflag);

                    if (isUpdated)
                    {
                        userEventInfo.ClearFlag = setEventInfo.clearflag;
                        userEventInfo.RewardFlag = setEventInfo.rewardflag;
                        retError = SetUser_7Day_Event_Info(ref TB, AID, userEventInfo);
                    }
                }
                else
                {
                    setEventInfo.max_value1 = setEventInfo.currentvalue1 = setItem.ClearTriggerType1_Value3;
                    setEventInfo.max_value2 = setEventInfo.currentvalue2 = setItem.ClearTriggerType2_Value3;
                    setEventInfo.clearflag = userEventInfo.ClearFlag;
                    setEventInfo.rewardflag = userEventInfo.RewardFlag;
                }
                setEventInfo.user_event_idx = userEventInfo.User_Event_ID;

                if (Trigger_Define.TriggerType[setItem.ClearTriggerType1] == Trigger_Define.eTriggerType.Event7Day)
                    setDayKey = setEventInfo.max_value1;
                else if (Trigger_Define.TriggerType[setItem.ClearTriggerType2] == Trigger_Define.eTriggerType.Event7Day)
                    setDayKey = setEventInfo.max_value2;

                List<Ret7DayReward> rewardList = new List<Ret7DayReward>();
                if (setItem.Reward_Box1ID > 0)
                    rewardList.AddRange(Get7Day_Reward_List(ref TB, AID, CID, setItem.Reward_Box1ID));

                if (charInfo.cid > 0)
                {
                    if (setItem.Reward_Box2ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Warrior)
                        rewardList.AddRange(Get7Day_Reward_List(ref TB, AID, CID, setItem.Reward_Box2ID));
                    if (setItem.Reward_Box3ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Swordmaster)
                        rewardList.AddRange(Get7Day_Reward_List(ref TB, AID, CID, setItem.Reward_Box3ID));
                    if (setItem.Reward_Box4ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Taoist)
                        rewardList.AddRange(Get7Day_Reward_List(ref TB, AID, CID, setItem.Reward_Box4ID));
                }
                setEventInfo.reward_item = rewardList;

                if (!retEventList.ContainsKey(setDayKey))
                    retEventList[setDayKey] = new List<Ret_Event_7Day_Data>();                    

                retEventList[setDayKey].Add(setEventInfo);
            }
            
            return retEventList;
        }

        public static Dictionary<long, Ret7Day_PackageList> Check_7Day_Package_List(ref TxnBlock TB, long AID, long CID, bool Flush = false)
        {
            Dictionary<long, Ret7Day_PackageList> retObj = new Dictionary<long, Ret7Day_PackageList>();
            List<System_Event_7Day_Package_List> shopPackageList = GetSystem_7Day_Event_Package_List(ref TB);

            Character charInfo = CharacterManager.GetCharacter(ref TB, AID, CID);

            foreach (System_Event_7Day_Package_List setShop in shopPackageList)
            {
                User_Event_7Day_Data userEventInfo = GetUser_7Day_Package_Info(ref TB, AID, setShop.Package_ID);
                Ret7Day_PackageList setPackage = new Ret7Day_PackageList(setShop, userEventInfo.RewardFlag.Equals("Y") ? 1 : 0);

                List<Ret7DayReward> packageRewardList = new List<Ret7DayReward>();
                if (setShop.Reward_Box1ID > 0)
                    packageRewardList.AddRange(Get7Day_Reward_List(ref TB, AID, CID, setShop.Reward_Box1ID));
                if (setShop.Reward_Box2ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Warrior)
                    packageRewardList.AddRange(Get7Day_Reward_List(ref TB, AID, CID, setShop.Reward_Box2ID));
                if (setShop.Reward_Box3ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Swordmaster)
                    packageRewardList.AddRange(Get7Day_Reward_List(ref TB, AID, CID, setShop.Reward_Box3ID));
                if (setShop.Reward_Box4ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Taoist)
                    packageRewardList.AddRange(Get7Day_Reward_List(ref TB, AID, CID, setShop.Reward_Box4ID));

                setPackage.item_list = packageRewardList;                
                retObj[setShop.Buy_Day] = setPackage;               
            }
            return retObj;
        }

        //public static void RemoveAchieveDataFromRedis(long AID, bool isSystem = false)
        //{
        //    string setKey = "";
        //    if (isSystem)
        //    {
        //        setKey = string.Format("{0}_{1}", Trigger_Define.Trigger_Prefix, Trigger_Define.System_Achieve_TableName);
        //        RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_System, setKey);
        //    }
        //    setKey = string.Format("{0}_{1}_{2}", Trigger_Define.User_Event_Prefix, Trigger_Define.User_Achieve_Data_TableName, AID);
        //    RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        //}

    }
}
