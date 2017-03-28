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

namespace TheSoul.DataManager
{
    public static partial class FriendManager
    {
        public static List<RetFriendsInfo> GetFriendsInfoListFromDB(ref TxnBlock TB, string friendAidList, string dbkey = Account_Define.AccountShardingDB)
        {
            SqlCommand cmdFriendsInfo = new SqlCommand();
            cmdFriendsInfo.CommandText = "System_GetFriendsInfo_ForPartyPvP";
            cmdFriendsInfo.Parameters.Add("@FRIENDSLIST", SqlDbType.NVarChar).Value = friendAidList;
            List<RetFriendsInfo> retList = new List<RetFriendsInfo>();
            SqlDataReader getDr = null;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref cmdFriendsInfo, ref getDr))
            {
                if (getDr != null)
                {
                    var r = SQLtoJson.Serialize(ref getDr);
                    string json = mJsonSerializer.ToJsonString(r);
                    getDr.Dispose(); getDr.Close();
                    retList = mJsonSerializer.JsonToObject<List<RetFriendsInfo>>(json);
                }
            }
            return retList;
        }

        public static List<FriendsList> GetFriendList(ref TxnBlock TB, long AID, string dbkey = Friend_Define.FriendList_DBName, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", Friend_Define.FriendListPrefix, AID);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE myaid={1} AND acceptfriend='Y' AND delflag='N'", Friend_Define.FriendList_TableName, AID);
            return TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow<FriendsList>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
        }

        public static List<long> GetFriend_AID_List(ref TxnBlock TB, long AID)
        {
            List<FriendsList> getMyFriends = GetFriendList(ref TB, AID);
            List<long> setAIDList = new List<long>();

            foreach (FriendsList setFriends in getMyFriends)
            {
                if (setFriends.friendaid > 0)
                    setAIDList.Add(setFriends.friendaid);
            }
            return setAIDList;
        }

        public static Dictionary<long, Friends> GetFriendsListFetchFromDB(ref TxnBlock TB, long AID, string dbkey = Friend_Define.FriendList_DBName, bool Flush = false)
        {
            if (AID == 0)
                return null;

            SqlDataReader getDr = null;
            string setKey = string.Empty;
            string setQuery = string.Format("SELECT friendaid, friendname, lastgiftsend FROM {0} WITH(NOLOCK)  WHERE myaid = {1} AND acceptfriend='Y' AND delflag='N'", Friend_Define.FriendList_TableName, AID);

            TB.ExcuteSqlCommand(dbkey, setQuery, ref getDr);

            Dictionary<long, Friends> retSet = new Dictionary<long, Friends>();

            if (getDr != null)
            {
                var r = SQLtoJson.Serialize(ref getDr);
                string json = mJsonSerializer.ToJsonString(r);

                getDr.Dispose(); getDr.Close();
                Friends[] FriendList = mJsonSerializer.JsonToObject<Friends[]>(json);

                foreach (Friends friendinfo in FriendList)
                {
                    Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
                    Account getAccInfo = TheSoul.DataManager.AccountManager.GetAccountData(ref TB, friendinfo.friendaid, ref retError);
                    friendinfo.friendlastconntime=getAccInfo.LastConnTime;
                    friendinfo.friendname = getAccInfo.UserName;
                    friendinfo.charinfo = new Character_Simple(CharacterManager.GetCharacter(ref TB, friendinfo.friendaid, getAccInfo.EquipCID));
                    retSet.Add(friendinfo.friendaid, friendinfo);
                }
            }

            return retSet;
        }

        private static string GetFriendsListRedisKey(long AID)
        {
            return string.Format("{0}_{1}_{2}", Friend_Define.FriendListPrefix, Friend_Define.FriendList_TableName, AID);
        }

        private static Dictionary<long, Friends> GetFriendsListFetchFromRedis(ref TxnBlock TB, long AID, string dbkey = Friend_Define.FriendList_DBName, bool Flush = false)
        {
            Dictionary<long, Friends> setFriendsList = new Dictionary<long, Friends>();
            string setKey = GetFriendsListRedisKey(AID);
            Dictionary<string, string> getFriendsList = RedisConst.GetRedisInstance().GetHashsAll_Item(DataManager_Define.RedisServerAlias_User, setKey);

            if (getFriendsList == null || Flush)
                Flush = true;
            else if (getFriendsList.Count == 0)
                Flush = true;

            DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            dbDate = dbDate.AddDays(1);
            TimeSpan TS = dbDate - dbDate;

            if (Flush)
            {
                RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey);
                Dictionary<long, Friends> charFriendsList = GetFriendsListFetchFromDB(ref TB, AID, dbkey, Flush);

                foreach (KeyValuePair<long, Friends> setItem in charFriendsList)
                {
                    TS = setItem.Value.lastgiftsend - dbDate;

                    if (TS.Days != 0)
                        setItem.Value.keysendremaintime = 0;
                    else
                    {
                        TS = dbDate - DateTime.Now;
                        setItem.Value.keysendremaintime = System.Convert.ToInt32(TS.TotalSeconds);
                    }
                    setItem.Value.keysendremaintime = setItem.Value.keysendremaintime < 0 ? 0 : setItem.Value.keysendremaintime;

                    setFriendsList[setItem.Key] = setItem.Value;
                    RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, setItem.Key.ToString(), setItem.Value);
                }
                RedisConst.GetRedisInstance().SetExpireTimeHash(DataManager_Define.RedisServerAlias_User, setKey);
            }
            else
            {
                foreach (KeyValuePair<string, string> setJson in getFriendsList)
                {
                    Friends setItem = mJsonSerializer.JsonToObject<Friends>(setJson.Value);
                    TS = setItem.lastgiftsend - dbDate;

                    if (TS.Days != 0)
                        setItem.keysendremaintime = 0;
                    else
                    {
                        TS = dbDate - DateTime.Now;
                        setItem.keysendremaintime = System.Convert.ToInt32(TS.TotalSeconds);
                    }
                    setItem.keysendremaintime = setItem.keysendremaintime < 0 ? 0 : setItem.keysendremaintime;

                    setFriendsList[setItem.friendaid] = setItem;
                }
            }
            return setFriendsList;
        }

        public static Dictionary<long, Friends> GetFriendsList(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Friend_Define.FriendList_DBName)
        {
            return GetFriendsListFetchFromRedis(ref TB, AID, dbkey, Flush);
        }
        public static void RemoveFriendListCache(ref TxnBlock TB, long AID, string dbkey = Friend_Define.FriendList_DBName)
        {
            string setKey = GetFriendsListRedisKey(AID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey);
            //return GetFriendsListFetchFromRedis(ref TB, AID, dbkey, true);
        }

        private static Dictionary<long, Friends> GetRequestFriendsListFetchFromDB(ref TxnBlock TB, long AID, string dbkey = Friend_Define.FriendList_DBName, bool Flush = false)
        {
            if (AID == 0)
                return null;

            SqlDataReader getDr = null;
            string setKey = string.Empty;
            string setQuery = string.Format("SELECT friendaid, friendname, lastgiftsend FROM {0} WITH(NOLOCK)  WHERE myaid = {1} AND acceptfriend='N' AND delflag='N'", Friend_Define.FriendList_TableName, AID);

            TB.ExcuteSqlCommand(dbkey, setQuery, ref getDr);

            Dictionary<long, Friends> retSet = new Dictionary<long, Friends>();

            if (getDr != null)
            {
                var r = SQLtoJson.Serialize(ref getDr);
                string json = mJsonSerializer.ToJsonString(r);

                getDr.Dispose(); getDr.Close();
                Friends[] FriendList = mJsonSerializer.JsonToObject<Friends[]>(json);

                foreach (Friends friendinfo in FriendList)
                {
                    Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
                    Account getAccInfo = TheSoul.DataManager.AccountManager.GetAccountData(ref TB, friendinfo.friendaid, ref retError);
                    friendinfo.friendlastconntime = getAccInfo.LastConnTime;
                    friendinfo.friendname = getAccInfo.UserName;
                    friendinfo.charinfo = new Character_Simple(CharacterManager.GetCharacter(ref TB, friendinfo.friendaid, getAccInfo.EquipCID));
                    retSet.Add(friendinfo.friendaid, friendinfo);
                }
            }

            return retSet;
        }

        private static string GetRequestFriendsListRedisKey(long AID)
        {
            return string.Format("{0}_{1}_{2}", Friend_Define.RequestFriendPrefix, Friend_Define.FriendList_TableName, AID);
        }

        private static Dictionary<long, Friends> GetRequestFriendsListFetchFromRedis(ref TxnBlock TB, long AID, string dbkey = Friend_Define.FriendList_DBName, bool Flush = false)
        {            
            Dictionary<long, Friends> setFriendsList = new Dictionary<long, Friends>();
            string setKey = GetRequestFriendsListRedisKey(AID);
            Dictionary<string, string> getFriendsList = RedisConst.GetRedisInstance().GetHashsAll_Item(DataManager_Define.RedisServerAlias_User, setKey);

            if (getFriendsList == null || Flush)
                Flush = true;
            else if (getFriendsList.Count == 0)
                Flush = true;

            if (Flush)
            {
                RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey);
                Dictionary<long, Friends> charFriendsList = GetRequestFriendsListFetchFromDB(ref TB, AID, dbkey, Flush);

                foreach (KeyValuePair<long, Friends> setItem in charFriendsList)
                {
                    setFriendsList[setItem.Key] = setItem.Value;
                    RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, setItem.Key.ToString(), setItem.Value);
                }
                RedisConst.GetRedisInstance().SetExpireTimeHash(DataManager_Define.RedisServerAlias_User, setKey);
            }
            else
            {
                foreach (KeyValuePair<string, string> setJson in getFriendsList)
                {
                    Friends setItem = mJsonSerializer.JsonToObject<Friends>(setJson.Value);
                    setFriendsList[setItem.friendaid] = setItem;
                }
            }
            return setFriendsList;
        }
        public static Dictionary<long, Friends> GetRequestFriendsList(ref TxnBlock TB, long AID, string dbkey = Friend_Define.FriendList_DBName, bool Flush = false)
        {
            return GetRequestFriendsListFetchFromRedis(ref TB, AID, dbkey, Flush);
        }

        public static void RemoveRequestFriendsCache(ref TxnBlock TB, long AID, string dbkey = Friend_Define.FriendList_DBName)
        {
            string setKey = GetRequestFriendsListRedisKey(AID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey);
            //return GetRequestFriendsListFetchFromRedis(ref TB, AID, dbkey, true);
        }

        private static string GetExceptFriendsList(ref TxnBlock TB, long AID, string dbkey = Friend_Define.FriendList_DBName, bool Flush = false)
        {
            string ExceptFriendsList = "";
            int loopnum = 0;

            //친구리스트 제외
            string setKey = string.Format("{0}_{1}_{2}", Friend_Define.FriendListPrefix, Friend_Define.FriendList_TableName, AID);
            Dictionary<long, Friends> setFriendsList = new Dictionary<long, Friends>();
            Dictionary<string, string> getFriendsList = RedisConst.GetRedisInstance().GetHashsAll_Item(DataManager_Define.RedisServerAlias_User, setKey);
            if (getFriendsList == null || Flush)
                Flush = true;
            else if (getFriendsList.Count == 0)
                Flush = true;

            if (Flush)
            {
                RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey);
                Dictionary<long, Friends> charFriendsList = GetFriendsListFetchFromDB(ref TB, AID, dbkey, Flush);

                foreach (KeyValuePair<long, Friends> setItem in charFriendsList)
                {
                    if (loopnum == 0)
                    {
                        ExceptFriendsList = System.Convert.ToString(setItem.Key);
                    }
                    else
                    {
                        ExceptFriendsList = ExceptFriendsList + "," + setItem.Key;
                    }
                    loopnum = ++loopnum;
                }
            }
            else
            {
                foreach (KeyValuePair<string, string> setJson in getFriendsList)
                {
                    Friends setItem = mJsonSerializer.JsonToObject<Friends>(setJson.Value);
                    if (loopnum == 0)
                    {
                        ExceptFriendsList = System.Convert.ToString(setItem.friendaid);
                    }
                    else
                    {
                        ExceptFriendsList = ExceptFriendsList + "," + setItem.friendaid;
                    }
                    loopnum = ++loopnum;
                }
            }
            //신청한 친구 리스트 제외
            Dictionary<long, Friends> setReqFriendsList = new Dictionary<long, Friends>();
            string setKey2 = string.Format("{0}_{1}_{2}", Friend_Define.RequestFriendPrefix, Friend_Define.FriendList_TableName, AID);
            Dictionary<string, string> getReqFriendsList = RedisConst.GetRedisInstance().GetHashsAll_Item(DataManager_Define.RedisServerAlias_User, setKey2);

            if (getReqFriendsList == null || Flush)
                Flush = true;
            else if (getReqFriendsList.Count == 0)
                Flush = true;

            if (Flush)
            {
                RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey);
                Dictionary<long, Friends> charFriendsList2 = GetRequestFriendsListFetchFromDB(ref TB, AID, dbkey, Flush);

                foreach (KeyValuePair<long, Friends> setItem2 in charFriendsList2)
                {
                    if (ExceptFriendsList == "")
                    {
                        ExceptFriendsList = System.Convert.ToString(setItem2.Key);
                    }
                    else
                    {
                        ExceptFriendsList = ExceptFriendsList + "," + setItem2.Key;
                    }
                    loopnum = ++loopnum;
                }
            }
            else
            {
                foreach (KeyValuePair<string, string> setJson in getReqFriendsList)
                {
                    Friends setItem2 = mJsonSerializer.JsonToObject<Friends>(setJson.Value);
                    if (ExceptFriendsList == "")
                    {
                        ExceptFriendsList = System.Convert.ToString(setItem2.friendaid);
                    }
                    else
                    {
                        ExceptFriendsList = ExceptFriendsList + "," + setItem2.friendaid;
                    }
                    loopnum = ++loopnum;
                }
            }
            if (ExceptFriendsList == "")
            {
                ExceptFriendsList = System.Convert.ToString(0);
            }

            return ExceptFriendsList;
        }

        private static RecommendFriendCount GetServerRecommendCount(ref TxnBlock TB, int Level, string dbkey = Friend_Define.RecommendFriendList_DBName, bool Flush = false)
        {
            int LevelGrade = (Level / 10) + 1;
            string setKey = string.Format("{0}_{1}", Friend_Define.RecommendFriendCountPrefix, LevelGrade);
            string setQuery = string.Format(@"SELECT count FROM {0} WITH(NOLOCK)  where LevelGrade={1}", Friend_Define.RecommendGradeCount_TableName, LevelGrade);

            RecommendFriendCount retObj = GenericFetch.FetchFromRedis_Hash<RecommendFriendCount>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, LevelGrade.ToString(), setQuery, dbkey, Flush);
            return (retObj != null) ? retObj : new RecommendFriendCount();
        }

        public static List<Friends> GetRecommandFriendsList(ref TxnBlock TB, long AID, string dbkey = Friend_Define.AccountInfo_DBName, bool Flush = false)
        {
            int userLevel = CharacterManager.GetCharacterMaxLevel_FromDB(ref TB, AID);
            int setMin = (userLevel - Friend_Define.RecommendLevelRange) < 0 ? 0 : (userLevel - Friend_Define.RecommendLevelRange);
            int setMax = ((userLevel + Friend_Define.RecommendLevelRange) > Character_Define.Max_CharacterLevel ? Character_Define.Max_CharacterLevel : (userLevel + Friend_Define.RecommendLevelRange));

            if (setMax < Friend_Define.RecommendLevelRange * 2)
                setMax = Friend_Define.RecommendLevelRange * 2;
            if (setMin > (Character_Define.Max_CharacterLevel - Friend_Define.RecommendLevelRange * 2))
                setMin = Character_Define.Max_CharacterLevel - Friend_Define.RecommendLevelRange * 2;

            string setKey = string.Format("{0}_{1}_{2}_{3}", Friend_Define.RecommendFriendPrefix, Friend_Define.FriendList_TableName, setMin, setMax);

            List<RecommandFriends> getFriendsList = RedisConst.GetRedisInstance().GetRandomList<RecommandFriends>(DataManager_Define.RedisServerAlias_System, setKey, Friend_Define.RecommendCount+1);

            if (getFriendsList.Count < Friend_Define.RecommendCount || Flush)
                Flush = true;
            
            if (Flush)
            {
                RedisConst.GetRedisInstance().RemoveList(DataManager_Define.RedisServerAlias_System, setKey);

                string setQuery = string.Format(@"
                                                SELECT
                                                    TOP {4}
	                                                AID as friendaid, UserName as friendname, datediff(ss,'1970-01-01',LastConnTime) as friendlastconntime, EquipCID 
	                                                , ISNULL((SELECT COUNT(*) FROM {1}.dbo.User_FriendsList WITH(NOLOCK) WHERE acceptfriend = 'Y' AND delflag = 'N' AND myaid = AID GROUP BY myaid), 0) as friendscount
	                                                , ISNULL((SELECT COUNT(*) FROM {1}.dbo.User_FriendsList WITH(NOLOCK) WHERE acceptfriend = 'N' AND delflag = 'N' AND myaid = AID GROUP BY myaid), 0) as friendswait
                                                  FROM {0}.dbo.Account as ACC WITH(NOLOCK, INDEX(IDX_Account_Lv))
                                                  WHERE 
                                                  LV > {2} AND LV <= {3} AND LastConnTime > DATEADD(day, {5}, GETDATE())
                                                  ORDER BY NEWID()
                                                ", GlobalManager.CurrentDB.sharding_db_name
                                                 , GlobalManager.CurrentDB.common_db_name
                                                 //, Friend_Define.MaxFriendCount
                                                 , setMin
                                                 , setMax
                                                 , Friend_Define.MaxFriendPool
                                                 , Account_Define.MaxLastConnDay
                                                 );
                getFriendsList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<RecommandFriends>(ref TB, setQuery, dbkey);
                getFriendsList = getFriendsList.Where(friends => friends.friendscount < Friend_Define.MaxFriendCount && friends.friendswait < Friend_Define.MaxFriendCount).ToList();

                RedisConst.GetRedisInstance().ListAdds<RecommandFriends>(DataManager_Define.RedisServerAlias_System, setKey, getFriendsList.ToArray());
                RedisConst.GetRedisInstance().ListExpireTimeSet(DataManager_Define.RedisServerAlias_System, setKey, new TimeSpan(0, Friend_Define.RecommendRefreshTime, 0));
            }

            var rnd = new Random();
            List<RecommandFriends> CheckList = getFriendsList.OrderBy(x => rnd.Next()).ToList();
            List<Friends> retObj = new List<Friends>();
            foreach (RecommandFriends setInfo in CheckList)
            {
                if (setInfo.friendaid != AID && setInfo.EquipCID != 0)
                {
                    setInfo.charinfo = new Character_Simple(CharacterManager.GetCharacter(ref TB, setInfo.friendaid, setInfo.EquipCID));
                    if(setInfo.charinfo.cid != 0)
                        retObj.Add(new Friends(setInfo));
                }

                if (retObj.Count >= Friend_Define.RecommendCount)
                    break;
            }

            return retObj;
        }

        private static Result_Define.eResult CheckDeleteFriendOneDay(ref TxnBlock TB, long AID, long FAID, string dbkey = Friend_Define.FriendList_DBName)
        {
            string setQuery = string.Format("SELECT aid FROM {0} WITH(NOLOCK)  WHERE DATEADD(HOUR,{3},regdate)>GETDATE() AND aid={1} AND faid={2}", Friend_Define.FriendDeleteLog_TableName, AID, FAID, Friend_Define.DeleteCheckTime_Hour);
            SqlDataReader getDr = null;
            if(TB.ExcuteSqlCommand(dbkey, setQuery, ref getDr))    // check return by command success
            {
                Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
                if (getDr != null)
                {
                    while (getDr.Read())
                    {
                        if (System.Convert.ToInt64(getDr["aid"]) > 0)
                        {
                            retError = Result_Define.eResult.CANT_DELETEFRIEND_ONEDAY;
                        }
                        else
                        {
                            retError = Result_Define.eResult.SUCCESS;
                        }
                    }
                    getDr.Dispose(); getDr.Close();
                }
                return retError;
            }
            else
            {
                return Result_Define.eResult.SUCCESS;
            }
        }
        private static Result_Define.eResult CheckMyFriendDeleteMeOneDay(ref TxnBlock TB, long AID, long FAID, string dbkey = Friend_Define.FriendList_DBName)
        {
            string setQuery = string.Format("SELECT aid FROM {0} WITH(NOLOCK)  WHERE DATEADD(HOUR,{3},regdate)>GETDATE() AND aid={2} AND faid={1}", Friend_Define.FriendDeleteLog_TableName, AID, FAID, Friend_Define.DeleteCheckTime_Hour);
            SqlDataReader getDr = null;
            if (TB.ExcuteSqlCommand(dbkey, setQuery, ref getDr))    // check return by command success
            {
                Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
                if (getDr != null)
                {
                    while (getDr.Read())
                    {
                        if (System.Convert.ToInt64(getDr["aid"]) > 0)
                        {
                            retError = Result_Define.eResult.CANT_DELETEFRIEND_ONEDAY;
                        }
                        else
                        {
                            retError = Result_Define.eResult.SUCCESS;
                        }
                    }
                    getDr.Dispose(); getDr.Close();
                }
                return retError;
            }
            else
            {
                return Result_Define.eResult.SUCCESS;
            }
        }
        private static Result_Define.eResult CheckRequestDuplicateFriend(ref TxnBlock TB, long AID, long FAID, string dbkey = Friend_Define.FriendList_DBName)
        {
            string setQuery = string.Format("SELECT myaid FROM {0} WITH(NOLOCK)  WHERE myaid={2} AND friendaid={1} AND acceptfriend='N' AND delflag='N'", Friend_Define.FriendList_TableName, AID, FAID);
            SqlDataReader getDr = null;
            if (TB.ExcuteSqlCommand(dbkey, setQuery, ref getDr))    // check return by command success
            {
                Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
                if (getDr != null)
                {
                    while (getDr.Read())
                    {
                        if (System.Convert.ToInt64(getDr["myaid"]) > 0)
                        {
                            retError = Result_Define.eResult.FRIEND_DUPLICATE_JOINFRIENDLIST;
                        }
                        else
                        {
                            retError = Result_Define.eResult.SUCCESS;
                        }
                    }
                    getDr.Dispose(); getDr.Close();
                }
                return retError;
            }
            else
            {
                return Result_Define.eResult.SUCCESS;
            }
        }
        private static Result_Define.eResult CheckRequestFriend(ref TxnBlock TB, long AID, long FAID, string dbkey = Friend_Define.FriendList_DBName)
        {
            string setQuery = string.Format("SELECT myaid FROM {0} WITH(NOLOCK)  WHERE myaid={2} AND friendaid={1} AND acceptfriend='Y' AND delflag='N'", Friend_Define.FriendList_TableName, AID, FAID);
            SqlDataReader getDr = null;
            if (TB.ExcuteSqlCommand(dbkey, setQuery, ref getDr))    // check return by command success
            {
                Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
                if (getDr != null)
                {
                    while (getDr.Read())
                    {
                        if (System.Convert.ToInt64(getDr["myaid"]) > 0)
                        {
                            retError = Result_Define.eResult.ALREADY_FRIEND_ACCOUNT;
                        }
                        else
                        {
                            retError = Result_Define.eResult.SUCCESS;
                        }
                    }
                    getDr.Dispose(); getDr.Close();
                }
                return retError;
            }
            else
            {
                return Result_Define.eResult.SUCCESS;
            }
        }
        private static Result_Define.eResult CheckMyFriendRequest(ref TxnBlock TB, long AID, long FAID, string dbkey = Friend_Define.FriendList_DBName)
        {
            string setQuery = string.Format("SELECT myaid FROM {0} WITH(NOLOCK) WHERE myaid={1} AND friendaid={2}", Friend_Define.FriendList_TableName, AID, FAID);
            SqlDataReader getDr = null;
            if (TB.ExcuteSqlCommand(dbkey, setQuery, ref getDr))    // check return by command success
            {
                Result_Define.eResult retError = Result_Define.eResult.FRIEND_NOTEXIST_ACCEPTFRIEND_INFO;
                if (getDr != null)
                {
                    while (getDr.Read())
                    {
                        if (System.Convert.ToInt64(getDr["myaid"]) > 0)
                        {
                            retError = Result_Define.eResult.SUCCESS;
                        }
                        else
                        {
                            retError = Result_Define.eResult.FRIEND_NOTEXIST_ACCEPTFRIEND_INFO;
                        }
                    }
                    getDr.Dispose(); getDr.Close();
                }
                return retError;
            }
            else
            {
                return Result_Define.eResult.FRIEND_NOTEXIST_ACCEPTFRIEND_INFO;
            }
        }

        public static FriendStandByCount GetFriendStandByCount(ref TxnBlock TB, long AID, string dbkey = Friend_Define.FriendList_DBName, bool Flush = false)
        {
            string setQuery = string.Format(@"SELECT Count(*) as count FROM {0} WITH(NOLOCK)  WHERE myaid = {1} AND acceptfriend='N' AND delflag='N'", Friend_Define.FriendList_TableName, AID);
            FriendStandByCount retObj = GenericFetch.FetchFromDB<FriendStandByCount>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new FriendStandByCount();
        }
        public static FriendCount GetMyFriendCount(ref TxnBlock TB, long AID, string dbkey = Friend_Define.FriendList_DBName, bool Flush = false)
        {
            string setQuery = string.Format(@"SELECT Count(*) as count FROM {0} WITH(NOLOCK)  WHERE myaid = {1} AND acceptfriend='Y' AND delflag='N'", Friend_Define.FriendList_TableName, AID);
            FriendCount retObj = GenericFetch.FetchFromDB<FriendCount>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new FriendCount();
        }
        public static Result_Define.eResult RequestFriend(ref TxnBlock TB, long AID, long FAID, string dbkey = Friend_Define.FriendList_DBName)
        {
            FriendStandByCount getFriendStandByCountInfo = FriendManager.GetFriendStandByCount(ref TB, FAID);
            FriendStandByCount getMyStabdByCountInfo = FriendManager.GetFriendStandByCount(ref TB, AID);
            FriendCount getMyFriendCount = FriendManager.GetMyFriendCount(ref TB, AID);
            FriendCount getFriendCount = FriendManager.GetMyFriendCount(ref TB, FAID);
            Result_Define.eResult retError = (getMyFriendCount.count >= Friend_Define.MaxFriendCount || getFriendCount.count >= Friend_Define.MaxFriendCount) ? Result_Define.eResult.FULL_FRIENDLIST : Result_Define.eResult.SUCCESS;

            if(retError == Result_Define.eResult.SUCCESS)
            {
                retError = (getFriendStandByCountInfo.count >= Friend_Define.MaxFriendCount || getMyStabdByCountInfo.count >= Friend_Define.MaxFriendCount) ? Result_Define.eResult.FRIEND_EXCESS_WAITING_JOINFRIENDLIST : Result_Define.eResult.SUCCESS;
            }                 
            
            //if (getMyFriendCount.count >= Friend_Define.MaxFriendCount || getFriendCount.count >= Friend_Define.MaxFriendCount)//최대 친구 초과(상수값 없음)
            //    retError = Result_Define.eResult.FULL_FRIENDLIST;

            //if (getFriendStandByCountInfo.count >= Friend_Define.MaxFriendCount || getMyStabdByCountInfo.count >= Friend_Define.MaxFriendCount)//친구 신청자 대기열 없음(상수값 없음 요청)
            //    retError = Result_Define.eResult.FRIEND_EXCESS_WAITING_JOINFRIENDLIST;
            if (retError == Result_Define.eResult.SUCCESS)
                retError = FriendManager.CheckDeleteFriendOneDay(ref TB, AID, FAID);//친구 삭제 로그 확인

            if (retError == Result_Define.eResult.SUCCESS)
                retError = FriendManager.CheckMyFriendDeleteMeOneDay(ref TB, AID, FAID);//친구가 나를 삭제했는지 확인

            if (retError == Result_Define.eResult.SUCCESS)
                retError = FriendManager.CheckRequestDuplicateFriend(ref TB, AID, FAID);//친구 신청 중복 체크

            if (retError == Result_Define.eResult.SUCCESS)
                retError = FriendManager.CheckRequestFriend(ref TB, AID, FAID);//이미 친구인지 아닌지 확인

            if (retError == Result_Define.eResult.SUCCESS)
            {
                Account getAccInfo = TheSoul.DataManager.AccountManager.GetAccountData(ref TB, AID, ref retError);
                if (retError != Result_Define.eResult.SUCCESS)
                    return retError;

                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "UserRequestFriend";
                Cmd.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                Cmd.Parameters.Add("@FAID", SqlDbType.BigInt).Value = FAID;
                Cmd.Parameters.Add("@FriendName", SqlDbType.NVarChar, 32).Value = getAccInfo.UserName;
                if (TB.ExcuteSqlStoredProcedure(dbkey, ref Cmd))    // check return by command success
                {
                    RemoveRequestFriendsCache(ref TB, AID);
                    RemoveRequestFriendsCache(ref TB, FAID);
                    retError = Result_Define.eResult.SUCCESS;
                }
                else
                {
                    retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                }
                Cmd.Dispose();
            }
            return retError;
        }

        public static Result_Define.eResult AcceptFriend(ref TxnBlock TB, long AID, long FAID, string dbkey = Friend_Define.FriendList_DBName)
        {
            FriendCount GetMyFriendCount = FriendManager.GetMyFriendCount(ref TB, AID);
            FriendCount GetReqFriendCount = FriendManager.GetMyFriendCount(ref TB, FAID);

            if (GetMyFriendCount.count >= Friend_Define.MaxFriendCount)//최대 친구 초과(상수값 없음)
            {
                return Result_Define.eResult.FULL_FRIENDLIST;
            }
            else if (GetReqFriendCount.count >= Friend_Define.MaxFriendCount)// 신청 친구 최대 친구 초과(상수값 없음)
            {
                return Result_Define.eResult.FULL_FRIENDLIST_TARGET;
            }
            else
            {
                Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                retError = FriendManager.CheckMyFriendRequest(ref TB, AID, FAID);//친구 신청 유무 확인

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    Account_Simple getAccInfo = TheSoul.DataManager.AccountManager.GetSimpleAccountInfo(ref TB, AID);
                    SqlCommand Cmd = new SqlCommand();
                    Cmd.CommandText = "UserAcceptFriend";
                    Cmd.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                    Cmd.Parameters.Add("@FAID", SqlDbType.BigInt).Value = FAID;
                    Cmd.Parameters.Add("@FriendName", SqlDbType.NVarChar, 32).Value = getAccInfo.username;
                    if (TB.ExcuteSqlStoredProcedure(dbkey, ref Cmd))    // check return by command success
                    {
                        RemoveFriendListCache(ref TB, AID, dbkey);
                        retError = Result_Define.eResult.SUCCESS;
                    }
                    else
                    {
                        retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                    }
                    Cmd.Dispose();

                    RemoveFriendListCache(ref TB, AID);
                    RemoveFriendListCache(ref TB, FAID);
                    RemoveRequestFriendsCache(ref TB, AID);
                    RemoveRequestFriendsCache(ref TB, FAID);
                    return retError;
                }
                else
                {
                    return retError;
                }
            }
        }

        public static Result_Define.eResult DeclineFriend(ref TxnBlock TB, long AID, long FAID, string dbkey = Friend_Define.FriendList_DBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            retError = FriendManager.CheckRequestFriend(ref TB, AID, FAID);//이미 친구인지 아닌지 확인

            if (retError == Result_Define.eResult.SUCCESS)
                retError = FriendManager.CheckMyFriendRequest(ref TB, AID, FAID);//친구 신청 유무 확인

            if (retError == Result_Define.eResult.SUCCESS)
            {
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "UserDeclineFriend";
                Cmd.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                Cmd.Parameters.Add("@FAID", SqlDbType.BigInt).Value = FAID;
                if (TB.ExcuteSqlStoredProcedure(dbkey, ref Cmd))    // check return by command success
                {
                    RemoveFriendListCache(ref TB, AID, dbkey);
                    retError = Result_Define.eResult.SUCCESS;
                }
                else
                {
                    retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                }
                Cmd.Dispose();

                RemoveRequestFriendsCache(ref TB, AID);
                RemoveRequestFriendsCache(ref TB, FAID);
                return retError;
            }
            else
            {
                return retError;
            }
        }

        public static int GetDeleteCount(ref TxnBlock TB, long AID, string dbkey = Friend_Define.FriendList_DBName)
        {
            string setQuery = string.Format("SELECT COUNT(*) as count FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND CONVERT(varchar(10),regdate,21) = CONVERT(varchar(10),getdate(),21) ", Friend_Define.FriendDeleteLog_TableName, AID);
            FriendCount retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<FriendCount>(ref TB, setQuery, dbkey);

            if (retObj == null)
                return 0;

            return retObj.count;
        }

        public static Result_Define.eResult DeleteFriend(ref TxnBlock TB, long AID, long FAID, string dbkey = Friend_Define.FriendList_DBName)
        {
            Result_Define.eResult retError = Friend_Define.MaxDeleteFriendCountPerDay > GetDeleteCount(ref TB, AID) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.FRIEND_EXCESS_DELETE_DAYCOUNT;

            if (retError == Result_Define.eResult.SUCCESS)
            {
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "UserDeleteFriend";
                var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                Cmd.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                Cmd.Parameters.Add("@FAID", SqlDbType.BigInt).Value = FAID;
                Cmd.Parameters.Add(result);
                if (TB.ExcuteSqlStoredProcedure(dbkey, ref Cmd))    // check return by command success
                {
                    if (System.Convert.ToInt32(result.Value) < 0)
                    {
                        int dbresult = System.Convert.ToInt32(result.Value) * -1;       // re delcare result code 
                        if ((Result_Define.eResult)dbresult == Result_Define.eResult.FRIEND_NOTEXIST_ACCEPTFRIEND_INFO)
                            retError = Result_Define.eResult.FRIEND_NOTEXIST_ACCEPTFRIEND_INFO;
                        else
                            retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                    }
                    else
                    {
                        RemoveFriendListCache(ref TB, AID, dbkey);
                        retError = Result_Define.eResult.SUCCESS;
                    }
                }
                else
                {
                    retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                }
                Cmd.Dispose();

                RemoveRequestFriendsCache(ref TB, AID);
                RemoveRequestFriendsCache(ref TB, FAID);
                return retError;
            }
            else
            {
                return retError;
            }
        }

        public static Account_Simple_With_Connection SearchFriend(ref TxnBlock TB, string search_name, string dbkey = Account_Define.AccountShardingDB)
        {
            //System.Text.Encoding utf8_encoder = System.Text.Encoding.UTF8;
            //byte[] utf8Bytes = utf8_encoder.GetBytes(search_name);
            //string set_search_name_utf8 = utf8_encoder.GetString(utf8Bytes);
            string setKey = string.Format("{0}_{1}_{2}", Friend_Define.FriendSearchPrefix, Account_Define.AccountDBTableName, search_name);
            Account_OnlyAID friendinfo = GenericFetch.FetchFromOnly_Redis<Account_OnlyAID>(DataManager_Define.RedisServerAlias_System, setKey);

            Account_Simple_With_Connection setObj = new Account_Simple_With_Connection();
            if (friendinfo == null)
            {
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = string.Format(@"SELECT aid FROM {0} WITH(NOLOCK)  WHERE UserName = @search_name", Account_Define.AccountDBTableName);
                Cmd.Parameters.AddWithValue("@search_name", search_name);

                SqlDataReader getDr = null;

                if (TB.ExcuteSqlCommand(dbkey, ref Cmd, ref getDr))
                {
                    if (getDr != null)
                    {
                        var r = SQLtoJson.Serialize(ref getDr);
                        string json = mJsonSerializer.ToJsonString(r);
                        getDr.Dispose(); getDr.Close();

                        Account_OnlyAID[] retSet = mJsonSerializer.JsonToObject<Account_OnlyAID[]>(json);
                        Cmd.Dispose();
                        if (retSet.Length > 0)
                        {
                            friendinfo = retSet[0];
                            RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_User, setKey, friendinfo);
                        }
                    }
                }
            }
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            if (friendinfo != null)
            {
                Account getAccInfo = TheSoul.DataManager.AccountManager.GetAccountData(ref TB, friendinfo.aid, ref retError);

                if (getAccInfo.EquipCID > 0 && retError == Result_Define.eResult.SUCCESS)
                {
                    setObj.friendlastconntime = getAccInfo.LastConnTime;
                    setObj.username = getAccInfo.UserName;
                    setObj.charinfo = new Character_Simple(CharacterManager.GetCharacter(ref TB, friendinfo.aid, getAccInfo.EquipCID));
                    setObj.cid = getAccInfo.EquipCID;
                    setObj.aid = friendinfo.aid;
                }
            }
            else
            {
                setObj.aid = 0;
                setObj.charinfo = new Character_Simple();
            }

            return setObj;
        }

        public static Result_Define.eResult UpdateSendGiftTime(ref TxnBlock TB, long AID, long FAID, string dbkey = Friend_Define.FriendList_DBName)
        {
            string setQuery = string.Format("UPDATE {0} SET lastgiftsend = GETDATE() WHERE myaid = {1} AND friendaid = {2}", Friend_Define.FriendList_TableName, AID, FAID);
            return (TB.ExcuteSqlCommand(dbkey, setQuery)) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult UpdateSendGiftTimeReset(ref TxnBlock TB, long AID, string dbkey = Friend_Define.FriendList_DBName)
        {
            string setQuery = string.Format("UPDATE {0} SET lastgiftsend = DATEADD(HOUR, -24, GETDATE()) WHERE myaid = {1}", Friend_Define.FriendList_TableName, AID);
            return (TB.ExcuteSqlCommand(dbkey, setQuery)) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }
    }
}
