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


namespace TheSoulGMTool
{
    public partial class userList : System.Web.UI.Page
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
                    string name = queryFetcher.QueryParam_Fetch("username", "");
                    string platformID = queryFetcher.QueryParam_Fetch("platformid", "");
                    int slevel = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("slevel", "0"));
                    int elevel = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("elevel", "0"));
                    string sdate = queryFetcher.QueryParam_Fetch("sdate", "").Replace("/","-");
                    string edate = queryFetcher.QueryParam_Fetch("edate", "").Replace("/", "-");

                    List<ListItem> levelList = GMDataManager.GetLevelListItem(ref tb, true);
                    sLevel.DataSource = levelList;
                    sLevel.DataTextField = "Text";
                    sLevel.DataValueField = "Value";
                    sLevel.DataBind();

                    eLevel.DataSource = levelList;
                    eLevel.DataTextField = "Text";
                    eLevel.DataValueField = "Value";
                    eLevel.DataBind();

                    username.Text = name;
                    sDate.Text = sdate;
                    eDate.Text = edate;
                    sLevel.SelectedValue = slevel.ToString();
                    eLevel.SelectedValue = elevel.ToString();
                    platform.Text = platformID;
                    if (Page.IsPostBack)
                    {
                        slevel = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(sLevel.UniqueID, "0"));
                        elevel = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(eLevel.UniqueID, "0"));
                        name = queryFetcher.QueryParam_Fetch(username.UniqueID, "");
                        sdate = queryFetcher.QueryParam_Fetch(sDate.UniqueID, "").Replace("/", "=");
                        edate = queryFetcher.QueryParam_Fetch(eDate.UniqueID, "").Replace("/", "=");
                        platformID = queryFetcher.QueryParam_Fetch(platform.UniqueID, "");
                    }

                    if (serverID > -1 && ((slevel > 0 && elevel > 0) || !string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(platformID) || (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))))
                    {
                        sLevel.SelectedValue = slevel.ToString();
                        eLevel.SelectedValue = elevel.ToString();
                        username.Text = name;
                        sDate.Text = sdate;
                        eDate.Text = edate;
                        platform.Text = platformID;
                        
                        List<GM_UserAccount> list = GMDataManager.GetUserList(ref tb, name, platformID, slevel, elevel, sdate, edate);
                        list.ForEach(item => {
                            GM_Global_UserSimple itemInfo = GMDataManager.GetUserGloblaSimpleInfo(ref tb, item.AID);
                            item.SNO = itemInfo.platform_idx;
                            item.equipclass = CharacterManager.GetCharacter(ref tb, item.AID, item.EquipCID, false).Class;
                            int stageID = Dungeon_Manager.GetUser_Mission_LastStage(ref tb, item.AID);
                            int world = 1;
                            int stage = 1;
                            if (stageID > 0)
                            {
                                string stageName = Dungeon_Manager.GetSystem_MissionStageInfo(ref tb, stageID).NamingCN;
                                stageName = stageName.Replace("STRING_NAMING_SCENARIO_WORLD_", "").Replace("_STAGE", "");
                                string[] temName = stageName.Split('_');
                                if (!string.IsNullOrEmpty(temName[0]))
                                    world = System.Convert.ToInt32(temName[0]);
                                if (!string.IsNullOrEmpty(temName[1]))
                                    stage = System.Convert.ToInt32(temName[1]);
                            }
                            item.stageInfo = string.Format("{0}-{1}", world, stage);
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
                        gmid = HttpContext.Current.Request.Cookies["mseedadmin"]["userid"];
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
            string filename = string.Format("attachment;filename=userlist_{0}.xls", DateTime.Now);
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