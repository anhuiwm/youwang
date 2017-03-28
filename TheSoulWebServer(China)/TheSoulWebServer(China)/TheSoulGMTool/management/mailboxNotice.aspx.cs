using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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
    public partial class mailboxNotice : System.Web.UI.Page
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
            long reqIdx = queryFetcher.QueryParam_FetchLong("idx");
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);

            Dictionary<long, TxnBlock> TxnBlackServer = new Dictionary<long, TxnBlock>();
            TxnBlock tb = new TxnBlock();
            try
            {
                GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                TxnBlackServer.Add(serverID, tb);
                tb.IsoLevel = IsolationLevel.ReadCommitted;

                string serverlist = GMDataManager.GetServerCheckList(ref tb, serverID);
                change_server.InnerHtml = serverlist;
                idx.Value = reqIdx.ToString();
                Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                if (!Page.IsPostBack)
                    pageInit(ref tb, reqIdx);
                else
                {
                    if (reqIdx > 0)
                    {
                        retError = GMDataManager.DeleteAdminMail(ref tb, reqIdx);
                        if (retError == Result_Define.eResult.SUCCESS)
                            retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, reqIdx, "", GMResult_Define.ControlType.MAIL_NOTICE_DELETE, queryFetcher.GetReqParams(), serverID.ToString());
                    }
                    else
                    {
                        string reqServer = queryFetcher.QueryParam_Fetch("serverid", "");
                        string Title = queryFetcher.QueryParam_Fetch(title.UniqueID, "none");
                        string Contents = queryFetcher.QueryParam_Fetch(contents.UniqueID, "");
                        string sdate = queryFetcher.QueryParam_Fetch(startDay.UniqueID, "");
                        string edate = queryFetcher.QueryParam_Fetch(endDay.UniqueID, "");
                        string shour = queryFetcher.QueryParam_Fetch(startHour.UniqueID, "");
                        string ehour = queryFetcher.QueryParam_Fetch(endHour.UniqueID, "");
                        string smin = queryFetcher.QueryParam_Fetch(startMin.UniqueID, "");
                        string emin = queryFetcher.QueryParam_Fetch(endMin.UniqueID, "");

                        long itemid1 = queryFetcher.QueryParam_FetchLong(reward1.UniqueID);
                        int itema1 = queryFetcher.QueryParam_FetchInt(rewardcnt1.UniqueID);
                        byte itemGrade1 = queryFetcher.QueryParam_FetchByte(grade1.UniqueID);
                        byte itemLevel1 = queryFetcher.QueryParam_FetchByte(lelvel1.UniqueID);
                        long itemid2 = queryFetcher.QueryParam_FetchLong(reward2.UniqueID);
                        int itema2 = queryFetcher.QueryParam_FetchInt(rewardcnt2.UniqueID);
                        byte itemGrade2 = queryFetcher.QueryParam_FetchByte(grade2.UniqueID);
                        byte itemLevel2 = queryFetcher.QueryParam_FetchByte(lelvel2.UniqueID);
                        long itemid3 = queryFetcher.QueryParam_FetchLong(reward3.UniqueID);
                        int itema3 = queryFetcher.QueryParam_FetchInt(rewardcnt3.UniqueID);
                        byte itemGrade3 = queryFetcher.QueryParam_FetchByte(grade3.UniqueID);
                        byte itemLevel3 = queryFetcher.QueryParam_FetchByte(lelvel3.UniqueID);
                        long itemid4 = queryFetcher.QueryParam_FetchLong(reward4.UniqueID);
                        int itema4 = queryFetcher.QueryParam_FetchInt(rewardcnt4.UniqueID);
                        byte itemGrade4 = queryFetcher.QueryParam_FetchByte(grade4.UniqueID);
                        byte itemLevel4 = queryFetcher.QueryParam_FetchByte(lelvel4.UniqueID);
                        long itemid5 = queryFetcher.QueryParam_FetchLong(reward5.UniqueID);
                        int itema5 = queryFetcher.QueryParam_FetchInt(rewardcnt5.UniqueID);
                        byte itemGrade5 = queryFetcher.QueryParam_FetchByte(grade5.UniqueID);
                        byte itemLevel5 = queryFetcher.QueryParam_FetchByte(lelvel5.UniqueID);
                        byte MailType = queryFetcher.QueryParam_FetchByte(mailType.UniqueID);

                        string startDate = string.Format("{0} {1}:{2}:00", sdate, shour, smin);
                        string endDate = string.Format("{0} {1}:{2}:59", edate, ehour, emin);

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

                        Admin_System_MailNotice setItem = new Admin_System_MailNotice();
                        List<Admin_System_MailNotice_Reward> reward = new List<Admin_System_MailNotice_Reward>();
                        int itemIndexNum = 1;
                        setItem.title = Title.Replace("'", "''");
                        setItem.message = Contents.Replace("'", "''");
                        setItem.MailType = MailType;
                        setItem.senderName = "";
                        setItem.startDate = System.Convert.ToDateTime(startDate);
                        setItem.endDate = System.Convert.ToDateTime(endDate);
                        if (itemid1 > 0 && itemGrade1 >= 0 & itema1 > 0)
                        {
                            Admin_System_MailNotice_Reward item = new Admin_System_MailNotice_Reward();
                            item.Item_ID = itemid1;
                            item.Item_Grade = itemGrade1;
                            item.Item_Level = itemLevel1;
                            item.Item_Num = itema1;
                            item.ItemIndex = itemIndexNum;
                            reward.Add(item);
                            itemIndexNum += 1;
                        }
                        if (itemid2 > 0 && itemGrade2 >= 0 & itema2 > 0)
                        {
                            Admin_System_MailNotice_Reward item = new Admin_System_MailNotice_Reward();
                            item.Item_ID = itemid2;
                            item.Item_Grade = itemGrade2;
                            item.Item_Level = itemLevel2;
                            item.Item_Num = itema2;
                            item.ItemIndex = itemIndexNum;
                            reward.Add(item);
                            itemIndexNum += 1;
                        }
                        if (itemid3 > 0 && itemGrade3 >= 0 & itema3 > 0)
                        {
                            Admin_System_MailNotice_Reward item = new Admin_System_MailNotice_Reward();
                            item.Item_ID = itemid3;
                            item.Item_Grade = itemGrade3;
                            item.Item_Level = itemLevel3;
                            item.Item_Num = itema3;
                            item.ItemIndex = itemIndexNum;
                            reward.Add(item);
                            itemIndexNum += 1;
                        }
                        if (itemid4 > 0 && itemGrade4 >= 0 & itema4 > 0)
                        {
                            Admin_System_MailNotice_Reward item = new Admin_System_MailNotice_Reward();
                            item.Item_ID = itemid4;
                            item.Item_Grade = itemGrade4;
                            item.Item_Level = itemLevel4;
                            item.Item_Num = itema4;
                            item.ItemIndex = itemIndexNum;
                            reward.Add(item);
                            itemIndexNum += 1;
                        }
                        if (itemid5 > 0 && itemGrade5 >= 0 & itema5 > 0)
                        {
                            Admin_System_MailNotice_Reward item = new Admin_System_MailNotice_Reward();
                            item.Item_ID = itemid5;
                            item.Item_Grade = itemGrade5;
                            item.Item_Level = itemLevel5;
                            item.Item_Num = itema5;
                            item.ItemIndex = itemIndexNum;
                            reward.Add(item);
                            itemIndexNum += 1;
                        }

                        retError = GMDataManager.InsertAdminMail(ref TxnBlackServer, startDate, endDate, setItem, reward);
                        if (retError == Result_Define.eResult.SUCCESS)
                            retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, reqIdx, "", GMResult_Define.ControlType.MAIL_NOTICE_ADD, queryFetcher.GetReqParams(), serverID.ToString());
                    }
                    if (retError == Result_Define.eResult.SUCCESS)
                        result = true;
                    retJson = queryFetcher.GM_Render("", retError);
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
                Response.Redirect("/management/milboxNoticeList.aspx?ca2=" + queryFetcher.QueryParam_Fetch_Request("ca2", "1") + "&select_server=" + serverID);
        }

        protected void pageInit(ref TxnBlock TB, long index)
        {
            List<ListItem> hourList = GMDataManager.GetHourList();
            List<ListItem> minList = GMDataManager.GetMinList(1);
            List<Admin_System_Item> allList = GMDataManager.GetNonEquip_Accessory_ItemList(ref TB, GMData_Define.ShardingDBName);
            startHour.DataSource = hourList;
            startHour.DataTextField = "Text";
            startHour.DataValueField = "Value";
            startHour.DataBind();

            endHour.DataSource = hourList;
            endHour.DataTextField = "Text";
            endHour.DataValueField = "Value";
            endHour.DataBind();

            startMin.DataSource = minList;
            startMin.DataTextField = "Text";
            startMin.DataValueField = "Value";
            startMin.DataBind();

            endMin.DataSource = minList;
            endMin.DataTextField = "Text";
            endMin.DataValueField = "Value";
            endMin.DataBind();

            ListItem selectItem = new ListItem("select", "-1");
            reward1.DataSource = allList; reward1.DataTextField = "Description"; reward1.DataValueField = "Item_IndexID"; reward1.DataBind();
            reward1.Items.Insert(0, selectItem);
            reward2.DataSource = allList; reward2.DataTextField = "Description"; reward2.DataValueField = "Item_IndexID"; reward2.DataBind();
            reward2.Items.Insert(0, selectItem);
            reward3.DataSource = allList; reward3.DataTextField = "Description"; reward3.DataValueField = "Item_IndexID"; reward3.DataBind();
            reward3.Items.Insert(0, selectItem);
            reward4.DataSource = allList; reward4.DataTextField = "Description"; reward4.DataValueField = "Item_IndexID"; reward4.DataBind();
            reward4.Items.Insert(0, selectItem);
            reward5.DataSource = allList; reward5.DataTextField = "Description"; reward5.DataValueField = "Item_IndexID"; reward5.DataBind();
            reward5.Items.Insert(0, selectItem);

            if (index > 0)
            {
                Admin_System_MailNotice dataInfo = GMDataManager.GetAminMailInfo(ref TB, index);
                List<Admin_System_MailNotice_Reward> rewardList = MailManager.GetAdmin_NoticeMailReward(ref TB, index);
                startDay.Text = dataInfo.startDate.ToString("yyyy-MM-dd");
                startHour.SelectedValue = dataInfo.startDate.ToString("HH");
                startMin.SelectedValue = dataInfo.startDate.ToString("mm");
                endDay.Text = dataInfo.endDate.ToString("yyyy-MM-dd");
                endHour.SelectedValue = dataInfo.endDate.ToString("HH");
                endMin.SelectedValue = dataInfo.endDate.ToString("mm");
                title.Text = dataInfo.title;
                contents.Text = dataInfo.message;
                mailType.SelectedValue = dataInfo.MailType.ToString();
                if (rewardList.Count > 0)
                {
                    foreach (Admin_System_MailNotice_Reward item in rewardList)
                    {
                        if (item.ItemIndex == 1)
                        {
                            reward1.SelectedValue = item.Item_ID.ToString();
                            lelvel1.Text = item.Item_Level.ToString();
                            grade1.Text = item.Item_Grade.ToString();
                            rewardcnt1.Text = item.Item_Num.ToString();
                        }
                        else if (item.ItemIndex == 2)
                        {
                            reward2.SelectedValue = item.Item_ID.ToString();
                            lelvel2.Text = item.Item_Level.ToString();
                            grade2.Text = item.Item_Grade.ToString();
                            rewardcnt2.Text = item.Item_Num.ToString();
                        }
                        else if (item.ItemIndex == 3)
                        {
                            reward3.SelectedValue = item.Item_ID.ToString();
                            lelvel3.Text = item.Item_Level.ToString();
                            grade3.Text = item.Item_Grade.ToString();
                            rewardcnt3.Text = item.Item_Num.ToString();
                        }
                        else if (item.ItemIndex == 4)
                        {
                            reward4.SelectedValue = item.Item_ID.ToString();
                            lelvel4.Text = item.Item_Level.ToString();
                            grade4.Text = item.Item_Grade.ToString();
                            rewardcnt4.Text = item.Item_Num.ToString();
                        }
                        else if (item.ItemIndex == 5)
                        {
                            reward5.SelectedValue = item.Item_ID.ToString();
                            lelvel5.Text = item.Item_Level.ToString();
                            grade5.Text = item.Item_Grade.ToString();
                            rewardcnt5.Text = item.Item_Num.ToString();
                        }
                    }
                }
            }
        }
    }
}