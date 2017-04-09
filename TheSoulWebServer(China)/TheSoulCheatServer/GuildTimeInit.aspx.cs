using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using mSeed.mDBTxnBlock;
using mSeed.RedisManager;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Global;
using TheSoulWebServer.Tools;
using TheSoulCheatServer.lib;
using System.Net.Json;

namespace TheSoulCheatServer
{
    public partial class GuildTimeInit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JsonObjectCollection res = new JsonObjectCollection();
            if ((Request["guildname"] == null || Request["guildname"] == "") && (Request["username"] == null || Request["username"] == ""))
            {
                res.Add(new JsonNumericValue("resultcode", 1));
                res.Add(new JsonStringValue("message", "Post 값 전달 실패"));
                string json_text = res.ToString();
                Response.Write(json_text);
            }
            else
            {
                string guildname = Request["guildname"];
                string username = Request["username"];

                WebQueryParam queryFetcher = new WebQueryParam();
                string retJson = "";
                queryFetcher.SetDebugMode = true;
                int usercnt = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("usercnt", "0"));
                int usercnt2 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("usercnt2", "0"));
                string delTime = queryFetcher.QueryParam_Fetch("delTime", "");
                string skillTime = queryFetcher.QueryParam_Fetch("skillTime", "");
                string guildTime = queryFetcher.QueryParam_Fetch("guildTime", "");
                string DonationDate = queryFetcher.QueryParam_Fetch("DonationDate", "");
                string joinDate = queryFetcher.QueryParam_Fetch("joinDate", "");
                string EntrustTime = queryFetcher.QueryParam_Fetch("EntrustTime", "");
                int level = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("level", "0"));

                TxnBlock tb = new TxnBlock();
                {
                    long GuildID = 0;
                    try
                    {    
                        queryFetcher.TxnBlockInit(ref tb, ref GuildID);
                        queryFetcher.GlobalDBOpen(ref tb);
                        Result_Define.eResult retErr = Result_Define.eResult.POST_DATA_ERROR;
                        if (!string.IsNullOrEmpty(guildname))
                        {
                            GuildID = GuildManager.GetSearchGuildInfo_ByName(ref tb, guildname).GuildID;
                            
                            if (GuildID > 0)
                            {
                                if (level > 0)
                                {
                                    long GuildLVUpExp = 0;
                                    for (int i = 1; i <= (level + 1); i++)
                                    {
                                        GuildLVUpExp = GuildLVUpExp + GuildManager.GetSystemGuildData(ref tb, i).NeedExp;
                                    }
                                    if (GuildLVUpExp > 0)
                                        GuildLVUpExp = GuildLVUpExp - 200;
                                    string setQuery = string.Format("Update {0} Set GuildLevel = {2}, GuildExp = {3}, GuildWithdrawExp = 0 Where GuildID={1}", GuildManager.GuildCreationDBTableName, GuildID, level, GuildLVUpExp);
                                    retErr = tb.ExcuteSqlCommand(GuildManager.GuildcommonDBName, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                                    if (retErr == Result_Define.eResult.SUCCESS)
                                    {
                                        GuildManager.GetGuilData(ref tb, GuildID, true);
                                    }
                                }
                                if (!string.IsNullOrEmpty(skillTime))
                                {
                                    string setQuery = string.Format(@"UPDATE A SET A.YesterdayAttendCheck = B.cntUser FROM {0} A Join 
                                                                (Select COUNT(*) as cntUser, GuildID  From dbo.{1} Where GuildID = {2} Group by GuildID) as B
                                                                ON A.GuildID = B.GuildID", GuildManager.GuildCreationDBTableName, GuildManager.GuildJoinerDBTableName, GuildID);
                                    retErr = tb.ExcuteSqlCommand(GuildManager.GuildcommonDBName, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                                    if (retErr == Result_Define.eResult.SUCCESS)
                                    {
                                        string setQuery2 = string.Format("Update {0} Set YesterdayAttendDate = DATEADD(DD,-1,TodayAttendDate), TodayAttendDate = NULL WHERE GuildID={1}", GuildManager.GuildJoinerDBTableName, GuildID);
                                        retErr = tb.ExcuteSqlCommand(GuildManager.GuildcommonDBName, setQuery2) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;    
                                    }

                                }
                                if (!string.IsNullOrEmpty(guildTime))
                                {
                                    string setQuery = string.Format("Update {0} Set GuildIntroduceModifyDate = NULL, GuildNoticeModifyDate = NULL WHERE GuildID={1}", GuildManager.GuildCreationDBTableName, GuildID);
                                    retErr = tb.ExcuteSqlCommand(GuildManager.GuildcommonDBName, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                                    if (retErr == Result_Define.eResult.SUCCESS)
                                        GuildManager.GetGuilData(ref tb, GuildID, true);
                                }

                                if (!string.IsNullOrEmpty(EntrustTime))
                                {
                                    string setQuery = string.Format("Update {0} Set EntrustAskDate = NULL WHERE GuildID={1}", GuildManager.GuildJoinerDBTableName, GuildID);
                                    retErr = tb.ExcuteSqlCommand(GuildManager.GuildcommonDBName, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                                    if (retErr == Result_Define.eResult.SUCCESS)
                                        GuildManager.GetGuilData(ref tb, GuildID, true);
                                }

                                if (usercnt > 0)
                                {
                                    int AttendCount = 0;
                                    List<GuildJoiner> joinerList = cheatData.GetGuildJoinerAllList(ref tb, GuildID, true);
                                    foreach (GuildJoiner joiner in joinerList)
                                    {
                                        if (AttendCount < usercnt)
                                        {
                                            DateTime dt1 = DateTime.Now;
                                            if (joiner.TodayAttendDate == null)
                                            {
                                                TimeSpan oneDay = new TimeSpan(1, 0, 0, 0);
                                                DateTime tomorrow = dt1 + oneDay;

                                                joiner.TodayAttendDate = tomorrow;
                                            }
                                            if (joiner.JoinerState == "S" && joiner.TodayAttendDate > dt1)
                                            {
                                                string setQuery = string.Format("UPDATE {0} SET TodayAttendDate=GETDATE() WHERE GuildID={1} AND JoinerState='S' AND JoinerAID={2}", GuildManager.GuildJoinerDBTableName, GuildID, joiner.JoinerAID);
                                                retErr = tb.ExcuteSqlCommand(GuildManager.GuildcommonDBName, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                                                AttendCount = AttendCount + 1;
                                            }
                                        }
                                    }
                                }
                                if (usercnt2 > 0)
                                {
                                    int AttendCount = 0;
                                    List<GuildJoiner> joinerList = cheatData.GetGuildJoinerAllList(ref tb, GuildID, true);
                                    foreach (GuildJoiner joiner in joinerList)
                                    {
                                        if (AttendCount < usercnt2)
                                        {
                                            if (joiner.JoinerState == "S")
                                            {
                                                string setQuery = string.Format("UPDATE {0} SET TodayAttendDate = DateAdd(D, -1, GETDATE()) WHERE GuildID={1} AND JoinerState='S' AND JoinerAID={2}", GuildManager.GuildJoinerDBTableName, GuildID, joiner.JoinerAID);

                                                retErr = tb.ExcuteSqlCommand(GuildManager.GuildcommonDBName, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                                                AttendCount = AttendCount + 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(username))
                        {
                            long AID = 0;
                            User_account user_server = cheatData.GetUserAID(ref tb, username);
                            AID = user_server.AID;
                            if (AID > 0)
                            {
                                if (!string.IsNullOrEmpty(joinDate))
                                {
                                    string setQuery = string.Format("UPDATE {0} SET JoinerDeleteDate=NULL, JoinerbanishDate=NULL WHERE JoinerAID={1}", GuildManager.GuildJoinerDBTableName, AID);
                                    retErr = tb.ExcuteSqlCommand(Account_Define.AccountCommonDB,setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                                }
                                if (!string.IsNullOrEmpty(DonationDate))
                                {
                                    string setQuery = string.Format("UPDATE {0} SET JoinerDonationDate=NULL, TodayDonationExp = 0 WHERE JoinerAID={1}", GuildManager.GuildJoinerDBTableName, AID);
                                    retErr = tb.ExcuteSqlCommand(Account_Define.AccountCommonDB,setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                                    if (retErr == Result_Define.eResult.SUCCESS)
                                    {
                                        GuildManager.GetJoinerData(ref tb, AID, true);
                                    }
                                }
                            }
                        }

                       
                        retJson = queryFetcher.Render("", retErr);

                    }
                    catch (Exception errorEx)
                    {
                        retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(errorEx.Message), Result_Define.eResult.System_Exception);
                    }
                    tb.EndTransaction(queryFetcher.Render_errorFlag);
                    tb.Dispose();
                }
            }
        }
    }
}