using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using TheSoul.DataManager.DBClass;



namespace TheSoul.DataManager.DBClass
{
    public enum GuildStateChangeType
    {
        NONE = 0,
        ISPUBLIC = 1,       // 1 공개/비공개여부
        INTRODUCE = 2,      // 2 소개글
        NOTICE = 3,         // 3 공지사항
        CHECKATTEND = 4,    // 4 출석체크
        DISSOLUTION = 5,    // 5 해산처리
        DONATION = 6,       // 6 길드기부						 
    }

    public enum GuildEntrustState
    {
        NONE = 0,
        ENTRUSTING = 1,     // 기존 길드장이 위임 진행중
        ENTRUSTED = 2,     // 대상이 위임 받음
        REJECTED = 3,     // 기존 길드장이 위임 거절당함
        REJECTING = 4,     // 대상이 위임을 거절함
        ACCEPTED = 5,     // 기존 길드장이 위임 수락받음
    }

    public enum GuildEntrustPopup
    {
        NONE = 0,           // 위임상태없음
        ENTRUSTED = 1,      // 대상이 위임 받음
        REJECTED = 2,       // 기존 길드장이 위임 거절당함
        ACCEPTED = 3,       // 기존 길드장이 위임 수락받음
    }
}

namespace TheSoul.DataManager
{
    public static partial class GuildManager
    {
        const string GuildshardingDBName = "sharding";
        public const string GuildcommonDBName = "common";
        const string ShopGuildSoldLogDBTableName = "ShopGuildSoldLog";
        const string ShopGuildSoldLogPrefix = "Guild_Shop";

        const string SystemGuildDBTableName = "System_Guild";
        const string SystemGuildPrefix = "System_Guild";

        public const string GuildCreationDBTableName = "Guild_GuildCreation";
        const string GuildCreationPrefix = "Guild";

        public const string GuildRankPointDBTableName = "System_GuildRanking_Data";

        public const string GuildJoinerDBTableName = "Guild_Joiner";
        const string GuildJoinerPrefix = "Guild_Joiner";
        const string GuildJoinerSamplePrefix = "Sample_Guild_Joiner";

        public const string GuildRecommendDBTableName = "GuildRecommend";
        const string GuildRecommendPrefix = "GuildRecommend";
        const string GuildSystemMaxLevel = "System_Guild_MaxLevel";
        const int GuildSystemMaxLevel_Default = 10;
        const int GuildSystemExpireTime_Sec = 86400; // sec 1 day 
        const int GuildRecommendCount = 5;
        const int MaxGuildRecommendPool = 50;
        const int GuildRecommendRefreshTime = 10;

        public const string PVP_GuildWarRecordDBTableName = "PVP_GuildWarRecord";//acva 20151006 길드전용. 길드의 전적기록용.(네이밍 통일을 위해 PVP_GuildRecord 에서 이름 변경)
        public const string PVP_GuildWarRecord_JoinerDBTableName = "PVP_GuildWarRecord_Joiner";//acva 20151006 길드전용. 길드원들의 길드전 전적기록용.

        public const string GuildBanishLogDBTableName = "GuildBanishLog";

        const string ConstTableName = "Const";
        const string AccountTableName = "Account";
        const string ShopGuildTableName = "GameData_ShopGuild";

        const string Donation_Gold = "DEF_GUILD_DONATION_1_USE_GOLD";
        const string Donation_Ruby = "DEF_GUILD_DONATION_2_USE_RUBY";
        const string Donation_Rubys = "DEF_GUILD_DONATION_3_USE_RUBY";

        const string Guild_Create_Gold = "DEF_GUILD_CREAT_GOLD";

