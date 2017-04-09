using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Tools;
using TheSoul.DataManager.Global;
using TheSoulWebServer.Tools;
using TheSoulGMTool.DBClass;
using TheSoul.DataManager.TCP_Packet;

namespace TheSoulGMTool.kr
{
    public partial class user_restrictList : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                GetDataList(1);
            else
            {
                WebQueryParam queryFetcher = new WebQueryParam(true);
                queryFetcher.bDBLog = true;
                string retJson = "";
                TxnBlock tb = new TxnBlock();
                {
                    try
                    {
                        GMDataManager.GetServerinit(ref tb);
                        Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;

                        int restrict = queryFetcher.QueryParam_FetchInt(restrictType.UniqueID);
                        long AID = queryFetcher.QueryParam_FetchLong(idx.UniqueID);
                        string reqMemo = queryFetcher.QueryParam_Fetch(reason.UniqueID, GetGlobalResourceObject("languageResource", "lang_lift").ToString());
                        if (AID > 0)
                        {
                            user_account_restrict restrictInfo = GlobalManager.GetUserRestrict(ref tb, AID);
                            TimeSpan chatRestrict = restrictInfo.chat_restrict_endate - DateTime.Now;
                                                        
                            retError = GlobalManager.SetUserRestrict(ref tb, AID, 0, (Global_Define.eAccountRestrictType)restrict);
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                user_restrict_log setData = new user_restrict_log();
                                setData.user_account_idx = AID;
                                setData.memo = reqMemo.Replace("'", "''");
                                setData.login_restrict_enddate = DateTime.Now;
                                setData.chat_restrict_endate = DateTime.Now;
                                retError = GMDataManager.InsertUserRestrictLog(ref tb, setData);
                            }
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                bool result = false;
                                List<server_config> serverInfo = GlobalManager.GetServerList(ref tb).FindAll(item => item.server_type == "cs_game");
                                serverInfo.ForEach(item =>
                                {
                                    if ((Global_Define.eAccountRestrictType)restrict == Global_Define.eAccountRestrictType.CHAT_REMOVE && chatRestrict.TotalSeconds > 0)
                                    {
                                        int port = item.server_private_port != null ? (int)item.server_private_port : 0;
                                        result = TCP_GM_Operation.User_ChatRestrict(item.server_private_ip, port, AID, 0);
                                        if (!result)
                                        {
                                            port = item.server_public_port != null ? (int)item.server_public_port : 0;
                                            result = TCP_GM_Operation.User_ChatRestrict(item.server_public_ip, port, AID, 0);
                                        }
                                        if (!result)
                                            retJson = retJson + item.server_group_id.ToString() + " Chat Restrict Fail.";
                                    }
                                });
                                if (string.IsNullOrEmpty(retJson))
                                    retJson = "Success";
                                string alertmsg = "alert('" + retJson + "');";
                                Page.ClientScript.RegisterStartupScript(GetType(), "alert", alertmsg, true);
                                GetDataList(1);
                            }

                            idx.Value = "";
                            restrictType.Value = "";
                            
                        }
                        queryFetcher.GM_Render(retError);
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
                }
            }
        }

        protected void GetDataList(int pageIndex)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            queryFetcher.bDBLog = true;
            string retJson = "";

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb);
                    Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

                    long totalCount = GMDataManager.GetUserRestrictCount(ref tb);

                    List<GM_account_restrict> logList = GMDataManager.GetUserRestrictList(ref tb, pageIndex);
                    dataList.DataSource = logList;
                    dataList.DataBind();

                    GMDataManager.PopulatePager(ref dlPager, totalCount, pageIndex);
                    queryFetcher.GM_Render(retError);

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

        protected void Unnamed_Click(object sender, EventArgs e)
        {

        }

    }
}