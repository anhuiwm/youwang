using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Global;

namespace TheSoulGMTool.DBClass
{
    public class Gm_UserLogLevel : User_Admin_LogLevel
    {
        public long aid { get; set; }
        public string username { get; set; }
        public DateTime enddate { get; set; }
        public DateTime regdate { get; set; }
    }

    public class System_Product_ID
    {
        public long Billing_Platform_Type { get; set; }
        public int PriceValue { get; set; }
        public int Real_PriceValue { get; set; }
        public string Product_ID { get; set; }
        public int PriceTier { get; set; }
    }

    public class UserSession
    {
        public string userid { get; set; }
        public string name { get; set; }
        public List<GM_UserAuth> authlist { get; set; }
    }

    public class GM_User
    {
        public int idx { get; set; }
        public string userid { get; set; }
        public string pwd { get; set; }
        public byte userlevel { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string part { get; set; }
        public string rank { get; set; }
        public byte language { get; set; }
        public string reason { get; set; }
        public string isusing { get; set; }
        public string serverAuth { get; set; }
        public DateTime regdate { get; set; }
    }

    public class GM_Global_UserSimple
    {
        public long AID { get; set; }
        public long platform_idx { get; set; }
        public string platform_user_id { get; set; }
    }

    public class GM_UserAccountSimple : GM_Global_UserSimple
    {
        public string UserName { get; set; }
    }

    public class GM_UserAuth
    {
        public string userid { get; set; }
        public short mdiv { get; set; }
        public short auth { get; set; }
    }

    public class GM_ControlLog
    {
        public long idx { get; set; }
        public byte targetType { get; set; }
        public long targetIndex { get; set; }
        public string targetName { get; set; }
        public int controlType { get; set; }
        public string server_id { get; set; }
        public string beforeData { get; set; }
        public string afterData { get; set; }
        public string etcData { get; set; }
        public string adminID { get; set; }
        public string adminName { get; set; }
        public DateTime regdate { get; set; }
    }

    public class System_TriggerType
    {
        public int TriggerID { get; set; }
        public string Trigger { get; set; }
        public string TriggerType { get; set; }
        public string value1 { get; set; }
        public string value2 { get; set; }
        public string value3 { get; set; }
        public string etc { get; set; }
    }

    public class GM_menu
    {
        public int idx { get; set; }
        public short gdiv { get; set; }
        public short mdiv { get; set; }
        public short viewidx { get; set; }
        public string menuname { get; set; }
        public string linkurl { get; set; }
        public string isusing { get; set; }
    }

    public class GM_menu_large
    {
        public int idx { get; set; }
        public short mdiv { get; set; }
        public string menuname { get; set; }
    }

    public class GM_EventGroup_Admin
    {
        public int Event_Group_Type { get; set; }
        public int Event_Index { get; set; }
        public string Event_Title { get; set; }
        public string Event_Intro { get; set; }
        public string Event_Type { get; set; }
        public int Order_Index { get; set; }
        public string ActiveState { get; set; }
    }

    public class GM_Event_Reward_Box
    {
        public int VIP_Level { get; set; }
        public byte BoxItemIndex { get; set; }
        public string EventItem_TargetType { get; set; }
        public long EventItem_ID { get; set; }
        public byte EventItem_Level { get; set; }
        public byte EventItem_Grade { get; set; }
        public int EventItem_Num { get; set; } // <= 지급수량임!!!
    }

    public class Admin_LineNotice
    {
        public long idx { get; set; }
        public DateTime sdate { get; set; }
        public DateTime edate { get; set; }
        public int repeatTime { get; set; }
        public string title { get; set; }
        public string regid { get; set; }
        public DateTime regdate { get; set; }
        public string editeID { get; set; }
        public DateTime? editeDate { get; set; }
        public string flrag { get; set; }
    }

    public class Admin_Notice
    {
        public long idx { get; set; }
        public byte notic_type { get; set; }
        public string title { get; set; }
        public string contents { get; set; }
        public byte active { get; set; }
        public DateTime sdate { get; set; }
        public DateTime edate { get; set; }
        public string type { get; set; }
        public int orderNum { get; set; }
        public int repeatTime { get; set; }
        public DateTime regdate { get; set; }
        public string regid { get; set; }
        public DateTime? editedate { get; set; }
        public string editeid { get; set; }
    }