        public static Guild GetGuildInfo(ref TxnBlock TB, long AID, string dbkey = GuildcommonDBName, bool Flush = false)
        {
            string setQuery = string.Format(@"Select 0 as cid, joiner.GuildID as guild_id, guild.GuildName as guild_name, guild.GuildMark as guild_mark,  Case When JoinerState = 'S' Then 'Y' Else 'N' End as guild_state, Case When DATEDIFF(D,isnull(TodayAttendDate, '1970-01-01'),GETDATE()) = 0 then 'Y' else 'N' end as guild_attend
                                            From {0} as joiner WITH(INDEX(IDX_JoinerAID)) inner join {1} as guild on joiner.GuildID=guild.GuildID Where joiner.JoinerAID = {2} And joiner.JoinerState='S'", GuildJoinerDBTableName, GuildCreationDBTableName, AID);
            Guild setGuild = DataManager.GenericFetch.FetchFromDB<Guild>(ref TB, setQuery, dbkey);
            if(setGuild == null)
            {
                setGuild = new Guild();
                setGuild.guild_id = 0;
                setGuild.guild_name = string.Empty;
                setGuild.guild_state = "N";
                setGuild.guild_attend = "N";
                setGuild.guild_mark = 0;
            }
            return setGuild;
        }

        public static int GetGuildJoinerCount(ref TxnBlock TB, long GuildID, string JoinerState = "S", string dbkey = GuildcommonDBName)
        {
            string setQuery = string.Format("Select COUNT(*) as count FROM {0} WITH(NOLOCK)  Where GuildID = {1} And JoinerState = N'{2}'", GuildJoinerDBTableName, GuildID, JoinerState);
            GuildCount retObj = DataManager.GenericFetch.FetchFromDB<GuildCount>(ref TB, setQuery, dbkey);
            return retObj.count;
        }

        
        public static bool GuildNameCheck(ref TxnBlock TB, string GuildName, string dbkey = GuildcommonDBName, bool Flush = false)
        {
            //길드이름 체크
            bool returnValue = false;
            string setQuery = string.Format("SELECT Count(*) as count FROM {0} WITH(NOLOCK)  WHERE GuildName = N'{1}'", GuildCreationDBTableName, GuildName);
            GuildCount retObj = DataManager.GenericFetch.FetchFromDB<GuildCount>(ref TB, setQuery, dbkey);
            if (retObj.count > 0)
            {
                returnValue = true;
            }
            return returnValue;
        }

        public static Guild_GuildCreation GetGuilData(ref TxnBlock TB, long GuildID, bool Flush = false, string dbkey = GuildcommonDBName)
        {
            string setKey = string.Format("{0}_{1}_{2}", GuildCreationPrefix, GuildCreationDBTableName, GuildID);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  Where GuildID = {1}", GuildCreationDBTableName, GuildID);
            Guild_GuildCreation retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<Guild_GuildCreation>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);

            if (retObj == null)
                retObj = new Guild_GuildCreation();

            System_GuildRanking_Data setRankPoint = GetGuildRankPoint(ref TB, GuildID, Flush);
            retObj.GuildRankingPoint = setRankPoint.weekGuildRankPoint;

            return retObj;
        }
        
        // guildranking point per week - ADD by Manstar 2015/08/18
        public static System_GuildRanking_Data GetGuildRankPoint(ref TxnBlock TB, long gid, bool Flush = false, string dbkey = GuildcommonDBName)
        {
            long week = PvPManager.GetSeperater_Week(ref TB);

            string setKey = string.Format("{0}_{1}_{2}", GuildCreationPrefix, GuildRankPointDBTableName, gid);
            string setQuery = string.Format("SELECT TOP 1 * FROM {0} WITH(NOLOCK) Where gid = {1} AND seperater = {2}", GuildRankPointDBTableName, gid, week);
            System_GuildRanking_Data retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<System_GuildRanking_Data>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);

            return (retObj == null) ? new System_GuildRanking_Data() : retObj;
        }

        public static int GetGuildLV(ref TxnBlock TB, long AID, string dbkey = GuildcommonDBName , bool Flush = false)
        {
            // string setQuery = string.Format("SELECT A.GuildLevel as count FROM {0} A, {1} B WITH(NOLOCK)  WHERE A.GuildID = B.GuildID AND B.JoinerAID = {2}", GuildCreationDBTableName, GuildJoinerDBTableName, AID);
            string setQuery = string.Format("SELECT A.GuildLevel as count FROM {1} as B WITH(NOLOCK) INNER JOIN {0} as A WITH(NOLOCK) ON A.GuildID = B.GuildID WHERE B.JoinerAID = {2}"
                , GuildCreationDBTableName, GuildJoinerDBTableName, AID);
            GuildCount retObj = DataManager.GenericFetch.FetchFromDB<GuildCount>(ref TB, setQuery, dbkey);
            if(retObj==null)
                retObj= new GuildCount();
            return retObj.count;
        }

        public static int GetGuildLV_From_GID(ref TxnBlock TB, long GID, string dbkey = GuildcommonDBName, bool Flush = false)
        {
            Guild_GuildCreation retObj = GetGuilData(ref TB, GID, Flush);
            return retObj.GuildLevel;
        }

        public static int GetGuildAttendCount(ref TxnBlock TB, long GuildID, string dbkey = GuildcommonDBName, bool Flush = false)
        {
            // TODO : use redis
            string setQuery = string.Format("SELECT COUNT(*) AS count FROM {0} WITH(NOLOCK)  WHERE YesterdayAttendDate IS NOT NULL AND GuildID = {1} ", GuildJoinerDBTableName, GuildID);
            GuildCount retObj = DataManager.GenericFetch.FetchFromDB<GuildCount>(ref TB, setQuery, dbkey);
            return retObj.count;
        }

        public static int GetToDayAttendCount(ref TxnBlock TB, long GuildID, string dbkey = GuildcommonDBName, bool Flush = false)
        {
            string setQuery = string.Format("SELECT COUNT(*) AS count FROM {0} WITH(NOLOCK)  WHERE CONVERT(nvarchar(10),TodayAttendDate,121)=N'{2}' AND GuildID = {1} ", GuildJoinerDBTableName, GuildID, DateTime.Today.ToString("yyyy-MM-dd"));
            GuildCount retObj = DataManager.GenericFetch.FetchFromDB<GuildCount>(ref TB, setQuery, dbkey);
            return retObj.count;
        }
        
