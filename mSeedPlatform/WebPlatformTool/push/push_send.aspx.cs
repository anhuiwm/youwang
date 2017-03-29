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
    public partial class push_send : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam();

            if (!Page.IsPostBack)
            {
                //string retJson = "";
                TxnBlock tb = new TxnBlock();
                {
                    try
                    {
                        tb.IsoLevel = IsolationLevel.ReadCommitted;
                        ToolDataManager.DBconn(ref tb);

                        Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                        List<game_service> gameList = GameServiceManager.GetGameService(ref tb);
                        game_id.DataSource = gameList;
                        game_id.DataTextField = "service_name";
                        game_id.DataValueField = "game_service_id";
                        game_id.DataBind();
                        List<ListItem> hourList = ToolDataManager.GetHourList();
                        List<ListItem> minList = ToolDataManager.GetMinList(1);

                        hour.DataSource = hourList;
                        hour.DataTextField = "Text";
                        hour.DataValueField = "Value";
                        hour.DataBind();

                        min.DataSource = minList;
                        min.DataTextField = "Text";
                        min.DataValueField = "Value";
                        min.DataBind();
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
            else
            {
                long game_service_id = queryFetcher.QueryParam_FetchLong(game_id.UniqueID, 1);
                string title = queryFetcher.QueryParam_Fetch(txtTitle.UniqueID,"test");
                string message = queryFetcher.QueryParam_Fetch(txtMessage.UniqueID, "test message");
                string reason = queryFetcher.QueryParam_Fetch(txtReason.UniqueID, "test");
                string sendDay = queryFetcher.QueryParam_Fetch(date.UniqueID);
                int sendHour = queryFetcher.QueryParam_FetchInt(hour.UniqueID);
                int sendMin = queryFetcher.QueryParam_FetchInt(min.UniqueID);
                byte push_type = queryFetcher.QueryParam_FetchByte(pushType.UniqueID);
                

                string sendDate = "";
                if (!string.IsNullOrEmpty(sendDay) && sendHour >= 0 && sendMin >= 0)
                    sendDate = string.Format("{0} {1}:{2}", sendDay, sendHour, sendMin);

                system_push_service setData = new system_push_service();
                setData.push_type = push_type;
                setData.game_service_id = game_service_id;
                setData.title = title;
                setData.message = message;
                setData.push_reason = reason;
                setData.push_status = (byte)ePushStatus.Unconfirmed;
                setData.send_reserv_date = string.IsNullOrEmpty(sendDate) ? DateTime.Now.AddHours(2) : System.Convert.ToDateTime(sendDate);
                setData.register = "test2"; //TO DO : gm id로 추후 변경用后变更
                bool isResut = false;
                isResut = PushManager.InsertPushService(setData);
                if (isResut)
                    Response.Redirect("/push/push_list.aspx");
            }
            
        }
    }
}