    public class GM_UserAccount
    {
        public long AID { get; set; }
        public long SNO { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public int Cash { get; set; }
        public int EventCash { get; set; }
        public int Gold { get; set; }
        public DateTime CreationDate { get; set; }
        public int equipclass { get; set; }
        public long EquipCID { get; set; }
        public int VIPLevel { get; set; }
        public string stageInfo { get; set; }
        public byte Tutorial { get; set; }
    }

    public class GM_Billing_List
    {
        public long BillingIndex { get; set; }
        public long platform_idx { get; set; }
        public string platform_user_id { get; set; }
        public long AID { get; set; }
        public string UserName { get; set; }
        public long CID { get; set; }
        public int Shop_Goods_ID { get; set; }
        public string Billing_ID { get; set; }
        public string Billing_Token { get; set; }
        public string Platform_UID { get; set; }
        public int Buy_Platform { get; set; }
        public byte Billing_Status { get; set; }
        public DateTime regdate { get; set; }
        public int Buy_PriceValue { get; set; }
        public string shopType { get; set; }
        public int ItemDay { get; set; }
        public int ItemNum { get; set; }
        public int Bonus_ItemNum { get; set; }
        public string ErrorCode { get; set; }
        public string payResult { get; set; }
        public string goodsName { get; set; }
    }

    public class Admin_String_Naming
    {
        public string StringCN { get; set; }
        public string String { get; set; }
        public string kr { get; set; }
        public string en { get; set; }
        public string jp { get; set; }
        public string cns { get; set; }
        public string cnt { get; set; }
        public string spn { get; set; }
    }

    public class admin_language_code
    {
        public string country{get;set;}
        public string data_language { get; set; }
        public string web_language { get; set; }
    }

    public class GM_Number
    {
        public long number { get; set; }
    }

    public class GM_String
    {
        public string name { get; set; }
    }

    public class GM_System_Package_List
    {
        public long Package_ID { get; set; }
        public byte ActiveType { get; set; }
        public string Buy_PriceType { get; set; }
        public int Buy_PriceValue { get; set; }
        public byte VIP_Level { get; set; }
        public byte Grade { get; set; }
        public byte Max_Buy { get; set; }
        public string NameCN1 { get; set; }
        public string NameCN2 { get; set; }
        public string ToolTipCN { get; set; }
        public string DetailCN { get; set; }
        public long Reward_Box1ID { get; set; }
        public long Reward_Box2ID { get; set; }
        public long Reward_Box3ID { get; set; }
        public long Reward_Box4ID { get; set; }
        public int VIP_Point { get; set; }
        public DateTime SaleStartTime { get; set; }
        public DateTime SaleEndTime { get; set; }
        public byte LoopType { get; set; }
    }

    public class Admin_System_Item
    {
        public long Item_IndexID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ItemClass { get; set; }
        public short ClassNo { get; set; }
        public long Class_IndexID { get; set; }
        public string Buy_PriceType { get; set; }
        public long Buy_PriceValue { get; set; }
        public int Sell_Money { get; set; }
        public int StackMAX { get; set; }
    }

    public class GM_ItemChargeLog
    {
        public long idx { get; set; }
        public string userNames { get; set; }
        public string title { get; set; }
        public string bodytext { get; set; }
        public string item_name_1 { get; set; }
        public long item_id_1 { get; set; }
        public int itemea_1 { get; set; }
        public byte item_grade_1 { get; set; }
        public byte item_level_1 { get; set; }
        public string item_name_2 { get; set; }
        public long item_id_2 { get; set; }
        public int itemea_2 { get; set; }
        public byte item_grade_2 { get; set; }
        public byte item_level_2 { get; set; }
        public string item_name_3 { get; set; }
        public long item_id_3 { get; set; }
        public int itemea_3 { get; set; }
        public byte item_grade_3 { get; set; }
        public byte item_level_3 { get; set; }
        public string item_name_4 { get; set; }
        public long item_id_4 { get; set; }
        public int itemea_4 { get; set; }
        public byte item_grade_4 { get; set; }
        public byte item_level_4 { get; set; }
        public string item_name_5 { get; set; }
        public long item_id_5 { get; set; }
        public int itemea_5 { get; set; }
        public byte item_grade_5 { get; set; }
        public byte item_level_5 { get; set; }
        public DateTime regdate { get; set; }
    }

