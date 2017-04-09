using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

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

namespace TheSoulGMTool.management
{
    public partial class globalnotice_list : System.Web.UI.Page
    {
        protected long serverID = 1;
        
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            queryFetcher.bDBLog = true;
            string retJson = "";
            serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, serverID);

                    if (!Page.IsPostBack)
                    {
                        platformType.DataSource = Enum.GetNames(typeof(Global_Define.eNoticeBillingType)).Select(o => new { Text = Enum.Parse(typeof(Global_Define.eNoticeBillingType), o), Value = (int)(Enum.Parse(typeof(Global_Define.eNoticeBillingType), o)) });
                        platformType.DataTextField = "Text";
                        platformType.DataValueField = "Value";
                        platformType.DataBind();
                        platformType.Items.RemoveAt(0);
                    }

                    int platform = 0;

                    foreach (ListItem item in platformType.Items)
                    {
                        if (item.Selected)
                        {
                            platform += System.Convert.ToInt32(item.Value);
                        }
                    }
                    
                    string strVersion1 = queryFetcher.QueryParam_Fetch(Version1.UniqueID, "");
                    string strVersion2 = queryFetcher.QueryParam_Fetch(Version2.UniqueID, "");
                    string strVersion3 = queryFetcher.QueryParam_Fetch(Version3.UniqueID, "");
                    string strVersion4 = queryFetcher.QueryParam_Fetch(Version4.UniqueID, "");
                    int searchActive = queryFetcher.QueryParam_FetchInt(notice_active.UniqueID, -1);
                    if(!string.IsNullOrEmpty(strVersion1))
                        strVersion1 = strVersion1.Length == 1 ? "0" + strVersion1 : strVersion1;
                    if (!string.IsNullOrEmpty(strVersion2))
                        strVersion2 = strVersion2.Length == 1 ? "0" + strVersion2 : strVersion2;
                    if (!string.IsNullOrEmpty(strVersion3))
                        strVersion3 = strVersion3.Length == 1 ? "0" + strVersion3 : strVersion3;
                    if (!string.IsNullOrEmpty(strVersion4) && strVersion4.Length == 1)
                        strVersion4 = "00" + strVersion4;
                    else if (!string.IsNullOrEmpty(strVersion4) && strVersion4.Length == 2)
                        strVersion4 = "0" + strVersion4;

                    string strVersion = "";
                    if (!string.IsNullOrEmpty(strVersion1) && !string.IsNullOrEmpty(strVersion2) && !string.IsNullOrEmpty(strVersion3) && !string.IsNullOrEmpty(strVersion4))
                        strVersion = string.Format("{0}{1}{2}{3}", strVersion1, strVersion2, strVersion3, strVersion4);
                    Dictionary<string, string> searchList = new Dictionary<string, string>();
                    if (!string.IsNullOrEmpty(strVersion) && !searchList.ContainsKey(GMData_Define.eGlobalNoticeSearchKey[GMData_Define.eGlobalNoticeSearch.version]))
                        searchList.Add (GMData_Define.eGlobalNoticeSearchKey[GMData_Define.eGlobalNoticeSearch.version], strVersion);
                    if (searchActive > -1 && !searchList.ContainsKey(GMData_Define.eGlobalNoticeSearchKey[GMData_Define.eGlobalNoticeSearch.active]))
                        searchList.Add(GMData_Define.eGlobalNoticeSearchKey[GMData_Define.eGlobalNoticeSearch.active], searchActive.ToString());
                    if (platform > 0 && !searchList.ContainsKey(GMData_Define.eGlobalNoticeSearchKey[GMData_Define.eGlobalNoticeSearch.platform]))
                        searchList.Add(GMData_Define.eGlobalNoticeSearchKey[GMData_Define.eGlobalNoticeSearch.platform], platform.ToString());

                    List<Admin_GlobalNotice> noticeList = GMDataManager.GetAdminGlobalNoticeList(ref tb, searchList);
                    noticeList.ForEach(item => {
                        if (item.billing_platform_type > 0)
                        {
                            item.editid = "";
                            BitArray bits = new BitArray(System.BitConverter.GetBytes(item.billing_platform_type));
                            bool openCheck = false;
                            for (int i = 0; i < Enum.GetNames(typeof(Global_Define.eNoticeBillingType)).Length; i++)
                            {
                                openCheck = bits[i];
                                if (openCheck)
                                    item.editid = string.IsNullOrEmpty(item.editid) ? Enum.GetNames(typeof(Global_Define.eNoticeBillingType))[i+1].ToString() : item.editid + ", " + Enum.GetNames(typeof(Global_Define.eNoticeBillingType))[i+1].ToString();
                            }
                        }
                        else
                            item.editid = Global_Define.eNoticeBillingType.None.ToString();
                    });
                    dataList.DataSource = noticeList;
                    dataList.DataBind();

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
                    string gmid = Request.Cookies.Count == 0 ? "" : HttpContext.Current.Request.Cookies["mseedadmin"]["userid"];
                    queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                    tb.Dispose();
                }
            }
        }

        protected void dataList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dataList.PageIndex = e.NewPageIndex;
            dataList.DataBind();
        }
    }
}
