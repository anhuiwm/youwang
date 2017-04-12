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
using mSeed.Platform.Coupon;
using mSeed.Platform.Coupon.DBClass;

namespace WebPlatformTool.coupon
{
    public partial class coupon_query : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long groupid = 0;
            int makeCount = 0;
            long rewardid = 0;
            int gameid = 0;
            string startDate = "";
            string endDate = "";
            List<string> list = CouponManager.SerialNumberGenerator(groupid, 16, makeCount);
            int numCount = 0;
            foreach (string item in list)
            {
                long num = 0;
                if (!long.TryParse(item, out num))
                {
                    string query = string.Format("insert into System_Coupon values ({1}, {2}, '{0}',1,{3},0,0,0,'{4} 00:00:00','{5} 23:59:59')<br/>", item, groupid, gameid, rewardid, startDate, endDate);
                    Response.Write(item+"<br />");
                }
                else
                    numCount = numCount + 1;
            }

            Response.Write(numCount.ToString());

        }
    }
}