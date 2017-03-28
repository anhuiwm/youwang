using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager.DBClass;

namespace TheSoul.DataManager.DBClass
{
    public class System_Event
    {
        public long Event_ID { get; set; }
        public string Event_Type { get; set; }
        public string Event_Dev_Name { get; set; }
        public string Event_Tooltip { get; set; }
        public byte ActiveType { get; set; }
        public DateTime Event_StartTime { get; set; }
        public DateTime Event_EndTime { get; set; }
        public byte Event_Loop { get; set; }
        public byte Event_LoopType { get; set; }
        public string ActiveTriggerType1 { get; set; }
        public int ActiveTriggerType1_Value1 { get; set; }
        public int ActiveTriggerType1_Value2 { get; set; }
        public int ActiveTriggerType1_Value3 { get; set; }
        public string ActiveTriggerType2 { get; set; }
        public int ActiveTriggerType2_Value1 { get; set; }
        public int ActiveTriggerType2_Value2 { get; set; }
        public int ActiveTriggerType2_Value3 { get; set; }
        public string ClearTriggerType1 { get; set; }
        public int ClearTriggerType1_Value1 { get; set; }
        public int ClearTriggerType1_Value2 { get; set; }
        public int ClearTriggerType1_Value3 { get; set; }
        public string ClearTriggerType2 { get; set; }
        public int ClearTriggerType2_Value1 { get; set; }
        public int ClearTriggerType2_Value2 { get; set; }
        public int ClearTriggerType2_Value3 { get; set; }
        public int Reward_Box1ID { get; set; }      // System_Event_Reward_Box.EventBoxID
        public int Reward_Box2ID { get; set; }
        public int Reward_Box3ID { get; set; }
        public int Reward_Box4ID { get; set; }
        public int Event_Price_Ruby { get; set; }
        public string Reward_Mail_Subject_CN { get; set; }
        public string Reward_Mail_Text_CN { get; set; }
        public long OrderID { get; set; }
    }

    public class System_Event_Reward_Box
    {
        public long Reward_BoxID { get; set; }
        public int EventBoxID { get; set; }
        public int VIP_Level { get; set; }
        public byte BoxItemIndex { get; set; }
        public string EventItem_TargetType { get; set; }
        public long EventItem_ID { get; set; }
        public byte EventItem_Level { get; set; }
        public byte EventItem_Grade { get; set; }
        public byte EventItem_Rnd1Type { get; set; }
        public long EventItem_Rnd1Value { get; set; }
        public byte EventItem_Rnd2Type { get; set; }
        public long EventItem_Rnd2Value { get; set; }
        public byte EventItem_Rnd3Type { get; set; }
        public long EventItem_Rnd3Value { get; set; }
        public int EventItem_Num { get; set; }
    }

    public class System_EventGroup_Admin
    {
        public int Event_Group_Type { get; set; }
        public int Event_Index { get; set; }
        public string Event_Title { get; set; }
        public string Event_Intro { get; set; }
        public string Event_Type { get; set; }
        public int Order_Index { get; set; }
    }
    
    public class RetEventGroupAdmin
    {
        public int event_group_type { get; set; }
        public string event_title { get; set; }
        public string event_intro { get; set; }
        public string event_type { get; set; }
        public byte nflag { get; set; }
        public int order_index { get; set; }

        public RetEventGroupAdmin(System_EventGroup_Admin setData)
        {
            event_group_type = setData.Event_Group_Type;
            event_title = setData.Event_Title;
            event_intro = setData.Event_Intro;
            event_type = setData.Event_Type;
            order_index = setData.Order_Index;
            nflag = 0;
        }
    }

    public class System_Event_Daily
    {
        public int Event_Daily_ID { get; set; }
        public string Event_Daily_Type { get; set; }
        public int ActiveType_Event { get; set; }
        public string Event_DevName { get; set; }
        public byte Event_Loop { get; set; }
        public byte Event_LoopType { get; set; }
        public byte Daily_Count { get; set; }
        public int Reward_Box1ID { get; set; }
        public int Reward1_VIP_Level { get; set; }
        public int Reward2_VIP_Level { get; set; }
        public int Reward3_VIP_Level { get; set; }
        public int Reward4_VIP_Level { get; set; }
        public int Reward5_VIP_Level { get; set; }
        public string Reward_Mail_Subject_CN { get; set; }
        public string Reward_Mail_Text_CN { get; set; }
    }

