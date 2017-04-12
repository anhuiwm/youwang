using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



using System.Text;
using System.Data;
using System.Data.SqlClient;
using mSeed.mDBTxnBlock;
using mSeed.RedisManager;
using mSeed.Common;
using ServiceStack.Text;

using System.IO;
using System.Net;


namespace WebPlatform.Tools
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

        public string UrlEncode(string a_key)
        {
            //Encoding gb2312 = Encoding.GetEncoding("gb2312");
            //Encoding utf8 = Encoding.UTF8;
            ////首先用utf-8进行解码
            //string key = HttpUtility.UrlDecode(a_key, utf8);
            //// 将已经解码的字符再次进行编码.
            //string encode = HttpUtility.UrlEncode(key, utf8).ToLower();
            //return encode;


            Encoding utf8 = Encoding.UTF8;
            string key = HttpUtility.UrlEncode(a_key, utf8);
            return key;
            // 将已经解码的字符再次进行编码.
            // string encode = HttpUtility.UrlEncode(key, utf8).ToLower();
            // return encode;
        }

                 protected void HttpPost(string url, byte[] data)
                 {
                     Encoding DEFAULT_ENCODING = Encoding.GetEncoding("GB2312");
                     string CONTENT_TYPE = "application/x-www-form-urlencoded";
                     string ACCEPT = "Accept: */*\r\n";
                     string USERAGENT = "";

                     string httpUrl = url + data;
                     if (string.IsNullOrEmpty(httpUrl))
                     {
                         throw new ArgumentNullException("httpUrl");
                     }

                     HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                  
                     httpWebRequest.ContentType = CONTENT_TYPE;
                    // httpWebRequest.Accept = ACCEPT;
                    // httpWebRequest.Referer = server;
                    // httpWebRequest.UserAgent = USERAGENT;
                     httpWebRequest.Method = "Post";
                     httpWebRequest.ContentLength = data.Length;

                     Stream myStream = httpWebRequest.GetRequestStream();
                     myStream.Write(data, 0, data.Length);
                     myStream.Close();

                     httpWebRequest.BeginGetResponse(new AsyncCallback(ReadPostCallback), httpWebRequest);
                 }


                private void ReadPostCallback(IAsyncResult asyncResult)
                {
                    WebRequest request = (HttpWebRequest)asyncResult.AsyncState;
                    HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult);
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var resultString = streamReader.ReadToEnd();
                        resultString = resultString.Replace("\r", "").Replace("\n", "").Replace("\t", "");
                        Console.WriteLine("ReadGetCallback:relust=" + resultString);
                    }
                    int status = (int)response.StatusCode;

                   
