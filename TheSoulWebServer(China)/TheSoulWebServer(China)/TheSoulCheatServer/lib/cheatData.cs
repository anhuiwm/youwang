using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager;
using TheSoul.DataManager.Global;
using TheSoul.DataManager.DBClass;


namespace TheSoulCheatServer.lib
{
    public class cheatData
    {
        public static User_account GetUserAID(ref TxnBlock TB, string username, string dbkey = GuildManager.GuildcommonDBName)
        {
            string accQuery = string.Format("select * from dbo.User_account where nick_name=N'{0}'", username);
            return TheSoul.DataManager.GenericFetch.FetchFromDB<User_account>(ref TB, accQuery, dbkey);
        }

        public static List<User_Mail_Datail> GetUser_Mail_All_List(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Mail_Define.Mail_Info_DB)
        {
            string setQuery = string.Format(@"SELECT *, (DATEDIFF(SS,GETDATE(), closedate)) as remaintime FROM {0} WHERE aid = {1} ORDER BY mailseq DESC", Mail_Define.Mail_Box_TableName, AID);
            List<User_Mail_Datail> listObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_Mail_Datail>(ref TB, setQuery, dbkey);
            return listObj;
        }

        public static List<GuildJoiner> GetGuildJoinerAllList(ref TxnBlock TB, long GuildID, bool myguild = false, bool Flush = false, string dbkey = GuildManager.GuildcommonDBName)
        {
            //길드정보에 노출되는 길드원리스트
            string setQuery = string.Format("SELECT * FROM dbo.{0} WITH(NOLOCK) WHERE GUILDID = {1}  AND JoinerState = 'S' ", GuildManager.GuildJoinerDBTableName, GuildID);
            List<GuildJoiner> getInfo = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GuildJoiner>(ref TB, setQuery, dbkey);

            return getInfo;
        }

        public static List<User_GuerrillaDungeon_Play> GetUser_DarkPassagePlayList(ref TxnBlock TB, long AID, string dbkey = GuildManager.GuildcommonDBName)
        {
            string setQuery = string.Format("SELECT * FROM dbo.{0} WHERE AID = {1}  AND regdate = CONVERT(nvarchar(10),GETDATE(),121) ", Dungeon_Define.Dark_Passage_Play_TableName, AID);
            List<User_GuerrillaDungeon_Play> getInfo = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_GuerrillaDungeon_Play>(ref TB, setQuery, dbkey);

            return getInfo;
        }

        public static List<User_Mission_Play> GetUser_MissionPlayList(ref TxnBlock TB, long AID, string dbkey = Account_Define.AccountShardingDB)
        {
            string setQuery = string.Format("SELECT * FROM dbo.{0} WHERE AID = {1}  AND regdate = CONVERT(nvarchar(10),GETDATE(),121) ", Dungeon_Define.Mission_Play_TableName, AID);
            List<User_Mission_Play> getInfo = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_Mission_Play>(ref TB, setQuery, dbkey);

            return getInfo;
        }
    }
}