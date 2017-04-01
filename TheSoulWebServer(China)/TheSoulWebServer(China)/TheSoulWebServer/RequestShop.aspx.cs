using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using mSeed.mDBTxnBlock;
using mSeed.RedisManager;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Global;
using TheSoulWebServer.Tools;
using ServiceStack.Text;
using logWeb;
using TheSoulWebServer.Platform;

namespace TheSoulWebServer
{
    public partial class RequestShop : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] ops = new string[] {
                // not use gacha in china version
                "draw_gacha",
                "free_gacha",
                "free_premium_gacha",

                // replace gacha contents to treasure box
                "treasure_box_list",
                "buy_treasure_box",

                "shop_list",
                "shop_package_list",
                "shop_cheap_package_list",
                "buy_package",
                "buy_cheap_package",
                "buy_shop_item",    

                "create_billing_id",       // request create billing id (md5 + md5 auth key)
                "buy_billing_cash",     // billing
                "get_product_id",       // for taiwan billing code

                "shop_buy_count_reset",
                "blackmarket_soul_disassemble",

                "get_vip_reward",
                "aosbilling_success",

            };

            WebQueryParam queryFetcher = new WebQueryParam();
            string logreq = System.Web.HttpContext.Current.Request.Url.PathAndQuery;
            string type = Request.RequestType;
            string opPost = Request.Form["op"];
            string opGet = Request.QueryString["op"];
            MyLog4NetInfo.LogInfo("type:" + type + "!" + opPost + "!" + opGet + "!");
            MyLog4NetInfo.LogInfo("req:" + logreq);
            string retJson = "";
            TxnBlock tb = new TxnBlock();
            long AID = 0;

            try
            {
                queryFetcher.TxnBlockInit(ref tb, ref AID);
                string requestOp = queryFetcher.QueryParam_Fetch("op");
                JsonObject json = new JsonObject();

                MyLog4NetInfo.LogInfo("requestOp:"+requestOp+"&AID:"+AID.ToString());
                if (queryFetcher.ReRequestFlag)
                {
                    retJson = queryFetcher.ReRequestRender();
                }
                else if (Array.IndexOf(ops, requestOp) >= 0)
                {
                    Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;

                    DealOp.dealRequestOp(ref tb, ref queryFetcher, ref requestOp, ref json, ref  AID, ref retError);
   
                    retJson = queryFetcher.Render(json.ToJson(), retError);
                }
                else
                {
                    retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(DefineError.System_Unknown_Operation), Result_Define.eResult.System_Unknown_Operation);
                }
            }
            catch (Exception errorEx)
            {
                string error = "";
    #if DEBUG
                error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
    #else
                if (queryFetcher.SetDebugMode)
                    error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
    #endif

                error = mJsonSerializer.AddJson(error, "Message", mJsonSerializer.ToJsonString(errorEx.Message));
                retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(error), Result_Define.eResult.System_Exception);
            }
            finally
            {
                //if (AID == 3)
                //{
                //    string DBLog = mJsonSerializer.ToJsonString(queryFetcher.GetDBLog());
                //    //error = mJsonSerializer.AddJson(error, "Message", mJsonSerializer.ToJsonString(errorEx.Message));
                //    Response.Write(DBLog);
                //}

                //if (AID > 0)
                //    queryFetcher.CheckSnail_ID(ref tb, AID);
                queryFetcher.SetShowLogMode = tb.EndTransaction(queryFetcher.Render_errorFlag);
                MyLog4NetInfo.LogInfo("resp:" + retJson);
                queryFetcher.ErrorLogWrite(retJson, ref tb);
                tb.Dispose();
            } 
        }
    }
    
}