using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using mSeed.Platform.Coupon.DBClass;

using mSeed.Common;

namespace mSeed.Platform.Coupon
{
    public class Coupon_Define
    {
        public const string CouponDBName = "coupon";
        public const string GmDBName = "webadmin";

        public const string SystemCouponGroupTableName = "System_Coupon_Group";
        public const string SystemCouponTableName = "System_Coupon";
        public const string SystemCouponRewardTableName = "System_Coupon_Reward";
        public const string UserCouponLogTableName = "User_CouponLog";

        public const string SystemGameItemTableName = "System_game_item";

        public const int CompressionLength = 2048; // 2k byte over
    }

    public class CouponManager
    {
        public static void GetCouponDB(ref TxnBlock TB)
        {
            TB.DBConn(mSeed.Platform.SystemConfig.GetSystemConfigInstance().couponDB, mSeed.Platform.SystemConfig.GetSystemConfigInstance().couponDB.SetDBAlias);
        }

        public static SystemGameItem GetGameItem(ref TxnBlock TB, long gameID, string dbkey = Coupon_Define.CouponDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Where game_service_id = {1}", Coupon_Define.SystemGameItemTableName, gameID);
            SystemGameItem retObj = GenericFetch.FetchFromDB<SystemGameItem>(ref TB, setQuery, dbkey);
            return retObj == null ? new SystemGameItem() : retObj;
        }

        public static System_Coupon_Reward GetSystemCouponReward(ref TxnBlock TB, long rewardID, string dbkey = Coupon_Define.CouponDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Where Coupon_RewardID = {1}", Coupon_Define.SystemCouponRewardTableName, rewardID);
            System_Coupon_Reward retObj = GenericFetch.FetchFromDB<System_Coupon_Reward>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new System_Coupon_Reward();
        }

        public static System_Coupon GetSystemCouponCode(ref TxnBlock TB, string couponCode, string dbkey = Coupon_Define.CouponDBName)
        {
            string setQuery = string.Format(@"Select top 1 * From {0} WITH(NOLOCK, INDEX(IDX_System_Coupon_Check))
                                                Where Coupon_Code = N'{1}' And DATEDIFF(n, GETDATE(), Coupon_Startdate) < 0 And DATEDIFF(n, GETDATE(), Coupon_Enddate) > 0 Order by Coupon_Group_ID DESC"
                                            , Coupon_Define.SystemCouponTableName, couponCode);
            System_Coupon retObj = GenericFetch.FetchFromDB<System_Coupon>(ref TB, setQuery, dbkey);

            return retObj == null ? new System_Coupon() : retObj;
        }

        public static List<System_Coupon> GetSystemCouponCodeList(ref TxnBlock TB, long groupID, string dbkey = Coupon_Define.CouponDBName)
        {
            string setQuery = string.Format(@"Select * From {0} WITH(NOLOCK)
                                                Where Coupon_Group_ID = {1} "
                                            , Coupon_Define.SystemCouponTableName, groupID);
            List<System_Coupon> retObj = GenericFetch.FetchFromDB_MultipleRow<System_Coupon>(ref TB, setQuery, dbkey);

            return retObj.Count == 0 ? new List<System_Coupon>() : retObj;
        }

        public static System_Coupon_Group GetSystemCouponGroup(ref TxnBlock TB, long grouID, string dbkey = Coupon_Define.CouponDBName)
        {
            string setQuery = string.Format(@"Select * From {0} WITH(NOLOCK) Where Coupon_Group_ID = {1}", Coupon_Define.SystemCouponGroupTableName, grouID);
            System_Coupon_Group retObj = GenericFetch.FetchFromDB<System_Coupon_Group>(ref TB, setQuery, dbkey);
            return retObj == null ? new System_Coupon_Group() : retObj;
        }


        public static User_CouponLog GetUserCouponLog(ref TxnBlock TB, long AID, string couponCode, string dbkey = Coupon_Define.CouponDBName)
        {
            string setQuery = string.Format(@"Select top 1 * From {0} WITH(NOLOCK, INDEX(IDX_UserCouponSearch)) Where AID = {1} And coupon_Code = N'{2}' And completeflag = 0 order by idx desc", Coupon_Define.UserCouponLogTableName, AID, couponCode);
            User_CouponLog retObj = GenericFetch.FetchFromDB<User_CouponLog>(ref TB, setQuery, dbkey);
            return retObj == null ? new User_CouponLog() : retObj;
        }

