using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using TheSoul.DataManager.DBClass;
using ServiceStack.Text;

namespace TheSoul.DataManager.DBClass
{
    public class System_Expedition_Dungeon
    {
        public int ExpeditionID { get; set; }
        public short Stage_No { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public string Desc { get; set; }
        public string Stage_Scene_name { get; set; }
        public int Next_ExpeditionID { get; set; }
        public short Condition_PlayCoin { get; set; }
        public int Max_Play_Time { get; set; }
        public short Base_Reward_Point { get; set; }
        public int Best_Reward_Item1_PC1 { get; set; }
        public int Item1_Grade_PC1 { get; set; }
        public int Best_Reward_Item1_PC2 { get; set; }
        public int Item1_Grade_PC2 { get; set; }
        public int Best_Reward_Item1_PC3 { get; set; }
        public int Item1_Grade_PC3 { get; set; }
        public int Best_Reward_Item2_PC1 { get; set; }
        public int Item2_Grade_PC1 { get; set; }
        public int Best_Reward_Item2_PC2 { get; set; }
        public int Item2_Grade_PC2 { get; set; }
        public int Best_Reward_Item2_PC3 { get; set; }
        public int Item2_Grade_PC3 { get; set; }
        public int Rand_DropBoxGroupId { get; set; }
        public byte Booster_Group_ID { get; set; }
    }


    public class User_GE_Boost_Item
    {
        public long aid { get; set; }
        public int boost_1 { get; set; }
        public int boost_2 { get; set; }
        public int boost_3 { get; set; }
    }
    
    public class User_GE_Stage_Info
    {
        public long AID { get; set; }
        public short Clear_Stage { get; set; }
        public string MyCharacter_Info_Json { get; set; }
        public string MyCharacter_Detail_Json { get; set; }
        public string AllyCharacter_Info_Json { get; set; }
        public string AllyCharacter_Detail_Json { get; set; }
        public string AllyUserName { get; set; }
        public short HireCount { get; set; }
        public short ResetCount { get; set; }
        public DateTime RegDate { get; set; }

        public User_GE_Stage_Info()
        {
            RegDate = DateTime.Now;
        }
    }

    public class User_Guild_Mercenary_Info
    {
        public long GID { get; set; }
        public long AID { get; set; }
        public long CID { get; set; }
        public int IncomeGold { get; set; }
        public string Character_Info_Json { get; set; }
        public string Character_Detail_Json { get; set; }
        public string AllyUserName { get; set; }
        public string ActiveFlag { get; set; }
        public DateTime RegDate { get; set; }

        public static List<Ret_Guild_Mecenary_Info> makeGE_Guild_Mecenary_InfoJson(ref List<User_Guild_Mercenary_Info> enemyList)
        {
            List<Ret_Guild_Mecenary_Info> retList = new List<Ret_Guild_Mecenary_Info>();

            enemyList.ForEach(info =>
            {
                Ret_Guild_Mecenary_Info setObj = new Ret_Guild_Mecenary_Info(info);
                retList.Add(setObj);
            }
            );

            return retList;
        }
    }

    public class Ret_Guild_Mecenary_Info
    {
        public long gid { get; set; }
        public long aid { get; set; }
        public int income_gold { get; set; }
        public string ally_username { get; set; }
        public long regtime { get; set; }
        public User_Character_HP_Info char_info { get; set; }

        public Ret_Guild_Mecenary_Info(User_Guild_Mercenary_Info setInfo)
        {
            char_info = mJsonSerializer.JsonToObject<User_Character_HP_Info>(setInfo.Character_Info_Json);
            ally_username = setInfo.AllyUserName;
            gid = setInfo.GID;
            aid = setInfo.AID;
            TimeSpan dateDiff = DateTime.Now - setInfo.RegDate;

            double diffMinute = dateDiff.TotalMinutes;
            int timeIncome = System.Convert.ToInt32((diffMinute / GoldExpedition_Define.MercenaryTimeIncomeMinutes)) * (char_info.level * GoldExpedition_Define.MercenaryTimeIncomeGoldPerLevel);
            regtime = System.Convert.ToInt64(dateDiff.TotalSeconds);
            //int timegold = setInfo.RegDate
            income_gold = GoldExpedition_Define.MercenaryIncomGold_LimteMax < (setInfo.IncomeGold + timeIncome) ? GoldExpedition_Define.MercenaryIncomGold_LimteMax : (setInfo.IncomeGold + timeIncome);
        }
    }
    
