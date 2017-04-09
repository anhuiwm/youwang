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

namespace TheSoul.DataManager
{
    public static partial class GoldExpedition_Manager
    {
        public static Result_Define.eResult SetUser_GE_Stage_InfoToDB(ref TxnBlock TB, long AID, User_GE_Stage_Info setInfo, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON AID = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                Clear_Stage = '{2}',
	                                                MyCharacter_Info_Json = N'{3}',
	                                                MyCharacter_Detail_Json = N'{4}',
	                                                AllyCharacter_Info_Json = N'{5}',
	                                                AllyCharacter_Detail_Json = N'{6}',
	                                                AllyUserName = N'{7}',
	                                                HireCount = '{8}',
	                                                ResetCount = '{9}',
	                                                RegDate = '{10}'
                                                WHEN NOT MATCHED THEN
                                                   INSERT VALUES ('{1}', '{2}', N'{3}', N'{4}', N'{5}', N'{6}', N'{7}', '{8}', '{9}', '{10}');
                                            ",
                                            GoldExpedition_Define.User_GE_Stage_Info_TableName,
                                            AID,
                                            setInfo.Clear_Stage,
                                            setInfo.MyCharacter_Info_Json,
                                            setInfo.MyCharacter_Detail_Json,
                                            setInfo.AllyCharacter_Info_Json,
                                            setInfo.AllyCharacter_Detail_Json,
                                            setInfo.AllyUserName,
                                            setInfo.HireCount,
                                            setInfo.ResetCount,
                                            setInfo.RegDate.ToString("yyyy-MM-dd HH:mm:ss")
                                 );
            RemoveCacheUser_GE_Stage_Info(AID);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        private static User_GE_Stage_Enemy makeEnemyInfo(ref TxnBlock TB, long AID, User_WarPoint enemyInfo, long TargetWarPoint, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            User_GE_Stage_Enemy setEnemyInfo = new User_GE_Stage_Enemy();
            Account_Simple enemyUserInfo = AccountManager.GetSimpleAccountInfo(ref TB, enemyInfo.AID);
            setEnemyInfo.AID = AID;
            setEnemyInfo.EnemyAID = enemyUserInfo.aid;
            setEnemyInfo.UserName = enemyUserInfo.username;
            setEnemyInfo.CurrentWarPoint = enemyInfo.WAR_POINT;
            setEnemyInfo.AdjustWarPoint = enemyInfo.WAR_POINT;
            List<Character_Detail_With_HP> enemyCharList = CharacterManager.GetCharacterListWithEquip_HP(ref TB, setEnemyInfo.EnemyAID).OrderBy(charInfo => charInfo.warpoint).ToList();

            if (enemyCharList.Find(setCharInfo => setCharInfo.equiplist.Count == 0) != null || enemyCharList.Count == 0)
            {
                setEnemyInfo.EnemyAID = 0;
                return setEnemyInfo;
            }

            List<User_Character_HP_Info> EnemeyHPInfo = new List<User_Character_HP_Info>();
            enemyCharList.ForEach(charinfo =>
            {
                //double hpRate = setEnemyInfo.AdjustWarPoint != setEnemyInfo.CurrentWarPoint ? ((double)setEnemyInfo.AdjustWarPoint / (double)setEnemyInfo.CurrentWarPoint) : 1.0f;
                EnemeyHPInfo.Add(new User_Character_HP_Info(charinfo, false)); ;
            });

            setEnemyInfo.EnemyCharacter_Main_Info_Json = mJsonSerializer.ToJsonString(EnemeyHPInfo.OrderByDescending(item => item.warpoint).FirstOrDefault());
            setEnemyInfo.EnemyCharacter_Info_Json = mJsonSerializer.ToJsonString(EnemeyHPInfo);
            setEnemyInfo.EnemyCharacter_Detail_Json = Character_Detail_With_HP.makeCharacter_DetailListJson(ref enemyCharList);

            return setEnemyInfo;
        }

