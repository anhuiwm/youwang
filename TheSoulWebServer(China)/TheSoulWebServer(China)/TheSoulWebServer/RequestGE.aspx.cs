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
using TheSoulWebServer.Tools;
using ServiceStack.Text;

namespace TheSoulWebServer
{
    public partial class RequestGE : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] ops = new string[] {
                "lobby",                // 황금원정단 로비 : 나의 황금원정단, 상대방 매칭 정보
                "stage",                // 황금원정단 스테이지
                "resetge",             // 원정단 리셋 (다시 하기)
                "playstart",            // 원정단 시작,진입
                "playresult",           // 원정단 결과,보상
                "set_ge_group",           // 원정단 캐릭터 순서 

                // not use yet : last change 2015-12-07
                //"registerlist",         // 길드 용병 등록 정보 표시
                //"mercenarylist",        // 길드 용병 고용 정보 표시
                //"mercenaryregister",    // 길드 용병 등록            
                //"mercenaryemploy",      // 길드 용병 고용
                //"mercenarycall",        // 길드 용병 불러오기

                "shoplist",             // 원정단 상점 정보 표시
                "shopreset",            // 원정단 상점 초기화
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

                    if (Array.IndexOf(ops, requestOp) >= 0)
                    {
                        Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                        queryFetcher.operation = requestOp;
                        long CID = queryFetcher.QueryParam_FetchLong("cid");
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.op], requestOp);
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], AID);

                        if (queryFetcher.ReRequestFlag)
                        {
                            retJson = queryFetcher.ReRequestRender();
                        }
                        else if (requestOp.Equals("lobby") || requestOp.Equals("resetge"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            bool bReset = requestOp.Equals("resetge");
                            User_GE_Stage_Info userGEInfo = GoldExpedition_Manager.GetUser_GE_Stage_Info(ref tb, AID);

                            if (userGEInfo.AID != AID)
                                retError = Result_Define.eResult.GE_USER_STAGE_INFO_NOT_FOUND;
                            else
                                retError = Result_Define.eResult.SUCCESS;

                            //if (retError == Result_Define.eResult.SUCCESS && bReset && !(AID == 103 || AID == 933))
                            if (retError == Result_Define.eResult.SUCCESS && bReset)
                            {
                                //if (userGEInfo.ResetCount < GoldExpedition_Define.DayResetCount)
                                if (VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.DUNGEONCOUNT_RESET_EXPEDITION, userGEInfo.ResetCount))
                                    userGEInfo = GoldExpedition_Manager.GetUser_GE_Stage_Info(ref tb, AID, bReset);
                                else
                                    retError = Result_Define.eResult.GE_RESET_COUNT_OVER;
                            }

                            List<User_GE_Stage_Enemy> getEnemeyList = null;
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                getEnemeyList = GoldExpedition_Manager.GetUser_GE_Stage_Enemy(ref tb, AID).OrderBy(stage => stage.Stage).ToList();
                                if (getEnemeyList.Count < GoldExpedition_Define.GetEnemyRangeList.Sum(item => item.getCount) || bReset)
                                {
                                    retError = GoldExpedition_Manager.MakeMatchingEnemyInfo(ref tb, AID);
                                    //retError = Result_Define.eResult.GE_ENEMY_COUNT_NOT_ENOUGH;
                                    getEnemeyList = GoldExpedition_Manager.GetUser_GE_Stage_Enemy(ref tb, AID, true);
                                }
                            }

                            if (retError == Result_Define.eResult.SUCCESS && bReset)
                            {
                                User_GE_CharacterGroup setGroup = new User_GE_CharacterGroup();
                                setGroup.aid = AID;
                                userGEInfo.RegDate = DateTime.Now;
                                userGEInfo.ResetCount++;
                                retError = GoldExpedition_Manager.SetUser_GE_CharacterGroupInfoToDB(ref tb, AID, setGroup);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<User_Character_HP_Info> dbcharInfo = mJsonSerializer.JsonToObject<List<User_Character_HP_Info>>(userGEInfo.MyCharacter_Info_Json);
                                List<Character_Detail_With_HP> userCharList = CharacterManager.GetCharacterListWithEquip_HP(ref tb, AID);

                                if (dbcharInfo.Count != userCharList.Count)
                                {
                                    foreach (Character_Detail_With_HP chk_charInfo in userCharList)
                                    {
                                        if (dbcharInfo.Find(charinfo => charinfo.cid == chk_charInfo.cid) == null)
                                        {
                                            dbcharInfo = GoldExpedition_Manager.ReCalcUser_GE_Stage_Info(ref tb, AID, chk_charInfo.cid, ref userGEInfo);
                                        }
                                    }
                                    bReset = true;
                                    userGEInfo.MyCharacter_Info_Json = mJsonSerializer.ToJsonString(dbcharInfo);
                                }

                                //if (bReset)
                                //{
                                //    if (AID == 103 || AID == 933)
                                //        userGEInfo.ResetCount = 0;
                                //    retError = GoldExpedition_Manager.SetUser_GE_Stage_InfoToDB(ref tb, AID, userGEInfo);
                                //}
                                if (bReset)
                                    retError = GoldExpedition_Manager.SetUser_GE_Stage_InfoToDB(ref tb, AID, userGEInfo);

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    List<User_Character_HP_Info> currentHPInfo = new List<User_Character_HP_Info>();
                                    userCharList.ForEach(charinfo =>
                                    {
                                        User_Character_HP_Info setObj = new User_Character_HP_Info(charinfo);
                                        setObj.curhp = dbcharInfo.Find(item => item.cid == charinfo.cid).curhp;
                                        currentHPInfo.Add(setObj);
                                    });

                                    //User_Character_HP_Info allyInfo = mJsonSerializer.JsonToObject<User_Character_HP_Info>(userGEInfo.AllyCharacter_Info_Json);
                                    //if (allyInfo == null)
                                    //    allyInfo = new User_Character_HP_Info();
                                    List<Ret_GE_StageInfo> stageInfo = Ret_GE_StageInfo.makeStageInfoJson(ref getEnemeyList);
                                    User_GE_CharacterGroup userGEGroup = GoldExpedition_Manager.GetGECharacterGroupInfo(ref tb, AID);

                                    json = mJsonSerializer.AddJson(json, GoldExpedition_Define.GE_Ret_KeyList[GoldExpedition_Define.eGEReturnKeys.Clear_Stage], mJsonSerializer.ToJsonString(userGEInfo.Clear_Stage));
                                    json = mJsonSerializer.AddJson(json, GoldExpedition_Define.GE_Ret_KeyList[GoldExpedition_Define.eGEReturnKeys.MyCharacterInfo], mJsonSerializer.ToJsonString(currentHPInfo));
                                    //json = mJsonSerializer.AddJson(json, GoldExpedition_Define.GE_Ret_KeyList[GoldExpedition_Define.eGEReturnKeys.AllyUserName], userGEInfo.AllyUserName);
                                    //json = mJsonSerializer.AddJson(json, GoldExpedition_Define.GE_Ret_KeyList[GoldExpedition_Define.eGEReturnKeys.AllyCharacterInfo], mJsonSerializer.ToJsonString(allyInfo));
                                    json = mJsonSerializer.AddJson(json, GoldExpedition_Define.GE_Ret_KeyList[GoldExpedition_Define.eGEReturnKeys.GE_CharacterGruop], mJsonSerializer.ToJsonString(userGEGroup));
                                    json = mJsonSerializer.AddJson(json, GoldExpedition_Define.GE_Ret_KeyList[GoldExpedition_Define.eGEReturnKeys.StageInfo], mJsonSerializer.ToJsonString(stageInfo));
                                    json = mJsonSerializer.AddJson(json, GoldExpedition_Define.GE_Ret_KeyList[GoldExpedition_Define.eGEReturnKeys.ResetCount], userGEInfo.ResetCount.ToString());
                                }
                            }
                        }
                        else if (requestOp.Equals("stage"))
                        {
                            short Stage = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch("stage"));
                            User_GE_Stage_Enemy enemyInfo = GoldExpedition_Manager.GetUser_GE_Stage_Enemy(ref tb, AID, Stage);

                            List<User_Character_HP_Info> dbcharInfo = mJsonSerializer.JsonToObject<List<User_Character_HP_Info>>(enemyInfo.EnemyCharacter_Info_Json);
                            List<Character_Detail_With_HP> userCharList = mJsonSerializer.JsonToObject<List<Character_Detail_With_HP>>(enemyInfo.EnemyCharacter_Detail_Json);
                            bool bReset = false;
                            if (dbcharInfo.Count != userCharList.Count)
                            {
                                foreach (Character_Detail_With_HP chk_charInfo in userCharList)
                                {
                                    if (dbcharInfo.Find(charinfo => charinfo.cid == chk_charInfo.cid) == null)
                                    {
                                        dbcharInfo.Add(new User_Character_HP_Info(chk_charInfo, false)); ;
                                    }
                                }
                                bReset = true;
                                enemyInfo.EnemyCharacter_Info_Json = mJsonSerializer.ToJsonString(dbcharInfo);
                            }

                            if (bReset)
                                retError = GoldExpedition_Manager.SetEnemyInfoToDB(ref tb, AID, enemyInfo.Stage, enemyInfo);

                            retError = enemyInfo.Stage <= 0 ? Result_Define.eResult.GE_USER_STAGE_INFO_NOT_FOUND : Result_Define.eResult.SUCCESS;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Ret_GE_Stage_Detail detailInfo = new Ret_GE_Stage_Detail(ref enemyInfo);
                                json = JsonObject.Parse(detailInfo.ToJson());

                                User_GE_Boost_Item boostInfo = GoldExpedition_Manager.GetUser_GE_Boost_Item(ref tb, AID);
                                json = mJsonSerializer.AddJson(json, GoldExpedition_Define.GE_Ret_KeyList[GoldExpedition_Define.eGEReturnKeys.BoostInfo], mJsonSerializer.ToJsonString(boostInfo));
                            }
                        }
                        else if (requestOp.Equals("playstart"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            short Stage = queryFetcher.QueryParam_FetchShort("stage");
                            short Boost1 = queryFetcher.QueryParam_FetchShort("boost1");
                            short Boost2 = queryFetcher.QueryParam_FetchShort("boost2");
                            short Boost3 = queryFetcher.QueryParam_FetchShort("boost3");
                            bool bTest = queryFetcher.SetDebugMode && queryFetcher.QueryParam_FetchByte("bot") > 0;

                            // china version don't use booster item in Gold Expedition.
                            if (SystemData.GetServiceArea(ref tb) == DataManager_Define.eCountryCode.China)
                                Boost1 = Boost2 = Boost3 = 0;

                            User_GE_Stage_Info userGEInfo = GoldExpedition_Manager.GetUser_GE_Stage_Info(ref tb, AID);

                            retError = ((userGEInfo.Clear_Stage + 1) != Stage) ? Result_Define.eResult.GE_USER_STAGE_INVALID : Result_Define.eResult.SUCCESS;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                if (VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.BAGSLOT_MAX_ITEM))
                                    retError = Result_Define.eResult.SUCCESS;
                                else
                                    retError = Result_Define.eResult.ITEM_INVENTORY_OVER;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<User_Character_HP_Info> setUserHPInfo = mJsonSerializer.JsonToObject<List<User_Character_HP_Info>>(userGEInfo.MyCharacter_Info_Json);
                                User_Character_HP_Info setAllyHPInfo = mJsonSerializer.JsonToObject<User_Character_HP_Info>(userGEInfo.AllyCharacter_Info_Json);

                                if (bTest)
                                {
                                    setUserHPInfo.ForEach(setCharInfo => { setCharInfo.curhp = 1; });
                                }

                                int allyHP = (setAllyHPInfo != null) ? setAllyHPInfo.curhp : 0;

                                if (retError == Result_Define.eResult.SUCCESS && Boost3 > 0)
                                {
                                    setUserHPInfo.ForEach(item =>
                                    {
                                        item.curhp += (int)(item.maxhp * GoldExpedition_Define.Boost3RecorveryHPRate);
                                    }
                                    );

                                    if (setAllyHPInfo != null)
                                    {
                                        setAllyHPInfo.curhp += (int)(setAllyHPInfo.maxhp * GoldExpedition_Define.Boost3RecorveryHPRate);
                                        userGEInfo.AllyCharacter_Info_Json = mJsonSerializer.ToJsonString(setAllyHPInfo);
                                    }

                                    userGEInfo.MyCharacter_Info_Json = mJsonSerializer.ToJsonString(setUserHPInfo);

                                    retError = GoldExpedition_Manager.SetUser_GE_Stage_InfoToDB(ref tb, AID, userGEInfo);
                                }

                                if ((setUserHPInfo.Sum(item => item.curhp) + allyHP) <= 0)
                                    retError = Result_Define.eResult.GE_USER_CHARACTER_HP_NOT_ENOUGH;
                            }

                            System_Expedition_Dungeon dungeonInfo = null;
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                int MaxLevel = CharacterManager.GetCharacterMaxLevel_FromDB(ref tb, AID);
                                dungeonInfo = GoldExpedition_Manager.GetSystem_Expedition_Dungeon(ref tb, MaxLevel, Stage);

                                if (dungeonInfo.ExpeditionID > 0)
                                    retError = AccountManager.UseUserTicket(ref tb, AID, dungeonInfo.Condition_PlayCoin);
                                else
                                    retError = Result_Define.eResult.GE_SYSTEM_STAGE_INFO_NOT_FOUND;
                            }

                            List<Return_DisassableItems_List> retDeletedItem = new List<Return_DisassableItems_List>();
                            if (retError == Result_Define.eResult.SUCCESS && (Boost1 + Boost2 + Boost3) > 0)
                            {
                                System_Booster_Group missionBooster = Dungeon_Manager.GetSystem_BoosterGroup(ref tb, dungeonInfo.Booster_Group_ID);

                                if (Boost1 > 0 && retError == Result_Define.eResult.SUCCESS)
                                {
                                    retError = ItemManager.UseItem(ref tb, AID, missionBooster.Boost1_ItemID, 1, ref retDeletedItem);
                                    if (retError == Result_Define.eResult.NOT_ENOUGH_USE_ITEM)
                                    {
                                        retError = ShopManager.PayBuyPrice(ref tb, AID, missionBooster.Boost1_PriceValue, Item_Define.Item_BuyType_List[missionBooster.Boost1_PriceType]);
                                    }
                                }
                                if (Boost2 > 0 && retError == Result_Define.eResult.SUCCESS)
                                {
                                    retError = ItemManager.UseItem(ref tb, AID, missionBooster.Boost2_ItemID, 1, ref retDeletedItem);
                                    if (retError == Result_Define.eResult.NOT_ENOUGH_USE_ITEM)
                                    {
                                        retError = ShopManager.PayBuyPrice(ref tb, AID, missionBooster.Boost2_PriceValue, Item_Define.Item_BuyType_List[missionBooster.Boost2_PriceType]);
                                    }
                                }
                                if (Boost3 > 0 && retError == Result_Define.eResult.SUCCESS)
                                {
                                    retError = ItemManager.UseItem(ref tb, AID, missionBooster.Boost3_ItemID, 1, ref retDeletedItem);
                                    if (retError == Result_Define.eResult.NOT_ENOUGH_USE_ITEM)
                                    {
                                        retError = ShopManager.PayBuyPrice(ref tb, AID, missionBooster.Boost1_PriceValue, Item_Define.Item_BuyType_List[missionBooster.Boost3_PriceType]);
                                    }
                                }
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = TriggerManager.ProgressTrigger(ref tb, AID, Trigger_Define.eTriggerType.Play_PVP, (int)Trigger_Define.ePvPType.MATCH_GOLDEXPEDITION);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account UserInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_act_type], 0);
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], SnailLog_Define.Operation_To_S_Event_ID(SnailLog_Define.GetOperationSID[requestOp]));
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_game_player_action_log]);
                                    
                                    //Character_Detail_With_HP ally_char_info = mJsonSerializer.JsonToObject<Character_Detail_With_HP>(userGEInfo.AllyCharacter_Detail_Json);
                                    //if (ally_char_info == null)
                                    //    ally_char_info = new Character_Detail_With_HP();
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetTicket], UserInfo.Ticket.ToString());
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetGold], UserInfo.Gold.ToString());
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.RetRuby], (UserInfo.Cash + UserInfo.EventCash).ToString());
                                    json = mJsonSerializer.AddJson(json, GoldExpedition_Define.GE_Ret_KeyList[GoldExpedition_Define.eGEReturnKeys.DungeonID], dungeonInfo.ExpeditionID.ToString());
                                    //json = mJsonSerializer.AddJson(json, GoldExpedition_Define.GE_Ret_KeyList[GoldExpedition_Define.eGEReturnKeys.AllyCharacterDetailInfo], ally_char_info.ToJson());
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.DeletedItem], mJsonSerializer.ToJsonString(retDeletedItem));
                                }
                            }
                        }
                        else if (requestOp.Equals("playresult"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            short Stage = queryFetcher.QueryParam_FetchShort("stage");
                            bool bTest = queryFetcher.SetDebugMode && queryFetcher.QueryParam_FetchInt("bot", 0) > 0; 
                            List<User_Character_HP_Info> userHPInfo = mJsonSerializer.JsonToObject<List<User_Character_HP_Info>>(queryFetcher.QueryParam_Fetch("mycharinfo", "[]"));
                            List<User_Character_HP_Info> enemyHPInfo = mJsonSerializer.JsonToObject<List<User_Character_HP_Info>>(queryFetcher.QueryParam_Fetch("enemycharinfo", "[]"));
                            bool bClear = false;
                            User_GE_Stage_Info userGEInfo = GoldExpedition_Manager.GetUser_GE_Stage_Info(ref tb, AID);

                            retError = ((userGEInfo.Clear_Stage + 1) != Stage) ? Result_Define.eResult.GE_USER_STAGE_INVALID : Result_Define.eResult.SUCCESS;

                            if (bTest)
                            {
                                User_GE_Stage_Enemy enemyInfo = GoldExpedition_Manager.GetUser_GE_Stage_Enemy(ref tb, AID, Stage);
                                enemyHPInfo = mJsonSerializer.JsonToObject<List<User_Character_HP_Info>>(enemyInfo.EnemyCharacter_Info_Json);
                                userHPInfo = mJsonSerializer.JsonToObject<List<User_Character_HP_Info>>(userGEInfo.MyCharacter_Info_Json);
                                enemyHPInfo.ForEach(setCharInfo => { setCharInfo.curhp = 0; });
                            }

                            List<Client_Use_SkillInfo> useSkillList = mJsonSerializer.JsonToObject<List<Client_Use_SkillInfo>>(queryFetcher.QueryParam_Fetch("maxdmg", "[]"));
                            bool bVerify = false;
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Error_Skill_Info errSkill = new Error_Skill_Info();
                                bVerify = SoulManager.CheckSkillVerify(ref tb, AID, useSkillList, ref retError, out errSkill);
                                json = mJsonSerializer.AddJson(json, "errskill", mJsonSerializer.ToJsonString(errSkill));

                                if (!bVerify && retError == Result_Define.eResult.SUCCESS)
                                    retError = Result_Define.eResult.System_Hack_Detected;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                User_GE_Stage_Enemy enemyInfo = GoldExpedition_Manager.GetUser_GE_Stage_Enemy(ref tb, AID, Stage);

                                List<User_Character_HP_Info> setEnemyHPInfo = mJsonSerializer.JsonToObject<List<User_Character_HP_Info>>(enemyInfo.EnemyCharacter_Info_Json);

                                setEnemyHPInfo.ForEach(setCharInfo =>
                                {
                                    var findCharInfo = enemyHPInfo.Find(findChar => findChar.cid == setCharInfo.cid);
                                    if (findCharInfo != null)
                                        setCharInfo.curhp = findCharInfo.curhp;
                                }
                                );

                                enemyInfo.EnemyCharacter_Info_Json = mJsonSerializer.ToJsonString(setEnemyHPInfo);
                                bClear = setEnemyHPInfo.Sum(charinfo => charinfo.curhp) <= 0;
                                retError = GoldExpedition_Manager.SetEnemyInfoToDB(ref tb, AID, Stage, enemyInfo);
                            }

                            List<User_Inven> makeRealItem = new List<User_Inven>();
                            int getExpoint = 0;
                            if (retError == Result_Define.eResult.SUCCESS && bClear)
                            {
                                int MaxLevel = CharacterManager.GetCharacterMaxLevel_FromDB(ref tb, AID);
                                System_Expedition_Dungeon dungeonInfo = GoldExpedition_Manager.GetSystem_Expedition_Dungeon(ref tb, MaxLevel, Stage);

                                if (dungeonInfo.ExpeditionID > 0)
                                {
                                    getExpoint = dungeonInfo.Base_Reward_Point;
                                    retError = AccountManager.AddUserExpeditionPoint(ref tb, AID, getExpoint);

                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        Character charInfo = CharacterManager.GetCharacter(ref tb, AID, CID);

                                        List<System_Drop_Group> getDropList = DropManager.GetDropResult(ref tb, AID, dungeonInfo.Rand_DropBoxGroupId, (short)charInfo.Class);

                                        int GE_RewardBaseGold = SystemData.GetConstValueInt(ref tb, GoldExpedition_Define.GE_Const_Def_Key_List[GoldExpedition_Define.eGEConstDef.DEF_EXPEDITION_DUNGEON_REWARD_GOLDBASE]);
                                        int GE_RewardLevel1 = SystemData.GetConstValueInt(ref tb, GoldExpedition_Define.GE_Const_Def_Key_List[GoldExpedition_Define.eGEConstDef.DEF_EXPEDITION_DUNGEON_REWARD_LEVEL1]);
                                        int GE_RewardLevel2 = SystemData.GetConstValueInt(ref tb, GoldExpedition_Define.GE_Const_Def_Key_List[GoldExpedition_Define.eGEConstDef.DEF_EXPEDITION_DUNGEON_REWARD_LEVEL2]);
                                        int GE_RewardGold1 = SystemData.GetConstValueInt(ref tb, GoldExpedition_Define.GE_Const_Def_Key_List[GoldExpedition_Define.eGEConstDef.DEF_EXPEDITION_DUNGEON_REWARD_GOLD1]);
                                        int GE_RewardGold2 = SystemData.GetConstValueInt(ref tb, GoldExpedition_Define.GE_Const_Def_Key_List[GoldExpedition_Define.eGEConstDef.DEF_EXPEDITION_DUNGEON_REWARD_GOLD2]);
                                        int GE_RewardStageCount = SystemData.GetConstValueInt(ref tb, GoldExpedition_Define.GE_Const_Def_Key_List[GoldExpedition_Define.eGEConstDef.DEF_EXPEDITION_DUNGEON_REWARD_STAGECOUNT]);

                                        int rewardGold = (int)(
                                                (GE_RewardBaseGold
                                                + ((MaxLevel - GE_RewardLevel1) * GE_RewardGold1)
                                                + (MaxLevel >= GE_RewardLevel2 ? ((MaxLevel - GE_RewardLevel2) * GE_RewardGold2) : 0))
                                                / GE_RewardStageCount
                                            );

                                        if (rewardGold > 0)
                                        {
                                            System_Drop_Group setFakeGoldItem = new System_Drop_Group();
                                            setFakeGoldItem.DropItemID = GoldExpedition_Define.GE_Fake_Gold_ItemID;
                                            setFakeGoldItem.DropMinNum = rewardGold;
                                            setFakeGoldItem.DropMaxNum = rewardGold;
                                            setFakeGoldItem.DropTargetType = "Gold";
                                            getDropList.Add(setFakeGoldItem);
                                        }

                                        foreach (System_Drop_Group setDrop in getDropList)
                                        {
                                            List<User_Inven> getItem = new List<User_Inven>();
                                            retError = DropManager.MakeDropItem(ref tb, ref getItem, setDrop, AID, CID);

                                            if (retError != Result_Define.eResult.SUCCESS)
                                                break;

                                            makeRealItem.AddRange(getItem);
                                        }

                                        if (retError == Result_Define.eResult.SUCCESS)
                                            retError = makeRealItem.Count > 0 ? Result_Define.eResult.SUCCESS : Result_Define.eResult.ITEM_CREATE_FAIL;
                                    }
                                }
                                else
                                    retError = Result_Define.eResult.GE_SYSTEM_STAGE_INFO_NOT_FOUND;
                            }

                            if (retError == Result_Define.eResult.SUCCESS && bClear)
                                retError = TriggerManager.ProgressTrigger(ref tb, AID, Trigger_Define.eTriggerType.Win_PVP, (int)Trigger_Define.ePvPType.MATCH_GOLDEXPEDITION);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                userGEInfo.Clear_Stage = bClear ? Stage : userGEInfo.Clear_Stage;

                                List<User_Character_HP_Info> setUserHPInfo = mJsonSerializer.JsonToObject<List<User_Character_HP_Info>>(userGEInfo.MyCharacter_Info_Json);

                                setUserHPInfo.ForEach(setCharInfo =>
                                {
                                    var findCharInfo = userHPInfo.Find(findChar => findChar.cid == setCharInfo.cid);
                                    if (findCharInfo != null)
                                        setCharInfo.curhp = findCharInfo.curhp;
                                }
                                );

                                User_Character_HP_Info setAllyHPInfo = mJsonSerializer.JsonToObject<User_Character_HP_Info>(userGEInfo.AllyCharacter_Info_Json);
                                if (setAllyHPInfo != null)
                                {
                                    var findAllyInfo = userHPInfo.Find(findChar => findChar.cid == setAllyHPInfo.cid);
                                    if (findAllyInfo != null)
                                        setAllyHPInfo.curhp = findAllyInfo.curhp;
                                    userGEInfo.AllyCharacter_Info_Json = mJsonSerializer.ToJsonString(setAllyHPInfo);
                                }

                                userGEInfo.MyCharacter_Info_Json = mJsonSerializer.ToJsonString(setUserHPInfo);
                                Guild userGuildInfo = GuildManager.GetGuildInfo(ref tb, AID);
                                if (userGuildInfo.guild_id > 0)
                                    retError = GuildManager.AddGuildPoint(ref tb, userGuildInfo.guild_id, AID, Guild_Define.AddGuildPoint_List[Guild_Define.ePlayType.GOLDEXPEDITION]);

                                if (retError == Result_Define.eResult.SUCCESS)
                                    retError = GoldExpedition_Manager.SetUser_GE_Stage_InfoToDB(ref tb, AID, userGEInfo);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account userInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError);

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_act_type], 1);
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], SnailLog_Define.Operation_To_S_Event_ID(SnailLog_Define.GetOperationSID[requestOp]));                
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_game_player_action_log]);

                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.RetGold], userInfo.Gold.ToString());
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.RetRuby], (userInfo.Cash + userInfo.EventCash).ToString());
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.RetExpeditionPoint], getExpoint.ToString());
                                    //json = mJsonSerializer.AddJson(json, GoldExpedition_Define.GE_Ret_KeyList[GoldExpedition_Define.eGEReturnKeys.BoostInfo], mJsonSerializer.ToJsonString(boostInfo));
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeRealItem));
                                }
                            }
                        }
                        else if (requestOp.Equals("set_ge_group"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long CID1 = queryFetcher.QueryParam_FetchLong("cid1", 0);
                            long CID2 = queryFetcher.QueryParam_FetchLong("cid2", 0);
                            long CID3 = queryFetcher.QueryParam_FetchLong("cid3", 0);
                            long CID4 = queryFetcher.QueryParam_FetchLong("cid4", 0);

                            User_GE_CharacterGroup setGroup = new User_GE_CharacterGroup();
                            setGroup.aid = AID;
                            setGroup.cid1 = CID1;
                            setGroup.cid2 = CID2;
                            setGroup.cid3 = CID3;
                            setGroup.cid4 = CID4;
                            retError = GoldExpedition_Manager.SetUser_GE_CharacterGroupInfoToDB(ref tb, AID, setGroup);
                        }

                            /*
                "registerlist",         // 길드 용병 등록 정보 표시
                "mercenarylist",        // 길드 용병 고용 정보 표시
                "mercenaryregister",    // 길드 용병 등록            
                "mercenaryemploy",      // 길드 용병 고용
                "mercenarycall",        // 길드 용병 불러오기                             
                             */
                        else if (requestOp.Equals("registerlist"))
                        {
                            retError = Result_Define.eResult.SUCCESS;
                            List<User_Guild_Mercenary_Info> getList = GoldExpedition_Manager.GetUser_Mercenary_Info(ref tb, AID);
                            List<Ret_Guild_Mecenary_Info> retList = User_Guild_Mercenary_Info.makeGE_Guild_Mecenary_InfoJson(ref getList);
                            json = mJsonSerializer.AddJson(json, GoldExpedition_Define.GE_Ret_KeyList[GoldExpedition_Define.eGEReturnKeys.MyMercenaryInfo], mJsonSerializer.ToJsonString(retList));
                        }
                        else if (requestOp.Equals("mercenarylist"))
                        {
                            retError = Result_Define.eResult.SUCCESS;
                            Account userInfo = AccountManager.GetAccountData(ref tb, AID, ref retError);
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<User_Guild_Mercenary_Info> getList = GoldExpedition_Manager.GetUser_Guild_Mercenary_Info(ref tb, AID, userInfo.GuildID);
                                List<Ret_Guild_Mecenary_Info> retList = User_Guild_Mercenary_Info.makeGE_Guild_Mecenary_InfoJson(ref getList);

                                retList.ForEach(retinfo =>
                                {
                                    retinfo.income_gold = retinfo.char_info.level * GoldExpedition_Define.MercenaryEmployIncomeGoldPerLevel;
                                }
                                );
                                json = mJsonSerializer.AddJson(json, GoldExpedition_Define.GE_Ret_KeyList[GoldExpedition_Define.eGEReturnKeys.GuildMercenaryInfo], mJsonSerializer.ToJsonString(retList));
                            }
                        }
                        else if (requestOp.Equals("mercenaryregister"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            List<User_Guild_Mercenary_Info> getList = GoldExpedition_Manager.GetUser_Mercenary_Info(ref tb, AID);

                            retError = !VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.EXPEDITION_HERO_REGI_MAX, getList.Count) ?
                                Result_Define.eResult.VIP_EXPEDITION_HERO_REGI_MAX : Result_Define.eResult.SUCCESS;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Character baseCharInfo = CharacterManager.GetCharacter(ref tb, AID, CID);
                                Character_Stat getStat = CharacterManager.GetCharacterStat(ref tb, baseCharInfo.cid);
                                Character_Detail_With_HP setObj = new Character_Detail_With_HP(baseCharInfo, getStat.HPMax, getStat.HPMax);
                                setObj.warpoint = getStat.WAR_POINT + getStat.ACTIVE_SOUL_WAR_POINT + getStat.PASSIVE_SOUL_WAR_POINT;
                                setObj.equiplist = ItemManager.GetEquipList(ref tb, AID, baseCharInfo.cid);
                                setObj.equip_ultimate = ItemManager.GetEquipUltimate(ref tb, AID, baseCharInfo.cid);
                                setObj.equip_active_soul = SoulManager.GetRet_Active_Soul_Equip_List(ref tb, AID, baseCharInfo.cid);
                                setObj.equip_passive_soul = SoulManager.GetRet_Passive_Soul_Equip_List(ref tb, AID, baseCharInfo.cid);

                                retError = GoldExpedition_Manager.SetUser_Mercenary_InfoToDB(ref tb, AID, setObj, true);
                            }
                        }
                        else if (requestOp.Equals("mercenaryemploy"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            long mAID = queryFetcher.QueryParam_FetchLong("mercenary_aid");
                            long mCID = queryFetcher.QueryParam_FetchLong("mercenary_cid");
                            long mGID = queryFetcher.QueryParam_FetchLong("mercenary_gid");

                            User_Guild_Mercenary_Info mercenaryInfo = GoldExpedition_Manager.GetGuild_Mercenary_Info(ref tb, mAID, mCID);

                            User_GE_Stage_Info userGEInfo = GoldExpedition_Manager.GetUser_GE_Stage_Info(ref tb, AID);
                            User_Character_HP_Info oldAllyInfo = mJsonSerializer.JsonToObject<User_Character_HP_Info>(userGEInfo.AllyCharacter_Info_Json);
                            if (oldAllyInfo == null)
                                oldAllyInfo = new User_Character_HP_Info();

                            if (!VipManager.CheckVIPCountOver(ref tb, AID, 0, VIP_Define.eVipType.EXPEDITION_HERO_HIRE_MAX, userGEInfo.HireCount))
                                retError = Result_Define.eResult.VIP_EXPEDITION_HIRE_MAX;
                            else
                                retError = Result_Define.eResult.SUCCESS;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                //List<User_Character_HP_Info> setUserHPInfo = mJsonSerializer.JsonToObject<List<User_Character_HP_Info>>(userGEInfo.MyCharacter_Info_Json);
                                userGEInfo.AllyCharacter_Detail_Json = mercenaryInfo.Character_Detail_Json;
                                userGEInfo.AllyCharacter_Info_Json = mercenaryInfo.Character_Info_Json;
                                userGEInfo.AllyUserName = mercenaryInfo.AllyUserName;
                                userGEInfo.HireCount++;
                                userGEInfo.RegDate = DateTime.Now;
                                retError = GoldExpedition_Manager.SetUser_GE_Stage_InfoToDB(ref tb, AID, userGEInfo);
                            }

                            int employCost = 0;
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                User_Character_HP_Info setAllyInfo = mJsonSerializer.JsonToObject<User_Character_HP_Info>(userGEInfo.AllyCharacter_Info_Json);
                                employCost = GoldExpedition_Define.MercenaryEmployIncomeGoldPerLevel * setAllyInfo.level;

                                if (userGEInfo.HireCount > 1)
                                    retError = AccountManager.UseUserCash(ref tb, AID, SystemData.GetConstValueInt(ref tb, GoldExpedition_Define.GE_Const_Def_Key_List[GoldExpedition_Define.eGEConstDef.DEF_EXPEDITION_HIRE_RUBY_COST]));
                                else
                                    retError = AccountManager.UseUserGold(ref tb, AID, employCost);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                mercenaryInfo.IncomeGold += (int)(employCost * GoldExpedition_Define.MercenaryEmployIncomeRate);
                                retError = GoldExpedition_Manager.SetUser_Mercenary_IncomeGoldToDB(ref tb, ref mercenaryInfo, true);
                            }

                            if (retError == Result_Define.eResult.SUCCESS && oldAllyInfo.cid > 0)
                            {
                                User_GE_CharacterGroup userGEGroup = GoldExpedition_Manager.GetGECharacterGroupInfo(ref tb, AID);
                                userGEGroup.cid1 = (userGEGroup.cid1 == oldAllyInfo.cid) ? mercenaryInfo.CID : userGEGroup.cid1;
                                userGEGroup.cid2 = (userGEGroup.cid2 == oldAllyInfo.cid) ? mercenaryInfo.CID : userGEGroup.cid2;
                                userGEGroup.cid3 = (userGEGroup.cid3 == oldAllyInfo.cid) ? mercenaryInfo.CID : userGEGroup.cid3;
                                userGEGroup.cid4 = (userGEGroup.cid4 == oldAllyInfo.cid) ? mercenaryInfo.CID : userGEGroup.cid4;

                                retError = GoldExpedition_Manager.SetUser_GE_CharacterGroupInfoToDB(ref tb, AID, userGEGroup);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account userInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    User_Character_HP_Info allyInfo = mJsonSerializer.JsonToObject<User_Character_HP_Info>(userGEInfo.AllyCharacter_Info_Json);
                                    if (allyInfo == null)
                                        allyInfo = new User_Character_HP_Info();

                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetGold], userInfo.Gold.ToString());
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.RetRuby], (userInfo.Cash + userInfo.EventCash).ToString());
                                    json = mJsonSerializer.AddJson(json, GoldExpedition_Define.GE_Ret_KeyList[GoldExpedition_Define.eGEReturnKeys.AllyUserName], userGEInfo.AllyUserName);
                                    json = mJsonSerializer.AddJson(json, GoldExpedition_Define.GE_Ret_KeyList[GoldExpedition_Define.eGEReturnKeys.AllyCharacterInfo], mJsonSerializer.ToJsonString(allyInfo));
                                }
                            }
                        }
                        else if (requestOp.Equals("mercenarycall"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            User_Guild_Mercenary_Info mercenaryInfo = GoldExpedition_Manager.GetGuild_Mercenary_Info(ref tb, AID, CID);

                            retError = (mercenaryInfo.AID > 0) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.GE_MERCENARY_INFO_NOT_FOUND;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Ret_Guild_Mecenary_Info retInfo = new Ret_Guild_Mecenary_Info(mercenaryInfo);
                                if (retInfo.regtime >= GoldExpedition_Define.MercenaryCallLimitTime)
                                    retError = AccountManager.AddUserGold(ref tb, AID, retInfo.income_gold);
                                else
                                    retError = Result_Define.eResult.GE_MERCENARY_CALL_TIME_NOT_ENOUGH;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                mercenaryInfo.IncomeGold = 0;
                                retError = GoldExpedition_Manager.SetUser_Mercenary_IncomeGoldToDB(ref tb, ref mercenaryInfo, false);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account userInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetGold], userInfo.Gold.ToString());
                                }
                            }
                        }
                        else if (requestOp.Equals("enemyinfo"))
                        {
                            List<User_GE_Stage_Enemy> enemy = GoldExpedition_Manager.GetUser_GE_Stage_Enemy(ref tb, AID);

                            Ret_GE_StageInfo setEnemy = new Ret_GE_StageInfo(enemy.FirstOrDefault());
                        }

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