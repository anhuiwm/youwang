using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSoul.DataManager
{
    public static partial class Character_Define
    {
        public const string CharacterDBName = "sharding";
        public const string CharacterTableName = "Character";
        public const string CharacterGroupTable = "User_CharacterGroup";
        public const string CharacterPrefix = "Character";
        public const string Character_Stat_TableName = "User_Character_Stat";
        public const string Character_TotalWarPoint_TableName = "User_WarPoint";

        public const short Max_CharacterGroup = 3;
        public const short Max_CharacterLevel = 90;

        public const short Minimum_Attack = 100;

        public enum SystemClassType
        {
            Class_None = 0,
            Class_Warrior = 1,
            Class_Swordmaster = 2,
            Class_Taoist = 3,
            Class_First = Class_Warrior,
            Class_Last = Class_Taoist,
        };

        public static Dictionary<string, SystemClassType> ClassTypeToEnum = new Dictionary<string, SystemClassType>()
        {
            { "Warrior", SystemClassType.Class_Warrior },
            { "Swordmaster", SystemClassType.Class_Swordmaster },
            { "Taoist", SystemClassType.Class_Taoist },
            { "All", SystemClassType.Class_None },
        };

        public static Dictionary<SystemClassType, string> ClassEnumToType = new Dictionary<SystemClassType, string>()
        {
            { SystemClassType.Class_Warrior, "Warrior" },
            { SystemClassType.Class_Swordmaster, "Swordmaster" },
            { SystemClassType.Class_Taoist, "Taoist" },
        };

        public enum eCharacterConstDef
        {
            DEF_ENERGY_PVE_INIT_VALUE,
            DEF_ENERGY_PVE_INC_LEVEL,
            DEF_ENERGY_PVE_INC_VALUE,
            DEF_ENERGY_PVE_TIME_PERIOD,
            DEF_ENERGY_PVE_TIME_INC_VALUE,
            DEF_ENERGY_PVE_LEVELUP_CHARGE_INIT_VALUE,
            DEF_ENERGY_PVE_LEVELUP_CHARGE_PERIOD_VALUE,
            DEF_ENERGY_PVE_LEVELUP_CHARGE_INC_VALUE,
            DEF_ENERGY_PVP_INIT_VALUE,
            DEF_ENERGY_PVP_INC_LEVEL,
            DEF_ENERGY_PVP_INC_VALUE,
            DEF_ENERGY_PVP_TIME_PERIOD,
            DEF_ENERGY_PVP_TIME_INC_VALUE,
            DEF_ENERGY_PVP_LEVELUP_CHARGE_INIT_VALUE,
            DEF_ENERGY_PVP_LEVELUP_CHARGE_PERIOD_VALUE,
            DEF_ENERGY_PVP_LEVELUP_CHARGE_INC_VALUE,
        }

        public static readonly Dictionary<eCharacterConstDef, string> Character_Const_Def_Key_List = new Dictionary<eCharacterConstDef, string>()
        {
            { eCharacterConstDef.DEF_ENERGY_PVE_INIT_VALUE, "DEF_ENERGY_PVE_INIT_VALUE"},
            { eCharacterConstDef.DEF_ENERGY_PVE_INC_LEVEL, "DEF_ENERGY_PVE_INC_LEVEL"},
            { eCharacterConstDef.DEF_ENERGY_PVE_INC_VALUE, "DEF_ENERGY_PVE_INC_VALUE"},
            { eCharacterConstDef.DEF_ENERGY_PVE_TIME_PERIOD, "DEF_ENERGY_PVE_TIME_PERIOD"},
            { eCharacterConstDef.DEF_ENERGY_PVE_TIME_INC_VALUE, "DEF_ENERGY_PVE_TIME_INC_VALUE"},
            { eCharacterConstDef.DEF_ENERGY_PVE_LEVELUP_CHARGE_INIT_VALUE, "DEF_ENERGY_PVE_LEVELUP_CHARGE_INIT_VALUE"},
            { eCharacterConstDef.DEF_ENERGY_PVE_LEVELUP_CHARGE_PERIOD_VALUE, "DEF_ENERGY_PVE_LEVELUP_CHARGE_PERIOD_VALUE"},
            { eCharacterConstDef.DEF_ENERGY_PVE_LEVELUP_CHARGE_INC_VALUE, "DEF_ENERGY_PVE_LEVELUP_CHARGE_INC_VALUE"},
            { eCharacterConstDef.DEF_ENERGY_PVP_INIT_VALUE, "DEF_ENERGY_PVP_INIT_VALUE"},
            { eCharacterConstDef.DEF_ENERGY_PVP_INC_LEVEL, "DEF_ENERGY_PVP_INC_LEVEL"},
            { eCharacterConstDef.DEF_ENERGY_PVP_INC_VALUE, "DEF_ENERGY_PVP_INC_VALUE"},
            { eCharacterConstDef.DEF_ENERGY_PVP_TIME_PERIOD, "DEF_ENERGY_PVP_TIME_PERIOD"},
            { eCharacterConstDef.DEF_ENERGY_PVP_TIME_INC_VALUE, "DEF_ENERGY_PVP_TIME_INC_VALUE"},
            { eCharacterConstDef.DEF_ENERGY_PVP_LEVELUP_CHARGE_INIT_VALUE, "DEF_ENERGY_PVP_LEVELUP_CHARGE_INIT_VALUE"},
            { eCharacterConstDef.DEF_ENERGY_PVP_LEVELUP_CHARGE_PERIOD_VALUE, "DEF_ENERGY_PVP_LEVELUP_CHARGE_PERIOD_VALUE"},
            { eCharacterConstDef.DEF_ENERGY_PVP_LEVELUP_CHARGE_INC_VALUE, "DEF_ENERGY_PVP_LEVELUP_CHARGE_INC_VALUE"},
        };
    }
}
