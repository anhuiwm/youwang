﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Text;
using System.Data;

using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Tools;
using TheSoul.DataManager.Global;

using TheSoulGMTool.DBClass;
using TheSoulGMTool;

using OperationTool.Tools;

namespace OperationTool
{
    public partial class Main : System.Web.UI.MasterPage
    {
        public string ca2 { get; set; }
        public long serverID { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            bool errorFlag = false;
            TxnBlock tb = new TxnBlock();
            {
                try
                {

                    string gmid = HttpContext.Current.Request.Cookies.Count == 0 ? "" : HttpContext.Current.Request.Cookies["mseedadmin"]["userid"];
                    gmName.Text = HttpContext.Current.Request.Cookies.Count == 0 ? "" : HttpContext.Current.Request.Cookies["mseedadmin"]["name"];

                    gmid="test2";

                    if (string.IsNullOrEmpty(gmid))
                    {
                        Response.Redirect("/index.aspx");
                    }
                    else
                    {
                        ca2 = queryFetcher.QueryParam_Fetch_Request("ca2", "1");
                        serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
                        string savePath = Request.PhysicalApplicationPath;
                        GMDataManager.GetGMServerIni(ref tb, savePath);
                        GlobalManager.GetGlobalServerIni(ref tb, savePath);
                        List<GM_UserAuth> adminAuth = GMDataManager.GetUserAuth(ref tb, gmid);
                        StringBuilder leftMenu = new StringBuilder();

                        List<ListItem> serverList = GMDataManager.GetServerListItem(ref tb, serverID);
                        serverlist.DataSource = serverList;
                        serverlist.DataTextField = "Text";
                        serverlist.DataValueField = "Value";
                        serverlist.DataBind();
                        serverlist.Value = serverID.ToString();
                        List<GM_menu_large> largeMeunList = GMDataManager.GetLargeMenu(ref tb);
                        int lageMenuCount = 0;
                        foreach (GM_menu_large largeMenu in largeMeunList)
                        {
                            GM_UserAuth auth = adminAuth.Find(item => item.mdiv.Equals(largeMenu.mdiv));
                            if (auth == null)
                                auth = new GM_UserAuth();
                            if (auth.auth > 0 || gmid == GMData_Define.SuperAdminID)
                            {
                                List<GM_menu> menuList = GMDataManager.GetMenuList(ref tb, largeMenu.mdiv);
                                if (menuList.Count > 0)
                                {
                                    string LargeSelect = "";
                                    if (largeMenu.mdiv.ToString().Equals(ca2))
                                        LargeSelect = " in";
                                    leftMenu.Append("<div class=\"accordion-group\">\n");
                                    leftMenu.Append("<div class=\"accordion-heading\">\n");
                                    leftMenu.Append("<a class=\"accordion-toggle\" data-parent=\"#accordion2\" data-toggle=\"collapse\" href=\"#collapse" + lageMenuCount + "\">" + largeMenu.menuname + "</a>\n</div>");
                                    leftMenu.Append("<div class=\"accordion-body collapse" + LargeSelect + "\" id=\"collapse" + lageMenuCount + "\">\n");
                                    leftMenu.Append("<div class=\"accordion-inner\">\n<ul class=\"nav nav-list\">\n");
                                    foreach (GM_menu menu in menuList)
                                    {
                                        if (menu.linkurl.IndexOf("?") > 0)
                                        {
                                            leftMenu.Append("<li><a href=\"" + menu.linkurl + "&ca2=" + menu.mdiv + "&select_server=" + serverID + "\">" + menu.menuname + "</a></li>\n");
                                        }
                                        else
                                        {
                                            leftMenu.Append("<li><a href=\"" + menu.linkurl + "?ca2=" + menu.mdiv + "&select_server=" + serverID + "\">" + menu.menuname + "</a></li>\n");
                                        }
                                    }
                                    leftMenu.Append("</ul>\n</div>\n</div>\n</div>");
                                    lageMenuCount = lageMenuCount + 1;
                                }
                            }
                        }

                        accordion2.InnerHtml = leftMenu.ToString();
                        errorFlag = true;
                    }
                }
                catch (Exception errorEx)
                {
                    errorFlag = false;
                    string error = "";
                    error = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(errorEx.Message), Result_Define.eResult.System_Exception);
                }
                tb.EndTransaction(errorFlag);
                tb.Dispose();
            }
        }
    }
}