    public class System_Event_First_Payment
    {
        public string Event_FirstPayment_Tooltip1 { get; set; }
        public string Event_FirstPayment_Tooltip2 { get; set; }
        public byte ActiveType_Event { get; set; }
        public int Reward_Box1ID { get; set; }
        public string Reward_Mail_Subject_CN { get; set; }
        public string Reward_Mail_Text_CN { get; set; }
    }

    public class System_Achieve
    {
        public long AchieveID { get; set; }
        public string TaskCN { get; set; }
        public string Description { get; set; }
        public byte Achieve_Type { get; set; }
        public byte Event_LoopType { get; set; }
        public byte Acquire_Type { get; set; }
        public string Icon_Type { get; set; }
        public long Require_AchieveID { get; set; }
        public string ActiveTriggerType1 { get; set; }
        public int ActiveTriggerType1_Value1 { get; set; }
        public int ActiveTriggerType1_Value2 { get; set; }
        public int ActiveTriggerType1_Value3 { get; set; }
        public string ActiveTriggerType2 { get; set; }
        public int ActiveTriggerType2_Value1 { get; set; }
        public int ActiveTriggerType2_Value2 { get; set; }
        public int ActiveTriggerType2_Value3 { get; set; }
        public string ClearTriggerType1 { get; set; }
        public int ClearTriggerType1_Value1 { get; set; }
        public int ClearTriggerType1_Value2 { get; set; }
        public int ClearTriggerType1_Value3 { get; set; }
        public string ClearTriggerType2 { get; set; }
        public int ClearTriggerType2_Value1 { get; set; }
        public int ClearTriggerType2_Value2 { get; set; }
        public int ClearTriggerType2_Value3 { get; set; }
        public long Reward_Box1ID { get; set; }
        public long Reward_Box2ID { get; set; }
        public long Reward_Box3ID { get; set; }
        public long Reward_Box4ID { get; set; }
        public int Reward_EXP { get; set; }
    }

    public class System_Achieve_RewardBox : System_Event_Reward_Box
    {
    }

    public class System_Achieve_PvP : System_Achieve
    {
        public int PVP_Type { get; set; }
        public string Ranking_List_Type { get; set; }

        public static System_Achieve_PvP CastToSystem_Achieve_PvP(System_Achieve baseObj)
        {
            System_Achieve_PvP setObj = new System_Achieve_PvP();
            setObj.AchieveID = baseObj.AchieveID;
            setObj.TaskCN = baseObj.TaskCN;
            setObj.Description = baseObj.Description;
            setObj.Achieve_Type = baseObj.Achieve_Type;
            setObj.Event_LoopType = baseObj.Event_LoopType;
            setObj.Acquire_Type = baseObj.Acquire_Type;
            setObj.Icon_Type = baseObj.Icon_Type;
            setObj.Require_AchieveID = baseObj.Require_AchieveID;
            setObj.ActiveTriggerType1 = baseObj.ActiveTriggerType1;
            setObj.ActiveTriggerType1_Value1 = baseObj.ActiveTriggerType1_Value1;
            setObj.ActiveTriggerType1_Value2 = baseObj.ActiveTriggerType1_Value2;
            setObj.ActiveTriggerType1_Value3 = baseObj.ActiveTriggerType1_Value3;
            setObj.ActiveTriggerType2 = baseObj.ActiveTriggerType2;
            setObj.ActiveTriggerType2_Value1 = baseObj.ActiveTriggerType2_Value1;
            setObj.ActiveTriggerType2_Value2 = baseObj.ActiveTriggerType2_Value2;
            setObj.ActiveTriggerType2_Value3 = baseObj.ActiveTriggerType2_Value3;
            setObj.ClearTriggerType1 = baseObj.ClearTriggerType1;
            setObj.ClearTriggerType1_Value1 = baseObj.ClearTriggerType1_Value1;
            setObj.ClearTriggerType1_Value2 = baseObj.ClearTriggerType1_Value2;
            setObj.ClearTriggerType1_Value3 = baseObj.ClearTriggerType1_Value3;
            setObj.ClearTriggerType2 = baseObj.ClearTriggerType2;
            setObj.ClearTriggerType2_Value1 = baseObj.ClearTriggerType2_Value1;
            setObj.ClearTriggerType2_Value2 = baseObj.ClearTriggerType2_Value2;
            setObj.ClearTriggerType2_Value3 = baseObj.ClearTriggerType2_Value3;
            setObj.Reward_Box1ID = baseObj.Reward_Box1ID;
            setObj.Reward_Box2ID = baseObj.Reward_Box2ID;
            setObj.Reward_Box3ID = baseObj.Reward_Box3ID;
            setObj.Reward_Box4ID = baseObj.Reward_Box4ID;
            setObj.Reward_EXP = baseObj.Reward_EXP;
            setObj.PVP_Type = 0;
            setObj.Ranking_List_Type = "";
            return setObj;
        }
    }

