using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using ServiceStack.Text;

using mSeed.Common;
using mSeed.Platform;
using mSeed.Platform.Coupon;
using mSeed.Platform.Coupon.DBClass;

using WebPlatform.Tools;

namespace WebPlatformTool.coupon
{
    public partial class coupon_make : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            WebQueryParam queryFetcher = new WebQueryParam();
            //string retJson = "";
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    tb.IsoLevel = IsolationLevel.ReadCommitted;
                    ToolDataManager.DBconn(ref tb);
                    
                    Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                    if (!Page.IsPostBack)
                        pageInit(ref tb);                        
                    else
                    {

                        long game_service_id = queryFetcher.QueryParam_FetchLong(game_id.UniqueID, 1);
                        SystemGameItem getData = CouponManager.GetGameItem(ref tb, game_service_id);

                        string sdate = queryFetcher.QueryParam_Fetch(startDay.UniqueID, DateTime.Today.ToString("yyyy-MM-dd"));
                        string edate = queryFetcher.QueryParam_Fetch(endDay.UniqueID, DateTime.Today.AddDays(1).ToString("yyyy-MM-dd"));
                        string strMemo = queryFetcher.QueryParam_Fetch(memo.UniqueID, "");
                        string strTitle = queryFetcher.QueryParam_Fetch(eventName.UniqueID, "defult");
                        byte coupon_Type = queryFetcher.QueryParam_FetchByte(couponType.UniqueID, 1);
                        int date_Type = queryFetcher.QueryParam_FetchInt(checkDate.UniqueID, 1);
                        string coupon = queryFetcher.QueryParam_Fetch(couponCode.UniqueID);
                        int couponNum = queryFetcher.QueryParam_FetchInt(couponCount.UniqueID);
                        
                        string itemJson = "";
                        List<string> item_param = new List<string>();
                        string firstKey = "";
                        if (!string.IsNullOrEmpty(getData.reqitem_info))
                        {
                            List<string> paramList = getData.reqitem_info.Split(',').ToList();
                            firstKey = paramList.Count > 0?paramList[0] :"";
                            paramList.ForEach(item =>
                            {
                                string tem_param = queryFetcher.QueryParam_Fetch(string.Format("{0}${1}", Master.FindControl("ContentPlaceHolder1").UniqueID,item.Trim()));
                                List<string> tem_paramValue = System.Text.RegularExpressions.Regex.Split(tem_param, ",").ToList();
                                for (int i = 0; i < tem_paramValue.Count; i++)
                                {

                                    if (string.IsNullOrEmpty(tem_paramValue[i]))
                                        tem_paramValue[i] = "0";
                                    string tem_json = mJsonSerializer.AddJson("", item.Trim(), tem_paramValue[i]);
                                    if (paramList.IndexOf(item) == 0)
                                        item_param.Add(tem_json);
                                    else
                                        item_param[i] = mJsonSerializer.MergeJson(item_param[i], tem_json);                                    

                                }
                            });
                        }
                        item_param.ForEach(item =>
                        {
                            if(mJsonSerializer.GetJsonValue(item, firstKey) != "0")
                                itemJson = mJsonSerializer.AddJsonArray(itemJson, item);
                        });

                        if (item_param.Count > 0)
                        {
                            System_Coupon_Group groupInfo = new System_Coupon_Group();
                            System_Coupon couponInfo = new System_Coupon();

                            groupInfo.Coupon_Active = 1;
                            groupInfo.game_service_id = game_service_id;
                            groupInfo.Coupon_Memo = strMemo;
                            groupInfo.Coupon_Title = strTitle;
                            groupInfo.Coupon_Type = coupon_Type; //1=> 1:1, 2 1:n
                            groupInfo.Coupon_Num = couponNum;
                            groupInfo.Reg_id = "tset2";

                            couponInfo.Coupon_Startdate = System.Convert.ToDateTime(string.Format("{0} 00:00:00", date_Type == 1 ? DateTime.Today.ToShortDateString() : sdate));
                            couponInfo.Coupon_Enddate = System.Convert.ToDateTime(string.Format("{0} 23:59:59", date_Type == 1 ? DateTime.Today.AddYears(100).ToShortDateString() : edate));
                            couponInfo.game_service_id = game_service_id;
                            couponInfo.Coupon_Type = coupon_Type;
                            if (coupon_Type == 2)
                                couponInfo.Coupon_Code = coupon;
                            retError = CouponManager.InsertCoupon(ref tb, groupInfo, couponInfo, itemJson);

                        }
                    }
                    queryFetcher.NoRenderWirte(retError);
                }
                catch (Exception errorEx)
                {
                    queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(errorEx.Message), Result_Define.eResult.System_Exception);
                }
                finally
                {
                    tb.EndTransaction(queryFetcher.Render_errorFlag);
                    tb.Dispose();
                }
                if (queryFetcher.Render_errorFlag)
                    Response.Redirect("/coupon/coupon_list.aspx");

            }

        }

        protected void pageInit(ref TxnBlock TB)
        {
            List<game_service> gameList = GameServiceManager.GetGameService(ref TB);
            game_id.DataSource = gameList;
            game_id.DataTextField = "service_name";
            game_id.DataValueField = "game_service_id";
            game_id.DataBind();

            SystemGameItem getData = CouponManager.GetGameItem(ref TB, 1);
            if (!string.IsNullOrEmpty(getData.itme_info))
            {
                List<string> tableCellList = getData.reqitem_info.Split(',').ToList();
                List<RetGameItem> itemList = mJsonSerializer.JsonToObject<List<RetGameItem>>(mJsonSerializer.JsonToDictionary(getData.itme_info)["itemlist"]);
                DropDownList selectItem = new DropDownList();
                selectItem.DataSource = itemList;
                selectItem.DataTextField = "item_name";
                selectItem.DataValueField = "item_id";
                selectItem.DataBind();
                selectItem.Items.Insert(0,new ListItem("Select", "0"));


                HtmlTableRow titleRow = new HtmlTableRow();
                HtmlTableRow itemRow = new HtmlTableRow();
                tableCellList.ForEach(item => {
                    HtmlTableCell titleCell = new HtmlTableCell();
                    HtmlTableCell itemCell = new HtmlTableCell();
                    titleCell.Width = string.Format("{0}%", 100 / tableCellList.Count);
                    if (tableCellList.IndexOf(item) == 0)
                    {
                        selectItem.ID = item.Trim();
                        selectItem.Width = 150;
                        itemCell.Controls.Add(selectItem);
                    }
                    else
                    {
                        HtmlInputText itembox = new HtmlInputText();
                        itembox.ID = item.Trim();
                        itembox.Name = item.Trim();
                        itembox.Size = 100;
                        itemCell.Controls.Add(itembox);
                    }
                    titleCell.InnerText = item.Trim();
                    titleRow.Cells.Add(titleCell);
                    itemRow.Cells.Add(itemCell);
                });
                HtmlTableCell delete_button = new HtmlTableCell();
                delete_button.InnerHtml = "<button type=\"button\" class=\"btn\" onclick=\"deleteTr(this, 'allreward');\">Del</button>";
                titleRow.Cells.Add(new HtmlTableCell());
                itemRow.Cells.Add(delete_button);
                allreward.Rows.Add(titleRow);
                allreward.Rows.Add(itemRow);
            }
            
        }
    }
}