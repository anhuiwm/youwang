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
namespace TheSoulGMTool.management
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
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
                    GMDataManager.GetServerinit(ref tb, serverID);
                    if (serverID > -1)
                    {
                        List<BestGachaReward> DataList = new List<BestGachaReward>();
                        List<System_Gacha_Best_DropGrop> rewardList = GMDataManager.GetBestGachaDropGroupList(ref tb, reward);
                        int allProb = rewardList.Sum(item => item.DropProb);
                        rewardList.ForEach(item =>
                        {
                            BestGachaReward data = new BestGachaReward();
                            System_Item_Base itemInfo = ItemManager.GetSystem_Item_Base(ref tb, item.DropItemID);
                            data.itemName = GMDataManager.GetItmeName(ref tb, itemInfo.Name);
                            data.itemIndex = item.DropIndex;
                            data.maxNum = item.DropMaxNum;
                            data.minNum = item.DropMinNum;
                            data.itemProb = (float)item.DropProb / allProb;
                            DataList.Add(data);
                        });
                        dataList.DataSource = DataList;
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
    }
}