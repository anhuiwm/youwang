using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager.DBClass;

namespace TheSoul.DataManager.DBClass
{
    public class User_Encrypt
    {
        public long AID { get; set; }
        public string EncryptKey { get; set; }
        public byte LogLevel { get; set; }

        public User_Encrypt(string setKey = "")
        {
            EncryptKey = setKey;
        }
    }
}

namespace TheSoul.DataManager
{
    public class Encrypt_Define
    {
        public const string User_Encrypte_DB = "sharding";
        public const string User_Encrypt_TableName = "User_Encrypt";
        public enum eLogLevel
        {
            None = 0,
            SystemNote = 1,
            ReturnJson = 2,
            DetailDBLog = 3,
        }
    }
}

namespace TheSoul.DataManager
{
    public static partial class EncryptManager
    {
        public static void RemvoeUser_EncryptKey(long AID)
        {
            string setKey = string.Format("{0}_{1}", Encrypt_Define.User_Encrypt_TableName, AID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }

        public static User_Encrypt GetUser_Encrypt(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Encrypt_Define.User_Encrypte_DB)
        {
            string setKey = string.Format("{0}_{1}", Encrypt_Define.User_Encrypt_TableName, AID);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE AID = {1}", Encrypt_Define.User_Encrypt_TableName, AID);
            User_Encrypt retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<User_Encrypt>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
            //User_Encrypt retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<User_Encrypt>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new User_Encrypt();
        }

        public static Result_Define.eResult SetUser_Encrypte(ref TxnBlock TB, long AID, string setEnryptKey, Encrypt_Define.eLogLevel setLogLevel = Encrypt_Define.eLogLevel.None  , string dbkey = Encrypt_Define.User_Encrypte_DB)
        {
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON AID = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                 EncryptKey = '{2}'
                                                    ,LogLevel = '{3}'
                                                WHEN NOT MATCHED THEN
                                                   INSERT VALUES ({1}, '{2}', '{3}');
                                    ", Encrypt_Define.User_Encrypt_TableName
                                     , AID
                                     , setEnryptKey
                                     , (byte)setLogLevel
                         );
            RemvoeUser_EncryptKey(AID);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }
    }
}