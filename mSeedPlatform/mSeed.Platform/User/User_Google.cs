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
    public partial class UserManager
    {
        private static string GetGooglePlatformID(string service_app_id, string service_secret, string request_token)
        {
            JsonObject retJson = GoogleJsonWebToken.GetGooglePlusCodeExchange(service_app_id, service_secret, request_token);
            string acctoken = string.Empty;
            
            if (retJson.TryGetValue(GoogleJsonWebToken.GoogleAccessTokenJsonKey, out acctoken))
            {
                retJson = GoogleJsonWebToken.GetUserProfileJson(acctoken);
                if (!retJson.TryGetValue(GoogleJsonWebToken.GoogleValidateJsonKey, out acctoken))
                    acctoken = string.Empty;
            }

            return acctoken;
        }
    }
}
