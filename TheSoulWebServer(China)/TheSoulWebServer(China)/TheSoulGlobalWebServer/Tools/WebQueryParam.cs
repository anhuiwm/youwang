using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using mSeed.mDBTxnBlock;
using mSeed.RedisManager;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Global;
using ServiceStack.Text;

using NAMU;   // for compress string and convert base64

namespace TheSoulGlobalWebServer.Tools
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
        public string ipaddress = "";

        private void DetailLogWriteToDB(ref TxnBlock TB, string retJson, string LogDB = DataManager_Define.LogDB)
        {
            bDBLog = true;
            if (bDBLog)
            {
                //if (requestAID > 0)
                if (!string.IsNullOrEmpty(operation))
                {
                    string ParamJson;
                    if (ReqParams.Count > 0)
                        ParamJson = mJsonSerializer.ToJsonString(ReqParams);
                    else
                    {
                        JsonObject setJson = new JsonObject();

                        foreach (string setKeys in System.Web.HttpContext.Current.Request.RequestContext.HttpContext.Request.Params.AllKeys)
                        {
                            setJson.Add(setKeys, System.Web.HttpContext.Current.Request.RequestContext.HttpContext.Request.Params[setKeys]);
                        }
                        ParamJson = setJson.ToJson();
                    }

                    //ParamJson = mJsonSerializer.ToJsonString(System.Web.HttpContext.Current.Request.RequestContext.HttpContext.Request.Params);

                    retJson = string.Format("{0},encryptKey={1}", retJson, userEncryptKey.EncryptKey);

                    string DBLog = mJsonSerializer.ToJsonString(detailDBLog);
                    DateTime ReqTime = System.Web.HttpContext.Current.Request.RequestContext.HttpContext.Timestamp;
                    string query = string.Format(@"INSERT INTO {0} (AID, CID, ErrorCode, RequestURL, Operation, RequestParams, ResponseResult, BaseJson, DetailDBLog, requesttime) VALUES ( @aid, @cid, @errcode, @url, @op, @params, @response, @baseJson, @DBLog, @reqTime)", DataManager_Define.LogTableName);
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@aid", requestAID);
                    cmd.Parameters.AddWithValue("@cid", ReqParams.ContainsKey("cid") ? System.Convert.ToInt64(ReqParams["cid"]) : 0);
                    cmd.Parameters.AddWithValue("@errcode", errCode);
                    cmd.Parameters.AddWithValue("@url", requestUrl);
                    cmd.Parameters.AddWithValue("@op", operation);
                    cmd.Parameters.AddWithValue("@params", ParamJson);
                    cmd.Parameters.AddWithValue("@response", retJson);
                    cmd.Parameters.AddWithValue("@baseJson", returnJson);
                    cmd.Parameters.AddWithValue("@DBLog", DBLog);
                    cmd.Parameters.AddWithValue("@reqTime", ReqTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                    TB.ExcuteSqlCommand(LogDB, ref cmd);
                    cmd.Dispose();
                }
            }
        }

        public void GMToolLogToDB(ref TxnBlock TB, string gmID, string dbkey)
        {
            if (ReqParams.Count > 0)
            {
                string GMLogTable = "gm_log";

                string ParamJson = mJsonSerializer.ToJsonString(ReqParams);
                DateTime ReqTime = System.Web.HttpContext.Current.Request.RequestContext.HttpContext.Timestamp;
                string query = string.Format(@"INSERT INTO {0} (gmid, ErrorCode, RequestURL, Params, DetailDBLog, regdate) VALUES ( @gmID, @errcode, @url, @params, '', @reqTime)", GMLogTable);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@gmID", gmID);
                cmd.Parameters.AddWithValue("@errcode", errCode);
                cmd.Parameters.AddWithValue("@url", requestUrl);
                cmd.Parameters.AddWithValue("@params", ParamJson);
                cmd.Parameters.AddWithValue("@reqTime", ReqTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                bool check = TB.ExcuteSqlCommand(dbkey, ref cmd);
                cmd.Dispose();
            }
        }

        public string operation = string.Empty;

        private bool render_errorFlag = false;
        private long requestAID = 0;
        private long requestIndex = 0;
        private string requestUrl = string.Empty;
        private string returnJson = string.Empty;
        private Result_Define.eResult errCode = Result_Define.eResult.SUCCESS;
        private User_Encrypt userEncryptKey = new User_Encrypt();

        public bool Render_errorFlag
        {
            get { return render_errorFlag; }
        }

        public void Ignore_Render_ErrorFlag()
        {
            if (errCode == Result_Define.eResult.BOSSRAID_CREATE_RATE_CHECK_FAIL || errCode == Result_Define.eResult.BOSSRAID_ALREADY_OPEN)
                render_errorFlag = true;
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
                //if(setDebug)
                detailDBLog.Add(e);
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
            try
            {
                if (errorCode != Result_Define.eResult.SUCCESS)
                    render_errorFlag = false;
                else
                    render_errorFlag = true;

                json = EncryptParam(errorCode, json);
                System.Web.HttpContext.Current.Response.Write(json);
                if (!requestUrl.Contains("RequestPrivateServer"))
                    AddReRequestInfo();
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
#else
            if (System.Web.HttpContext.Current.Request.Params.AllKeys.Contains("op"))
                //if(System.Web.HttpContext.Current.Request.Params["op"].Equals("load_ip_table"))
                    setDebug = true;
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

        public bool setDebug = false;

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

        Dictionary<string, string> ReqParams = new Dictionary<string, string>();

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
                    if (ReqParams.Count <= 0)
                    {
                        ParamDecrypt(userEncryptKey.EncryptKey);
                    }

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
                
                // Decrypt request value to json string
                var DecryptString = "";
                if (!string.IsNullOrEmpty(Value))
                {
                    Value = Value.Replace(" ", "+");
                    TheSoulEncrypt.DecryptData(Value, encryptKey, ref DecryptString);
                }
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
                render_compression = json.Length > DataManager_Define.CompressionLength;
                string EncryptString = "";
                if (render_compression)
                    TheSoulEncrypt.CompressionEncrypt(json, userEncryptKey.EncryptKey, ref EncryptString);
                else
                    TheSoulEncrypt.EncryptData(json, userEncryptKey.EncryptKey, ref EncryptString);

                json = mJsonSerializer.AddJson("{}", DefineError.retEncryptData, EncryptString);
            }

            json = mJsonSerializer.AddJson(json, DefineError.retResult, (!setDebug && render_compression) ? "1" : "0");

            return json;
        }

        public void SnailLogWrite(ref TxnBlock TB)
        {
            long setAID = System.Convert.ToInt64(TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid]));
            SnailLogManager.SetOperationTo_Snail_SID(ref TB, operation);

            if (setAID > 0)
            {
                if (TB.SystemLogData.ContainsKey(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_game_player_action_log]))
                {
                    SnailLogManager.SnailLog_write_game_player_action_log(ref TB, setAID);
                }

                if (TB.SystemLogData.ContainsKey(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_role_log]))
                {
                    SnailLogManager.SnailLog_write_role_log(ref TB, setAID);
                }

                if (TB.SystemLogData.ContainsKey(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.update_role_log]))
                {
                    SnailLogManager.SnailLog_update_role_log(ref TB, setAID);
                }

                if (TB.SystemLogData.ContainsKey(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_task_log]))
                {
                    string setTaskLogJson = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.trigger_id_list]);
                    List<TaskLogInfo> setLog = mJsonSerializer.JsonToObject<List<TaskLogInfo>>(setTaskLogJson);
                    SnailLogManager.SnailLog_write_task_log(ref TB, setAID, ref setLog);
                }

                if (TB.SystemLogData.ContainsKey(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_money_log]))
                {
                    string setMoneyLogJson = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.money_log_list]);
                    List<MoneyLogInfo> setLog = mJsonSerializer.JsonToObject<List<MoneyLogInfo>>(setMoneyLogJson);
                    SnailLogManager.SnailLog_write_money_log(ref TB, setAID, ref setLog);
                }

                if (TB.SystemLogData.ContainsKey(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_item_log]))
                {
                    string setItemLogJson = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.item_log_list]);
                    List<ItemLogInfo> setLog = mJsonSerializer.JsonToObject<List<ItemLogInfo>>(setItemLogJson);
                    SnailLogManager.SnailLog_write_item_log(ref TB, setAID, ref setLog);
                    if (SystemData.GetServiceArea(ref TB) != DataManager_Define.eCountryCode.Korea && TB.SystemLogData.ContainsKey(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_item_log]))
                        SnailLogManager.MseedLog_mSeed_item_log(ref TB, setAID, ref setLog);
                }

                if (TB.SystemLogData.ContainsKey(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_role_upgrade_log]))
                {
                    SnailLogManager.SnailLog_write_role_upgrade_log(ref TB, setAID);
                }

                if (TB.SystemLogData.ContainsKey(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_instance_log]))
                {
                    SnailLogManager.SnailLog_write_instance_log(ref TB, setAID);
                    SnailLogManager.SnailLog_write_scene_log(ref TB, setAID);
                }
            }
        }
        
        public void TxnBlockInit(ref TxnBlock TB, ref long retAID)
        {
            //string savePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
            //GlobalManager.GetGlobalServerIni(ref tb, savePath);

            // TxnBlock Initialize
            TB.IsoLevel = IsolationLevel.ReadUncommitted;     // set transaction IsolationLevel (default ReadUncommited)
            //RedisConst.GetRedisInstance().Elog = ErrorLog;

            ipaddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipaddress))
                ipaddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (!string.IsNullOrEmpty(ipaddress))
                TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_ip], ipaddress);

            retAID = System.Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["aid"]);
            bDBLog = TheSoulDBcon.GetInstance().TheSoulDBInitGlobal(ref TB);
            userEncryptKey = new User_Encrypt();
            userEncryptKey.EncryptKey = TheSoulEncrypt.base64_key;
            ParamDecrypt(userEncryptKey.EncryptKey);

            if (bDBLog)
                TB.Elog = DBLog;

            requestAID = retAID;
            requestIndex = QueryParam_FetchLong("reqid");
            operation = QueryParam_Fetch("op");
            long lastIndex = GetLastRequestIndex();

            // requestid auto increment
            if (requestIndex == 0 && requestIndex <= lastIndex)
                requestIndex = lastIndex + 1;
            else if (requestIndex <= lastIndex)
            {
                WebRequestLog reRequestInfo = CheckReRequest();
                if (reRequestInfo.requestID == requestIndex)
                {
                    check_rerequestflag = true;
                    returnJson = reRequestInfo.returnJson;
                    errCode = reRequestInfo.reterror;
                }
            }
        }

        public long GlobalDBOpen(ref TxnBlock TB)
        {
            string savePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
            long groupid = GlobalManager.GetGlobalServerIni(ref TB, savePath);
            if (bDBLog)
                TB.Elog = DBLog;
            return groupid;
        }

        public void LogDBOpen(ref TxnBlock TB)
        {
            //string savePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
            TheSoulDBcon.GetInstance().TheSoulLogDBInit(ref TB);
        }

        private string GetReRequestKey()
        {
            return string.Format("{0}_{1}", DataManager_Define.UserRequestCache_Prefix, requestAID);
        }

        public WebRequestLog CheckReRequest()
        {
            string setKey = GetReRequestKey();
            List<WebRequestLog> reqList = RedisConst.GetRedisInstance().GetAllItemFromList<WebRequestLog>(DataManager_Define.RedisServerAlias_System, setKey);

            WebRequestLog retInfo = reqList.Find(reqinfo => reqinfo.requestID == requestIndex
                                                            && reqinfo.requestOp == operation
                                                            && reqinfo.requestUrl == requestUrl);
            return retInfo == null ? new WebRequestLog() : retInfo;
        }

        public long GetLastRequestIndex()
        {
            string setKey = GetReRequestKey();
            return RedisConst.GetRedisInstance().GetObj<long>(DataManager_Define.RedisServerAlias_System, setKey);
        }

        public void SetLastRequestIndex()
        {
            string setKey = GetReRequestKey();
            RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_System, setKey, requestIndex);
        }

        public void AddReRequestInfo()
        {
            if (requestIndex < 1)
                return;

            WebRequestLog setInfo = new WebRequestLog();
            setInfo.requestID = requestIndex;
            setInfo.requestOp = operation;
            setInfo.requestUrl = requestUrl;
            setInfo.returnJson = returnJson;
            setInfo.reterror = errCode;

            string setKey = GetReRequestKey();
            RedisConst.GetRedisInstance().EnqueueList(DataManager_Define.RedisServerAlias_System, setKey, setInfo);
            SetLastRequestIndex();
            RedisConst.GetRedisInstance().ListExpireTimeSet(DataManager_Define.RedisServerAlias_System, setKey);
            long getCount = RedisConst.GetRedisInstance().GetListCount(DataManager_Define.RedisServerAlias_System, setKey);

            // dequeue list object
            for (int chkCount = DataManager_Define.UserReRequestQueueSize; chkCount < getCount; chkCount++)
            {
                RedisConst.GetRedisInstance().DequeueListString(DataManager_Define.RedisServerAlias_System, setKey);
            }
        }

        public void RemoveReRequestInfo()
        {
            requestIndex = 0;
            string setKey = GetReRequestKey();
            RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_System, setKey);
            RedisConst.GetRedisInstance().RemoveList(DataManager_Define.RedisServerAlias_System, setKey);
        }
    }
}