        private static User_GE_Stage_Enemy makeEnemyInfoByCharacterWarPoint(ref TxnBlock TB, long AID, User_WarPoint enemyInfo, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            User_GE_Stage_Enemy setEnemyInfo = new User_GE_Stage_Enemy();
            Account_Simple enemyUserInfo = AccountManager.GetSimpleAccountInfo(ref TB, enemyInfo.AID);
            setEnemyInfo.AID = AID;
            setEnemyInfo.EnemyAID = enemyUserInfo.aid;
            setEnemyInfo.UserName = enemyUserInfo.username;
            setEnemyInfo.CurrentWarPoint = enemyInfo.MAX_WAR_POINT;
            setEnemyInfo.AdjustWarPoint = enemyInfo.MAX_WAR_POINT;
            List<Character_Detail_With_HP> enemyCharList = CharacterManager.GetCharacterListWithEquip_HP(ref TB, setEnemyInfo.EnemyAID).OrderBy(charInfo => charInfo.warpoint).ToList();

            if (enemyCharList.Find(setCharInfo => setCharInfo.equiplist.Count == 0) != null || enemyCharList.Count == 0)
            {
                setEnemyInfo.EnemyAID = 0;
                return setEnemyInfo;
            }

            List<User_Character_HP_Info> EnemeyHPInfo = new List<User_Character_HP_Info>();
            enemyCharList.ForEach(charinfo =>
            {
                //double hpRate = setEnemyInfo.AdjustWarPoint != setEnemyInfo.CurrentWarPoint ? ((double)setEnemyInfo.AdjustWarPoint / (double)setEnemyInfo.CurrentWarPoint) : 1.0f;
                EnemeyHPInfo.Add(new User_Character_HP_Info(charinfo, false)); ;
            });

            setEnemyInfo.EnemyCharacter_Main_Info_Json = mJsonSerializer.ToJsonString(EnemeyHPInfo.OrderByDescending(item => item.warpoint).FirstOrDefault());
            setEnemyInfo.EnemyCharacter_Info_Json = mJsonSerializer.ToJsonString(EnemeyHPInfo);
            setEnemyInfo.EnemyCharacter_Detail_Json = Character_Detail_With_HP.makeCharacter_DetailListJson(ref enemyCharList);

            return setEnemyInfo;
        }

        private static User_GE_Stage_Enemy makeDummyInfo(ref TxnBlock TB, long AID, User_WarPoint enemyInfo, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            User_GE_Stage_Enemy setEnemyInfo = new User_GE_Stage_Enemy(true);
            Account_Simple enemyUserInfo = DummyManager.GetSimpleAccountInfo(ref TB, enemyInfo.AID);
            setEnemyInfo.AID = AID;
            setEnemyInfo.EnemyAID = enemyUserInfo.aid;
            setEnemyInfo.UserName = enemyUserInfo.username;
            setEnemyInfo.CurrentWarPoint = enemyInfo.WAR_POINT;
            setEnemyInfo.AdjustWarPoint = enemyInfo.WAR_POINT;
            List<Character_Detail_With_HP> enemyCharList = DummyManager.GetCharacterListWithEquip_HP(ref TB, setEnemyInfo.EnemyAID).OrderBy(charInfo => charInfo.warpoint).ToList();

            List<User_Character_HP_Info> EnemeyHPInfo = new List<User_Character_HP_Info>();
            enemyCharList.ForEach(charinfo =>
            {
                //double hpRate = setEnemyInfo.AdjustWarPoint != setEnemyInfo.CurrentWarPoint ? ((double)setEnemyInfo.AdjustWarPoint / (double)setEnemyInfo.CurrentWarPoint) : 1.0f;
                EnemeyHPInfo.Add(new User_Character_HP_Info(charinfo, false)); ;
            });

            setEnemyInfo.EnemyCharacter_Main_Info_Json = mJsonSerializer.ToJsonString(EnemeyHPInfo.OrderByDescending(item => item.warpoint).FirstOrDefault());
            setEnemyInfo.EnemyCharacter_Info_Json = mJsonSerializer.ToJsonString(EnemeyHPInfo);
            setEnemyInfo.EnemyCharacter_Detail_Json = Character_Detail_With_HP.makeCharacter_DetailListJson(ref enemyCharList);

            return setEnemyInfo;
        }

