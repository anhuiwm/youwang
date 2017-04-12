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
using WebPlatform.Tools;

using mSeed.Common;
using mSeed.Platform.Coupon;
using mSeed.Platform.Coupon.DBClass;

namespace WebPlatform
{
    public partial class requestcoupon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] ops = new string[] {
                "coupon_use",
                "coupon_result"
            };

            WebQueryParam queryFetcher = new WebQueryParam();
            //string retJson = "";

            TxnBlock tb = new TxnBlock();
            {

                Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                try
                {
                    
                    
                    CouponManager.GetCouponDB(ref tb);
                    string requestOp = queryFetcher.QueryParam_Fetch("op");
                    JsonObject json = new JsonObject();

                    if (Array.IndexOf(ops, requestOp) >= 0)
                    {
                        queryFetcher.operation = requestOp;
                        if (requestOp.Equals("get_coupon_reward"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long AID = queryFetcher.QueryParam_FetchLong("aid");
                            string couponCode = queryFetcher.QueryParam_Fetch("coupon");
                            long service_access_id = queryFetcher.QueryParam_FetchLong("service_access_id");
                            string service_key = queryFetcher.QueryParam_Fetch("service_key");

                            User_CouponLog couponLog = CouponManager.GetUserCouponLog(ref tb, AID, couponCode);
                            if (couponLog.completeflag == 0)
                            {
                                System_Coupon couponInfo = CouponManager.GetSystemCouponCode(ref tb, couponCode);
                                string rewardList = CouponManager.GetSystemCouponReward(ref tb, couponInfo.Coupon_RewardID1).item_info;
                                if (!string.IsNullOrEmpty(rewardList))
                                    retError = Result_Define.eResult.SUCCESS;
                                json = mJsonSerializer.AddJson(json, "coupon_reward_list", rewardList);
                            }
                            else
                                retError = Result_Define.eResult.COUPONCODE_REWARD_ALREADY;

                        }
                        else if (requestOp.Equals("coupon_use"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long AID = queryFetcher.QueryParam_FetchLong("user_id");
                            int serverGroupID = queryFetcher.QueryParam_FetchInt("server_group_id");
                            string username = queryFetcher.QueryParam_Fetch("nickname");
                            string couponCode = queryFetcher.QueryParam_Fetch("coupon");
                            long service_access_id = queryFetcher.QueryParam_FetchLong("service_access_id");
                            string service_key = queryFetcher.QueryParam_Fetch("service_key");

                            System_Coupon couponInfo = CouponManager.GetSystemCouponCode(ref tb, couponCode);
                            
                            if (string.IsNullOrEmpty(couponInfo.Coupon_Code))
                            {
                                    retError = Result_Define.eResult.COUPONCODE_INCORRECT;
                            }
                            else{
                                if (CouponManager.GetSystemCouponGroup(ref tb, couponInfo.Coupon_Group_ID).Coupon_Active > 0) // 사용 중지 되지 않은 쿠폰그룹
                                {
                                    User_CouponLog couponLog = CouponManager.GetUserCouponLog(ref tb, AID, couponCode);
                                    if (couponLog.idx > 0 && couponLog.completeflag == 1)
                                    {//해당 쿠폰을 사용하고 로그까지 받았을 경우
                                        retError = Result_Define.eResult.COUPONCODE_REWARD_ALREADY;
                                    }
                                    else if (couponLog.idx > 0 && couponLog.completeflag == 0)
                                    {// 사용등록은 되었지만 보상을 못받았을 경우
                                        string rewardList = CouponManager.GetSystemCouponReward(ref tb, couponInfo.Coupon_RewardID1).item_info;
                                        JsonArrayObjects jsonarr = JsonArrayObjects.Parse(rewardList);
                                        string rewardChekList = "";
                                        jsonarr.ForEach(item => {
                                            List<string> values = item.Values.ToList();
                                            if (values.Count > 0)
                                            {   
                                                if(System.Convert.ToInt64(values[0])>0)
                                                    rewardChekList = mJsonSerializer.AddJsonArray(rewardChekList, item.ToJson());
                                            }
                                        });
                                        if (!string.IsNullOrEmpty(rewardChekList))
                                        {
                                            json = mJsonSerializer.AddJson(json, "coupon_reward_list", rewardChekList);
                                            retError = Result_Define.eResult.SUCCESS;
                                        }
                                        else
                                            retError = Result_Define.eResult.COUPONCODE_NOT_EXIST_REWARDS;
                                    }
                                    else
                                    {

                                        if (CouponManager.GetUseCouponGroupCheck(ref tb, AID, couponInfo.Coupon_Group_ID))
                                        {
                                            long useAID = CouponManager.GetCouponUesCheck(ref tb, couponInfo.Coupon_Code); // 해당 쿠폰 사용여부 및 사용자 체크
                                            if (couponInfo.Coupon_Type == 1 && useAID != AID && useAID > 0) // 1:1 다른 사람이 사용
                                                retError = Result_Define.eResult.COUPONCODE_REWARD_ALREADY;
                                            else
                                            {
                                                //쿠폰 1:1 이고 사용자가 없거나 1:n 이고 내가 사용한 로그가 없을 경우
                                                User_CouponLog userCoupon = new User_CouponLog();
                                                userCoupon.AID = AID;
                                                userCoupon.coupon_Code = couponCode;
                                                userCoupon.Coupon_Group_ID = couponInfo.Coupon_Group_ID;
                                                userCoupon.userNickName = username;
                                                userCoupon.server_group_id = serverGroupID;
                                                retError = CouponManager.InertUserCouponLog(ref tb, userCoupon);
                                                if (retError == Result_Define.eResult.SUCCESS)
                                                {
                                                    string rewardList = CouponManager.GetSystemCouponReward(ref tb, couponInfo.Coupon_RewardID1).item_info;
                                                    JsonArrayObjects jsonarr = JsonArrayObjects.Parse(rewardList);
                                                    string rewardChekList = "";
                                                    jsonarr.ForEach(item => {
                                                        List<string> values = item.Values.ToList();
                                                        if (values.Count > 0)
                                                        {   
                                                            if(System.Convert.ToInt64(values[0])>0)
                                                                rewardChekList = mJsonSerializer.AddJsonArray(rewardChekList, item.ToJson());
                                                        }
                                                    });
                                                    if (!string.IsNullOrEmpty(rewardList))
                                                        json = mJsonSerializer.AddJson(json, "coupon_reward_list", rewardChekList);
                                                    else
                                                        retError = Result_Define.eResult.COUPONCODE_NOT_EXIST_REWARDS;
                                                }
                                            }
                                        }
                                        else
                                            retError = Result_Define.eResult.COUPONCODE_REWARD_ALREADY;
                                    }
                                    
                                }
                                else
                                    retError = Result_Define.eResult.COUPONCODE_INCORRECT;
                            }
                        }
                        else if (requestOp.Equals("coupon_result"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long AID = queryFetcher.QueryParam_FetchLong("user_id");
                            string couponCode = queryFetcher.QueryParam_Fetch("coupon");
                            string result = queryFetcher.QueryParam_Fetch("result");

                            User_CouponLog couponLog = CouponManager.GetUserCouponLog(ref tb, AID, couponCode);
                            if (couponLog.completeflag == 0 && mJsonSerializer.GetJsonValue(result, "resultcode").Equals("0"))
                            {
                                retError = CouponManager.SetUserCouponLog(ref tb, AID, couponCode);
                            }
                            else
                                retError = Result_Define.eResult.COUPONCODE_REWARD_ALREADY;

                        }

                        queryFetcher.Render(json, retError);
                    }
                    else
                    {
                        queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(DefineError.System_Unknown_Operation), Result_Define.eResult.System_Unknown_Operation);
                    }
                }
                catch (Exception errorEx)
                {
                    string error = "";
#if DEBUG
                    error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
#endif
                    error = mJsonSerializer.AddJson(error, "Message", mJsonSerializer.ToJsonString(errorEx.Message));
                    queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(error), Result_Define.eResult.System_Exception);
                }
                finally
                {
                    tb.EndTransaction(queryFetcher.Render_errorFlag);
                    tb.Dispose();
                }
            }
        }

    }
}