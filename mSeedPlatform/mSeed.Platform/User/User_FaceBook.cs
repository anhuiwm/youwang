using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using mSeed.Common;
using mSeed.mDBTxnBlock;
using System.Data;
using System.Data.SqlClient;
using ServiceStack.Text;

namespace mSeed.Platform
{
    public class FaceBookJson
    {
        private struct FaceBookAppKey
        {
            public string App_Access_Token;
            public DateTime ExpireTime;

            public FaceBookAppKey(string apptoken)
            {
                App_Access_Token = apptoken;
                ExpireTime = DateTime.Now;
            }
        };

        private static FaceBookJson _FaceBookJson = new FaceBookJson();

        private FaceBookJson()
        {
            FaceBookKeyList = new Dictionary<string, FaceBookAppKey>();
        }

        public static FaceBookJson GetFaceBookInstance()
        {
            return _FaceBookJson;
        }

        Dictionary<string, FaceBookAppKey> FaceBookKeyList;

        private const string FaceBookURL_GetUserInfo = "https://graph.facebook.com/debug_token";
        private const string FaceBookURL_GetAccessToken = "https://graph.facebook.com/oauth/access_token";

        private string _FaceBook_App_Access_Token = string.Empty;
        private const int ExpireInterval = -60;
        public const string FaceBookValidateJsonKey = "user_id";

        const string testClientID = "371278319899416";
        const string testClientSecret = "718013615b771c2d6cdcee3f76ed9ef8";
        /*
         *         const string testClientID = "1620687168258784";
        const string testClientSecret = "d6688a5c6b308c298c0b2213bd04697a";
            // Exchange the code for an extended access token
            Uri eatTargetUri = new Uri("https://graph.facebook.com/oauth/access_token?grant_type=fb_exchange_token&client_id=" + ConfigurationManager.AppSettings["FacebookAppId"] + "&client_secret=" + ConfigurationManager.AppSettings["FacebookAppSecret"] + "&fb_exchange_token=" + accessToken);
            HttpWebRequest eat = (HttpWebRequest)HttpWebRequest.Create(eatTargetUri);

            StreamReader eatStr = new StreamReader(eat.GetResponse().GetResponseStream());
            string eatToken = eatStr.ReadToEnd().ToString().Replace("access_token=", "");

         */

        private string GetFaceBook_AppToken(string clientID, string clientSecret)
        {
            if (FaceBookKeyList.ContainsKey(clientID))
                if (FaceBookKeyList[clientID].ExpireTime.AddMinutes(ExpireInterval) > DateTime.Now)
                    return FaceBookKeyList[clientID].App_Access_Token;

            StringBuilder dataParams = new StringBuilder();
            dataParams.Append("client_id=");
            dataParams.Append(clientID);
            dataParams.Append("&client_secret=");
            dataParams.Append(clientSecret);
            dataParams.Append("&grant_type=client_credentials");
            //mSeed.Common.mLogger.mLogger.Info("wmlog::GetFaceBook_AppToken dataParams.ToString()=" + dataParams.ToString());
            string responseBody = WebTools.GetReqeustURL(FaceBookURL_GetAccessToken, dataParams.ToString(), false);

            //mSeed.Common.mLogger.mLogger.Info("wmlog::GetFaceBook_AppToken responseBody=" + responseBody);

            if (!responseBody.Contains("access_token"))
                FaceBookKeyList.Add(clientID, new FaceBookAppKey(responseBody));
            
            return responseBody;
        }

        public JsonObject GetFaceBookCodeExchange(string clientID, string clientSecret, string authCode)
        {
            StringBuilder dataParams = new StringBuilder();
            dataParams.Append("grant_type=fb_exchange_token");
            dataParams.Append("&client_id=");
            dataParams.Append(clientID);
            dataParams.Append("&client_secret=");
            dataParams.Append(clientSecret);
            dataParams.Append("&fb_exchange_token=");
            dataParams.Append(authCode);

            string responseBody = WebTools.GetReqeustURL(FaceBookURL_GetAccessToken, dataParams.ToString(), false);
            if (!responseBody.Contains("access_token"))
            {
                JsonObject retJson = new JsonObject();
                retJson["error"] = "app_access_token_fail";
                return retJson;
            }

            try
            {
                JsonObject retJson = JsonObject.Parse(responseBody);
                return retJson == null ? new JsonObject() : retJson;
            }
            catch (Exception ex)
            {
                JsonObject retJson = new JsonObject();
                retJson["error"] = ex.Message;
                return retJson;
            }
        }

