using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using mSeed.mDBTxnBlock;
using mSeed.RedisManager;
using ServiceStack.Text;

using NAMU;   // for compress string and convert base64

namespace OperationTool.Tools
{
    public class WebRequestLog
    {
        public long requestID { get; set; }
        public string requestUrl { get; set; }
        public string requestOp { get; set; }
        public string returnJson { get; set; }
        public Result_Define.eResult reterror { get; set; }
    }

    public class WebQueryParam
    {
        public bool bDBLog = false;


        public string GetReqParams()
        {
            return mJsonSerializer.ToJsonString(ReqParams);
        }

        public string operation = string.Empty;

        private bool render_errorFlag = false;
        //private long requestAID = 0;
        //private long requestIndex = 0;
        private string requestUrl = string.Empty;
        private string returnJson = string.Empty;
        private Result_Define.eResult errCode = Result_Define.eResult.SUCCESS;

        public bool Render_errorFlag
        {
            get { return render_errorFlag; }
        }

        private bool render_compression = false;

        public bool SetCompressionMode
        {
            set { render_compression = value; }
        }

        public bool Render_compressionFlag
        {
            get { return render_compression; }
        }


        private bool check_rerequestflag = false;

        public bool ReRequestFlag
        {
            get { return check_rerequestflag; }
        }

        private List<string> detailDBLog = new List<string>();

        public void DBLog(string e)
        {
            if (bDBLog)
                detailDBLog.Add(e);
        }

        public List<string> GetDBLog()
        {
            return detailDBLog;
        }

