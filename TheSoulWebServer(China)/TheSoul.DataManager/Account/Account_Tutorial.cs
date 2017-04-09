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
    public partial class AccountManager
    {
        public static Result_Define.eResult UpdateTutorial(ref TxnBlock TB, long AID, long tutorialStep, string dbkey = Account_Define.AccountShardingDB)
        {
            if (AID < 1)
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            string setQuery = string.Format(@"UPDATE {0} SET Tutorial = {2} WHERE AID = {1} ", Account_Define.AccountDBTableName, AID, tutorialStep);
            return (TB.ExcuteSqlCommand(dbkey, setQuery)) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static List<System_Tutorial_Reward> GetSystem_Tutorial_RewardBox(ref TxnBlock TB, long RewardBoxID, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setKey = string.Format("{0}_{1}", Trigger_Define.Trigger_Prefix, Account_Define.System_Tutorial_Reward_TableName);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE RewardBoxID = {1}", Account_Define.System_Tutorial_Reward_TableName, RewardBoxID);

            return GenericFetch.FetchFromRedis_MultipleRow_Hash<System_Tutorial_Reward>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, RewardBoxID.ToString(), setQuery, dbkey, Flush);
        }

        public static Tutorial_Step Get_User_Tutorial(ref TxnBlock TB, long AID, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE AID = {1}", Account_Define.User_Tutorial_Step_TableName, AID);

            User_Tutorial getDBObj = GenericFetch.FetchFromDB<User_Tutorial>(ref TB, setQuery, dbkey);
            if (getDBObj == null)
                getDBObj = new User_Tutorial();

            Tutorial_Step retObj = new Tutorial_Step();
            if (!string.IsNullOrEmpty(getDBObj.TutorialStepJson))
                retObj = mJsonSerializer.JsonToObject<Tutorial_Step>(getDBObj.TutorialStepJson);

            return retObj;
        }

        public static Result_Define.eResult Set_User_Tutorial(ref TxnBlock TB, long AID, Tutorial_Step setStepInfo, bool Flush = false, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON AID = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                TutorialStepJson = '{2}'
                                                WHEN NOT MATCHED THEN
                                                   INSERT VALUES ({1}, '{2}');
                                    ", Account_Define.User_Tutorial_Step_TableName
                                     , AID
                                     , mJsonSerializer.ToJsonString(setStepInfo)
                                     );
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult End_User_Tutorial(ref TxnBlock TB, long AID, bool bEnd = true, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setQuery = string.Format(@" UPDATE {0} SET Tutorial = {2} WHERE AID = {1}", Account_Define.AccountDBTableName, AID, bEnd ? 1 : 0);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }
    }
}
