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

public partial class FacebookFriendlyPointGacha : System.Web.UI.Page
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

                int CID = System.Convert.ToInt32(jparse["cid"].GetValue().ToString());

                //파라미터 값 확인 로그
                string reqval = "aid=" + Request["aid"] + "cid=" + CID;
                string reqURL = System.Convert.ToString(Request.Url);

                //친구 우편함 키 발송
                DB_sharding.Open();
                var command4 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "FacebookFriendlyPointGachaSpend" };
                var outputRsult = new SqlParameter("@RESULT", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var outputFriendlyPoint = new SqlParameter("@RESULTFRIENDLYPOINT", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command4.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                command4.Parameters.Add(outputRsult);
                command4.Parameters.Add(outputFriendlyPoint);
                command4.ExecuteNonQuery();
                uint retResult = System.Convert.ToUInt32(outputRsult.Value.ToString());
                uint retFriendlyPoint = System.Convert.ToUInt32(outputFriendlyPoint.Value.ToString());
                DB_sharding.Close();

                if (retResult == 0)
                {
                    string retItemType="ItemType_LuckyBox";
                    int retItemCode=315000001;
                    int retItemGrade = 1;
                    int retItemLevel = 0;
                    int retItemEA = 0;
                    int retInvenSEQ = 0;
                    string retMaxLevel = "Y";
                    res.Add(new JsonNumericValue("resultcode", retResult));
                    res.Add(new JsonNumericValue("friendlypoint", retFriendlyPoint));
                    res.Add(new JsonStringValue("itemtype", retItemType));
                    res.Add(new JsonNumericValue("itemcode", retItemCode));
                    res.Add(new JsonNumericValue("grade", retItemGrade));
                    res.Add(new JsonNumericValue("level", retItemLevel));
                    res.Add(new JsonNumericValue("itemea", retItemEA));
                    res.Add(new JsonNumericValue("invenseq", retInvenSEQ));
                    res.Add(new JsonStringValue("maxlevel", retMaxLevel));

                    JsonArrayCollection MakeJson2 = new JsonArrayCollection("itemlist");
                    //랜덤 박스 보상
                    DB_sharding.Open();
                    var command6 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "SETFriendlyPointBOX" };
                    command6.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                    command6.Parameters.Add("@CID", SqlDbType.BigInt).Value = CID;
                    //command6.ExecuteNonQuery();
                    SqlDataReader reader2 = command6.ExecuteReader();
                    int cnt = 0;
                    cnt = cnt + 1;
                    while (reader2.Read())
                    {
                        uint itemid = System.Convert.ToUInt32(reader2["itemid"]);
                        JsonObjectCollection items2 = new JsonObjectCollection();

                        if (System.Convert.ToString(reader2["ItemType"]) != "ItemType_GetSoulGacha")
                        {
                            int seq = 1;
                            items2.Add(new JsonNumericValue("SEQ", seq));
                            int GACHA_GROUP_ID = 0;
                            items2.Add(new JsonNumericValue("GACHA_GROUP_ID", GACHA_GROUP_ID));
                            byte ITEM_TYPE = 1;
                            items2.Add(new JsonNumericValue("ITEM_TYPE", ITEM_TYPE));
                            int ITEM_ID = System.Convert.ToInt32(reader2["itemid"]);
                            items2.Add(new JsonNumericValue("ITEM_ID", ITEM_ID));
                            int ITEM_EA = System.Convert.ToInt32(reader2["itemval"]);
                            items2.Add(new JsonNumericValue("ITEM_EA", ITEM_EA));
                            items2.Add(new JsonStringValue("ITEM_TYPE2", System.Convert.ToString(reader2["ItemType"])));
                            int ITEM_GRADE = System.Convert.ToInt32(reader2["grade"]);
                            items2.Add(new JsonNumericValue("ITEM_GRADE", ITEM_GRADE));
                            int Tier = (int)System.Convert.ToInt32(reader2["Tier"]);
                            int SOUL_ID = 0;
                            items2.Add(new JsonNumericValue("SOUL_ID", SOUL_ID));
                            int SOUL_GRADE = 0;
                            items2.Add(new JsonNumericValue("SOUL_GRADE", SOUL_GRADE));
                            int SOUL_BUFF_1 = 0;
                            items2.Add(new JsonNumericValue("SOUL_BUFF_1", SOUL_BUFF_1));
                            int SOUL_BUFF_2 = 0;
                            items2.Add(new JsonNumericValue("SOUL_BUFF_2", SOUL_BUFF_2));
                            int SOUL_UNIQUE = 0;
                            items2.Add(new JsonNumericValue("SOUL_UNIQUE", SOUL_UNIQUE));
                            string RESULTOPTIONTYPE1 = System.Convert.ToString(reader2["optiontype1"]);
                            items2.Add(new JsonStringValue("ITEM_OPTION1", RESULTOPTIONTYPE1));
                            int RESULTOPTIONVALUE1 = System.Convert.ToInt32(reader2["optionvalue1"]);
                            items2.Add(new JsonNumericValue("ITEM_VALUE1", RESULTOPTIONVALUE1));
                            string RESULTOPTIONTYPE2 = System.Convert.ToString(reader2["optiontype2"]);
                            items2.Add(new JsonStringValue("ITEM_OPTION2", RESULTOPTIONTYPE2));
                            int RESULTOPTIONVALUE2 = System.Convert.ToInt32(reader2["optionvalue2"]);
                            items2.Add(new JsonNumericValue("ITEM_VALUE2", RESULTOPTIONVALUE2));
                            string RESULTOPTIONTYPE3 = System.Convert.ToString(reader2["optiontype3"]);
                            items2.Add(new JsonStringValue("ITEM_OPTION3", RESULTOPTIONTYPE3));
                            int RESULTOPTIONVALUE3 = System.Convert.ToInt32(reader2["optionvalue3"]);
                            items2.Add(new JsonNumericValue("ITEM_VALUE3", RESULTOPTIONVALUE3));
                            string RESULTOPTIONTYPE4 = System.Convert.ToString(reader2["optiontype4"]);
                            items2.Add(new JsonStringValue("ITEM_OPTION4", RESULTOPTIONTYPE4));
                            int RESULTOPTIONVALUE4 = System.Convert.ToInt32(reader2["optionvalue4"]);
                            items2.Add(new JsonNumericValue("ITEM_VALUE4", RESULTOPTIONVALUE4));
                            string RESULTOPTIONTYPE5 = System.Convert.ToString(reader2["optiontype5"]);
                            items2.Add(new JsonStringValue("ITEM_OPTION5", RESULTOPTIONTYPE5));
                            int RESULTOPTIONVALUE5 = System.Convert.ToInt32(reader2["optionvalue5"]);
                            items2.Add(new JsonNumericValue("ITEM_VALUE5", RESULTOPTIONVALUE5));

                            ulong INVEN_SEQ = System.Convert.ToUInt64(reader2["ItemSeq"]);
                            items2.Add(new JsonNumericValue("INVEN_SEQ", INVEN_SEQ));

                            string MAXLEVEL = "Y";
                            items2.Add(new JsonStringValue("MAXLEVEL", MAXLEVEL));

                            MakeJson2.Add(items2);
                        }
                    }
                    DB_sharding.Close();
                    res.Add(MakeJson2);
                }
                else
                {
                    res.Add(new JsonNumericValue("resultcode", retResult));
                    res.Add(new JsonNumericValue("friendlypoint", retFriendlyPoint));
                }
                
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
                if (DB_log != null)
                    DB_log.Close();

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