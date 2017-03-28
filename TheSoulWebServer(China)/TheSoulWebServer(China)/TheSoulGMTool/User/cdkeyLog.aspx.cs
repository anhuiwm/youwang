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

namespace TheSoulGMTool.User
{
    public partial class cdkeyLog : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(sDate.Text))
                sDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            if (string.IsNullOrEmpty(eDate.Text))
                eDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
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
                    long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
                    GMDataManager.GetServerinit(ref tb, serverID);
                    if (Page.IsPostBack)
                    {
                        string sdate = queryFetcher.QueryParam_Fetch(sDate.UniqueID, "").Replace("/", "-");
                        string edate = queryFetcher.QueryParam_Fetch(eDate.UniqueID, "").Replace("/", "-");
                        if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                        {
                            long totalCount = GMDataManager.GetUserCdkeyLogCount(ref tb, sdate, edate);
                            List<Cdkey_Log> list = GMDataManager.GetUserCdkeyLogList(ref tb, pageIndex, sdate, edate);
                            dataList.DataSource = list;
                            dataList.DataBind();
                            GMDataManager.PopulatePager(ref dlPager, totalCount, pageIndex);
                        }
                        queryFetcher.GM_Render(Result_Define.eResult.SUCCESS);
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
                        gmid = HttpContext.Current.Request.Cookies["mseedadmin"]["userid"];
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



        protected void Button2_Click(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            queryFetcher.bDBLog = true;
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
                    GMDataManager.GetServerinit(ref tb, serverID);

                    string sdate = queryFetcher.QueryParam_Fetch(sDate.UniqueID, "").Replace("/", "-");
                    string edate = queryFetcher.QueryParam_Fetch(eDate.UniqueID, "").Replace("/", "-");

                    if (string.IsNullOrEmpty(sdate) && string.IsNullOrEmpty(edate))
                    {
                        string errorMsg = "alert('" + Resources.languageResource.lang_MsgExcelDate + "');";
                        Page.ClientScript.RegisterStartupScript(GetType(), "alert", errorMsg, true);
                    }
                    else
                    {

                        List<Cdkey_Log> list = GMDataManager.GetUserCdkeyLogList_Excel(ref tb, sdate, edate);

                        string lang = GMDataManager.GetGmToolLanguage();
                        string charSet = "";
                        if (lang == "kr")
                            charSet = "euc-kr";
                        else
                            charSet = "GB2312";

                        string filename = string.Format("attachment;filename=usercdkeylog_{0}.csv", DateTime.Now);
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = charSet;
                        Response.ContentEncoding = System.Text.Encoding.GetEncoding(charSet);
                        Response.AddHeader("content-disposition", filename);
                        Response.ContentType = "text/csv";

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();

                        foreach (System.Web.UI.WebControls.BoundField item in dataList.Columns)
                        {
                            sb.Append(item.HeaderText + ',');

                        }
                        
                        foreach (Cdkey_Log item in list)
                        {
                            sb.Append("\r\n");
                            string stritem = string.Format(@"{0},{1},{2},{3},{4},{5},{6},{7}"
                                                            , item.AID, item.platform_idx, item.platform_user_id, item.userName
                                                            , item.cdkey, item.mailseq, item.stateflag, item.regdate);
                            sb.Append(stritem);
                        }
                        Response.Output.Write(sb.ToString());
                        Response.Flush();
                        Response.End();
                    }
                    queryFetcher.GM_Render(Result_Define.eResult.SUCCESS);
                }
                catch (Exception errorEx)
                {
                    Console.Write(errorEx);
                    queryFetcher.DBLog("StackTrace" + mJsonSerializer.ToJsonString(errorEx.StackTrace));
                    queryFetcher.DBLog(errorEx.Message);
                    queryFetcher.GM_Render(Result_Define.eResult.SUCCESS);
                }
                finally
                {

                    tb.EndTransaction(queryFetcher.Render_errorFlag);
                    string gmid = "";
                    if (Request.Cookies.Count > 0)
                        gmid = HttpContext.Current.Request.Cookies["mseedadmin"]["userid"];
                    queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                    tb.Dispose();
                }
            }
        }
    }
}