        public JsonObject GetUserProfileJson(string service_app_id, string service_secret, string access_token)
        {
            string appkey = GetFaceBook_AppToken(service_app_id, service_secret);
            if (!appkey.Contains("access_token"))
            {
                JsonObject retJson = new JsonObject();
                retJson["error"] = "app_access_token_fail";
                return retJson;
            }
            JsonObject retjson = JsonObject.Parse(appkey);
            string access_token_value;
            if (!retjson.TryGetValue("access_token", out access_token_value))
                access_token_value = string.Empty;

            string token_type_value;
            if (!retjson.TryGetValue("token_type", out token_type_value))
                token_type_value = string.Empty;

            StringBuilder dataParams = new StringBuilder();
            dataParams.Append("input_token=");
            dataParams.Append(access_token);
            dataParams.Append("&");
            dataParams.Append("access_token=");
            dataParams.Append(access_token_value);
            dataParams.Append("&");
            dataParams.Append("token_type=");
            dataParams.Append(token_type_value);


            try
            {
                mSeed.Common.mLogger.mLogger.Info("wmlog::GetUserProfileJson dataParams.ToString()=" + dataParams.ToString());
                string responseBody = WebTools.GetReqeustURL(FaceBookURL_GetUserInfo, dataParams.ToString(), false);
                mSeed.Common.mLogger.mLogger.Info("wmlog::GetUserProfileJson responseBody=" + responseBody);
                JsonObject retJson = JsonObject.Parse(responseBody);
                return retJson == null ? new JsonObject() : retJson;
            }
            catch (Exception ex)
            {
                JsonObject retJson = new JsonObject();
                retJson["error"] = ex.Message;
                return retJson;
            }
        }


        public JsonObject GetUserProfileJson(string access_token)
        {
            string appkey = GetFaceBook_AppToken(testClientID, testClientSecret);
            if (!appkey.Contains("access_token"))
            {
                JsonObject retJson = new JsonObject();
                retJson["error"] = "app_access_token_fail";
                return retJson;
            }

            JsonObject retjson = JsonObject.Parse(appkey);
            string access_token_value;
            if (!retjson.TryGetValue("access_token", out access_token_value))
                access_token_value = string.Empty;
            string token_type_value;
            if (!retjson.TryGetValue("token_type", out token_type_value))
                token_type_value = string.Empty;

            StringBuilder dataParams = new StringBuilder();
            dataParams.Append("input_token=");
            dataParams.Append(access_token);
            dataParams.Append("&");
            dataParams.Append("access_token=");
            dataParams.Append(access_token_value);
            dataParams.Append("&");
            dataParams.Append("token_type=");
            dataParams.Append(token_type_value);


            try
            {
                string responseBody = WebTools.GetReqeustURL(FaceBookURL_GetUserInfo, dataParams.ToString(), false);
                JsonObject retJson = JsonObject.Parse(responseBody);
                return retJson == null ? new JsonObject() : retJson;
            }
            catch (Exception ex)
            {
                JsonObject retJson = new JsonObject();
                retJson["error"] = ex.Message;
                return retJson;
            }
        }
    }

    public partial class UserManager
    {
        private static string GetFaceBookPlatformID(string service_app_id, string service_secret, string access_token)
        {
            string retid = string.Empty;
            JsonObject retJson = FaceBookJson.GetFaceBookInstance().GetUserProfileJson(service_app_id, service_secret, access_token);
            if (retJson.TryGetValue("data", out retid))
            {
                retJson = JsonObject.Parse(retid);
                if (!retJson.TryGetValue(FaceBookJson.FaceBookValidateJsonKey, out retid))
                    retid = string.Empty;
            }
            return retid;
        }

        private static string GetFaceBookPlatformIDByAuthCode(string service_app_id, string service_secret, string access_token)
        {
            string retid = string.Empty;
            JsonObject retJson = FaceBookJson.GetFaceBookInstance().GetFaceBookCodeExchange(service_app_id, service_secret, access_token);
            if (retJson.TryGetValue("data", out retid))
            {
                retJson = JsonObject.Parse(retid);
                if (!retJson.TryGetValue(FaceBookJson.FaceBookValidateJsonKey, out retid))
                    retid = string.Empty;
            }
            return retid;
        }
    }
}
