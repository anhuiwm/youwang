using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections;

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
    public partial class globalnotice_form : System.Web.UI.Page
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
            bool result = false;
            long reqIdx = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("idx", "0"));
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
            int reqMode = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(mode.UniqueID, "-1"));
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                    tb.IsoLevel = IsolationLevel.ReadCommitted;
                    idx.Value = reqIdx.ToString();
                    mode.Value = reqMode.ToString();
                    
                    int platform = 0;

                    foreach (ListItem item in platformType.Items)
                    {
                        if (item.Selected)
                        {
                            platform += System.Convert.ToInt32(item.Value);
                        }
                    }

                    pageInit(ref tb);
                    
                    Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                    
                    if (reqIdx > 0 && reqMode < 1)
                    {
                        Admin_GlobalNotice dataInfo = GlobalManager.GetAdminNoticeBody(ref tb, reqIdx);
                        notice_tag.SelectedValue = dataInfo.noticeTag;
                        notice_type.SelectedValue = dataInfo.noticeStyle;
                        sDate.Text = dataInfo.startDate.ToString("yyyy-MM-dd");
                        eDate.Text = dataInfo.endDate.ToString("yyyy-MM-dd");
                        ordernum.Text = dataInfo.orderNumber.ToString();
                        notice_active.SelectedValue = dataInfo.active.ToString();
                        string strVersion = string.Format("{0:000000000}", dataInfo.target_version).Insert(6, ".").Insert(4, ".").Insert(2, ".");
                        string[] version = strVersion.Split('.');
                        if (version.Length == 4)
                        {
                            Version1.Text = version[0];
                            Version2.Text = version[1];
                            Version3.Text = version[2];
                            Version4.Text = version[3];
                        }
                        if (dataInfo.billing_platform_type > 0)
                        {
                            BitArray bits = new BitArray(System.BitConverter.GetBytes(dataInfo.billing_platform_type));
                            bool openCheck = false;
                            for (int i = 0; i < platformType.Items.Count; i++)
                            {
                                openCheck = bits[i];
                                platformType.Items[i].Selected = openCheck;
                            }
                        }
                        int hour = dataInfo.startDate.Hour;
                        int min = dataInfo.startDate.Minute;
                        if (hour < 10)
                            sHour.SelectedValue = "0" + hour;
                        else
                            sHour.SelectedValue = hour.ToString();
                        if (min < 10)
                            sMin.SelectedValue = "0" + min;
                        else
                            sMin.SelectedValue = min.ToString();
                        hour = dataInfo.endDate.Hour;
                        min = dataInfo.endDate.Minute;
                        if (hour < 10)
                            eHour.SelectedValue = "0" + hour;
                        else
                            eHour.SelectedValue = hour.ToString();
                        if (min < 10)
                            eMin.SelectedValue = "0" + min;
                        else
                            eMin.SelectedValue = min.ToString();

                        title.Text = dataInfo.title;
                        ctlCkeditor.Text = replceContent(dataInfo.contents, false);
                    }
                    else
                        mode.Value = "0";
                    if (reqMode > -1)
                    {
                        string reqTag = queryFetcher.QueryParam_Fetch(notice_tag.UniqueID, "");
                        string reqType = queryFetcher.QueryParam_Fetch(notice_type.UniqueID, "");
                        string sdate = queryFetcher.QueryParam_Fetch(sDate.UniqueID, "");
                        string edate = queryFetcher.QueryParam_Fetch(eDate.UniqueID, "");
                        string shour = queryFetcher.QueryParam_Fetch(sHour.UniqueID, "");
                        string ehour = queryFetcher.QueryParam_Fetch(eHour.UniqueID, "");
                        string smin = queryFetcher.QueryParam_Fetch(sMin.UniqueID, "");
                        string emin = queryFetcher.QueryParam_Fetch(eMin.UniqueID, "");
                        int reqOrderNumber = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(ordernum.UniqueID, "1"));
                        byte reqActive = System.Convert.ToByte(queryFetcher.QueryParam_Fetch(notice_active.UniqueID, "0"));
                        string reqTitle = queryFetcher.QueryParam_Fetch(title.UniqueID, "");
                        string reqContent = queryFetcher.QueryParam_Fetch(ctlCkeditor.UniqueID, "");
                        string startDate = string.Format("{0} {1}:{2}:00", sdate, shour, smin);
                        string endDate = string.Format("{0} {1}:{2}:59", edate, ehour, emin);
                        string strVersion1 = queryFetcher.QueryParam_Fetch(Version1.UniqueID, "00");
                        string strVersion2 = queryFetcher.QueryParam_Fetch(Version2.UniqueID, "00");
                        string strVersion3 = queryFetcher.QueryParam_Fetch(Version3.UniqueID, "00");
                        string strVersion4 = queryFetcher.QueryParam_Fetch(Version4.UniqueID, "000");
                        strVersion1 = strVersion1.Length == 1 ? "0" + strVersion1 : strVersion1;
                        strVersion2 = strVersion2.Length == 1 ? "0" + strVersion2 : strVersion2;
                        strVersion3 = strVersion3.Length == 1 ? "0" + strVersion3 : strVersion3;
                        if (strVersion4.Length == 1)
                            strVersion4 = "00" + strVersion4;
                        else if (strVersion4.Length == 2)
                            strVersion4 = "0" + strVersion4;
                        string strVersion = string.Format("{0}{1}{2}{3}", strVersion1, strVersion2, strVersion3, strVersion4);
                        

                        reqContent = replceContent(reqContent);

                        if (reqMode == 0)
                        {
                            retError = GMDataManager.InsertGlobalNotice(ref tb, reqTag, reqType, reqTitle, reqContent, startDate, endDate, reqActive, reqOrderNumber, platform ,strVersion);
                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, 0, "", GMResult_Define.ControlType.NOTICE_ADD, queryFetcher.GetReqParams(), "0");
                        }
                        else if (reqMode == 1)
                        {
                            retError = GMDataManager.UpdateGlobalNotice(ref tb, reqIdx, reqTag, reqType, reqTitle, reqContent, startDate, endDate, reqActive, reqOrderNumber, platform, strVersion);
                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, reqIdx, "", GMResult_Define.ControlType.NOTICE_EDIT, queryFetcher.GetReqParams(), "0");
                        }
                        else
                        {
                            retError = GMDataManager.DeleteGlobalNotice(ref tb, reqIdx);
                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, reqIdx, "", GMResult_Define.ControlType.NITICE_DELETE, queryFetcher.GetReqParams(), "0");
                        }

                        if (retError == Result_Define.eResult.SUCCESS)
                        {
                            result = true;
                        }
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

                    tb.EndTransaction(queryFetcher.Render_errorFlag);
                    string gmid = "";
                    //if (Request.Cookies.Count > 0)
                    //    gmid = HttpContext.Current.Request.Cookies["mseedadmin"]["userid"];
                    queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                    tb.Dispose();
                }

                if (result)
                    Response.Redirect("/management/globalnotice_list.aspx?ca2=" + queryFetcher.QueryParam_Fetch_Request("ca2", "1") + "&select_server=" + serverID);
            }
        }

        protected string replceContent(string data, bool htmlMode = true)
        {
            string retunValue = "";
            if (htmlMode)
            {
                retunValue = data.Replace("&lt;p&gt;\r\n\t", "");
                retunValue = retunValue.Replace("&lt;p&gt;\r\n", "");
                retunValue = retunValue.Replace("&lt;/p&gt;\r\n", "\n\n");
                retunValue = retunValue.Replace("&lt;br /&gt;", "\n");
                retunValue = retunValue.Replace("&lt;/span&gt;", "[-]");
                retunValue = retunValue.Replace("&lt;span style=\"color:", "[\"");
                retunValue = retunValue.Replace("&lt;span style=\"color: ", "[\"");
                retunValue = retunValue.Replace(";\"&gt;", "\"]");
                retunValue = retunValue.Replace("\"&gt;", "\"]");
                retunValue = retunValue.Replace("\r", "");
                retunValue = retunValue.Replace("\t", "");
                retunValue = retunValue.Replace("&amp;", "&");                
                bool rgbTagCheck = retunValue.Contains("rgb(");
                int loopCnt = 0;
                while (rgbTagCheck && loopCnt<1000)
                {
                    int startNum = retunValue.LastIndexOf("rgb(");
                    int lastNum = retunValue.LastIndexOf(")") + 1;
                    string rgbValue = retunValue.Substring(startNum, (lastNum - startNum));

                    string chageStr = rgbValue.Replace("rgb(", "");
                    chageStr = chageStr.Replace(")", "");
                    string[] rgb = System.Text.RegularExpressions.Regex.Split(chageStr, ",");
                    string colorHex = "";
                    if (rgb.Length == 3)
                    {
                        int colorRed = System.Convert.ToInt32(rgb[0]);
                        int colorGreen = System.Convert.ToInt32(rgb[1]);
                        int colorBlue = System.Convert.ToInt32(rgb[2]);

                        Color rgbColor = Color.FromArgb(colorRed, colorGreen, colorBlue);
                        colorHex = ColorTranslator.ToHtml(rgbColor);
                    }

                    chageStr = colorHex;
                    retunValue = retunValue.Replace(rgbValue, chageStr);
                    rgbTagCheck = retunValue.Contains("rgb(");

                    loopCnt += 1;

                }
            }
            else
            {
                retunValue = data.Replace("\n\n", "&lt;br /&gt;");
                retunValue = retunValue.Replace("\n", "&lt;br /&gt;");
                retunValue = retunValue.Replace("[-]", "&lt;/span&gt;");
                retunValue = retunValue.Replace("[\"", "&lt;span style=\"color:");
                retunValue = retunValue.Replace("\"]",";\"&gt;");
            }
            return retunValue;
        }
        protected void pageInit(ref TxnBlock TB)
        {
            List<ListItem> hourList = GMDataManager.GetHourList();
            List<ListItem> minList = GMDataManager.GetMinList(1);
            sHour.DataSource = hourList;
            sHour.DataBind();

            eHour.DataSource = hourList;
            eHour.DataBind();

            sMin.DataSource = minList;
            sMin.DataBind();

            eMin.DataSource = minList;
            eMin.DataBind();

            platformType.DataSource = Enum.GetNames(typeof(Global_Define.eNoticeBillingType)).Select(o => new { Text = Enum.Parse(typeof(Global_Define.eNoticeBillingType), o), Value = (int)(Enum.Parse(typeof(Global_Define.eNoticeBillingType), o)) });
            platformType.DataTextField = "Text";
            platformType.DataValueField = "Value";
            platformType.DataBind();
            platformType.Items.RemoveAt(0);

            List<string> notice_TagList = Global_Define.GlobalNoticeTag.Select(item => item.Key).ToList();
            List<string> notice_TypeList = Global_Define.GlobalNoticeType.Select(item => item.Key).ToList();
            notice_TagList.RemoveAt(0);
            notice_TypeList.RemoveAt(0);
            notice_tag.DataSource = notice_TagList;
            notice_tag.DataBind();
            notice_type.DataSource = notice_TypeList;
            notice_type.DataBind();

            string maxVersion = string.Format("{0:000000000}", GMDataManager.GetMaxTargetVersion(ref TB)).Insert(6, ".").Insert(4, ".").Insert(2, ".");
            string[] version = maxVersion.Split('.');
            if (version.Length == 4)
            {
                Version1.Text = version[0];
                Version2.Text = version[1];
                Version3.Text = version[2];
                Version4.Text = version[3];
            }
        }
    }
}