    public class System_Achieve_PvP_RewardBox : System_Achieve_RewardBox
    {
    }

    public class User_Event_Data
    {
        public long User_Event_ID { get; set; }
        public long AID { get; set; }
        public long Event_ID { get; set; }
        public string Event_Type { get; set; }
        public short Event_LoopType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public long CurrentValue1 { get; set; }
        public string ClearTriggerType1 { get; set; }
        public int ClearTriggerType1_Value1 { get; set; }
        public int ClearTriggerType1_Value2 { get; set; }
        public int ClearTriggerType1_Value3 { get; set; }
        public long CurrentValue2 { get; set; }
        public string ClearTriggerType2 { get; set; }
        public int ClearTriggerType2_Value1 { get; set; }
        public int ClearTriggerType2_Value2 { get; set; }
        public int ClearTriggerType2_Value3 { get; set; }
        public string ClearFlag { get; set; }
        public string RewardFlag { get; set; }
        public bool isActive;
        public bool isFail;
        public bool isStart;
        public int Event_Price_Ruby;
        public int PVP_Type { get; set; }
        public string Ranking_List_Type { get; set; }
        public long EventOrderID { get; set; }

        public User_Event_Data() {
            isActive = isFail = isStart = false;
            Event_Type = ClearTriggerType1 = ClearTriggerType2 = Ranking_List_Type = string.Empty;
            ClearFlag = RewardFlag = "N";
        }

        public User_Event_Data(System_Event setEvent, long setAID)
        {
            AID = setAID;
            Event_ID = setEvent.Event_ID;
            Event_Type = string.IsNullOrEmpty(setEvent.Event_Type) ? "" : setEvent.Event_Type;
            Event_LoopType = setEvent.Event_LoopType;
            StartTime = DateTime.Now;
            EndTime = setEvent.Event_EndTime;
            ClearTriggerType1 = setEvent.ClearTriggerType1;
            ClearTriggerType1_Value1 = setEvent.ClearTriggerType1_Value1;
            ClearTriggerType1_Value2 = setEvent.ClearTriggerType1_Value2;
            ClearTriggerType1_Value3 = setEvent.ClearTriggerType1_Value3;
            ClearTriggerType2 = setEvent.ClearTriggerType2;
            ClearTriggerType2_Value1 = setEvent.ClearTriggerType2_Value1;
            ClearTriggerType2_Value2 = setEvent.ClearTriggerType2_Value2;
            ClearTriggerType2_Value3 = setEvent.ClearTriggerType2_Value3;
            Event_Price_Ruby = setEvent.Event_Price_Ruby;
            ClearFlag = RewardFlag = "N";
            CurrentValue1 = CurrentValue2 = 0;
            isActive = isFail = isStart = false;
            EventOrderID = setEvent.OrderID;
        }
    }

    public class User_Event_Check_Data
    {
        public long AID { get; set; }
        public DateTime RegDate { get; set; }
        public int CheckCount { get; set; }
        public int RewardCount { get; set; }
        public int AddCount { get; set; }
        public string VIPRewardList { get; set; }
        public string FirstPaymentFlag { get; set; }

        public User_Event_Check_Data(long setAID = 0)
        {
            AID = setAID;
            RegDate = DateTime.Now;
            AddCount = RewardCount = 0;
            CheckCount = 1;
            FirstPaymentFlag = "N";
            VIPRewardList = "[]";
        }
    }

    public class Ret_Event_Check_Data
    {
        public int checkcount { get; set; }
        public int rewardcount { get; set; }
        public int addcount { get; set; }
        public int max_addcount { get; set; }
        public int add_limit { get; set; }
        public string first_pay_flag { get; set; }

        public Ret_Event_Check_Data() { first_pay_flag = "N"; }
        public Ret_Event_Check_Data(User_Event_Check_Data setEv, int maxcount)
        {
            checkcount = setEv.CheckCount;
            rewardcount = setEv.RewardCount;
            first_pay_flag = setEv.FirstPaymentFlag;
            addcount = setEv.AddCount;
            max_addcount = maxcount;
            add_limit = DateTime.Now.Day - checkcount;            
        }
    }

