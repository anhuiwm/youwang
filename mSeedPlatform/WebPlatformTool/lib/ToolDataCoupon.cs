using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using ServiceStack.Text;

using mSeed.Common;
using mSeed.Platform;
using mSeed.Platform.Coupon;
using mSeed.Platform.Coupon.DBClass;


namespace WebPlatformTool
{
    public partial class ToolDataManager
    {
        public static long GetCouponGroupCount(ref TxnBlock TB, string searchTxet = "", string dbkey = ToolData_Define.CouponDBName)
        {
            string query_search = string.IsNullOrEmpty(searchTxet) ? "" : string.Format(" Where Coupon_Title like N'%{0}%'", searchTxet);
            string query_Option = string.Format("WITH(NOLOCK{0}){1}", string.IsNullOrEmpty(searchTxet) ? "" : " , INDEX(IDX_SEARCH_TITLE)", query_search);
            
            string setQuery = string.Format("Select Count(*) number From {0} {1}", Coupon_Define.SystemCouponGroupTableName, query_Option);
            Number retObj = GenericFetch.FetchFromDB<Number>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.number;
        }

        public static List<CouponGroup> GetCouponGroupList(ref TxnBlock TB, int page = 1, string searchTxet = "", string dbkey = ToolData_Define.CouponDBName)
        {
            string setQuery = "";
            string query_search = string.IsNullOrEmpty(searchTxet) ? "" : string.Format(" Where Coupon_Title like N'%{0}%'", searchTxet);
            string query_Option = string.Format("WITH(NOLOCK{0}){1}", string.IsNullOrEmpty(searchTxet) ? "" : " , INDEX(IDX_SEARCH_TITLE)", query_search);
            if (page <= 0)
                setQuery = string.Format("Select * From {0} {1}", Coupon_Define.SystemCouponGroupTableName, query_Option);
            else
            {
                setQuery = string.Format(@"SELECT TOP({1}) resultTable.* FROM (
                                                 Select TOP {2} ROW_NUMBER() over (order by reg_date Desc) as rownumber, * From {0} {4}) as resultTable
                                            WHERE rownumber > {3}", Coupon_Define.SystemCouponGroupTableName, ToolData_Define.pageSize, (ToolData_Define.pageSize * page), (page - 1) * ToolData_Define.pageSize, query_Option);
            }
            List<System_Coupon_Group> getObj = GenericFetch.FetchFromDB_MultipleRow<System_Coupon_Group>(ref TB, setQuery, dbkey);
            List<CouponGroup> retObj = new List<CouponGroup>();
            if (getObj.Count > 0)
            {
                foreach (System_Coupon_Group item in getObj)
                {
                    CouponGroup setItem = new CouponGroup();
                    setItem.Coupon_Group_ID = item.Coupon_Group_ID;
                    setItem.Coupon_Title = item.Coupon_Title.Length > 70 ? item.Coupon_Title.Substring(0, 70) + "..." : item.Coupon_Title;
                    setItem.Reg_date = item.Reg_date;
                    setItem.Reg_id = item.Reg_id;
                    setItem.strCoupon_Type = item.Coupon_Type == 1 ? Resources.StringResource.lang_couponTypeSmall1 : Resources.StringResource.lang_couponTypeSmall2;
                    setItem.gameName = GameServiceManager.GetGameService(ref TB, item.game_service_id).service_name;
                    System_Coupon couponItem = CouponManager.GetSystemCouponCodeList(ref TB, item.Coupon_Group_ID).First();
                    if (couponItem == null)
                    {
                        setItem.startDate = DateTime.Today;
                        setItem.endDate = DateTime.Today;
                    }
                    else
                    {
                        setItem.startDate = couponItem.Coupon_Startdate;
                        setItem.endDate = couponItem.Coupon_Enddate;
                    }
                    retObj.Add(setItem);
                }
            }
            return retObj;
        }

        private static long GetCouponUseCount(ref TxnBlock TB, long idx, string dbkey = ToolData_Define.CouponDBName)
        {
            string setQuery = string.Format("Select Count(*) number From {0} WITH(NOLOCK) Where Coupon_Group_ID = {1}", Coupon_Define.UserCouponLogTableName, idx);
            Number retObj = GenericFetch.FetchFromDB<Number>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.number;
        }

        public static List<ExcelCoupon> GetCouponList_Excel(ref TxnBlock TB, long idx, string dbkey = ToolData_Define.CouponDBName)
        {
            string setQuery = string.Format(@"Select A.Coupon_Code, B.server_group_id, B.AID, B.userNickName, B.reg_date, B.complete_date
                                                    , Case When B.completeflag = 0 Then 'Y' Else 'N' End complete
                                                From {0} A WITH(NOLOCK) left outer join {1} B WITH(NOLOCK) on A.Coupon_Code = B.coupon_Code
                                                Where A.Coupon_Group_ID = {2}"
                                            , Coupon_Define.SystemCouponTableName, Coupon_Define.UserCouponLogTableName, idx);
            List<ExcelCoupon> retObj = GenericFetch.FetchFromDB_MultipleRow<ExcelCoupon>(ref TB, setQuery, dbkey);
            return retObj.Count == 0 ? new List<ExcelCoupon>() : retObj;
        }

        public static CouponGroup GetCouponGroup(ref TxnBlock TB, long idx, string dbkey = ToolData_Define.CouponDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Where Coupon_Group_ID = {1}", Coupon_Define.SystemCouponGroupTableName, idx);
            
            System_Coupon_Group getObj = GenericFetch.FetchFromDB<System_Coupon_Group>(ref TB, setQuery, dbkey);
            CouponGroup retObj = new CouponGroup();
            if (getObj != null)
            {
                retObj.Coupon_Title = getObj.Coupon_Title;
                retObj.game_service_id = getObj.game_service_id;
                retObj.Coupon_Active = getObj.Coupon_Active;
                retObj.Coupon_Memo = getObj.Coupon_Memo;
                retObj.Coupon_Group_ID = getObj.Coupon_Group_ID;
                retObj.Coupon_Num = getObj.Coupon_Num;
                retObj.Reg_date = getObj.Reg_date;
                retObj.Reg_id = getObj.Reg_id;
                retObj.Discontinue_date = getObj.Discontinue_date;
                retObj.strCoupon_Type = getObj.Coupon_Type == 1 ? Resources.StringResource.lang_couponTypeSmall1 : Resources.StringResource.lang_couponTypeSmall2;
                retObj.gameName = GameServiceManager.GetGameService(ref TB, getObj.game_service_id).service_name;
                retObj.useCount = GetCouponUseCount(ref TB, idx);
                System_Coupon couponItem = CouponManager.GetSystemCouponCodeList(ref TB, getObj.Coupon_Group_ID).First();
                if (couponItem == null)
                {
                    retObj.startDate = DateTime.Today;
                    retObj.endDate = DateTime.Today;
                    retObj.coupon = "";
                    retObj.rewardID = 0;
                }
                else
                {
                    retObj.startDate = couponItem.Coupon_Startdate;
                    retObj.endDate = couponItem.Coupon_Enddate;                    
                    retObj.coupon = getObj.Coupon_Type == 1 ? "" : couponItem.Coupon_Code;
                    retObj.rewardID = couponItem.Coupon_RewardID1;
                }
            }
            return retObj;
        }

    }
}