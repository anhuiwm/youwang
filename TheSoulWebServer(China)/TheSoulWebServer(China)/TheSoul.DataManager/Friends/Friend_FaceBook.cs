using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Global;

namespace TheSoul.DataManager.DBClass
{
    public class User_FriendList_FaceBook
    {
        public string fb_uid { get; set; }
    }
}

namespace TheSoul.DataManager
{
    public static partial class Friend_Define
    {
        public const string FaceBookFriendList_DBName = "sharding";
        public const string FaceBookFriendList_TableName = "User_FriendList_FaceBook";
    }

    public static partial class FriendManager
    {
        public class GetCount
        {
            public int count { get; set; }
        }

        private static string GetFaceBookFriendsListRedisKey(long AID)
        {
            return string.Format("{0}_{1}", Friend_Define.FaceBookFriendList_TableName, AID);
        }

        public static List<string> GetFaceBookFriendsList(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Friend_Define.FaceBookFriendList_DBName)
        {
            string setKey = GetFaceBookFriendsListRedisKey(AID);
            string setQuery = string.Format("SELECT fb_uid FROM {0} WITH(NOLOCK)  WHERE aid = {1}", Friend_Define.FaceBookFriendList_TableName, AID);
            return TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow<User_FriendList_FaceBook>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush).Select(item => item.fb_uid).ToList();
        }

        public static Result_Define.eResult InsertFaceBookFriend(ref TxnBlock TB, long AID, string facebook_uid, string dbkey = Friend_Define.FaceBookFriendList_DBName)
        {
            
            string setQuery = string.Format(@"
                                        IF NOT EXISTS (SELECT fb_uid FROM {0} WHERE aid = {1} AND fb_uid = N'{2}')
	                                        INSERT INTO {0} VALUES ({1}, N'{2}', GETDATE());
                                            ", Friend_Define.FaceBookFriendList_TableName, AID, facebook_uid);
            Result_Define.eResult retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            if (retError == Result_Define.eResult.SUCCESS)
                RemoveFaceBookFriendsListCache(AID);
            return retError;
        }

        public static void RemoveFaceBookFriendsListCache(long AID)
        {
            string setKey = GetFaceBookFriendsListRedisKey(AID);
            RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }

        public static int GetFaceBookFriendsCount(ref TxnBlock TB, long AID, string dbkey = Friend_Define.FaceBookFriendList_DBName)
        {
            string setQuery = string.Format("SELECT count(*) as count FROM {0} WITH(NOLOCK)  WHERE aid = {1}", Friend_Define.FaceBookFriendList_TableName, AID);
            GetCount retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<GetCount>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.count;
        }
    }
}
