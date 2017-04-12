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
using mSeed.RedisManager;

using System.Text.RegularExpressions;

namespace mSeed.Platform
{
    public partial class UserManager
    {
        private const string checkmycardAuThURL_Test = "https://test.b2b.mycard520.com.tw/MyBillingPay/api/TradeQuery";//测试环境
        private const string checkmycardAuThURL_Product = "https://b2b.mycard520.com.tw/MyBillingPay/api/TradeQuery";//正式环境

        // 將授權碼傳至 Android SDK Interface / MyCard 網站，並開始進行交易
        private const string paymycardURL_Test = "https://test.b2b.mycard520.com.tw/MyBillingPay/api/PaymentConfirm";
        private const string paymycardURL_Product = "https://b2b.mycard520.com.tw/MyBillingPay/api/PaymentConfirm";// 正式


        public static Result_Define.eResult MyCardIABVerify(ref JsonObject json, string purchaseToken, string product_id)
        {

            mSeed.Common.mLogger.mLogger.Info("MyCardIABVerify coming", "billing");

          
            Result_Define.eResult retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;
            string Uri = checkmycardAuThURL_Test;
            string dataParams = "AuthCode=" + purchaseToken;
            string retBody = WebTools.GetReqeustURL(Uri, dataParams, true);

            mSeed.Common.mLogger.mLogger.Info(string.Format("dataParams= {0}", dataParams), "billing");
           // mSeed.Common.mLogger.mLogger.Info(string.Format("Uri= {0}", Uri), "billing");
           
            mSeed.Common.mLogger.mLogger.Info(string.Format("retBody= {0}", retBody), "billing");
          
            if (string.IsNullOrEmpty(retBody))
            {
                retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;
                mSeed.Common.mLogger.mLogger.Info(string.Format("GetReqeustURL为空{0}", Uri), "billing");
                return retError;
            }
            else
            {
                retBody = retBody.Substring(retBody.IndexOf("PayResult"));
                retBody = retBody.Substring(12);
                string PayResult = retBody.Substring(0, (retBody.IndexOf(",") - 1));

                mSeed.Common.mLogger.mLogger.Info(string.Format("PayResult= {0}", PayResult), "billing");
               
              
                if (PayResult == "3")
                {
                    mSeed.Common.mLogger.mLogger.Info("PayResult ok", "billing");
                  
                    retError = Result_Define.eResult.SUCCESS;
                }
                

            
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;

                string payUrl = paymycardURL_Test;
                string paydataParams = "AuthCode=" + purchaseToken;
                string retBodyPay = WebTools.GetReqeustURL(payUrl, paydataParams, false);
                mSeed.Common.mLogger.mLogger.Info(string.Format("retBodyPay= {0}", retBodyPay), "billing");

                if (string.IsNullOrEmpty(retBodyPay))
                {
                    retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;
                    mSeed.Common.mLogger.mLogger.Info(string.Format("GetReqeustpayUrl为空{0}", payUrl), "billing");
                    return retError;
                }
                else
                {
                    retBodyPay = retBodyPay.Substring(retBodyPay.IndexOf("ReturnCode"));
                    retBodyPay = retBodyPay.Substring(13);
                    string ReturnCodePay = retBodyPay.Substring(0, (retBodyPay.IndexOf(",") - 1));

                    mSeed.Common.mLogger.mLogger.Info(string.Format("ReturnCodePay= {0}", ReturnCodePay), "billing");

                 
                    if (ReturnCodePay == "1")
                    {
                        mSeed.Common.mLogger.mLogger.Info("ReturnCode ok", "billing");
                 
                        retError = Result_Define.eResult.SUCCESS;                                                                
                    };
                    
                }        
           }
       
           return retError;
       }  

    }
}

