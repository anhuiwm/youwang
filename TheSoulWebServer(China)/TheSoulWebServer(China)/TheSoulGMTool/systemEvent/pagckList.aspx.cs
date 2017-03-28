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
    public partial class pagckList : System.Web.UI.Page
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
                    List<GM_System_Package_List> packList = GMDataManager.GetGM_Package_List(ref tb, true);
                    dataList.DataSource = packList;
                    dataList.DataBind();
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

        public string GetReward(long id1, long id2, long id3, long id4)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            string retJson = "";
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    List<System_Package_RewardBox> rewardList = new List<System_Package_RewardBox>();
                    long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
                    GMDataManager.GetServerinit(ref tb, serverID);
                    if (id1 > 0)
                    {
                        rewardList.AddRange(ShopManager.GetShop_System_Package_RewardBox(ref tb, id1, true));
                    }
                    if (id2 > 0)
                    {
                        rewardList.AddRange(ShopManager.GetShop_System_Package_RewardBox(ref tb, id2, true));
                    }
                    if (id3 > 0)
                    {
                        rewardList.AddRange(ShopManager.GetShop_System_Package_RewardBox(ref tb, id3, true));
                    }
                    if (id4 > 0)
                    {
                        rewardList.AddRange(ShopManager.GetShop_System_Package_RewardBox(ref tb, id4, true));
                    }
                    if (rewardList.Count > 0)
                    {
                        foreach (System_Package_RewardBox item in rewardList)
                        {
                            retJson = retJson + item.Item_TargetType + " " + item.Item_Num + "<br>";
                        }
                    }
                }
                catch (Exception errorEx)
                {
                    retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(errorEx.Message), Result_Define.eResult.System_Exception);
                }
                tb.EndTransaction(queryFetcher.Render_errorFlag);
                tb.Dispose();
            }
            return retJson;
        }

        protected void dataList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dataList.PageIndex = e.NewPageIndex;
            dataList.DataBind();
        }
    }
}