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
        public static void GetServerinit(ref TxnBlock TB, long serverID = 1)
        {
            GlobalManager.GM_CurrentDB = null;
            bool bForceInit = true;
            if (serverID == 999  || serverID==201)
                bForceInit = false;
            TheSoulDBcon.GetInstance().TheSoulDBInitFromGlobal(ref TB, (int)serverID, bForceInit);
            TB.SetLogData(SystemData_Define.Service_AreaKey, (int)TheSoulDBcon.service_area);
            string savePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
            GetGMServerIni(ref TB, savePath);
        }

        public static void GetServerinit(ref TxnBlock TB, ref WebQueryParam queryFetcher, long serverID = 1)
        {
            TB.Elog = queryFetcher.DBLog;
            GlobalManager.GM_CurrentDB = null;
            bool bForceInit = true;
            if (serverID == 999 || serverID == 201)
                bForceInit = false;
            TheSoulDBcon.GetInstance().TheSoulDBInitFromGlobal(ref TB, (int)serverID, bForceInit);
            TB.SetLogData(SystemData_Define.Service_AreaKey, (int)TheSoulDBcon.service_area);
            string savePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
            GetGMServerIni(ref TB, savePath);
        }

        public static string GetServerTime(ref TxnBlock TB, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("Select CONVERT(nvarchar(16), GETDATE(), 121) as name ");
            GM_String retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_String>(ref TB, setQuery, dbkey);
            return retObj == null ? DateTime.Now.ToString("yyyy-MM-dd hh:mm") : retObj.name;
        }

        public static void SetRedisDataInit(ref Dictionary<long, TxnBlock> server)
        {
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                long serverID = tb.Key;
                server_config dbServer = GlobalManager.GetServerList(ref TB).Find(item => item.server_type.Equals("web_server") && item.server_group_id == serverID);
                string WebIP = string.IsNullOrEmpty(dbServer.server_public_ip) ? dbServer.server_private_ip : dbServer.server_public_ip;
                string WebPort = (dbServer.server_public_port == 0) ? dbServer.server_private_port.ToString() : dbServer.server_public_port.ToString();
                string url = string.Format("http://{0}:{1}/RequestPrivateServer.aspx?op=redis_flush", WebIP, WebPort);
                //string dataParam = "op=redis_flush";
                try
                {
                    HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(url);
                    webReq.Method = "Get";
                    webReq.ContentType = "application/x-www-form-urlencoded";

                    HttpWebResponse wRespFirst = (HttpWebResponse)webReq.GetResponse();
                    Stream respPostStream = wRespFirst.GetResponseStream();
                    StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);
                    string resultPost = readerPost.ReadToEnd();

                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
        }

        private static Result_Define.eResult CreateNoticeIndex(ref TxnBlock TB, ref long idx, int noticeType = 1, string dbkey = GMData_Define.GlobalDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            idx = 0;
            string setQuery = string.Format("Insert into {0} (noticeType) Values ({1})", GMData_Define.NoticeTable, noticeType);

            retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            if (retError == Result_Define.eResult.SUCCESS)
            {
                string setQuery2 = string.Format("SELECT IDENT_CURRENT('{0}') as number", GMData_Define.NoticeTable);
                GM_Number retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery2, dbkey);
                idx = retObj.number;

            }
            return retError;
        }

        public static long GetMaxIndexCheck(ref TxnBlock TB, int system = 1, GMData_Define.eSystemType systemType = GMData_Define.eSystemType.EVENT, string dbkey = GMData_Define.GlobalDBName)
        {
            string setQuery = string.Format("Select ISNULL(MAX(idx),0) as number From {0} WITH(NOLOCK) Where systemType = {1}", GMData_Define.Global_Index_TableList[system], (int)systemType);
            GM_Number maxIndexValue = GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            return maxIndexValue.number;
        }

        public static long GetMaxIndexValue(ref TxnBlock TB, int system = 1, GMData_Define.eSystemType systemType = GMData_Define.eSystemType.EVENT, string dbkey = GMData_Define.GlobalDBName)
        {
            /*
             * systemtype = 1 - event, 2 - package
             */
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            string setQuery = string.Format("Select ISNULL(MAX(idx),0) + 1 as number From {0} WITH(NOLOCK) Where systemType = {1}", GMData_Define.Global_Index_TableList[system], (int)systemType);
            GM_Number maxIndexValue = GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            if (maxIndexValue == null || maxIndexValue.number == 1)
            {
                //인덱스가 없을때 설정되어 있는 db의 시스템 정보를 가져와 설정
                string countQuery = string.Format("Select count(idx) as number From {0} WITH(NOLOCK) Where systemType = {1}", GMData_Define.Global_Index_TableList[system], (int)systemType);
                GM_Number eventCount = GenericFetch.FetchFromDB<GM_Number>(ref TB, countQuery, dbkey);
                if (eventCount.number == 0)
                {
                    if (systemType == GMData_Define.eSystemType.EVENT)
                    {
                        if (system == 1)
                        {
                            List<System_Event> eventIndesxList = TriggerManager.GetSystem_Event_All_List(ref TB);
                            foreach (System_Event item in eventIndesxList)
                            {
                                retError = InsertSystemIndex(ref TB, item.Event_ID, system, systemType);
                                if (retError != Result_Define.eResult.SUCCESS)
                                    break;
                            }
                        }
                        else
                        {
                            string indexQuery = string.Format("Select DISTINCT eventboxid as number From {0} WITH(NOLOCK)", Trigger_Define.System_Event_Reward_Box_TableName);
                            List<GM_Number> eventIndesxList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GM_Number>(ref TB, indexQuery, GMData_Define.ShardingDBName);
                            foreach (GM_Number item in eventIndesxList)
                            {
                                retError = InsertSystemIndex(ref TB, item.number, system, systemType);
                                if (retError != Result_Define.eResult.SUCCESS)
                                    break;
                            }
                        }
                    }
                    else if (systemType == GMData_Define.eSystemType.PACKAGE)
                    {
                        if (system == 1)
                        {
                            List<System_Package_List> eventIndesxList = ShopManager.GetShop_System_Package_List(ref TB, Shop_Define.eBillingType.None, true);
                            eventIndesxList.AddRange(ShopManager.GetShop_System_Package_Cheap_List(ref TB, Shop_Define.eBillingType.None, true));
                            foreach (System_Package_List item in eventIndesxList)
                            {
                                retError = InsertSystemIndex(ref TB, item.Package_ID, system, systemType);
                                if (retError != Result_Define.eResult.SUCCESS)
                                    break;
                            }
                        }
                        else
                        {
                            string indexQuery = string.Format("Select DISTINCT RewardBoxID as number From {0} WITH(NOLOCK)", Shop_Define.Shop_System_Package_RewardBox_TableName);
                            List<GM_Number> eventIndesxList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GM_Number>(ref TB, indexQuery, GMData_Define.ShardingDBName);
                            string indexQuery2 = string.Format("Select DISTINCT RewardBoxID as number From {0} WITH(NOLOCK)", Shop_Define.Shop_System_Package_Cheap_RewardBox_TableName);
                            eventIndesxList.AddRange(TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GM_Number>(ref TB, indexQuery2, GMData_Define.ShardingDBName));
                            foreach (GM_Number item in eventIndesxList)
                            {
                                retError = InsertSystemIndex(ref TB, item.number, system, systemType);
                                if (retError != Result_Define.eResult.SUCCESS)
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (system == 1)
                        {
                            List<System_Event_7Day> eventIndesxList = TriggerManager.GetSystem_7Day_Event_List(ref TB);
                            foreach (System_Event_7Day item in eventIndesxList)
                            {
                                retError = InsertSystemIndex(ref TB, item.Event_ID, system, systemType);
                                if (retError != Result_Define.eResult.SUCCESS)
                                    break;
                            }
                        }
                        else
                        {
                            string indexQuery = string.Format("Select DISTINCT RewardBoxID as number From {0} WITH(NOLOCK)", Trigger_Define.System_7Day_Event_Reward_TableName);
                            List<GM_Number> eventIndesxList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GM_Number>(ref TB, indexQuery, GMData_Define.ShardingDBName);
                            foreach (GM_Number item in eventIndesxList)
                            {
                                retError = InsertSystemIndex(ref TB, item.number, system, systemType);
                                if (retError != Result_Define.eResult.SUCCESS)
                                    break;
                            }
                        }
                    }
                }
                setQuery = string.Format("Select ISNULL(MAX(idx),0) + 1 as number From {0} WITH(NOLOCK) Where systemType = {1}", GMData_Define.Global_Index_TableList[system], (int)systemType);
                maxIndexValue = GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
                if (maxIndexValue == null)
                    maxIndexValue = new GM_Number();
            }

            if (maxIndexValue.number > 0 && retError == Result_Define.eResult.SUCCESS)
            {
                retError = InsertSystemIndex(ref TB, maxIndexValue.number, system, systemType);
            }
            if (retError != Result_Define.eResult.SUCCESS)
            {
                maxIndexValue.number = 0;
            }
            return maxIndexValue.number;
        }

        public static Result_Define.eResult InsertSystemIndex(ref TxnBlock TB, long index, int system = 1, GMData_Define.eSystemType systemType = GMData_Define.eSystemType.EVENT, string dbkey = GMData_Define.GlobalDBName)
        {
            string setQuery = string.Format("Insert into {0} (idx, systemType, regdate) Values ({1}, {2}, getdate())", GMData_Define.Global_Index_TableList[system], index, (int)systemType);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static List<ListItem> GetServerListItem(ref TxnBlock TB, long serverID = -1, bool addAll = true, string dbkey = GMData_Define.GlobalDBName)
        {
            List<server_group_config> serverGourpList = GlobalManager.GetServerGroupList(ref TB);
            List<server_config> serverConfigList = GlobalManager.GetServerList(ref TB);
            string userUseServer = GetUserCookies("UserServer");
#if DEBUG
            userUseServer = string.IsNullOrEmpty(userUseServer) ? string.Join(",", serverGourpList.Select(x => x.server_group_id.ToString()).ToArray()) : userUseServer;
#endif
            List<string> userServer = System.Text.RegularExpressions.Regex.Split(userUseServer, ",").ToList();

            List<ListItem> serverList = new List<ListItem>();
            if (addAll)
            {
                ListItem setitem = new ListItem("Select", "-1");
                if (serverID.Equals(-1))
                    setitem.Selected = true;
                serverList.Add(setitem);
            }
            foreach (server_group_config server in serverGourpList)
            {
                server.server_info = new List<server_config>();
                serverConfigList.ForEach(setServerinfo =>
                {
                    if (setServerinfo.server_group_id == server.server_group_id
                                && (setServerinfo.server_type.Contains("web_server")
                                    || setServerinfo.server_type.Contains("cs_login")
                                        || setServerinfo.server_type.Contains("cs_game"))
                                && setServerinfo.server_group_id > 0
                            )
                    {
                        if (!(setServerinfo.server_type.Equals("web_server") && server.server_info.Find(item => item.Equals("web_server")) != null))
                            server.server_info.Add(setServerinfo);
                    }
                });
                bool userCheck = false;
                if(!string.IsNullOrEmpty(userServer.Find(item => item == server.server_group_id.ToString())) || GetUserCookies() == "superadmin")
                    userCheck = true;
                if (server.server_group_id > 0 && server.server_info.Count > 0 && userCheck)
                {
                    var item = new ListItem(server.server_group_name, server.server_group_id.ToString());
                    if (serverID.ToString().Equals(item.Value))
                        item.Selected = true;
                    serverList.Add(item);
                }
            }
            return serverList;
        }

        public static List<Admin_GlobalNotice> GetAdminGlobalNoticeList(ref TxnBlock TB, Dictionary<string, string> search, string dbkey = GMData_Define.GlobalDBName)
        {
            string searchQuery = "";
            if (search.Count > 0)
            {
                int numCheck = 0;
                foreach (KeyValuePair<string, string> item in search)
                {
                    if (string.IsNullOrEmpty(searchQuery))
                        searchQuery = item.Key.Equals(GMData_Define.eGlobalNoticeSearchKey[GMData_Define.eGlobalNoticeSearch.platform]) ? string.Format("Where ({0} & {1}) != 0", item.Key, item.Value) : int.TryParse(item.Value, out numCheck) ? string.Format("Where {0} = {1}", item.Key, numCheck) : string.Format("Where {0} = N'{1}'", item.Key, item.Value);
                    else
                        searchQuery = item.Key.Equals(GMData_Define.eGlobalNoticeSearchKey[GMData_Define.eGlobalNoticeSearch.platform]) ? string.Format("{0} And ({1} & {2}) != 0", searchQuery, item.Key, item.Value) : int.TryParse(item.Value, out numCheck) ? string.Format("{0} And {1} = {2}", searchQuery, item.Key, numCheck) :  string.Format("{0} And {1} = N'{2}'", searchQuery, item.Key, item.Value);
                }
            }
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) {1} Order by regdate Desc, orderNumber asc", Global_Define.Admin_GlobalNotice_TableName, searchQuery);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<Admin_GlobalNotice>(ref TB, setQuery, dbkey);
        }

        public static Result_Define.eResult InsertGlobalNotice(ref TxnBlock TB, string tag, string style, string title, string contents, string startDate, string endDate, byte active, int ordernum, int platform, string version, string dbkey = GMData_Define.GlobalDBName)
        {
            string gmId = "";
            if (HttpContext.Current.Request.Cookies.Count == 0)
            {
                gmId = "test2";
            }
            else
            {
                gmId = HttpContext.Current.Request.Cookies["mseedadmin"]["userid"];
            }

            Result_Define.eResult retErr = Result_Define.eResult.DB_ERROR;
            string setQuery = string.Format(@"Insert Into {0} (noticeTag, noticeStyle, title, contents, startDate, endDate, regdate, regid, editdate, editid, active, orderNumber, billing_platform_type, target_version) 
                                                                Values (N'{1}',N'{2}',N'{3}',N'{4}','{5}','{6}', getdate(), '{7}', getdate(), '{7}',{8},{9}, {10}, '{11}')",
                                                                Global_Define.Admin_GlobalNotice_TableName, tag, style, title, contents, startDate, endDate, gmId, active, ordernum, platform, version);
            retErr = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            return retErr;

        }

        public static Result_Define.eResult UpdateGlobalNotice(ref TxnBlock TB, long idx, string tag, string style, string title, string contents, string startDate, string endDate, byte active, int ordernum, int platform, string version, string dbkey = GMData_Define.GlobalDBName)
        {
            string gmId = "";
            if (HttpContext.Current.Request.Cookies.Count == 0)
            {
                gmId = "test2";
            }
            else
            {
                gmId = HttpContext.Current.Request.Cookies["mseedadmin"]["userid"];
            }

            Result_Define.eResult retErr = Result_Define.eResult.DB_ERROR;
            //            string setQuery = string.Format(@"Update {0} Set noticeTag = N'{1}', noticeStyle = N'{2}', title = N'{3}', contents = N'{4}', active = {9}, orderNumber = {10}, 
            //                                                            startDate = '{5}', endDate = '{6}', editdate = getdate(), editid = '{7}' Where idx = {8} ",
            //                                                            GlobalManager.Admin_GlobalNotice_TableName, tag, style, title, contents, startDate, endDate, gmId, idx, active, ordernum);
            string setQuery = string.Format(@"MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON idx = {8}
                                                WHEN MATCHED THEN
                                                    UPDATE SET 
                                                    noticeTag = N'{1}'
                                                    , noticeStyle = N'{2}'
                                                    , title = N'{3}'
                                                    , contents = N'{4}'
                                                    , active = {9}
                                                    , orderNumber = {10}
                                                    , startDate = '{5}'
                                                    , endDate = '{6}'
                                                    , editdate = getdate()
                                                    , editid = '{7}'
                                                    , billing_platform_type = {11}
                                                    , target_version = '{12}'
                                                WHEN NOT MATCHED THEN
                                                   INSERT (
                                                        [noticeTag]
                                                       ,[noticeStyle]
                                                       ,[title]
                                                       ,[contents]
                                                       ,[active]
                                                       ,[orderNumber]
                                                       ,[startDate]
                                                       ,[endDate]
                                                       ,[regdate]
                                                       ,[regid]
                                                       ,[editdate]
                                                       ,[editid]
                                                       ,billing_platform_type
                                                       ,target_version
                                                    )
                                                   VALUES (
                                                        N'{1}', N'{2}', N'{3}', N'{4}', N'{9}', N'{10}', N'{5}', N'{6}', getdate(), N'{7}', getdate(), N'{7}', {11}, '{12}'
                                                    );
                                                "
                                            , Global_Define.Admin_GlobalNotice_TableName, tag, style, title, contents, startDate, endDate, gmId, idx, active, ordernum, platform, version);
            retErr = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            return retErr;

        }

        public static Result_Define.eResult DeleteGlobalNotice(ref TxnBlock TB, long idx, string dbkey = GMData_Define.GlobalDBName)
        {
            Result_Define.eResult retErr = Result_Define.eResult.DB_ERROR;
            string setQuery = string.Format(@"Delete From {0} Where idx = {1} ", Global_Define.Admin_GlobalNotice_TableName, idx);
            retErr = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            return retErr;
        }

        public static Result_Define.eResult DeleteServerVersion(ref TxnBlock TB, long serverID, int platformType, int version, string dbkey = GMData_Define.GlobalDBName)
        {
            string setQuery = string.Format("Delete From {0} Where server_group_id = {1} And billing_platform_type = {2} And target_version = {3}", Global_Define.Server_Group_Active_TableName, serverID, platformType, version);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static Result_Define.eResult InsertServerVersion(ref TxnBlock TB, long serverID, int platformType, string version, string dbkey = GMData_Define.GlobalDBName)
        {
            string setQuery = string.Format("Insert Into {0} (server_group_id, billing_platform_type, target_version) Values ({1}, {2}, '{3}');", Global_Define.Server_Group_Active_TableName, serverID, platformType, version);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static Result_Define.eResult SetServerState(ref TxnBlock TB, long serverID, Global_Define.eServerStatus state = Global_Define.eServerStatus.Normal, string dbkey = GMData_Define.GlobalDBName)
        {
            string setQuery = string.Format("Update {0} Set server_group_status = {1} Where server_group_id = {2}", Global_Define.Server_Group_TableName, (int)state, serverID);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static Result_Define.eResult InsertSnailIP(ref TxnBlock TB, string ip, Global_Define.eServerStatus state = Global_Define.eServerStatus.Normal, string dbkey = GMData_Define.GlobalDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            string setQuery = string.Format("Insert Into {0} (ip_address) Values ('{1}');", Global_Define.Snail_IP_TableName, ip);
            retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            if (retError == Result_Define.eResult.SUCCESS)
            {//to do: global web call op=load_ip_table

            }
            return retError;
        }

        public static Result_Define.eResult DeleteSnailIP(ref TxnBlock TB, int idx, Global_Define.eServerStatus state = Global_Define.eServerStatus.Normal, string dbkey = GMData_Define.GlobalDBName)
        {
            string setQuery = string.Format("Delete From {0} Where idx = {1}", Global_Define.Snail_IP_TableName, idx);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static long GetMaxTargetVersion(ref TxnBlock TB, string dbkey = GMData_Define.GlobalDBName)
        {
            string setQuery = string.Format(@"SELECT MAX(target_version) as number FROM {0} WITH(NOLOCK)", Global_Define.Server_Group_Active_TableName);
            GM_Number retObj = GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.number;
        }

        
    }
}