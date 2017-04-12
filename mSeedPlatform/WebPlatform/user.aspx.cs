using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ServiceStack.Text;
using mSeed.Common;
using mSeed.Platform;
using WebPlatform.Tools;
using mSeed.RedisManager;

namespace WebPlatform
{
    public partial class user : System.Web.UI.Page
    {
        private string[] ops = new string[] {
            "get_user_id",
            "set_push_token",

            //Debug
            "access_token_validate",
            "request_token_validate",
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam();
            string requestOp = queryFetcher.QueryParam_Fetch("op");

            try
            {
                if (Array.IndexOf(ops, requestOp) >= 0)
                {
                    JsonObject json = new JsonObject();
                    queryFetcher.operation = requestOp;
                    Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                    long service_access_id = queryFetcher.QueryParam_FetchLong("service_access_id");
                    string service_key = queryFetcher.QueryParam_Fetch("service_key");
                    ePlatformType platform_type = (ePlatformType)queryFetcher.QueryParam_FetchInt("platform_type", (int)ePlatformType.EPlatformType_Google);

                    if (requestOp.Equals("get_user_id"))
                    {
                        string acc_token = queryFetcher.QueryParam_Fetch("acc_token");
                        string auth_token = queryFetcher.QueryParam_Fetch("auth_token");

                        ret_user_platform_id retObj = UserManager.UserPlatformAuth(service_access_id, service_key, platform_type, acc_token, auth_token);
                        if (retObj.platform_idx > 0)
                        {
                            retObj.error = (int)Result_Define.eResult.SUCCESS;
                            string jsonstring = mJsonSerializer.ToJsonString(retObj);
                            queryFetcher.Render(jsonstring, retError);
                        }
                        else
                            retError = Result_Define.eResult.USER_ID_NOT_FOUND;
                    }
                    else if (requestOp.Equals("set_push_token"))
                    {
                        string user_id = queryFetcher.QueryParam_Fetch("platform_user_id");
                        string push_token = queryFetcher.QueryParam_Fetch("push_token");
                        bool serverOff = queryFetcher.QueryParam_FetchInt("push_off", 0) > 0;
                        ePushType push_type = (ePushType)queryFetcher.QueryParam_FetchInt("push_type");

                        if (!(string.IsNullOrEmpty(user_id) && string.IsNullOrEmpty(push_token)))
                        {
                            ret_user_platform_id retObj = UserManager.GetUserPlatformUID(service_access_id, service_key, user_id, platform_type);
                            if (retObj.platform_idx > 0)
                            {
                                retError = (mSeed.Platform.PushNotification.PushManager.SetPushToken(service_access_id, service_key, retObj.platform_idx, push_token, push_type, serverOff)) ?
                                    Result_Define.eResult.SUCCESS : Result_Define.eResult.PUSH_TOKEN_REGISTRATION_FAIL;
                            }
                            else
                                retError = Result_Define.eResult.USER_ID_NOT_FOUND;
                        }
                        else
                            retError = Result_Define.eResult.SYSTEM_PARAM_ERROR;
                    }
                    else if (requestOp.Equals("access_token_validate"))
                    {
                        string acc_token = queryFetcher.QueryParam_Fetch("acc_token");

                        if (platform_type == ePlatformType.EPlatformType_Google)
                        {
                            json = GoogleJsonWebToken.GetUserProfileJson(acc_token);
                            json.TryGetValue(GoogleJsonWebToken.GoogleValidateJsonKey, out acc_token);
                        }
                        else
                        {
                            json = FaceBookJson.GetFaceBookInstance().GetUserProfileJson(acc_token);
                            json.TryGetValue(GoogleJsonWebToken.GoogleValidateJsonKey, out acc_token);
                        }
                    }
                    queryFetcher.Render(json, retError);
                }
            }
            catch (Exception errorEx)
            {
                JsonObject error = new JsonObject();
                error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
                error = mJsonSerializer.AddJson(error, "Message", mJsonSerializer.ToJsonString(errorEx.Message));
                mSeed.Common.mLogger.mLogger.Critical(error.ToJson(), "user");                
                queryFetcher.Render(Result_Define.eResult.System_Exception);
            }

            //mSeed.Platform.UserManager.UserPlatformAuth(mSeed.Common.ePlatformType.EPlatformType_Guest_Editer, "aa9244ec196b037b74e22e52af44d3ed6907ee7ddb5ed22fb1933aeeaaae11db", "pBzJ2VsezU2Sa1n6h8OXnQ==");

            //return;
            ////SystemConfig setConfig = SystemConfig.GetSystemConfigInstance();
            //GoogleJsonWebToken.GetGooglePlusAccessToken();
            ////string ret = GoogleJsonWebToken.GoogleOAuthLogin(@"817162163610-639pkv0va0injkhocf9j15soajskdikj.apps.googleusercontent.com", @"http://29ffdb6c.ngrok.io/Oauth.aspx");
            //mSeed.Common.mLogger.mLogger.GetLoggerInstance().LogfileName = @"log\testlog";
            ////Response.Write(ret);
            //token = @"4/hQMbyRgsIf9ZaR69ZAFNbQW3PhBemDPjI_OsY5qaumg#";
            //appid = @"817162163610-639pkv0va0injkhocf9j15soajskdikj.apps.googleusercontent.com";
            //app_secret = @"riH4s_SaxklZ4qE6QlPzb1dp";
            //mSeed.Common.BackgroundWorker.BackgroundTaskRunner.FireAndForgetTask(() => ThreadTest());
            //mSeed.Common.BackgroundWorker.BackgroundTaskRunner.FireAndForgetTask(() => ThreadTest());
            //mSeed.Common.BackgroundWorker.BackgroundTaskRunner.FireAndForgetTask(() => ThreadTest());
            //mSeed.Common.BackgroundWorker.BackgroundTaskRunner.FireAndForgetTask(() => ThreadTest());

            //mSeed.Common.mLogger.mLogger.GetLoggerInstance().FlushLog();
        }
    }
}