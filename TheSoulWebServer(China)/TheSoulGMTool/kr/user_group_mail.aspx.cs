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

namespace TheSoulGMTool.kr
{
    public partial class user_group_mail : System.Web.UI.Page
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
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
            List<string> failUserList = new List<string>();
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                    tb.IsoLevel = IsolationLevel.ReadCommitted;

                    if (Page.IsPostBack)
                    {
                        retError = Result_Define.eResult.SUCCESS;
                        string reqTtitle = queryFetcher.QueryParam_Fetch(title.UniqueID, "default");
                        string reqMsg = queryFetcher.QueryParam_Fetch(contents.UniqueID, "default");
                        string reqUser = queryFetcher.QueryParam_Fetch(username.UniqueID);
                        string itemID = queryFetcher.QueryParam_Fetch("itemid", "");
                        string itemCount = queryFetcher.QueryParam_Fetch("itema", "");
                        string itemLevel = queryFetcher.QueryParam_Fetch("level", "");
                        string itemGrade = queryFetcher.QueryParam_Fetch("grade", "");
                        List<string> userList = string.IsNullOrEmpty(reqUser) ? new List<string>() : mJsonSerializer.JsonToObject<List<string>>(reqUser.Replace("\r\n", ","));
                        List<long> itemIDList = string.IsNullOrEmpty(itemID) ? new List<long>() : mJsonSerializer.JsonToObject<List<long>>(itemID);
                        List<int> itemaList = string.IsNullOrEmpty(itemCount) ? new List<int>() : mJsonSerializer.JsonToObject<List<int>>(itemCount);
                        List<short> levelList = string.IsNullOrEmpty(itemLevel) ? new List<short>() : mJsonSerializer.JsonToObject<List<short>>(itemLevel);
                        List<short> gradeList = string.IsNullOrEmpty(itemGrade) ? new List<short>() : mJsonSerializer.JsonToObject<List<short>>(itemGrade);

                        reqTtitle = reqTtitle.Replace("'", "''");
                        reqMsg = reqMsg.Replace("'", "''");

                        if (userList.Count > 0 && itemIDList.Count > 0)
                        {
                            List<Set_Mail_Item> setMailItem = new List<Set_Mail_Item>();
                            foreach (long item in itemIDList)
                            {
                                int getIndex = itemIDList.IndexOf(item);
                                if (itemaList[getIndex] > 0)
                                    setMailItem.Add(new Set_Mail_Item(item, itemaList[getIndex], gradeList[getIndex], levelList[getIndex]));
                            }

                            foreach (string user in userList)
                            {
                                long AID = GMDataManager.GetSearchAID_BYUserName(ref tb, user);
                                if (AID > 0 && retError == Result_Define.eResult.SUCCESS)
                                {
                                    retError = MailManager.SendMail(ref tb, ref setMailItem, AID, GMData_Define.AdminSender, GMData_Define.AdminSenerName, reqTtitle, reqMsg, 0);
                                }
                                else
                                    failUserList.Add(user);
                            }
                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_USER, 0, "", GMResult_Define.ControlType.USER_ITEM_ADD, queryFetcher.GetReqParams(), serverID.ToString());

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = GMDataManager.InsertItemChargeLog(ref tb, reqUser, reqTtitle, reqMsg, setMailItem);

                            if (retError == Result_Define.eResult.SUCCESS)
                                retJson = queryFetcher.GM_Render(retError);
                        }
                        else
                        {
                            failUserList = userList;
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
                    string msg = "";
                    if (failUserList.Count == 0)
                        msg = "alert('Success');";//"alert('Success');";
                    else
                    {
                        msg = "mail_fail();";//"Fail List\n";
                        fail_list.DataSource = failUserList;
                        fail_list.DataBind();
                    }
                    Page.ClientScript.RegisterStartupScript(GetType(), "alert", msg, true);
                }
            }
        }

        protected void btn_Click(object sender, EventArgs e)
        {
        }
    }
}