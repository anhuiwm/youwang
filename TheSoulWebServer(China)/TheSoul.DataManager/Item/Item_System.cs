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
    public static partial class ItemManager
    {
        // Get System Item table Info
        public static System_Item_Base GetSystem_Item_Base(ref TxnBlock TB, long ItemID, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Base_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Item_IndexID = {1}", Item_Define.Item_System_Item_Base_Table, ItemID);
            System_Item_Base retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Base>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, ItemID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Item_Base();
            return retObj;
        }

        public static List<System_Item_Base> GetSystem_Item_BaseList(ref TxnBlock TB, string itemClass, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  Where ItemClass = '{1}'", Item_Define.Item_System_Item_Base_Table, itemClass);
            List<System_Item_Base> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<System_Item_Base>(ref TB, setQuery, dbkey);
            if (retObj == null)
                retObj = new List<System_Item_Base>();
            return retObj;
        }

        public static System_Item_Equip GetSystem_Item_Equip(ref TxnBlock TB, long ItemID, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}_{2}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Base_Table, Item_Define.Item_System_Item_Equip_Table);
            string setQuery = string.Format(@"SELECT *
                                                  FROM
                                                   {0} AS A WITH(NOLOCK) 
                                                   INNER JOIN {1} AS B WITH(NOLOCK) 
                                                   ON A.Class_IndexID = B.EquipItem_IndexID 
                                                  WHERE A.Item_IndexID = {2}
                                                ", Item_Define.Item_System_Item_Base_Table, Item_Define.Item_System_Item_Equip_Table, ItemID);
            System_Item_Equip retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Equip>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, ItemID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Item_Equip();
            return retObj;
        }

        public static System_Item_Use GetSystem_Item_Use(ref TxnBlock TB, long ItemID, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}_{2}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Base_Table, Item_Define.Item_System_Item_Use_Table);
            string setQuery = string.Format(@"SELECT *
                                                  FROM
                                                   {0} AS A WITH(NOLOCK) 
                                                   INNER JOIN {1} AS B WITH(NOLOCK) 
                                                   ON A.Class_IndexID = B.UseItem_IndexID 
                                                  WHERE A.Item_IndexID = {2}
                                                ", Item_Define.Item_System_Item_Base_Table, Item_Define.Item_System_Item_Use_Table, ItemID);
            System_Item_Use retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Use>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, ItemID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Item_Use();
            return retObj;
        }


