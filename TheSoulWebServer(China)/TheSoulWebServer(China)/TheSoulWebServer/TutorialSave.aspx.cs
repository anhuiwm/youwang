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
using TheSoulWebServer.Tools;

public partial class TutorialSave : System.Web.UI.Page
{
    private SqlConnection DB_common = null;
    private SqlConnection DB_sharding = null;
    private SqlConnection DB_log = null;

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
                int TutorialNUM = System.Convert.ToUInt16(jparse["tutorialnum"].GetValue().ToString());
                string SkipYN = System.Convert.ToString(jparse["skipyn"].GetValue().ToString());
                if (SkipYN == "" || SkipYN == null) { SkipYN = "N"; }

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
                connSharding = TheSoulDBcon.GetInstance().GetShardingDB(retDB_INDEX);
                DB_sharding = new SqlConnection(connSharding);

                string connLog = "";
                connLog = TheSoulDBcon.GetInstance().GetLogDB(retDB_INDEX);
                DB_log = new SqlConnection(connLog);

                //파라미터 값 확인 로그
                string reqval = "aid=" + AID + "&tutorialnum=" + TutorialNUM + "&skipyn=" + SkipYN;
                string reqURL = System.Convert.ToString(Request.Url);

                //PVE 시작 관련
                DB_sharding.Open();
                var command = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "TutorialSave" };
                var outputResult = new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var outputCID = new SqlParameter("@CID", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                var outputCHARLV = new SqlParameter("@CHARLV", SqlDbType.Int) { Direction = ParameterDirection.Output };               
                var outputSOULSEQ = new SqlParameter("@SOULSEQ", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                var outputSOULID = new SqlParameter("@SOULID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var outputGRADE = new SqlParameter("@GRADE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var outputLEVEL = new SqlParameter("@LEVEL", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var outputCLASS = new SqlParameter("@CLASS", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var outputSPECIALBUFF1 = new SqlParameter("@SPECIALBUFF1", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var outputSPECIALBUFF2 = new SqlParameter("@SPECIALBUFF2", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var outputUNIQUELBUFF = new SqlParameter("@UNIQUELBUFF", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var outputCreateTime = new SqlParameter("@CreateTime", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var outputActive_Or_Passive = new SqlParameter("@Active_Or_Passive", SqlDbType.NVarChar, 32) { Direction = ParameterDirection.Output };
                command.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                command.Parameters.Add("@TUTORIALNUM", SqlDbType.TinyInt).Value = TutorialNUM;
                command.Parameters.Add("@SKIPYN", SqlDbType.Char, 1).Value = SkipYN;
                command.Parameters.Add(outputResult);
                command.Parameters.Add(outputSOULSEQ);
                command.Parameters.Add(outputSOULID);
                command.Parameters.Add(outputCID);
                command.Parameters.Add(outputCHARLV);
                command.Parameters.Add(outputGRADE);
                command.Parameters.Add(outputLEVEL);
                command.Parameters.Add(outputCLASS);
                command.Parameters.Add(outputSPECIALBUFF1);
                command.Parameters.Add(outputSPECIALBUFF2);
                command.Parameters.Add(outputUNIQUELBUFF);
                command.Parameters.Add(outputCreateTime);
                command.Parameters.Add(outputActive_Or_Passive);
                command.ExecuteNonQuery();
                int retResult = int.Parse(outputResult.Value.ToString());
                ulong retSOULSEQ = System.Convert.ToUInt64(outputSOULSEQ.Value.ToString());
                int retSOULID = int.Parse(outputSOULID.Value.ToString());
                ulong retCID = System.Convert.ToUInt64(outputCID.Value.ToString());
                int retLEVEL = int.Parse(outputCHARLV.Value.ToString());
                int retGRADE = int.Parse(outputGRADE.Value.ToString());
                int retCHARLV = int.Parse(outputLEVEL.Value.ToString());
                int retCLASS = int.Parse(outputCLASS.Value.ToString());
                int retSPECIALBUFF1 = int.Parse(outputSPECIALBUFF1.Value.ToString());
                int retSPECIALBUFF2 = int.Parse(outputSPECIALBUFF2.Value.ToString());
                int retUNIQUELBUFF = int.Parse(outputUNIQUELBUFF.Value.ToString());
                int retCreateTime = int.Parse(outputCLASS.Value.ToString());
                string retActive_Or_Passive = outputActive_Or_Passive.Value.ToString();
                DB_sharding.Close();

                res.Add(new JsonNumericValue("resultcode", retResult));
                res.Add(new JsonNumericValue("soulseq", retSOULSEQ));
                res.Add(new JsonNumericValue("soulid", retSOULID));
                res.Add(new JsonNumericValue("grade", retGRADE));
                res.Add(new JsonNumericValue("level", retLEVEL));
                res.Add(new JsonNumericValue("class", retCLASS));
                res.Add(new JsonNumericValue("specialbuff1", retSPECIALBUFF1));
                res.Add(new JsonNumericValue("specialbuff2", retSPECIALBUFF2));
                res.Add(new JsonNumericValue("uniquebuff", retUNIQUELBUFF));
                res.Add(new JsonNumericValue("createtime", retCreateTime));
                res.Add(new JsonStringValue("activeorpassive", retActive_Or_Passive));

                if (retSOULSEQ != 0)
                {
                    InsertSoulAcquireLog(retDB_INDEX, retSNO, AID, retCID, retCLASS, retCHARLV, retSOULSEQ,
                    retSOULID, retGRADE, retLEVEL, retActive_Or_Passive);
                }

                string json_text = res.ToString();
                var EncryptString = "";
                string JsonConvertData = "";
                TheSoulEncrypt.EncryptData(0, json_text, "0", ref EncryptString);
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

    public void InsertSoulAcquireLog(int retDB_INDEX, ulong SNO, ulong AID, ulong CID, int CLASS,
                                    int LEVEL, ulong SOULSEQ, int SOULID, int GRADE, int SOULLV, string retActive_Or_Passive)
    {
        string connLog = "";
        connLog = TheSoulDBcon.GetInstance().GetLogDB(retDB_INDEX);
        DB_log = new SqlConnection(connLog);

        DB_log.Open();

        var command20 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_log, CommandText = "InsertSoulAcquireLog" };
        command20.Parameters.Add("@SNO", SqlDbType.BigInt).Value = SNO;
        command20.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
        command20.Parameters.Add("@cid", SqlDbType.BigInt).Value = CID;
        command20.Parameters.Add("@Class", SqlDbType.Int).Value = CLASS;
        command20.Parameters.Add("@Level", SqlDbType.Int).Value = LEVEL; //캐릭터 살 때 레벨은 무조건 1 
        command20.Parameters.Add("@soulseq", SqlDbType.BigInt).Value = SOULSEQ;
        command20.Parameters.Add("@soulid", SqlDbType.BigInt).Value = SOULID;
        command20.Parameters.Add("@grade", SqlDbType.Int).Value = GRADE;
        command20.Parameters.Add("@soullevel", SqlDbType.Int).Value = SOULLV;
        command20.Parameters.Add("@special_buff1", SqlDbType.Int).Value = 0;
        command20.Parameters.Add("@special_buff2", SqlDbType.Int).Value = 0;
        command20.Parameters.Add("@Active_Or_Passive", SqlDbType.NVarChar, 8).Value = retActive_Or_Passive;
        command20.Parameters.Add("@route", SqlDbType.Int).Value = 9;
        command20.Parameters.Add("@subRoute", SqlDbType.Int).Value = 3;
        command20.Parameters.Add("@DescRoute", SqlDbType.BigInt).Value = SOULSEQ;

        command20.ExecuteNonQuery();

        DB_log.Close();

    }
}