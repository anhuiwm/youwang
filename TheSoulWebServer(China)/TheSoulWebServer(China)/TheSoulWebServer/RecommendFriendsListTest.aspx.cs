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

public partial class RecommendFriendsListTest : System.Web.UI.Page
{
    public class friends
    {
        public uint keysendremaintime;
        public string friendusername;
        public uint friendlastconntime;
        public uint friendlv;
        public string friendyn;
        public uint classdiv;
        public string newyn;
    }

    private SqlConnection DB_common = null;
    private SqlConnection DB_sharding = null;
    private SqlConnection DB_sharding2 = null;
    private SqlConnection DB_log = null;
    private SqlConnection DB_sharding_var = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        JsonObjectCollection res = new JsonObjectCollection();
        if (Request["aid"] == null || Request["aid"] == "")
        {
            res.Add(new JsonNumericValue("resultcode", 1));
            string json_text = res.ToString();
            Response.Write(json_text);
        }
        else
        {
            try
            {
                ulong AID = System.Convert.ToUInt64(Request["aid"]);

                string DBconString = "";
                string savePath = Request.PhysicalApplicationPath;
                TheSoulDBcon.GetCommonDB(savePath, ref DBconString);
                DB_common = new SqlConnection(DBconString);

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

                //파라미터 값 확인 로그
                string reqval = "aid=" + Request["aid"];
                string reqURL = System.Convert.ToString(Request.Url);
                TheSoulWebServerErrorLog.WriteError(savePath, reqURL, reqval);

                ulong retFriendAID;
                int retFriendKeySendRemainTime;
                int retRecommendCNT;
                string retFriendUserName;
                int retFriendLastConnTime;
                int retLV;
                string retAcceptFriend;
                int retClass;

                //내가 추천한 친구 정보
                DB_sharding.Open();
                var command = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "GetMyRecommendFriend" };
                var resultFriendAID = new SqlParameter("@FRIENDAID", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                var resultKeySendRemainTime = new SqlParameter("@KEYSENDREMAINTIME", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var resultRecommendCNT = new SqlParameter("@RECOMMENDCNT", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                command.Parameters.Add(resultFriendAID);
                command.Parameters.Add(resultKeySendRemainTime);
                command.Parameters.Add(resultRecommendCNT);
                command.ExecuteNonQuery();
                retFriendAID = ulong.Parse(resultFriendAID.Value.ToString());
                retFriendKeySendRemainTime = int.Parse(resultKeySendRemainTime.Value.ToString());
                retRecommendCNT = int.Parse(resultRecommendCNT.Value.ToString());
                DB_sharding.Close();

                if (retFriendAID != 0)
                {

                    //친구 DB분산 정보 가져오기
                    DB_common.Open();
                    var friendcommand = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_common, CommandText = "GetAccountDB" };
                    var outputRetDB_INDEXF = new SqlParameter("@RetDB_INDEX", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var outputRetUserNameF = new SqlParameter("@RetUserName", SqlDbType.NVarChar, 32) { Direction = ParameterDirection.Output };
                    var outputResultAccountF = new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    friendcommand.Parameters.Add("@AID", SqlDbType.BigInt).Value = retFriendAID;
                    friendcommand.Parameters.Add(outputRetDB_INDEXF);
                    friendcommand.Parameters.Add(outputRetUserNameF);
                    friendcommand.Parameters.Add(outputResultAccountF);
                    friendcommand.ExecuteNonQuery();
                    int retDB_INDEXF = int.Parse(outputRetDB_INDEXF.Value.ToString());
                    string retUserNameF = outputRetUserNameF.Value.ToString();
                    int retResultAccountF = int.Parse(outputResultAccountF.Value.ToString());
                    DB_common.Close();

                    string connFri2 = TheSoulDBcon.GetShardingDB(retDB_INDEXF);
                    DB_sharding2 = new SqlConnection(connFri2);

                    //내가 추천한 친구 정보
                    DB_sharding2.Open();
                    var command3 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding2, CommandText = "GetMyRecommendFriendInfo" };
                    var resultUserName = new SqlParameter("@USERNAME", SqlDbType.NVarChar, 32) { Direction = ParameterDirection.Output };
                    var resultLastConnTime = new SqlParameter("@LASTCONNTIME", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var resultLV = new SqlParameter("@LV", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var resultAcceptFriend = new SqlParameter("@AcceptFriend", SqlDbType.Char, 1) { Direction = ParameterDirection.Output };
                    var resultClass = new SqlParameter("@equipclass", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    command3.Parameters.Add("@AID", SqlDbType.BigInt).Value = retFriendAID;
                    command3.Parameters.Add("@FAID", SqlDbType.BigInt).Value = AID;
                    command3.Parameters.Add(resultUserName);
                    command3.Parameters.Add(resultLastConnTime);
                    command3.Parameters.Add(resultLV);
                    command3.Parameters.Add(resultAcceptFriend);
                    command3.Parameters.Add(resultClass);
                    command3.ExecuteNonQuery();
                    retFriendUserName = resultUserName.Value.ToString();
                    retFriendLastConnTime = int.Parse(resultLastConnTime.Value.ToString());
                    retLV = int.Parse(resultLV.Value.ToString());
                    retAcceptFriend = resultAcceptFriend.Value.ToString();
                    retClass = int.Parse(resultClass.Value.ToString());
                    DB_sharding2.Close();
                }
                else
                {
                    retFriendAID = 0;
                    retFriendKeySendRemainTime = 0;
                    retRecommendCNT = 0;
                    retFriendUserName = "";
                    retFriendLastConnTime = 0;
                    retLV = 0;
                    retAcceptFriend = "N";
                    retClass = 0;
                }

                //친구 배열
                Dictionary<long, friends> friendslist = new Dictionary<long, friends>();
                //나의 친구 리스트 가져오기(콤마로 구분 배열)
                int cnt = 0;
                string FriendList = "";
                //샤딩 DB 만큼 루프 돌리기
                DB_common.Open();
                var indexdblist = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_common, CommandText = "GetDBList" };
                SqlDataReader reader2 = indexdblist.ExecuteReader();
                while (reader2.Read())
                {
                    //샤딩 DB 만큼 루프 돌리기
                    int DB_INDEX = System.Convert.ToInt32(reader2["DB_INDEX"]);
                    string connFrivar = TheSoulDBcon.GetShardingDB(DB_INDEX);
                    DB_sharding_var = new SqlConnection(connFrivar);

                    //각 서버 추천 친구 리스트에서 검색
                    DB_sharding_var.Open();
                    var command4 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding_var, CommandText = "GetMeRecommendFriendsList" };
                    command4.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                    SqlDataReader reader = command4.ExecuteReader();
                    while (reader.Read())
                    {
                        friends setfriends = new friends();
                        setfriends.friendusername = System.Convert.ToString(reader["UserName"]);
                        setfriends.friendlastconntime = System.Convert.ToUInt32(reader["LastConnTime"]);
                        setfriends.friendlv = System.Convert.ToUInt32(reader["LV"]);
                        setfriends.friendyn = System.Convert.ToString(reader["acceptfriend"]);
                        setfriends.classdiv = System.Convert.ToUInt32(reader["equipclass"]);
                        setfriends.newyn = System.Convert.ToString(reader["newyn"]);
                        friendslist.Add(System.Convert.ToInt64(reader["myaid"]), setfriends);
                        if (cnt == 0)
                        {
                            FriendList = FriendList + System.Convert.ToUInt64(reader["myaid"]);
                        }
                        else
                        {
                            if (FriendList == "")
                                FriendList = FriendList + System.Convert.ToUInt64(reader["myaid"]);
                            else
                                FriendList = FriendList + "," + System.Convert.ToUInt64(reader["myaid"]);
                        }
                        cnt = cnt + 1;
                    }
                    DB_sharding_var.Close();
                }
                DB_common.Close();

                string[] flist;
                if (FriendList != "")
                {
                    //나를 추천한 친구 리스트
                    DB_sharding.Open();
                    var command2 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "GetMYFriendsKeyInfo" };
                    command2.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                    command2.Parameters.Add("@FRIENDSLIST", SqlDbType.NVarChar).Value = FriendList;
                    SqlDataReader reader3 = command2.ExecuteReader();
                    while (reader3.Read())
                    {
                        if (friendslist.ContainsKey(System.Convert.ToInt64(reader3["friendaid"])) == true)
                        {
                            //친구 개별 DB 정보 추가
                            friends setfriends = friendslist[System.Convert.ToInt64(reader3["friendaid"])];
                            setfriends.keysendremaintime = System.Convert.ToUInt32(reader3["keysendremaintime"]);
                        }
                    }
                    DB_sharding.Close();
                    //친구 리스트 확인
                    int cnt2 = 0;
                    flist = new string[cnt];
                    DB_common.Open();
                    var command5 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_common, CommandText = "GetFriendsList" };
                    command5.Parameters.Add("@FRIENDSLIST", SqlDbType.NVarChar).Value = FriendList;
                    SqlDataReader reader4 = command5.ExecuteReader();
                    while (reader4.Read())
                    {
                        flist[cnt2] = System.Convert.ToString(reader4["AID"]);
                        cnt2 = cnt2 + 1;
                    }
                    DB_common.Close();
                }
                else
                {
                    flist = new string[0];
                }
                JsonArrayCollection MakeJson = new JsonArrayCollection("myrecommendfriendslist");
                foreach (string keyval in flist)
                {
                    JsonObjectCollection items = new JsonObjectCollection();
                    items.Add(new JsonNumericValue("friendaid", System.Convert.ToInt32(keyval)));
                    items.Add(new JsonNumericValue("keysendremaintime", friendslist[System.Convert.ToInt32(keyval)].keysendremaintime));
                    items.Add(new JsonStringValue("friendusername", System.Convert.ToString(friendslist[System.Convert.ToInt32(keyval)].friendusername)));
                    items.Add(new JsonNumericValue("friendlastconntime", friendslist[System.Convert.ToInt32(keyval)].friendlastconntime));
                    items.Add(new JsonNumericValue("friendlv", friendslist[System.Convert.ToInt32(keyval)].friendlv));
                    items.Add(new JsonStringValue("friendyn", friendslist[System.Convert.ToInt32(keyval)].friendyn));
                    items.Add(new JsonNumericValue("class", friendslist[System.Convert.ToInt32(keyval)].classdiv));
                    items.Add(new JsonStringValue("newyn", friendslist[System.Convert.ToInt32(keyval)].newyn));
                    MakeJson.Add(items);
                }

                //추천 보상 리워드 리스트
                JsonArrayCollection MakeJson2 = new JsonArrayCollection("rewardlist");
                DB_sharding.Open();
                var command6 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "GetMyRecommendReward" };
                command6.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                SqlDataReader reader5 = command6.ExecuteReader();
                while (reader5.Read())
                {
                    JsonObjectCollection items2 = new JsonObjectCollection();
                    items2.Add(new JsonNumericValue("rewardnum", System.Convert.ToInt32(reader5["rewardnum"])));
                    MakeJson2.Add(items2);
                }
                DB_sharding.Close();

                //새로운 친구 요청 OR 추천 친구이 있는지 확인 
                DB_sharding.Open();
                var commandF = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "GetFriendNeWCheckRecommend" };
                var outputRecomCNT = new SqlParameter("@RESULTRECOMCNT", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var outputReqCNT = new SqlParameter("@RESULTREQCNT", SqlDbType.Int) { Direction = ParameterDirection.Output };
                commandF.Parameters.Add("@aid", SqlDbType.BigInt).Value = AID;
                commandF.Parameters.Add(outputRecomCNT);
                commandF.Parameters.Add(outputReqCNT);
                commandF.ExecuteNonQuery();
                int retRecomCNT = int.Parse(outputRecomCNT.Value.ToString());
                int retReqCNT = int.Parse(outputReqCNT.Value.ToString());
                DB_sharding.Close();
                string RecommendYN;
                string RequestYN;
                if (retRecomCNT > 0)
                {
                    RecommendYN = "Y";
                }
                else
                {
                    RecommendYN = "N";
                }

                if (retReqCNT > 0)
                {
                    RequestYN = "Y";
                }
                else
                {
                    RequestYN = "N";
                }

                res.Add(new JsonNumericValue("resultcode", 0));
                res.Add(new JsonNumericValue("myrecommentcount", retRecommendCNT));
                res.Add(new JsonNumericValue("friendaid", retFriendAID));
                res.Add(new JsonNumericValue("keysendremaintime", retFriendKeySendRemainTime));
                res.Add(new JsonStringValue("friendusername", retFriendUserName));
                res.Add(new JsonNumericValue("friendlastconntime", retFriendLastConnTime));
                res.Add(new JsonNumericValue("friendlv", retLV));
                res.Add(new JsonStringValue("friendyn", retAcceptFriend));
                res.Add(new JsonNumericValue("class", retClass));
                res.Add(new JsonStringValue("recommendyn", RecommendYN));
                res.Add(new JsonStringValue("requestyn", RequestYN));
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
                if (DB_sharding2 != null)
                    DB_sharding2.Close();
                if (DB_log != null)
                    DB_log.Close();
                if (DB_sharding_var != null)
                    DB_sharding_var.Close();

                string reqval = "aid=" + Request["aid"];
                string reqURL = System.Convert.ToString(Request.Url);
                string savePath2 = Request.PhysicalApplicationPath;
                TheSoulWebServerErrorLog2.WriteError(savePath2, ex.Message, reqURL, reqval);
                res.Add(new JsonNumericValue("resultcode", 97));
                res.Add(new JsonStringValue("message", ex.Message));
                res.Add(new JsonStringValue("message", ex.ToString()));
                string json_text = res.ToString();
                Response.Write(json_text);
            }
        }
    }
}