//        public static System_Item_Use GetSystem_Item_Use(ref TxnBlock TB, long ItemID, Character_Define.SystemClassType setClass, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
//        {
//            string setKey = string.Format("{0}_{1}_{2}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Base_Table, Item_Define.Item_System_Item_Use_Table);
//            string dict_key = string.Format("{0}_{1}", ItemID, setClass);
//            string setQuery = string.Format(@"SELECT * FROM {1} WHERE 
//                                                            TierGroup = (SELECT TierGroup FROM {0} AS A INNER JOIN {1} AS B ON A.Class_IndexID = B.UseItem_IndexID WHERE A.Item_IndexID = {2})
//                                                        AND TargetCondition = '{3}'
//                                            ", Item_Define.Item_System_Item_Base_Table, Item_Define.Item_System_Item_Use_Table, ItemID, Character_Define.ClassEnumToType[setClass]);
//            System_Item_Use retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Use>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, dict_key, setQuery, dbkey, Flush);
//            if (retObj == null)
//                retObj = new System_Item_Use();
//            return retObj;
//        }


        public static System_Item_Use GetSystem_Item_Use_TierGroup(ref TxnBlock TB, int TierGroup, Character_Define.SystemClassType setClass, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}_{2}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Base_Table, Item_Define.Item_System_Item_Use_Table);
            string dict_key = string.Format("{0}_{1}", TierGroup, Character_Define.ClassEnumToType[setClass]);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE TierGroup = {1} AND TargetCondition = '{2}'", Item_Define.Item_System_Item_Use_Table, TierGroup, Character_Define.ClassEnumToType[setClass]);
            System_Item_Use retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Use>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, dict_key, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Item_Use();
            return retObj;
        }


        public static System_Item_Info GetSystem_Item_Info(ref TxnBlock TB, long ItemID, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}_{2}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Base_Table, Item_Define.Item_System_Item_Info_Table);
            string setQuery = string.Format(@"SELECT *
                                                  FROM
                                                   {0} AS A WITH(NOLOCK) 
                                                   INNER JOIN {1} AS B WITH(NOLOCK) 
                                                   ON A.Class_IndexID = B.InfoItem_IndexID 
                                                  WHERE A.Item_IndexID = {2}
                                                ", Item_Define.Item_System_Item_Base_Table, Item_Define.Item_System_Item_Info_Table, ItemID);
            System_Item_Info retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Info>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, ItemID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Item_Info();
            return retObj;
        }

        public static System_Item_Costume GetSystem_Item_Costume(ref TxnBlock TB, long ItemID, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}_{2}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Base_Table, Item_Define.Item_System_Item_Costume_Table);
            string setQuery = string.Format(@"SELECT *
                                                  FROM
                                                   {0} AS A WITH(NOLOCK) 
                                                   INNER JOIN {1} AS B WITH(NOLOCK) 
                                                   ON A.Class_IndexID = B.CostumeItem_IndexID 
                                                  WHERE A.Item_IndexID = {2}
                                                ", Item_Define.Item_System_Item_Base_Table, Item_Define.Item_System_Item_Costume_Table, ItemID);
            System_Item_Costume retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Costume>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, ItemID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Item_Costume();
            return retObj;
        }

        // Get System Item Contents info table
        public static System_Item_Option GetSystem_Item_Option(ref TxnBlock TB, long Option_IndexID, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Option_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Option_IndexID = {1}", Item_Define.Item_System_Item_Option_Table, Option_IndexID);
            System_Item_Option retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Option>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Option_IndexID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Item_Option();
            return retObj;
        }

        const string OR_Condition = " OR ";
        const string HashKey_Seperator = "_";
        public static List<System_Item_Option> GetSystem_Item_Option_List(ref TxnBlock TB, List<long> Option_IndexIDs, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Option_Table);
            List<System_Item_Option> retObj = new List<System_Item_Option>();
            if (Option_IndexIDs.Count > 0)
            {
                StringBuilder sethashKey = new StringBuilder();
                StringBuilder query_condition = new StringBuilder();
                
                Option_IndexIDs.ForEach(setID =>
                {
                    if (setID > 0)
                    {
                        query_condition.Append(string.Format("Option_IndexID = {0}{1} ", setID, OR_Condition));
                        sethashKey.Append(string.Format("{0}{1}", setID, HashKey_Seperator));
                    }
                });

                if (query_condition.Length > 0)
                {
                    query_condition.Remove(query_condition.Length - OR_Condition.Length, OR_Condition.Length);
                    sethashKey.Remove(sethashKey.Length - HashKey_Seperator.Length, HashKey_Seperator.Length);
                }

                string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE {1}", Item_Define.Item_System_Item_Option_Table, query_condition.ToString());
                retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow_Hash<System_Item_Option>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, sethashKey.ToString(), setQuery, dbkey, Flush);
            }

            return retObj;
        }

        public static System_Item_Option PickSystemItemOption(ref TxnBlock TB, string OptionType, string ParamType, int Grade, int Tier, string dbkey = Item_Define.Item_InvenDB)
        {
            string dict_key = string.Format("{0}_{1}_{2}_{3}", OptionType, ParamType, Grade, Tier);
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Option_Table);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE ParameterType = '{1}' AND OptionValue_Grade = {2} AND OptionValue_Tier = {3}
                                                                    AND OptionType = '{4}' AND (OptionEquip = '{5}' OR OptionEquip = '{6}');"
                                                    , Item_Define.Item_System_Item_Option_Table, ParamType, Grade, Tier, Item_Define.OptionType_Random, Item_Define.OptionEquip_All, Item_Define.OptionTypeToOptionEquip[OptionType]);
            System_Item_Option optionInfo = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Option>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, dict_key, setQuery, dbkey);

            if (optionInfo == null)
                optionInfo = new System_Item_Option();
            return optionInfo;
        }

        public static System_Item_Option_UseInfo GetSystem_Item_Option_UseInfo(ref TxnBlock TB, long Option_Useinfo_IndexID, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Option_UseInfo_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Option_Useinfo_IndexID = {1}", Item_Define.Item_System_Item_Option_UseInfo_Table, Option_Useinfo_IndexID);
            System_Item_Option_UseInfo retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Option_UseInfo>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Option_Useinfo_IndexID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Item_Option_UseInfo();
            return retObj;
        }


        public static List<User_Inven_Option> PickSystemItemOptionUseInfo(ref TxnBlock TB, Item_Define.eItemType ItemType, int optionCount = 0, int grade = 0, int tier = 0, bool Flush = false, string dbkey = Item_Define.Item_InvenDB)
        {
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Option_UseInfo_Table);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE ItemType = '{1}' AND FixOption != 'FixOption_Use' AND UseAvailability = 'TRUE'", Item_Define.Item_System_Item_Option_UseInfo_Table, Item_Define.ItemTypeToOptionType[ItemType]);

            List<System_Item_Option_UseInfo> optionList = TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow_Hash<System_Item_Option_UseInfo>
                                                          (ref TB, DataManager_Define.RedisServerAlias_System, setKey, Item_Define.ItemTypeToOptionType[ItemType], setQuery, dbkey, Flush);

            var rnd = new Random();

            List<User_Inven_Option> setOptionList = new List<User_Inven_Option>();
            bool isAccessory = Item_Define.ItemAccessoryTypeList.Contains(ItemType);

            int tryCount = 0;

            if (optionList.Count > 0) 
            {
                for (int getRandomOptionCount = 0; getRandomOptionCount < optionCount && tryCount < Item_Define.ItemOptionMakeTryCount; getRandomOptionCount++)
                {
                    var shuffle_optionList = optionList.OrderBy(item => rnd.Next()).FirstOrDefault();
                    string OptionType = Item_Define.ItemTypeToOptionType[ItemType];
                    byte setGrade = (byte)grade;
                    if (isAccessory)
                        setGrade = Item_Define.AccessoryOptionGradeRate[grade][getRandomOptionCount];

                    System_Item_Option setOption = PickSystemItemOption(ref TB, OptionType, shuffle_optionList.ParameterType, setGrade, tier);

                    if (!isAccessory)
                        setGrade = 0;

                    if (setOption != null)
                    {
                        setOptionList.Add(MakeUserInvenOption(setOption, isAccessory, setGrade));
                    }
                    else
                    {
                        tryCount++;
                    }
                }
            }
            return setOptionList;
        }

        public static User_Inven_Option MakeUserInvenOption(System_Item_Option setOption, bool isAccessory, byte setGrade = 0)
        {
            User_Inven_Option retObj = new User_Inven_Option();
            int setOptionValue = 0;

            int min = 0;
            int max = 0;
            if (setOption.OptionType.Equals(Item_Define.Item_Option_Type_Random) && setOption.OptionValue2 > 0)
            {
                min = setOption.OptionValue1;
                max = setOption.OptionValue2;
            }
            else
            {
                min = max = setOption.OptionValue1 > 0 ? setOption.OptionValue1 :
                                            setOption.OptionValue2 > 0 ? setOption.OptionValue2 : 0;
            }

            //if (isAccessory)
            //{
            //    int totalRange = max - min;
            //    if (totalRange > 0)
            //    {
            //        min = min + System.Convert.ToInt32((totalRange * (setGrade * Item_Define.ItemOptionGradeGap)));
            //        max = min + System.Convert.ToInt32((totalRange * (setGrade * Item_Define.ItemOptionGradeGap)));
            //    }
            //}
            setOptionValue = TheSoul.DataManager.Math.GetRandomInt(min, max);
            retObj.optiontype = setOption.ParameterType;
            retObj.option_value = setOptionValue;
            retObj.option_grade = setGrade;
            retObj.delflag = "N";
            return retObj;
        }

        public static System_Item_TierInfo GetSystem_Item_TierInfo(ref TxnBlock TB, long Level, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_TierInfo_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Level = {1}", Item_Define.Item_System_Item_TierInfo_Table, Level);
            System_Item_TierInfo retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_TierInfo>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Level.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Item_TierInfo();
            return retObj;
        }

        public static System_Item_Enchant GetSystem_Item_Enchant(ref TxnBlock TB, long Enchant_IndexID, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Enchant_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Enchant_IndexID = {1}", Item_Define.Item_System_Item_Enchant_Table, Enchant_IndexID);
            System_Item_Enchant retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Enchant>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Enchant_IndexID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Item_Enchant();
            return retObj;
        }

        public static System_Item_Enchant GetSystem_Item_Enchant(ref TxnBlock TB, long EnchantGroup, int Tier, int Level, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Enchant_Table);
            string dict_key = string.Format("{0}_{1}_{2}", EnchantGroup, Tier, Level);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE EnchantGroup = {1} AND Tier = {2} AND EnchantLevel = {3}", Item_Define.Item_System_Item_Enchant_Table, EnchantGroup, Tier, Level);
            System_Item_Enchant retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Enchant>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, dict_key, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Item_Enchant();
            return retObj;
        }

        public static System_Item_Enchant_Armor GetSystem_Item_Enchant_Armor(ref TxnBlock TB, long Enchant_IndexID, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Enchant_Armor_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Enchant_IndexID = {1}", Item_Define.Item_System_Item_Enchant_Armor_Table, Enchant_IndexID);
            System_Item_Enchant_Armor retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Enchant_Armor>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Enchant_IndexID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Item_Enchant_Armor();
            return retObj;
        }

        public static System_Item_Enchant_Armor GetSystem_Item_Enchant_Armor(ref TxnBlock TB, long EnchantGroup, int Grade, int Level, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Enchant_Armor_Table);
            string dict_key = string.Format("{0}_{1}_{2}", EnchantGroup, Grade, Level);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE EnchantGroup = {1} AND EnchantGrade = {2} AND EnchantLevel = {3}", Item_Define.Item_System_Item_Enchant_Armor_Table, EnchantGroup, Grade, Level);
            System_Item_Enchant_Armor retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Enchant_Armor>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, dict_key, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Item_Enchant_Armor();
            return retObj;
        }

        public static System_Item_Grade_Accessory GetSystem_Item_Grade_Accessory(ref TxnBlock TB, long Accessory_IndexID, int grade, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Grade_Accessory_Table);
            string dict_key = string.Format("{0}_{1}", Accessory_IndexID, grade);
            string setQuery = string.Format(@" SELECT * FROM {0} WITH(NOLOCK)  WHERE
                                                                        AcceGroup  = (SELECT AcceGroup FROM {0} WITH(NOLOCK)  WHERE Accessory_IndexID = {1})
                                                                    AND Grade = {2}
                                                ", Item_Define.Item_System_Item_Grade_Accessory_Table, Accessory_IndexID, grade);
            System_Item_Grade_Accessory retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Grade_Accessory>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, dict_key, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Item_Grade_Accessory();
            return retObj;
        }

        public static System_Item_Grade_Weapon GetSystem_Item_Grade_Weapon(ref TxnBlock TB, long Weapon_IndexID, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Grade_Weapon_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Weapon_IndexID = {1}", Item_Define.Item_System_Item_Grade_Weapon_Table, Weapon_IndexID);
            System_Item_Grade_Weapon retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Grade_Weapon>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Weapon_IndexID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Item_Grade_Weapon();
            return retObj;
        }

        public static System_Item_Grade_Weapon GetSystem_Item_Grade_Weapon(ref TxnBlock TB, long EnchantGroup, int Tier, int Grade, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Grade_Weapon_Table);
            string dict_key = string.Format("{0}_{1}_{2}", EnchantGroup, Tier, Grade);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE WeaponGroup = {1} AND Tier = {2} AND Grade = {3}", Item_Define.Item_System_Item_Grade_Weapon_Table, EnchantGroup, Tier, Grade);
            System_Item_Grade_Weapon retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Grade_Weapon>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, dict_key, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Item_Grade_Weapon();
            return retObj;
        }

        public static System_Item_Refining_Weapon GetSystem_Item_Refining_Weapon(ref TxnBlock TB, long Refining_IndexID, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Refining_Weapon_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Refining_IndexID = {1}", Item_Define.Item_System_Item_Refining_Weapon_Table, Refining_IndexID);
            System_Item_Refining_Weapon retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Refining_Weapon>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Refining_IndexID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Item_Refining_Weapon();
            return retObj;
        }
        
        public static System_Item_Refining_Weapon GetSystem_Item_Refining_Weapon(ref TxnBlock TB, string Refining_Type, short tier, short level, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Refining_Weapon_Table);
            string dict_key = string.Format("{0}_{1}_{2}", Refining_Type, tier, level);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE RefiningType = '{1}' AND Tier = {2} AND RefiningLevel = {3}", Item_Define.Item_System_Item_Refining_Weapon_Table, Refining_Type, tier, level);
            System_Item_Refining_Weapon retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Refining_Weapon>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, dict_key, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Item_Refining_Weapon();
            return retObj;
        }

        public static List<System_Item_Refining_Weapon> GetSystem_Item_Refining_Weapon_TierAll(ref TxnBlock TB, string Refining_Type, short tier, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Refining_Weapon_Table);
            string dict_key = string.Format("{0}_{1}_TierAll", Refining_Type, tier);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE RefiningType = '{1}' AND Tier = {2}", Item_Define.Item_System_Item_Refining_Weapon_Table, Refining_Type, tier);
            return TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow_Hash<System_Item_Refining_Weapon>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, dict_key, setQuery, dbkey, Flush);            
        }


        public static System_Item_Cape GetSystem_Item_Cape(ref TxnBlock TB, long Cape_IndexID, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Cape_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Cape_IndexID = {1}", Item_Define.Item_System_Item_Cape_Table, Cape_IndexID);
            System_Item_Cape retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Cape>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Cape_IndexID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Item_Cape();
            return retObj;
        }

        public static System_Item_Tier_SetInfo GetSystem_Item_Tier_SetInfo(ref TxnBlock TB, long TierSet_IndexID, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Tier_SetInfo_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE TierSet_IndexID = {1}", Item_Define.Item_System_Item_Tier_SetInfo_Table, TierSet_IndexID);
            System_Item_Tier_SetInfo retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Item_Tier_SetInfo>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, TierSet_IndexID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Item_Tier_SetInfo();
            return retObj;
        }

        public static System_Ultimate_Weapon GetSystem_Ultimate_Weapon(ref TxnBlock TB, long ItemID, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}_{2}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Base_Table, Item_Define.Item_System_Ultimate_Weapon_Table);
            string setQuery = string.Format(@"SELECT *
                                                  FROM
                                                   {0} AS A WITH(NOLOCK) 
                                                   INNER JOIN {1} AS B WITH(NOLOCK) 
                                                   ON A.Class_IndexID = B.Ultimate_ID 
                                                  WHERE A.Item_IndexID = {2}
                                                ", Item_Define.Item_System_Item_Base_Table, Item_Define.Item_System_Ultimate_Weapon_Table, ItemID);
            System_Ultimate_Weapon retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Ultimate_Weapon>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, ItemID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Ultimate_Weapon();
            return retObj;
        }

        public static System_Ultimate_Orb GetSystem_Ultimate_Orb(ref TxnBlock TB, long ItemID, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}_{2}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Item_Base_Table, Item_Define.Item_System_Ultimate_Orb_Table);
            string setQuery = string.Format(@"SELECT *
                                                  FROM
                                                   {0} AS A WITH(NOLOCK) 
                                                   INNER JOIN {1} AS B WITH(NOLOCK) 
                                                   ON A.Class_IndexID = B.Orb_Item_ID 
                                                  WHERE A.Item_IndexID = {2}
                                                ", Item_Define.Item_System_Item_Base_Table, Item_Define.Item_System_Ultimate_Orb_Table, ItemID);
            System_Ultimate_Orb retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Ultimate_Orb>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, ItemID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Ultimate_Orb();
            return retObj;
        }

        public static System_Ultimate_Enchant GetSystem_Ultimate_Enchant(ref TxnBlock TB, long Enchant_IndexID, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Ultimate_Enchant_Table);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE Ultimate_Enchant_Index = {1}", Item_Define.Item_System_Ultimate_Enchant_Table, Enchant_IndexID);
            System_Ultimate_Enchant retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Ultimate_Enchant>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, Enchant_IndexID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Ultimate_Enchant();
            return retObj;
        }

        public static System_Ultimate_Enchant GetSystem_Ultimate_Enchant(ref TxnBlock TB, long Ultimate_ID, int Level, int Step, string dbkey = Item_Define.Item_InvenDB, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", Item_Define.SystemItem_Prefix, Item_Define.Item_System_Ultimate_Enchant_Table);
            string dict_key = string.Format("{0}_{1}_{2}", Ultimate_ID, Level, Step);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK, INDEX(IX_Ultimate_Enchant))  WHERE Ultimate_ID = {1} AND EnchantLevel = {2} AND Step = {3}", Item_Define.Item_System_Ultimate_Enchant_Table, Ultimate_ID, Level, Step);
            System_Ultimate_Enchant retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Ultimate_Enchant>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, dict_key, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Ultimate_Enchant();
            return retObj;
        }
    }
}
