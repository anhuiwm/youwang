using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Json;
using System.Net;
using TheSoulWebServer.Tools;

public partial class NoticeInfo : System.Web.UI.Page
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

                //파라미터 값 확인 로그
                string reqval = "aid=" + Request["aid"];
                string reqURL = System.Convert.ToString(Request.Url);

                string DBconString = "";
                string savePath = Request.PhysicalApplicationPath;
                TheSoulDBcon.GetInstance().GetIniFileLoad(savePath, ref DBconString);
                DB_common = new SqlConnection(DBconString);

                //DB분산 정보 가져오기
                DB_common.Open();
                var indexcommand = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_common, CommandText = "GetAccountDBSearch" };
                var outputDBSNO = new SqlParameter("@RetSNO", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                var outputDB_INDEX = new SqlParameter("@RetDB_INDEX", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var outputRetUserName = new SqlParameter("@USERNAME", SqlDbType.NVarChar, 32) { Direction = ParameterDirection.Output };
                var outputResultAccount = new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                indexcommand.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                indexcommand.Parameters.Add(outputDBSNO);
                indexcommand.Parameters.Add(outputDB_INDEX);
                indexcommand.Parameters.Add(outputRetUserName);
                indexcommand.Parameters.Add(outputResultAccount);
                indexcommand.ExecuteNonQuery();
                ulong retSNO = System.Convert.ToUInt64(outputDBSNO.Value.ToString());
                int retDB_INDEX = int.Parse(outputDB_INDEX.Value.ToString());
                string retMyName = outputRetUserName.Value.ToString();
                int retResultAccount = int.Parse(outputResultAccount.Value.ToString());
                DB_common.Close();

                //DB연결 정의
                string connSharding = "";
                uint LanguageCode = 0;
                connSharding = TheSoulDBcon.GetInstance().GetShardingDB(retDB_INDEX);
                DB_sharding = new SqlConnection(connSharding);
                if (AID == 0)
                {
                    LanguageCode = 1;
                }
                else
                {
                    DB_sharding.Open();
                    var commandAccount = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "GetNoticeLanguage" };
                    var outputLanguageCode = new SqlParameter("@LanguageCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    commandAccount.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                    commandAccount.Parameters.Add(outputLanguageCode);
                    commandAccount.ExecuteNonQuery();
                    LanguageCode = System.Convert.ToUInt32(outputLanguageCode.Value.ToString());
                    DB_sharding.Close();
                }
                JsonArrayCollection MakeJson = new JsonArrayCollection("notices");
                DB_common.Open();
                var command = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_common, CommandText = "NoticeList" };
                command.Parameters.Add("@LanguageCode", SqlDbType.Int).Value = LanguageCode;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JsonObjectCollection items = new JsonObjectCollection();
                    items.Add(new JsonNumericValue("id", System.Convert.ToUInt64(reader["orderNum"])));
                    items.Add(new JsonStringValue("image", System.Convert.ToString(reader["imgUrl"])));
                    items.Add(new JsonStringValue("link", System.Convert.ToString(reader["linkUrl"])));
                    MakeJson.Add(items);
                }
                DB_common.Close();

                res.Add(MakeJson);
                string json_text = res.ToString();
                var EncryptString = "";
                TheSoulEncrypt.EncryptData(0, json_text, "0", ref EncryptString);
                string JsonConvertData = "";
                JsonConvert.Convert(1, EncryptString, ref JsonConvertData);
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
                string reqval = "aid=" + Request["aid"] + "Value=" + Request.Params["value"];
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