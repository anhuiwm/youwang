using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Net;

using mSeed.Common;
using mSeed.RedisManager;
using ServiceStack.Text;

namespace mSeed.Platform
{
    public class GoogleJsonWebToken
    {
        public enum eGooglePurchaseState
        {
            BILLING_RESPONSE_RESULT_UNKNOWN             = -1,   //Unknow error
            BILLING_RESPONSE_RESULT_OK	                = 0,	//Success
            BILLING_RESPONSE_RESULT_USER_CANCELED       = 1,	// User pressed back or canceled a dialog
            BILLING_RESPONSE_RESULT_SERVICE_UNAVAILABLE = 2,	//Network connection is down
            BILLING_RESPONSE_RESULT_BILLING_UNAVAILABLE = 3,    //Billing API version is not supported for the type requested
            BILLING_RESPONSE_RESULT_ITEM_UNAVAILABLE    = 4,	// Requested product is not available for purchase
            BILLING_RESPONSE_RESULT_DEVELOPER_ERROR     = 5,    // Invalid arguments provided to the API. This error can also indicate that the application was not correctly signed or properly set up for In-app Billing in Google Play, or does not have the necessary permissions in its manifest
            BILLING_RESPONSE_RESULT_ERROR               = 6,    //Fatal error during the API action
            BILLING_RESPONSE_RESULT_ITEM_ALREADY_OWNE   = 7,    //Failure to purchase since item is already owned
            BILLING_RESPONSE_RESULT_ITEM_NOT_OWNED      = 8,    // Failure to consume since item is not owned
        }

        private const string GoogleURL_IABVerify_Format = "https://www.googleapis.com/androidpublisher/v1.1/applications/{0}/inapp/{1}/purchases/{2}";
        private const string GoogleURL_GetAccessToken = "https://accounts.google.com/o/oauth2/token";
        private const string GoogleURL_GetAccessToken_v4 = "https://www.googleapis.com/oauth2/v4/token";
        private const string GoogleURL_OAuthLogin = "https://accounts.google.com/o/oauth2/auth";
        private const string GoogleURL_GetUserInfo = "https://www.googleapis.com/oauth2/v3/tokeninfo";
        public const string GoogleValidateJsonKey = "sub";
        public const string GoogleAccessTokenJsonKey = "access_token";
        public const string GoogleExpireJsonKey = "expires_in";
        public const string GoogleValidatePurchaseJsonKey = "purchaseState";

        private static GoogleJsonWebToken _GoogleJWT = new GoogleJsonWebToken();

        private GoogleJsonWebToken()
        {
            ExpireTime = DateTime.Now;
            GetGooglePlusAccessToken();
        }

        public static GoogleJsonWebToken GetGoogleJWTInstance()
        {
            return _GoogleJWT;
        }

        // singleton Google+ AccessToken
        private JsonObject _GooglePlusAccessToken = null;

        private DateTime ExpireTime = DateTime.Now;
        private const int ExpireInterval = -60;

        private string GetGooglePlusAccessToken()
        {
            if (ExpireTime.AddMinutes(ExpireInterval) <= DateTime.Now)
            {
                string[] setScope = { GoogleJsonWebToken.SCOPE_USER_EMAIL, GoogleJsonWebToken.SCOPE_USER_INFO, GoogleJsonWebToken.SCOPE_OPENID, GoogleJsonWebToken.SCOPE_PUBLISH };
                var auth = GetAccessToken(
                                SystemConfig.GetSystemConfigInstance().GoogleServiceAccount,
                                SystemConfig.GetSystemConfigInstance().GoogleServiceKeyFilePath,
                                setScope);
                _GooglePlusAccessToken = JsonObject.Parse(mJsonSerializer.ToJsonString(auth));

                if (_GooglePlusAccessToken != null)
                {
                    int setTime = 0;
                    Int32.TryParse(_GooglePlusAccessToken[GoogleExpireJsonKey], out setTime);
                    ExpireTime = DateTime.Now.AddSeconds(setTime);
                    return _GooglePlusAccessToken[GoogleAccessTokenJsonKey];
                }
            }

            return _GooglePlusAccessToken[GoogleAccessTokenJsonKey];
        }

