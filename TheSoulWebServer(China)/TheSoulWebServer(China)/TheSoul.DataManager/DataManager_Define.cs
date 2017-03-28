using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;

namespace TheSoul.DataManager
{
    public static class DataManager_Define
    {
        public const string RedisTagPrefix = "TheSoul";
        public const string RedisServerAlias_User = "User";
        public const string RedisServerAlias_System = "System";
        public const string RedisServerAlias_Ranking = "Ranking";
        public const string UserRequestCache_Prefix = "UserRequest";
        public const int UserReRequestQueueSize = 10;

        public const string DB_CHAR_FLAG_TRUE = "Y";
        public const string DB_CHAR_FLAG_FALSE = "N";
        //static bool localTest = true;       

        public const string GlobalDB = "global";
        public const string CommonDB = "common";
        public const string ShardingDB = "sharding";
        public const string LogDB = "log";
        public const string LogTableName = "_Request_Log";

        public static List<RedisEndpoint> RedisServerList = new List<RedisEndpoint>();

        public const int CompressionLength = 2048; // 2k byte over

        public enum eCountryCode
        {
            Korea = 0,
            English = 1,
            Japan = 2,
            China = 3,
            Taiwan = 4,
            International = 101,
            Default = China,
        }
    }
}

