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

namespace TheSoulGMTool.kr
{
    public partial class mailLog : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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
                    long idx = queryFetcher.QueryParam_FetchLong(mailidx.UniqueID);
                    string startDate = queryFetcher.QueryParam_Fetch(sDate.UniqueID, "").Replace("/","-");
                    string endDate = queryFetcher.QueryParam_Fetch(eDate.UniqueID, "").Replace("/", "-");
                    username.Text = Username;
                    uid.Text = Uid;
                    sDate.Text = startDate.ToString();
                    eDate.Text = endDate.ToString();                    
                    Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
                    
                    if (!string.IsNullOrEmpty(Username) || !string.IsNullOrEmpty(Uid))
                    {

                        long AID = 0;
                        if (!string.IsNullOrEmpty(Username))
                            AID = GMDataManager.GetSearchAID_BYUserName(ref tb, Username);
                        if (!string.IsNullOrEmpty(Uid))
                            AID = GMDataManager.GetSearchAID_BYSnailPlatformID(ref tb, Uid).AID;
                        if (idx > 0 && AID>0)
                        {
                            //.delflag == "Y"
                            //// 邮件删除还原的功能，我现在改成了判断.mailseq >=1 ,即为，有没有找到玩家身上的邮件
                            retError = GMDataManager.SetUserMail(ref tb, AID, idx, MailManager.GetUser_Mail_Detail(ref tb, AID, idx).mailseq >= 1 ? true : false);
                            mailidx.Value = "";
                        }
                        long totalCount = GMDataManager.GetUserMailCount(ref tb, AID, startDate, endDate);

                        List<GMUserMail> logList = GMDataManager.GetUserMailList(ref tb, AID, pageIndex, startDate, endDate);
                        logList.ForEach(item =>
                        {
                            string rewardItem = "";
                            if(item.item_id_1 > 0){
                                System_Item_Base itemInfo = ItemManager.GetSystem_Item_Base(ref tb, item.item_id_1);
                                string itemName = string.Format("{0} {1}", GMDataManager.GetItmeName(ref tb, itemInfo.Name), item.itemea_1);
                                rewardItem += string.IsNullOrEmpty(rewardItem) ? itemName : "<br />" + itemName;
                            }
                            if (item.item_id_2 > 0)
                            {
                                System_Item_Base itemInfo = ItemManager.GetSystem_Item_Base(ref tb, item.item_id_2);
                                string itemName = string.Format("{0} {1}", GMDataManager.GetItmeName(ref tb, itemInfo.Name), item.itemea_2);
                                rewardItem += string.IsNullOrEmpty(rewardItem) ? itemName : "<br />" + itemName;
                            }
                            if (item.item_id_2 > 0)
                            {
                                System_Item_Base itemInfo = ItemManager.GetSystem_Item_Base(ref tb, item.item_id_3);
                                string itemName = string.Format("{0} {1}", GMDataManager.GetItmeName(ref tb, itemInfo.Name), item.itemea_3);
                                rewardItem += string.IsNullOrEmpty(rewardItem) ? itemName : "<br />" + itemName;
                            }
                            if (item.item_id_4 > 0)
                            {
                                System_Item_Base itemInfo = ItemManager.GetSystem_Item_Base(ref tb, item.item_id_4);
                                string itemName = string.Format("{0} {1}", GMDataManager.GetItmeName(ref tb, itemInfo.Name), item.itemea_4);
                                rewardItem += string.IsNullOrEmpty(rewardItem) ? itemName : "<br />" + itemName;
                            }
                            if (item.item_id_5 > 0)
                            {
                                System_Item_Base itemInfo = ItemManager.GetSystem_Item_Base(ref tb, item.item_id_5);
                                string itemName = string.Format("{0} {1}", GMDataManager.GetItmeName(ref tb, itemInfo.Name), item.itemea_5);
                                rewardItem += string.IsNullOrEmpty(rewardItem) ? itemName : "<br />" + itemName;
                            }
                            item.bodytext = rewardItem;
                        });
                        dataList.DataSource = logList;
                        dataList.DataBind();

                        GMDataManager.PopulatePager(ref dlPager, totalCount, pageIndex);
                        queryFetcher.GM_Render(retError);
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            GetDataList(1);
        }
    }
}