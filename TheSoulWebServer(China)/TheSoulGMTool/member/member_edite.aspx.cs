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
using System.Text;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Tools;
using TheSoul.DataManager.Global;
using TheSoulWebServer.Tools;
using TheSoulGMTool.DBClass;

namespace TheSoulGMTool.member
{
    public partial class member_edite : System.Web.UI.Page
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
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, ref queryFetcher);
                    
                    if (Page.IsPostBack)
                    {
                        int useridx = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(idx.UniqueID,"0"));
                        string userPass = queryFetcher.QueryParam_Fetch(userpw.UniqueID, "");
                        string userName = queryFetcher.QueryParam_Fetch(username.UniqueID, "");
                        string userPhone = queryFetcher.QueryParam_Fetch(phone.UniqueID, "");
                        string userEmail = queryFetcher.QueryParam_Fetch(usermail.UniqueID, "");
                        string userPart = queryFetcher.QueryParam_Fetch(userpart.UniqueID, "");
                        string userRank = queryFetcher.QueryParam_Fetch(userrank.UniqueID, "");
                        string userAuth = queryFetcher.QueryParam_Fetch("auth", "");
                        string userServer = queryFetcher.QueryParam_Fetch("serverid", "");
                        string isUsing = "N";
                        if (isusing.Checked)
                            isUsing = "Y";

                        Result_Define.eResult retError = GMDataManager.UpdateGMUSer(ref tb, useridx, userAuth, userServer, user_id.Text.Trim(), userPass, userName, userEmail, userPhone, userPart, userRank, "", 1, isUsing);
                        if (retError == Result_Define.eResult.SUCCESS)
                            retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, useridx, user_id.Text, GMResult_Define.ControlType.GMUSER_EDIT, queryFetcher.GetReqParams(), "-1");
                        retJson = queryFetcher.GM_Render("", retError);
                    }
                    else
                    {
                        int useridx = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("idx", "0"));
                        GM_User data = GMDataManager.GetGMUserData(ref tb, useridx);
                        idx.Value = useridx.ToString();
                        user_id.Text = data.userid;
                        username.Text = data.name;
                        userpw.Text = data.pwd;
                        userpart.Text = data.part;
                        userrank.Text = data.rank;
                        phone.Text = data.phone;
                        usermail.Text = data.email;
                        string serverlist = GMDataManager.GetServerCheckList(ref tb, 0, false, data.serverAuth);
                        serverAuth.InnerHtml = serverlist;
                        StringBuilder authList = new StringBuilder();
                        List<GM_menu_large> menulist = GMDataManager.GetLargeMenu(ref tb);
                        List<GM_UserAuth> userAuthList = GMDataManager.GetUserAuth(ref tb, data.userid);
                        foreach (GM_menu_large menu in menulist)
                        {
                            string menuChecked = "";
                            GM_UserAuth menuAuth = userAuthList.Find(item => item.mdiv.Equals(menu.mdiv));
                            if (menuAuth != null)
                            {
                                menuChecked = menuAuth.auth > 0 ? "checked" : "";
                            }
                            authList.Append("<input type=\"checkbox\" name=\"auth\" value=\"" + menu.mdiv + "\" " + menuChecked + " /> " + menu.menuname + " ");
                        }
                        authlist.InnerHtml = authList.ToString();
                        if (data.isusing == "Y")
                            isusing.Checked = true;
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
                }
                if (queryFetcher.Render_errorFlag)
                {
                    Response.Redirect("/member/member_list.aspx");
                }
            }
        }
    }
}