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
    public partial class gameOpenTime : System.Web.UI.Page
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

            string reqServer = queryFetcher.QueryParam_Fetch("serverid", "");
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);

            Dictionary<long, TxnBlock> TxnBlackServer = new Dictionary<long, TxnBlock>();
            TxnBlock tb = new TxnBlock();
            try
            {
                GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                TxnBlackServer.Add(serverID, tb);
                string serverlist = GMDataManager.GetServerCheckList(ref tb, serverID);
                change_server.InnerHtml = serverlist;

                tb.IsoLevel = IsolationLevel.ReadCommitted;
                Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
                if (serverID > -1)
                {
                    string dbkey = GMData_Define.ShardingDBName;
                    int blackMarket = 0;
                    bool checkFlag = false;
                    foreach (ListItem item in blackOpenDay.Items)
                    {
                        checkFlag = item.Selected;
                        if (checkFlag)
                        {
                            blackMarket += System.Convert.ToInt32(item.Value);
                        }
                    }
                    int hotExp_content = 0;
                    foreach (ListItem item in hot_content.Items)
                    {
                        checkFlag = item.Selected;
                        if (checkFlag)
                        {
                            hotExp_content += System.Convert.ToInt32(item.Value);
                        }
                    }

                    if (!Page.IsPostBack)
                    {
                        pageInit(ref tb, dbkey);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(reqServer))
                        {
                            int pvpfree_shour1 = queryFetcher.QueryParam_FetchInt(PvPfree_shour1.UniqueID, -1);
                            int pvpfree_ehour1 = queryFetcher.QueryParam_FetchInt(PvPfree_ehour1.UniqueID, -1);
                            int pvpfree_smin1 = queryFetcher.QueryParam_FetchInt(PvPfree_smin1.UniqueID, -1);
                            int pvpfree_emin1 = queryFetcher.QueryParam_FetchInt(PvPfree_emin1.UniqueID, -1);
                            int pvpfree_shour2 = queryFetcher.QueryParam_FetchInt(PvPfree_shour2.UniqueID, -1);
                            int pvpfree_ehour2 = queryFetcher.QueryParam_FetchInt(PvPfree_ehour2.UniqueID, -1);
                            int pvpfree_smin2 = queryFetcher.QueryParam_FetchInt(PvPfree_smin2.UniqueID, -1);
                            int pvpfree_emin2 = queryFetcher.QueryParam_FetchInt(PvPfree_emin2.UniqueID, -1);
                            int pvp1vs1_shour1 = queryFetcher.QueryParam_FetchInt(PvP1vs1_shour1.UniqueID, -1);
                            int pvp1vs1_ehour1 = queryFetcher.QueryParam_FetchInt(PvP1vs1_ehour1.UniqueID, -1);
                            int pvp1vs1_smin1 = queryFetcher.QueryParam_FetchInt(PvP1vs1_smin1.UniqueID, -1);
                            int pvp1vs1_emin1 = queryFetcher.QueryParam_FetchInt(PvP1vs1_emin1.UniqueID, -1);
                            int pvp1vs1_shour2 = queryFetcher.QueryParam_FetchInt(PvP1vs1_shour2.UniqueID, -1);
                            int pvp1vs1_ehour2 = queryFetcher.QueryParam_FetchInt(PvP1vs1_ehour2.UniqueID, -1);
                            int pvp1vs1_smin2 = queryFetcher.QueryParam_FetchInt(PvP1vs1_smin2.UniqueID, -1);
                            int pvp1vs1_emin2 = queryFetcher.QueryParam_FetchInt(PvP1vs1_emin2.UniqueID, -1);
                            int pvpruby_shour1 = queryFetcher.QueryParam_FetchInt(PvPruby_shour1.UniqueID, -1);
                            int pvpruby_ehour1 = queryFetcher.QueryParam_FetchInt(PvPruby_ehour1.UniqueID, -1);
                            int pvpruby_smin1 = queryFetcher.QueryParam_FetchInt(PvPruby_smin1.UniqueID, -1);
                            int pvpruby_emin1 = queryFetcher.QueryParam_FetchInt(PvPruby_emin1.UniqueID, -1);
                            int pvpruby_shour2 = queryFetcher.QueryParam_FetchInt(PvPruby_shour2.UniqueID, -1);
                            int pvpruby_ehour2 = queryFetcher.QueryParam_FetchInt(PvPruby_ehour2.UniqueID, -1);
                            int pvpruby_smin2 = queryFetcher.QueryParam_FetchInt(PvPruby_smin2.UniqueID, -1);
                            int pvpruby_emin2 = queryFetcher.QueryParam_FetchInt(PvPruby_emin2.UniqueID, -1);
                            int pvpguild_shour1 = queryFetcher.QueryParam_FetchInt(PvPguild_shour1.UniqueID, -1);
                            int pvpguild_ehour1 = queryFetcher.QueryParam_FetchInt(PvPguild_ehour1.UniqueID, -1);
                            int pvpguild_smin1 = queryFetcher.QueryParam_FetchInt(PvPguild_smin1.UniqueID, -1);
                            int pvpguild_emin1 = queryFetcher.QueryParam_FetchInt(PvPguild_emin1.UniqueID, -1);
                            int pvpguild_shour2 = queryFetcher.QueryParam_FetchInt(PvPguild_shour2.UniqueID, -1);
                            int pvpguild_ehour2 = queryFetcher.QueryParam_FetchInt(PvPguild_ehour2.UniqueID, -1);
                            int pvpguild_smin2 = queryFetcher.QueryParam_FetchInt(PvPguild_smin2.UniqueID, -1);
                            int pvpguild_emin2 = queryFetcher.QueryParam_FetchInt(PvPguild_emin2.UniqueID, -1);
                            int BossRate = queryFetcher.QueryParam_FetchInt(bossRate.UniqueID, -1);
                            int blackMarket_shour = queryFetcher.QueryParam_FetchInt(black_shour.UniqueID, -1);
                            int blackMarket_ehour = queryFetcher.QueryParam_FetchInt(black_ehour.UniqueID, -1);
                            int blackMarket_smin = queryFetcher.QueryParam_FetchInt(black_smin.UniqueID, -1);
                            int blackMarket_emin = queryFetcher.QueryParam_FetchInt(black_emin.UniqueID, -1);
                            int shopNew = queryFetcher.QueryParam_FetchInt(shop_new.UniqueID, 0);
                            int matchingValue = queryFetcher.QueryParam_FetchInt(matching.UniqueID, 99999);
                            int couponOnOff = queryFetcher.QueryParam_FetchInt(coupon.UniqueID, 0);
                            int iosCouponOnOff = queryFetcher.QueryParam_FetchInt(ios_coupon.UniqueID, 0);
                            int sevendayEvent = queryFetcher.QueryParam_FetchInt(sevenday.UniqueID);
                            int hotTime_shour = queryFetcher.QueryParam_FetchInt(hot_shour.UniqueID, -1);
                            int hotTime_ehour = queryFetcher.QueryParam_FetchInt(hot_ehour.UniqueID, -1);
                            int hotTime_smin = queryFetcher.QueryParam_FetchInt(hot_smin.UniqueID, -1);
                            int hotTime_emin = queryFetcher.QueryParam_FetchInt(hot_emin.UniqueID, -1);
                            int hot_rate = queryFetcher.QueryParam_FetchInt(hotTimeRate.UniqueID, 100);
                            int pvpparty_shour1 = queryFetcher.QueryParam_FetchInt(PvPparty_shour1.UniqueID, -1);
                            int pvpparty_ehour1 = queryFetcher.QueryParam_FetchInt(PvPparty_ehour1.UniqueID, -1);
                            int pvpparty_smin1 = queryFetcher.QueryParam_FetchInt(PvPparty_smin1.UniqueID, -1);
                            int pvpparty_emin1 = queryFetcher.QueryParam_FetchInt(PvPparty_emin1.UniqueID, -1);
                            int pvpparty_shour2 = queryFetcher.QueryParam_FetchInt(PvPparty_shour2.UniqueID, -1);
                            int pvpparty_ehour2 = queryFetcher.QueryParam_FetchInt(PvPparty_ehour2.UniqueID, -1);
                            int pvpparty_smin2 = queryFetcher.QueryParam_FetchInt(PvPparty_smin2.UniqueID, -1);
                            int pvpparty_emin2 = queryFetcher.QueryParam_FetchInt(PvPparty_emin2.UniqueID, -1);
                            int party2ndUseCheck = queryFetcher.QueryParam_FetchInt(pvpPartyUse.UniqueID, 0);
                            if (party2ndUseCheck == 0)
                            {
                                pvpparty_shour2 = pvpparty_shour1;
                                pvpparty_ehour2 = pvpparty_ehour1;
                                pvpparty_smin2 = pvpparty_smin1;
                                pvpparty_emin2 = pvpparty_emin1;
                            }

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
                            int timeValue = 0;
                            int hourValue = 0;
                            int minValue = 0;

                            if (couponOnOff >= 0 && retError == Result_Define.eResult.SUCCESS)
                            {
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.ADMIN_CONST_DEF_COUPON_ON_OFF], couponOnOff);
                            }

                            if (sevendayEvent >= 0 && retError == Result_Define.eResult.SUCCESS)
                            {
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.ADMIN_7DAY_EVENT_ON_OFF], sevendayEvent);
                            }

                            if (iosCouponOnOff >= 0 && retError == Result_Define.eResult.SUCCESS)
                            {
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.ADMIN_CONST_DEF_COUPON_IOS_ON_OFF], iosCouponOnOff);
                            }

                            if (matchingValue != 99999 && retError == Result_Define.eResult.SUCCESS)
                            {
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GoldExpedition_Define.GE_Const_Def_Key_List[GoldExpedition_Define.eGEConstDef.DEF_EXPEDITION_MATCHING_VALUE], matchingValue);
                            }
                            if (pvpparty_shour1 > -1 && pvpparty_smin1 > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (pvpparty_shour1 == 0) ? 0 : (pvpparty_shour1 * 3600);
                                minValue = (pvpparty_smin1 == 0) ? 0 : (pvpparty_smin1 * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, "DEF_PVP_PARTY_START_TIME_1st", timeValue);
                            }
                            if (pvpparty_ehour1 > -1 && pvpparty_emin1 > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (pvpparty_ehour1 == 0) ? 0 : (pvpparty_ehour1 * 3600);
                                minValue = (pvpparty_emin1 == 0) ? 0 : (pvpparty_emin1 * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, "DEF_PVP_PARTY_END_TIME_1st", timeValue);
                            }
                            if (pvpparty_shour2 > -1 && pvpparty_smin2 > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (pvpparty_shour2 == 0) ? 0 : (pvpparty_shour2 * 3600);
                                minValue = (pvpparty_smin2 == 0) ? 0 : (pvpparty_smin2 * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, "DEF_PVP_PARTY_START_TIME_2nd", timeValue);
                            }
                            if (pvpparty_ehour2 > -1 && pvpparty_emin2 > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (pvpparty_ehour2 == 0) ? 0 : (pvpparty_ehour2 * 3600);
                                minValue = (pvpparty_emin2 == 0) ? 0 : (pvpparty_emin2 * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, "DEF_PVP_PARTY_END_TIME_2nd", timeValue);
                            }
                            if (pvpfree_shour1 > -1 && pvpfree_smin1 > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (pvpfree_shour1 == 0) ? 0 : (pvpfree_shour1 * 3600);
                                minValue = (pvpfree_smin1 == 0) ? 0 : (pvpfree_smin1 * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BATTLE_FREEFORALL_START_TIME_1st], timeValue);
                            }
                            if (pvpfree_ehour1 > -1 && pvpfree_emin1 > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (pvpfree_ehour1 == 0) ? 0 : (pvpfree_ehour1 * 3600);
                                minValue = (pvpfree_emin1 == 0) ? 0 : (pvpfree_emin1 * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BATTLE_FREEFORALL_END_TIME_1st], timeValue);
                            }
                            if (pvpfree_shour2 > -1 && pvpfree_smin2 > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (pvpfree_shour2 == 0) ? 0 : (pvpfree_shour2 * 3600);
                                minValue = (pvpfree_smin2 == 0) ? 0 : (pvpfree_smin2 * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BATTLE_FREEFORALL_START_TIME_2nd], timeValue);
                            }
                            if (pvpfree_ehour2 > -1 && pvpfree_emin2 > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (pvpfree_ehour2 == 0) ? 0 : (pvpfree_ehour2 * 3600);
                                minValue = (pvpfree_emin2 == 0) ? 0 : (pvpfree_emin2 * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BATTLE_FREEFORALL_END_TIME_2nd], timeValue);
                            }
                            //1vs1
                            if (pvp1vs1_shour1 > -1 && pvp1vs1_smin1 > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (pvp1vs1_shour1 == 0) ? 0 : (pvp1vs1_shour1 * 3600);
                                minValue = (pvp1vs1_smin1 == 0) ? 0 : (pvp1vs1_smin1 * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.DEF_1VS1REAL_START_TIME], timeValue);
                            }
                            if (pvp1vs1_ehour1 > -1 && pvp1vs1_emin1 > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (pvp1vs1_ehour1 == 0) ? 0 : (pvp1vs1_ehour1 * 3600);
                                minValue = (pvp1vs1_emin1 == 0) ? 0 : (pvp1vs1_emin1 * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.DEF_1VS1REAL_END_TIME], timeValue);
                            }
                            if (pvp1vs1_shour2 > -1 && pvp1vs1_smin2 > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (pvp1vs1_shour2 == 0) ? 0 : (pvp1vs1_shour2 * 3600);
                                minValue = (pvp1vs1_smin2 == 0) ? 0 : (pvp1vs1_smin2 * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.DEF_1VS1REAL_START_TIME_BONUS], timeValue);
                            }
                            if (pvp1vs1_ehour2 > -1 && pvp1vs1_emin2 > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (pvp1vs1_ehour2 == 0) ? 0 : (pvp1vs1_ehour2 * 3600);
                                minValue = (pvp1vs1_emin2 == 0) ? 0 : (pvp1vs1_emin2 * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.DEF_1VS1REAL_END_TIME_BOUNS], timeValue);
                            }
                            //ruby
                            if (pvpruby_shour1 > -1 && pvpruby_smin1 > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (pvpruby_shour1 == 0) ? 0 : (pvpruby_shour1 * 3600);
                                minValue = (pvpruby_smin1 == 0) ? 0 : (pvpruby_smin1 * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.PVP_GLADIATOR_START_TIME_1st], timeValue);
                            }
                            if (pvpruby_ehour1 > -1 && pvpruby_emin1 > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (pvpruby_ehour1 == 0) ? 0 : (pvpruby_ehour1 * 3600);
                                minValue = (pvpruby_emin1 == 0) ? 0 : (pvpruby_emin1 * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.PVP_GLADIATOR_END_TIME_1st], timeValue);
                            }
                            if (pvpruby_shour2 > -1 && pvpruby_smin2 > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (pvpruby_shour2 == 0) ? 0 : (pvpruby_shour2 * 3600);
                                minValue = (pvpruby_smin2 == 0) ? 0 : (pvpruby_smin2 * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.PVP_GLADIATOR_START_TIME_2nd], timeValue);
                            }
                            if (pvpruby_ehour2 > -1 && pvpruby_emin2 > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (pvpruby_ehour2 == 0) ? 0 : (pvpruby_ehour2 * 3600);
                                minValue = (pvpruby_emin2 == 0) ? 0 : (pvpruby_emin2 * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.PVP_GLADIATOR_END_TIME_2nd], timeValue);
                            }
                            //guild
                            if (pvpguild_shour1 > -1 && pvpguild_smin1 > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (pvpguild_shour1 == 0) ? 0 : (pvpguild_shour1 * 3600);
                                minValue = (pvpguild_smin1 == 0) ? 0 : (pvpguild_smin1 * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BATTLE_GUILD_G3VS3_START_TIME_1st], timeValue);
                            }
                            if (pvpguild_ehour1 > -1 && pvpguild_emin1 > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (pvpguild_ehour1 == 0) ? 0 : (pvpguild_ehour1 * 3600);
                                minValue = (pvpguild_emin1 == 0) ? 0 : (pvpguild_emin1 * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BATTLE_GUILD_G3VS3_END_TIME_1st], timeValue);
                            }
                            if (pvpguild_shour2 > -1 && pvpguild_smin2 > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (pvpguild_shour2 == 0) ? 0 : (pvpguild_shour2 * 3600);
                                minValue = (pvpguild_smin2 == 0) ? 0 : (pvpguild_smin2 * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BATTLE_GUILD_G3VS3_START_TIME_2nd], timeValue);
                            }
                            if (pvpguild_ehour2 > -1 && pvpguild_emin2 > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (pvpguild_ehour2 == 0) ? 0 : (pvpguild_ehour2 * 3600);
                                minValue = (pvpguild_emin2 == 0) ? 0 : (pvpguild_emin2 * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BATTLE_GUILD_G3VS3_END_TIME_2nd], timeValue);
                            }

                            //boss
                            if (BossRate > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BOSSRAID_APPEAR_PROBABILITY], BossRate);
                            }

                            //shop
                            if (!string.IsNullOrEmpty(shopNew.ToString()) && retError == Result_Define.eResult.SUCCESS)
                            {
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.SHOP_NEW_ON_OFF], shopNew);
                            }

                            //blackMarket
                            if (blackMarket > 0 && retError == Result_Define.eResult.SUCCESS)
                            {
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BLACKMARKET_OPEN_DAY], blackMarket);
                            }
                            if (blackMarket_shour > -1 && blackMarket_smin > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (blackMarket_shour == 0) ? 0 : (blackMarket_shour * 3600);
                                minValue = (blackMarket_smin == 0) ? 0 : (blackMarket_smin * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BLACKMARKET_OPEN_START_TIME], timeValue);
                            }
                            if (blackMarket_ehour > -1 && blackMarket_emin > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (blackMarket_ehour == 0) ? 0 : (blackMarket_ehour * 3600);
                                minValue = (blackMarket_emin == 0) ? 0 : (blackMarket_emin * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BLACKMARKET_OPEN_END_TIME], timeValue);
                            }
                            //hot time
                            if (hotExp_content > 0 && retError == Result_Define.eResult.SUCCESS)
                            {
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, "EXTRA_EXP_CONTENTS", hotExp_content);
                            }

                            if (hot_rate > 100 && retError == Result_Define.eResult.SUCCESS)
                            {
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, SystemData_Define.AdminConstKey_List[SystemData_Define.eAdminConst.EXTRA_EXP_RATE], hot_rate);
                            }
                            if (hotTime_shour > -1 && hotTime_smin > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (hotTime_shour == 0) ? 0 : (hotTime_shour * 3600);
                                minValue = (hotTime_smin == 0) ? 0 : (hotTime_smin * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, SystemData_Define.AdminConstKey_List[SystemData_Define.eAdminConst.EXTRA_EXP_START_TIME], timeValue);
                            }
                            if (hotTime_ehour > -1 && hotTime_emin > -1 && retError == Result_Define.eResult.SUCCESS)
                            {
                                hourValue = (hotTime_ehour == 0) ? 0 : (hotTime_ehour * 3600);
                                minValue = (hotTime_emin == 0) ? 0 : (hotTime_emin * 60);
                                timeValue = hourValue + minValue;
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, SystemData_Define.AdminConstKey_List[SystemData_Define.eAdminConst.EXTRA_EXP_END_TIME], timeValue);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, 0, "", GMResult_Define.ControlType.OPNE_TIME_EDIT, queryFetcher.GetReqParams(), reqServer);
                            if (retError == Result_Define.eResult.SUCCESS)
                                result = true;
                            retJson = queryFetcher.GM_Render(retError);
                        }
                    }
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

            if (result)
                Response.Redirect(Request.RawUrl);
        }

        protected void pageInit(ref TxnBlock TB, string dbkey)
        {
            int bossrate = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BOSSRAID_APPEAR_PROBABILITY], dbkey, true);
            int PvPfreeStime1 = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BATTLE_FREEFORALL_START_TIME_1st], dbkey, true);
            int PvPfreeEtime1 = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BATTLE_FREEFORALL_END_TIME_1st], dbkey, true);
            int PvPfreeStime2 = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BATTLE_FREEFORALL_START_TIME_2nd], dbkey, true);
            int PvPfreeEtime2 = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BATTLE_FREEFORALL_END_TIME_2nd], dbkey, true);
            int PvP1vs1Stime1 = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.DEF_1VS1REAL_START_TIME], dbkey, true);
            int PvP1vs1Etime1 = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.DEF_1VS1REAL_END_TIME], dbkey, true);
            int PvP1vs1Stime2 = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.DEF_1VS1REAL_START_TIME_BONUS], dbkey, true);
            int PvP1vs1Etime2 = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.DEF_1VS1REAL_END_TIME_BOUNS], dbkey, true);
            int PvPrubyStime1 = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.PVP_GLADIATOR_START_TIME_1st], dbkey, true);
            int PvPrubyEtime1 = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.PVP_GLADIATOR_END_TIME_1st], dbkey, true);
            int PvPrubyStime2 = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.PVP_GLADIATOR_START_TIME_2nd], dbkey, true);
            int PvPrubyEtime2 = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.PVP_GLADIATOR_END_TIME_2nd], dbkey, true);
            int PvPguildStime1 = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BATTLE_GUILD_G3VS3_START_TIME_1st], dbkey, true);
            int PvPguildEtime1 = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BATTLE_GUILD_G3VS3_END_TIME_1st], dbkey, true);
            int PvPguildStime2 = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BATTLE_GUILD_G3VS3_START_TIME_2nd], dbkey, true);
            int PvPguildEtime2 = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BATTLE_GUILD_G3VS3_END_TIME_2nd], dbkey, true);
            int blackMarket = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BLACKMARKET_OPEN_DAY], dbkey, true);
            int blackMarketStime = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BLACKMARKET_OPEN_START_TIME], dbkey, true);
            int blackMarketEtime = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.BLACKMARKET_OPEN_END_TIME], dbkey, true);
            int shopNewOnOff = SystemData.AdminConstValueFetchFromRedis(ref TB, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.SHOP_NEW_ON_OFF], dbkey, true);
            int matchingValue = SystemData.AdminConstValueFetchFromRedis(ref TB, GoldExpedition_Define.GE_Const_Def_Key_List[GoldExpedition_Define.eGEConstDef.DEF_EXPEDITION_MATCHING_VALUE], dbkey, true);
            int couponOnOff = SystemData.AdminConstValueFetchFromRedis(ref TB, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.ADMIN_CONST_DEF_COUPON_ON_OFF], dbkey, true);
            int iosCouponOnOff = SystemData.AdminConstValueFetchFromRedis(ref TB, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.ADMIN_CONST_DEF_COUPON_IOS_ON_OFF], dbkey, true);
            int sevendayEvent = SystemData.AdminConstValueFetchFromRedis(ref TB, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.ADMIN_7DAY_EVENT_ON_OFF], dbkey, true);
            int hotStartTime = SystemData.AdminConstValueFetchFromRedis(ref TB, SystemData_Define.AdminConstKey_List[SystemData_Define.eAdminConst.EXTRA_EXP_START_TIME], dbkey, true);
            int hotEndTime = SystemData.AdminConstValueFetchFromRedis(ref TB, SystemData_Define.AdminConstKey_List[SystemData_Define.eAdminConst.EXTRA_EXP_END_TIME], dbkey, true);
            int hotRate = SystemData.AdminConstValueFetchFromRedis(ref TB, SystemData_Define.AdminConstKey_List[SystemData_Define.eAdminConst.EXTRA_EXP_RATE], dbkey, true);
            int hotContent = SystemData.AdminConstValueFetchFromRedis(ref TB, "EXTRA_EXP_CONTENTS", dbkey, true);
            int PvPPartyStime1 = SystemData.AdminConstValueFetchFromRedis(ref TB, "DEF_PVP_PARTY_START_TIME_1st", dbkey, true);
            int PvPPartyEtime1 = SystemData.AdminConstValueFetchFromRedis(ref TB, "DEF_PVP_PARTY_END_TIME_1st", dbkey, true);
            int PvPPartyStime2 = SystemData.AdminConstValueFetchFromRedis(ref TB, "DEF_PVP_PARTY_START_TIME_2nd", dbkey, true);
            int PvPPartyEtime2 = SystemData.AdminConstValueFetchFromRedis(ref TB, "DEF_PVP_PARTY_END_TIME_2nd", dbkey, true);
            labGold.Text = matchingValue.ToString();

            shop_new.SelectedValue = shopNewOnOff.ToString();
            coupon.SelectedValue = couponOnOff.ToString();
            ios_coupon.SelectedValue = iosCouponOnOff.ToString();
            sevenday.SelectedValue = sevendayEvent.ToString();

            hot_content.DataSource = Enum.GetNames(typeof(SystemData_Define.eContentsType)).Select(o => new { Text = GMData_Define.ContentsType_List[(SystemData_Define.eContentsType)Enum.Parse(typeof(SystemData_Define.eContentsType), o)], Value = (int)(Enum.Parse(typeof(SystemData_Define.eContentsType), o)) });
            hot_content.DataTextField = "Text";
            hot_content.DataValueField = "Value";
            hot_content.DataBind();
            hot_content.Items.RemoveAt(hot_content.Items.Count - 1);
            hot_content.Items.RemoveAt(0);

            if (blackMarket == 128)
            {
                foreach (ListItem item in blackOpenDay.Items)
                {
                    item.Selected = true;
                }
            }
            else
            {
                if (blackMarket > 0)
                {
                    BitArray bits = new BitArray(System.BitConverter.GetBytes(blackMarket));
                    bool openCheck = false;
                    for (int i = 0; i < blackOpenDay.Items.Count; i++)
                    {
                        openCheck = bits[i];
                        blackOpenDay.Items[i].Selected = openCheck;
                    }
                }
            }

            if (hotContent == 16383)
            {
                foreach (ListItem item in hot_content.Items)
                {
                    item.Selected = true;
                }
            }
            else
            {
                if (hotContent > 0)
                {
                    BitArray bits = new BitArray(System.BitConverter.GetBytes(hotContent));
                    bool openCheck = false;
                    for (int i = 0; i < hot_content.Items.Count; i++)
                    {
                        openCheck = bits[i];
                        hot_content.Items[i].Selected = openCheck;
                    }
                }
            }


            labHotTime.Text = calTime(hotStartTime) + "~" + calTime(hotEndTime);
            labHotTimeRate.Text = hotRate + "%";
            labBlackMarket.Text = calTime(blackMarketStime) + "~" + calTime(blackMarketEtime);
            labBoss.Text = bossrate + "%";
            labPvPfree_1.Text = calTime(PvPfreeStime1) + "~" + calTime(PvPfreeEtime1);
            labPvPfree_2.Text = calTime(PvPfreeStime2) + "~" + calTime(PvPfreeEtime2);
            labPvP1vs1_1.Text = calTime(PvP1vs1Stime1) + "~" + calTime(PvP1vs1Etime1);
            labPvP1vs1_2.Text = calTime(PvP1vs1Stime2) + "~" + calTime(PvP1vs1Etime2);
            labPvPruby_1.Text = calTime(PvPrubyStime1) + "~" + calTime(PvPrubyEtime1);
            labPvPruby_2.Text = calTime(PvPrubyStime2) + "~" + calTime(PvPrubyEtime2);
            labPvPguild_1.Text = calTime(PvPguildStime1) + "~" + calTime(PvPguildEtime1);
            labPvPguild_2.Text = calTime(PvPguildStime2) + "~" + calTime(PvPguildEtime2);
            labPvpParty1.Text = calTime(PvPPartyStime1) + "~" + calTime(PvPPartyEtime1);
            labPvpParty2.Text = (PvPPartyStime1 == PvPPartyStime2 && PvPPartyEtime1 == PvPPartyEtime2) ? "None" : calTime(PvPPartyStime2) + "~" + calTime(PvPPartyEtime2);

            pvpPartyUse.SelectedValue = (PvPPartyStime1 == PvPPartyStime2 && PvPPartyEtime1 == PvPPartyEtime2) ? "0" : "1";

            if (PvPPartyStime1 == PvPPartyStime2 && PvPPartyEtime1 == PvPPartyEtime2)
            {
                
            }

            List<ListItem> hourList = GMDataManager.GetHourList();
            List<ListItem> minList = GMDataManager.GetMinList(1);
            PvPfree_shour1.DataSource = hourList;
            PvPfree_shour1.DataTextField = "Text"; PvPfree_shour1.DataValueField = "Value";
            PvPfree_shour1.DataBind();

            PvPfree_ehour1.DataSource = hourList;
            PvPfree_ehour1.DataTextField = "Text"; PvPfree_ehour1.DataValueField = "Value";
            PvPfree_ehour1.DataBind();

            PvPfree_shour2.DataSource = hourList;
            PvPfree_shour2.DataTextField = "Text"; PvPfree_shour2.DataValueField = "Value";
            PvPfree_shour2.DataBind();

            PvPfree_ehour2.DataSource = hourList;
            PvPfree_ehour2.DataTextField = "Text"; PvPfree_ehour2.DataValueField = "Value";
            PvPfree_ehour2.DataBind();

            PvPfree_smin1.DataSource = minList;
            PvPfree_smin1.DataTextField = "Text"; PvPfree_smin1.DataValueField = "Value";
            PvPfree_smin1.DataBind();

            PvPfree_emin1.DataSource = minList;
            PvPfree_emin1.DataTextField = "Text"; PvPfree_emin1.DataValueField = "Value";
            PvPfree_emin1.DataBind();

            PvPfree_smin2.DataSource = minList;
            PvPfree_smin2.DataTextField = "Text"; PvPfree_smin2.DataValueField = "Value";
            PvPfree_smin2.DataBind();

            PvPfree_emin2.DataSource = minList;
            PvPfree_emin2.DataTextField = "Text"; PvPfree_emin2.DataValueField = "Value";
            PvPfree_emin2.DataBind();

            PvP1vs1_shour1.DataSource = hourList;
            PvP1vs1_shour1.DataTextField = "Text"; PvP1vs1_shour1.DataValueField = "Value";
            PvP1vs1_shour1.DataBind();

            PvP1vs1_ehour1.DataSource = hourList;
            PvP1vs1_ehour1.DataTextField = "Text"; PvP1vs1_ehour1.DataValueField = "Value";
            PvP1vs1_ehour1.DataBind();

            PvP1vs1_shour2.DataSource = hourList;
            PvP1vs1_shour2.DataTextField = "Text"; PvP1vs1_shour2.DataValueField = "Value";
            PvP1vs1_shour2.DataBind();

            PvP1vs1_ehour2.DataSource = hourList;
            PvP1vs1_ehour2.DataTextField = "Text"; PvP1vs1_ehour2.DataValueField = "Value";
            PvP1vs1_ehour2.DataBind();

            PvP1vs1_smin1.DataSource = minList;
            PvP1vs1_smin1.DataTextField = "Text"; PvP1vs1_smin1.DataValueField = "Value";
            PvP1vs1_smin1.DataBind();

            PvP1vs1_emin1.DataSource = minList;
            PvP1vs1_emin1.DataTextField = "Text"; PvP1vs1_emin1.DataValueField = "Value";
            PvP1vs1_emin1.DataBind();

            PvP1vs1_smin2.DataSource = minList;
            PvP1vs1_smin2.DataTextField = "Text"; PvP1vs1_smin2.DataValueField = "Value";
            PvP1vs1_smin2.DataBind();

            PvP1vs1_emin2.DataSource = minList;
            PvP1vs1_emin2.DataTextField = "Text"; PvP1vs1_emin2.DataValueField = "Value";
            PvP1vs1_emin2.DataBind();

            PvPruby_shour1.DataSource = hourList;
            PvPruby_shour1.DataTextField = "Text"; PvPruby_shour1.DataValueField = "Value";
            PvPruby_shour1.DataBind();

            PvPruby_ehour1.DataSource = hourList;
            PvPruby_ehour1.DataTextField = "Text"; PvPruby_ehour1.DataValueField = "Value";
            PvPruby_ehour1.DataBind();

            PvPruby_shour2.DataSource = hourList;
            PvPruby_shour2.DataTextField = "Text"; PvPruby_shour2.DataValueField = "Value";
            PvPruby_shour2.DataBind();

            PvPruby_ehour2.DataSource = hourList;
            PvPruby_ehour2.DataTextField = "Text"; PvPruby_ehour2.DataValueField = "Value";
            PvPruby_ehour2.DataBind();

            PvPruby_smin1.DataSource = minList;
            PvPruby_smin1.DataTextField = "Text"; PvPruby_smin1.DataValueField = "Value";
            PvPruby_smin1.DataBind();

            PvPruby_emin1.DataSource = minList;
            PvPruby_emin1.DataTextField = "Text"; PvPruby_emin1.DataValueField = "Value";
            PvPruby_emin1.DataBind();

            PvPruby_smin2.DataSource = minList;
            PvPruby_smin2.DataTextField = "Text"; PvPruby_smin2.DataValueField = "Value";
            PvPruby_smin2.DataBind();

            PvPruby_emin2.DataSource = minList;
            PvPruby_emin2.DataTextField = "Text"; PvPruby_emin2.DataValueField = "Value";
            PvPruby_emin2.DataBind();

            PvPguild_shour1.DataSource = hourList;
            PvPguild_shour1.DataTextField = "Text"; PvPguild_shour1.DataValueField = "Value";
            PvPguild_shour1.DataBind();

            PvPguild_ehour1.DataSource = hourList;
            PvPguild_ehour1.DataTextField = "Text"; PvPguild_ehour1.DataValueField = "Value";
            PvPguild_ehour1.DataBind();

            PvPguild_shour2.DataSource = hourList;
            PvPguild_shour2.DataTextField = "Text"; PvPguild_shour2.DataValueField = "Value";
            PvPguild_shour2.DataBind();

            PvPguild_ehour2.DataSource = hourList;
            PvPguild_ehour2.DataTextField = "Text"; PvPguild_ehour2.DataValueField = "Value";
            PvPguild_ehour2.DataBind();

            black_shour.DataSource = hourList;
            black_shour.DataTextField = "Text"; black_shour.DataValueField = "Value";
            black_shour.DataBind();

            black_ehour.DataSource = hourList;
            black_ehour.DataTextField = "Text"; black_ehour.DataValueField = "Value";
            black_ehour.DataBind();

            PvPguild_smin1.DataSource = minList;
            PvPguild_smin1.DataTextField = "Text"; PvPguild_smin1.DataValueField = "Value";
            PvPguild_smin1.DataBind();

            PvPguild_emin1.DataSource = minList;
            PvPguild_emin1.DataTextField = "Text"; PvPguild_emin1.DataValueField = "Value";
            PvPguild_emin1.DataBind();

            PvPguild_smin2.DataSource = minList;
            PvPguild_smin2.DataTextField = "Text"; PvPguild_smin2.DataValueField = "Value";
            PvPguild_smin2.DataBind();

            PvPguild_emin2.DataSource = minList;
            PvPguild_emin2.DataTextField = "Text"; PvPguild_emin2.DataValueField = "Value";
            PvPguild_emin2.DataBind();

            black_smin.DataSource = minList;
            black_smin.DataTextField = "Text"; black_smin.DataValueField = "Value";
            black_smin.DataBind();

            black_emin.DataSource = minList;
            black_emin.DataTextField = "Text"; black_emin.DataValueField = "Value";
            black_emin.DataBind();

            hot_shour.DataSource = hourList;
            hot_shour.DataTextField = "Text"; hot_shour.DataValueField = "Value";
            hot_shour.DataBind();

            hot_ehour.DataSource = hourList;
            hot_ehour.DataTextField = "Text"; hot_ehour.DataValueField = "Value";
            hot_ehour.DataBind();

            hot_smin.DataSource = minList;
            hot_smin.DataTextField = "Text"; hot_smin.DataValueField = "Value";
            hot_smin.DataBind();

            hot_emin.DataSource = minList;
            hot_emin.DataTextField = "Text"; hot_emin.DataValueField = "Value";
            hot_emin.DataBind();

            PvPparty_shour1.DataSource = hourList;
            PvPparty_shour1.DataTextField = "Text"; PvPparty_shour1.DataValueField = "Value";
            PvPparty_shour1.DataBind();

            PvPparty_shour2.DataSource = hourList;
            PvPparty_shour2.DataTextField = "Text"; PvPparty_shour2.DataValueField = "Value";
            PvPparty_shour2.DataBind();

            PvPparty_ehour1.DataSource = hourList;
            PvPparty_ehour1.DataTextField = "Text"; PvPparty_ehour1.DataValueField = "Value";
            PvPparty_ehour1.DataBind();

            PvPparty_ehour2.DataSource = hourList;
            PvPparty_ehour2.DataTextField = "Text"; PvPparty_ehour2.DataValueField = "Value";
            PvPparty_ehour2.DataBind();

            PvPparty_smin1.DataSource = minList;
            PvPparty_smin1.DataTextField = "Text"; PvPparty_smin1.DataValueField = "Value";
            PvPparty_smin1.DataBind();

            PvPparty_smin2.DataSource = minList;
            PvPparty_smin2.DataTextField = "Text"; PvPparty_smin2.DataValueField = "Value";
            PvPparty_smin2.DataBind();

            PvPparty_emin1.DataSource = minList;
            PvPparty_emin1.DataTextField = "Text"; PvPparty_emin1.DataValueField = "Value";
            PvPparty_emin1.DataBind();

            PvPparty_emin2.DataSource = minList;
            PvPparty_emin2.DataTextField = "Text"; PvPparty_emin2.DataValueField = "Value";
            PvPparty_emin2.DataBind();
        }

        protected string calTime(int sec)
        {
            string retValue = "";
            string hour = (sec / 3600).ToString();
            string min = ((sec % 3600) == 0) ? "00" : ((sec % 3600) / 60).ToString();
            retValue = hour + " : " + min;
            return retValue;
        }

    }
}