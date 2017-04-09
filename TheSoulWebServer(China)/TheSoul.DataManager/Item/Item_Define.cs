using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSoul.DataManager
{
    public class Item_Define
    {
        public const string Item_InvenDB = "sharding";

        public const string Item_System_Item_Base_Table = "System_Item_Base";
        public const string Item_System_Item_Equip_Table = "System_Item_Equip";
        public const string Item_System_Item_Use_Table = "System_Item_Use";
        public const string Item_System_Item_Info_Table = "System_Item_Info";
        public const string Item_System_Item_Band_Table = "System_Item_Band";
        public const string Item_System_Item_Option_Table = "System_Item_Option";
        public const string Item_System_Item_Option_UseInfo_Table = "System_Item_Option_UseInfo";
        public const string Item_System_Item_TierInfo_Table = "System_Item_TierInfo";
        public const string Item_System_Item_Enchant_Armor_Table = "System_Item_Enchant_Armor";
        public const string Item_System_Item_Grade_Accessory_Table = "System_Item_Grade_Accessory";
        public const string Item_System_Item_Grade_Weapon_Table = "System_Item_Grade_Weapon";
        public const string Item_System_Item_Refining_Weapon_Table = "System_Item_Refining_Weapon";
        public const string Item_System_Item_Enchant_Table = "System_Item_Enchant";
        public const string Item_System_Item_Cape_Table = "System_Item_Cape";
        public const string Item_System_Item_Costume_Table = "System_Item_Costume";
        public const string Item_System_Item_Tier_SetInfo_Table = "System_Item_Tier_SetInfo";
        public const string Item_User_Inven_Table = "User_Inven";
        public const string Item_User_Inven_Option_Table = "User_Inven_Option";
        public const string Item_User_Item_Enchant_Table = "User_Item_Enchant";
        public const string Item_User_Character_VIP_Costume_Table = "User_Character_VIP_Costume";

        public const string Item_System_Ultimate_Weapon_Table = "System_Ultimate_Weapon";
        public const string Item_System_Ultimate_Enchant_Table = "System_Ultimate_Enchant";
        public const string Item_System_Ultimate_Orb_Table = "System_Ultimate_Orb";

        public const string Item_User_Ultimate_Inven_Table = "User_Ultimate_Inven";
        //public const string Item_User_Ultimate_Orb_Slot_Table = "User_Ultimate_Orb_Slot";
        public const string Item_User_Orb_Inven_Table = "User_Orb_Inven";

        public const string SystemItem_Prefix = "SystemItem";
        public const string User_Inven_Prefix = "UserInven";
        public const string User_Equip_Prefix = "UserEquip";
        public const string User_Ultimate_Equip_Prefix = "UserUltimateEquip";

        public const int Bag_Capacity_Limit_Over_Count = 10;

        /*
ItemClass_Equip: 장착장비 아이템
ItemClass_Use: 사용형 아이템
ItemClass_Info: 정보형 아이템
ItemClass_Costume: 코스튬 아이템
ItemClass_Band: 패키지 아이템
         */
        public enum eSystemItemType
        {
            ItemClass_NONE = 0,
            ItemClass_Equip = 1,
            ItemClass_Use,
            ItemClass_Info,
            ItemClass_Costume,
            Soul_Equip,
            Soul_Parts,
            ItemClass_Band,
            ItemClass_Ultimate,
            ItemClass_Orb,
            ItemClass_First = ItemClass_Equip,
        };

        public static readonly Dictionary<string, eSystemItemType> SystemItemType = new Dictionary<string, eSystemItemType>()
        {
            {"ItemClass_Equip", eSystemItemType.ItemClass_Equip },
            {"ItemClass_Use", eSystemItemType.ItemClass_Use },
            {"ItemClass_Info", eSystemItemType.ItemClass_Info },
            {"ItemClass_Costume", eSystemItemType.ItemClass_Costume },
            {"Soul_Equip", eSystemItemType.Soul_Equip },
            {"Soul_Parts", eSystemItemType.Soul_Parts },
            {"ItemClass_Ultimate", eSystemItemType.ItemClass_Ultimate },
            {"ItemClass_Orb", eSystemItemType.ItemClass_Orb },
        };

        public const string UltimateItemType = "ItemType_Ultimate";
        public const string UltimateWeaponItemType = "ItemType_Ultimate_Weapon";
        public const string UltimateOrbItemType = "ItemType_Ultimate_Orb";

        public const int UltimateWeaponBaseLevel = 1;
        public const int UltimateWeaponBaseStep = 1;

        public static readonly Dictionary<byte, byte[]> UltimateOrbEquipSlot = new Dictionary<byte, byte[]>()
        {
            { 1, new byte[] { 1, 2 } },
            { 2, new byte[] { 3, 4 } },
            { 3, new byte[] { 5, 6 } },
        };

        public static readonly Dictionary<string, eItemType> ItemType = new Dictionary<string, eItemType>()
        {
            {"None", eItemType.None},
            {"ItemType_Helmet", eItemType.ItemType_Helmet},
            {"ItemType_Armor", eItemType.ItemType_Armor},
            {"ItemType_Glove", eItemType.ItemType_Glove},
            {"ItemType_Shoes", eItemType.ItemType_Shoes},

            {"ItemType_GSword", eItemType.ItemType_GSword},
            {"ItemType_DSword", eItemType.ItemType_DSword},
            {"ItemType_Staff", eItemType.ItemType_Staff},

            {"ItemType_Cape", eItemType.ItemType_Cape},

            {"ItemType_Necklace", eItemType.ItemType_Necklace},
            {"ItemType_Ring", eItemType.ItemType_Ring},
            {"ItemType_Earing", eItemType.ItemType_Earing},
            {"ItemType_Belt", eItemType.ItemType_Belt},
            {"ItemType_Bracelet", eItemType.ItemType_Bracelet},
            {"ItemType_Amulet", eItemType.ItemType_Amulet},

            {"ItemType_HPPotion", eItemType.ItemType_HPPotion},
            {"ItemType_MPPotion", eItemType.ItemType_MPPotion},
            {"ItemType_MakeCape", eItemType.ItemType_MakeCape},

            {"ItemType_GetCash", eItemType.ItemType_GetCash},
            {"ItemType_GetRealCash", eItemType.ItemType_GetRealCash},
            {"ItemType_GetChallenge", eItemType.ItemType_GetChallenge},
            {"ItemType_GetExpeditionPoint", eItemType.ItemType_GetExpeditionPoint},
            {"ItemType_GetGuildPoint", eItemType.ItemType_GetGuildPoint},
            {"ItemType_GetRankingPoint", eItemType.ItemType_GetRankingPoint},
            {"ItemType_GetBattlePoint", eItemType.ItemType_GetBattlePoint},
            {"ItemType_GetHonorPoint", eItemType.ItemType_GetHonorPoint},
            {"ItemType_GetPartyPoint", eItemType.ItemType_GetPartyPoint},
            {"ItemType_GetPassiveSoul", eItemType.ItemType_GetPassiveSoul},
            {"ItemType_GetBlackMarketPoint", eItemType.ItemType_GetBlackMarketPoint},
            
            {"ItemType_GetCostume_PC1", eItemType.ItemType_GetCostume_PC1},
            {"ItemType_GetCostume_PC2", eItemType.ItemType_GetCostume_PC2},
            {"ItemType_GetG3VS3Box", eItemType.ItemType_GetG3VS3Box},
            {"ItemType_GetGachaCoin", eItemType.ItemType_GetGachaCoin},
            {"ItemType_GetGold", eItemType.ItemType_GetGold},
            {"ItemType_GetGuildEXPBuff", eItemType.ItemType_GetGuildEXPBuff},
            {"ItemType_GetGuildSkillBuff", eItemType.ItemType_GetGuildSkillBuff},
            {"ItemType_GetItemGacha", eItemType.ItemType_GetItemGacha},
            {"ItemType_GetMedal", eItemType.ItemType_GetMedal},
            {"ItemType_GetPCEXPBuff", eItemType.ItemType_GetPCEXPBuff},
            {"ItemType_GetPvECoin", eItemType.ItemType_GetPvECoin},
            {"ItemType_GetPvPCoin", eItemType.ItemType_GetPvPCoin},
            {"ItemType_GetSoulEXPBuff", eItemType.ItemType_GetSoulEXPBuff},
            {"ItemType_GetSoulGacha", eItemType.ItemType_GetSoulGacha},
            {"ItemType_GetSummonStone", eItemType.ItemType_GetSummonStone},
            {"ItemType_InfinitePvECoin", eItemType.ItemType_InfinitePvECoin},
            {"ItemType_InfinitePvPCoin", eItemType.ItemType_InfinitePvPCoin},
            {"ItemType_LifeStone", eItemType.ItemType_LifeStone},
            {"ItemType_LuckyBox", eItemType.ItemType_LuckyBox},
            {"ItemType_Material", eItemType.ItemType_Material},
            {"Itemtype_Boost", eItemType.Itemtype_Boost},
            {"ItemType_Modification_Soul", eItemType.ItemType_Modification_Soul},
            {"ItemType_Subscription", eItemType.ItemType_Subscription},
            {"ItemType_Subscription_Week", eItemType.ItemType_Subscription_Week},
            {"ItemType_Wing", eItemType.ItemType_Wing},
            {"ItemType_Costume", eItemType.ItemType_Costume},

            {"ItemType_Ultimate", eItemType.ItemType_Ultimate},
            {"ItemType_Ultimate_Weapon", eItemType.Itemtype_Ultimate_Weapon},
            {"ItemType_Ultimate_Orb", eItemType.Itemtype_Ultimate_Orb},
        };

        public static readonly Dictionary<eItemType, string> ItemTypeToOptionType = new Dictionary<eItemType, string>()
        {
            {eItemType.ItemType_Helmet,"ItemType_Wear"},
            {eItemType.ItemType_Armor,"ItemType_Wear"},
            {eItemType.ItemType_Glove,"ItemType_Wear"},
            {eItemType.ItemType_Shoes,"ItemType_Wear"},
            {eItemType.ItemType_GSword,"ItemType_Weapon"},
            {eItemType.ItemType_DSword,"ItemType_Weapon"},
            {eItemType.ItemType_Staff,"ItemType_Weapon"},
            {eItemType.ItemType_Cape,"ItemType_Cape"},
            {eItemType.ItemType_Ring,"ItemType_Ring"},
            {eItemType.ItemType_Necklace,"ItemType_Necklace"},
            {eItemType.ItemType_Earing,"ItemType_Earing"},
            {eItemType.ItemType_Belt,"ItemType_Belt"},
            {eItemType.ItemType_Bracelet,"ItemType_Bracelet"},
            {eItemType.ItemType_Amulet,"ItemType_Amulet"},

            {eItemType.ItemType_Wing,"ItemType_Wear"},
            {eItemType.ItemType_Costume,"ItemType_Wear"},
        };

        public static readonly Dictionary<string, string> OptionTypeToOptionEquip = new Dictionary<string, string>()
        {
            {"ItemType_Wear","OptionEquip_Wear"},
            {"ItemType_Weapon","OptionEquip_Weapon"},
            {"ItemType_Cape","OptionEquip_Cape"},
            {"ItemType_Ring","OptionEquip_Accessory"},
            {"ItemType_Necklace","OptionEquip_Accessory"},
            {"ItemType_Earing","OptionEquip_Accessory"},
            {"ItemType_Belt","OptionEquip_Accessory"},
            {"ItemType_Bracelet","OptionEquip_Accessory"},
            {"ItemType_Amulet","OptionEquip_Accessory"},
        };

        public const string OptionEquip_All = "OptionEquip_ALL";
        public const string OptionType_Random = "OptionType_Random";

        public static readonly Dictionary<eItemMakeType, string> ItemRateType = new Dictionary<eItemMakeType, string>()
        {
            {eItemMakeType.OptionRateType_Drop,"OptionRateType_Drop"},
            {eItemMakeType.OptionRateType_Cash,"OptionRateType_Cash"},
            {eItemMakeType.OptionRateType_Special,"OptionRateType_Special"},
            {eItemMakeType.MailOpen, "OptionRateType_Drop"},
        };

        public enum eItemType
        {
            None = 0,    // Non Item  (band?)

            ItemType_Helmet = 1,    // Equip Armor
            ItemType_Armor = 2,
            ItemType_Glove = 3,
            ItemType_Shoes = 4,

            ItemType_GSword = 5,    // Equip Weapon
            ItemType_DSword = 6,
            ItemType_Staff = 7,

            ItemType_Cape = 8,      // Equip Cape

            ItemType_Ring = 11,     // Accesorary
            ItemType_Necklace = 12,
            ItemType_Earing = 13,
            ItemType_Belt = 14,
            ItemType_Bracelet = 15,
            ItemType_Amulet = 16,

            ItemType_HPPotion = 21,     // Use Item
            ItemType_MPPotion = 22,
            ItemType_MakeCape = 23,

            ItemType_GetCash = 31,      // Info Item
            ItemType_GetRealCash,
            ItemType_GetChallenge,
            ItemType_GetExpeditionPoint,
            ItemType_GetGuildPoint,
            ItemType_GetRankingPoint,
            ItemType_GetBattlePoint,
            ItemType_GetHonorPoint,
            ItemType_GetPartyPoint,
            ItemType_GetPassiveSoul,
            ItemType_GetBlackMarketPoint,

            ItemType_GetCostume_PC1,
            ItemType_GetCostume_PC2,
            ItemType_GetG3VS3Box,
            ItemType_GetGachaCoin,
            ItemType_GetGold,
            ItemType_GetGuildEXPBuff,
            ItemType_GetGuildSkillBuff,
            ItemType_GetItemGacha,
            ItemType_GetMedal,
            ItemType_GetPCEXPBuff,
            ItemType_GetPvECoin,
            ItemType_GetPvPCoin,
            ItemType_GetSoulEXPBuff,
            ItemType_GetSoulGacha,
            ItemType_GetSummonStone,
            ItemType_InfinitePvECoin,
            ItemType_InfinitePvPCoin,
            ItemType_LifeStone,
            ItemType_LuckyBox,
            ItemType_Material,
            ItemType_Modification_Soul,
            ItemType_Subscription,
            ItemType_Subscription_Week,
            Itemtype_Boost,

            ItemType_Ultimate,
            Itemtype_Ultimate_Weapon,
            Itemtype_Ultimate_Orb,

            ItemType_Wing = 101,      // Costume
            ItemType_Costume = 102,
        };

        public static readonly List<eItemType> ItemArmorTypeList = new List<eItemType>()
        {
            eItemType.ItemType_Helmet,
            eItemType.ItemType_Armor,
            eItemType.ItemType_Glove,
            eItemType.ItemType_Shoes,
        };

        public static readonly List<eItemType> ItemWeaponTypeList = new List<eItemType>()
        {
            eItemType.ItemType_GSword,
            eItemType.ItemType_DSword,
            eItemType.ItemType_Staff,
        };

        public static readonly List<eItemType> ItemAccessoryTypeList = new List<eItemType>()
        {
            eItemType.ItemType_Ring,
            eItemType.ItemType_Earing,
            eItemType.ItemType_Necklace,
            eItemType.ItemType_Belt,
            eItemType.ItemType_Bracelet,
            eItemType.ItemType_Amulet,
        };

        public static readonly List<eItemType> ItemCapeTypeList = new List<eItemType>()
        {
            eItemType.ItemType_Cape,
        };

        public static readonly List<eItemType> ItemCostumeTypeList = new List<eItemType>()
        {
            eItemType.ItemType_Wing,
            eItemType.ItemType_Costume,
        };


        public enum eItemMakeType
        {
            OptionRateType_Drop = 1,
            OptionRateType_Cash = 2,
            OptionRateType_Special = 3,
            MailOpen = 1001,
        };

        public static readonly Dictionary<string, int[]> GradeOptionCount = new Dictionary<string, int[]>()
        {
                                    // not use yet 0 grade
            {"ItemType_Wear",       new int[] { 0, 0, 0, 0, 0, 0 } },       // armor or wear
            {"ItemType_Weapon",     new int[] { 0, 0, 0, 1, 2, 3 } },       // weapon
            {"ItemType_Cape",       new int[] { 0, 3, 3, 3, 3, 3 } },       // cape (1~3 random count check)
            {"ItemType_Ring",       new int[] { 0, 0, 2, 2, 2, 2 } },       // accessory has only random option count (fix option not count yet)
            {"ItemType_Necklace",   new int[] { 0, 0, 2, 2, 2, 2 } },
            {"ItemType_Earing",     new int[] { 0, 0, 2, 2, 2, 2 } },
            {"ItemType_Belt",       new int[] { 0, 0, 2, 2, 2, 2 } },
            {"ItemType_Bracelet",   new int[] { 0, 0, 2, 2, 2, 2 } },
            {"ItemType_Amulet",     new int[] { 0, 0, 2, 2, 2, 2 } },
        };

        public static readonly Dictionary<int, List<byte>> AccessoryOptionGradeRate = new Dictionary<int, List<byte>>()
        {                                
            {0,       new List<byte> { 0, 0, 0, 0, 0 } },      
            {1,       new List<byte> { 0, 0, 0, 0, 0 } },       // Each column means option grade rate
            {2,       new List<byte> { 2, 1, 0, 0, 0 } },       
            {3,       new List<byte> { 3, 1, 0, 0, 0 } },       
            {4,       new List<byte> { 4, 1, 0, 0, 0 } },       
            {5,       new List<byte> { 5, 1, 0, 0, 0 } },
        };

        public const float ItemOptionGradeGap = 0.2f;
        public const int ItemOptionMakeTryCount = 3;
        public const int ItemCape_RandomOptionMax = 3;
        public const string Item_Option_Type_Random = "OptionType_Random";

        public const bool bItemBaseOption = true;
        public const bool bUnEquip = false;
        public const bool bEquip= true;

        public static readonly string[] Item_Option_Type_Float_Value = new string[]
        {
            "ParameterType_RATE_HP",
            "ParameterType_RATE_MP",
            "ParameterType_RATE_AP",
            "ParameterType_RATE_DFP",
            "ParameterType_CPR",
            "ParameterType_CP",
            "ParameterType_CR",
            "ParameterType_DCR",
        };

        public const string CapefParam_DEF = "ParameterType_DFP";
        public const string WeaponParam_Min_Damage = "ParameterType_AP_MIN";
        public const string WeaponParam_Max_Damage = "ParameterType_AP_MAX";

        public const int Item_Option_Float_To_Int_Rate = 100;      // only use x.xx

        public static readonly Dictionary<eItemType, eInventoryType> Item_Make_Inventory_Type = new Dictionary<eItemType, eInventoryType>()
        {
            { eItemType.ItemType_Helmet, eInventoryType.Character_Inven },       // Armor
            { eItemType.ItemType_Armor, eInventoryType.Character_Inven },
            { eItemType.ItemType_Glove, eInventoryType.Character_Inven },
            { eItemType.ItemType_Shoes, eInventoryType.Character_Inven },

            { eItemType.ItemType_GSword, eInventoryType.Character_Inven },     // weapon
            { eItemType.ItemType_DSword, eInventoryType.Character_Inven },
            { eItemType.ItemType_Staff, eInventoryType.Character_Inven },

            { eItemType.ItemType_Cape, eInventoryType.Character_Inven },           // Cape

            { eItemType.ItemType_Ring, eInventoryType.Account_Inven },       // accessory
            { eItemType.ItemType_Necklace, eInventoryType.Account_Inven },
            { eItemType.ItemType_Earing, eInventoryType.Account_Inven },
            { eItemType.ItemType_Belt, eInventoryType.Account_Inven },
            { eItemType.ItemType_Bracelet, eInventoryType.Account_Inven },
            { eItemType.ItemType_Amulet, eInventoryType.Account_Inven },

            { eItemType.ItemType_Wing, eInventoryType.Character_Inven },           // Wing??
            { eItemType.ItemType_Costume, eInventoryType.Character_Inven },
        };

        public enum eInventoryType
        {
            Account_Inven = 1,
            Character_Inven = 2,
            Soul_Inven = 3,
            Ultimate_Inven = 4,
            Temp_Inven = 100,
        };

        public enum eItemType_Inven
        {
            Equip = 1,
            Gacha = 2,
            Costume = 3,
            Info = 2,
            Use = 2,
            Ultimate = 4,
            None = 2,
        };

        public const string Item_OptionTypeColumn = "optiontype";
        public const string Item_OptionValueColumn = "optionvalue";
        public const byte Item_MaxOptionCount = 5;

        public const byte ItemMaxEvolutionGrade = 5;
        public const byte ItemMaxGradeLevel = 5;
        public const float Item_Grade_Diff_Rate = 0.2f;

        //public const byte ItemMax_Weapon_Level = 10;
        public const byte ItemMax_Weapon_Grade = 5;
        public const byte ItemMax_Weapon_Option_Level = 10;

        public const byte Item_Armor_Base_Grade = 1;
        public const byte Item_Armor_Base_Level = 0;

        public const byte Item_Weapon_Adv_Disassemble_Grade = 3;
        //public const byte Item_Weapon_Adv_Disassemble_Level = 5;
        //public const byte Item_Weapon_Adv_Disassemble_High_Rate_Level = 9;

        public const byte Item_Accessory_Option_Max_Count = 4;
        public const byte Item_Accessory_Option_Change_Min_Grade = 3;

        //public const byte ItemMax_Cape_Level = 10;

        public const int Item_MaxRate = 100;

        public static readonly List<float> Item_Disassemble_Level_Rate = new List<float>()
        {
            0.5f,                         // 0 base 
            0.5f, 0.5f, 0.5f, 0.5f, 0.5f,
            0.5f, 0.5f, 0.9f, 0.9f, 0.9f,
            0.9f, 0.9f, 0.9f, 0.9f, 0.9f,
        };

        public enum eItemReturnKeys
        {
            RetGold,
            RetRuby,
            EnchantLevel,
            EnchantExp,
            EnchantGrade,
            ItemInfo,
            ItemList,
            GetItemList,
            EnchantSuccess,
            EvolutionSuccess,
            DisassembleItem,
            DeletedItem,
            AccountInven,
            CharacterInven,
            SellPrice,
            UpdateItemInfo,
            BSuccess,
            ItemOption,
            SmeltUseRuby,
            MaterialUpdateItemInfo,
            RetStone,
            RetKey,
            RetTicket,
            EquipItemList,
            EquipUltimateWeaponList,
            UltimateInfo,
            OrbInfo,
            BeforeEquipWeapon,
            AfterEquipWeapon,
        };

        public static readonly Dictionary<eItemReturnKeys, string> Item_Ret_KeyList = new Dictionary<eItemReturnKeys, string>()
        {
            { eItemReturnKeys.RetGold,           "retgold"          },
            { eItemReturnKeys.RetRuby,           "retruby"          },
            { eItemReturnKeys.ItemList,          "itemlist"     },
            { eItemReturnKeys.AccountInven,      "accountinven"     },
            { eItemReturnKeys.CharacterInven,    "characterinven"     },
            { eItemReturnKeys.ItemInfo,          "iteminfo"     },
            { eItemReturnKeys.GetItemList,       "getitem"     },
            { eItemReturnKeys.EnchantLevel,      "enchantlevel"     },
            { eItemReturnKeys.EnchantExp,        "enchantexp"       },
            { eItemReturnKeys.EnchantGrade,      "enchantgrade"       },
            { eItemReturnKeys.EnchantSuccess,    "enchantsuccess"       },
            { eItemReturnKeys.EvolutionSuccess,  "evolutionsuccess"       },
            { eItemReturnKeys.DisassembleItem,   "disassembleitem"       },
            { eItemReturnKeys.DeletedItem,       "deletedlist"       },
            { eItemReturnKeys.SellPrice,         "sellprice"       },
            { eItemReturnKeys.UpdateItemInfo,    "update_item_info"       },
            { eItemReturnKeys.BSuccess,          "success"       },
            { eItemReturnKeys.ItemOption,  "item_detail_option"   },
            { eItemReturnKeys.SmeltUseRuby,  "disassemblecost"   },
            { eItemReturnKeys.MaterialUpdateItemInfo,  "material_update_item"   },
            { eItemReturnKeys.RetStone,           "retstone"          },
            { eItemReturnKeys.RetKey,           "retkey"          },
            { eItemReturnKeys.RetTicket,           "retticket"          },
            { eItemReturnKeys.EquipItemList,           "equip_item_list"          },
            { eItemReturnKeys.EquipUltimateWeaponList,           "equip_ultimate_list"          },
            { eItemReturnKeys.UltimateInfo,           "ultimate_info"          },
            { eItemReturnKeys.OrbInfo,           "orb_info"          },
            { eItemReturnKeys.BeforeEquipWeapon,           "before_equip"          },
            { eItemReturnKeys.AfterEquipWeapon,           "after_equip"          },
        };        

        public enum eItemBuyPriceType
        {
            None = 0,
            PriceType_PayReal,
            PriceType_PayCash,
            PriceType_PayGold,
            PriceType_PayExpeditionPoint,
            PriceType_PayGachaCoin,
            PriceType_PayGuildPoint,
            PriceType_PayMedal,
            PriceType_HonorPoint,
            PriceType_PartyDungeonPoint,
            PriceType_CombatPoint,
            PriceType_RankingPoint,//패왕의길
            PriceType_PayStack,
            PriceType_BlackMarketPoint,
            PriceType_Key,
            PriceType_Ticket,
        };

        public static readonly Dictionary<string, eItemBuyPriceType> Item_BuyType_List = new Dictionary<string, eItemBuyPriceType>()
        {
            { "PriceType_PayReal",              eItemBuyPriceType.PriceType_PayReal       },
            { "PriceType_PayCash",              eItemBuyPriceType.PriceType_PayCash       },
            { "PriceType_PayGold",              eItemBuyPriceType.PriceType_PayGold       },
            { "PriceType_PayExpeditionPoint",   eItemBuyPriceType.PriceType_PayExpeditionPoint       },
            { "PriceType_PayGachaCoin",         eItemBuyPriceType.PriceType_PayGachaCoin       },
            { "PriceType_PayGuildPoint",        eItemBuyPriceType.PriceType_PayGuildPoint       },
            { "PriceType_PayMedal",             eItemBuyPriceType.PriceType_PayMedal       },
            { "PriceType_HonorPoint",           eItemBuyPriceType.PriceType_HonorPoint       },
            { "PriceType_PartyDungeonPoint",    eItemBuyPriceType.PriceType_PartyDungeonPoint       },
            { "PriceType_CombatPoint",          eItemBuyPriceType.PriceType_CombatPoint       },
            { "PriceType_RankingPoint",          eItemBuyPriceType.PriceType_RankingPoint       },
            { "PriceType_PayStack",          eItemBuyPriceType.PriceType_PayStack       },
            { "PriceType_BlackMarketPoint",          eItemBuyPriceType.PriceType_BlackMarketPoint       },
            { "PriceType_Key",          eItemBuyPriceType.PriceType_Key       },
            { "PriceType_Ticket",          eItemBuyPriceType.PriceType_Ticket       },
        };


        public enum eItemConstDef
        {
            DEF_OPTIONWEIGT_WHITE,     // 랜덤옵션 선택 가중치(흰색)
            DEF_OPTIONWEIGT_GREEN,     // 랜덤옵션 선택 가중치(녹색)
            DEF_OPTIONWEIGT_BLUE,     // 랜덤옵션 선택 가중치(청색)
            DEF_OPTIONWEIGT_PURPLE,     // 랜덤옵션 선택 가중치(자색)
            DEF_OPTIONWEIGT_YELLOW,     // 랜덤옵션 선택 가중치(황색)
            DEF_RUBYTRADE_METAL,     // 루비 1당 철조각 환산치
            DEF_REFINING_START_ITEMLEVEL,     // 무기 정련 가능 시작레벨(황색만 해당)
            DEF_REFINING_EXPBONUS,     // 정련재료 동일 옵션 수량 1개에 대한 추가 경험치 보너스 비율(%단위)
            DEF_DISASSEMBLE_RATE_1,     // 분해시 녹색~자색 철조각 회수 비율(%단위)
            DEF_DISASSEMBLE_RATE_2,     // 분해시 황색 1~4레벨 철조각 회수 비율(%단위)
            DEF_SMELTING_RATE_1,     // 제련시 황색 5~8레벨 철조각, 강화석, 정련석, 강화부적 회수 비율(%단위)
            DEF_SMELTING_RATE_2,     // 제련시 황색 9~10레벨 철조각, 강화석, 정련석, 강화부적 회수 비율(%단위)
            DEF_SMELTING_PRICE,     // 제련시 소비 루비 금액
            DEF_WPNINCHANT_PROBUP_ID,     // 성공확률을 올려주는 강화부적의 아이템 인덱스 ID
            DEF_WPNINCHANT_PROBUP_VALUE,     // 강화부적 1장당 성공확률 상승량(%단위)
            DEF_RUBYTRADE_THREAD,     // 루비 1당 강화 실타래 환산치
            DEF_RUBYRETRY_MOD,     // 액세서리 개조 재시도 비용
            DEF_RECOVERY_ITEMID_METAL,     // 분해/제련시 회수 철조각
            DEF_RECOVERY_ITEMID_THREAD,     // 분해/제련시 회수 실타래
            DEF_RECOVERY_ITEMID_LVUP_STONE,     // 분해/제련시 회수 강화석
            DEF_RECOVERY_ITEMID_TALISMAN,     // 분해/제련시 회수 강화부적
            DEF_RECOVERY_ITEMID_HP_STONE,     // 분해/제련시 회수 HP정련석
            DEF_RECOVERY_ITEMID_AP_STONE,     // 분해/제련시 회수 AP정련석
            DEF_RECOVERY_ITEMID_DFP_STONE,     // 분해/제련시 회수 DFP정련석
            DEF_GOLD_OPTIONCHANGEALL_EACHTIER,  // 티어당 옵션 교체 비용. 높은티어 기준. 예)3티어<-->5티어는 좌측금액*5
            DEF_ARMORUI_ITEM_SLOT_MAX,          // 계정 가방 슬롯 최대 수
            DEF_WEAPONUI_ITEM_SLOT_MAX,         // 캐릭터 가방 슬롯 최대 수
            DEF_ITEM_MAX_ENCHANT_CNT,       // 최대 강화 레벨
            DEF_DISASSEMBLE_LEVEL,          // 분해 제한 레벨
            DEF_SMELTING_LEVEL1,            // 제련 시작 레벨 (?? 필요없음)
            DEF_SMELTING_LEVEL2,            // 고급 제련 시작레벨
        };

        public static readonly Dictionary<eItemConstDef, string> Item_Const_Def_Key_List = new Dictionary<eItemConstDef,string>()
        {
            { eItemConstDef.DEF_OPTIONWEIGT_WHITE, "DEF_OPTIONWEIGT_WHITE"},
            { eItemConstDef.DEF_OPTIONWEIGT_GREEN, "DEF_OPTIONWEIGT_GREEN"},
            { eItemConstDef.DEF_OPTIONWEIGT_BLUE, "DEF_OPTIONWEIGT_BLUE"},
            { eItemConstDef.DEF_OPTIONWEIGT_PURPLE, "DEF_OPTIONWEIGT_PURPLE"},
            { eItemConstDef.DEF_OPTIONWEIGT_YELLOW, "DEF_OPTIONWEIGT_YELLOW"},
            { eItemConstDef.DEF_RUBYTRADE_METAL, "DEF_RUBYTRADE_METAL"},
            { eItemConstDef.DEF_REFINING_START_ITEMLEVEL, "DEF_REFINING_START_ITEMLEVEL"},
            { eItemConstDef.DEF_REFINING_EXPBONUS, "DEF_REFINING_EXPBONUS"},
            { eItemConstDef.DEF_DISASSEMBLE_RATE_1, "DEF_DISASSEMBLE_RATE_1"},
            { eItemConstDef.DEF_DISASSEMBLE_RATE_2, "DEF_DISASSEMBLE_RATE_2"},
            { eItemConstDef.DEF_SMELTING_RATE_1, "DEF_SMELTING_RATE_1"},
            { eItemConstDef.DEF_SMELTING_RATE_2, "DEF_SMELTING_RATE_2"},
            { eItemConstDef.DEF_SMELTING_PRICE, "DEF_SMELTING_PRICE"},
            { eItemConstDef.DEF_WPNINCHANT_PROBUP_ID, "DEF_WPNINCHANT_PROBUP_ID"},
            { eItemConstDef.DEF_WPNINCHANT_PROBUP_VALUE, "DEF_WPNINCHANT_PROBUP_VALUE"},
            { eItemConstDef.DEF_RUBYTRADE_THREAD, "DEF_RUBYTRADE_THREAD"},
            { eItemConstDef.DEF_RUBYRETRY_MOD, "DEF_RUBYRETRY_MOD"},
            { eItemConstDef.DEF_RECOVERY_ITEMID_METAL, "DEF_RECOVERY_ITEMID_METAL"},
            { eItemConstDef.DEF_RECOVERY_ITEMID_THREAD, "DEF_RECOVERY_ITEMID_THREAD"},
            { eItemConstDef.DEF_RECOVERY_ITEMID_LVUP_STONE, "DEF_RECOVERY_ITEMID_LVUP_STONE"},
            { eItemConstDef.DEF_RECOVERY_ITEMID_TALISMAN, "DEF_RECOVERY_ITEMID_TALISMAN"},
            { eItemConstDef.DEF_RECOVERY_ITEMID_HP_STONE, "DEF_RECOVERY_ITEMID_HP_STONE"},
            { eItemConstDef.DEF_RECOVERY_ITEMID_AP_STONE, "DEF_RECOVERY_ITEMID_AP_STONE"},
            { eItemConstDef.DEF_RECOVERY_ITEMID_DFP_STONE, "DEF_RECOVERY_ITEMID_DFP_STONE"},
            { eItemConstDef.DEF_GOLD_OPTIONCHANGEALL_EACHTIER, "DEF_GOLD_OPTIONCHANGEALL_EACHTIER"},
            { eItemConstDef.DEF_ARMORUI_ITEM_SLOT_MAX, "DEF_ARMORUI_ITEM_SLOT_MAX"},
            { eItemConstDef.DEF_WEAPONUI_ITEM_SLOT_MAX, "DEF_WEAPONUI_ITEM_SLOT_MAX"},
            { eItemConstDef.DEF_ITEM_MAX_ENCHANT_CNT, "DEF_ITEM_MAX_ENCHANT_CNT"},
            { eItemConstDef.DEF_DISASSEMBLE_LEVEL, "DEF_DISASSEMBLE_LEVEL"},
            { eItemConstDef.DEF_SMELTING_LEVEL1, "DEF_SMELTING_LEVEL1"},
            { eItemConstDef.DEF_SMELTING_LEVEL2, "DEF_SMELTING_LEVEL2"},
        };

        public static readonly Dictionary<byte, string> Item_Const_Def_Option_Grade_Weight = new Dictionary<byte, string>()
        {
            { 1,    Item_Const_Def_Key_List[eItemConstDef.DEF_OPTIONWEIGT_WHITE] },
            { 2,    Item_Const_Def_Key_List[eItemConstDef.DEF_OPTIONWEIGT_GREEN] },
            { 3,    Item_Const_Def_Key_List[eItemConstDef.DEF_OPTIONWEIGT_BLUE] },
            { 4,    Item_Const_Def_Key_List[eItemConstDef.DEF_OPTIONWEIGT_PURPLE] },
            { 5,    Item_Const_Def_Key_List[eItemConstDef.DEF_OPTIONWEIGT_YELLOW] },
        };

        public enum eItemParamType
        {
            ParameterType_AP,
            ParameterType_CP,
            ParameterType_CPR,
            ParameterType_CR,
            ParameterType_DCR,
            ParameterType_DFP,
            ParameterType_HP,
            ParameterType_RATE_AP,
            ParameterType_RATE_DFP,
            ParameterType_RATE_HP,
            ParameterType_RATE_MP,
            ParameterType_AP_Min,
            ParameterType_AP_Max,
        };


        public static readonly Dictionary<string, eItemParamType> Item_Param_Enum_List = new Dictionary<string, eItemParamType>()
        {
            { "ParameterType_AP",eItemParamType.ParameterType_AP },
            { "ParameterType_CP",eItemParamType.ParameterType_CP },
            { "ParameterType_CPR",eItemParamType.ParameterType_CPR },
            { "ParameterType_CR",eItemParamType.ParameterType_CR },
            { "ParameterType_DCR",eItemParamType.ParameterType_DCR },
            { "ParameterType_DFP",eItemParamType.ParameterType_DFP },
            { "ParameterType_HP",eItemParamType.ParameterType_HP },
            { "ParameterType_RATE_AP",eItemParamType.ParameterType_RATE_AP },
            { "ParameterType_RATE_DFP",eItemParamType.ParameterType_RATE_DFP },
            { "ParameterType_RATE_HP",eItemParamType.ParameterType_RATE_HP },
            { "ParameterType_RATE_MP",eItemParamType.ParameterType_RATE_MP },
            { "ParameterType_AP_Min",eItemParamType.ParameterType_AP_Min },
            { "ParameterType_AP_Max",eItemParamType.ParameterType_AP_Max },

        };

        public static readonly Dictionary<eItemParamType, string> Item_Param_Key_List = new Dictionary<eItemParamType, string>()
        {
            { eItemParamType.ParameterType_AP, "ParameterType_AP" },
            { eItemParamType.ParameterType_CP, "ParameterType_CP" },
            { eItemParamType.ParameterType_CPR, "ParameterType_CPR" },
            { eItemParamType.ParameterType_CR, "ParameterType_CR" },
            { eItemParamType.ParameterType_DCR, "ParameterType_DCR" },
            { eItemParamType.ParameterType_DFP, "ParameterType_DFP" },
            { eItemParamType.ParameterType_HP, "ParameterType_HP" },
            { eItemParamType.ParameterType_RATE_AP, "ParameterType_RATE_AP" },
            { eItemParamType.ParameterType_RATE_DFP, "ParameterType_RATE_DFP" },
            { eItemParamType.ParameterType_RATE_HP, "ParameterType_RATE_HP" },
            { eItemParamType.ParameterType_RATE_MP, "ParameterType_RATE_MP" },
            { eItemParamType.ParameterType_AP_Min, "ParameterType_AP_Min" },
            { eItemParamType.ParameterType_AP_Max, "ParameterType_AP_Max" },
        };

        public static readonly Dictionary<eItemParamType, eItemRefiningType> Item_Rifining_Check_List = new Dictionary<eItemParamType,eItemRefiningType>()
        {
            { eItemParamType.ParameterType_AP, eItemRefiningType.RefiningType_AP },
            { eItemParamType.ParameterType_HP, eItemRefiningType.RefiningType_HP },
            { eItemParamType.ParameterType_DFP, eItemRefiningType.RefiningType_DFP },
        };

        public enum eItemRefiningType
        {
            RefiningType_HP,
            RefiningType_AP,
            RefiningType_DFP,
        }

        public static readonly Dictionary<eItemRefiningType, string> Item_Rifining_ParamKey_List = new Dictionary<eItemRefiningType, string>()
        {
            { eItemRefiningType.RefiningType_HP, Item_Param_Key_List[eItemParamType.ParameterType_HP]  },
            { eItemRefiningType.RefiningType_AP, Item_Param_Key_List[eItemParamType.ParameterType_AP]  },
            { eItemRefiningType.RefiningType_DFP, Item_Param_Key_List[eItemParamType.ParameterType_DFP]  },
        };

        public static readonly Dictionary<eItemRefiningType, string> Item_Rifining_Key_List = new Dictionary<eItemRefiningType, string>()
        {
            { eItemRefiningType.RefiningType_HP, "RefiningType_HP" },
            { eItemRefiningType.RefiningType_AP, "RefiningType_AP" },
            { eItemRefiningType.RefiningType_DFP, "RefiningType_DFP" },
        };

        //public Dictionary<int, float> Item_Grade_Diff_Rate = new Dictionary<int, float>()
        //{
        //    { -3,   0.4f },
        //    { -2,   0.6f },
        //    { -1,   0.8f },
        //    { 0,    1.0f },
        //    { 1,    1.2f },
        //    { 2,    1.4f },
        //    { 3,    1.6f },
        //};
    }
}
