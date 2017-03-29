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
    public enum eServiceAccessLevel
    {
        None = 0,
        All = 100,
    }

    public partial class GameServiceManager
    {
        internal static game_access_auth GetGameServiceInfoDB(ref TxnBlock TB, long access_id, string authkey)
        {
            game_access_auth retObj = null;
            string query = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) WHERE api_access_id = @p1 AND access_auth_key = @p2", DB_Define.DBTables[DB_Define.eDBTables.game_access_auth]);
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@p1", access_id);
                cmd.Parameters.AddWithValue("@p2", authkey);
                retObj = GenericFetch.FetchFromDB<game_access_auth>(ref TB, cmd);
                cmd.Dispose();
            }

            return retObj;
        }

        public static game_service_info GetGameServiceInfo(long access_id, string authkey, int service_type)
        {
            game_service_info retObj = null;
            game_access_auth authInfo = Server_Cache.GetSystemCacheInstance().GetCacheAuthInfo(access_id, authkey);

            if (authInfo != null)
            {
                GameServiceInfoStruct serviceInfo = Server_Cache.GetSystemCacheInstance().GetCacheServiceInfo(authInfo.game_service_id);
                serviceInfo.service_info_list.TryGetValue(service_type, out retObj);
            }
            return retObj == null ? new game_service_info() : retObj;
        }

        public static game_service_info GetFCMPushServiceInfo(long game_service_id)
        {
            game_service_info retObj = null;
            GameServiceInfoStruct serviceInfo = Server_Cache.GetSystemCacheInstance().GetCacheServiceInfo(game_service_id);
            serviceInfo.service_info_list.TryGetValue((int)eServiceInfoType.GoogleFCM_ToAndroid, out retObj);
            return retObj == null ? new game_service_info() : retObj;
        }

        public static game_service_info GetAPNSPushServiceInfo(long game_service_id)
        {
            game_service_info retObj = null;
            GameServiceInfoStruct serviceInfo = Server_Cache.GetSystemCacheInstance().GetCacheServiceInfo(game_service_id);
            serviceInfo.service_info_list.TryGetValue((int)eServiceInfoType.Apple_APNS, out retObj);
            return retObj == null ? new game_service_info() : retObj;
        }

        public static game_service GetGameService(ref TxnBlock TB, long game_service_id)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) WHERE game_service_id = {1}", DB_Define.DBTables[DB_Define.eDBTables.game_service], game_service_id);

            game_service retObj = GenericFetch.FetchFromDB<game_service>(ref TB, setQuery);
            if (retObj == null)
                retObj = new game_service();
            return retObj;
        }

        public static List<game_service> GetGameService(ref TxnBlock TB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)", DB_Define.DBTables[DB_Define.eDBTables.game_service]);

            List<game_service> retObj = GenericFetch.FetchFromDB_MultipleRow<game_service>(ref TB, setQuery);
            if (retObj.Count == 0)
                retObj = new List<game_service>();
            return retObj;
        }
    }
}
