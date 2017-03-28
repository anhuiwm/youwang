using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Net;

using TheSoul.DataManager;
using TheSoul.DataManager.Global;
using mSeed.RedisManager;
using ServiceStack.Text;

public class GoogleJsonWebToken
{
    private const string GoogleValideJsonKey = "sub";
    private const string GoogleAccessTokenJsonKey = "access_token";
    private const string GoogleExpireJsonKey = "expires_in";
    // singleton Google+ AccessToken
    public static JsonObject _GooglePlusAccessToken = null;
    // Synchronization object to the Lock
    private static object _GooglePlusAccessToken_syncLock = new object();
    /// <summary> get CustomRedisPooledClient Instance </summary>

    private static DateTime ExpireTime = DateTime.Now;
    private const int ExpireInterval = -60;

    public static string GetGooglePlusAccessToken()
    {
        if (_GooglePlusAccessToken == null || ExpireTime.AddSeconds(ExpireInterval) < DateTime.Now)
        {
            lock (_GooglePlusAccessToken_syncLock)
            {
                if (_GooglePlusAccessToken == null)
                {
                    string serviceAccountID = "817162163610-compute@developer.gserviceaccount.com";
                    string keyFilePath = "D:\\Work\\1_Dark_Blaze\\google_auth_key\\Google Play Android Developer-27d86138bd13.p12";
                    string[] setScope = { GoogleJsonWebToken.SCOPE_USER_EMAIL, GoogleJsonWebToken.SCOPE_USER_INFO, GoogleJsonWebToken.SCOPE_openid, "https://www.googleapis.com/auth/androidpublisher" };
                    var auth = GoogleJsonWebToken.GetAccessToken(
                                    serviceAccountID,
                                    keyFilePath,
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
            }
        }

        return _GooglePlusAccessToken[GoogleAccessTokenJsonKey];
    }

    public static Result_Define.eResult GoogleIABVerify(string purchaseToken)
    {
        string Uri = string.Format("https://www.googleapis.com/androidpublisher/v1.1/applications/{0}/inapp/{1}/purchases/{2}"
                                            , Global_Define.appIDs[Global_Define.ePlatformType.EPlatformType_Google].package_name
                                            , Global_Define.appIDs[Global_Define.ePlatformType.EPlatformType_Google].app_id
                                            , purchaseToken);
        StringBuilder dataParams = new StringBuilder();
        dataParams.Append("access_token=");
        dataParams.Append(GetGooglePlusAccessToken());
        //dataParams.Append("&client_secret=");
        //dataParams.Append(Global_Define.appIDs[Global_Define.ePlatformType.EPlatformType_Google].app_secret);
        //dataParams.Append("&grant_type=client_credentials");

        string retBody = GlobalManager.GetReqeustURL(Uri, dataParams.ToString(), false);                        
        if(string.IsNullOrEmpty(retBody))
            return Result_Define.eResult.SHOP_BILLING_ID_NOT_FOUND;

        return Result_Define.eResult.SUCCESS;
    }

    public const string SCOPE_ANALYTICS_READONLY = "https://www.googleapis.com/auth/analytics.readonly";
    public const string SCOPE_USER_EMAIL = "https://www.googleapis.com/auth/userinfo.email";
    public const string SCOPE_USER_INFO = "https://www.googleapis.com/auth/userinfo.profile";
    public const string SCOPE_openid = "https://www.googleapis.com/auth/plus.me";
    
    public static dynamic GetAccessToken(string clientIdEMail, string keyFilePath, string[] scope)
    {
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
            aud = "https://accounts.google.com/o/oauth2/token",
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
        var uri = "https://accounts.google.com/o/oauth2/token";
        var content = new NameValueCollection();

        content["assertion"] = jwt;
        content["grant_type"] = "urn:ietf:params:oauth:grant-type:jwt-bearer";

        string response = Encoding.UTF8.GetString(client.UploadValues(uri, "POST", content));

        var result = ser.Deserialize<dynamic>(response);

        return result;
    }

    private static int[] GetExpiryAndIssueDate()
    {
        var utc0 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        var issueTime = DateTime.UtcNow;

        var iat = (int)issueTime.Subtract(utc0).TotalSeconds;
        var exp = (int)issueTime.AddMinutes(55).Subtract(utc0).TotalSeconds;

        return new[] { iat, exp };
    }
}
