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
using Microsoft.VisualBasic;
using ServiceStack.Text;

namespace TheSoul.DataManager
{
    public static partial class GuildManager
    {
        public enum eGuildReturnKeys
        {
            GildJoiner,
            DonationList
        }

        public static readonly Dictionary<eGuildReturnKeys, string> Guild_Ret_KeyList = new Dictionary<eGuildReturnKeys, string>()
        {
            { eGuildReturnKeys.GildJoiner,           "guildjoiner"          },
            { eGuildReturnKeys.DonationList,           "donationlist"          },
        };

        private static Result_Define.eResult UpdateGuildAttendUser(ref TxnBlock TB, long GuildID, string dbKey = GuildcommonDBName)
        {
            Result_Define.eResult retErr = Result_Define.eResult.DB_ERROR;

            string setQuery2 = string.Format(@"Select Sum(Case When isnull(DATEDIFF(d, TodayAttendDate, getdate()),0) > 0 
                                                            Or isnull(DATEDIFF(d, YesterdayAttendDate, getdate()),0) > 1 
                                                            Or isnull(DATEDIFF(d, JoinerDonationDate, getdate()),0) > 0 then 1 else 0 end) as count
                                                From {0} WITH(NOLOCK) Where GuildID = {1}", GuildJoinerDBTableName, GuildID);
            bool isUpdate = DataManager.GenericFetch.FetchFromDB<GuildCount>(ref TB, setQuery2, dbKey).count > 0 ? true : false;
            if (isUpdate)
            {
                List<GuildJoiner> joinerList = GetGuildJoinerInfoList(ref TB, GuildID);
                foreach (GuildJoiner joiner in joinerList)
                {
                    if (joiner.TodayAttendDate != null || joiner.JoinerDonationDate != null || joiner.YesterdayAttendDate != null)
                    {
                        string setQuery = string.Format(@"Update {0} Set TodayAttendDate = case when DATEDIFF(D,TodayAttendDate,GETDATE()) > 0 then null else TodayAttendDate end, 
                                                                            YesterdayAttendDate = Case When DATEDIFF(D, isnull(YesterdayAttendDate,getdate()), GETDATE()) <> 1 And DATEDIFF(D, isnull(TodayAttendDate,getdate()), GETDATE()) = 1  then TodayAttendDate 
                                                                                                    When DATEDIFF(D, isnull(YesterdayAttendDate,getdate()), GETDATE()) <> 1 then null else YesterdayAttendDate end,
                                                                            JoinerDonationDate = case when DATEDIFF(D,JoinerDonationDate,GETDATE()) > 0 then null else JoinerDonationDate end,
                                                                            TodayDonationExp = case when DATEDIFF(D,JoinerDonationDate,GETDATE()) > 0 then 0 else TodayDonationExp end
                                                            Where GuildID = {1} And JoinerAID = {2}", GuildJoinerDBTableName, GuildID, joiner.JoinerAID);
                        retErr = TB.ExcuteSqlCommand(dbKey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                    }
                }
                if (retErr == Result_Define.eResult.SUCCESS)
                {
                    string setQuery3 = string.Format(@"UPDATE A SET A.YesterdayAttendCheck = B.cntUser FROM {0} A Join 
                                                                (Select sum(Case When datediff(d,YesterdayAttendDate,GETDATE()) = 1 then 1 else 0 end) as cntUser, GuildID  From dbo.{1} WITH(NOLOCK) Where GuildID = {2} Group by GuildID) as B
                                                                ON A.GuildID = B.GuildID", GuildCreationDBTableName, GuildJoinerDBTableName, GuildID);
                    retErr = TB.ExcuteSqlCommand(dbKey, setQuery3) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                }

            }
            else
                retErr = Result_Define.eResult.SUCCESS;
            return retErr;
        }

        public static Result_Define.eResult GuildExpUp(ref TxnBlock TB, long AID, long AddExp, string dbKey = GuildcommonDBName)
        { //길드 경험치 올리기(기부에서만 사용)
            bool isLeveUp = false;
            GuildJoiner joiner = GetJoinerData(ref TB, AID);
            Guild_GuildCreation guildInfo = GetGuilData(ref TB, joiner.GuildID);
            long totalAddExp = 0;
            long GuildLVUpExp = 0;
            int maxLevel = GetSystemGuildList(ref TB).Max(item => item.Level);

            Result_Define.eResult retErr = Result_Define.eResult.DB_ERROR;
            CalcGuildBuffEndTime(ref TB, ref guildInfo, dbKey);
            if (guildInfo.GuildExpbuff == 0)
            {
                totalAddExp = AddExp;
            }
            else
            {
                totalAddExp = AddExp * 2;
            }

            if (guildInfo.GuildLevel >= maxLevel)
                retErr = Result_Define.eResult.SUCCESS;
            else
            {
                long realExp = totalAddExp + guildInfo.GuildExp;
                for (int i = 1; i <= (guildInfo.GuildLevel + 1); i++)
                {
                    GuildLVUpExp = GuildLVUpExp + GuildManager.GetSystemGuildData(ref TB, i).NeedExp;

                }

                if (guildInfo.GuildLevel != maxLevel && GuildLVUpExp != 0 && (realExp + guildInfo.GuildWithdrawExp) >= GuildLVUpExp)
                {
                    isLeveUp = true;
                }

                int upLevel = guildInfo.GuildLevel;

                if (isLeveUp)
                {
                    upLevel = guildInfo.GuildLevel + 1;
                    if (guildInfo.GuildLevel == maxLevel - 1)
                    {
                        string setQuery = string.Format("UPDATE {0} SET GuildLevel = {1}, GuildExp = {2} WHERE GuildID = {3}", GuildCreationDBTableName, upLevel, GuildLVUpExp, guildInfo.GuildID);
                        retErr = TB.ExcuteSqlCommand(dbKey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                    }
                    else
                    {
                        string setQuery = string.Format("UPDATE {0} SET GuildLevel = {1}, GuildExp = {2} WHERE GuildID = {3}", GuildCreationDBTableName, upLevel, realExp, guildInfo.GuildID);
                        retErr = TB.ExcuteSqlCommand(dbKey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                    }
                }
                else
                {

                    string setQuery = string.Format("UPDATE {0} SET GuildExp = {1} WHERE GuildID = {2}", GuildCreationDBTableName, realExp, guildInfo.GuildID);
                    retErr = TB.ExcuteSqlCommand(dbKey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                }

                if (retErr == Result_Define.eResult.SUCCESS)
                {
                    long JoinerExp = joiner.JoinerExp + totalAddExp;
                    long JoinerPoint = joiner.JoinerPoint + totalAddExp;
                    string setQuery = string.Format("UPDATE {0} SET JoinerExp = {1} WHERE GuildID = {2} AND JoinerAID = {3}", GuildJoinerDBTableName, JoinerExp, joiner.GuildID, AID);
                    retErr = TB.ExcuteSqlCommand(dbKey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                }
            }

            // check trigger guild level and exp
            if (retErr == Result_Define.eResult.SUCCESS)
            {
                List<TriggerProgressData> setDataList = new List<TriggerProgressData>();
                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Guild_Level));
                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Guild_User_EXP, 0, 0, totalAddExp));
                retErr = TriggerManager.ProgressTrigger(ref TB, AID, setDataList);
            }

            return retErr;
        }

        public static Result_Define.eResult CalcGuildBuffEndTime(ref TxnBlock TB, ref Guild_GuildCreation guildInfo, string dbkey = GuildcommonDBName, bool Flush = false)
        {
            guildInfo.GuildExpbuff = 0;
            guildInfo.GuildSkillBuff = 0;
            return Result_Define.eResult.SUCCESS;
        }

        public static Result_Define.eResult SetGuildAttend(ref TxnBlock TB, long GID, long AID, string dbKey = GuildcommonDBName)
        {
            string setQuery = string.Format("Update {0} SET TodayAttendDate = GETDATE() WHERE GuildID = {1} AND JoinerAID = {2}", GuildJoinerDBTableName, GID, AID);
            Result_Define.eResult retErr = TB.ExcuteSqlCommand(dbKey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.GUILD_NOEXIST_INFO;

            if (retErr == Result_Define.eResult.SUCCESS)
                retErr = TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.Guild_Attendance);

            return retErr;
        }

        public static Result_Define.eResult GuildNoticeModify(ref TxnBlock TB, long GuildID, string changedata, string dbKey = GuildcommonDBName, bool Flush = false)
        {//공지 수정
            Guild_GuildCreation guildInfo = GetGuilData(ref TB, GuildID);
            Result_Define.eResult retErr = Result_Define.eResult.GUILD_NOEXIST_INFO;
            if (guildInfo.GuildDeleteDate == null)
            {
                if (guildInfo.GuildNoticeModifyDate == null)
                {

                    string setQuery = string.Format("UPDATE {0} SET  GuildNotice = N'{1}', GuildNoticeModifyDate = getdate() WHERE GuildID = {2}", GuildCreationDBTableName, changedata.Length == 1 ? changedata + " " : changedata, GuildID); ;
                    retErr = TB.ExcuteSqlCommand(dbKey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.GUILD_NOEXIST_INFO;
                }
                else
                {
                    TimeSpan? ts = DateTime.Now - guildInfo.GuildNoticeModifyDate;
                    long time = (long)ts.Value.TotalSeconds;
                    if (time > 43200)
                    {
                        string setQuery = string.Format("UPDATE {0} SET  GuildNotice = N'{1}', GuildNoticeModifyDate = getdate() WHERE GuildID = {2}", GuildCreationDBTableName, changedata.Length == 1 ? changedata + " " : changedata, GuildID);
                        retErr = TB.ExcuteSqlCommand(dbKey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.GUILD_NOEXIST_INFO;
                    }
                    else
                        retErr = Result_Define.eResult.GUILD_CANT_CHANGE_HALFDAY;
                }
            }
            return retErr;
        }

        public static Result_Define.eResult GuildIntroduceModify(ref TxnBlock TB, long GuildID, string changedata, string dbKey = GuildcommonDBName, bool Flush = false)
        {//소개 수정
            Guild_GuildCreation guildInfo = GetGuilData(ref TB, GuildID);
            Result_Define.eResult retErr = Result_Define.eResult.GUILD_NOEXIST_INFO;
            if (guildInfo.GuildDeleteDate == null)
            {
                if (guildInfo.GuildIntroduceModifyDate == null)
                {
                    string setQuery = string.Format("UPDATE {0} SET  GuildIntroduce = N'{1}', GuildIntroduceModifyDate = getdate() WHERE GuildID = {2}", GuildCreationDBTableName, changedata.Length == 1 ? changedata + " " : changedata, GuildID);
                    retErr = TB.ExcuteSqlCommand(dbKey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.GUILD_NOEXIST_INFO;
                }
                else
                {
                    TimeSpan? ts = DateTime.Now - guildInfo.GuildIntroduceModifyDate;
                    long time = (long)ts.Value.TotalSeconds;
                    if (time > 43200)
                    {
                        string setQuery = string.Format("UPDATE {0} SET  GuildIntroduce = N'{1}', GuildIntroduceModifyDate = getdate() WHERE GuildID = {2}", GuildCreationDBTableName, changedata.Length == 1 ? changedata + " " : changedata, GuildID);
                        retErr = TB.ExcuteSqlCommand(dbKey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.GUILD_NOEXIST_INFO;
                    }
                    else
                        retErr = Result_Define.eResult.GUILD_CANT_CHANGE_HALFDAY;
                }
            }
            return retErr;
        }

        public static Result_Define.eResult GuildDissolution(ref TxnBlock TB, long GuildID, string dbKey = GuildcommonDBName)
        {
            Result_Define.eResult retErr = Result_Define.eResult.GUILD_NOEXIST_INFO;
            string setQuery = string.Format("UPDATE {0} SET  GuildDeleteDate = getdate() WHERE GuildID = {1}", GuildCreationDBTableName, GuildID);
            retErr = TB.ExcuteSqlCommand(dbKey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.GUILD_NOEXIST_INFO;
            if (retErr == Result_Define.eResult.SUCCESS)
            {
                string joinerQuery = string.Format("Select * From {0} WITH(NOLOCK, Index(IDX_GuildIDState))  WHERE GuildID = {1} AND (JoinerState ='S' OR JoinerState ='I')", GuildJoinerDBTableName, GuildID);
                List<GuildJoiner> joinerList = GenericFetch.FetchFromDB_MultipleRow<GuildJoiner>(ref TB, joinerQuery, dbKey);
                foreach (GuildJoiner joiner in joinerList)
                {
                    string setQuery2 = string.Format("UPDATE {0} SET JoinerState='D', TodayDonationExp = 0, JoinerDonationExp = 0, EntrustState=0, EntrustAskDate=null WHERE GuildID = {1} AND joinerAID = {2}", GuildJoinerDBTableName, GuildID, joiner.JoinerAID);
                    retErr = TB.ExcuteSqlCommand(dbKey, setQuery2) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.GUILD_NOEXIST_INFO;

                    if (retErr == Result_Define.eResult.SUCCESS)
                    {
                        retErr = PvPManager.DeleteGuild_PvP_Record(ref TB, joiner.GuildID, joiner.GuildID);
                    }

                    if (retErr != Result_Define.eResult.SUCCESS)
                        break;
                }
                if (retErr == Result_Define.eResult.SUCCESS)
                {

                    string setQuery3 = string.Format("SELECT * FROM  dbo.{0} WITH(NOLOCK) WHERE GUILDID = {1} AND JoinerState ='D'", GuildJoinerDBTableName, GuildID);
                    List<GuildJoiner> retLlist = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GuildJoiner>(ref TB, setQuery3, dbKey);
                    if (retLlist.Count > 0)
                    {
                        foreach (GuildJoiner joiner in retLlist)
                        {
                            retErr = AccountManager.UpdateGuildID(ref TB, joiner.JoinerAID, 0);
                            if(retErr == Result_Define.eResult.SUCCESS)
                                AccountManager.FlushAccountData(ref TB, joiner.JoinerAID, ref retErr);
                        }
                    }
                }
            }

            return retErr;
        }

        public static Result_Define.eResult GuildDonation(ref TxnBlock TB, ref int donationPoint, ref int plusExp, ref Account UserInfo, string changedata, string dbKey = GuildcommonDBName)
        {
            Result_Define.eResult retErr = Result_Define.eResult.GUILD_NOEXIST_INFO;
            plusExp = 0;
            donationPoint = 0;
            if (System.Convert.ToInt32(changedata) == 1)
            {
                plusExp = System.Convert.ToInt32(SystemData.GetConstValue(ref TB, "DEF_GUILD_DONATION_1_GET_EXP"));
            }
            else if (System.Convert.ToInt32(changedata) == 2)
            {
                plusExp = System.Convert.ToInt32(SystemData.GetConstValue(ref TB, "DEF_GUILD_DONATION_2_GET_EXP"));
            }
            else if (System.Convert.ToInt32(changedata) == 3)
            {
                plusExp = System.Convert.ToInt32(SystemData.GetConstValue(ref TB, "DEF_GUILD_DONATION_3_GET_EXP"));
            }
            retErr = GuildExpUp(ref TB, UserInfo.AID, plusExp);
            if (retErr == Result_Define.eResult.SUCCESS)
            {
                string setQuery = string.Format("UPDATE {0} Set JoinerDonationDate = getDate(), JoinerDonationExp = JoinerDonationExp + {1},TodayDonationExp = {1} WHERE GuildID = {2} AND JoinerAID = {3}", GuildJoinerDBTableName, plusExp, UserInfo.GuildID, UserInfo.AID);
                retErr = TB.ExcuteSqlCommand(dbKey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            }

            if (retErr == Result_Define.eResult.SUCCESS)
            {
                if (System.Convert.ToInt32(changedata) == 1)
                {
                    int reqGold = System.Convert.ToInt32(SystemData.GetConstValue(ref TB, Donation_Gold));
                    retErr = AccountManager.UseUserGold(ref TB, UserInfo.AID, reqGold);
                }
                else
                {
                    int reqCash = 0;
                    if (System.Convert.ToInt32(changedata) == 3)
                    {
                        reqCash = System.Convert.ToInt32(SystemData.GetConstValue(ref TB, Donation_Rubys));
                    }
                    else
                    {
                        reqCash = System.Convert.ToInt32(SystemData.GetConstValue(ref TB, Donation_Ruby));
                    }

                    retErr = AccountManager.UseUserCash(ref TB, UserInfo.AID, reqCash);
                }

                GetJoinerData(ref TB, UserInfo.AID, true);
            }

            if (retErr == Result_Define.eResult.SUCCESS)
            {
                int getPoint = GetContributionPointValue(ref TB, System.Convert.ToInt32(changedata));
                retErr = AddGuildContributionPoint(ref TB, UserInfo.AID, getPoint);
            }

            if (retErr == Result_Define.eResult.SUCCESS)
                retErr = TriggerManager.ProgressTrigger(ref TB, UserInfo.AID, Trigger_Define.eTriggerType.Guild_Donation);

            return retErr;
        }

        public static long GetGuildMasterAID(ref TxnBlock TB, long guildID, string dbKey = GuildcommonDBName)
        {
            long createAID = GetGuilData(ref TB, guildID).GuildCreateAID;
            return createAID;
        }

        public static Result_Define.eResult GuildStateChange(ref TxnBlock TB, ref JsonObject json, long GuildID, string dbKey = GuildcommonDBName)
        {
            Result_Define.eResult retErr = Result_Define.eResult.GUILD_NOEXIST_INFO;

            Guild_GuildCreation guildInfo = GetGuilData(ref TB, GuildID);
            if (guildInfo.StateChangeTime == null)
                guildInfo.StateChangeTime = DateTime.Today.AddDays(-1);
            TimeSpan? ts = DateTime.Now - guildInfo.StateChangeTime;
            long time = (long)ts.Value.TotalSeconds;
            if (time > 43200)
            {
                int MaxUserCount = GetSystemGuildData(ref TB, guildInfo.GuildLevel).Max_Member;
                int userCount = GetGuildJoinerCount(ref TB, GuildID, "S");
                int waitCount = GetGuildJoinerCount(ref TB, GuildID, "I");
                int AcceptPossibleCnt = 0;
                int CancelUserCnt = 0;
                if (guildInfo.GuildState == 1)
                {
                    string setQuery = string.Format("UPDATE {0} SET  GuildState  = 0 , StateChangeTime = getdate() WHERE GuildID = {1}", GuildCreationDBTableName, GuildID);
                    retErr = TB.ExcuteSqlCommand(dbKey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                }
                else
                {
                    string setQuery = string.Format("UPDATE {0} SET  GuildState  = 1 , StateChangeTime = getdate() WHERE GuildID = {1}", GuildCreationDBTableName, GuildID);
                    retErr = TB.ExcuteSqlCommand(dbKey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                }

                if (retErr == Result_Define.eResult.SUCCESS)
                {
                    Guild_GuildCreation new_guildInfo = GetGuilData(ref TB, GuildID, true);
                    if ((MaxUserCount - userCount) > 0)
                    {
                        AcceptPossibleCnt = MaxUserCount - userCount;
                    }
                    if (AcceptPossibleCnt >= waitCount)
                    {
                        AcceptPossibleCnt = waitCount;
                    }
                    CancelUserCnt = waitCount - AcceptPossibleCnt;

                    if (AcceptPossibleCnt > 0 || CancelUserCnt > 0 && guildInfo.GuildState == 0)
                    {
                        SqlDataReader getDr = null;
                        string setQuery2 = string.Format("SELECT JoinerAID FROM {0} WITH(NOLOCK) WHERE GuildID = {1} AND JoinerState = 'I' ORDER BY JoinerWaitDate ASC", GuildJoinerDBTableName, GuildID);
                        TB.ExcuteSqlCommand(dbKey, setQuery2, ref getDr);
                        if (getDr != null)
                        {
                            var r = SQLtoJson.Serialize(ref getDr);
                            json = mJsonSerializer.AddJson(json, "guildjoiner", mJsonSerializer.ToJsonString(r));
                            getDr.Dispose(); getDr.Close();
                        }

                        if (new_guildInfo.GuildState == 0)
                        {
                            // 계정 길드 정보 변경할 길드원
                            string setQuery5 = string.Format("SELECT Top {1} * FROM  dbo.{0} WITH(NOLOCK) WHERE GuildID = {2} AND JoinerState = 'I' ORDER BY JoinerWaitDate ASC", GuildJoinerDBTableName, AcceptPossibleCnt, GuildID);
                            List<GuildJoiner> joinerList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GuildJoiner>(ref TB, setQuery5, dbKey);

                            foreach (GuildJoiner joiner in joinerList)
                            {
                                string setQuery3 = string.Format(@"UPDATE {0} Set JoinerState = 'S', JoinerCreateDate = GETDATE(), JoinerWaitDate = NULL, EntrustState = 0, EntrustAskDate = null Where GuildID = {2} And JoinerAID = {1}", GuildJoinerDBTableName, joiner.JoinerAID, GuildID);
                                retErr = TB.ExcuteSqlCommand(dbKey, setQuery3) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                                if (retErr == Result_Define.eResult.SUCCESS)
                                {
                                    AccountManager.UpdateGuildID(ref TB, joiner.JoinerAID, joiner.GuildID);
                                    AccountManager.FlushAccountData(ref TB, joiner.JoinerAID, ref retErr);

                                }
                                else
                                    break;
                            }
                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                string setQuery6 = string.Format("SELECT * FROM  dbo.{0} WITH(NOLOCK) WHERE GuildID = {1} AND JoinerState = 'I'", GuildJoinerDBTableName, GuildID);
                                List<GuildJoiner> joinerRejectList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GuildJoiner>(ref TB, setQuery6, dbKey);
                                foreach (GuildJoiner joiner in joinerRejectList)
                                {
                                    string setQuery4 = string.Format("UPDATE {0} Set JoinerState = 'R', JoinerWaitDate = NULL Where GuildID = {1} AND JoinerAID = {2}", GuildJoinerDBTableName, GuildID, joiner.JoinerAID);
                                    if (retErr != Result_Define.eResult.SUCCESS)
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            else
                retErr = Result_Define.eResult.GUILD_CANT_CHANGE_HALFDAY;

            return retErr;
        }

        public static Result_Define.eResult CheckGuildDonation(ref TxnBlock TB, long AID, string changedata, string dbKey = GuildshardingDBName)
        {
            Result_Define.eResult retErr = Result_Define.eResult.SUCCESS;
            Account userInfo = AccountManager.GetAccountData(ref TB, AID, ref retErr);

            if (retErr != Result_Define.eResult.SUCCESS)
                return retErr;

            GuildJoiner joinerInfo = GetJoinerData(ref TB, AID);

            int myCash = 0, myGold = 0, reqCash = 0, reqGold = 0;
            
            if (CharacterManager.GetCharacter(ref TB, AID, userInfo.EquipCID).level < 10)
            {
                retErr = Result_Define.eResult.GUILD_DONATION_LEVELNOT_ENOUGH;
                return retErr;
            }

            if (!string.IsNullOrEmpty(userInfo.UserName))
            {
                myCash = (userInfo.EventCash + userInfo.Cash);
                myGold = userInfo.Gold;
                int useVal = System.Convert.ToInt32(changedata);

                if (useVal == 1)
                {
                    reqGold = System.Convert.ToInt32(SystemData.GetConstValue(ref TB, Donation_Gold));
                    if (reqGold > myGold)
                    {
                        retErr = Result_Define.eResult.NOT_ENOUGH_GOLD;
                    }
                    else
                    {
                        retErr = Result_Define.eResult.SUCCESS;
                    }
                }
                else
                {
                    if (useVal == 3)
                    {
                        reqCash = System.Convert.ToInt32(SystemData.GetConstValue(ref TB, Donation_Rubys));
                    }
                    else
                    {
                        reqCash = System.Convert.ToInt32(SystemData.GetConstValue(ref TB, Donation_Ruby));
                    }
                    if (reqCash > myCash)
                    {
                        retErr = Result_Define.eResult.NOT_ENOUGH_CASH;
                    }
                    else
                    {
                        retErr = Result_Define.eResult.SUCCESS;
                    }
                }

            }
            else
                retErr = Result_Define.eResult.ACCOUNT_ID_NOT_FOUND;

            if (retErr == Result_Define.eResult.SUCCESS)
            {
                if (string.IsNullOrEmpty(joinerInfo.JoinerDonationDate.ToString()))
                    retErr = Result_Define.eResult.SUCCESS;
                else
                {
                    retErr = Result_Define.eResult.ALREADY_GUILD_DONATION;
                }
            }

            return retErr;
        }

        private static Result_Define.eResult GuildJoinserCancel(ref TxnBlock TB, long AID, string dbkey = GuildcommonDBName)
        {//취소
            GuildJoiner joiner = GetJoinerData(ref TB, AID, true);
            Result_Define.eResult retErr = Result_Define.eResult.GUILD_NOEXIST_INFO;
            if (joiner.JoinerState == "I")
            {
                string setQuery = string.Format("Update {0} Set JoinerState = 'C' , JoinerWaitDate = NULL WHERE GuildID = {1} And JoinerAID = {2}", GuildJoinerDBTableName, joiner.GuildID, AID);
                retErr = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.GUILD_NOEXIST_INFO;
            }
            return retErr;
        }

        private static Result_Define.eResult GuildJoinserAccept(ref TxnBlock TB, long AID, string dbkey = GuildcommonDBName)
        {//수락 - account update
            GuildJoiner joiner = GetJoinerData(ref TB, AID, true);

            Result_Define.eResult retErr = Result_Define.eResult.GUILD_JOIN_CANCEL;
            if (joiner.JoinerState == "I")
            {
                string setQuery = string.Format("Update {0} Set JoinerState = 'S', JoinerCreateDate = getdate() , JoinerWaitDate = NULL, EntrustState = 0, EntrustAskDate = null WHERE GuildID = {1} And JoinerAID = {2}", GuildJoinerDBTableName, joiner.GuildID, AID);
                retErr = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.GUILD_NOEXIST_INFO;

                if (retErr == Result_Define.eResult.SUCCESS)
                {
                    retErr = AccountManager.UpdateGuildID(ref TB, AID, joiner.GuildID);
                    if(retErr == Result_Define.eResult.SUCCESS)
                        AccountManager.FlushAccountData(ref TB, AID, ref retErr);
                }
            }
            return retErr;
        }

        private static Result_Define.eResult GuildJoinserRejection(ref TxnBlock TB, long AID, string dbkey = GuildcommonDBName)
        {//거절
            GuildJoiner joiner = GetJoinerData(ref TB, AID, true);
            Result_Define.eResult retErr = Result_Define.eResult.GUILD_NOEXIST_INFO;
            if (joiner.JoinerState == "I")
            {
                string setQuery = string.Format("Update {0} Set JoinerState = 'R' , JoinerWaitDate = NULL WHERE GuildID = {1} And JoinerAID = {2}", GuildJoinerDBTableName, joiner.GuildID, AID);
                retErr = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.GUILD_NOEXIST_INFO;
            }
            return retErr;
        }

        private static Result_Define.eResult GuildJoinserWithdraw(ref TxnBlock TB, long AID, string dbkey = GuildcommonDBName)
        {//탈퇴
            Result_Define.eResult retErr = Result_Define.eResult.GUILD_NOEXIST_INFO;
            GuildJoiner joiner = GetJoinerData(ref TB, AID, true);
            if (joiner.JoinerState == "S")
            {
                retErr = GuildJoinserWithdrawPoint(ref TB, joiner, dbkey);
                if (retErr == Result_Define.eResult.SUCCESS)
                {
                    string setQuery = string.Format("UPDATE  {0} SET JoinerState = 'W' , JoinerDeleteDate = getdate() ,joiner3vs3point = 0 , JoinerTotal3vs3Point = 0, EntrustState = 0, EntrustAskDate = null WHERE GuildID = {1} And JoinerAID = {2}", GuildJoinerDBTableName, joiner.GuildID, AID);
                    retErr = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.GUILD_NOEXIST_INFO;

                    if (retErr == Result_Define.eResult.SUCCESS)
                    {
                        retErr = PvPManager.DeleteGuild_PvP_Record(ref TB, AID, joiner.GuildID);
                    }

                    if (retErr == Result_Define.eResult.SUCCESS)
                    {
                        retErr = AccountManager.UpdateGuildID(ref TB, AID, 0);
                        if(retErr == Result_Define.eResult.SUCCESS)
                            AccountManager.FlushAccountData(ref TB, AID, ref retErr);
                    }
                }

            }

            return retErr;
        }

        private static Result_Define.eResult GuildJoinserBanish(ref TxnBlock TB, long AID, string dbkey = GuildcommonDBName)
        {//추방
            Result_Define.eResult retErr = Result_Define.eResult.GUILD_NOEXIST_INFO;
            GuildJoiner joiner = GetJoinerData(ref TB, AID, true);
            if (joiner.JoinerState == "S")
            {
                retErr = GuildJoinserWithdrawPoint(ref TB, joiner, dbkey);
                if (retErr == Result_Define.eResult.SUCCESS)
                {
                    string setQuery = string.Format("UPDATE  {0} SET JoinerState = 'B' , JoinerbanishDate = getdate() ,joiner3vs3point = 0 , JoinerTotal3vs3Point = 0, EntrustState = 0, EntrustAskDate = null WHERE GuildID = {1} And JoinerAID = {2}", GuildJoinerDBTableName, joiner.GuildID, AID);
                    retErr = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.GUILD_NOEXIST_INFO;
                }

                if (retErr == Result_Define.eResult.SUCCESS)
                {
                    retErr = PvPManager.DeleteGuild_PvP_Record(ref TB, AID, joiner.GuildID);
                }

                if (retErr == Result_Define.eResult.SUCCESS)
                {
                    string setQuery = string.Format("Insert Into {0} Values ({1}, {2}, getdate())", GuildBanishLogDBTableName, joiner.GuildID, AID);
                    retErr = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;

                    if (retErr == Result_Define.eResult.SUCCESS)
                    {
                        retErr = AccountManager.UpdateGuildID(ref TB, AID, 0);
                        Account userInfo = AccountManager.FlushAccountData(ref TB, AID, ref retErr);
                    }
                }
            }

            return retErr;
        }

        private static Result_Define.eResult GuildJoinserWithdrawPoint(ref TxnBlock TB, GuildJoiner joiner, string dbkey = GuildcommonDBName)
        {//추방, 탈퇴 시 길드 경험치 및 포인트 처리
            Result_Define.eResult retErr = Result_Define.eResult.GUILD_NOEXIST_INFO;

            int guildLevel = GetGuildLV(ref TB, joiner.JoinerAID);
            if (guildLevel == 10)
            {
                string setQuery = string.Format("UPDATE {0} SET GuildRankingPoint = GuildRankingPoint - {1}, GuildWithdrawPoint = GuildWithdrawPoint + {1} WHERE GuildID = {2}", GuildCreationDBTableName, joiner.JoinerPoint, joiner.GuildID);
                retErr = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.GUILD_NOEXIST_INFO;
            }
            else
            {
                string setQuery = string.Format("UPDATE {0} SET  GuildExp = GuildExp - {1}, GuildWithdrawExp = GuildWithdrawExp + {1}, GuildRankingPoint = GuildRankingPoint - {2}, GuildWithdrawPoint = GuildWithdrawPoint + {2} WHERE GuildID = {3}", GuildCreationDBTableName, joiner.JoinerExp, joiner.JoinerPoint, joiner.GuildID);
                retErr = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.GUILD_NOEXIST_INFO;
            }

            if (retErr == Result_Define.eResult.SUCCESS)
            {
                string setQuery = string.Format("UPDATE {0} SET weekpoint = weekpoint - {1} WHERE guildid = {2}", PVP_GuildWarRecordDBTableName, joiner.Joiner3vs3Point, joiner.GuildID);
                retErr = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.GUILD_NOEXIST_INFO;
            }

            return retErr;
        }

        public static Result_Define.eResult GuildJoinserChange(ref TxnBlock TB, long AID, long joinerAID, int ChangeType, string dbkey = GuildcommonDBName)
        {
            Result_Define.eResult retErr = Result_Define.eResult.SUCCESS;
            if (ChangeType != 5 && ChangeType != 3)
            {
                long CreateAID = GetJoinerData(ref TB, AID, true).GuildCreateAID;
                if (AID != CreateAID)
                    retErr = Result_Define.eResult.ONLY_GUILD_MASTER;
            }
            if (retErr == Result_Define.eResult.SUCCESS)
            {
                if (ChangeType == 1)
                {
                    int joinerCount = GetGuildJoinerCount(ref TB, GetGuildInfo(ref TB, AID).guild_id);
                    int maxJoiner = GetSystemGuildData(ref TB,  GetGuildLV(ref TB, AID)).Max_Member;

                    if (joinerCount >= maxJoiner)
                        retErr = Result_Define.eResult.GUILD_CANT_JOIN_FULL_PUBLIC;
                    else
                        retErr = GuildJoinserAccept(ref TB, joinerAID, dbkey);
                }
                else if (ChangeType == 2)
                {
                    retErr = GuildJoinserRejection(ref TB, joinerAID, dbkey);
                }
                else if (ChangeType == 3)
                {
                    retErr = GuildJoinserWithdraw(ref TB, joinerAID, dbkey);
                }
                else if (ChangeType == 4)
                {
                    retErr = GuildJoinserBanish(ref TB, joinerAID, dbkey);
                }
                else if (ChangeType == 5)
                {
                    retErr = GuildJoinserCancel(ref TB, joinerAID, dbkey);
                }
            }

            return retErr;
        }

        private static int GetGuildMaxLevel_FromDB(ref TxnBlock TB, bool Flush = false, string dbkey = GuildcommonDBName)
        {
            string setKey = GuildSystemMaxLevel;
            string setQuery = string.Format("SELECT MAX([Level]) as count FROM {0} WITH(NOLOCK)", SystemGuildDBTableName);
            Rank_Count retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<Rank_Count>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush, GuildSystemExpireTime_Sec);
            return (int)(retObj == null ? GuildSystemMaxLevel_Default : retObj.count);
        }

        public static List<GuildRecommend> GetRecommandGuildList(ref TxnBlock TB, long AID, string dbkey = GuildcommonDBName, bool Flush = false)
        {
            GuildJoiner joiner = GuildManager.GetJoinerData(ref TB, AID, true);
            bool bCheckMyGuild = (joiner.GuildID > 0 && (joiner.JoinerState == "I" || joiner.JoinerState == "S"));
            int GetCount = bCheckMyGuild ? GuildRecommendCount - 1 : GuildRecommendCount;

            string setKey = string.Format("{0}_{1}", GuildRecommendPrefix, SystemGuildDBTableName);
            List<GuildRecommend> getRecommandGuildList = RedisConst.GetRedisInstance().GetRandomList<GuildRecommend>(DataManager_Define.RedisServerAlias_System, setKey, GetCount + 1);
            if (getRecommandGuildList == null)
                Flush = true;
            else if (getRecommandGuildList.Count < GetCount || Flush)
                Flush = true;

            if (Flush)
            {
                RedisConst.GetRedisInstance().RemoveList(DataManager_Define.RedisServerAlias_System, setKey);
                if (getRecommandGuildList == null)
                    getRecommandGuildList = new List<GuildRecommend>();
                else
                    getRecommandGuildList.Clear();

                string setQuery = string.Empty;
                int SystemMaxLevel = GuildManager.GetGuildMaxLevel_FromDB(ref TB);

                for (int setLevel = 1; setLevel <= SystemMaxLevel; setLevel++)
                {
                    // add normal guild state
                    setQuery = string.Format(@" SELECT * FROM (
                                                    SELECT Top {3} C.GuildID, C.GuildMark, C.GuildName, C.GuildCreateUserName as MasterName, C.GuildIntroduce, C.GuildLevel, C.GuildState 
                                                        FROM {0} C WITH(NOLOCK)
                                                            JOIN {1} J WITH(NOLOCK)
                                                                ON C.GuildID = J.GuildID AND C.GuildDeleteDate IS NULL AND C.GuildState = {4} AND C.GuildLevel = {2}  
                                                        GROUP BY  C.GuildID, C.GuildMark, C.GuildName, C.GuildCreateUserName, C.GuildIntroduce, C.GuildLevel, C.GuildState ,C.GuildCreateDate
                                                        HAVING COUNT(J.JoinerAID) < (select Max_Member FROM {6} WITH(NOLOCK) WHERE [LEVEL] = C.GuildLevel )
                                                        ORDER BY C.GuildCreateDate DESC
                                                UNION ALL
                                                    SELECT Top {3} C.GuildID, C.GuildMark, C.GuildName, C.GuildCreateUserName as MasterName, C.GuildIntroduce, C.GuildLevel, C.GuildState 
                                                        FROM {0} C WITH(NOLOCK)
                                                            JOIN {1} J WITH(NOLOCK)
                                                                ON C.GuildID = J.GuildID AND C.GuildDeleteDate IS NULL AND C.GuildState = {5} AND C.GuildLevel = {2} 
                                                                    AND  J.JoinerState ='S' AND C.GuildWaitingCount < 30
                                                        GROUP BY  C.GuildID, C.GuildMark, C.GuildName, C.GuildCreateUserName, C.GuildIntroduce, C.GuildLevel, C.GuildState ,C.GuildCreateDate
                                                        HAVING COUNT(J.JoinerAID) < (select Max_Member FROM {6} WITH(NOLOCK) WHERE [LEVEL] = C.GuildLevel )
                                                        ORDER BY C.GuildCreateDate DESC
                                                        ) as result
                                                    ", GuildCreationDBTableName
                                                     , GuildJoinerDBTableName
                                                     , setLevel
                                                     , MaxGuildRecommendPool
                                                     , (int)GuildStateChangeType.NONE
                                                     , (int)GuildStateChangeType.ISPUBLIC
                                                     , SystemGuildDBTableName
                                                     );
                    getRecommandGuildList.AddRange(TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GuildRecommend>(ref TB, setQuery, dbkey));

                    // add public guild state
//                    setQuery = string.Format(@"
//                                                    SELECT Top {4} C.GuildID, C.GuildMark, C.GuildName, C.GuildCreateUserName as MasterName, C.GuildIntroduce, C.GuildLevel, C.GuildState 
//                                                        FROM {0} C WITH(NOLOCK)
//                                                            JOIN {1} J 
//                                                                ON C.GuildID = J.GuildID AND C.GuildDeleteDate IS NULL AND C.GuildState = {2} AND C.GuildLevel = {3} 
//                                                                    AND  J.JoinerState ='S' AND C.GuildWaitingCount < 30
//                                                        GROUP BY  C.GuildID, C.GuildMark, C.GuildName, C.GuildCreateUserName, C.GuildIntroduce, C.GuildLevel, C.GuildState ,C.GuildCreateDate
//                                                        HAVING COUNT(J.JoinerAID) < (select Max_Member FROM Guild WITH(NOLOCK) WHERE [LEVEL] = C.GuildLevel )
//                                                        ORDER BY C.GuildCreateDate DESC;
//                                                ", GuildCreationDBTableName
//                                                     , GuildJoinerDBTableName
//                                                     , setLevel
//                                                     , MaxGuildRecommendPool
//                                                     );
//                    getRecommandGuildList.AddRange(TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GuildRecommend>(ref TB, setQuery, dbkey));
                }

                RedisConst.GetRedisInstance().ListAdds<GuildRecommend>(DataManager_Define.RedisServerAlias_System, setKey, getRecommandGuildList.ToArray());
                RedisConst.GetRedisInstance().ListExpireTimeSet(DataManager_Define.RedisServerAlias_System, setKey, new TimeSpan(0, GuildRecommendRefreshTime, 0));
            }

            var rnd = new Random();
            List<GuildRecommend> CheckList = getRecommandGuildList.OrderBy(x => rnd.Next()).ToList();
            List<GuildRecommend> retObj = new List<GuildRecommend>();

            if (bCheckMyGuild)
            {
                string setQuery = string.Format(@"
                                                    SELECT GuildID, GuildMark, GuildName, GuildCreateUserName as MasterName, GuildIntroduce, GuildLevel, GuildState 
                                                        FROM {0} WITH(NOLOCK) WHERE GuildID = {1}
                                                ", GuildCreationDBTableName
                                                , joiner.GuildID
                                                );
                GuildRecommend setObj = TheSoul.DataManager.GenericFetch.FetchFromDB<GuildRecommend>(ref TB, setQuery, dbkey);
                if(setObj != null)
                    retObj.Add(setObj);
            }

            foreach (GuildRecommend setInfo in CheckList)
            {
                if(setInfo.GuildID != joiner.GuildID && !retObj.Contains(setInfo))
                    retObj.Add(setInfo);

                if (retObj.Count >= GuildRecommendCount)
                    break;
            }

            return retObj;
        }

        public static Result_Define.eResult GetSearchGuildList(ref TxnBlock TB, ref List<GuildRecommend> list, long AID, string GuildName, string dbkey = GuildcommonDBName)
        {
            string setQuery = "";
            long GetGuildID = CheckWaitGuild(ref TB, AID, dbkey);

            if (GetGuildID == 0)
            {
                setQuery = string.Format("Select Top 1 1 as IDX, GuildID, GuildName, GuildCreateUserName as mastername , GuildMark, GuildLevel, GuildState, GuildIntroduce FROM dbo.{0} WITH(NOLOCK) WHERE GuildName = N'{1}' AND GuildDeleteDate IS NULL", GuildCreationDBTableName, GuildName);
            }
            else
            {
                setQuery = string.Format("Select ROW_NUMBER() over(order by (case when GuildID = {2} then 1 else 0 end) desc) as idx, GuildID, GuildName, GuildCreateUserName as mastername, GuildMark, GuildLevel, GuildState, GuildIntroduce FROM dbo.{0} WITH(NOLOCK) WHERE (GuildName = N'{1}'  AND GuildDeleteDate IS NULL) Or GuildID= {2}", GuildCreationDBTableName, GuildName, GetGuildID);
            }
            list = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GuildRecommend>(ref TB, setQuery, dbkey);

            if (list.Count == 0)
                return Result_Define.eResult.GUILD_NOEXIST_INFO;
            else
                return Result_Define.eResult.SUCCESS;
        }

        public static Guild_GuildCreation GetSearchGuildInfo_ByName(ref TxnBlock TB, string GuildName, string dbkey = GuildcommonDBName)
        {
            //string setQuery = string.Format("SELECT * FROM {0} WHERE GuildName = '{1}' AND GuildDeleteDate IS NULL", GuildCreationDBTableName, GuildName);
            string setQuery = string.Format("SELECT * FROM {0} WHERE GuildName = N'{1}'", GuildCreationDBTableName, GuildName);
            Guild_GuildCreation retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<Guild_GuildCreation>(ref TB, setQuery, dbkey);

            if (retObj == null)
                retObj = new Guild_GuildCreation();
            return retObj;
        }

        private static long CheckWaitGuild(ref TxnBlock TB, long AID, string dbkey = GuildcommonDBName)
        {
            string setQuery = string.Format("Select * From dbo.{0} WITH(NOLOCK) WHERE JOINERAID = {1} AND JoinerState ='I'", GuildJoinerDBTableName, AID);
            Guild_GuildCreation retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<Guild_GuildCreation>(ref TB, setQuery, dbkey);

            if (retObj == null)
                retObj = new Guild_GuildCreation();

            return retObj.GuildID;
        }

        public static System_Guild GetSystemGuildData(ref TxnBlock TB, int Level, string dbkey = GuildcommonDBName, bool Flush = false)
        {
            //시스템 길드 정보 - 길드 레벨이용 하여 체크
            string setKey = string.Format("{0}", SystemGuildPrefix);

            System_Guild retObj = GetSystemGuildList(ref TB, dbkey, true).Find(item => item.Level.Equals(Level));
            return (retObj != null) ? retObj : new System_Guild();
        }

        private static List<System_Guild> GetSystemGuildList(ref TxnBlock TB, string dbkey = GuildcommonDBName, bool Flush = false)
        {
            string setKey = string.Format("{0}", SystemGuildPrefix);
            
            List<System_Guild> retObj = new List<System_Guild>();
            if (!Flush)
            {
                retObj = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis_Hash_All<System_Guild>(DataManager_Define.RedisServerAlias_System, setKey);
                if (retObj.Count == 0)
                    Flush = true;
            }

            if (Flush)
            {
                RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey);
                string setQuery = string.Format("SELECT * FROM {0}  WITH(NOLOCK)", SystemGuildDBTableName);
                retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<System_Guild>(ref TB, setQuery, dbkey);

                retObj.ForEach(item =>
                {
                    RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, item.Level.ToString(), item);
                });
            }

            return retObj;
        }

        public static List<DonateInfo> GetDonateList(ref TxnBlock TB, long GuildID, bool Flush = false, string dbkey = GuildcommonDBName)
        {
            List<GuildJoiner> joinerList = GuildManager.GetGuildJoinerInfoList(ref TB, GuildID, Flush, dbkey).OrderByDescending(item => item.JoinerDonationDate).ToList();
            int donationCnt = 0;
            List<DonateInfo> DonateList = new List<DonateInfo>();
            foreach (GuildJoiner joiner in joinerList)
            {
                if (joiner.TodayDonationExp > 0)
                {
                    DonateInfo donateInfo = new DonateInfo();
                    donateInfo.joinerAID = joiner.JoinerAID;
                    donateInfo.TodayDonationExp = joiner.TodayDonationExp;
                    DonateList.Add(donateInfo);
                    donationCnt = donationCnt + 1;
                }
            }

            return DonateList;
        }

        public static Ret_MyGuild_Info GetMyGuildInfo(ref TxnBlock TB, ref Guild_GuildCreation guildInfo, long AID, bool Flush = false, string dbkey = GuildcommonDBName)
        {
            //길드 출석 초기화처리
            UpdateGuildAttendUser(ref TB, guildInfo.GuildID);

            guildInfo = GetGuilData(ref TB, guildInfo.GuildID, true);

            GuildJoiner joinerInfo = GetJoinerData(ref TB, AID, Flush);
            //내길드정보
            Ret_MyGuild_Info myGuildInfo = new Ret_MyGuild_Info();
            List<GuildJoiner> joinerInfoList = GuildManager.GetGuildJoinerInfoList(ref TB, guildInfo.GuildID, Flush);
            List<Sample_GuildJoiner> joinerList = new List<Sample_GuildJoiner>();
            List<DonateInfo> DonateList = new List<DonateInfo>();

            string isAttend = "N";
            int donationCnt = 0;
            bool myGuildCheck = false;
            long myDonationPoint = 0;
            foreach (GuildJoiner joiner in joinerInfoList)
            {
                Sample_GuildJoiner sample_Joiner = new Sample_GuildJoiner();
                sample_Joiner.joinerAID = joiner.JoinerAID;
                sample_Joiner.joinerPoint = joiner.JoinerPoint;
                sample_Joiner.JoinerDonationExp = joiner.JoinerDonationExp;
                sample_Joiner.TodayDonationExp = joiner.JoinerPoint;
                sample_Joiner.isJoinerAttend = string.IsNullOrEmpty(joiner.TodayAttendDate.ToString()) ? "N" : "Y";

                Account userinfo = AccountManager.GetAccountData(ref TB, joiner.JoinerAID);
                if (!string.IsNullOrEmpty(userinfo.UserName))
                {
                    sample_Joiner.JoinerName = userinfo.UserName;
                    sample_Joiner.Lastconntime = userinfo.LastConnTime;
                    Character equipCharacter = CharacterManager.GetCharacter(ref TB, userinfo.AID, userinfo.EquipCID);
                    sample_Joiner.JoinerLevel = equipCharacter.level;
                    sample_Joiner.ClassType = equipCharacter.Class;
                }
                joinerList.Add(sample_Joiner);
            }
            joinerInfoList.OrderByDescending(item => item.JoinerDonationDate).ToList().ForEach(joiner =>
            {
                if (joiner.TodayDonationExp > 0 && donationCnt <= 10)
                {
                    DonateInfo donateInfo = new DonateInfo();
                    donateInfo.joinerAID = joiner.JoinerAID;
                    donateInfo.TodayDonationExp = joiner.TodayDonationExp;
                    DonateList.Add(donateInfo);
                    donationCnt = donationCnt + 1;
                }
                if (joiner.JoinerAID == AID)
                {
                    isAttend = string.IsNullOrEmpty(joiner.TodayAttendDate.ToString()) ? "N" : "Y";
                    myGuildCheck = true;
                    myDonationPoint = joinerInfo.JoinerDonationPoint;
                }
            });
            long IntroduceUpdatetime = 0;
            long NoticeUpdatetime = 0;
            long ExpBuff = 0;
            string setQuery = string.Format("select case when isnull(GuildExpBuff,0) = 0 then 0 else isnull(GuildExpBuff,0)-DATEDIFF(SS,'1970-01-01',GETDATE()) end as expBuff, isnull(DATEDIFF(SS,'1970-01-01',GuildIntroduceModifyDate),0) AS GuildIntroduceModifyDate, isnull(DATEDIFF(SS,'1970-01-01',GuildNoticeModifyDate),0) AS GuildNoticeModifyDate From {0} WITH(NOLOCK) WHERE GuildID = {1}", GuildCreationDBTableName, guildInfo.GuildID);
            SqlDataReader getDr = null;
            TB.ExcuteSqlCommand(dbkey, setQuery, ref getDr);
            if (getDr != null)
            {
                while (getDr.Read())
                {
                    IntroduceUpdatetime = System.Convert.ToInt64(getDr["GuildIntroduceModifyDate"]);
                    NoticeUpdatetime = System.Convert.ToInt64(getDr["GuildNoticeModifyDate"]);
                    ExpBuff = System.Convert.ToInt64(getDr["expBuff"]);
                }
                getDr.Dispose(); getDr.Close();
            }

            if (ExpBuff < 0)
                ExpBuff = 0;
            if (joinerInfo.JoinerState == "S")
            {
                if (joinerInfo.JoinerDonationDate == null)
                    myGuildInfo.IsDonate = "N";
                else
                    myGuildInfo.IsDonate = "Y";
            }
            else
                myGuildInfo.IsDonate = "N";
            long rankingTotalCount = 0;
            myGuildInfo.GuildID = guildInfo.GuildID;
            myGuildInfo.GuildName = guildInfo.GuildName.ToString();
            myGuildInfo.MasterAid = guildInfo.GuildCreateAID;
            myGuildInfo.MasterName = guildInfo.GuildCreateUserName;
            myGuildInfo.GuildIntroduce = guildInfo.GuildIntroduce;
            myGuildInfo.GuildIntroduceupdatetime = IntroduceUpdatetime;
            myGuildInfo.GuildNotice = guildInfo.GuildNotice;
            myGuildInfo.GuildNoticeupdatetime = NoticeUpdatetime;
            myGuildInfo.GuildMark = guildInfo.GuildMark;
            myGuildInfo.GuildLevel = guildInfo.GuildLevel;
            myGuildInfo.GuildState = guildInfo.GuildState;
            myGuildInfo.GuildExp = guildInfo.GuildExp + guildInfo.GuildWithdrawExp;
            myGuildInfo.GuildRankingPoint = guildInfo.GuildRankingPoint;
            myGuildInfo.GuildUsercnt = joinerList.Count;
            myGuildInfo.GuildMaxUsercnt = GuildManager.GetSystemGuildData(ref TB, guildInfo.GuildLevel).Max_Member;
            myGuildInfo.YesterdayCheck = guildInfo.YesterdayAttendCheck;
            myGuildInfo.TodayCheck = GuildManager.GetToDayAttendCount(ref TB, guildInfo.GuildID);
            myGuildInfo.IsAttend = isAttend;
            myGuildInfo.GuildRank = PvPManager.GetUser_PvP_Guild_Rank_Info(ref TB, guildInfo.GuildID, ref rankingTotalCount).rank;
            myGuildInfo.GuildDonationpoint = System.Convert.ToInt32(myDonationPoint);
            myGuildInfo.MyGuildInfo = myGuildCheck;
            myGuildInfo.GuildExpbuff = ExpBuff;
            myGuildInfo.GuildSkillbuff = 0;
            myGuildInfo.GuildJoiner = joinerList;
            myGuildInfo.donationlist = DonateList;
            return myGuildInfo;
        }

        public static Ret_Guild_Info GetGuildInfo(ref TxnBlock TB, ref Guild_GuildCreation guildInfo, long AID, bool Flush = false, string dbkey = GuildcommonDBName)
        {
            //길드 출석 초기화처리
            UpdateGuildAttendUser(ref TB, guildInfo.GuildID);

            //길드 정보
            Ret_Guild_Info myGuildInfo = new Ret_Guild_Info();
            List<GuildJoiner> joinerInfoList = GuildManager.GetGuildJoinerInfoList(ref TB, guildInfo.GuildID, Flush);
            List<Sample_GuildJoiner> joinerList = new List<Sample_GuildJoiner>();
            List<DonateInfo> DonateList = new List<DonateInfo>();

            foreach (GuildJoiner joiner in joinerInfoList)
            {
                Sample_GuildJoiner sample_Joiner = new Sample_GuildJoiner();
                sample_Joiner.joinerAID = joiner.JoinerAID;
                sample_Joiner.joinerPoint = joiner.JoinerPoint;
                sample_Joiner.JoinerDonationExp = joiner.JoinerDonationExp;
                sample_Joiner.TodayDonationExp = joiner.JoinerPoint;
                sample_Joiner.isJoinerAttend = string.IsNullOrEmpty(joiner.TodayAttendDate.ToString()) ? "1970-01-01" : joiner.TodayAttendDate.ToString();

                Account userinfo = AccountManager.GetAccountData(ref TB, joiner.JoinerAID);
                if (!string.IsNullOrEmpty(userinfo.UserName))
                {
                    sample_Joiner.JoinerName = userinfo.UserName;
                    sample_Joiner.Lastconntime = userinfo.LastConnTime;
                    Character equipCharacter = CharacterManager.GetCharacter(ref TB, userinfo.AID, userinfo.EquipCID);
                    sample_Joiner.JoinerLevel = equipCharacter.level;
                    sample_Joiner.ClassType = equipCharacter.Class;
                }
                joinerList.Add(sample_Joiner);
            }
            joinerInfoList.OrderByDescending(item => item.JoinerDonationDate).ToList().ForEach(joiner =>
            {
                DonateInfo donateInfo = new DonateInfo();
                donateInfo.joinerAID = joiner.JoinerAID;
                donateInfo.TodayDonationExp = joiner.TodayDonationExp;
                DonateList.Add(donateInfo);

            });
            long rankingTotalCount = 0;
            myGuildInfo.GuildID = guildInfo.GuildID;
            myGuildInfo.GuildName = guildInfo.GuildName.ToString();
            myGuildInfo.MasterAid = guildInfo.GuildCreateAID;
            myGuildInfo.MasterName = guildInfo.GuildCreateUserName;
            myGuildInfo.GuildIntroduce = guildInfo.GuildIntroduce;
            myGuildInfo.GuildNotice = guildInfo.GuildNotice;
            myGuildInfo.GuildMark = guildInfo.GuildMark;
            myGuildInfo.GuildLevel = guildInfo.GuildLevel;
            myGuildInfo.GuildState = guildInfo.GuildState;
            myGuildInfo.GuildExp = guildInfo.GuildExp + guildInfo.GuildWithdrawExp;
            myGuildInfo.GuildRankingPoint = guildInfo.GuildRankingPoint;
            myGuildInfo.GuildUsercnt = joinerList.Count;
            myGuildInfo.GuildMaxUsercnt = GuildManager.GetSystemGuildData(ref TB, guildInfo.GuildLevel).Max_Member;
            myGuildInfo.GuildRank = PvPManager.GetUser_PvP_Guild_Rank_Info(ref TB, guildInfo.GuildID, ref rankingTotalCount).rank;
            myGuildInfo.MyGuildInfo = false;
            myGuildInfo.GuildJoiner = joinerList;
            myGuildInfo.donationlist = DonateList;
            return myGuildInfo;
        }

        public static List<GuildJoiner> GetGuildJoinerInfoList(ref TxnBlock TB, long GuildID, bool Flush = false, string dbkey = GuildcommonDBName)
        {
            //길드원 전체 정보 리스트
            string setQuery = string.Format("SELECT * FROM dbo.{0} WITH(NOLOCK) WHERE GUILDID = {1}  AND JoinerState = 'S' order by Case When JoinerAID = GuildCreateAID Then 0 Else 1 End ASC, JoinerCreateDate ASC", GuildJoinerDBTableName, GuildID);

            //string setKey = string.Format("{0}_{1}", GuildJoinerPrefix, GuildID);

            //List<GuildJoiner> getInfo = DataManager.GenericFetch.FetchFromRedis_MultipleRow<GuildJoiner>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
            List<GuildJoiner> getInfo = DataManager.GenericFetch.FetchFromDB_MultipleRow<GuildJoiner>(ref TB, setQuery, dbkey);
            return getInfo;
        }

        private static Dictionary<long, ManagedJoiner> ManagedJoinerFetchFromDB(ref TxnBlock TB, string setQuery, string dbkey = GuildcommonDBName)
        {
            SqlDataReader getDr = null;
            TB.ExcuteSqlCommand(dbkey, setQuery, ref getDr);
            Dictionary<long, ManagedJoiner> retSet = new Dictionary<long, ManagedJoiner>();

            if (getDr != null)
            {
                var r = SQLtoJson.Serialize(ref getDr);
                string json = mJsonSerializer.ToJsonString(r);
                getDr.Dispose(); getDr.Close();
                ManagedJoiner[] setData = mJsonSerializer.JsonToObject<ManagedJoiner[]>(json);

                foreach (ManagedJoiner joiner in setData)
                {
                    Account userinfo = AccountManager.GetAccountData(ref TB, joiner.JoinerAid);
                    if (!string.IsNullOrEmpty(userinfo.UserName))
                    {
                        joiner.JoinerName = userinfo.UserName;
                        joiner.JoinerLevel = CharacterManager.GetCharacter(ref TB, userinfo.AID, userinfo.EquipCID).level;
                        joiner.LastConntime = userinfo.LastConnTime;
                        joiner.ClassType = userinfo.equipclass;
                        retSet.Add(joiner.JoinerAid, joiner);
                    }
                }
            }

            return retSet;
        }

        public static Dictionary<long, ManagedJoiner> GetGuildManagedJoinerList(ref TxnBlock TB, long GuildID, long GuildCreateAID, int GuildState, int GuildType, bool Flush = false, string dbkey = GuildcommonDBName)
        {
            string setKey = string.Format("{0}_{1}_{2}_{3}", GuildJoinerPrefix, GuildID, GuildState, GuildType);
            string setQuery = "";
            if (GuildState == 1)
            {
                if (GuildType == 0)
                    setQuery = string.Format("SELECT * FROM dbo.{0} WITH(NOLOCK, INDEX(IDX_JoinerState)) WHERE GUILDID = {1}  AND JoinerState = 'I' AND JoinerAID <> {2} order by JoinerWaitDate ASC", GuildJoinerDBTableName, GuildID, GuildCreateAID);
                else
                    setQuery = string.Format("SELECT JoinerAID, JoinerPoint FROM  dbo.{0} WITH(NOLOCK, INDEX(IDX_JoinerState)) WHERE GUILDID = {1} AND JoinerState ='S' AND JoinerAID <> {2}", GuildJoinerDBTableName, GuildID, GuildCreateAID);
            }
            else
            {
                if (GuildType == 1)
                    setQuery = string.Format("SELECT JoinerAID,JoinerPoint FROM  dbo.{0} WITH(NOLOCK, INDEX(IDX_JoinerState)) WHERE GUILDID = {1} AND JoinerState ='S' AND JoinerAID <> {2}", GuildJoinerDBTableName, GuildID, GuildCreateAID);
            }
            Dictionary<long, ManagedJoiner> getInfo = RedisConst.GetRedisInstance().GetObj<Dictionary<long, ManagedJoiner>>(DataManager_Define.RedisServerAlias_User, setKey);

            if (getInfo == null || Flush)
                Flush = true;
            else if (getInfo.Count == 0)
                Flush = true;

            if (Flush)
            {
                getInfo = ManagedJoinerFetchFromDB(ref TB, setQuery, dbkey);
                RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_User, setKey, getInfo);
            }

            return getInfo;
        }


        public static long GetJoinerGuildID(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = GuildcommonDBName)
        {
            //길드원 정보
            //string setKey = string.Format("{0}_{1}", GuildJoinerPrefix, AID);
            string setQuery = string.Format(@"SELECT GuildID as count FROM dbo.{0} WITH(NOLOCK, INDEX(IDX_JoinerAID)) WHERE JoinerAID = {1}", GuildJoinerDBTableName, AID);
            Rank_Count retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<Rank_Count>(ref TB, setQuery, dbkey);

            return (retObj == null) ? 0 : retObj.count;
        }

        public static GuildJoiner GetJoinerData(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = GuildcommonDBName)
        {
            //길드원 정보
            string setKey = string.Format("{0}_{1}", GuildJoinerPrefix, AID);
            string setQuery = string.Format(@"SELECT GuildID, JoinerAID, GuildCreateAID, JoinerExp, JoinerPoint, JoinerState, JoinerCreateDate, JoinerWaitDate, JoinerDeleteDate, JoinerbanishDate
                                                , YesterdayAttendDate, TodayAttendDate, Joiner3vs3Point, JoinerTotal3vs3Point, EntrustState, EntrustAskDate, JoinerDonationExp, 0 as joinerDonationPoint
                                                , JoinerDonationDate, TodayDonationExp FROM dbo.{0} WITH(NOLOCK, INDEX(IDX_JoinerAID)) WHERE JoinerAID = {1}", GuildJoinerDBTableName, AID);
            GuildJoiner retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<GuildJoiner>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);

            if (retObj != null)
            {
                retObj.JoinerDonationPoint = AccountManager.GetAccountData(ref TB, AID, Flush).ContributionPoint;
            }
            return (retObj == null) ? new GuildJoiner() : retObj;
        }

        public static Result_Define.eResult CreateGuild(ref TxnBlock TB, ref long GuildID, long AID, string GuildName, int GuildMark, int GuildState, string dbkey = GuildcommonDBName, bool Flush = false)
        {
            //길드 생성
            if (string.IsNullOrEmpty(GuildName)) //길드명이 없을 경우
                return Result_Define.eResult.CANT_USE_NICKNAME;

            if (GuildMark < 1) // 길드마크 미 선택
                return Result_Define.eResult.NO_GUILD_MARK;

            if (GuildNameCheck(ref TB, GuildName))
                return Result_Define.eResult.ALREADY_EXIST_Guild;
            Result_Define.eResult err = Result_Define.eResult.SUCCESS;
            Account userInfo = AccountManager.GetAccountData(ref TB, AID, ref err);
            int createNeedGold = System.Convert.ToInt32(SystemData.GetConstValue(ref TB, "DEF_GUILD_CREAT_GOLD"));
            int createNeedCash = System.Convert.ToInt32(SystemData.GetConstValue(ref TB, "DEF_MAKEGUILD_RUBY"));
            if (userInfo.Gold < createNeedGold) // 길드 생성 금액 부족  
                return Result_Define.eResult.NOT_ENOUGH_GOLD;
            DataManager_Define.eCountryCode serviceArea = SystemData.GetServiceArea(ref TB);
            if (serviceArea == DataManager_Define.eCountryCode.Taiwan)
            {
                if ((userInfo.Cash + userInfo.EventCash) < createNeedCash)
                    return Result_Define.eResult.NOT_ENOUGH_CASH;

                if (err == Result_Define.eResult.SUCCESS)
                    err = AccountManager.UseUserCash(ref TB, AID, createNeedCash);
            }

            if(err == Result_Define.eResult.SUCCESS)
                err = AccountManager.UseUserGold(ref TB, AID, createNeedGold);

            if (err == Result_Define.eResult.SUCCESS)
            {
                //길드 생성
                SqlCommand CreateGuildCommand = new SqlCommand();
                CreateGuildCommand.CommandText = "CreateGuild2";
                CreateGuildCommand.CommandType = CommandType.StoredProcedure;
                var outputResultGuildID = new SqlParameter("@RESULTGUILDID", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                var outputResult = new SqlParameter("@RESULT", SqlDbType.Int) { Direction = ParameterDirection.Output };
                CreateGuildCommand.Parameters.Add("@GuildName", SqlDbType.NVarChar, 8).Value = GuildName;
                CreateGuildCommand.Parameters.Add("@GuildCreateAID", SqlDbType.BigInt).Value = AID;
                CreateGuildCommand.Parameters.Add("@GuildCreateUserName", SqlDbType.NVarChar, 32).Value = userInfo.UserName;
                CreateGuildCommand.Parameters.Add("@GuildMark", SqlDbType.Int).Value = GuildMark;
                CreateGuildCommand.Parameters.Add("@GuildState", SqlDbType.TinyInt).Value = GuildState;
                CreateGuildCommand.Parameters.Add(outputResultGuildID);
                CreateGuildCommand.Parameters.Add(outputResult);

                TB.ExcuteSqlStoredProcedure(dbkey, ref CreateGuildCommand);
                
                GuildID = System.Convert.ToInt64(outputResultGuildID.Value);
                int result = System.Convert.ToInt32(outputResult.Value);
                if (result > 0)
                {
                    CreateGuildCommand.Dispose();
                    if ((Result_Define.eResult)result == Result_Define.eResult.ALREADY_EXIST_Guild)
                        return Result_Define.eResult.ALREADY_EXIST_Guild;
                    else
                        return Result_Define.eResult.DB_ERROR;
                }
                else
                {
                    CreateGuildCommand.Dispose();
                    return Result_Define.eResult.SUCCESS;
                }
            }
            else
                return err;
        }


        public static Result_Define.eResult GuildJoin(ref TxnBlock TB, long AID, long GuildID, short GuildState, string dbkey = GuildcommonDBName)
        {
            //길드가입
            string joinState = (GuildState == 0) ? "S" : "I";
            int joinerCount = GetGuildJoinerCount(ref TB, GuildID, joinState, dbkey);

            if (GuildID <= 0)
                return Result_Define.eResult.GUILD_NOEXIST_INFO;

            int maxJoiner = GetSystemGuildList(ref TB).Max(item => item.Max_Member);
            if (GuildState != 0)
            {
                if (joinerCount >= maxJoiner)
                    return Result_Define.eResult.GUILD_CANT_JOIN_FULL_PRIVATE;
            }
            
            SqlCommand JoinCommand = new SqlCommand();
            JoinCommand.CommandText = "GuildJoin";
            JoinCommand.CommandType = CommandType.StoredProcedure;
            var outputResult = new SqlParameter("@RESULT", SqlDbType.Int) { Direction = ParameterDirection.Output };
            JoinCommand.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            JoinCommand.Parameters.Add("@GUILDID", SqlDbType.BigInt).Value = GuildID;
            JoinCommand.Parameters.Add("@GUILDSTATE", SqlDbType.TinyInt).Value = GuildState;
            JoinCommand.Parameters.Add(outputResult);

            TB.ExcuteSqlStoredProcedure(dbkey, ref JoinCommand);

            int result = System.Convert.ToInt32(outputResult.Value);
            JoinCommand.Dispose();
            if (result > 0)
            {
                if ((Result_Define.eResult)result == Result_Define.eResult.GUILD_CANT_CHANGE_HALFDAY)
                    return Result_Define.eResult.GUILD_CANT_CHANGE_HALFDAY;
                if ((Result_Define.eResult)result == Result_Define.eResult.GUILD_CANT_EXILE_REJOIN_HALFDAY)
                    return Result_Define.eResult.GUILD_CANT_EXILE_REJOIN_HALFDAY;
                if ((Result_Define.eResult)result == Result_Define.eResult.GUILD_OTHER_JOIN)
                    return Result_Define.eResult.GUILD_OTHER_JOIN;
                if ((Result_Define.eResult)result == Result_Define.eResult.GUILD_REQUESTJOIN_CHANGE_PUBLIC_TO_PRIVATE)
                    return Result_Define.eResult.GUILD_REQUESTJOIN_CHANGE_PUBLIC_TO_PRIVATE;
                if ((Result_Define.eResult)result == Result_Define.eResult.GUILD_JOINED_CHANGE_PRIVATE_TO_PUBLIC)
                    return Result_Define.eResult.GUILD_JOINED_CHANGE_PRIVATE_TO_PUBLIC;
                if ((Result_Define.eResult)result == Result_Define.eResult.GUILD_NOEXIST_INFO)
                    return Result_Define.eResult.GUILD_NOEXIST_INFO;
                if ((Result_Define.eResult)result == Result_Define.eResult.GUILD_CANT_JOIN_FULL_PUBLIC)
                    return Result_Define.eResult.GUILD_CANT_JOIN_FULL_PUBLIC;
                if ((Result_Define.eResult)result == Result_Define.eResult.GUILD_CANT_REJOIN_HALFDAY)
                    return Result_Define.eResult.GUILD_CANT_REJOIN_HALFDAY;

                return Result_Define.eResult.DB_ERROR;
            }
            else
                return Result_Define.eResult.SUCCESS;
        }

        //위임관련 일부 수정
        public static Result_Define.eResult GuildEntrustReply(ref TxnBlock TB, long AID, int changeType, string changedata, string dbkey = GuildcommonDBName, bool Flush = false)
        {
            // 길드 관련 추가 기능 SP -> Func 수정 작업
            long GuildID = 0, createAID = 0;
            int entrustState = 0;
            bool flag = false;
            DateTime entrustAskDate = new DateTime();
            DateTime todayDate = DateTime.Now;
            Result_Define.eResult retErr = Result_Define.eResult.DB_ERROR;
            retErr = GetGuildEntrustInfo(ref TB, AID, ref GuildID, ref createAID, ref entrustState, ref entrustAskDate, ref flag);

            if (entrustState == (int)GuildEntrustState.NONE)
            {
                retErr = Result_Define.eResult.GUILD_ENTRUST_CANCEL;
                return retErr;
            }

            if (changeType == 0)        // 위임 거절
            {
                UpdateGuildEntrustState(ref TB, GuildJoinerDBTableName, GuildID, createAID, (int)GuildEntrustState.REJECTED);
                UpdateGuildEntrustState(ref TB, GuildJoinerDBTableName, GuildID, AID, (int)GuildEntrustState.REJECTING);
                retErr = Result_Define.eResult.SUCCESS;
            }
            else if (changeType == 1)   // 위임 수락
            {
                UpdateGuildEntrustState(ref TB, GuildJoinerDBTableName, GuildID, createAID, (int)GuildEntrustState.ACCEPTED);
                UpdateGuildEntrustState(ref TB, GuildJoinerDBTableName, GuildID, AID, (int)GuildEntrustState.NONE);
                string setQuery3 = string.Format("UPDATE {0} SET GuildCreateAID = {1}, GuildCreateUserName = N'{2}' WHERE GuildID = {3}", GuildCreationDBTableName, AID, changedata, GuildID);
                TB.ExcuteSqlCommand(dbkey, setQuery3);
                //edit
                string setQuery4 = string.Format("UPDATE {0} SET GuildCreateAID = {1} WHERE GuildID = {2}", GuildJoinerDBTableName, AID, GuildID);
                TB.ExcuteSqlCommand(dbkey, setQuery4);
                retErr = Result_Define.eResult.SUCCESS;                                                                                                         
            }

            return retErr;
        }

        public static Result_Define.eResult GuildEntrustCheck(ref TxnBlock TB, ref int YNButton, ref long prevAID, ref string prevName, long AID, long entrustid, string dbkey = GuildcommonDBName, bool Flush = false)
        {
            // 길드 관련 추가 기능 SP -> Func 수정 작업 (test)
            // ynbutton : 팝업창 버튼 구분 - 0:에러코드 확인, 1: 위임 팝업창  2: 변경 팝업창
            // prevaid : 이미 위임 요청한 길드원의 계정 아이디

            long GuildID = 0, createAID = 0;
            int entrustState = 0;
            bool flag = false;
            DateTime entrustAskDate = new DateTime();
            DateTime todayDate = DateTime.Now;
            Result_Define.eResult retErr = Result_Define.eResult.DB_ERROR;
            GetGuildEntrustInfo(ref TB, AID, ref GuildID, ref createAID, ref entrustState, ref entrustAskDate, ref flag);

            if (flag == false)
            {
                YNButton = 0;
                prevAID = 0;
                prevName = "";
                return retErr;
            }

            if (createAID != AID)
            {
                retErr = Result_Define.eResult.GUILD_ALREADY_CHANGE_CREATEAID; // 312;
                YNButton = 0;
                prevAID = 0;
                prevName = "";
                return retErr;
            }

            
            if (entrustAskDate.AddHours(1) >= todayDate)
            {
                retErr = Result_Define.eResult.GUILD_ENTRUST_ONEHOUR; // 313;
                YNButton = 0;
                prevAID = 0;
                prevName = "";
                return retErr;
            }

            if (entrustState == (int)GuildEntrustState.ENTRUSTING)
            {
                string setQuery3 = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE GuildID = {1} AND EntrustState = 2", GuildJoinerDBTableName, GuildID);
                GuildJoiner retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<GuildJoiner>(ref TB, setQuery3, dbkey);
                if (retObj != null)
                {
                    if (entrustid == retObj.JoinerAID)
                    {
                        retErr = Result_Define.eResult.GUILD_ENTRUST_ALREADY; // 317;
                        YNButton = 0;
                    }
                    else
                    {
                        retErr = Result_Define.eResult.SUCCESS;
                        prevAID = retObj.JoinerAID;
                        prevName = AccountManager.GetAccountData(ref TB, retObj.JoinerAID, ref retErr).UserName;
                        YNButton = 2;
                    }
                }
            }
            else
            {
                retErr = Result_Define.eResult.SUCCESS;
                YNButton = 1;
                prevAID = 0;
            }

            return retErr;
        }

        public static Result_Define.eResult GuildEntrustAsk(ref TxnBlock TB, long AID, long EntrustAID, string dbkey = GuildcommonDBName, bool Flush = false)
        {
            // 길드 관련 추가 기능 SP -> Func 수정 작업
            long GuildID = 0, createAID = 0;
            int entrustState = 0;
            bool flag = false;
            DateTime entrustAskDate = new DateTime();
            DateTime todayDate = DateTime.Now;

            Result_Define.eResult retErr = Result_Define.eResult.DB_ERROR;
            retErr = GetGuildEntrustInfo(ref TB, AID, ref GuildID, ref createAID, ref entrustState, ref entrustAskDate, ref flag);
            if (AID != createAID)
            {
                retErr = Result_Define.eResult.GUILD_ALREADY_CHANGE_CREATEAID;   // 312;
                return retErr;
            }
            if (retErr == Result_Define.eResult.SUCCESS)
            {
                string setQuery3 = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE GuildID = {1} AND EntrustState = {2}", GuildJoinerDBTableName, GuildID, (int)GuildEntrustState.ENTRUSTED);
                GuildJoiner retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<GuildJoiner>(ref TB, setQuery3, dbkey);
                if (retObj != null)
                {
                    string setQuery4 = string.Format("UPDATE {0} SET EntrustState = 0, EntrustAskDate = NULL WHERE GuildID = {1} AND JoinerAID = {2}", GuildJoinerDBTableName, retObj.GuildID, retObj.JoinerAID);
                    retErr = TB.ExcuteSqlCommand(dbkey, setQuery4) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                }

                if (retErr == Result_Define.eResult.SUCCESS)
                {
                    retErr = UpdateGuildEntrustState(ref TB, GuildID, AID, (int)GuildEntrustState.ENTRUSTING);
                    retErr = UpdateGuildEntrustState(ref TB, GuildID, EntrustAID, (int)GuildEntrustState.ENTRUSTED);
                }
            }
            return retErr;

        }


        public static Result_Define.eResult GetGuildEntrustInfo(ref TxnBlock TB, long AID, ref long GuildID, ref long createAID, ref int entruststate, ref DateTime entrustaskdate, ref bool flag, string dbkey = GuildcommonDBName)
        {
            Result_Define.eResult retErr = Result_Define.eResult.NO_GUILD_USER;
            if (AID == 0)
                return retErr;

            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK, INDEX(IDX_JoinerAID)) WHERE JoinerAID = {1}", GuildJoinerDBTableName, AID);
            GuildJoiner retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<GuildJoiner>(ref TB, setQuery, dbkey);
            if (retObj != null)
            {
                GuildID = retObj.GuildID;
                entrustaskdate = retObj.EntrustAskDate == null ? System.Convert.ToDateTime("1970-01-01") : System.Convert.ToDateTime(retObj.EntrustAskDate);
                entruststate = retObj.EntrustState;
                createAID = retObj.GuildCreateAID;
                flag = true;
                retErr = Result_Define.eResult.SUCCESS;
            }
            return retErr;
        }

        // 길드 위임 상태 갱신
        public static Result_Define.eResult UpdateGuildEntrustState(ref TxnBlock TB, long ParamGID, long ParamAID, int entrustState, string dbkey = GuildcommonDBName)
        {
            string setStateQuery = "";

            // 위임 요청 시 날짜 갱신
            if (entrustState == (int)GuildEntrustState.ENTRUSTED || entrustState == (int)GuildEntrustState.ENTRUSTING)
            {
                setStateQuery = string.Format("UPDATE {0} SET EntrustState = {1}, EntrustAskDate = GETDATE() WHERE GuildID = {2} AND JoinerAID = {3}", GuildJoinerDBTableName, entrustState, ParamGID, ParamAID);
            }
            else if (entrustState == (int)GuildEntrustState.NONE)
            {
                setStateQuery = string.Format("UPDATE {0} SET EntrustState = {1}, EntrustAskDate = NULL WHERE GuildID = {2} AND JoinerAID = {3}", GuildJoinerDBTableName, entrustState, ParamGID, ParamAID);
            }
            else
            {
                setStateQuery = string.Format("UPDATE {0} SET EntrustState = {1} WHERE GuildID = {2} AND JoinerAID = {3}", GuildJoinerDBTableName, entrustState, ParamGID, ParamAID);
            }
            Result_Define.eResult retErr = TB.ExcuteSqlCommand(dbkey, setStateQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            return retErr;
        }

        public static int GetContributionPointValue(ref TxnBlock TB, int useVal)
        {
            int getPoint = 0;
            string strConstDefine = "";
            if (useVal == 1)
            {
                strConstDefine = "DEF_GUILD_DONATION_1_GET_CONTRIBUTION";
            }
            else if (useVal == 2)
            {
                strConstDefine = "DEF_GUILD_DONATION_2_GET_CONTRIBUTION";
            }
            else if (useVal == 3)
            {
                strConstDefine = "DEF_GUILD_DONATION_3_GET_CONTRIBUTION";
            }
            getPoint = System.Convert.ToInt32(SystemData.GetConstValue(ref TB, strConstDefine));

            return getPoint;
        }

        public static Result_Define.eResult UseGuildDonation(ref TxnBlock TB, ref int resultGold, ref int resultCash, ref int resultPoint, long AID, int changedata)
        {
            // 길드 관련 추가 기능 SP -> Func 수정 작업 (coding)
            int myGold = 0, reqGold = 0, myCash = 0, reqCash = 0, myPoint = 0, getPoint = 0;
            Result_Define.eResult retErr = Result_Define.eResult.DB_ERROR;

            Account userInfo = AccountManager.GetAccountData(ref TB, AID);


            if (!string.IsNullOrEmpty(userInfo.UserName))
            {
                myCash = (userInfo.EventCash + userInfo.Cash);
                myGold = userInfo.Gold;
                myPoint = userInfo.ContributionPoint;
            }
            else
            {
                retErr = Result_Define.eResult.ACCOUNT_ID_NOT_FOUND;
                resultGold = myGold;
                resultCash = myCash;
                resultPoint = myPoint;
            }
            if (retErr != Result_Define.eResult.ACCOUNT_ID_NOT_FOUND)
            {
                int useVal = System.Convert.ToInt32(changedata);
                getPoint = GetContributionPointValue(ref TB, useVal);
                resultPoint = myPoint + getPoint;

                if (useVal == 1)
                {
                    reqGold = System.Convert.ToInt32(SystemData.GetConstValue(ref TB, Donation_Gold));

                    retErr = AccountManager.UseUserGold(ref TB, AID, reqGold);
                    if (retErr == Result_Define.eResult.SUCCESS)
                    {
                        Account userinfo = AccountManager.FlushAccountData(ref TB, AID, ref retErr);
                        resultGold = userinfo.Gold;
                        resultCash = myCash;
                    }
                }
                else
                {
                    //Result_Define.eSubRoute donateType = Result_Define.eSubRoute.GUILD_DONATION_RUBY;

                    if (useVal == 3)
                    {
                        reqCash = System.Convert.ToInt32(SystemData.GetConstValue(ref TB, Donation_Rubys));
                        //donateType = Result_Define.eSubRoute.GUILD_DONATION_RUBYS;
                    }
                    else
                    {
                        reqCash = System.Convert.ToInt32(SystemData.GetConstValue(ref TB, Donation_Ruby));
                        //donateType = Result_Define.eSubRoute.GUILD_DONATION_RUBY;
                    }
                    retErr = AccountManager.UseUserCash(ref TB, AID, reqCash);
                    if (retErr == Result_Define.eResult.SUCCESS)
                    {
                        Account userinfo = AccountManager.FlushAccountData(ref TB, AID, ref retErr);
                        resultGold = myGold;
                        resultCash = (userinfo.EventCash + userinfo.Cash);
                    }
                }
                if (retErr == Result_Define.eResult.SUCCESS)
                {
                    retErr = AddGuildContributionPoint(ref TB, AID, resultPoint);
                }
            }
            return retErr;

        }

        public static Result_Define.eResult AddGuildPoint(ref TxnBlock TB, long GuildID, long AID, int Point, string dbkey = GuildcommonDBName)
        {
            //난전
            long GID = GuildManager.GetJoinerGuildID(ref TB, AID);
            Guild_GuildCreation guildInfo = GetGuilData(ref TB, GID, true);
            Result_Define.eResult retErr = Result_Define.eResult.DB_ERROR;

            if (guildInfo.GuildID > 0)
            {
                long totalAddExp = 0;
                CalcGuildBuffEndTime(ref TB, ref guildInfo);
                if (guildInfo.GuildExpbuff == 0)
                {
                    totalAddExp = Point;
                }
                else
                {
                    totalAddExp = Point * 2;
                }


                string setQuery = string.Format("UPDATE {0} SET GuildRankingPoint = GuildRankingPoint + ({1}) WHERE GuildID = {2}", GuildCreationDBTableName, totalAddExp, guildInfo.GuildID);
                retErr = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;

                if (retErr == Result_Define.eResult.SUCCESS)
                {
                    string setQuery2 = string.Format("UPDATE {0} SET JoinerPoint = JoinerPoint + ({1}) WHERE GuildID = {2} AND JoinerAID = {3}", GuildJoinerDBTableName, totalAddExp, GuildID, AID);
                    retErr = TB.ExcuteSqlCommand(dbkey, setQuery2) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                }

                SetGuildRankingPoint(ref TB, guildInfo.GuildID, totalAddExp);
            }
            else
                retErr = Result_Define.eResult.GUILD_NOEXIST_INFO;

            return retErr;
        }

        public static Result_Define.eResult UseGuildContributionPoint(ref TxnBlock TB, long AID, long UseValue)
        {
            Account userInfo = AccountManager.GetAccountData(ref TB, AID);

            if (userInfo.ContributionPoint < UseValue)
                return Result_Define.eResult.NOT_ENOUGH_CONTIRIBUTION_POINT;

            string setQuery = string.Format(@"UPDATE {0} SET ContributionPoint = ContributionPoint - {1} WHERE AID={2}", Account_Define.AccountDBTableName, UseValue, AID);
            bool bShardingQuery = TB.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery);
            
            Result_Define.eResult retErr = (bShardingQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            if (retErr == Result_Define.eResult.SUCCESS)
            {
                AccountManager.FlushAccountData(ref TB, AID, ref retErr);
                GuildManager.GetJoinerData(ref TB, AID, true);
            }

            return retErr;
        }

        public static Result_Define.eResult AddGuildContributionPoint(ref TxnBlock TB, long AID, long addValue, string dbkey = Account_Define.AccountShardingDB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET ContributionPoint = ContributionPoint + {1} WHERE AID={2}", Account_Define.AccountDBTableName, addValue, AID);
            bool bShardingQuery = TB.ExcuteSqlCommand(dbkey, setQuery);

            Result_Define.eResult retErr = (bShardingQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            if (retErr == Result_Define.eResult.SUCCESS)
            {
                AccountManager.FlushAccountData(ref TB, AID, ref retErr);
                GuildManager.GetJoinerData(ref TB, AID, true);
            }

            return retErr;
        }

        public static Result_Define.eResult SetGuildRankingPoint(ref TxnBlock TB, long GID, long point, string dbkey = GuildcommonDBName)
        {
            long week = System.Convert.ToInt32(PvPManager.GetSeperater_Week(ref TB));

            SqlCommand CmdInsertRanking = new SqlCommand();
            CmdInsertRanking.CommandText = "InsertGuildRankPointWeekly";
            CmdInsertRanking.Parameters.Add("@gid", SqlDbType.BigInt).Value = GID;
            CmdInsertRanking.Parameters.Add("@currSeperaterWeekNum", SqlDbType.Int).Value = week;
            CmdInsertRanking.Parameters.Add("@rankpoint", SqlDbType.NVarChar).Value = point;
            var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            CmdInsertRanking.Parameters.Add(result);

            if (TB.ExcuteSqlStoredProcedure(dbkey, ref CmdInsertRanking))
            {
                if (System.Convert.ToInt32(result.Value) < 0)
                {
                    CmdInsertRanking.Dispose();
                    return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                }
            }
            else
            {
                CmdInsertRanking.Dispose();
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }

            System_GuildRanking_Data setObj = GetGuildRankPoint(ref TB, GID, true);
            PvPManager.SetUser_GuildRanking_Point(ref TB, ref setObj);
            return Result_Define.eResult.SUCCESS;
        }
    }
}
