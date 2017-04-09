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
    public partial class cheat_Guild_Member : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JsonObjectCollection res = new JsonObjectCollection();
            if (Request["guildname"] == null || Request["guildname"] == ""){
                res.Add(new JsonNumericValue("resultcode", 1));
                res.Add(new JsonStringValue("message", "Post 값 전달 실패"));
                string json_text = res.ToString();
                Response.Write(json_text);
            }
            else
            {
                string guildname = Request["guildname"];
                
                WebQueryParam queryFetcher = new WebQueryParam();
                string retJson = "";
                queryFetcher.SetDebugMode = true;
                int usercnt = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("gcount", "0"));
                

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

                                if (usercnt > 0)
                                {
                                    Guild_GuildCreation guildInfo = GuildManager.GetGuilData(ref tb, GuildID);
                                    string setQuery = string.Format(@"Insert into {0} (GuildID, GuildCreateAID, JoinerAID, JoinerState, JoinerCreateDate)
                                                                   Select Top {1} {2}, {3}, AID, 'S', Getdate() From User_account Where AID Not In (Select JoinerAID From {0}) ORDER BY AID ASC", GuildManager.GuildJoinerDBTableName, usercnt, GuildID, guildInfo.GuildCreateAID);
                                    retErr = tb.ExcuteSqlCommand(GuildManager.GuildcommonDBName,setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;

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