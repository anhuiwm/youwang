using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Tools;
using TheSoul.DataManager.Global;
using TheSoulWebServer.Tools;
using TheSoulGMTool.DBClass;

namespace TheSoulGMTool.kr
{
    public partial class guildView : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            queryFetcher.bDBLog = true;
            string retJson = "";
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                    Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
                    long GID = queryFetcher.QueryParam_FetchLong("gid");
                    long AID = queryFetcher.QueryParam_FetchLong(idx.UniqueID);
                    tb.Elog = queryFetcher.DBLog;

                    if (GID > 0)
                    {

                        if (!Page.IsPostBack)
                        {
                            Guild_GuildCreation guildInfo = GuildManager.GetGuilData(ref tb, GID);
                            guildName.Text = guildInfo.GuildName;
                            List<GuildJoiner> joinerInfoList = GuildManager.GetGuildJoinerInfoList(ref tb, GID);
                            List<Sample_GuildJoiner> joinerList = new List<Sample_GuildJoiner>();
                            foreach (GuildJoiner joiner in joinerInfoList)
                            {
                                Sample_GuildJoiner sample_Joiner = new Sample_GuildJoiner();
                                sample_Joiner.joinerAID = joiner.JoinerAID == guildInfo.GuildCreateAID ? 0 : joiner.JoinerAID;
                                sample_Joiner.isJoinerAttend = joiner.JoinerAID == guildInfo.GuildCreateAID ? HttpContext.GetGlobalResourceObject("languageResource", "lang_guildLeader").ToString() : HttpContext.GetGlobalResourceObject("languageResource", "lang_guildMember").ToString();

                                Account userinfo = AccountManager.GetAccountData(ref tb, joiner.JoinerAID);
                                if (!string.IsNullOrEmpty(userinfo.UserName))
                                {
                                    sample_Joiner.JoinerName = userinfo.UserName;
                                    sample_Joiner.Lastconntime = userinfo.LastConnTime;
                                    Character equipCharacter = CharacterManager.GetCharacter(ref tb, userinfo.AID, userinfo.EquipCID);
                                    sample_Joiner.JoinerLevel = equipCharacter.level;
                                    sample_Joiner.ClassType = equipCharacter.Class;
                                }
                                joinerList.Add(sample_Joiner);
                            }
                            dataList.DataSource = joinerList;
                            dataList.DataBind();
                        }
                        else
                        {
                            if (AID > 0)
                            {
                                long creatAID = GuildManager.GetGuildMasterAID(ref tb, GID);
                                retError = GuildManager.GuildEntrustAsk(ref tb, creatAID, AID);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    retError = GuildManager.GuildEntrustReply(ref tb, AID, 1, AccountManager.GetSimpleAccountInfo(ref tb, AID).username);
                                }
                            }
                        }
                        
                        queryFetcher.GM_Render(retError);
                    }

                }
                catch (Exception errorEx)
                {
                    queryFetcher.DBLog("StackTrace" + mJsonSerializer.ToJsonString(errorEx.StackTrace));
                    queryFetcher.DBLog(errorEx.Message);
                    retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(errorEx.Message), Result_Define.eResult.System_Exception);
                }
                finally
                {

                    tb.EndTransaction(queryFetcher.Render_errorFlag);
                    string gmid = "";
                    if (Request.Cookies.Count > 0)
                        gmid = GMDataManager.GetUserCookies("userid");
                    queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                    tb.Dispose();

                    if (Page.IsPostBack && queryFetcher.Render_errorFlag)
                        Response.Redirect(Request.RawUrl);
                }
            }
        }
    }
}