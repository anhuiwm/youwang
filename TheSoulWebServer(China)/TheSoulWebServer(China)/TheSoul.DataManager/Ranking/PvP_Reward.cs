using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;

namespace TheSoul.DataManager
{
    public partial class PvPManager
    {
        public static PvP_Define.ePvPRewardRepeatType GetPvP_RewardRepeatType(ref TxnBlock TB, PvP_Define.ePvPType pvpType, string getDate = "", string dbkey = PvP_Define.PvP_Info_DB)
        {
            DateTime setDate = DateTime.Now;

            if (!string.IsNullOrEmpty(getDate))
            {
                if (!DateTime.TryParse(getDate, out setDate))
                    setDate = DateTime.Now;
            }
                        
            SqlCommand commandPvP_RewardType = new SqlCommand();
            commandPvP_RewardType.CommandText = "System_PvP_Reward_Type";
            commandPvP_RewardType.Parameters.Add("@pvpType", SqlDbType.Int).Value = (int)pvpType;
            commandPvP_RewardType.Parameters.Add("@CURDATE", SqlDbType.NVarChar, 128).Value = setDate.ToShortDateString();
            var outputResult = new SqlParameter("@rewardType", SqlDbType.Int) { Direction = ParameterDirection.Output };
            commandPvP_RewardType.Parameters.Add(outputResult);
            int retValue = 0;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandPvP_RewardType))
                retValue = System.Convert.ToInt32(outputResult.Value);

            commandPvP_RewardType.Dispose();
            return (PvP_Define.ePvPRewardRepeatType)retValue;
        }

        public static string GetBattleReward_Mail_Body(PvP_Define.ePvPType pvpType, PvP_Define.ePvPRewardRepeatType rewardType, long setRank)
        {
            KeyValuePair<PvP_Define.ePvPType, PvP_Define.ePvPRewardRepeatType> setKey = new KeyValuePair<PvP_Define.ePvPType, PvP_Define.ePvPRewardRepeatType>(pvpType, rewardType);
            if (PvP_Define.PvPType_BattleRewardMailStringList.ContainsKey(setKey))
                return string.Format("{0}##{1}", PvP_Define.PvPType_BattleRewardMailStringList[setKey], setRank);
            else
                return string.Empty;
        }

        public static string GetBattleReward_Mail_Title(PvP_Define.ePvPType pvpType, PvP_Define.ePvPRewardRepeatType rewardType, long setRank)
        {
            return GetBattleReward_Mail_Body(pvpType, rewardType, setRank);
        }

        public static List<System_Battle_Reward> GetSystem_Battle_Reward_List(ref TxnBlock TB, PvP_Define.ePvPType pvpType, PvP_Define.ePvPRewardRepeatType rewardType, string dbkey = PvP_Define.PvP_Info_DB, bool Flush = true)
        {
            List<System_Battle_Reward> retList = new List<System_Battle_Reward>();
            if (PvP_Define.PvPType_BattleRewardStringList.ContainsKey(pvpType))
            {
                string setKey = string.Format("{0}_{1}", PvP_Define.PvP_Reward_Prefix, PvP_Define.System_Battle_Reward_TableName);
                string setMember = string.Format("{0}_{1}", pvpType, rewardType);
                string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE Battle_Type = '{1}' AND Day_Type = {2}", PvP_Define.System_Battle_Reward_TableName, PvP_Define.PvPType_BattleRewardStringList[pvpType], (int)rewardType);
                retList = GenericFetch.FetchFromRedis_MultipleRow_Hash<System_Battle_Reward>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setMember, setQuery, dbkey, Flush);
            }
            return retList;
        }

        public static List<ServerCreate_RankingReward> GetRankRewardList(ref TxnBlock TB, long AID, string dbkey = PvP_Define.ServerRankingReward_DB_Info)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE AID = {1} AND get_reward = 'N' AND reward_date > DATEADD(MINUTE, {2}, GETDATE()) ", PvP_Define.System_ServerCreate_RankingReward_TableName, AID, Mail_Define.Mail_Close_Min * -1);
            return GenericFetch.FetchFromDB_MultipleRow<ServerCreate_RankingReward>(ref TB, setQuery, dbkey);
        }

        public static Result_Define.eResult UpdateRankRewardInfo(ref TxnBlock TB, ServerCreate_RankingReward setRewardInfo, string dbkey = PvP_Define.ServerRankingReward_DB_Info)
        {
            string setQuery = string.Format(@"UPDATE {0} SET get_reward = 'Y' WHERE AID = {1} AND get_reward = '{2}' AND pvp_type = {3} AND reward_type = {4} AND reward_date = '{5}'",
                    PvP_Define.System_ServerCreate_RankingReward_TableName,
                    setRewardInfo.AID,
                    setRewardInfo.get_reward,
                    setRewardInfo.pvp_type,
                    setRewardInfo.reward_type,
                    setRewardInfo.reward_date.ToShortDateString()

                );

            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult SetPvP_RewardMail(ref TxnBlock TB, ref List<ServerCreate_RankingReward> setRewardList, string dbkey = PvP_Define.PvP_Info_DB)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            foreach (ServerCreate_RankingReward setReward in setRewardList)
            {
                PvP_Define.ePvPType pvpType = (PvP_Define.ePvPType)setReward.pvp_type;
                PvP_Define.ePvPRewardRepeatType rewardType = (PvP_Define.ePvPRewardRepeatType)setReward.reward_type;

                List<System_Battle_Reward> systemRewardInfo = PvPManager.GetSystem_Battle_Reward_List(ref TB, pvpType, rewardType);
                var findRewardInfo = systemRewardInfo.Find(setInfo => setInfo.MinValue <= setReward.rank && (setInfo.MaxValue >= setReward.rank || setInfo.MaxValue <= 0));

                if (findRewardInfo != null)
                {
                    List<Set_Mail_Item> setMailItem = new List<Set_Mail_Item>();
                    if (findRewardInfo.Reward1_Type > 0 && findRewardInfo.Reward1_Value > 0)
                    {
                        int setValue = findRewardInfo.Reward1_Value / setReward.divReward;
                        setMailItem.Add(new Set_Mail_Item(findRewardInfo.Reward1_Type, setValue < 1 ? 1 : setValue));
                    }
                    if (findRewardInfo.Reward2_Type > 0 && findRewardInfo.Reward2_Value > 0)
                    {
                        int setValue = findRewardInfo.Reward2_Value / setReward.divReward;
                        setMailItem.Add(new Set_Mail_Item(findRewardInfo.Reward2_Type, setValue < 1 ? 1 : setValue));
                    }

                    if (setMailItem.Count > 0)
                    {
                        TimeSpan TS = DateTime.Now - setReward.reward_date;
                        int CloseMin = (int)(Mail_Define.Mail_Close_Min - TS.TotalMinutes);
                        string setTitle = PvPManager.GetBattleReward_Mail_Title(pvpType, rewardType, setReward.rank);
                        string setText = PvPManager.GetBattleReward_Mail_Body(pvpType, rewardType, setReward.rank);                        
                        retError = MailManager.SendMail(ref TB, ref setMailItem, setReward.AID, 0, "", setTitle, setText, CloseMin);
                    }
                }

                if (retError == Result_Define.eResult.SUCCESS)
                    retError = UpdateRankRewardInfo(ref TB, setReward);

                if (retError != Result_Define.eResult.SUCCESS)
                    return retError;
            }
            return Result_Define.eResult.SUCCESS;
        }
    }
}
