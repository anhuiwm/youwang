using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using mSeed.mDBTxnBlock;
using mSeed.RedisManager;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Global;
using TheSoulWebServer.Tools;
using ServiceStack.Text;

namespace TheSoulWebServer
{
    public partial class RequestMail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] ops = new string[] {
                "mail_list",
                "mail_detail",
                "mail_open",

                // for Debug test
                "mail_send",
            };


            WebQueryParam queryFetcher = new WebQueryParam();
            string retJson = "";

            TxnBlock tb = new TxnBlock();
            {
                long AID = 0;
                try
                {
                    queryFetcher.TxnBlockInit(ref tb, ref AID);

                    string requestOp = queryFetcher.QueryParam_Fetch("op");
                    JsonObject json = new JsonObject();

                    if (queryFetcher.ReRequestFlag)
                    {
                        retJson = queryFetcher.ReRequestRender();
                    }
                    else if (Array.IndexOf(ops, requestOp) >= 0)
                    {
                        queryFetcher.operation = requestOp;
                        Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.op], requestOp);
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], AID);

                        if (requestOp.Equals("mail_list"))
                        {
                            retError = Result_Define.eResult.SUCCESS;

                            User_Admin_Mail_Check userAdminCheck = MailManager.GetUser_Admin_Mail_Check(ref tb, AID);
                            List<Admin_System_MailNotice> adminMailList = MailManager.GetAdmin_NoticeMailList(ref tb, userAdminCheck.last_checked_main_idx);
                            long lastcheckseq = userAdminCheck.last_checked_main_idx;

                            foreach (Admin_System_MailNotice sendmail in adminMailList)
                            {
                                userAdminCheck.last_checked_main_idx = sendmail.idx > userAdminCheck.last_checked_main_idx ? sendmail.idx : userAdminCheck.last_checked_main_idx;

                                List<Set_Mail_Item> setMailItem = sendmail.MailType == (int)Mail_Define.eMailNoticeType.MAILNOTICE ?
                                                                    new List<Set_Mail_Item>() :
                                                                    MailManager.GetAdminSendMailItemList(ref tb, sendmail.idx);

                                TimeSpan TS = sendmail.endDate - DateTime.Now;
                                MailManager.SendMail(ref tb, ref setMailItem, AID, 0, sendmail.senderName, sendmail.title, sendmail.message, (int)TS.TotalMinutes);
                            }

                            if (lastcheckseq != userAdminCheck.last_checked_main_idx)
                                retError = MailManager.SetUser_Admin_Mail_Check(ref tb, userAdminCheck);

                            List<User_MailBox> simpleMailList = new List<User_MailBox>(MailManager.GetUser_Mail_List(ref tb, AID).OrderByDescending(item => item.mailseq));

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                json = mJsonSerializer.AddJson(json, "mail_list", mJsonSerializer.ToJsonString(simpleMailList));
                            }
                        }
                        else if (requestOp.Equals("mail_detail"))
                        {
                            retError = Result_Define.eResult.SUCCESS;

                            long mailSeq = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("mailseq"));

                            retError = MailManager.Update_MailReadFlag(ref tb, AID, mailSeq);

                            User_Mail_Datail mailInfo = null;
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                mailInfo = MailManager.GetUser_Mail_Detail(ref tb, AID, mailSeq, true);

                                if (mailInfo.mailseq < 1)
                                    retError = Result_Define.eResult.NOEXIST_MAILINFO;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                json = mJsonSerializer.AddJson(json, "mail_detail", mJsonSerializer.ToJsonString(mailInfo));
                            }
                        }
                        else if (requestOp.Equals("mail_open"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long CID = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("cid"));
                            long mailSeq = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("mailseq"));

                            User_Mail_Datail mailInfo = MailManager.GetUser_Mail_Detail(ref tb, AID, mailSeq);

                            if (mailInfo.mailseq < 1)
                                retError = Result_Define.eResult.NOEXIST_MAILINFO;
                            else
                                retError = MailManager.Update_MailOpenFlag(ref tb, AID, mailSeq);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                if (VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.BAGSLOT_MAX_ITEM))
                                    retError = Result_Define.eResult.SUCCESS;
                                else
                                    retError = Result_Define.eResult.ITEM_INVENTORY_OVER;
                            }

                            List<User_Inven> makeRealItem = new List<User_Inven>();
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<User_Inven> makeItemList = new List<User_Inven>();
                                if (mailInfo.item_id_1 > 0 && mailInfo.itemea_1 > 0)
                                    makeItemList.Add(new User_Inven(mailInfo.item_id_1, mailInfo.itemea_1, mailInfo.item_grade_1, mailInfo.item_level_1));
                                if (mailInfo.item_id_2 > 0 && mailInfo.itemea_2 > 0)
                                    makeItemList.Add(new User_Inven(mailInfo.item_id_2, mailInfo.itemea_2, mailInfo.item_grade_2, mailInfo.item_level_2));
                                if (mailInfo.item_id_3 > 0 && mailInfo.itemea_3 > 0)
                                    makeItemList.Add(new User_Inven(mailInfo.item_id_3, mailInfo.itemea_3, mailInfo.item_grade_3, mailInfo.item_level_3));
                                if (mailInfo.item_id_4 > 0 && mailInfo.itemea_4 > 0)
                                    makeItemList.Add(new User_Inven(mailInfo.item_id_4, mailInfo.itemea_4, mailInfo.item_grade_4, mailInfo.item_level_4));
                                if (mailInfo.item_id_5 > 0 && mailInfo.itemea_5 > 0)
                                    makeItemList.Add(new User_Inven(mailInfo.item_id_5, mailInfo.itemea_5, mailInfo.item_grade_5, mailInfo.item_level_5));

                                foreach (User_Inven makeInfo in makeItemList)
                                {
                                    List<User_Inven> makeItem = new List<User_Inven>();
                                    retError = ItemManager.MakeItem(ref tb, ref makeItem, AID, makeInfo.itemid, makeInfo.itemea, CID, makeInfo.level, makeInfo.grade);

                                    if (retError != Result_Define.eResult.SUCCESS)
                                        break;
                                    makeItem.ForEach(item => item.itemea = makeInfo.itemea);
                                    makeRealItem.AddRange(makeItem);
                                }
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                MailManager.RemoveMailCache(AID);

                                Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userAccount);

                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));
                                json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeRealItem));
                            }
                        }
