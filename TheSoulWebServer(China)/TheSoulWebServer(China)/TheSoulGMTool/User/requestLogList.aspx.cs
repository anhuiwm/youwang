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
    public partial class requestLogList : System.Web.UI.Page
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
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, serverID);
                    if (string.IsNullOrEmpty(sDate.Text))
                        sDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                    if (string.IsNullOrEmpty(eDate.Text))
                        eDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                    if (Page.IsPostBack)
                    {
                        string sdate = queryFetcher.QueryParam_Fetch(sDate.UniqueID, "").Replace("/", "-");
                        string edate = queryFetcher.QueryParam_Fetch(eDate.UniqueID, "").Replace("/", "-");
                        long aid = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch(AID.UniqueID, "0"));
                        int error = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(errorCode.UniqueID, "0"));
                        string operation = queryFetcher.QueryParam_Fetch(op.UniqueID, "");
                        string fields1 = queryFetcher.QueryParam_Fetch(field1.UniqueID, "");
                        string fields2 = queryFetcher.QueryParam_Fetch(field2.UniqueID, "");
                        string fields3 = queryFetcher.QueryParam_Fetch(field3.UniqueID, "");
                        string fields4 = queryFetcher.QueryParam_Fetch(field4.UniqueID, "");
                        string fields5 = queryFetcher.QueryParam_Fetch(field5.UniqueID, "");
                        string fields6 = queryFetcher.QueryParam_Fetch(field6.UniqueID, "");
                        string fields7 = queryFetcher.QueryParam_Fetch(field7.UniqueID, "");
                        string fields8 = queryFetcher.QueryParam_Fetch(field8.UniqueID, "");
                        List<string> fieldList = new List<string>();
                        if (!string.IsNullOrEmpty(fields1))
                        {
                            field1.Checked = true;
                            fieldList.Add(fields1);
                        }
                        if (!string.IsNullOrEmpty(fields2))
                        {
                            field2.Checked = true;
                            fieldList.Add(fields2);
                        }
                        if (!string.IsNullOrEmpty(fields3))
                        {
                            field3.Checked = true;
                            fieldList.Add(fields3);
                        }
                        if (!string.IsNullOrEmpty(fields4))
                        {
                            field4.Checked = true;
                            fieldList.Add(fields4);
                        }
                        if (!string.IsNullOrEmpty(fields5))
                        {
                            field5.Checked = true;
                            fieldList.Add(fields5);
                        }
                        if (!string.IsNullOrEmpty(fields6))
                        {
                            field6.Checked = true;
                            fieldList.Add(fields6);
                        }
                        if (!string.IsNullOrEmpty(fields7))
                        {
                            field7.Checked = true;
                            fieldList.Add(fields7);
                        }
                        if (!string.IsNullOrEmpty(fields8))
                        {
                            field8.Checked = true;
                            fieldList.Add(fields8);
                        }
                        op.Text = operation;
                        AID.Text = aid.ToString();
                        errorCode.Text = error.ToString();
                        sDate.Text = sdate;
                        eDate.Text = edate;
                        for (ushort i = (ushort)dataList.Columns.Count; i > 0; i--)
                        { /// Removes columns at the end (created for other category)
                            dataList.Columns.RemoveAt(i - 1);
                        }
                        foreach (string item in fieldList)
                        {
                            BoundField bf = new BoundField();
                            bf.HtmlEncode = false;
                            bf.HeaderText = item;
                            bf.DataField = item;
                            dataList.Columns.Add(bf);
                        }
                        List<Request_Log> list = GMDataManager.GetEorrorLogList(ref tb, aid, error, operation, sdate, edate);
                        list.ForEach(item =>
                        {
                            item.DetailDBLog = item.DetailDBLog.Substring(1, item.DetailDBLog.Length - 1);
                            item.DetailDBLog = item.DetailDBLog.Replace("\\r\\n", "<br/>");
                            item.DetailDBLog = item.DetailDBLog.Replace("\"query[", "<br/><br/>\"query[");
                        });
                        dataList.DataSource = list;
                        dataList.DataBind();
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

        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
            // Confirms that an HtmlForm control is rendered for the specified ASP.NET server control at run time.
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string filename = string.Format("attachment;filename=erroLogList_{0}.xls", DateTime.Now);
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", filename);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                dataList.AllowPaging = false;
                this.DataBind();

                foreach (TableCell cell in dataList.HeaderRow.Cells)
                {
                    cell.BackColor = dataList.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in dataList.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = dataList.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = dataList.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                dataList.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
    }
}