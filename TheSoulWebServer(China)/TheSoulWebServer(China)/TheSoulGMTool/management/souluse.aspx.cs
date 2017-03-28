using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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
using TheSoulWebServer.Tools;
using TheSoulGMTool.DBClass;

namespace TheSoulGMTool.management
{
    public partial class souluse : System.Web.UI.Page
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

            Dictionary<long, TxnBlock> TxnBlackServer = new Dictionary<long, TxnBlock>();
            TxnBlock tb = new TxnBlock();

            try
            {
                GMDataManager.GetServerinit(ref tb, serverID);
                TxnBlackServer.Add(serverID, tb);
                tb.IsoLevel = IsolationLevel.ReadCommitted;

                string serverlist = GMDataManager.GetServerCheckList(ref tb, serverID);
                change_server.InnerHtml = serverlist;
                if (!Page.IsPostBack)
                {
                    List<GM_SoulGroup> showSoulList = GMDataManager.GetSoulActiveList(ref tb);
                    List<GM_SoulGroup> hideSoulList = GMDataManager.GetSoulActiveList(ref tb, false);
                    showSoul.DataSource = showSoulList;
                    showSoul.DataTextField = "SoulName";
                    showSoul.DataValueField = "SoulGroup";
                    showSoul.DataBind();
                    hideSoul.DataSource = hideSoulList;
                    hideSoul.DataTextField = "SoulName";
                    hideSoul.DataValueField = "SoulGroup";
                    hideSoul.DataBind();
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
                foreach (KeyValuePair<long, TxnBlock> setItem in TxnBlackServer)
                {
                    setItem.Value.EndTransaction(queryFetcher.Render_errorFlag);
                    if (setItem.Key == serverID)
                    {
                        string gmid = "";
                        if (Request.Cookies.Count > 0)
                            gmid = GMDataManager.GetUserCookies("userid");
                        queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                    }
                    setItem.Value.Dispose();
                }
            }

        }

        protected void addItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < showSoul.Items.Count; i++)
            {
                if (showSoul.Items[i].Selected)
                {
                    hideSoul.Items.Add(showSoul.Items[i]);
                    hideSoul.ClearSelection();
                    showSoul.Items.Remove(showSoul.Items[i]);
                }
            }
        }

        protected void removeItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < hideSoul.Items.Count; i++)
            {
                if (hideSoul.Items[i].Selected)
                {
                    showSoul.Items.Add(hideSoul.Items[i]);
                    showSoul.ClearSelection();
                    hideSoul.Items.Remove(hideSoul.Items[i]);
                }
            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            queryFetcher.bDBLog = true;
            string retJson = "";
            string reqServer = queryFetcher.QueryParam_Fetch("serverid", "");
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);

            Dictionary<long, TxnBlock> TxnBlackServer = new Dictionary<long, TxnBlock>();
            TxnBlock tb = new TxnBlock();

            try
            {
                GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                TxnBlackServer.Add(serverID, tb);
                tb.IsoLevel = IsolationLevel.ReadCommitted;

                if (Page.IsPostBack)
                {
                    string[] reqServerList = System.Text.RegularExpressions.Regex.Split(reqServer, ",");
                    foreach (string Key in reqServerList)
                    {
                        long ServerKey = System.Convert.ToInt64(Key);
                        if (!TxnBlackServer.ContainsKey(ServerKey))
                        {
                            TxnBlock tb2 = new TxnBlock();
                            TheSoulDBcon.GetInstance().TheSoulDBInitFromGlobal(ref tb2, (int)ServerKey, true);
                            TxnBlackServer.Add(ServerKey, tb2);
                        }
                    }
                    List<GM_SoulGroup> soulList = new List<GM_SoulGroup>();
                    foreach (ListItem item in showSoul.Items)
                    {
                        GM_SoulGroup soulItem = new GM_SoulGroup();
                        soulItem.SoulGroup = System.Convert.ToInt64(item.Value);
                        soulItem.hide = 0;
                        soulList.Add(soulItem);
                    }
                    foreach (ListItem item in hideSoul.Items)
                    {
                        GM_SoulGroup soulItem = new GM_SoulGroup();
                        soulItem.SoulGroup = System.Convert.ToInt64(item.Value);
                        soulItem.hide = 1;
                        soulList.Add(soulItem);
                    }
                    Result_Define.eResult retError = GMDataManager.SetSoulActive(ref TxnBlackServer, soulList);
                    //if (retError == Result_Define.eResult.SUCCESS)
                    //    SoulManager.RemoveAdmin_System_Soul_ActiveList();
                    retJson = queryFetcher.GM_Render(retError);
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
                foreach (KeyValuePair<long, TxnBlock> setItem in TxnBlackServer)
                {
                    setItem.Value.EndTransaction(queryFetcher.Render_errorFlag);
                    if (setItem.Key == serverID)
                    {
                        string gmid = "";
                        if (Request.Cookies.Count > 0)
                            gmid = GMDataManager.GetUserCookies("userid");
                        queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                    }
                    setItem.Value.Dispose();
                }
            }
            if (queryFetcher.Render_errorFlag)
                Response.Redirect(Request.RawUrl);
        }

    }
}