using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mSeed.Common
{
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

            DB_STOREDPROCEDURE_ERROR = 97,          // DB SP Error
            DB_ERROR = 99,                          // DB System or Query Error

            // platform error use 100~999
            // platform user
            USER_ID_NOT_FOUND = 101,

            // push notification
            PUSH_TOKEN_REGISTRATION_FAIL = 801,

            // billing
            BILLING_TOKEN_INVALIDE = 901,
            
            // system error
            System_Unknown_Operation = 1900, // STRING_MSG_ERROR_SYSTEM_UNKNOWN    유효하지 않은 요청입니다. 
            System_ItemBase_NOT_FOUND = 1901, // STRING_MSG_ERROR_ITEM_BASE    아이템 시스템 정보를 찾을 수 없습니다. (시스템 오류)
            SYSTEM_PARAM_ERROR = 1902, // STRING_MSG_ERROR_PARAM    요청을 위한 인자값이 부족합니다.

            REDIS_COMMAND_FAIL = 1950, // STRING_MSG_ERROR_REDIS    랭킹 서버가 응답하지 않습니다. (시스템 오류)
            System_Exception = 1999, // STRING_MSG_ERROR_SYSTEM_EXCEPTION    시스템 에러입니다. 개발팀에 문의해 주세요.

            //coupon
            COUPONCODE_REGIST_ALREADY = 10500, //이미 생성된 쿠폰코드
            COUPONCODE_INCORRECT = 10501, //잘못 된 쿠폰 번호
            COUPONCODE_REWARD_ALREADY = 10502, //이미 보상을 받은 쿠폰
            COUPONCODE_NOT_REWARD = 10503, //보상 받지 못하고 등록된 쿠폰
            COUPONCODE_NOT_EXIST_REWARDS = 10504, //쿠폰 보상이 등록 되있지 않은 쿠폰
        };
    }
}
