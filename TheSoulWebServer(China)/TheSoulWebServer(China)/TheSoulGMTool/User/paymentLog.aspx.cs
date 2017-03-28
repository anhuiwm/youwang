using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

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
    public partial class paymentLog : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                GetDataList(1);
        }

        protected void GetDataList(int pageIndex)
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
                    string sdate = queryFetcher.QueryParam_Fetch(sDate.UniqueID, "");
                    string edate = queryFetcher.QueryParam_Fetch(eDate.UniqueID, "");
                    string Username = queryFetcher.QueryParam_Fetch(username.UniqueID, "");
                    string Uid = queryFetcher.QueryParam_Fetch(uid.UniqueID, "");
                    int shop = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(shoptype.UniqueID, "-1"));
                    int PayResult = queryFetcher.QueryParam_FetchInt(payResult.UniqueID, -1);
                    long billingIndex = queryFetcher.QueryParam_FetchLong(billingindex.UniqueID);
                    List<ListItem> billingStatusItem = new List<ListItem>();
                    billingStatusItem.Add(new ListItem("All", "-1"));
                    Array itemNames = System.Enum.GetNames(typeof(Shop_Define.eBillingStatus));
                    foreach (String name in itemNames)
                    {
                        int value = (int)Enum.Parse(typeof(Shop_Define.eBillingStatus), name);
                        ListItem listItem = new ListItem(name, value.ToString());
                        billingStatusItem.Add(listItem);
                    }
                    payResult.DataSource = billingStatusItem;
                    payResult.DataTextField = "Text";
                    payResult.DataValueField = "Value";
                    payResult.DataBind();

                    payResult.SelectedValue = PayResult.ToString();
                    Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
                    if (billingIndex > 0)
                    {
                        retError = GMDataManager.SetBillingStatus(ref tb, billingIndex);
                    }
                    long totalCount = GMDataManager.GetBillingListCount(ref tb, Username, Uid, sdate, edate, PayResult, shop);

                    List<GM_Billing_List> logList = GMDataManager.GetBillingList(ref tb, pageIndex, Username, Uid, sdate, edate, PayResult, shop);
                    logList.ForEach(item =>
                    {
                        GM_Global_UserSimple userInfo = GMDataManager.GetUserGloblaSimpleInfo(ref tb, item.AID);
                        item.platform_idx = userInfo.platform_idx;
                        item.platform_user_id = userInfo.platform_user_id;
                        item.shopType = GMData_Define.ShopSaleType_List[(TheSoul.DataManager.Shop_Define.eShopSaleType)System.Convert.ToInt32(item.shopType)];
                        item.payResult = Enum.GetName(typeof(Shop_Define.eBillingStatus), item.Billing_Status);
                        if (string.IsNullOrEmpty(item.goodsName))
                            item.goodsName = GMDataManager.GetGM_Package_Data(ref tb, item.Shop_Goods_ID).NameCN1;
                        if (string.IsNullOrEmpty(item.goodsName))
                            item.goodsName = GMDataManager.GetGM_Package_Data(ref tb, item.Shop_Goods_ID, GMData_Define.ShardingDBName, false).NameCN1;
                    });
                    payList.DataSource = logList;
                    payList.DataBind();

                    GMDataManager.PopulatePager(ref dlPager, totalCount, pageIndex);
                    queryFetcher.GM_Render(retError);

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
                        gmid = HttpContext.Current.Request.Cookies["mseedadmin"]["userid"];
                    queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                    tb.Dispose();
                }
            }
        }

        protected void dlPager_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "PageNo")
            {
                int page = System.Convert.ToInt32(e.CommandArgument);
                this.GetDataList(page);
            }
        }

        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
            // Confirms that an HtmlForm control is rendered for the specified ASP.NET server control at run time.
        }

        protected void csvSave(object sender, EventArgs e)
        {

            WebQueryParam queryFetcher = new WebQueryParam(true);
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
                    GMDataManager.GetServerinit(ref tb, serverID);
                    string sdate = queryFetcher.QueryParam_Fetch(sDate.UniqueID, "");
                    string edate = queryFetcher.QueryParam_Fetch(eDate.UniqueID, "");
                    string Username = queryFetcher.QueryParam_Fetch(username.UniqueID, "");
                    string Uid = queryFetcher.QueryParam_Fetch(uid.UniqueID, "");
                    int shop = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(shoptype.UniqueID, "-1"));
                    int PayResult = queryFetcher.QueryParam_FetchInt(payResult.UniqueID, -1);
                    if (string.IsNullOrEmpty(sdate) && string.IsNullOrEmpty(edate))
                    {
                        string errorMsg = "alert('" + Resources.languageResource.lang_MsgExcelDate + "');";
                        Page.ClientScript.RegisterStartupScript(GetType(), "alert", errorMsg, true);
                    }
                    else
                    {
                        List<GM_Billing_List> logList = GMDataManager.GetBillingList_Excel(ref tb, Username, Uid, sdate, edate, PayResult, shop);

                        string lang = GMDataManager.GetGmToolLanguage();
                        string charSet = "utf-8";
                        if (lang == "kr")
                            charSet = "euc-kr";
                        else
                            charSet = "GB2312";

                        string filename = string.Format("attachment;filename=paymentLog_{0}.csv", DateTime.Now);
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = charSet;
                        Response.ContentEncoding = System.Text.Encoding.GetEncoding(charSet);
                        Response.AddHeader("content-disposition", filename);
                        Response.ContentType = "text/csv";

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        for (var i = 0; i < payList.Columns.Count - 1; i++)
                        {
                            var column = payList.Columns[i] as BoundField;
                            if (column == null && i < payList.Columns.Count - 1)
                                continue;
                            if (!string.IsNullOrEmpty(column.HeaderText))
                                sb.Append(column.HeaderText + ',');
                        }

                        logList.ForEach(item =>
                        {
                            sb.Append("\r\n");
                            GM_Global_UserSimple userInfo = GMDataManager.GetUserGloblaSimpleInfo(ref tb, item.AID);
                            item.platform_idx = userInfo.platform_idx;
                            item.platform_user_id = userInfo.platform_user_id;
                            item.shopType = GMData_Define.ShopSaleType_List[(TheSoul.DataManager.Shop_Define.eShopSaleType)System.Convert.ToInt32(item.shopType)];
                            item.payResult = Enum.GetName(typeof(Shop_Define.eBillingStatus), item.Billing_Status);
                            item.UserName = item.UserName.Replace("\"", "\"\"");
                            if (string.IsNullOrEmpty(item.goodsName))
                                item.goodsName = GMDataManager.GetGM_Package_Data(ref tb, item.Shop_Goods_ID).NameCN1;
                            if (string.IsNullOrEmpty(item.goodsName))
                                item.goodsName = GMDataManager.GetGM_Package_Data(ref tb, item.Shop_Goods_ID, GMData_Define.ShardingDBName, false).NameCN1;
                            string strItem = string.Format(@"{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}"
                                                            , item.regdate, item.payResult, item.ErrorCode, item.platform_user_id, "\"" + item.UserName + "\""
                                                            , item.Shop_Goods_ID, item.goodsName, item.shopType
                                                            , item.ItemDay, item.ItemNum, item.Bonus_ItemNum, item.Buy_PriceValue);
                            sb.Append(strItem);
                        });
                        Response.Output.Write(sb.ToString());
                        Response.Flush();
                        Response.End();
                    }
                    queryFetcher.GM_Render(Result_Define.eResult.SUCCESS);
                }
                catch (Exception errorEx)
                {
                    Console.Write(errorEx);
                    queryFetcher.DBLog("StackTrace" + mJsonSerializer.ToJsonString(errorEx.StackTrace));
                    queryFetcher.DBLog(errorEx.Message);
                    queryFetcher.GM_Render(Result_Define.eResult.SUCCESS);
                }
                finally
                {

                    tb.EndTransaction(queryFetcher.Render_errorFlag);
                    string gmid = "";
                    if (Request.Cookies.Count > 0)
                        gmid = HttpContext.Current.Request.Cookies["mseedadmin"]["userid"];
                    queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                    tb.Dispose();
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            GetDataList(1);
        }

    }
}