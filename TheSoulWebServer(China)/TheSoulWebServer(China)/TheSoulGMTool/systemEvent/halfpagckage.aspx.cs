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
    public partial class halfpagckage : System.Web.UI.Page
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

                    if (serverID > -1)
                    {
                        
                        List<System_Event_7Day_Package_List> eventList = TriggerManager.GetSystem_7Day_Event_Package_List(ref tb, true);
                        eventList.ForEach(item => {
                            string strReward = "";
                            List<System_Package_RewardBox> rewardList = new List<System_Package_RewardBox>();
                            if (item.Reward_Box1ID > 0)
                            {
                                rewardList.AddRange(TriggerManager.GetSystem_7Day_Event_Package_RewardBox(ref tb, item.Reward_Box1ID, false));
                            }
                            if (item.Reward_Box2ID > 0)
                            {
                                rewardList.AddRange(TriggerManager.GetSystem_7Day_Event_Package_RewardBox(ref tb, item.Reward_Box2ID, false));
                            }
                            if (item.Reward_Box3ID > 0)
                            {
                                rewardList.AddRange(TriggerManager.GetSystem_7Day_Event_Package_RewardBox(ref tb, item.Reward_Box3ID, false));
                            }
                            if (item.Reward_Box4ID > 0)
                            {
                                rewardList.AddRange(TriggerManager.GetSystem_7Day_Event_Package_RewardBox(ref tb, item.Reward_Box4ID, false));
                            }
                            if (rewardList.Count > 0)
                            {
                                foreach (System_Package_RewardBox rewardItem in rewardList)
                                {
                                    strReward = strReward + rewardItem.Item_TargetType + " " + rewardItem.Item_Num + "<br />";
                                }
                            }
                            item.ToolTipCN = strReward;
                        });
                        dataList.DataSource = eventList;
                        dataList.DataBind();
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
    }
}