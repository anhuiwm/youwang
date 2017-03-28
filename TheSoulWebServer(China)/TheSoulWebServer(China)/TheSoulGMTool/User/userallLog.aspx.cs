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
    public partial class userallLog : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
            WebQueryParam queryFetcher = new WebQueryParam(true);
            if (string.IsNullOrEmpty(sDate.Text))
                sDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            if (string.IsNullOrEmpty(eDate.Text))
                eDate.Text = DateTime.Today.ToString("yyyy-MM-dd");

            minDate.Value = DateTime.Today.AddDays(GMData_Define.AllLogSearchMinDay).ToString("yyyy-MM-dd");

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    string savePath = Request.PhysicalApplicationPath;
                    GMDataManager.GetGMServerIni(ref tb, savePath);
                    if (!Page.IsPostBack)
                    {
                        List<system_log_operation> opList = GMDataManager.GetOperation(ref tb);
                        ListItem selectItem = new ListItem("select", "");
                        op1.DataSource = opList; op1.DataTextField = "String"; op1.DataValueField = "Operation"; op1.DataBind();
                        op1.Items.Insert(0, selectItem);
                        op2.DataSource = opList; op2.DataTextField = "String"; op2.DataValueField = "Operation"; op2.DataBind();
                        op2.Items.Insert(0, selectItem);
                        op3.DataSource = opList; op3.DataTextField = "String"; op3.DataValueField = "Operation"; op3.DataBind();
                        op3.Items.Insert(0, selectItem);


                    }
                    else
                    {
                        int page = queryFetcher.QueryParam_FetchInt(pg.UniqueID, 1);
                        if(page == 1)
                            GetDataList(page);
                    }
                }
                catch (Exception errorEx)
                {
                    Response.Write(errorEx.Message);
                }
                finally
                {

                    tb.EndTransaction();
                    tb.Dispose();
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
                    long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
                    GMDataManager.GetServerinit(ref tb, serverID);
                    if (Page.IsPostBack)
                    {
                        string sdate = queryFetcher.QueryParam_Fetch(sDate.UniqueID, "").Replace("/", "-");
                        string edate = queryFetcher.QueryParam_Fetch(eDate.UniqueID, "").Replace("/", "-");
                        string platformID = queryFetcher.QueryParam_Fetch(platform.UniqueID, "");
                        string userName = string.IsNullOrEmpty(platformID) ? queryFetcher.QueryParam_Fetch(username.UniqueID, "") : "";
                        string operation1 = queryFetcher.QueryParam_Fetch(op1.UniqueID, "");
                        string operation2 = queryFetcher.QueryParam_Fetch(op2.UniqueID, "");
                        string operation3 = queryFetcher.QueryParam_Fetch(op3.UniqueID, "");
                        int errorCode = -1;
                        string operation = "";
                        long AID = string.IsNullOrEmpty(platformID) ? GMDataManager.GetSearchAID_BYUserName(ref tb, userName): GMDataManager.GetSearchAID_BYSnailPlatformID(ref tb, platformID).AID;
                        if (AID > 0 && !string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                        {
                            if (!string.IsNullOrEmpty(operation1))
                                operation = string.Format("'{0}'", operation1);
                            if (!string.IsNullOrEmpty(operation2))
                                operation = string.IsNullOrEmpty(operation) ? operation2 : string.Format("{0},'{1}'", operation, operation2);
                            if (!string.IsNullOrEmpty(operation3))
                                operation = string.IsNullOrEmpty(operation) ? operation3 : string.Format("{0},'{1}'", operation, operation3);

                            if (string.IsNullOrEmpty(operation))
                            {
                                List<string> oplist = op1.Items.Cast<ListItem>().Where(item => !string.IsNullOrEmpty(item.Value)).Select(item => item.Value).ToList();
                                oplist.ForEach(item => {
                                    if (!string.IsNullOrEmpty(operation))
                                        operation = operation + ",";
                                    operation = operation + string.Format("'{0}'",item);
                                });
                            }
                            
                            long totalCount = GMDataManager.GetUserAllLogListCount(ref tb, AID, errorCode, operation, sdate, edate);
                            List<Request_Log> list = GMDataManager.GetUserAllLogList(ref tb, pageIndex, AID, errorCode, operation, sdate, edate);
                            list.ForEach(item =>
                            {
                                if (item.ResponseResult.Length > 100)
                                    item.ResponseResult = string.Format("{0}...", item.ResponseResult.Substring(0, 100));
                                if (item.DetailDBLog.Length > 100)
                                    item.DetailDBLog = string.Format("{0}...", item.DetailDBLog.Substring(0, 100));
                            });
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
                pg.Value = page.ToString();
                this.GetDataList(page);                
            }
        }

        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
            // Confirms that an HtmlForm control is rendered for the specified ASP.NET server control at run time.
        }

        protected string CsvQuote(string text)
        {
            if (text == null)
            {
                return string.Empty;
            }

            bool containsQuote = false;
            bool containsComma = false;
            int len = text.Length;
            for (int i = 0; i < len && (containsComma == false || containsQuote == false); i++)
            {
                char ch = text[i];
                if (ch == '"')
                {
                    containsQuote = true;
                }
                else if (ch == ',' || char.IsControl(ch))
                {
                    containsComma = true;
                }
            }

            bool mustQuote = containsComma || containsQuote;

            if (containsQuote)
            {
                text = text.Replace("\"", "\"\"");
            }

            if (mustQuote)
            {
                return "\"" + text + "\"";  // Quote the cell and replace embedded quotes with double-quote
            }
            else
            {
                return text;
            }
        }

        
        protected void Button2_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "submit", "unRunnig();", true);
            
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
                    string platformID = queryFetcher.QueryParam_Fetch(platform.UniqueID, "");
                    string userName = string.IsNullOrEmpty(platformID) ? queryFetcher.QueryParam_Fetch(username.UniqueID, "") : "";
                    string operation1 = queryFetcher.QueryParam_Fetch(op1.UniqueID, "");
                    string operation2 = queryFetcher.QueryParam_Fetch(op2.UniqueID, "");
                    string operation3 = queryFetcher.QueryParam_Fetch(op3.UniqueID, "");
                    string operation = "";
                    
                    int errorCode = -1;
                    
                    if (string.IsNullOrEmpty(sdate) && string.IsNullOrEmpty(edate))
                    {
                        string errorMsg = "alert('" + Resources.languageResource.lang_MsgExcelDate + "');";
                        Page.ClientScript.RegisterStartupScript(GetType(), "alert", errorMsg, true);
                    }
                    else
                    {
                        long AID = string.IsNullOrEmpty(platformID) ? GMDataManager.GetSearchAID_BYUserName(ref tb, userName) : GMDataManager.GetSearchAID_BYSnailPlatformID(ref tb, platformID).AID;
                        if (AID > 0)
                        {
                            if (!string.IsNullOrEmpty(operation1))
                                operation = operation1;
                            if (!string.IsNullOrEmpty(operation2))
                                operation = string.IsNullOrEmpty(operation) ? operation2 : string.Format("'{0}','{1}'", operation, operation2);
                            if (!string.IsNullOrEmpty(operation3))
                                operation = string.IsNullOrEmpty(operation) ? operation3 : string.Format("'{0}','{1}'", operation, operation3);

                            if (string.IsNullOrEmpty(operation))
                            {
                                List<string> oplist = op1.Items.Cast<ListItem>().Where(item => !string.IsNullOrEmpty(item.Value)).Select(item => item.Value).ToList();
                                oplist.ForEach(item =>
                                {
                                    if (!string.IsNullOrEmpty(operation))
                                        operation = operation + ",";
                                    operation = operation + string.Format("'{0}'", item);
                                });
                            }

                            List<Request_Log> list = GMDataManager.GetUserAllLogList_Excel(ref tb, AID, errorCode, operation, sdate, edate);

                            string lang = GMDataManager.GetGmToolLanguage();
                            string charSet = "utf-8";
                            if (lang == "kr")
                                charSet = "euc-kr";
                            else
                                charSet = "GB2312";

                            string filename = string.Format("attachment;filename=useralllog_{0}.csv", DateTime.Now);
                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = charSet;
                            Response.ContentEncoding = System.Text.Encoding.GetEncoding(charSet);
                            Response.AddHeader("content-disposition", filename);
                            Response.ContentType = "text/csv";

                            System.Text.StringBuilder sb = new System.Text.StringBuilder();

                            for (int i = 0; i < dataList.Columns.Count - 1; i++)
                            {
                                if (!string.IsNullOrEmpty(dataList.Columns[i].HeaderText))
                                    sb.Append(dataList.Columns[i].HeaderText + ',');
                            }
                            foreach (Request_Log item in list)
                            {
                                sb.Append("\r\n");
                                item.RequestParams = item.RequestParams.Replace("\"", "\"\"");
                                if (string.IsNullOrEmpty(item.BaseJson))
                                    item.BaseJson = GMDataManager.GetLogDecryptData(item.ResponseResult);
                                if(item.Operation.Equals("login")){
                                    item.BaseJson = mJsonSerializer.RemoveJson(item.BaseJson, "lastclearstage");
                                    item.BaseJson = mJsonSerializer.RemoveJson(item.BaseJson, "my_daily_event");
                                    item.BaseJson = mJsonSerializer.RemoveJson(item.BaseJson, "firstpay_reward_list");
                                    item.BaseJson = mJsonSerializer.RemoveJson(item.BaseJson, "event_daily_list");
                                    item.BaseJson = mJsonSerializer.RemoveJson(item.BaseJson, "gacha_info");
                                    item.BaseJson = mJsonSerializer.RemoveJson(item.BaseJson, "event_7day_flag");
                                    item.BaseJson = mJsonSerializer.RemoveJson(item.BaseJson, "vip_reward_list");
                                    item.BaseJson = mJsonSerializer.RemoveJson(item.BaseJson, "shopitemlist");
                                }
                                item.BaseJson = CsvQuote(item.BaseJson);
                                item.ResponseResult = item.ResponseResult.Replace("\"", "\"\"");
                                item.DetailDBLog = item.DetailDBLog.Replace("\"", "|");
                                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(item.DetailDBLog);
                                string strBase64 = Convert.ToBase64String(bytes);
                                string stritem = string.Format(@"{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}"
                                                                , item.regdate, item.CID, item.ErrorCode, item.RequestURL, item.Operation
                                                                , "\"" + item.RequestParams + "\"", item.BaseJson , "\"" + item.ResponseResult + "\"", "\"" + strBase64 + "\"", item.requesttime);
                                sb.Append(stritem);
                            }
                            Response.Output.Write(sb.ToString());
                            Response.Flush();
                            HttpContext.Current.ApplicationInstance.CompleteRequest();                            
                        }
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