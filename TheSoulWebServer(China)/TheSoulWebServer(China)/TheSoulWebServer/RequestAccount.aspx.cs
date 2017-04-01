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
using ServiceStack.Text;
using logWeb;
using System.IO;
using System.Text;
 

namespace TheSoulWebServer
{
    public partial class RequestAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] ops = new string[] {
                "createaccount",
                "createcharacter",
                "login",

                "create_new_character",
                "refresh_account",
                "change_character_group",

                // chat method
                "chat_ignore_list",
                "chat_ignore_add",
                "chat_ignore_remove",

                // for test method only work in debug mode
                "create_copy_character",
                "logout",
                "getexp",
                "tutorial_reset",
                "create_test_character",
            };


            WebQueryParam queryFetcher = new WebQueryParam();
            string retJson = "";
            TxnBlock tb = new TxnBlock();
            {
                long AID = 0;
                try
                {
                    queryFetcher.TxnBlockInit(ref tb, ref AID);

                    string requestOp = queryFetcher.QueryParam_Fetch("op");
                    JsonObject json = new JsonObject();

                    if (queryFetcher.ReRequestFlag)
                    {
                        retJson = queryFetcher.ReRequestRender();
                    }
                    else if (Array.IndexOf(ops, requestOp) >= 0)
                    {
                        queryFetcher.operation = requestOp;
                        Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;

                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.op], requestOp);
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], AID);

                        if (requestOp.Equals("createaccount"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long server_group_id = TheSoulDBcon.GetInstance().TheSoulGlobalDBInit(ref tb);

                            string userID = queryFetcher.QueryParam_Fetch("userid");
                            string userName = queryFetcher.QueryParam_Fetch("username");
                            string CountryCode = queryFetcher.QueryParam_Fetch("cc", "cn");
                            Global_Define.ePlatformType platform_type = (Global_Define.ePlatformType)queryFetcher.QueryParam_FetchInt("platform_type", (int)Global_Define.ePlatformType.EPlatformType_SnailSDK);
                            int LanguageCode = queryFetcher.QueryParam_FetchInt("languagecode");

                            if (LanguageCode < 1)
                            {
                                switch (CountryCode.ToLower())
                                {
                                    case "kr":
                                        LanguageCode = (int)DataManager_Define.eCountryCode.Korea; // 한국어
                                        break;
                                    case "jp":
                                        LanguageCode = (int)DataManager_Define.eCountryCode.Japan; // 일본어
                                        break;
                                    case "cn":
                                        LanguageCode = (int)DataManager_Define.eCountryCode.China; // 중국어
                                        break;
                                    default:
                                        LanguageCode = (int)DataManager_Define.eCountryCode.English; // 영어
                                        break;
                                }
                            }

                            if (!(platform_type == Global_Define.ePlatformType.EPlatformType_SnailSDK || platform_type == Global_Define.ePlatformType.EPlatformType_UnityEditer))
                                userID = string.Format("{0}_type_{1}", userID, (int)platform_type);

                            //bool ignoreLength = queryFetcher.QueryParam_FetchByte("Debug") > 0;
                            //retError = userName.Length > Account_Define.Max_UserName_Length && !ignoreLength ? Result_Define.eResult.ACCOUNT_LENGTH_OVER : Result_Define.eResult.SUCCESS;
                            retError = Result_Define.eResult.SUCCESS;
                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = AccountManager.CreateAccountCommon(ref tb, AID, userID, userName);

                            if (retError == Result_Define.eResult.ACCOUNT_ID_ALREAD_CREATED)
                            {
                                if (CharacterManager.GetCharacterList(ref tb, AID).Count > 0)
                                    retError = Result_Define.eResult.ACCOUNT_ID_LOGIN_TO_PLAY;
                                else
                                    retError = Result_Define.eResult.ACCOUNT_ID_ALREAD_CREATED_BUT_NEED_CHARACTER_CREATE;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = AccountManager.RegistAccountGlobal(ref tb, AID, userName, server_group_id);

                            if (retError == Result_Define.eResult.SUCCESS || retError == Result_Define.eResult.ACCOUNT_ID_GLOBAL_REGIST_ALREADY)
                                retError = AccountManager.CreateAccountSharding(ref tb, AID, userName, CountryCode, LanguageCode);

                            //else if (retError == Result_Define.eResult.ACCOUNT_ID_GLOBAL_REGIST_ALREADY)
                            //    retError = Result_Define.eResult.SUCCESS;       // ignore regist error for smooth play
                        }
                        else if (requestOp.Equals("createcharacter") || requestOp.Equals("create_new_character"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long server_group_id = TheSoulDBcon.GetInstance().TheSoulGlobalDBInit(ref tb);
                            short setClass = queryFetcher.QueryParam_FetchByte("class");
                            long CID = 0;

                            List<Character> CharacterList = CharacterManager.FlushCharacterList(ref tb, AID);
                            if (requestOp.Equals("createcharacter") && CharacterList.Count > 0)
                                retError = Result_Define.eResult.CHARACTER_ID_ALREAD_CREATED;
                            else if (requestOp.Equals("create_new_character"))
                            {
                                foreach (Character setChar in CharacterList)
                                {
                                    if (setChar.Class == setClass)
                                    {
                                        retError = Result_Define.eResult.DUPLICATE_CHARACTER_CLASS;
                                        break;
                                    }
                                    else
                                        retError = Result_Define.eResult.SUCCESS;
                                }
                            }
                            else
                                retError = Result_Define.eResult.SUCCESS;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                if (setClass < (short)Character_Define.SystemClassType.Class_First || setClass > (short)Character_Define.SystemClassType.Class_Last)
                                    retError = Result_Define.eResult.CHARACTER_CLASS_INVALIDE;
                            }

                            //int checkLevel = 0;
                            if (requestOp.Equals("create_new_character") && retError == Result_Define.eResult.SUCCESS)
                            {
                                // not use yet
                                //int makeLevel = SystemData.GetConstValueInt(ref tb, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.DEF_2ndCLASS_RESTRICTION_LEVEL]);
                                //checkLevel = CharacterList.Count < 2 ?  makeLevel :
                                //                                        CharacterList.Count < 3 ? SystemData.GetConstValueInt(ref tb, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.DEF_3ndCLASS_RESTRICTION_LEVEL]) :
                                //                                        Account_Define.Max_Characeter_Level_Limit;

                                //bool bVipCheck = setClass == (short)Character_Define.SystemClassType.Class_Warrior ? VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.UNLOCK_CHARACTER_1ST) :
                                //                    setClass == (short)Character_Define.SystemClassType.Class_Swordmaster ? VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.UNLOCK_CHARACTER_2ND) :
                                //                    setClass == (short)Character_Define.SystemClassType.Class_Taoist ? VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.UNLOCK_CHARACTER_3RD) : false;

                                //if (!(checkLevel <= CharacterManager.GetCharacterMaxLevel(ref tb, AID) || bVipCheck))
                                //    retError = Result_Define.eResult.NO_BUYCHARACTER_LEVEL;                                
                                //if (!(checkLevel <= CharacterManager.GetCharacterMaxLevel(ref tb, AID)))
                                //    retError = Result_Define.eResult.NO_BUYCHARACTER_LEVEL;

                                //checkLevel = makeLevel;

                                int makeSoulCount = SystemData.GetConstValueInt(ref tb, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.DEF_2ndCLASS_RESTRICTION_SOUL_COUNT]);
                                int userSoulCount = SoulManager.GetUser_ActiveSoul(ref tb, AID).Count(findSoul => findSoul.grade > 0 && findSoul.level > 0 && findSoul.starlevel > 0);

                                retError = makeSoulCount <= userSoulCount ? Result_Define.eResult.SUCCESS : Result_Define.eResult.NO_BUYCHARACTER_LEVEL;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = CharacterManager.RegistCharacterGlobal(ref tb, ref CID, AID, server_group_id, requestOp.Equals("create_new_character"));

                            if (retError == Result_Define.eResult.SUCCESS || retError == Result_Define.eResult.CHARACTER_ID_GLOBAL_REGIST_ALREADY)
                                retError = CharacterManager.CreateCharacterSharding(ref tb, AID, CID, setClass);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                //System_Character_EXP setExp = CharacterManager.GetSystemExp(ref tb, checkLevel);
                                retError = CharacterManager.UpdateCharacterInfo(ref tb, CID, AID, 0, 0);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                System_PC_BASE PcBaseInfo = null;
                                PcBaseInfo = CharacterManager.GetPCbaseInfo(ref tb, System.Convert.ToInt32(setClass));

                                if (PcBaseInfo != null)
                                {
                                    CharacterList = CharacterManager.FlushCharacterList(ref tb, AID);

                                    List<User_Inven> setMakeItem = new List<User_Inven>();

                                    setMakeItem.Add(new User_Inven(PcBaseInfo.Base_Weapon, 1, 2, 0));
                                    setMakeItem.Add(new User_Inven(PcBaseInfo.Base_Armor, 1));
                                    setMakeItem.Add(new User_Inven(PcBaseInfo.Base_Helmet, 1));
                                    setMakeItem.Add(new User_Inven(PcBaseInfo.Base_Glove, 1));
                                    setMakeItem.Add(new User_Inven(PcBaseInfo.Base_Shoes, 1));
                                    List<User_Inven> setEquipItem = new List<User_Inven>();

                                    retError = ItemManager.MakeEquipArray(ref tb, ref setEquipItem, AID, setMakeItem, CID);

                                    if (setEquipItem.Count < 0 || retError != Result_Define.eResult.SUCCESS)
                                        retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

                                    setEquipItem.ForEach(item => { ItemManager.EquipItemToCharacter(ref tb, AID, item.invenseq, CID); });
                                }
                                else
                                    retError = Result_Define.eResult.CHARACTER_ID_GLOBAL_REGIST_FAIL;
                            }


                            // vip reward send
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<int> getRewardList = new List<int>();
                                User_Event_Check_Data userEventData = TriggerManager.GetUser_Event_Check_Data(ref tb, AID);
                                if (string.IsNullOrEmpty(userEventData.VIPRewardList))
                                    userEventData.VIPRewardList = "[]";
                                getRewardList = mJsonSerializer.JsonToObject<List<int>>(userEventData.VIPRewardList);
                                foreach (int getRewardLevel in getRewardList)
                                {
                                    List<System_VIP_RewardBox> systemRewardList = VipManager.GetSystem_VIP_RewardList(ref tb, getRewardLevel);
                                    Dictionary<short, List<Set_Mail_Item>> sendMailList = new Dictionary<short, List<Set_Mail_Item>>();

                                    systemRewardList.ForEach(setItem =>
                                    {
                                        if (!sendMailList.ContainsKey(setItem.ClassType))
                                            sendMailList[setItem.ClassType] = new List<Set_Mail_Item>();

                                        sendMailList[setItem.ClassType].Add(new Set_Mail_Item(setItem.Item_ID, setItem.Num, setItem.Grade, setItem.Level));
                                    }
                                    );

                                    if (sendMailList.ContainsKey(setClass))
                                    {
                                        int sendItemCount = 0;
                                        List<Set_Mail_Item> setMailItem = new List<Set_Mail_Item>();

                                        string mailTitle = VipManager.GetVIPRewardMailTitle(getRewardLevel, setClass);
                                        string mailBody = VipManager.GetVIPRewardMailBody(getRewardLevel, setClass);

                                        foreach (Set_Mail_Item setItem in sendMailList[setClass])
                                        {
                                            if (setItem.itemid > 0 && setItem.itemea > 0)
                                            {
                                                sendItemCount++;
                                                setMailItem.Add(setItem);
                                            }

                                            if (sendItemCount >= Mail_Define.Mail_MaxItem)
                                            {
                                                retError = MailManager.SendMail(ref tb, ref setMailItem, AID, 0, "", mailTitle, mailBody, Mail_Define.Mail_VIP_CloseTime_Min);

                                                if (retError == Result_Define.eResult.SUCCESS)
                                                {
                                                    setMailItem.Clear();
                                                    sendItemCount = 0;
                                                }
                                                else
                                                    break;
                                            }
                                        }

                                        if (retError == Result_Define.eResult.SUCCESS && sendItemCount > 0)
                                        {
                                            retError = MailManager.SendMail(ref tb, ref setMailItem, AID, 0, "", mailTitle, mailBody, Mail_Define.Mail_VIP_CloseTime_Min);
                                        }
                                    }
                                }
                            }

                            //if (retError == Result_Define.eResult.SUCCESS)
                            //{
                            //    User_CharacterGroup setCharacterGroup = CharacterManager.GetCharacterGroupInfo(ref tb, AID, CID);
                            //    setCharacterGroup.cid1 = CID;
                            //    retError = AccountManager.UpdateEquipCID(ref tb, AID, setCharacterGroup.cid1);

                            //    if (retError == Result_Define.eResult.SUCCESS)
                            //        retError = CharacterManager.UpdateCharacterGroup(ref tb, AID, setCharacterGroup);
                            //}

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                CharacterManager.FlushCharacterList(ref tb, AID);
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.CID], mJsonSerializer.ToJsonString(CID));
                            }
                        }
                        else if (requestOp.Equals("login") || requestOp.Equals("refresh_account"))
                        {
                            Global_Define.ePlatformType platform_type = (Global_Define.ePlatformType)queryFetcher.QueryParam_FetchInt("platform_type", (int)Global_Define.ePlatformType.EPlatformType_SnailSDK);
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            if (requestOp.Equals("login"))
                                retError = AccountManager.UpdateLastConnectionTime(ref tb, AID);
                            else
                                retError = Result_Define.eResult.SUCCESS;

                            Account userAccount = new Account();
                            if (retError == Result_Define.eResult.SUCCESS)
                                userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);

                            if (retError == Result_Define.eResult.SUCCESS && userAccount.AID < 1)
                                retError = Result_Define.eResult.ACCOUNT_ID_NOT_FOUND;

                            string user_s_mac = queryFetcher.QueryParam_Fetch("s_mac", "");
                            string user_s_os_type = queryFetcher.QueryParam_Fetch("s_os_type", "");
                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.d_logintime], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.d_logouttime], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_role_log]);
                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_mac], user_s_mac);
                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_os_type], user_s_os_type);

                            List<Character_Detail> userCharList = new List<Character_Detail>();
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                if (requestOp.Equals("login") && userAccount.PVEPlayState == 1)
                                    AccountManager.UpdatePVEFlag(ref tb, AID, false, (int)userAccount.LastWorldID, (int)userAccount.LastStageID);

                                CharacterManager.FlushCharacterList(ref tb, AID);
                                userCharList = CharacterManager.GetCharacterListWithEquip(ref tb, AID, true);

                                if (userCharList.Count < 1)
                                    retError = Result_Define.eResult.ACCOUNT_ID_ALREAD_CREATED_BUT_NEED_CHARACTER_CREATE;
                            }


                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                //List<ActiveSoulListClass> SoulActiveClass = null;
                                //List<ActiveSoul> SoulActive = SoulManager.GetActiveSoulList(ref tb, AID, userAccount.EquipCID).Values.ToList();
                                //List<PassiveSoul> SoulPassive = SoulManager.GetPassiveSoulList(ref tb, AID, userAccount.EquipCID).Values.ToList();
                                List<User_Inven> CharItem = ItemManager.GetInvenList(ref tb, AID, userAccount.EquipCID, true, true);
                                List<User_Inven> AccountInven = CharItem.Where(item => item.cid == 0).ToList();
                                List<User_Inven> CharInven = CharItem.Where(item => item.cid > 0).ToList();
                                //List<User_Orb_Inven> OrbInven = ItemManager.GetUserOrbInvenList(ref tb, AID, true);
                                ItemManager.RemoveUltimateOrbCache(AID, 0);
                                //foreach (Character setItem in userCharList)
                                //{
                                //    SoulActiveClass = SoulManager.GetActiveSoulClassList(ref tb, AID, setItem.cid).Values.ToList();
                                //}

                                Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userAccount, userCharList.Count);

                                string limitview = "";
                                AccountManager.CalcLimitBuy(ref tb, ref limitview);
                                retAccount.limitbuyitemview = limitview;
                                //retAccount.passivesoulpoint = userAccount.PassiveSoulPoint;
                                retAccount.laststageid = System.Convert.ToInt32(userAccount.LastStageID);

                                User_CharacterGroup setCharacterGroup = CharacterManager.GetCharacterGroupInfo(ref tb, AID, userCharList.FirstOrDefault().cid, true);

                                List<long> retCharInfo = new List<long>();

                                if (setCharacterGroup.cid1 > 0) retCharInfo.Add(setCharacterGroup.cid1);
                                if (setCharacterGroup.cid2 > 0) retCharInfo.Add(setCharacterGroup.cid2);
                                if (setCharacterGroup.cid3 > 0) retCharInfo.Add(setCharacterGroup.cid3);

                                for (int i = retCharInfo.Count; i < Character_Define.Max_CharacterGroup; i++)
                                {
                                    retCharInfo.Add(0);
                                }

                                /*
                                
                                Ret_Daily_Event_List dailyEventList = new Ret_Daily_Event_List();
                                TriggerManager.GetDailyEvent_RewardList(ref tb, true);
                                User_Event_Check_Data userDailyInfo = new User_Event_Check_Data();
                                TriggerManager.Check_User_Daily_Event(ref tb, AID, true);
                                Ret_Event_Check_Data retDailyInfo = new Ret_Event_Check_Data();
                                new Ret_Event_Check_Data(userDailyInfo);
                                 */
                                int admin_7Day_flag = TriggerManager.Check7DayEvent(ref tb, AID, userAccount.CreationDate);
                                int admin_firstpayment_flag = SystemData.AdminConstValueFetchFromRedis(ref tb, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.ADMIN_FIRST_PAYMENT_ON_OFF]);
                                int admin_daily_flag = SystemData.AdminConstValueFetchFromRedis(ref tb, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.ADMIN_DAILY_ON_OFF]);

                                Ret_Daily_Event_List dailyEventList = new Ret_Daily_Event_List();
                                User_Event_Check_Data userDailyInfo = new User_Event_Check_Data(AID);
                                int DailyCheck_RubyPrice = 0;
                                int DailyCheck_BuyMax = 0;

                                if (admin_daily_flag > 0)
                                {
                                    dailyEventList = TriggerManager.GetDailyEvent_RewardList(ref tb);
                                    userDailyInfo = TriggerManager.Check_User_Daily_Event(ref tb, AID, true);
                                    DailyCheck_RubyPrice = SystemData.AdminConstValueFetchFromRedis(ref tb, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.DAILY_ADD_RUBY]);
                                    DailyCheck_BuyMax = SystemData.AdminConstValueFetchFromRedis(ref tb, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.DAILY_ADD_MAX]);
                                }

                                Ret_Event_Check_Data retDailyInfo = new Ret_Event_Check_Data(userDailyInfo, DailyCheck_BuyMax);

                                Dictionary<short, List<Ret_Reward_Item>> firstPayReward_item_dic = new Dictionary<short, List<Ret_Reward_Item>>();

                                if (admin_firstpayment_flag > 0)
                                {
                                    if (retDailyInfo.first_pay_flag.Equals("N"))
                                    {
                                        System_Event_First_Payment getFirstPayRewardInfo = TriggerManager.GetSystem_Event_First_Payment(ref tb);

                                        List<System_Event_Reward_Box> setOpenBoxList = new List<System_Event_Reward_Box>();
                                        List<Ret_Reward_Item> firstPayReward_item = new List<Ret_Reward_Item>();

                                        if (getFirstPayRewardInfo.Reward_Box1ID > 0)
                                            setOpenBoxList.AddRange(TriggerManager.GetSystem_Event_Reward_Box_List(ref tb, getFirstPayRewardInfo.Reward_Box1ID));

                                        foreach (System_Event_Reward_Box setBox in setOpenBoxList)
                                        {
                                            if (setBox.EventItem_ID > 0)
                                                firstPayReward_item.Add(new Ret_Reward_Item(setBox));
                                        }
                                        firstPayReward_item_dic.Add(0, firstPayReward_item);
                                    }
                                }
                                else
                                    retDailyInfo.first_pay_flag = "Y";

                                TriggerManager.RemoveEventDataFromRedis(AID);
                                TriggerManager.RemoveAchieveDataFromRedis(AID, false);
                                TriggerManager.RemoveAchieveDataFromRedis(AID, true);

                                if (requestOp.Equals("login"))
                                {
                                    List<TriggerProgressData> setDataList = new List<TriggerProgressData>();
                                    setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.CHARGE_FIXED));
                                    setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Game_Access, (int)Trigger_Define.eGameAccessType.TotalLoginCount));
                                    setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Game_Access, (int)Trigger_Define.eGameAccessType.AccumulateLoginDay));
                                    setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Game_Access, (int)Trigger_Define.eGameAccessType.CountinueLoginDay));
                                    setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Game_Access, (int)Trigger_Define.eGameAccessType.CountLoginDay));
                                    if (platform_type == Global_Define.ePlatformType.EPlatformType_Google || platform_type == Global_Define.ePlatformType.EPlatformType_Facebook)
                                        setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.ACCOUNT_REGIST));
                                    TriggerManager.ProgressTrigger(ref tb, AID, setDataList);
                                    queryFetcher.RemoveReRequestInfo();
                                }
                                //if (userDailyInfo.CheckCount > userDailyInfo.RewardCount)
                                //{
                                //    TriggerManager.EventRewardMailSend(ref tb, ref userDailyInfo, AID, userDailyInfo.CheckCount - userDailyInfo.RewardCount);
                                //    TriggerManager.Check_User_Daily_Event(ref tb, AID, true);
                                //}

                                List<ServerCreate_RankingReward> rewardList = PvPManager.GetRankRewardList(ref tb, AID);
                                if (rewardList.Count > 0)
                                    retError = PvPManager.SetPvP_RewardMail(ref tb, ref rewardList);

                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));

                                int lastClearStage = Dungeon_Manager.GetUser_Mission_LastStage(ref tb, AID);
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.PvELastStage], lastClearStage.ToString());

                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.CharacterList], Character_Detail.makeCharacter_DetailListJson(ref userCharList));
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.CharacterGroup], mJsonSerializer.ToJsonString(retCharInfo));
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.ItemInventory_Account], Ret_Inven_Item.makeInvenListJson(ref AccountInven));
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.ItemInventory_Character], Ret_Inven_Item.makeInvenListJson(ref CharInven));
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.ItemInventory_Orb], "[]");
                                //json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.ItemInventory_Orb], User_Orb_Inven.makeOrbListJson(ref OrbInven));
                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.DailyEventInfo], mJsonSerializer.ToJsonString(retDailyInfo));
                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.DailyCheckBuyRubyPrice], mJsonSerializer.ToJsonString(DailyCheck_RubyPrice));
                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.DailyCheckBuyMax], mJsonSerializer.ToJsonString(DailyCheck_BuyMax));
                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.FirstPaymentRewardItemList], mJsonSerializer.ToJsonString(firstPayReward_item_dic));
                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.DailyEventList], mJsonSerializer.ToJsonString(dailyEventList));

                                // decision this gacha contents info remove or keep (china version not use yet gacha contents. dont' need this);
                                RetGachaInfo setGachainfo = new RetGachaInfo(ref tb, ShopManager.GetUserGachaInfo(ref tb, AID), ShopManager.GetUser_Gacha_Special_Info(ref tb, AID));
                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.FreeGachaInfo], mJsonSerializer.ToJsonString(setGachainfo));

                                // user soul list
                                //if (AID == 383)
                                {
                                    List<Ret_Soul_Active_Account> allActiveSoulList = new List<Ret_Soul_Active_Account>();
                                    userCharList.ForEach(charInfo =>
                                    {
                                        Ret_Soul_Active_Account addObj = new Ret_Soul_Active_Account();
                                        addObj.cid = charInfo.cid;
                                        addObj.active_soul_list = SoulManager.GetActive_Soul_Ret_List(ref tb, AID, charInfo.cid, true);
                                        addObj.passive_soul_list = SoulManager.GetPassive_Soul_Ret_List(ref tb, AID, charInfo.cid, true);
                                        allActiveSoulList.Add(addObj);
                                    });
                                    json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.AllSoulList], mJsonSerializer.ToJsonString(allActiveSoulList));
                                }
                                //else
                                //{
                                //    List<Ret_Soul_Active> getActiveSoulList = SoulManager.GetActive_Soul_Ret_List(ref tb, AID, userAccount.EquipCID, true);
                                //    List<Ret_Soul_Passive> getPassiveSoulList = SoulManager.GetPassive_Soul_Ret_List(ref tb, AID, userAccount.EquipCID, true);
                                //    json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.ActiveSoulLIst], mJsonSerializer.ToJsonString(getActiveSoulList));
                                //    json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.PassiveSoulList], mJsonSerializer.ToJsonString(getPassiveSoulList));
                                //}
                                User_Soul_Make_Info makeCountInfo = SoulManager.GetUserSoulMakeInfo(ref tb, AID, true);
                                json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.PassiveRubyMakeCount], mJsonSerializer.ToJsonString(makeCountInfo.Passive_Make_Count));

                                // user soul equip list
                                List<User_Soul_Equip_Inven> getSoulEquipList = SoulManager.GetSoul_Equip_Ret_List(ref tb, AID, true);
                                json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.SoulEquipItemList], mJsonSerializer.ToJsonString(getSoulEquipList));

                                // system cash shop item list
                                Shop_Define.eBillingType BillingType = (Shop_Define.eBillingType)queryFetcher.QueryParam_FetchInt("billing_type");
                                int shopRemainTime = 0;
                                List<RetShopItem> shopItemList = ShopManager.GetUser_ShopList(ref tb, AID, Shop_Define.eShopType.Cash, BillingType, ref shopRemainTime, ref retError);
                                json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.ShopItemList], mJsonSerializer.ToJsonString(shopItemList));

                                // user tutorial info
                                Tutorial_Step userTutorialStep = AccountManager.Get_User_Tutorial(ref tb, AID);
                                if (userTutorialStep.forced_tutorial == 8)
                                    userTutorialStep.forced_tutorial = 0;
                                else if (userTutorialStep.forced_tutorial == 76)
                                    userTutorialStep.forced_tutorial = 66;

                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.TutorialStepInfo], mJsonSerializer.ToJsonString(userTutorialStep));

                                // user 7day event info
                                int loginCount = 0;
                                retError = AccountManager.GetUserLoginCount(ref tb, AID, out loginCount);
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Event_7Day_Flag], mJsonSerializer.ToJsonString(admin_7Day_flag));
                                json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.VIPRewardList], userDailyInfo.VIPRewardList);
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.User_Login_Day_Count], loginCount.ToString());

                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.ServerTimeString], GenericFetch.GetServerTimeString());

                                // set encrypt key
                                json = mJsonSerializer.AddJson(json, AccountManager.Ret_EncryptKey, "1");
                            }
                        }
                        else if (requestOp.Equals("change_character_group"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            User_CharacterGroup setCharacterGroup = new User_CharacterGroup();
                            setCharacterGroup.cid1 = queryFetcher.QueryParam_FetchLong("cid1");
                            setCharacterGroup.cid2 = queryFetcher.QueryParam_FetchLong("cid2");
                            setCharacterGroup.cid3 = queryFetcher.QueryParam_FetchLong("cid3");

                            if (setCharacterGroup.cid1 == 0)
                                retError = Result_Define.eResult.CHARACTER_NOT_FOUND;
                            else
                                retError = Result_Define.eResult.SUCCESS;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                if (setCharacterGroup.cid1 == setCharacterGroup.cid2 && setCharacterGroup.cid2 > 0)
                                    retError = Result_Define.eResult.CHARACTER_ID_DUPLICATE_IN_GROUP;

                                if (setCharacterGroup.cid1 == setCharacterGroup.cid3 && setCharacterGroup.cid3 > 0)
                                    retError = Result_Define.eResult.CHARACTER_ID_DUPLICATE_IN_GROUP;

                                if (setCharacterGroup.cid2 == setCharacterGroup.cid3 && setCharacterGroup.cid2 > 0 && setCharacterGroup.cid3 > 0)
                                    retError = Result_Define.eResult.CHARACTER_ID_DUPLICATE_IN_GROUP;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<Character> userCharList = CharacterManager.FlushCharacterList(ref tb, AID);

                                if (userCharList.Find(charinfo => charinfo.cid == setCharacterGroup.cid1) == null && setCharacterGroup.cid1 > 0) retError = Result_Define.eResult.CHARACTER_NOT_FOUND;
                                if (userCharList.Find(charinfo => charinfo.cid == setCharacterGroup.cid2) == null && setCharacterGroup.cid2 > 0) retError = Result_Define.eResult.CHARACTER_NOT_FOUND;
                                if (userCharList.Find(charinfo => charinfo.cid == setCharacterGroup.cid3) == null && setCharacterGroup.cid3 > 0) retError = Result_Define.eResult.CHARACTER_NOT_FOUND;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = AccountManager.UpdateEquipCID(ref tb, AID, setCharacterGroup.cid1);

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = CharacterManager.UpdateCharacterGroup(ref tb, AID, setCharacterGroup);

                            //List<Character_Simple> retCharInfo = new List<Character_Simple>();

                            List<long> retCharInfo = new List<long>();

                            if (setCharacterGroup.cid1 > 0) retCharInfo.Add(setCharacterGroup.cid1);
                            if (setCharacterGroup.cid2 > 0) retCharInfo.Add(setCharacterGroup.cid2);
                            if (setCharacterGroup.cid3 > 0) retCharInfo.Add(setCharacterGroup.cid3);

                            for (int i = retCharInfo.Count; i < Character_Define.Max_CharacterGroup; i++)
                            {
                                retCharInfo.Add(0);
                            }

                            //if (retError == Result_Define.eResult.SUCCESS)
                            //{
                            //    userCharList = CharacterManager.FlushCharacterList(ref tb, AID);
                            //    if (userCharList.ContainsKey(setCharacterGroup.cid1) && setCharacterGroup.cid1 > 0) retCharInfo.Add(new Character_Simple(userCharList[setCharacterGroup.cid1])); ;
                            //    if (userCharList.ContainsKey(setCharacterGroup.cid1) && setCharacterGroup.cid2 > 0) retCharInfo.Add(new Character_Simple(userCharList[setCharacterGroup.cid2])); ;
                            //    if (userCharList.ContainsKey(setCharacterGroup.cid1) && setCharacterGroup.cid3 > 0) retCharInfo.Add(new Character_Simple(userCharList[setCharacterGroup.cid3])); ;
                            //}

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                //userCharList = CharacterManager.FlushCharacterList(ref tb, AID);
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.CharacterGroup], mJsonSerializer.ToJsonString(retCharInfo));
                            }
                        }
                        else if (requestOp.Equals("chat_ignore_list"))
                        {
                            retError = Result_Define.eResult.SUCCESS;

                            List<ChatIgnore> ignoreList = AccountManager.GetUserIgnoreList(ref tb, AID);
                            json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.ChatIgnoreList], mJsonSerializer.ToJsonString(ignoreList));
                        }
                        else if (requestOp.Equals("chat_ignore_add"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long ignoreAID = queryFetcher.QueryParam_FetchLong("ignore_aid");

                            if (AID == ignoreAID)
                                retError = Result_Define.eResult.CHAT_IGNORE_CAN_NOT_SELF;
                            else
                                retError = AccountManager.AddUserIgnoreList(ref tb, AID, ignoreAID);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<ChatIgnore> ignoreList = AccountManager.GetUserIgnoreList(ref tb, AID, true);
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.ChatIgnoreList], mJsonSerializer.ToJsonString(ignoreList));
                            }
                        }
                        else if (requestOp.Equals("chat_ignore_remove"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long ignoreAID = queryFetcher.QueryParam_FetchLong("ignore_aid");

                            retError = AccountManager.RemoveUserIgnoreList(ref tb, AID, ignoreAID);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<ChatIgnore> ignoreList = AccountManager.GetUserIgnoreList(ref tb, AID, true);
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.ChatIgnoreList], mJsonSerializer.ToJsonString(ignoreList));
                            }
                        }
                        else if (requestOp.Equals("logout"))
                        {
                        }
