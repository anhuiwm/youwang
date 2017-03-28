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
namespace TheSoulGMTool.systemEvent
{
    public partial class popReward : System.Web.UI.Page
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
            //bool result = false;

            long reward = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("reward", "0"));
            int rewardType = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("rewardType", "1"));
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, serverID);
                    
                    if (serverID > -1)
                    {
                        string dbkey = GMData_Define.ShardingDBName;
                        string strReward = "";
                        if (rewardType > 1)
                        {
                            List<System_Event_7Day_Reward> rewardList = TriggerManager.GetSystem_7Day_Event_Package_RewardBox(ref tb, reward, true, dbkey);

                            foreach (System_Event_7Day_Reward item in rewardList)
                            {
                                //item.EventItem_ID
                                System_Item_Base itemInfo = ItemManager.GetSystem_Item_Base(ref tb, item.Item_ID, dbkey);
                                if (itemInfo.Item_IndexID > 0)
                                {
                                    if (string.IsNullOrEmpty(strReward))
                                        strReward = GMDataManager.GetItmeName(ref tb, itemInfo.Name) + " " + item.Item_Num;
                                    else
                                        strReward = strReward + "<br>" + GMDataManager.GetItmeName(ref tb, itemInfo.Name) + " " + item.Item_Num;
                                }
                            }
                        }
                        else
                        {
                            List<System_Event_Reward_Box> rewardList = TriggerManager.GetSystem_Event_Reward_Box_List(ref tb, reward, true, dbkey);
                            foreach (System_Event_Reward_Box item in rewardList)
                            {
                                //item.EventItem_ID
                                System_Item_Base itemInfo = ItemManager.GetSystem_Item_Base(ref tb, item.EventItem_ID, dbkey);
                                if (itemInfo.Item_IndexID > 0)
                                {
                                    if (string.IsNullOrEmpty(strReward))
                                        strReward = GMDataManager.GetItmeName(ref tb, itemInfo.Name) + " " + item.EventItem_Num;
                                    else
                                        strReward = strReward + "<br>" + GMDataManager.GetItmeName(ref tb, itemInfo.Name) + " " + item.EventItem_Num;
                                }
                            }
                        }
                        labReward.Text = strReward;
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
    }
}