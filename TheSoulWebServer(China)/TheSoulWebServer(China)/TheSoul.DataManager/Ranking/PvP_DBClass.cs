using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TheSoul.DataManager.DBClass
{
    public class User_PVP_Overlord_Ranking
    {
        public long Ranking { get; set; }
        public long AID { get; set; }
        public byte Flag { get; set; }
        public byte isNPC { get; set; }
        public DateTime update_date { get; set; }

        public User_PVP_Overlord_Ranking() { update_date = DateTime.Now.AddMinutes(PvP_Define.Overlord_Play_Time_Min * -1); }
    }

    public class User_PVP_Overlord_Record
    {
        public long rec_idx { get; set; }
        public long AID { get; set; }
        public byte win { get; set; }
        public int beforeRank { get; set; }
        public int afterRank { get; set; }
        public long challengerAID { get; set; }
        public string challengerUsername { get; set; }
        public long challengerRank { get; set; }
        public int challengerClasstype { get; set; }
        public int challengerLevel { get; set; }               
        public DateTime updateDate { get; set; }

        public User_PVP_Overlord_Record() { challengerUsername = ""; }
    }
    
    public class User_PVP_Overlord_ReadRecord
    {
        public long AID { get; set; }
        public long rec_idx { get; set; }
    }

    public class User_PvP_Play
    {
        public long aid { get; set; }
        public byte pvp_type { get; set; }
        public short play_count { get; set; }
        public short play_resetcount { get; set; }
        public DateTime? regdate { get; set; }
        public int map_index { get; set; }
    }

    public class User_PvP_Play_Info
    {
        public int pvp_type { get; set; }
        public long play_count { get; set; }
        public long max_play_count { get; set; }
        public short reset_count { get; set; }
        public long map_index { get; set; }
    }

    public class User_PvP_Play_Count
    {
        public long play_count { get; set; }
        public long max_play_count { get; set; }
        public long total_rank_player { get; set; }

        public User_PvP_Play_Count(User_PvP_Play_Info setInfo)
        {
            play_count = setInfo.play_count;
            max_play_count = setInfo.max_play_count;
            total_rank_player = 0;
        }
    }

    public class User_PartyDungeon_Clear
    {
        public int map_index { get; set; }
        public byte clear { get; set; }
    }

    public class Ret_Overlord_Record
    {
        public long aid { get; set; }
        public byte win { get; set; }
        public int before_rank { get; set; }
        public int after_rank { get; set; }
        public long challenger_aid { get; set; }
        public string challenger_username { get; set; }
        public long challenger_rank { get; set; }
        public int challenger_class { get; set; }
        public int challenger_level { get; set; }   
        public static Ret_Overlord_Record CastToRet_Overlord_Record(User_PVP_Overlord_Record setInfo)
        {
            Ret_Overlord_Record setObj = new Ret_Overlord_Record();
            setObj.aid = setInfo.AID;
            setObj.win = setInfo.win;
            setObj.before_rank = setInfo.beforeRank;
            setObj.after_rank = setInfo.afterRank;
            setObj.challenger_aid = setInfo.challengerAID;
            setObj.challenger_username = setInfo.challengerUsername;
            setObj.challenger_rank = setInfo.challengerRank;
            setObj.challenger_class = setInfo.challengerClasstype;
            setObj.challenger_level = setInfo.challengerLevel;
            return setObj;
        }
    }

    public class Ret_Overlord_User_Info
    {
        public long my_ranking { get; set; }
        public long play_count { get; set; }
        public long max_play_count { get; set; }
        public long total_rank_player { get; set; }

        public Ret_Overlord_User_Info(User_PvP_Play_Info setInfo)
        {
            play_count = setInfo.play_count;
            max_play_count = setInfo.max_play_count;
            total_rank_player = 0;
        }
    }

    public class PvP_GuildWar_Result
    {
        public long my_gid { get; set; }
        public long enemy_gid { get; set; }
        public byte match_rank { get; set; }
        public int my_rating { get; set; }
        public int enemy_rating { get; set; }
    }

    public class PvP_Player_Result
    {
        public long gid { get; set; }
        public long aid { get; set; }
        public byte match_rank { get; set; }
        public int kill_count { get; set; }
        public int death_count { get; set; }
        public int kill_streak { get; set; }
        public int death_streak { get; set; }
    }

    public class PVP_Record
    {
        public long aid { get; set; }
        public string user_nick { get; set; }
        public int totalkill { get; set; }
        public int totaldeath { get; set; }
        public int totalhonorpoint { get; set; }
        public int weekkill { get; set; }
        public int weekdeath { get; set; }
        public int weekhonorpoint { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime update_date { get; set; }
        public byte pvp_type { get; set; }
        public int straightwin { get; set; }
        public int straightlose { get; set; }
        public string dailycashgetyn { get; set; }
        public int seperater { get; set; }
        public int updateTimeRedisValue { get; set; }

        public PVP_Record() { dailycashgetyn = ""; }
        public PVP_Record(long setAID = 0, int setSeper = 0)
        {
            aid = setAID;
            seperater = setSeper;
            user_nick = string.Empty;
            update_date = creation_date = DateTime.Now;
            straightwin = straightlose = 0;
            dailycashgetyn = "";
        }
    }

    public class PvP_Reward_Info
    {

    }

    public class Guild_PVP_Record
    {
        public long gid { get; set; }
        public int win { get; set; }
        public int lose { get; set; }
        public int rankpoint { get; set; }
        public int straightwin { get; set; }
        public int straightlose { get; set; }
        public int seperater { get; set; }
        public int updateTimeRedisValue { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime update_date { get; set; }

        public Guild_PVP_Record(long setGID = 0, int setSeper = 0)
        {
            gid = setGID;
            seperater = setSeper;
            update_date = creation_date = DateTime.Now;
            straightwin = straightlose = 0;
        }
    }

    public class Ret_PvP_Record_Result
    {
        public long aid { get; set; }
        public int before_rankpoint { get; set; }
        public int after_rankpoint { get; set; }
        public int price_type { get; set; }
        public int add_user_value { get; set; }
        public int total_user_value { get; set; }
        public int straightwin { get; set; }
        public int straightlose { get; set; }

        public Ret_PvP_Record_Result(long setAid)
        {
            aid = setAid;
        }

        public Ret_PvP_Record_Result(long setAid, int set_before_rankpoint, int set_after_rankpoint, int set_price_type, int addvalue  = 0, int total_value = 0, int setwin = 0, int setlose = 0)
        {
            aid = setAid;
            before_rankpoint = set_before_rankpoint;
            after_rankpoint = set_after_rankpoint;
            price_type = set_price_type;
            add_user_value = addvalue;
            total_user_value = total_value;
            straightlose = setlose;
            straightwin = setwin;
        }
    }

    public class Guild_User_PVP_Record
    {
        public long aid { get; set; }
        public long gid { get; set; }
        public string user_nick { get; set; }
        public int totalkill { get; set; }
        public int totaldeath { get; set; }
        public int totalhonorpoint { get; set; }
        public int totalwin { get; set; }
        public int totallose{ get; set; }
        public int rating_point { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime update_date { get; set; }
        public DateTime lastjoin_date { get; set; }
        public int straightwin { get; set; }
        public int straightlose { get; set; }
        public string dailycashgetyn { get; set; }
        public int seperater { get; set; }
        public int updateTimeRedisValue { get; set; }

        public Guild_User_PVP_Record() { }
        public Guild_User_PVP_Record(long setAID, long setGID, int setSeper)
        {
            aid = setAID;
            gid = setGID;
            seperater = setSeper;
            user_nick = string.Empty;
            update_date = creation_date = DateTime.Now;
            lastjoin_date = DateTime.Now.AddDays(-1);
            dailycashgetyn = "N";
            straightwin = straightlose = 0;
        }
    }

    public class GuildPvP_DateTime
    {
        public DateTime lastjoin_date { get; set; }
        public GuildPvP_DateTime() { lastjoin_date = DateTime.Now.AddDays(-1); }
    }

    public class PVP_GuildWarRecord
    {
        public long guildid { get; set; }
        public byte pvp_type { get; set; }
        public int totalwin { get; set; }
        public int totallose { get; set; }
        public int totalpoint { get; set; }
        public int weekwin { get; set; }
        public int weeklose { get; set; }
        public int weekpoint { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime update_date { get; set; }
        public int seperater { get; set; }
        public int updateTimeRedisValue { get; set; }
    }

    public class RankingBase
    {
        public long aid { get; set; }
        public string username { get; set; }
        public double point { get; set; }
        public long rank { get; set; }
    }

    public class Ret_PvP : RankingBase
    {
        public int Class { get; set; }
        public int level { get; set; }
        public long first_value { get; set; }
        public long second_value { get; set; }
    }

    public class Ret_OverLord
    {
        public long aid { get; set; }
        public string username { get; set; }
        public long rank { get; set; }
        public int classtype { get; set; }
        public int level { get; set; }
        public long warpoint { get; set; }
        public byte playflag { get; set; }
        public byte isnpc { get; set; }
    }

    public class Ret_GuildWarPvP : RankingBase
    {
        public long gid { get; set; }
        public string guild_name { get; set; }
        public int guild_level { get; set; }
        public int guild_mark { get; set; }
        public long first_value { get; set; }
        public long second_value { get; set; }
    }

    public class Ret_GuildPvP : RankingBase
    {
        public long gid { get; set; }
        public string guild_name { get; set; }
        public int guild_level { get; set; }
        public int guild_mark { get; set; }
        public double last_point { get; set; }
    }

    public class Ret_GuildWarJoinerPvP
    {
        public double point { get; set; }
        public long first_value { get; set; }
        public long second_value { get; set; }

        public Ret_GuildWarJoinerPvP() { }
        public Ret_GuildWarJoinerPvP(Guild_User_PVP_Record setInfo)
        {
            point = setInfo.rating_point;
            first_value = setInfo.totalwin;
            second_value= setInfo.totallose;
        }
    }

    public class PvP_WarPoint
    {
        public long aid { get; set; }
        public long cid { get; set; }
        public long warpoint { get; set; }
    }

    public class PvP_GuildPoint
    {
        public long GuildID { get; set; }
        public long GuildRankingPoint { get; set; }
    }

    public class PvP_Grade
    {
        public int high_grade { get; set; }
    }
    
    public class Rank_Count
    {
        public long count{ get; set; }
    }

    public class PvP_OpenTime
    {
        public int pvp_type { get; set; }
        public int starttime { get; set; }
        public int endtime { get; set; }

        public PvP_OpenTime() { }
        public PvP_OpenTime(PvP_Define.ePvPType pvpType, int setStart, int setEnd)
        {
            pvp_type = (int)pvpType;
            starttime = setStart;
            endtime = setEnd;
        }
    }

    public class ServerCreate_RankingReward
    {
        public long AID { get; set; }
        public byte pvp_type { get; set; }
        public byte reward_type { get; set; }
        public string get_reward { get; set; }
        public DateTime reward_date { get; set; }
        public int rank { get; set; }
        public int divReward { get; set; }
    }

    public class System_Battle_Reward
    {
        public int ID { get; set; }
        public string Desc { get; set; }
        public string Battle_Type { get; set; }
        public int Index { get; set; }
        public string Ranking_Type { get; set; }
        public byte Day_Type { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public long Reward1_Type { get; set; }
        public int Reward1_Value { get; set; }
        public long Reward2_Type { get; set; }
        public int Reward2_Value { get; set; }
    }

    public class RetBattleReward
    {
        public int Index { get; set; }
        public int PVP_TYPE { get; set; }
        public byte Day_Type { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public long Reward1_Type { get; set; }
        public int Reward1_Value { get; set; }
        public long Reward2_Type { get; set; }
        public int Reward2_Value { get; set; }

        public RetBattleReward() { }
        public RetBattleReward(System_Battle_Reward setReward) 
        {
            if (PvP_Define.PvPBattleReawrd_PvPTypeList.ContainsKey(setReward.Battle_Type))
                PVP_TYPE = (int)PvP_Define.PvPBattleReawrd_PvPTypeList[setReward.Battle_Type];
            Index = setReward.Index;
            Day_Type = setReward.Day_Type;
            MinValue = setReward.MinValue;
            MaxValue = setReward.MaxValue;
            Reward1_Type = setReward.Reward1_Type;
            Reward1_Value = setReward.Reward1_Value;
            Reward2_Type = setReward.Reward2_Type;
            Reward2_Value = setReward.Reward2_Value;
        }
    }

    public class System_Rankup_Reward
    {
        public long RankUp_Reward_Idx { get; set; }
        public string Battle_Type { get; set; }
        public byte Day_Type { get; set; }
        public int StartRank { get; set; }
        public int EndRank { get; set; }
        public string Reward_Type { get; set; }
        public int Reward_Num { get; set; }
    }
}