        public static User_CouponLog GetUserCouponLog(ref TxnBlock TB, long AID, long grouID, string couponCode, string dbkey = Coupon_Define.CouponDBName)
        {
            string setQuery = string.Format(@"Select * From {0} WITH(NOLOCK, INDEX(IDX_UserCouponSearch)) Where AID = {1} And coupon_Code = N'{2}' And Coupon_Group_ID = {3}", Coupon_Define.UserCouponLogTableName, AID, couponCode, grouID);
            User_CouponLog retObj = GenericFetch.FetchFromDB<User_CouponLog>(ref TB, setQuery, dbkey);
            return retObj == null ? new User_CouponLog() : retObj;
        }

        public static long GetCouponUesCheck(ref TxnBlock TB, string couponCode, string dbkey = Coupon_Define.CouponDBName)
        {
            string setQuery = string.Format(@"Select * From {0} WITH(NOLOCK) Where coupon_Code = N'{1}'", Coupon_Define.UserCouponLogTableName, couponCode);
            User_CouponLog retObj = GenericFetch.FetchFromDB<User_CouponLog>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.AID;
        }

        public static bool GetUseCouponGroupCheck(ref TxnBlock TB, long AID, long groupID, string dbkey = Coupon_Define.CouponDBName)
        {
            string setQuery = string.Format(@"Select * From {0} WITH(NOLOCK, INDEX(IDX_UseGroupCheck)) Where AID = {1} And Coupon_Group_ID = {2}", Coupon_Define.UserCouponLogTableName, AID, groupID);
            User_CouponLog retObj = GenericFetch.FetchFromDB<User_CouponLog>(ref TB, setQuery, dbkey);
            return retObj == null ? true : false;
        }

