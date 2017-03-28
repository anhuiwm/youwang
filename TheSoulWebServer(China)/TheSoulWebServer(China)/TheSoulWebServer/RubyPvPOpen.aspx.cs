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

public partial class RubyPvPOpen : System.Web.UI.Page
{
    private SqlConnection DB_common = null;
    private const bool CHECK_DEBUG = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        JsonObjectCollection res = new JsonObjectCollection();
        JsonTextParser parser = new JsonTextParser();

        if ((Request.Params["value"] == null || Request["value"] == "") && !CHECK_DEBUG)
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
                //string Value = System.Convert.ToString(Request.Params["value"]);

                //if (Value == null)
                //    Response.Write("wtf?");
                ////복호화
                //var DecryptString = "";
                //TheSoulEncrypt.EncryptData(1, Value, "0", ref DecryptString);

                ulong AID = System.Convert.ToUInt64(Request.Params["aid"]);
                string Value = System.Convert.ToString(Request.Params["value"]);

                string DBconString = "";
                string savePath = Request.PhysicalApplicationPath;
                TheSoulDBcon.GetInstance().GetIniFileLoad(savePath, ref DBconString);
                DB_common = new SqlConnection(DBconString);

                //파라미터 값 확인 로그
                string reqval = "aid=" + Request["aid"];
                string reqURL = System.Convert.ToString(Request.Url);
                //TheSoulWebServerErrorLog.WriteError(savePath, reqURL, reqval);

                //common db 에서 opentime 가져오기
                DB_common.Open();
                var dbcommand = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_common, CommandText = "RubyPVPOpenTimeCheck" };
                var outputResult = new SqlParameter("@OpenResult", SqlDbType.Int) { Direction = ParameterDirection.Output };
                dbcommand.Parameters.Add(outputResult);
                dbcommand.ExecuteNonQuery();
                   
                int OpenResult = int.Parse(outputResult.Value.ToString());
                DB_common.Close();

                OpenResult = 1;
                res.Add(new JsonNumericValue("resultcode", 0));
                res.Add(new JsonNumericValue("isopen", OpenResult));

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

                string reqval = "aid=" + Request["aid"] + "value=" + Request["value"];
                string reqURL = System.Convert.ToString(Request.Url);
                ErrorLogManager.Instance.WriteLogMessage("URL : {0}\r\nparameter : {1}\r\nmessage1 : {2}\r\nmessage2 : {3}", reqURL, reqval, ex.Message, ex.ToString());
                int errornum = 97;
                if (ex.HResult == -2146233033 || ex.HResult == -2146233296) { errornum = 200; }
                res.Add(new JsonNumericValue("resultcode", errornum));
                res.Add(new JsonNumericValue("message1", ex.HResult));
                res.Add(new JsonStringValue("message2", ex.Message));
                res.Add(new JsonStringValue("message3", ex.ToString()));
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
