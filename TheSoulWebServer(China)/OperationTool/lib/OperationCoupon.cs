using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Text;
using System.Data;

using TheSoul.DataManager;
using OperationTool.DBClass;

namespace OperationTool
{
    public class OperationCouponManager
    {

        public static List<RetCouponReward> GetCouponRewardList(ref TxnBlock TB, System_Coupon setCoupon){
            List<RetCouponReward> RewardList = new List<RetCouponReward>();
            if (setCoupon.Coupon_RewardID1 > 0)
                GetSystemCouponRewardList(ref TB, setCoupon.Coupon_RewardID1).ForEach(setItem =>
                {
                    RewardList.Add(new RetCouponReward(setItem));
                });
            if (setCoupon.Coupon_RewardID2 > 0)
                GetSystemCouponRewardList(ref TB, setCoupon.Coupon_RewardID2).ForEach(setItem =>
                {
                    RewardList.Add(new RetCouponReward(setItem));
                });
            if (setCoupon.Coupon_RewardID3 > 0)
                GetSystemCouponRewardList(ref TB, setCoupon.Coupon_RewardID3).ForEach(setItem =>
                {
                    RewardList.Add(new RetCouponReward(setItem));
                });
            if (setCoupon.Coupon_RewardID4 > 0)
                GetSystemCouponRewardList(ref TB, setCoupon.Coupon_RewardID4).ForEach(setItem =>
                {
                    RewardList.Add(new RetCouponReward(setItem));
                });

            return RewardList;
        }

        public static List<System_Coupon_Reward> GetSystemCouponRewardList(ref TxnBlock TB, long rewardID, string dbkey = Operation_Define.OperationDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Where Coupon_RewardID = {1}", Operation_Define.SystemCouponRewardTableName, rewardID);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<System_Coupon_Reward>(ref TB, setQuery, dbkey);
        }

        public static System_Coupon GetSystemCouponCode(ref TxnBlock TB, string couponCode, string dbkey = Operation_Define.OperationDBName)
        {
            string setQuery = string.Format(@"Select * From {0} WITH(NOLOCK, INDEX(IDX_System_Coupon_Check))
                                                Where Coupon_Code = N'{1}' And DATEDIFF(s, GETDATE(), Coupon_Startdate) < 0 And DATEDIFF(s, GETDATE(), Coupon_Enddate) > 0 "
                                            , Operation_Define.SystemCouponTableName, couponCode);
            System_Coupon retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<System_Coupon>(ref TB, setQuery, dbkey);

            return retObj == null ? new System_Coupon() : retObj;
        }

        public static System_Coupon_Group GetSystemCouponGroup(ref TxnBlock TB, long grouID, string dbkey = Operation_Define.OperationDBName)
        {
            string setQuery = string.Format(@"Select * From {0} WITH(NOLOCK) Where Coupon_Group_ID = {1}", Operation_Define.SystemCouponGroupTableName, grouID);
            System_Coupon_Group retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<System_Coupon_Group>(ref TB, setQuery, dbkey);
            return retObj == null ? new System_Coupon_Group() : retObj;
        }

        public static User_CouponLog GetUserCouponLog(ref TxnBlock TB, long AID, string couponCode, string dbkey = Operation_Define.OperationDBName)
        {
            string setQuery = string.Format(@"Select * From {0} WITH(NOLOCK, INDEX(IDX_UserCouponSearch)) Where AID = {1} And coupon_Code = N'{2}'", Operation_Define.UserCouponLogTableName, AID, couponCode);
            User_CouponLog retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<User_CouponLog>(ref TB, setQuery, dbkey);
            return retObj == null ? new User_CouponLog() : retObj;
        }