        public string ReRequestRender()
        {
            try
            {
                if (errCode != Result_Define.eResult.SUCCESS)
                    render_errorFlag = false;
                else
                    render_errorFlag = true;

                string json = EncryptParam(errCode, returnJson);
                System.Web.HttpContext.Current.Response.Write(json);
                return json;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string Render(string json, Result_Define.eResult errorCode)
        {
            //System.Threading.Thread.Sleep(TheSoul.DataManager.Math.GetRandomInt(2000, 5000));
            try
            {
                if (errorCode != Result_Define.eResult.SUCCESS)
                    render_errorFlag = false;
                else
                    render_errorFlag = true;

                //if (errorCode == Result_Define.eResult.REDIS_COMMAND_FAIL)
                //    TheSoulDBcon.TheSoulRedisReconnect();

                json = EncryptParam(errorCode, json);
                System.Web.HttpContext.Current.Response.Write(json);
                return json;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string Render(Result_Define.eResult errorCode)
        {
            try
            {
                if (errorCode != Result_Define.eResult.SUCCESS)
                    render_errorFlag = false;
                else
                    render_errorFlag = true;

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }


        public string Render<T>(T obj, Result_Define.eResult errorCode)
        {
            try
            {
                if (errorCode != Result_Define.eResult.SUCCESS)
                    render_errorFlag = false;
                else
                    render_errorFlag = true;

                string json = mJsonSerializer.ToJsonString(obj);
                json = EncryptParam(errorCode, json);
                System.Web.HttpContext.Current.Response.Write(json);
                return json;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string RenderOK(Result_Define.eResult errorCode)
        {
            try
            {
                if (errorCode != Result_Define.eResult.SUCCESS)
                    render_errorFlag = false;
                else
                    render_errorFlag = true;

                string json = EncryptParam(errorCode, string.Empty);
                System.Web.HttpContext.Current.Response.Write(json);
                return json;
            }
            catch
            {
                return string.Empty;
            }
        }

        public WebQueryParam()
        {
#if DEBUG
            if (System.Web.HttpContext.Current.Request.Params.AllKeys.Contains("Debug"))
                SetDebugMode = true;
            if (System.Web.HttpContext.Current.Request.Params.AllKeys.Contains("Compression"))
                SetCompressionMode = true;
#endif
            requestUrl = System.Web.HttpContext.Current.Request.Url.LocalPath;
        }

        public WebQueryParam(bool setMode, bool setCompression = false)
        {
            setDebug = setMode;
            SetCompressionMode = setCompression;
            requestUrl = System.Web.HttpContext.Current.Request.Url.LocalPath;
        }

        bool setShowLog = false;

        public bool SetShowLogMode
        {
            get { return setShowLog; }
            set { setShowLog = value; }
        }

        private bool setDebug = false;

        public bool SetDebugMode
        {
            get { return setDebug; }
            set { setDebug = value; }
        }

        //public void CheckSnail_ID(ref TxnBlock TB, long AID, ref Account userInfo)
        //{
        //    string s_account = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_account]);

        //    if (string.IsNullOrEmpty(s_account) || userInfo.AID != AID)
        //        userInfo = AccountManager.GetAccountData(ref TB, AID);

        //    string n_role_level = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_role_level]);
        //    if (string.IsNullOrEmpty(n_role_level))
        //    {
        //        if (userInfo.AID != AID)
        //            userInfo = AccountManager.GetAccountData(ref TB, AID);
        //        CharacterManager.GetCharacter(ref TB, AID, userInfo.EquipCID);
        //    }
        //}

        public Dictionary<string, string> ReqParams = new Dictionary<string, string>();

        public string QueryParam_Fetch_Request(string key, string default_value = default(string))
        {
            string retValue = default_value;
            if (System.Web.HttpContext.Current.Request.Params.AllKeys.Contains(key))
                retValue = System.Convert.ToString(System.Web.HttpContext.Current.Request.Params[key]);
            return retValue;
        }

        public double QueryParam_FetchDouble(string key, double default_value = 0)
        {
            double retValue = default_value;
            double.TryParse(QueryParam_Fetch(key, default_value.ToString()), out retValue);
            return retValue;
        }

        public float QueryParam_FetchFloat(string key, float default_value = 0)
        {
            float retValue = default_value;
            float.TryParse(QueryParam_Fetch(key, default_value.ToString()), out retValue);
            return retValue;
        }

        public byte QueryParam_FetchByte(string key, byte default_value = 0)
        {
            byte retValue = default_value;
            byte.TryParse(QueryParam_Fetch(key, default_value.ToString()), out retValue);
            return retValue;
        }

        public short QueryParam_FetchShort(string key, short default_value = 0)
        {
            short retValue = default_value;
            short.TryParse(QueryParam_Fetch(key, default_value.ToString()), out retValue);
            return retValue;
        }

        public int QueryParam_FetchInt(string key, int default_value = 0)
        {
            int retValue = default_value;
            int.TryParse(QueryParam_Fetch(key, default_value.ToString()), out retValue);
            return retValue;
        }
        public long QueryParam_FetchLong(string key, long default_value = 0)
        {
            long retValue = default_value;
            long.TryParse(QueryParam_Fetch(key, default_value.ToString()), out retValue);
            return retValue;
        }

        public string QueryParam_Fetch(string key, string default_value = default(string), string encryptKey = TheSoulEncrypt.baseEncrypt)
        {
            string retValue = default_value;
            string realKey = key;
            key = key.ToLower();

            try
            {
                if (setDebug)
                {
                    if (System.Web.HttpContext.Current.Request.Params.AllKeys.Contains(key))
                    {
                        retValue = System.Convert.ToString(System.Web.HttpContext.Current.Request.Params[key]);
                        retValue = string.IsNullOrEmpty(retValue) ? default_value : retValue;
                        if (!ReqParams.ContainsKey(key))
                            ReqParams.Add(key, retValue);
                        else
                            ReqParams[key] = retValue;
                    }
                    else if (System.Web.HttpContext.Current.Request.Params.AllKeys.Contains(realKey))
                    {
                        retValue = System.Convert.ToString(System.Web.HttpContext.Current.Request.Params[realKey]);
                        retValue = string.IsNullOrEmpty(retValue) ? default_value : retValue;
                        if (!ReqParams.ContainsKey(key))
                            ReqParams.Add(key, retValue);
                        else
                            ReqParams[key] = retValue;
                    }
                    //retValue  = System.Web.HttpContext.Current.Request.Params.AllKeys.Contains(key) ? System.Convert.ToString(System.Web.HttpContext.Current.Request.Params["key"]) : default_value;
                }
                else
                {
                    retValue = ReqParams.ContainsKey(key) ? ReqParams[key] : default_value;
                }
            }
            catch (Exception e)
            {
                DBLog("StackTrace" + mJsonSerializer.ToJsonString(e.StackTrace));
                DBLog(e.Message);
                retValue = default_value;
            }

            return retValue;
        }

        public void ParamDecrypt(uint encryptKey) { ParamDecrypt(System.Convert.ToString(encryptKey)); }
        public void ParamDecrypt(string encryptKey = TheSoulEncrypt.baseEncrypt)
        {
            if (!setDebug)
            {
                ReqParams.Clear();
                string Value = System.Convert.ToString(System.Web.HttpContext.Current.Request.Params["value"]);
                Value = Value.Replace(" ", "+");
                // Decrypt request value to json string
                var DecryptString = "";
                if (!string.IsNullOrEmpty(Value))
                    TheSoulEncrypt.DecryptData(Value, encryptKey, ref DecryptString);
                ReqParams = mJsonSerializer.JsonToDictionary(DecryptString);
            }
        }

        public string EncryptParam(Result_Define.eResult errorCode, string json)
        {
            returnJson = json;
            errCode = errorCode;
            json = mJsonSerializer.AddJson(json, DefineError.reqOperation, operation);
            json = mJsonSerializer.AddJson(json, DefineError.retResultCode, ((int)errorCode).ToString());

            //if (!render_errorFlag)
            //{                
            //    json = mJsonSerializer.AddJson(json, DefineError.reqParams, mJsonSerializer.ToJsonString(ReqParams));
            //}

            if (!setDebug)
            {
                render_compression = json.Length > Operation_Define.CompressionLength;
                string EncryptString = "";
                //if (render_compression)
                //    TheSoulEncrypt.CompressionEncrypt(json, userEncryptKey.EncryptKey, ref EncryptString);
                //else
                //    TheSoulEncrypt.EncryptData(json, userEncryptKey.EncryptKey, ref EncryptString);

                json = mJsonSerializer.AddJson("{}", DefineError.retEncryptData, EncryptString);
            }

            json = mJsonSerializer.AddJson(json, DefineError.retResult, (!setDebug && render_compression) ? "1" : "0");

            return json;
        }

        public void ErrorLogWrite(string jsonObj, ref TxnBlock TB)
        {
            try
            {
                //DetailLogWriteToDB(ref TB, jsonObj);
                Console.WriteLine(jsonObj);
            }
            catch (Exception ex)
            {
                DBLog("StackTrace" + mJsonSerializer.ToJsonString(ex.StackTrace));
                DBLog(ex.Message);
                //Render(ex.Message, Result_Define.eResult.System_Exception);
            }
            TB.EndTransaction();
            TB.Dispose();
        }

        
    }
}