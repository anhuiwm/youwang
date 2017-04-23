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

/*
[2017_03_07 07:32:12.746] [Info (billing) : mol httpRequst: Uri = https://sandbox-api.mol.com/payout/payments]
[2017_03_07 07:32:12.746] [Info (billing) : mol httpRequst: dataParams = applicationCode=09CPNLtJ1IDNwpXTu8hC7Z3OnUPK8b7R&
 * referenceId=31ol_20170307_203135.595&amount=5000&currencyCode=USD&paymentId=2&returnUrl=120.92.227.117:11000&
 * version=v1&description=product&customerId=1544&signature=6a850601eff592c4ee2c2f125c62386b]
[2017_03_07 07:32:14.978] [Info (billing) : retBody= {"paymentId":"MPO399132","referenceId":"31ol_20170307_203135.595",
 * "paymentUrl":"https://sandbox-global.mol.com/PaymentWall/Checkout/index?token=qipDtsNLDSLY79hEORdluWRDsDc0%2f9HjMvLbw1yZ6MI0oGS7N0mifH%2b0anrxsLdtu2AlYeAVuw%2bLEtdL%2bGld04Xqwi2R7HwtqhJ9yHODd%2fgsgYbFQAr6AX9x%2by5V879a1i%2fWQQVJt%2brm2lgVqNly7Z0lt1BzPtdUQM%2boSKnF%2fJZO7linj53PylBsg2vPK2BxLPY%2bQdSjhIg%3d",
 * "amount":5000,"currencyCode":"USD","version":"v1","signature":"47974612beb3cc0016f0127a5f367526",
 * "applicationCode":"09CPNLtJ1IDNwpXTu8hC7Z3OnUPK8b7R"}]
 * 

iOS API参数
Application Code:	FNWqwwgGrrtR5Bg5l6dcnEigVb3yenTC
Secret Key:	XhtNbbDYOOQo5TG5I621KWFDs03dBKqU
API Version:	v1
	
安卓SDK参数
Application Code:	8DtBu1b8SSOaiGCegeWsG7VOQZDOJQpL
Secret Key:	8VQTRy08pplAFYUBGBtPY7slnwVlgnMi
API Version:	v1
*/

namespace mSeed.Platform
{
    public partial class UserManager
    {
        /// <summary>
        /// IOS的参数在Platform中配置，AOS的参数在客户端配置、
        /// </summary>
        private const string molURL_IABVerify_SandBox = "https://sandbox-api.mol.com/payout/payments";      //测试环境
        private const string molStoreURL_IABVerify_Product = "https://api.mol.com/payout/payments";         //正式环境

        /// <summary>
        ///  是否为测试环境
        /// </summary>
        private const bool isDebug_MOL = true;

        //private const string molPinURL_IABVerify_SandBox = "https://sandbox.api.mol.com/payout/payments/molpoints/pin";         //测试环境
        //private const string molPinStoreURL_IABVerify_Product = "https://api.mol.com/payout/payments/molpoints/pin";            //正式环境
        private const string molPinURL_IABVerify_SandBox = "https://sandbox-api.mol.com/payout/payments";      //测试环境
        private const string molPinStoreURL_IABVerify_Product = "https://api.mol.com/payout/payments";         //正式环境

        public static Result_Define.eResult iOSMolIABVerify(ref JsonObject json, string purchaseToken, string product_id, string webIPandPort, string realToken, string price)
        {

            //mSeed.Common.mLogger.mLogger.Info("iOSMolIABVerify coming", "billing");

            Result_Define.eResult retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;
            string Uri = isDebug_MOL?  molURL_IABVerify_SandBox: molStoreURL_IABVerify_Product;

            string applicationCode = isDebug_MOL ? "09CPNLtJ1IDNwpXTu8hC7Z3OnUPK8b7R" : "FNWqwwgGrrtR5Bg5l6dcnEigVb3yenTC"; 
            string SecretKey = isDebug_MOL? "x9UmhiQgyfVhbMuqR8EU7w3lKrmh807o" : "XhtNbbDYOOQo5TG5I621KWFDs03dBKqU";
            string amount = price;
            string currencyCode = "USD";
            
            string returnUrl = DataManger.GetInstance().GetIosMolResUrl();
            string version = "v1";
            string description = "product";
            string customerId = webIPandPort + "-" + realToken;
           
           
            string data = amount + applicationCode + currencyCode + customerId + description + purchaseToken + returnUrl + version + SecretKey;           
            MD5 md5Hash = MD5.Create();
            string signature = MD5Tool.GetMd5Hash(md5Hash, data);

            string dataParams = "applicationCode=" + applicationCode + "&referenceId=" + purchaseToken + "&amount=" + amount +
                "&currencyCode=" + currencyCode + "&paymentId=" + product_id + "&returnUrl=" + returnUrl + "&version=" + version + "&description=" + description + 
                "&customerId=" + customerId + "&signature=" + signature;

            mSeed.Common.mLogger.mLogger.Info("dataParams::" + dataParams);
            string retBody = WebTools.GetReqeustURL(Uri, dataParams, true);
            mSeed.Common.mLogger.mLogger.Info(string.Format("retBody= {0}", retBody), "billing");
            if (string.IsNullOrEmpty(retBody))
            {
                //mSeed.Common.mLogger.mLogger.Info(string.Format("GetReqeustURL为空{0}", Uri), "billing");
                retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;
            }
            else
            {
                try
                {
                    json = JsonObject.Parse(retBody);
                    //mSeed.Common.mLogger.mLogger.Info(string.Format("返回的JSON{0}", retBody), "billing");
                    string strpaymentUrl = string.Empty;
                    if (json.TryGetValue("paymentUrl", out strpaymentUrl))
                    {
                        retError = Result_Define.eResult.SUCCESS;
                    }
                    return retError;
                }
                catch (Exception ex)
                {
                    JsonObject retJson = new JsonObject();
                    retJson["error"] = ex.Message;
                    retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;
                    return retError;
                }
            }
            return retError;
        }


