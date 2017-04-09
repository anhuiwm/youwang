using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
        public static string GetLogDecryptData(string encryptData)
        {
            string[] encrypt = System.Text.RegularExpressions.Regex.Split(encryptData, ",encryptKey=");
            string setKey = encrypt[1];
            Dictionary<string, string> jsonData = mJsonSerializer.JsonToDictionary(encrypt[0]);
            string decryptString = "";
            if (jsonData.ContainsKey("returndata"))
            {
                if (jsonData["result"] == "1")
                    TheSoulEncrypt.CompressionDecrypt(jsonData["returndata"], setKey, ref decryptString);
                else
                    TheSoulEncrypt.DecryptData(jsonData["returndata"], setKey, ref decryptString);
            }
            else
                decryptString = encrypt[0];
            return decryptString;
        }

        public static List<system_log_operation> GetOperation(ref TxnBlock TB, bool takeAll = false, string dbkey = GMData_Define.GmDBName)
        {
            string setLanguage = GetGmToolLanguage();
            string setQuery = takeAll ? string.Format("Select orderNum, url, Operation, ISNULL({1}, string) string, useflag From {0} With(nolock) order by orderNum ", GMData_Define.SystemLogOperation, setLanguage) :
                                        string.Format("Select orderNum, url, Operation, ISNULL({1}, string) string, useflag From {0} WITH(NOLOCK, INDEX(IDX_Search_useflag)) Where useflag = 1  order by orderNum", GMData_Define.SystemLogOperation, setLanguage);
            List<system_log_operation> retObj = GenericFetch.FetchFromDB_MultipleRow<system_log_operation>(ref TB, setQuery, dbkey);
            if (retObj == null || retObj.Count == 0)
                retObj = new List<system_log_operation>();
            return retObj;
        }

        public static long GetLoginCount(ref TxnBlock TB, string dbkey = GMData_Define.LogDBName)
        {
            string setQuery = string.Format("SELECT TOP 1 user_count as number FROM {0} ORDER BY idx DESC", string.Format("{0}_{1}", SnailLog_Define._CurrentUser_Log_tablename, System.Convert.ToDateTime(GetServerTime(ref TB)).ToString("yyyyMMdd")));
            
            GM_Number retObj = GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.number;
        }

        public static List<GM_String> GetLogTableName(ref TxnBlock TB, string table, string sdate, string edate, string dbkey = GMData_Define.LogDBName)
        {
            string setQuery = string.Format(@"select name from sys.objects 
                                                where type = 'U' and name like '{0}%'
                                                and Convert(varchar(10), dateadd(d,1,modify_date),121) >= '{1}' and Convert(varchar(10), modify_date,121) <= '{2}'
                                                order by create_date desc", table, sdate, edate);
            return GenericFetch.FetchFromDB_MultipleRow<GM_String>(ref TB, setQuery, dbkey);
        }

        private static List<GM_String> GetLogTableNameWeek(ref TxnBlock TB, string tableName, string sdate, string edate, string dbkey = GMData_Define.LogDBName)
        {
            string startDate = System.Convert.ToDateTime(sdate).AddDays(0 - (int)(System.Convert.ToDateTime(sdate).DayOfWeek)+1).ToString("yyyy-MM-dd");
            string endDate = System.Convert.ToDateTime(edate).AddDays(2 - (int)(System.Convert.ToDateTime(edate).DayOfWeek)).ToString("yyyy-MM-dd");
            string setQuery = string.Format(@"select name from sys.objects 
                                                where type = 'U' and name like '{0}%'
                                                and Convert(varchar(10), dateadd(d,1,modify_date),121) >= '{1}' and Convert(varchar(10), modify_date,121) <= '{2}'
                                                order by create_date desc", tableName, startDate, endDate);
            return GenericFetch.FetchFromDB_MultipleRow<GM_String>(ref TB, setQuery, dbkey);

        }

        public static List<_snail_money_log> GetUserRubyList(ref TxnBlock TB, long AID, int page, string sdate, string edate, int eventType = 0, int moneyType = 0, string dbkey = GMData_Define.LogDBName)
        {
            string condition1 = "";
            string condition2 = eventType >= 0 ? string.Format(" And n_event_type = {0} ", eventType) : "";
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                condition1 = string.Format(" And (CONVERT(varchar(10), d_create,121) >= '{0}' and CONVERT(varchar(10), d_create,121) <= '{1}')", sdate, edate);

            string tableQuery = "";
            List<GM_String> getTableList = GetLogTableName(ref TB, SnailLog_Define.money_log_tablename, sdate, edate);
            getTableList.ForEach(item =>
            {
                if (!string.IsNullOrEmpty(tableQuery))
                    tableQuery += "Union All";
                tableQuery += string.Format(@"
                                            Select d_create, s_event_id, n_money, n_before, n_after, n_event_type, '{0}' as tableName From {0} WITH(NOLOCK) Where AID = {1} And n_money_type={4} {2} {3}"
                                            , item.name, AID, condition1, condition2, moneyType);
            });

            string setQuery = string.Format(@"SELECT TOP({1}) resultTable.* FROM (
                                                    Select TOP {2} ROW_NUMBER() over (order by d_create Desc) as rownumber, * From ({0}) as logTable ) as resultTable
                                                WHERE rownumber > {3}"
                                                , tableQuery, GMData_Define.pageSize, (GMData_Define.pageSize * page), (page - 1) * GMData_Define.pageSize);
            List<_snail_money_log> retObj = GenericFetch.FetchFromDB_MultipleRow<_snail_money_log>(ref TB, setQuery, dbkey);
            if (retObj.Count == 0)
                retObj = new List<_snail_money_log>();
            return retObj;
        }

        public static long GetUserRubyCount(ref TxnBlock TB, long AID, string sdate, string edate, int eventType = 0, int moneyType = 0, string dbkey = GMData_Define.LogDBName)
        {
            string condition1 = "";
            string condition2 = eventType >= 0 ? string.Format(" And n_event_type = {0} ", eventType) : "";
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                condition1 = string.Format(" And (CONVERT(varchar(10), d_create,121) >= '{0}' and CONVERT(varchar(10), d_create,121) <= '{1}')", sdate, edate);

            string tableQuery = "";
            List<GM_String> getTableList = GetLogTableName(ref TB, SnailLog_Define.money_log_tablename, sdate, edate);
            getTableList.ForEach(item =>
            {
                if (!string.IsNullOrEmpty(tableQuery))
                    tableQuery += "Union All";
                tableQuery += string.Format(@"
                                            Select *, '{0}' as tableName From {0} WITH(NOLOCK) Where AID = {1} And n_money_type={4} {2} {3}"
                                            , item.name, AID, condition1, condition2, moneyType);
            });
            string setQuery = string.Format(@"Select count(*) as number From ({0}) as logTable", tableQuery);
            GM_Number retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.number;
        }

        public static List<GMUserMail> GetUserMailList(ref TxnBlock TB, long AID, int page, string sdate, string edate, string dbkey = GMData_Define.ShardingDBName)
        {
            string condition1 = "";
            if(string.IsNullOrEmpty(sdate) || string.IsNullOrEmpty(edate))
                condition1 = string.Format(@" And DATEADD(MINUTE, Case When (title like '%#STRING_MSG_VIP_REWARD_TITLE%' Or title ='{0}') Then -{1} Else -{2} End, closedate) >= dateadd(d,-30,GETDATE())", Mail_Define.Coupon_Mail_Title, Mail_Define.Mail_VIP_CloseTime_Min, Mail_Define.Mail_Close_Min);
            else
                condition1 = string.Format(@" And Convert(nvarchar(10),DATEADD(MINUTE, Case When (title like '%#STRING_MSG_VIP_REWARD_TITLE%' Or title ='{0}') Then -{1} Else -{2} End, closedate), 121) >= '{3}'
                                                And Convert(nvarchar(10),DATEADD(MINUTE, Case When (title like '%#STRING_MSG_VIP_REWARD_TITLE%' Or title ='{0}') Then -{1} Else -{2} End, closedate), 121) <= '{4}'"
                                            , Mail_Define.Coupon_Mail_Title, Mail_Define.Mail_VIP_CloseTime_Min, Mail_Define.Mail_Close_Min, sdate, edate);

            string setQuery = string.Format(@"SELECT TOP({1}) resultTable.* FROM (
                                                    Select TOP {2} ROW_NUMBER() over (order by mailseq Desc) as rownumber, *,
                                                        DATEADD(MINUTE, Case When (title like '%#STRING_MSG_VIP_REWARD_TITLE%' Or title ='{6}') Then -{7} Else -{8} End, closedate) as reg_date
                                                    From {0} WITH(NOLOCK) Where AID = {4} {5}) as resultTable
                                                WHERE rownumber > {3}",
                                                Mail_Define.Mail_Box_TableName,
                                                GMData_Define.pageSize, (GMData_Define.pageSize * page), (page - 1) * GMData_Define.pageSize, AID, condition1,
                                                Mail_Define.Coupon_Mail_Title, Mail_Define.Mail_VIP_CloseTime_Min, Mail_Define.Mail_Close_Min);
            List<GMUserMail> retObj = GenericFetch.FetchFromDB_MultipleRow<GMUserMail>(ref TB, setQuery, dbkey);
            return (retObj == null || retObj.Count == 0) ? new List<GMUserMail>() : retObj;
        }

        public static long GetUserMailCount(ref TxnBlock TB, long AID, string sdate, string edate, string dbkey = GMData_Define.ShardingDBName)
        {
            string condition1 = "";
            if (string.IsNullOrEmpty(sdate) || string.IsNullOrEmpty(edate))
                condition1 = string.Format(@" And DATEADD(MINUTE, Case When (title like '%#STRING_MSG_VIP_REWARD_TITLE%' Or title ='{0}') Then -{1} Else -{2} End, closedate) >= dateadd(d,-30,GETDATE())", Mail_Define.Coupon_Mail_Title, Mail_Define.Mail_VIP_CloseTime_Min, Mail_Define.Mail_Close_Min);
            else
                condition1 = string.Format(@" And Convert(nvarchar(10),DATEADD(MINUTE, Case When (title like '%#STRING_MSG_VIP_REWARD_TITLE%' Or title ='{0}') Then -{1} Else -{2} End, closedate), 121) >= '{3}' 
                                                And Convert(nvarchar(10),DATEADD(MINUTE, Case When (title like '%#STRING_MSG_VIP_REWARD_TITLE%' Or title ='{0}') Then -{1} Else -{2} End, closedate), 121) <= '{4}'"
                                            , Mail_Define.Coupon_Mail_Title, Mail_Define.Mail_VIP_CloseTime_Min, Mail_Define.Mail_Close_Min, sdate, edate);
            string setQuery = string.Format(@"SELECT Count(*) number From {0} WITH(NOLOCK) Where AID = {1} {2}",
                                                Mail_Define.Mail_Box_TableName, AID, condition1);
            GM_Number retObj = GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.number;
        }

        public static Result_Define.eResult SetUserMail(ref TxnBlock TB, long AID, long idx, bool delFlag, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = delFlag ? string.Format("Update {0} Set delflag='Y' Where mailseq = {1} And AID= {2}", Mail_Define.Mail_Box_TableName, idx, AID) :
                                        string.Format("Update {0} Set delflag='N', closedate = dateadd(d, 7, getdate()) Where mailseq = {1} And AID= {2}", Mail_Define.Mail_Box_TableName, idx, AID);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static List<GM_Mseed_item_log> GetUserDeleteItemList(ref TxnBlock TB, long AID, int page, string sdate, string edate, string dbkey = GMData_Define.LogDBName)
        {
            string condition1 = "";
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                condition1 = string.Format(" And (CONVERT(varchar(10), reg_date,121) >= '{0}' and CONVERT(varchar(10), reg_date,121) <= '{1}')", sdate, edate);
            string tableQuery = "";
            List<GM_String> getTableList = GetLogTableName(ref TB, SnailLog_Define.mseed_item_log_tablename, sdate, edate);
            getTableList.ForEach(item =>
            {
                if (!string.IsNullOrEmpty(tableQuery))
                    tableQuery += "Union All";
                tableQuery += string.Format(@"
                                            Select *, '{0}' as tableName From {0} WITH(NOLOCK, INDEX=IDX_{0}_aid, INDEX=IDX_{0}_datetime) Where AID = {1} {2}"
                                            , item.name, AID, condition1);
            });
            string setQuery = string.Format(@"SELECT TOP({1}) resultTable.* FROM (
                                                    Select TOP {2} ROW_NUMBER() over (order by reg_date Desc) as rownumber, * From ({0}) as logTable ) as resultTable
                                                WHERE rownumber > {3}"
                                                , tableQuery, GMData_Define.pageSize, (GMData_Define.pageSize * page), (page - 1) * GMData_Define.pageSize);
            List<GM_Mseed_item_log> retObj = GenericFetch.FetchFromDB_MultipleRow<GM_Mseed_item_log>(ref TB, setQuery, dbkey);
            return (retObj == null || retObj.Count == 0) ? new List<GM_Mseed_item_log>() : retObj;
        }

        public static long GetUserDeleteItemCount(ref TxnBlock TB, long AID, string sdate, string edate, string dbkey = GMData_Define.LogDBName)
        {
            string condition1 = "";
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                condition1 = string.Format(" And (CONVERT(varchar(10), reg_date,121) >= '{0}' and CONVERT(varchar(10), reg_date,121) <= '{1}')", sdate, edate);
            string tableQuery = "";
            List<GM_String> getTableList = GetLogTableName(ref TB, SnailLog_Define.mseed_item_log_tablename, sdate, edate);
            getTableList.ForEach(item =>
            {
                if (!string.IsNullOrEmpty(tableQuery))
                    tableQuery += "Union All";
                tableQuery += string.Format(@"
                                            Select *, '{0}' as tableName From {0} WITH(NOLOCK, INDEX=IDX_{0}_aid, INDEX=IDX_{0}_datetime) Where AID = {1} {2}"
                                            , item.name, AID, condition1);
            });
            string setQuery = string.Format(@"Select count(*) as number From ({0}) as logTable", tableQuery);
            GM_Number retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.number;
        }

        public static List<User_PassiveSoul> GetUserDeletePassiveSoulList(ref TxnBlock TB, long AID, int page, string sdate, string edate, string dbkey = GMData_Define.ShardingDBName)
        {
            string condition1 = "";
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                condition1 = string.Format(@" And creation_date >= '{0}' And creation_date <= '{1}'", sdate, edate);

            string setQuery = string.Format(@"SELECT TOP({1}) resultTable.* FROM (
                                                    Select TOP {2} ROW_NUMBER() over (order by creation_date Desc) as rownumber, *
                                                    From {0} WITH(NOLOCK) Where AID = {4} And delflag='Y' {5}) as resultTable
                                                WHERE rownumber > {3}",
                                                Soul_Define.User_PassiveSoul_Table,
                                                GMData_Define.pageSize, (GMData_Define.pageSize * page), (page - 1) * GMData_Define.pageSize, AID, condition1);
            List<User_PassiveSoul> retObj = GenericFetch.FetchFromDB_MultipleRow<User_PassiveSoul>(ref TB, setQuery, dbkey);
            return (retObj == null || retObj.Count == 0) ? new List<User_PassiveSoul>() : retObj;
        }

        public static long GetUserDeletePassiveSoulCount(ref TxnBlock TB, long AID, string sdate, string edate, string dbkey = GMData_Define.ShardingDBName)
        {
            string condition1 = "";
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                condition1 = string.Format(@" And creation_date >= '{0}' And creation_date <= '{1}'", sdate, edate);
            string setQuery = string.Format(@"SELECT Count(*) number From {0} WITH(NOLOCK) Where AID = {1} And delflag='Y' {2}",
                                                Soul_Define.User_PassiveSoul_Table, AID, condition1);
            GM_Number retObj = GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.number;
        }

        public static Result_Define.eResult SetUserItemRestore(ref TxnBlock TB, long AID, long idx, GMData_Define.GMDeleteItemType itemType = GMData_Define.GMDeleteItemType.ItemClass_Equip, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("Update {0} Set delflag='N' Where {1} = {2} And AID= {3}", GMData_Define.InvenTableList[itemType], GMData_Define.UserInvenTableSeq[itemType], idx, AID);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;

        }

        public static Result_Define.eResult SetUserItemRestore(ref TxnBlock TB, long AID, long idx, long log_idx, string logTable, GMData_Define.GMDeleteItemType itemType = GMData_Define.GMDeleteItemType.ItemClass_Equip, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            string setQuery = string.Format("Update {0} Set delflag='N', itemea = itemea+1 Where {1} = {2} And AID= {3}", GMData_Define.InvenTableList[itemType], GMData_Define.UserInvenTableSeq[itemType], idx, AID);
            retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            if (retError == Result_Define.eResult.SUCCESS)
            {
                setQuery = string.Format("Update {0} Set status = 1 Where log_idx = {1}", logTable, log_idx);
                retError = TB.ExcuteSqlCommand(GMData_Define.LogDBName, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            }
            return retError;
        }

        public static List<GMUserInvenItem> GetUserInvenList(ref TxnBlock TB, long AID, int page, GMData_Define.GMDeleteItemType itemType = GMData_Define.GMDeleteItemType.ItemClass_Equip, string dbkey = GMData_Define.ShardingDBName)
        {
            string condition = "";
            string selectQuery = "";
            string joinQuery = "";
            string indexQuery = "";
            if (itemType == GMData_Define.GMDeleteItemType.ItemClass_Orb)
            {
                selectQuery = "UI.orb_inven_seq idx, UI.aid, SI.Name, SI.[Description], SI.Item_IndexID, 1 itemea, 0 grade, 0 as [level]";
                joinQuery = "UI.orb_id = SI.Item_IndexID";
                condition = " And UI.delflag = 'N' And ultimate_inven_seq = 0";
                indexQuery = ", INDEX(IX_User_Orb_Inven_Ultimate_Inven_Seq)";
            }
            else if (itemType == GMData_Define.GMDeleteItemType.Soul_Parts)
            {
                selectQuery = "UI.soulseq idx, UI.aid, SI.Name, SI.[Description], SI.Item_IndexID, UI.soulparts_ea itemea, 0 grade, 0 as [level]";
                joinQuery = "UI.soulgroup = SI.Class_IndexID";
                condition = " And UI.soulparts_ea > 0 And UI.delflag = 'N'";
            }
            else if (itemType == GMData_Define.GMDeleteItemType.Soul_Equip)
            {
                selectQuery = "UI.soulseq idx, UI.aid, SI.Name, SI.[Description], SI.Item_IndexID, UI.soulparts_ea itemea, 0 grade, 0 as [level]";
                joinQuery = "UI.soulgroup = SI.Class_IndexID";
                condition = " And UI.soul_equip_ea > 0 And UI.delflag = 'N' And equipflag = 'N'";
            }
            else
            {
                selectQuery = "UI.invenseq idx, UI.aid, SI.Name, SI.[Description], SI.Item_IndexID, UI.itemea, UI.grade, UI.[level]";
                joinQuery = "UI.itemid = SI.Class_IndexID";
                condition = " And UI.itemea > 0 And UI.delflag = 'N'";
            }
            string setQuery = string.Format(@"SELECT TOP({0}) resultTable.* FROM (
                                                    Select TOP {1} ROW_NUMBER() over (order by {10} Desc) as rownumber, {6}
                                                    From {3} UI WITH(NOLOCK{7}) Inner Join {4} SI On {5} Where AID = {8} {9}) as resultTable
                                                WHERE rownumber > {2}"
                                                , GMData_Define.pageSize, (GMData_Define.pageSize * page), (page - 1) * GMData_Define.pageSize
                                                , GMData_Define.InvenTableList[itemType], Item_Define.Item_System_Item_Base_Table
                                                , joinQuery, selectQuery, indexQuery, AID, condition, GMData_Define.UserInvenTableSeq[itemType]);
            List<GMUserInvenItem> retObj = GenericFetch.FetchFromDB_MultipleRow<GMUserInvenItem>(ref TB, setQuery, dbkey);
            return (retObj == null || retObj.Count == 0) ? new List<GMUserInvenItem>() : retObj;
        }

        public static GMUserInvenItem GetUserInven(ref TxnBlock TB, long AID, long idx, GMData_Define.GMDeleteItemType itemType = GMData_Define.GMDeleteItemType.ItemClass_Equip, string dbkey = GMData_Define.ShardingDBName)
        {
            string selectQuery = "";
            string joinQuery = "";
            if (itemType == GMData_Define.GMDeleteItemType.ItemClass_Orb)
            {
                selectQuery = "UI.orb_inven_seq idx, UI.aid, SI.Name, SI.[Description], SI.Item_IndexID, 1 itemea, 0 grade, 0 as [level]";
                joinQuery = "UI.orb_id = SI.Item_IndexID";
            }
            else if (itemType == GMData_Define.GMDeleteItemType.Soul_Parts)
            {
                selectQuery = "UI.soulseq idx, UI.aid, SI.Name, SI.[Description], SI.Item_IndexID, UI.soulparts_ea itemea, 0 grade, 0 as [level]";
                joinQuery = "UI.soulgroup = SI.Class_IndexID";
            }
            else if (itemType == GMData_Define.GMDeleteItemType.Soul_Equip)
            {
                selectQuery = "UI.soulseq idx, UI.aid, SI.Name, SI.[Description], SI.Item_IndexID, UI.soulparts_ea itemea, 0 grade, 0 as [level]";
                joinQuery = "UI.soulgroup = SI.Class_IndexID";
            }
            else
            {
                selectQuery = "UI.invenseq idx, UI.aid, SI.Name, SI.[Description], SI.Item_IndexID, UI.itemea itemea, grade, [level]";
                joinQuery = "UI.itemid = SI.Class_IndexID";
            }
            string setQuery = string.Format(@"Select {3} From {0} UI WITH(NOLOCK) Inner Join {1} SI On {2} Where AID = {4} And {5}={6}"
                                                , GMData_Define.InvenTableList[itemType], Item_Define.Item_System_Item_Base_Table
                                                , joinQuery, selectQuery, AID, GMData_Define.UserInvenTableSeq[itemType], idx);
            GMUserInvenItem retObj = GenericFetch.FetchFromDB<GMUserInvenItem>(ref TB, setQuery, dbkey);
            return (retObj == null) ? new GMUserInvenItem() : retObj;
        }

        public static long GetUserInvenCount(ref TxnBlock TB, long AID, GMData_Define.GMDeleteItemType itemType = GMData_Define.GMDeleteItemType.ItemClass_Equip, string dbkey = GMData_Define.ShardingDBName)
        {
            string condition = "";
            if (itemType == GMData_Define.GMDeleteItemType.ItemClass_Orb)
                condition = " And delflag = 'N' And ultimate_inven_seq = 0";
            else if (itemType == GMData_Define.GMDeleteItemType.Soul_Parts)
                condition = " And soulparts_ea > 0 And delflag = 'N'";
            else if (itemType == GMData_Define.GMDeleteItemType.Soul_Equip)
                condition = " And soul_equip_ea > 0 And delflag = 'N'";
            else
                condition = " And itemea > 0 And delflag = 'N' And equipflag = 'N'";
            string setQuery = string.Format(@"SELECT Count(*) number From {0} WITH(NOLOCK) Where AID = {1} {2}"
                                                , GMData_Define.InvenTableList[itemType], AID, condition);
            GM_Number retObj = GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.number;
        }

        public static Result_Define.eResult DeleteUserItem(ref TxnBlock TB, long idx, int deleteItemCount = 1, GMData_Define.GMDeleteItemType itemType = GMData_Define.GMDeleteItemType.ItemClass_Equip, string dbkey = GMData_Define.ShardingDBName)
        {

            string deleteQuery = "";
            if (itemType == GMData_Define.GMDeleteItemType.ItemClass_Orb)
                deleteQuery = "delflag = 'Y'";
            else if (itemType == GMData_Define.GMDeleteItemType.Soul_Parts)
                deleteQuery = string.Format("soulparts_ea = soulparts_ea - {0}", deleteItemCount);
            else if (itemType == GMData_Define.GMDeleteItemType.Soul_Equip)
                deleteQuery = string.Format("soul_equip_ea = soul_equip_ea - {0}, delflag = Case When soul_equip_ea - {0} > 0 Then 'N' Else 'Y' End", deleteItemCount);
            else
                deleteQuery = string.Format("itemea = itemea - {0}, delflag = Case When itemea - {0} > 0 Then 'N' Else 'Y' End", deleteItemCount);

            string setQuery = string.Format("Update {0} Set {1} Where {2} = {3}", GMData_Define.InvenTableList[itemType], deleteQuery, GMData_Define.UserInvenTableSeq[itemType], idx);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;

        }

        public static List<Guild_GuildCreation> GetGuildList(ref TxnBlock TB, int page, long GID, string dbkey = GMData_Define.CommonDBName)
        {
            string condition1 = "";
            if (GID > 0)
                condition1 = string.Format(@" Where GuildID = {0}", GID);

            string setQuery = string.Format(@"SELECT TOP({1}) resultTable.* FROM (
                                                    Select TOP {2} ROW_NUMBER() over (order by GuildCreateDate Desc) as rownumber, *
                                                    From {0} WITH(NOLOCK) {4}) as resultTable
                                                WHERE rownumber > {3}",
                                                GuildManager.GuildCreationDBTableName,
                                                GMData_Define.pageSize, (GMData_Define.pageSize * page), (page - 1) * GMData_Define.pageSize, condition1);
            List<Guild_GuildCreation> retObj = GenericFetch.FetchFromDB_MultipleRow<Guild_GuildCreation>(ref TB, setQuery, dbkey);
            return (retObj == null || retObj.Count == 0) ? new List<Guild_GuildCreation>() : retObj;
        }

        public static long GetGuildCount(ref TxnBlock TB, long GID, string dbkey = GMData_Define.CommonDBName)
        {
            string condition1 = "";
            if (GID > 0)
                condition1 = string.Format(@" Where GuildID = {0}", GID);
            
            string setQuery = string.Format(@"SELECT Count(*) number From {0} WITH(NOLOCK) {1}",
                                                GuildManager.GuildCreationDBTableName, condition1);
            GM_Number retObj = GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.number;
        }

        public static List<Gm_UserLogLevel> GetUserLogLevelList(ref TxnBlock TB, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format(@"Select a.username, b.* From {0} a with(nolock) inner join {1} b with(nolock) on a.AID = b.AID
                                                Where enddate > GETDATE()"
                                            , Account_Define.AccountDBTableName, Log_Define.User_Admin_LogLevel_TableName);
            List<Gm_UserLogLevel> retObj = GenericFetch.FetchFromDB_MultipleRow<Gm_UserLogLevel>(ref TB, setQuery, dbkey);
            return (retObj == null || retObj.Count == 0) ? new List<Gm_UserLogLevel>() : retObj;
        }

        public static long GetUserRestrictLogCount(ref TxnBlock TB, long AID, string dbkey = GMData_Define.GmDBName)
        {
            string setQuery = string.Format(@"Select Count(*) number From {0} With(nolock{1}) {2}"
                                                , GMData_Define.UserRestrictLogTable
                                                , AID > 0 ? ", index(IDX_SERCH_BY_AID)" : ""
                                                , AID > 0 ? string.Format("Where user_account_idx={0}", AID) : "");
            GM_Number retObj = GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.number;
        }

        public static List<user_restrict_log> GetUserRestrictLogList(ref TxnBlock TB, int page, long AID, string dbkey = GMData_Define.GmDBName)
        {
            string setQuery = string.Format(@"SELECT TOP({3}) resultTable.* FROM (
                                                    Select TOP {4} ROW_NUMBER() over (order by idx Desc) as rownumber, *
                                                    From  {0} se with(nolock{1}) {2}) as resultTable
                                                WHERE rownumber > {5}"
                                                , GMData_Define.UserRestrictLogTable
                                                , AID > 0 ? ", index(IDX_SERCH_BY_AID)" : ""
                                                , AID > 0 ? string.Format("Where user_account_idx={0}", AID) : ""
                                                , GMData_Define.pageSize, (GMData_Define.pageSize * page), (page - 1) * GMData_Define.pageSize);
            List<user_restrict_log> retObj = GenericFetch.FetchFromDB_MultipleRow<user_restrict_log>(ref TB, setQuery, dbkey);
            return (retObj == null || retObj.Count == 0) ? new List<user_restrict_log>() : retObj;
        }

        public static Result_Define.eResult InsertUserRestrictLog(ref TxnBlock TB, user_restrict_log setData, string dbkey = GMData_Define.GmDBName)
        {
            string setQuery = string.Format(@"Insert Into {0} (user_account_idx, login_restrict_enddate, chat_restrict_endate, memo, regdate)
                                                            Values ({1}, '{2}', '{3}', N'{4}', getdate())"
                                                , GMData_Define.UserRestrictLogTable
                                                , setData.user_account_idx, setData.login_restrict_enddate.ToString("yyyy-MM-dd hh:mm")
                                                , setData.chat_restrict_endate.ToString("yyyy-MM-dd hh:mm"), setData.memo);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }
    }
}