    public class Ret_Daily_Event_List
    {
        public Dictionary<long, List<Ret_Reward_Item>> count_list { get; set; }
        public Dictionary<long, List<Ret_Reward_Item>> over_list { get; set; }
        public List<Ret_Reward_Item> everyday { get; set; }

        public Ret_Daily_Event_List()
        {
            count_list = new Dictionary<long, List<Ret_Reward_Item>>();
            over_list = new Dictionary<long, List<Ret_Reward_Item>>();
            everyday = new List<Ret_Reward_Item>();
        }
    }

    public class Ret_Reward_Item
    {
        public long itemid { get; set; }
        public int itemea { get; set; }
        public short itemgrade { get; set; }
        public short itemlevel { get; set; }
        public long viplevel { get; set; }

        public Ret_Reward_Item() { }
        public Ret_Reward_Item(long setitemid, int setitemea, short grade, short level, long setviplevel)
        {
            itemid = setitemid;
            itemea = setitemea;
            itemgrade = grade;
            itemlevel = level;
            viplevel = setviplevel;
        }

        public Ret_Reward_Item(System_Event_Reward_Box setitem)
        {
            itemid = setitem.EventItem_ID;
            itemea = setitem.EventItem_Num;
            itemgrade = setitem.EventItem_Grade;
            itemlevel = setitem.EventItem_Level;
            viplevel = setitem.VIP_Level;
        }

        public Ret_Reward_Item(System_Achieve_PvP_RewardBox setitem)
        {
            itemid = setitem.EventItem_ID;
            itemea = setitem.EventItem_Num;
            itemgrade = setitem.EventItem_Grade;
            itemlevel = setitem.EventItem_Level;
            viplevel = setitem.VIP_Level;
        }
    }

    public class Ret_Event_Data
    {
        public long user_event_idx { get; set; }
        public long aid { get; set; }
        public long event_id { get; set; }
        public string event_type { get; set; }
        public short event_loop_type { get; set; }
        public long lefttime { get; set; }
        public long currentvalue1 { get; set; }
        public long max_value1 { get; set; }
        public long currentvalue2 { get; set; }
        public long max_value2 { get; set; }
        public string clearflag { get; set; }
        public string rewardflag { get; set; }
        public long event_price_ruby { get; set; }
        public string event_name { get; set; }
        public List<Ret_Reward_Item> reward_item { get; set; }
        public long orderid { get; set; }

        public Ret_Event_Data() { }
        public Ret_Event_Data(User_Event_Data setEvent, long MaxValue1, long MaxValue2, bool isClear = false, string PvPRankListType = "")
        {
            user_event_idx = setEvent.User_Event_ID;
            aid = setEvent.AID;
            event_id = setEvent.Event_ID;
            event_type = string.IsNullOrEmpty(PvPRankListType) ? setEvent.Event_Type : PvPRankListType;
            event_loop_type = setEvent.Event_LoopType;

            TimeSpan TS = setEvent.EndTime - DateTime.Now;

            lefttime = System.Convert.ToInt64(TS.TotalSeconds);

            if (!Trigger_Define.TriggerString[Trigger_Define.eTriggerType.Clear_Event].Equals(setEvent.ClearTriggerType1))
            {
                currentvalue1 = setEvent.CurrentValue1;
                max_value1 = MaxValue1;
            }
            if (!Trigger_Define.TriggerString[Trigger_Define.eTriggerType.Clear_Event].Equals(setEvent.ClearTriggerType2))
            {
                currentvalue2 = setEvent.CurrentValue2;
                max_value2 = MaxValue2;
            }
            clearflag = isClear ? "Y" : "N";
            rewardflag = setEvent.RewardFlag;
            orderid = setEvent.EventOrderID;
        }
    }
    
    public class Ret_Achieve_Data
    {
        public long user_event_idx { get; set; }
        public long aid { get; set; }
        public long event_id { get; set; }
        public string event_type { get; set; }
        public short event_loop_type { get; set; }
        public long lefttime { get; set; }
        public long currentvalue1 { get; set; }
        public long max_value1 { get; set; }
        public long currentvalue2 { get; set; }
        public long max_value2 { get; set; }
        public string clearflag { get; set; }
        public string rewardflag { get; set; }

