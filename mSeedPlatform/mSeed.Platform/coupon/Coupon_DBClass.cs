using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mSeed.Platform.Coupon.DBClass
{
    public class System_Coupon
    {
        public long Coupon_Group_ID { get; set; }
        public long game_service_id { get; set; }
        public string Coupon_Code { get; set; }
        public byte Coupon_Type { get; set; }
        public long Coupon_RewardID1 { get; set; }
        public long Coupon_RewardID2 { get; set; }
        public long Coupon_RewardID3 { get; set; }
        public long Coupon_RewardID4 { get; set; }
        public DateTime Coupon_Startdate { get; set; }
        public DateTime Coupon_Enddate { get; set; }
    }

    public class System_Coupon_Group
    {
        public long Coupon_Group_ID { get; set; }
        public long game_service_id { get; set; }
        public byte Coupon_Type { get; set; }
        public byte Coupon_Active { get; set; }
        public string Coupon_Title { get; set; }
        public string Coupon_Memo { get; set; }
        public int Coupon_Num { get; set; }
        public DateTime Discontinue_date { get; set; }
        public DateTime Reg_date { get; set; }
        public string Reg_id { get; set; }
    }

    public class System_Coupon_Reward
    {
        public long idx { get; set; }
        public long game_service_id { get; set; }
        public int Coupon_RewardID { get; set; }
        public string item_info { get; set; }
    }

    public class User_CouponLog
    {
        public long idx { get; set; }
        public long AID { get; set; }
        public string userNickName { get; set; }
        public int server_group_id { get; set; }
        public long Coupon_Group_ID { get; set; }
        public string coupon_Code { get; set; }
        public byte completeflag { get; set; }
        public DateTime reg_date { get; set; }
    }

    public class Number
    {
        public long number { get; set; }
    }

    public class SystemGameItem
    {
        public long game_service_id { get; set; }
        public string itme_info { get; set; }
        public string reqitem_info { get; set; }
    }

    public class RetGameItem
    {
        public string item_id { get; set; }
        public string item_name { get; set; }
    }
}