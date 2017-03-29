using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using mSeed.Common;
using mSeed.mDBTxnBlock;
using System.Data;
using System.Data.SqlClient;
using ServiceStack.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Net;

namespace mSeed.Platform
{
    public class AppleGameCenterAuth
    {
        public string PlayerID { get; set; }
        public string BundleID { get; set; }
        public string PublicKeyUrl { get; set; }
        public string Signature { get; set; }
        public string Salt { get; set; }
        public ulong Timestamp { get; set; }
    }

    public partial class UserManager
    {
        private const string AppleURL_IABVerify_SandBox = "https://sandbox.itunes.apple.com/verifyReceipt";
        private const string AppleURL_IABVerify_Product = "https://buy.itunes.apple.com/verifyReceipt";
        private const string AppleValidateReceiptJsonKey = "receipt";
        private const string AppleValidateAppBidJsonKey = "bid";
        private const string AppleValidateAppProductJsonKey = "product_id";
        private const string AppleValidatePurchaseJsonKey = "status";
        private const string AppleValidateContentsType = "application/json";            // old style
        private const string AppleValidateContentsType2 = "application/x-www-form-urlencoded";  // ios 7 style

        public enum eApplePurchaseState
        {
            BILLING_RESPONSE_RESULT_OK                                  = 0,        // Success
            BILLING_RESPONSE_JSON_ERROR                                 = 21000,    // The App Store could not read the JSON object you provided.
            BILLING_RESPONSE_RECEIPT_MISSING                            = 21002,    // The data in the receipt-data property was malformed or missing.
            BILLING_RESPONSE_RECEIPT_AUTH_FAIL                          = 21003,    // The receipt could not be authenticated
            BILLING_RESPONSE_SECRET_NOT_MATCHED                         = 21004,    // The shared secret you provided does not match the shared secret on file for your account. Only returned for iOS 6 style transaction receipts for auto-renewable subscriptions.
            BILLING_RESPONSE_SERVER_ERROR                               = 21005,    // The receipt server is not currently available.
            BILLING_RESPONSE_RECEIPT_EXPIRED                            = 21006,    // This receipt is valid but the subscription has expired. When this status code is returned to your server, the receipt data is also decoded and returned as part of the response. Only returned for iOS 6 style transaction receipts for auto-renewable subscriptions.
            BILLING_RESPONSE_RECEIPT_ENVIRONMENT_NOT_MATCHED_TEST       = 21007,    // This receipt is from the test environment, but it was sent to the production environment for verification. Send it to the test environment instead.
            BILLING_RESPONSE_RECEIPT_ENVIRONMENT_NOT_MATCHED_PRODUCTION = 21008,    // This receipt is from the production environment, but it was sent to the test environment for verification. Send it to the production environment instead.
        }

