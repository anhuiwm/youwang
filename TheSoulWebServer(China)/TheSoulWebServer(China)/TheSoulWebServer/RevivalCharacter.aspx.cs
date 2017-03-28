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

public partial class RevivalCharacter : System.Web.UI.Page
{
    private SqlConnection DB_common = null;
    private SqlConnection DB_sharding = null;
    private SqlConnection DB_log = null;

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

                string connLog = "";
                connLog = TheSoulDBcon.GetInstance().GetLogDB(retDB_INDEX);
                DB_log = new SqlConnection(connLog);

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

                ulong CID = System.Convert.ToUInt64(jparse["cid"].GetValue().ToString());
                uint Mode =System.Convert.ToUInt16(jparse["mode"].GetValue().ToString());
                uint StageID = System.Convert.ToUInt32(jparse["stageid"].GetValue().ToString());

                //Response.Write(UserID);
                //파라미터 값 확인 로그
                string reqval = "aid=" + Request["aid"] + "&cid=" + CID + "&Mode=" + Mode + "&StageID=" + StageID + "&updatetime=" + retMyUpdateTime;
                string reqURL = System.Convert.ToString(Request.Url);

                //DB Data Set
                DB_sharding.Open();
                var command = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "RivivalCharacter" };
                var resultCode = new SqlParameter("@RESULT", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var level = new SqlParameter("@LEVEL", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var resultClass = new SqlParameter("@Class", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                command.Parameters.Add("@CID", SqlDbType.BigInt).Value = CID; 
                command.Parameters.Add("@MODE", SqlDbType.Int).Value = Mode; //부활장소
                command.Parameters.Add("@MAPID", SqlDbType.Int).Value = StageID; //부활장소
                command.Parameters.Add(resultCode);
                command.Parameters.Add(level);
                command.Parameters.Add(resultClass); 


                command.ExecuteNonQuery();
                int resultcode = int.Parse(resultCode.Value.ToString());
                int retClassType = int.Parse(resultClass.Value.ToString());
                int retLEVEL = int.Parse(level.Value.ToString());
                DB_sharding.Close();

                //Make Json
                res.Add(new JsonNumericValue("resultcode", resultcode));
                if (resultcode == 0)
                {
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

                    InsertRevivalLog(retDB_INDEX, AID, CID, retClassType, retLEVEL, Mode, StageID);
                }

                string json_text = res.ToString();
                var EncryptString = "";
                string JsonConvertData = "";
                if (resultcode == 0)
                {
                    TheSoulEncrypt.EncryptData(0, json_text, retMyUpdateTime, ref EncryptString);
                    JsonConvert.Convert(0, EncryptString, ref JsonConvertData);
                }
                else
                {
                    TheSoulEncrypt.EncryptData(0, json_text, "0", ref EncryptString);
                    JsonConvert.Convert(1, EncryptString, ref JsonConvertData);
                }
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
                if (DB_log != null)
                    DB_log.Close();

                string reqval = "aid=" + Request["aid"] + "value=" + Request["value"];
                string reqURL = System.Convert.ToString(Request.Url);
                ErrorLogManager.Instance.WriteLogMessage("URL : {0}\r\nparameter : {1}\r\nmessage1 : {2}\r\nmessage2 : {3}", reqURL, reqval, ex.Message, ex.ToString());
                int errornum = 97;
                if (ex.HResult == -2146233033 || ex.HResult == -2146233296) { errornum = 200; }
                res.Add(new JsonNumericValue("resultcode", errornum));
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


    public void InsertRevivalLog(int iDB_INDEX, ulong ulAid, ulong CID, int level, int ClassType, uint MODE, uint StageID)
    {
        string connLog = "";
        connLog = TheSoulDBcon.GetInstance().GetLogDB(iDB_INDEX);
        DB_log = new SqlConnection(connLog);

        DB_log.Open();

        var command = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_log, CommandText = "InsertRivivalLog" };
        command.Parameters.Add("@AID", SqlDbType.BigInt).Value = ulAid;
        command.Parameters.Add("@CID", SqlDbType.BigInt).Value = CID;
        command.Parameters.Add("@LEVEL", SqlDbType.Int).Value = level;
        command.Parameters.Add("@CLASS", SqlDbType.Int).Value = ClassType;
        command.Parameters.Add("@MODE", SqlDbType.Int).Value = MODE;
        command.Parameters.Add("@StageID", SqlDbType.Int).Value = StageID;

        command.ExecuteNonQuery();

        DB_log.Close();

    }
}
