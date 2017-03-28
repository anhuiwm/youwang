using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using TheSoul.DataManager.DBClass;

namespace TheSoul.DataManager
{
    public static partial class DropManager
    {
        // get system drop box group, drop box, drop group
        const string DropDBName = "sharding";
        const string System_Drop_Box_Group_TableName = "System_Drop_Box_Group";
        const string System_Drop_Box_Group_Prefix = "SystemDropBoxGroup";

        const string System_Gacha_Box_Group_TableName = "System_Gacha";

        private static System_Drop_Box_Group GetSystemInfo_DropBoxGroup(ref TxnBlock TB, long DropBoxGroupID, bool Flush = false, string dbkey = DropDBName)
        {
            string setKey = string.Format("{0}_{1}", System_Drop_Box_Group_Prefix, System_Drop_Box_Group_TableName);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE DropBoxGroupID = {1}", System_Drop_Box_Group_TableName, DropBoxGroupID);
            string memebrKey = DropBoxGroupID.ToString();
            System_Drop_Box_Group retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Drop_Box_Group>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, memebrKey, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Drop_Box_Group();
            return retObj;
        }

        private static System_Gacha_Box_Group GetSystemInfo_GachaBoxGroup(ref TxnBlock TB, int DropBoxType, int Level_MatchingID, bool Flush = false, string dbkey = DropDBName)
        {
            string setKey = string.Format("{0}_{1}", System_Drop_Box_Group_Prefix, System_Gacha_Box_Group_TableName);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE DropBoxType = {1} AND Level_MatchingID = {2}", System_Gacha_Box_Group_TableName, DropBoxType, Level_MatchingID);
            string memebrKey = string.Format("{0}_{1}", DropBoxType, Level_MatchingID);
            System_Gacha_Box_Group retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Gacha_Box_Group>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, memebrKey, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Gacha_Box_Group();
            return retObj;
        }


        const string System_Drop_Box_TableName = "System_Drop_Box";
        const string System_Drop_Box_Prefix = "SystemDropBox";

        const string System_Gacha_Box_TableName = "System_Gacha_Box";

        private static System_Drop_Box GetSystemInfo_DropBox(ref TxnBlock TB, long DropBoxID, bool isGacha = false, bool Flush = false, string dbkey = DropDBName)
        {
            string setTable = isGacha ? System_Gacha_Box_TableName : System_Drop_Box_TableName;
            string setKey = string.Format("{0}_{1}", System_Drop_Box_Prefix, setTable);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE DropBoxID = {1}", setTable, DropBoxID);
            string memebrKey = DropBoxID.ToString();
            System_Drop_Box retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Drop_Box>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, memebrKey, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Drop_Box();
            return retObj;
        }

        const string System_Drop_Group_TableName = "System_Drop_Group";
        const string System_Drop_Group_Prefix = "SystemDropGroup";
        const string System_Gacha_Group_TableName = "System_Gacha_Group";

        private static List<System_Drop_Group> GetSystemInfo_DropGroup(ref TxnBlock TB, long DropGroupID, bool isGacha = false, bool Flush = false, string dbkey = DropDBName)
        {
            string setTable = isGacha ? System_Gacha_Group_TableName : System_Drop_Group_TableName;
            string setKey = string.Format("{0}_{1}", System_Drop_Group_Prefix, setTable);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE DropGroupID = {1}", setTable, DropGroupID);
            string memebrKey = DropGroupID.ToString();
            List<System_Drop_Group> retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow_Hash<System_Drop_Group>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, memebrKey, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new List<System_Drop_Group>();
            return retObj;
        }
        
