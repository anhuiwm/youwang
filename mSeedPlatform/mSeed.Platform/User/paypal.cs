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
        private const string ExpresCheckoutPaypalURL = "http://107.150.101.9:8980/merchant-sdk-php-master/samples/ExpressCheckout/GetExpressCheckout.php";
        private const string ExpressPaypalURL = "http://107.150.101.9:8980/merchant-sdk-php-master/samples/ExpressCheckout/DoExpressCheckout.php";

        public static Result_Define.eResult PayPalIABVerify(ref JsonObject json, string purchaseToken, string product_id)
        {

            mSeed.Common.mLogger.mLogger.Info("paypalIABVerify coming", "billing");


            Result_Define.eResult retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;
            string Uri = ExpresCheckoutPaypalURL;
            string dataParams = "token=" + purchaseToken;

            mSeed.Common.mLogger.mLogger.Info(string.Format("dataParams= {0}", dataParams), "billing");

            string retBody = WebTools.GetReqeustURL(Uri, dataParams, true);
            mSeed.Common.mLogger.mLogger.Info(string.Format("retBody= {0}", retBody), "billing");

            

            if (string.IsNullOrEmpty(retBody))
            {
                retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;
                mSeed.Common.mLogger.mLogger.Info(string.Format("GetReqeustURL为空{0}", Uri), "billing");
                return retError;
            }
            else
            {        
                retBody = retBody.Substring(retBody.IndexOf("PayerStatus"));
                retBody = retBody.Substring(12);
                string PayerStatus = retBody.Substring(0, (retBody.IndexOf(",") - 1));

                mSeed.Common.mLogger.mLogger.Info(string.Format("PayResult= {0}", PayerStatus), "billing");


                if (PayerStatus == "3")
                {
                    mSeed.Common.mLogger.mLogger.Info("paypalPayResult ok", "billing");

                    retError = Result_Define.eResult.SUCCESS;
                }



            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;

                string payUrl = ExpressPaypalURL;
                string paydataParams = "token=" + purchaseToken + "&payerID=111" + "&paymentAction=111" + "&currencyCode=Sale" + "&amt=500";//
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

                    retBodyPay = retBodyPay.Substring(retBodyPay.IndexOf("Ack"));
                    retBodyPay = retBodyPay.Substring(4);
                    string Ack = retBodyPay.Substring(0, (retBodyPay.IndexOf(",") - 1));

                    mSeed.Common.mLogger.mLogger.Info(string.Format("Ack= {0}", Ack), "billing");


                    if (Ack == "3")
                    {
                        mSeed.Common.mLogger.mLogger.Info("paypalAck ok", "billing");

                        retError = Result_Define.eResult.SUCCESS;
                    }
                    mSeed.Common.mLogger.mLogger.Info("Ack ok", "billing");
                    retError = Result_Define.eResult.SUCCESS;

                }
            }

            return retError;
        }

    }
}