        public Ret_Achieve_Data() { }
        public Ret_Achieve_Data(User_Event_Data setEvent, long MaxValue1, long MaxValue2, bool isClear = false, string PvPRankListType = "")
        {
            user_event_idx = setEvent.User_Event_ID;
            aid = setEvent.AID;
            event_id = setEvent.Event_ID;
            event_type = string.IsNullOrEmpty(PvPRankListType) ? setEvent.Event_Type : PvPRankListType;
            event_loop_type = setEvent.Event_LoopType;

            TimeSpan TS = setEvent.EndTime - DateTime.Now;

            lefttime = System.Convert.ToInt64(TS.TotalSeconds);

            currentvalue1 = setEvent.CurrentValue1;
            currentvalue2 = setEvent.CurrentValue2;
            max_value1 = MaxValue1;
            max_value2 = MaxValue2;
            clearflag = isClear ? "Y" : "N";
            rewardflag = setEvent.RewardFlag;
        }
    }

    public class TriggerProgressData
    {
        public Trigger_Define.eTriggerType Trigger_type { get; set; }
        public long CheckValue1 { get; set; }
        public long CheckValue2 { get; set; }
        public long CheckValue3 { get; set; }

        public TriggerProgressData(Trigger_Define.eTriggerType type, long value1 = 0, long value2 = 0, long value3 = 1)
        {
            Trigger_type = type;
            CheckValue1 = value1;
            CheckValue2 = value2;
            CheckValue3 = value3;
        }
    }

    public class System_Event_7Day : System_Event
    {
    }

    public class User_Event_7Day_Data
    {
        public long User_Event_ID { get; set; }
        public long AID { get; set; }
        public long Event_ID { get; set; }
        public long ShopGoodsID { get; set; }
        public string ClearFlag { get; set; }
        public string RewardFlag { get; set; }

        public User_Event_7Day_Data()
        {
            ClearFlag = RewardFlag = "N";
        }
    }

    public class Ret_Event_7Day_Data
    {
        public long user_event_idx { get; set; }
        public long aid { get; set; }
        public long event_id { get; set; }
        public string event_type { get; set; }
        public short event_loop_type { get; set; }
        public long currentvalue1 { get; set; }
        public long max_value1 { get; set; }
        public long currentvalue2 { get; set; }
        public long max_value2 { get; set; }
        public string clearflag { get; set; }
        public string rewardflag { get; set; }
        public long event_price_ruby { get; set; }
        public string event_name { get; set; }
        public List<Ret7DayReward> reward_item { get; set; }

        public Ret_Event_7Day_Data(System_Event setEvent, long setAID)
        {
            aid = setAID;
            event_id = setEvent.Event_ID;
            event_type = setEvent.Event_Type;
            event_loop_type = setEvent.Event_LoopType;
            aid = setAID;
            event_id = setEvent.Event_ID;
            clearflag = rewardflag = "N";
            event_name = setEvent.Event_Tooltip;
            reward_item = new List<Ret7DayReward>();
        }
    }

    public class System_Event_7Day_Package_List : System_Package_List
    {
        public byte Buy_Day { get; set; }
    }

    public class System_Event_7Day_Reward : System_Package_RewardBox
    {
    }


    public class Ret7Day_PackageList
    {
        public long package_id { get; set; }
        public byte buy_price_type { get; set; }
        public int buy_price_value { get; set; }
        public byte buy_day { get; set; }
        public byte grade { get; set; }
        public int buy_count { get; set; }
        public int max_buy_count { get; set; }
        public string name_cn1 { get; set; }
        public string name_cn2 { get; set; }
        public string tooltip_cn { get; set; }
        public string detail_cn { get; set; }
        public List<Ret7DayReward> item_list { get; set; }

        public Ret7Day_PackageList() { item_list = new List<Ret7DayReward>(); }
        public Ret7Day_PackageList(System_Event_7Day_Package_List setList, int buyCount = 0)
        {
            package_id = setList.Package_ID;
            buy_price_type = (byte)Item_Define.Item_BuyType_List[setList.Buy_PriceType];
            buy_price_value = setList.Buy_PriceValue;
            buy_day = setList.Buy_Day;
            grade = setList.Grade;
            buy_count = buyCount;
            max_buy_count = setList.Max_Buy;
            name_cn1 = setList.NameCN1;
            name_cn2 = setList.NameCN2;
            tooltip_cn = setList.ToolTipCN;
            detail_cn = setList.DetailCN;
            item_list = new List<Ret7DayReward>();
        }
    }

    public class Ret7DayReward : RetPackageReward
    {
        public Ret7DayReward(System_Event_7Day_Reward setItem)
        {
            item_id = setItem.Item_ID;
            item_num = setItem.Item_Num;
            item_grade = setItem.Item_Grade;
            item_level = setItem.Item_Level;
        }
    }
}
