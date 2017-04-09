using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Net;

namespace TheSoulCheatServer
{
    public partial class cheat_init : System.Web.UI.Page
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
                string co_op = queryFetcher.QueryParam_Fetch("co_op", "");
                string co_op2 = queryFetcher.QueryParam_Fetch("co_op2", "");
                string bark = queryFetcher.QueryParam_Fetch("bark", "");
                string expedition = queryFetcher.QueryParam_Fetch("expedition", "");
                string mission = queryFetcher.QueryParam_Fetch("mission", "");
                string hero = queryFetcher.QueryParam_Fetch("hero", "");
                string hier = queryFetcher.QueryParam_Fetch("hier", "");
                string shop = queryFetcher.QueryParam_Fetch("shop", "");
                int viplevel = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("level", "0"));
                int worldId = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("world", "0"));
                int stageID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("stage", "0"));
                string del_Mission = queryFetcher.QueryParam_Fetch("del_mission", "");
                
                TxnBlock tb = new TxnBlock();
                {
                    long AID = 0;
                    try
                    {
    
                        queryFetcher.TxnBlockInit(ref tb, ref AID);
                        long serverID = queryFetcher.GlobalDBOpen(ref tb);


                        User_account user_server = cheatData.GetUserAID(ref tb, username);
                        AID = user_server.AID;

                        Result_Define.eResult retErr = Result_Define.eResult.POST_DATA_ERROR;
                        Account userInfo = AccountManager.FlushAccountData(ref tb, AID, ref retErr);

                        if (!string.IsNullOrEmpty(co_op))
                        {
                            string setQuery = string.Format("Update {0} Set play_resetcount = 0 Where Aid={1}", PvP_Define.PvP_PlayInfo_TableName, AID);
                            retErr = tb.ExcuteSqlCommand(Dungeon_Define.Dungeon_Info_DB,setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                for (int i = 1; i <= 7; i++)
                                {
                                    PvPManager.GetUser_PvPInfo(ref tb, AID, (PvP_Define.ePvPType)i, true);
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(del_Mission))
                        {
                            string setQuery = string.Format("Delete From {0} Where AID = {1}", Dungeon_Define.Mission_Play_TableName, AID);
                            string setQuery2 = string.Format("Delete From {0} Where AID = {1}", Dungeon_Define.World_Rank_Reward_TableName, AID);
                            retErr = tb.ExcuteSqlCommand(Dungeon_Define.Dungeon_Info_DB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                            if(retErr == Result_Define.eResult.SUCCESS)
                                retErr = tb.ExcuteSqlCommand(Dungeon_Define.Dungeon_Info_DB, setQuery2) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                server_config dbServer = GlobalManager.GetServerList(ref tb).Find(item => item.server_type.Equals("web_server") && item.server_group_id == (int)serverID);
                                string WebIP = string.IsNullOrEmpty(dbServer.server_public_ip) ? dbServer.server_private_ip : dbServer.server_public_ip;
                                string WebPort = (dbServer.server_public_port == 0) ? dbServer.server_private_port.ToString() : dbServer.server_public_port.ToString();
                                string url = string.Format("http://{0}:{1}/RequestPrivateServer.aspx?op=redis_flush", WebIP, WebPort);
                                //string dataParam = "op=redis_flush";
                                try
                                {
                                    HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(url);
                                    webReq.Method = "Get";
                                    webReq.ContentType = "application/x-www-form-urlencoded";

                                    HttpWebResponse wRespFirst = (HttpWebResponse)webReq.GetResponse();
                                    Stream respPostStream = wRespFirst.GetResponseStream();
                                    StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);
                                    string resultPost = readerPost.ReadToEnd();

                                }
                                catch (Exception ae)
                                {
                                    Console.Write(ae.Message);
                                }
                            }
                            
                        }

                        if (!string.IsNullOrEmpty(co_op2))
                        {
                            string setQuery = string.Format("Update {0} Set play_count = 0 Where Aid={1}", PvP_Define.PvP_PlayInfo_TableName, AID);
                            retErr = tb.ExcuteSqlCommand(Dungeon_Define.Dungeon_Info_DB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                for (int i = 1; i <= 7; i++)
                                {
                                    PvPManager.GetUser_PvPInfo(ref tb, AID, (PvP_Define.ePvPType)i, true);
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(bark))
                        {
                            string setQuery = string.Format("Update {0} Set challengereset = 0 Where Aid={1}", Dungeon_Define.Dark_Passage_Play_TableName, AID);
                            retErr = tb.ExcuteSqlCommand(Dungeon_Define.Dungeon_Info_DB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                List<User_GuerrillaDungeon_Play> list = cheatData.GetUser_DarkPassagePlayList(ref tb, AID);
                                foreach (User_GuerrillaDungeon_Play data in list)
                                {
                                    Dungeon_Manager.GetUser_DarkPassagePlayInfo(ref tb, ref retErr, AID, data.dungeonid, true);
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(mission))
                        {
                            string setQuery = string.Format("Update {0} Set ChallengeReset = 0 Where Aid={1}", Dungeon_Define.Mission_Play_TableName, AID);
                            retErr = tb.ExcuteSqlCommand(Dungeon_Define.Dungeon_Info_DB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                List<User_Mission_Play> list = cheatData.GetUser_MissionPlayList(ref tb, AID);
                                foreach (User_Mission_Play data in list)
                                {
                                    Dungeon_Manager.GetUser_MissionInfo(ref tb, ref retErr, AID, data.worldid, data.stageid, true);
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(shop))
                        {
                            string setQuery = string.Format("Update {0} Set reset_count = 0 Where Aid={1}", Shop_Define.Shop_User_Reset_TableName, AID);
                            retErr = tb.ExcuteSqlCommand(Shop_Define.Shop_Info_DB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                foreach (Shop_Define.eShopType ShopType in Enum.GetValues(typeof(Shop_Define.eShopType)))
                                {
                                    ShopManager.GetUser_All_BuyItemCount(ref tb, AID, ShopType, true);
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(expedition))
                        {
                            string setQuery = string.Format("Update {0} Set ResetCount =0 Where Aid={1}", GoldExpedition_Define.User_GE_Stage_Info_TableName, AID);
                            retErr = tb.ExcuteSqlCommand(GoldExpedition_Define.GoldExpedition_Info_DB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                GoldExpedition_Manager.RemoveCacheUser_GE_Stage_Info(AID);
                                GoldExpedition_Manager.RemoveUser_GE_Stage_Enemy(AID);
                                GoldExpedition_Manager.RemoveCacheUser_GE_Stage_Info(AID);
                                GoldExpedition_Manager.RemoveCacheUser_GE_Boost_Item(AID);
                            }
                        }

                        if (!string.IsNullOrEmpty(hier))
                        {
                            string setQuery = string.Format("Update {0} Set HireCount = 0 Where Aid={1}", GoldExpedition_Define.User_GE_Stage_Info_TableName, AID);
                            retErr = tb.ExcuteSqlCommand(GoldExpedition_Define.GoldExpedition_Info_DB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                GoldExpedition_Manager.RemoveUser_GE_Stage_Enemy(AID);
                                GoldExpedition_Manager.RemoveCacheUser_GE_Stage_Info(AID);
                                GoldExpedition_Manager.RemoveCacheUser_GE_Boost_Item(AID);
                            }
                        }

                        if (!string.IsNullOrEmpty(hero))
                        {
                            string setQuery = string.Format("Update {0} Set regdate = DateAdd(D,-1,regdate) Where Aid={1}", GoldExpedition_Define.User_Guild_Mercenary_Info_TableName, AID);
                            retErr = tb.ExcuteSqlCommand(GoldExpedition_Define.GoldExpedition_Guild_Info_DB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                GoldExpedition_Manager.RemoveUser_GE_Stage_Enemy(AID);
                                GoldExpedition_Manager.RemoveCacheUser_GE_Stage_Info(AID);
                                GoldExpedition_Manager.RemoveCacheUser_GE_Boost_Item(AID);
                            }
                        }

                        if (viplevel > 0)
                        {
                            System_VIP_Level levelInfo = VipManager.GetSystem_VIP_Level(ref tb, (viplevel-1));
                            string setQuery = string.Format("Update {0} Set VIPPoint = 0, TotalVIPPoint = 0 Where Aid={1}", VIP_Define.User_VIP_TableName, AID);
                            retErr = tb.ExcuteSqlCommand(VIP_Define.VIP_InfoDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                retErr = VipManager.VIPPointAdd(ref tb, AID, levelInfo.LevelUpPoint);
                            }
                        }

                        if (worldId > 0 && stageID > 0)
                        {
                            int LoopCnt = 1;
                            int LoopCnt2 = 1;
                            while (LoopCnt < (worldId + 1))
                            {
                                int calNum = 1;
                                while (LoopCnt2 < ((worldId * 10) - (10 - stageID) + 1))
                                {
                                    if (calNum > 10)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        System_Mission_Stage stageInfo = Dungeon_Manager.GetSystem_MissionStageInfo(ref tb, LoopCnt2);
                                        User_Mission_Play userMissionInfo = Dungeon_Manager.GetUser_MissionInfo(ref tb, ref retErr, AID, LoopCnt, LoopCnt2);
                                        RetTaskResult UserTask1 = new RetTaskResult();
                                        RetTaskResult UserTask2 = new RetTaskResult();
                                        RetTaskResult UserTask3 = new RetTaskResult();

                                        if (stageInfo.Task1ID > 0)
                                            UserTask1 = Dungeon_Manager.CheckTaskStatus(ref tb, stageInfo.Task1ID, userMissionInfo.task1value, 0, userMissionInfo.task1);
                                        if (stageInfo.Task2ID > 0)
                                            UserTask2 = Dungeon_Manager.CheckTaskStatus(ref tb, stageInfo.Task2ID, userMissionInfo.task2value, 0, userMissionInfo.task2);
                                        if (stageInfo.Task3ID > 0)
                                            UserTask3 = Dungeon_Manager.CheckTaskStatus(ref tb, stageInfo.Task3ID, userMissionInfo.task3value, 0, userMissionInfo.task3);

                                        userMissionInfo.rank = 3;
                                        userMissionInfo.ClearTime = 10;
                                        userMissionInfo.task1 = UserTask1.taskClear;
                                        userMissionInfo.task1value = UserTask1.taskValue;
                                        userMissionInfo.task2 = UserTask2.taskClear;
                                        userMissionInfo.task2value = UserTask2.taskValue;
                                        userMissionInfo.task3 = UserTask3.taskClear;
                                        userMissionInfo.task3value = UserTask3.taskValue;

                                        Dungeon_Manager.UpdateMissionTask(ref tb, ref userMissionInfo);

                                        User_Mission_Play userMission = Dungeon_Manager.GetUser_MissionInfo(ref tb, ref retErr, AID, LoopCnt, LoopCnt2, true);
                                        if (userMission != null)
                                        {
                                            retErr = Result_Define.eResult.SUCCESS;
                                            LoopCnt2 = LoopCnt2 + 1;
                                            calNum = calNum + 1;
                                        }
                                    }
                                }
                                LoopCnt = LoopCnt + 1; 
                            }
                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                AccountManager.UpdatePVEFlag(ref tb, AID, false, worldId, LoopCnt2);
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