using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Net.Http;
using System.Net;

using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Tools;
using TheSoul.DataManager.Global;
using TheSoulGMTool.DBClass;
using TheSoulWebServer.Tools;

namespace TheSoulGMTool
{
    public partial class GMDataManager
    {
        public static long GetUserRestrictCount(ref TxnBlock TB, string dbkey = GMData_Define.GlobalDBName)
        {
            string setQuery = string.Format(@"Select count(*) as number From {0} WITH(NOLOCK) Where DATEDIFF(MINUTE,GETDATE(), login_restrict_enddate) > 0 or DATEDIFF(MINUTE,GETDATE(), chat_restrict_endate) > 0", Global_Define.User_Account_Restrict_TableName);
            GM_Number retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.number;
        }

        public static List<GM_account_restrict> GetUserRestrictList(ref TxnBlock TB, int page, string dbkey = GMData_Define.GlobalDBName)
        {
            string setQuery = string.Format(@"SELECT TOP({1}) resultTable.* FROM (
                                                    Select TOP {2} ROW_NUMBER() over (order by login_restrict_reg_date Desc, chat_restrict_reg_date Desc) as rownumber, *
                                                                , Case When DATEDIFF(MINUTE,GETDATE(), login_restrict_enddate)>0 Then 1 Else 0 End as loginActive
                                                                , Case When DATEDIFF(MINUTE,GETDATE(), chat_restrict_endate)>0 Then 1 Else 0 End as chatActive
                                                    From {0} WITH(NOLOCK) Where DATEDIFF(MINUTE,GETDATE(), login_restrict_enddate) > 0 or DATEDIFF(MINUTE,GETDATE(), chat_restrict_endate) > 0) as resultTable
                                                WHERE rownumber > {3}",
                                                Global_Define.User_Account_Restrict_TableName,
                                                GMData_Define.pageSize, (GMData_Define.pageSize * page), (page - 1) * GMData_Define.pageSize);
            List<GM_account_restrict> retObj = GenericFetch.FetchFromDB_MultipleRow<GM_account_restrict>(ref TB, setQuery, dbkey);
            if (retObj == null || retObj.Count == 0)
                retObj = new List<GM_account_restrict>();
            retObj.ForEach(item =>
            {
            });
            return retObj;
        }

