using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Json;

public partial class test : System.Web.UI.Page
{
    public class fbgamefriends
    {
        public ulong friendaid;
        public uint lv;
        public uint keysendremaintime;
    }
    public class fbfriends
    {
        public uint invitetime;
    }

    private SqlConnection DB_common = null;
    private SqlConnection DB_sharding = null;
    private SqlConnection DB_log = null;
    private SqlConnection DB_sharding_var = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        JsonObjectCollection res = new JsonObjectCollection();
        JsonTextParser parser = new JsonTextParser();
        if (Request.Params["value"] == null || Request["value"] == "")
        {
            res.Add(new JsonNumericValue("resultcode", 1));
            string json_text = res.ToString();
            Response.Write(json_text);
        }
        else
        {
            try
            {
                string Value = System.Convert.ToString(Request.Params["value"]);

                string DBconString = "";
                string savePath = Request.PhysicalApplicationPath;
                TheSoulDBcon.GetCommonDB(savePath, ref DBconString);
                DB_common = new SqlConnection(DBconString);

                //json parsing
                JsonObject jsonObj = parser.Parse(Value);
                JsonObjectCollection jparse = (JsonObjectCollection)jsonObj;
                JsonArrayCollection gamefriends = (JsonArrayCollection)jparse["gamefriends"];
                JsonArrayCollection friends = (JsonArrayCollection)jparse["facebookfriends"];

                ulong AID = System.Convert.ToUInt64(jparse["aid"].GetValue().ToString());
                ulong FNO = System.Convert.ToUInt64(jparse["fno"].GetValue().ToString());

                //파라미터 값 확인 로그
                string reqval = "aid=" + Request["aid"] + "&myFNO=" + FNO;
                string reqURL = System.Convert.ToString(Request.Url);
                TheSoulWebServerErrorLog.WriteError(savePath, reqURL, reqval);

                //DB분산 정보 가져오기
                DB_common.Open();
                var indexcommand = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_common, CommandText = "GetAccountDB" };
                var outputDB_INDEX = new SqlParameter("@RetDB_INDEX", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var outputRetUserName = new SqlParameter("@RetUserName", SqlDbType.NVarChar, 32) { Direction = ParameterDirection.Output };
                var outputResultAccount = new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                indexcommand.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                indexcommand.Parameters.Add(outputDB_INDEX);
                indexcommand.Parameters.Add(outputRetUserName);
                indexcommand.Parameters.Add(outputResultAccount);
                indexcommand.ExecuteNonQuery();
                int retDB_INDEX = int.Parse(outputDB_INDEX.Value.ToString());
                string retMyName = outputRetUserName.Value.ToString();
                int retResultAccount = int.Parse(outputResultAccount.Value.ToString());
                DB_common.Close();

                //DB연결 정의
                string connSharding = "";
                connSharding = TheSoulDBcon.GetShardingDB(retDB_INDEX);
                DB_sharding = new SqlConnection(connSharding);

                string connLog = "";
                connLog = TheSoulDBcon.GetLogDB(retDB_INDEX);
                DB_log = new SqlConnection(connLog);



                //페이스북 계정 연동 체크
                DB_sharding.Open();
                var commandfb = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "CheckFacebookUserinfo" };
                var outputFBResult = new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                commandfb.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                commandfb.Parameters.Add("@FNO", SqlDbType.BigInt).Value = FNO;
                commandfb.Parameters.Add(outputFBResult);
                commandfb.ExecuteNonQuery();
                string retFBresult = System.Convert.ToString(outputFBResult.Value.ToString());
                DB_sharding.Close();

                //페이스북 게임 친구 추출
                int cnt = 0;
                string GameFriendList = "";
                string[] flist = new string[gamefriends.Count];
                //게임 친구 리스트 입력
                Dictionary<long, fbgamefriends> fbgamefriendslist = new Dictionary<long, fbgamefriends>();
                foreach (JsonObject item in gamefriends)
                {
                    JsonObjectCollection GameFriend = (JsonObjectCollection)item;
                    if (GameFriendList == "")
                    {
                        GameFriendList = GameFriend["fno"].GetValue().ToString();
                    }
                    else
                    {
                        GameFriendList = GameFriendList + "," + GameFriend["fno"].GetValue().ToString();
                    }
                }

                if (gamefriends.Count > 0)
                {
                    //DB분산 정보 가져오기
                    DB_common.Open();
                    var indexdblist = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_common, CommandText = "GetDBList" };
                    SqlDataReader reader = indexdblist.ExecuteReader();
                    while (reader.Read())
                    {
                        //샤딩 DB 만큼 루프 돌리기
                        int DB_INDEX = System.Convert.ToInt32(reader["DB_INDEX"]);
                        string connFrivar = TheSoulDBcon.GetShardingDB(DB_INDEX);
                        DB_sharding_var = new SqlConnection(connFrivar);

                        //친구 리스트 계정 AID,LV 검색
                        DB_sharding_var.Open();
                        var command = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding_var, CommandText = "GetFacebookFriendsList" };
                        command.Parameters.Add("@FRIENDSLIST", SqlDbType.NVarChar).Value = GameFriendList;
                        SqlDataReader reader2 = command.ExecuteReader();
                        while (reader2.Read())
                        {
                            fbgamefriends setgamefriends = new fbgamefriends();
                            setgamefriends.friendaid = System.Convert.ToUInt64(reader2["AID"]);
                            setgamefriends.lv = System.Convert.ToUInt32(reader2["LV"]);
                            fbgamefriendslist.Add(System.Convert.ToInt64(reader2["FNO"]), setgamefriends);
                            flist[cnt] = System.Convert.ToString(reader2["FNO"]);
                            cnt = cnt + 1;
                        }
                        DB_sharding_var.Close();

                        //친구 KEY 발송 검색
                        DB_sharding_var.Open();
                        var command2 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding_var, CommandText = "GetFacebookFriendsKeySendTime" };
                        command2.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                        command2.Parameters.Add("@FRIENDSLIST", SqlDbType.NVarChar).Value = GameFriendList;
                        SqlDataReader reader3 = command2.ExecuteReader();
                        while (reader3.Read())
                        {
                            if (fbgamefriendslist.ContainsKey(System.Convert.ToInt64(reader3["friendfno"])) == true)
                            {
                                //친구 개별 DB 정보 추가
                                fbgamefriends setgamefriends = fbgamefriendslist[System.Convert.ToInt64(reader3["friendfno"])];
                                setgamefriends.keysendremaintime = System.Convert.ToUInt32(reader3["requestkeytime"]);
                            }
                        }
                        DB_sharding_var.Close();
                    }
                    DB_common.Close();
                }
                JsonArrayCollection MakeJson = new JsonArrayCollection("gamefriends");
                foreach (string keyval in flist)
                {
                    JsonObjectCollection gameflist = new JsonObjectCollection();
                    gameflist.Add(new JsonNumericValue("fno", System.Convert.ToUInt64(keyval)));
                    gameflist.Add(new JsonNumericValue("friendaid", fbgamefriendslist[System.Convert.ToInt64(keyval)].friendaid));
                    gameflist.Add(new JsonNumericValue("lv", fbgamefriendslist[System.Convert.ToInt64(keyval)].lv));
                    gameflist.Add(new JsonNumericValue("keysendremaintime", fbgamefriendslist[System.Convert.ToInt64(keyval)].keysendremaintime));
                    MakeJson.Add(gameflist);
                }

                //페이스북 일반 친구 리스트 추출
                int cnt2 = 0;
                string FriendList = "";
                string[] flist2 = new string[friends.Count];
                //게임 친구 리스트 입력
                Dictionary<long, fbfriends> fbfriendslist = new Dictionary<long, fbfriends>();
                foreach (JsonObject item in friends)
                {
                    JsonObjectCollection FBFriend = (JsonObjectCollection)item;
                    fbfriends setfbfriends = new fbfriends();
                    fbfriendslist.Add(System.Convert.ToInt64(FBFriend["fno"].GetValue().ToString()), setfbfriends);
                    if (FriendList == "")
                    {
                        FriendList = FBFriend["fno"].GetValue().ToString();
                    }
                    else
                    {
                        FriendList = FriendList + "," + FBFriend["fno"].GetValue().ToString();
                    }
                    flist2[cnt2] = System.Convert.ToString(FBFriend["fno"].GetValue().ToString());
                    cnt2 = cnt2 + 1;
                }

                if (friends.Count > 0)
                {
                    //DB분산 정보 가져오기
                    DB_common.Open();
                    var indexdblist = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_common, CommandText = "GetDBList" };
                    SqlDataReader reader = indexdblist.ExecuteReader();
                    while (reader.Read())
                    {
                        //샤딩 DB 만큼 루프 돌리기
                        int DB_INDEX = System.Convert.ToInt32(reader["DB_INDEX"]);
                        string connFrivar = TheSoulDBcon.GetShardingDB(DB_INDEX);
                        DB_sharding_var = new SqlConnection(connFrivar);

                        //친구 KEY 발송 검색
                        DB_sharding_var.Open();
                        var command3 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding_var, CommandText = "GetFacebookFriendsInviteTime" };
                        command3.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                        command3.Parameters.Add("@FRIENDSLIST", SqlDbType.NVarChar).Value = FriendList;
                        SqlDataReader reader4 = command3.ExecuteReader();
                        while (reader4.Read())
                        {
                            if (fbfriendslist.ContainsKey(System.Convert.ToInt64(reader4["friendfno"])) == true)
                            {
                                //친구 개별 DB 정보 추가
                                fbfriends setfbfriends = fbfriendslist[System.Convert.ToInt64(reader4["friendfno"])];
                                setfbfriends.invitetime = System.Convert.ToUInt32(reader4["invitetime"]);
                            }
                        }
                        DB_sharding_var.Close();
                    }
                    DB_common.Close();
                }
                JsonArrayCollection MakeJson2 = new JsonArrayCollection("facebookfriends");
                foreach (string keyval in flist2)
                {
                    JsonObjectCollection fblist = new JsonObjectCollection();
                    fblist.Add(new JsonNumericValue("fno", System.Convert.ToUInt64(keyval)));
                    fblist.Add(new JsonNumericValue("invitetime", fbfriendslist[System.Convert.ToInt64(keyval)].invitetime));
                    MakeJson2.Add(fblist);
                }

                res.Add(new JsonNumericValue("resultcode", 0));
                res.Add(MakeJson);
                res.Add(MakeJson2);

                string json_text = res.ToString();
                Response.Write(json_text);
            }
            catch (Exception ex)
            {
                if (DB_common != null)
                    DB_common.Close();
                if (DB_sharding != null)
                    DB_sharding.Close();
                if (DB_sharding_var != null)
                    DB_sharding_var.Close();
                if (DB_log != null)
                    DB_log.Close();

                string reqval = "aid=" + Request["aid"] + "value=" + Request["value"];
                string reqURL = System.Convert.ToString(Request.Url);
                string savePath2 = Request.PhysicalApplicationPath;
                TheSoulWebServerErrorLog2.WriteError(savePath2, "message1:" + ex.Message + "message2:" + ex.ToString(), reqURL, reqval);
                int errornum = 97;
                if (ex.HResult == -2146233033 || ex.HResult == -2146233296) { errornum = 200; }
                res.Add(new JsonNumericValue("resultcode", errornum));
                res.Add(new JsonNumericValue("message1", ex.HResult));
                res.Add(new JsonStringValue("message", ex.Message));
                res.Add(new JsonStringValue("message", ex.ToString()));
                string json_text = res.ToString();
                Response.Write(json_text);
            }
        }
    }
}