#if DEBUG
                        // for Debug
                        else if (requestOp.Equals("create_copy_character"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long server_group_id = TheSoulDBcon.GetInstance().TheSoulGlobalDBInit(ref tb);
                            long targetAID = queryFetcher.QueryParam_FetchLong("from_aid");
                            if (targetAID > 0)
                            {
                                long CID = 0;

                                List<Character> CharacterList = CharacterManager.FlushCharacterList(ref tb, targetAID);
                                List<Character> MyCharacterList = CharacterManager.FlushCharacterList(ref tb, AID);
                                retError = Result_Define.eResult.SUCCESS;
                                int curExp = 0;
                                int checkLevel = CharacterList.FirstOrDefault().level;
                                
                                checkLevel = checkLevel > Account_Define.Max_Characeter_Level_Limit ? Account_Define.Max_Characeter_Level_Limit :
                                                            (checkLevel < 10 ? 10 : checkLevel);

                                foreach (Character setCharacter in CharacterList)
                                {
                                    var checkClass = MyCharacterList.Find(charinfo => charinfo.Class == setCharacter.Class);
                                    if (checkClass == null)
                                    {
                                        if (retError == Result_Define.eResult.SUCCESS)
                                            retError = CharacterManager.RegistCharacterGlobal(ref tb, ref CID, AID, server_group_id, true);

                                        if (retError == Result_Define.eResult.SUCCESS || retError == Result_Define.eResult.CHARACTER_ID_GLOBAL_REGIST_ALREADY)
                                            retError = CharacterManager.CreateCharacterSharding(ref tb, AID, CID, (short)setCharacter.Class);
                                    }
                                    else
                                        CID = checkClass.cid;

                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        System_Character_EXP setExp = CharacterManager.GetSystemExp(ref tb, checkLevel);
                                        int addExp = setExp.ACC_exp < curExp ? 0 : setExp.ACC_exp - curExp;
                                        retError = CharacterManager.UpdateCharacterInfo(ref tb, CID, AID, addExp, 0);
                                    }

                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        List<User_Inven> equipList = ItemManager.GetEquipList(ref tb, setCharacter.aid, setCharacter.cid);
                                        List<User_Inven> setMakeItem = new List<User_Inven>();
                                        equipList.ForEach(setItem =>
                                        {
                                            setMakeItem.Add(new User_Inven(setItem.itemid, 1, setItem.grade, setItem.level));
                                        }
                                        );

                                        List<User_Inven> setEquipItem = new List<User_Inven>();
                                        retError = ItemManager.MakeEquipArray(ref tb, ref setEquipItem, AID, setMakeItem, CID);
                                        setEquipItem.ForEach(item => { ItemManager.EquipItemToCharacter(ref tb, AID, item.invenseq, CID); });
                                    }
                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        CharacterManager.FlushCharacterList(ref tb, AID);
                                        //json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.CID], mJsonSerializer.ToJsonString(CID));
                                    }
                                }

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    List<User_ActiveSoul> SetSoulList = SoulManager.GetUser_ActiveSoul(ref tb, targetAID);
                                    foreach (User_ActiveSoul setSoul in SetSoulList)
                                    {
                                        retError = DummyManager.MakeActiveSoul(ref tb, AID, setSoul.soulid, setSoul.soulgroup, setSoul.grade, setSoul.level, setSoul.starlevel);
                                    }
                                }

                                if (retError == Result_Define.eResult.SUCCESS)
                                    AccountManager.End_User_Tutorial(ref tb, AID, true);
                            }
                            
                            //retError = Result_Define.eResult.DB_ERROR;
                        }
                        else if (Request.Params.AllKeys.Contains("Debug"))
                        {
                            retError = Result_Define.eResult.SUCCESS;
                            if (requestOp.Equals("getexp"))
                            {
                                long CID = queryFetcher.QueryParam_FetchLong("cid");
                                int getExp = queryFetcher.QueryParam_FetchInt("exp");

                                retError = CharacterManager.UpdateCharacterInfo(ref tb, CID, AID, getExp, 0);
                            }
                            else if (requestOp.Equals("tutorial_reset"))
                            {
                                Tutorial_Step setStepInfo = new Tutorial_Step();
                                retError = AccountManager.Set_User_Tutorial(ref tb, AID, setStepInfo);
                                retError = AccountManager.End_User_Tutorial(ref tb, AID, false);
                            }
                            else if (requestOp.Equals("create_test_character"))
                            {
                                tb.IsoLevel = IsolationLevel.ReadCommitted;
                                long server_group_id = TheSoulDBcon.GetInstance().TheSoulGlobalDBInit(ref tb);
                                byte setClass = queryFetcher.QueryParam_FetchByte("class");
                                int checkLevel = queryFetcher.QueryParam_FetchInt("level", 1);
                                long CID = 0;
                                int curExp = 0;

                                List<Character> CharacterList = CharacterManager.FlushCharacterList(ref tb, AID);
                                retError = Result_Define.eResult.SUCCESS;
                                Response.Write("start<br>");
                                foreach (Character setChar in CharacterList)
                                {
                                    if (setChar.Class == setClass)
                                    {
                                        retError = Result_Define.eResult.DUPLICATE_CHARACTER_CLASS;
                                        break;
                                    }
                                    curExp = curExp < setChar.totalexp ? setChar.totalexp : curExp;
                                }

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    if (setClass < (short)Character_Define.SystemClassType.Class_First || setClass > (short)Character_Define.SystemClassType.Class_Last)
                                        retError = Result_Define.eResult.CHARACTER_CLASS_INVALIDE;
                                }

                                checkLevel = checkLevel > Account_Define.Max_Characeter_Level_Limit ? Account_Define.Max_Characeter_Level_Limit :
                                                            (checkLevel < 10 ? 10 : checkLevel);
                                Response.Write("check<br>");
                                
                                if (retError == Result_Define.eResult.SUCCESS)
                                    retError = CharacterManager.RegistCharacterGlobal(ref tb, ref CID, AID, server_group_id, true);

                                if (retError == Result_Define.eResult.SUCCESS || retError == Result_Define.eResult.CHARACTER_ID_GLOBAL_REGIST_ALREADY)
                                    retError = CharacterManager.CreateCharacterSharding(ref tb, AID, CID, setClass);

                                Response.Write("create<br>");
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    System_Character_EXP setExp = CharacterManager.GetSystemExp(ref tb, checkLevel);
                                    int addExp = setExp.ACC_exp < curExp ? 0 : setExp.ACC_exp - curExp;
                                    retError = CharacterManager.UpdateCharacterInfo(ref tb, CID, AID, addExp, 0);
                                }
                                Response.Write("levelup<br>");
                                
                                long dummyCharID = queryFetcher.QueryParam_FetchLong("dummy_id", 0);

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    Character charInfo = dummyCharID > 0 ? DummyManager.GetCharacterByCID(ref tb, dummyCharID) : DummyManager.GetCharacterByLevel(ref tb, checkLevel, setClass);
                                    if (charInfo.cid > 0)
                                        dummyCharID = charInfo.cid;
                                    List<User_Inven> equipList = DummyManager.GetEquipList(ref tb, charInfo.aid, charInfo.cid);
                                    List<User_Inven> setMakeItem = new List<User_Inven>();
                                    equipList.ForEach(setItem =>
                                    {
                                        setMakeItem.Add(new User_Inven(setItem.itemid, 1, setItem.grade, setItem.level));
                                    }
                                    );

                                    List<User_Inven> setEquipItem = new List<User_Inven>();
                                    retError = ItemManager.MakeEquipArray(ref tb, ref setEquipItem, AID, setMakeItem, CID);
                                    setEquipItem.ForEach(item => { ItemManager.EquipItemToCharacter(ref tb, AID, item.invenseq, CID); });

                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        List<System_Soul_Active> soulList = DummyManager.GetSoul_System_Soul_Active_Random(ref tb, TheSoul.DataManager.Math.GetRandomInt(1, 4));
                                        foreach (System_Soul_Active setSoul in soulList)
                                        {
                                            retError = DummyManager.MakeActiveSoul(ref tb, AID, setSoul.SoulID, setSoul.SoulGroup, setSoul.Grade, checkLevel, TheSoul.DataManager.Math.GetRandomInt(1, 4));
                                        }
                                    }

                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        List<SoulEquipSlot> EquipList = new List<SoulEquipSlot>();
                                        List<User_ActiveSoul> SoulList = SoulManager.GetUser_ActiveSoul(ref tb, AID, true);
                                        List<User_Character_Equip_Soul> updateEquipSoulList = new List<User_Character_Equip_Soul>();
                                        List<User_ActiveSoul> setSoulGroupList = new List<User_ActiveSoul>();

                                        short setSlot = 0;
                                        foreach (User_ActiveSoul setSoul in SoulList)
                                        {
                                            var checkSoul = setSoulGroupList.FindAll(item => item.soulgroup == setSoul.soulgroup);
                                            if (checkSoul.Count == 0)
                                            {
                                                setSoulGroupList.Add(setSoul);
                                                setSlot++;
                                                SoulEquipSlot setObj = new SoulEquipSlot();
                                                setObj.slotnum = setSlot;
                                                setObj.soulseq = setSoul.soulseq;
                                                EquipList.Add(setObj);

                                                if (setSlot >= 4)
                                                    break;
                                            }
                                        }

                                        retError = SoulManager.EquipActiveSoulToCharacter(ref tb, AID, CID, ref EquipList, ref updateEquipSoulList, false);
                                    }
                                }
                                json = mJsonSerializer.AddJson(json, "DUMMY_CID", mJsonSerializer.ToJsonString(dummyCharID));
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    CharacterManager.FlushCharacterList(ref tb, AID);
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.CID], mJsonSerializer.ToJsonString(CID));
                                }
                            }
                        }
#endif
                        retJson = queryFetcher.Render(json.ToJson(), retError);
                    }
                    else
                    {
                        retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(DefineError.System_Unknown_Operation), Result_Define.eResult.System_Unknown_Operation);
                    }
                }
                catch (Exception errorEx)
                {
                    string error = "";
#if DEBUG
                    error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
#else
                    if (queryFetcher.SetDebugMode)
                        error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
#endif
                    error = mJsonSerializer.AddJson(error, "Message", mJsonSerializer.ToJsonString(errorEx.Message));
                    retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(error), Result_Define.eResult.System_Exception);
                }
                finally
                {
                    //if (AID > 0)
                    //    queryFetcher.CheckSnail_ID(ref tb, AID);
                    queryFetcher.SetShowLogMode = tb.EndTransaction(queryFetcher.Render_errorFlag);
                    queryFetcher.ErrorLogWrite(retJson, ref tb);
                    tb.Dispose();
                }
            }
        }
    }
}


