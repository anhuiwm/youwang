using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSoul.DataManager.DBClass
{
    public class Guild
    {
        public long cid { get; set; }
        public long guild_id { get; set; }
        public string guild_name { get; set; }
        public int guild_mark { get; set; }
        public string guild_state { get; set; }
        public string guild_attend { get; set; }
    }

    public class GuildSoldItem
    {
        public int num { get; set; }
        public int itemcode { get; set; }
    }

    public class GuildUser
    {
        public long joinerAID { get; set; }
        public long JoinerRankingPoint { get; set; }
        public int Joinerlastconntime { get; set; }
        public string JoinerUserName { get; set; }
        public int JoinerLV { get; set; }
        public int JoinerEquipClass { get; set; }
        public long JoinerDonationExp { get; set; }
        public long TodayDonationExp { get; set; }
        public string isJoinerAttend { get; set; }
        //public long JoinerDonationPoint;
    }

    public class System_Guild
    {
        public int Level { get; set; }
        public long NeedExp { get; set; }
        public int Max_Member { get; set; }
        public long Guild_Buff_Id { get; set; }
    }

    public class Guild_GuildCreation
    {
        public long GuildID { get; set; }
        public string GuildName { get; set; }
        public long GuildCreateAID { get; set; }
        public string GuildCreateUserName { get; set; }
        public DateTime GuildCreateDate { get; set; }
        public DateTime? GuildDeleteDate { get; set; }
        public string GuildIntroduce { get; set; }
        public DateTime? GuildIntroduceModifyDate { get; set; }
        public string GuildNotice { get; set; }
        public DateTime? GuildNoticeModifyDate { get; set; }
        public int GuildMark { get; set; }
        public int GuildLevel { get; set; }
        public long GuildExp { get; set; }
        public long GuildWithdrawExp { get; set; }
        public long GuildRankingPoint { get; set; }
        public long GuildWithdrawPoint { get; set; }
        public byte GuildState { get; set; }
        public DateTime? StateChangeTime { get; set; }
        public int YesterdayAttendCheck { get; set; }
        public int? GuildExpbuff { get; set; }
        public int? GuildSkillBuff { get; set; }
        public int GuildWaitingCount { get; set; }
        public long Guild3vs3Point { get; set; }
    }

    // guildranking point per week - ADD by Manstar 2015/08/18
    public class System_GuildRanking_Data
    {
        public long gid { get; set; }
        public int weekGuildRankPoint { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime update_date { get; set; }
        public string dailycashgetyn { get; set; }
        public int seperater { get; set; }
    }


    public class GuildJoiner
    {
        public long GuildID { get; set; }
        public long JoinerAID { get; set; }
        public long GuildCreateAID { get; set; }
        public long JoinerExp { get; set; }
        public long JoinerPoint { get; set; }
        public string JoinerState { get; set; }
        public DateTime? JoinerCreateDate { get; set; }
        public DateTime? JoinerWaitDate { get; set; }
        public DateTime? JoinerDeleteDate { get; set; }
        public DateTime? JoinerbanishDate { get; set; }
        public DateTime? YesterdayAttendDate { get; set; }
        public DateTime? TodayAttendDate { get; set; }
        public int Joiner3vs3Point { get; set; }
        public int JoinerTotal3vs3Point { get; set; }
        public int EntrustState { get; set; }
        public DateTime? EntrustAskDate { get; set; }
        public int JoinerDonationExp { get; set; }
        public int JoinerDonationPoint { get; set; }
        public DateTime? JoinerDonationDate { get; set; }
        public int TodayDonationExp { get; set; }
        public byte? JoinerGrade { get; set; }
    }

    public class DonateInfo
    {
        public long joinerAID { get; set; }
        public long TodayDonationExp { get; set; }
    }

    public class Sample_GuildJoiner
    {
        public long joinerAID { get; set; }
        public long joinerPoint { get; set; }
        public string JoinerName { get; set; }
        public int JoinerLevel { get; set; }
        public int ClassType { get; set; }
        public int Lastconntime { get; set; }
        public long JoinerDonationExp { get; set; }
        public long TodayDonationExp { get; set; }        
        public string isJoinerAttend { get; set; }
    }

    public class Ret_MyGuild_Info
    {
        public long GuildID { get; set; }
        public string GuildName { get; set; }
        public long MasterAid { get; set; }
        public string MasterName { get; set; }
        public string GuildIntroduce { get; set; }
        public long GuildIntroduceupdatetime { get; set; }
        public string GuildNotice { get; set; }
        public long GuildNoticeupdatetime { get; set; }
        public int GuildMark { get; set; }
        public int GuildLevel { get; set; }
        public byte GuildState { get; set; }
        public long GuildExp { get; set; }
        public long GuildRankingPoint { get; set; }
        public int GuildUsercnt { get; set; }
        public int GuildMaxUsercnt { get; set; }
        public int YesterdayCheck { get; set; }
        public int TodayCheck { get; set; }
        public string IsAttend { get; set; }
        public long GuildRank { get; set; }
        public bool MyGuildInfo { get; set; }
        public long GuildExpbuff { get; set; }
        public int GuildSkillbuff { get; set; }
        public int GuildDonationpoint { get; set; }
        public string IsDonate { get; set; }
        public List<Sample_GuildJoiner> GuildJoiner { get; set; }
        public List<DonateInfo> donationlist { get; set; }
    }
    
    public class Ret_Guild_Info
    {
        public long GuildID { get; set; }
        public string GuildName { get; set; }
        public long MasterAid { get; set; }
        public string MasterName { get; set; }
        public string GuildIntroduce { get; set; }
        public string GuildNotice { get; set; }
        public int GuildMark { get; set; }
        public int GuildLevel { get; set; }
        public byte GuildState { get; set; }
        public long GuildExp { get; set; }
        public long GuildRankingPoint { get; set; }
        public int GuildUsercnt { get; set; }
        public int GuildMaxUsercnt { get; set; }
        public long GuildRank { get; set; }
        public bool MyGuildInfo { get; set; }
        public List<Sample_GuildJoiner> GuildJoiner { get; set; }
        public List<DonateInfo> donationlist { get; set; }
    }

    public class ManagedJoiner
    {
        public long JoinerAid { get; set; }
        public long JoinerPoint { get; set; }
        public string JoinerName { get; set; }
        public int ClassType { get; set; }
        public int JoinerLevel { get; set; }
        public int LastConntime { get; set; }

    }

    public class GuildRecommend
    {
        public int IDX { get; set; }
        public long GuildID { get; set; }
        public string GuildName { get; set; }
        public string MasterName { get; set; }
        public int GuildMark { get; set; }
        public int GuildLevel { get; set; }
        public int GuildState { get; set; }
        public string Guildintroduce { get; set; }
    }

    public class GuildCount
    {
        public int count { get; set; }
    }
}