        public static Result_Define.eResult AppleIABVerify(string purchaseToken, bool isProduct, string app_id, string product_id)
        {
            string Uri = isProduct ? AppleURL_IABVerify_Product : AppleURL_IABVerify_SandBox;
            StringBuilder dataParams = new StringBuilder();
            JsonObject json = new JsonObject();
            json["receipt-data"] = purchaseToken;
            dataParams.Append(json.ToJson());

            string retBody = WebTools.GetReqeustURL(Uri, dataParams.ToString(), true, AppleValidateContentsType2);
            mSeed.Common.mLogger.mLogger.Info(Uri, "billing");

            if (string.IsNullOrEmpty(retBody))
            {
                mSeed.Common.mLogger.mLogger.Info(string.Format("IAB Fail - Apple retBody Empty : token = {0}", purchaseToken), "billing");
                return Result_Define.eResult.BILLING_TOKEN_INVALIDE;
            }
            else
            {
                JsonObject validateReceipt = JsonObject.Parse(retBody);
                string checkReceipt;
                string stateValue;
                if (validateReceipt.TryGetValue(AppleValidatePurchaseJsonKey, out stateValue))
                {
                    Int16 stateFlag;
                    if (!Int16.TryParse(stateValue, out stateFlag))
                        stateFlag = -1;

                    eApplePurchaseState purchaseState = (eApplePurchaseState)stateFlag;

                    // for enviroment retry
                    if (purchaseState == eApplePurchaseState.BILLING_RESPONSE_RECEIPT_ENVIRONMENT_NOT_MATCHED_TEST || purchaseState == eApplePurchaseState.BILLING_RESPONSE_RECEIPT_ENVIRONMENT_NOT_MATCHED_PRODUCTION)
                    {
                        Uri = isProduct ? AppleURL_IABVerify_SandBox : AppleURL_IABVerify_Product;
                        retBody = WebTools.GetReqeustURL(Uri, dataParams.ToString(), true, AppleValidateContentsType);
                        mSeed.Common.mLogger.mLogger.Info(Uri, "billing");

                        if (string.IsNullOrEmpty(retBody))
                        {
                            mSeed.Common.mLogger.mLogger.Info(string.Format("IAB Fail - Apple retBody Empty : token = {0}", purchaseToken), "billing");
                            return Result_Define.eResult.BILLING_TOKEN_INVALIDE;
                        }
                        mSeed.Common.mLogger.mLogger.Info(Uri, "billing");

                        validateReceipt = JsonObject.Parse(retBody);
                        if (validateReceipt.TryGetValue(AppleValidatePurchaseJsonKey, out stateValue))
                        {                            
                            if (!Int16.TryParse(stateValue, out stateFlag))
                                stateFlag = -1;
                            purchaseState = (eApplePurchaseState)stateFlag;

                            if (purchaseState != eApplePurchaseState.BILLING_RESPONSE_RESULT_OK)
                            {
                                mSeed.Common.mLogger.mLogger.Info(string.Format("IAB Fail - Apple : token = {0}, retjson = {1}", purchaseToken, validateReceipt.ToJson()), "billing");
                                return Result_Define.eResult.BILLING_TOKEN_INVALIDE;
                            }
                            else
                            {
                                if (validateReceipt.TryGetValue(AppleValidateReceiptJsonKey, out checkReceipt))
                                {
                                    JsonObject chkApp = JsonObject.Parse(checkReceipt);
                                    if (!chkApp[AppleValidateAppBidJsonKey].Equals(app_id) || !chkApp[AppleValidateAppProductJsonKey].Equals(product_id))
                                        return Result_Define.eResult.BILLING_TOKEN_INVALIDE;
                                }

                                mSeed.Common.mLogger.mLogger.Info(string.Format("IAB Success - Apple : token = {0}, retjson = {1}", purchaseToken, validateReceipt.ToJson()), "billing");
                                return Result_Define.eResult.SUCCESS;
                            }
                        }
                        else
                        {
                            mSeed.Common.mLogger.mLogger.Info(string.Format("IAB Fail - Apple json invalide : token = {0}, retBody = {1}", purchaseToken, retBody), "billing");
                            return Result_Define.eResult.BILLING_TOKEN_INVALIDE;
                        }
                    }else if (purchaseState != eApplePurchaseState.BILLING_RESPONSE_RESULT_OK)
                    {
                        mSeed.Common.mLogger.mLogger.Info(string.Format("IAB Fail - Apple : token = {0}, retjson = {1}", purchaseToken, validateReceipt.ToJson()), "billing");
                        return Result_Define.eResult.BILLING_TOKEN_INVALIDE;
                    }
                    else
                    {

                        if (validateReceipt.TryGetValue(AppleValidateReceiptJsonKey, out checkReceipt))
                        {
                            JsonObject chkApp = JsonObject.Parse(checkReceipt);
                            if (!chkApp[AppleValidateAppBidJsonKey].Equals(app_id) || !chkApp[AppleValidateAppProductJsonKey].Equals(product_id))
                                return Result_Define.eResult.BILLING_TOKEN_INVALIDE;
                        }

                        mSeed.Common.mLogger.mLogger.Info(string.Format("IAB Success - Apple : token = {0}, retjson = {1}", purchaseToken, validateReceipt.ToJson()), "billing");
                        return Result_Define.eResult.SUCCESS;
                    }
                }
                else
                {
                    mSeed.Common.mLogger.mLogger.Info(string.Format("IAB Fail - Apple json invalide : token = {0}, retBody = {1}", purchaseToken, retBody), "billing");
                    return Result_Define.eResult.BILLING_TOKEN_INVALIDE;
                }
            }
        }

        public static string iOSGameCenterAuth(AppleGameCenterAuth data)
        {
            string retValue;
            if (ValidateSignature(data))
            {
                retValue = "ok";
            }else
            {
                retValue = "fail";
            }
            return retValue;
        }

        private static bool ValidateSignature(AppleGameCenterAuth auth)
        {
            try
            {
                // apple test certificate url https://sandbox.gc.apple.com/public-key/gc-sb-2.cer
                // apple real certificate url https://service.gc.apple.com/public-key/gc-sb-2.cer
                var cert = GetCertificate(auth.PublicKeyUrl);   
                if (cert.Verify())      // hash verifying
                {
                    var csp = cert.PublicKey.Key as RSACryptoServiceProvider;
                    if(csp != null)
                    {
                        var sha256 = new SHA256Managed();
                        var sig = ConcatSignature(auth.PlayerID, auth.BundleID, auth.Timestamp, auth.Salt);
                        var hash = sha256.ComputeHash(sig);
                        if (csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA256"), Convert.FromBase64String(auth.Signature)))
                        {
                            // verify success;
                            return true;
                        }
                    }
                }
                // verify fail;
                return false;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                //exception verify fail;
                return false;
            }
        }

        private static byte[] ToBigEndian(ulong value)
        {
            var buffer = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                buffer[7 - i] = unchecked((byte)(value & 0xff));
                value = value >> 8;
            }
            return buffer;
        }

        private static X509Certificate2 GetCertificate(string url)
        {
            var client = new WebClient();
            var rawData = client.DownloadData(url);
            return new X509Certificate2(rawData);
        }

        private static byte[] ConcatSignature(string playerID, string bundleID, ulong timestemp, string salt)
        {
            var data = new List<byte>();
            data.AddRange(Encoding.UTF8.GetBytes(playerID));
            data.AddRange(Encoding.UTF8.GetBytes(bundleID));
            data.AddRange(ToBigEndian(timestemp));
            data.AddRange(Convert.FromBase64String(salt));
            return data.ToArray();
        }
    }
}
