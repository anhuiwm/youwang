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
    public partial class RequestOverlord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] ops = new string[] {
                "overlord_list",
                "overlord_top_rank",
                "get_targetinfo", //대전 상대 리스트를 선택하여 대전이 가능한 경우, 상대의 소지 캐릭터 정보, 장착된 아이템을 클라이언트가 요청함.
                "overlord_result",
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

                        if (requestOp.Equals("overlord_list"))
                        {
                            User_PVP_Overlord_Ranking getUserRankInfo = PvPManager.GetUser_Overlord_Ranking_ByAID(ref tb, AID);
                            //retError = getUserRankInfo.Ranking > 0 ? Result_Define.eResult.SUCCESS : Result_Define.eResult.PVP_OVERLORD_INFO_NOT_FOUND;
                            List<User_PVP_Overlord_Record> playRecordList = new List<User_PVP_Overlord_Record>();

                            //if (retError == Result_Define.eResult.SUCCESS)
                            if (getUserRankInfo.Ranking > 0)
                                retError = PvPManager.GetUser_PVP_Overlord_Record(ref tb, AID, ref playRecordList);
                            else
                                retError = Result_Define.eResult.SUCCESS;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {                                
                                List<Ret_OverLord> RankList = new List<Ret_OverLord>();
                                List<Ret_Overlord_Record> retRecordList = playRecordList.ConvertAll<Ret_Overlord_Record>(setInfo => Ret_Overlord_Record.CastToRet_Overlord_Record(setInfo));

                                long totalPlayer = 0;
                                retError = PvPManager.MakeMatchingOverlordEnemyInfo(ref tb, AID, getUserRankInfo.Ranking, ref RankList, out totalPlayer);

                                Ret_Overlord_User_Info userPvPInfo = new Ret_Overlord_User_Info(PvPManager.GetUser_PvPInfo(ref tb, AID, PvP_Define.ePvPType.MATCH_OVERLORD));                                
                                userPvPInfo.total_rank_player = totalPlayer;
                                userPvPInfo.my_ranking = getUserRankInfo.Ranking;
                                
                                json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Overlord_UserInfo], mJsonSerializer.ToJsonString(userPvPInfo));
                                json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Overlord_List], mJsonSerializer.ToJsonString(RankList));
                                json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Overlord_PlayList], mJsonSerializer.ToJsonString(retRecordList));
                            }
                        }
                        else if (requestOp.Equals("overlord_top_rank"))
                        {
                            retError = Result_Define.eResult.SUCCESS;
                            List<Ret_OverLord> getList = PvPManager.GetUser_Overlord_Top_Ranking(ref tb);
                            json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Overlord_RankList], mJsonSerializer.ToJsonString(getList));
                        }
                        else if (requestOp.Equals("get_targetinfo"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            long targetAID = queryFetcher.QueryParam_FetchLong("target");
                            byte isNpc = queryFetcher.QueryParam_FetchByte("isnpc");
                            byte rubyStart = queryFetcher.QueryParam_FetchByte("rubytry");

                            User_PVP_Overlord_Ranking getMyRankInfo = PvPManager.GetUser_Overlord_Ranking_ByAID(ref tb, AID);
                            User_PVP_Overlord_Ranking getEnemyRankInfo = getMyRankInfo.Ranking > 0 ? PvPManager.GetUser_Overlord_Ranking_ByAID(ref tb, targetAID, isNpc) : PvPManager.GetUser_Overlord_Ranking_Dummy_ByAID(ref tb, targetAID);
                            bool bCheckFirstPlay = getMyRankInfo.Ranking == 0;

                            retError = getEnemyRankInfo.Flag > (byte)PvP_Define.ePvPPlayFlag.None && !bCheckFirstPlay ? Result_Define.eResult.PVP_PLAYER_ALREADY_JOIN_BATTLE : Result_Define.eResult.SUCCESS;

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = (getEnemyRankInfo.AID == targetAID) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.PVP_OVERLORD_INFO_NOT_FOUND;

                            if (retError == Result_Define.eResult.SUCCESS && !bCheckFirstPlay)
                            {
                                getMyRankInfo.Flag = (byte)PvP_Define.ePvPPlayFlag.Play;
                                retError = PvPManager.SetUser_Overlord_Ranking(ref tb, getMyRankInfo);
                            }

                            if (retError == Result_Define.eResult.SUCCESS && !bCheckFirstPlay)
                            {
                                getEnemyRankInfo.Flag = (byte)PvP_Define.ePvPPlayFlag.Play;
                                retError = PvPManager.SetUser_Overlord_Ranking(ref tb, getEnemyRankInfo);
                            }

                            if (retError == Result_Define.eResult.SUCCESS && rubyStart < 1)
                                retError = AccountManager.UseUserTicket(ref tb, AID, SystemData.GetConstValueInt(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_BATTLE_RANKING_ENTER_COST]));

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Ret_Overlord_User_Info userPvPInfo = new Ret_Overlord_User_Info(PvPManager.GetUser_PvPInfo(ref tb, AID, PvP_Define.ePvPType.MATCH_OVERLORD));

                                if (userPvPInfo.max_play_count <= userPvPInfo.play_count)
                                    retError = rubyStart < 1 ? Result_Define.eResult.PVP_PLAYCOUNT_OVER : AccountManager.UseUserCash(ref tb, AID, SystemData.GetConstValueInt(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_BATTLE_RANKING_EX_TRY_RUBY]));
                                else
                                    retError = Result_Define.eResult.SUCCESS;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = PvPManager.AddUser_PvP_CountToDB(ref tb, AID, PvP_Define.ePvPType.MATCH_OVERLORD);

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = TriggerManager.ProgressTrigger(ref tb, AID, Trigger_Define.eTriggerType.Play_PVP, (int)Trigger_Define.ePvPType.MATCH_OVERLORD);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<Character_Detail> getList = new List<Character_Detail>();

                                if (targetAID > 0 && isNpc > 0)     // npc player (dummy)
                                {
                                    long enemyAID = targetAID % PvP_Define.Overlord_NPC_Seperator;
                                    getList = DummyManager.makeDummyCharacterListInfo(ref tb, AID, enemyAID);
                                }
                                else if (targetAID > 0)
                                {
                                    getList = CharacterManager.GetCharacterListWithEquip(ref tb, targetAID);
                                }
                                else
                                {
                                    retError = Result_Define.eResult.ACCOUNT_ID_NOT_FOUND;
                                }

                                if (retError == Result_Define.eResult.SUCCESS && getList.Count < 1)
                                    retError = Result_Define.eResult.CHARACTER_NOT_FOUND;

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_act_type], 0);
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], SnailLog_Define.Operation_To_S_Event_ID(SnailLog_Define.GetOperationSID[requestOp]));                
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_game_player_action_log]);

                                    Account_OnlyAID setAID = new Account_OnlyAID();
                                    setAID.aid = targetAID;

                                    json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Overlord_EnemyInfo], Character_Detail.makeCharacter_DetailListJson(ref getList));
                                }
                            }
                        }
                        else if (requestOp.Equals("overlord_result"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            long targetAID = queryFetcher.QueryParam_FetchLong("target");
                            byte isNpc = queryFetcher.QueryParam_FetchByte("isnpc");
                            bool bWin = queryFetcher.QueryParam_FetchByte("iswin") > 0;
                            long CID = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("cid"));

                            User_PVP_Overlord_Ranking getMyRankInfo = new User_PVP_Overlord_Ranking();
                            User_PVP_Overlord_Ranking getEnemyRankInfo = new User_PVP_Overlord_Ranking();

                            getMyRankInfo = PvPManager.GetUser_Overlord_Ranking_ByAID(ref tb, AID);

                            long myOldRank = 0;
                            long enemyOldRank = 0;
                            bool bCheckFirstPlay = getMyRankInfo.Ranking == 0;

                            if (!bCheckFirstPlay)
                            {
                                if (targetAID > 0 && AID > 0)
                                {
                                    getEnemyRankInfo = PvPManager.GetUser_Overlord_Ranking_ByAID(ref tb, targetAID, isNpc);
                                    retError = (getEnemyRankInfo.AID == targetAID) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.PVP_OVERLORD_INFO_NOT_FOUND;
                                }
                                else
                                    retError = Result_Define.eResult.ACCOUNT_ID_NOT_FOUND;

                                //if (retError == Result_Define.eResult.SUCCESS && getMyRankInfo.Ranking > 0)
                                //    retError = getMyRankInfo.Flag > (byte)PvP_Define.ePvPPlayFlag.None ? Result_Define.eResult.SUCCESS : Result_Define.eResult.PVP_PLAYER_NOT_IN_PLAY;

                                myOldRank = getMyRankInfo.Ranking;
                                enemyOldRank = getEnemyRankInfo.Ranking;
                            }
                            else
                            {
                                long totalPlayerRanking = PvPManager.GetUser_Overlord_Ranking_TotalPlayer(ref tb);
                                if (targetAID > 0 && AID > 0)
                                {
                                    getEnemyRankInfo = PvPManager.GetUser_Overlord_Ranking_Dummy_ByAID(ref tb, targetAID);
                                    getEnemyRankInfo.Ranking += totalPlayerRanking;
                                    retError = Result_Define.eResult.SUCCESS;
                                }
                                else
                                    retError = Result_Define.eResult.ACCOUNT_ID_NOT_FOUND;

                                myOldRank = totalPlayerRanking + PvP_Define.PvP_Overlord_Ranking_Dummy_Count;
                                enemyOldRank = getEnemyRankInfo.Ranking;
                            }
                            Result_Define.eResult rankResult = Result_Define.eResult.SUCCESS;

                            List<Client_Use_SkillInfo> useSkillList = mJsonSerializer.JsonToObject<List<Client_Use_SkillInfo>>(queryFetcher.QueryParam_Fetch("maxdmg", "[]"));
                            bool bVerify = false;
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Error_Skill_Info errSkill = new Error_Skill_Info();
                                bVerify = SoulManager.CheckSkillVerify(ref tb, AID, useSkillList, ref retError, out errSkill);
                                json = mJsonSerializer.AddJson(json, "errskill", mJsonSerializer.ToJsonString(errSkill));
                                if (!bVerify && retError == Result_Define.eResult.SUCCESS)
                                    retError = Result_Define.eResult.System_Hack_Detected;
                            }

                            if (retError == Result_Define.eResult.SUCCESS && !bCheckFirstPlay)
                            {
                                getMyRankInfo.Flag = getEnemyRankInfo.Flag = (byte)PvP_Define.ePvPPlayFlag.None;
                                rankResult = bWin ? (getMyRankInfo.Ranking < getEnemyRankInfo.Ranking ? Result_Define.eResult.E_OVERLOD_END_RESULT_NO_CHANGE_RANKING : Result_Define.eResult.E_OVERLOD_END_RESULT_CHANGE_RANKING) : Result_Define.eResult.E_OVERLOD_END_RESULT_REGISTERED_NO_CHANGE_RANKING_BY_LOSE;
                                if (rankResult == Result_Define.eResult.E_OVERLOD_END_RESULT_CHANGE_RANKING)
                                {
                                    getMyRankInfo.Ranking = getEnemyRankInfo.Ranking;
                                    getEnemyRankInfo.Ranking = myOldRank;
                                }
                            }
                            else if (retError == Result_Define.eResult.SUCCESS && bCheckFirstPlay && bVerify)
                            {
                                getMyRankInfo.Flag = getEnemyRankInfo.Flag = (byte)PvP_Define.ePvPPlayFlag.None;
                                rankResult = bWin ? Result_Define.eResult.E_OVERLOD_END_RESULT_CHANGE_RANKING : Result_Define.eResult.E_OVERLOD_END_RESULT_REGISTERED_NO_CHANGE_RANKING_BY_LOSE;
                                if (rankResult == Result_Define.eResult.E_OVERLOD_END_RESULT_CHANGE_RANKING)
                                {
                                    getMyRankInfo = PvPManager.InsertPvP_OverlordRanking(ref tb, AID);
                                }
                            }

                            if (retError == Result_Define.eResult.SUCCESS && !bCheckFirstPlay)
                                retError = PvPManager.SetUser_Overlord_Ranking(ref tb, getMyRankInfo);

                            if (retError == Result_Define.eResult.SUCCESS && !bCheckFirstPlay)
                                retError = PvPManager.SetUser_Overlord_Ranking(ref tb, getEnemyRankInfo);

                            if (retError == Result_Define.eResult.SUCCESS && getEnemyRankInfo.isNPC < 1 && !bCheckFirstPlay)
                                retError = PvPManager.InsertUser_PvP_Overlord_PlayRecord(ref tb, getEnemyRankInfo.AID, enemyOldRank, getEnemyRankInfo.Ranking, AID, !bWin);

                            Character charInfo = retError == Result_Define.eResult.SUCCESS ? CharacterManager.GetCharacter(ref tb, AID, CID) : new Character();
                            RetBeforeInfo retBefore = new RetBeforeInfo();
                            Account userInfo = retError == Result_Define.eResult.SUCCESS ? AccountManager.GetAccountData(ref tb, AID, ref retError) : new Account();
                            //int SetExp = 0;
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                retBefore = new RetBeforeInfo(charInfo.level, charInfo.exp, userInfo.Gold, userInfo.Cash + userInfo.EventCash,
                                                            userInfo.Key, userInfo.KeyFillMaxEA, userInfo.Ticket, userInfo.TicketFillMaxEA, userInfo.ChallengeTicket);

                                //int getExp = (int)(SystemData.GetConstValueInt(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_BATTLE_RANKING_ENTER_COST]) * 10 * (bWin ? 1.0f : 0.5f));
                                //retError = CharacterManager.UpdateCharacterInfo(ref tb, CID, AID, getExp, 0);

                                //charInfo = CharacterManager.GetCharacter(ref tb, AID, CID, true);
                                //retBefore.levelup = retBefore.beforelevel < charInfo.level ? 1 : 0;
                                //charInfo.exp = retBefore.beforelevel == charInfo.level && charInfo.level == Character_Define.Max_CharacterLevel ? SetExp : charInfo.exp;

                                if (retError == Result_Define.eResult.SUCCESS && userInfo.GuildID > 0 && bWin)
                                {
                                    int guildDonatePoint = Guild_Define.AddGuildPoint_List[Guild_Define.ePlayType.MATCH_OVERLORD];
                                    retError = GuildManager.AddGuildPoint(ref tb, userInfo.GuildID, AID, guildDonatePoint);
                                }
                            }

                            long rewardHighGrade = 0, highGrade = 0;
                            if (retError == Result_Define.eResult.SUCCESS && getMyRankInfo.Ranking > 0 && getMyRankInfo.Ranking < PvP_Define.Overlord_HighGradeReward_Min)
                            {
                                highGrade = PvPManager.GetUser_PvP_High_Grade(ref tb, AID, (int)PvP_Define.ePvPType.MATCH_OVERLORD);                                
                                highGrade = highGrade < 1 ? PvP_Define.Overlord_HighGradeReward_Min : highGrade;
                                if (highGrade > getMyRankInfo.Ranking)
                                {
                                    rewardHighGrade = PvPManager.CaclHighGradeReward(ref tb, highGrade, getMyRankInfo.Ranking);
                                    retError = PvPManager.SetUser_PvP_High_Grade(ref tb, AID, getMyRankInfo.Ranking, (int)PvP_Define.ePvPType.MATCH_OVERLORD);
                                }
                            }

                            if (retError == Result_Define.eResult.SUCCESS && rewardHighGrade > 0 && highGrade > 0)
                            {
                                List<Set_Mail_Item> setMailItem = new List<Set_Mail_Item>();
                                setMailItem.Add(new Set_Mail_Item(PvP_Define.RewardItemID_Ruby, (int)rewardHighGrade));
                                string setTitle = PvPManager.GetBattleReward_Mail_Title(PvP_Define.ePvPType.MATCH_OVERLORD, PvP_Define.ePvPRewardRepeatType.Once, getMyRankInfo.Ranking);
                                string setText = PvPManager.GetBattleReward_Mail_Body(PvP_Define.ePvPType.MATCH_OVERLORD, PvP_Define.ePvPRewardRepeatType.Once, getMyRankInfo.Ranking);
                                retError = MailManager.SendMail(ref tb, ref setMailItem, AID, 0, "", setTitle, setText);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<TriggerProgressData> setDataList = new List<TriggerProgressData>();
                                if(bWin)
                                    setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Win_PVP, (int)Trigger_Define.ePvPType.MATCH_OVERLORD));

                                //setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_PVP, (int)Trigger_Define.ePvPType.MATCH_OVERLORD));
                                retError = TriggerManager.ProgressTrigger(ref tb, AID, setDataList);                                
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], SnailLog_Define.Operation_To_S_Event_ID(SnailLog_Define.GetOperationSID[requestOp]));
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_act_type], 1);
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_game_player_action_log]);
                                                                        
                                    Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userAccount);

                                    json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Overlord_MatchResult], ((int)rankResult).ToString());
                                    json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Overlord_BeforeRank], ((int)myOldRank).ToString());
                                    json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Overlord_AfterRank], ((int)getMyRankInfo.Ranking).ToString());
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.CharacterInfo], mJsonSerializer.ToJsonString(charInfo));
                                    json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.BeforeInfo], mJsonSerializer.ToJsonString(retBefore));
                                }
                            }
                        }

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

