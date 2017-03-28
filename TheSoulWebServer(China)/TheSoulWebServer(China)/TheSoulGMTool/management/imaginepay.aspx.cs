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
    public partial class imaginepay : System.Web.UI.Page
    {
        protected List<GMShopItem> itemList = new List<GMShopItem>();

        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            queryFetcher.bDBLog = true;
            string retJson = "";
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                    if (Page.IsPostBack)
                    {   
                        string platformID = queryFetcher.QueryParam_Fetch(uid.UniqueID, "");
                        string userName = string.IsNullOrEmpty(platformID) ? queryFetcher.QueryParam_Fetch(username.UniqueID, "") : string.Empty;
                        long AID = queryFetcher.QueryParam_FetchLong(userid.UniqueID);
                        long shopGoodsID = queryFetcher.QueryParam_FetchLong(goodsID.UniqueID);

                        if (AID > 0 && shopGoodsID > 0)
                        {
                            long CID = AccountManager.GetAccountData(ref tb, AID).EquipCID;
                            string platform_id = GlobalManager.GetUserAccountConfig(ref tb, AID).platform_user_id;
                            Shop_Define.eShopItemType shopItemType = Shop_Define.eShopItemType.None;
                            Shop_Define.eShopType ShopType = Shop_Define.eShopType.None;
                            List<User_Inven> makeRealItem = new List<User_Inven>();
                            Item_Define.eItemBuyPriceType BuyPriceType = Item_Define.eItemBuyPriceType.None;
                            int BuyPriceValue = 0;

                            List<System_Package_List> getPackageList = ShopManager.GetShop_System_Package_List(ref tb, Shop_Define.eBillingType.None, true, true);
                            System_Package_List pickPackage = getPackageList.Find(item => item.Package_ID == shopGoodsID);
                            bool isCheap = (pickPackage == null);
                            if (isCheap)
                            {
                                getPackageList = ShopManager.GetShop_System_Package_Cheap_List(ref tb, Shop_Define.eBillingType.None, true, true);
                                pickPackage = getPackageList.Find(item => item.Package_ID == shopGoodsID);
                                isCheap = (pickPackage != null);
                            }
                            if (pickPackage != null)
                            {
                                shopItemType = isCheap ? Shop_Define.eShopItemType.Chep_Package : Shop_Define.eShopItemType.Package;
                                retError = Result_Define.eResult.SUCCESS;
                            }
                            else
                            {
                                bool isShopType = true;
                                foreach (Shop_Define.eShopType item in Enum.GetValues(typeof(Shop_Define.eShopType)))
                                {
                                    System_Shop_Goods ShopGoods = ShopManager.GetSystem_ShopList(ref tb, item, true, false).Find(goods => goods.Shop_Goods_ID == shopGoodsID);
                                    isShopType = (ShopGoods == null);
                                    if (!isShopType)
                                    {
                                        ShopType = item;
                                        shopItemType = ShopType == Shop_Define.eShopType.Billing ? Shop_Define.eShopItemType.Cash : Shop_Define.eShopItemType.Item;
                                        retError = Result_Define.eResult.SUCCESS;
                                        break;
                                    }
                                }
                                if (isShopType)
                                    retError = Result_Define.eResult.SHOP_ID_NOT_FOUND;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                retError = ShopManager.BuyShopItemProgress(ref tb, AID, CID, shopGoodsID, shopItemType, ShopType, 1,
                                                        ref makeRealItem, ref BuyPriceType, ref BuyPriceValue, Shop_Define.eBillingType.Kr_aOS_Google, platform_id, "", "", "", true, false);
                                //TODD : 재화로그 처리
                                if (retError == Result_Define.eResult.SUCCESS)
                                    retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_GUILD, AID, AccountManager.GetSimpleAccountInfo(ref tb, AID).username, GMResult_Define.ControlType.USER_IMAGINE_PAY, queryFetcher.GetReqParams(), serverID.ToString());
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    userid.Value = string.Empty;
                                    goodsID.Value = string.Empty;
                                    string select = "$(\"#shop_" + AID + " option:eq(0)\").attr(\"selected\", \"selected\");";
                                    Page.ClientScript.RegisterStartupScript(GetType(), "selected", select, true);
                                }
                                else
                                {
                                    string msg = "alert('" + Resources.languageResource.lang_msgShopError + "');";
                                    Page.ClientScript.RegisterStartupScript(GetType(), "alert", msg, true);
                                    retError = Result_Define.eResult.SUCCESS;
                                }

                            }
                            else
                            {
                                string msg = "";
                                if(retError == Result_Define.eResult.SHOP_ID_NOT_FOUND)
                                    msg = "alert('" + Resources.languageResource.lang_msgShopID + "');";
                                else
                                    msg = "alert('" + Resources.languageResource.lang_msgShopError + "');";
                                Page.ClientScript.RegisterStartupScript(GetType(), "alert", msg, true);
                                retError = Result_Define.eResult.SUCCESS;
                            }
                            retJson = queryFetcher.GM_Render(retError);
                        }
                        else
                        {//userlist
                            List<GM_UserAccountSimple> userlist = new List<GM_UserAccountSimple>();
                            GMShopItem emptyItem = new GMShopItem();
                            emptyItem.itemName = "Select";
                            emptyItem.itemID = 0;
                            itemList.Add(emptyItem);
                            foreach (Shop_Define.eShopType item in Enum.GetValues(typeof(Shop_Define.eShopType)))
                            {
                                if (item != Shop_Define.eShopType.None)
                                {
                                    List<System_Shop_Goods> ShopGoods = ShopManager.GetSystem_ShopList(ref tb, item, true, false);
                                    ShopGoods.ForEach(goodsItem =>
                                    {
                                        GMShopItem shopitem = new GMShopItem();
                                        shopitem.itemID = goodsItem.Shop_Goods_ID;
                                        shopitem.itemName = string.Format("{0}_{1}", goodsItem.Shop_Goods_ID, GMDataManager.GetItmeName(ref tb, goodsItem.NameCN1));
                                        if (item == Shop_Define.eShopType.Billing && goodsItem.Type == 1)
                                            shopitem.itemName = shopitem.itemName + "（" + GetGlobalResourceObject("languageResource", "lang_firstPay") + ")";
                                        itemList.Add(shopitem);
                                    });
                                }
                            }
                            List<System_Package_List> getPackageList = ShopManager.GetShop_System_Package_List(ref tb, Shop_Define.eBillingType.None, true, true);
                            getPackageList.AddRange(ShopManager.GetShop_System_Package_Cheap_List(ref tb, Shop_Define.eBillingType.None, true, true));
                            getPackageList.ForEach(goodsItem =>
                            {
                                GMShopItem shopitem = new GMShopItem();
                                shopitem.itemID = goodsItem.Package_ID;
                                shopitem.itemName = string.Format("{0}_{1}",goodsItem.Package_ID, GMDataManager.GetItmeName(ref tb, goodsItem.NameCN1));
                                itemList.Add(shopitem);
                            });
                            if (!string.IsNullOrEmpty(userName))
                            {
                                userlist = GMDataManager.GetUserSimpleList_BYUserName(ref tb, userName);
                                userlist.ForEach(item =>
                                {
                                    GM_Global_UserSimple itemInfo = GMDataManager.GetUserGloblaSimpleInfo(ref tb, item.AID);
                                    item.platform_idx = itemInfo.platform_idx;
                                    item.platform_user_id = itemInfo.platform_user_id;
                                });
                            }
                            else if (!string.IsNullOrEmpty(platformID))
                            {
                                GM_Global_UserSimple user = GMDataManager.GetSearchAID_BYSnailPlatformID(ref tb, platformID);
                                GM_UserAccountSimple userInfo = new GM_UserAccountSimple();
                                userInfo.UserName = AccountManager.GetAccountData(ref tb, user.AID, false).UserName;
                                userInfo.AID = user.AID;
                                userInfo.platform_idx = user.platform_idx;
                                userInfo.platform_user_id = user.platform_user_id;
                                userlist.Add(userInfo);
                            }

                            dataList.DataSource = userlist;
                            dataList.DataBind();
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
                if (retError == Result_Define.eResult.SUCCESS)
                {
                    string msg = "alert('" + Resources.languageResource.lang_msgShopOk + "');";
                    Page.ClientScript.RegisterStartupScript(GetType(), "alert", msg, true);
                    
                }
            }
        }

        protected void dataList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dataList.PageIndex = e.NewPageIndex;
            dataList.DataBind();
        }

        protected void dataList_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            var grid = sender as GridView;
            if (grid == null)
            {
                return;
            }

            for (var i = 0; i < grid.Columns.Count; i++)
            {
                var column = grid.Columns[i] as TemplateField;
                if (column == null && i < grid.Columns.Count-1)
                    continue;
                if (!string.IsNullOrEmpty(column.HeaderText))
                {
                    long AID = System.Convert.ToInt64(dataList.DataKeys[e.Row.RowIndex].Values[0]);
                    var cell = e.Row.Cells[i];
                    DropDownList dropdown = new DropDownList();
                    dropdown.DataSource = itemList;
                    dropdown.DataTextField = "itemName";
                    dropdown.DataValueField = "itemID";
                    dropdown.DataBind();
                    dropdown.ID = "shop_" + AID;
                    cell.Controls.Add(dropdown);
                }
            }
        }
        
    }
}