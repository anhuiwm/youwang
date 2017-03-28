using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager.DBClass;

namespace TheSoul.DataManager
{
    public static partial class SoulManager
    {
        // Get System Skill table Info
        public static System_Skill GetSoul_System_Skill(ref TxnBlock TB, long SkillID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Skill_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE SkillID = {1}", Soul_Define.Soul_System_Skill_Table, SkillID);
            System_Skill retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Skill>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, SkillID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Skill();
            return retObj;
        }

        public static System_Skill_Animation GetSoul_System_Skill_Animation(ref TxnBlock TB, long Skill_AnimationIndex, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Skill_Animation_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Skill_AnimationIndex = {1}", Soul_Define.Soul_System_Skill_Animation_Table, Skill_AnimationIndex);
            System_Skill_Animation retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Skill_Animation>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Skill_AnimationIndex.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Skill_Animation();
            return retObj;
        }

        public static System_Skill_Buff GetSoul_System_Skill_Buff(ref TxnBlock TB, long Skill_BuffID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Skill_Buff_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Skill_BuffID = {1}", Soul_Define.Soul_System_Skill_Buff_Table, Skill_BuffID);
            System_Skill_Buff retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Skill_Buff>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Skill_BuffID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Skill_Buff();
            return retObj;
        }

        public static System_Skill_Buff_Effect GetSoul_System_Skill_Buff_Effect(ref TxnBlock TB, long Skill_Buff_EffectID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Skill_Buff_Effect_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Skill_Buff_EffectID = {1}", Soul_Define.Soul_System_Skill_Buff_Effect_Table, Skill_Buff_EffectID);
            System_Skill_Buff_Effect retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Skill_Buff_Effect>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Skill_Buff_EffectID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Skill_Buff_Effect();
            return retObj;
        }

        public static System_Skill_Buff_Group GetSoul_System_Skill_Buff_Group(ref TxnBlock TB, long Skill_Buff_GroupIndex, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Skill_Buff_Group_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Skill_Buff_GroupIndex = {1}", Soul_Define.Soul_System_Skill_Buff_Group_Table, Skill_Buff_GroupIndex);
            System_Skill_Buff_Group retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Skill_Buff_Group>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Skill_Buff_GroupIndex.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Skill_Buff_Group();
            return retObj;
        }

        public static System_Skill_Hon_Effect GetSoul_System_Skill_Hon_Effect(ref TxnBlock TB, long Hon_Effect_ID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Skill_Hon_Effect_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Hon_Effect_ID = {1}", Soul_Define.Soul_System_Skill_Hon_Effect_Table, Hon_Effect_ID);
            System_Skill_Hon_Effect retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Skill_Hon_Effect>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Hon_Effect_ID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Skill_Hon_Effect();
            return retObj;
        }

        public static System_Skill_Level GetSoul_System_Skill_Level(ref TxnBlock TB, long Skill_Level_Index, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Skill_Level_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Skill_Level_Index = {1}", Soul_Define.Soul_System_Skill_Level_Table, Skill_Level_Index);
            System_Skill_Level retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Skill_Level>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Skill_Level_Index.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Skill_Level();
            return retObj;
        }

        public static System_Skill_Level GetSoul_System_Skill_Level(ref TxnBlock TB, long SkillLevelGroup, byte SkillLevel, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Skill_Level_Table);
            string dict_key = string.Format("{0}_{1}", SkillLevelGroup, SkillLevel);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE SkillLevelGroup = {1} AND SkillLevel = {2}", Soul_Define.Soul_System_Skill_Level_Table, SkillLevelGroup, SkillLevel);
            System_Skill_Level retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Skill_Level>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, dict_key, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Skill_Level();
            return retObj;
        }

        public static System_Skill_Option GetSoul_System_Skill_Option(ref TxnBlock TB, long Skill_OptionIndex, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Skill_Option_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Skill_OptionIndex = {1}", Soul_Define.Soul_System_Skill_Option_Table, Skill_OptionIndex);
            System_Skill_Option retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Skill_Option>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Skill_OptionIndex.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Skill_Option();
            return retObj;
        }

