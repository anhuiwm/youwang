using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mSeed.Common
{
    public enum eServiceInfoType
    {
        /// <summary>
        ///  登录
        /// </summary>
        EPlatformType_Guest_Editer = 100,
        EPlatformType_Guest = 101,
        EPlatformType_Google = 102,
        EPlatformType_Facebook = 103,


        //====================== drop out of the game
        EPlatformType_DropAccount = -1,
        //====================== for Snail & Debug
        EPlatformType_UnityEditer = 0,
        EPlatformType_SnailSDK = 1,
        //======================
        EPlatformType_UC = 201,	// UC
        EPlatformType_360 = 202,	// 360
        EPlatformType_Baidu = 203,	// 百度
        EPlatformType_Xiaomi = 204,	// 小米
        EPlatformType_Oppo = 205,	// oppo
        EPlatformType_Vivo = 206,	// VIVO
        EPlatformType_Huawei = 207,	// 华为
        EPlatformType_Lenovo = 208,	// 联想
        EPlatformType_Gionee = 209,	// 金立
        EPlatformType_Coolpad = 210,	// 酷派dksy 
        EPlatformType_Meizu = 211,	// 魅族
        EPlatformType_Le = 212,	// " 乐视(러쓰) "
        EPlatformType_Tencent = 213,	// 텐센트
        //======================
        EPlatformType_TW_Guest_Editor = 300,
        EPlatformType_TW_Guest = 301,
        EPlatformType_TW_Google = 302,
        EPlatformType_TW_Facebook = 303,
        EPlatformType_TW_3rd = 304,

        //====================== for mfun  publishing(需要接入)
        EPlatformType_mfun_aosGuest = 501,
        EPlatformType_mfun_Google = 502,
        EPlatformType_mfun_aosFacebook = 503,
        EPlatformType_mfun_iosGuest = 504,
        EPlatformType_mfun_iosFacebook = 505,
        EPlatformType_KT_CLOUD_BOT = 10000,


        /// <summary>
        /// 支付
        /// </summary>
        ///
        /*//先前
        UnityDebug = 1000,
        iOS_Appstore = 2000,
        iOS_JailBreak = 3000,
        Android_3rdParty = 4000,

        Kr_aOS_Google = 11000,
        Kr_iOS_Appstore = 12000,
        Kr_aOS_OneStore = 13000,
        Apple_APNS = 10100,
        GoogleFCM_ToAndroid = 10200,
        GoogleFCM_ToiOS = 10300,

        //=================== for Global publishing
        Global_aOS_Google = 21000,
        Global_iOS_Appstore = 22000,

        //=================== for wannaplay mfun publishing(还没有完成需要现在自己做的)
        mfun_aOS_Paypal = 20100,
        mfun_aOS_Mycard = 22200,
        mfun_aOS_MOL = 20300,
        mfun_aOS_Google = 20400,
        mfun_aOS_facebook = 20500,
        mfun_iOS_Paypal = 21100,
        mfun_iOS_Mycard = 21200,
        mfun_iOS_MOL = 21300,
        mfun_iOS_Appstore = 21400,
        mfun_iOS_facebook = 21500,

        //=================== for Taiwan aicombo publishing
        Tw_iOS_Appstore = 31000,
        Tw_aOS_GooglePlaystore = 32000,
        Tw_MOL = 33001,
        Tw_mycard_TW = 33002,
        Tw_mycard_HK = 33003,
        Tw_gash_TW = 33004,
        Tw_gash_HK = 33005,
        Tw_TWM = 33006,
        Tw_aicombo = 33007,
        Tw_pepay = 33008,
         */



         
        None = 0,
        UnityDebug = 1000,
        iOS_Appstore = 2000,
        iOS_JailBreak = 3000,
        Android_3rdParty = 4000,
        //=================== for Korea publishing
        Kr_aOS_Google = 11000,
        Kr_iOS_Appstore = 12000,
        Kr_aOS_OneStore = 13000,
        Apple_APNS = 10100,
        GoogleFCM_ToAndroid = 10200,
        GoogleFCM_ToiOS = 10300,
        //=================== for Global publishing
        Global_aOS_Google = 21000,
        //new(需要实现)
        Global_aOS_PayPal = 21001,
        Global_aOS_MOL = 21002,
        Global_aOS_MyCard = 21003,
        //end

        Global_iOS_Appstore = 22000,
        //new(需要实现)
        Global_iOS_PayPal = 22001,
        Global_iOS_MOL = 22002,
        Global_iOS_MyCard = 22003,
        //end
        //=================== for Taiwan aicombo publishing
        Tw_iOS_Appstore = 31000,
        Tw_aOS_GooglePlaystore = 32000,
        Tw_MOL = 33001,
        Tw_mycard_TW = 33002,
        Tw_mycard_HK = 33003,
        Tw_gash_TW = 33004,
        Tw_gash_HK = 33005,
        Tw_TWM = 33006,
        Tw_aicombo = 33007,
        Tw_pepay = 33008,

        //=================== for wannaplay mfun publishing(新马泰还没有完成需要现在自己做的)
        mfun_aOS_Google = 40000,
        mfun_aOS_Paypal = 40001,   
        mfun_aOS_MOL = 40002,
        mfun_aOS_Mycard = 40003,

        mfun_iOS_Appstore = 41000,
        mfun_iOS_Paypal = 41001,          
        mfun_iOS_MOL = 41002,
        mfun_iOS_Mycard = 41003,

        //=================== for wannaplay yuenan publishing(越南还没有完成需要现在自己做的)
        yuenan_aOS_Google = 50000,
        yuenan_aOS_Mobile = 50001,

        yuenan_iOS_Appstore = 51000,
        yuenan_iOS_Mobile = 51001,
 
        



    }

    public enum ePlatformType
    {
        //====================== drop out of the game
        EPlatformType_DropAccount = -1,
        //====================== for Snail & Debug
        EPlatformType_UnityEditer = 0,
        EPlatformType_SnailSDK = 1,
        //====================== for mSeed publishing
        EPlatformType_Guest_Editer = 100,       /// 游客编辑器登录
        EPlatformType_Guest = 101,              /// 游客登录
        EPlatformType_Google = 102,         //全球谷歌（谷歌没有ios）
        EPlatformType_Facebook = 103,

        //new
        EPlatformType_iosFacebook = 113,//全球ios facebook
        //end
        //======================
        EPlatformType_UC = 201,	// UC
        EPlatformType_360 = 202,	// 360
        EPlatformType_Baidu = 203,	// 百度
        EPlatformType_Xiaomi = 204,	// 小米
        EPlatformType_Oppo = 205,	// oppo
        EPlatformType_Vivo = 206,	// VIVO
        EPlatformType_Huawei = 207,	// 华为
        EPlatformType_Lenovo = 208,	// 联想
        EPlatformType_Gionee = 209,	// 金立
        EPlatformType_Coolpad = 210,	// 酷派dksy 
        EPlatformType_Meizu = 211,	// 魅族
        EPlatformType_Le = 212,	// " 乐视(러쓰) "
        EPlatformType_Tencent = 213,	// 텐센트
        //======================
        EPlatformType_TW_Guest_Editor = 300,
        EPlatformType_TW_Guest = 301,
        EPlatformType_TW_Google = 302,
        EPlatformType_TW_Facebook = 303,
        EPlatformType_TW_3rd = 304,

        //====================== for mfun  publishing(新马泰需要接入)
        EPlatformType_mfun_aosGoogle = 502,//新马泰谷歌
        EPlatformType_mfun_aosFacebook = 503,  //新马泰安卓facebook   
        EPlatformType_mfun_iosFacebook = 513,//新马泰苹果facebook  

        //====================== for yuenan publishing(越南需要接入)
        EPlatformType_yuenan_aosGoogle = 602,//越南谷歌（谷歌没有ios）
        EPlatformType_yuenan_aosFacebook = 603,//越南安卓facebook
        EPlatformType_yuenan_iosFacebook = 613,//越南苹果facebook


        EPlatformType_KT_CLOUD_BOT = 10000,
    };

    public enum ePushType
    {
        AppleAPNS = 100,
        GoogleFCM_ToAndroid = 200,
        GoogleFCM_ToiOS = 300,
    }

    public enum eBillingType
    {
        None = 0,
        UnityDebug = 1000,
        iOS_Appstore = 2000,
        iOS_JailBreak = 3000,
        Android_3rdParty = 4000,
        //=================== for Korea publishing
        Kr_aOS_Google = 11000,
        Kr_iOS_Appstore = 12000,
        Kr_aOS_OneStore = 13000,
        //=================== for Global publishing
        Global_aOS_Google = 21000,
        //new(需要实现)
        Global_aOS_PayPal = 21001,
        Global_aOS_MOL = 21002,
        Global_aOS_MyCard = 21003,
        Global_aOS_MOLPin = 21004,//mol点卡
        //end

        Global_iOS_Appstore = 22000,
        //new(需要实现)
        Global_iOS_PayPal = 22001,
        Global_iOS_MOL = 22002,
        Global_iOS_MyCard = 22003,
        Global_iOS_MOLPin = 22004,//mol点卡
        //end
        //=================== for Taiwan aicombo publishing
        Tw_iOS_Appstore = 31000,
        Tw_aOS_GooglePlaystore = 32000,
        Tw_MOL = 33001,
        Tw_mycard_TW = 33002,
        Tw_mycard_HK = 33003,
        Tw_gash_TW = 33004,
        Tw_gash_HK = 33005,
        Tw_TWM = 33006,
        Tw_aicombo = 33007,
        Tw_pepay = 33008,

        //=================== for wannaplay mfun publishing(新马泰还没有完成需要现在自己做的)
        mfun_aOS_Google = 40000,
        mfun_aOS_Paypal = 40001,
        mfun_aOS_MOL = 40002,
        mfun_aOS_Mycard = 40003,
        mfun_aOS_MOLPin = 40004,//mol点卡

        mfun_iOS_Appstore = 41000,
        mfun_iOS_Paypal = 41001,
        mfun_iOS_MOL = 41002,
        mfun_iOS_Mycard = 41003,
        mfun_iOS_MOLPin = 41004,//mol点卡

        //=================== for wannaplay yuenan publishing(越南还没有完成需要现在自己做的)
        yuenan_aOS_Google = 50000,
        yuenan_aOS_Mobile = 50001,

        yuenan_iOS_Appstore = 51000,
        yuenan_iOS_Mobile = 51001,

    }

    public enum ePushStatus
    {
        Stop = 0,
        Unconfirmed = 1,
        Confirm = 2,
        Progress = 3,
        Finish = 4,
    }
}