        public static Result_Define.eResult InertUserCouponLog(ref TxnBlock TB, User_CouponLog setData, string dbkey = Coupon_Define.CouponDBName)
        {
            string setQuery = string.Format(@"Insert Into {0} (AID, userNickName, server_group_id, Coupon_Group_ID, coupon_Code, completeflag, complete_date, Reg_date)
                                                        Values({1}, N'{2}', {3}, {4}, N'{5}', 0, dateadd(d,-1,getdate()), getdate())"
                                                        , Coupon_Define.UserCouponLogTableName, setData.AID, setData.userNickName, setData.server_group_id, setData.Coupon_Group_ID, setData.coupon_Code);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static Result_Define.eResult SetUserCouponLog(ref TxnBlock TB, long AID, string couponCode, string dbkey = Coupon_Define.CouponDBName)
        {
            string setQuery = string.Format(@"Update {0} Set completeflag = 1, complete_date = getdate() Where AID = {1} And coupon_Code = N'{2}'"
                                                        , Coupon_Define.UserCouponLogTableName, AID, couponCode);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static Result_Define.eResult SetCouponActive(ref TxnBlock TB, long groupID, string dbkey = Coupon_Define.CouponDBName)
        {
            string setQuery = string.Format(@"Update {0} Set Coupon_Active = 0, Discontinue_date = getdate() Where Coupon_Group_ID = {1}"
                                                        , Coupon_Define.SystemCouponGroupTableName, groupID);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static Result_Define.eResult InsertCoupon(ref TxnBlock TB, System_Coupon_Group setData, System_Coupon setCoupon
                                                            , string setReward1, string dbkey = Coupon_Define.CouponDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            long groupID = GetCouponGroupID(ref TB);
            if (groupID > 0)
            {
                setCoupon.Coupon_Group_ID = groupID;

                string setQuery = string.Format(@"Insert Into {0} (Coupon_Group_ID, game_service_id, Coupon_Type, Coupon_Active, Coupon_Title, Coupon_Memo, Coupon_Num, Discontinue_date, Reg_date, Reg_id)
                                                        Values({1}, {2}, N'{3}', 1, N'{4}', N'{5}', {6}, dateadd(d, -1, getdate()), getdate(), N'{7}')"
                                                        , Coupon_Define.SystemCouponGroupTableName, groupID, setData.game_service_id, setData.Coupon_Type, setData.Coupon_Title, setData.Coupon_Memo, setData.Coupon_Num, setData.Reg_id);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;

                if (retError == Result_Define.eResult.SUCCESS && !string.IsNullOrEmpty(setReward1))
                {
                    long rewardID = GetCouponRewardID(ref TB);
                    if (rewardID > 0)
                    {
                        setCoupon.Coupon_RewardID1 = rewardID;
                        retError = InsertCouponReward(ref TB, rewardID, setData.game_service_id, setReward1);
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

        private static Result_Define.eResult InsertCouponCode(ref TxnBlock TB, System_Coupon setCoupon, int makeCount, string dbkey = Coupon_Define.CouponDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            int defaultLength = 16;
            string setQuery = "";
            if (setCoupon.Coupon_Type == 1)
            {
                List<string> setData = SerialNumberGenerator(setCoupon.Coupon_Group_ID, defaultLength, makeCount);
                foreach (string item in setData)
                {
                    setQuery = string.Format(@"Insert Into {0} (Coupon_Group_ID, game_service_id, Coupon_Code, Coupon_Type, Coupon_RewardID1, Coupon_RewardID2, Coupon_RewardID3, Coupon_RewardID4, Coupon_Startdate, Coupon_Enddate)
                                                    Values ({1}, {2}, N'{3}', {4}, {5}, {6}, {7}, {8}, N'{9}', N'{10}')"
                                                        , Coupon_Define.SystemCouponTableName, setCoupon.Coupon_Group_ID, setCoupon.game_service_id, item, setCoupon.Coupon_Type
                                                        , setCoupon.Coupon_RewardID1, setCoupon.Coupon_RewardID2, setCoupon.Coupon_RewardID3, setCoupon.Coupon_RewardID4
                                                        , setCoupon.Coupon_Startdate.ToString("yyyy-MM-dd HH:mm:ss"), setCoupon.Coupon_Enddate.ToString("yyyy-MM-dd HH:mm:ss"));
                    retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                    if (retError != Result_Define.eResult.SUCCESS)
                        break;
                }
            }
            else
            {
                setQuery = string.Format(@"Insert Into {0} (Coupon_Group_ID, game_service_id, Coupon_Code, Coupon_Type, Coupon_RewardID1, Coupon_RewardID2, Coupon_RewardID3, Coupon_RewardID4, Coupon_Startdate, Coupon_Enddate)
                                                    Values ({1}, {2}, N'{3}', {4}, {5}, {6}, {7}, {8}, N'{9}', N'{10}')"
                                                        , Coupon_Define.SystemCouponTableName, setCoupon.Coupon_Group_ID, setCoupon.game_service_id, setCoupon.Coupon_Code, setCoupon.Coupon_Type
                                                        , setCoupon.Coupon_RewardID1, setCoupon.Coupon_RewardID2, setCoupon.Coupon_RewardID3, setCoupon.Coupon_RewardID4
                                                        , setCoupon.Coupon_Startdate.ToString("yyyy-MM-dd HH:mm:ss"), setCoupon.Coupon_Enddate.ToString("yyyy-MM-dd HH:mm:ss"));
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            }
            return retError;
        }

        private static Result_Define.eResult InsertCouponReward(ref TxnBlock TB, long rewardID, long game_service_id, string setReward, string dbkey = Coupon_Define.CouponDBName)
        {
            string setQuery = string.Format(@"Insert Into {0} (game_service_id, Coupon_RewardID, item_info)
                                                        Values({1}, {2}, N'{3}')"
                                             , Coupon_Define.SystemCouponRewardTableName, game_service_id, rewardID, setReward);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        private static long GetCouponGroupID(ref TxnBlock TB, string dbkey = Coupon_Define.CouponDBName)
        {
            string setQuery = string.Format("Select IsNull(Max(Coupon_Group_ID),0)+1 as number From {0} WiTH(NOLOCK)", Coupon_Define.SystemCouponGroupTableName);
            Number retObj = GenericFetch.FetchFromDB<Number>(ref TB, setQuery, dbkey);
            return retObj.number;
        }

        private static long GetCouponRewardID(ref TxnBlock TB, string dbkey = Coupon_Define.CouponDBName)
        {
            string setQuery = string.Format("Select IsNull(Max(Coupon_RewardID),0)+1 as number From {0} WiTH(NOLOCK)", Coupon_Define.SystemCouponRewardTableName);
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
                string strGroupID = groupID.ToString();
                if (groupID.ToString().Length > 4 && groupID > 16)
                {
                    strGroupID = String.Format("{0:X}", groupID);
                    strGroupID = strGroupID.Length > 4 ? string.Join("", strGroupID.OrderBy(groupItem => Guid.NewGuid()).Take(4).ToList()) : strGroupID;
                }

                string SerialNumber = string.Format("{0}{1}", strGroupID, Guid.NewGuid().ToString("N").Substring(0, keyLength - strGroupID.Length).ToUpper());
                for (int iCount = 0; iCount < keyLength; iCount += 4)
                    newSerialNumber = newSerialNumber + SerialNumber.Substring(iCount, 4) + "-";
                newSerialNumber = newSerialNumber.Substring(0, newSerialNumber.Length - 1);
                long numChk = 0;
                
                bool isNum = long.TryParse(SerialNumber, out numChk);
                if (!keyList.ContainsKey(newSerialNumber) && !isNum)
                    keyList.Add(newSerialNumber, SerialNumber);
                else
                    breakCount++;
            }

            return keyList.Values.ToList<string>();
        }

    }
}