#if DEBUG
                        else if (Request.Params.AllKeys.Contains("Debug"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            if (requestOp.Equals("mail_send"))
                            {
                                long MakeItemID_1 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("mail_item_id_1", "0"));
                                int MakeItemEA_1 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("mail_item_ea_1", "0"));
                                int MakeItemGrade_1 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("mail_item_grade_1", "1"));
                                int MakeItemLevel_1 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("mail_item_level_1", "0"));
                                long MakeItemID_2 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("mail_item_id_2", "0"));
                                int MakeItemEA_2 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("mail_item_ea_2", "0"));
                                int MakeItemGrade_2 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("mail_item_grade_2", "1"));
                                int MakeItemLevel_2 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("mail_item_level_2", "0"));
                                long MakeItemID_3 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("mail_item_id_3", "0"));
                                int MakeItemEA_3 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("mail_item_ea_3", "0"));
                                int MakeItemGrade_3 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("mail_item_grade_3", "1"));
                                int MakeItemLevel_3 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("mail_item_level_3", "0"));
                                long MakeItemID_4 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("mail_item_id_4", "0"));
                                int MakeItemEA_4 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("mail_item_ea_4", "0"));
                                int MakeItemGrade_4 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("mail_item_grade_4", "1"));
                                int MakeItemLevel_4 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("mail_item_level_4", "0"));
                                long MakeItemID_5 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("mail_item_id_5", "0"));
                                int MakeItemEA_5 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("mail_item_ea_5", "0"));
                                int MakeItemGrade_5 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("mail_item_grade_5", "1"));
                                int MakeItemLevel_5 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("mail_item_level_5", "0"));

                                string Title = queryFetcher.QueryParam_Fetch("mail_title", "");
                                string Body = queryFetcher.QueryParam_Fetch("mail_body", "");

                                long SenderID = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("sender_aid", "0"));
                                long ReceiverID = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("receiver_aid", "0"));

                                if (ReceiverID < 1)
                                    retError = Result_Define.eResult.SYSTEM_PARAM_ERROR;
                                else
                                    retError = Result_Define.eResult.SUCCESS;

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    Account_Simple setSenderInfo = AccountManager.GetSimpleAccountInfo(ref tb, SenderID);
                                    retError = MailManager.SendMail(ref tb, ReceiverID, SenderID, setSenderInfo.username, Title, Body
                                        , MakeItemID_1, MakeItemEA_1, MakeItemGrade_1, MakeItemLevel_1
                                        , MakeItemID_2, MakeItemEA_2, MakeItemGrade_2, MakeItemLevel_2
                                        , MakeItemID_3, MakeItemEA_3, MakeItemGrade_3, MakeItemLevel_3
                                        , MakeItemID_4, MakeItemEA_4, MakeItemGrade_4, MakeItemLevel_4
                                        , MakeItemID_5, MakeItemEA_5, MakeItemGrade_5, MakeItemLevel_5
                                                                    );
                                    MailManager.RemoveMailCache(SenderID);
                                    MailManager.RemoveMailCache(ReceiverID);
                                }
                            }
                        }
#endif

                        retJson = queryFetcher.Render(json.ToJson(), retError);
                    }
                    else
                    {
                        retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(DefineError.System_Unknown_Operation), Result_Define.eResult.System_Unknown_Operation);
                    }
                }
                catch (Exception errorEx)
                {
                    string error = "";
#if DEBUG
                    error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
#else
                    if (queryFetcher.SetDebugMode)
                        error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
#endif

                    error = mJsonSerializer.AddJson(error, "Message", mJsonSerializer.ToJsonString(errorEx.Message));
                    retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(error), Result_Define.eResult.System_Exception);
                }
                finally
                {
                    //if (AID > 0)
                    //    queryFetcher.CheckSnail_ID(ref tb, AID);
                    queryFetcher.SetShowLogMode = tb.EndTransaction(queryFetcher.Render_errorFlag);
                    queryFetcher.ErrorLogWrite(retJson, ref tb);
                    tb.Dispose();
                } 
            }
        }
    }
}