        private static User_GE_Stage_Enemy makeDummyInfoByCharacterWarPoint(ref TxnBlock TB, long AID, User_WarPoint enemyInfo, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            User_GE_Stage_Enemy setEnemyInfo = new User_GE_Stage_Enemy(true);
            Account_Simple enemyUserInfo = DummyManager.GetSimpleAccountInfo(ref TB, enemyInfo.AID);
            setEnemyInfo.AID = AID;
            setEnemyInfo.EnemyAID = enemyUserInfo.aid;
            setEnemyInfo.UserName = enemyUserInfo.username;
            setEnemyInfo.CurrentWarPoint = enemyInfo.MAX_WAR_POINT;
            setEnemyInfo.AdjustWarPoint = enemyInfo.MAX_WAR_POINT;
            List<Character_Detail_With_HP> enemyCharList = DummyManager.GetCharacterListWithEquip_HP(ref TB, setEnemyInfo.EnemyAID).OrderBy(charInfo => charInfo.warpoint).ToList();

            List<User_Character_HP_Info> EnemeyHPInfo = new List<User_Character_HP_Info>();
            enemyCharList.ForEach(charinfo =>
            {
                //double hpRate = setEnemyInfo.AdjustWarPoint != setEnemyInfo.CurrentWarPoint ? ((double)setEnemyInfo.AdjustWarPoint / (double)setEnemyInfo.CurrentWarPoint) : 1.0f;
                EnemeyHPInfo.Add(new User_Character_HP_Info(charinfo, false)); ;
            });

            setEnemyInfo.EnemyCharacter_Main_Info_Json = mJsonSerializer.ToJsonString(EnemeyHPInfo.OrderByDescending(item => item.warpoint).FirstOrDefault());
            setEnemyInfo.EnemyCharacter_Info_Json = mJsonSerializer.ToJsonString(EnemeyHPInfo);
            setEnemyInfo.EnemyCharacter_Detail_Json = Character_Detail_With_HP.makeCharacter_DetailListJson(ref enemyCharList);

            return setEnemyInfo;
        }


