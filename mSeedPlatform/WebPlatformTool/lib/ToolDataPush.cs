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
using mSeed.Platform.PushNotification;

namespace WebPlatformTool
{
    public partial class ToolDataManager
    {
        public static long GetPushCount(ref TxnBlock TB, long game_service_id = 0, string sdate = "", string edate = "", string dbkey = ToolData_Define.PlatformDBName)
        {
            string query_search = "";
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                query_search = game_service_id > 0 ? string.Format(@"WITH(NOLOCK, INDEX(IDX_PushDateSearch)) Where game_service_id = {0} And (CONVERT(varchar(10), send_reserv_date,121) >= '{1}' and CONVERT(varchar(10), send_reserv_date,121) <= '{2}')", game_service_id, sdate, edate)
                                                    : string.Format(@"WITH(NOLOCK, INDEX(IDX_PushDateSearch)) Where game_service_id > {0} And (CONVERT(varchar(10), send_reserv_date,121) >= '{1}' and CONVERT(varchar(10), send_reserv_date,121) <= '{2}')", game_service_id, sdate, edate);
            else
                query_search = game_service_id > 0 ? string.Format(@"WITH(NOLOCK) Where game_service_id = {0}", game_service_id) : "WITH(NOLOCK)";

            string setQuery = string.Format("Select Count(*) number From {0} {1}", DB_Define.DBTables[DB_Define.eDBTables.system_push_service], query_search);
            mSeed.Platform.Coupon.DBClass.Number retObj = GenericFetch.FetchFromDB<mSeed.Platform.Coupon.DBClass.Number>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.number;
        }

        public static List<ToolPush> GetPushList(ref TxnBlock TB, int page, long game_service_id = 0, string sdate = "", string edate = "", string dbkey = ToolData_Define.PlatformDBName)
        {
            string query_search = "";
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                query_search = game_service_id > 0 ? string.Format(@"WITH(NOLOCK, INDEX(IDX_PushDateSearch)) Where game_service_id = {0} And (CONVERT(varchar(10), send_reserv_date,121) >= '{1}' and CONVERT(varchar(10), send_reserv_date,121) <= '{2}')", game_service_id, sdate, edate)
                                                    : string.Format(@"WITH(NOLOCK, INDEX(IDX_PushDateSearch)) Where game_service_id > {0} And (CONVERT(varchar(10), send_reserv_date,121) >= '{1}' and CONVERT(varchar(10), send_reserv_date,121) <= '{2}')", game_service_id, sdate, edate);
            else
                query_search = game_service_id > 0 ? string.Format(@"WITH(NOLOCK) Where game_service_id = {0}", game_service_id) : "WITH(NOLOCK)";

            string setQuery = setQuery = string.Format(@"SELECT TOP({1}) resultTable.* FROM (
                                                 Select TOP {2} ROW_NUMBER() over (order by reg_date Desc) as rownumber, * From {0} {4}) as resultTable
                                            WHERE rownumber > {3}", DB_Define.DBTables[DB_Define.eDBTables.system_push_service], ToolData_Define.pageSize, (ToolData_Define.pageSize * page), (page - 1) * ToolData_Define.pageSize, query_search);
            List<ToolPush> retObj = GenericFetch.FetchFromDB_MultipleRow<ToolPush>(ref TB, setQuery, dbkey);
            return (retObj == null || retObj.Count == 0) ? new List<ToolPush>() : retObj;
        }
    }
}