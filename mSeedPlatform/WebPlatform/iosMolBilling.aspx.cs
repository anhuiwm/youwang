﻿using System;
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
using System.Text;

using System.IO;
using mSeed.mDBTxnBlock;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Collections;

namespace WebPlatform
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam();
            try
            {
                JsonObject json = new JsonObject();
                Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                //string application_Code = "09CPNLtJ1IDNwpXTu8hC7Z3OnUPK8b7R";
                //string secretkey = "x9UmhiQgyfVhbMuqR8EU7w3lKrmh807o";

                mSeed.Common.mLogger.mLogger.Info("call back mol ios is coming", "billing");

                string type = System.Web.HttpContext.Current.Request.RequestType;
                //MyLog4NetInfo.LogInfo("start:::Type:" + type);

                string applicationCode;// = queryFetcher.QueryParam_Fetch("applicationCode");
                string referenceId;// = queryFetcher.QueryParam_Fetch("referenceId");
                string version;// = queryFetcher.QueryParam_Fetch("version");
                string amount;// = queryFetcher.QueryParam_Fetch("amount");
                string currencyCode;// = queryFetcher.QueryParam_Fetch("currencyCode");
                string paymentId;// = queryFetcher.QueryParam_Fetch("paymentId");
                string paymentStatusCode;// = queryFetcher.QueryParam_Fetch("paymentStatusCode");
                string paymentStatusDate;// = queryFetcher.QueryParam_Fetch("paymentStatusDate");
                string channelId;// = queryFetcher.QueryParam_Fetch("channelId");
                string customerId;// = queryFetcher.QueryParam_Fetch("customerId");
                string virtualCurrencyAmount;// = queryFetcher.QueryParam_Fetch("virtualCurrencyAmount");
                string signature;// = queryFetcher.QueryParam_Fetch("signature");

                if (type == "GET")
                {

                    string logreq = System.Web.HttpContext.Current.Request.Url.PathAndQuery;
                    mSeed.Common.mLogger.mLogger.Info("logreq:" + logreq);

                    applicationCode = queryFetcher.QueryParam_Fetch("applicationCode");
                    referenceId = queryFetcher.QueryParam_Fetch("referenceId");
                    version = queryFetcher.QueryParam_Fetch("version");
                    amount = queryFetcher.QueryParam_Fetch("amount");
                    currencyCode = queryFetcher.QueryParam_Fetch("currencyCode");
                    paymentId = queryFetcher.QueryParam_Fetch("paymentId");
                    paymentStatusCode = queryFetcher.QueryParam_Fetch("paymentStatusCode");
                    paymentStatusDate = queryFetcher.QueryParam_Fetch("paymentStatusDate");
                    channelId = queryFetcher.QueryParam_Fetch("channelId");
                    customerId = queryFetcher.QueryParam_Fetch("customerId");
                    virtualCurrencyAmount = queryFetcher.QueryParam_Fetch("virtualCurrencyAmount");
                    signature = queryFetcher.QueryParam_Fetch("signature");
                }
                else
                {
                    string customerIdpost = Request.Form["customerId"];
                    mSeed.Common.mLogger.mLogger.Info(string.Format("customerIdpost:{0}", customerIdpost), "billing");
                    applicationCode = Request.Form["applicationCode"];
                    referenceId = Request.Form["referenceId"];
                    version = Request.Form["version"];
                    amount = Request.Form["amount"];
                    currencyCode = Request.Form["currencyCode"];
                    paymentId = Request.Form["paymentId"];
                    paymentStatusCode = Request.Form["paymentStatusCode"];
                    paymentStatusDate = Request.Form["paymentStatusDate"];
                    channelId = Request.Form["channelId"];
                    customerId = Request.Form["customerId"];
                    virtualCurrencyAmount = Request.Form["virtualCurrencyAmount"];
                    signature = Request.Form["signature"];
                }




                mSeed.Common.mLogger.mLogger.Info(string.Format("applicationCode:{0}", applicationCode), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("referenceId:{0}", referenceId), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("version:{0}", version), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("amount:{0}", amount), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("currencyCode:{0}", currencyCode), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("paymentId:{0}", paymentId), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("paymentStatusCode:{0}", paymentStatusCode), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("paymentStatusDate:{0}", paymentStatusDate), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("channelId:{0}", channelId), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("customerId:{0}", customerId), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("virtualCurrencyAmount:{0}", virtualCurrencyAmount), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("signature:{0}", signature), "billing");


                //if (paymentStatusCode != "00")
                //{
                //    retError = (Result_Define.eResult)1;
                //    queryFetcher.Render(json, retError);
                //    return;
                //}

                referenceId = "494d5ace26ad7cd75e091df398912a97da4f3df84b8af29158";

                string[] subStr = customerId.Split('-');
                //if (subStr.Length < 3)
                //{
                //    mSeed.Common.mLogger.mLogger.Critical("subStr count error");
                //    retError = (Result_Define.eResult)3;
                //    queryFetcher.Render(json, retError);
                //    return;
                //}

                string ip = subStr[0];
                string port = subStr[1];
                string billing_token = subStr[2];

                string httpUrl = "http://" + ip + ":" + port + "/RequestPrivateServer.aspx";
                //string httpUrl = "http://" + "120.92.227.117:11000" + "/RequestPrivateServer.aspx";
                string dataParams = "op=billing_progress&billing_token=" + billing_token  + "&amount="  +virtualCurrencyAmount+ "&Debug=1";


                mSeed.Common.mLogger.mLogger.Info("httpUrl:" + httpUrl);
                mSeed.Common.mLogger.mLogger.Info("dataParams:" + dataParams);

                string retBody = WebTools.GetReqeustURL(httpUrl, dataParams, false);

                //mSeed.Common.mLogger.mLogger.Info("iosMolBilling ok");
               // mSeed.Common.mLogger.mLogger.Info(string.Format("retBody:{0}", retBody), "billing");

                //retError = Result_Define.eResult.SUCCESS;
                System.Web.HttpContext.Current.Response.Write("ok!");
                //queryFetcher.Render(json, retError);

            }
            catch (Exception errorEx)
            {
                JsonObject error = new JsonObject();
                error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
                error = mJsonSerializer.AddJson(error, "Message", mJsonSerializer.ToJsonString(errorEx.Message));
                mSeed.Common.mLogger.mLogger.Critical(error.ToJson(), "billing");
                //queryFetcher.Render(Result_Define.eResult.System_Exception);
                System.Web.HttpContext.Current.Response.Write("ok!");
            }
        }
    }
}