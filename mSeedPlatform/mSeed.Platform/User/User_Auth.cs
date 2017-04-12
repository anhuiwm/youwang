using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using mSeed.Common;
using mSeed.mDBTxnBlock;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using mSeed.Platform.Cache;

namespace mSeed.Platform
{
    public partial class UserManager
    {
        private const string log_tag = "user";

        private static ret_user_platform_id GetPlatformUID(ref TxnBlock TB, string platform_user_id, mSeed.Common.ePlatformType platform_type)
        {
            ret_user_platform_id retObj = new ret_user_platform_id();
            SqlCommand command_userInfo = new SqlCommand();
            command_userInfo.CommandText = "System_Get_UID";
            command_userInfo.Parameters.Add("@platform_type", SqlDbType.Int).Value = (int)platform_type;
            command_userInfo.Parameters.Add("@platform_user_id", SqlDbType.NVarChar, 128).Value = platform_user_id;
            command_userInfo.Parameters.Add("@user_account_status", SqlDbType.NVarChar, 128).Value = 0;
            var outputResult = new SqlParameter("@ret_result", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            var outputID = new SqlParameter("@ret_platform_user_id", SqlDbType.NVarChar, 128) { Direction = ParameterDirection.Output };
            command_userInfo.Parameters.Add(outputResult);
            command_userInfo.Parameters.Add(outputID);

            if (TB.ExcuteSqlStoredProcedure(ref command_userInfo))
            {
                retObj.platform_user_id = System.Convert.ToString(outputID.Value);
                retObj.platform_idx = System.Convert.ToInt64(outputResult.Value);
            }

            command_userInfo.Dispose();
            return retObj;
        }

        public static ret_user_platform_id GetUserPlatformUID(long service_access_id, string service_key, string platform_user_id, mSeed.Common.ePlatformType platform_type)
        {
            ret_user_platform_id retObj = null;
            PlatformLogger logger = new PlatformLogger(log_tag);
            TxnBlock TB = new TxnBlock();
            {
                TB.DBConn(SystemConfig.GetSystemConfigInstance().platformDB, SystemConfig.GetSystemConfigInstance().platformDB.SetDBAlias);
                TB.IsoLevel = IsolationLevel.ReadUncommitted;
                TB.Elog = logger.DBLog;

                game_access_auth authInfo = Server_Cache.GetSystemCacheInstance().GetCacheAuthInfo(service_access_id, service_key);
                if (authInfo.game_service_id > 0)
                    retObj = GetPlatformUID(ref TB, platform_user_id, platform_type);
            }
            TB.EndTransaction();
            TB.Dispose();
            logger.Dispose();
            return retObj == null ? new ret_user_platform_id() : retObj;
        }

        public static ret_user_platform_id UserPlatformAuth(long service_access_id, string service_key, mSeed.Common.ePlatformType platform_type, string acc_token, string auth_token = "")
        {
            ret_user_platform_id retObj = null;
            PlatformLogger logger = new PlatformLogger(log_tag);
            TxnBlock TB = new TxnBlock();
            {
                TB.DBConn(SystemConfig.GetSystemConfigInstance().platformDB, SystemConfig.GetSystemConfigInstance().platformDB.SetDBAlias);
                TB.IsoLevel = IsolationLevel.ReadUncommitted;
                TB.Elog = logger.DBLog;

                game_service_info serviceInfo;

                switch (platform_type)
                {
                    case mSeed.Common.ePlatformType.EPlatformType_Guest:
                    case mSeed.Common.ePlatformType.EPlatformType_Guest_Editer:
                        user_guest_auth_id userid = GetGuestUserID(ref TB, acc_token, auth_token);

                        if (userid == null)
                            userid = CreateGuestUser(ref TB, acc_token, auth_token, platform_type);

                        if (userid != null && !string.IsNullOrEmpty(userid.auth_md5_id))
                            retObj = GetPlatformUID(ref TB, userid.auth_md5_id, platform_type);
                        break;
                    case mSeed.Common.ePlatformType.EPlatformType_Google:
                        serviceInfo = GameServiceManager.GetGameServiceInfo(service_access_id, service_key, (int)platform_type);
                        if (serviceInfo != null && serviceInfo.game_service_id > 0 && !string.IsNullOrEmpty(acc_token))         // by access token                 
                        {
                            string googleID = GoogleJsonWebToken.GetUserProfileID(acc_token);
                            if (!string.IsNullOrEmpty(googleID))
                                retObj = GetPlatformUID(ref TB, googleID, platform_type);
                        }
                        // by request token
                        else if (serviceInfo != null && serviceInfo.game_service_id > 0 && string.IsNullOrEmpty(acc_token) && !string.IsNullOrEmpty(auth_token))
                        {
                            string googleID = GetGooglePlatformID(serviceInfo.service_app_id, serviceInfo.service_secret, auth_token);
                            if (!string.IsNullOrEmpty(googleID))
                                retObj = GetPlatformUID(ref TB, googleID, platform_type);
                        }
                        break;
                    case mSeed.Common.ePlatformType.EPlatformType_Facebook:
                    case mSeed.Common.ePlatformType.EPlatformType_iosFacebook:
                    case mSeed.Common.ePlatformType.EPlatformType_mfun_aosFacebook:
                    case mSeed.Common.ePlatformType.EPlatformType_mfun_iosFacebook:
                    case  mSeed.Common.ePlatformType.EPlatformType_yuenan_aosFacebook:
                    case mSeed.Common.ePlatformType.EPlatformType_yuenan_iosFacebook:
                        serviceInfo = GameServiceManager.GetGameServiceInfo(service_access_id, service_key, (int)platform_type);
                        if (serviceInfo != null && serviceInfo.game_service_id > 0 && !string.IsNullOrEmpty(acc_token))         // by access token                 
                        {
                            string facebookID = GetFaceBookPlatformID(serviceInfo.service_app_id, serviceInfo.service_secret, acc_token); // by access token
                            if (!string.IsNullOrEmpty(facebookID))
                                retObj = GetPlatformUID(ref TB, facebookID, platform_type);
                        }                        // by request token
                        else if (serviceInfo != null && serviceInfo.game_service_id > 0 && string.IsNullOrEmpty(acc_token) && !string.IsNullOrEmpty(auth_token))
                        {
                            string facebookID = GetFaceBookPlatformIDByAuthCode(serviceInfo.service_app_id, serviceInfo.service_secret, auth_token);
                            if (!string.IsNullOrEmpty(facebookID))
                                retObj = GetPlatformUID(ref TB, facebookID, platform_type);
                        }
                        break;
                }
            }
            TB.EndTransaction();
            TB.Dispose();
            logger.Dispose();

            return retObj == null ? new ret_user_platform_id() : retObj;
        }

        //public static Result_Define.eResult SwitchPlatformAccount(long beforeid, long afterid)
        //{
        //    PlatformLogger logger = new PlatformLogger(log_tag);
        //    TxnBlock TB = new TxnBlock();
        //    {
        //        TB.DBConn(SystemConfig.GetSystemConfigInstance().platformDB, SystemConfig.GetSystemConfigInstance().platformDB.SetDBAlias);
        //        TB.IsoLevel = IsolationLevel.ReadUncommitted;
        //        TB.Elog = logger.DBLog;
        //    }
        //    TB.EndTransaction();
        //    TB.Dispose();
        //    logger.Dispose();
        //}

        //private static Result_Define.eResult UpdateGuestState(ref TxnBlock TB, string beforeid, string auth_token)
        //{
        //    user_guest_auth_id retObj = null;
        //    string query = string.Format(@"UPDATE * FROM {0} WHERE auth_md5_id = @p1 AND client_auth_token = @p2", DB_Define.DBTables[DB_Define.eDBTables.user_guest_auth_id]);
        //    {
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.CommandText = query;
        //        cmd.Parameters.AddWithValue("@p1", acc_token);
        //        cmd.Parameters.AddWithValue("@p2", auth_token);
        //        retObj = GenericFetch.FetchFromDB<user_guest_auth_id>(ref TB, cmd);
        //        cmd.Dispose();
        //    }
        //    return retObj;
        //}
    }
}