    public class User_GE_Stage_Enemy
    {
        public long AID { get; set; }
        public byte Stage { get; set; }
        public long EnemyAID { get; set; }
        public long CurrentWarPoint { get; set; }
        public long AdjustWarPoint { get; set; }
        public string EnemyCharacter_Main_Info_Json { get; set; }       // simple main character info
        public string EnemyCharacter_Info_Json { get; set; }            // simple all character
        public string EnemyCharacter_Detail_Json { get; set; }          // detail info for ingame
        public string UserName { get; set; }
        public byte isDummy { get; set; }

        public User_GE_Stage_Enemy(bool bDummy = false)
        {
            isDummy = (byte)(bDummy ? 1 : 0);
        }
    }

    public class User_WarPoint
    {
        public long CID { get; set; }
        public long AID { get; set; }
        public byte CHARACTER_MAX_LEVEL { get; set; }
        public long WAR_POINT { get; set; }
        public long MAX_WAR_POINT { get; set; }
    }

    public class User_Character_HP_Info
    {
        public long cid { get; set; }
        public int curhp { get; set; }
        public int maxhp { get; set; }
        public int warpoint { get; set; }
        public int class_type { get; set; }
        public int level { get; set; }

        public User_Character_HP_Info() {}
        public User_Character_HP_Info(Character_Detail_With_HP charInfo, bool myChar = false)
        {
            cid = charInfo.cid;
            //curhp = System.Convert.ToInt32(charInfo.curhp * setHpRate);
            //maxhp = System.Convert.ToInt32(charInfo.maxhp * setHpRate);
            // hardcoding : first generate enemy hp is -1
            curhp = maxhp = myChar ? charInfo.curhp : -1 ;
            //warpoint = System.Convert.ToInt32(charInfo.warpoint * setHpRate);
            warpoint = charInfo.warpoint;
            class_type = charInfo.Class;
            level = charInfo.level;
        }
    }

    public class Ret_GE_StageInfo
    {
        public long enemy_aid { get; set; }
        public short stage { get; set; }
        public long total_warpoint { get; set; }
        public User_Character_HP_Info enemy_main_charinfo { get; set; }

        public Ret_GE_StageInfo() { }
        public Ret_GE_StageInfo(User_GE_Stage_Enemy enemyInfo)
        {
            enemy_aid = enemyInfo.EnemyAID;
            stage = enemyInfo.Stage;
            enemy_main_charinfo = mJsonSerializer.JsonToObject<User_Character_HP_Info>(enemyInfo.EnemyCharacter_Main_Info_Json);
            total_warpoint = enemyInfo.AdjustWarPoint;
        }

        public Ret_GE_StageInfo(User_GE_Stage_Enemy enemyInfo, int accountWarpoint)
        {
            enemy_aid = enemyInfo.EnemyAID;
            stage = enemyInfo.Stage;
            enemy_main_charinfo = mJsonSerializer.JsonToObject<User_Character_HP_Info>(enemyInfo.EnemyCharacter_Main_Info_Json);
            total_warpoint = accountWarpoint;
            //enemy_main_charinfo.warpoint = accountWarpoint;
        }

        public static List<Ret_GE_StageInfo> makeStageInfoJson(ref List<User_GE_Stage_Enemy> enemyList)
        {
            List<Ret_GE_StageInfo> retList = new List<Ret_GE_StageInfo>();

            enemyList.ForEach(info =>
            {
                Ret_GE_StageInfo setObj = new Ret_GE_StageInfo(info);
                setObj.total_warpoint = info.AdjustWarPoint;
                //setObj.enemy_main_charinfo.warpoint = info.AdjustWarPoint;
                retList.Add(setObj);
            }
            );

            return retList;
        }
    }

    public class Ret_GE_Stage_Detail
    {
        public long enemy_aid { get; set; }
        public string enemy_username { get; set; }
        public short stage { get; set; }
        public long current_warpoint { get; set; }
        public long adjust_warpoint { get; set; }
        public List<User_Character_HP_Info> enemy_hp_info { get; set; }
        public List<Character_Detail_With_HP> enemy_char_info { get; set; }