//                     if (status == 0)
//                     {
//                         msgAck.m_result = 1;
//                     }
                   

                }


        protected void HttpGet(string url, string data)
        {
            string httpUrl = url + data;
            if (string.IsNullOrEmpty(httpUrl))
            {
                throw new ArgumentNullException("httpUrl");
            }

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(httpUrl);
            httpRequest.Timeout = 2000;
            httpRequest.Method = "GET";
            httpRequest.BeginGetResponse(ReadGetCallback, httpRequest);
        }

        private void ReadGetCallback(IAsyncResult asynchronousResult)
        {
            var request = (HttpWebRequest)asynchronousResult.AsyncState;
            var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            var streamReader = new StreamReader(response.GetResponseStream());
            var resultString = streamReader.ReadToEnd();
            resultString = resultString.Replace("\r", "").Replace("\n", "").Replace("\t", "");

            int status = (int)response.StatusCode;
            if (resultString == "0")
            {
                
               // UnityUtility.CTrace.Singleton.infor("SDK请求认证成功[Account={0}][Token={1}][ZoneID={2}][Rresult={3}]", m_accountName, m_token, m_zoneID, resultString);
      

            }
            else
            {
             
              //  UnityUtility.CTrace.Singleton.error("SDK请求认证失败[Account={0}][Token={1}][ZoneID={2}][Rresult={3}][Status={4}]", m_accountName, m_token, m_zoneID, resultString, status);
            }

           

        }


        private void DetailLogWriteToFile(string retJson)
        {
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
                    
                    DateTime ReqTime = System.Web.HttpContext.Current.Request.RequestContext.HttpContext.Timestamp;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("RequestURL : ");
                    sb.Append(requestUrl);
                    sb.AppendLine();
                    sb.Append("RequestParam : ");
                    sb.Append(ParamJson);
                    sb.AppendLine();
                    sb.Append("ReturnJson : ");
                    sb.Append(retJson);
                    sb.AppendLine();

                    mSeed.Common.mLogger.mLogger.Debug(sb.ToString());
                }
            }
            mSeed.Common.mLogger.mLogger.GetLoggerInstance().FlushLog();
        }

        public string GetReqParams()
        {
            return mJsonSerializer.ToJsonString(ReqParams);
        }

        public string operation = string.Empty;
        
        private bool render_errorFlag = false;
        private string requestUrl = string.Empty;
        private string returnJson = string.Empty;

        public bool Render_errorFlag
        {
            get { return render_errorFlag; }
        }

        const string JsonErrorKey = "error";
        bool renderFlag = false;

        public void Render(string json, Result_Define.eResult errorCode)
        {
            if (renderFlag)
                return;

            //System.Threading.Thread.Sleep(TheSoul.DataManager.Math.GetRandomInt(2000, 5000));
            try
            {
                if (errorCode != Result_Define.eResult.SUCCESS)
                    render_errorFlag = false;
                else
                    render_errorFlag = true;

                System.Web.HttpContext.Current.Response.Write(json);
                renderFlag = true;
                if (Array.IndexOf(ignore_retjson_ops, operation) < 0)
                    DetailLogWriteToFile(json);
            }
            catch(Exception ex)
            {
                mSeed.Common.mLogger.mLogger.Critical(string.Format("system exception error = {0}, stacktrace = {1}", ex.Message, ex.StackTrace, "webapi"));
            }       
        }

        private string[] ignore_retjson_ops = new string[] {
            "push_msg_check",
            "get_pushlist",
        };

        public void Render(JsonObject json, Result_Define.eResult errorCode)
        {
            if (renderFlag)
                return;

            //System.Threading.Thread.Sleep(TheSoul.DataManager.Math.GetRandomInt(2000, 5000));
            try
            {
                if (errorCode != Result_Define.eResult.SUCCESS)
                    render_errorFlag = false;
                else
                    render_errorFlag = true;
                json[JsonErrorKey] = ((int)errorCode).ToString();
                string retjson = json.ToJson();

                System.Web.HttpContext.Current.Response.Write(retjson);
                renderFlag = true;
                if (Array.IndexOf(ignore_retjson_ops, operation) < 0)
                    DetailLogWriteToFile(retjson);
            }
            catch (Exception ex)
            {
                mSeed.Common.mLogger.mLogger.Critical(string.Format("system exception error = {0}, stacktrace = {1}", ex.Message, ex.StackTrace, "webapi"));
            }
        }


        public void NoRenderWirte(Result_Define.eResult errorCode)
        {
            try
            {
                if (errorCode != Result_Define.eResult.SUCCESS)
                    render_errorFlag = false;
                else
                    render_errorFlag = true;
                JsonObject json = new JsonObject();
                json[JsonErrorKey] = ((int)errorCode).ToString();
                string retjson = json.ToJson();

                if (Array.IndexOf(ignore_retjson_ops, operation) >= 0)
                    retjson = "";

                DetailLogWriteToFile(retjson);
                renderFlag = true;
            }
            catch (Exception ex)
            {
                mSeed.Common.mLogger.mLogger.Critical(string.Format("system exception error = {0}, stacktrace = {1}", ex.Message, ex.StackTrace, "webapi"));
            }
        }
        
        public void Render(Result_Define.eResult errorCode)
        {
            try
            {
                if (errorCode != Result_Define.eResult.SUCCESS)
                    render_errorFlag = false;
                else
                    render_errorFlag = true;
                JsonObject json = new JsonObject();
                json[JsonErrorKey] = ((int)errorCode).ToString();
                string retjson = json.ToJson();

                System.Web.HttpContext.Current.Response.Write(retjson);
                renderFlag = true;
                if (Array.IndexOf(ignore_retjson_ops, operation) < 0)
                    DetailLogWriteToFile(retjson);
            }
            catch (Exception ex)
            {
                mSeed.Common.mLogger.mLogger.Critical(string.Format("system exception error = {0}, stacktrace = {1}", ex.Message, ex.StackTrace, "webapi"));
            }
        }

        public void Render<T>(T obj, Result_Define.eResult errorCode)
        {
            try
            {
                if (errorCode != Result_Define.eResult.SUCCESS)
                    render_errorFlag = false;
                else
                    render_errorFlag = true;
                JsonObject json = JsonObject.Parse(mJsonSerializer.ToJsonString(obj));
                json[JsonErrorKey] = ((int)errorCode).ToString();
                string retjson = json.ToJson();

                System.Web.HttpContext.Current.Response.Write(retjson);
                renderFlag = true;
                if (Array.IndexOf(ignore_retjson_ops, operation) < 0)
                    DetailLogWriteToFile(retjson);
            }
            catch (Exception ex)
            {
                mSeed.Common.mLogger.mLogger.Critical(string.Format("system exception error = {0}, stacktrace = {1}", ex.Message, ex.StackTrace, "webapi"));
            }         
        }

        public void RenderOK(Result_Define.eResult errorCode)
        {
            Render(errorCode);
        }

        public WebQueryParam()
        {
            requestUrl = System.Web.HttpContext.Current.Request.Url.LocalPath;
        }

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

        public void LogWriter(string setLog)
        {            
        }

        public string QueryParam_Fetch(string key, string default_value = "")
        {
            string retValue = default_value;
            string realKey = key;
            key = key.ToLower();

            try
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
            }
            catch (Exception e)
            {
                LogWriter("StackTrace" + mJsonSerializer.ToJsonString(e.StackTrace));
                LogWriter(e.Message);
                retValue = default_value;
            }

            return retValue;
        }
        
        public void ErrorLogWrite(string jsonObj)
        {
            try
            {
                LogWriter(jsonObj);
            }
            catch (Exception ex)
            {
                LogWriter("StackTrace" + mJsonSerializer.ToJsonString(ex.StackTrace));
                LogWriter(ex.Message);
            }
        }

    }
}