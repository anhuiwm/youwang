using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager.DBClass;
using ServiceStack.Text;

namespace TheSoul.DataManager.DBClass
{
    public class User_account
    {
        public long AID { get; set; }
        public string platform_user_id { get; set; }
        public string nick_name { get; set; }
        public int shard_dbno { get; set; }
        public DateTime reg_date { get; set; }
    }

    public class PvPInfo
    {
        public int straightwin { get; set; }
        public int straightlose { get; set; }
    }

    public class RetUserVIP
    {
        public int viplevel { get; set; }
        public int vippoint { get; set; }
        public int totalvippoint { get; set; }

        public RetUserVIP(User_VIP setVIP)
        {
            viplevel = setVIP.viplevel;
            vippoint = setVIP.vippoint;
            totalvippoint = setVIP.totalvippoint;
        }

        public RetUserVIP()
        {
        }
    }


    public class GetAccountDBSDK
    {
        public long RetDB_INDEX { get; set; }
        public long RetAID { get; set; }
        public long Result { get; set; }
    }

    public class GetAccountDB
    {
        public long RetDB_INDEX { get; set; }
        public string RetUserName { get; set; }
        public long Result { get; set; }
    }

    public class EncryptKey
    {
        public uint UpdateTime { get; set; }
    }

    [Serializable]
    public class Account
    {
        public long AID { get; set; }
        public long SNO { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public int Cash { get; set; }

        public int EventCash { get; set; }
        public int Gold { get; set; }
        public DateTime CreationDate { get; set; }
        public int LastConnTime { get; set; }
        public int PVEPlayState { get; set; }

        public int CharSlot { get; set; }
        public int Key { get; set; }
        public int Ticket { get; set; }
        public int KeyLastChargeTime { get; set; }
        public int KeyFillMaxEA { get; set; }

        public int TicketLastChargeTime { get; set; }
        public int TicketFillMaxEA { get; set; }
        public int RewardBuff1 { get; set; }
        public int BuffEndTime1 { get; set; }
        public int RewardBuff2 { get; set; }

        public int BuffEndTime2 { get; set; }
        public int LV { get; set; }
        public int equipclass { get; set; }
        public int UpdateTime { get; set; }
        public int OldTime { get; set; }
        
        public int RecommendReward { get; set; }
        public short RecommendCNT { get; set; }
        public int Tutorial { get; set; }
        public string VirginBossRaid { get; set; }
        public int PCEXPBuffEndTime { get; set; }

        public int SoulEXPBuffEndTime { get; set; }        
        public int InvenSlot { get; set; }
        public int InvenSlotCNT { get; set; }
        public int SoulInvenSlot { get; set; }
        public int SoulInvenSlotCNT { get; set; }

        public int TreasureInvenSlot { get; set; }
        public int TreasureInvenSlotCNT { get; set; }        
        public string Flatform { get; set; }
        public string CountryCode { get; set; }
        public int? LastWorldID { get; set; }

        public int? LastStageID { get; set; }
        public int OS { get; set; }
        public long EquipCID { get; set; }        
        public int DailyKey { get; set; }
        public int DailyTicket { get; set; }

        public int DailyRuby { get; set; }
        public int LanguageCode { get; set; }
        public int PCEXPBuffEndTime2 { get; set; }
        public int SoulEXPBuffEndTime2 { get; set; }
        public int friendscount { get; set; }
        
        public int friendswait { get; set; }
        public long FNO { get; set; }
        public int FriendlyPoint { get; set; }
        public int Medal { get; set; }
        public int ChallengeTicket { get; set; }
        
        public int ChallengeTicketLastChargeTime { get; set; }
        public short ChallengeTicketFillMaxEA { get; set; }
        public int Stone { get; set; }
        public int LuckyBoxCount { get; set; }
        public int? ItemFreeGachaCoolTime { get; set; }
        
        public int? SoulFreeGachaCoolTime { get; set; }
        public int LuckyBoxBonus { get; set; }
        public string PvPFriendFlag { get; set; }
        public int Honorpoint { get; set; }
        
        public int ContributionPoint { get; set; }
        public int PartyDungeonPoint { get; set; }
        public int CombatPoint { get; set; }
        public int OverlordPoint { get; set; }
        public int ExpeditionPoint { get; set; }
        public int BlackMarketPoint { get; set; }

        // for guild info
        public long GuildID { get; set; }
        public string GuildName { get; set; }
        public string GuildState { get; set; }
        public string GuildAttend { get; set; }
        public int GuildShopReset { get; set; }
        public long GuildMark { get; set; }

        public void SetGuildInfo(Guild setInfo)
        {
            GuildID = setInfo.guild_id;
            GuildName = setInfo.guild_name;
            GuildState = setInfo.guild_state;
            GuildAttend = setInfo.guild_attend;
            GuildMark= setInfo.guild_mark;
        }
    }


    public class Account_OnlyAID
    {
        public long aid { get; set; }
    }

    public class Account_Simple
    {
        public long aid { get; set; }
        public string username { get; set; }
        //public Character_Simple charinfo { get; set; }
    }

    public class Account_Simple_With_Character : Account_Simple
    {
        public long cid { get; set; }
        public Character_Simple charinfo { get; set; }
    }

    public class Account_Simple_With_Connection : Account_Simple_With_Character
    {
        public int friendlastconntime { get; set; }
    }

    public class Account_TownSimple
    {
        public long aid { get; set; }
        public long cid { get; set; }
        public string username { get; set; }
        public long guild_id { get; set; }
        public string guildname { get; set; }
        public int guild_mark{ get; set; }
        public Character_Simple_With_Equip charinfo { get; set; }

