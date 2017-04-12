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

namespace mSeed.Platform.Cache
{
    public class Server_Cache
    {
        private const string log_tag = "cache";

        // singleton Server_Cache instance
        public static Server_Cache _syscache = new Server_Cache();
        /// <summary> get _syscache Instance </summary>
        public static Server_Cache GetSystemCacheInstance()
        {
            return _syscache;
        }

        private Server_Cache()
        {
            _cachedServiceInfoList = new Dictionary<long, GameServiceInfoStruct>();
            _cachedAccessInfoList = new Dictionary<long, GameAccessAuth>();
        }
        private Dictionary<long, GameServiceInfoStruct> _cachedServiceInfoList = null;
        private Dictionary<long, GameAccessAuth> _cachedAccessInfoList = null;

        private const int DefaultCacheExpireTimeSec = 600;

        // Game Service Info
        public void RemoveCacheServiceInfoList()
        {
            _cachedServiceInfoList.Clear();
        }

        public void RemoveCacheServiceInfo(long service_id)
        {
            _cachedServiceInfoList.Remove(service_id);
        }

        public void SetCacheServiceInfo(long service_id, GameServiceInfoStruct setInfo, int setExpireSec = DefaultCacheExpireTimeSec)
        {
            if (setInfo.service_info_list.Count > 0)
            {                
                _cachedServiceInfoList[service_id] = setInfo;
                _cachedServiceInfoList[service_id].ExpireTime = DateTime.Now.AddSeconds(setExpireSec);
            }
        }

        public GameServiceInfoStruct GetCacheServiceInfo(long service_id)
        {
            GameServiceInfoStruct retObj = null;
            if (_cachedServiceInfoList.ContainsKey(service_id))
                if (_cachedServiceInfoList[service_id].ExpireTime > DateTime.Now && _cachedServiceInfoList[service_id].service_info_list.Count > 0)
                    retObj = _cachedServiceInfoList[service_id];
            
            if(retObj == null)
            {
                PlatformLogger logger = new PlatformLogger(log_tag);
                TxnBlock TB = new TxnBlock();
                {
                    TB.DBConn(SystemConfig.GetSystemConfigInstance().platformDB, SystemConfig.GetSystemConfigInstance().platformDB.SetDBAlias);
                    TB.IsoLevel = IsolationLevel.ReadUncommitted;
                    TB.Elog = logger.DBLog;

                    string query = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) WHERE game_service_id = @p1", DB_Define.DBTables[DB_Define.eDBTables.game_service_info]);
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@p1", service_id);
                        List<game_service_info> setList = GenericFetch.FetchFromDB_MultipleRow<game_service_info>(ref TB, cmd);
                        cmd.Dispose();

                        if (setList.Count > 0)
                        {
                            GameServiceInfoStruct setObj = new GameServiceInfoStruct(service_id);
                            setList.ForEach(setItem => setObj.service_info_list.Add(setItem.service_type, setItem));
                            SetCacheServiceInfo(service_id, setObj);
                            retObj = _cachedServiceInfoList[service_id];
                        }
                    }
                }
                TB.Dispose();
                logger.Dispose();
            }

            return retObj == null ? new GameServiceInfoStruct() : retObj;
        }

        // AuthInfo
        public void RemoveCacheAuthInfoList()
        {
            _cachedAccessInfoList.Clear();
        }

        public void RemoveCacheAuthInfo(long access_id)
        {
            _cachedAccessInfoList.Remove(access_id);
        }

        public void SetCacheAuthInfo(long access_id, GameAccessAuth setInfo, int setExpireSec = DefaultCacheExpireTimeSec)
        {
            _cachedAccessInfoList[access_id] = setInfo;
            _cachedAccessInfoList[access_id].ExpireTime = DateTime.Now.AddSeconds(setExpireSec);
        }

        public GameAccessAuth GetCacheAuthInfo(long access_id, string auth_key)
        {
            GameAccessAuth retObj = null;
            if (_cachedAccessInfoList.ContainsKey(access_id))
                if (_cachedAccessInfoList[access_id].ExpireTime > DateTime.Now)
                    retObj = _cachedAccessInfoList[access_id];
            
            if(retObj == null)
            {
                PlatformLogger logger = new PlatformLogger(log_tag);
                TxnBlock TB = new TxnBlock();
                {
                    TB.DBConn(SystemConfig.GetSystemConfigInstance().platformDB, SystemConfig.GetSystemConfigInstance().platformDB.SetDBAlias);
                    TB.IsoLevel = IsolationLevel.ReadUncommitted;
                    TB.Elog = logger.DBLog;

                    retObj = new GameAccessAuth(GameServiceManager.GetGameServiceInfoDB(ref TB, access_id, auth_key));
                    
                    if(retObj != null)
                        SetCacheAuthInfo(access_id, retObj);
                }
                TB.Dispose();
                logger.Dispose();
            }
            return retObj == null ? new GameAccessAuth() : retObj;
        }
    }
}
