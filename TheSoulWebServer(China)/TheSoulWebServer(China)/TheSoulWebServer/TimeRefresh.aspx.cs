using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Json;
using TheSoulWebServer.Tools;

public partial class TimeRefresh : System.Web.UI.Page
{
    private SqlConnection DB_common = null;
    private SqlConnection DB_sharding = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        JsonObjectCollection res = new JsonObjectCollection();
        JsonTextParser parser = new JsonTextParser();
        if (Request.Params["value"] == null || Request["value"] == "")
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

                //복호화
                var DecryptString = "";
                TheSoulEncrypt.EncryptData(1, Value, "0", ref DecryptString);
                //json parsing
                JsonObject jsonObj = parser.Parse(DecryptString);
                JsonObjectCollection jparse = (JsonObjectCollection)jsonObj;

                ulong AID = System.Convert.ToUInt64(jparse["aid"].GetValue().ToString());

                string DBconString = "";
                string savePath = Request.PhysicalApplicationPath;
                TheSoulDBcon.GetInstance().GetIniFileLoad(savePath, ref DBconString);
                DB_common = new SqlConnection(DBconString);

                //파라미터 값 확인 로그
                string reqval = "aid=" + Request["aid"];
                string reqURL = System.Convert.ToString(Request.Url);

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

                if (retResultAccount == 0)
                {
                    //DB연결 정의
                    string connSharding = "";
                    connSharding = TheSoulDBcon.GetInstance().GetShardingDB(retDB_INDEX);
                    DB_sharding = new SqlConnection(connSharding);

                    //키, 티켓 동기화
                    DB_sharding.Open();
                    var friendcommand = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "TimeRefresh" };
                    var outputMyKey = new SqlParameter("@RESULTMYKEY", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var outputKeyRemainChargeSEC = new SqlParameter("@RESULTREMAINCHARGESEC", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var outputMyTicket = new SqlParameter("@RESULTMYTICKET", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var outputTicketRemainChargeSEC = new SqlParameter("@RESULTTICKETREMAINCHARGESEC", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var outputServerTime = new SqlParameter("@RESULTSERVERTIME", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var outputChallengeTicket = new SqlParameter("@RESULTMYCHALLENGETICKET", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var outputChallengeTicketRemainChargeSEC = new SqlParameter("@RESULTCHALLENGETICKETREMAINCHARGESEC", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    friendcommand.Parameters.Add("@AID", SqlDbType.NVarChar).Value = AID;
                    friendcommand.Parameters.Add(outputMyKey);
                    friendcommand.Parameters.Add(outputKeyRemainChargeSEC);
                    friendcommand.Parameters.Add(outputMyTicket);
                    friendcommand.Parameters.Add(outputTicketRemainChargeSEC);
                    friendcommand.Parameters.Add(outputServerTime);
                    friendcommand.Parameters.Add(outputChallengeTicket);
                    friendcommand.Parameters.Add(outputChallengeTicketRemainChargeSEC);
                    friendcommand.ExecuteNonQuery();
                    int retMyKey = int.Parse(outputMyKey.Value.ToString());
                    int retKeyRemainChargeSEC = int.Parse(outputKeyRemainChargeSEC.Value.ToString());
                    int retMyTicket = int.Parse(outputMyTicket.Value.ToString());
                    int retTicketRemainChargeSEC = int.Parse(outputTicketRemainChargeSEC.Value.ToString());
                    int retServerTime = int.Parse(outputServerTime.Value.ToString());
                    int retChallengeTicket = int.Parse(outputChallengeTicket.Value.ToString());
                    int retChallengeTicketRemainChargeSEC = int.Parse(outputChallengeTicketRemainChargeSEC.Value.ToString());
                    DB_sharding.Close();

                    //게릴라 던전 시간 정보
                    JsonArrayCollection MakeJson7 = new JsonArrayCollection("guerrilladungeontime");
                    DB_common.Open();
                    var command13 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_common, CommandText = "GetGuerrillaDungeonTime" };
                    command13.ExecuteNonQuery();
                    SqlDataReader reader7 = command13.ExecuteReader();
                    while (reader7.Read())
                    {
                        JsonObjectCollection items7 = new JsonObjectCollection();
                        items7.Add(new JsonNumericValue("starttime", System.Convert.ToUInt64(reader7["SDATE"])));
                        items7.Add(new JsonNumericValue("endtime", System.Convert.ToUInt64(reader7["EDATE"])));
                        MakeJson7.Add(items7);
                    }
                    DB_common.Close();

                    res.Add(new JsonNumericValue("resultcode", retResultAccount));
                    res.Add(new JsonNumericValue("key", retMyKey));
                    res.Add(new JsonNumericValue("keyremainchargesec", retKeyRemainChargeSEC));
                    res.Add(new JsonNumericValue("ticket", retMyTicket));
                    res.Add(new JsonNumericValue("ticketremainchargesec", retTicketRemainChargeSEC));
                    res.Add(new JsonNumericValue("servertime", retServerTime));
                    res.Add(new JsonNumericValue("challengeticket", retChallengeTicket));
                    res.Add(new JsonNumericValue("challengeticketremainchargesec", retChallengeTicketRemainChargeSEC));
                    res.Add(MakeJson7);

                    string json_text = res.ToString();
                    var EncryptString = "";
                    TheSoulEncrypt.EncryptData(0, json_text, "0", ref EncryptString);
                    string JsonConvertData = "";
                    JsonConvert.Convert(1, EncryptString, ref JsonConvertData);
                    Response.Write(JsonConvertData);
                    //로그 저장
                    LogManager_Old.Instance.WriteLogMessage("URL : {0}\r\nparameter : {1}\r\nencrypt_parameter : {2}\r\nresult : {3}", reqURL, reqval, Value, json_text);
                }
                else
                {
                    res.Add(new JsonNumericValue("resultcode", 40));
                    string json_text = res.ToString();
                    var EncryptString = "";
                    TheSoulEncrypt.EncryptData(0, json_text, "0", ref EncryptString);
                    string JsonConvertData = "";
                    JsonConvert.Convert(1, EncryptString, ref JsonConvertData);
                    Response.Write(JsonConvertData);
                    //로그 저장
                    LogManager_Old.Instance.WriteLogMessage("URL : {0}\r\nparameter : {1}\r\nencrypt_parameter : {2}\r\nresult : {3}", reqURL, reqval, Value, json_text);
                }
            }
            catch (Exception ex)
            {
                if (DB_common != null)
                    DB_common.Close();
                if (DB_sharding != null)
                    DB_sharding.Close();
                string reqval = "aid=" + Request["aid"] + "value=" + Request["value"];
                string reqURL = System.Convert.ToString(Request.Url);
                ErrorLogManager.Instance.WriteLogMessage("URL : {0}\r\nparameter : {1}\r\nmessage1 : {2}\r\nmessage2 : {3}", reqURL, reqval, ex.Message, ex.ToString());
                res.Add(new JsonNumericValue("resultcode", 97));
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