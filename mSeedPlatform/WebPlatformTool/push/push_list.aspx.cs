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

using WebPlatform.Tools;

namespace WebPlatformTool.push
{
    public partial class push_list : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetDataList();
        }

        protected void GetDataList(int page = 1)
        {
            WebQueryParam queryFetcher = new WebQueryParam();
            //string retJson = "";
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    tb.IsoLevel = IsolationLevel.ReadCommitted;
                    ToolDataManager.DBconn(ref tb);
                    Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                    string searchSartDate = queryFetcher.QueryParam_Fetch(sdate.UniqueID);
                    string searchEndDate = queryFetcher.QueryParam_Fetch(edate.UniqueID);
                    long searchGame = queryFetcher.QueryParam_FetchLong(game_id.UniqueID);
                    long game_service_id = queryFetcher.QueryParam_FetchLong(game.UniqueID);
                    long push_id = queryFetcher.QueryParam_FetchLong(idx.UniqueID);
                    int statusValue = queryFetcher.QueryParam_FetchInt(status.UniqueID);
                    if (!Page.IsPostBack)
                    {
                        List<game_service> gameList = GameServiceManager.GetGameService(ref tb);
                        game_id.DataSource = gameList;
                        game_id.DataTextField = "service_name";
                        game_id.DataValueField = "game_service_id";
                        game_id.DataBind();
                    }
                    if (searchGame > 0)
                        game_id.SelectedValue = searchGame.ToString();
                    if (game_service_id > 0 && push_id > 0)
                    {
                        bool isResult = PushManager.SetPushStatus(game_service_id, push_id, (ePushStatus)statusValue);
                        string errorMsg = isResult ? "alert('" + Resources.StringResource.lang_msgSucces + "');" : "alert('" + Resources.StringResource.lang_msgFail + "');";
                        Page.ClientScript.RegisterStartupScript(GetType(), "alert", errorMsg, true);
                    }

                    long totalCount = ToolDataManager.GetPushCount(ref tb, searchGame, searchSartDate, searchEndDate);
                    List<ToolPush> list = ToolDataManager.GetPushList(ref tb, page, searchGame, searchSartDate, searchEndDate);
                    list.ForEach(item => {
                        item.strStatus = ToolData_Define.PushStatus[(ePushStatus)item.push_status];
                        item.game_name = GameServiceManager.GetGameService(ref tb, item.game_service_id).service_name;
                    });
                    dataList.DataSource = list;
                    dataList.DataBind();
                    ToolDataManager.PopulatePager(ref dlPager, totalCount, page);
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
                this.GetDataList(page);
            }
        }

        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
            // Confirms that an HtmlForm control is rendered for the specified ASP.NET server control at run time.
        }
    }
}