        public Ret_GE_Stage_Detail() { }
        public Ret_GE_Stage_Detail(ref User_GE_Stage_Enemy enemyInfo) 
        {
            enemy_aid = enemyInfo.EnemyAID;
            enemy_username = enemyInfo.UserName;
            stage = enemyInfo.Stage;
            current_warpoint = enemyInfo.CurrentWarPoint;
            adjust_warpoint = enemyInfo.AdjustWarPoint;
            enemy_hp_info = mJsonSerializer.JsonToObject<List<User_Character_HP_Info>>(enemyInfo.EnemyCharacter_Info_Json);
            enemy_char_info = mJsonSerializer.JsonToObject<List<Character_Detail_With_HP>>(enemyInfo.EnemyCharacter_Detail_Json);
        }

        public string ToJson()
        {
            JsonObject setJson = new JsonObject();
            setJson = mJsonSerializer.AddJson(setJson, "enemy_aid", enemy_aid.ToString());
            setJson = mJsonSerializer.AddJson(setJson, "enemy_username", enemy_username);
            setJson = mJsonSerializer.AddJson(setJson, "stage", stage.ToString());
            setJson = mJsonSerializer.AddJson(setJson, "current_warpoint", current_warpoint.ToString());
            setJson = mJsonSerializer.AddJson(setJson, "adjust_warpoint", adjust_warpoint.ToString());
            setJson = mJsonSerializer.AddJson(setJson, "enemy_hp_info", mJsonSerializer.ToJsonString(enemy_hp_info));
            setJson = mJsonSerializer.AddJson(setJson, "enemy_char_info", Character_Detail_With_HP.makeCharacter_DetailListJson(enemy_char_info));

            return setJson.ToJson();
        }
    }
    // old code

    public class GoldExpeditionInfo
    {
        public long AID { get; set; }
        public long mercenaryAID { get; set; }
        public int currentStage { get; set; }
        public int expeditionpoint { get; set; }
        public int expeditionresetcnt { get; set; }
        public int expeditionshopreset { get; set; }
        //public DateTime geDate { get; set; }
    }

    public class GameData_ExpeditionDungeon
    {
        public int ExpeditionID { get; set; }
        public int Stage_No { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public string Desc { get; set; }
        public string Stage_Scene_Name { get; set; }
        public int Next_Stage_No { get; set; }
        public int Condition_Ticket { get; set; }
        public int Clear_Time { get; set; }
        public int Base_Reward_Point { get; set; }
        public int Best_Reward_Item1_PC1 { get; set; }
        public int Item1_Grade_PC1 { get; set; }
        public int Best_Reward_Item1_PC2 { get; set; }
        public int Item1_Grade_PC2 { get; set; }
        public int Best_Reward_Item1_PC3 { get; set; }
        public int Item1_Grade_PC3 { get; set; }
        public int Best_Reward_Item2_PC1 { get; set; }
        public int Item2_Grade_PC1 { get; set; }
        public int Best_Reward_Item2_PC2 { get; set; }
        public int Item2_Grade_PC2 { get; set; }
        public int Best_Reward_Item2_PC3 { get; set; }
        public int Item2_Grade_PC3 { get; set; }
        public long RandBox_DropBoxGroupId1 { get; set; }
        public long RandBox_DropBoxGroupId2 { get; set; }
    }

    public class myExpeditionInfo
    {
        public Int64 aid { get; set; }
        public Int64 mercenary_aid { get; set; }
        public int currentstage { get; set; }
        public int expeditionpoint { get; set; }
        public int expeditionresetcnt { get; set; }
        public int expeditionshopreset { get; set; }

        public myExpeditionInfo() { }
        public myExpeditionInfo(GoldExpeditionInfo setInfo)
        {
            aid = setInfo.AID;
            mercenary_aid = setInfo.mercenaryAID;
            currentstage = setInfo.currentStage;
            expeditionpoint = setInfo.expeditionpoint;
            expeditionresetcnt = setInfo.expeditionresetcnt;
            expeditionshopreset = setInfo.expeditionshopreset;
        }
    }

    public class GoldExpeditionStageInfo
    {
        public Int64 aid { get; set; }
        public int firstenemyhp { get; set; }
        public int firstenemykill { get; set; }
        public int secondenemyhp { get; set; }
        public int secondenemykill { get; set; }
        public int thirdenemyhp { get; set; }
        public int thirdenemykill { get; set; }
        public int booster1cnt { get; set; }
        public int booster2cnt { get; set; }
        public int booster3cnt { get; set; }
    }

    public class GECharInfo
    {
        public Int64 cid { get; set; }
        public int classtype { get; set; }
        public int cindex { get; set; }
        public int curhp { get; set; }
        public int maxhp { get; set; }

