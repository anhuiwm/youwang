using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSoul.DataManager
{
    public class Soul_Define
    {
        public const string Soul_InvenDB = "sharding";

        public const string Soul_System_Skill_Table = "System_Skill";
        public const string Soul_System_Skill_Animation_Table = "System_Skill_Animation";
        public const string Soul_System_Skill_Buff_Table = "System_Skill_Buff";
        public const string Soul_System_Skill_Buff_Effect_Table = "System_Skill_Buff_Effect";
        public const string Soul_System_Skill_Buff_Group_Table = "System_Skill_Buff_Group";
        public const string Soul_System_Skill_Hon_Effect_Table = "System_Skill_Hon_Effect";
        public const string Soul_System_Skill_Level_Table = "System_Skill_Level";
        public const string Soul_System_Skill_Option_Table = "System_Skill_Option";
        public const string Soul_System_Skill_Projectile_Table = "System_Skill_Projectile";
        public const string Soul_System_Soul_Active_Table = "System_Soul_Active";
        public const string Soul_System_Soul_Craft_Table = "System_Soul_Craft";
        public const string Soul_System_Soul_Equip_Table = "System_Soul_Equip";
        public const string Soul_System_Soul_Evolution_Table = "System_Soul_Evolution";
        public const string Soul_System_Soul_Grade_Rate_Table = "System_Soul_Grade_Rate";
        public const string Soul_System_Soul_Parts_Table = "System_Soul_Parts";
        public const string Soul_System_Soul_Passive_Table = "System_Soul_Passive";
        public const string Soul_System_Soul_Passive_Prob_Table = "System_Soul_Passive_Prob";
        public const string Soul_System_Soul_Skill_Group_Table = "System_Soul_Skill_Group";

        public const string User_Hack_Detect_Table = "User_HackDetect";

        public const string Admin_System_SoulGroup_Active_Table = "Admin_System_SoulGroup_Active";
        public const byte Admin_System_SoulGroup_Active_Show_Flag = 0;
        public const string User_ActiveSoul_Table = "User_ActiveSoul";
        public const string User_ActiveSoul_Equip_Table = "User_ActiveSoul_Equip";
        public const string User_PassiveSoul_Table = "User_PassiveSoul";
        public const string User_Soul_Make_Info_Table = "User_Soul_Make_Info";
        public const string User_Soul_Equip_Inven_Table = "User_Soul_Equip_Inven";
        public const string User_ActiveSoul_Special_Buff_Table = "User_ActiveSoul_Special_Buff";
        public const string User_Character_Equip_Soul_Table = "User_Character_Equip_Soul";

        public const string SystemSoul_Prefix = "SystemSoul";
        public const string User_Soul_Prefix = "UserSoul";
        public const string User_Soul_Equip_Prefix = "UserSoulEquip";

        public const int Soul_Base_Grade = 1;
        public const int Soul_Base_Level = 1;

        public const int Soul_Max_Level = 90;
        public const int Soul_Max_Grade = 10;
        public const int Soul_Max_Star = 5;

        public const int Soul_Base_StarLevel = 1;
        public const byte Equip_Soul_Type_Acitve = 1;
        public const byte Equip_Soul_Type_Passive = 2;

        public const byte Soul_Special_Buff_Need_Grade_1_CN = 3;
        public const byte Soul_Special_Buff_Need_Grade_1_KR = 2;
        public const byte Soul_Special_Buff_Need_Grade_2 = 5;
        public const byte Soul_Special_Buff_Need_Grade_3 = 8;

        public const float Soul_Skill_Passive_Max_Damage_Facter= 2.6f;

        public static readonly List<short> EquipSoulSlot = new List<short>()
        {
            1,2,3,4
        };

        public static readonly Dictionary<short, eSoulConstDef> ActiveEquipSlot_ConstDef = new Dictionary<short, eSoulConstDef>()
        {
            { 1, eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT1_EXPAND_LEVEL },
            { 2, eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT2_EXPAND_LEVEL },
            { 3, eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT3_EXPAND_LEVEL },
            { 4, eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT4_EXPAND_LEVEL },
        };
        public static readonly Dictionary<short, eSoulConstDef> PassiveEquipSlot_ConstDef = new Dictionary<short, eSoulConstDef>()
        {
            { 1, eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT1_EXPAND_LEVEL },
            { 2, eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT2_EXPAND_LEVEL },
            { 3, eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT3_EXPAND_LEVEL },
            { 4, eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT4_EXPAND_LEVEL },
        };

        public enum eSoulConstDef
        {
            DEF_PASSIVE_SOUL_COST_STONE,
            DEF_PASSIVE_SOUL_COST_RUBY,
            DEF_LIMIT_PASSIVE_SOUL_COST_RUBY,
            DEF_PASSIVE_SOUL_EQUIPSLOT1_EXPAND_LEVEL,
            DEF_PASSIVE_SOUL_EQUIPSLOT2_EXPAND_LEVEL,
            DEF_PASSIVE_SOUL_EQUIPSLOT3_EXPAND_LEVEL,
            DEF_PASSIVE_SOUL_EQUIPSLOT4_EXPAND_LEVEL,
            DEF_PASSIVE_SOUL_BASIC_STORAGE,
            DEF_ACTIVE_SOUL_EQUIPSLOT1_EXPAND_LEVEL,
            DEF_ACTIVE_SOUL_EQUIPSLOT2_EXPAND_LEVEL,
            DEF_ACTIVE_SOUL_EQUIPSLOT3_EXPAND_LEVEL,
            DEF_ACTIVE_SOUL_EQUIPSLOT4_EXPAND_LEVEL,
            DEF_ACTIVE_SOUL_SELECT_SPECIALBUFF_COST_RUBY,
            DEF_ACTIVE_SOUL_SELECT_SPECIALBUFF_MAGIC_COST_RUBY,
            DEF_ACTIVE_SOUL_SELECT_SPECIALBUFF_RARE_COST_RUBY,
            DEF_ACTIVE_SOUL_EQUIPSLOT1_EXPAND_COST,
            DEF_ACTIVE_SOUL_EQUIPSLOT2_EXPAND_COST,
            DEF_ACTIVE_SOUL_EQUIPSLOT3_EXPAND_COST,
            DEF_ACTIVE_SOUL_EQUIPSLOT4_EXPAND_COST,
            DEF_PASSIVE_SOUL_EQUIPSLOT1_EXPAND_COST,
            DEF_PASSIVE_SOUL_EQUIPSLOT2_EXPAND_COST,
            DEF_PASSIVE_SOUL_EQUIPSLOT3_EXPAND_COST,
            DEF_PASSIVE_SOUL_EQUIPSLOT4_EXPAND_COST,
            DEF_PASSIVE_SOUL_FIRST_GET_ID,
            DEF_PASSIVE_SOUL_SECOND_GET_ID,
        }

        public static readonly Dictionary<eSoulConstDef, string> Soul_Const_Def_Key_List = new Dictionary<eSoulConstDef, string>()
        {
            { eSoulConstDef.DEF_PASSIVE_SOUL_COST_STONE, "DEF_PASSIVE_SOUL_COST_STONE"},
            { eSoulConstDef.DEF_PASSIVE_SOUL_COST_RUBY, "DEF_PASSIVE_SOUL_COST_RUBY"},
            { eSoulConstDef.DEF_LIMIT_PASSIVE_SOUL_COST_RUBY, "DEF_LIMIT_PASSIVE_SOUL_COST_RUBY"},
            { eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT1_EXPAND_LEVEL, "DEF_PASSIVE_SOUL_EQUIPSLOT1_EXPAND_LEVEL"},
            { eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT2_EXPAND_LEVEL, "DEF_PASSIVE_SOUL_EQUIPSLOT2_EXPAND_LEVEL"},
            { eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT3_EXPAND_LEVEL, "DEF_PASSIVE_SOUL_EQUIPSLOT3_EXPAND_LEVEL"},
            { eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT4_EXPAND_LEVEL, "DEF_PASSIVE_SOUL_EQUIPSLOT4_EXPAND_LEVEL"},
            { eSoulConstDef.DEF_PASSIVE_SOUL_BASIC_STORAGE, "DEF_PASSIVE_SOUL_BASIC_STORAGE"},
            { eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT1_EXPAND_LEVEL, "DEF_ACTIVE_SOUL_EQUIPSLOT1_EXPAND_LEVEL"},
            { eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT2_EXPAND_LEVEL, "DEF_ACTIVE_SOUL_EQUIPSLOT2_EXPAND_LEVEL"},
            { eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT3_EXPAND_LEVEL, "DEF_ACTIVE_SOUL_EQUIPSLOT3_EXPAND_LEVEL"},
            { eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT4_EXPAND_LEVEL, "DEF_ACTIVE_SOUL_EQUIPSLOT4_EXPAND_LEVEL"},
            { eSoulConstDef.DEF_ACTIVE_SOUL_SELECT_SPECIALBUFF_COST_RUBY, "DEF_ACTIVE_SOUL_SELECT_SPECIALBUFF_COST_RUBY"},
            { eSoulConstDef.DEF_ACTIVE_SOUL_SELECT_SPECIALBUFF_MAGIC_COST_RUBY, "DEF_ACTIVE_SOUL_SELECT_SPECIALBUFF_MAGIC_COST_RUBY"},
            { eSoulConstDef.DEF_ACTIVE_SOUL_SELECT_SPECIALBUFF_RARE_COST_RUBY, "DEF_ACTIVE_SOUL_SELECT_SPECIALBUFF_RARE_COST_RUBY"},
            { eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT1_EXPAND_COST, "DEF_ACTIVE_SOUL_EQUIPSLOT1_EXPAND_COST"},
            { eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT2_EXPAND_COST, "DEF_ACTIVE_SOUL_EQUIPSLOT2_EXPAND_COST"},
            { eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT3_EXPAND_COST, "DEF_ACTIVE_SOUL_EQUIPSLOT3_EXPAND_COST"},
            { eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT4_EXPAND_COST, "DEF_ACTIVE_SOUL_EQUIPSLOT4_EXPAND_COST"},
            { eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT1_EXPAND_COST, "DEF_PASSIVE_SOUL_EQUIPSLOT1_EXPAND_COST"},
            { eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT2_EXPAND_COST, "DEF_PASSIVE_SOUL_EQUIPSLOT2_EXPAND_COST"},
            { eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT3_EXPAND_COST, "DEF_PASSIVE_SOUL_EQUIPSLOT3_EXPAND_COST"},
            { eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT4_EXPAND_COST, "DEF_PASSIVE_SOUL_EQUIPSLOT4_EXPAND_COST"},
            { eSoulConstDef.DEF_PASSIVE_SOUL_FIRST_GET_ID, "DEF_PASSIVE_SOUL_FIRST_GET_ID"},
            { eSoulConstDef.DEF_PASSIVE_SOUL_SECOND_GET_ID, "DEF_PASSIVE_SOUL_SECOND_GET_ID"},
        };

        public static readonly List<eSoulConstDef> SoulActiveSlot_Price_Key = new List<eSoulConstDef>()
        {
            eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT1_EXPAND_COST,       // skip 0 slot
            eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT1_EXPAND_COST,
            eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT2_EXPAND_COST,
            eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT3_EXPAND_COST,
            eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT4_EXPAND_COST,
        };

        public static readonly List<eSoulConstDef> SoulPassiveSlot_Price_Key = new List<eSoulConstDef>()
        {
            eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT1_EXPAND_COST,      // skip 0 slot
            eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT1_EXPAND_COST,
            eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT2_EXPAND_COST,
            eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT3_EXPAND_COST,
            eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT4_EXPAND_COST,
        };

        public enum eSoulReturnKeys
        {
            AllSoulList,
            ActiveSoulLIst,
            PassiveSoulList,
            HideActiveSoulLIst,

            EquipActiveSoulList,
            EquipPassiveSoulList,
            SoulEquipItemList,
            DeletedSoulEquip,

            ActiveSoulInfo,
            ActiveSoulInfoList,
            PassiveSoulInfo,
            SoulEquipInfo,

            PassiveRubyMakeCount,
            PassiveSoulGetExp,
            PassiveSoulUseExp,
            RetPassiveExp,

            RetActiveSlot,
            RetPassiveSlot,
        };

        public static readonly Dictionary<eSoulReturnKeys, string> Soul_Ret_KeyList = new Dictionary<eSoulReturnKeys, string>()
        {
            { eSoulReturnKeys.AllSoulList,           "soul_list"          },
            { eSoulReturnKeys.ActiveSoulLIst,           "active_soullist"          },
            { eSoulReturnKeys.HideActiveSoulLIst,           "hide_soullist"          },
            { eSoulReturnKeys.PassiveSoulList,           "passive_soullist"          },
            { eSoulReturnKeys.ActiveSoulInfo,           "active_soul_info"          },
            { eSoulReturnKeys.ActiveSoulInfoList,           "active_soul_info_list"          },
            { eSoulReturnKeys.PassiveSoulInfo,           "passive_soul_info"          },
            { eSoulReturnKeys.SoulEquipInfo,           "soul_equip_info"          },
            { eSoulReturnKeys.EquipActiveSoulList,           "equip_active_soul"          },
            { eSoulReturnKeys.EquipPassiveSoulList,           "equip_passive_soul"          },
            { eSoulReturnKeys.SoulEquipItemList,           "soul_equip_item_list"          },
            { eSoulReturnKeys.PassiveRubyMakeCount,           "ruby_make_count"          },
            { eSoulReturnKeys.DeletedSoulEquip,           "deleted_soul_equip_list"          },
            { eSoulReturnKeys.PassiveSoulGetExp,           "passive_soul_get_exp"          },
            { eSoulReturnKeys.PassiveSoulUseExp,           "passive_soul_use_exp"          },
            { eSoulReturnKeys.RetPassiveExp,           "ret_passive_exp"          },
            { eSoulReturnKeys.RetActiveSlot,           "ret_active_soul_slot"          },
            { eSoulReturnKeys.RetPassiveSlot,           "ret_passive_soul_slot"          },

        };

        public enum ePassiveSoulStates
        {
            Create = 1,
            Equip,
            Store,
            Deleted,
        };

        public static readonly Dictionary<ePassiveSoulStates, string> PassiveSoulStates = new Dictionary<ePassiveSoulStates, string>()
        {
            { ePassiveSoulStates.Create,   "C" },
            { ePassiveSoulStates.Equip,    "E" },
            { ePassiveSoulStates.Store,     "S" },
            { ePassiveSoulStates.Deleted,     "D" },
        };

        public static readonly Dictionary<string, ePassiveSoulStates> PassiveSoulStatesToEnum = new Dictionary<string, ePassiveSoulStates>()
        {
            { "C" , ePassiveSoulStates.Create},
            { "E" , ePassiveSoulStates.Equip },
            { "S" , ePassiveSoulStates.Store },
            { "D" , ePassiveSoulStates.Deleted },
        };
    }
}