        // 매칭 함수 User_WarPoint
        public static Result_Define.eResult MakeMatchingEnemyInfo(ref TxnBlock TB, long AID, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            // 1~3스테이지      // 유저 전투력 기준 – A-10% ~ A%
            // 4~6스테이지      // 유저 전투력 기준 – A% ~ A+10%
            // 7~9스테이지      // 유저 전투력 기준 – A+10% ~ A+20%
            // 10~12스테이지    // 유저 전투력 기준 - A+20% ~ A+30%
            // 13~15스테이지    // 유저 전투력 기준 - A+30% ~ A+50%
            // A는 const에 등록된 상수 값이며 운영툴에서 수정이 가능한 형태이다.

            // A 의 Const 값 = 313	DEF_EXPEDITION_MATCHING_VALUE            
            double matchingVal = SystemData.AdminConstValueFetchFromRedis(ref TB, GoldExpedition_Define.GE_Const_Def_Key_List[GoldExpedition_Define.eGEConstDef.DEF_EXPEDITION_MATCHING_VALUE]);

            //User_WarPoint getUserWP = GetUserWarPoint(ref TB, AID);
            User_WarPoint getUserWP = GetUserCharacterMaxWarPoint(ref TB, AID);
            List<User_GE_Stage_Enemy> getEnemeyList = new List<User_GE_Stage_Enemy>();
            foreach (PvP_Define.FindEnemyRange setEnemyRange in GoldExpedition_Define.GetEnemyRangeList)
            {
                double baseMatch = (matchingVal / GoldExpedition_Define.PercentageDivede) + 1;
                double setMin = (baseMatch + (setEnemyRange.minRate / GoldExpedition_Define.PercentageDivede)) * getUserWP.MAX_WAR_POINT;
                double setMax = (baseMatch + (setEnemyRange.maxRate / GoldExpedition_Define.PercentageDivede)) * getUserWP.MAX_WAR_POINT;

                double setMinCharWarPoint = (baseMatch + (setEnemyRange.minRate / GoldExpedition_Define.PercentageDivede)) * getUserWP.WAR_POINT;
                double setMaxCharWarPoint = (baseMatch + (setEnemyRange.maxRate / GoldExpedition_Define.PercentageDivede)) * getUserWP.WAR_POINT;

                //List<User_WarPoint> checkEnemyList = GetUserWarPointList(ref TB, AID, setEnemyRange.getCount, setMin, setMax);
                List<User_WarPoint> checkEnemyList = GetUserWarPointListByCharacterMaxWarPoint(ref TB, AID, setEnemyRange.getCount, setMin, setMax, setMinCharWarPoint, setMaxCharWarPoint);
                List<User_GE_Stage_Enemy> setRangeEnemyList = new List<User_GE_Stage_Enemy>();

                int addCount = 0;
                foreach (User_WarPoint enemyInfo in checkEnemyList)
                {
                    //User_GE_Stage_Enemy setEnemyInfo = makeEnemyInfo(ref TB, AID, enemyInfo, enemyInfo.WAR_POINT, dbkey);
                    User_GE_Stage_Enemy setEnemyInfo = makeEnemyInfoByCharacterWarPoint(ref TB, AID, enemyInfo, dbkey);
                    if (setEnemyInfo.EnemyAID > 0)
                    {
                        setRangeEnemyList.Add(setEnemyInfo);
                        addCount++;
                    }
                }

                int leftCount = setEnemyRange.getCount - addCount;

                if (leftCount > 0)
                {
                    //checkEnemyList = DummyManager.GetDummyUserWarPointList(ref TB, leftCount, setMin, setMax);
                    checkEnemyList = DummyManager.GetDummyUserWarPointListByCharacterWarPoint(ref TB, leftCount, setMin, setMax, setMinCharWarPoint, setMaxCharWarPoint);

                    foreach (User_WarPoint enemyInfo in checkEnemyList)
                    {
                        //User_GE_Stage_Enemy setEnemyInfo = makeDummyInfo(ref TB, AID, enemyInfo, dbkey);
                        User_GE_Stage_Enemy setEnemyInfo = makeDummyInfoByCharacterWarPoint(ref TB, AID, enemyInfo, dbkey);
                        setRangeEnemyList.Add(setEnemyInfo);
                    }
                }

                //for (int checkCount = 0; checkCount < leftCount; checkCount++)
                //{
                //    int adjustWarpoint = System.Convert.ToInt32(Math.GetRandomDouble(setMin, setMax));

                //    User_WarPoint checkEnemy = GetUserWarPointList_Top100_Random(ref TB, AID);
                //    if (checkEnemy.AID < 0)
                //        return Result_Define.eResult.GE_ENEMY_ID_NOT_FOUND;

                //    User_GE_Stage_Enemy setEnemyInfo = makeEnemyInfo(ref TB, AID, checkEnemy, adjustWarpoint, dbkey);
                //    setRangeEnemyList.Add(setEnemyInfo);
                //}

                getEnemeyList.AddRange(setRangeEnemyList);
            }

            if (getEnemeyList.Count >= GoldExpedition_Define.GetEnemyRangeList.Sum(item => item.getCount))
            {
                getEnemeyList = getEnemeyList.OrderBy(item => item.CurrentWarPoint).ToList();
                short setStage = 0;
                foreach (User_GE_Stage_Enemy enemyInfo in getEnemeyList)
                {
                    setStage++;
                    Result_Define.eResult retResult = SetEnemyInfoToDB(ref TB, AID, setStage, enemyInfo);
                    if (retResult != Result_Define.eResult.SUCCESS)
                        return retResult;
                }
            }
            else
                return Result_Define.eResult.GE_ENEMY_COUNT_NOT_ENOUGH;

            return Result_Define.eResult.SUCCESS;
        }

        public static Result_Define.eResult SetEnemyInfoToDB(ref TxnBlock TB, long AID, short Stage, User_GE_Stage_Enemy setInfo, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON AID = {1} AND Stage = {2}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                EnemyAID = '{3}',
	                                                CurrentWarPoint = '{4}',
	                                                AdjustWarPoint = '{5}',
	                                                EnemyCharacter_Main_Info_Json = N'{6}',
	                                                EnemyCharacter_Info_Json = N'{7}',
	                                                EnemyCharacter_Detail_Json = N'{8}',
	                                                UserName = N'{9}',
	                                                isDummy = '{10}'
                                                WHEN NOT MATCHED THEN
                                                   INSERT VALUES ('{1}', '{2}', '{3}', '{4}', '{5}', N'{6}', N'{7}', N'{8}', N'{9}', '{10}');
                                            ",
                                            GoldExpedition_Define.User_GE_Stage_Enemy_TableName,
                                            AID,
                                            Stage,
                                            setInfo.EnemyAID,
                                            setInfo.CurrentWarPoint,
                                            setInfo.AdjustWarPoint,
                                            setInfo.EnemyCharacter_Main_Info_Json,
                                            setInfo.EnemyCharacter_Info_Json,
                                            setInfo.EnemyCharacter_Detail_Json,
                                            setInfo.UserName,
                                            setInfo.isDummy
                                 );
            RemoveUser_GE_Stage_Enemy(AID);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }


