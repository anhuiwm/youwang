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
    public class FriendsList
    {
        public long myaid { get; set; }
        public long friendaid { get; set; }
        public string friendname { get; set; }
        public int keysendremaintime { get; set; }
        public string acceptfriend { get; set; }
        public string delflag { get; set; }
        public string newyn { get; set; }
        public DateTime acceptdate { get; set; }
        public int remaintime { get; set; }
    }

    public class RecommandFriends : Friends
    {
        public int friendscount { get; set; }
        public int friendswait { get; set; }
        public long EquipCID { get; set; }
        public RecommandFriends() { lastgiftsend = DateTime.Now; }
    }

    public class Friends
    {
        public long friendaid { get; set; }
        public int keysendremaintime { get; set; }
        public string friendname { get; set; }
        public int friendlastconntime { get; set; }
        public DateTime lastgiftsend { get; set; }
        public Character_Simple charinfo { get; set; }

        public Friends() { lastgiftsend = DateTime.Now; }
        public Friends(RecommandFriends setInfo) 
        {
            friendaid = setInfo.friendaid;
            keysendremaintime = setInfo.keysendremaintime;
            friendname = setInfo.friendname;
            friendlastconntime = setInfo.friendlastconntime;
            lastgiftsend = setInfo.lastgiftsend;
            charinfo = setInfo.charinfo;
        }
    }

    //aid	cid1	level	class	warpoint

    public class RetFriendsInfo
    {
        public long aid { get; set; }
        public string friendsname { get; set; }
        public long cid { get; set; }
        public int charclass { get; set; }
        public short level { get; set; }
        public int warpoint { get; set; }
    }    

    public class FriendStandByCount
    {
        public int count { get; set; }
    }
    public class FriendCount
    {
        public int count { get; set; }
    }
    public class RecommendFriendCount
    {
        public int count { get; set; }
    }
}
