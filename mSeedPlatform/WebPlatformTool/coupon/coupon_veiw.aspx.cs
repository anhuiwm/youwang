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
using System.Text;
using System.Data;
using ServiceStack.Text;


using mSeed.Common;
using mSeed.Platform.Coupon;
using mSeed.Platform.Coupon.DBClass;

using WebPlatform.Tools;

namespace WebPlatformTool.coupon
{
    public partial class coupon_veiw : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            WebQueryParam queryFetcher = new WebQueryParam();
            //string retJson = "";
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    tb.IsoLevel = IsolationLevel.ReadCommitted;
                    ToolDataManager.DBconn(ref tb);

                    long groupID = queryFetcher.QueryParam_FetchLong("idx");
                    int pageNum = queryFetcher.QueryParam_FetchInt("pg", 1);
                    string searchText = queryFetcher.QueryParam_Fetch("search");
                    int activeCheck = queryFetcher.QueryParam_FetchInt(active.UniqueID);
                    idx.Value = groupID.ToString();
                    pg.Value = pageNum.ToString();
                    search.Value = searchText;
                    if (groupID > 0)
                    {
                        CouponGroup getData = ToolDataManager.GetCouponGroup(ref tb, groupID);
                        System_Coupon_Reward getReward = CouponManager.GetSystemCouponReward(ref tb, getData.rewardID);
                        SystemGameItem gameItem = CouponManager.GetGameItem(ref tb, getData.game_service_id);
                        if (!string.IsNullOrEmpty(getReward.item_info) && !string.IsNullOrEmpty(gameItem.reqitem_info))
                        {
                            List<string> tableCellList = gameItem.reqitem_info.Split(',').ToList();
                            List<RetGameItem> itemList = mJsonSerializer.JsonToObject<List<RetGameItem>>(mJsonSerializer.JsonToDictionary(gameItem.itme_info)["itemlist"]);
                            List<JsonObject> rewardList = mJsonSerializer.JsonToObject<List<JsonObject>>(getReward.item_info);

                            HtmlTableRow titleRow = new HtmlTableRow();

                            tableCellList.ForEach(item =>
                            {
                                HtmlTableCell titleCell = new HtmlTableCell();
                                titleCell.InnerText = item.Trim();
                                titleRow.Cells.Add(titleCell);
                            });
                            allreward.Rows.Add(titleRow);

                            rewardList.ForEach(item =>
                            {
                                
                                HtmlTableRow itemRow = new HtmlTableRow();
                                string temp_info = "";
                                for (int i = 0; i < tableCellList.Count; i++)
                                {
                                    HtmlTableCell cell = new HtmlTableCell();
                                    temp_info = mJsonSerializer.GetJsonValue(item.ToJson(), tableCellList[i].Trim());
                                    if (i == 0 && !string.IsNullOrEmpty(temp_info) && temp_info != "0")
                                    {
                                        temp_info = itemList.Find(findItem => findItem.item_id == temp_info).item_name;
                                    }
                                    cell.InnerText = temp_info;
                                    itemRow.Cells.Add(cell);
                                }
                                if (!string.IsNullOrEmpty(temp_info) && temp_info != "0")
                                    allreward.Rows.Add(itemRow);
                            });
                        }
                        couponType.Text = getData.strCoupon_Type;
                        title.Text = getData.Coupon_Title;
                        couponCode.Text = getData.coupon;
                        couponNum.Text = getData.Coupon_Num.ToString();
                        couponUse.Text = getData.useCount.ToString();

                        memo.Text = getData.Coupon_Memo;
                        reg_date.Text = getData.Reg_date.ToString();
                        reg_id.Text = getData.Reg_id;
                        string date = string.Format("{0} ~ {1}", getData.startDate.ToShortDateString(), getData.endDate.ToShortDateString());
                        coupon_active.Text = date;
                        discontinue_date.Text = getData.Coupon_Active == 0 ? getData.Discontinue_date.ToString() : "";
                        if (activeCheck == 1)
                            retError = CouponManager.SetCouponActive(ref tb, groupID);
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
                if (retError == Result_Define.eResult.SUCCESS)
                {
                    Response.Redirect(Request.RawUrl);
                }
            }
        }

        protected void excel_Click(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam();
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    tb.IsoLevel = IsolationLevel.ReadCommitted;
                    ToolDataManager.DBconn(ref tb);
                    long groupID = queryFetcher.QueryParam_FetchLong(idx.UniqueID);
                    int pageNum = queryFetcher.QueryParam_FetchInt(pg.UniqueID, 1);
                    idx.Value = groupID.ToString();
                    pg.Value = pageNum.ToString();
                    if (groupID > 0)
                    {
                        List<ExcelCoupon> list = ToolDataManager.GetCouponList_Excel(ref tb, groupID);

                        string lang = System.Web.HttpContext.Current.Request.UserLanguages[0];
                        string charSet = "utf-8";
                        lang = lang.Split(new char[] { ';' })[0];
                        if (lang == "zh-CN")
                            charSet = "GB2312";
                        else
                            charSet = "euc-kr";

                        string filename = string.Format("attachment;filename=coupon_log_{0}.csv", DateTime.Now);
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = charSet;
                        Response.ContentEncoding = System.Text.Encoding.GetEncoding(charSet);
                        Response.AddHeader("content-disposition", filename);
                        Response.ContentType = "text/csv";

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();

                        sb.Append("CouponCode,Game ID,NickName,Game Server,Reward,Reg Date");

                        foreach (ExcelCoupon item in list)
                        {
                            sb.Append("\r\n");
                            string stritem = string.Format(@"{0},{1},{2},{3},{4},{5}"
                                                            , item.coupon_Code, item.AID, item.userNickName, item.server_group_id, item.complete, item.reg_date);
                            sb.Append(stritem);
                        }
                        Response.Output.Write(sb.ToString());
                        Response.Flush();
                        Response.End();
                    }
                }
                catch (Exception errorEx)
                {
                    Console.Write(errorEx);
                }
                finally
                {
                    tb.EndTransaction();
                    tb.Dispose();
                }
            }
        }
    }
}