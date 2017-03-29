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

using  System.Text.RegularExpressions;

namespace WebPlatform
{
    public partial class MyCardAuthcode : System.Web.UI.Page
    {

        private static string sha256(string input)
        {
            System.Security.Cryptography.SHA256 sha = new System.Security.Cryptography.SHA256CryptoServiceProvider();
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = sha.ComputeHash(inputBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam();
            try
            {

                Result_Define.eResult retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;
                string key = "412717eb29f2c8d18f3b3960f32088d2";
                //string key = "0";
                string mycardAuThURL_Test = "https://test.b2b.mycard520.com.tw/MyBillingPay/api/AuthGlobal";//测试环境
                string mycardAuThURL_Product = "https://b2b.mycard520.com.tw/MyBillingPay/api/AuthGlobal";//正式环境

                // 將授權碼傳至 Android SDK Interface / MyCard 網站，並開始進行交易
                 string postmycardAuThURL_Test = "https://test.mycard520.com.tw/MyCardPay/";
                string postmycardAuThURL_Product = "https://www.mycard520.com.tw/MyCardPay/";// 正式


                mSeed.Common.mLogger.mLogger.Info("mycard authcode coming", "billing");

                JsonObject json = new JsonObject();
                string token = queryFetcher.QueryParam_Fetch("token");
                string amount = queryFetcher.QueryParam_Fetch("amount");
                string groupid = queryFetcher.QueryParam_Fetch("groupid");
                string product_id = queryFetcher.QueryParam_Fetch("product_id");
                string aid = queryFetcher.QueryParam_Fetch("aid");
                eBillingType BillingType = (eBillingType)queryFetcher.QueryParam_FetchInt("billingType");


                

                string Uri = mycardAuThURL_Test;
                string FacServiceId = "GFD3085";//sdk
                string FacTradeSeq = token;
                string TradeType = "1";//1:Android SDK (手遊適用)2:WEB
                if (BillingType == eBillingType.Global_iOS_MyCard || BillingType == eBillingType.mfun_iOS_Mycard)
                {
                    TradeType = "2";//1:Android SDK (手遊適用)2:WEB
                    FacServiceId = "warnsul";
                }

                string ServerId = groupid;//把服务器的ip端口作为大区标示

                string CustomerId = aid;
                string PaymentType = "";//此參數非必填，參數為空時將依交易金額(Amount)和幣別(Currency)判斷可用的付費方呈現給用戶選擇
                string ItemCode = "";//此參數非必填，參數為空時將依交易金額(Amount)和幣別(Currency)判斷可用的付費方式呈現給用戶選擇
                string ProductName = product_id;//產品名稱用戶購買的產品名稱中文字及全型符號一個字算兩個字元不可以輸入 ' < > 其他皆可
                string Amount = amount;
                string Currency = "TWD";
                string SandBoxMode = "true";


                //mSeed.Common.mLogger.mLogger.Info(string.Format("CustomerId= {0}", CustomerId), "billing");
                //mSeed.Common.mLogger.mLogger.Info(string.Format("Amount= {0}", Amount), "billing");
               // mSeed.Common.mLogger.mLogger.Info(string.Format("FacTradeSeq= {0}", FacTradeSeq), "billing");
               // mSeed.Common.mLogger.mLogger.Info(string.Format("ProductName= {0}", ProductName), "billing");

           

                string temStr = FacServiceId + FacTradeSeq + TradeType + ServerId + CustomerId + PaymentType + ItemCode + ProductName + Amount + Currency + SandBoxMode + key;
                string encodeStr = HttpUtility.UrlEncode(temStr);
                string Hash = sha256(encodeStr);


               // mSeed.Common.mLogger.mLogger.Info(string.Format("temStr= {0}", temStr), "billing");
                //mSeed.Common.mLogger.mLogger.Info(string.Format("encodeStr= {0}", encodeStr), "billing");
               // mSeed.Common.mLogger.mLogger.Info(string.Format("Hash= {0}", Hash), "billing");


                string dataParams = "FacServiceId=" + FacServiceId + "&FacTradeSeq=" + FacTradeSeq + "&TradeType=" + TradeType + "&ItemCode=" + ItemCode + "&PaymentType=" + PaymentType +
                    "&CustomerId=" + CustomerId + "&ProductName=" + ProductName + "&Amount=" + Amount + "&Currency=" + Currency + "&SandBoxMode=" + SandBoxMode + "&ServerId=" + ServerId  + "&Hash=" + Hash;

                //mSeed.Common.mLogger.mLogger.Info(string.Format("dataParams= {0}", dataParams), "billing");

               string retBody = WebTools.GetReqeustURL(Uri, dataParams, false);


                //mSeed.Common.mLogger.mLogger.Info(string.Format("retBody= {0}", retBody), "billing");   
 
                if (string.IsNullOrEmpty(retBody))
                {
                    retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;
                    mSeed.Common.mLogger.mLogger.Info(string.Format("GetReqeustURL为空{0}", Uri), "billing");
                }
                else
                {
                    
                    retBody = retBody.Substring(retBody.IndexOf("AuthCode"));
                    retBody = retBody.Substring(11);
                    string AuthCode = retBody.Substring(0,( retBody.IndexOf(",")-1));

                   // mSeed.Common.mLogger.mLogger.Info(string.Format("AuthCode= {0}", AuthCode), "billing");


                    //if (!string.IsNullOrEmpty(AuthCode))
                    //{

                    //    if (BillingType == eBillingType.Global_iOS_MyCard || BillingType == eBillingType.mfun_iOS_Mycard)
                    //    {
                    //        string UriPost = postmycardAuThURL_Test;
                    //        string dataParamsPost = "AuthCode=" + AuthCode;
                    //        string retBody2 = WebTools.GetReqeustURL(UriPost, dataParams, false);

                    //        mSeed.Common.mLogger.mLogger.Info(string.Format("dataParamsPost= {0}", dataParamsPost), "billing");
                    //        mSeed.Common.mLogger.mLogger.Info(string.Format("retBody2= {0}", retBody2), "billing");

                    //    }

                    //    retError = Result_Define.eResult.SUCCESS;
                    //    json = mJsonSerializer.AddJson(json, "AuthCode", AuthCode);
                    //    mSeed.Common.mLogger.mLogger.Info("mycard authcode ok", "billing");



                    //}


                    retError = Result_Define.eResult.SUCCESS;
                    json = mJsonSerializer.AddJson(json, "AuthCode", AuthCode);
                               
                }

               

                queryFetcher.Render(json, retError);
                

            }
            catch (Exception errorEx)
            {
                JsonObject error = new JsonObject();
                error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
                error = mJsonSerializer.AddJson(error, "Message", mJsonSerializer.ToJsonString(errorEx.Message));
                mSeed.Common.mLogger.mLogger.Critical(error.ToJson(), "billing");
                queryFetcher.Render(Result_Define.eResult.System_Exception);
            }


        }

    }
        
        
        
        
}