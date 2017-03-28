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
    public partial class cheat_recommendFriend : System.Web.UI.Page
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

                WebQueryParam queryFetcher = new WebQueryParam(true,false);
                string retJson = "";
                int friendCount = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("friendCount", "0"));
                int recommendCount = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("recommendCount", "0"));

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

                        if (recommendCount > 0)
                        {
                            List<Friends> recommendfriendList = FriendManager.GetRequestFriendsList(ref tb, AID, Friend_Define.FriendList_DBName, true).Values.ToList();
                            if ((recommendCount - recommendfriendList.Count) > 0)
                            {
                                //string setQuery = string.Format("Select top {3} AID as friendaid, 0 as keysendremaintime, UserName as friendname, datediff(ss,'1970-01-01',LastConnTime) as friendlastconntime From {0} Where AID NOT IN (Select friendaid From {1} Where myaid={2}) And AID <> {2} ORDER BY NEWID()", Friend_Define.RecommendFriendList_TableName, Friend_Define.FriendList_TableName, AID, (recommendCount - recommendfriendList.Count));
                                string setQuery = string.Format(@"
                                                SELECT
                                                    TOP {4}
	                                                AID as friendaid, UserName as friendname, datediff(ss,'1970-01-01',LastConnTime) as friendlastconntime, EquipCID 
	                                                , ISNULL((SELECT COUNT(*) FROM {1}.dbo.User_FriendsList WITH(NOLOCK) WHERE acceptfriend = 'Y' AND delflag = 'N' AND myaid = AID GROUP BY myaid), 0) as friendscount
	                                                , ISNULL((SELECT COUNT(*) FROM {1}.dbo.User_FriendsList WITH(NOLOCK) WHERE acceptfriend = 'N' AND delflag = 'N' AND myaid = AID GROUP BY myaid), 0) as friendswait
                                                  FROM {0}.dbo.Account as ACC WITH(NOLOCK, INDEX(IDX_Account_Lv))
                                                  WHERE 
                                                  LV > {2} AND LV <= {3}
                                                  ORDER BY NEWID()
                                                ", Friend_Define.AccountInfo_DBName
                                                 , Friend_Define.FriendList_DBName
                                                 //, Friend_Define.MaxFriendCount
                                                 , 1
                                                 , 90
                                                 , (recommendCount - recommendfriendList.Count)
                                                 );
                                List<Friends> friendList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<Friends>(ref tb, setQuery, Friend_Define.FriendList_DBName);
                                foreach (Friends friend in friendList)
                                {
                                    retErr = FriendManager.RequestFriend(ref tb, friend.friendaid, AID);
                                }
                                if (retErr == Result_Define.eResult.SUCCESS)
                                {
                                    FriendManager.GetRecommandFriendsList(ref tb, AID);
                                }
                            }
                        }
                        if (friendCount > 0)
                        {
                            List<Friends> recommendfriendList = FriendManager.GetRequestFriendsList(ref tb, AID, Friend_Define.FriendList_DBName, true).Values.ToList();
                            List<TheSoul.DataManager.DBClass.FriendsList> myFriend = FriendManager.GetFriendList(ref tb, AID, Friend_Define.FriendList_DBName, true);
                            int acceptCount = friendCount - myFriend.Count;
                            int needCount = friendCount - myFriend.Count - recommendfriendList.Count;
                            if (needCount > 0)
                            {
                                string setQuery = string.Format(@"
                                                SELECT
                                                    TOP {4}
	                                                AID as friendaid, UserName as friendname, datediff(ss,'1970-01-01',LastConnTime) as friendlastconntime, EquipCID 
	                                                , ISNULL((SELECT COUNT(*) FROM {1}.dbo.User_FriendsList WITH(NOLOCK) WHERE acceptfriend = 'Y' AND delflag = 'N' AND myaid = AID GROUP BY myaid), 0) as friendscount
	                                                , ISNULL((SELECT COUNT(*) FROM {1}.dbo.User_FriendsList WITH(NOLOCK) WHERE acceptfriend = 'N' AND delflag = 'N' AND myaid = AID GROUP BY myaid), 0) as friendswait
                                                  FROM {0}.dbo.Account as ACC WITH(NOLOCK, INDEX(IDX_Account_Lv))
                                                  WHERE 
                                                  LV > {2} AND LV <= {3}
                                                  ORDER BY NEWID()
                                                ", Friend_Define.AccountInfo_DBName
                                                 , Friend_Define.FriendList_DBName
                                                 //, Friend_Define.MaxFriendCount
                                                 , 1
                                                 , 90
                                                 , needCount
                                                 );
                                //string.Format("Select top {3} AID as friendaid, 0 as keysendremaintime, UserName as friendname, datediff(ss,'1970-01-01',LastConnTime) as friendlastconntime From {0} Where AID NOT IN (Select friendaid From {1} Where myaid={2}) And AID <> {2} ORDER BY NEWID()", Friend_Define.RecommendFriendList_TableName, Friend_Define.FriendList_TableName, AID, needCount);
                                List<Friends> friendList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<Friends>(ref tb, setQuery, Friend_Define.FriendList_DBName);
                                foreach (Friends friend in friendList)
                                {
                                    retErr = FriendManager.RequestFriend(ref tb, friend.friendaid, AID);
                                }
                            }

                            string setQuery2 = string.Format("Select top {2} friendaid, 0 as keysendremaintime, friendname, getdate() as friendlastconntime From {0} Where myaid={1} And acceptfriend = 'N' And delflag = 'N' ORDER BY acceptdate ASC", Friend_Define.FriendList_TableName, AID, acceptCount);
                            List<Friends> friendList2 = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<Friends>(ref tb, setQuery2, Friend_Define.FriendList_DBName);
                            foreach (Friends friend in friendList2)
                            {
                                retErr = FriendManager.AcceptFriend(ref tb, AID, friend.friendaid);
                            }
                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                FriendManager.GetFriendList(ref tb, AID, Friend_Define.FriendList_DBName, true);    
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