        public static System_Skill_Projectile GetSoul_System_Skill_Projectile(ref TxnBlock TB, long ProjectileID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Skill_Projectile_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE ProjectileID = {1}", Soul_Define.Soul_System_Skill_Projectile_Table, ProjectileID);
            System_Skill_Projectile retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Skill_Projectile>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, ProjectileID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Skill_Projectile();
            return retObj;
        }


        public static void RemoveAdmin_System_Soul_ActiveList()
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Admin_System_SoulGroup_Active_Table);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }

        public static List<Admin_System_SoulGroup_Active> GetAdmin_System_Soul_ActiveList(ref TxnBlock TB, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Admin_System_SoulGroup_Active_Table);
            string setQuery = string.Format("SELECT SoulGroup as soulgroup, hide FROM {0} WITH(NOLOCK) WHERE hide = 1", Soul_Define.Admin_System_SoulGroup_Active_Table);
            List<Admin_System_SoulGroup_Active> retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow<Admin_System_SoulGroup_Active>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
            return retObj;
        }

        public static System_Soul_Active GetSoul_System_Soul_Active(ref TxnBlock TB, long SoulID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Soul_Active_Table);
            string setQuery = string.Format(@"SELECT * FROM ( 
                                                SELECT SA_SOUL.*, ISNULL(SA_Admin.hide, 0) as hide FROM {0}  AS SA_SOUL WITH(NOLOCK) 
                                                    LEFT OUTER JOIN  
                                                    {1} AS SA_Admin WITH(NOLOCK) 
                                                    ON SA_SOUL.SoulGroup = SA_Admin.SoulGroup
                                                WHERE SA_SOUL.SoulID = {2}
                                                ) as ResultTable WHERE ResultTable.hide = 0
                                                    ", Soul_Define.Soul_System_Soul_Active_Table
                                                     , Soul_Define.Admin_System_SoulGroup_Active_Table
                                                     , SoulID
                                                     , Soul_Define.Admin_System_SoulGroup_Active_Show_Flag
                                                     );
            System_Soul_Active retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Soul_Active>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, SoulID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Soul_Active();
            return retObj;
        }

        public static System_Soul_Active GetSoul_System_Soul_Active(ref TxnBlock TB, long SoulGroup, int Grade, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Soul_Active_Table);
            string dict_key = string.Format("{0}_{1}", SoulGroup, Grade);
            string setQuery = string.Format(@"SELECT * FROM ( 
                                                SELECT SA_SOUL.*, ISNULL(SA_Admin.hide, 0) as hide FROM {0}  AS SA_SOUL WITH(NOLOCK) 
                                                    LEFT OUTER JOIN  
                                                    {1} AS SA_Admin WITH(NOLOCK) 
                                                    ON SA_SOUL.SoulGroup = SA_Admin.SoulGroup
                                                WHERE SA_SOUL.SoulGroup = {2} AND SA_SOUL.Grade = {3}
                                                ) as ResultTable WHERE ResultTable.hide = 0
                                                    ", Soul_Define.Soul_System_Soul_Active_Table
                                                     , Soul_Define.Admin_System_SoulGroup_Active_Table
                                                     , SoulGroup
                                                     , Grade
                                                     , Soul_Define.Admin_System_SoulGroup_Active_Show_Flag
                                                     );
            System_Soul_Active retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Soul_Active>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, dict_key, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Soul_Active();
            return retObj;
        }

        public static System_Soul_Craft GetSoul_System_Soul_Craft(ref TxnBlock TB, long Crafted_Item_Index, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Soul_Craft_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Crafted_Item_Index = {1}", Soul_Define.Soul_System_Soul_Craft_Table, Crafted_Item_Index);
            System_Soul_Craft retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Soul_Craft>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Crafted_Item_Index.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Soul_Craft();
            return retObj;
        }

        public static System_Soul_Equip GetSoul_System_Soul_Equip(ref TxnBlock TB, long Soul_Equip_ID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Soul_Equip_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Soul_Equip_ID = {1}", Soul_Define.Soul_System_Soul_Equip_Table, Soul_Equip_ID);
            System_Soul_Equip retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Soul_Equip>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Soul_Equip_ID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Soul_Equip();
            return retObj;
        }

        public static System_Soul_Evolution GetSoul_System_Soul_Evolution(ref TxnBlock TB, long Soul_Evolution_ID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Soul_Evolution_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Soul_Evolution_ID = {1}", Soul_Define.Soul_System_Soul_Evolution_Table, Soul_Evolution_ID);
            System_Soul_Evolution retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Soul_Evolution>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Soul_Evolution_ID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Soul_Evolution();
            return retObj;
        }

        public static System_Soul_Evolution GetSoul_System_Soul_Evolution(ref TxnBlock TB, long Group_ID, byte Soul_Star_Level, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Soul_Evolution_Table);
            string dict_key = string.Format("{0}_{1}", Group_ID, Soul_Star_Level);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Group_ID = {1} AND Soul_Star_Level = {2} ", Soul_Define.Soul_System_Soul_Evolution_Table, Group_ID, Soul_Star_Level);
            System_Soul_Evolution retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Soul_Evolution>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, dict_key, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Soul_Evolution();
            return retObj;
        }

        public static System_Soul_Parts GetSoul_System_Soul_Parts(ref TxnBlock TB, long SoulPartsIndex, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Soul_Parts_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE SoulPartsIndex = {1}", Soul_Define.Soul_System_Soul_Parts_Table, SoulPartsIndex);
            System_Soul_Parts retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Soul_Parts>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, SoulPartsIndex.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Soul_Parts();
            return retObj;
        }

        public static System_Soul_Parts GetSoul_System_Soul_Parts_By_Soul_Group(ref TxnBlock TB, long Soul_Group, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}_Group", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Soul_Parts_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Soul_Group = {1}", Soul_Define.Soul_System_Soul_Parts_Table, Soul_Group);
            System_Soul_Parts retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Soul_Parts>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Soul_Group.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Soul_Parts();
            return retObj;
        }

        public static System_Soul_Passive GetSoul_System_Soul_Passive(ref TxnBlock TB, long Soul_PassiveID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Soul_Passive_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Soul_PassiveID = {1}", Soul_Define.Soul_System_Soul_Passive_Table, Soul_PassiveID);
            System_Soul_Passive retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Soul_Passive>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Soul_PassiveID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Soul_Passive();
            return retObj;
        }

        public static System_Soul_Passive GetSoul_System_Soul_Passive(ref TxnBlock TB, int Grade, long GroupIndex, int Level, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Soul_Passive_Table);
            string dict_key = string.Format("{0}_{1}_{2}", Grade, GroupIndex, Level);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Grade = {1} AND GroupIndex = {2} AND Level = {3}", Soul_Define.Soul_System_Soul_Passive_Table, Grade, GroupIndex, Level);
            System_Soul_Passive retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Soul_Passive>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, dict_key, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Soul_Passive();
            return retObj;
        }

        public static System_Soul_Passive_Prob GetSoul_System_Soul_Passive_Prob(ref TxnBlock TB, long Soul_Passive_ProbID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Soul_Passive_Prob_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Soul_Passive_ProbID = {1}", Soul_Define.Soul_System_Soul_Passive_Prob_Table, Soul_Passive_ProbID);
            System_Soul_Passive_Prob retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Soul_Passive_Prob>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Soul_Passive_ProbID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Soul_Passive_Prob();
            return retObj;
        }

        public static List<System_Soul_Passive_Prob> GetSoul_System_Soul_Passive_Prob_All(ref TxnBlock TB, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}_All", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Soul_Passive_Prob_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) ", Soul_Define.Soul_System_Soul_Passive_Prob_Table);
            return TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow<System_Soul_Passive_Prob>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
        }

        public static System_Soul_Skill_Group GetSoul_System_Soul_Skill_Group(ref TxnBlock TB, long BuffID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Soul_Skill_Group_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE BuffID = {1}", Soul_Define.Soul_System_Soul_Skill_Group_Table, BuffID);
            System_Soul_Skill_Group retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Soul_Skill_Group>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, BuffID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Soul_Skill_Group();
            return retObj;
        }

        public static List<System_Soul_Skill_Group> GetSoul_System_Soul_Skill_Group(ref TxnBlock TB, long GroupID, byte Star_Level, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Soul_Skill_Group_Table);
            string dict_key = string.Format("{0}_{1}", GroupID, Star_Level);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE GroupID = {1} AND Star_Level = {2}", Soul_Define.Soul_System_Soul_Skill_Group_Table, GroupID, Star_Level);
            return TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow_Hash<System_Soul_Skill_Group>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, dict_key, setQuery, dbkey, Flush);
        }

        public static System_Soul_Skill_Group GetSoul_System_Soul_Skill_Group(ref TxnBlock TB, long GroupID, byte Star_Level, long Group, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Soul_Define.SystemSoul_Prefix, Soul_Define.Soul_System_Soul_Skill_Group_Table);
            string dict_key = string.Format("{0}_{1}_{2}", GroupID, Star_Level, Group);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE GroupID = {1} AND Star_Level = {2} AND [Group] = {3}", Soul_Define.Soul_System_Soul_Skill_Group_Table, GroupID, Star_Level, Group);
            System_Soul_Skill_Group retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Soul_Skill_Group>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, dict_key, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Soul_Skill_Group();
            return retObj;
        }

        public static List<User_ActiveSoul> GetUser_ActiveSoul(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}_{2}", Soul_Define.SystemSoul_Prefix, Soul_Define.User_ActiveSoul_Table, AID);
            string setQuery = string.Format(@"SELECT * FROM ( 
                                                SELECT User_SA.*, ISNULL(SA_Admin.hide, 0) as hide FROM {0} AS User_SA WITH(NOLOCK) 
                                                    LEFT OUTER JOIN
                                                    {1} AS SA_Admin WITH(NOLOCK) 
                                                    ON User_SA.soulgroup = SA_Admin.SoulGroup
                                                    WHERE aid = {2}
                                                ) as ResultTable WHERE ResultTable.hide = 0
                                                "
                                    , Soul_Define.User_ActiveSoul_Table
                                    , Soul_Define.Admin_System_SoulGroup_Active_Table
                                    , AID
                                    , Soul_Define.Admin_System_SoulGroup_Active_Show_Flag
                                    );
            return TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow<User_ActiveSoul>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);            
        }

