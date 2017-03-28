using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;


namespace TheSoul.DataManager
{
    public static partial class Friend_Define
    {
        public const string FriendList_DBName = "common";
        public const string AccountInfo_DBName = "sharding";

        //public const string FriendList_DBName_3w = "common_3w";
        //public const string AccountInfo_DBName_3w = "sharding_3w";

        public const string FriendList_TableName = "User_FriendsList";
        public const string FriendDeleteLog_TableName = "User_FriendDeleteLog";

        public const string FriendListPrefix = "User_FriendList";
        public const string FriendStandByCountPrefix = "User_FriendStandByCount";
        public const string FriendCountPrefix = "User_FriendCount";
        public const string RequestFriendPrefix = "User_RequestFriendList";
        public const string RecommendFriendPrefix = "User_RcommendFriendList";
        public const string RecommendFriendList_TableName = "RecommendFriendServer";
        public const string RecommendGradeCount_TableName = "RecommendGradeCount";

        //public const string FriendList_TableName = "User_FriendsList";

        public const string RecommendFriendList_DBName = "common";
        public const string RecommendFriendCountPrefix = "RcommendFriendCount";
        public const int RecommendCount = 5;
        public const int RecommendRefreshTime = 10;
        public const int RecommendLevelRange = 5;
        public const int MaxFriendCount = 30;
        public const int MaxFriendPool = 1000;
        public const int MaxDeleteFriendCountPerDay = 10;

        public const string FriendSearchPrefix = "Friend_Search";

        public const int DeleteCheckTime_Hour = 24;


        public enum eFriendReturnKeys
        {
            MyFriendList,
            ReqFriendList,
            RecommendFriendList,
            SendRewardItemID,
            SendRewardCount,
            SendRemainTime,
            MyFaceBookFriendsList,
        }

        public static readonly Dictionary<eFriendReturnKeys, string> Friend_Ret_KeyList = new Dictionary<eFriendReturnKeys, string>()
        {
            { eFriendReturnKeys.MyFriendList,           "my_list"          },
            { eFriendReturnKeys.ReqFriendList,           "request_friend_list"          },
            { eFriendReturnKeys.RecommendFriendList,     "recommend_friend_list"        },
            { eFriendReturnKeys.SendRewardItemID,     "send_reward_itemid"        },
            { eFriendReturnKeys.SendRewardCount,     "send_reward_count"        },
            { eFriendReturnKeys.SendRemainTime,     "send_remaintime"        },
            { eFriendReturnKeys.MyFaceBookFriendsList,     "fb_list"        },
        };
    }
}