        public Result_Define.eResult GoogleIABVerify(string purchaseToken, string productID, string appID)
        {
            //private const string GoogleURL_IABVerify_Format = "https://www.googleapis.com/androidpublisher/v1.1/applications/{0}/inapp/{1}/purchases/{2}";
            string Uri = string.Format(GoogleURL_IABVerify_Format
                                                , appID
                                                , productID
                                                , purchaseToken);
            StringBuilder dataParams = new StringBuilder();
            dataParams.Append("access_token=");
            dataParams.Append(GetGooglePlusAccessToken());
            //dataParams.Append("&client_secret=");
            //dataParams.Append(Global_Define.appIDs[Global_Define.ePlatformType.EPlatformType_Google].app_secret);
            //dataParams.Append("&grant_type=client_credentials");

            string retBody = WebTools.GetReqeustURL(Uri, dataParams.ToString(), false);
            mSeed.Common.mLogger.mLogger.Info(Uri, "billing");

            if (string.IsNullOrEmpty(retBody))
            {
                mSeed.Common.mLogger.mLogger.Info(string.Format("IAB Fail - Google retBody Empty : token = {0}, productID = {1}, appID = {2}", purchaseToken, productID, appID), "billing");
                return Result_Define.eResult.BILLING_TOKEN_INVALIDE;
            }
            else
            {
                JsonObject validateReceipt = JsonObject.Parse(retBody);
                string stateValue;
                if (validateReceipt.TryGetValue(GoogleValidatePurchaseJsonKey, out stateValue))
                {
                    mSeed.Common.mLogger.mLogger.Info(validateReceipt.ToJson(), "billing");
                    Int16 stateFlag;
                    if (!Int16.TryParse(stateValue, out stateFlag))
                        stateFlag = -1;

                    GoogleJsonWebToken.eGooglePurchaseState purchaseState = (GoogleJsonWebToken.eGooglePurchaseState)stateFlag;
                    if (purchaseState != GoogleJsonWebToken.eGooglePurchaseState.BILLING_RESPONSE_RESULT_OK)
                    {
                        mSeed.Common.mLogger.mLogger.Info(string.Format("IAB Fail - Google : token = {0}, productID = {1}, appID = {2}, retjson = {3}", purchaseToken, productID, appID, validateReceipt.ToJson()), "billing");
                        return Result_Define.eResult.BILLING_TOKEN_INVALIDE;
                    }
                    else
                        mSeed.Common.mLogger.mLogger.Info(string.Format("IAB Success - Google : token = {0}, productID = {1}, appID = {2}, retjson = {3}", purchaseToken, productID, appID, validateReceipt.ToJson()), "billing");
                        return Result_Define.eResult.SUCCESS;
                }
                else
                {
                    mSeed.Common.mLogger.mLogger.Info(string.Format("IAB Fail - Google json invalide : token = {0}, productID = {1}, appID = {2}, retBody = {3}", purchaseToken, productID, appID, retBody), "billing");
                    return Result_Define.eResult.BILLING_TOKEN_INVALIDE;
                }
            }
        }

        public const string SCOPE_ANALYTICS_READONLY = "https://www.googleapis.com/auth/analytics.readonly";
        public const string SCOPE_USER_EMAIL = "https://www.googleapis.com/auth/userinfo.email";
        public const string SCOPE_USER_INFO = "https://www.googleapis.com/auth/userinfo.profile";
        public const string SCOPE_OPENID = "https://www.googleapis.com/auth/plus.me";
        public const string SCOPE_PUBLISH = "https://www.googleapis.com/auth/androidpublisher";

