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

namespace TheSoulGMTool.member
{
    public partial class menuList : System.Web.UI.Page
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
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb);

                    List<GM_menu> list = GMDataManager.GetAllMenuList(ref tb);
                    List<GM_menu_large> largeMenuList = GMDataManager.GetLargeMenu(ref tb);
                    list.ForEach(item => {
                        GM_menu_large largeMenu = largeMenuList.Find(largemenuItem => largemenuItem.mdiv == item.mdiv);
                        if (largeMenu == null)
                            item.linkurl = "";
                        else
                            item.linkurl = largeMenu.menuname;
                    });
                    dataList.DataSource = list;
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
    }
}