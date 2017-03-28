using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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

namespace TheSoulGMTool
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            string retJson = "";
            string userid = queryFetcher.QueryParam_Fetch("uid", "");

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    string savePath = Request.PhysicalApplicationPath;
                    GMDataManager.GetGMServerIni(ref tb, savePath);
                    Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                    if (Page.IsPostBack)
                    {
                        string userpw = queryFetcher.QueryParam_Fetch("upw", "");
                        string lang = queryFetcher.QueryParam_Fetch(gm_lang.UniqueID, "kr");

                        retError = (Result_Define.eResult)GMDataManager.GetLogin(ref tb, userid, userpw, lang);
                    }
                    else
                    {
                        List<admin_language_code> lang_list = GMDataManager.GetGMToolLanguage(ref tb);
                        gm_lang.DataSource = lang_list;
                        gm_lang.DataTextField = "country";
                        gm_lang.DataValueField = "data_language";
                        gm_lang.DataBind();
                        gm_lang.SelectedValue = "kr";
                        retError = Result_Define.eResult.SUCCESS;
                    }
                    retJson = queryFetcher.GM_Render(retError);
                }
                catch (Exception errorEx)
                {
                    retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(errorEx.Message), Result_Define.eResult.System_Exception);
                }
                finally
                {

                    tb.EndTransaction(queryFetcher.Render_errorFlag);
                    string gmid = "";
                    queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                    tb.Dispose();
                }
                if (Page.IsPostBack)
                {
                    if (queryFetcher.Render_errorFlag)
                    {
                        Response.Redirect("/main.aspx");
                    }
                    else
                    {
                        Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert(\"You do not have permission to access\")</SCRIPT>");
                    }
                }
            }
        }
    }
}