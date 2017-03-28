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
    public partial class RequestFriends : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] ops = new string[] {
                // 1 depth my friend list view op
                "my_list",
                "delete",
                "sendgift",
                "sendgift_all",
                "detail_info",
                "firend_set_pvp",

                // 2 depth find friend view op
                "friend_request_list",
                "friend_recommand_list",
                "friend_recommand_list_refresh",
                "accept_friend",
                "decline_friend",
                "invite_friend",
                "search_friend",

                // for facebook
                "fb_friend_list",
                "fb_friend_add",

                // for Debug test
                "reset_remaintime",
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

                        if (requestOp.Equals("my_list"))
                        {
                            retError = Result_Define.eResult.SUCCESS;
                            Dictionary<long, Friends> myFriendList = FriendManager.GetFriendsList(ref tb, AID);

                            json = mJsonSerializer.AddJson(json, Friend_Define.Friend_Ret_KeyList[Friend_Define.eFriendReturnKeys.MyFriendList], mJsonSerializer.ToJsonString(myFriendList.Values.ToList()));
                        }
                        else if (requestOp.Equals("search_friend"))
                        {
                            string searchName = queryFetcher.QueryParam_Fetch("friend_name");
                            Account_Simple_With_Connection findUserInfo = FriendManager.SearchFriend(ref tb, searchName);

                            if (findUserInfo.aid == 0)
                                retError = Result_Define.eResult.FRIEND_SEARCH_NO_RESULTS;
                            else
                                retError = Result_Define.eResult.SUCCESS;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.AccountSimpleInfo], mJsonSerializer.ToJsonString(findUserInfo));
                            }
                        }
                        else if (requestOp.Equals("invite_friend"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            long FAID = queryFetcher.QueryParam_FetchLong("friend_aid");
                            retError = FriendManager.RequestFriend(ref tb, AID, FAID);
                        }
                        else if (requestOp.Equals("accept_friend"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            long FAID = queryFetcher.QueryParam_FetchLong("friend_aid");
                            retError = FriendManager.AcceptFriend(ref tb, AID, FAID);
                        }
                        else if (requestOp.Equals("decline_friend"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            long FAID = queryFetcher.QueryParam_FetchLong("friend_aid");
                            retError = FriendManager.DeclineFriend(ref tb, AID, FAID);
                        }
                        else if (requestOp.Equals("friend_request_list"))
                        {
                            retError = Result_Define.eResult.SUCCESS;

                            Dictionary<long, Friends> reqFriendList = FriendManager.GetRequestFriendsList(ref tb, AID);
                            List<Friends> recommendFriendList = FriendManager.GetRecommandFriendsList(ref tb, AID);

                            json = mJsonSerializer.AddJson(json, Friend_Define.Friend_Ret_KeyList[Friend_Define.eFriendReturnKeys.ReqFriendList], mJsonSerializer.ToJsonString(reqFriendList.Values.ToList()));
                            json = mJsonSerializer.AddJson(json, Friend_Define.Friend_Ret_KeyList[Friend_Define.eFriendReturnKeys.RecommendFriendList], mJsonSerializer.ToJsonString(recommendFriendList));
                        }
                        else if (requestOp.Equals("friend_recommand_list") || requestOp.Equals("friend_recommand_list_refresh"))
                        {
                            retError = Result_Define.eResult.SUCCESS;
                            List<Friends> recommendFriendList = FriendManager.GetRecommandFriendsList(ref tb, AID);
                            json = mJsonSerializer.AddJson(json, Friend_Define.Friend_Ret_KeyList[Friend_Define.eFriendReturnKeys.RecommendFriendList], mJsonSerializer.ToJsonString(recommendFriendList));
                        }
                        else if (requestOp.Equals("delete"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            long FAID = queryFetcher.QueryParam_FetchLong("friend_aid");
                            retError = FriendManager.DeleteFriend(ref tb, AID, FAID);
                        }
                        else if (requestOp.Equals("sendgift"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            Account_Simple myInfo = AccountManager.GetSimpleAccountInfo(ref tb, AID);
                            string AID_list = queryFetcher.QueryParam_Fetch("send_list");
                            List<long> AID_Items = mJsonSerializer.JsonToObject<List<long>>(AID_list);
                            if (AID_Items == null)
                            {
                                AID_Items = new List<long>();
                                retError = Result_Define.eResult.ACCOUNT_ID_NOT_FOUND;
                            }
                            else if (AID_Items.Count < 1)
                                retError = Result_Define.eResult.ACCOUNT_ID_NOT_FOUND;
                            else
                                retError = Result_Define.eResult.SUCCESS;


                            Dictionary<long, Friends> myFriendList;
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                myFriendList = FriendManager.GetFriendsList(ref tb, AID);
                                foreach (long setAID in AID_Items)
                                {
                                    if (!myFriendList.ContainsKey(setAID))
                                    {
                                        retError = Result_Define.eResult.FRIEND_NO_ACCOUNT;
                                        break;
                                    }
                                    else if (myFriendList[setAID].keysendremaintime > 0)
                                    {
                                        retError = Result_Define.eResult.FRIEND_SEND_GIFT_TIME_REMAIN;
                                        break;
                                    }
                                }
                            }

                            int makeMyRewardCount = 0;
                            long MakeMailItemID = System.Convert.ToInt64(SystemData.GetConstValue(ref tb, "DEF_FRIEND_DAY_SEND_ITEM_TYPE"));
                            int MakeMailItemCount = System.Convert.ToInt32(SystemData.GetConstValue(ref tb, "DEF_FRIEND_DAY_SEND_ITEM_VALUE"));

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                foreach (long setAID in AID_Items)
                                {
                                    retError = MailManager.SendMail(ref tb, setAID, myInfo.aid, myInfo.username, TheSoul_String_Define.SetString_List[TheSoul_String_Define.eSystemString.System_Mail_SendGift], "", MakeMailItemID, MakeMailItemCount);

                                    if (retError != Result_Define.eResult.SUCCESS)
                                        break;

                                    retError = FriendManager.UpdateSendGiftTime(ref tb, AID, setAID);
                                    if (retError != Result_Define.eResult.SUCCESS)
                                        break;

                                    MailManager.RemoveMailCache(AID);
                                    makeMyRewardCount += MakeMailItemCount;
                                }
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<User_Inven> makeItem = new List<User_Inven>();
                                retError = ItemManager.MakeItem(ref tb, ref makeItem, AID, MakeMailItemID, makeMyRewardCount);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = TriggerManager.ProgressTrigger(ref tb, AID, Trigger_Define.eTriggerType.Friend_Key);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                myFriendList = FriendManager.GetFriendsList(ref tb, AID, true);

                                AID_Items.FirstOrDefault();

                                json = mJsonSerializer.AddJson(json, Friend_Define.Friend_Ret_KeyList[Friend_Define.eFriendReturnKeys.SendRewardItemID], mJsonSerializer.ToJsonString(MakeMailItemID));
                                json = mJsonSerializer.AddJson(json, Friend_Define.Friend_Ret_KeyList[Friend_Define.eFriendReturnKeys.SendRewardCount], mJsonSerializer.ToJsonString(makeMyRewardCount));
                                json = mJsonSerializer.AddJson(json, Friend_Define.Friend_Ret_KeyList[Friend_Define.eFriendReturnKeys.SendRemainTime], mJsonSerializer.ToJsonString(myFriendList[AID_Items.FirstOrDefault()].keysendremaintime));
                            }
                        }
                        else if (requestOp.Equals("detail_info"))
                        {
                            long FAID = queryFetcher.QueryParam_FetchLong("friend_aid");
                            long FCID = queryFetcher.QueryParam_FetchLong("friend_cid");
                            int ServerGroupID = queryFetcher.QueryParam_FetchInt("group_id");

                            TxnBlock globalTB = TheSoulDBcon.server_group_id != ServerGroupID && ServerGroupID > 0 ? new TxnBlock() : tb;

                            if (TheSoulDBcon.server_group_id != ServerGroupID && ServerGroupID > 0)
                            {
                                TheSoulDBcon.GetInstance().TheSoulDBInitFromGlobal(ref globalTB, ServerGroupID);
                                //RedisConst.GetRedisInstance().SetPrefixTag(string.Format("{0}_{1}", DataManager_Define.RedisTagPrefix, ServerGroupID));
                            }

                            List<Character> userCharList = CharacterManager.GetCharacterList(ref globalTB, FAID, true);

                            var findChar = userCharList.Find(charinfo => charinfo.cid == FCID);
                            if (findChar == null && FCID != 0)
                                retError = Result_Define.eResult.CHARACTER_NOT_FOUND;
                            else if (userCharList.Count < 1)
                                retError = Result_Define.eResult.CHARACTER_NOT_FOUND;
                            else
                                retError = Result_Define.eResult.SUCCESS;

                            if (FCID == 0 && retError == Result_Define.eResult.SUCCESS)
                            {
                                findChar = userCharList.FirstOrDefault();
                                if (findChar != null)
                                    FCID = findChar.cid;
                                else
                                    retError = Result_Define.eResult.CHARACTER_NOT_FOUND;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Character_Detail charInfo = new Character_Detail(findChar);
                                // use only db and not set cache. cause user info confused (not matched)
                                charInfo.equiplist = ItemManager.GetEquipList(ref globalTB, FAID, FCID, true, true);
                                charInfo.equip_active_soul = SoulManager.GetRet_Active_Soul_Equip_List(ref globalTB, FAID, FCID, true);
                                charInfo.equip_passive_soul = SoulManager.GetRet_Passive_Soul_Equip_List(ref globalTB, FAID, FCID, true);
                                charInfo.equip_ultimate = ItemManager.GetEquipUltimate(ref globalTB, FAID, FCID, true);

                                //ItemManager.RemoveEquipList(FAID, FCID);
                                //SoulManager.RemoveCacheUser_ActiveSoul(FAID);
                                //SoulManager.RemoveCacheUser_ActiveSoul_Equip(FAID);
                                //SoulManager.RemoveCacheUser_ActiveSoul_Special_Buff(FAID, FCID);
                                //SoulManager.RemoveCacheUser_Equip_Soul(FAID, FCID);
                                //SoulManager.RemoveCacheUser_PassiveSoul(FAID, FCID);
                                //SoulManager.RemoveCacheUser_Soul_Equip_Inven(FAID);
                                //ItemManager.RemoveUltimateWeaonCache(FAID, FCID);
                                //charInfo.equip_ultimate.ForEach(setUltimate =>
                                //{
                                //    ItemManager.RemoveUltimateOrbCache(FAID, setUltimate.ultimate_inven_seq);
                                //});

                                List<Character_Simple> cidList = new List<Character_Simple>();
                                foreach (Character setInfo in userCharList)
                                {
                                    cidList.Add(new Character_Simple(setInfo));
                                }

                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.CharacterDetailInfo], charInfo.ToJson());
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.CharacterSimpleInfoList], mJsonSerializer.ToJsonString(cidList));
                            }

                            //RedisConst.GetRedisInstance().SetPrefixTag(string.Format("{0}_{1}", DataManager_Define.RedisTagPrefix, TheSoulDBcon.server_group_id));

                            if (tb != globalTB)
                            {
                                globalTB.Dispose();                                
                            }
                        }
                        else if (requestOp.Equals("firend_set_pvp"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            int setFlag = queryFetcher.QueryParam_FetchInt("pvp_friend_flag");
                            retError = AccountManager.UpdatePVPFriendFlag(ref tb, AID, setFlag > 0);
                        }
                        else if (requestOp.Equals("fb_friend_list"))
                        {
                            retError = Result_Define.eResult.SUCCESS;
                            List<string> myFriendList = FriendManager.GetFaceBookFriendsList(ref tb, AID);
                            json = mJsonSerializer.AddJson(json, Friend_Define.Friend_Ret_KeyList[Friend_Define.eFriendReturnKeys.MyFaceBookFriendsList], mJsonSerializer.ToJsonString(myFriendList));
                        }
                        else if (requestOp.Equals("fb_friend_add"))
                        {
                            List<string> fb_uid_list = mJsonSerializer.JsonToObject<List<string>>(queryFetcher.QueryParam_Fetch("fb_uid", "[]"));

                            foreach (string fb_uid in fb_uid_list)
                            {
                                retError = FriendManager.InsertFaceBookFriend(ref tb, AID, fb_uid);
                                if (retError != Result_Define.eResult.SUCCESS)
                                    break;
                            }
                        }
#if DEBUG
                        else if (Request.Params.AllKeys.Contains("Debug"))
                        {
                            if (requestOp.Equals("reset_remaintime"))
                            {
                                retError = Result_Define.eResult.SUCCESS;
                                FriendManager.UpdateSendGiftTimeReset(ref tb, AID);
                                FriendManager.RemoveFriendListCache(ref tb, AID);
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