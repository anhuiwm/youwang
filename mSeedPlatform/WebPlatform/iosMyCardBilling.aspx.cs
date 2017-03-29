using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mSeed.Common;
using mSeed.mDBTxnBlock;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using ServiceStack.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.IO;
using System.Web;

using mSeed.Platform;
using WebPlatform.Tools;
using mSeed.RedisManager;

namespace WebPlatform
{
    public partial class iosMyCardBilling : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {



            //WebQueryParam queryFetcher = new WebQueryParam();
            //string requestOp = queryFetcher.QueryParam_Fetch("op");

            //try
            //{

            //    JsonObject json = new JsonObject();
            //    string AuthCode = queryFetcher.QueryParam_Fetch("AuthCode");
               

            //    Result_Define.eResult retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;

            //    string checkmycardAuThURL_Test = "https://test.b2b.mycard520.com.tw/MyBillingPay/api/TradeQuery";//测试环境
            //    string checkmycardAuThURL_Product = "https://b2b.mycard520.com.tw/MyBillingPay/api/TradeQuery";//正式环境

            //    // 將授權碼傳至 Android SDK Interface / MyCard 網站，並開始進行交易
            //    string paymycardURL_Test = "https://test.b2b.mycard520.com.tw/MyBillingPay/api/PaymentConfirm";
            //    string paymycardURL_Product = "https://b2b.mycard520.com.tw/MyBillingPay/api/PaymentConfirm";// 正式

            //    string Uri = checkmycardAuThURL_Test;





            //    mSeed.Common.mLogger.mLogger.Info(string.Format("AuthCode= {0}", AuthCode), "billing");



            //    string dataParams = "AuthCode=" + AuthCode;
            //    string retBody = WebTools.GetReqeustURL(Uri, dataParams, false);
            //    mSeed.Common.mLogger.mLogger.Info(string.Format("retBody= {0}", retBody), "billing");
            //    if (string.IsNullOrEmpty(retBody))
            //    {
            //        retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;
            //        mSeed.Common.mLogger.mLogger.Info(string.Format("GetReqeustURL为空{0}", Uri), "billing");
            //    }
            //    else
            //    {

            //        json = JsonObject.Parse(retBody);
            //        mSeed.Common.mLogger.mLogger.Info(string.Format("返回的JSON{0}", retBody), "billing");
            //        string ReturnCode = string.Empty;
            //        string PayResult = string.Empty;
            //        if (json.TryGetValue("ReturnCode", out ReturnCode))
            //        {
            //            if (ReturnCode == "1")
            //            {
            //                if (json.TryGetValue("PayResult", out AuthCode))
            //                {
            //                    if (PayResult == "3")
            //                    {
            //                        retError = Result_Define.eResult.SUCCESS;
            //                    }
            //                }
            //            }
            //        }

            //        if (retError == Result_Define.eResult.SUCCESS)
            //        {

            //            retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;

            //            string payUrl = paymycardURL_Test;
            //            string paydataParams = "AuthCode=" + AuthCode;
            //            string retBodyPay = WebTools.GetReqeustURL(payUrl, paydataParams, false);
            //            mSeed.Common.mLogger.mLogger.Info(string.Format("retBodyPay= {0}", retBodyPay), "billing");

            //            if (string.IsNullOrEmpty(retBodyPay))
            //            {
            //                retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;
            //                mSeed.Common.mLogger.mLogger.Info(string.Format("GetReqeustpayUrl为空{0}", payUrl), "billing");
            //            }
            //            else
            //            {
            //                json = JsonObject.Parse(retBody);

            //                string ReturnCodePay = string.Empty;

            //                if (json.TryGetValue("ReturnCode", out ReturnCodePay))
            //                {
            //                    if (ReturnCodePay == "1")
            //                    {
            //                         retError = Result_Define.eResult.SUCCESS;                                                                
            //                    }
            //                }
            //            }



                    
            //        }
                    

            //    }


            //    queryFetcher.Render(json, retError);

            //}
            //catch (Exception errorEx)
            //{
            //    JsonObject error = new JsonObject();
            //    error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
            //    error = mJsonSerializer.AddJson(error, "Message", mJsonSerializer.ToJsonString(errorEx.Message));
            //    mSeed.Common.mLogger.mLogger.Critical(error.ToJson(), "billing");
            //    queryFetcher.Render(Result_Define.eResult.System_Exception);
            //}



        }
    }
}