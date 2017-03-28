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
    public partial class popCheapReward : System.Web.UI.Page
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

            long reward = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("packid", "0"));
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, serverID);

                    if (serverID > -1)
                    {
                        string dbkey = GMData_Define.ShardingDBName;
                        bool checkPackage = false;
                        GM_System_Package_List dataInfo1 = GMDataManager.GetGM_Package_Data(ref tb, reward, dbkey, checkPackage);
                        long index2 = GMDataManager.GetPackageCheapIndex1Yuan(ref tb, reward, dataInfo1.SaleStartTime.ToString("yyyy-MM-dd HH:mm:ss"), dataInfo1.SaleEndTime.ToString("yyyy-MM-dd HH:mm:ss"), dbkey);
                        GM_System_Package_List dataInfo2 = GMDataManager.GetGM_Package_Data(ref tb, index2, dbkey, checkPackage);

                        // 1위안
                        List<System_Package_RewardBox> rewardList1 = ShopManager.GetShop_System_Package_Cheap_RewardBox(ref tb, dataInfo2.Reward_Box1ID, true, dbkey);
                        rewardList1.ForEach(item =>
                        {
                            System_Item_Base itemInfo = ItemManager.GetSystem_Item_Base(ref tb, item.Item_ID, dbkey);
                            if (!string.IsNullOrEmpty(itemInfo.Name))
                                item.Item_TargetType = GMDataManager.GetItmeName(ref tb, itemInfo.Name, dbkey);
                        });

                        // 3위안
                        List<System_Package_RewardBox> rewardList2 = ShopManager.GetShop_System_Package_Cheap_RewardBox(ref tb, dataInfo1.Reward_Box1ID, true, dbkey);
                        rewardList2.ForEach(item =>
                        {
                            System_Item_Base itemInfo = ItemManager.GetSystem_Item_Base(ref tb, item.Item_ID, dbkey);
                            if (!string.IsNullOrEmpty(itemInfo.Name))
                                item.Item_TargetType = GMDataManager.GetItmeName(ref tb, itemInfo.Name, dbkey);
                        });
                        dataList.DataSource = rewardList2;
                        dataList.DataBind();

                        GridView1.DataSource = rewardList1;
                        GridView1.DataBind();
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