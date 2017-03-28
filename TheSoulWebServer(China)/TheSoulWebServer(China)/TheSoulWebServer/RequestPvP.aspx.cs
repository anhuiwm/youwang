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

namespace TheSoulWebServer
{
    public partial class RequestPvP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] ops = new string[] {
                // all PvP
                "pvp_rank",
                "pvp_rank_lastweek",
                "friend_rank",
                
                //// 1vs1 PvP   // not use yet. instead all pvp rank use op=pvp_rank
                //"1vs1pvp_rank",
                //"1vs1pvp_rank_lastweek",

                //guildwar PvP
                "guildwarpvp_rank",
                "guildwarpvp_rank_lastweek",

                // charater ranking - not pvp
                "character_rank",

                // guild ranking - not pvp
                "guild_rank",

                // update chracter warpoint
                "set_warpoint",

                // pvp open time list
                "get_pvp_open",

                // party dungeon sweep 
                "party_dungeon_clearlist",
                "party_dungeon_sweep",
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
                        if (requestOp.Equals("pvp_rank") || requestOp.Equals("pvp_rank_lastweek"))
                        {
                            PvP_Define.ePvPType setPvPType = (PvP_Define.ePvPType)queryFetcher.QueryParam_FetchInt("pvp_type", (int)PvP_Define.ePvPType.MATCH_FREE);
                            int get_rankPos = queryFetcher.QueryParam_FetchInt("rank_pos");
                            int get_rankCount = queryFetcher.QueryParam_FetchInt("rank_count", PvP_Define.PvP_RankRange);

                            int seperaterWeekOrSeason = PvPManager.GetSeperater(ref tb, setPvPType);
                            retError = PvPManager.SetUserPvP_Rank_Info(ref tb, AID, seperaterWeekOrSeason, setPvPType);

                            if (requestOp.Equals("pvp_rank_lastweek") && 1 < seperaterWeekOrSeason)//난전,길드전만 지난주 랭킹 요청 가능임.
                                seperaterWeekOrSeason -= 1;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Ret_PvP my_PvP_Rank = PvPManager.GetUser_PvP_Rank_Info(ref tb, AID, seperaterWeekOrSeason, setPvPType);
                                Ret_PvP my_PvP_Last_Rank = PvPManager.GetUser_PvP_Rank_Info(ref tb, AID, seperaterWeekOrSeason - 1, setPvPType);

                                get_rankPos = get_rankPos < 1 ? System.Convert.ToInt32(my_PvP_Rank.rank) : get_rankPos;
                                get_rankPos = get_rankPos - (get_rankCount / 2) < 0 ? 1 : get_rankPos - (get_rankCount / 2);
                                get_rankPos = get_rankPos < 1 ? 0 : get_rankPos - 1;   // set start pos

                                List<Ret_PvP> top_PvP_Rank_List = PvPManager.GetUser_PvP_Rank_List(ref tb, AID, seperaterWeekOrSeason, setPvPType, get_rankPos, get_rankPos + get_rankCount).OrderBy(item => item.rank).ToList();
                                User_PvP_Play_Count userPvPInfo = new User_PvP_Play_Count(PvPManager.GetUser_PvPInfo(ref tb, AID, setPvPType));

                                var CheckRank = top_PvP_Rank_List.Find(rankitem => rankitem.aid == AID);
                                if (CheckRank != null)
                                    my_PvP_Rank = CheckRank;
                                byte achiveRewardNewFlag = PvPManager.CheckPvPAchiveReward(ref tb, AID, setPvPType);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    userPvPInfo.total_rank_player = PvPManager.GetTotal_PvP_Rank_Player(ref tb, seperaterWeekOrSeason, setPvPType);
                                    json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_PlayInfo], mJsonSerializer.ToJsonString(userPvPInfo));
                                    json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Rank], mJsonSerializer.ToJsonString(my_PvP_Rank));
                                    json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_PlayInfo_LastWeek], mJsonSerializer.ToJsonString(my_PvP_Last_Rank));
                                    json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Top_Rank], mJsonSerializer.ToJsonString(top_PvP_Rank_List));

                                    // check new flag pvp achive reward
                                    json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Achive_NewFlag], achiveRewardNewFlag.ToString());
                                }
                            }
                        }
                        if (requestOp.Equals("friend_rank"))
                        {
                            PvP_Define.ePvPType setPvPType = (PvP_Define.ePvPType)queryFetcher.QueryParam_FetchInt("pvp_type", (int)PvP_Define.ePvPType.MATCH_FREE);

                            List<long> friendAIDList = FriendManager.GetFriend_AID_List(ref tb, AID);
                            friendAIDList.Add(AID);
                            List<Ret_PvP> friend_Rank_List = new List<Ret_PvP>();

                            int seperaterWeekOrSeason = PvPManager.GetSeperater(ref tb, setPvPType);
                            long TotalRank = PvPManager.GetTotal_PvP_Rank_Player(ref tb, seperaterWeekOrSeason, setPvPType) + 1;
                            friendAIDList.ForEach(FAID =>
                            {
                                Ret_PvP getInfo = PvPManager.GetUser_PvP_Rank_Info(ref tb, FAID, seperaterWeekOrSeason, setPvPType);
                                if (getInfo.rank == 0)
                                    getInfo.rank = TotalRank;
                                //if(getInfo.rank > 0)
                                friend_Rank_List.Add(getInfo);
                            });
                            friend_Rank_List = friend_Rank_List.OrderBy(item => item.rank).ThenByDescending(item => item.level).ToList();
                            retError = Result_Define.eResult.SUCCESS;
                            json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Friend_Rank], mJsonSerializer.ToJsonString(friend_Rank_List));
                        }
                        else if (requestOp.Equals("set_warpoint"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long CID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("cid"));
                            int warpoint = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("warpoint"));
                            Character_Stat charStat = mJsonSerializer.JsonToObject<Character_Stat>(queryFetcher.QueryParam_Fetch("stat", ""));
                            charStat.WAR_POINT = warpoint;
                            PvP_WarPoint setWarPoint = CharacterManager.GetCharacterWarpoint(ref tb, CID);

                            //retError = CharacterManager.UpdateCharacterWarpoint(ref tb, AID, CID, warpoint);
                            //if (retError == Result_Define.eResult.SUCCESS)
                            //    retError = CharacterManager.UpdateCharacterStat(ref tb, AID, CID, ref charStat);
                            //Character charInfo = CharacterManager.FlushCharacter(ref tb, AID, CID);

                            PvPManager.SetUser_PvP_Warpoint(ref setWarPoint);
                        }
                        else if (requestOp.Equals("character_rank"))
                        {
                            long CID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("cid"));
                            int req_rank_type = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("rank_type", ((int)(PvP_Define.eCharacterRankType.WARPOINT)).ToString()));

                            PvP_Define.eCharacterRankType setPvPType = (PvP_Define.eCharacterRankType)req_rank_type;

                            //Character charInfo = CharacterManager.GetCharacter(ref tb, AID, CID);
                            PvP_WarPoint setWarPoint = CharacterManager.GetCharacterWarpoint(ref tb, CID);

                            retError = PvPManager.SetUser_PvP_Warpoint(ref setWarPoint);
                            long totalPlayer = 0;
                            Ret_PvP my_PvP_Rank = null;
                            List<Ret_PvP> top_PvP_Rank_List = null;
                            if (setPvPType == PvP_Define.eCharacterRankType.WARPOINT)
                            {
                                my_PvP_Rank = PvPManager.GetUser_PvP_Warpoint_Rank_Info(ref tb, ref setWarPoint, AID, ref totalPlayer);
                                top_PvP_Rank_List = PvPManager.GetUser_PvP_Warpoint_Rank_List(ref tb).OrderBy(item => item.rank).ToList();
                            }
                            else
                            {
                                my_PvP_Rank = new Ret_PvP();
                                top_PvP_Rank_List = new List<Ret_PvP>();
                            }
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Rank], mJsonSerializer.ToJsonString(my_PvP_Rank));
                                json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Top_Rank], mJsonSerializer.ToJsonString(top_PvP_Rank_List));
                                json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_TotalPlayer], mJsonSerializer.ToJsonString(totalPlayer));
                            }
                        }
                        else if (requestOp.Equals("guild_rank"))
                        {
                            long CID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("cid"));
                            int req_rank_type = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("rank_type", ((int)(PvP_Define.eGuildRankType.GUILDPOINT)).ToString()));
                            long GID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("gid", "0"));
                            string GuildName = queryFetcher.QueryParam_Fetch("guild_name", string.Empty);
                            int get_rankCount = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("rank_count", PvP_Define.PvP_GuilRankRange.ToString()));

                            Guild_GuildCreation searchGuildInfo = null;
                            if (!string.IsNullOrEmpty(GuildName) && GID < 1)
                            {
                                searchGuildInfo = GuildManager.GetSearchGuildInfo_ByName(ref tb, GuildName);
                                GID = searchGuildInfo.GuildID;
                                if (string.IsNullOrEmpty(searchGuildInfo.GuildName))
                                    retError = Result_Define.eResult.GUILD_RANK_SEARCH_NOEXIST_INFO;
                                else
                                    retError = Result_Define.eResult.SUCCESS;
                            }
                            else
                                retError = Result_Define.eResult.SUCCESS;

                            PvP_Define.eGuildRankType setPvPType = (PvP_Define.eGuildRankType)req_rank_type;

                            long totalPlayer = 0;
                            int get_rankPos = 0;

                            if (setPvPType == PvP_Define.eGuildRankType.GUILDPOINT)
                            {
                                if (GID > 0)
                                {
                                    Ret_GuildPvP CurrentRank = PvPManager.GetUser_PvP_Guild_Rank_Info(ref tb, GID, ref totalPlayer);
                                    get_rankPos = System.Convert.ToInt32(CurrentRank.rank);
                                    get_rankCount = PvP_Define.PvP_GuilRankSearchRange;
                                }
                                else if (!string.IsNullOrEmpty(GuildName) && GID < 1)
                                {
                                    get_rankPos = 0;
                                    get_rankCount = 0;
                                }

                                //Ret_GuildPvP my_PvP_Rank = PvPRankingManager.GetUser_PvP_Guild_Rank_Info(ref tb, GID, ref totalPlayer);

                                //get_rankPos = get_rankPos < 1 ? System.Convert.ToInt32(my_PvP_Rank.rank) : get_rankPos;
                                if(get_rankPos == 0 && GID > 0)
                                    retError = Result_Define.eResult.GUILD_RANK_SEARCH_NOEXIST_INFO;
                                get_rankPos = get_rankPos - (get_rankCount / 2) <= 0 ? 1 : get_rankPos - (get_rankCount / 2);
                                get_rankPos -= 1;   // set start pos

                                List<Ret_GuildPvP> top_PvP_Rank_List = get_rankCount > 0 ? PvPManager.GetUser_PvP_Guild_Rank_List(ref tb, get_rankPos, (get_rankPos + get_rankCount)).OrderBy(item => item.rank).ToList() : new List<Ret_GuildPvP>();

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    //json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Rank], mJsonSerializer.ToJsonString(my_PvP_Rank));
                                    json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Top_Rank], mJsonSerializer.ToJsonString(top_PvP_Rank_List));
                                    //json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_TotalPlayer], mJsonSerializer.ToJsonString(totalPlayer));
                                }
                            }
                        }
                        else if (requestOp.Equals("guildwarpvp_rank") || requestOp.Equals("guildwarpvp_rank_lastweek"))
                        {
                            long GID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("gid", "0"));
                            string GuildName = queryFetcher.QueryParam_Fetch("guild_name", string.Empty);

                            Guild_GuildCreation searchGuildInfo = null;
                            if (!string.IsNullOrEmpty(GuildName) && GID < 1)
                            {
                                searchGuildInfo = GuildManager.GetSearchGuildInfo_ByName(ref tb, GuildName);
                                GID = searchGuildInfo.GuildID;
                            }

                            long totalPlayer = 0;
                            int get_rankPos = 0;
                            int get_rankCount = 100;
                            
                            retError = PvPManager.SetGuildPvP_GuildWar_Rank_Info(ref tb, GID);

                            retError = Result_Define.eResult.SUCCESS;
                            long latweektotalPlayer = 0;
                            int seperaterWeekOrSeason = PvPManager.GetSeperater(ref tb, PvP_Define.ePvPType.MATCH_GUILD_3VS3);
                            byte achiveRewardNewFlag = PvPManager.CheckPvPAchiveReward(ref tb, AID, PvP_Define.ePvPType.MATCH_GUILD_3VS3);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Ret_GuildWarPvP myGuildWarRank = PvPManager.GetGuildPvP_GuildWar_Rank_Info(ref tb, GID, ref totalPlayer, seperaterWeekOrSeason);
                                Ret_GuildWarPvP myGuildWarRank_LastWeek = PvPManager.GetGuildPvP_GuildWar_Rank_Info(ref tb, GID, ref latweektotalPlayer, seperaterWeekOrSeason - 1);

                                Ret_GuildWarJoinerPvP myGuildWarPlayInfo = PvPManager.GetUser_GuildWar_Rank_Info(ref tb, AID, GID, seperaterWeekOrSeason);
                                Ret_GuildWarJoinerPvP myGuildWarPlayInfo_LastWeek = PvPManager.GetUser_GuildWar_Rank_Info(ref tb, AID, GID, seperaterWeekOrSeason - 1);

                                if (requestOp.Equals("guildwarpvp_rank_lastweek"))//난전,길드전만 지난주 랭킹 요청 가능임.
                                    seperaterWeekOrSeason -= 1;

                                List<Ret_GuildWarPvP> top_PvP_Rank_List = PvPManager.GetGuildPvP_GuildWar_Rank_List(ref tb, get_rankPos, get_rankCount, seperaterWeekOrSeason).OrderBy(item => item.rank).ToList();
                                User_PvP_Play_Count userPvPInfo = new User_PvP_Play_Count(PvPManager.GetUser_PvPInfo(ref tb, AID, PvP_Define.ePvPType.MATCH_GUILD_3VS3));
                                long userGID = GuildManager.GetGuildInfo(ref tb, AID).guild_id;
                                Guild_User_PVP_Record getInfo = PvPManager.GetGuild_User_PvP_Record(ref tb, AID, userGID, PvP_Define.PvP_GuildUser_Monthly_TableName, 0, true);
                                DateTime checkTime = getInfo.lastjoin_date.AddMinutes(SystemData.GetConstValueInt(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_GUILD_G3VS3_COOLTIME_MINUTE]));
                                TimeSpan TS = checkTime - DateTime.Now;
                                int RejoinLeftTime = TS.TotalSeconds > 0 ? (int)TS.TotalSeconds : 0;
                                userPvPInfo.total_rank_player = totalPlayer;
                                json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_PlayInfo], mJsonSerializer.ToJsonString(userPvPInfo));
                                json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_GuildWarRank], mJsonSerializer.ToJsonString(myGuildWarRank));
                                json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_GuildWarMyPlayInfo], mJsonSerializer.ToJsonString(myGuildWarPlayInfo));
                                json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_GuildWarRank_LastWeek], mJsonSerializer.ToJsonString(myGuildWarRank_LastWeek));
                                json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_GuildWarMyPlayInfo_LastWeek], mJsonSerializer.ToJsonString(myGuildWarPlayInfo_LastWeek));
                                json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Top_Rank], mJsonSerializer.ToJsonString(top_PvP_Rank_List));
                                json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_GuildRejoinTime], mJsonSerializer.ToJsonString(RejoinLeftTime));
                                // check new flag pvp achive reward
                                json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Achive_NewFlag], achiveRewardNewFlag.ToString());
                            }
                        }
                        else if (requestOp.Equals("get_pvp_open"))
                        {
                            List<PvP_OpenTime> setOpenTime = new List<PvP_OpenTime>();

                            // 1vs1
                            setOpenTime.Add(new PvP_OpenTime(PvP_Define.ePvPType.MATCH_1VS1,
                                                            SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_1VS1REAL_START_TIME]),
                                                            SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_1VS1REAL_END_TIME])));
                            setOpenTime.Add(new PvP_OpenTime(PvP_Define.ePvPType.MATCH_1VS1,
                                                            SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_1VS1REAL_START_TIME_BONUS]),
                                                            SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_1VS1REAL_END_TIME_BOUNS])));
                            // free for all
                            setOpenTime.Add(new PvP_OpenTime(PvP_Define.ePvPType.MATCH_FREE,
                                                            SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_BATTLE_FREEFORALL_START_TIME_1st]),
                                                            SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_BATTLE_FREEFORALL_END_TIME_1st])));
                            setOpenTime.Add(new PvP_OpenTime(PvP_Define.ePvPType.MATCH_FREE,
                                                            SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_BATTLE_FREEFORALL_START_TIME_2nd]),
                                                            SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_BATTLE_FREEFORALL_END_TIME_2nd])));
                            // ruby pvp
                            setOpenTime.Add(new PvP_OpenTime(PvP_Define.ePvPType.MATCH_RUBY_PVP,
                                                            SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_PVP_GLADIATOR_START_TIME_1st]),
                                                            SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_PVP_GLADIATOR_END_TIME_1st])));
                            setOpenTime.Add(new PvP_OpenTime(PvP_Define.ePvPType.MATCH_RUBY_PVP,
                                                            SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_PVP_GLADIATOR_START_TIME_2nd]),
                                                            SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_PVP_GLADIATOR_END_TIME_2nd])));
                            // guild pvp
                            setOpenTime.Add(new PvP_OpenTime(PvP_Define.ePvPType.MATCH_GUILD_3VS3,
                                                            SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_BATTLE_GUILD_G3VS3_START_TIME_1st]),
                                                            SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_BATTLE_GUILD_G3VS3_END_TIME_1st])));
                            setOpenTime.Add(new PvP_OpenTime(PvP_Define.ePvPType.MATCH_GUILD_3VS3,
                                                            SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_BATTLE_GUILD_G3VS3_START_TIME_2nd]),
                                                            SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_BATTLE_GUILD_G3VS3_END_TIME_2nd])));
                            // party dungeon
                            setOpenTime.Add(new PvP_OpenTime(PvP_Define.ePvPType.MATCH_PARTY,
                                                            SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_PVP_PARTY_START_TIME_1st]),
                                                            SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_PVP_PARTY_END_TIME_1st])));
                            setOpenTime.Add(new PvP_OpenTime(PvP_Define.ePvPType.MATCH_PARTY,
                                                            SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_PVP_PARTY_START_TIME_2nd]),
                                                            SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_PVP_PARTY_END_TIME_2nd])));

                            retError = Result_Define.eResult.SUCCESS;

                            long currentTime = (long)(DateTime.Now - DateTime.Parse(DateTime.Now.ToShortDateString())).TotalSeconds;

                            json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_ServerTime], mJsonSerializer.ToJsonString(currentTime));
                            json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_OpenTime], mJsonSerializer.ToJsonString(setOpenTime));
                        }
                        else if (requestOp.Equals("party_dungeon_clearlist"))
                        {
                            retError = Result_Define.eResult.SUCCESS;
                            List<User_PartyDungeon_Clear> clearList = PvPManager.GetUserPartyDungeonClearList(ref tb, AID);
                            json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_PartyDungeonClear_List], mJsonSerializer.ToJsonString(clearList));
                        }
                        else if (requestOp.Equals("party_dungeon_sweep"))
                        {
                            int mapid = queryFetcher.QueryParam_FetchInt("mapid");
                            long CID = queryFetcher.QueryParam_FetchLong("cid");
                            int SweepCount = queryFetcher.QueryParam_FetchInt("sweep_count", 1);

                            Account userAccount = AccountManager.GetAccountData(ref tb, AID, ref retError);
                            Character charInfo = CharacterManager.GetCharacter(ref tb, AID, CID);
                            System_Party_Dungeon dungeonInfo = PvPManager.GetSystemPartyDungeonInfo(ref tb, mapid);

                            int getExp = 0;
                            int getGold = 0;

                            List<User_PartyDungeon_Clear> clearList = PvPManager.GetUserPartyDungeonClearList(ref tb, AID, true);
                            var checkClear = clearList.Find(map => map.map_index == mapid && map.clear > 0);

                            retError = checkClear != null ? Result_Define.eResult.SUCCESS : Result_Define.eResult.VIP_SWEEP_TYPE_INVALIDE;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<User_PvP_Play_Info> getPvPCountList = PvPManager.GetUser_PvPInfo_List(ref tb, AID, PvP_Define.ePvPType.MATCH_PARTY);
                                var pvpInfo = getPvPCountList.Find(item => item.map_index == mapid);

                                if (pvpInfo != null)
                                    retError = (pvpInfo.play_count + SweepCount > pvpInfo.max_play_count) ? Result_Define.eResult.PVP_PLAYCOUNT_OVER : Result_Define.eResult.SUCCESS;
                                else
                                    retError = Result_Define.eResult.PVP_PLAYER_NOT_IN_PLAY;
                            }

                            if (retError == Result_Define.eResult.SUCCESS && dungeonInfo.Condition_PlayCoin > 0)
                                retError = AccountManager.UseUserTicket(ref tb, AID, dungeonInfo.Condition_PlayCoin);

                            List<User_Inven> makeRealItem = new List<User_Inven>();
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<System_Drop_Group> getDropList = new List<System_Drop_Group>();
                                getExp = dungeonInfo.Base_Reward_EXP * SweepCount;

                                bool bUpdateTable = SystemData.GetServiceArea(ref tb) == DataManager_Define.eCountryCode.International;

                                for (int i = 0; i < SweepCount; i++)
                                {
                                    getGold += TheSoul.DataManager.Math.GetRandomInt(dungeonInfo.Base_Reward_GOLD_MIN1, dungeonInfo.Base_Reward_GOLD_MAX1);
                                    getDropList.AddRange(DropManager.GetDropResult(ref tb, AID, bUpdateTable ? dungeonInfo.Rand_DropBoxGroupId1 : dungeonInfo.Rand_DropBoxGroupId, (short)charInfo.Class));
                                }

                                foreach (System_Drop_Group setDrop in getDropList)
                                {
                                    List<User_Inven> getItem = new List<User_Inven>();
                                    retError = DropManager.MakeDropItem(ref tb, ref getItem, setDrop, AID, CID);

                                    if (retError != Result_Define.eResult.SUCCESS)
                                        break;

                                    makeRealItem.AddRange(getItem);
                                }
                            }

                            RetBeforeInfo retBefore = new RetBeforeInfo(charInfo.level, charInfo.exp, userAccount.Gold, userAccount.Cash + userAccount.EventCash,
                                                                        userAccount.Key, userAccount.KeyFillMaxEA, userAccount.Ticket, userAccount.TicketFillMaxEA, userAccount.ChallengeTicket);

                            // update character info exp, gold
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                if (getExp > 0)
                                {
                                    int checkContents = 0;
                                    float bonusRate = AccountManager.CheckExpRate(ref tb, out checkContents);
                                    if (bonusRate > 1.0f && TriggerManager.IsSetMask(checkContents, (int)SystemData_Define.eContentsType.MATCH_PARTY))
                                        getExp = (int)System.Math.Floor(getExp * bonusRate);
                                }
                                retError = CharacterManager.UpdateCharacterInfo(ref tb, CID, AID, getExp, getGold);
                            }

                            // update character info exp, gold
                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = PvPManager.AddUser_PvP_CountToDB(ref tb, AID, PvP_Define.ePvPType.MATCH_PARTY, mapid, SweepCount);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<TriggerProgressData> setDataList = new List<TriggerProgressData>();
                                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Clear_Party, mapid));
                                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Clear_Party_First, mapid));
                                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_Party, mapid));
                                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_Party_First, mapid));
                                retError = TriggerManager.ProgressTrigger(ref tb, AID, setDataList);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_act_type], 1);
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_game_player_action_log]);

                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.PvPOperationSID.MATCH_PARTY_RESULT);
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], (long)SnailLog_Define.PvPOperationSID.MATCH_PARTY_RESULT);
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_instance_log]);
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_scene_id], ((int)mapid + SnailLog_Define.Snail_s_id_Seperator_pve_party).ToString());
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_duration], 0);

                                    charInfo = CharacterManager.FlushCharacter(ref tb, AID, CID);
                                    retBefore.levelup = retBefore.beforelevel < charInfo.level ? 1 : 0;
                                    charInfo.exp = retBefore.beforelevel == charInfo.level && charInfo.level == Character_Define.Max_CharacterLevel ? getExp : charInfo.exp;
                                    Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userAccount, CharacterManager.GetCharacterCount_FromDB(ref tb, AID));
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.CharacterInfo], mJsonSerializer.ToJsonString(charInfo));
                                    json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.BeforeInfo], mJsonSerializer.ToJsonString(retBefore));
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeRealItem));
                                }
                            }
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