        public static Result_Define.eResult InertUserCouponLog(ref TxnBlock TB, User_CouponLog setData, string dbkey = Operation_Define.OperationDBName)
        {
            string setQuery = string.Format(@"Insert Into {0} (AID, userNickName, server_group_id, Coupon_Group_ID, coupon_Code, completeflag, complete_date, Reg_date)
                                                        Values({1}, N'{2}', {3}, {4}, N'{5}', 0, dateadd(d,-1,getdate()), getdate())"
                                                        , Operation_Define.UserCouponLogTableName, setData.AID, setData.userNickName, setData.server_group_id, setData.Coupon_Group_ID, setData.coupon_Code);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static Result_Define.eResult SetUserCouponLog(ref TxnBlock TB, long AID, string couponCode, string dbkey = Operation_Define.OperationDBName)
        {
            string setQuery = string.Format(@"Update {0} Set completeflag = 1, complete_date = getdate() Where AID = {1} And coupon_Code = N'{2}'"
                                                        , Operation_Define.UserCouponLogTableName, AID, couponCode);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static Result_Define.eResult InsertCoupon(ref TxnBlock TB, System_Coupon_Group setData, System_Coupon setCoupon
                                                            , List<System_Coupon_Reward> setReward1, List<System_Coupon_Reward> setReward2
                                                            , List<System_Coupon_Reward> setReward3, List<System_Coupon_Reward> setReward4, string dbkey = Operation_Define.OperationDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            long groupID = GetCouponGroupID(ref TB);
            if (groupID > 0)
            {
                setCoupon.Coupon_Group_ID = groupID;

                string setQuery = string.Format(@"Insert Into {0} (Coupon_Group_ID, Coupon_Type, Coupon_Active, Coupon_Title, Coupon_Memo, Coupon_Num, Discontinue_date, Reg_date, Reg_id)
                                                        Values({1}, N'{2}', 1, N'{3}', N'{4}', {5}, dateadd(d, -1, getdate()), getdate(), N'{6}')"
                                                        , Operation_Define.SystemCouponGroupTableName, groupID, setData.Coupon_Type, setData.Coupon_Title, setData.Coupon_Memo, setData.Coupon_Num, setData.Reg_id);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;

                if (retError == Result_Define.eResult.SUCCESS && setReward1.Count > 0)
                {
                    long rewardID = GetCouponRewardID(ref TB);
                    if (rewardID > 0)
                    {
                        setCoupon.Coupon_RewardID1 = rewardID;
                        retError = InsertCouponReward(ref TB, rewardID, setReward1);
                    }
                    else
                        retError = Result_Define.eResult.DB_ERROR;
                }
                if (retError == Result_Define.eResult.SUCCESS && setReward2.Count > 0)
                {
                    long rewardID = GetCouponRewardID(ref TB);
                    if (rewardID > 0)
                    {
                        setCoupon.Coupon_RewardID2 = rewardID;
                        retError = InsertCouponReward(ref TB, rewardID, setReward2);
                    }
                    else
                        retError = Result_Define.eResult.DB_ERROR;
                }
                if (retError == Result_Define.eResult.SUCCESS && setReward3.Count > 0)
                {
                    long rewardID = GetCouponRewardID(ref TB);
                    if (rewardID > 0)
                    {
                        setCoupon.Coupon_RewardID3 = rewardID;
                        retError = InsertCouponReward(ref TB, rewardID, setReward3);
                    }
                    else
                        retError = Result_Define.eResult.DB_ERROR;
                }
                if (retError == Result_Define.eResult.SUCCESS && setReward4.Count > 0)
                {
                    long rewardID = GetCouponRewardID(ref TB);
                    if (rewardID > 0)
                    {
                        setCoupon.Coupon_RewardID4 = rewardID;
                        retError = InsertCouponReward(ref TB, rewardID, setReward4);
                    }
                    else
                        retError = Result_Define.eResult.DB_ERROR;
                }
                //couponCode
                if (retError == Result_Define.eResult.SUCCESS)
                {
                    retError = InsertCouponCode(ref TB, setCoupon, setData.Coupon_Num);
                }
            }
            else
                retError = Result_Define.eResult.DB_ERROR;
            return retError;
        }

        private static Result_Define.eResult InsertCouponCode(ref TxnBlock TB, System_Coupon setCoupon, int makeCount, string dbkey = Operation_Define.OperationDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            int defaultLength = 16;
            string setQuery = "";
            List<string> setData = SerialNumberGenerator(setCoupon.Coupon_Group_ID, defaultLength, makeCount);
            foreach (string item in setData)
            {
                setQuery = string.Format(@"Insert Into {0} (Coupon_Group_ID, Coupon_Code, Coupon_Type, Coupon_RewardID1, Coupon_RewardID2, Coupon_RewardID3, Coupon_RewardID4, Coupon_Startdate, Coupon_Enddate)
                                                    Values ({1}, N'{2}', {3}, {4}, {5}, {6}, {7}, N'{8}', N'{9}')"
                                                    , Operation_Define.SystemCouponTableName, setCoupon.Coupon_Group_ID, item, setCoupon.Coupon_Type
                                                    , setCoupon.Coupon_RewardID1, setCoupon.Coupon_RewardID2, setCoupon.Coupon_RewardID3, setCoupon.Coupon_RewardID4
                                                    , setCoupon.Coupon_Startdate.ToString("yyyy-MM-dd HH:mm:ss"), setCoupon.Coupon_Enddate.ToString("yyyy-MM-dd HH:mm:ss"));
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }
            return retError;
        }

        private static Result_Define.eResult InsertCouponReward(ref TxnBlock TB, long rewardID, List<System_Coupon_Reward> setReward, string dbkey = Operation_Define.OperationDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            string setQuery = "";
            foreach (System_Coupon_Reward item in setReward)
            {
                setQuery = string.Format(@"Insert Into {0} (Coupon_RewardID, ItemIndex, Item_ID, Item_Level, Item_Grade, Item_Num)
                                                        Values({1}, {2}, {3}, {4}, {5}, {6})"
                                                            , Operation_Define.SystemCouponRewardTableName, rewardID, item.ItemIndex, item.Item_ID, item.Item_Level, item.Item_Grade, item.Item_Num);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }
            return retError;
        }

        private static long GetCouponGroupID(ref TxnBlock TB, string dbkey = Operation_Define.OperationDBName)
        {
            string setQuery = string.Format("Select IsNull(Max(Coupon_Group_ID),0)+1 as number From {0} WiTH(NOLOCK)", Operation_Define.SystemCouponGroupTableName);
            Number retObj = GenericFetch.FetchFromDB<Number>(ref TB, setQuery, dbkey);
            return retObj.number;
        }

        private static long GetCouponRewardID(ref TxnBlock TB, string dbkey = Operation_Define.OperationDBName)
        {
            string setQuery = string.Format("Select IsNull(Max(Coupon_RewardID),0)+1 as number From {0} WiTH(NOLOCK)", Operation_Define.SystemCouponRewardTableName);
            Number retObj = GenericFetch.FetchFromDB<Number>(ref TB, setQuery, dbkey);
            return retObj.number;
        }

        public static List<string> SerialNumberGenerator(long groupID, int keyLength, int getCount)
        {
            Dictionary<string, string> keyList = new Dictionary<string, string>();
            int breakCount = 0;
            while (keyList.Count < getCount && breakCount < getCount)
            {
                string newSerialNumber = "";
                string SerialNumber =string.Format("{0}{1}", groupID, Guid.NewGuid().ToString("N").Substring(0, keyLength-groupID.ToString().Length).ToUpper());
                for (int iCount = 0; iCount < keyLength; iCount += 4)
                    newSerialNumber = newSerialNumber + SerialNumber.Substring(iCount, 4) + "-";
                newSerialNumber = newSerialNumber.Substring(0, newSerialNumber.Length - 1);

                if (!keyList.ContainsKey(newSerialNumber))
                    keyList.Add(newSerialNumber, SerialNumber);
                else
                    breakCount++;
            }

            return keyList.Values.ToList<string>();
        }
    }
}