//        public static List<User_ActiveSoul> GetUser_ActiveSoul(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
//        {
//            string setKey = string.Format("{0}_{1}_{2}", Soul_Define.SystemSoul_Prefix, Soul_Define.User_ActiveSoul_Table, AID);
//            string setQuery = string.Format(@"SELECT UA.*, ISNULL(AdminSoul.hide, 0) as hide FROM {0} AS UA WITH(NOLOCK)  
//                                                        {1} AS AdminSoul WITH(NOLOCK)
//                                                        ON UA.soulgroup = AdminSoul.SoulGroup
//                                                        WHERE aid = {2}"
//                , Soul_Define.User_ActiveSoul_Table, Soul_Define.Admin_System_SoulGroup_Active_Table, AID);
//            return TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow<User_ActiveSoul>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
//        }

        public static List<User_ActiveSoul> GetUser_ActiveSoul_WithEquip(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            List<User_ActiveSoul> getActiveSoulList = SoulManager.GetUser_ActiveSoul(ref TB, AID, Flush);
            List<User_ActiveSoul_Equip> getSoulEquipList = SoulManager.GetUser_ActiveSoul_Equip(ref TB, AID, Flush);

            getActiveSoulList.ForEach(soul =>
                {
                    soul.soulequiplist = getSoulEquipList.FindAll(equip => soul.soulseq == equip.soulseq);
                });
            return getActiveSoulList;
        }

        public static void RemoveCacheUser_ActiveSoul(long AID)
        {
            string setKey = string.Format("{0}_{1}_{2}", Soul_Define.SystemSoul_Prefix, Soul_Define.User_ActiveSoul_Table, AID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }

        public static List<User_ActiveSoul_Equip> GetUser_ActiveSoul_Equip(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}_{2}", Soul_Define.SystemSoul_Prefix, Soul_Define.User_ActiveSoul_Equip_Table, AID);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK, INDEX(IDX_User_ActiveSoul_Equip))  WHERE aid = {1} AND delflag = 'N'", Soul_Define.User_ActiveSoul_Equip_Table, AID);
            return TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow<User_ActiveSoul_Equip>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
        }

        public static void RemoveCacheUser_ActiveSoul_Equip(long AID)
        {
            string setKey = string.Format("{0}_{1}_{2}", Soul_Define.SystemSoul_Prefix, Soul_Define.User_ActiveSoul_Equip_Table, AID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }

        public static List<User_PassiveSoul> GetUser_PassiveSoul(ref TxnBlock TB, long AID, long CID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}_{2}_{3}", Soul_Define.SystemSoul_Prefix, Soul_Define.User_PassiveSoul_Table, AID, CID);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND cid = {2} AND delflag = 'N'", Soul_Define.User_PassiveSoul_Table, AID, CID);
            return TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow<User_PassiveSoul>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
        }

        public static void RemoveCacheUser_PassiveSoul(long AID, long CID)
        {
            string setKey = string.Format("{0}_{1}_{2}_{3}", Soul_Define.SystemSoul_Prefix, Soul_Define.User_PassiveSoul_Table, AID, CID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }

        // check passive soul make count
        public static User_Soul_Make_Info GetUserSoulMakeInfo(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}", Soul_Define.User_Soul_Prefix, Soul_Define.User_Soul_Make_Info_Table, AID);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE AID = {1}", Soul_Define.User_Soul_Make_Info_Table, AID);

            User_Soul_Make_Info retObj = GenericFetch.FetchFromRedis<User_Soul_Make_Info>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
            if (retObj == null)
            {
                setQuery = string.Format(@"INSERT INTO {0} (AID, Total_Passive_Make_Count, Passive_Make_Count, Passive_Make_regdate)
										        VALUES ({1}, 0, 0, DATEADD(DAY, -1,GETDATE()))", Soul_Define.User_Soul_Make_Info_Table, AID);
                TB.ExcuteSqlCommand(dbkey, setQuery);
                retObj = new User_Soul_Make_Info();
                retObj.AID = AID;
                retObj.Passive_Make_regdate = DateTime.Now.AddDays(-1);
            }

            DateTime curDate = DateTime.Parse(retObj.Passive_Make_regdate.ToShortDateString());
            DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            TimeSpan TS = dbDate - curDate;

            if (TS.Days != 0)
                retObj.Passive_Make_Count = 0;

            return retObj;
        }

        public static List<User_Soul_Equip_Inven> GetUser_Soul_Equip_Inven(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}_{2}", Soul_Define.SystemSoul_Prefix, Soul_Define.User_Soul_Equip_Inven_Table, AID);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND delflag = 'N'", Soul_Define.User_Soul_Equip_Inven_Table, AID);
            return TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow<User_Soul_Equip_Inven>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
        }

        public static void RemoveCacheUser_Soul_Equip_Inven(long AID)
        {
            string setKey = string.Format("{0}_{1}_{2}", Soul_Define.SystemSoul_Prefix, Soul_Define.User_Soul_Equip_Inven_Table, AID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }

        public static List<User_Character_Equip_Soul> GetUser_Character_Equip_Soul(ref TxnBlock TB, long AID, long CID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}_{2}_{3}", Soul_Define.SystemSoul_Prefix, Soul_Define.User_Character_Equip_Soul_Table, AID, CID);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND cid = {2} AND soulseq > 0 and slot_num > 0", Soul_Define.User_Character_Equip_Soul_Table, AID, CID);
            return TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow<User_Character_Equip_Soul>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);            
        }

        public static void RemoveCacheUser_Equip_Soul(long AID, long CID)
        {
            string setKey = string.Format("{0}_{1}_{2}_{3}", Soul_Define.SystemSoul_Prefix, Soul_Define.User_Character_Equip_Soul_Table, AID, CID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }


        public static List<User_ActiveSoul_Special_Buff> GetUser_ActiveSoul_Special_Buff(ref TxnBlock TB, long AID, long CID, long soulseq, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            //string setKey = string.Format("{0}_{1}_{2}_{3}", Soul_Define.SystemSoul_Prefix, Soul_Define.User_ActiveSoul_Special_Buff_Table, AID, CID);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND cid = {2} AND soulseq = {3}", Soul_Define.User_ActiveSoul_Special_Buff_Table, AID, CID, soulseq);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_ActiveSoul_Special_Buff>(ref TB, setQuery, dbkey);
        }

        public static List<User_ActiveSoul_Special_Buff> GetUser_ActiveSoul_Special_Buff(ref TxnBlock TB, long soulseq, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            //string setKey = string.Format("{0}_{1}_{2}_{3}", Soul_Define.SystemSoul_Prefix, Soul_Define.User_ActiveSoul_Special_Buff_Table, AID, CID);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE soulseq = {1}", Soul_Define.User_ActiveSoul_Special_Buff_Table, soulseq);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_ActiveSoul_Special_Buff>(ref TB, setQuery, dbkey);
        }

        public static List<User_ActiveSoul_Special_Buff> GetUser_ActiveSoul_Special_Buff(ref TxnBlock TB, long AID, long CID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}_{2}_{3}", Soul_Define.SystemSoul_Prefix, Soul_Define.User_ActiveSoul_Special_Buff_Table, AID, CID);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND cid = {2}", Soul_Define.User_ActiveSoul_Special_Buff_Table, AID, CID);
            return TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow<User_ActiveSoul_Special_Buff>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
        }


        public static void RemoveCacheUser_ActiveSoul_Special_Buff(long AID, long CID)
        {
            string setKey = string.Format("{0}_{1}_{2}_{3}", Soul_Define.SystemSoul_Prefix, Soul_Define.User_ActiveSoul_Special_Buff_Table, AID, CID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }

        public static List<User_HackDetect> GetUserHackDetectList(ref TxnBlock TB, long AID, int checkCount = 100, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE total_detect_count >= {1}", Soul_Define.User_Hack_Detect_Table, checkCount);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_HackDetect>(ref TB, setQuery, dbkey);
        }

        public static Result_Define.eResult SetUserHackDetect(ref TxnBlock TB, long AID, int AddCount = 1, string dbkey = Soul_Define.Soul_InvenDB)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            string username = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_role]);
            if (string.IsNullOrEmpty(username))
                username = AccountManager.GetAccountData(ref TB, AID, ref retError).UserName;

            if (retError == Result_Define.eResult.SUCCESS)
            {
                string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON AID = @aid
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                    username = @username,
                                                    total_detect_count = total_detect_count + @addcount,
                                                    today_detect_count = CASE WHEN reg_date != CONVERT(VARCHAR(10),GETDATE(),21) THEN @addcount ELSE today_detect_count + @addcount END,
                                                    reg_date = GETDATE()
                                                WHEN NOT MATCHED THEN
                                                   INSERT ([aid], [username], [total_detect_count], [today_detect_count], [reg_date])
                                                   VALUES (@aid, @username, @addcount, @addcount, GETDATE());
                                    ", Soul_Define.User_Hack_Detect_Table);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = setQuery;
                cmd.Parameters.AddWithValue("@aid", AID);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@addcount", AddCount);
                retError = TB.ExcuteSqlCommand(dbkey, ref cmd) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }
            return retError;
        }

        public static Result_Define.eResult DeleteUserHackDetect(ref TxnBlock TB, long AID, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setQuery = string.Format("DELETE FROM {0} WHERE aid = {1} ", Soul_Define.User_Hack_Detect_Table, AID);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }
    }
}
