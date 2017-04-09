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
using TheSoulGlobalWebServer.Tools;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Global;
using ServiceStack.Text;

namespace TheSoulGlobalWebServer
{
    public partial class RequestGlobal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string[] ops = new string[] {
                "serverlist",
                "load_ip_table",

                // global notice
                "global_notice",
            };

            WebQueryParam queryFetcher = new WebQueryParam();
            queryFetcher.setDebug = (false);
            string retJson = "";

            TxnBlock tb = new TxnBlock();
            {
                long AID = 0;
                try
                {
                    queryFetcher.TxnBlockInit(ref tb, ref AID);

                    string requestOp = queryFetcher.QueryParam_Fetch("op");
                    JsonObject json = new JsonObject();

                    if (Array.IndexOf(ops, requestOp) >= 0)
                    {
                        queryFetcher.operation = requestOp;
                        if (requestOp.Equals("serverlist"))
                        {
                            int platformType = queryFetcher.QueryParam_FetchInt("platform_type");
                            int billingType = queryFetcher.QueryParam_FetchInt("billing_type");
                            int versionNo = queryFetcher.QueryParam_FetchInt("version");

                            /// 如果不是Editor平台，则会去寻找billType和VersionNo都匹配的
                            List<server_group_config_snail> SetServerList = platformType != (int)Global_Define.ePlatformType.EPlatformType_UnityEditer ? 
                                                        GlobalManager.GetServerGroupList_Snail(ref tb, billingType, versionNo)
                                                        : GlobalManager.GetServerGroupList_Snail(ref tb);
                            List<server_config> systemServerList = GlobalManager.GetServerList(ref tb);
                            List<server_group_info> retServerList = new List<server_group_info>();

                            /// snail_ip_table for iptalble?
                            if(!TheSoulDBcon.bIptable)
                                TheSoulDBcon.Snail_ips = GlobalManager.GetSnailIPList(ref tb);

                            string ipfirst = queryFetcher.ipaddress.Split('.')[0];
                            //json = mJsonSerializer.AddJson(json, "ipfirst", mJsonSerializer.ToJsonString(ipfirst));

                            /// 是否是内部IP访问
                            bool bIPPass = (ipfirst.Equals("10") || ipfirst.Equals("127") || ipfirst.Equals("172") || ipfirst.Equals("192"))
                                || TheSoulDBcon.Snail_ips.FindAll(ipaddr => ipaddr.ip_address.Equals(queryFetcher.ipaddress)).Count > 0
                                ;
                            json = mJsonSerializer.AddJson(json, "bIPPass", bIPPass ? "pass" : "fail");

                            /// 不是内部IP，且不是编辑器模式
                            if (platformType != (int)Global_Define.ePlatformType.EPlatformType_UnityEditer && !bIPPass)
                                /// 在serverList中找到billingType不等于0且等于传过来的serverList
                                SetServerList = SetServerList.FindAll(serverinfo => billingType == serverinfo.billing_platform_type && serverinfo.billing_platform_type != 0);
                            json = mJsonSerializer.AddJson(json, "callip", mJsonSerializer.ToJsonString(queryFetcher.ipaddress));

                            bool bServerDown = SetServerList.Count < 1;

                            /// 如果一个serverList也没有
                            if (bServerDown)
                            {
                                server_group_config_snail downGroup = new server_group_config_snail();
                                downGroup.server_group_id = 1;
                                downGroup.server_group_name = "游旺1";
                                downGroup.server_group_status = (int)Global_Define.eServerStatus.Maintenance;
                                SetServerList.Add(downGroup);

                                server_group_config_snail downGroup2 = new server_group_config_snail();
                                downGroup2.server_group_id = 2;
                                downGroup2.server_group_name = "游旺2";
                                downGroup2.server_group_status = (int)Global_Define.eServerStatus.Maintenance;
                                SetServerList.Add(downGroup2);
                            }

                            foreach (server_group_config setGroup in SetServerList)
                            {
                                setGroup.server_info = new List<server_config>();
                                systemServerList.ForEach(setServerinfo =>
                                {
                                    if (setServerinfo.server_group_id == setGroup.server_group_id
                                                && (setServerinfo.server_type.Contains("web_server")
                                                    || setServerinfo.server_type.Contains("cs_login")
                                                        || setServerinfo.server_type.Contains("cs_game"))
                                                && setServerinfo.server_group_id > 0
                                            )
                                    {
                                        if (!(setServerinfo.server_type.Equals("web_server") && setGroup.server_info.Find(server => server.Equals("web_server")) != null)) 
                                            setGroup.server_info.Add(setServerinfo);
                                    }
                                });

                                if (setGroup.server_group_id > 0 && (setGroup.server_group_status != (int)Global_Define.eServerStatus.Hide || bIPPass) && setGroup.server_info.Count > 0)
                                {
                                    server_group_info setRet = new server_group_info();
                                    setRet.server_group_id = setGroup.server_group_id;
                                    setRet.server_group_name = setGroup.server_group_name;
                                    setRet.server_group_status = bIPPass ? (setGroup.server_group_status < (int)Global_Define.eServerStatus.Maintenance ? setGroup.server_group_status : (int)Global_Define.eServerStatus.Normal) :
                                        (setGroup.server_group_status < (int)Global_Define.eServerStatus.Hide ? setGroup.server_group_status : (int)Global_Define.eServerStatus.Maintenance);
                                        //: (setGroup.server_group_status != (int)Global_Define.eServerStatus.Hide ? (int)Global_Define.eServerStatus.Maintenance : (int)Global_Define.eServerStatus.Hide);
                                    //setRet.isplay = (setGroup.user_account_idx > 0) ? 1 : 0;
                                    setRet.server_info = new List<server_info>();
                                    setGroup.server_info.ForEach(setPublic =>
                                    {
                                        server_info setServerInfo = new server_info();
                                        setServerInfo.server_public_ip = setPublic.server_public_ip;
                                        setServerInfo.server_public_port = setPublic.server_public_port;
                                        setServerInfo.server_public_ipv6 = setPublic.server_public_ipv6;
                                        setServerInfo.server_public_ipv6_port = setPublic.server_public_ipv6_port;
                                        setServerInfo.server_type = setPublic.server_type;
                                        setServerInfo.server_status = setPublic.server_status;
                                        setRet.server_info.Add(setServerInfo);
                                    });

                                    retServerList.Add(setRet);
                                }
                            }

                            retServerList = retServerList.OrderByDescending(info => info.server_group_status == (int)Global_Define.eServerStatus.Recommand ? 1 : 0).ThenByDescending(info => info.server_group_id).ToList();

                            json = mJsonSerializer.AddJson(json, Global_Define.RetKey_GlobalServerList, mJsonSerializer.ToJsonString(retServerList));
                        }
                        else if (requestOp.Equals("load_ip_table"))
                        {
                            TheSoulDBcon.Snail_ips = GlobalManager.GetSnailIPList(ref tb);
                        }
                        else if (requestOp.Equals("global_notice"))
                        {
                            long noticeID = queryFetcher.QueryParam_FetchLong("notice_idx");
                            int platformType = queryFetcher.QueryParam_FetchInt("platform_type");
                            //int billingType = queryFetcher.QueryParam_FetchInt("billing_type");
                            Shop_Define.eBillingType billingType = (Shop_Define.eBillingType)queryFetcher.QueryParam_FetchInt("billing_type");
                            int versionNo = queryFetcher.QueryParam_FetchInt("version");

                            Admin_GlobalNotice setNotice = new Admin_GlobalNotice();
                            if (noticeID > 0)
                            {
                                setNotice = GlobalManager.GetAdminNoticeBody(ref tb, noticeID);
                                setNotice.contents = setNotice.contents.Replace("[\"#", "[");
                                setNotice.contents = setNotice.contents.Replace("\"]", "]");
                                setNotice.contents = Server.HtmlDecode(setNotice.contents);
                            }
                            else
                            {
                                Global_Define.eNoticeBillingType chkType = Global_Define.GlobalNoticeBillingType.ContainsKey(billingType) ? Global_Define.GlobalNoticeBillingType[billingType] : Global_Define.eNoticeBillingType.None;
                                List<Admin_GlobalNotice> noticeList = GlobalManager.GetAdminNoticeList(ref tb, versionNo).FindAll(noti => noti.billing_platform_type == 0 || TriggerManager.IsSetMask(noti.billing_platform_type, (int)chkType));
                                if (noticeList == null)
                                    noticeList = new List<Admin_GlobalNotice>();

                                if (noticeList.Count > 0)
                                {
                                    setNotice = noticeList.OrderByDescending(notice => notice.orderNumber).FirstOrDefault();
                                    setNotice.contents = setNotice.contents.Replace("[\"#", "[");
                                    setNotice.contents = setNotice.contents.Replace("\"]", "]");
                                    setNotice.contents = Server.HtmlDecode(setNotice.contents);
                                }
                                json = mJsonSerializer.AddJson(json, Global_Define.RetKey_GlobalNoticeList, RetGlobalNoticeList.MakeRetGlobalNoticeJson(ref noticeList));
                            }

                            if (setNotice != null)
                            {
                                json = mJsonSerializer.AddJson(json, Global_Define.RetKey_GlobalNoticeBody, setNotice.contents);
                            }
                        }
                        retJson = queryFetcher.Render(json, Result_Define.eResult.SUCCESS);
                    }else
                        retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(DefineError.System_Unknown_Operation), Result_Define.eResult.System_Unknown_Operation);
                }
                catch (Exception errorEx)
                {
                    retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(errorEx.Message), Result_Define.eResult.System_Exception);
                }
            }
            tb.EndTransaction();
            tb.Dispose();
        }
    }
}