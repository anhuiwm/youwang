using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Tools;
using TheSoul.DataManager.Global;
using TheSoulGMTool.DBClass;

namespace TheSoulGMTool
{
    public partial class GMDataManager
    {
        public static List<System_BOSS_RAID> GetSystemBossRaidList(ref TxnBlock TB, string dbkey = GMData_Define.ShardingDBName)
        {
            string setKey = string.Format("{0}_{1}", BossRaid_Define.BossRaid_Prefix, BossRaid_Define.BossRaid_Surfix);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)", BossRaid_Define.BossRaid_TableName);
            List<System_BOSS_RAID> retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow<System_BOSS_RAID>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey);
            if (retObj == null)
                retObj = new List<System_BOSS_RAID>();
            return retObj;
        }

        public static bool CheckTablEexist(ref TxnBlock TB, string tableName, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("Select COUNT(*)  as number From sys.objects WITH(NOLOCK) Where type = 'U' and name = '{0}'", tableName);
            GM_Number retObj = GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            return retObj.number == 0 ? true : false;
        }

        public static List<GM_SoulGroup> GetSoulActiveList(ref TxnBlock TB, bool useSoul = true, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            if (CheckTablEexist(ref TB, GMData_Define.AdminSystemSoulGroupActiveTable))
            {
                retError = CreateAdminSystemItemTable(ref TB);
            }
            if (retError == Result_Define.eResult.SUCCESS)
            {
                string setQuery = string.Format(@"Select A.SoulGroup, A.NamingCN, A.DescCN, isnull(B.hide, 0) as hide 
                                            From {0} as A WITH (NOLOCK) left outer join {1} as B WITH (NOLOCK) on A.SoulGroup = B.SoulGroup Where isnull(B.hide, 0) = {2}
                                            Group By A.SoulGroup, A.NamingCN, A.DescCN, isnull(B.hide, 0)"
                        , Soul_Define.Soul_System_Soul_Active_Table, GMData_Define.AdminSystemSoulGroupActiveTable, (useSoul ? 0 : 1));
                List<GM_SoulGroup> retObj = GenericFetch.FetchFromDB_MultipleRow<GM_SoulGroup>(ref TB, setQuery, dbkey);
                foreach (GM_SoulGroup item in retObj)
                {
                    item.SoulName = GetItmeName(ref TB, item.NamingCN);
                }
                return retObj;
            }
            else
                return new List<GM_SoulGroup>();
        }

        public static Result_Define.eResult SetSoulActive(ref Dictionary<long, TxnBlock> server, List<GM_SoulGroup> soulList, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                if (CheckTablEexist(ref TB, GMData_Define.AdminSystemSoulGroupActiveTable))
                {
                    retError = CreateAdminSystemItemTable(ref TB);
                }
                if (retError == Result_Define.eResult.SUCCESS)
                {

                    foreach (GM_SoulGroup item in soulList)
                    {
                        string setQuery = string.Format(@"MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON SoulGroup = '{1}'
                                                WHEN MATCHED THEN
                                                    UPDATE SET 
                                                    hide={2}
                                                WHEN NOT MATCHED THEN
                                                   INSERT (
                                                        [SoulGroup]
                                                       ,[hide]
                                                    )
                                                   VALUES (
                                                        '{1}'
                                                        ,'{2}'
                                                    );
                                                "
                                                , GMData_Define.AdminSystemSoulGroupActiveTable, item.SoulGroup, item.hide);
                        retError = tb.Value.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retError != Result_Define.eResult.SUCCESS)
                            break;
                    }
                }
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }
            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            return retError;
        }

        private static Result_Define.eResult CreateAdminSystemItemTable(ref TxnBlock TB, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format(@"CREATE TABLE [dbo].[{0}](
                                                    [SoulGroup] [bigint] NOT NULL,
                                                    [hide] [tinyint] NOT NULL,
                                                CONSTRAINT [PK_Admin_System_SoulGroup_Active] PRIMARY KEY CLUSTERED (
                                                    [SoulGroup] ASC
                                                )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                                                ) ON [PRIMARY]", GMData_Define.AdminSystemSoulGroupActiveTable);

            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static string GetItmeName(ref TxnBlock TB, string stringCN, string dbkey = GMData_Define.GmDBName)
        {
            string retData = "";
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Where StringCN='{1}'", GMData_Define.AdminStringNamingTable, stringCN);
            Admin_String_Naming retObj = GenericFetch.FetchFromDB<Admin_String_Naming>(ref TB, setQuery, dbkey);
            if (retObj != null)
            {
                string lang = GMDataManager.GetGmToolLanguage();
                retData = (string)retObj.GetType().GetProperty(lang).GetValue(retObj);
            }
            return retData;
        }

        public static List<Admin_System_Item> GetEquipItemList(ref TxnBlock TB, string equipClass, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQeury = string.Format(@"Select A.Item_IndexID, A.Name, A.Description,
                                            A.ItemClass, A.ClassNo, A.Class_IndexID, A.Buy_PriceType, A.Buy_PriceValue, A.Sell_Money, A.StackMAX 
                                            From {0} as A WITH (NOLOCK) left outer join {1} as B WITH (NOLOCK) on A.Item_IndexID = B.EquipItem_IndexID 
                                            left outer join {3} as D WITH (NOLOCK) on A.Item_IndexID = D.CostumeItem_IndexID 
                                            Where A.ItemClass in ('ItemClass_Equip', 'ItemClass_Costume') And (B.EquipClass = '{2}' or D.EquipClass = '{2}')"
                                            , GMData_Define.AdminSystemItemTable, Item_Define.Item_System_Item_Equip_Table
                                            , equipClass, Item_Define.Item_System_Item_Costume_Table);
            List<Admin_System_Item> retObj = GenericFetch.FetchFromDB_MultipleRow<Admin_System_Item>(ref TB, setQeury, dbkey);
            if (retObj != null && retObj.Count > 0)
            {
                foreach (Admin_System_Item item in retObj)
                {
                    string itemName = GMDataManager.GetItmeName(ref TB, item.Name);
                    if (!string.IsNullOrEmpty(itemName))
                        item.Description = itemName;
                }
            }
            else
                retObj = new List<Admin_System_Item>();
            return retObj;
        }

        public static List<Admin_System_Item> GetNonEquip_Accessory_ItemList(ref TxnBlock TB, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQeury = string.Format(@"Select A.Item_IndexID, A.Name,  A.Description,  
                                            A.ItemClass, A.ClassNo, A.Class_IndexID, A.Buy_PriceType, A.Buy_PriceValue, A.Sell_Money, A.StackMAX
                                            From {0} as A WITH (NOLOCK) left outer join {1} as B WITH (NOLOCK) on A.Item_IndexID = B.EquipItem_IndexID
                                            Where A.ItemClass <> 'ItemClass_Costume' And (B.EquipClass = 'All' or B.EquipClass is null)
                                            Order by A.Item_IndexID", GMData_Define.AdminSystemItemTable, Item_Define.Item_System_Item_Equip_Table);
            List<Admin_System_Item> retObj = GenericFetch.FetchFromDB_MultipleRow<Admin_System_Item>(ref TB, setQeury, dbkey);
            if (retObj != null && retObj.Count > 0)
            {
                foreach (Admin_System_Item item in retObj)
                {
                    string itemName =  GMDataManager.GetItmeName(ref TB, item.Name);
                    if (!string.IsNullOrEmpty(itemName))
                        item.Description = itemName;
                }
            }
            else
                retObj = new List<Admin_System_Item>();
            return retObj;
        }

        public static List<Admin_System_Item> GetItemClassItemList(ref TxnBlock TB, string itemType, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQeury = string.Format(@"Select A.Item_IndexID, A.Name, A.Description,  
                                            A.ItemClass, A.ClassNo, A.Class_IndexID, A.Buy_PriceType, A.Buy_PriceValue, A.Sell_Money, A.StackMAX
                                            From {0} as A WITH (NOLOCK) 
                                            Where A.ItemClass = '{1}'", 
                                            GMData_Define.AdminSystemItemTable, itemType);
            List<Admin_System_Item> retObj = GenericFetch.FetchFromDB_MultipleRow<Admin_System_Item>(ref TB, setQeury, dbkey);
            if (retObj != null && retObj.Count > 0)
            {
                foreach (Admin_System_Item item in retObj)
                {
                    string itemName = GMDataManager.GetItmeName(ref TB, item.Name);
                    if (!string.IsNullOrEmpty(itemName))
                        item.Description = itemName;
                }
            }
            else
                retObj = new List<Admin_System_Item>();
            return retObj;
        }

        public static List<Admin_System_Item> GetNoneEquipSoulItemList(ref TxnBlock TB, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQeury = string.Format(@"Select A.Item_IndexID, A.Name, A.Description,  
                                            A.ItemClass, A.ClassNo, A.Class_IndexID, A.Buy_PriceType, A.Buy_PriceValue, A.Sell_Money, A.StackMAX
                                            From {0} as A WITH (NOLOCK) left outer join {1} as B WITH (NOLOCK) on A.Item_IndexID = B.EquipItem_IndexID
                                            Where A.ItemClass <> 'ItemClass_Costume' And A.ItemClass <> 'Soul_Parts' And A.ItemClass <> 'Soul_Equip' And (B.EquipClass = 'All' or B.EquipClass is null)"
                                            , GMData_Define.AdminSystemItemTable, Item_Define.Item_System_Item_Equip_Table);
            List<Admin_System_Item> retObj = GenericFetch.FetchFromDB_MultipleRow<Admin_System_Item>(ref TB, setQeury, dbkey);
            if (retObj != null && retObj.Count > 0)
            {
                foreach (Admin_System_Item item in retObj)
                {
                    string itemName = GMDataManager.GetItmeName(ref TB, item.Name);
                    if (!string.IsNullOrEmpty(itemName))
                        item.Description = itemName;
                }
            }
            else
                retObj = new List<Admin_System_Item>();
            return retObj;
        }

        public static List<System_VIP_Level> GetSystemVIPLevelList(ref TxnBlock TB, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)", VIP_Define.VIP_ExpTableName);
            List<System_VIP_Level> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<System_VIP_Level>(ref TB, setQuery, dbkey);
            return retObj;
        }

        public static List<GM_ControlLog> GetGMToolLog(ref TxnBlock TB, string sdate = "", string edate = "", string dbkey = GMData_Define.GmDBName)
        {
            string condition = "";
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                condition = string.Format(" Where  CONVERT(varchar(10), regDate,121) >= '{0}' and  CONVERT(varchar(10), regDate,121) <= '{1}'", sdate, edate);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) {1} Order By regDate Desc", GMData_Define.GmControlLog, condition);
            List<GM_ControlLog> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GM_ControlLog>(ref TB, setQuery, dbkey);
            return retObj;
        }

        public static List<GM_ItemChargeLog> GetItemChargeLog(ref TxnBlock TB, string sdate = "", string edate = "", string dbkey = GMData_Define.GmDBName)
        {
            string condition = "";
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                condition = string.Format(" Where  CONVERT(varchar(10), regDate,121) >= '{0}' and  CONVERT(varchar(10), regDate,121) <= '{1}'", sdate, edate);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) {1} Order By regDate Desc", GMData_Define.GMItemChargeLogTable, condition);
            List<GM_ItemChargeLog> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GM_ItemChargeLog>(ref TB, setQuery, dbkey);
            return retObj;
        }

        public static Result_Define.eResult SetAdminConstValue(ref Dictionary<long, TxnBlock> server, string setKey, int value, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            //string setQuery = string.Format("Update {0} Set value={1} Where ConstDefine='{2}'", SystemData_Define.AdminConstTableName, value, setKey);
            string setQuery = string.Format(@"MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON ConstDefine = '{2}'
                                                WHEN MATCHED THEN
                                                    UPDATE SET 
                                                    value={1}
                                                WHEN NOT MATCHED THEN
                                                   INSERT (
                                                        [ID]
                                                       ,[value]
                                                       ,[ConstDefine]
                                                    )
                                                   VALUES (
                                                         (SELECT MAX(ID) +1 FROM {0})
                                                        ,'{1}'
                                                        ,'{2}'
                                                    );
                                                "
                                            , SystemData_Define.AdminConstTableName, value, setKey);
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                retError = tb.Value.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
                else
                {
                    SystemData.AdminConstValueFetchFromRedis(ref TB, setKey, dbkey, true);
                }
            }
            return retError;
        }

        public static Result_Define.eResult SetConstValue(ref Dictionary<long, TxnBlock> server, string setKey, int value, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            //string setQuery = string.Format("Update {0} Set value={1} Where ConstDefine='{2}'", SystemData_Define.ConstTableName, value, setKey);
            string setQuery = string.Format(@"MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON ConstDefine = '{1}'
                                                WHEN MATCHED THEN
                                                    UPDATE SET 
                                                    value={2}
                                                WHEN NOT MATCHED THEN
                                                   INSERT (
                                                        [ID]
                                                       ,[ConstDefine]
                                                       ,[value]
                                                    )
                                                   VALUES (
                                                         0
                                                        ,'{1}'
                                                        ,{2}
                                                    );
                                                "
                                            , SystemData_Define.AdminConstTableName, setKey, value);
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                retError = tb.Value.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
                else
                {
                    SystemData.GetConstValue(ref TB, setKey, dbkey, true);
                }
            }
            return retError;
        }

        public static Result_Define.eResult SetShopBuyUserInit(ref Dictionary<long, TxnBlock> server, long itemid, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            //string setQuery = string.Format("Update {0} Set Buy_Count = 0 WHERE Shop_Goods_ID = {1}", Shop_Define.Shop_User_Buy_TableName, itemid);
            string setQuery = string.Format(@"MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON Shop_Goods_ID = {1}
                                                WHEN MATCHED THEN
                                                    UPDATE SET 
                                                    Buy_Count = 0
                                                WHEN NOT MATCHED THEN
                                                   INSERT (
                                                        [AID]
                                                       ,[Shop_Goods_ID]
                                                       ,[TotalBuy_Count]
                                                       ,[Buy_Count]
                                                       ,[regdate]
                                                    )
                                                   VALUES (
                                                        0, {1}, 0, 0, getdate()
                                                    );
                                                "
                                            , Shop_Define.Shop_User_Buy_TableName, itemid);
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                List<User_Shop_Buy> buyUserList = ShopManager.GetUserBuyItemCount_BY_ShopGoodsID(ref TB, itemid, dbkey);
                retError = tb.Value.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
                else
                {
                    buyUserList.ForEach(item =>
                    {
                        string setKey = ShopManager.GetItemBuyItemKey(item.AID);
                        TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
                    });
                }
            }
            return retError;
        }

        public static Result_Define.eResult SetShopItemOnOff(ref Dictionary<long, TxnBlock> server, long itemid, int active, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            string setQuery = "";
            if (active > 0)
                setQuery = string.Format("Delete From {0} Where Shop_Goods_ID={1}", Shop_Define.Shop_Limit_TableName, itemid);
            else
                setQuery = string.Format("Insert into {0} (Shop_Goods_ID, Sale_Rate, SaleStartTime, SaleEndTime, SaleType, DefaultSaleType) Values ({1}, 0, dateadd(d,-2, getdate()), dateadd(d,-1, getdate()), {2}, {2})",
                                                Shop_Define.Shop_Limit_TableName, itemid, (int)Shop_Define.eShopSaleType.BuyOnceSale);
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                long count = GetShopItemCount(ref TB, itemid, dbkey);
                if ((count == 0 && active == 0) || (count > 0 && active > 0))
                    retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                else
                    retError = Result_Define.eResult.SUCCESS;

                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }

            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            return retError;
        }

        public static Result_Define.eResult SetBarckMarketItem(ref Dictionary<long, TxnBlock> server, System_Shop_Point item, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            //string setQuery = string.Format("Update {0} Set NameCN1 = N'{1}', ToolTipCN = N'{2}', itemClass = '{3}', itemID={4}, itemNum = {5}, Buy_PriceValue={6} Where Shop_Goods_ID = {7} ",
            //                                    Shop_Define.Shop_Type_TableList[Shop_Define.eShopType.BlackMarket], item.NameCN1, item.ToolTipCN, item.ItemClass, item.ItemID, item.ItemNum, item.Buy_PriceValue, item.Shop_Goods_ID);
            string setQuery = string.Format(@"MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON Shop_Goods_ID = '{7}'
                                                WHEN MATCHED THEN
                                                    UPDATE SET 
                                                    NameCN1 = N'{1}', ToolTipCN = N'{2}', itemClass = N'{3}', itemID={4}, itemNum = {5}, Buy_PriceValue={6}
                                                WHEN NOT MATCHED THEN
                                                   INSERT (
                                                        [Shop_Goods_ID]
                                                       ,[NameCN1]
                                                       ,[NameCN2]
                                                       ,[ToolTipCN]
                                                       ,[Desc]
                                                       ,[ItemClass]
                                                       ,[ItemID]
                                                       ,[ItemNum]
                                                       ,[Buy_PriceType]
                                                       ,[Buy_PriceValue]
                                                       ,[Type]
                                                       ,[Max_Buy]
                                                    )
                                                   VALUES (
                                                        N'{1}','xxx', N'{2}','xxx', N'{3}', '{4}', '{5}', 'PriceType_BlackMarketPoint', '{6}', 0, 0
                                                    );
                                                "
                                            , Shop_Define.Shop_Type_TableList[Shop_Define.eShopType.BlackMarket], item.NameCN1, item.ToolTipCN, item.ItemClass, item.ItemID, item.ItemNum, item.Buy_PriceValue, item.Shop_Goods_ID);
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }
            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);

            return retError;
        }
        
        private static long GetShopItemCount(ref TxnBlock TB, long itemid, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("Select Count(*) as count From {0} WITH(NOLOCK) Where Shop_Goods_ID = {1}", Shop_Define.Shop_Limit_TableName, itemid);
            Rank_Count retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<Rank_Count>(ref TB, setQuery, dbkey);
            if (retObj == null)
                retObj = new Rank_Count();
            return retObj.count;
        }

        public static List<System_Character_EXP> GetCharacterLevelList(ref TxnBlock TB, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)", CharacterManager.CharacterExpTableName);
            List<System_Character_EXP> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<System_Character_EXP>(ref TB, setQuery, dbkey);
            return retObj;
        }

        public static Result_Define.eResult SetCharacterLevel(ref TxnBlock TB, long AID, long CID, int level, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retErr = Result_Define.eResult.DB_ERROR;
            System_Character_EXP needExp = CharacterManager.GetSystemExp(ref TB, level, dbkey);
            if (needExp != null)
            {
                int addExp = System.Convert.ToInt32(needExp.ACC_exp);
                int Gold = 0;
                long curExp = CharacterManager.GetCharacter(ref TB, AID, CID, false, dbkey).totalexp;
                curExp = curExp * (-1);
                retErr = CharacterManager.UpdateCharacterInfo(ref TB, CID, AID, (int)curExp, Gold, dbkey, false, true);
                if (retErr == Result_Define.eResult.SUCCESS)
                {
                    retErr = CharacterManager.UpdateCharacterInfo(ref TB, CID, AID, addExp, Gold, dbkey, true, true);
                    if (retErr == Result_Define.eResult.SUCCESS)
                    {
                        CharacterManager.FlushCharacter(ref TB, AID, CID, dbkey);
                    }
                }
            }
            return retErr;
        }

        public static Result_Define.eResult SetMissionDungeon(ref TxnBlock TB, long AID, int wordID, int missionID, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            //mission list 체크
            if (wordID > 0 && missionID > 0)
            {
                int LoopCnt = 1;
                int LoopCnt2 = 1;
                while (LoopCnt < (wordID + 1))
                {
                    int calNum = 1;
                    while (LoopCnt2 < ((wordID * 10) - (10 - missionID) + 1))
                    {
                        if (calNum > 10)
                        {
                            break;
                        }
                        else
                        {
                            System_Mission_Stage stageInfo = Dungeon_Manager.GetSystem_MissionStageInfo(ref TB, LoopCnt2, dbkey);
                            User_Mission_Play userMissionInfo = Dungeon_Manager.GetUser_MissionInfo(ref TB, ref retError, AID, LoopCnt, LoopCnt2, false, dbkey);

                            userMissionInfo.rank = 3;
                            userMissionInfo.ClearTime = 10;

                            retError = Dungeon_Manager.UpdateMissionTask(ref TB, ref userMissionInfo, 1, dbkey);
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                LoopCnt2 = LoopCnt2 + 1;
                                calNum = calNum + 1;
                            }
                            else
                                break;
                        }
                    }
                    if (retError == Result_Define.eResult.SUCCESS)
                        LoopCnt = LoopCnt + 1;
                    else
                        break;
                }
            }

            return retError; 
        }

        public static Result_Define.eResult SetVIPLevel(ref TxnBlock TB, long AID, int level, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retErr = Result_Define.eResult.DB_ERROR;
            System_VIP_Level levelInfo = VipManager.GetSystem_VIP_Level(ref TB, (level - 1),false, dbkey);
            //string setQuery = string.Format("Update {0} Set VIPPoint = 0, TotalVIPPoint = 0 Where Aid={1}", VIP_Define.User_VIP_TableName, AID);
            string setQuery = string.Format(@"MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON Aid = {1}
                                                WHEN MATCHED THEN
                                                    UPDATE SET 
                                                    VIPPoint = 0
                                                    , TotalVIPPoint = 0
                                                WHEN NOT MATCHED THEN
                                                   INSERT (
                                                        [AID]
                                                       ,[VIPLevel]
                                                       ,[VIPPoint]
                                                       ,[TotalVIPPoint]
                                                       ,[regdate]
                                                    )
                                                   VALUES (
                                                        {1}, 0, 0, 0, getdate()
                                                    );
                                                "
                                            , VIP_Define.User_VIP_TableName, AID);
            retErr = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            if (retErr == Result_Define.eResult.SUCCESS)
            {
                retErr = VipManager.VIPPointAdd(ref TB, AID, levelInfo.LevelUpPoint, dbkey);
            }
            return retErr;
        }

        public static List<System_Shop_BlackMarket> GetBlackMarketList(ref TxnBlock TB, int slotID, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK, INDEX(IDX_GM_SlotList)) Where SlotID = {1} And delflag = 1 Order by Shop_Goods_ID desc", Shop_Define.Shop_Type_TableList[Shop_Define.eShopType.BlackMarket], slotID);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<System_Shop_BlackMarket>(ref TB, setQuery, dbkey);
        }

        private static Result_Define.eResult GetBlackMarketIndex(ref TxnBlock TB, ref long index, string dbkey = GMData_Define.GmDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            string setQuery = string.Format("Select ISNULL(MAX(idx),0) as number From {0} WITH(NOLOCK)", GMData_Define.AdminSystemShopBlackMarketndexTable);
            long maxNum  = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey).number;
            setQuery = string.Format("Select ISNULL(MAX(Shop_Goods_ID), 0) as number From {0} WITH(NOLOCK)", Shop_Define.Shop_Type_TableList[Shop_Define.eShopType.BlackMarket]);
            long shopMaxNum = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, GMData_Define.ShardingDBName).number;

            if (shopMaxNum > maxNum)
            {
                string setQuery3 = string.Format("Insert into {0} Values ({1}, getdate())", GMData_Define.AdminSystemShopBlackMarketndexTable, shopMaxNum);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery3) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                setQuery = string.Format("Select ISNULL(MAX(idx),10001) + 1 as number From {0} WITH(NOLOCK)", GMData_Define.AdminSystemShopBlackMarketndexTable);
                index = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey).number;

                string setQuery2 = string.Format("Insert into {0} Values ({1}, getdate())", GMData_Define.AdminSystemShopBlackMarketndexTable, index);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery2) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            }
            return retError;
        }

        private static int GetBlackMarketSoltIndex(ref TxnBlock TB, int slotID, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("Select ISNULL(MAX(SlotIndex),0) + 1 as number From {0} WITH(NOLOCK) Where SlotID = {1}", Shop_Define.Shop_Type_TableList[Shop_Define.eShopType.BlackMarket], slotID);
            return (int)TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey).number;
        }

        public static Result_Define.eResult InsertShopBlackMarket(ref Dictionary<long, TxnBlock> server, System_Shop_BlackMarket setdata, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            long goodsID = 0;
            TxnBlock TB = server.First().Value;
            retError = GetBlackMarketIndex(ref TB, ref goodsID);
            if (retError == Result_Define.eResult.SUCCESS)
            {
                string setQuery = "";
                foreach (KeyValuePair<long, TxnBlock> tb in server)
                {
                    TB = tb.Value;
                    setdata.SlotIndex = GetBlackMarketSoltIndex(ref TB, setdata.SlotID, dbkey);
                    setQuery = string.Format(@"Insert Into {0} (Shop_Goods_ID, GroupID, SlotID, SlotIndex, NameCN1, NameCN2, ToolTipCN, ItemClass, ItemID, ItemNum, Buy_PriceType, Buy_PriceValue, Type, Max_Buy, ItemProb, delflag) 
                                                Values ({1}, 1, {2}, {3}, N'{4}', 'xxx', N'{5}', N'{6}', {7}, {8}, 'PriceType_BlackMarketPoint', {9}, 0, 1, {10}, 1);",
                                                Shop_Define.Shop_Type_TableList[Shop_Define.eShopType.BlackMarket], goodsID, setdata.SlotID, setdata.SlotIndex, setdata.NameCN1,
                                                setdata.ToolTipCN, setdata.ItemClass, setdata.ItemID, setdata.ItemNum, setdata.Buy_PriceValue, setdata.ItemProb);

                    retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                    if (retError != Result_Define.eResult.SUCCESS)
                        break;
                }
            }

            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            
            return retError;
        }

        public static Result_Define.eResult DeleteShopBlackMarket(ref Dictionary<long, TxnBlock> server, long goodsID, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            string setQuery = string.Format(@"UPDATE {0} SET delflag = 0 Where Shop_Goods_ID = {1}",
                                            Shop_Define.Shop_Type_TableList[Shop_Define.eShopType.BlackMarket], goodsID);
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }
            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            
            return retError;
        }

        public static List<System_Gacha_Best> GetBestGachaList(ref TxnBlock TB, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Order by GachaIndex desc", Shop_Define.Shop_Gacha_Best_TableName);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<System_Gacha_Best>(ref TB, setQuery, dbkey);
        }

        public static System_Gacha_Best GetBestGachaInfo(ref TxnBlock TB, long gachaIndex, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Where GachaIndex = {1}", Shop_Define.Shop_Gacha_Best_TableName, gachaIndex);
            return TheSoul.DataManager.GenericFetch.FetchFromDB<System_Gacha_Best>(ref TB, setQuery, dbkey);
        }

        public static List<System_Gacha_Best_DropGrop> GetBestGachaDropGroupList(ref TxnBlock TB, long gachaIndex, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Where DropGroupID = {1}", Shop_Define.Shop_Gacha_Best_DropGrop_TableName, gachaIndex);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<System_Gacha_Best_DropGrop>(ref TB, setQuery, dbkey);
        }

        public static Result_Define.eResult InsertBestGacha(ref Dictionary<long, TxnBlock> server, System_Gacha_Best gachaInfo, List<System_Gacha_Best_DropGrop> droplist, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            long gaChaIndex = 0;
            TxnBlock TB = server.First().Value;
            retError = GetBestGachaIndex(ref TB, ref gaChaIndex);
            if (retError == Result_Define.eResult.SUCCESS)
            {
                string setQuery = string.Format(@"Insert Into {0} (GachaIndex, StartDate, EndDate, Gacha_Cash, Main_SoulItemID, Sub_SoulItemID_1, Sub_SoulItemID_2, Sub_SoulItemID_3)
                                                Values ({1}, N'{2}', N'{3}', {4}, {5}, {6}, {7}, {8})",
                                                Shop_Define.Shop_Gacha_Best_TableName, gaChaIndex, gachaInfo.StartDate.ToString("yyyy-MM-dd HH:mm:ss"), gachaInfo.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                                gachaInfo.Gacha_Cash, gachaInfo.Main_SoulItemID, gachaInfo.Sub_SoulItemID_1, gachaInfo.Sub_SoulItemID_2, gachaInfo.Sub_SoulItemID_3);
                foreach (KeyValuePair<long, TxnBlock> tb in server)
                {
                    TB = tb.Value;
                    retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                    if (retError != Result_Define.eResult.SUCCESS)
                        break;
                }

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    retError = SetBestGachaDropGroup(ref server, gaChaIndex, droplist);
                }
            }

            return retError;
        }

        public static Result_Define.eResult DeleteBestGacha(ref Dictionary<long, TxnBlock> server, long gachaIndex, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            string setQuery = string.Format(@"Delete From {0} Where GachaIndex = {1}",
                                            Shop_Define.Shop_Gacha_Best_TableName, gachaIndex);
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }
            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            return retError;
        }

        public static Result_Define.eResult UpdateBestGacha(ref Dictionary<long, TxnBlock> server, System_Gacha_Best gachaInfo, List<System_Gacha_Best_DropGrop> droplist, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            string setQuery = string.Format(@"MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON GachaIndex = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                StartDate = N'{2}', EndDate = N'{3}', Gacha_Cash = {4},
                                                    Main_SoulItemID = {5}, Sub_SoulItemID_1 = {6}, Sub_SoulItemID_2 = {7}, Sub_SoulItemID_3 = {8}
                                                WHEN NOT MATCHED THEN
                                                   INSERT (GachaIndex, StartDate, EndDate, Gacha_Cash, Main_SoulItemID, Sub_SoulItemID_1, Sub_SoulItemID_2, Sub_SoulItemID_3) Values ({1}, N'{2}', N'{3}', {4}, {5}, {6}, {7}, {8});",
                                            Shop_Define.Shop_Gacha_Best_TableName, gachaInfo.GachaIndex, gachaInfo.StartDate.ToString("yyyy-MM-dd HH:mm:ss"), gachaInfo.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                            gachaInfo.Gacha_Cash, gachaInfo.Main_SoulItemID, gachaInfo.Sub_SoulItemID_1, gachaInfo.Sub_SoulItemID_2, gachaInfo.Sub_SoulItemID_3);
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }
            if (retError == Result_Define.eResult.SUCCESS)
                retError = DeleteBestGachaDropGroup(ref server, gachaInfo.GachaIndex);
            if (retError == Result_Define.eResult.SUCCESS)
                retError = SetBestGachaDropGroup(ref server, gachaInfo.GachaIndex, droplist);

            return retError;
        }
        private static Result_Define.eResult DeleteBestGachaDropGroup(ref Dictionary<long, TxnBlock> server, long gachaIndex, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            string setQuery = string.Format(@"Delete From {0} Where DropGroupID = {1}",
                                            Shop_Define.Shop_Gacha_Best_DropGrop_TableName, gachaIndex);
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }
            return retError;
        }

        private static Result_Define.eResult SetBestGachaDropGroup(ref Dictionary<long, TxnBlock> server, long gachaIndex, List<System_Gacha_Best_DropGrop> droplist, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            string setQuery = "";
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                foreach (System_Gacha_Best_DropGrop item in droplist)
                {
                    setQuery = string.Format(@"Insert Into {0} (DropGroupID, DropIndex, DropTargetType, DropItemID, DropItemLevel, DropItemGrade, DropMinNum, DropMaxNum, DropProb)
                                                Values ({1}, {2}, N'{3}', {4}, {5}, {6}, {7}, {8}, {9})",
                                            Shop_Define.Shop_Gacha_Best_DropGrop_TableName, gachaIndex, item.DropIndex, item.DropTargetType,
                                            item.DropItemID, item.DropItemLevel, item.DropItemGrade, item.DropMinNum, item.DropMaxNum, item.DropProb);
                    retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                    if (retError != Result_Define.eResult.SUCCESS)
                        break;
                }
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }
            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            return retError;
        }

        private static Result_Define.eResult GetBestGachaIndex(ref TxnBlock TB, ref long index, string dbkey = GMData_Define.GmDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            string setQuery = string.Format("Select ISNULL(MAX(idx),0) as number From {0} WITH(NOLOCK)", GMData_Define.AdminSystemGachaBestIndexTable);
            long maxNum  = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey).number;
            setQuery = string.Format("Select ISNULL(MAX(GachaIndex), 0) as number From {0} WITH(NOLOCK)", Shop_Define.Shop_Gacha_Best_TableName);
            long gachaMaxNum = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, GMData_Define.ShardingDBName).number;

            if (gachaMaxNum > maxNum)
            {
                string setQuery3 = string.Format("Insert into {0} Values ({1}, getdate())", GMData_Define.AdminSystemGachaBestIndexTable, gachaMaxNum);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery3) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                setQuery = string.Format("Select ISNULL(MAX(idx),0) + 1 as number From {0} WITH(NOLOCK)", GMData_Define.AdminSystemGachaBestIndexTable);
                index = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey).number;

                string setQuery2 = string.Format("Insert into {0} Values ({1}, getdate())", GMData_Define.AdminSystemGachaBestIndexTable, index);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery2) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            }
            return retError;
        }

        public static List<string> GetBestGachaMaxDateServerList(ref Dictionary<long, TxnBlock> server, long gachaIndex, string setDate, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format(@"Select * From {0} WITH(NOLOCK, INDEX(IDX_System_Gacha_Best_Date)) Where StartDate <= '{1}' And EndDate >= '{1}' And GachaIndex != {2}", Shop_Define.Shop_Gacha_Best_TableName, setDate, gachaIndex);
            List<string> resultServerList = new List<string>();
            TxnBlock TB = server.First().Value;
            List<server_group_config> serverList = GlobalManager.GetServerGroupList(ref TB).FindAll(item => item.server_group_id > 0);
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TB = tb.Value;
                List<System_Gacha_Best> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<System_Gacha_Best>(ref TB, setQuery, dbkey);
                if (retObj.Count > 0)
                {
                    resultServerList.Add(serverList.Find(item => item.server_group_id == tb.Key).server_group_name);
                }
            }
            return resultServerList;
        }

        public static DateTime GetBestGachaMaxDate(ref Dictionary<long, TxnBlock> server, string dbkey = GMData_Define.ShardingDBName)
        {
            DateTime endDate = new DateTime();
            string setQuery = string.Format(@"Select Top 1 * From {0} WITH(NOLOCK) Order By GachaIndex Desc", Shop_Define.Shop_Gacha_Best_TableName);
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                System_Gacha_Best retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<System_Gacha_Best>(ref TB, setQuery, dbkey);
                if (retObj != null)
                {
                    int dateResult = DateTime.Compare(endDate, retObj.EndDate);
                    if (dateResult < 0)
                        endDate = retObj.EndDate;
                }
            }
            return endDate;
        }

        public static List<ListItem> GetHourList(bool emptyValue = true)
        {
            List<ListItem> retObj = new List<ListItem>();
            if (emptyValue)
                retObj.Add(new ListItem("select", "-1"));
            for (int i = 0; i <= 23; i++)
            {
                string time = i.ToString();
                if (i < 10)
                {
                    time = "0" + i;
                }
                var addData = new ListItem(time, time);
                retObj.Add(addData);
            }
            return retObj;
        }

        public static List<ListItem> GetMinList(int interval, bool emptyValue = true)
        {
            List<ListItem> retObj = new List<ListItem>();
            if (emptyValue)
                retObj.Add(new ListItem("select", "-1"));
            for (int i = 0; i <= 59; i = i + interval)
            {
                string time = i.ToString();
                if (i < 10)
                {
                    time = "0" + i;
                }
                var addData = new ListItem(time, time);
                retObj.Add(addData);
            }
            return retObj;
        }

        public static List<ListItem> GetSelectBoxList(int maxCount)
        {
            List<ListItem> retObj = new List<ListItem>();
            retObj.Add(new ListItem("select", "0"));
            for (int i = 1; i <= maxCount; i++)
            {
                var addData = new ListItem(i.ToString());
                retObj.Add(addData);
            }
            return retObj;
        }

        public static bool GetDateBetween(DateTime input, DateTime date1, DateTime date2)
        {
            return (input > date1 && input < date2);
        }

        public static void PopulatePager(ref DataList dlPager, long recordCount, int currentPage, int pageSize = GMData_Define.pageSize)
        {
            double dblPageCount = (double)((decimal)recordCount / pageSize);
            int pageCount = (int)System.Math.Ceiling(dblPageCount);
            int prePage = ((currentPage - 1) / GMData_Define.pageBlock) * GMData_Define.pageBlock + 1;
            int endPage = prePage + GMData_Define.pageBlock - 1;
            if (pageCount < endPage)
                endPage = pageCount;
            List<ListItem> pages = new List<ListItem>();
            if (pageCount > 0)
            {
                if (currentPage > GMData_Define.pageBlock)
                {

                    pages.Add(new ListItem("...", (prePage - 1).ToString()));
                }

                for (int i = prePage; i <= endPage; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }

                if (pageCount > endPage)
                {
                    pages.Add(new ListItem("...", (endPage + 1).ToString()));
                }
            }
            dlPager.DataSource = pages;
            dlPager.DataBind();
        }
    }
}