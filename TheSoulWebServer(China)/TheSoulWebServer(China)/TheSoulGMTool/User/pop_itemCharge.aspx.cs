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

namespace TheSoulGMTool.User
{
    public partial class pop_itemCharge : System.Web.UI.Page
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
            if (Page.IsPostBack)
            {
                long itemID = 0;
                int itemCount = 0;
                int itemLevel = 0;
                int itemGrade = 0;
                string itemName = "";
                String csname = "ButtonClickScript";
                ClientScriptManager cs = Page.ClientScript;

                Type cstype = this.GetType();
                TxnBlock tb = new TxnBlock();
                {
                    try
                    {
                        long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
                        GMDataManager.GetServerinit(ref tb, serverID);
                        itemID = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch(itemid.UniqueID, "0"));
                        itemCount = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(itema.UniqueID, "0"));
                        itemLevel = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(level.UniqueID, "0"));
                        itemGrade = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(grade.UniqueID, "0"));
                        System_Item_Base itemInfo = ItemManager.GetSystem_Item_Base(ref tb, itemID);
                        if (Item_Define.SystemItemType[itemInfo.ItemClass] == Item_Define.eSystemItemType.ItemClass_Equip)
                            itemCount = 1;

                        if (itemInfo.Item_IndexID > 0)
                        {
                            itemName = GMDataManager.GetItmeName(ref tb, itemInfo.Name);
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

                    string scriptString = "<script type='text/javascript'>popup_itemadd(" + itemID + ",\"" + itemName + "\"," + itemGrade + "," + itemLevel + "," + itemCount + ");</script>";
                    cs.RegisterStartupScript(cstype, csname, scriptString, false);
                }
            }
        }
    }
}