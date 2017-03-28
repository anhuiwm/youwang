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


namespace TheSoulGMTool.User
{
    public partial class userDetail : System.Web.UI.Page
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

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, serverID);

                    long AID = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("aid", "0"));
                    userIdx.Value = AID.ToString();
                    
                    if (serverID > -1 && AID > 0)
                    {
                        Account userInfo = AccountManager.GetAccountData(ref tb, AID, true);
                        userInfo.SetGuildInfo(GuildManager.GetGuildInfo(ref tb, AID));//길드명 설정을 위해 추가
                        List<Character> characterList = CharacterManager.GetCharacterList(ref tb, AID, true);
                        User_VIP vipInfo = VipManager.GetUser_VIPInfo(ref tb, AID, true);
                        labVipInfo.Text = string.Format("{0} ({1})", vipInfo.viplevel, vipInfo.vippoint);
                        labGold.Text = userInfo.Gold.ToString();
                        labKey.Text = userInfo.Key + " / " + userInfo.KeyFillMaxEA;
                        labTicket.Text = userInfo.Ticket + " / " + userInfo.TicketFillMaxEA;
                        labBattlepoint.Text = userInfo.CombatPoint.ToString();
                        labBlackMarketPoint.Text = userInfo.BlackMarketPoint.ToString();
                        labDonationpoint.Text = userInfo.ContributionPoint.ToString();
                        labPartypoint.Text = userInfo.PartyDungeonPoint.ToString();
                        labExpeditionpoint.Text = userInfo.ExpeditionPoint.ToString();
                        labHonorpoint.Text = userInfo.Honorpoint.ToString();
                        labOverloadPoint.Text = userInfo.OverlordPoint.ToString();
                        labGuildName.Text = userInfo.GuildName;
                        labTutorial.Text = userInfo.Tutorial == 0 ? "On" : "Off";
                        labCash.Text = userInfo.Cash.ToString();
                        labEventCash.Text = userInfo.EventCash.ToString();
                        lab_stone.Text = userInfo.Stone.ToString();

                        int lastMissionID = Dungeon_Manager.GetUser_Mission_LastStage(ref tb, AID);
                        if (lastMissionID == 0)
                            lastMissionID = 1;

                        string missionName = Dungeon_Manager.GetSystem_MissionStageInfo(ref tb, lastMissionID).NamingCN.Replace("STRING_NAMING_SCENARIO_WORLD_", "").Replace("_STAGE", "");
                        if (!string.IsNullOrEmpty(missionName))
                        {
                            string[] arryMissionName = missionName.Split('_');
                            int wordID = System.Convert.ToInt32(arryMissionName[0]);
                            int missionID = System.Convert.ToInt32(arryMissionName[0]);
                            labMissionName.Text = wordID.ToString() + "-" + missionID.ToString();
                        }
                        
                        labPc1.Text = "0";
                        labPc2.Text = "0";
                        labPc3.Text = "0";

                        int maxLevel = GMDataManager.GetCharacterLevelList(ref tb).Max(item => item.Level);
                        List<User_Ultimate_Inven> ultimateItemList = new List<User_Ultimate_Inven>();
                        foreach (Character item in characterList)
                        {
                            int nextLevel = item.level;
                            if (item.level < maxLevel)
                                nextLevel = item.level + 1;
                            int nextExp = CharacterManager.GetSystemExp(ref tb, nextLevel).PCLevelUpEXP;
                            
                            Character_Detail_With_HP charInfo = CharacterManager.GetCharacterWithEquip_HP(ref tb, AID, item.cid);
                            List<User_Inven> equipList = charInfo.equiplist; //ItemManager.GetEquipList(ref tb, AID, item.cid, true, true);
                            List<Ret_Equip_Soul_Active> activeSoul = charInfo.equip_active_soul;
                            List<Ret_Soul_Passive> passiveSoul = charInfo.equip_passive_soul;
                            string activeInfo = "";
                            activeSoul.ForEach(soul =>
                                {
                                    string itemName = SoulManager.GetSoul_System_Soul_Active(ref tb, soul.soulid, false).NamingCN;
                                    if (string.IsNullOrEmpty(activeInfo))
                                    {
                                        activeInfo = string.Format("{0} (Level : {1} / Grade : {2})", GMDataManager.GetItmeName(ref tb, itemName), soul.level, soul.grade);
                                    }
                                    else
                                    {
                                        activeInfo = string.Format("{0}<br>{1} (Level : {2} / Grade : {3})", activeInfo, GMDataManager.GetItmeName(ref tb, itemName), soul.level, soul.grade);
                                    }
                                }
                            );
                            string passiveInfo = "";
                            passiveSoul.ForEach(soul =>
                            {
                                string itemName = SoulManager.GetSoul_System_Soul_Passive(ref tb, soul.soulid, false).NamingCN;
                                if (string.IsNullOrEmpty(activeInfo))
                                {
                                    passiveInfo = string.Format("{0} (Level : {1})", GMDataManager.GetItmeName(ref tb, itemName), soul.level);
                                }
                                else
                                {
                                    passiveInfo = string.Format("{0}<br>{1} (Level : {2})", passiveInfo, GMDataManager.GetItmeName(ref tb, itemName), soul.level);
                                }
                            });
                            string soulInfo = string.Format("<b>ActiveSoul</b><br>{0}<br><b>PassiveSoul</b><br>{1}", activeInfo, passiveInfo);
                            if (string.IsNullOrEmpty(activeInfo) && string.IsNullOrEmpty(passiveInfo))
                                soulInfo = "";
                            string equipItemInfo = "";
                            foreach (User_Inven invenItem in equipList)
                            {
                                string itemName = ItemManager.GetSystem_Item_Base(ref tb, invenItem.itemid).Name;
                                if (string.IsNullOrEmpty(equipItemInfo))
                                {
                                    equipItemInfo = string.Format("{0} (Level : {1} / Grade : {2})", GMDataManager.GetItmeName(ref tb, itemName), invenItem.level, invenItem.grade);
                                }
                                else
                                {
                                    equipItemInfo = string.Format("{0}<br>{1} (Level : {2} / Grade : {3})", equipItemInfo, GMDataManager.GetItmeName(ref tb, itemName), invenItem.level, invenItem.grade);
                                }
                            }
                            //ultimate item
                            List<User_Ultimate_Inven> ultimateEquipList = ItemManager.GetEquipUltimate(ref tb, AID, item.cid).FindAll(ultimateItem => ultimateItem.equipflag == "Y");
                            foreach (User_Ultimate_Inven ultimate in ultimateEquipList)
                            {
                                long ultimateID = ItemManager.GetSystem_Ultimate_Enchant(ref tb, ultimate.item_id).Ultimate_ID;
                                string itemName = ItemManager.GetSystem_Item_Base(ref tb, ultimateID).Name;
                                if (string.IsNullOrEmpty(equipItemInfo))
                                {
                                    equipItemInfo = string.Format("{0} (Level : {1} / Step : {2})", GMDataManager.GetItmeName(ref tb, itemName), ultimate.level, ultimate.step);
                                }
                                else
                                {
                                    equipItemInfo = string.Format("{0}<br>{1} (Level : {2} / Step : {3})", equipItemInfo, GMDataManager.GetItmeName(ref tb, itemName), ultimate.level, ultimate.step);
                                }
                            }

                            Character_Stat statInfo = CharacterManager.GetCharacterStat(ref tb, item.cid, true);
                            string setStat = string.Format(@"{0} : {9}<br>
                                                            {1} : {10}<br>
                                                            {2} : {11}<br>
                                                            {3} : {12}<br>
                                                            {4} : {13}<br>
                                                            {5} : {14}<br>
                                                            {6} : {15}<br>
                                                            {7} : {16}<br>
                                                            {8} : {17}<br>
                                                            {18} : {19}<br>",
                                                            Resources.languageResource.lang_hp, Resources.languageResource.lang_mp, Resources.languageResource.lang_minAP, Resources.languageResource.lang_maxAP,
                                                            Resources.languageResource.lang_dp, Resources.languageResource.lang_CPR, Resources.languageResource.String187, Resources.languageResource.lang_CP, Resources.languageResource.lang_DCR,
                                                            statInfo.HP, statInfo.MP, statInfo.ATTACK_MIN, statInfo.ATTACK_MAX, statInfo.DEFENCE_POINT, statInfo.CPR, statInfo.CRITICAL_PROTECTION, statInfo.CRITICAL_RATING, statInfo.DEFENCE_CRITICAL_RATING,
                                                            Resources.languageResource.lang_totalWarPoint, (statInfo.WAR_POINT+statInfo.ACTIVE_SOUL_WAR_POINT+statInfo.PASSIVE_SOUL_WAR_POINT)/10);
                            string setData = string.Format("{0}({1}/{2})", item.level, item.exp, nextExp);
                            if (item.Class == 1){
                                labPc1.Text = setData;
                                labPc1Stat.Text = setStat;
                                labPc1Item.Text = equipItemInfo;
                                labPc1Soul.Text = soulInfo;
                            }
                            else if (item.Class == 2)
                            {
                                labPc2.Text = setData;
                                labPc2Stat.Text = setStat;
                                labPc2Item.Text = equipItemInfo;
                                labPc2Soul.Text = soulInfo;
                            }
                            else
                            {
                                labPc3.Text = setData;
                                labPc3Stat.Text = setStat;
                                labPc3Item.Text = equipItemInfo;
                                labPc3Soul.Text = soulInfo;
                            }
                            ultimateItemList.AddRange(ItemManager.GetEquipUltimate(ref tb, AID, item.cid));    
                        }

                        ultimateItemList.ForEach(invenItem =>
                        {
                            long ultimateID = ItemManager.GetSystem_Ultimate_Enchant(ref tb, invenItem.item_id).Ultimate_ID;
                            string itemName = ItemManager.GetSystem_Item_Base(ref tb, ultimateID).Name;
                            invenItem.equipflag = string.IsNullOrEmpty(itemName) ? "" : GMDataManager.GetItmeName(ref tb, itemName);
                        });
                        ultimatelist.DataSource = ultimateItemList;
                        ultimatelist.DataBind();

                        List<User_Inven> itemList = GMDataManager.GetUserInvenList(ref tb, AID);
                        itemList.ForEach(invenItem =>
                        {
                            string stringCN = ItemManager.GetSystem_Item_Base(ref tb, invenItem.itemid).Name;
                            invenItem.delflag = string.IsNullOrEmpty(stringCN) ? "" : GMDataManager.GetItmeName(ref tb, stringCN);
                            invenItem.newyn = invenItem.class_type > 0 ? Character_Define.ClassEnumToType[(Character_Define.SystemClassType)invenItem.class_type] : "All";
                        });
                        itemlist.DataSource = itemList;
                        itemlist.DataBind();

                        List<User_ActiveSoul> soulList1 = SoulManager.GetUser_ActiveSoul(ref tb, AID).FindAll(soul => soul.delflag == "N");
                        soulList1.ForEach(soul =>
                        {
                            string stringCN = SoulManager.GetSoul_System_Soul_Active(ref tb, soul.soulid).NamingCN;
                            soul.delflag = string.IsNullOrEmpty(stringCN) ? "" : GMDataManager.GetItmeName(ref tb, stringCN);
                        });
                        soullist1.DataSource = soulList1;
                        soullist1.DataBind();

                        List<User_PassiveSoul> soulList2 = GMDataManager.GetUserPassiveSoulList(ref tb, AID);
                        soulList2.ForEach(soul =>
                        {
                            string stringCN = SoulManager.GetSoul_System_Soul_Passive(ref tb, soul.soulid).NamingCN;
                            soul.delflag = string.IsNullOrEmpty(stringCN) ? "" : GMDataManager.GetItmeName(ref tb, stringCN);
                            soul.stateflag = soul.class_type > 0 ? Character_Define.ClassEnumToType[(Character_Define.SystemClassType)soul.class_type] : "";
                        });
                        soullist2.DataSource = soulList2;
                        soullist2.DataBind();
                        queryFetcher.GM_Render(Result_Define.eResult.SUCCESS);
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
            }
        }
    }
}