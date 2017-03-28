using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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


namespace TheSoulGMTool.systemEvent
{
    public partial class eventList : System.Web.UI.Page
    {
        protected int group = 1;
        
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            queryFetcher.bDBLog = true;
            string retJson = "";
            group = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("group", "1"));
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, serverID);

                    if (serverID > -1)
                    {
                        List<GM_EventGroup_Admin> eventGroupList = GMDataManager.GetEventAdminList(ref tb, group);
                        eventType.DataSource = eventGroupList;
                        eventType.DataTextField = "Event_Title";
                        eventType.DataValueField = "Event_Type";
                        eventType.DataBind();
                        eventType.Items.Insert(0, new ListItem("select", ""));

                        string reqEventType = queryFetcher.QueryParam_Fetch(eventType.UniqueID, "");
                        if (string.IsNullOrEmpty(reqEventType))
                            reqEventType = queryFetcher.QueryParam_Fetch("eventType", "");
                        if (!string.IsNullOrEmpty(reqEventType))
                        {
                            eventType.SelectedValue = reqEventType;
                            List<System_Event> eventList = GMDataManager.GetSystem_Event_List(ref tb, group).FindAll(item => item.Event_Type.Equals(reqEventType));
                            eventList.ForEach(
                                item => { 
                                    item.ActiveTriggerType1 = GMDataManager.GetSystemTriggerType(ref tb, item.ActiveTriggerType1).Trigger; 
                                    item.ActiveTriggerType2 = GMDataManager.GetSystemTriggerType(ref tb, item.ActiveTriggerType2).Trigger;
                                    item.ClearTriggerType1 = GMDataManager.GetSystemTriggerType(ref tb, item.ClearTriggerType1).Trigger;
                                    item.ClearTriggerType2 = GMDataManager.GetSystemTriggerType(ref tb, item.ClearTriggerType2).Trigger;
                                    item.Event_Type = TriggerManager.GetSystem_EventGroup_Admin(ref tb, false).Find(setItem => setItem.Event_Type.Equals(item.Event_Type)).Event_Title;
                                }
                            );
                            groupList.DataSource = eventList;
                            groupList.DataBind();
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
            }
        }

        protected void groupList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            groupList.PageIndex = e.NewPageIndex;
            groupList.DataBind();
        }

    }
}