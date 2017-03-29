using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using mSeed.Common;
using mSeed.mDBTxnBlock;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace mSeed.Platform
{
    public partial class UserManager
    {
        private const string OneStoreURL_IABVerify_SandBox = "http://iapdev.tstore.co.kr:8082/billIntf/billinglog/billloginquiry.action";
        private const string OneStoreURL_IABVerify_Product = "http://iap.tstore.co.kr:8090/billIntf/billinglog/billloginquiry.action";
        private const string OneStoreValidatePurchaseJsonKey = "status";
        private const string OneStoreValidateContentsType = "application/json";

        public enum eOneStoreRequestState
        {
            BILLING_RESPONSE_RESULT_OK = 0,        // 성공
            BILLING_RESPONSE_PARAM_OR_ERROR = 9,    // 조회결과가 없거나 파라미터 및 시스템 오류
        }
        public enum eOneStorePurchaseState
        {
            BILLING_RESPONSE_RESULT_OK = 0000,      // 성공
            BILLING_RESPONSE_PARAM_ERROR = 1000, // 필수 파라미터 오류
            BILLING_RESPONSE_UNKNOWN_OPERTAION = 2000,  //  정의되지 않은 요청
            BILLING_RESPONSE_REQUEST_COUNT_ERROR = 3000,  //  요청개수 오류
            BILLING_RESPONSE_NOT_COMPLETE = 9100,  //  결제정보 조회결과 없음
            BILLING_RESPONSE_REQUEST_COUNT_OVER = 9200,  //  요청개수 최대값(20) 초과
            BILLING_RESPONSE_SYSTEMERROR = 9999,  //  시스템 오류
        }

        public static Result_Define.eResult OneStoreIABVerify(string purchaseToken, bool isProduct, string appID, DateTime buyDate)
        {
            string Uri = isProduct ? OneStoreURL_IABVerify_Product : OneStoreURL_IABVerify_SandBox;
            Result_Define.eResult retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;

            int bFristCheck = 0;

            while (bFristCheck < 2)
            {
                for (int tryDate = -1; tryDate < 2; tryDate++)
                {
                    DateTime setDate = tryDate == -1 ? buyDate :        // current day
                                        (tryDate == 0 ? buyDate.AddDays(-1) :   // yesterday
                                                        buyDate.AddDays(1))     // tomorrow
                        ;
                    StringBuilder dataParams = new StringBuilder();
                    dataParams.Append("DATE=");
                    dataParams.Append(setDate.ToString("yyyyMMdd"));
                    dataParams.Append("&APPID=");
                    dataParams.Append(appID);
                    dataParams.Append("&TIDCNT=1");
                    dataParams.Append("&TID=");
                    dataParams.Append(purchaseToken);

                    string retBody = WebTools.GetReqeustURL(Uri, dataParams.ToString(), false);
                    mSeed.Common.mLogger.mLogger.Info(Uri, "billing");

                    if (string.IsNullOrEmpty(retBody))
                    {
                        mSeed.Common.mLogger.mLogger.Info(string.Format("IAB Fail - OneStore retBody Empty : token = {0}", purchaseToken), "billing");
                        retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;
                    }
                    else
                    {
                        XmlDocument xml = new XmlDocument(); // XmlDocument 생성
                        xml.LoadXml(retBody);
                        XmlNodeList xnList = xml.GetElementsByTagName("result"); //접근할 노드
                        XmlNode xn = xnList[0];

                        int statusCode = 0;
                        int detailCode = 0;
                        eOneStoreRequestState chkCode = (eOneStoreRequestState)int.Parse(xn["status"].InnerText);
                        eOneStorePurchaseState chkDetail = (eOneStorePurchaseState)int.Parse(xn["detail"].InnerText);

                        //if (int.TryParse(xn["status"].InnerText, out statusCode))
                        //    statusCode = (int)eOneStoreRequestState.BILLING_RESPONSE_PARAM_OR_ERROR;
                        //if (int.TryParse(xn["detail"].InnerText, out detailCode))
                        //    detailCode = (int)eOneStorePurchaseState.BILLING_RESPONSE_SYSTEMERROR;

                        //eOneStoreRequestState chkCode = (eOneStoreRequestState)statusCode;
                        //eOneStorePurchaseState chkDetail = (eOneStorePurchaseState)detailCode;

                        if (chkCode == eOneStoreRequestState.BILLING_RESPONSE_RESULT_OK)
                        {
                            if (chkDetail != eOneStorePurchaseState.BILLING_RESPONSE_RESULT_OK)
                            {
                                mSeed.Common.mLogger.mLogger.Info(string.Format("IAB Fail - OneStore tryDate {4} : token = {0}, code = {1}, detail = {2}, message = {3}", purchaseToken, statusCode, detailCode, xn["message"].InnerText, tryDate), "billing");
                                retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;
                            }
                            else
                            {
                                mSeed.Common.mLogger.mLogger.Info(string.Format("IAB Success - OneStore tryDate {3}  : token = {0}, code = {1}, detail = {2}", purchaseToken, statusCode, detailCode, tryDate), "billing");
                                return Result_Define.eResult.SUCCESS;
                            }
                        }
                        else
                        {
                            mSeed.Common.mLogger.mLogger.Info(string.Format("IAB Fail - OneStore tryDate {4} : token = {0}, code = {1}, detail = {2}, message = {3}", purchaseToken, statusCode, detailCode, xn["message"].InnerText, tryDate), "billing");
                            retError = Result_Define.eResult.BILLING_TOKEN_INVALIDE;
                        }
                    }
                }
                if(bFristCheck < 1)
                    Uri = isProduct ? OneStoreURL_IABVerify_SandBox : OneStoreURL_IABVerify_Product;
                bFristCheck++;
            }
            return retError;
        }

    }
}