        public static string makeAccount_TownSimpleListJson(ref List<Account_TownSimple> setList)
        {
            //string json = "";

            //setList.ForEach(item =>
            //{
            //    string setObjJson = "";
            //    setObjJson = mJsonSerializer.RemoveJson(mJsonSerializer.ToJsonString(item), "charinfo");
            //    setObjJson = mJsonSerializer.AddJson(setObjJson, "charinfo", item.charinfo.ToJson());

            //    json = mJsonSerializer.AddJsonArray(json, setObjJson);
            //}
            //);

            //return json;

            JsonArrayObjects json = new JsonArrayObjects();
            const string removeKey = "charinfo";
            setList.ForEach(item =>
            {
                JsonObject setObjJson = JsonObject.Parse(mJsonSerializer.ToJsonString(item));
                if (setObjJson.ContainsKey(removeKey))
                    setObjJson.Remove(removeKey);
                setObjJson = mJsonSerializer.AddJson(setObjJson, removeKey, item.charinfo.ToJson());

                json = mJsonSerializer.AddJsonArray(json, setObjJson);
            }
            );

            return json.ToJson();
        }
    }
    

    public class Ret_Login_Info
    {
        public long aid { get; set; }
        public string accountname { get; set; }
        public int cash { get; set; }
        public int gold { get; set; }
        public int stone { get; set; }
        public int medal { get; set; }
        public int characterslot { get; set; }
        public int charactercount { get; set; }
        public int key { get; set; }
        public int keyfillmax { get; set; }
        public int keyremainchargesec { get; set; }
        public int ticket { get; set; }
        public int ticketfillmax { get; set; }
        public int ticketremainchargesec { get; set; }
        //public int challengeticket { get; set; }
        //public int challengeticketfillmax { get; set; }
        //public int challengeticketremainchargesec { get; set; }
        public int servertime { get; set; }
        public int honorpoint { get; set; }
        public int tutorial { get; set; }
        public string countrycode { get; set; }
        public string limitbuyitemview { get; set; }
        public int laststageid { get; set; }
        public string pvpfriendflag { get; set; }

        public RetUserVIP vipinfo { get; set; }

        public long guild_id { get; set; }
        public string guildname { get; set; }
        public string guildstate { get; set; }
        public string guildattend { get; set; }

        public int honor_point { get; set; }
        public int contribution_point { get; set; }
        public int partydungeon_point { get; set; }
        public int combat_point { get; set; }
        public int expedition_point { get; set; }
        public int blackmarket_point { get; set; }
        public int overlord_point { get; set; }
    }

    public class Ret_GM_Event
    {
        public float GoldBoostRate { get; set; }
        public float PCEXPBoostRate { get; set; }
    }

    public class ChatIgnore
    {
        public long aid { get; set; }
        public string username { get; set; }
    }

    public class RefreshNewFlag
    {
        public byte seven_day { get; set; }
        public byte mission { get; set; }
        public byte achive { get; set; }
        public byte giftevent { get; set; }
        public byte userevent { get; set; }
        public byte guild { get; set; }
        public byte mail { get; set; }
        public byte friend { get; set; }
        public byte shop { get; set; }
        public byte bossraid_active { get; set; }
        public byte bossraid_reward { get; set; }
        public byte black_market { get; set; }
        public byte coupon { get; set; }
        public byte ios_coupon { get; set; }
        public byte gacha_shop { get; set; }
        public byte pvp_guild { get; set; }
    }

    public class BestGachaInfo
    {
        public int gacha_cash { get; set; }
        public List<long> soulid_list { get; set; }

        public BestGachaInfo()
        {
            soulid_list = new List<long>();
        }
    }

    public class System_Tutorial_Step
    {
        public long Tutorial_Index { get; set; }
        public long Step { get; set; }
        public byte Type { get; set; }
        public long NextStep { get; set; }
        public int Save { get; set; }
        public int ServerSave { get; set; }
        public long BoxID_all { get; set; }
        public long BoxID1 { get; set; }
        public long BoxID2 { get; set; }
        public long BoxID3 { get; set; }
    }

    public class System_Tutorial_Reward : System_Package_RewardBox
    {
    }

    public class User_Tutorial
    {
        public long AID { get; set; }
        public string TutorialStepJson { get; set; }
    }

    public class Tutorial_Step
    {
        public long forced_tutorial { get; set; }
        public List<long> conditional_tutorial { get; set; }
        public Tutorial_Step()
        {
            forced_tutorial = 0;
            conditional_tutorial = new List<long>();
        }
    }

    public class User_Login_Count
    {
        public long AID { get; set; }
        public int TotalLoginCount { get; set; }
        public DateTime regdate { get; set; }

        public User_Login_Count(long setAID = 0)
        {
            AID = setAID;
            TotalLoginCount = 0;
            regdate = DateTime.Now.AddDays(-1);
        }
    }

    public class User_Coupon_Key
    {
        public string coupon_key { get; set; }
        public long aid { get; set; }
        public string platform_user_id { get; set; }
        public string coupon_type { get; set; }
        public string mailseq_json { get; set; }
        public string stateflag { get; set; }
        public DateTime regdate { get; set; }
        public DateTime updatedate { get; set; }

        public User_Coupon_Key()
        {
            coupon_key = platform_user_id = stateflag = "";
            regdate = updatedate = DateTime.Now;
        }
    }
}