        private static List<System_Gacha_Best_DropGrop> GetSystemInfo_BestGachaDropGroup(ref TxnBlock TB, long DropGroupID, string dbkey = DropDBName)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK, INDEX(IDX_System_Gacha_Best_DropGroup)) WHERE DropGroupID = {1}", Shop_Define.Shop_Gacha_Best_DropGrop_TableName, DropGroupID);
            string memebrKey = DropGroupID.ToString();
            List<System_Gacha_Best_DropGrop> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<System_Gacha_Best_DropGrop>(ref TB, setQuery, dbkey);
            if (retObj == null)
                retObj = new List<System_Gacha_Best_DropGrop>();
            return retObj;
        }

        public static List<System_Drop_Group> GetDropResult(ref TxnBlock TB, long AID, long DropBoxGroupID, short setClass = 0, int dummyMakeCount = 0, float overRate = 0.0f, User_Inven dummyExcludeInfo = null)
        {
            System_Drop_Box_Group getDropBoxGroupInfo = GetSystemInfo_DropBoxGroup(ref TB, DropBoxGroupID);

            List<System_Drop_Box> setOpenBoxList = new List<System_Drop_Box>();
            if (getDropBoxGroupInfo.DropBox1ID > 0)
                setOpenBoxList.Add(GetSystemInfo_DropBox(ref TB, getDropBoxGroupInfo.DropBox1ID));

            if (getDropBoxGroupInfo.DropBox2ID > 0 && setClass == (short)Character_Define.SystemClassType.Class_Warrior)
                setOpenBoxList.Add(GetSystemInfo_DropBox(ref TB, getDropBoxGroupInfo.DropBox2ID));

            if (getDropBoxGroupInfo.DropBox3ID > 0 && setClass == (short)Character_Define.SystemClassType.Class_Swordmaster)
                setOpenBoxList.Add(GetSystemInfo_DropBox(ref TB, getDropBoxGroupInfo.DropBox3ID));

            if (getDropBoxGroupInfo.DropBox4ID > 0 && setClass == (short)Character_Define.SystemClassType.Class_Taoist)
                setOpenBoxList.Add(GetSystemInfo_DropBox(ref TB, getDropBoxGroupInfo.DropBox4ID));

            List<System_Drop_Group> getDropList = new List<System_Drop_Group>();
            foreach (System_Drop_Box setBox in setOpenBoxList)
            {
                if (setBox.DropGroup1ID > 0)
                    PickDropGroup(ref TB, AID, ref getDropList, setBox.DropGroup1ID, setBox.DropGroup1_Dropnum, dummyMakeCount, setBox.DropGroup1Type.Contains("Random"), overRate, false, dummyExcludeInfo);
                if (setBox.DropGroup2ID > 0)
                    PickDropGroup(ref TB, AID, ref getDropList, setBox.DropGroup2ID, setBox.DropGroup2_Dropnum, dummyMakeCount, setBox.DropGroup2Type.Contains("Random"), overRate, false, dummyExcludeInfo);
                if (setBox.DropGroup3ID > 0)
                    PickDropGroup(ref TB, AID, ref getDropList, setBox.DropGroup3ID, setBox.DropGroup3_Dropnum, dummyMakeCount, setBox.DropGroup3Type.Contains("Random"), overRate, false, dummyExcludeInfo);
                if (setBox.DropGroup4ID > 0)
                    PickDropGroup(ref TB, AID, ref getDropList, setBox.DropGroup4ID, setBox.DropGroup4_Dropnum, dummyMakeCount, setBox.DropGroup4Type.Contains("Random"), overRate, false, dummyExcludeInfo);
                if (setBox.DropGroup5ID > 0)
                    PickDropGroup(ref TB, AID, ref getDropList, setBox.DropGroup5ID, setBox.DropGroup5_Dropnum, dummyMakeCount, setBox.DropGroup5Type.Contains("Random"), overRate, false, dummyExcludeInfo);
            }
            /*
"Item: 아이템
Gold: 게임머니(골드)
Cash: 캐시코인(루비)
Key: 열쇠
Ticket: 티켓
Cash: 루비
Battlepint: 결투포인트(1vs1)
Partypoint: 협력포인트(협력전)
Honoroint: 명예포인트(난전)
Donationpoint: 공헌포인트(길드)
Expeditionpoint: 원정포인트(황금원정단)
Buff: 버프(Only Client)"
             */

            return getDropList;
        }

        public static List<System_Drop_Group> GetGachaResult(ref TxnBlock TB, ref int GachaGold, ref int GachaCash, int DropBoxType, int Level_MatchingID, short setClass = 0)
        {
            System_Gacha_Box_Group getDropBoxGroupInfo = GetSystemInfo_GachaBoxGroup(ref TB, DropBoxType, Level_MatchingID);

            GachaGold = getDropBoxGroupInfo.Gacha_Gold;
            GachaCash = getDropBoxGroupInfo.Gacha_Cash;

            List<System_Drop_Box> setOpenBoxList = new List<System_Drop_Box>();
            if (getDropBoxGroupInfo.DropBox1ID > 0)
                setOpenBoxList.Add(GetSystemInfo_DropBox(ref TB, getDropBoxGroupInfo.DropBox1ID, true));

            if (getDropBoxGroupInfo.DropBox2ID > 0 && setClass == (short)Character_Define.SystemClassType.Class_Warrior)
                setOpenBoxList.Add(GetSystemInfo_DropBox(ref TB, getDropBoxGroupInfo.DropBox2ID, true));

            if (getDropBoxGroupInfo.DropBox3ID > 0 && setClass == (short)Character_Define.SystemClassType.Class_Swordmaster)
                setOpenBoxList.Add(GetSystemInfo_DropBox(ref TB, getDropBoxGroupInfo.DropBox3ID, true));

            if (getDropBoxGroupInfo.DropBox4ID > 0 && setClass == (short)Character_Define.SystemClassType.Class_Taoist)
                setOpenBoxList.Add(GetSystemInfo_DropBox(ref TB, getDropBoxGroupInfo.DropBox4ID, true));

            List<System_Drop_Group> getDropList = new List<System_Drop_Group>();
            int dummyMakeCount = 0;
            float overRate = 0;

            foreach (System_Drop_Box setBox in setOpenBoxList)
            {
                if (setBox.DropGroup1ID > 0)
                    PickDropGroup(ref TB, 0, ref getDropList, setBox.DropGroup1ID, setBox.DropGroup1_Dropnum, dummyMakeCount, setBox.DropGroup1Type.Contains("Random"), overRate, true);
                if (setBox.DropGroup2ID > 0)
                    PickDropGroup(ref TB, 0, ref getDropList, setBox.DropGroup2ID, setBox.DropGroup2_Dropnum, dummyMakeCount, setBox.DropGroup2Type.Contains("Random"), overRate, true);
                if (setBox.DropGroup3ID > 0)
                    PickDropGroup(ref TB, 0, ref getDropList, setBox.DropGroup3ID, setBox.DropGroup3_Dropnum, dummyMakeCount, setBox.DropGroup3Type.Contains("Random"), overRate, true);
                if (setBox.DropGroup4ID > 0)
                    PickDropGroup(ref TB, 0, ref getDropList, setBox.DropGroup4ID, setBox.DropGroup4_Dropnum, dummyMakeCount, setBox.DropGroup4Type.Contains("Random"), overRate, true);
                if (setBox.DropGroup5ID > 0)
                    PickDropGroup(ref TB, 0, ref getDropList, setBox.DropGroup5ID, setBox.DropGroup5_Dropnum, dummyMakeCount, setBox.DropGroup5Type.Contains("Random"), overRate, true);
            }

            return getDropList;
        }

        const string User_Drop_AddProb_Info_TableName = "User_Drop_AddProb_Info";

        private static string GetRedisKeyUserDropInfo(long AID, long ProbID)
        {
            return string.Format("{0}_{1}_{2}", System_Drop_Group_Prefix, User_Drop_AddProb_Info_TableName, AID);
        }

        private static int GetUserDropInfo(ref TxnBlock TB, long AID, long ProbID, bool Flush = false, string dbkey = DropDBName)
        {
            if (ProbID < 1)
                return 0;

            string setKey = GetRedisKeyUserDropInfo(AID, ProbID);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE AID = {1} AND ProbID = {2}", User_Drop_AddProb_Info_TableName, AID, ProbID);
            User_Drop_AddProb_Info retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<User_Drop_AddProb_Info>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, ProbID.ToString(), setQuery, dbkey, Flush);
            return retObj != null ? retObj.AddProb : 0;
        }

        private static void RemoveCacheUserDropInfo(long AID, long ProbID)
        {
            string setKey = GetRedisKeyUserDropInfo(AID, ProbID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveHashItem(DataManager_Define.RedisServerAlias_User, setKey, ProbID.ToString());
        }

        private static Result_Define.eResult SetUserDropInfo(ref TxnBlock TB, long AID, long ProbID, int AddProb, bool bReset = false, string dbkey = DropDBName)
        {
            if (ProbID > 0 && AddProb == 0)
                bReset = true;
            else if (ProbID == 0)
                return Result_Define.eResult.SUCCESS;

            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON AID = @aid AND ProbID = @probid
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                    AddProb = CASE WHEN @reset = 1 THEN 0 ELSE AddProb + @addprob END
                                                WHEN NOT MATCHED THEN
                                                   INSERT ([AID], [ProbID], [AddProb])
                                                   VALUES (@aid, @probid, @addprob);
                                    ", User_Drop_AddProb_Info_TableName);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = setQuery;
            cmd.Parameters.AddWithValue("@aid", AID);
            cmd.Parameters.AddWithValue("@probid", ProbID);
            cmd.Parameters.AddWithValue("@addprob", bReset ? 0 : AddProb);
            cmd.Parameters.AddWithValue("@reset", bReset ? 1 : 0);
            RemoveCacheUserDropInfo(AID, ProbID);
            return TB.ExcuteSqlCommand(dbkey, ref cmd) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult PickBestGachaDropGroup(ref TxnBlock TB, long AID, ref List<System_Drop_Group> retDropGroupLIst, long DropGroupID, int makeCount = 1, int dummyMakeCount = 0)
        {
            int checkMakeCount = dummyMakeCount > 0 ? dummyMakeCount : makeCount;
            List<System_Gacha_Best_DropGrop> getDropGroupList = GetSystemInfo_BestGachaDropGroup(ref TB, DropGroupID);
            getDropGroupList = getDropGroupList.OrderByDescending(x => x.DropProb).ToList();

            var random = new Random();
            System_Drop_Group retDropGroup;

            while (checkMakeCount > 0 && getDropGroupList.Count > 0)
            {
                List<System_Drop_Group> setDropGroupList = new List<System_Drop_Group>();

                foreach (System_Drop_Group setDrop in getDropGroupList)
                {
                    int AddDropProb = setDrop.ProbID > 0 && AID > 0 ? GetUserDropInfo(ref TB, AID, setDrop.ProbID) : 0;
                    setDrop.DropProb += AddDropProb;

                    if (AddDropProb >= setDrop.MaxProb && setDrop.MaxProb > 0)
                    {
                        setDropGroupList.Clear();
                        setDropGroupList.Add(setDrop);
                        break;
                    }
                    else
                        setDropGroupList.Add(setDrop);
                }
                int Max = setDropGroupList.Sum(dropitem => dropitem.DropProb);
                int Min = 0;
                int curRate = TheSoul.DataManager.Math.GetRandomInt(Min, Max);
                int checkRate = 0;
                retDropGroup = new System_Drop_Group();

                foreach (System_Drop_Group setDropGroup in setDropGroupList)
                {
                    if (setDropGroup.DropProb > 0)
                    {
                        checkRate += setDropGroup.DropProb;
                        if (checkRate >= curRate)
                        {
                            retDropGroup = setDropGroup;
                            break;
                        }
                    }
                }
                if (retDropGroup.DropItemID > 0)
                {
                    retDropGroupLIst.Add(retDropGroup);
                    checkMakeCount--;
                }
                else
                    return Result_Define.eResult.PICK_DROP_GROUP_FAIL;
            }

            return Result_Define.eResult.SUCCESS;
        }

        // temp dummy overRate array float 10
        private static readonly float[] dummyOverRate = { 0.0f, 0.5f, 0.9f, 0.9f, 0.9f, 0.9f, 0.9f, 0.9f, 0.9f, 0.9f };

        private static Result_Define.eResult PickDropGroup(ref TxnBlock TB, long AID, ref List<System_Drop_Group> retDropGroupLIst, long DropGroupID, int makeCount, int dummyMakeCount = 0, bool beDuplicate = true, float overRate = 0.0f, bool isGacha = false, User_Inven dummyExcludeInfo = null)
        {
            int checkMakeCount = dummyMakeCount > 0 ? dummyMakeCount : makeCount;
            int overratepos = 0;
            List<System_Drop_Group> getDropGroupList = GetSystemInfo_DropGroup(ref TB, DropGroupID, isGacha);
            getDropGroupList = getDropGroupList.OrderByDescending(x => x.DropProb).ToList();
            List<System_Drop_Group> dummyRemoveList = new List<System_Drop_Group>();

            if (dummyExcludeInfo == null)
                dummyExcludeInfo = new User_Inven();

            System_Drop_Group retDropGroup = new System_Drop_Group();

            while (checkMakeCount > 0 && getDropGroupList.Count > 0)
            {
                float setOverRate = 0.0f;
                if (overRate <= 0.0f)
                {
                    setOverRate = (dummyMakeCount < checkMakeCount) ? overRate : ((overratepos >= dummyOverRate.Length) ? 0.0f : dummyOverRate[overratepos]);
                }
                
                List<System_Drop_Group> setDropGroupList = new List<System_Drop_Group>();

                foreach (System_Drop_Group setDrop in getDropGroupList)
                {
                    int AddDropProb = setDrop.ProbID > 0 && AID > 0 ? GetUserDropInfo(ref TB, AID, setDrop.ProbID) : 0;
                    setDrop.DropProb += AddDropProb;

                    if (AddDropProb >= setDrop.MaxProb && setDrop.MaxProb > 0)
                    {
                        setDropGroupList.Clear();
                        setDropGroupList.Add(setDrop);
                        break;
                    }
                    else
                        setDropGroupList.Add(setDrop);
                }
                int Max = setDropGroupList.Sum(dropitem => dropitem.DropProb);
                int Min = System.Convert.ToInt32(Max * setOverRate);
                int curRate = TheSoul.DataManager.Math.GetRandomInt(Min, Max);
                int checkRate = 0;
                retDropGroup = new System_Drop_Group();

                foreach (System_Drop_Group setDropGroup in setDropGroupList)
                {
                    if (setDropGroup.DropProb > 0)
                    {
                        checkRate += setDropGroup.DropProb;
                        if (checkRate >= curRate)
                        {
                            retDropGroup = setDropGroup;
                            break;
                        }
                    }
                }
                if (retDropGroup.DropItemID > 0)
                {
                    if (AID > 0)
                    {
                        Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
                        foreach (System_Drop_Group checkAddGroup in getDropGroupList)
                        {                            
                            retError = SetUserDropInfo(ref TB, AID, checkAddGroup.ProbID, checkAddGroup.AddProb, checkAddGroup.ProbID == retDropGroup.ProbID);
                            if (retError != Result_Define.eResult.SUCCESS)
                                return retError;
                        }
                    }

                    if ((!(retDropGroup.DropItemID == dummyExcludeInfo.itemid
                                && retDropGroup.DropItemGrade == dummyExcludeInfo.grade
                                && retDropGroup.DropItemLevel == dummyExcludeInfo.level)
                        || dummyExcludeInfo.itemid == 0)
                        )
                    {
                        retDropGroupLIst.Add(retDropGroup);
                        overratepos = (dummyMakeCount < checkMakeCount) ? overratepos : overratepos + 1;
                        checkMakeCount--;

                        if (!beDuplicate)
                        {
                            dummyRemoveList.Add(retDropGroup);
                            getDropGroupList.Remove(retDropGroup);
                        }
                    }
                    else if (!beDuplicate)
                    {
                        dummyRemoveList.Add(retDropGroup);
                        getDropGroupList.Remove(retDropGroup);
                    }
                }
                else
                    return Result_Define.eResult.PICK_DROP_GROUP_FAIL;
            }

            if (checkMakeCount > 0)
            {
                if (dummyRemoveList.Count >= checkMakeCount)
                    retDropGroupLIst.AddRange(dummyRemoveList.GetRange(0, checkMakeCount));
                else
                    retDropGroupLIst.AddRange(dummyRemoveList);                
            }
            return Result_Define.eResult.SUCCESS;
        }

        public static Result_Define.eResult MakeDropItem(ref TxnBlock TB, ref List<User_Inven> makeItem, System_Drop_Group setDrop, long AID, long CID)
        {
            int getValue = TheSoul.DataManager.Math.GetRandomInt(setDrop.DropMinNum, setDrop.DropMaxNum);
            if (getValue < 1)
                return Result_Define.eResult.SUCCESS;

            User_Inven setNonItem = new User_Inven();
            setNonItem.itemid = setDrop.DropItemID;
            setNonItem.itemea = getValue;            

            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            switch (setDrop.DropTargetType)
            {
                case "Gold":                    
                    retError = AccountManager.AddUserGold(ref TB, AID, getValue);
                    makeItem.Add(setNonItem);
                    break;
                case "Key":
                    retError = AccountManager.AddUserKey(ref TB, AID, getValue);
                    makeItem.Add(setNonItem);
                    break;
                case "Ticket":
                    retError = AccountManager.AddUserTicket(ref TB, AID, getValue);
                    makeItem.Add(setNonItem);
                    break;
                case "Cash":
                    retError = AccountManager.AddUserEventCash(ref TB, AID, getValue);
                    makeItem.Add(setNonItem);
                    break;
                case "Battlepoint":
                    retError = AccountManager.AddUserCombatPoint(ref TB, AID, getValue);
                    makeItem.Add(setNonItem);
                    break;
                case "Partypoint":
                    retError = AccountManager.AddUserCombatPoint(ref TB, AID, getValue);
                    makeItem.Add(setNonItem);
                    break;
                case "Honoroint":
                    retError = AccountManager.AddUserHonor(ref TB, AID, getValue);
                    makeItem.Add(setNonItem);
                    break;
                case "Expeditionpoint":
                    retError = AccountManager.AddUserExpeditionPoint(ref TB, AID, getValue);
                    makeItem.Add(setNonItem);
                    break;
                case "Item":
                    int makeCount = getValue;
                    short setLevel = System.Convert.ToInt16(setDrop.DropItemLevel);
                    short setGrade = System.Convert.ToInt16(setDrop.DropItemGrade);

                    retError = ItemManager.MakeItem(ref TB, ref makeItem, AID, setDrop.DropItemID, makeCount, CID, setLevel, setGrade);

                    makeItem.ForEach(item => item.itemea = getValue);
                    break;
            }

            return retError;
        }

        const int Drop_SoulParts_Limit = 10;

        public static System_Drop_Group CheckDropLimit(ref TxnBlock TB, System_Drop_Group setDrop, long AID, long CID, ref Result_Define.eResult retError, string dbkey = Item_Define.Item_InvenDB)
        {            
            Object SysItem = TheSoul.DataManager.ItemManager.GetSystemItemInfo(ref TB, setDrop.DropItemID, false, dbkey);

            if (SysItem == null)
            {
                retError = Result_Define.eResult.ITEM_ID_NOT_FOUND;
                return setDrop;
            }

            System_Item_Base setItemBaseInfo = (System_Item_Base)SysItem;
            if (setItemBaseInfo.Item_IndexID <= 0)
            {
                retError = Result_Define.eResult.ITEM_SYSTEM_ID_NOT_FOUND;
                return setDrop;
            }

            if (!Item_Define.SystemItemType.ContainsKey(setItemBaseInfo.ItemClass))
            {
                retError =  Result_Define.eResult.ITEM_INFO_TYPE_INVALIDE;
                return setDrop;
            }

            Item_Define.eSystemItemType checkType = Item_Define.SystemItemType[setItemBaseInfo.ItemClass];

            if (checkType == Item_Define.eSystemItemType.Soul_Parts)
            {
                System_Soul_Parts partsInfo = SoulManager.GetSoul_System_Soul_Parts(ref TB, setItemBaseInfo.Class_IndexID);
                //System_Soul_Active makeInfo = SoulManager.GetSoul_System_Soul_Active(ref TB, partsInfo.Soul_Group, Soul_Define.Soul_Base_Grade);
                
                List<User_ActiveSoul> userSoulList =  SoulManager.GetUser_ActiveSoul(ref TB, AID);

                var findSoul = userSoulList.Find(item => item.soulgroup == partsInfo.Soul_Group);

                if (findSoul != null)
                {
                    if (findSoul.grade > 0 && findSoul.level > 0 && findSoul.starlevel > 0)
                    {
                        int dropLimit = SystemData.GetConstValueInt(ref TB, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.Shop_Gacha_Soul_Count_Def_List[partsInfo.Create_Star_Level]]);

                        if (dropLimit > 0)
                        {
                            setDrop.DropMaxNum = dropLimit;
                            setDrop.DropMinNum = dropLimit;
                        }
                    }
                    else if (findSoul.soulparts_ea >= partsInfo.SoulPartsValue)
                    {
                        int dropLimit = SystemData.GetConstValueInt(ref TB, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.Shop_Gacha_SoulParts_Count_Def_List[partsInfo.Create_Star_Level]]);
                        if (dropLimit > 0)
                        {
                            setDrop.DropMaxNum = dropLimit;
                            setDrop.DropMinNum = dropLimit;
                        }
                    }                    
                }
            }
            return setDrop;
        }
    }
}
