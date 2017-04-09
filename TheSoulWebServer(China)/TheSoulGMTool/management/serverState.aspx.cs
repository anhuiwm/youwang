using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Tools;
using TheSoul.DataManager.Global;
using TheSoulWebServer.Tools;
using TheSoulGMTool.DBClass;


namespace TheSoulGMTool.management
{
    public partial class serverState : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            queryFetcher.bDBLog = true;
            string retJson = "";

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb);
                    tb.IsoLevel = IsolationLevel.ReadCommitted;

                    List<server_group_config> datalist = GlobalManager.GetServerGroupList(ref tb);
                    datalist.RemoveAll(item => item.server_group_id == 0);
                    datalist.ForEach(item => {
                        if (item.server_group_status < (int)Global_Define.eServerStatus.Maintenance)
                        {
                            TxnBlock tb2 = new TxnBlock();
                            TheSoulDBcon.GetInstance().TheSoulDBInitFromGlobal(ref tb2, (int)item.server_group_id, true);
                            item.user_account_idx = GMDataManager.GetLoginCount(ref tb2);
                            tb2.EndTransaction();
                            tb2.Dispose();
                        }
                        else
                            item.user_account_idx = 0;
                    });
                    dataList.DataSource = datalist;
                    dataList.DataBind();
                }
                catch (Exception errorEx)
                {
                    retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(errorEx.Message), Result_Define.eResult.System_Exception);
                }
                finally
                {

                    tb.EndTransaction(queryFetcher.Render_errorFlag);
                    string gmid = "";
                    if (Request.Cookies.Count > 0)
                        gmid = GMDataManager.GetUserCookies("userid");
                    queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                    tb.Dispose();
                }
            }
        }

        protected void dataList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dataList.PageIndex = e.NewPageIndex;
            dataList.DataBind();
        }

        protected void dataList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateState")
            {
                int rowIndex = System.Convert.ToInt32(e.CommandArgument);
                long serverID = System.Convert.ToInt64(dataList.DataKeys[rowIndex].Values[0]);

                //DeleteServerVersion
                WebQueryParam queryFetcher = new WebQueryParam(true);
                string retJson = "";
                
                TxnBlock tb = new TxnBlock();
                {
                    try
                    {

                        GMDataManager.GetServerinit(ref tb, ref queryFetcher);
                        tb.IsoLevel = IsolationLevel.ReadCommitted;

                        DropDownList stateList = dataList.Rows[rowIndex].FindControl("serverState") as DropDownList;
                        int serverState = queryFetcher.QueryParam_FetchInt(stateList.UniqueID);

                        Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                        server_group_config serverInfo = GlobalManager.GetServerGroupList(ref tb).Find(item => item.server_group_id == serverID);
                        if (serverInfo != null)
                        {
                            if (serverInfo.server_group_status < 20 && serverState >= 0)
                            {
                                retError = GMDataManager.SetServerState(ref tb, serverID, (Global_Define.eServerStatus)serverState);
                                if (retError == Result_Define.eResult.SUCCESS)
                                    retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, serverID, "", GMResult_Define.ControlType.SERVER_STATE_EDIT, serverState.ToString(), serverID.ToString());
                                queryFetcher.GM_Render(retError);
                            }
                            else
                            {
                                string msg = "";
                                if (serverInfo.server_group_status < 20)
                                    msg = "alert('" + Resources.languageResource.lang_msgSeverChangeError2 + "');";
                                else
                                    msg = "alert('" + Resources.languageResource.lang_msgSeverChangeError1 + "');";
                                Page.ClientScript.RegisterStartupScript(GetType(), "alert", msg, true);

                            }
                        }
                    }
                    catch (Exception errorEx)
                    {
                        queryFetcher.DBLog("StackTrace" + mJsonSerializer.ToJsonString(errorEx.StackTrace));
                        queryFetcher.DBLog(errorEx.Message);
                        retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(errorEx.Message), Result_Define.eResult.System_Exception);
                    }
                    finally
                    {

                        tb.EndTransaction(queryFetcher.Render_errorFlag);
                        string gmid = "";
                        if (Request.Cookies.Count > 0)
                            gmid = GMDataManager.GetUserCookies("userid");
                        queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                        tb.Dispose();
                    }
                    if (queryFetcher.Render_errorFlag)
                        Response.Redirect(Request.RawUrl);
                }
            }
        }
    }
}