        // 길드 위임 상태 갱신
        public static void UpdateGuildEntrustState(ref TxnBlock TB, string tableName, long ParamGID, long ParamAID, int entrustState, string dbkey = GuildcommonDBName)
        {
            string setStateQuery = "";

            // 위임 요청 시 날짜 갱신
            if (entrustState == (int)GuildEntrustState.ENTRUSTED || entrustState == (int)GuildEntrustState.ENTRUSTING)
            {
                setStateQuery = string.Format("UPDATE {0} SET EntrustState = {1}, EntrustAskDate = GETDATE() WHERE GuildID = {2} AND JoinerAID = {3}", GuildJoinerDBTableName, entrustState, ParamGID, ParamAID);
            }
            else if (entrustState == (int)GuildEntrustState.NONE)
            {
                setStateQuery = string.Format("UPDATE {0} SET EntrustState = {1}, EntrustAskDate = NULL WHERE GuildID = {2} AND JoinerAID = {3}", GuildJoinerDBTableName, entrustState, ParamGID, ParamAID);
            }
            else
            {
                setStateQuery = string.Format("UPDATE {0} SET EntrustState = {1} WHERE GuildID = {2} AND JoinerAID = {3}", GuildJoinerDBTableName, entrustState, ParamGID, ParamAID);
            }
            TB.ExcuteSqlCommand(dbkey, setStateQuery);
        }

        public static void SelectGuildInfoEntrust(ref TxnBlock TB, ref int resultCode, ref int popuptype, ref long resultAID, long AID, string dbkey = GuildcommonDBName)
        {
            // 길드 관련 추가 기능 SP -> Func 수정 작업
            long GuildID = 0, createAID = 0;
            int entrustState = 0;
            bool flag = false;
            DateTime entrustAskDate = new DateTime();
            DateTime todayDate = DateTime.Now;

            resultCode = (int)GetGuildEntrustInfo(ref TB, AID, ref GuildID, ref createAID, ref entrustState, ref entrustAskDate, ref flag);

            if (resultCode == (int)Result_Define.eResult.NO_GUILD_USER)
            {
                resultCode = (int)Result_Define.eResult.SUCCESS;
                popuptype = (int)GuildEntrustPopup.NONE;
                resultAID = 0;
                return;
            }

            if (entrustState == (int)GuildEntrustState.ENTRUSTED)
            {
                resultCode = (int)Result_Define.eResult.SUCCESS;
                popuptype = (int)GuildEntrustPopup.ENTRUSTED;
                resultAID = createAID;
            }
            else if (entrustState == (int)GuildEntrustState.ACCEPTED)
            {
                resultCode = (int)Result_Define.eResult.SUCCESS;
                popuptype = (int)GuildEntrustPopup.ACCEPTED;
                resultAID = createAID;

                UpdateGuildEntrustState(ref TB, GuildJoinerDBTableName, GuildID, resultAID, (int)GuildEntrustState.NONE);
                UpdateGuildEntrustState(ref TB, GuildJoinerDBTableName, GuildID, AID, (int)GuildEntrustState.NONE);
            }
            else
            {
                resultCode = (int)Result_Define.eResult.SUCCESS;
                popuptype = (int)GuildEntrustPopup.NONE;
                resultAID = 0;
            }

            if (AID == createAID)
            {
                if (entrustState == (int)GuildEntrustState.REJECTED)
                {
                    SqlDataReader getDr2 = null;
                    string setQuery2 = string.Format("SELECT JoinerAID FROM {0} WITH(NOLOCK)  WHERE GuildID = {1} AND EntrustState = {2}", GuildJoinerDBTableName, GuildID, (int)GuildEntrustState.REJECTING);
                    TB.ExcuteSqlCommand(dbkey, setQuery2, ref getDr2);
                    if (getDr2 != null)
                    {
                        if (getDr2.Read())
                        {
                            resultAID = System.Convert.ToInt64(getDr2["JoinerAID"]);
                            getDr2.Dispose(); getDr2.Close();
                            UpdateGuildEntrustState(ref TB, GuildJoinerDBTableName, GuildID, resultAID, (int)GuildEntrustState.NONE);
                            UpdateGuildEntrustState(ref TB, GuildJoinerDBTableName, GuildID, AID, (int)GuildEntrustState.NONE);

                            resultCode = (int)Result_Define.eResult.SUCCESS;
                            popuptype = (int)GuildEntrustPopup.REJECTED;
                        }
                        else
                        {
                            getDr2.Dispose(); getDr2.Close();
                        }
                    }
                }
            }

            return;
        }

    }

}
