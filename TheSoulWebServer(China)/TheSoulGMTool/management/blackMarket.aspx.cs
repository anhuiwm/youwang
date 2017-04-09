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
    public partial class blackMarket : System.Web.UI.Page
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
            bool result = false;

            string reqServer = queryFetcher.QueryParam_Fetch("serverid", "");
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
            int reqSlotID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("slotid", "1"));

            Dictionary<long, TxnBlock> TxnBlackServer = new Dictionary<long, TxnBlock>();
            TxnBlock tb = new TxnBlock();

            try
            {
                GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                TxnBlackServer.Add(serverID, tb);
                tb.IsoLevel = IsolationLevel.ReadCommitted;

                slotID.Value = reqSlotID.ToString();

                string serverlist = GMDataManager.GetServerCheckList(ref tb, serverID);
                change_server.InnerHtml = serverlist;

                Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                if (!Page.IsPostBack)
                {

                    List<Admin_System_Item> allList = null;
                    if (reqSlotID == 1 || reqSlotID == 2)
                        allList = GMDataManager.GetItemClassItemList(ref tb, "Soul_Parts");
                    else if (reqSlotID == 3 || reqSlotID == 4)
                        allList = GMDataManager.GetNoneEquipSoulItemList(ref tb);
                    else
                        allList = GMDataManager.GetItemClassItemList(ref tb, "Soul_Equip");

                    ListItem selectItem = new ListItem("select", "-1");
                    item.DataSource = allList;
                    item.DataTextField = "Description";
                    item.DataValueField = "Item_IndexID";
                    item.DataBind();
                    item.Items.Insert(0, selectItem);
                }
                else
                {
                    if (!string.IsNullOrEmpty(reqServer))
                    {
                        string[] reqServerList = System.Text.RegularExpressions.Regex.Split(reqServer, ",");
                        foreach (string Key in reqServerList)
                        {
                            long ServerKey = System.Convert.ToInt64(Key);
                            if (!TxnBlackServer.ContainsKey(ServerKey))
                            {
                                TxnBlock tb2 = new TxnBlock();
                                TheSoulDBcon.GetInstance().TheSoulDBInitFromGlobal(ref tb2, (int)ServerKey, true);
                                TxnBlackServer.Add(ServerKey, tb2);
                            }
                        }
                        string nameCN = queryFetcher.QueryParam_Fetch(name.UniqueID, "");
                        int itemID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(item.UniqueID, "0"));
                        int itemCount = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(itema.UniqueID, "0"));
                        int itemPrice = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(price.UniqueID, "0"));
                        int itemProb = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(itemprob.UniqueID, "0"));

                        System_Shop_BlackMarket chageItem = new System_Shop_BlackMarket();
                        System_Item_Base itemInfo = ItemManager.GetSystem_Item_Base(ref tb, (long)itemID);
                        chageItem.ToolTipCN = "xxx";
                        chageItem.NameCN1 = nameCN.Replace("'", "''");
                        chageItem.ItemID = itemID;
                        chageItem.ItemNum = itemCount;
                        chageItem.ItemClass = itemInfo.ItemClass;
                        chageItem.Buy_PriceValue = itemPrice;
                        chageItem.ItemProb = itemProb;
                        chageItem.SlotID = reqSlotID;

                        retError = GMDataManager.InsertShopBlackMarket(ref TxnBlackServer, chageItem);
                        if (retError == Result_Define.eResult.SUCCESS)
                            retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, 0, "", GMResult_Define.ControlType.BLACKMARKET_ADD, queryFetcher.GetReqParams(), reqServer);
                        if (retError == Result_Define.eResult.SUCCESS)
                            result = true;
                        retJson = queryFetcher.GM_Render("", retError);
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
                foreach (KeyValuePair<long, TxnBlock> setItem in TxnBlackServer)
                {
                    setItem.Value.EndTransaction(queryFetcher.Render_errorFlag);
                    if (setItem.Key == serverID)
                    {
                        string gmid = "";
                        if (Request.Cookies.Count > 0)
                            gmid = GMDataManager.GetUserCookies("userid");
                        queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                    }
                    setItem.Value.Dispose();
                }
            }

            if (result)
                Response.Redirect("blackmarketList.aspx?ca2=" + queryFetcher.QueryParam_Fetch_Request("ca2", "1") + "&select_server=" + serverID + "&slotid=" + reqSlotID);

        }

    }
}