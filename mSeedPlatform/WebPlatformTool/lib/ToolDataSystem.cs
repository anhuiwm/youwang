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
        public static void DBconn(ref TxnBlock TB)
        {
            PlatformBase.GetPlatformDB(ref TB);
            CouponManager.GetCouponDB(ref TB);

        }

        public static void PopulatePager(ref DataList dlPager, long recordCount, int currentPage)
        { // paging
            double dblPageCount = (double)((decimal)recordCount / ToolData_Define.pageSize);
            int pageCount = (int)System.Math.Ceiling(dblPageCount);
            int prePage = ((currentPage - 1) / ToolData_Define.pageBlock) * ToolData_Define.pageBlock + 1;
            int endPage = prePage + ToolData_Define.pageBlock - 1;
            if (pageCount < ToolData_Define.pageBlock)
                endPage = pageCount;
            List<ListItem> pages = new List<ListItem>();
            if (pageCount > 0)
            {
                if (currentPage > ToolData_Define.pageBlock)
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
    }
}