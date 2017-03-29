using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

using ServiceStack.Text;
using mSeed.Common;
using mSeed.Platform;
using WebPlatform.Tools;
using mSeed.RedisManager;
using mSeed.Platform.PushNotification;

namespace WebPlatform
{
    public partial class push : System.Web.UI.Page
    {
        private string[] ops = new string[] {
            "send_push",
            "push_msg_check",
            "get_pushlist",
            "set_push_send_status",
            "delete_fail_token",

            "get_pushlist_test",
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam();
            string requestOp = queryFetcher.QueryParam_Fetch("op");

            try
            {
                if (Array.IndexOf(ops, requestOp) >= 0)
                {
                    JsonObject json = new JsonObject();
                    queryFetcher.operation = requestOp;
                    Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
                    long service_access_id = queryFetcher.QueryParam_FetchLong("service_access_id");
                    string service_key = queryFetcher.QueryParam_Fetch("service_key");
                    ePlatformType platform_type = (ePlatformType)queryFetcher.QueryParam_FetchInt("platform_type");

                    if (requestOp.Equals("send_push"))
                    {
                        string set_token = queryFetcher.QueryParam_Fetch("push_token");
                        List<string> ids = string.IsNullOrEmpty(set_token) ? new List<string>() {
                        "cWVEDrNCCOc:APA91bFFZqyCruASOcOCAePzJPjMP5haB3XOPwUQlJQObgQlDA3gvEX1uSBHZLRp-9xfARmHStMlcmMjDHPjk0i8vlNE_sNqrgBuWozKHoqFMmUUBZp2V0mT-pqsDnEmWRvQ-ABW0RQz"
                        ,"This-is-my-Device-token5"
                    }
                        : new List<string>() {
                        set_token
                    };

                        Dictionary<string, string> msg = new Dictionary<string, string>() { { "title", "test title" }, { "text", "test msg" } };

                        string retBody = FCMManager.SendToAOS(ids, msg);
                        Response.Write(retBody);
                    }
                    else if (requestOp.Equals("push_msg_check"))
                    {
                        ePushStatus setStatus = (ePushStatus)queryFetcher.QueryParam_FetchByte("status", (byte)ePushStatus.Confirm);
                        push_msg_result setObj = new push_msg_result();
                        setObj.push_msg_list = PushManager.GetPushServiceData(setStatus);
                        setObj.error = (int)retError;
                        queryFetcher.Render(mJsonSerializer.ToJsonString(setObj), retError);
                    }
                    else if (requestOp.Equals("get_pushlist"))
                    {
                        long game_id = queryFetcher.QueryParam_FetchLong("game_service_id");
                        int getpage = queryFetcher.QueryParam_FetchInt("getpage");
                        int getcount = queryFetcher.QueryParam_FetchInt("getcount", PushManager.pagelimit);

                        push_token_result setObj = new push_token_result();
                        setObj.push_token_list = PushManager.GetPushTokenList(game_id, getpage, getcount);
                        setObj.error = (int)retError;
                        queryFetcher.Render(mJsonSerializer.ToJsonString(setObj), retError);
                    }
                    else if (requestOp.Equals("set_push_send_status"))
                    {
                        long push_id = queryFetcher.QueryParam_FetchLong("push_id");
                        long game_id = queryFetcher.QueryParam_FetchLong("game_service_id");
                        ePushStatus setStatus = (ePushStatus)queryFetcher.QueryParam_FetchByte("status", (byte)ePushStatus.Finish);

                        retError = PushManager.SetPushStatus(game_id, push_id, setStatus) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                        queryFetcher.Render(json, retError);
                    }
                    else if (requestOp.Equals("delete_fail_token"))
                    {
                        List<long> token_list = mJsonSerializer.JsonToObject<List<long>>(queryFetcher.QueryParam_Fetch("tokens", "[]"));
                        retError = PushManager.DeleteTokens(ref token_list);
                        queryFetcher.Render(json, retError);
                    }
                    else if (requestOp.Equals("get_pushlist_test"))
                    {
                        //RedisManager.GetRedisInstance().GetRedisController().Elog = mSeed.Common.mLogger.LoggerMessage.setLog;
                        //RedisManager.GetRedisInstance().GetRedisController().SetRetryCount = 3;
                        //long gameid = 1;
                        //long chkCount = 0;
                        //try
                        //{
                        //    List<ret_push_token> getObj = PushManager.CreatePushList(gameid, out chkCount);
                        //    int trycount = 0;
                        //    long time_bgn = DateTime.Now.Ticks;
                        //    long time_end = DateTime.Now.Ticks;

                        //    while (chkCount * 10 > trycount)
                        //    {
                        //        PushManager.GetRemainTokenCount(gameid);
                        //        Thread.Sleep(1000);
                        //        trycount++;
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    mSeed.Common.mLogger.mLogger.Error(string.Format("message : {0} , call stack : {1}", ex.Message, ex.StackTrace));
                        //    mSeed.Common.mLogger.mLogger.GetLoggerInstance().FlushLog();

                        //}

                        //while (PushManager.checkPushSetCount[1].Count < chkCount)
                        //{
                        //    time_end = DateTime.Now.Ticks;
                        //    mSeed.Common.mLogger.mLogger.Debug(string.Format("{0} - pagecount : {1} / {2}", ((time_end - time_bgn) / tick_to_sec)
                        //        , PushManager.checkPushSetCount[1].Keys.Count
                        //        , chkCount
                        //        //, PushManager.checkPushSetCount[1].Sum(item => item.Value.Count))
                        //        ));
                        //    mSeed.Common.mLogger.mLogger.GetLoggerInstance().FlushLog();
                        //    Thread.Sleep(1000);
                        //}
                        //Thread.Sleep(3000);

                        //trycount = PushManager.checkPushSetCount[1].Values.Sum(item => item == null ? 0 : item.Count);
                        //while (PushManager.checkPushSetCount[1].Count * 10000 > trycount)
                        //{
                        //    time_end = DateTime.Now.Ticks;
                        //    mSeed.Common.mLogger.mLogger.Debug(string.Format("{0} - pagecount : {1}, totalitemcount : {2}", ((time_end - time_bgn) / tick_to_sec)
                        //        , PushManager.checkPushSetCount[1].Keys.Count
                        //        , PushManager.checkPushSetCount[1].Values.Sum(item => item.Count)
                        //        ));
                        //    mSeed.Common.mLogger.mLogger.GetLoggerInstance().FlushLog();
                        //    Thread.Sleep(1000);
                        //    trycount = PushManager.checkPushSetCount[1].Values.Sum(item => item == null ? 0 : item.Count);
                        //}
                        //Response.Write("finish");
                    }
                    //else if (requestOp.Equals("get_pushtokens"))
                    //{
                    //    TxnBlock TB = new TxnBlock();
                    //    {
                    //        TB.DBConn(SystemConfig.GetSystemConfigInstance().platformDB, SystemConfig.GetSystemConfigInstance().platformDB.SetDBAlias);
                    //        TB.IsoLevel = IsolationLevel.ReadUncommitted;
                    //        //TB.Elog = logger.DBLog;
                    //        chkCount = GetTargetTokenCount(ref TB, game_service_id);

                    //        totalpageCount = (int)Math.Floor((double)(chkCount / pagelimit));
                    //        TB.EndTransaction();
                    //        TB.Dispose();
                    //    }

                    //}
                }
            }
            catch (Exception errorEx)
            {
                JsonObject error = new JsonObject();
                error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
                error = mJsonSerializer.AddJson(error, "Message", mJsonSerializer.ToJsonString(errorEx.Message));
                mSeed.Common.mLogger.mLogger.Critical(error.ToJson(), "push");
                queryFetcher.Render(Result_Define.eResult.System_Exception);
            }
        }
    }
}