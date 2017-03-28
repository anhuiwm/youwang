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
using TheSoulWebServer.Tools;

public partial class FacebookFriendsRequestKeyReject : System.Web.UI.Page
{
    private SqlConnection DB_common = null;
    private SqlConnection DB_sharding = null;
    private SqlConnection DB_sharding_var = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        JsonObjectCollection res = new JsonObjectCollection();
        JsonTextParser parser = new JsonTextParser();
        if (Request["aid"] == null || Request["aid"] == "" || Request.Params["value"] == null || Request["value"] == "")
        {
            res.Add(new JsonNumericValue("resultcode", 1));
            string json_text = res.ToString();
            var EncryptString = "";
            TheSoulEncrypt.EncryptData(0, json_text, "0", ref EncryptString);
            string JsonConvertData = "";
            JsonConvert.Convert(1, EncryptString, ref JsonConvertData);
            Response.Write(JsonConvertData);
        }
        else
        {
            try
            {
                string Value = System.Convert.ToString(Request.Params["value"]);
                ulong AID = System.Convert.ToUInt64(Request.Params["aid"]);

                string DBconString = "";
                string savePath = Request.PhysicalApplicationPath;
                TheSoulDBcon.GetInstance().GetIniFileLoad(savePath, ref DBconString);
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
                connSharding = TheSoulDBcon.GetInstance().GetShardingDB(retDB_INDEX);
                DB_sharding = new SqlConnection(connSharding);

                //업데이트 시간 가져오기
                DB_sharding.Open();
                var commandUpdateTime = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "UpdateMyTime" };
                var outputMyUpdateTime = new SqlParameter("@RESULTUPDATETIME", SqlDbType.Int) { Direction = ParameterDirection.Output };
                commandUpdateTime.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                commandUpdateTime.Parameters.Add(outputMyUpdateTime);
                commandUpdateTime.ExecuteNonQuery();
                string retMyUpdateTime = System.Convert.ToString(outputMyUpdateTime.Value.ToString());
                DB_sharding.Close();

                //복호화
                var DecryptString = "";
                TheSoulEncrypt.EncryptData(1, Value, retMyUpdateTime, ref DecryptString);
                //json parsing
                JsonObject jsonObj = parser.Parse(DecryptString);
                JsonObjectCollection jparse = (JsonObjectCollection)jsonObj;
                JsonArrayCollection gamefriends = (JsonArrayCollection)jparse["gamefriends"];

                //파라미터 값 확인 로그
                string reqval = "aid=" + Request["aid"];
                string reqURL = System.Convert.ToString(Request.Url);
                //TheSoulWebServerErrorLog.WriteError(savePath, reqURL, reqval);

                //페이스북 게임 친구 추출
                foreach (JsonObject item in gamefriends)
                {
                    JsonObjectCollection GameFriend = (JsonObjectCollection)item;
                    //친구 키 요청 보낸 시간 갱신
                    DB_sharding.Open();
                    var command2 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "FacebookFriendsRequestKeyReject" };
                    command2.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                    command2.Parameters.Add("@FNO", SqlDbType.BigInt).Value = System.Convert.ToInt64(GameFriend["fno"].GetValue().ToString());
                    command2.ExecuteNonQuery();
                    DB_sharding.Close();
                }

                //친구 KEY 요청 리스트 검색
                JsonArrayCollection MakeJson = new JsonArrayCollection("reqkeyfriends");
                DB_sharding.Open();
                var command4 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "GetFacebookFriendsRequestKeyTime" };
                command4.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                SqlDataReader reader5 = command4.ExecuteReader();
                while (reader5.Read())
                {
                    JsonObjectCollection reqkey = new JsonObjectCollection();
                    reqkey.Add(new JsonStringValue("fno", System.Convert.ToString(reader5["friendfno"])));
                    if (System.Convert.ToInt32(reader5["requestkeytime"]) < 1)
                    {
                        reqkey.Add(new JsonNumericValue("requestkeytime", System.Convert.ToInt32(reader5["requestkeytime"])));
                    }
                    else
                    {
                        reqkey.Add(new JsonNumericValue("requestkeytime", System.Convert.ToInt32(reader5["requestkeytime"]) + 86400));
                    }

                    MakeJson.Add(reqkey);
                }
                DB_sharding.Close();


                res.Add(new JsonNumericValue("resultcode", 0));
                res.Add(MakeJson);
                //업데이트 시간 변경
                DB_sharding.Open();
                var commandUpdateCheck = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "UpdateCheck" };
                var outputUpdateTime = new SqlParameter("@RESULTUPDATETIME", SqlDbType.Int) { Direction = ParameterDirection.Output };
                commandUpdateCheck.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                commandUpdateCheck.Parameters.Add(outputUpdateTime);
                commandUpdateCheck.ExecuteNonQuery();
                uint retUpdateTime = System.Convert.ToUInt32(outputUpdateTime.Value.ToString());
                DB_sharding.Close();
                res.Add(new JsonNumericValue("updatetime", retUpdateTime));

                string json_text = res.ToString();
                var EncryptString = "";
                TheSoulEncrypt.EncryptData(0, json_text, retMyUpdateTime, ref EncryptString);
                string JsonConvertData = "";
                JsonConvert.Convert(0, EncryptString, ref JsonConvertData);
                Response.Write(JsonConvertData);
                //로그 저장
                LogManager_Old.Instance.WriteLogMessage("URL : {0}\r\nparameter : {1}\r\nencrypt_parameter : {2}\r\nresult : {3}", reqURL, reqval, Value, json_text);
            }
            catch (Exception ex)
            {
                if (DB_common != null)
                    DB_common.Close();
                if (DB_sharding != null)
                    DB_sharding.Close();
                if (DB_sharding_var != null)
                    DB_sharding_var.Close();

                string reqval = "aid=" + Request["aid"] + "value=" + Request["value"];
                string reqURL = System.Convert.ToString(Request.Url);
                ErrorLogManager.Instance.WriteLogMessage("URL : {0}\r\nparameter : {1}\r\nmessage1 : {2}\r\nmessage2 : {3}", reqURL, reqval, ex.Message, ex.ToString());
                int errornum = 97;
                if (ex.HResult == -2146233033 || ex.HResult == -2146233296) { errornum = 200; }
                res.Add(new JsonNumericValue("resultcode", errornum));
                res.Add(new JsonNumericValue("message1", ex.HResult));
                res.Add(new JsonStringValue("message", ex.Message));
                res.Add(new JsonStringValue("message", ex.ToString()));
                string json_text = res.ToString();
                var EncryptString = "";
                TheSoulEncrypt.EncryptData(0, json_text, "0", ref EncryptString);
                string JsonConvertData = "";
                JsonConvert.Convert(1, EncryptString, ref JsonConvertData);
                Response.Write(JsonConvertData);
            }
        }
    }
}