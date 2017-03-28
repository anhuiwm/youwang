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
        
        public static List<User_Inven> GetUserInvenList(ref TxnBlock TB, long AID, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Where AID = {1} And delflag = 'N' And itemea > 0", Item_Define.Item_User_Inven_Table, AID);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_Inven>(ref TB, setQuery, dbkey);
        }

        public static List<User_PassiveSoul> GetUserPassiveSoulList(ref TxnBlock TB, long AID, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND delflag = 'N'", Soul_Define.User_PassiveSoul_Table, AID);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_PassiveSoul>(ref TB, setQuery, dbkey);
        }

        public static List<GM_UserAccountSimple> GetUserSimpleList_BYUserName(ref TxnBlock TB, string userName, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = String.Format(@"Select AID, UserName From {0} WITH(NOLOCK) Where UserName like N'%{1}%'", Account_Define.AccountDBTableName, userName);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GM_UserAccountSimple>(ref TB, setQuery, dbkey);
        }

        public static Result_Define.eResult SetUser_PvP_point(ref TxnBlock TB, long AID, long seperaterWeekOrSeason, PvP_Define.ePvPType PvPType = PvP_Define.ePvPType.MATCH_FREE, string dbkey =GMData_Define.ShardingDBName)
        {
            string setDBTable = PvPType == PvP_Define.ePvPType.MATCH_1VS1 ? PvP_Define.PvP_PvP_Season_TableName : PvP_Define.PvP_PvP_Weekly_TableName;
            string setQuery = string.Format("Update {0} Set totalhonorpoint = 0 WHERE pvp_type = {1} AND seperater = {2} AND aid = {3} ", PvP_Define.PvP_Table_List[PvPType], (int)PvPType, seperaterWeekOrSeason, AID);

            Result_Define.eResult retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;

            if (retError == Result_Define.eResult.SUCCESS && PvPType == PvP_Define.ePvPType.MATCH_1VS1)
            {
                setQuery = string.Format("Update {0} Set totalhonorpoint = 0 WHERE pvp_type = {1} AND seperater = {2} AND aid = {3} ", PvP_Define.PvP_PvP_Weekly_TableName, (int)PvPType, seperaterWeekOrSeason, AID);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            }
            return retError;
        }

        public static Result_Define.eResult SetGuild_point(ref TxnBlock TB, long GID, long seperaterWeekOrSeason, string dbkey=GMData_Define.CommonDBName)
        {
            string setQuery = string.Format("Update {0} Set weekGuildRankPoint = 0 WHERE seperater = {1} AND gid = {2} ", PvP_Define.GuildRank_Table_List[PvP_Define.eGuildRankType.GUILDPOINT], seperaterWeekOrSeason, GID);

            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static Result_Define.eResult UseUserEventCash(ref TxnBlock TB, long AID, int cash, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("Update {0} Set EventCash = EventCash - {2} WHERE AID = {1}", Account_Define.AccountDBTableName, AID, cash);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static Result_Define.eResult UseUserCash(ref TxnBlock TB, long AID, int cash, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("Update {0} Set Cash = Cash - {2} WHERE AID = {1}", Account_Define.AccountDBTableName, AID, cash);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static GM_Billing_List GetBillingData(ref TxnBlock TB, long idx, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Where BillingIndex = {1}", Shop_Define.Shop_User_BillingInfo_TableName, idx);
            GM_Billing_List retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Billing_List>(ref TB, setQuery, dbkey);
            return retObj == null ? new GM_Billing_List() : retObj;
        }

        public static long GetBillingListCount(ref TxnBlock TB, string username, string uid, string sdate, string edate, int payResult = (int)Shop_Define.eBillingStatus.Complete, int shoptype = -1, string dbkey = GMData_Define.ShardingDBName)
        {
            string condition1 = "";
            string condition2 = "";
            string condition3 = "";
            string condition4 = "";
            string condition5 = "";
            if (!string.IsNullOrEmpty(username))
                condition1 = string.Format(" And BL.aid in (select aid From {0} WITH(NOLOCK) Where username like N'%{1}%')", Account_Define.AccountDBTableName, username);
            if (!string.IsNullOrEmpty(uid))
            {
                long getAID = GetSearchAID_BYSnailPlatformID(ref TB, uid).AID;
                condition2 = string.Format(" And BL.aid = {0} ", getAID);
            }
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                condition3 = string.Format(" And CONVERT(varchar(10), BL.regDate,121) >= '{0}' and  CONVERT(varchar(10), BL.regDate,121) <= '{1}' ", sdate, edate);
            if (shoptype > -1)
                condition4 = string.Format(" And SG.Type = {0}", shoptype);
            if (payResult >= 0)
                condition5 = string.Format(" And BL.Billing_Status = {0}", payResult);

            string setQuery = string.Format(@"SELECT count(*) as number From {0} as BL WITH(NOLOCK) left outer join {1} as SG WITH(NOLOCK) On BL.Shop_Goods_ID=SG.Shop_Goods_ID
                                                    Where 1=1 {5} {2}{3}{4}",
                                                Shop_Define.Shop_User_BillingInfo_TableName, Shop_Define.Shop_Type_TableList[Shop_Define.eShopType.Billing],
                                                condition1, condition2, condition3, condition5);

            GM_Number retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.number;
        }

        public static List<GM_Billing_List> GetBillingList(ref TxnBlock TB, int page, string username, string uid, string sdate, string edate, int payResult = (int)Shop_Define.eBillingStatus.Complete, int shoptype = -1, string dbkey= GMData_Define.ShardingDBName)
        {
            string condition1 = "";
            string condition2 = "";
            string condition3 = "";
            string condition4 = "";
            string condition5 = "";
            if(!string.IsNullOrEmpty(username))
                condition1 = string.Format(" And BL.aid in (select aid From {0} WITH(NOLOCK) Where username like N'%{1}%')", Account_Define.AccountDBTableName, username);
            if (!string.IsNullOrEmpty(uid))
            {
                long getAID = GetSearchAID_BYSnailPlatformID(ref TB, uid).AID;
                condition2 = string.Format(" And BL.aid = {0} ", getAID);
            }
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                condition3 = string.Format(" And CONVERT(varchar(10), BL.regDate,121) >= '{0}' and  CONVERT(varchar(10), BL.regDate,121) <= '{1}' ", sdate, edate);
            if (shoptype > -1) // not use
                condition4 = string.Format(" And SG.Type = {0}", shoptype);
            if(payResult>=0)
                condition5 = string.Format(" And BL.Billing_Status = {0}", payResult);

            string setQuery = string.Format(@"SELECT TOP({8}) resultTable.* FROM (
                                                    Select TOP {9} ROW_NUMBER() over (order by BL.BillingIndex Desc) as rownumber, BL.*, isnull(SG.Type,100) as shopType, Case When Type = 2 Then isnull(SG.ItemNum,0) Else 0 End as ItemDay , 
                                                    Case When Type = 2 Then 0 Else isnull(SG.ItemNum,0) End as ItemNum , isnull(SG.Bonus_ItemNum,0) as Bonus_ItemNum, isnull(SG.[Desc], '') goodsName,
                                                    (Select UserName From {2} Where AID = BL.AID) as UserName, isnull(Convert(nvarchar(10), BE.ErrorCode), 'Success') as ErrorCode
                                                    From {0} as BL WITH(NOLOCK) left outer join {1} as SG WITH(NOLOCK) On BL.Shop_Goods_ID=SG.Shop_Goods_ID
                                                        left outer join {3} as BE WITH(NOLOCK) On BL.BillingIndex=BE.BillingIndex
                                                    Where 1=1 {7} {4}{5}{6}) as resultTable
                                                WHERE rownumber > {10}",
                                                Shop_Define.Shop_User_BillingInfo_TableName, Shop_Define.Shop_Type_TableList[Shop_Define.eShopType.Billing], Account_Define.AccountDBTableName,
                                                Shop_Define.Shop_User_BillingError_TableName,
                                                condition1, condition2, condition3, condition5, GMData_Define.pageSize, (GMData_Define.pageSize * page), (page - 1) * GMData_Define.pageSize);

            List<GM_Billing_List> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GM_Billing_List>(ref TB, setQuery, dbkey);
            if (retObj == null)
                new List<GM_Billing_List>();
            return retObj;
        }

        public static List<GM_Billing_List> GetBillingList_Excel(ref TxnBlock TB, string username, string uid, string sdate, string edate, int payResult = (int)Shop_Define.eBillingStatus.Complete, int shoptype = -1, string dbkey = GMData_Define.ShardingDBName)
        {
            string condition1 = "";
            string condition2 = "";
            string condition3 = "";
            string condition4 = "";
            string condition5 = "";
            if (!string.IsNullOrEmpty(username))
                condition1 = string.Format(" And BL.aid in (select aid From {0} WITH(NOLOCK) Where username like N'%{1}%')", Account_Define.AccountDBTableName, username);
            if (!string.IsNullOrEmpty(uid))
            {
                long getAID = GetSearchAID_BYSnailPlatformID(ref TB, uid).AID;
                condition2 = string.Format(" And BL.aid = {0} ", getAID);
            }
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                condition3 = string.Format(" And CONVERT(varchar(10), BL.regDate,121) >= '{0}' and  CONVERT(varchar(10), BL.regDate,121) <= '{1}' ", sdate, edate);
            if (shoptype > -1)
                condition4 = string.Format(" And SG.Type = {0}", shoptype);
            if (payResult >= 0)
                condition5 = string.Format(" And BL.Billing_Status = {0}", payResult);
            string setQuery = string.Format(@"Select BL.*, isnull(SG.Type,100) as shopType, Case When Type = 2 Then isnull(SG.ItemNum,0) Else 0 End as ItemDay , 
                                              Case When Type = 2 Then 0 Else isnull(SG.ItemNum,0) End as ItemNum , isnull(SG.Bonus_ItemNum,0) as Bonus_ItemNum, isnull(SG.[Desc], '') goodsName,
                                              (Select UserName From {2} Where AID = BL.AID) as UserName, isnull(Convert(nvarchar(10), BE.ErrorCode), 'Success') as ErrorCode
                                                From {0} as BL WITH(NOLOCK) left outer join {1} as SG WITH(NOLOCK) On BL.Shop_Goods_ID=SG.Shop_Goods_ID
                                                    left outer join {3} as BE WITH(NOLOCK) On BL.BillingIndex=BE.BillingIndex
                                                Where 1=1 {7} {4}{5}{6}  Order By BL.regdate Desc",
                                    Shop_Define.Shop_User_BillingInfo_TableName, Shop_Define.Shop_Type_TableList[Shop_Define.eShopType.Billing]
                                    , Account_Define.AccountDBTableName, Shop_Define.Shop_User_BillingError_TableName
                                    , condition1, condition2, condition3, condition5);

            List<GM_Billing_List> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GM_Billing_List>(ref TB, setQuery, dbkey);
            if (retObj == null)
                new List<GM_Billing_List>();
            return retObj;
        }

        public static Result_Define.eResult SetBillingStatus(ref TxnBlock TB, long index, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("Update {0} Set Billing_Status = {1} Where BillingIndex = {2}", Shop_Define.Shop_User_BillingInfo_TableName, (int)Shop_Define.eBillingStatus.GmComplete, index);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static List<GM_Overlord_Ranking> GetOverLoadRanking(ref TxnBlock TB, ref long totalPlayer, int page = 1, string username = "", string dbkey = GMData_Define.ShardingDBName)
        {
            string condition1 = "";
            if (!string.IsNullOrEmpty(username))
                condition1 = string.Format(" Where AID in (Select AID from {0} WITH(NOLOCK) Where UserName like N'%{1}%')", Account_Define.AccountDBTableName, username);
            string setQuery = string.Format(@"SELECT TOP({1}) resultTable.* FROM (
                                                SELECT TOP {2} ROW_NUMBER() over (order by Ranking) as rownumber, *  FROM {0} WITH(NOLOCK) {4}) as resultTable
                                            WHERE rownumber > {3}", PvP_Define.PvP_User_PVP_Overlord_Ranking_TableName, GMData_Define.pageSize, (GMData_Define.pageSize * page), (page - 1) * GMData_Define.pageSize, condition1);
            List<User_PVP_Overlord_Ranking> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_PVP_Overlord_Ranking>(ref TB, setQuery, dbkey);
            if (!string.IsNullOrEmpty(username))
            {
                string setQuery2 = string.Format(@"SELECT Count(*) as number  FROM {0} WITH(NOLOCK) {1}", PvP_Define.PvP_User_PVP_Overlord_Ranking_TableName, condition1);
                totalPlayer = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery2, dbkey).number;
            }
            List<GM_Overlord_Ranking> rankingList = new List<GM_Overlord_Ranking>();
            foreach (User_PVP_Overlord_Ranking getRankingInfo in retObj)
            {
                GM_Overlord_Ranking setObj = new GM_Overlord_Ranking();

                getRankingInfo.AID = getRankingInfo.isNPC > 0 ? getRankingInfo.AID % PvP_Define.Overlord_NPC_Seperator : getRankingInfo.AID;
                Account_Simple userInfo = getRankingInfo.isNPC > 0 ? DummyManager.GetSimpleAccountInfo(ref TB, getRankingInfo.AID, false, dbkey) : AccountManager.GetSimpleAccountInfo(ref TB, getRankingInfo.AID, false, dbkey);
                setObj.rank = getRankingInfo.Ranking;
                setObj.AID = userInfo.aid;
                setObj.UserName = userInfo.username;
                rankingList.Add(setObj);
            }
            return rankingList;
        }
    }
}