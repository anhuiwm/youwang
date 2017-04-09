using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSoul.DataManager
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
            POST_DATA_ERROR = 1,
            DUPLICATE_USERNAME = 2,
            BEFORE_CREATE_CHARACTER = 3,
            FAIL_EQUIP_WEAPONARMOR = 4,
            FAIL_DEFAULTCHARACTER = 5,
            FAIL_EQUIP_SOUL = 6,
            SELLITEM_NOT_FOUND = 7,
            DUPLICATE_PLAYING = 8,
            DUPLICATE_CHARACTER_CLASS = 9,

            NO_KEY_ENTERDUNGEON = 10,
            NOT_ENOUGH_KEY = 10,

            NO_EQUIP_ENCHANT_NEEDGRADEUP = 11,
            NOMONEY_REQUIREENCHANT = 12,
            NO_EQUIP_ENCHANT_MAX = 13,
            NO_EQUIP_GRADEUP = 14,
            NOGOLD_REQUIREGRADEUP = 15,
            NO_EQUIP_GRADEUP_MAX = 16,
            NO_SOULSYNTHESIZE_LEVELUP = 17,
            FULL_SOULUPGRADE = 18,
            NO_SOULUPGRADE_NEEDENCHANT = 19,
            NOT_ENOUGH_SOULUPGRADE_COST = 20,
            CANT_SOULUPGRADE_MAX = 21,
            NOT_ENOUGH_SOULUPGRADE_GUAGE = 22,
            NO_SOULREGRANT_LOWGRADE = 23,
            NOT_ENOUGH_SOULREGRANT_COST = 24,
            CANT_SOULREGRANT_GRADE_MATERIAL = 25,
            CANT_SOULREGRANT_GRADE_SOUL = 26,
            FAIL_SOULUPGRADE_DATA = 27,
            NOTEXIST_SOULSYNTHESIZE_INFO = 28,
            NOTEXIST_NEED_UPGRADE_GUAGE_INFO = 29,
            NOTEXIST_ACCOUNT_GOLD = 30,
            NOTEXIST_UPGRADE_SERVER_INFO = 31,
            NOTEXIST_UPGRADE_NEED_DATA = 32,
            NOTEXIST_GACHA_DATA = 33,
            NO_GACHA_REQUIRE_LEVEL = 34,            

            NOT_ENOUGH_HONOR = 35,
            NOT_ENOUGH_CASH = 36,
            NOT_ENOUGH_GOLD = 37,

            FAIL_RATE_TABLE_LOADING = 38,
            CHARACTER_NOT_FOUND = 39,

            ACCOUNT_ID_NOT_FOUND = 40,
            FAIL_BONUS_TABLE = 41,
            FAIL_SELECT_TRYSALERATE_TABLE = 42,
            CANT_GIVE_MAIL = 43,
            NOTEXIST_SELL_SOUL_INFO = 44,
            
            FULL_RECEIVERECOMMEND = 45,
            FRIEND_EXCESS_DELETE_DAYCOUNT = 46,
            FRIEND_NO_ACCOUNT = 47,
            FRIEND_EXCESS_WAITING_JOINFRIENDLIST = 48,
            FRIEND_DUPLICATE_JOINFRIENDLIST = 49,
            FRIEND_NOTEXIST_ACCEPTFRIEND_INFO = 50,
            FRIEND_NOTEXIST_REJECTFRIEND_INFO = 51,
            
            NO_BUYCHARACTER_LEVEL = 52,
            NOT_ENOUGH_MONEY = 53,
            DUPLICATE_CLASS = 54,
            FAIL_GRANT_ACHIEVEMENT = 55,
            EQUIPITEM_CHOOSE_ERROR = 56,
            NOTEXIST_ACHIEVEMENT_INFO = 57,
            NOTEXIST_RECOMMENDINFO = 58,
            FULL_FRIENDLIST = 59,
            CANT_DELETEFRIEND_ONEDAY = 60,
            ALREADY_FRIEND_ACCOUNT = 61,

            BOSSRAID_ID_NOT_FOUND = 62,
            BOSSRAID_NPC_INFO_NOT_FOUND = 63,
            BOSSRAID_CREATE_RATE_CHECK_FAIL = 64,
            BOSSRAID_INVALID_STAGE = 65,
            BOSSRAID_ALREADY_OPEN = 66,
            BOSSRAID_NOT_ENOUGH_COST = 67,
            BOSSRAID_HAS_BEEN_CLOSED = 68,
            BOSSRAID_ALREADY_CLEARED = 69,
            BOSSRAID_CANT_JOIN_IS_NOT_PUBLIC_OR_NOT_IN_FRIEND_LIST = 70,
            BOSSRAID_REWARD_ALREADY_GIVE = 71,
            BOSSRAID_REWARD_CREATE_FAIL = 73,

            FULL_FRIENDLIST_TARGET = 80,
            NO_GUILD_USER = 81,
            NO_BUY_ITEM = 82,
            NO_RANDOMBOX_SECOND_INFO = 83,
            EXCESS_BUY_SLOT = 84,
            NOT_ENOUGH_RECOMMENDINFO = 85,
            DUPLICATE_REQUEST_REWARD = 86,

            NOT_ENOUGH_TICKET = 87,
            MAX_ITEM_INVEN_ADDSLOT = 88,
            MAX_SOUL_INVEN_ADDSLOT = 89,
            NO_SOULILLUSTRATEDREWARD_TARGET = 90,
            NOEXIST_MAILINFO = 91,
            REVIVE_BUYCHARACTER = 92,
            UNAVAILABLE_COUPONCODE = 93,
            NO_ALLUSED_COUPON = 94,
            BEFORE_SENDKEY_ONEDAY = 95,
            DB_STOREDPROCEDURE_ERROR = 97,
            FAIL_CREATE_ACCOUNT = 98,
            DB_ERROR = 99,

            CANT_SENDKEY_ONEDAY = 201,
            NO_CHARACTER_EXIST_ACCOUNT = 203,
            ALREADY_GIVE_COUPON = 204,
            EXPIRED_LIMIT_PRODUCT = 205,
            ALREADY_SELL_PRODUCT = 206,
            ACCOUNT_BLOCK = 207,
            CANT_REPURCHASE_PRODUCT = 208,

            FULL_INVEN_BOSSRAIDREWARD = 209,
            FAIL_GET_SHARDINGDB = 210,
            FAIL_GET_LOGDB = 211,
            DUPLICATE_PAYMENT = 212,
            ONEACCOUNT_PREREGISTER_COUPON = 213,
            WRONG_STAGEINFO = 214,
            ALREADY_DELETED_ITEM = 215,
            CANT_DISASSEMBLE_ZEROTIRE = 216,
            CHOOSE_SOULMATERIAL_DELETEDSOUL = 217,
            CANT_CHOOSE_SOULMATERIAL = 218,
            NOTEXIST_UNEQUIPITEMINFO = 219,
            CANT_USE_NICKNAME = 220,
            ALREADY_EXIST_NICKNAME = 221,

            // Guild
            GUILD_NOEXIST_INFO = 222,
            GUILD_CANT_CHANGE_HALFDAY = 223,
            GUILD_ALREADY_JOIN = 224,
            GUILD_OTHER_JOIN = 225,
            GUILD_CANT_REJOIN_HALFDAY = 226,
            GUILD_CANT_EXILE_REJOIN_HALFDAY = 227,
            GUILD_CANT_JOIN_FULL_PRIVATE = 228,
            GUILD_CANT_JOIN_FULL_PUBLIC = 229,
            GUILD_REQUESTJOIN_CHANGE_PUBLIC_TO_PRIVATE = 230,
            GUILD_JOINED_CHANGE_PRIVATE_TO_PUBLIC = 231,
            SEARCH_GUILDRANKING_NOWEEKRANKING = 232,
            CANT_CHANGE_NOTICE_INTRODUCE_HALFDAY = 233,
            CANT_BUYSHOP_NO_GUILDMASTER = 234,
            NO_SEARCH_CREATED_GUILD_ONEHOUR = 235,
            GUILD_RANK_SEARCH_NOEXIST_INFO = 238,

            //pekadd
            NO_GUILD_MARK = 1801,
            ALREADY_EXIST_Guild = 1802,
            NOT_ENOUGH_CONTIRIBUTION_POINT = 1803,
            GUILD_JOIN_CANCEL = 1804,
            GUILD_CANT_CREATE_ONEHOUR = 1805,
            ALREADY_GUILD_DONATION = 1806,
            ONLY_GUILD_MASTER = 1807,
            GUILD_DONATION_LEVELNOT_ENOUGH = 1808, // 길드 기부 및 출석이 10레벨 미만일 경우

            BUY_COSTUMEITEM_ONECOUNT = 240,
            CANT_BUY_COSTUMEITEM_DIFFERENT_CLASS = 241,
            CANT_GIVE_DIFFERENT_COSTUME_EQUIPED_CLASS = 242,

            NOITEM_ITEMREGRANT = 243,
            NOMONEY_REQUIRE_ITEMREGRANT = 244,

            NO_SUMMONSTONE_GACHA = 245,
            SYNTHESIZE_UPPER_SOULMATERIAL = 246,
            NOT_ENOUGH_MEDAL = 247,
            ALREADY_GIVE_REWARD = 248,
            ONEDAY_FREE_GACHA = 250,
            CANT_BUY_CHEERSHOP_MAXCOUNT = 260,
            CANT_BUY_CHEERSHOP_REQUIRE_LEVEL = 261,
            NOT_ENOUGH_FRIEND_POINT = 262,
            NOT_ENOUGH_REGRANTSTONE = 263,
            
            // DayDungeon
            DAYDUNGEON_WRONG_REWARD_ITEMINFO = 264,
            DAYDUNGEON_CLOSED = 301,
            DAYDUNGEON_MAX_SUCCESS_COUNT = 302,
            
            // GoldExpedition
            GOLDEXPEDITION_NOT_ENOUGH_CHARACTER_HP = 303,
            GOLDEXPEDITION_REQUIRE_LEVEL = 304,
            GOLDEXPEDITION_CANT_BUY_POTION = 305,
            GOLDEXPEDITION_ALREADY_BUY_POTION = 306,
            GOLDEXPEDITION_EXCESS_RESET_COUNT = 307,
            GOLDEXPEDITION_FAIL_STAGE_PLAYSTART = 308,
            GOLDEXPEDITION_NOT_ENOUGH_GEPOINT = 309,
            GOLDEXPEDITION_CURHP_EQUAL_MAXHP_USE_HPPOTION = 310,
            GOLDEXPEDITION_CANT_USE_POTION_ALLCLEAR = 311,

            // Guild Entrust
            GUILD_ALREADY_CHANGE_CREATEAID = 312,
            GUILD_ENTRUST_ONEHOUR = 313,
            GUILD_ENTRUST_CANCEL = 314,
            GUILD_EXCESS_SHOPRESET_COUNT = 315,
            GUILD_NOT_ENOUGH_GUILDLV = 316,
            GUILD_ENTRUST_ALREADY = 317,

            ALREADY_CREATED_SOUL = 320,
            CANT_CREATED_NOT_ENOUGH_SOULPIECE = 321,
            CANT_CREATED_PASSIVE_SOUL_NOSUMMONSTONE = 322,
            NOTEXIST_TARGET_SOULINFO = 323,
            FULL_SOUL_ENCHANT = 324,
            NOT_ENOUGH_EXP_NEED_PASSIVE_SOUL_ENCHANT = 325,
            NO_EXCESS_CHARACTERLV_SOUL_LEVELUP = 326,
            LEVELUP_SOUL_AFTER_LEVELPASS = 327,
            NOT_ENOUGH_LEVELUP_GOLD = 328,
            CANT_TRY_LEVELPASS_REQUIRE_CHARACTERLV = 329,
            NO_LEVELPASS_PHASE = 330,
            NOT_ENOUGH_MATERIAL_NEED_SOUL_LEVELPASS = 331,
            NOT_ENOUGH_MATERIAL_NEED_SOUL_UPGRADE = 332,
            NO_EQUIP_SOULINFO = 333,
            FAIL_SOUL_CHANGE_BUFF = 334,
            FULL_PASSIVE_SOUL_INVEN = 335,

            PASSIVE_SOUL_CREATE_RUBY_NOT_ENOUGH = 336,
            PASSIVE_SOUL_LIMIIT_OVER = 337,
            MISSION_TRY_COUNT_MAX = 338,
            CANT_MISSION_RESET_NO_MISSIONINFO = 339,
            MISSION_RESET_ONEDAY = 340,
            CANT_USE_SWEEPTICKET_NO_RANK_THREE = 341,
            NOT_ENOUGH_RANK_SCORE_REWARD = 342,
            DUPLICATE_REWARD_MISSION = 343,

            GOLDEXPEDITION_GUILD_MERCENARY_ONLYONE = 401,
            GOLDEXPEDITION_GUILD_MERCENARY_CALL_HALFHOUR = 402,
            GOLDEXPEDITION_NOT_ENOUGH_HP = 403,

            //add pek
            COUPONCODE_INCORRECT = 501, //잘못 된 쿠폰 번호
            COUPONCODE_REWARD_ALREADY = 502, //이미 보상을 받은 쿠폰
            COUPONCODE_NOT_REWARD = 503, //보상 받지 못하고 등록된 쿠폰 coupon_sever to game_server errorcode

            OVER_MAX_GOLD = 900,
            OVER_MAX_EXP = 901,

            // ADD by manstar
            ACCOUNT_GLOBAL_ID_NOT_FOUND = 1000, // STRING_MSG_ACCOUNT_ERROR8    계정을 찾을 수 없습니다.
            ACCOUNT_ID_ALREAD_CREATED = 1001,           // Account Error // STRING_MSG_ACCOUNT_ERROR1    이미 생성된 계정입니다.
            ACCOUNT_ID_CRETE_FAIL = 1002, // STRING_MSG_ACCOUNT_ERROR6    계정 생성에 실패했습니다. (시스템 오류)

            ACCOUNT_ID_GLOBAL_REGIST_ALREADY = 1003, // STRING_MSG_ACCOUNT_ERROR1    이미 생성된 계정입니다.
            ACCOUNT_ID_GLOBAL_REGIST_FAIL = 1004, // STRING_MSG_ACCOUNT_ERROR6    계정 생성에 실패했습니다. (시스템 오류)

            ACCOUNT_ID_LOGIN_TO_PLAY = 1011, // STRING_MSG_LOGIN_ALREADYCONNECT    계정과 캐릭터가 존재합니다. 재로그인이 필요합니다.
            ACCOUNT_ID_ALREAD_CREATED_BUT_NEED_CHARACTER_CREATE = 1012, // STRING_MSG_ACCOUNT_ERROR9    계정이 생성 되어 있습니다. 캐릭터를 생성해주세요.
            ACCOUNT_NICKNAME_DUPLICATED = 1013, // STRING_MSG_ACCOUNT_ERROR1    이미 존재하는 이름입니다. 다른 이름으로 생성해 주세요
            ACCOUNT_LENGTH_OVER = 1014, 

            CHARACTER_ID_GLOBAL_REGIST_ALREADY = 1021, // STRING_MSG_CHARCREATE_ERROR2    이미 생성된 캐릭터입니다.
            CHARACTER_ID_GLOBAL_REGIST_FAIL = 1022, // STRING_MSG_CHARCREATE_ERROR1    캐릭터 생성에 실패했습니다. (시스템 오류)

            CHARACTER_ID_ALREAD_CREATED = 1023, // STRING_MSG_CHARCREATE_ERROR2    이미 생성된 캐릭터입니다.
            CHARACTER_ID_CRETE_FAIL = 1024, // STRING_MSG_CHARCREATE_ERROR1    캐릭터 생성에 실패했습니다. (시스템 오류)
            CHARACTER_CLASS_INVALIDE = 1025, // STRING_MSG_CHARCREATE_ERROR2    캐릭터 직업이 맞지 않습니다.
            CHARACTER_ID_DUPLICATE_IN_GROUP = 1026, // STRING_MSG_CHARCREATE_ERROR2    중복 캐릭터를 등록할 수 없습니다. (캐릭터 PvE 그룹 등록시 발생)
            CHARACTER_BUY_SOUL_COUNT_NOT_ENOUGH = 1040, // 캐릭터 구입을 위한 혼 갯수가 부족합니다.

            ACCOUNT_ALREADY_LOGIN = 1050, // STRING_MSG_LOGIN_ALREADYCONNECT    현재 접속 중인 계정이어서 로그인 할 수 없습니다. 재로그인이 필요합니다.
            CHAT_IGNORE_CAN_NOT_SELF = 1051,    // 자신을 채팅 차단 할 수 없습니다.

            ACCOUNT_ID_NOT_MATCHED = 1060,              // 계정 전환할려는 AID와 맞지 않습니다.
            ACCOUNT_ID_ALREADY_LINKED = 1061,           // 이미 계정 등록된 아이디 입니다.

            ACCOUNT_PLATFORM_INFO_NOT_FOUND = 1090,     // 계정 인증 정보를 찾을수 없습니다.
            ACCOUNT_PLATFORM_INVALIDE = 1091,           // 정상적이지 않은 인증 플렛폼 입니다.
            // Item error
            ITEM_ID_NOT_FOUND = 1101, // STRING_MSG_ITEM_ERROR_02    해당 아이템을 찾을 수 없습니다.                 
            NOT_ENOUGH_USE_ITEM = 1102, // STRING_MSG_ITEM_ERROR_01    사용할 아이템이 충분하지 않습니다.
            DELETEITEM_CHOOSE_ERROR = 1103, // STRING_MSG_ITEM_ERROR_03    삭제할 아이템이 존재하지 않습니다.
            ITEM_ENCHANCE_LEVEL_MAX = 1105, // STRING_MSG_ITEM_ERROR_04    더 이상 강화할 수 없습니다.
            ITEM_ENCHANCE_TYPE_INVALIDE = 1106, // STRING_MSG_ITEM_ERROR_05    강화할 수 없는 아이템입니다.
            ITEM_ENCHANCE_DB_NOT_FOUND = 1107, // STRING_MSG_ITEM_ERROR_06    강화 정보를 찾을 수 없습니다.
            ITEM_SELL_FAIL_NEED_IGNORE = 1108, // STRING_MSG_ITEM_ERROR_07    판매를 실패했습니다. (아이템 판매가격이 0 입니다.)
            ITEM_SYSTEM_ID_NOT_FOUND = 1109, // STRING_MSG_ITEM_ERROR_02    해당 아이템 정보를 찾을 수 없습니다. (시스템 오류)

            ITEM_EQUIP_INFO_NOT_FOUND = 1130, // STRING_MSG_ITEM_ERROR_08    장착할 수 없는 아이템입니다.
            ITEM_EQUIP_NEED_CID = 1131, // STRING_MSG_ITEM_ERROR_09    장착할 수 없는 캐릭터입니다.
            ITEM_EQUIP_CLASS_TYPE_INVALIDE = 1132, // STRING_MSG_ITEM_ERROR_09    장착할 수 없는 캐릭터입니다.
            ITEM_TYPE_INVALIDE = 1133, // STRING_MSG_ITEM_ERROR_02    아이템 타입이 맞지 않습니다.
            ITEM_EQUIP_NOT_ENOUGH_LEVEL = 1134, // STRING_MSG_ITEM_EQUIP_ERROR2    레벨이 낮아 착용할 수 없습니다.
            ITEM_EQUIP_CAN_NOT_USE = 1135,      // 더 이상 장착아이템을 강화 할 수 없습니다. (강화 / 진화 / 판금 최고 레벨 아이템시 발생)

            ITEM_EVOLUTION_TYPE_INVALIDE = 1151, // STRING_MSG_ITEM_ERROR_10    진화할 수 없는 아이템입니다.
            ITEM_EVOLUTION_LEVEL_NOT_ENOUGH = 1152, // STRING_MSG_ITEM_ERROR_11    레벨이 낮아 진화할 수 없습니다.
            ITEM_EVOLUTION_GRADE_MAX = 1153, // STRING_MSG_ITEM_ERROR_12    더 이상 진화할 수 없습니다.
            ITEM_EVOLUTION_DB_NOT_FOUND = 1154, // STRING_MSG_ITEM_ERROR_13    진화 정보를 찾을 수 없습니다.
            ITEM_EVOLUTION_GRADE_NOT_ENOUGH = 1155, // STRING_MSG_ITEM_ERROR_14    등급이 부족해 진화를 할 수 없습니다.

            ITEM_DISASSAEMBLE_TYPE_INVALIDE = 1161, // STRING_MSG_ITEM_ERROR_15    분해 정보를 찾을 수 없습니다.

            ITEM_OPTION_MAX = 1170, // STRING_MSG_ITEM_ERROR_16    옵션을 추가 할 수 없습니다.
            ITEM_TUNING_TYPE_INVALIDE = 1171, // STRING_MSG_ITEM_ERROR_17    개조 정보를 찾을 수 없습니다.
            ITEM_TUNING_NOT_ENOUGH_GRADE = 1172, // ?? (개조 기능이 삭제되었음)

            ITEM_RIFINING_LEVEL_NOT_ENOUGH = 1173, // STRING_MSG_ITEM_ERROR_18    레벨이 낮아 정련을 할 수 없습니다.
            ITEM_RIFINING_GRADE_NOT_ENOUGH = 1174, // STRING_MSG_ITEM_ERROR_19    대상 아이템의 진화 등급이 부족해 정련을 할 수 없습니다.
            ITEM_RIFINING_TYPE_INVALIDE = 1175, // STRING_MSG_ITEM_ERROR_20    정련 정보를 찾을 수 없습니다.
            ITEM_RIFINING_NEED_SAME_GRADE_ITEM = 1176, // STRING_MSG_ITEM_ERROR_19    대상과 재료 아이템의 진화 등급이 동일해야 합니다..
            ITEM_RIFINING_LEVEL_MAX = 1177, // 더 이상 정련할 수 없습니다.

            ITEM_OPTION_ID_NOT_FOUND = 1180, // STRING_MSG_ITEM_ERROR_21    옵션 아이디를 찾을 수 없습니다.
            ITEM_OPTION_TYPE_INVALIDE = 1181, // STRING_MSG_ITEM_ERROR_22    옵션 타입이 맞지 않습니다.

            SUCCESS_BUT_RATE_CHECK_FAIL = 1199, // STRING_MSG_ITEM_ERROR_23    강화에 실패했습니다.

            ITEM_CREATE_FAIL = 1200, // STRING_MSG_ITEM_ERROR_24    아이템 생성에 실패했습니다.
            USE_ITEM_TYPE_INVALIDE = 1201, // STRING_MSG_ITEM_ERROR_01    사용할 수 없는 아이템입니다.
            ITEM_INVENTORY_OVER = 1210,   // 가방이 가득 찼습니다. 가방을 정리해 주세요.

            ITEM_ULTIMATE_ALREADY_EXIST = 1230,   // 이미 봉인 해제된 신의무기 입니다.
            ITEM_ULTIMATE_LEVEL_NOT_ENOUGH = 1231,   // 신의 무기 레벨이 부족합니다.
            ITEM_ULTIMATE_ENHANCE_MAX = 1233,   // 신의 무기를 더이상 강화 할 수 없습니다.

            ITEM_ULTIMATE_ORB_SLOT_INVALIDE = 1240,   // 장착할 수 없는 슬롯입니다.
            ITEM_ULTIMATE_ORB_ALREADY_EQUIP = 1241,   // 이미 장착된 오브입니다.
            ITEM_ULTIMATE_ORB_SLOT_ALREADY_EQUIPED = 1242,   // 오브가 장착되어 있는 슬롯입니다.

            ITEM_INFO_TYPE_INVALIDE = 1290, // STRING_MSG_ITEM_ERROR_25    아이템 정보를 찾을 수 없습니다.
            ITEM_MAKE_TO_INVEN = 1299,    // 서버내부에서만 씁니다.

            // drop group error
            PICK_DROP_GROUP_FAIL = 1301, // STRING_MSG_ITEM_ERROR_26    아이템 드랍 정보를 찾지 못했습니다. 아이템 획득에 실패했습니다. (시스템 오류)
            PICK_DROP_ITEM_EMPTY = 1302, // STRING_MSG_ITEM_ERROR_26    드랍 아이템이 없습니다. 아이템 획득에 실패했습니다. (시스템 오류)

            // Shop error
            SHOP_ID_NOT_FOUND = 1401, // STRING_MSG_SHOP_ERROR_01    상품 ID를 찾을수 엇습니다. 아이템 구입에 실패했습니다.
            SHOP_ITEM_BUY_TIME_INVALIDE = 1402, // STRING_MSG_SHOP_ERROR_02    상품 판매시간이 아닙니다. 아이템을 구입할 수 없는 시간입니다.
            SHOP_ITEM_BUY_PRICE_INFO_NOT_FOUND = 1403, // STRING_MSG_SHOP_ERROR_01    상품 판매 가격정보를 찾을수 없습니다. 아이템 구입에 실패했습니다. (시스템 오류)
            SHOP_BILLING_ALREADY_COMPLETE = 1450, // STRING_MSG_SHOP_ERROR_01    이미 결제가 완료되었습니다. 결제 처리를 종료합니다.
            SHOP_BILLING_ID_NOT_FOUND = 1451, // STRING_MSG_SHOP_ERROR_03    결제 정보를 찾을 수 없습니다.
            SHOP_BUY_NO_ITEM = 1452, // 아이템 구매 수량이 0입니다. 구매 수량을 입력해주세요.
            SHOP_BUY_MAX_COUNT_OVER = 1453,     // 아이템 최대 구매 수량을 초과해서 구매할 수 없습니다.

            SHOP_BILLING_SUBSCRIPTION_LEFT = 1454,    // 기간제 상품 기간이 남아있습니다.
            SHOP_BILLING_ID_CREATE_FAIL = 1460,    // 결제 ID 생성에 실패했습니다.

            SHOP_UNKNOWN_BUY_TYPE = 1471, // STRING_MSG_SHOP_ERROR_01    상품 구매 정보를 확인 할수 업습니다. 아이템 구입에 실패했습니다.
            SHOP_NOT_ENOUGH_COMBAT_POINT = 1472, // STRING_MSG_1VS1REAL_SHOP_NEEDMOREPOIINT    결투 포인트가 부족합니다.
            SHOP_NOT_ENOUGH_PARTY_DUNGEON_POINT = 1473, // STRING_MSG_3PVE_SHOP_NEEDMOREPOIINT    협력 포인트가 부족합니다.
            SHOP_NOT_ENOUGH_OVERLORD_POINT = 1474, // STRING_MSG_RANKING_SHOP_NEEDMOREPOINT    랭킹 포인트가 부족합니다. 
            SHOP_NOT_ENOUGH_GE_POINT = 1475,    // 황금 원정단 포인트가 부족합니다.
            SHOP_NOT_ENOUGH_BLACKMARKET_POINT = 1476,       // 암시장 포인트가 부족합니다.
            SHOP_SOUL_SEQ_EMPTY_OR_INVALIDE = 1477,         // 분해 혼조각 정보가 유효하지 않습니다.

            SHOP_FREE_GACHA_LEFT_TIME_OVER = 1481, // STRING_MSG_SHOP_ERROR_04    무료 뽑기를 할 수 없는 시간입니다.
            SHOP_FREE_GACHA_COUNT_OVER = 1482, // STRING_MSG_SHOP_ERROR_05    더 이상 무료 뽑기를 할 수 없습니다.
            SHOP_FREE_PREMIUM_GACHA_LEFT_TIME_OVER = 1483, // STRING_MSG_SHOP_ERROR_04   무료 프리미엄 뽑기를 할 수 없는 시간입니다.
            SHOP_FREE_GACHA_TYPE_INVALIDE = 1484,     // 무료 뽑기 타입이 유효하지 않습니다
            SHOP_BEST_GACHA_NOT_OPEN = 1490,    // 최고급 뽑기가 열려있지 않습니다.

            SHOP_TREASURE_BOX_TYPE_INVALIDE = 1491, // 보물뽑기 타입이 유효하지 않습니다.
            SHOP_TREASURE_BOX_CREATE_FAIL = 1492, // 보물뽑기 목록 생성에 실패했습니다.
            SHOP_TREASURE_BOX_NOT_ENOUGH_RUBY_BUY_COUNT = 1493, // 루비상자 구매 횟수가 부족합니다.

            SHOP_BILLING_PROGRESS_FAIL = 1498,    // 결제 진행 과정이 실패했습니다.
            SHOP_BILLING_ID_ALREADY_REGISTERED = 1499,    // 이미 등록된 결제 정보 입니다.

            FAIL_CREATE_BILLING_ID = 236,
            FAIL_CHANGE_BILLING_ID = 237,

            // Friend error
            FRIEND_SEARCH_NO_RESULTS = 1501, // STRING_MSG_FRIEND_009    해당 유저가 없습니다.
            FRIEND_SEND_GIFT_TIME_REMAIN = 1502, // STRING_MSG_FRIEND_ERROR_KEYTIME    아직 열쇠를 보낼 수 없습니다.

            NOT_ENOUGH_FRIENDLYPOINT = 1503, // ?? (친구 포인트가 뭔지....)     더이상 사용하지 않습니다. FaceBook 용도 였던듯.
            FRIEND_SEND_GIFT_LIST_EMPTY = 1510, // 이미 모든 친구들에게 열쇠를 보냈습니다. 내일 다시 보내주세요.

            // Trigger Error
            TRIGGER_IS_NOT_CLEAR = 1601, // STRING_MSG_EVENT_ERROR_01    완료 조건을 채우지 못했습니다.
            TRIGGER_ID_NOT_FOUND = 1602, // STRING_MSG_EVENT_ERROR_02    존재하지 않는 이벤트입니다.
            TRIGGER_REWARD_NOT_FOUND = 1603, // STRING_MSG_EVENT_ERROR_03    보상 획득에 실패했습니다.
            TRIGGER_REWARD_EMPTY = 1604, // STRING_MSG_EVENT_ERROR_03    보상 획득에 실패했습니다.
            TRIGGER_EVENT_TYPE_EMPTY = 1611, // STRING_MSG_EVENT_ERROR_03    현재 활성화 되지 않은 이벤트 타입 입니다.

            TRIGGER_EVENT_DAILY_REWARD_COUNT_OVER = 1621, // STRING_MSG_EVENT_ERROR_04    더 이상 보상을 받을 수 없습니다.
            TRIGGER_EVENT_DAILY_ADD_COUNT_OVER = 1622, // 더 이상 추가출석을 할 수 없습니다.

            TRIGGER_EVENT_FIRSTPAY_NOT_FOUND = 1631, // STRING_MSG_EVENT_ERROR_05    첫 구매 이벤트 정보를 찾을수 없습니다. (시스템 오류)
            TRIGGER_EVENT_FIRSTPAY_ALREADY_GIVE = 1632, // STRING_MSG_EVENT_ERROR_06    첫 구매가 필요합니다.

            // VIP Error
            VIP_DUNGEON_RESET_COUNT_OVER = 1651,      // 던전 초기화 횟수를 초과하였습니다.
            VIP_DUNGEON_RESET_TYPE_INVALIDE = 1652,   // 던전 초기화 타입이 맞지 않습니다.
            VIP_SHOP_BUY_COUNT_OVER = 1653,           // 상점 구매 횟수를 초과하였습니다.
            VIP_CONTENTS_LOCKED = 1654,         // VIP 레벨이 부족합니다.
            VIP_SWEEP_TYPE_INVALIDE = 1655,           // 소탕을 사용할 수 없습니다.
            VIP_SHOP_REST_COUNT_OVER = 1656,          // 상점 초기화 횟수를 초과하였습니다.
            VIP_SHOP_REST_TYPE_INVALIDE = 1657,             // 상점 초기화 타입이 맞지 않습니다.
            VIP_EXPEDITION_HERO_REGI_MAX = 1660,            // 황금원정단 용병 등록 횟수를 초과해서 더이상 등록할 수 없습니다.
            VIP_EXPEDITION_HIRE_MAX = 1661,           // 황금원정단 용병 고용 횟수를 초과해서 더이상 고용할 수 없습니다.
            VIP_REWARD_LEVEL_INVALID = 1670,           // VIP 보상 레벨이 맞지 않습니다.

            // Soul Error
            SOUL_SYSTEM_INFO_NOT_FOUND = 1700, // STRING_MSG_SOUL_ERROR    혼정보를 찾을수 없습니다. (시스템 오류)
            SOUL_ID_NOT_FOUND = 1701, // STRING_MSG_SOUL_ERROR    존재하지 않는 혼입니다.
            SOUL_ALREADY_ACTIVATE = 1702, // STRING_MSG_SOUL_ERROR_01    이미 활성화된 엑티브 혼입니다.
            SOUL_PARTS_NOT_ENOUGH = 1703, // STRING_MSG_SOUL_030    혼 조각이 부족하여 생성이 불가능합니다.

            SOUL_EQUIPSLOT_INVALIDE = 1711, // STRING_MSG_SOUL_ERROR_02    혼 장비 슬롯을 찾을 수 없습니다.
            SOUL_EQUIPSLOT_LIMIT_LEVEL_NOT_ENOUGH = 1712, // STRING_MSG_SOUL_ERROR_03    레벨이 부족하여 혼 장비 슬롯을 사용할 수 없습니다.
            SOUL_NOT_ACTIVATED = 1713, // STRING_MSG_SOUL_ERROR_04    아직 획득하지 못한 혼입니다.

            SOUL_EQUIP_ID_INVALIDE = 1721, // STRING_MSG_SOUL_ERROR_05    혼 장비를 찾을 수 없습니다.
            SOUL_EQUIP_ID_SLOT_FULL = 1722, // STRING_MSG_SOUL_ERROR_06    혼 슬롯이 가득 찼습니다.
            SOUL_EQUIP_ID_EQUIP_FAIL = 1723, // STRING_MSG_SOUL_EQUIP_ERROR    혼 장착에 실패했습니다.
            SOUL_EQUIP_GROUP_INVALIDE = 1724,   // 같은 혼은 동시에 장착할 수 없습니다.

            SOUL_PASSIVE_ALREADY_CREATED = 1731, // STRING_MSG_SOUL_029    혼 생성 슬롯에 이미 혼이 생성되어 있습니다. (생성규칙 변경으로 더이상 사용하지 않음)
            SOUL_PASSIVE_CREATE_LIMIT = 1732, // STRING_MSG_SOUL_ERROR_07    일일 보조혼 생성 횟수를 초과하여 더 이상 생성할 수 없습니다. (루비로 생성하는 횟수 제한에 걸렸을시)
            SOUL_NOT_ENOUGH_PASSIVE_STONE = 1733, // STRING_MSG_SOUL_031    혼 소환석이 부족하여 보조 혼 생성이 불가능합니다.

            SOUL_ENHANCE_INFO_NOT_FOUND = 1741, // STRING_MSG_SOUL_ERROR_08    혼 강화 정보를 찾을 수 없습니다.
            SOUL_ENHANCE_LEVEL_MAX = 1742, // STRING_MSG_SOUL_033    더 이상 혼 레벨 강화를 할 수 없습니다.
            SOUL_ENHANCE_CANT_OVER_CHAR_LEVEL = 1743, // STRING_MSG_SOUL_035    혼 레벨업은 캐릭터 레벨을 초과할 수 없습니다.

            SOUL_ENHANCE_GRADE_MAX = 1744, // STRING_MSG_SOUL_033    더이상 혼 등급 강화를 할 수 없습니다.
            SOUL_ENHANCE_GRADE_NOT_ENOUGH_EQUIP = 1745, // STRING_MSG_SOUL_ERROR_09    혼 장비가 부족해서 등급을 올릴 수 없습니다.

            SOUL_ENHANCE_STARLEVEL_MAX = 1746, // STRING_MSG_SOUL_ERROR_10    더 이상 혼의 성급을 할 수 없습니다.
            SOUL_ENHANCE_STAR_UP_NOT_ENOUGH_PARTS = 1747, // STRING_MSG_SOUL_ERROR_11    성급을 하기 위한 혼 조각이 부족합니다.

            SOUL_NOT_ENOUGH_SOUL_EQUIP = 1751, // STRING_MSG_SOUL_ERROR_12    강화에 필요한 혼장비가 부족합니다.

            SOUL_PASSIVE_STORAGE_MAX = 1761, // STRING_MSG_SOUL_044    보조 혼 보관함이 가득 찼습니다.
            SOUL_PASSIVE_NOT_ENOUGH_EXP = 1762, // STRING_MSG_SOUL_034    보조 혼 강화에 필요한 경험치가 부족합니다.

            SOUL_ACTIVE_BUFF_ID_NOT_FOUND = 1771, // STRING_MSG_SOUL_ERROR_13    액티브 혼 버프 ID를 찾을 수 없습니다. (시스템 오류)
            SOUL_ACTIVE_BUFF_POS_INVALIDE = 1772, // STRING_MSG_SOUL_ERROR_14    액티브 혼 버프 위치를 찾을 수 없습니다. (시스템 오류)

            SOUL_MAX_SLOT = 1781,   // 더 이상 혼 슬롯을 확장 할 수 없습니다.
            SOUL_SLOT_TYPE_INVALIDE = 1782, // 혼을 장착할 슬롯 위치가 맞지 않습니다.

            // Gold Expedition && PvP
            GE_ENEMY_ID_NOT_FOUND = 1851, // STRING_MSG_EXPEDITION_ERROR_01    상대 정보를 찾을 수 없습니다.
            GE_ENEMY_COUNT_NOT_ENOUGH = 1852, // STRING_MSG_EXPEDITION_ERROR_02    매칭 대상의 수가 부족합니다.
            GE_SYSTEM_STAGE_INFO_NOT_FOUND = 1853, // STRING_MSG_EXPEDITION_ERROR_01    상대 정보를 찾을 수 없습니다.

            GE_USER_STAGE_INFO_NOT_FOUND = 1861, // STRING_MSG_EXPEDITION_ERROR_01    상대 정보를 찾을 수 없습니다.
            GE_USER_STAGE_INVALID = 1862, // STRING_MSG_EXPEDITION_ERROR_01    상대 정보를 찾을 수 없습니다.
            GE_USER_CHARACTER_HP_NOT_ENOUGH = 1863, // STRING_MSG_EXPEDITION_ERROR_03    캐릭터 HP 정보를 찾을 수 없습니다.

            GE_MERCENARY_REGIST_FAIL_NO_GUILD = 1871, // STRING_MSG_EXPEDITION_ERROR_04    용병 정보를 찾을 수 없습니다.
            GE_MERCENARY_INFO_NOT_FOUND = 1872, // STRING_MSG_EXPEDITION_ERROR_04    용병 정보를 찾을 수 없습니다.
            GE_MERCENARY_CALL_TIME_NOT_ENOUGH = 1873, // STRING_MSG_EXPEDITION_ERROR_05    용병 요청 시간 오류입니다.

            GE_RESET_COUNT_OVER = 1881,         // 황금원정단 초기화 횟수를 초과하여 더 이상 초기화 할 수 없습니다.

            PVP_PLAYTIME_CLOSED = 1890,         // PVP 오픈시간이 아닙니다.
            PVP_GUILD_REJOIN_TIME_LEFT = 1891,  // 길드 PVP 도전 쿨타임이 남았습니다.

            PVP_BOT_INFO_NOT_FOUND = 1895,  // PVP 봇 정보를 찾지 못하였습니다.

            // PVP error code add to 801~899 range
            PVP_OVERLORD_INFO_NOT_FOUND = 801,          // 패왕의길 상대 정보를 찾을수 없습니다.
            PVP_PLAYCOUNT_OVER = 802,                   // 패왕의길 참전횟수를 초과했습니다.
            PVP_PLAYER_ALREADY_JOIN_BATTLE = 803,       // 상대가 이미 전투에 참여하고 있습니다.
            PVP_PLAYER_NOT_IN_PLAY = 804,               // 전투에 참여하지 않고 결과처리를 요청할 수 없습니다.

            E_OVERLOD_END_RESULT_NO_CHANGE_RANKING = 810,   // 랭킹 등록시 상대 랭킹이 나의 랭킹보다 낮을 경우 랭킹은 변경되지 않음. (클라에 메시지 알림 필요)
            E_OVERLOD_END_RESULT_CHANGE_RANKING = 811,        // 상대유저의 랭킹과 나의 랭킹이 체인지됨. (정상 체인지)
            E_OVERLOD_END_RESULT_REGISTERED_NO_CHANGE_RANKING_BY_LOSE = 812,    //졌으므로 순위변경 없음.

            // system error
            System_Unknown_Operation = 1900, // STRING_MSG_ERROR_SYSTEM_UNKNOWN    유효하지 않은 요청입니다. 
            System_ItemBase_NOT_FOUND = 1901, // STRING_MSG_ERROR_ITEM_BASE    아이템 시스템 정보를 찾을 수 없습니다. (시스템 오류)
            SYSTEM_PARAM_ERROR = 1902, // STRING_MSG_ERROR_PARAM    요청을 위한 인자값이 부족합니다.

            System_Hack_Detected = 1910, // 비정상 요청입니다. (핵유저 의심)

            REDIS_COMMAND_FAIL = 1950, // STRING_MSG_ERROR_REDIS    랭킹 서버가 응답하지 않습니다. (시스템 오류)

            System_Exception = 1999, // STRING_MSG_ERROR_SYSTEM_EXCEPTION    시스템 에러입니다. 개발팀에 문의해 주세요.
        };

        public enum eRoute
        {
            CHARACTER = 1,
            SHOP = 2,
            SELL = 3,
            SCENARIO = 4,
            GUERRILLA_DUNGEON = 5,
            BOSSRAID = 6,
            PVP_SOLO = 7,
            PVP_INFINITE = 8,
            REWARD = 9,
            ACHIEVEMENT = 10,
            SOUL_ILLUSTRATED = 11,
            GMTOOL = 12,
            EVENTEXEC = 13,
            SOUL = 14,
            ITEM = 15,
            GIFTING = 16,
            COUPON = 17,
            CGP = 18,
            GUILD = 19,
            FREE_GACHAR = 20,
            GUILD_3VS3 = 21,
            RUBY_PVP = 22,
            ELITE_DUNGEON = 23,
            GOLDEXPEDITION = 24,
        };

        public enum eSubRoute
        {
            // eRoute 1 캐릭터
            CHARACTER_BUY = 1,

            // eRoute 4 ~ 8 / 21 ~ 24
            DUNGEON_ENTER = 1,         //  1	입장
            RANDOM_REWARD = 2,         //  2	게임내획득

            CLEAR_REWARD = 3,          //  3	클리어보상
            BASE_RANDOM_REWARD = 4,    //  4	기본 랜덤보상
            ADD_RANDOM_REWARD = 5,     //  5	추가 랜덤보상
            CHALLENGE_REWARD_1 = 6,    //  6	도전 과제보상1
            CHALLENGE_REWARD_2 = 7,    //  7	도전 과제보상2
            CHALLENGE_REWARD_3 = 8,    //  8	도전 과제보상3
            STRAIGHT_WIN_REWARD = 9,   //  9	연승보상
            REVIVE = 11,               //  11	부활
            ONEPLAY_REWARD = 12,       //  12	1회플레이보상
            USE_BOOSTER = 13,          //  13	부스터사용
            RETRY = 14,                //  14	다시한번더
            LASTWEEK_REWARD = 15,      //  15	지난주보상
            FAIL_RESTORE_KEY = 16,     //  16	실패복원키
            CHANGE = 17,               //  17	교체
            GUILD_REWARD_BOX = 18,     //  18	길드보상상자
            DAY_CHECK_REWARD = 19,     //  19	일일참여보상

            // eRoute 9
            REWARD_LV_UP = 1,          //  1	레벨업
            REWARD_FRIEND = 2,         //  2	친구
            REWARD_TUTORIAL = 3,       //  3	튜토리얼

            // eRoute 10
            ACHIEVEMENT_END_REWARD = 1, // 업적 완료 보상
            ACHIEVEMENT_FIN_REWARD = 2, // 달성 보상
            
            // eRoute 12
            GMTOOL_EXEC = 1,            // 1    지급
            GMTOOL_BACK = 2,            // 2    회수
            GMTOOL_CHANGEINFO = 3,      // 3    정보 변경

            // eRoute 13
            EVENTEXEC_ATTEND_BASEREWARD = 1,    // 1 출석기본보상
            EVENTEXEC_ATTEND_DAYREWARD = 2,     // 2 출석일일보상
            EVENTEXEC_EVENT = 3,                // 3 이벤트

            // eRoute 14
            SOUL_INVEN_ADDSLOT = 1,     //  1	혼 가방확장
            SOUL_GACHAR = 2,            //  2	가챠
            SOUL_CHANGE_ABILITY = 3,    //  3	혼정보 정예 능력 변경
            SOUL_CHANGE_HERO = 4,       //  4	혼정보 영웅 능력 변경
            SOUL_REGRANT = 5,           //  5	혼정보 모두개조
            SOUL_ENCHANT = 6,           //  6	혼강화
            SOUL_GRADEUP = 7,           //  7	혼진화
            SOUL_SLOT_FORCEOPEN = 8,    //  8	혼슬롯강제열기

            // eRoute 15
            ITEM_INVEN_ADDSLOT = 1,     // 1	가방확장
            ITEM_EQUIP_ENCHANT = 2,     // 2	장착 장비 강화
            ITEM_EQUIP_GRADEUP = 3,     // 3	장착 장비 진화
            ITEM_GACHAR = 4,            // 4	가챠
            ITEM_BOOSTER = 5,           // 5	부스터
            ITEM_COSTUME = 6,           // 6	코스튬
            
            // eRoute 16
            GIFTING_CHANGE = 1,         // 1 교환
            GIFTING_FACEBOOK = 2,       // 2 페이스북 친구
            
            // eRoute 19
            GUILD_JOIN = 1,             //  1	가입
            GUILD_REJECT = 2,           //  2	가입거절
            GUILD_SECESSION = 3,        //  3	탈퇴
            GUILD_EXILE = 4,            //  4	제명(추방)
            GUILD_CANCEL_JOIN = 5,      //  5	가입취소
            GUILD_REQUEST_JOIN = 6,     //  6	가입신청
            GUILD_ATTEND_CHECK = 7,     //  7	출석체크
            GUILD_CREATE = 9,           //  9	창설
            GUILD_ENTRUST_MASTER = 10,  //  10	길드장 위임
            GUILD_DONATION_GOLD = 11,   //  11  길드 기부 골드 1
            GUILD_DONATION_RUBY = 12,   //  12  길드 기부 루비 2
            GUILD_DONATION_RUBYS = 13,  //  13  길드 기부 루비 3
            
            // eRoute 20
            FREEGACHAR_ITEM = 1,        // 1 일일장비무료가차
            FREEGACHAR_SOUL = 2,        // 2 일일소울무료가차
            
        };

        public enum eArchiveSubType
        {
            BOSSRAID = 58,
        }
    }
}
