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
using ServiceStack.Text;

using OperationTool.Tools;
using OperationTool.DBClass;

namespace OperationTool
{
    public partial class RequestCoupon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] ops = new string[] {
                "coupon_use",
                "get_coupon_reward",
                "coupon_use_result"
            };

            WebQueryParam queryFetcher = new WebQueryParam();
            string retJson = "";

            TxnBlock tb = new TxnBlock();
            {

                Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                try
                {
                    OperationManager.GetOperationDB(ref tb);
                    string requestOp = queryFetcher.QueryParam_Fetch("op");
                    JsonObject json = new JsonObject();

                    if (queryFetcher.ReRequestFlag)
                    {
                        retJson = queryFetcher.ReRequestRender();
                    }
                    else if (Array.IndexOf(ops, requestOp) >= 0)
                    {
                        queryFetcher.operation = requestOp;
                        if (requestOp.Equals("get_coupon_reward"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long AID = queryFetcher.QueryParam_FetchLong("aid");
                            string couponCode = queryFetcher.QueryParam_Fetch("coupon");

                            User_CouponLog couponLog = OperationCouponManager.GetUserCouponLog(ref tb, AID, couponCode);
                            if (couponLog.completeflag == 0)
                            {
                                System_Coupon couponInfo = OperationCouponManager.GetSystemCouponCode(ref tb, couponCode);
                                List<RetCouponReward> rewardList = OperationCouponManager.GetCouponRewardList(ref tb, couponInfo);
                                if (rewardList.Count > 0)
                                    retError = Result_Define.eResult.SUCCESS;
                                json = mJsonSerializer.AddJson(json, "coupon_reward_list", mJsonSerializer.ToJsonString(rewardList));
                            }
                            else
                                retError = Result_Define.eResult.COUPONCODE_REWARD_ALREADY;

                        }
                        else if (requestOp.Equals("coupon_use"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long AID = queryFetcher.QueryParam_FetchLong("aid");
                            int serverGroupID = queryFetcher.QueryParam_FetchInt("serverid");
                            string username = queryFetcher.QueryParam_Fetch("nickname");
                            string couponCode = queryFetcher.QueryParam_Fetch("coupon");

                            System_Coupon couponInfo = OperationCouponManager.GetSystemCouponCode(ref tb, couponCode);
                            User_CouponLog couponLog = OperationCouponManager.GetUserCouponLog(ref tb, AID, couponCode);
                            if (string.IsNullOrEmpty(couponInfo.Coupon_Code) || couponLog.idx > 0)
                                retError = Result_Define.eResult.COUPONCODE_INCORRECT;
                            else
                            {
                                if (OperationCouponManager.GetSystemCouponGroup(ref tb, couponInfo.Coupon_Group_ID).Coupon_Active > 0)
                                {
                                    User_CouponLog userCoupon = new User_CouponLog();
                                    userCoupon.AID = AID;
                                    userCoupon.coupon_Code = couponCode;
                                    userCoupon.Coupon_Group_ID = couponInfo.Coupon_Group_ID;
                                    userCoupon.userNickName = username;
                                    userCoupon.server_group_id = serverGroupID;
                                    retError = OperationCouponManager.InertUserCouponLog(ref tb, userCoupon);
                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        List<RetCouponReward> rewardList = OperationCouponManager.GetCouponRewardList(ref tb, couponInfo);
                                        json = mJsonSerializer.AddJson(json, "coupon_reward_list", mJsonSerializer.ToJsonString(rewardList));
                                    }
                                }
                                else
                                    retError = Result_Define.eResult.COUPONCODE_INCORRECT;
                            }
                        }
                        else if (requestOp.Equals("coupon_use_result"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long AID = queryFetcher.QueryParam_FetchLong("aid");
                            string couponCode = queryFetcher.QueryParam_Fetch("coupon");

                            User_CouponLog couponLog = OperationCouponManager.GetUserCouponLog(ref tb, AID, couponCode);
                            if (couponLog.completeflag == 0)
                            {
                                retError = OperationCouponManager.SetUserCouponLog(ref tb, AID, couponCode);
                            }
                            else
                                retError = Result_Define.eResult.COUPONCODE_REWARD_ALREADY;

                        }

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
#endif
                    error = mJsonSerializer.AddJson(error, "Message", mJsonSerializer.ToJsonString(errorEx.Message));
                    retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(error), Result_Define.eResult.System_Exception);
                }
                finally
                {
                    queryFetcher.SetShowLogMode = tb.EndTransaction(queryFetcher.Render_errorFlag);
                    tb.Dispose();
                } 
            }
        }

    }
}