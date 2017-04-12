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

using WebPlatform.Tools;

namespace WebPlatformTool.coupon
{
    public partial class coupon_list : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam();
            //string retJson = "";
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    tb.IsoLevel = IsolationLevel.ReadCommitted;
                    ToolDataManager.DBconn(ref tb);
                    Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
                    string searchText = queryFetcher.QueryParam_Fetch(search.UniqueID);
                    if (string.IsNullOrEmpty(searchText))
                        searchText = queryFetcher.QueryParam_Fetch("search");
                    int req_page = queryFetcher.QueryParam_FetchInt("pg", 1);
                    search.Text = searchText;
                    pg.Value = req_page.ToString();
                    long totalCount = ToolDataManager.GetCouponGroupCount(ref tb, searchText);
                    List<CouponGroup> list = ToolDataManager.GetCouponGroupList(ref tb, req_page, searchText);
                    dataList.DataSource = list;
                    dataList.DataBind();
                    ToolDataManager.PopulatePager(ref dlPager, totalCount, req_page);
                    queryFetcher.NoRenderWirte(retError);
                }
                catch (Exception errorEx)
                {
                    queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(errorEx.Message), Result_Define.eResult.System_Exception);
                }
                finally
                {
                    tb.EndTransaction(queryFetcher.Render_errorFlag);
                    tb.Dispose();
                }
            }
        }
        
        protected void dlPager_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "PageNo")
            {
                int page = System.Convert.ToInt32(e.CommandArgument);
                pg.Value = page.ToString();
                string url = string.Format("{0}?pg={1}&search={2}", Request.Url.AbsolutePath, page, search.Text);
                Response.Redirect(url);
            }
        }

        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
            // Confirms that an HtmlForm control is rendered for the specified ASP.NET server control at run time.
        }
    }
}