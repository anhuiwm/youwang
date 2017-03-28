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

public partial class CouponCheck : System.Web.UI.Page
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
                int CouponResult = 1;

                string DBconString = "";
                string savePath = Request.PhysicalApplicationPath;
                TheSoulDBcon.GetInstance().GetIniFileLoad(savePath, ref DBconString);
                DB_common = new SqlConnection(DBconString);
                //DB분산 정보 가져오기`
                DB_common.Open();
                var indexcommand = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_common, CommandText = "GetAccountDB" };
                var outputRetDB_INDEX = new SqlParameter("@RetDB_INDEX", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var outputRetUserName = new SqlParameter("@RetUserName", SqlDbType.NVarChar, 32) { Direction = ParameterDirection.Output };
                var outputResultAccount = new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                indexcommand.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                indexcommand.Parameters.Add(outputRetDB_INDEX);
                indexcommand.Parameters.Add(outputRetUserName);
                indexcommand.Parameters.Add(outputResultAccount);
                indexcommand.ExecuteNonQuery();
                int retDB_INDEX = int.Parse(outputRetDB_INDEX.Value.ToString());
                string retUserName = outputRetUserName.Value.ToString();
                int retResultAccount = int.Parse(outputResultAccount.Value.ToString());
                DB_common.Close();

                if (retResultAccount == 0)
                {
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
                    JsonObject jsonObjpacket = parser.Parse(DecryptString);
                    JsonObjectCollection jparsepacket = (JsonObjectCollection)jsonObjpacket;
                
                    string CouponCode = System.Convert.ToString(jparsepacket["couponcode"].GetValue().ToString());

                    int CodeLength = System.Convert.ToInt32(CouponCode.Length.ToString());

                    //파라미터 값 확인 로그
                    string reqval = "aid=" + AID + "&CouponCode=" + CouponCode + "&retMyUpdateTime=" + retMyUpdateTime;
                    string reqURL = System.Convert.ToString(Request.Url);

                    //쿠폰 아이템 확인
                    DB_common.Open();
                    var command2 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_common, CommandText = "CouponCheck" };
                    var outputResult = new SqlParameter("@RESULT", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var outputCouponType = new SqlParameter("@RESULTCOUPONTYPE", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var outputRewardItemCode1 = new SqlParameter("@RESULTREWARDITEMCODE1", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var outputRewardItemCode2 = new SqlParameter("@RESULTREWARDITEMCODE2", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var outputRewardItemCode3 = new SqlParameter("@RESULTREWARDITEMCODE3", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var outputRewardValue1 = new SqlParameter("@RESULTREWARDVALUE1", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var outputRewardValue2 = new SqlParameter("@RESULTREWARDVALUE2", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var outputRewardValue3 = new SqlParameter("@RESULTREWARDVALUE3", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    command2.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                    command2.Parameters.Add("@COUPONCODE", SqlDbType.NVarChar, 16).Value = CouponCode;
                    command2.Parameters.Add(outputResult);
                    command2.Parameters.Add(outputCouponType);
                    command2.Parameters.Add(outputRewardItemCode1);
                    command2.Parameters.Add(outputRewardItemCode2);
                    command2.Parameters.Add(outputRewardItemCode3);
                    command2.Parameters.Add(outputRewardValue1);
                    command2.Parameters.Add(outputRewardValue2);
                    command2.Parameters.Add(outputRewardValue3);
                    command2.ExecuteNonQuery();
                    int retResult = int.Parse(outputResult.Value.ToString());
                    int retCouponType = int.Parse(outputCouponType.Value.ToString());
                    int retRewardItemCode1 = int.Parse(outputRewardItemCode1.Value.ToString());
                    int retRewardItemCode2 = int.Parse(outputRewardItemCode2.Value.ToString());
                    int retRewardItemCode3 = int.Parse(outputRewardItemCode3.Value.ToString());
                    int retRewardValue1 = int.Parse(outputRewardValue1.Value.ToString());
                    int retRewardValue2 = int.Parse(outputRewardValue2.Value.ToString());
                    int retRewardValue3 = int.Parse(outputRewardValue3.Value.ToString());
                    DB_common.Close();

                    if (retResult == 0)
                    {
                        if (retCouponType == 0)
                        {
                            //A타입 쿠폰 지급
                            if (retRewardItemCode1 != 0)
                            {
                                //쿠폰 상품 지급
                                DB_sharding.Open();
                                var command1 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "SendMail" };
                                var outputResultMail1 = new SqlParameter("@RESULT", SqlDbType.Int) { Direction = ParameterDirection.Output };
                                command1.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                                command1.Parameters.Add("@ITEMCODE", SqlDbType.BigInt).Value = retRewardItemCode1;
                                command1.Parameters.Add("@ITEMEA", SqlDbType.BigInt).Value = retRewardValue1;
                                command1.Parameters.Add("@GRADE", SqlDbType.BigInt).Value = 0;
                                command1.Parameters.Add("@TITLE", SqlDbType.NVarChar, 128).Value = "STRING_UI_MAIL_SYSTEM_016";
                                command1.Parameters.Add("@SENDAID", SqlDbType.BigInt).Value = 0;
                                command1.Parameters.Add("@SENDUSERNAME", SqlDbType.NVarChar, 32).Value = "STRING_UI_MAIL_SYSTEM_012";
                                command1.Parameters.Add("@ROUTE", SqlDbType.Int).Value = 17;
                                command1.Parameters.Add("@SUBROUTE", SqlDbType.Int).Value = retRewardItemCode1;
                                command1.Parameters.Add("@DESCROUTE", SqlDbType.BigInt).Value = 0;
                                command1.Parameters.Add("@MAILDIV", SqlDbType.TinyInt).Value = 12;
                                command1.Parameters.Add(outputResultMail1);
                                command1.ExecuteNonQuery();
                                int retResultMail1 = int.Parse(outputResultMail1.Value.ToString());
                                DB_sharding.Close();
                            }

                            if (retRewardItemCode2 != 0)
                            {
                                //쿠폰 상품 지급
                                DB_sharding.Open();
                                var command4 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "SendMail" };
                                var outputResultMail1 = new SqlParameter("@RESULT", SqlDbType.Int) { Direction = ParameterDirection.Output };
                                command4.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                                command4.Parameters.Add("@ITEMCODE", SqlDbType.BigInt).Value = retRewardItemCode2;
                                command4.Parameters.Add("@ITEMEA", SqlDbType.BigInt).Value = retRewardValue2;
                                command4.Parameters.Add("@GRADE", SqlDbType.BigInt).Value = 0;
                                command4.Parameters.Add("@TITLE", SqlDbType.NVarChar, 128).Value = "STRING_UI_MAIL_SYSTEM_016";
                                command4.Parameters.Add("@SENDAID", SqlDbType.BigInt).Value = 0;
                                command4.Parameters.Add("@SENDUSERNAME", SqlDbType.NVarChar, 32).Value = "STRING_UI_MAIL_SYSTEM_012";
                                command4.Parameters.Add("@ROUTE", SqlDbType.Int).Value = 17;
                                command4.Parameters.Add("@SUBROUTE", SqlDbType.Int).Value = retRewardItemCode2;
                                command4.Parameters.Add("@DESCROUTE", SqlDbType.BigInt).Value = 0;
                                command4.Parameters.Add("@MAILDIV", SqlDbType.TinyInt).Value = 12;
                                command4.Parameters.Add(outputResultMail1);
                                command4.ExecuteNonQuery();
                                int retResultMail1 = int.Parse(outputResultMail1.Value.ToString());
                                DB_sharding.Close();
                            }

                            if (retRewardItemCode3 != 0)
                            {
                                //쿠폰 상품 지급
                                DB_sharding.Open();
                                var command3 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "SendMail" };
                                var outputResultMail1 = new SqlParameter("@RESULT", SqlDbType.Int) { Direction = ParameterDirection.Output };
                                command3.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                                command3.Parameters.Add("@ITEMCODE", SqlDbType.BigInt).Value = retRewardItemCode3;
                                command3.Parameters.Add("@ITEMEA", SqlDbType.BigInt).Value = retRewardValue3;
                                command3.Parameters.Add("@GRADE", SqlDbType.BigInt).Value = 0;
                                command3.Parameters.Add("@TITLE", SqlDbType.NVarChar, 128).Value = "STRING_UI_MAIL_SYSTEM_016";
                                command3.Parameters.Add("@SENDAID", SqlDbType.BigInt).Value = 0;
                                command3.Parameters.Add("@SENDUSERNAME", SqlDbType.NVarChar, 32).Value = "STRING_UI_MAIL_SYSTEM_012";
                                command3.Parameters.Add("@ROUTE", SqlDbType.Int).Value = 17;
                                command3.Parameters.Add("@SUBROUTE", SqlDbType.Int).Value = retRewardItemCode3;
                                command3.Parameters.Add("@DESCROUTE", SqlDbType.BigInt).Value = 0;
                                command3.Parameters.Add("@MAILDIV", SqlDbType.TinyInt).Value = 12;
                                command3.Parameters.Add(outputResultMail1);
                                command3.ExecuteNonQuery();
                                int retResultMail1 = int.Parse(outputResultMail1.Value.ToString());
                                DB_sharding.Close();
                            }
                            //쿠폰 사용 처리
                            DB_common.Open();
                            var command5 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_common, CommandText = "UseCouponAID" };
                            var outputResultUseCoupon = new SqlParameter("@RESULT", SqlDbType.Int) { Direction = ParameterDirection.Output };
                            command5.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                            command5.Parameters.Add("@COUPONCODE", SqlDbType.NVarChar, 16).Value = CouponCode;
                            command5.Parameters.Add(outputResultUseCoupon);
                            command5.ExecuteNonQuery();
                            int retResultUseCoupon = int.Parse(outputResultUseCoupon.Value.ToString());
                            DB_common.Close();
                            CouponResult = retResultUseCoupon;
                            res.Add(new JsonNumericValue("resultcode", 0));
                        }
                        else if (retCouponType == 1)
                        {
                            if (retRewardItemCode1 != 0)
                            {
                                //쿠폰 상품 지급
                                DB_sharding.Open();
                                var command1 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "SendMail" };
                                var outputResultMail1 = new SqlParameter("@RESULT", SqlDbType.Int) { Direction = ParameterDirection.Output };
                                command1.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                                command1.Parameters.Add("@ITEMCODE", SqlDbType.BigInt).Value = retRewardItemCode1;
                                command1.Parameters.Add("@ITEMEA", SqlDbType.BigInt).Value = retRewardValue1;
                                command1.Parameters.Add("@GRADE", SqlDbType.BigInt).Value = 0;
                                command1.Parameters.Add("@TITLE", SqlDbType.NVarChar, 128).Value = "STRING_UI_MAIL_SYSTEM_016";
                                command1.Parameters.Add("@SENDAID", SqlDbType.BigInt).Value = 0;
                                command1.Parameters.Add("@SENDUSERNAME", SqlDbType.NVarChar, 32).Value = "STRING_UI_MAIL_SYSTEM_012";
                                command1.Parameters.Add("@ROUTE", SqlDbType.Int).Value = 17;
                                command1.Parameters.Add("@SUBROUTE", SqlDbType.Int).Value = retRewardItemCode1;
                                command1.Parameters.Add("@DESCROUTE", SqlDbType.BigInt).Value = retCouponType;
                                command1.Parameters.Add("@MAILDIV", SqlDbType.TinyInt).Value = 12;
                                command1.Parameters.Add(outputResultMail1);
                                command1.ExecuteNonQuery();
                                int retResultMail1 = int.Parse(outputResultMail1.Value.ToString());
                                DB_sharding.Close();

                                //쿠폰 사용 처리
                                DB_common.Open();
                                var command4 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_common, CommandText = "UseCoupon" };
                                var outputResultUseCoupon = new SqlParameter("@RESULT", SqlDbType.Int) { Direction = ParameterDirection.Output };
                                command4.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                                command4.Parameters.Add("@COUPONCODE", SqlDbType.NVarChar, 16).Value = CouponCode;
                                command4.Parameters.Add(outputResultUseCoupon);
                                command4.ExecuteNonQuery();
                                int retResultUseCoupon = int.Parse(outputResultUseCoupon.Value.ToString());
                                DB_common.Close();

                                if (retResultMail1 == 0)
                                {
                                    CouponResult = 0;
                                }
                                else
                                {
                                    CouponResult = retResult;
                                }

                            }
                            res.Add(new JsonNumericValue("resultcode", CouponResult));
                        }
                        else if (retCouponType == 2)
                        {
                            if (retRewardItemCode1 != 0)
                            {
                                //쿠폰 상품 지급
                                DB_sharding.Open();
                                var command1 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "SendMail" };
                                var outputResultMail1 = new SqlParameter("@RESULT", SqlDbType.Int) { Direction = ParameterDirection.Output };
                                command1.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                                command1.Parameters.Add("@ITEMCODE", SqlDbType.BigInt).Value = retRewardItemCode1;
                                command1.Parameters.Add("@ITEMEA", SqlDbType.BigInt).Value = retRewardValue1;
                                command1.Parameters.Add("@GRADE", SqlDbType.BigInt).Value = 0;
                                command1.Parameters.Add("@TITLE", SqlDbType.NVarChar, 128).Value = "STRING_UI_MAIL_SYSTEM_016";
                                command1.Parameters.Add("@SENDAID", SqlDbType.BigInt).Value = 0;
                                command1.Parameters.Add("@SENDUSERNAME", SqlDbType.NVarChar, 32).Value = "STRING_UI_MAIL_SYSTEM_012";
                                command1.Parameters.Add("@ROUTE", SqlDbType.Int).Value = 17;
                                command1.Parameters.Add("@SUBROUTE", SqlDbType.Int).Value = retRewardItemCode1;
                                command1.Parameters.Add("@DESCROUTE", SqlDbType.BigInt).Value = retCouponType;
                                command1.Parameters.Add("@MAILDIV", SqlDbType.TinyInt).Value = 12;
                                command1.Parameters.Add(outputResultMail1);
                                command1.ExecuteNonQuery();
                                int retResultMail1 = int.Parse(outputResultMail1.Value.ToString());
                                DB_sharding.Close();
                                if (retResultMail1 == 0)
                                {
                                    CouponResult = 0;
                                }
                                else
                                {
                                    CouponResult = retResult;
                                }
                            }
                            res.Add(new JsonNumericValue("resultcode", CouponResult));
                        }
                        else
                        {
                            res.Add(new JsonNumericValue("resultcode", 93));
                            string json_text = res.ToString();
                            var EncryptString = "";
                            TheSoulEncrypt.EncryptData(0, json_text, "0", ref EncryptString);
                            string JsonConvertData = "";
                            JsonConvert.Convert(1, EncryptString, ref JsonConvertData);
                            Response.Write(JsonConvertData);
                        }

                        if (CouponResult == 0)
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
                        }
                        string json_text2 = res.ToString();
                        var EncryptString2 = "";
                        string JsonConvertData2 = "";
                        if (CouponResult == 0)
                        {
                            TheSoulEncrypt.EncryptData(0, json_text2, retMyUpdateTime, ref EncryptString2);
                            JsonConvert.Convert(0, EncryptString2, ref JsonConvertData2);
                        }
                        else
                        {
                            TheSoulEncrypt.EncryptData(0, json_text2, "0", ref EncryptString2);
                            JsonConvert.Convert(1, EncryptString2, ref JsonConvertData2);
                        }
                        Response.Write(JsonConvertData2);
                        //로그 저장
                        LogManager_Old.Instance.WriteLogMessage("URL : {0}\r\nparameter : {1}\r\nencrypt_parameter : {2}\r\nresult : {3}", reqURL, reqval, Value, json_text2);
                    
                    }
                    else
                    {
                        res.Add(new JsonNumericValue("resultcode", retResult));
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
                else
                {
                    res.Add(new JsonNumericValue("resultcode", 40));
                    //파라미터 값 확인 로그
                    string reqval = "aid=" + AID;
                    string reqURL = System.Convert.ToString(Request.Url);
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
                if (DB_log != null)
                    DB_log.Close();

                string reqval = "aid=" + Request["aid"] + "&value=" + Request["value"];
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
}