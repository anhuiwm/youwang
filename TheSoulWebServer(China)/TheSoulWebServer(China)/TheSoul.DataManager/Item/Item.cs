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
        // Item List in Inven 
        //const string InvenDBName = "sharding";
        //const string InvenDBTableName = "User_Inven";
        const string InvenPrefix = "CharacterItem";

        private static List<User_Inven_Option> GetInvenOptionList(ref TxnBlock TB, long AID, long CID = 0, bool Flush = false, string dbkey = Item_Define.Item_InvenDB)
        {
            string setKey = string.Format("{0}_{1}_{2}_{3}", Item_Define.User_Inven_Prefix, Item_Define.Item_User_Inven_Option_Table, AID, CID);
            List<User_Inven_Option> retObj = new List<User_Inven_Option>();

            if (!Flush)
            {
                retObj = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis_Hash_All<User_Inven_Option>(DataManager_Define.RedisServerAlias_User, setKey);
                if (retObj.Count == 0)
                    Flush = true;
            }

            if (Flush)
            {
                RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey);
                string setQuery = string.Format(@"SELECT A.* FROM {0} AS A WITH(NOLOCK), {1} AS B 
                                                WITH(NOLOCK) WHERE B.aid = {2} AND B.cid = {3} AND B.invenseq = A.invenseq AND B.delflag = 'N'
                                            ", Item_Define.Item_User_Inven_Option_Table, Item_Define.Item_User_Inven_Table, AID, CID);
                retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_Inven_Option>(ref TB, setQuery, dbkey);

                retObj.ForEach(item =>
                {
                    RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, item.optionseq.ToString(), item);
                });
            }

            RedisConst.GetRedisInstance().SetExpireTimeHash(DataManager_Define.RedisServerAlias_User, setKey);

            return retObj;
        }

        public static List<User_Inven> GetCharacterInvenList(ref TxnBlock TB, long AID, long CID, bool bWithOption = true, bool Flush = false, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Inven> retObj = new List<User_Inven>();

            {
                string setKey = GetInvenKey(AID, CID);
                List<User_Inven> setObj = new List<User_Inven>();
                if (!Flush)
                {
                    setObj = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis_MultipleRow<User_Inven>(DataManager_Define.RedisServerAlias_User, setKey);
                    //setObj = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis_Hash_All<User_Inven>(DataManager_Define.RedisServerAlias_User, setKey);

                    if (setObj.Count == 0)
                        Flush = true;
                }
                if (Flush)
                {
                    RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey);
                    string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND cid = {2} AND delflag='N' AND itemea > 0 ", Item_Define.Item_User_Inven_Table, AID, CID);
                    setObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_Inven>(ref TB, setQuery, dbkey);

                    if (bWithOption)
                    {
                        List<User_Inven_Option> option_list = GetInvenOptionList(ref TB, AID, CID, Flush, dbkey);
                        if(option_list.Count == 0)
                            option_list = GetInvenOptionList(ref TB, AID, CID, true, dbkey);

                        foreach (User_Inven chkItem in setObj)
                        {
                            chkItem.base_option = option_list.Where(setoption => setoption.isbase.Equals("Y") && setoption.invenseq == chkItem.invenseq).ToList();
                            chkItem.random_option = option_list.Where(setoption => setoption.isbase.Equals("N") && setoption.invenseq == chkItem.invenseq).ToList();

                            if (chkItem.base_option.Count + chkItem.random_option.Count < 1)
                            {
                                option_list = GetInvenOptionList(ref TB, AID, CID, true, dbkey);
                                chkItem.base_option = option_list.Where(setoption => setoption.isbase.Equals("Y") && setoption.invenseq == chkItem.invenseq).ToList();
                                chkItem.random_option = option_list.Where(setoption => setoption.isbase.Equals("N") && setoption.invenseq == chkItem.invenseq).ToList();
                            }

                            retObj.Add(chkItem);
                            //RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, chkItem.invenseq.ToString(), chkItem);
                        }
                        RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_User, setKey, retObj);

                        //setObj.ForEach(item =>
                        //{
                        //    item.base_option = option_list.Where(setoption => setoption.isbase.Equals("Y") && setoption.invenseq == item.invenseq).ToList();
                        //    item.random_option = option_list.Where(setoption => setoption.isbase.Equals("N") && setoption.invenseq == item.invenseq).ToList();
                        //    retObj.Add(item);
                        //    RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, item.invenseq.ToString(), item);
                        //});
                    }
                    else
                    {
                        setObj.ForEach(item =>
                        {
                            item.base_option = new List<User_Inven_Option>();
                            item.random_option = new List<User_Inven_Option>();
                            retObj.Add(item);
                        });
                    }
                }
                else
                    retObj.AddRange(setObj);

                //RedisConst.GetRedisInstance().SetExpireTimeHash(DataManager_Define.RedisServerAlias_User, setKey);

                if (CID > 0)
                {
                    string setEquipKey = GetEquipKey(AID, CID);
                    //RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setEquipKey);
                    RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_User, setEquipKey, setObj.Where(item => item.equipflag.Equals("Y")).ToList());

                    //setObj.Where(item => item.equipflag.Equals("Y")).ToList().ForEach(item =>
                    //{
                    //    RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setEquipKey, item.invenseq.ToString(), item);
                    //}
                    //);
                }
            }

            return retObj;
        }

        private static string GetInvenKey(long AID, long CID)
        {
            return string.Format("{0}_{1}_{2}_{3}", Item_Define.User_Inven_Prefix, Item_Define.Item_User_Inven_Table, AID, CID);
        }

        public static List<User_Inven> GetInvenList(ref TxnBlock TB, long AID, long CID = 0, bool bWithOption = true, bool Flush = false, string dbkey = Item_Define.Item_InvenDB)
        {
            List<Character> charList = CharacterManager.GetCharacterList(ref TB, AID, Flush, dbkey);
            List<User_Inven> retObj = new List<User_Inven>();

            charList.Add(new Character());

            foreach (Character charInfo in charList)
            {
                if (charInfo.cid == 0 || CID == charInfo.cid || CID == 0)
                {
                    retObj.AddRange(GetCharacterInvenList(ref TB, AID, charInfo.cid, bWithOption, Flush, dbkey));
                }
            }            

            return retObj;
        }

        public static void FlushInvenList(ref TxnBlock TB, long AID, string dbkey = Item_Define.Item_InvenDB)
        {
            List<Character> charList = CharacterManager.GetCharacterList(ref TB, AID);
            charList.Add(new Character());

            foreach (Character charInfo in charList)
            {
                FlushCharacterInvenList(AID, charInfo.cid);
            }
        }

        public static void FlushCharacterInvenList(long AID, long CID, string dbkey = Item_Define.Item_InvenDB)
        {
            string setKey = GetInvenKey(AID, CID);
            RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);

            string setEquipKey = GetEquipKey(AID, CID);
            RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setEquipKey);            
        }
        
        // public method : Get System ItemInfo 
        public static Object GetSystemItemInfo(ref TxnBlock TB, long ItemID, bool Flush = false, string dbkey = Item_Define.Item_InvenDB)
        {
            Object retObj = null;            
            System_Item_Base itemBaseInfo = GetSystem_Item_Base(ref TB, ItemID, dbkey, Flush);

            if (itemBaseInfo.Item_IndexID == 0)
                return retObj;
            Item_Define.eSystemItemType itemType = Item_Define.eSystemItemType.ItemClass_NONE;

            if(itemBaseInfo != null)
                itemType = Item_Define.SystemItemType[itemBaseInfo.ItemClass];

            switch (itemType)
            {
                case Item_Define.eSystemItemType.ItemClass_Equip:
                    retObj = GetSystem_Item_Equip(ref TB, itemBaseInfo.Item_IndexID, dbkey, Flush);
                    break;
                case Item_Define.eSystemItemType.ItemClass_Use:
                    retObj = GetSystem_Item_Use(ref TB, itemBaseInfo.Item_IndexID, dbkey, Flush);
                    break;
                case Item_Define.eSystemItemType.ItemClass_Info:
                    retObj = GetSystem_Item_Info(ref TB, itemBaseInfo.Item_IndexID, dbkey, Flush);
                    break;
                case Item_Define.eSystemItemType.ItemClass_Costume:
                    retObj = GetSystem_Item_Costume(ref TB, itemBaseInfo.Item_IndexID, dbkey, Flush);
                    break;
                case Item_Define.eSystemItemType.ItemClass_Band:
                default:
                    retObj = itemBaseInfo;
                    break;
            }

            return retObj;
        }


        // Make Item to DB
        //const string User_Item_Inventory_TableName = "User_Inven";
        private static List<User_Inven> MakeItemToDB(ref TxnBlock TB, ref User_Inven setItem, int makeCount = 1, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Inven> MakeResultItemList = new List<User_Inven>();
            while (makeCount > 0)
            {
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "User_Insert_Item_Inven";
                Cmd.Parameters.Add("@make_count", SqlDbType.Int).Value = makeCount;
                Cmd.Parameters.Add("@cid", SqlDbType.BigInt).Value = setItem.cid;
                Cmd.Parameters.Add("@aid", SqlDbType.BigInt).Value = setItem.aid;
                Cmd.Parameters.Add("@inventory_type", SqlDbType.SmallInt).Value = setItem.inventory_type;
                Cmd.Parameters.Add("@itemid", SqlDbType.Int).Value = setItem.itemid;
                Cmd.Parameters.Add("@itemea", SqlDbType.Int).Value = setItem.itemea;
                Cmd.Parameters.Add("@itemtype", SqlDbType.TinyInt).Value = setItem.item_type;
                Cmd.Parameters.Add("@classtype", SqlDbType.TinyInt).Value = setItem.class_type;
                Cmd.Parameters.Add("@grade", SqlDbType.TinyInt).Value = setItem.grade;
                Cmd.Parameters.Add("@level", SqlDbType.TinyInt).Value = setItem.level;
                Cmd.Parameters.Add("@equipflag", SqlDbType.Char).Value = setItem.equipflag;
                Cmd.Parameters.Add("@newyn", SqlDbType.Char).Value = setItem.newyn;
                Cmd.Parameters.Add("@delflag", SqlDbType.Char).Value = setItem.delflag;
                Cmd.Parameters.Add("@equipposition", SqlDbType.VarChar).Value = setItem.equipposition;
                var outputMakeCount = new SqlParameter("@return_makecount", SqlDbType.Int) { Direction = ParameterDirection.Output };
                Cmd.Parameters.Add(outputMakeCount);
                var result = new SqlParameter("@ret_result", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                Cmd.Parameters.Add(result);

                SqlDataReader getDr = null;
                TB.ExcuteSqlStoredProcedure(dbkey, ref Cmd, ref getDr);

                if (getDr != null)
                {
                    int checkValue = System.Convert.ToInt32(result.Value);

                    if (checkValue < 0)
                    {
                        getDr.Dispose(); getDr.Close();
                        Cmd.Dispose();
                        return MakeResultItemList;
                    }

                    var r = SQLtoJson.Serialize(ref getDr);
                    string json = mJsonSerializer.ToJsonString(r);

                    getDr.Dispose(); getDr.Close();
                    List<User_Inven> InvenList = mJsonSerializer.JsonToObject<List<User_Inven>>(json);

                    int retMakeCount = System.Convert.ToInt32(outputMakeCount.Value);
                    Cmd.Dispose();
                    if (InvenList != null)
                    {
                        makeCount -= retMakeCount;
                        if (InvenList.Count > 0)
                        {
                            var makeItem = InvenList.FirstOrDefault();
                            var findItem = MakeResultItemList.Find(item => item.invenseq == makeItem.invenseq);
                            if (findItem != null)
                            {
                                MakeResultItemList.Remove(findItem);
                            }
                            else
                            {
                                makeItem.random_option = new List<User_Inven_Option>();
                                makeItem.base_option = new List<User_Inven_Option>();
                            }
                            MakeResultItemList.Add(makeItem);

                            string setItemLogJson = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.item_log_list]);

                            TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_item_log]);
                            ItemLogInfo setLog = new ItemLogInfo(makeItem.itemid, (int)SnailLog_Define.Snail_Money_Event_type.add, makeCount, makeItem.itemea - retMakeCount, makeItem.itemea);
                            setItemLogJson = mJsonSerializer.AddJsonArray(setItemLogJson, mJsonSerializer.ToJsonString(setLog));

                            TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.item_log_list], setItemLogJson);
                        }
                    }

                    if (retMakeCount == 0)
                        return MakeResultItemList;                    
                }
                else
                {
                    Cmd.Dispose();
                    return MakeResultItemList;
                }
            }

            return MakeResultItemList;
        }



        // Make Item to DB
        //const string User_Item_Inventory_TableName = "User_Inven";
        private static Result_Define.eResult MakeItemOptionToDB(ref TxnBlock TB, ref User_Inven_Option setOption,  Item_Define.eInventoryType setInven = Item_Define.eInventoryType.Account_Inven,  string dbkey = Item_Define.Item_InvenDB)
        {
            SqlCommand Cmd = new SqlCommand();
            if (setInven == Item_Define.eInventoryType.Account_Inven || setInven == Item_Define.eInventoryType.Character_Inven)
                Cmd.CommandText = "User_Insert_Item_Inven_Option";
            else if (setInven == Item_Define.eInventoryType.Ultimate_Inven)
                Cmd.CommandText = "User_Insert_Ultimate_Inven_Optione";
            else
                return Result_Define.eResult.ITEM_INFO_TYPE_INVALIDE;
            
            Cmd.Parameters.Add("@invenseq", SqlDbType.BigInt).Value = setOption.invenseq;
            Cmd.Parameters.Add("@isbase", SqlDbType.Char, 1).Value = setOption.isbase;
            Cmd.Parameters.Add("@optiontype", SqlDbType.NVarChar).Value = setOption.optiontype;
            Cmd.Parameters.Add("@option_value", SqlDbType.Int).Value = setOption.option_value;
            Cmd.Parameters.Add("@option_add_value", SqlDbType.Int).Value = setOption.option_add_value;
            Cmd.Parameters.Add("@option_grade", SqlDbType.Int).Value = setOption.option_grade;
            Cmd.Parameters.Add("@option_level", SqlDbType.Int).Value = setOption.option_level;
            Cmd.Parameters.Add("@option_exp", SqlDbType.Int).Value = setOption.option_exp;
            Cmd.Parameters.Add("@delflag", SqlDbType.Char, 1).Value = setOption.delflag;
            var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            Cmd.Parameters.Add(result);

            SqlDataReader getDr = null;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref Cmd, ref getDr))
            {
                if (getDr != null)
                {
                    int checkValue = System.Convert.ToInt32(result.Value);

                    if (checkValue < 0)
                    {
                        getDr.Dispose(); getDr.Close();
                        Cmd.Dispose();
                        return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                    }

                    var r = SQLtoJson.Serialize(ref getDr);
                    string json = mJsonSerializer.ToJsonString(r);

                    getDr.Dispose(); getDr.Close();
                    Cmd.Dispose();
                    User_Inven_Option getOption = mJsonSerializer.JsonToObject<User_Inven_Option[]>(json).FirstOrDefault();

                    if (getOption != null)
                    {
                        setOption = getOption;
                        return Result_Define.eResult.SUCCESS;
                    }
                    else
                        return Result_Define.eResult.ITEM_CREATE_FAIL;
                }
                else
                {
                    Cmd.Dispose();
                    return Result_Define.eResult.ITEM_CREATE_FAIL;
                }
            }
            else
            {
                Cmd.Dispose();
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }
        }

        public static Object CheckItemType(ref TxnBlock TB, long ItemID, ref Item_Define.eSystemItemType checkType, string dbkey = Item_Define.Item_InvenDB)
        {
            Object SysItem = TheSoul.DataManager.ItemManager.GetSystemItemInfo(ref TB, ItemID, false, dbkey);

            if (SysItem != null)
            {
                System_Item_Base setItemBaseInfo = (System_Item_Base)SysItem;
                if (!Item_Define.SystemItemType.ContainsKey(setItemBaseInfo.ItemClass))
                    return Result_Define.eResult.ITEM_INFO_TYPE_INVALIDE;
                checkType = Item_Define.SystemItemType[setItemBaseInfo.ItemClass];
            }
            
            return SysItem;
        }

        // public method : make user item
        public static Result_Define.eResult MakeItem(ref TxnBlock TB, ref List<User_Inven> MakeList, long AID, long ItemID, int makeCount, long CID = 0, short setEnchantLevel = 0, short setEnchantGrade = 1, long ShopID = 0, string dbkey = Item_Define.Item_InvenDB)
        {
            TheSoul.DataManager.Item_Define.eSystemItemType checkType = Item_Define.eSystemItemType.ItemClass_NONE;
            Object SysItem = TheSoul.DataManager.ItemManager.CheckItemType(ref TB, ItemID, ref checkType, dbkey);

            if (SysItem == null)
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            System_Item_Base setItemBaseInfo = (System_Item_Base)SysItem;
            if (setItemBaseInfo.Item_IndexID <= 0)
                return Result_Define.eResult.ITEM_SYSTEM_ID_NOT_FOUND;

            if (!Item_Define.SystemItemType.ContainsKey(setItemBaseInfo.ItemClass))
                return Result_Define.eResult.ITEM_INFO_TYPE_INVALIDE;

            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            
            switch (checkType)
            {
                case TheSoul.DataManager.Item_Define.eSystemItemType.ItemClass_Equip:
                    retError = MakeEquip(ref TB, ref MakeList, AID, ItemID, makeCount, CID, setEnchantLevel, setEnchantGrade, dbkey);
                    break;
                case TheSoul.DataManager.Item_Define.eSystemItemType.ItemClass_Costume:
                    retError = MakeCostume(ref TB, ref MakeList, AID, ItemID, makeCount, CID, setEnchantLevel, setEnchantGrade, dbkey);
                    break;
                case TheSoul.DataManager.Item_Define.eSystemItemType.ItemClass_Use:
                case TheSoul.DataManager.Item_Define.eSystemItemType.ItemClass_Info:
                case TheSoul.DataManager.Item_Define.eSystemItemType.ItemClass_Band:
                    retError = MakeNonEquip(ref TB, ref MakeList, ref SysItem, ref checkType, AID, ItemID, makeCount, CID, ShopID, dbkey);
                    break;
                case TheSoul.DataManager.Item_Define.eSystemItemType.Soul_Parts:
                    retError = SoulManager.MakeSoulParts(ref TB, setItemBaseInfo.Class_IndexID, AID, makeCount, setItemBaseInfo.StackMAX);
                    if(retError == Result_Define.eResult.SUCCESS)
                        MakeList.Add(new User_Inven(setItemBaseInfo.Item_IndexID, makeCount));
                    break;
                case TheSoul.DataManager.Item_Define.eSystemItemType.Soul_Equip:
                    retError = SoulManager.MakeSoulEquip(ref TB, setItemBaseInfo.Class_IndexID, AID, makeCount, setItemBaseInfo.StackMAX);
                    if(retError == Result_Define.eResult.SUCCESS)
                        MakeList.Add(new User_Inven(setItemBaseInfo.Item_IndexID, makeCount));
                    break;
                case TheSoul.DataManager.Item_Define.eSystemItemType.ItemClass_Ultimate:
                    User_Ultimate_Inven MakeInfo = new User_Ultimate_Inven();
                    retError = MakeUltimateWeapon(ref TB, ref MakeInfo, setItemBaseInfo.Class_IndexID, AID, CID);
                    if (retError == Result_Define.eResult.SUCCESS)
                        MakeList.Add(new User_Inven(MakeInfo.item_id, makeCount));
                    break;
                case TheSoul.DataManager.Item_Define.eSystemItemType.ItemClass_Orb:
                    for (int checkCount = 0; checkCount < makeCount; checkCount++)
                    {
                        User_Orb_Inven MakeOrbInfo = new User_Orb_Inven();
                        retError = MakeUltimateOrb(ref TB, ref MakeOrbInfo, AID, setItemBaseInfo.Class_IndexID);
                        if (retError == Result_Define.eResult.SUCCESS)
                            MakeList.Add(new User_Inven(MakeOrbInfo));
                    }
                    break;
                default:
                    retError = Result_Define.eResult.ITEM_INFO_TYPE_INVALIDE;
                    break;
            }

            FlushInvenList(ref TB, AID, dbkey);

            return retError;
        }

        // private method : make equip
        private static Result_Define.eResult MakeCostume(ref TxnBlock TB, ref List<User_Inven> MakeList, long AID, long ItemID, int makeCount, long CID = 0, short setEnchantLevel = 0, short setEnchantGrade = 1, string dbkey = Item_Define.Item_InvenDB)
        {
            System_Item_Costume ItemInfo = GetSystem_Item_Costume(ref TB, ItemID, dbkey);
            if (ItemInfo == null)
                return Result_Define.eResult.ITEM_EQUIP_INFO_NOT_FOUND;

            Item_Define.eItemType setItemType = Item_Define.eItemType.ItemType_Costume;

            for (int setCount = 0; setCount < makeCount; setCount++)
            {
                List<User_Inven_Option> getOptionList = new List<User_Inven_Option>();
                User_Inven setItem = new User_Inven();
                setItem.aid = AID;
                setItem.inventory_type = (short)(Item_Define.Item_Make_Inventory_Type.ContainsKey(setItemType) ? Item_Define.Item_Make_Inventory_Type[setItemType] : Item_Define.eInventoryType.Account_Inven);

                if ((Item_Define.eInventoryType)setItem.inventory_type == Item_Define.eInventoryType.Character_Inven)
                {
                    if (CID == 0)
                        return Result_Define.eResult.ITEM_EQUIP_NEED_CID;
                    else
                        setItem.cid = CID;
                }
                else
                    setItem.cid = 0;

                if (setItem.cid > 0)
                {
                    Character charInfo = CharacterManager.GetCharacter(ref TB, AID, setItem.cid, false, dbkey);
                    Character_Define.SystemClassType setClassType = Character_Define.ClassTypeToEnum[ItemInfo.EquipClass];
                    if (setClassType != (Character_Define.SystemClassType)charInfo.Class && setClassType != Character_Define.SystemClassType.Class_None)    // None is All Equip
                        return Result_Define.eResult.ITEM_EQUIP_CLASS_TYPE_INVALIDE;
                    else
                        setItem.class_type = (byte)charInfo.Class;
                }
                else
                {
                    setItem.class_type = 0;
                }
                
                setItem.itemid = ItemID;
                setItem.item_type = (byte)Item_Define.eItemType_Inven.Equip;
                setItem.itemea = 1;
                setItem.grade = setEnchantGrade;
                setItem.equipposition = ItemInfo.EquipPosition;
                setItem.level = setEnchantLevel;
                setItem.equipflag = setItem.delflag = "N";
                setItem.newyn = "Y";

                if (Item_Define.ItemCostumeTypeList.Contains(setItemType))
                {
                    System_Item_Option setFixOption = null;
                    if (ItemInfo.FixOptionID1 > 0)
                    {
                        setFixOption = GetSystem_Item_Option(ref TB, ItemInfo.FixOptionID1);
                        if (setFixOption.Option_IndexID > 0)
                        {
                            User_Inven_Option setOption = MakeUserInvenOption(setFixOption, false);
                            setOption.isbase = "Y";
                            getOptionList.Add(setOption);
                        }
                    }
                    if (ItemInfo.FixOptionID2 > 0)
                    {
                        setFixOption = GetSystem_Item_Option(ref TB, ItemInfo.FixOptionID2);
                        if (setFixOption.Option_IndexID > 0)
                        {
                            User_Inven_Option setOption = MakeUserInvenOption(setFixOption, false);
                            setOption.isbase = "Y";
                            getOptionList.Add(setOption);
                        }
                    }
                    if (ItemInfo.FixOptionID3 > 0)
                    {
                        setFixOption = GetSystem_Item_Option(ref TB, ItemInfo.FixOptionID3);
                        if (setFixOption.Option_IndexID > 0)
                        {
                            User_Inven_Option setOption = MakeUserInvenOption(setFixOption, false);
                            setOption.isbase = "Y";
                            getOptionList.Add(setOption);
                        }
                    }
                }

                List<User_Inven> makeItems = MakeItemToDB(ref TB, ref setItem, 1, dbkey);

                foreach (User_Inven item in makeItems)
                {
                    foreach (User_Inven_Option setOption in getOptionList)
                    {
                        setOption.delflag = "N";
                        setOption.invenseq = item.invenseq;

                        User_Inven_Option refOption = setOption;
                        if (MakeItemOptionToDB(ref TB, ref refOption) != Result_Define.eResult.SUCCESS)
                            return Result_Define.eResult.ITEM_CREATE_FAIL;

                        if (setOption.isbase.Equals("Y"))
                            item.base_option.Add(refOption);
                        else
                            item.random_option.Add(refOption);
                    }
                }

                MakeList.AddRange(makeItems);
            }

            return MakeList.Count > 0 ? Result_Define.eResult.SUCCESS : Result_Define.eResult.ITEM_CREATE_FAIL;
        }

        // private method : make equip
        private static Result_Define.eResult MakeEquip(ref TxnBlock TB, ref List<User_Inven> MakeList, long AID, long ItemID, int makeCount, long CID = 0, short setEnchantLevel = 0, short setEnchantGrade = 1, string dbkey = Item_Define.Item_InvenDB)
        {
            System_Item_Equip ItemInfo = GetSystem_Item_Equip(ref TB, ItemID, dbkey);
            if (ItemInfo == null)
                return Result_Define.eResult.ITEM_EQUIP_INFO_NOT_FOUND;

            if (!Item_Define.ItemType.ContainsKey(ItemInfo.ItemType))
                return Result_Define.eResult.ITEM_TYPE_INVALIDE;

            Item_Define.eItemType setItemType = Item_Define.ItemType[ItemInfo.ItemType];

            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            for (int setCount = 0; setCount < makeCount; setCount++)
            {
                List<User_Inven_Option> getOptionList = Make_ItemOption(ref TB, setItemType, setEnchantGrade, ItemInfo.Tier, out retError, dbkey);

                if (retError != Result_Define.eResult.SUCCESS)
                    return retError;

                getOptionList.ForEach(item => { item.isbase = "N"; });
                User_Inven setItem = new User_Inven();

                setItem.aid = AID;
                setItem.inventory_type = (short)(Item_Define.Item_Make_Inventory_Type.ContainsKey(setItemType) ? Item_Define.Item_Make_Inventory_Type[setItemType] : Item_Define.eInventoryType.Account_Inven);

                if ((Item_Define.eInventoryType)setItem.inventory_type == Item_Define.eInventoryType.Character_Inven)
                {
                    if (CID == 0)
                        return Result_Define.eResult.ITEM_EQUIP_NEED_CID;
                    else
                        setItem.cid = CID;
                }
                else
                    setItem.cid = 0;

                if (setItem.cid > 0)
                {
                    Character charInfo = CharacterManager.GetCharacter(ref TB, AID, setItem.cid, false, dbkey);
                    Character_Define.SystemClassType setClassType = Character_Define.ClassTypeToEnum[ItemInfo.EquipClass];
                    if (setClassType != (Character_Define.SystemClassType)charInfo.Class && setClassType != Character_Define.SystemClassType.Class_None)    // None is All Equip
                        return Result_Define.eResult.ITEM_EQUIP_CLASS_TYPE_INVALIDE;
                    else
                        setItem.class_type = (byte)charInfo.Class;
                }
                else
                {
                    setItem.class_type = 0;
                }

                setItem.itemid = ItemID;
                setItem.item_type = (byte)Item_Define.eItemType_Inven.Equip;
                setItem.itemea = 1;
                setItem.grade = setEnchantGrade;
                setItem.equipposition = ItemInfo.EquipPosition;
                setItem.level = setEnchantLevel;
                setItem.equipflag = setItem.delflag = "N";
                setItem.newyn = "Y";

                // make main param
                if (Item_Define.ItemArmorTypeList.Contains(setItemType))
                {
                    System_Item_Enchant_Armor equipBaseInfo = GetSystem_Item_Enchant_Armor(ref TB, ItemInfo.GrowthTableID1);    // weapon use enchant table for base option
                    if (equipBaseInfo.Enchant_IndexID > 0)
                    {
                        setItem.grade = setItem.grade > 1 ? setItem.grade : equipBaseInfo.EnchantGrade;
                        if (equipBaseInfo.MainParameter_Type.Length > 0 )
                            getOptionList.Add(new User_Inven_Option(equipBaseInfo.MainParameter_Type, equipBaseInfo.MainParameter_Value1, Item_Define.bItemBaseOption));
                        if (equipBaseInfo.SubParameter_Type.Length > 0 )
                            getOptionList.Add(new User_Inven_Option(equipBaseInfo.SubParameter_Type, equipBaseInfo.SubParameter_Value1, Item_Define.bItemBaseOption));
                    }
                    else
                        return Result_Define.eResult.ITEM_ENCHANCE_DB_NOT_FOUND;
                }
                else if (Item_Define.ItemWeaponTypeList.Contains(setItemType))
                {
                    System_Item_Grade_Weapon equipBaseInfo = GetSystem_Item_Grade_Weapon(ref TB, ItemInfo.GrowthTableID2);      // weapon use grade table for base option
                    if (!(equipBaseInfo.Tier == ItemInfo.Tier && equipBaseInfo.Grade == setItem.grade))
                        equipBaseInfo = GetSystem_Item_Grade_Weapon(ref TB, equipBaseInfo.WeaponGroup, ItemInfo.Tier, setItem.grade);

                    if (equipBaseInfo.Weapon_IndexID > 0)
                    {
                        setItem.grade = setItem.grade > 1 ? setItem.grade : equipBaseInfo.Grade;
                        if (equipBaseInfo.minAP_Fix >= 0)
                            getOptionList.Add(new User_Inven_Option(Item_Define.WeaponParam_Min_Damage, equipBaseInfo.minAP_Fix, Item_Define.bItemBaseOption));
                        if (equipBaseInfo.maxAP_Fix >= 0)
                            getOptionList.Add(new User_Inven_Option(Item_Define.WeaponParam_Max_Damage, equipBaseInfo.maxAP_Fix, Item_Define.bItemBaseOption));
                    }
                    else
                        return Result_Define.eResult.ITEM_ENCHANCE_DB_NOT_FOUND;
                }
                else if (Item_Define.ItemAccessoryTypeList.Contains(setItemType))
                {
                    System_Item_Grade_Accessory equipBaseInfo = GetSystem_Item_Grade_Accessory(ref TB, ItemInfo.GrowthTableID2, setEnchantGrade);    // accessory use grade table for base option
                    if (equipBaseInfo.Accessory_IndexID > 0)
                    {
                        setItem.grade = setItem.grade > 1 ? setItem.grade : equipBaseInfo.Grade;
                        System_Item_Option setFixOption = GetSystem_Item_Option(ref TB, equipBaseInfo.FixOptionID1);
                        if (setFixOption.Option_IndexID > 0)
                        {
                            User_Inven_Option setOption = MakeUserInvenOption(setFixOption, false);
                            setOption.isbase = "Y";
                            getOptionList.Add(setOption);
                        }
                    }
                    else
                        return Result_Define.eResult.ITEM_ENCHANCE_DB_NOT_FOUND;
                }
                else if (Item_Define.ItemCapeTypeList.Contains(setItemType))
                {
                    System_Item_Cape equipBaseInfo = GetSystem_Item_Cape(ref TB, ItemInfo.GrowthTableID2); // cape use grade table for base option
                    if (equipBaseInfo.Cape_IndexID > 0)
                    {
                        if (equipBaseInfo.DFP_Fix > 0)
                            getOptionList.Add(new User_Inven_Option(Item_Define.CapefParam_DEF, equipBaseInfo.DFP_Fix, Item_Define.bItemBaseOption));
                    }
                }

                List<User_Inven> makeItems = MakeItemToDB(ref TB, ref setItem, 1, dbkey);

                foreach(User_Inven item in makeItems)
                {
                    foreach (User_Inven_Option setOption in getOptionList)
                    {
                        setOption.delflag = "N";
                        setOption.invenseq = item.invenseq;

                        User_Inven_Option refOption = setOption;
                        if (MakeItemOptionToDB(ref TB, ref refOption) != Result_Define.eResult.SUCCESS)
                            return Result_Define.eResult.ITEM_CREATE_FAIL;

                        if (setOption.isbase.Equals("Y"))
                            item.base_option.Add(refOption);
                        else
                            item.random_option.Add(refOption);
                    }
                }

                MakeList.AddRange(makeItems);
            }

            retError = MakeList.Count > 0 ? Result_Define.eResult.SUCCESS : Result_Define.eResult.ITEM_CREATE_FAIL;

            if (retError == Result_Define.eResult.SUCCESS)
            {
                if (Item_Define.ItemWeaponTypeList.Contains(setItemType))
                    retError = TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.Equip_Acquire, (int)Trigger_Define.eEquipAccquireType.Weapon, 0, makeCount);
                else if (Item_Define.ItemAccessoryTypeList.Contains(setItemType))
                    retError = TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.Equip_Acquire, (int)Trigger_Define.eEquipAccquireType.Accessary, 0, makeCount);
            }

            return retError;
        }


        private static Result_Define.eResult MakeInfoItem(ref TxnBlock TB, System_Item_Info item, ref User_Inven retItem, long AID, long CID, Item_Define.eItemType setItemType, int makeCount = 0, long ShopID = 0)
        {
            User_Inven setNonItem = new User_Inven();
            retItem.itemid = item.Item_IndexID;

            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;

            retItem.itemea = item.ItemType_Value > 0 ? makeCount * item.ItemType_Value : makeCount;

            switch (setItemType)
            {
                case Item_Define.eItemType.ItemType_GetRealCash:
                    retError = AccountManager.AddUserCash(ref TB, AID, retItem.itemea);
                    break;
                case Item_Define.eItemType.ItemType_GetCash:
                    retError = AccountManager.AddUserEventCash(ref TB, AID, retItem.itemea);
                    break;
                case Item_Define.eItemType.ItemType_GetPvECoin:
                    retError = AccountManager.AddUserKey(ref TB, AID, retItem.itemea);
                    break;
                case Item_Define.eItemType.ItemType_GetPvPCoin:
                    retError = AccountManager.AddUserTicket(ref TB, AID, retItem.itemea);
                    break;
                case Item_Define.eItemType.ItemType_GetGold:
                    retError = AccountManager.AddUserGold(ref TB, AID, retItem.itemea);
                    break;
                case Item_Define.eItemType.ItemType_GetPassiveSoul:
                    retError = AccountManager.AddUserSoulStone(ref TB, AID, retItem.itemea);
                    break;
                // guild challange ticket not use yet. insteed give to ticket
                case Item_Define.eItemType.ItemType_GetChallenge:
                    retError = AccountManager.AddUserTicket(ref TB, AID, retItem.itemea);
                    break;
                case Item_Define.eItemType.ItemType_GetExpeditionPoint:
                    retError = AccountManager.AddUserExpeditionPoint(ref TB, AID, retItem.itemea);
                    break;
                case Item_Define.eItemType.ItemType_GetGuildPoint:
                    retError = GuildManager.AddGuildContributionPoint(ref TB, AID, retItem.itemea);
                    break;
                case Item_Define.eItemType.ItemType_GetRankingPoint:
                    retError = AccountManager.AddUserOverlordRankingPoint(ref TB, AID, retItem.itemea);
                    break;
                case Item_Define.eItemType.ItemType_GetBattlePoint:
                    retError = AccountManager.AddUserCombatPoint(ref TB, AID, retItem.itemea);
                    break;
                case Item_Define.eItemType.ItemType_GetHonorPoint:
                    retError = AccountManager.AddUserHonor(ref TB, AID, retItem.itemea);
                    break;
                case Item_Define.eItemType.ItemType_GetPartyPoint:
                    retError = AccountManager.AddUserPartyDungeonPoint(ref TB, AID, retItem.itemea);
                    break;
                case Item_Define.eItemType.ItemType_GetBlackMarketPoint:
                    retError = AccountManager.AddUserBlackMarketPoint(ref TB, AID, retItem.itemea);
                    break;
                case Item_Define.eItemType.ItemType_Subscription:
                    retError = ShopManager.UpdateUserSubscription(ref TB, AID, ShopID, retItem.itemea, false);
                    break;
                case Item_Define.eItemType.ItemType_Subscription_Week:
                    retError = ShopManager.UpdateUserSubscription(ref TB, AID, ShopID, retItem.itemea, true);
                    break;
                // Not Use yet ? old type
                //case Item_Define.eItemType.ItemType_GetGachaCoin:
                //case Item_Define.eItemType.ItemType_GetMedal:
                //case Item_Define.eItemType.ItemType_InfinitePvECoin:
                //case Item_Define.eItemType.ItemType_InfinitePvPCoin:
                //case Item_Define.eItemType.ItemType_GetCostume_PC1:
                //case Item_Define.eItemType.ItemType_GetCostume_PC2:
                //case Item_Define.eItemType.ItemType_GetG3VS3Box:
                //case Item_Define.eItemType.ItemType_GetGuildEXPBuff:
                //case Item_Define.eItemType.ItemType_GetGuildSkillBuff:
                //case Item_Define.eItemType.ItemType_GetItemGacha:
                //case Item_Define.eItemType.ItemType_GetPCEXPBuff:
                //case Item_Define.eItemType.ItemType_GetSoulEXPBuff:
                //case Item_Define.eItemType.ItemType_GetSoulGacha:
                //case Item_Define.eItemType.ItemType_GetSummonStone:
                //case Item_Define.eItemType.ItemType_LifeStone:
                //case Item_Define.eItemType.ItemType_LuckyBox:
                //case Item_Define.eItemType.ItemType_Modification_Soul:

                case Item_Define.eItemType.ItemType_Material:
                case Item_Define.eItemType.Itemtype_Boost:
                case Item_Define.eItemType.ItemType_Ultimate:
                    retError = Result_Define.eResult.ITEM_MAKE_TO_INVEN;
                    break;
                default:
                    retError = Result_Define.eResult.ITEM_INFO_TYPE_INVALIDE;
                    break;
            }

            return retError;
        }

        // private method : make non equip item 
        private static Result_Define.eResult MakeNonEquip(ref TxnBlock TB, ref List<User_Inven> MakeList, ref Object Item, ref TheSoul.DataManager.Item_Define.eSystemItemType checkType, long AID, long ItemID, int makeCount, long CID = 0, long ShopID = 0, string dbkey = Item_Define.Item_InvenDB)
        {
            Item_Define.eItemType setItemType = Item_Define.eItemType.None;
            Item_Define.eItemType_Inven setInven = Item_Define.eItemType_Inven.None;
            string equipItemType = "None";
            switch (checkType)
            {
                case TheSoul.DataManager.Item_Define.eSystemItemType.ItemClass_Use:
                    setItemType = Item_Define.ItemType[((System_Item_Use)Item).ItemType];
                    setInven = Item_Define.eItemType_Inven.Use;
                    equipItemType = ((System_Item_Use)Item).ItemType;
                    break;
                case TheSoul.DataManager.Item_Define.eSystemItemType.ItemClass_Info:
                    if (!Item_Define.ItemType.ContainsKey(((System_Item_Info)Item).ItemType))
                        return Result_Define.eResult.ITEM_INFO_TYPE_INVALIDE;
                    setItemType = Item_Define.ItemType[((System_Item_Info)Item).ItemType];
                    setInven = Item_Define.eItemType_Inven.Info;
                    equipItemType = ((System_Item_Info)Item).ItemType;
                    User_Inven retitem = new User_Inven();
                    Result_Define.eResult checkError = Result_Define.eResult.ITEM_MAKE_TO_INVEN;

                    checkError = MakeInfoItem(ref TB, ((System_Item_Info)Item), ref retitem, AID, CID, setItemType, makeCount, ShopID);

                    if (checkError == Result_Define.eResult.SUCCESS)
                    {
                        MakeList.Add(retitem);
                        return MakeList.Count > 0 ? Result_Define.eResult.SUCCESS : Result_Define.eResult.ITEM_CREATE_FAIL;
                    }
                    else if (checkError != Result_Define.eResult.ITEM_MAKE_TO_INVEN)
                        return Result_Define.eResult.ITEM_CREATE_FAIL;
                    break;
                case TheSoul.DataManager.Item_Define.eSystemItemType.ItemClass_Costume:
                    setItemType = Item_Define.ItemType[((System_Item_Costume)Item).ItemType];
                    setInven = Item_Define.eItemType_Inven.Costume;
                    equipItemType = ((System_Item_Costume)Item).ItemType;
                    break;
            }

            //ItemInfo.StackMAX;

            User_Inven setItem = new User_Inven();

            setItem.aid = AID;
            setItem.inventory_type = (short)(Item_Define.Item_Make_Inventory_Type.ContainsKey(setItemType) ? Item_Define.Item_Make_Inventory_Type[setItemType] : Item_Define.eInventoryType.Account_Inven);
            if ((Item_Define.eInventoryType)setItem.inventory_type == Item_Define.eInventoryType.Character_Inven)
            {
                if (CID == 0)
                    return Result_Define.eResult.ITEM_EQUIP_NEED_CID;
                else
                    setItem.cid = CID;
            }
            else
                setItem.cid = 0;

            if (setItem.cid > 0)
            {
                Character charInfo = CharacterManager.GetCharacter(ref TB, AID, setItem.cid, false, dbkey);
                setItem.class_type = (byte)charInfo.Class;
            }
            else
                setItem.class_type = 0;

            setItem.itemid = ItemID;
            setItem.item_type = (short)setInven;
            setItem.itemea = 1;
            setItem.grade = setItem.grade = 0;
            setItem.equipposition = equipItemType;
            setItem.level = 0;
            setItem.equipflag = setItem.delflag = "N";
            setItem.newyn = "Y";
            setItem.base_option = new List<User_Inven_Option>();
            setItem.random_option = new List<User_Inven_Option>();
            MakeList.AddRange(MakeItemToDB(ref TB, ref setItem, makeCount, dbkey));

            MakeList.ForEach(item => { item.base_option = new List<User_Inven_Option>(); item.random_option = new List<User_Inven_Option>();});

            return MakeList.Count > 0 ? Result_Define.eResult.SUCCESS : Result_Define.eResult.ITEM_CREATE_FAIL;
        }

        private static List<User_Inven_Option> Make_ItemOption(ref TxnBlock TB, Item_Define.eItemType ItemType, int Grade, int Tier, out Result_Define.eResult retError, string dbkey = Item_Define.Item_InvenDB)
        {
            string OptionType = Item_Define.ItemTypeToOptionType[ItemType];

            int randomOptionCount = Item_Define.GradeOptionCount[OptionType][Grade];

            // cape has random count in same grade 
            if (ItemType == Item_Define.eItemType.ItemType_Cape && randomOptionCount > 0)
                randomOptionCount = TheSoul.DataManager.Math.GetRandomInt(1, Item_Define.ItemCape_RandomOptionMax);
            List<User_Inven_Option> retOptionList = new List<User_Inven_Option>();

            if (randomOptionCount > 0)
            {
                retOptionList = PickSystemItemOptionUseInfo(ref TB, ItemType, randomOptionCount, Grade, Tier);
                retError = retOptionList.Count > 0 ?  Result_Define.eResult.SUCCESS : Result_Define.eResult.ITEM_OPTION_ID_NOT_FOUND;
            }
            else
                retError = Result_Define.eResult.SUCCESS;

            return retOptionList;
        }


        public static Result_Define.eResult MakeEquipArray(ref TxnBlock TB, ref List<User_Inven> MakeList, long AID, List<User_Inven> ItemList, long CID)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            foreach (User_Inven setItem in ItemList)
            {
                List<User_Inven> retResult = new List<User_Inven>();
                retError = MakeEquip(ref TB, ref retResult, AID, setItem.itemid, setItem.itemea, CID, setItem.level, setItem.grade);
                if (retError == Result_Define.eResult.SUCCESS)
                    MakeList.AddRange(retResult);
                else
                    return retError;
            }
            return retError;
        }

        private static string GetEquipKey(long AID, long CID)
        {
            return string.Format("{0}_{1}_{2}_{3}", Item_Define.User_Inven_Prefix, Item_Define.User_Equip_Prefix, AID, CID);
        }

        public static List<User_Inven> GetEquipList(ref TxnBlock TB, long AID, long CID, bool bWithOption = true, bool Flush = false, string dbkey = Item_Define.Item_InvenDB)
        {
            string setEquipKey = GetEquipKey(AID, CID);

            List<User_Inven> UserItem = new List<User_Inven>();
            if (!Flush)
                UserItem = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis_MultipleRow<User_Inven>(DataManager_Define.RedisServerAlias_User, setEquipKey);

            if (UserItem == null || Flush || UserItem.Count == 0)
            {
                UserItem = GetCharacterInvenList(ref TB, AID, CID, bWithOption, Flush, dbkey);
                UserItem = UserItem.Where(item => item.equipflag.Equals("Y")).ToList();
            }

            return UserItem;
        }

        //public static List<User_Inven> GetEquipList_OnlyDB(ref TxnBlock TB, long AID, long CID, bool bWithOption = true, bool Flush = false, string dbkey = Item_Define.Item_InvenDB)
        //{
        //    List<User_Inven> UserItem = new List<User_Inven>();

        //    UserItem = GetCharacterInvenList(ref TB, AID, CID, bWithOption, Flush, dbkey);
        //    UserItem = UserItem.Where(item => item.equipflag.Equals("Y")).ToList();
        //    return UserItem;
        //}

        private static Result_Define.eResult UpdateEquipItem(ref TxnBlock TB, bool EquipFlag, bool isAccessory, long AID, long invenseq, long CID, string dbkey = Item_Define.Item_InvenDB)
        {
            string setQuery = string.Format("UPDATE {0} SET equipflag = '{1}', cid = '{2}' WHERE AID = {3} AND invenseq = {4};",
                                            Item_Define.Item_User_Inven_Table, EquipFlag ? "Y" : "N", isAccessory && !EquipFlag ? 0 : CID, AID, invenseq);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult UnEquip(ref TxnBlock TB, long AID, long invenseq, long CID, ref List<User_Inven> equipItem, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Inven> itemList = GetInvenList(ref TB, AID, CID);

            var findItem = itemList.Find(item => item.invenseq == invenseq);

            if (findItem != null)
            {
                System_Item_Equip sysInfo = GetSystem_Item_Equip(ref TB, findItem.itemid);

                Item_Define.eItemType setItemType = Item_Define.ItemType.ContainsKey(sysInfo.ItemType) ? (Item_Define.eItemType)Item_Define.ItemType[sysInfo.ItemType] : Item_Define.eItemType.None;
                Result_Define.eResult retError = UpdateEquipItem(ref TB, Item_Define.bUnEquip, Item_Define.ItemAccessoryTypeList.Contains(setItemType), AID, findItem.invenseq, CID);

                if (setItemType == Item_Define.eItemType.ItemType_Cape && retError == Result_Define.eResult.SUCCESS)
                    retError = ItemManager.EquipCostumeFlag(ref TB, AID, CID, false);

                if (retError == Result_Define.eResult.SUCCESS)
                    equipItem = GetEquipList(ref TB, AID, CID, true, true);

                return retError;
            }
            else
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;                        
        }
        // item costume equip flag set to DB
        private static Result_Define.eResult EquipCostumeFlag(ref TxnBlock TB, long AID, long CID, bool bEquip, string dbkey = Item_Define.Item_InvenDB)
        {
            string setQuery = string.Format(@"MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON aid = {1} AND cid = {2}
                                                WHEN MATCHED THEN
                                                    UPDATE SET 
                                                        equipflag = '{3}'
                                                WHEN NOT MATCHED THEN
                                                    INSERT (aid, cid, equipflag) VALUES('{1}', '{2}', '{3}');"
                                            , Item_Define.Item_User_Character_VIP_Costume_Table, AID, CID, bEquip ? 'Y' : 'N');
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        // item equip to character
        public static Result_Define.eResult EquipItemToCharacter(ref TxnBlock TB, long AID, long invenseq, long CID, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Inven> EquipList = new List<User_Inven>();
            return EquipItemToCharacter(ref TB, AID, invenseq, CID, ref EquipList, dbkey);
        }

        public static Result_Define.eResult EquipItemToCharacter(ref TxnBlock TB, long AID, long invenseq, long CID, ref List<User_Inven> equipItem, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Inven> itemList = GetInvenList(ref TB, AID, CID);

            var findItem = itemList.Find(item => item.invenseq == invenseq);

            if (findItem != null)
            {
                var equiplist = itemList.Where(item => item.equipflag.Equals("Y") && item.cid == CID).ToList();
                var currentEquip = equiplist.FindAll(item => item.equipposition.Equals(findItem.equipposition));
                Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
                bool bFlushAll = false;
                Item_Define.eItemType setItemType = Item_Define.eItemType.None;
                if (currentEquip != null)
                {
                    foreach (User_Inven currentItem in currentEquip)
                    {
                        System_Item_Equip sysInfo = GetSystem_Item_Equip(ref TB, currentItem.itemid);
                        setItemType = (Item_Define.eItemType)Item_Define.ItemType[sysInfo.ItemType];
                        retError = UpdateEquipItem(ref TB, Item_Define.bUnEquip, Item_Define.ItemAccessoryTypeList.Contains(setItemType), AID, currentItem.invenseq, CID);
                        if (!bFlushAll)
                            bFlushAll = Item_Define.ItemAccessoryTypeList.Contains(setItemType);
                    }
                }

                // equip set new item;
                if (CID > 0 && retError == Result_Define.eResult.SUCCESS)
                {
                    Character charInfo = CharacterManager.GetCharacter(ref TB, AID, CID);
                    System_Item_Equip sysInfo = GetSystem_Item_Equip(ref TB, findItem.itemid);
                    setItemType = (Item_Define.eItemType)Item_Define.ItemType[sysInfo.ItemType];

                    if (!bFlushAll)
                        bFlushAll = Item_Define.ItemAccessoryTypeList.Contains(setItemType);

                    if (charInfo.level < sysInfo.EquipLevel)
                        retError = Result_Define.eResult.ITEM_EQUIP_NOT_ENOUGH_LEVEL;

                    if (setItemType == Item_Define.eItemType.ItemType_Cape && retError == Result_Define.eResult.SUCCESS)
                        retError = ItemManager.EquipCostumeFlag(ref TB, AID, CID, true);

                    if (retError == Result_Define.eResult.SUCCESS)
                        retError = UpdateEquipItem(ref TB, Item_Define.bEquip, false, AID, findItem.invenseq, CID);

                    if (retError == Result_Define.eResult.SUCCESS)
                    {
                        if (bFlushAll)
                            FlushInvenList(ref TB, AID);
                        else
                            FlushCharacterInvenList(AID, CID);
                    }

                    return retError;
                }
                else
                    return Result_Define.eResult.CHARACTER_NOT_FOUND;
            }
            else
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;
        }

        public static Result_Define.eResult UseItem(ref TxnBlock TB, long AID, long ItemID, int UseCount, ref List<Return_DisassableItems_List> DeletedItemList, string dbkey = Item_Define.Item_InvenDB)
        {
            if (UseCount < 1)
                return Result_Define.eResult.SUCCESS;

            List<User_Inven> itemList = GetInvenList(ref TB, AID);

            int CurrentCount = 0;
            List<long> targetInvenSeq = new List<long>();

            itemList.Where(item => item.itemid == ItemID).ToList().ForEach(item =>
            {
                if (CurrentCount < UseCount)
                {
                    CurrentCount += item.itemea;
                    targetInvenSeq.Add(item.invenseq);
                }
            }
            );

            if (CurrentCount < UseCount)
                return Result_Define.eResult.NOT_ENOUGH_USE_ITEM;

            string setItemLogJson = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.item_log_list]);
            string setQuery = "";
            foreach (long updateSeq in targetInvenSeq)
            {
                var findItem = itemList.Find(item => item.invenseq == updateSeq);
                int setea = findItem.itemea;
                if(setea > UseCount )
                {
                    setea = findItem.itemea - UseCount;
                    UseCount = 0;
                }else
                {
                    setea = 0;
                    UseCount -= findItem.itemea;
                }
                string delflag = setea > 0 ? "N" : "Y";

                setQuery = setQuery + string.Format("UPDATE {0} SET itemea = {1}, delflag = '{2}' WHERE AID = {3} AND invenseq = {4};", Item_Define.Item_User_Inven_Table, setea, delflag, AID, updateSeq);
                Return_DisassableItems_List retItem = new Return_DisassableItems_List();
                retItem.itemseq = updateSeq;
                retItem.itemid = ItemID;
                retItem.itemea = setea;
                DeletedItemList.Add(retItem);
                ItemLogInfo setLog = new ItemLogInfo(ItemID, (int)SnailLog_Define.Snail_Money_Event_type.use, UseCount, CurrentCount, CurrentCount - UseCount
                                                        , findItem.invenseq, findItem.class_type, findItem.grade, findItem.level, findItem.equipposition);
                setItemLogJson = mJsonSerializer.AddJsonArray(setItemLogJson, mJsonSerializer.ToJsonString(setLog));
            }

            TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_item_log]);
            if (SystemData.GetServiceArea(ref TB) != DataManager_Define.eCountryCode.China)
                TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.mseed_item_log]);
            TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.item_log_list], setItemLogJson);

            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        private static Result_Define.eResult DeleteItem(ref TxnBlock TB, long AID, long ItemSeq, ref List<Return_DisassableItems_List> DeletedItemList, int deleteCount = 1, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Inven> itemList = GetInvenList(ref TB, AID);

            var findItem = itemList.Find(item => item.invenseq == ItemSeq);
            if (findItem == null)
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            int CurrentCount = 0;

            string setQuery = "";
            if (findItem.itemea < deleteCount)
                return Result_Define.eResult.NOT_ENOUGH_USE_ITEM;
            else
            {
                CurrentCount = findItem.itemea;

                if (findItem.itemea > deleteCount)
                    findItem.itemea = findItem.itemea - deleteCount;
                else
                    findItem.itemea = 0;
                string delflag = findItem.itemea > 0 ? "N" : "Y";

                setQuery = setQuery + string.Format("UPDATE {0} SET itemea = {1}, delflag = '{2}' WHERE AID = {3} AND invenseq = {4};", Item_Define.Item_User_Inven_Table, findItem.itemea, delflag, AID, findItem.invenseq);
                Return_DisassableItems_List retItem = new Return_DisassableItems_List();
                retItem.itemseq = findItem.invenseq;
                retItem.itemid = findItem.itemid;
                retItem.itemea = findItem.itemea;
                DeletedItemList.Add(retItem);
            }
            
            string setItemLogJson = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.item_log_list]);

            TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_item_log]);
            if (SystemData.GetServiceArea(ref TB) != DataManager_Define.eCountryCode.China)
                TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.mseed_item_log]);
            ItemLogInfo setLog = new ItemLogInfo(findItem.itemid, (int)SnailLog_Define.Snail_Money_Event_type.use, deleteCount, CurrentCount, CurrentCount - deleteCount
                                                        , findItem.invenseq, findItem.class_type, findItem.grade, findItem.level, findItem.equipposition);
            setItemLogJson = mJsonSerializer.AddJsonArray(setItemLogJson, mJsonSerializer.ToJsonString(setLog));

            TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.item_log_list], setItemLogJson);

            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static void ItemDeleteAll(ref TxnBlock TB, long AID, long CID, long ItemID = 0, string dbkey = Item_Define.Item_InvenDB)
        {
            string setQuery = ItemID > 0 ? string.Format("UPDATE {0} SET delflag = 'Y' WHERE AID = {1} AND CID = {2} AND itemid = {3}", Item_Define.Item_User_Inven_Table, AID, CID, ItemID) :
                string.Format("UPDATE {0} SET delflag = 'Y' WHERE AID = {1} AND CID = {2}", Item_Define.Item_User_Inven_Table, AID, CID);
            TB.ExcuteSqlCommand(dbkey, setQuery);

            FlushInvenList(ref TB, AID);
        }

        // update change itemid , grade, level
        private static Result_Define.eResult UpdateItemInfo(ref TxnBlock TB, User_Inven setItem, string dbkey = Item_Define.Item_InvenDB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET 
                                                    itemid = {1},
                                                    grade = {2},
                                                    level = {3}
                                            WHERE invenseq = {4} AND aid = {5} AND cid = {6}
                                            ",
                                             Item_Define.Item_User_Inven_Table,
                                             setItem.itemid,
                                             setItem.grade,
                                             setItem.level,
                                             setItem.invenseq,
                                             setItem.aid,
                                             setItem.cid
                );
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        // update change itemid , grade, level
        private static Result_Define.eResult UpdateOptionInfo(ref TxnBlock TB, User_Inven_Option setOption, string dbkey = Item_Define.Item_InvenDB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET 
                                                    optiontype = '{1}',
                                                    option_value = {2},
                                                    option_add_value = {3},
                                                    option_grade = {4},
                                                    option_level = {5},
                                                    option_exp = {6},
                                                    delflag = '{7}'
                                            WHERE optionseq = {8} AND invenseq = {9}
                                            ",
                                             Item_Define.Item_User_Inven_Option_Table,
                                             setOption.optiontype,
                                             setOption.option_value,
                                             setOption.option_add_value,
                                             setOption.option_grade,
                                             setOption.option_level,
                                             setOption.option_exp,
                                             setOption.delflag,
                                             setOption.optionseq,
                                             setOption.invenseq
                );
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        // update change itemid , grade, level
        private static Result_Define.eResult UpdateOptionInvenSeq(ref TxnBlock TB, User_Inven_Option setOption, string dbkey = Item_Define.Item_InvenDB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET 
                                                    invenseq = {1}
                                            WHERE optionseq = {2}
                                            ",
                                             Item_Define.Item_User_Inven_Option_Table,
                                             setOption.invenseq,
                                             setOption.optionseq
                );
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult SellItem_InGame(ref TxnBlock TB, long AID, ref int SellPrice, ref List<Return_DisassableItems_List> retDeletedItem, List<long> material_itemSeq_List, int sellCount = 1, bool ignoreSell = false, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Inven> itemList = GetInvenList(ref TB, AID);
            SellPrice = 0;
            foreach (long targetItemID in material_itemSeq_List)
            {
                var findItem = itemList.Find(item => item.invenseq == targetItemID);

                if (findItem == null)
                    return Result_Define.eResult.ITEM_ID_NOT_FOUND;

                System_Item_Base baseItemInfo = GetSystem_Item_Base(ref TB, findItem.itemid);
                if (baseItemInfo.Sell_Money <= 0 && !ignoreSell)
                    return Result_Define.eResult.ITEM_SELL_FAIL_NEED_IGNORE;
                SellPrice += (baseItemInfo.Sell_Money * sellCount);

                if (ItemManager.DeleteItem(ref TB, AID, targetItemID, ref retDeletedItem, sellCount) != Result_Define.eResult.SUCCESS)
                    return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }

            FlushInvenList(ref TB, AID);
            return Result_Define.eResult.SUCCESS;
        }

        public static User_Inven_Count GetUserInvenCount(ref TxnBlock TB, long AID, long CID, string dbkey = Item_Define.Item_InvenDB)
        {
            string setQuery = string.Format(@"
                                                SELECT 	
	                                                Account_Inven_Count = (SELECT COUNT(*) FROM {0} WITH(NOLOCK) WHERE aid = {1} AND cid = 0 AND delflag='N' AND itemea > 0 AND equipflag='N'),
	                                                Character_Inven_Count = (SELECT COUNT(*) FROM {0} WITH(NOLOCK) WHERE aid = {1} AND cid = {2} AND delflag='N' AND itemea > 0 AND equipflag='N')                                                
                                        ", Item_Define.Item_User_Inven_Table, AID, CID);
            User_Inven_Count retObj = GenericFetch.FetchFromDB<User_Inven_Count>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new User_Inven_Count();
        }

/*
        // Item Option Change
        public static Result_Define.eResult ItemTuning(ref TxnBlock TB, long AID, long CID, long ItemID, short OptionPos, ref List<Return_DisassableItems_List> retDeletedItem, bool isValueChange = false, string dbkey = InvenDBName)
        {
            Dictionary<long, User_Inven> itemList = GetInvenList(ref TB, AID, CID, dbkey);
            if (!itemList.ContainsKey(ItemID))
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            System_ITEM_EQUIP baseItemInfo = GetSystemInfo_EquipItem(ref TB, itemList[ItemID].itemid);

            Item_Define.eItemType setItemType = (Item_Define.eItemType)Item_Define.ItemType[baseItemInfo.ItemType];
            long NeedItemID = 0;
            int NeedItemCount = 0;

            short baseTier = baseItemInfo.Tier;
            short baseGrade = itemList[ItemID].enchant_grade;
            short baseLevel = itemList[ItemID].enchant_level;

            if (baseGrade < Item_Define.ItemMaxGradeLevel)
                return Result_Define.eResult.ITEM_EVOLUTION_GRADE_MAX;

            if (Item_Define.ItemArmorTypeList.Contains(setItemType))
            {
                System_ITEM_NEEDEXP baseNeedExpInfo = GetSystemInfo_NeedExp(ref TB, baseItemInfo.GrowthTableID2, baseGrade, baseLevel);
                NeedItemID = baseNeedExpInfo.OptionChange_NeedItemID;
                NeedItemCount = baseNeedExpInfo.OptionChange_NeedItemCount;
            }
            else if (Item_Define.ItemWeaponTypeList.Contains(setItemType))
            {
                System_ITEM_EVOL_WEAPON baseEvolutionInfo = GetSystemInfo_Evolution_WPN(ref TB, baseItemInfo.GrowthTableID1, baseTier, baseGrade);
                NeedItemID = baseEvolutionInfo.OptionChange_NeedItemID;
                NeedItemCount = baseEvolutionInfo.OptionChange_NeedItemCount;
            }
            else if (Item_Define.ItemAccessoryTypeList.Contains(setItemType))
            {
                System_ITEM_EVOL_ACCESSORY baseEvolutionInfo = GetSystemInfo_Evolution_Accessory(ref TB, baseItemInfo.GrowthTableID1, baseGrade);
                NeedItemID = isValueChange ? baseEvolutionInfo.ValueChange_NeedItemID : baseEvolutionInfo.OptionChange_NeedItemID;
                NeedItemCount = isValueChange ? baseEvolutionInfo.ValueChange_NeedItemCount : baseEvolutionInfo.OptionChange_NeedItemCount;
            }
            else
                return Result_Define.eResult.ITEM_TUNING_TYPE_INVALIDE;

            Result_Define.eResult retUseResult = ItemManager.UseItem(ref TB, AID, NeedItemID, NeedItemCount, ref retDeletedItem);
            if (retUseResult != Result_Define.eResult.SUCCESS)
                return retUseResult;

            List<User_Inven_Option_type> currentOptionList = new List<User_Inven_Option_type>();

            if (!string.IsNullOrEmpty(itemList[ItemID].optiontype1)) currentOptionList.Add(new User_Inven_Option_type(itemList[ItemID].optiontype1, itemList[ItemID].optionvalue1));
            if (!string.IsNullOrEmpty(itemList[ItemID].optiontype2)) currentOptionList.Add(new User_Inven_Option_type(itemList[ItemID].optiontype2, itemList[ItemID].optionvalue2));
            if (!string.IsNullOrEmpty(itemList[ItemID].optiontype3)) currentOptionList.Add(new User_Inven_Option_type(itemList[ItemID].optiontype3, itemList[ItemID].optionvalue3));
            if (!string.IsNullOrEmpty(itemList[ItemID].optiontype4)) currentOptionList.Add(new User_Inven_Option_type(itemList[ItemID].optiontype4, itemList[ItemID].optionvalue4));
            if (!string.IsNullOrEmpty(itemList[ItemID].optiontype5)) currentOptionList.Add(new User_Inven_Option_type(itemList[ItemID].optiontype5, itemList[ItemID].optionvalue5));

            if (currentOptionList.Count >= OptionPos && !isValueChange) 
                currentOptionList.RemoveAt(OptionPos-1);

            bool setAddOption = true;
            while (setAddOption)
            {
                List<User_Inven_Option_type> getOptionList = GetSystemItemOption(ref TB, setItemType, baseGrade, baseTier, dbkey);
                if (isValueChange)
                {
                    if (getOptionList.Count > 0)
                    {
                        foreach (User_Inven_Option_type setOption in getOptionList)
                        {
                            if (currentOptionList[OptionPos - 1].optiontype == setOption.optiontype)
                            {
                                currentOptionList[OptionPos - 1].optionvalue = setOption.optionvalue;
                                setAddOption = false;
                            }
                        }
                    }
                    else
                        setAddOption = false;
                }
                else
                {
                    if (getOptionList.Count > 0)
                    {
                        foreach (User_Inven_Option_type setOption in getOptionList)
                        {
                            if (getOptionList.Count > (currentOptionList.Count - itemList[ItemID].fixoption))
                            {
                                bool isAdd = true;
                                foreach (User_Inven_Option_type haveOption in currentOptionList)
                                {
                                    if (haveOption.optiontype.Equals(setOption.optiontype))
                                    {
                                        isAdd = false;
                                        break;
                                    }
                                }
                                if (isAdd)
                                    currentOptionList.Insert(OptionPos - 1, setOption);
                            }
                            else
                                setAddOption = false;
                        }
                    }
                    else
                        setAddOption = false;
                }
            }

            User_Inven setItem = new User_Inven();
            int setOptionPos = 0;
            foreach (User_Inven_Option_type setOption in currentOptionList)
            {
                if (setOptionPos < Item_Define.Item_MaxOptionCount)
                {
                    setOptionPos++;
                    setItem[string.Format("{0}{1}", Item_Define.Item_OptionTypeColumn, setOptionPos)] = setOption.optiontype;
                    setItem[string.Format("{0}{1}", Item_Define.Item_OptionValueColumn, setOptionPos)] = setOption.optionvalue;
                }
            }

            for (setOptionPos++; setOptionPos <= Item_Define.Item_MaxOptionCount; setOptionPos++)
            {
                setItem[string.Format("{0}{1}", Item_Define.Item_OptionTypeColumn, setOptionPos)] = string.Empty;
                setItem[string.Format("{0}{1}", Item_Define.Item_OptionValueColumn, setOptionPos)] = 0;
            }

            string setQuery = string.Format(@"UPDATE {0} SET    enchant_grade = {1},
                                                                optiontype1 = '{2}', optionvalue1 = {3},
                                                                optiontype2 = '{4}', optionvalue2 = {5},
                                                                optiontype3 = '{6}', optionvalue3 = {7},
                                                                optiontype4 = '{8}', optionvalue4 = {9},
                                                                optiontype5 = '{10}', optionvalue5 = {11}
                                                    WHERE AID = {12} AND invenseq = {13}",
                                                    InvenDBTableName, baseGrade,
                                                    setItem.optiontype1, setItem.optionvalue1,
                                                    setItem.optiontype2, setItem.optionvalue2,
                                                    setItem.optiontype3, setItem.optionvalue3,
                                                    setItem.optiontype4, setItem.optionvalue4,
                                                    setItem.optiontype5, setItem.optionvalue5,
                                                    AID, ItemID);

            if (!TB.ExcuteSqlCommand(InvenDBName, setQuery))
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            FlushInvenList(ref TB, AID);

            return Result_Define.eResult.SUCCESS;
        }

        public static Result_Define.eResult UseItem_InGame(ref TxnBlock TB, long AID, long CID, long ItemID, int UseCount, ref List<Return_DisassableItems_List> retDeletedItem, ref User_Inven MakeItemInfo)
        {
            Dictionary<long, User_Inven> UserInvenList = GetInvenList(ref TB, AID, 0);
            if (!UserInvenList.ContainsKey(ItemID))
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            long baseItemID = UserInvenList[ItemID].itemid;

            Object SysItem = TheSoul.DataManager.ItemManager.GetSystemItemInfo(ref TB, baseItemID);

            TheSoul.DataManager.Item_Define.SystemItemType checkType = (TheSoul.DataManager.Item_Define.SystemItemType)((System_ITEM_BASE)SysItem).ClassNo;

            Result_Define.eResult retResult = Result_Define.eResult.USE_ITEM_TYPE_INVALIDE;
            switch (checkType)
            {
                //case TheSoul.DataManager.Item_Define.SystemItemType.ItemClass_Equip:
                //    UserItemIdx = MakeEquip(ref TB, AID, ItemID, makeCount, CID, makeType, dbkey);
                //    break;
                case TheSoul.DataManager.Item_Define.SystemItemType.ItemClass_Use:
                //case TheSoul.DataManager.Item_Define.SystemItemType.ItemClass_Info:
                //case TheSoul.DataManager.Item_Define.SystemItemType.ItemClass_Costume:
                //case TheSoul.DataManager.Item_Define.SystemItemType.ItemClass_Band:
                    System_ITEM_USE SetItemInfo = (System_ITEM_USE)SysItem;
                    if (Item_Define.ItemType.ContainsKey(SetItemInfo.ItemType))
                    {
                        if (Item_Define.ItemType[SetItemInfo.ItemType] == Item_Define.eItemType.ItemType_MakeCape)
                            retResult = UseCapePiece(ref TB, AID, CID, baseItemID, ref retDeletedItem, ref MakeItemInfo);
                    }
                    else
                    {
                        retResult = Result_Define.eResult.USE_ITEM_TYPE_INVALIDE;
                    }                    
                    break;
                default:
                    break;
            }

            return retResult;
        }

        public static Result_Define.eResult SellItem_InGame(ref TxnBlock TB, long AID, ref int SellPrice, ref List<Return_DisassableItems_List> retDeletedItem, List<long> material_itemID_List, int sellCount = 1, bool ignoreSell = false, string dbkey = InvenDBName)
        {
            Dictionary<long, User_Inven> UserInvenList = GetInvenList(ref TB, AID, 0);
            SellPrice = 0;
            foreach (long targetItemID in material_itemID_List)
            {
                if (!UserInvenList.ContainsKey(targetItemID))
                    return Result_Define.eResult.ITEM_ID_NOT_FOUND;

                System_ITEM_BASE baseItemInfo = GetItemBaseInfo(ref TB, UserInvenList[targetItemID].itemid);
                if (baseItemInfo.Sell_Money <= 0 && !ignoreSell)
                    return Result_Define.eResult.ITEM_SELL_FAIL_NEED_IGNORE;
                SellPrice += (baseItemInfo.Sell_Money * sellCount);
            }

            foreach (long targetItemID in material_itemID_List)
            {
                if (!ItemManager.DeleteItem(ref TB, AID, targetItemID, UserInvenList[targetItemID].itemid, ref retDeletedItem, sellCount))
                    return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }

            FlushInvenList(ref TB, AID);

            return Result_Define.eResult.SUCCESS;
        }
 */

    }
}