        public static GM_Global_UserSimple GetUserGloblaSimpleInfo(ref TxnBlock TB, long AID, string dbkey = GMData_Define.GlobalDBName)
        {
            string setQuery = string.Format(@"Select A.user_account_idx as AID, A.platform_user_id, b.platform_idx
                                                From {0} as A WITH(NOLOCK) left outer join {1} as B WITH(NOLOCK) on A.platform_user_id = B.platform_user_id
                                                Where A.user_account_idx = {2}", Global_Define.User_Account_TableName, Global_Define.User_Platform_ID_TableName, AID);
            GM_Global_UserSimple retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Global_UserSimple>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new GM_Global_UserSimple();
        }

        public static GM_Global_UserSimple GetSearchAID_BYSnailPlatformID(ref TxnBlock TB, string platformid, string dbkey = GMData_Define.GlobalDBName)
        {
            string setQuery = string.Format(@"Select A.user_account_idx as AID, A.platform_user_id, b.platform_idx
                                                From {0} as A WITH(NOLOCK) left outer join {1} as B WITH(NOLOCK) on A.platform_user_id = B.platform_user_id 
                                                Where A.platform_user_id = N'{2}'", Global_Define.User_Account_TableName, Global_Define.User_Platform_ID_TableName, platformid);
            GM_Global_UserSimple retObj = GenericFetch.FetchFromDB<GM_Global_UserSimple>(ref TB, setQuery, dbkey);
            return retObj == null ? new GM_Global_UserSimple() : retObj;
        }

        public static long GetSearchAID_BYSnailPlatformIndex(ref TxnBlock TB, long platformidx, string dbkey = GMData_Define.GlobalDBName)
        {
            string setQuery = string.Format(@"Select A.user_account_idx as AID, A.platform_user_id, b.platform_idx
                                                From {0} as A WITH(NOLOCK) left outer join {1} as B WITH(NOLOCK) on A.platform_user_id = B.platform_user_id
                                                Where A.platform_idx = {2}", Global_Define.User_Account_TableName, Global_Define.User_Platform_ID_TableName, platformidx);
            GM_Global_UserSimple retObj = GenericFetch.FetchFromDB<GM_Global_UserSimple>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.AID;
        }

        public static List<user_play_server_config> GetUserPlayServerList(ref TxnBlock TB, long AID, string dbkey = GMData_Define.GlobalDBName)
        {
            string setQuery = string.Format(@"Select * From {0} WITH(NOLOCK) Where user_account_idx = {1}", Global_Define.User_PlayServer_TableName, AID);
            List<user_play_server_config> retObj = GenericFetch.FetchFromDB_MultipleRow<user_play_server_config>(ref TB, setQuery, dbkey);
            if (retObj == null || retObj.Count == 0)
                retObj = new List<user_play_server_config>();
            List<server_group_config> serverList = GlobalManager.GetServerGroupList(ref TB);
            retObj.ForEach(item =>
            {
                item.server_group_name = serverList.Find(serverItem => serverItem.server_group_id == item.server_group_id).server_group_name;
            });
            return retObj;
        }

        public static long GetDeleteUserCount(ref TxnBlock TB, string dbkey = GMData_Define.GlobalDBName)
        {
            string setQuery = string.Format(@"Select count(*) as number From {0} WITH(NOLOCK) Where platform_type = -1", Global_Define.User_Account_TableName);
            GM_Number retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.number;
        }

        public static List<user_account_config> GetDeleteUserList(ref TxnBlock TB, int page, string dbkey = GMData_Define.GlobalDBName)
        {
            string setQuery = string.Format(@"SELECT TOP({1}) resultTable.* FROM (
                                                    Select TOP {2} ROW_NUMBER() over (order by reg_date Desc) as rownumber, *
                                                    From {0} WITH(NOLOCK) Where platform_type = -1) as resultTable
                                                WHERE rownumber > {3}",
                                                Global_Define.User_Account_TableName,
                                                GMData_Define.pageSize, (GMData_Define.pageSize * page), (page - 1) * GMData_Define.pageSize);
            List<user_account_config> retObj = GenericFetch.FetchFromDB_MultipleRow<user_account_config>(ref TB, setQuery, dbkey);
            if (retObj == null || retObj.Count == 0)
                retObj = new List<user_account_config>();
            retObj.ForEach(item =>
            {
                string[] setData = System.Text.RegularExpressions.Regex.Split(item.platform_user_id, "_#Deleted_");
                item.platform_user_id = setData[0];
                item.platform_type = System.Convert.ToInt32(setData[1].Split('_')[1]);
            });
            return retObj;
        }

        public static List<GM_user_account_config> GetAllUserAccountConfig(ref TxnBlock TB, string platformid, string dbkey = GMData_Define.GlobalDBName)
        {
            string setQuery = string.Format(@"Select * From {0} WITH(Nolock) Where platform_user_id like N'%{1}%'", Global_Define.User_Account_TableName, platformid);
            List<user_account_config> userList = GenericFetch.FetchFromDB_MultipleRow<user_account_config>(ref TB, setQuery, dbkey);

            List<GM_user_account_config> retObj = new List<GM_user_account_config>();
            foreach (user_account_config item in userList)
            {
                GM_user_account_config userInfo = new GM_user_account_config();
                userInfo.user_account_status = item.platform_type;
                if (item.platform_type > 0)
                {
                    userInfo.platform_user_id = item.platform_user_id;
                    userInfo.platform_type = item.platform_type;
                }
                else
                {
                    string[] setData = System.Text.RegularExpressions.Regex.Split(item.platform_user_id, "_#Deleted_");
                    userInfo.platform_user_id = setData[0];
                    userInfo.platform_type = System.Convert.ToInt32(setData[1].Split('_')[1]);
                }
                userInfo.user_account_idx = item.user_account_idx;
                userInfo.reg_date = item.reg_date;
                
                List<user_play_server_config> userServerList = GetUserPlayServerList(ref TB, item.user_account_idx);
                userServerList.ForEach(serveritem => {
                    userInfo.play_server = string.IsNullOrEmpty(userInfo.play_server) ? string.Format("{0} : {1}", serveritem.server_group_name, serveritem.user_server_nickname) : string.Format("{0}<br />{1} : {2}", userInfo.play_server, serveritem.server_group_name, serveritem.user_server_nickname);
                });
                retObj.Add(userInfo);
            }
            return retObj;
        }
    }
}