using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationTool
{
    public class Operation_Define
    {
        public const string OperationDBName = "operation";
        public const string GmDBName = "webadmin";
        public const string GlobalDBName = "global";
        public const string CommonDBName = "common";
        public const string CommonLogDBName = "commonLog";
        public const string ShardingDBName = "sharding";
        public const string LogDBName = "log";

        public const string SystemCouponGroupTableName = "System_Coupon_Group";
        public const string SystemCouponTableName = "System_Coupon";
        public const string SystemCouponRewardTableName = "System_Coupon_Reward";
        public const string UserCouponLogTableName = "User_CouponLog";

        public const int CompressionLength = 2048; // 2k byte over
    }

    public class DefineError
    {
        public const string reqOperation = "operation";
        public const string reqParams = "params";
        public const string retResult = "result";
        public const string retResultCode = "resultcode";
        public const string retEncryptData = "returndata";
        public const string retCompressionData = "compressdata";
        public const string System_ItemBase_NOT_FOUND = "couldn't read System_ITEM_Base data.";
        public const string System_Unknown_Operation = "unknown operation request";
    }

    public class ErrorReturnString
    {
        public string Error { get; set; }

        public ErrorReturnString(string e)
        {
            Error = e;
        }
    }

    public class Result_Define
    {
        public enum eResult
        {
            SUCCESS = 0,
            POST_DATA_ERROR = 1,
            DB_STOREDPROCEDURE_ERROR = 97,
            DB_ERROR = 99,

            //coupon
            COUPONCODE_REGIST_ALREADY = 500, //이미 생성된 쿠폰코드
            COUPONCODE_INCORRECT = 501, //잘못 된 쿠폰 번호
            COUPONCODE_REWARD_ALREADY = 502, //이미 보상을 받은 쿠폰

            System_Exception = 1999, // STRING_MSG_ERROR_SYSTEM_EXCEPTION    시스템 에러입니다. 개발팀에 문의해 주세요.
            System_Unknown_Operation = 1900, // STRING_MSG_ERROR_SYSTEM_UNKNOWN    유효하지 않은 요청입니다. 
        }
    }
}