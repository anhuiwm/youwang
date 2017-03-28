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
    public partial class sendReward : System.Web.UI.Page
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
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    if (Page.IsPostBack)
                    {
                        GMDataManager.GetServerinit(ref tb, serverID);
                        string sdate = queryFetcher.QueryParam_Fetch(start.UniqueID, "");
                        string edate = queryFetcher.QueryParam_Fetch(end.UniqueID, "");
                        List<Request_Log> list = GetCouponRewardList(ref tb, "coupon_send_mail", sdate, edate);
                        Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
                        foreach (Request_Log getdata in list)
                        {

                            string coupon = mJsonSerializer.GetJsonValue(getdata.RequestParams, "coupon");
                            long receiverAID = System.Convert.ToInt64(mJsonSerializer.GetJsonValue(getdata.RequestParams, "receiver"));
                            string itemList = mJsonSerializer.GetJsonValue(getdata.RequestParams, "coupon_reward_list");
                            if (receiverAID > 0)
                            {
                                List<Set_Mail_Item> rewardList = new List<Set_Mail_Item>();
                                if (!string.IsNullOrEmpty(itemList))
                                    rewardList = mJsonSerializer.JsonToObject<List<Set_Mail_Item>>(itemList);
                                int sendItemCount = 0;
                                Dictionary<short, List<Set_Mail_Item>> sendMailList = new Dictionary<short, List<Set_Mail_Item>>();

                                foreach (Set_Mail_Item setItem in rewardList)
                                {
                                    Item_Define.eSystemItemType checkType = Item_Define.eSystemItemType.ItemClass_NONE;
                                    Object SysItem = ItemManager.CheckItemType(ref tb, setItem.itemid, ref checkType);

                                    if (checkType == Item_Define.eSystemItemType.ItemClass_Equip)
                                    {
                                        System_Item_Equip itemInfo = ItemManager.GetSystem_Item_Equip(ref tb, setItem.itemid);
                                        short setItemClass = (short)(itemInfo.Class_IndexID == 0 ? 0 : Character_Define.ClassTypeToEnum[itemInfo.EquipClass]);

                                        if (!sendMailList.ContainsKey(setItemClass))
                                            sendMailList[setItemClass] = new List<Set_Mail_Item>();

                                        sendMailList[setItemClass].Add(setItem);
                                    }
                                    else
                                    {
                                        if (!sendMailList.ContainsKey((short)Character_Define.SystemClassType.Class_None))
                                            sendMailList[(short)Character_Define.SystemClassType.Class_None] = new List<Set_Mail_Item>();
                                        sendMailList[(short)Character_Define.SystemClassType.Class_None].Add(setItem);
                                    }
                                }

                                foreach (KeyValuePair<short, List<Set_Mail_Item>> mailList in sendMailList)
                                {
                                    List<Set_Mail_Item> setMailItem = new List<Set_Mail_Item>();

                                    foreach (Set_Mail_Item setItem in mailList.Value)
                                    {
                                        if (coupon.ToLower().Equals("dbzcafe0819"))
                                            setItem.itemea = setItem.itemid == 303000002 ? 80 : setItem.itemea;
                                        else
                                        {
                                            if (setItem.itemid == 303000005)
                                                setItem.itemea = 500;
                                            else if (setItem.itemid == 303000001)
                                                setItem.itemea = 200000;
                                        }

                                        if (setItem.itemid > 0 && setItem.itemea > 0)
                                        {
                                            sendItemCount++;
                                            setMailItem.Add(setItem);
                                        }
                                        if (sendItemCount >= Mail_Define.Mail_MaxItem)
                                        {
                                            retError = MailManager.SendMail(ref tb, ref setMailItem, receiverAID, 0, "", Mail_Define.Coupon_Mail_Title, Mail_Define.Coupon_Mail_Body, Mail_Define.Mail_VIP_CloseTime_Min);
                                            if (retError == Result_Define.eResult.SUCCESS)
                                            {
                                                setMailItem.Clear();
                                                sendItemCount = 0;
                                            }
                                            else
                                                break;
                                        }
                                    }

                                    if (retError == Result_Define.eResult.SUCCESS && sendItemCount > 0)
                                    {
                                        retError = MailManager.SendMail(ref tb, ref setMailItem, receiverAID, 0, "", Mail_Define.Coupon_Mail_Title, Mail_Define.Coupon_Mail_Body, Mail_Define.Mail_VIP_CloseTime_Min);
                                    }
                                    else
                                        break;
                                }
                            }
                        }
                        Response.Write(retError.ToString());
                        retJson = queryFetcher.GM_Render(retError);
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

        private List<Request_Log> GetCouponRewardList(ref TxnBlock TB, string op, string sdate, string edate, string dbkey = GMData_Define.LogDBName)
        {
            string condition1 = "";
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                condition1 = string.Format("AND (CONVERT(varchar(16), regdate,120) > '{0}' and CONVERT(varchar(16), regdate,120) <= '{1}')", sdate, edate);
            
            string tableQuery = "";
            List<GM_String> getTableList = GMDataManager.GetLogTableName(ref TB, DataManager_Define.LogTableName, sdate.Substring(0, 10), edate.Substring(0, 10));
            getTableList.ForEach(item =>
            {
                if (!string.IsNullOrEmpty(tableQuery))
                    tableQuery += "Union All";
                tableQuery += string.Format(@"
                                            Select * From {0} WITH(NOLOCK) Where Operation=N'{1}' {2} "
                                            , item.name, op, condition1);
            });


            List<Request_Log> retObj = GenericFetch.FetchFromDB_MultipleRow<Request_Log>(ref TB, tableQuery, dbkey);
            if (retObj.Count == 0)
                retObj = new List<Request_Log>();
            return retObj;
        }
    }
}