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
        const string GMIniFileName = "GMToolServer.ini";

        public static void GetGMServerIni(ref TxnBlock TB, string savePath)
        {
            DBEndpoint setDB = new DBEndpoint();
            try
            {
                string AppPath = "";
                AppPath = savePath + @"\dbcon\";//원본 위치
                string sourceFile = Path.Combine(AppPath, GMIniFileName);
                TheSoul.DataManager.Tools.IniParser parser = new TheSoul.DataManager.Tools.IniParser(sourceFile);
                setDB.Host = parser.GetSetting("WebAdminDB", "host");
                setDB.Database = parser.GetSetting("WebAdminDB", "db");
                setDB.UserID = parser.GetSetting("WebAdminDB", "id");
                setDB.UserPW = parser.GetSetting("WebAdminDB", "pw");
            }
            catch (Exception ex)
            {
                setDB.Host = "localhost";
                setDB.Database = "webadmin";
                setDB.UserID = "sa";
                setDB.UserPW = "dpaTlemrpdlawm!@#";
                Console.WriteLine(ex.Message);
            }

            TB.IsoLevel = IsolationLevel.ReadCommitted;     // set transaction IsolationLevel (default ReadUncommited)
            TB.DBConn(setDB, "webadmin");        // make alias name for this connection
        }
        
        public static GM_menu GetMenuData(ref TxnBlock TB, int idx, string dbKey = GMData_Define.GmDBName)
        {
            string setQuery = string.Format(@"Select a.idx, a.gdiv, a.mdiv, a.viewidx, isnull(mn.menuname, a.menuname) menuname, a.linkurl, a.isusing
                                                From {0} a with(nolock) left outer join (Select * From {1} with(nolock) where [language] = N'{2}') mn  on a.idx = mn.menu_idx
                                                Where a.idx={3}", GMData_Define.GmMenu, GMData_Define.GmMenuName, GMDataManager.GetGmToolLanguage(), idx);
            GM_menu retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_menu>(ref TB, setQuery, dbKey);

            return retObj;
        }

        public static Result_Define.eResult SetMenu(ref TxnBlock TB, int idx, string name, int mdiv, int ordernum, string isusing, string dbkey = GMData_Define.GmDBName)
        {
            string setLanguage = GMDataManager.GetGmToolLanguage();
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            if (setLanguage == "kr")
            {
                string setQuery = string.Format("Update {0} Set menuname=N'{2}', mdiv={3}, viewidx={4}, isusing=N'{5}' Where idx = {1}", GMData_Define.GmMenu, idx, name, mdiv, ordernum, isusing);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            }
            else
            {
                string setQuery = string.Format("Update {0} Set mdiv={2}, viewidx={3}, isusing=N'{4}' Where idx = {1}", GMData_Define.GmMenu, idx, mdiv, ordernum, isusing);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError == Result_Define.eResult.SUCCESS)
                {
                    string setQuery2 = string.Format(@"MERGE {0} USING (select 'X' as DUAL) AS B
                                                        ON menu_idx = {1} And [language] = N'{2}'
                                                        WHEN MATCHED THEN
                                                           Update Set 
                                                                menuname = N'{3}'
                                                        WHEN NOT MATCHED THEN
                                                            INSERT (menu_idx, [language], menuname)
                                                            Values ({1}, N'{2}', N'{3}');"
                                                        , GMData_Define.GmMenuName, idx, setLanguage, name);
                            retError = TB.ExcuteSqlCommand(dbkey, setQuery2) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                }
            }
            return retError;
        }

        public static List<GM_menu> GetMenuList(ref TxnBlock TB, int mdiv, string dbKey = GMData_Define.GmDBName)
        {
            //string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Where mdiv={1} Order by viewidx asc ", GMData_Define.GmMenu, mdiv);
            string setQuery = string.Format(@"Select a.idx, a.gdiv, a.mdiv, a.viewidx, isnull(mn.menuname, a.menuname) menuname, a.linkurl, a.isusing
                                                From {0} a with(nolock) left outer join (Select * From {1} with(nolock) where [language] = N'{2}') mn  on a.idx = mn.menu_idx
                                                Where a.mdiv={3} Order by a.viewidx asc", GMData_Define.GmMenu, GMData_Define.GmMenuName, GMDataManager.GetGmToolLanguage(), mdiv);
            List<GM_menu> MenuList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GM_menu>(ref TB, setQuery, dbKey);

            return MenuList;
        }

        public static List<GM_menu> GetAllMenuList(ref TxnBlock TB, string dbKey = GMData_Define.GmDBName)
        {
            string setQuery = string.Format(@"Select a.idx, a.gdiv, a.mdiv, a.viewidx, isnull(mn.menuname, a.menuname) menuname, a.linkurl, a.isusing
                                                From {0} a with(nolock) left outer join (Select * From {1} with(nolock) where [language] = N'{2}') mn  on a.idx = mn.menu_idx
                                                Order by mdiv, viewidx asc "
                                                , GMData_Define.GmMenu, GMData_Define.GmMenuName, GMDataManager.GetGmToolLanguage());
            List<GM_menu> MenuList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GM_menu>(ref TB, setQuery, dbKey);

            return MenuList;
        }

        public static List<GM_menu_large> GetLargeMenu(ref TxnBlock TB, string dbKey = GMData_Define.GmDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Order by idx asc", GMData_Define.GmLargeMenu);
            List<GM_menu_large> LMenuList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GM_menu_large>(ref TB, setQuery, dbKey);

            return LMenuList;
        }

        public static Result_Define.eResult CreateGMUSer(ref TxnBlock TB, string id, string pw, string name, string email, string phone, string part, string rank, string reason = "", short level = 1, short lang = 1, string dbkey = GMData_Define.GmDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            string setQuery = string.Format(@"insert into {0} (userid, pwd, userlevel, name, email, phone, part, rank, language, reason, isusing) 
                                                            values (N'{1}',N'{2}','{3}',N'{4}',N'{5}',N'{6}',N'{7}',N'{8}',N'{9}',N'{10}','N')", GMData_Define.GmUserTable, id, pw, level, name, email, phone, part, rank, lang, reason );

            retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            if (retError == Result_Define.eResult.SUCCESS)
            {
                retError = SetUserAuthInit(ref TB, id);
            }
            return retError;
        }

        public static Result_Define.eResult UpdateGMUSer(ref TxnBlock TB, int idx, string auth, string serverAuth, string id, string pw, string name, string email, string phone, string part, string rank, string reason = "", short lang = 1, string isusing = "N", string dbkey = GMData_Define.GmDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            reason = reason.Replace("'", "''");
            name = name.Replace("'", "''");
            string strPw = "";
            if (!string.IsNullOrEmpty(pw))
                strPw = string.Format(", pwd=N'{0}'", pw);
            string setQuery = string.Format(@"Update {0} Set name=N'{1}',email=N'{2}'{3},phone='{4}',part=N'{5}',rank=N'{6}',language='{7}',reason=N'{8}',isusing=N'{9}', serverAuth=N'{11}' Where idx={10}", GMData_Define.GmUserTable, name, email, strPw, phone, part, rank, lang, reason, isusing, idx, serverAuth);

            retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            if (retError == Result_Define.eResult.SUCCESS)
            {
                retError = UpdateUserAuth(ref TB, id, auth);
            }
            return retError;
        }

        private static Result_Define.eResult UpdateUserAuth(ref TxnBlock TB, string id, string auth, string dbkey = GMData_Define.GmDBName)
        {
            
            string setQuery = string.Format(@"Update {0} Set auth = 0 Where userid = N'{1}'", GMData_Define.GmUserAuthTable,id);
            Result_Define.eResult retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            if (retError == Result_Define.eResult.SUCCESS)
            {
                string[] authList = System.Text.RegularExpressions.Regex.Split(auth, ",");
                foreach (string item in authList)
                {
                    setQuery = string.Format(@"MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON mdiv={1} And userid = N'{2}'
                                                WHEN MATCHED THEN
                                                    UPDATE SET 
                                                    auth=1
                                                WHEN NOT MATCHED THEN
                                                   INSERT (
                                                        [userid]
                                                       ,[mdiv]
                                                       ,[auth]
                                                    )
                                                   VALUES (
                                                        N'{2}'
                                                        ,{1}
                                                        ,1
                                                    );",
                                                    GMData_Define.GmUserAuthTable, item, id);
                    retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                    if (retError != Result_Define.eResult.SUCCESS)
                        break;
                }
            }
            return retError;
        }

        private static Result_Define.eResult SetUserAuthInit(ref TxnBlock TB, string id, string dbkey = GMData_Define.GmDBName)
        {
            string setQuery = string.Format(@"Insert Into {0} (userid, mdiv)
                                                Select N'{2}' as userid, mdiv From {1}", GMData_Define.GmUserAuthTable, GMData_Define.GmLargeMenu, id);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static GM_User GetGMUserData(ref TxnBlock TB, int UserIndex, string dbKey = GMData_Define.GmDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Where idx = {1}", GMData_Define.GmUserTable, UserIndex);
            GM_User retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_User>(ref TB, setQuery, dbKey);
            if (retObj == null)
                retObj = new GM_User();
            return retObj;
        }

        public static List<GM_User> GetGMUserList(ref TxnBlock TB, string dbKey = GMData_Define.GmDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK)", GMData_Define.GmUserTable);
            List<GM_User> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GM_User>(ref TB, setQuery, dbKey);
            if (retObj == null)
                retObj = new List<GM_User>();
            return retObj;
        }

        public static List<admin_language_code> GetGMToolLanguage(ref TxnBlock TB, string dbkey = GMData_Define.GmDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK)", GMData_Define.GMToolLanguageCodeTable);
            List<admin_language_code> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<admin_language_code>(ref TB, setQuery, dbkey);
            if (retObj == null)
                retObj = new List<admin_language_code>();
            return retObj;
        }

        public static admin_language_code GetGMToolLanguageData(ref TxnBlock TB, string code, string dbkey = GMData_Define.GmDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Where data_language = N'{1}'", GMData_Define.GMToolLanguageCodeTable, code);
            admin_language_code retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<admin_language_code>(ref TB, setQuery, dbkey);            
            return retObj == null? new admin_language_code() : retObj;
        }

        public static string GetGMToolWebLanguage(ref TxnBlock TB, string code, string dbkey = GMData_Define.GmDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Where data_language = N'{1}'", GMData_Define.GMToolLanguageCodeTable, code);
            admin_language_code retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<admin_language_code>(ref TB, setQuery, dbkey);
            return retObj == null ? "ko-kr" : retObj.web_language;
        }

        public static Result_Define.eResult SetUserAuth(ref TxnBlock TB, string id, short mdiv, int auth = 0, string dbkey = GMData_Define.GmDBName)
        {
            string setQuery = string.Format("Update {0} Set auth = {3} Where userid=N'{1}' And mdiv={2}", GMData_Define.GmUserAuthTable, id, mdiv, auth);

            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static GMResult_Define.eResult GetLogin(ref TxnBlock TB, string id, string pw, string lang, string dbkey = GMData_Define.GmDBName)
        {
            GMResult_Define.eResult retError = GMResult_Define.eResult.DB_ERROR;
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Where userid=N'{1}' And pwd=N'{2}' And isusing='Y'", GMData_Define.GmUserTable, id, pw);
            GM_User gm = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_User>(ref TB, setQuery, dbkey);
            if (gm != null)
            {
                List<GM_UserAuth> authInfo = GetUserAuth(ref TB, id);
                if (authInfo.Count > 0 || gm.userid == GMData_Define.SuperAdminID)
                {
                    
                    HttpContext.Current.Response.Cookies["mseedadmin"]["userid"] = gm.userid;
                    HttpContext.Current.Response.Cookies["mseedadmin"]["name"] = gm.name;
                    HttpContext.Current.Response.Cookies["mseedadmin"]["language"] = lang;
                    HttpContext.Current.Response.Cookies["mseedadmin"]["languageCode"] = GetGMToolWebLanguage(ref TB,lang);
                    if (gm.serverAuth != null)
                        HttpContext.Current.Response.Cookies["mseedadmin"]["UserServer"] = gm.serverAuth.ToString();
                    HttpContext.Current.Response.Cookies["mseedadmin"].Expires = DateTime.Now.AddHours(3);
                    string langcode = GetGMToolLanguageData(ref TB, lang).web_language;
                    if (langcode !=null )
                        SetLanguage(GetGMToolLanguageData(ref TB, lang).web_language);
                }
                retError = GMResult_Define.eResult.SUCCESS;
            }
            return retError;

        }

        public static void SetUserCookiesLanguage(string data_lang, string web_lang)
        {
            //HttpContext.Current.Response.Cookies["mseedadmin"][key] = value;
            HttpCookie cookie = HttpContext.Current.Request.Cookies["mseedadmin"];
            cookie.Values.Set("language", data_lang);
            cookie.Values.Set("languageCode", web_lang);
            HttpContext.Current.Response.Cookies.Set(cookie);
            cookie.Expires = DateTime.Now.AddHours(2);
        }


        public static void SetLanguage(string langType = "")
        {
            CultureInfo ci = new CultureInfo(langType);
            Resources.languageResource.Culture = ci;
        }

        public static string GetGmToolLanguage()
        {
            string setLanguage = GMDataManager.GetUserCookies("language");
            if (string.IsNullOrEmpty(setLanguage))
                setLanguage = "kr";
            return setLanguage;
        }

        public static string GetGmToolWebLanguageCode()
        {
            string setLanguage = GMDataManager.GetUserCookies("languageCode");
            if (string.IsNullOrEmpty(setLanguage))
                setLanguage = "ko-kr";
            return setLanguage;
        }

        public static string GetUserCookies(string key = "userid")
        {
            if (HttpContext.Current.Request.Cookies.Count == 0)
            {
#if DEBUG
                HttpContext.Current.Response.Cookies["mseedadmin"]["userid"] = "superadmin";
                HttpContext.Current.Response.Cookies["mseedadmin"]["name"] = "superadmin";
                HttpContext.Current.Response.Cookies["mseedadmin"]["language"] = "kr";
                HttpContext.Current.Response.Cookies["mseedadmin"]["languageCode"] = "ko-kr";
                HttpContext.Current.Response.Cookies["mseedadmin"]["UserServer"] = "";

                HttpCookie cookie = HttpContext.Current.Request.Cookies["mseedadmin"];
                string retObj = cookie[key];
                cookie.Expires = DateTime.Now.AddHours(2);
                return retObj;
#else
                return "";
#endif
            }
            else
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["mseedadmin"];
                string retObj = cookie[key];
                cookie.Expires = DateTime.Now.AddHours(2);
                return retObj;
            }
        }

        public static List<GM_UserAuth> GetUserAuth(ref TxnBlock TB, string id, string dbkey = GMData_Define.GmDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Where userid='{1}'", GMData_Define.GmUserAuthTable, id);
            List<GM_UserAuth> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GM_UserAuth>(ref TB, setQuery, dbkey);
            if (retObj.Count == 0)
                retObj = new List<GM_UserAuth>();
            return retObj;
        }

        public static List<ListItem> GetLevelListItem(ref TxnBlock TB, bool addAll = true, string dbkey = GMData_Define.ShardingDBName)
        {
            List<ListItem> itemList = new List<ListItem>();
            if (addAll)
            {
                ListItem setitem = new ListItem("Select", "0");
                itemList.Add(setitem);
                string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) order by level asc ", CharacterManager.CharacterExpTableName);
                List<System_Character_EXP> checkList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<System_Character_EXP>(ref TB, setQuery, dbkey);
                foreach (System_Character_EXP data in checkList)
                {
                    var item = new ListItem(data.Level.ToString());
                    itemList.Add(item);
                }
            }
            return itemList;
        }

        public static List<GM_UserAccount> GetUserList(ref TxnBlock TB, string username = "", string uid = "", int sLevel = 0, int eLevel = 0, string sdate = "", string edate = "", string dbkey = GMData_Define.ShardingDBName)
        {
            string condition1 = "";
            string condition2 = "";
            string condition3 = "";
            string condition4 = "";
            if(!string.IsNullOrEmpty(username) && string.IsNullOrEmpty(uid))
                condition1 = string.Format(" And UserName like N'%{0}%'", username);
            if (!string.IsNullOrEmpty(uid))
            {
                long AID = GMDataManager.GetSearchAID_BYSnailPlatformID(ref TB, uid).AID;
                condition2 = string.Format(" And AID = {0}", AID);
            }
            if(sLevel > 0 && eLevel > 0)
                condition3 = string.Format(" And AID In (Select AID From {0} WITH(NOLOCK) Where level >= {1} and level <= {2})", Character_Define.CharacterTableName, sLevel, eLevel);
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                condition4 = string.Format(" And (CONVERT(varchar(10), CreationDate,121) >= '{0}' and CONVERT(varchar(10), CreationDate,121) <= '{1}')", sdate, edate);

            string setQuery = string.Format("Select * , (select VIPLevel from dbo.User_VIP WITH(NOLOCK) where AID = a.AID) as VIPLevel From {0} a WITH(NOLOCK) Where 1=1{1}{2}{3}{4} Order by a.CreationDate Desc", Account_Define.AccountDBTableName, condition1, condition2, condition3, condition4);
            List<GM_UserAccount> list = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GM_UserAccount>(ref TB, setQuery, dbkey);
            return list;
        }

        public static long GetSearchAID_BYUserName(ref TxnBlock TB, string username, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Where Username = N'{1}'", Account_Define.AccountDBTableName, username);
            Account retObj = GenericFetch.FetchFromDB<Account>(ref TB, setQuery, dbkey);
            return retObj== null ? 0 : retObj.AID;
        }

        public static List<Request_Log> GetEorrorLogList(ref TxnBlock TB, long AID, int errorCode, string op, string sdate, string edate, string dbkey = GMData_Define.LogDBName)
        {
            string condition = "";
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                condition = string.Format(" And (CONVERT(varchar(10), regdate,121) >= '{0}' and CONVERT(varchar(10), regdate,121) <= '{1}')", sdate, edate);

            string tableQuery = "";
            List<GM_String> getTableList = GetLogTableName(ref TB, DataManager_Define.LogTableName, sdate, edate);
            getTableList.ForEach(item =>
            {
                if (!string.IsNullOrEmpty(tableQuery))
                    tableQuery += "Union All";
                tableQuery += string.Format(@"
                                            Select * From {0} WITH(NOLOCK) Where AID = {1} And ErrorCode = {2} And Operation = N'{3}' {4}"
                                            , item.name, AID, errorCode, op, condition);
            });
            string setQuery = string.Format("Select * From ({0}) as logTable", tableQuery);
            List<Request_Log> retObj = GenericFetch.FetchFromDB_MultipleRow<Request_Log>(ref TB, setQuery, dbkey);
            if(retObj.Count == 0)
                retObj = new List<Request_Log>();
            return retObj;
        }

        public static List<Request_Log> GetUserAllLogList_Excel(ref TxnBlock TB, long AID, int errorCode, string op, string sdate, string edate, string dbkey = GMData_Define.LogDBName)
        {
            string condition1 = "";
            string condition2 = "";
            string condition3 = "";
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                condition1 = string.Format(" And (CONVERT(varchar(10), regdate,121) >= '{0}' and CONVERT(varchar(10), regdate,121) <= '{1}')", sdate, edate);
            if (op.IndexOf(',') > 0)
                condition2 = string.Format(" And Operation In ({0})", op);
            else
                condition2 = string.Format(" And Operation=N'{0}'", op);
            if (errorCode >= 0)
                condition3 = string.Format(" And ErrorCode={0}", errorCode);

            string tableQuery = "";
            List<GM_String> getTableList = GetLogTableName(ref TB, DataManager_Define.LogTableName, sdate, edate);
            getTableList.ForEach(item =>
            {
                if (!string.IsNullOrEmpty(tableQuery))
                    tableQuery += "Union All";
                tableQuery += string.Format(@"
                                            Select * From {0} WITH(NOLOCK) Where AID = {1} {2} {3} {4}"
                                            , item.name, AID, condition1, condition2, condition3);
            });

            string setQuery = string.Format("Select * From ({0}) as logTable Order By regdate Desc", tableQuery);
            List<Request_Log> retObj = GenericFetch.FetchFromDB_MultipleRow<Request_Log>(ref TB, setQuery, dbkey);
            if (retObj.Count == 0)
                retObj = new List<Request_Log>();
            return retObj;
        }

        public static Request_Log GetUserLogData(ref TxnBlock TB, long index, string tableName, string dbkey = GMData_Define.LogDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Where log_idx = {1}", tableName, index);
            Request_Log retObj = GenericFetch.FetchFromDB<Request_Log>(ref TB, setQuery, dbkey);
            return retObj == null ? new Request_Log() : retObj;
        }

        public static List<Request_Log> GetUserAllLogList(ref TxnBlock TB, int page, long AID, int errorCode, string op, string sdate, string edate, string dbkey = GMData_Define.LogDBName)
        {
            string condition1 = "";
            string condition2 = "";
            string condition3 = "";
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                condition1 = string.Format(" And (CONVERT(varchar(10), regdate,121) >= '{0}' and CONVERT(varchar(10), regdate,121) <= '{1}')", sdate, edate);
            if (op != "")
            {
                if (op.IndexOf(',') > 0)
                    condition2 = string.Format(" And Operation In ({0})", op);
                else
                    condition2 = string.Format(" And Operation=N{0}", op);
            }
            if (errorCode >= 0)
                condition3 = string.Format(" And ErrorCode={0}", errorCode);

            string tableQuery = "";
            List<GM_String> getTableList = GetLogTableName(ref TB, DataManager_Define.LogTableName, sdate, edate);
            getTableList.ForEach(item =>
            {
                if (!string.IsNullOrEmpty(tableQuery))
                    tableQuery += "Union All";
                tableQuery += string.Format(@"
                                            Select *, '{0}' as tableName From {0} WITH(NOLOCK) Where AID = {1} {2} {3} {4}"
                                            , item.name, AID, condition1, condition2, condition3);
            });

            string setQuery = string.Format(@"SELECT TOP({1}) resultTable.* FROM (
                                                    Select TOP {2} ROW_NUMBER() over (order by regdate Desc) as rownumber, * From ({0}) as logTable ) as resultTable
                                                WHERE rownumber > {3}"
                                                , tableQuery, GMData_Define.pageSize, (GMData_Define.pageSize * page), (page - 1) * GMData_Define.pageSize);
            List<Request_Log> retObj = GenericFetch.FetchFromDB_MultipleRow<Request_Log>(ref TB, setQuery, dbkey);
            if (retObj.Count == 0)
                retObj = new List<Request_Log>();
            return retObj;
        }
        
        public static long GetUserAllLogListCount(ref TxnBlock TB, long AID, int errorCode, string op, string sdate, string edate, string dbkey = GMData_Define.LogDBName)
        {
            string condition1 = "";
            string condition2 = "";
            string condition3 = "";
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                condition1 = string.Format(" And (CONVERT(varchar(10), regdate,121) >= '{0}' and CONVERT(varchar(10), regdate,121) <= '{1}')", sdate, edate);
            if (op != "")
            {
                if (op.IndexOf(',') > 0)
                    condition2 = string.Format(" And Operation In ({0})", op);
                else
                    condition2 = string.Format(" And Operation=N{0}", op);
            }
            if (errorCode>=0)
                condition3 = string.Format(" And ErrorCode={0}", errorCode);
            string tableQuery = "";
            List<GM_String> getTableList = GetLogTableName(ref TB, DataManager_Define.LogTableName, sdate, edate);
            getTableList.ForEach(item => {
                if (!string.IsNullOrEmpty(tableQuery))
                    tableQuery += "Union All";
                tableQuery += string.Format(@"
                                            Select * From {0} WITH(NOLOCK) Where AID = {1} {2} {3} {4}"
                                            , item.name, AID, condition1, condition2, condition3);
            });

            string setQuery = string.Format(@"Select count(*) as number From ({0}) as logTable", tableQuery);
            GM_Number retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.number;
        }

        public static long GetUserCdkeyLogCount(ref TxnBlock TB, string sdate, string edate, string dbkey = GMData_Define.LogDBName)
        {
            string condition1 = "";
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                condition1 = string.Format(" And (CONVERT(varchar(10), regdate,121) >= '{0}' and CONVERT(varchar(10), regdate,121) <= '{1}')", sdate, edate);

            string tableQuery = "";
            List<GM_String> getTableList = GetLogTableName(ref TB, DataManager_Define.LogTableName, sdate, edate);
            getTableList.ForEach(item =>
            {
                if (!string.IsNullOrEmpty(tableQuery))
                    tableQuery += "Union All";
                tableQuery += string.Format(@"
                                            Select *, '{0}' as tableName From {0} WITH(NOLOCK) Where Operation = N'reg_snail_cdkey' {1}"
                                            , item.name, condition1);
            });

            string setQuery = string.Format(@"SELECT count(*) as number From {0}) as logTable", tableQuery);
            GM_Number retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.number;
        }

        public static List<Cdkey_Log> GetUserCdkeyLogList(ref TxnBlock TB, int page,  string sdate, string edate, string dbkey = GMData_Define.LogDBName)
        {
            string condition1 = "";
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                condition1 = string.Format(" And (CONVERT(varchar(10), regdate,121) >= '{0}' and CONVERT(varchar(10), regdate,121) <= '{1}')", sdate, edate);

            string tableQuery = "";
            List<GM_String> getTableList = GetLogTableName(ref TB, DataManager_Define.LogTableName, sdate, edate);
            getTableList.ForEach(item =>
            {
                if (!string.IsNullOrEmpty(tableQuery))
                    tableQuery += "Union All";
                tableQuery += string.Format(@"
                                            Select *, '{0}' as tableName From {0} WITH(NOLOCK) Where Operation = N'reg_snail_cdkey' {1}"
                                            , item.name, condition1);
            });

            string setQuery = string.Format(@"SELECT TOP({1}) resultTable.* FROM (
                                                    Select TOP {2} ROW_NUMBER() over (order by regdate Desc) as rownumber, * From ({0}) as logTable ) as resultTable
                                                WHERE rownumber > {3}"
                                                , tableQuery, GMData_Define.pageSize, (GMData_Define.pageSize * page), (page - 1) * GMData_Define.pageSize);
            List<Request_Log> logObj = GenericFetch.FetchFromDB_MultipleRow<Request_Log>(ref TB, setQuery, dbkey);
            List<Cdkey_Log> retObj = new List<Cdkey_Log>();
            if (logObj.Count == 0)
                logObj = new List<Request_Log>();
            else
            {
                foreach(Request_Log item in logObj)
                {
                    Dictionary<string, string> jsonData = mJsonSerializer.JsonToDictionary(item.RequestParams);
                    string cdkey = jsonData.ContainsKey("couponkey") ? jsonData["couponkey"] : "notcouponkey";
                    string sendQuery = string.Format("Select * From {0} WITH(NOLOCK) Where Operation = N'send_mail' And RequestParams like '%{1}%'", item.tableName, cdkey);
                    Request_Log sendObj = GenericFetch.FetchFromDB<Request_Log>(ref TB, sendQuery, dbkey);
                    if (sendObj == null)
                        sendObj = new Request_Log();
                    if (sendObj.AID == 0)
                    {
                        Dictionary<string, string> sendJsonData = mJsonSerializer.JsonToDictionary(sendObj.RequestParams);
                        sendObj.AID = sendJsonData.ContainsKey("receiver") ? System.Convert.ToInt64(sendJsonData["receiver"]) : 0;
                    }
                    GM_Global_UserSimple userInfo = GetUserGloblaSimpleInfo(ref TB, sendObj.AID);
                    User_Coupon_Key userCdKey = AccountManager.GetUser_Coupon_Key(ref TB, cdkey);
                    Cdkey_Log logData = new Cdkey_Log();
                    //log data
                    logData.cdkey = cdkey;
                    logData.AID = sendObj.AID;
                    logData.CID = sendObj.CID;
                    logData.regdate = sendObj.regdate;
                    logData.mail_log_idx = sendObj.log_idx;
                    //user data
                    logData.platform_idx = userInfo.platform_idx;
                    logData.platform_user_id = userInfo.platform_user_id;
                    logData.userName = AccountManager.GetSimpleAccountInfo(ref TB, sendObj.AID).username;
                    //coupun data
                    logData.mailseq = userCdKey.mailseq_json;
                    logData.stateflag = userCdKey.stateflag;
                    //etc
                    logData.tableName = item.tableName;
                    retObj.Add(logData);
                }
            }

            return retObj;
        }

        public static List<Cdkey_Log> GetUserCdkeyLogList_Excel(ref TxnBlock TB, string sdate, string edate, string dbkey = GMData_Define.LogDBName)
        {
            string condition1 = "";
            if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                condition1 = string.Format(" And (CONVERT(varchar(10), regdate,121) >= '{0}' and CONVERT(varchar(10), regdate,121) <= '{1}')", sdate, edate);

            string tableQuery = "";
            List<GM_String> getTableList = GetLogTableName(ref TB, DataManager_Define.LogTableName, sdate, edate);
            getTableList.ForEach(item =>
            {
                if (!string.IsNullOrEmpty(tableQuery))
                    tableQuery += "Union All";
                tableQuery += string.Format(@"
                                            Select *, '{0}' as tableName From {0} WITH(NOLOCK) Where Operation = N'reg_snail_cdkey' {1}"
                                            , item.name, condition1);
            });

            string setQuery = string.Format(@"SELECT * From ({0}) as logTable order by regdate Desc", tableQuery);
            List<Request_Log> logObj = GenericFetch.FetchFromDB_MultipleRow<Request_Log>(ref TB, setQuery, dbkey);
            List<Cdkey_Log> retObj = new List<Cdkey_Log>();
            if (logObj.Count == 0)
                logObj = new List<Request_Log>();
            else
            {
                foreach (Request_Log item in logObj)
                {
                    Dictionary<string, string> jsonData = mJsonSerializer.JsonToDictionary(item.RequestParams);
                    string cdkey = jsonData.ContainsKey("couponkey") ? jsonData["couponkey"] : "notcouponkey";
                    string sendQuery = string.Format("Select * From {0} WITH(NOLOCK) Where Operation = N'send_mail' And RequestParams like '%{1}%'", item.tableName, cdkey);
                    Request_Log sendObj = GenericFetch.FetchFromDB<Request_Log>(ref TB, sendQuery, dbkey);
                    if (sendObj == null)
                        sendObj = new Request_Log();
                    if (sendObj.AID == 0)
                    {
                        Dictionary<string, string> sendJsonData = mJsonSerializer.JsonToDictionary(sendObj.RequestParams);
                        sendObj.AID = sendJsonData.ContainsKey("receiver") ? System.Convert.ToInt64(sendJsonData["receiver"]) : 0;
                    }
                    GM_Global_UserSimple userInfo = GetUserGloblaSimpleInfo(ref TB, sendObj.AID);
                    User_Coupon_Key userCdKey = AccountManager.GetUser_Coupon_Key(ref TB, cdkey);
                    Cdkey_Log logData = new Cdkey_Log();
                    //log data
                    logData.cdkey = cdkey;
                    logData.AID = sendObj.AID;
                    logData.CID = sendObj.CID;
                    logData.regdate = sendObj.regdate;
                    logData.mail_log_idx = sendObj.log_idx;
                    //user data
                    logData.platform_idx = userInfo.platform_idx;
                    logData.platform_user_id = userInfo.platform_user_id;
                    logData.userName = AccountManager.GetSimpleAccountInfo(ref TB, sendObj.AID).username;
                    //coupun data
                    logData.mailseq = userCdKey.mailseq_json;
                    logData.stateflag = userCdKey.stateflag;
                    //etc
                    logData.tableName = item.tableName;
                    retObj.Add(logData);
                }
            }

            return retObj;
        }

        public static Result_Define.eResult InsertGMControlLog(ref TxnBlock TB, GMResult_Define.TargetType targetType, long targetIdx, string targetName, GMResult_Define.ControlType controlType, string chageData, string serverID, string dbkey = GMData_Define.GmDBName)
        {
            string gmId = "";
            string gmName = "";
            if (HttpContext.Current.Request.Cookies.Count == 0)
            {
                gmId = "test2";
                gmName = "test";
            }
            else
            {
                gmId = HttpContext.Current.Request.Cookies["mseedadmin"]["userid"];
                gmName = HttpContext.Current.Request.Cookies["mseedadmin"]["name"];
            }
            chageData = chageData.Replace("''", "\"");
            chageData = chageData.Replace("'", "''");
            string setQuery = string.Format(@"Insert into {0} (targetType, targetIndex, targetName, controlType, server_id, etcData, adminID, adminName)
                                              Values (N'{1}',N'{2}',N'{3}',N'{4}',N'{5}',N'{6}',N'{7}',N'{8}')", GMData_Define.GmControlLog, (int)targetType, targetIdx, targetName, (int)controlType, serverID, chageData, gmId, gmName);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static Result_Define.eResult InsertGMEventLog(ref TxnBlock TB, string beforeData, string AfterData, string dbkey = GMData_Define.GmDBName)
        {
            string gmId = "";
            if (HttpContext.Current.Request.Cookies.Count == 0)
                gmId = "test2";
            else
                gmId = HttpContext.Current.Request.Cookies["mseedadmin"]["userid"];

            beforeData = beforeData.Replace("'", "''");
            AfterData = AfterData.Replace("'", "''");
            string setQuery = string.Format(@"Insert into {0} (gmid, beforeEvent, afterEvent)
                                              Values (N'{1}',N'{2}',N'{3}')", GMData_Define.GMEventLogTable, gmId, beforeData, AfterData);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static Result_Define.eResult InsertItemChargeLog(ref TxnBlock TB, string userName, string title, string body, List<Set_Mail_Item> itemList, string dbkey = GMData_Define.GmDBName)
        {
            Set_Mail_Item[] setItem = new Set_Mail_Item[Mail_Define.Mail_MaxItem] {
                new Set_Mail_Item(),
                new Set_Mail_Item(),
                new Set_Mail_Item(),
                new Set_Mail_Item(),
                new Set_Mail_Item()
            };

            int setPos = 0;

            itemList.ForEach(mailItem =>
            {
                setItem[setPos].itemid = mailItem.itemid;
                setItem[setPos].itemea = mailItem.itemea;
                setItem[setPos].grade = mailItem.grade;
                setItem[setPos].level = mailItem.level;
                setPos++;
            }
            );
            string setQuery = string.Format(@"Insert into {0} (userNames, title, bodytext,
                                                                item_id_1, itemea_1, item_grade_1, item_level_1,
                                                                item_id_2, itemea_2, item_grade_2, item_level_2,
                                                                item_id_3, itemea_3, item_grade_3, item_level_3,
                                                                item_id_4, itemea_4, item_grade_4, item_level_4,
                                                                item_id_5, itemea_5, item_grade_5, item_level_5,
                                                                regdate
                                                              ) VALUES (
                                                                N'{1}', N'{2}', N'{3}',
                                                                '{4}', '{5}', '{6}', '{7}',
                                                                '{8}','{9}','{10}','{11}',
                                                                '{12}','{13}','{14}','{15}',
                                                                '{16}','{17}','{18}','{19}',
                                                                '{20}','{21}','{22}','{23}',
                                                                GETDATE())", GMData_Define.GMItemChargeLogTable, userName, title, body
                                                        , setItem[0].itemid, setItem[0].itemea, setItem[0].grade, setItem[0].level
                                                        , setItem[1].itemid, setItem[1].itemea, setItem[1].grade, setItem[1].level
                                                        , setItem[2].itemid, setItem[2].itemea, setItem[2].grade, setItem[2].level
                                                        , setItem[3].itemid, setItem[3].itemea, setItem[3].grade, setItem[3].level
                                                        , setItem[4].itemid, setItem[4].itemea, setItem[4].grade, setItem[4].level);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static string GetServerCheckList(ref TxnBlock TB, long serverID = 1, bool allCheck = true, string userAuth = "")
        {
            List<server_group_config> serverGourpList = GlobalManager.GetServerGroupList(ref TB);
            //serverGourpList.ForEach(item => {
            //    item.server_info = GlobalManager.GetServerListByGroup(ref TB, System.Convert.ToInt32(item.server_group_id));
            //});
            //serverGourpList.RemoveAll(item => item.server_group_id == 0 || item.server_info.Count == 0);
            serverGourpList.RemoveAll(item => item.server_group_id == 0);
            StringBuilder serverlist = new StringBuilder();
            if(allCheck)
                serverlist.Append("<input type=\"checkbox\" name=\"All_Server\" value=\"0\" onclick=\"serverChecked();\" /> All Server <br />");
            int serverCount = 1;
            string userUseServer = serverID == 0 ? userAuth : GetUserCookies("UserServer");
            List<string> userServer = System.Text.RegularExpressions.Regex.Split(userUseServer, ",").ToList();
            List<server_config> serverList = GlobalManager.GetServerList(ref TB);
            foreach (server_group_config server in serverGourpList)
            {
                server.server_info = new List<server_config>();
                serverList.ForEach(setServerinfo =>
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
                if (!string.IsNullOrEmpty(userServer.Find(item => item == server.server_group_id.ToString())) || GetUserCookies() == "superadmin" || serverID == 0)
                    userCheck = true;
                if (server.server_info.Count > 0 && userCheck)
                {
                    if ((serverCount % 5) == 0)
                        serverlist.Append("<br />");
                    if (server.server_group_id == serverID)
                    {
                        serverlist.Append("<input type=\"checkbox\" name=\"serverid\" value=\"" + server.server_group_id + "\" checked runat=\"server\" /> " + server.server_group_name + " ");
                    }
                    else
                    {
                        if (serverID == 0 && !string.IsNullOrEmpty(userServer.Find(item => item == server.server_group_id.ToString())))
                        {
                            serverlist.Append("<input type=\"checkbox\" name=\"serverid\" value=\"" + server.server_group_id + "\" checked runat=\"server\" /> " + server.server_group_name + " ");
                        }
                        else
                            serverlist.Append("<input type=\"checkbox\" name=\"serverid\" value=\"" + server.server_group_id + "\" runat=\"server\" /> " + server.server_group_name + " ");
                    }
                    serverCount += 1;
                }
            }
            return serverlist.ToString();
        }
    }
}