        private dynamic GetAccessToken(string clientIdEMail, string keyFilePath, string[] scope)
        {
            try
            {
                mSeed.Common.mLogger.mLogger.Critical(keyFilePath, "billing");
                // certificate
                var certificate = new X509Certificate2(keyFilePath, "notasecret");

                // header
                var header = new { typ = "JWT", alg = "RS256" };

                StringBuilder SB = new StringBuilder();
                foreach (string setScope in scope)
                {
                    SB.Append(setScope);
                    SB.Append(" ");
                }
                // claimset
                var times = GetExpiryAndIssueDate();
                var claimset = new
                {
                    iss = clientIdEMail,
                    scope = SB.ToString(),
                    aud = GoogleURL_GetAccessToken,
                    iat = times[0],
                    exp = times[1],
                };

                JavaScriptSerializer ser = new JavaScriptSerializer();

                // encoded header
                var headerSerialized = ser.Serialize(header);
                var headerBytes = Encoding.UTF8.GetBytes(headerSerialized);
                var headerEncoded = Convert.ToBase64String(headerBytes);

                // encoded claimset
                var claimsetSerialized = ser.Serialize(claimset);
                var claimsetBytes = Encoding.UTF8.GetBytes(claimsetSerialized);
                var claimsetEncoded = Convert.ToBase64String(claimsetBytes);

                // input
                var input = headerEncoded + "." + claimsetEncoded;
                var inputBytes = Encoding.UTF8.GetBytes(input);

                // signiture
                var rsa = certificate.PrivateKey as RSACryptoServiceProvider;
                var cspParam = new CspParameters
                {
                    KeyContainerName = rsa.CspKeyContainerInfo.KeyContainerName,
                    KeyNumber = rsa.CspKeyContainerInfo.KeyNumber == KeyNumber.Exchange ? 1 : 2
                };
                var aescsp = new RSACryptoServiceProvider(cspParam) { PersistKeyInCsp = false };
                var signatureBytes = aescsp.SignData(inputBytes, "SHA256");
                var signatureEncoded = Convert.ToBase64String(signatureBytes);

                // jwt
                var jwt = headerEncoded + "." + claimsetEncoded + "." + signatureEncoded;

                var client = new WebClient();
                client.Encoding = Encoding.UTF8;
                var uri = GoogleURL_GetAccessToken;
                var content = new NameValueCollection();

                content["assertion"] = jwt;
                content["grant_type"] = "urn:ietf:params:oauth:grant-type:jwt-bearer";

                string response = Encoding.UTF8.GetString(client.UploadValues(uri, "POST", content));

                var result = ser.Deserialize<dynamic>(response);

                return result;
            }
            catch (Exception ex)
            {
                mSeed.Common.mLogger.mLogger.Critical(ex.Message);
                throw ex;
            }
        }

        private static int[] GetExpiryAndIssueDate()
        {
            var utc0 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var issueTime = DateTime.UtcNow;

            var iat = (int)issueTime.Subtract(utc0).TotalSeconds;
            var exp = (int)issueTime.AddMinutes(55).Subtract(utc0).TotalSeconds;

            return new[] { iat, exp };
        }

        public static string GoogleOAuthLogin(string appID, string redirectURL, string scope = SCOPE_USER_INFO)
        {
            StringBuilder dataParams = new StringBuilder();
            dataParams.Append("scope=");
            dataParams.Append(scope);
            dataParams.Append("&response_type=");
            dataParams.Append("code");
            dataParams.Append("&access_type=");
            dataParams.Append("offline");
            dataParams.Append("&redirect_uri=");
            dataParams.Append(redirectURL);
            dataParams.Append("&client_id=");
            dataParams.Append(appID);

            return WebTools.GetReqeustURL(GoogleURL_OAuthLogin, dataParams.ToString(), false);                        
        }

        public static JsonObject GetGooglePlusCodeExchange(string appID, string appSecret, string authCode)
        {            
            StringBuilder dataParams = new StringBuilder();
            dataParams.Append("code=");
            dataParams.Append(authCode);
            dataParams.Append("&client_id=");
            dataParams.Append(appID);
            dataParams.Append("&client_secret=");
            dataParams.Append(appSecret);
            dataParams.Append("&grant_type=authorization_code");
            string responseBody = WebTools.GetReqeustURL(GoogleURL_GetAccessToken, dataParams.ToString());
            if(string.IsNullOrEmpty(responseBody))
                responseBody = WebTools.GetReqeustURL(GoogleURL_GetAccessToken_v4, dataParams.ToString());

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

        public static JsonObject GetUserProfileJson(string access_token)
        {
            StringBuilder dataParams = new StringBuilder();
            dataParams.Append("access_token=");
            dataParams.Append(access_token);

            try
            {
                string responseBody = WebTools.GetReqeustURL(GoogleURL_GetUserInfo, dataParams.ToString(), false);
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

        public static string GetUserProfileID(string access_token)
        {
            StringBuilder dataParams = new StringBuilder();
            dataParams.Append("access_token=");
            dataParams.Append(access_token);
            string retID = string.Empty;
            try
            {
                string responseBody = WebTools.GetReqeustURL(GoogleURL_GetUserInfo, dataParams.ToString(), false);
                JsonObject retJson = JsonObject.Parse(responseBody);
                retJson.TryGetValue(GoogleJsonWebToken.GoogleValidateJsonKey, out retID);
                return retID;
            }
            catch (Exception ex)
            {
                JsonObject retJson = new JsonObject();
                mSeed.Common.mLogger.mLogger.Critical(ex.Message);
                mSeed.Common.mLogger.mLogger.GetLoggerInstance().FlushLog();
                return retID;
            }
        }
    }
}
