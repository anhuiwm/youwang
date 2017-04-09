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
using TheSoulCheatServer.lib;
using System.Net.Json;

namespace TheSoulCheatServer
{
    public partial class Account_init : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JsonObjectCollection res = new JsonObjectCollection();
            if (Request["username"] == null || Request["username"] == "")
            {
                res.Add(new JsonNumericValue("resultcode", 1));
                res.Add(new JsonStringValue("message", "Post 값 전달 실패"));
                string json_text = res.ToString();
                Response.Write(json_text);
            }
            else
            {
                string username = Request["username"];

                WebQueryParam queryFetcher = new WebQueryParam();
                string retJson = "";
                queryFetcher.SetDebugMode = true;
                string attend = queryFetcher.QueryParam_Fetch("attend", "");
                string mailbox = queryFetcher.QueryParam_Fetch("mailbox", "");
                string achievement = queryFetcher.QueryParam_Fetch("achievement", "");
                string missionWorld = queryFetcher.QueryParam_Fetch("missionWorld", "");
                string dungeonShop = queryFetcher.QueryParam_Fetch("dungeonShop", "");

                TxnBlock tb = new TxnBlock();
                {
                    long AID = 0;
                    try
                    {
    
                        queryFetcher.TxnBlockInit(ref tb, ref AID);
                        queryFetcher.GlobalDBOpen(ref tb);


                        User_account user_server = cheatData.GetUserAID(ref tb, username);
                        AID = user_server.AID;

                        Result_Define.eResult retErr = Result_Define.eResult.POST_DATA_ERROR;
                        Account userInfo = AccountManager.FlushAccountData(ref tb, AID, ref retErr);

                        if (!string.IsNullOrEmpty(mailbox))
                        {
                            List<User_Mail_Datail> mailList = cheatData.GetUser_Mail_All_List(ref tb, AID, true);
                            foreach (User_Mail_Datail mail in mailList)
                            {
                                retErr = MailManager.Update_MailReadFlag(ref tb, AID, mail.mailseq, false);
                            }

                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                MailManager.RemoveMailCache(AID);
                            }
                        }

                        if (!string.IsNullOrEmpty(attend))
                        {
                            string setQuery = string.Format("Update {0} Set regdate = DateAdd(D,-1,regdate) Where Aid={1}", Trigger_Define.User_Event_Check_Data_TableName, AID);
                            retErr = tb.ExcuteSqlCommand(Dungeon_Define.Dungeon_Info_DB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        }

                        if (!string.IsNullOrEmpty(achievement))
                        {
                            string setQuery = string.Format("Update {0} Set CurrentValue1 = 0, CurrentValue2 = 0, ClearFlag = 'P', RewardFlag='N' Where Aid={1}", Trigger_Define.User_Achieve_Data_TableName, AID);
                            retErr = tb.ExcuteSqlCommand(Dungeon_Define.Dungeon_Info_DB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                TriggerManager.Check_Achieve_Data_List(ref tb, AID, true);
                            }
                        }

                        if (!string.IsNullOrEmpty(dungeonShop))
                        {
                            string setQuery = string.Format("Update {0} Set regdate = DateAdd(D,-1,regdate) Where AID={1}", Shop_Define.Shop_User_Buy_TableName, AID);
                            retErr = tb.ExcuteSqlCommand(Dungeon_Define.Dungeon_Info_DB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        }

                        if (!string.IsNullOrEmpty(missionWorld))
                        {/*
                          * list
                            List<RetWorldRank> GetUser_All_WorldReward
                          * update
                          * UpdateWorldRankReward
                          */

                            List<RetWorldRank> worldList = Dungeon_Manager.GetUser_All_WorldReward(ref tb, AID);
                            foreach (RetWorldRank world in worldList)
                            {
                                retErr = Dungeon_Manager.UpdateWorldRankReward(ref tb, AID, world.worldid, 1, false);
                            }

                        }

                        retJson = queryFetcher.Render("", retErr);

                    }
                    catch (Exception errorEx)
                    {
                        retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(errorEx.Message), Result_Define.eResult.System_Exception);
                    }
                    tb.EndTransaction(queryFetcher.Render_errorFlag);
                    tb.Dispose();
                }
            }

        }
    }
}