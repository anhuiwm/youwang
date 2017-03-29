using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using mSeed.Common;
using mSeed.Platform;
using mSeed.Platform.Coupon;
using mSeed.Platform.Coupon.DBClass;

namespace WebPlatformTool
{
    public class CouponGroup :System_Coupon_Group
    {
        public string strCoupon_Type { get; set; }
        public string gameName { get; set; }
        public string coupon { get; set; }
        public long useCount { get; set; }
        public long rewardID { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }

    public class ExcelCoupon : User_CouponLog
    {
        public string coupon { get; set; }
        public string complete { get; set; }
    }

    public class ToolPush : system_push_service
    {
        public string game_name { get; set; }
        public string strStatus { get; set; }
        public bool buttonActive { get; set; }
    }
}