        public static Result_Define.eResult iOSMolPinIABVerify(ref JsonObject json, string purchaseToken, string product_id, string webIPandPort, string realToken, string price)
        {


            mSeed.Common.mLogger.mLogger.Info("iOSMolPinIABVerify coming", "billing");

            Result_Define.eResult retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;
            string Uri = isDebug_MOL ? molPinURL_IABVerify_SandBox : molPinStoreURL_IABVerify_Product;

            string applicationCode = isDebug_MOL ? "09CPNLtJ1IDNwpXTu8hC7Z3OnUPK8b7R" : "FNWqwwgGrrtR5Bg5l6dcnEigVb3yenTC";
            string SecretKey = isDebug_MOL ? "x9UmhiQgyfVhbMuqR8EU7w3lKrmh807o" : "XhtNbbDYOOQo5TG5I621KWFDs03dBKqU";

            // string amount = price;         
            // string currencyCode = "USD";
            //string returnUrl = "http://xjp.platform.rws.wannaplay.cn:5100/iosMolBilling.aspx";
            string returnUrl = DataManger.GetInstance().GetIosMolPinResUrl();
            string version = "v1";
            string description = "product";
            string customerId = webIPandPort + "-" + realToken;

            //string data = amount + applicationCode + currencyCode + customerId + description + purchaseToken + returnUrl + version + SecretKey;  ;
            string data = applicationCode + customerId + description + purchaseToken + returnUrl + version + SecretKey;
            MD5 md5Hash = MD5.Create();
            string signature = MD5Tool.GetMd5Hash(md5Hash, data);

            //             string dataParams = "applicationCode=" + applicationCode + "&referenceId=" + purchaseToken + "&amount=" + amount +
            //                 "&currencyCode=" + currencyCode + "&paymentId=" + product_id + "&returnUrl=" + returnUrl + "&version=" + version + "&description=" + description +
            //                 "&customerId=" + customerId + "&signature=" + signature;

            string dataParams = "applicationCode=" + applicationCode + "&referenceId=" + purchaseToken + "&paymentId=" + product_id + "&returnUrl=" + returnUrl + "&version=" + version + "&description=" + description +
              "&customerId=" + customerId + "&signature=" + signature;


            mSeed.Common.mLogger.mLogger.Info("data::" + data);
            mSeed.Common.mLogger.mLogger.Info("dataParams::" + dataParams);
            string retBody = WebTools.GetReqeustURL(Uri, dataParams, true);
            mSeed.Common.mLogger.mLogger.Info(string.Format("retBody= {0}", retBody), "billing");

            if (string.IsNullOrEmpty(retBody))
            {
                mSeed.Common.mLogger.mLogger.Info(string.Format("GetReqeustURL为空{0}", Uri), "billing");
                retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;
            }
            else
            {
                try
                {
                    json = JsonObject.Parse(retBody);
                    mSeed.Common.mLogger.mLogger.Info(string.Format("返回的JSON{0}", retBody), "billing");
                    string strpaymentUrl = string.Empty;
                    if (json.TryGetValue("paymentUrl", out strpaymentUrl))
                    {
                        retError = Result_Define.eResult.SUCCESS;
                    }
                    return retError;
                }
                catch (Exception ex)
                {
                    JsonObject retJson = new JsonObject();
                    retJson["error"] = ex.Message;
                    retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;
                    return retError;
                }
            }
            return retError;
        }

    }
}