        public GECharInfo() { }
        public GECharInfo(GoldExpeditionCharInfo setInfo, int setclasstype = 0)
        {
            cid = setInfo.CID;
            classtype = setclasstype;
            cindex = setInfo.CIndex;
            curhp = setInfo.CurHP;
            maxhp = setInfo.MaxHP;
        }

        public GECharInfo(GECharInfo setInfo, int setclasstype = 0)
        {
            cid = setInfo.cid;
            classtype = setInfo.classtype;
            cindex = setInfo.cindex;
            curhp = setInfo.curhp;
            maxhp = setInfo.maxhp;
        }
    }

    public class enemylobbychar
    {
        public int nclass { get; set; }
        public int level { get; set; }
    }

    public class StageInfoChar
    {
        public Int64 cid { get; set; }
        public int nclass { get; set; }
        public int level { get; set; }
    }

    public class StageInfoSoulActive
    {
        public Int64 cid { get; set; }
        public int soulid { get; set; }
        public string soulname { get; set; }
        public byte classtype { get; set; }
        public int soullevel { get; set; }
        public int soulgradelevel { get; set; }
        public int slotnum { get; set; }
        public int special_buff1 { get; set; }
        public int special_buff2 { get; set; }
        public int special_buff3 { get; set; }

        public StageInfoSoulActive() { }
        public StageInfoSoulActive(ActiveSoul setSoul)
        {
            cid = setSoul.cid;
            soulid = setSoul.soulid;
            soulname = setSoul.soulname;
            classtype = setSoul.classtype;
            soullevel = setSoul.soullevel;
            soulgradelevel = setSoul.soulgradelevel;
            slotnum = setSoul.slotnum;
            special_buff1 = setSoul.special_buff1;
            special_buff2 = setSoul.special_buff2;
            special_buff3 = setSoul.special_buff3;
        }
    }

    public class StageInfoSoulPassive
    {
        public Int64 cid { get; set; }
        public int soulid { get; set; }
        public string soulname { get; set; }
        public byte sclass { get; set; }
        public int soullevel { get; set; }
        public int slotnum { get; set; }
        public int buff { get; set; }

        public StageInfoSoulPassive() { }
        public StageInfoSoulPassive(PassiveSoul setSoul)
        {
            cid = cid;
            soulid = soulid;
            soulname = soulname;
            sclass = sclass;
            soullevel = soullevel;
            slotnum = slotnum;
            buff = buff;
        }
    }

    public class StageInfoItem
    {
        public long cid { get; set; }
        public long aid { get; set; }
        public short inventory_type { get; set; }
        public long itemid { get; set; }
        public int itemea { get; set; }
        public short itemtype { get; set; }
        public short iclass { get; set; }
        public short grade { get; set; }
        public string equipflag { get; set; }
        public string newyn { get; set; }
        public string equipposition { get; set; }
        public short enchant_grade { get; set; }
        public short enchant_level { get; set; }
        public short fixoption { get; set; }
        public int enchant_exp { get; set; }
        public string optiontype1 { get; set; }
        public int optionvalue1 { get; set; }
        public string optiontype2 { get; set; }
        public int optionvalue2 { get; set; }
        public string optiontype3 { get; set; }
        public int optionvalue3 { get; set; }
        public string optiontype4 { get; set; }
        public int optionvalue4 { get; set; }
        public string optiontype5 { get; set; }
        public int optionvalue5 { get; set; }

        public StageInfoItem() { }
        public StageInfoItem(User_Inven setSoul)
        {
            cid = setSoul.cid;
            aid = setSoul.aid;
            inventory_type = setSoul.inventory_type;
            itemid = setSoul.itemid;
            itemea = setSoul.itemea;
            itemtype = setSoul.item_type;
            iclass = setSoul.class_type;
            grade = setSoul.grade;
            equipflag = setSoul.equipflag;
            newyn = setSoul.newyn;
            equipposition = setSoul.equipposition;
        }
    }

    public class mercenaryExpeditionInfo
    {
        public Int64 aid { get; set; }
        public int lv { get; set; }
        public int nclass { get; set; }
        public string username { get; set; }
        public int warpoint { get; set; }
    }

    public class MercenaryRegister
    {
        public Int64 lv { get; set; }
        public int nclass { get; set; }
        public int income { get; set; }
        public uint regdate { get; set; }
    }

    public class MercenaryEmploy
    {
        public Int64 cid { get; set; }
        public int lv { get; set; }
        public int nclass { get; set; }
        public string username { get; set; }
        public int hp { get; set; }
        public int warpoint { get; set; }
    }

