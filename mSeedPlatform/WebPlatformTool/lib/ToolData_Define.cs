using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using mSeed.Common;

namespace WebPlatformTool
{
    public class ToolData_Define
    {
        public const string CouponDBName = "coupon";
        public const string PlatformDBName = "platform";
        public const string GmDBName = "webadmin";

        public const long AdminSender = 0;
        public const string AdminSenerName = "GM";

        public const int pageSize = 15;
        public const int pageBlock = 10;

        public static readonly Dictionary<ePushStatus, string> PushStatus = new Dictionary<ePushStatus, string>(){
            { ePushStatus.Stop,         "중지停止" },
            { ePushStatus.Unconfirmed,  "미확인未确认" },
            { ePushStatus.Confirm,      "确认完毕" },
            { ePushStatus.Progress,     "完成" },
            { ePushStatus.Finish,       "完成" },
        };
    }
}