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
    public partial class cheat_Ranking_orverload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JsonObjectCollection res = new JsonObjectCollection();
            if (Request["username1"] == null || Request["username1"] == "")
            {
                res.Add(new JsonNumericValue("resultcode", 1));
                res.Add(new JsonStringValue("message", "Post 값 전달 실패"));
                string json_text = res.ToString();
                Response.Write(json_text);
            }
            else
            {
                WebQueryParam queryFetcher = new WebQueryParam();
                string retJson = "";
                queryFetcher.SetDebugMode = true;
                string username1 = queryFetcher.QueryParam_Fetch("username1");
                string username2 = queryFetcher.QueryParam_Fetch("username2");
                int resultType = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("resultType", "-1"));
                
                TxnBlock tb = new TxnBlock();
                {
                    long AID1 = 0;
                    try
                    {    
                        queryFetcher.TxnBlockInit(ref tb, ref AID1);
                        queryFetcher.GlobalDBOpen(ref tb);

                        Result_Define.eResult retErr = Result_Define.eResult.DB_ERROR;
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = "PVP_ChangeOverlordUserRankingBoth_Cheat";
                        var retResult = new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        var retResultString = new SqlParameter("@ResultString", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add("@challengerName", SqlDbType.NVarChar, 32).Value = username1;
                        cmd.Parameters.Add("@targetName", SqlDbType.NVarChar, 32).Value = username2;
                        cmd.Parameters.Add("@RegisterBattleRecord", SqlDbType.Int).Value = resultType;
                        cmd.Parameters.Add(retResult);
                        cmd.Parameters.Add(retResultString);
                        tb.ExcuteSqlStoredProcedure(Account_Define.AccountShardingDB,ref cmd);
                        if (System.Convert.ToInt32(retResult.Value.ToString()) == 0)
                        {
                            retErr = Result_Define.eResult.SUCCESS;
                        }
                        else
                        {
                            Response.Write(retResultString.Value.ToString());
                        }
                        cmd.Dispose();
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