    public class GEShopItemList
    {
        public uint id { get; set; }
        public uint groupid { get; set; }
        public int salerate { get; set; }
        public int time { get; set; }
        public uint itemid { get; set; }
        public int sold { get; set; }
    }

    public class FullAccountInfo
    {
        public int matchnum { get; set; }
        public Account useraccount { get; set; }
        public List<FullCharaterInfo> char_info_list { get; set; }
        public buffAccountInfo buffaccount { get; set; }
    }

    public class FullCharaterInfo
    {
        public Character character_info { get; set; }
        public Dictionary<long, ActiveSoul> char_active_soul_list { get; set; }
        public Dictionary<long, PassiveSoul> char_passive_soul_list { get; set; }
        public List<User_Inven> char_equip_list { get; set; }
    }

    public class AllyCharacterInfo
    {
        public Character char_info { get; set; }
        public Dictionary<long, ActiveSoul> char_active_soul { get; set; }
        public Dictionary<long, PassiveSoul> char_passive_soul { get; set; }
        public List<User_Inven> char_equip { get; set; }
    }

    // Add by manstar
    public class GoldExpedition_EnemyInfo
    {
        public int matchnum { get; set; }
        public Account useraccount { get; set; }
        public List<Character_Detail_With_HP> char_info_list { get; set; }
        public buffAccountInfo buffaccount { get; set; }
    }

    public class retGe_EnemyInfo
    {
        public int matchnum { get; set; }
        public string username { get; set; }
        public List<Character_Detail_With_HP> char_info_list { get; set; }
        public buffAccountInfo buffaccount { get; set; }

        public retGe_EnemyInfo() { }
        public retGe_EnemyInfo(GoldExpedition_EnemyInfo setEnemy)
        {
            if (setEnemy != null)
            {
                username = setEnemy.useraccount.UserName;
                matchnum = setEnemy.matchnum;
                char_info_list = setEnemy.char_info_list;
                buffaccount = setEnemy.buffaccount;
            }
        }
    }

    public class GoldExpeditionCharInfo
    {
        public long AID { get; set; }
        public long CID { get; set; }
        public int ClassType { get; set; }
        public int CIndex { get; set; }
        public int CurHP { get; set; }
        public int MaxHP { get; set; }
        public DateTime gechrDate { get; set; }
    }

    public class retGoldExpeditionEnemyInfo
    {
        public long AID { get; set; }
        public string JSON_TEXT { get; set; }
    }

    public class AllyInfoSoulActive
    {
        public Int64 cid { get; set; }
        public int soulid { get; set; }
        public string soulname { get; set; }
        public byte classtype { get; set; }
        public int soullevel { get; set; }
        public int soulgradelevel { get; set; }
        public int slotnum { get; set; }
        public int special_buff1 { get; set; }
        public int special_buff2 { get; set; }
        public int special_buff3 { get; set; }
    }

    public class AllyInfoSoulPassive
    {
        public Int64 cid { get; set; }
        public int soulid { get; set; }
        public string soulname { get; set; }
        public byte sclass { get; set; }
        public int soullevel { get; set; }
        public int slotnum { get; set; }
        public int buff { get; set; }
    }

    public class AllyInfoItem
    {
        public long cid { get; set; }
        public long aid { get; set; }
        public short inventory_type { get; set; }
        public long itemid { get; set; }
        public int itemea { get; set; }
        public short itemtype { get; set; }
        public short iclass { get; set; }
        public short grade { get; set; }
        public string equipflag { get; set; }
        public string newyn { get; set; }
        public string equipposition { get; set; }
        public short enchant_grade { get; set; }
        public short enchant_level { get; set; }
        public short fixoption { get; set; }
        public int enchant_exp { get; set; }
        public string optiontype1 { get; set; }
        public int optionvalue1 { get; set; }
        public string optiontype2 { get; set; }
        public int optionvalue2 { get; set; }
        public string optiontype3 { get; set; }
        public int optionvalue3 { get; set; }
        public string optiontype4 { get; set; }
        public int optionvalue4 { get; set; }
        public string optiontype5 { get; set; }
        public int optionvalue5 { get; set; }
    }

    public class buffAccountInfo
    {
        public int buff_guild_lv { get; set; }
        public int buff_guild_atdcnt { get; set; }
    }

    public class randomboxitemlist
    {
        public long itemid { get; set; }
        public long itemvalue { get; set; }
    }
}