        public static Result_Define.eResult SetBoostInfoToDB(ref TxnBlock TB, long AID, User_GE_Boost_Item setInfo, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON AID = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                boost_1 = '{2}',
	                                                boost_1 = '{3}',
	                                                boost_1 = '{4}'
                                                WHEN NOT MATCHED THEN
                                                   INSERT VALUES ('{1}', '{2}', '{3}', '{4}');
                                            ",
                                            GoldExpedition_Define.User_GE_Boost_Item_TableName,
                                            AID,
                                            setInfo.boost_1,
                                            setInfo.boost_2,
                                            setInfo.boost_3
                                            );
            RemoveCacheUser_GE_Boost_Item(AID);            
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult SetUser_GE_CharacterGroupInfoToDB(ref TxnBlock TB, long AID, User_GE_CharacterGroup setInfo, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON AID = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                cid1 = '{2}',
	                                                cid2 = '{3}',
	                                                cid3 = '{4}',
	                                                cid4 = '{5}'
                                                WHEN NOT MATCHED THEN
                                                   INSERT VALUES ('{1}', '{2}', '{3}', '{4}', '{5}');
                                            ",
                                            GoldExpedition_Define.User_GE_CharacterGroup_TableName,
                                            AID,
                                            setInfo.cid1,
                                            setInfo.cid2,
                                            setInfo.cid3,
                                            setInfo.cid4
                                 );
            //GoldExpedition_Manager.GetGECharacterGroupInfo(ref TB, AID, true);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }


        public static Result_Define.eResult SetUser_Mercenary_InfoToDB(ref TxnBlock TB, long AID, Character_Detail_With_HP charInfo, bool isActive, string dbkey = GoldExpedition_Define.GoldExpedition_Guild_Info_DB)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            Account userInfo = AccountManager.GetAccountData(ref TB, AID, ref retError);
            if (retError != Result_Define.eResult.SUCCESS)
                return retError;

            if (userInfo.GuildID <= 0)
                return Result_Define.eResult.GOLDEXPEDITION_GUILD_MERCENARY_ONLYONE;
            string setInfoJson = mJsonSerializer.ToJsonString(new User_Character_HP_Info(charInfo, true));
            string setDetailInfoJson = charInfo.ToJson();

            string setQuery = string.Format(@"
                                                        MERGE {0} USING (select 'X' as DUAL) AS B
                                                        ON AID = {2} AND CID = {3}
                                                        WHEN MATCHED THEN
                                                           UPDATE SET 
        	                                                GID = '{1}'
        	                                                , AID = '{2}'
        	                                                , CID = '{3}'
        	                                                , IncomeGold = 0
        	                                                , Character_Info_Json = N'{4}'
        	                                                , Character_Detail_Json = N'{5}'
        	                                                , AllyUserName = N'{6}'
        	                                                , ActiveFlag = '{7}'
        	                                                , RegDate = GETDATE()
                                                        WHEN NOT MATCHED THEN
                                                           INSERT VALUES ('{1}', '{2}', '{3}', 0, N'{4}', N'{5}', N'{6}', '{7}', GETDATE());
                                                    ",
                                            GoldExpedition_Define.User_Guild_Mercenary_Info_TableName,
                                            userInfo.GuildID,
                                            userInfo.AID,
                                            charInfo.cid,
                                            setInfoJson,
                                            setDetailInfoJson,
                                            userInfo.UserName,
                                            isActive ? "Y" : "N"
                                 );
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult SetUser_Mercenary_IncomeGoldToDB(ref TxnBlock TB, ref User_Guild_Mercenary_Info mercenaryInfo, bool isActive, string dbkey = GoldExpedition_Define.GoldExpedition_Guild_Info_DB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET IncomeGold = {1}, ActiveFlag = '{4}' WHERE AID = {2} AND CID = {3}",
                                            GoldExpedition_Define.User_Guild_Mercenary_Info_TableName,
                                            mercenaryInfo.IncomeGold,
                                            mercenaryInfo.AID,
                                            mercenaryInfo.CID,
                                            isActive ? "Y" : "N"
                                 );
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }
    }
}
