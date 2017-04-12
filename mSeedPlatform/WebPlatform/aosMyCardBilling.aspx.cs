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
using System.Text;

using System.IO;
using mSeed.mDBTxnBlock;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
namespace WebPlatform
{
    public partial class aosMyCardBilling : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam();
            try
            {
                JsonObject json = new JsonObject();
                Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                string secretkey = "VOTa0mV66Yor4n8M8TcSVbXolztKpS5N";

                mSeed.Common.mLogger.mLogger.Info("mol aos is coming", "billing");

                string applicationCode = queryFetcher.QueryParam_Fetch("applicationCode");
                string referenceId = queryFetcher.QueryParam_Fetch("referenceId");
                string paymentId = queryFetcher.QueryParam_Fetch("paymentId");
                string version = queryFetcher.QueryParam_Fetch("version");
                string virtualCurrencyAmount = queryFetcher.QueryParam_Fetch("virtualCurrencyAmount");

                string paymentStatusCode = queryFetcher.QueryParam_Fetch("paymentStatusCode");
                string paymentStatusDate = queryFetcher.QueryParam_Fetch("paymentStatusDate");
                string amount = queryFetcher.QueryParam_Fetch("amount");
                string currencyCode = queryFetcher.QueryParam_Fetch("currencyCode");
                string customerId = queryFetcher.QueryParam_Fetch("customerId");
                string signature = queryFetcher.QueryParam_Fetch("signature");

                mSeed.Common.mLogger.mLogger.Info(string.Format("applicationCode:{0}", applicationCode), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("referenceId:{0}", referenceId), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("paymentId:{0}", paymentId), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("version:{0}", version), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("virtualCurrencyAmount:{0}", virtualCurrencyAmount), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("paymentStatusCode:{0}", paymentStatusCode), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("paymentStatusDate:{0}", paymentStatusDate), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("amount:{0}", amount), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("currencyCode:{0}", currencyCode), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("customerId:{0}", customerId), "billing");
                mSeed.Common.mLogger.mLogger.Info(string.Format("signature:{0}", signature), "billing");

                if (paymentStatusCode != "00")
                {
                    retError = (Result_Define.eResult)1;
                    queryFetcher.Render(json, retError);
                    return;
                }

                //string data = amount + applicationCode + currencyCode + customerId + paymentId + paymentStatusCode + paymentStatusDate + referenceId + version + virtualCurrencyAmount + secretkey;
                //MD5 md5Hash = MD5.Create();
                //string md5str = MD5Tool.GetMd5Hash(md5Hash, data);
                //if (md5str != signature)
                //{
                //    mSeed.Common.mLogger.mLogger.Info(string.Format("md5str:{0}", md5str), "billing");
                //    retError = (Result_Define.eResult)2;
                //    queryFetcher.Render(json, retError);
                //    return;

                //}

                string[] subStr = customerId.Split('-');
                if (subStr.Length < 6)
                {
                    mSeed.Common.mLogger.mLogger.Critical("subStr count error");
                    retError = (Result_Define.eResult)3;
                    queryFetcher.Render(json, retError);
                    return;
                }
                string ip = subStr[0];
                string port = subStr[1];
                string aid = subStr[2];

                string cid = subStr[3];
                string product_id = subStr[4];
                string buy_protuct_type = subStr[5];
                int buytype = 0;
                int.TryParse(buy_protuct_type, out buytype);
                buytype = buytype + 2;

                //http://127.0.0.1:11000/RequestShop.aspx?op=aosbilling_success&aid=227&cid=186&billing_type=22002&product_id=android_mfunsoul_45
                //&billing_uid=aa9244ec196b037b74e22e52af44d3edef5db07b81ec2f474ed8b34bd932218f&buy_protuct_type=2&product_GoodsID=3&Debug=1
                //+ "&product_GoodsID=" + product_GoodsID
                string httpUrl = "http://" + ip + ":" + port + "/RequestShop.aspx";
                string dataParams = "op=aosbilling_success&aid=" +
                    aid + "&cid=" + cid + "&buy_protuct_type=" + buytype + "&billing_uid=" + referenceId +
                    "&billing_type=21002" + "&product_id=" + product_id + "&Debug=1";

                string retBody = WebTools.GetReqeustURL(httpUrl, dataParams, false);
                retError = Result_Define.eResult.SUCCESS;

                mSeed.Common.mLogger.mLogger.Info(string.Format("retBody:{0}", retBody), "billing");

                mSeed.Common.mLogger.mLogger.Critical("aosMolBilling ok");
                mSeed.Common.mLogger.mLogger.Critical("httpUrl:" + httpUrl);
                mSeed.Common.mLogger.mLogger.Critical("dataParams:" + dataParams);
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