    public class GM_Overlord_Ranking
    {
        public long rank { get; set; }
        public long AID { get; set; }
        public string UserName { get; set; }
        public int Point { get; set; }
    }

    public class Request_Log
    {
        public long log_idx { get; set; }
        public long AID { get; set; }
        public long CID { get; set; }
        public int ErrorCode { get; set; }
        public string RequestURL { get; set; }
        public string Operation { get; set; }
        public string RequestParams { get; set; }
        public string ResponseResult { get; set; }
        public string BaseJson { get; set; }
        public string DetailDBLog { get; set; }
        public string tableName { get; set; }
        public DateTime requesttime { get; set; }
        public DateTime regdate { get; set; }
    }

    public class Cdkey_Log
    {
        public long log_idx { get; set; }
        public long AID { get; set; }
        public string userName { get; set; }
        public long platform_idx { get; set; }
        public string platform_user_id { get; set; }
        public long CID { get; set; }
        public int ErrorCode { get; set; }
        public string cdkey { get; set; }
        public string mailseq { get; set; }
        public string stateflag { get; set; }
        public long mail_log_idx { get; set; }
        public string RequestParams { get; set; }
        public string ResponseResult { get; set; }
        public string tableName { get; set; }
        public DateTime regdate { get; set; }
    }

    public class BestGachaReward
    {
        public string itemName { get; set; }
        public int itemIndex { get; set; }
        public int minNum { get; set; }
        public int maxNum { get; set; }
        public float itemProb { get; set; }
    }

    public class GM_SoulGroup
    {
        public long SoulGroup { get; set; }
        public byte hide { get; set; }
        public string NamingCN { get; set; }
        public string DescCN { get; set; }
        public string SoulName { get; set; }
    }

    public class GMShopItem
    {
        public long itemID { get; set; }
        public string itemName { get; set; }
    }

    public class GMUserMail
    {
        public long mailseq { get; set; }
        public long aid { get; set; }
        public long senderid { get; set; }
        public string sendername { get; set; }
        public string title { get; set; }
        public long item_id_1 { get; set; }
        public int itemea_1 { get; set; }
        public string bodytext { get; set; }
        public long item_id_2 { get; set; }
        public int itemea_2 { get; set; }
        public long item_id_3 { get; set; }
        public int itemea_3 { get; set; }
        public long item_id_4 { get; set; }
        public int itemea_4 { get; set; }
        public long item_id_5 { get; set; }
        public int itemea_5 { get; set; }
        public string readflag { get; set; }
        public string delflag { get; set; }
        public DateTime closedate { get; set; }
        public DateTime reg_date { get; set; }
    }

    public class GM_Mseed_item_log : mseed_item_log
    {
        public string itemName { get; set; }
        public string tableName { get; set; }
        public byte status { get; set; }
    }

    public class system_log_operation
    {
        public int orderNum { get; set; }
        public string url { get; set; }
        public string Operation { get; set; }
        public string String { get; set; }
        public byte useflag { get; set; }
    }

    public class GM_account_restrict : user_account_restrict
    {
        public int loginActive { get; set; }
        public int chatActive { get; set; }
    }

    public class GM_User_Event : User_Event_Data
    {
        public string Event_Dev_Name { get; set; }
        public string NameCN { get; set; }
        public DateTime Event_StartTime { get; set; }
        public DateTime Event_EndTime { get; set; }
    }

    public class GMUserInvenItem
    {
        public long idx { get; set; }
        public long AID { get; set; }
        public long Item_IndexID { get; set; }
        public int grade { get; set; }
        public int level { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int itemea { get; set; }
    }

    public class GM_user_account_config : user_account_config
    {
        public string play_server { get; set; }
    }

    public class user_restrict_log
    {
        public long idx { get; set; }
        public long user_account_idx { get; set; }
        public DateTime login_restrict_enddate { get; set; }
        public DateTime chat_restrict_endate { get; set; }
        public string memo { get; set; }
        public DateTime regdate { get; set; }
        public string userInfo { get; set; }
    }

}
