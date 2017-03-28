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
        /*
    public static partial class _DEL_SoulManager
    {
        const string SoulInvenDBName = "sharding";

        const string ActiveSoulDBTableName = "SoulActive";
        const string ActiveSoulPartsDBTableName = "SoulActiveParts";
        const string ActiveSoulPartsPrefix = "UserActiveSoulParts";
        const string ActiveSoulPrefix = "ActiveSoul";

        const string PassiveSoulDBTableName = "SoulPassive";
        const string PassiveSoulLimitDBTableName = "SoulPassiveLimit";
        const string SoulInvenPrefix = "UserSoul";
        const string PassiveSoulPrefix = "PassiveSoul";
        const string PassiveSoulLimitPrefix = "PassiveSoulLimit";

        private static Dictionary<long, ActiveSoul> GetActiveSoulListFetchFromDB(ref TxnBlock TB, long AID, long CID, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            if (AID == 0)
                return null;

            SqlDataReader getDr = null;
            string setKey = string.Empty;
            string setQuery = string.Format("SELECT SoulSEQ, AID, CID, SoulID, SoulName, ClassType, SoulLevel, SoulGradeLevel, Special_Buff1, Special_Buff2, Special_Buff3, SlotNum FROM {0} WHERE AID = {1} and CID={2} and delflag='N'", ActiveSoulDBTableName, AID, CID);

            TB.ExcuteSqlCommand(dbkey, setQuery, ref getDr);

            Dictionary<long, ActiveSoul> retSet = new Dictionary<long, ActiveSoul>();

            if (getDr != null)
            {
                var r = SQLtoJson.Serialize(ref getDr);
                string json = mJsonSerializer.ToJsonString(r);

                getDr.Dispose(); getDr.Close();
                ActiveSoul[] itemList = mJsonSerializer.JsonToObject<ActiveSoul[]>(json);

                foreach (ActiveSoul iteminfo in itemList)
                {
                    retSet.Add(iteminfo.soulseq, iteminfo);
                    setKey = string.Format("{0}_{1}_{2}_{3}", ActiveSoulPrefix, ActiveSoulDBTableName, AID, iteminfo.cid);

                    RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, iteminfo.soulseq.ToString(), iteminfo);
                }
            }

            return retSet;
        }
        private static Dictionary<long, ActiveSoul> GetActiveSoulListFetchFromRedis(ref TxnBlock TB, long AID, long CID, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            List<Character> charList = CharacterManager.GetCharacterList(ref TB, AID, dbkey, Flush);
            string setKey = string.Empty;
            Dictionary<long, ActiveSoul> ActiveSoulList = new Dictionary<long, ActiveSoul>();

            foreach (Character charInfo in charList)
            {
                if (CID == 0 || CID == charInfo.cid)
                {

                    setKey = string.Format("{0}_{1}_{2}_{3}", ActiveSoulPrefix, ActiveSoulDBTableName, AID, charInfo.cid);

                    Dictionary<string, string> getActiveSoulList = RedisConst.GetRedisInstance().GetHashsAll_Item(DataManager_Define.RedisServerAlias_User, setKey);

                    if (getActiveSoulList == null || Flush)
                        Flush = true;
                    else if (getActiveSoulList.Count == 0)
                        Flush = true;

                    if (Flush)
                    {
                        RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey);
                        Dictionary<long, ActiveSoul> charActiveSoulList = GetActiveSoulListFetchFromDB(ref TB, AID, CID, dbkey, Flush);

                        foreach (KeyValuePair<long, ActiveSoul> setItem in charActiveSoulList)
                        {
                            ActiveSoulList[setItem.Key] = setItem.Value;
                        }
                    }
                    else
                    {
                        foreach (KeyValuePair<string, string> setJson in getActiveSoulList)
                        {
                            ActiveSoul setItem = mJsonSerializer.JsonToObject<ActiveSoul>(setJson.Value);
                            ActiveSoulList[setItem.soulseq] = setItem;
                        }
                    }
                }
            }
            return ActiveSoulList;
        }
        public static Dictionary<long, ActiveSoul> GetActiveSoulList(ref TxnBlock TB, long AID, long CID, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            return GetActiveSoulListFetchFromRedis(ref TB, AID, CID, dbkey, Flush);
        }
        public static Dictionary<long, ActiveSoul> FlushActiveSoulList(ref TxnBlock TB, long AID, long CID, string dbkey = SoulInvenDBName)
        {
            return GetActiveSoulListFetchFromRedis(ref TB, AID, CID, dbkey, true);
        }
        public static Dictionary<long, ActiveSoul> GetActiveSoulEquipList(ref TxnBlock TB, long AID, long CID, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            Dictionary<long, ActiveSoul> setActiveSoulList = new Dictionary<long, ActiveSoul>();
            Dictionary<long, ActiveSoul> getActiveSoul = GetActiveSoulList(ref TB, AID, CID, dbkey, Flush);
            foreach (KeyValuePair<long, ActiveSoul> setItem in getActiveSoul)
            {
                if (setItem.Value.slotnum > 0 && setItem.Value.slotnum < 99)
                    setActiveSoulList[setItem.Key] = setItem.Value;
            }
            return setActiveSoulList;
        }

        public static ActiveSoul GetActiveSoul(ref TxnBlock TB, long AID, long CID, long SoulSEQ, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            Dictionary<long, ActiveSoul> setData = GetActiveSoulList(ref TB, AID, CID, dbkey, Flush);
            if (setData.ContainsKey(SoulSEQ))
                return setData[SoulSEQ];
            else
                return new ActiveSoul();
        }
        public static ActiveSoul GetActiveSoulGroup(ref TxnBlock TB, long AID, long CID, int GroupID, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            ActiveSoul ActiveSoulList = new ActiveSoul();
            Dictionary<long, ActiveSoul> setData = GetActiveSoulList(ref TB, AID, CID, dbkey, Flush);

            foreach (KeyValuePair<long, ActiveSoul> setItem in setData)
            {
                if (System.Convert.ToInt32(System.Convert.ToString(setItem.Value.soulid).Substring(0, System.Convert.ToInt32(System.Convert.ToString(setItem.Value.soulid).Length) - 2)) == GroupID)
                {
                    ActiveSoulList = setItem.Value;
                }
            }
            return ActiveSoulList;
        }
        public static Dictionary<long, ActiveSoulListClass> GetActiveSoulClassList(ref TxnBlock TB, long AID, long CID, string dbkey = SoulInvenDBName)
        {
            Dictionary<long, ActiveSoulListClass> ActiveSoulClassList = new Dictionary<long, ActiveSoulListClass>();
            Dictionary<long, ActiveSoul> ActiveSoulList = GetActiveSoulListFetchFromRedis(ref TB, AID, CID, dbkey, true);
            Character specChar = TheSoul.DataManager.CharacterManager.GetCharacter(ref TB, AID, CID, false, dbkey);
            foreach (KeyValuePair<long, ActiveSoul> getItem in ActiveSoulList)
            {
                ActiveSoulListClass setActiveSoulClass = new ActiveSoulListClass();
                if (getItem.Value.slotnum != 0)
                {
                    ActiveSoulParts getActivePartsInfo = GetActiveSoulPartsIndex(ref TB, AID, System.Convert.ToInt32(System.Convert.ToString(getItem.Value.soulid).Substring(0, 4)), dbkey, false);
                    setActiveSoulClass.SoulSEQ = getItem.Value.soulseq;
                    setActiveSoulClass.SoulID = getItem.Value.soulid;
                    setActiveSoulClass.SoulLevel = getItem.Value.soullevel;
                    setActiveSoulClass.SoulGradeLevel = getItem.Value.soulgradelevel;
                    setActiveSoulClass.SlotNum = getItem.Value.slotnum;
                    setActiveSoulClass.Buff1 = getItem.Value.special_buff1;
                    setActiveSoulClass.Buff2 = getItem.Value.special_buff2;
                    setActiveSoulClass.Buff3 = getItem.Value.special_buff3;
                    setActiveSoulClass.SoulPartsItemEA = getActivePartsInfo.ItemEA;
                    setActiveSoulClass.SoulPartsItemID = getActivePartsInfo.ItemID;
                    setActiveSoulClass.ClassType = specChar.Class;
                    ActiveSoulClassList.Add(getItem.Value.soulseq, setActiveSoulClass);
                }
            }
            return ActiveSoulClassList;
        }

        private static Dictionary<long, PassiveSoul> GetPassiveSoulListFetchFromDB(ref TxnBlock TB, long AID, long CID, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            if (AID == 0)
                return null;

            SqlDataReader getDr = null;
            string setKey = string.Empty;
            string setQuery = string.Format("SELECT SoulSEQ, SoulID, AID, CID, SoulName, sclass, SoulLevel, Buff, SlotNum FROM {0} WHERE AID = {1} and CID={2} and delflag='N'", PassiveSoulDBTableName, AID, CID);

            TB.ExcuteSqlCommand(dbkey, setQuery, ref getDr);

            Dictionary<long, PassiveSoul> retSet = new Dictionary<long, PassiveSoul>();

            if (getDr != null)
            {
                var r = SQLtoJson.Serialize(ref getDr);
                string json = mJsonSerializer.ToJsonString(r);

                getDr.Dispose(); getDr.Close();
                PassiveSoul[] itemList = mJsonSerializer.JsonToObject<PassiveSoul[]>(json);

                foreach (PassiveSoul iteminfo in itemList)
                {
                    retSet.Add(iteminfo.soulseq, iteminfo);
                    setKey = string.Format("{0}_{1}_{2}_{3}", PassiveSoulPrefix, PassiveSoulDBTableName, AID, iteminfo.cid);

                    RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, iteminfo.soulseq.ToString(), iteminfo);
                }
            }

            return retSet;
        }
        private static Dictionary<long, PassiveSoul> GetPassiveSoulListFetchFromRedis(ref TxnBlock TB, long AID, long CID, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            List<Character> charList = CharacterManager.GetCharacterList(ref TB, AID, dbkey, Flush);
            string setKey = string.Empty;
            Dictionary<long, PassiveSoul> PassiveSoulList = new Dictionary<long, PassiveSoul>();

            foreach (Character charInfo in charList)
            {
                if (CID == 0 || CID == charInfo.cid)
                {

                    setKey = string.Format("{0}_{1}_{2}_{3}", PassiveSoulPrefix, PassiveSoulDBTableName, AID, charInfo.cid);

                    Dictionary<string, string> getPassiveSoulList = RedisConst.GetRedisInstance().GetHashsAll_Item(DataManager_Define.RedisServerAlias_User, setKey);

                    if (getPassiveSoulList == null || Flush)
                        Flush = true;
                    else if (getPassiveSoulList.Count == 0)
                        Flush = true;

                    if (Flush)
                    {
                        RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey);
                        Dictionary<long, PassiveSoul> charPassiveSoulList = GetPassiveSoulListFetchFromDB(ref TB, AID, CID, dbkey, Flush);

                        foreach (KeyValuePair<long, PassiveSoul> setItem in charPassiveSoulList)
                        {
                            PassiveSoulList[setItem.Key] = setItem.Value;
                        }
                    }
                    else
                    {
                        foreach (KeyValuePair<string, string> setJson in getPassiveSoulList)
                        {
                            PassiveSoul setItem = mJsonSerializer.JsonToObject<PassiveSoul>(setJson.Value);
                            PassiveSoulList[setItem.soulseq] = setItem;
                        }
                    }
                }
            }
            return PassiveSoulList;
        }
        public static Dictionary<long, PassiveSoul> GetPassiveSoulList(ref TxnBlock TB, long AID, long CID, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            return GetPassiveSoulListFetchFromRedis(ref TB, AID, CID, dbkey, Flush);
        }
        public static Dictionary<long, PassiveSoul> FlushPassiveSoulList(ref TxnBlock TB, long AID, long CID, string dbkey = SoulInvenDBName)
        {
            return GetPassiveSoulListFetchFromRedis(ref TB, AID, CID, dbkey, true);
        }

        public static Dictionary<long, PassiveSoul> GetPassiveSoulEquipList(ref TxnBlock TB, long AID, long CID, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            Dictionary<long, PassiveSoul> setPassiveSoulList = new Dictionary<long, PassiveSoul>();
            Dictionary<long, PassiveSoul> getPassiveSoul = GetPassiveSoulList(ref TB, AID, CID, dbkey, Flush);
            foreach (KeyValuePair<long, PassiveSoul> setItem in getPassiveSoul)
            {
                if (setItem.Value.slotnum > 0 && setItem.Value.slotnum < 99)
                    setPassiveSoulList[setItem.Key] = setItem.Value;
            }
            return setPassiveSoulList;
        }
        public static PassiveSoul GetPassiveSoul(ref TxnBlock TB, long AID, long CID, long SoulSEQ, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            Dictionary<long, PassiveSoul> setData = GetPassiveSoulList(ref TB, AID, CID, dbkey, Flush);
            if (setData.ContainsKey(SoulSEQ))
                return setData[SoulSEQ];
            else
                return new PassiveSoul();
        }
        public static PassiveSoul GetPassiveSoulGroup(ref TxnBlock TB, long AID, long CID, long SoulGroupID, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            PassiveSoul PassiveSoulList = new PassiveSoul();
            Dictionary<long, PassiveSoul> getPassiveSoul = GetPassiveSoulList(ref TB, AID, CID, dbkey, Flush);
            SoulGroupID = System.Convert.ToInt32(System.Convert.ToString(SoulGroupID).Substring(0, System.Convert.ToInt32(System.Convert.ToString(SoulGroupID).Length) - 2));
            foreach (KeyValuePair<long, PassiveSoul> setItem in getPassiveSoul)
            {
                if (System.Convert.ToInt32(System.Convert.ToString(setItem.Value.soulid).Substring(0, System.Convert.ToInt32(System.Convert.ToString(setItem.Value.soulid).Length) - 2)) == SoulGroupID)
                {
                    PassiveSoulList = setItem.Value;
                }
            }
            return PassiveSoulList;
        }

        private static ActiveSoulMake MakeActiveSoulToDB(ref TxnBlock TB, long AID, long CID, int ItemID, int SoulID, int creatediv, int SoulPartsValue, int EVO_Level, string dbkey = SoulInvenDBName)
        {
            int retValue = 0;

            ActiveSoulMake MakeResultSoulList = new ActiveSoulMake();
            Character specChar = TheSoul.DataManager.CharacterManager.GetCharacter(ref TB, AID, CID, false, dbkey);
            GameData_Soul getSoulData = GetSystemSoul(ref TB, SoulID, dbkey, false);

            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = "UserSoulActiveCreate";
            Cmd.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            Cmd.Parameters.Add("@CID", SqlDbType.BigInt).Value = CID;
            Cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = ItemID;
            Cmd.Parameters.Add("@SoulID", SqlDbType.Int).Value = SoulID;
            Cmd.Parameters.Add("@Class", SqlDbType.Int).Value = specChar.Class;
            Cmd.Parameters.Add("@CreateDiv", SqlDbType.Int).Value = creatediv;
            Cmd.Parameters.Add("@SoulPartsValue", SqlDbType.BigInt).Value = SoulPartsValue;
            Cmd.Parameters.Add("@DescCN", SqlDbType.NVarChar, 32).Value = getSoulData.DescCN;
            Cmd.Parameters.Add("@EVO_Level", SqlDbType.TinyInt).Value = EVO_Level;
            SqlDataReader getDr = null;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref Cmd, ref getDr))    // check return by command success
            {
                if (getDr != null)
                {
                    var r = SQLtoJson.Serialize(ref getDr);
                    string json = mJsonSerializer.ToJsonString(r);
                    retValue = System.Convert.ToInt32(mJsonSerializer.GetJsonValue(json, "returncode"));
                    getDr.Dispose(); getDr.Close();

                    if (retValue == 0)
                    {
                        ActiveSoulMake[] ActiveSoulList = mJsonSerializer.JsonToObject<ActiveSoulMake[]>(json);
                        MakeResultSoulList = ActiveSoulList[0];
                        FlushActiveSoulList(ref TB, AID, CID, dbkey);
                    }
                    else
                    {
                        return MakeResultSoulList;
                    }
                }
                else
                {
                    return MakeResultSoulList;
                }
            }
            Cmd.Dispose();
            return MakeResultSoulList;
        }

        // creatediv 0 is not check soul piece, 1 is need check soul piece
        // MakeActiveSoul(ref tb, aid, cid, 0, soulid, 0);
        public static ActiveSoulMake MakeActiveSoul(ref TxnBlock TB, long AID, long CID, int itemid, int soulid, int creatediv = 1, string dbkey = SoulInvenDBName)
        {
            ActiveSoulMake MakeResultSoulList = new ActiveSoulMake();
            ActiveSoulParts getActivePartsInfo = GetActiveSoulParts(ref TB, AID, itemid, dbkey, false);
            GameData_SoulCreate getSoulCreateInfo = GetSystemSoulCreate(ref TB, getActivePartsInfo.SoulPartsIndex, dbkey, false);
            ActiveSoul getCheckActiveSoulGroup = GetActiveSoulGroup(ref TB, AID, CID, getActivePartsInfo.SoulPartsIndex, dbkey, false);
            if (creatediv == 1)
            {
                if (getActivePartsInfo.ItemEA >= getSoulCreateInfo.SoulPartsValue) //필요 혼조각 체크
                {
                    if (getCheckActiveSoulGroup.soulseq == 0) // 혼 생성 체크(혼 그룹번호로 체크)
                    {
                        MakeResultSoulList.resultcode = System.Convert.ToInt32(Result_Define.eResult.SUCCESS);
                        return MakeActiveSoulToDB(ref TB, AID, CID, itemid, getSoulCreateInfo.CreateSoul, creatediv, getSoulCreateInfo.SoulPartsValue, getSoulCreateInfo.EVO_Level, dbkey);
                    }
                    else
                    {
                        MakeResultSoulList.resultcode = System.Convert.ToInt32(Result_Define.eResult.ALREADY_CREATED_SOUL);
                        return MakeResultSoulList;
                    }
                }
                else
                {
                    MakeResultSoulList.resultcode = System.Convert.ToInt32(Result_Define.eResult.CANT_CREATED_NOT_ENOUGH_SOULPIECE);
                    return MakeResultSoulList;
                }
            }
            else
            {
                if (getCheckActiveSoulGroup.soulseq == 0) // 혼 생성 체크(혼 그룹번호로 체크)
                {
                    MakeResultSoulList.resultcode = System.Convert.ToInt32(Result_Define.eResult.SUCCESS);
                    return MakeActiveSoulToDB(ref TB, AID, CID, itemid, soulid, creatediv, getSoulCreateInfo.SoulPartsValue, getSoulCreateInfo.EVO_Level, dbkey);
                }
                else
                {
                    MakeResultSoulList.resultcode = System.Convert.ToInt32(Result_Define.eResult.ALREADY_CREATED_SOUL);
                    return MakeResultSoulList;
                }
            }

        }
        private static GameData_PassiveSoulProb GetPassiveSoulProbInfo(ref TxnBlock TB, long AID, long CID, int ProbID, string dbkey = SoulInvenDBName)
        {
            List<GameData_PassiveSoulProb> getSystemPassiveSoulProb = GetSystemPassiveSoulProb(ref TB, dbkey, false);
            if (getSystemPassiveSoulProb.Count > 0)
                return getSystemPassiveSoulProb.Find(item => item.ID == ProbID);
            else
                return new GameData_PassiveSoulProb();
        }
        private static PassiveSoulMake MakePassiveSoulToDB(ref TxnBlock TB, long AID, long CID, int SoulID, int creatediv, string dbkey = SoulInvenDBName)
        {
            int retValue = 0;
            PassiveSoulMake MakeResultSoulList = new PassiveSoulMake();
            Account getAccInfo = TheSoul.DataManager.AccountManager.GetAccountData(ref TB, AID, false, dbkey);
            Character specChar = TheSoul.DataManager.CharacterManager.GetCharacter(ref TB, AID, CID, false, dbkey);
            PassiveSoulLimit getPassiveLimit = TheSoul.DataManager._DEL_SoulManager.GetPassiveSoulLimitInfo(ref TB, AID, dbkey, false);
            float DEF_PASSIVE_SOUL_COST_RUBY = (float)TheSoul.DataManager.SystemData.GetConstValue(ref TB, "DEF_PASSIVE_SOUL_COST_RUBY", dbkey, false);

            if (creatediv == 1 || creatediv == 2)
            {
                List<GameData_PassiveSoulProb> getSystemPassiveSoulProb = GetSystemPassiveSoulProb(ref TB, dbkey, false);
                GameData_PassiveSoulProbMin getSystemPassiveSoulProbMin = GetSystemPassiveSoulProbMin(ref TB, dbkey, false); // 최저 프랍율
                GameData_PassiveSoulProbTCNT getSystemPassiveSoulProbTCNT = GetSystemPassiveSoulProbTCNT(ref TB, dbkey, false); //총 프랍 로우
                double TotalProb = 0;
                foreach (GameData_PassiveSoulProb iteminfo in getSystemPassiveSoulProb)
                {
                    PassiveSoul getPassiveSoulInfo = GetPassiveSoulGroup(ref TB, AID, CID, iteminfo.PassiveID, dbkey, false); //혼 그룹으로 유저 패시브혼 검색
                    if (getPassiveSoulInfo.soulseq == 0)
                        TotalProb = TotalProb + (double)iteminfo.PROB;//패시브 확율 총합 (내가 보유한 혼 제외)
                }

                long RootNum = 1;
                int MinProbLen = System.Convert.ToInt32(System.Convert.ToString(getSystemPassiveSoulProbMin.PROB).Length);
                for (int LoopNum = 1; LoopNum < MinProbLen; LoopNum++)
                {
                    RootNum = RootNum * 10;//정수 전환용 배수
                }
                long TotalDropProb = (long)(RootNum * TotalProb);//해당 그룹 랜덤 아이템 총 드랍률

                Random rnd = new Random();
                long RndNum = rnd.NextLong(1, TotalDropProb); // 총 드랍 수량에서 랜덤 숫자 생성

                long TotalProbAdd = 0;
                long TotalDropProb2 = 0;
                long ThisProb = 0;
                int LoopNum3 = 0;
                int DrobSoulID = 0;
                for (int LoopNum2 = 0; LoopNum2 < getSystemPassiveSoulProbTCNT.totalcount; LoopNum2++)//드랍율 테이블 로우 만큼 루프
                {
                    LoopNum3 = LoopNum2 + 1;
                    GameData_PassiveSoulProb getSystemPassiveSoulProbinfo = GetPassiveSoulProbInfo(ref TB, AID, CID, LoopNum3, dbkey);//혼 드랍 테이블 검색
                    PassiveSoul getPassiveSoulInfo = GetPassiveSoulGroup(ref TB, AID, CID, getSystemPassiveSoulProbinfo.PassiveID, dbkey, false); //혼 그룹으로 유저 패시브혼 검색
                    ThisProb = (long)(RootNum * (double)getSystemPassiveSoulProbinfo.PROB); //드랍그룹의 해당 아이템 현재 드랍률
                    TotalDropProb2 = TotalProbAdd + ThisProb; //총 드랍률 계산
                    if (getPassiveSoulInfo.soulseq == 0)//보유한 혼이 아닐때 실행
                    {
                        if (RndNum > TotalProbAdd && RndNum <= TotalDropProb2)//해당 드랍율 수량에 포함 될 경우 확정
                        {
                            DrobSoulID = getSystemPassiveSoulProbinfo.PassiveID;
                            break;
                        }
                        else
                        {
                            TotalProbAdd = TotalDropProb2;
                        }
                    }
                    else
                    {
                        TotalProbAdd = TotalDropProb2;
                    }
                }
                SoulID = DrobSoulID;
            }
            GameData_SoulPassive getSoulPassiveData = GetSystemSoulPassive(ref TB, SoulID, dbkey, false);
            int Buff = 0;
            if (specChar.Class == 1)
                Buff = getSoulPassiveData.Buff_1;
            else if (specChar.Class == 2)
                Buff = getSoulPassiveData.Buff_2;
            else if (specChar.Class == 3)
                Buff = getSoulPassiveData.Buff_3;

            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = "UserSoulPassiveCreate";
            Cmd.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            Cmd.Parameters.Add("@CID", SqlDbType.BigInt).Value = CID;
            Cmd.Parameters.Add("@SoulID", SqlDbType.Int).Value = SoulID;
            Cmd.Parameters.Add("@Class", SqlDbType.Int).Value = specChar.Class;
            Cmd.Parameters.Add("@MyCash", SqlDbType.Int).Value = getAccInfo.Cash + getAccInfo.EventCash;
            Cmd.Parameters.Add("@MyPassivePoint", SqlDbType.Int).Value = getAccInfo.Stone;
            Cmd.Parameters.Add("@RemainCreateCNT", SqlDbType.Int).Value = getPassiveLimit.PassiveLimitCost;
            Cmd.Parameters.Add("@CreateRubyCost", SqlDbType.Int).Value = DEF_PASSIVE_SOUL_COST_RUBY;
            Cmd.Parameters.Add("@CreateDiv", SqlDbType.Int).Value = creatediv;
            Cmd.Parameters.Add("@Buff", SqlDbType.Int).Value = Buff;
            Cmd.Parameters.Add("@SoulName", SqlDbType.NVarChar, 32).Value = getSoulPassiveData.DescCN;
            SqlDataReader getDr = null;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref Cmd, ref getDr))    // check return by command success
            {
                if (getDr != null)
                {
                    var r = SQLtoJson.Serialize(ref getDr);
                    string json = mJsonSerializer.ToJsonString(r);
                    retValue = System.Convert.ToInt32(mJsonSerializer.GetJsonValue(json, "returncode"));
                    getDr.Dispose(); getDr.Close();

                    if (retValue == 0)
                    {
                        if (creatediv == 2)
                        {
                            Result_Define.eResult UseUserGold = AccountManager.UseUserCash(ref TB, AID, System.Convert.ToInt32(DEF_PASSIVE_SOUL_COST_RUBY), dbkey);//캐시 차감
                        }
                        PassiveSoulMake[] PassiveSoulList = mJsonSerializer.JsonToObject<PassiveSoulMake[]>(json);
                        MakeResultSoulList = PassiveSoulList[0];
                        FlushPassiveSoulList(ref TB, AID, CID, dbkey);
                        FlushPassiveSoulLimitInfo(ref TB, AID, dbkey);
                    }
                    else
                    {
                        return MakeResultSoulList;
                    }
                }
                else
                {
                    return MakeResultSoulList;
                }
            }
            Cmd.Dispose();
            return MakeResultSoulList;
        }
        public static PassiveSoulMake MakePassiveSoul(ref TxnBlock TB, long AID, long CID, int SoulID, int creatediv = 1, string dbkey = SoulInvenDBName)
        {
            if (creatediv == 0 && CID != 0 && SoulID != 0 && AID != 0)
            {
                return MakePassiveSoulToDB(ref TB, AID, CID, SoulID, creatediv, dbkey);
            }
            else
            {
                Account getAccInfo = TheSoul.DataManager.AccountManager.GetAccountData(ref TB, AID, false, dbkey);
                float DEF_PASSIVE_SOUL_COST_RUBY = (float)TheSoul.DataManager.SystemData.GetConstValue(ref TB, "DEF_PASSIVE_SOUL_COST_RUBY", dbkey, false);
                PassiveSoulLimit getPassiveLimit = TheSoul.DataManager._DEL_SoulManager.GetPassiveSoulLimitInfo(ref TB, AID, dbkey, false);
                PassiveSoulMake MakeResultSoulList = new PassiveSoulMake();

                //패시브 생성 포인트 없을시
                if (getAccInfo.Stone < 1)
                {
                    if (getAccInfo.Cash + getAccInfo.EventCash < DEF_PASSIVE_SOUL_COST_RUBY)//루비 부족
                    {
                        MakeResultSoulList.resultcode = System.Convert.ToInt32(Result_Define.eResult.PASSIVE_SOUL_CREATE_RUBY_NOT_ENOUGH);
                        return MakeResultSoulList;
                    }
                    else if (getPassiveLimit.PassiveLimitCost < 1)//생성 초과
                    {
                        MakeResultSoulList.resultcode = System.Convert.ToInt32(Result_Define.eResult.PASSIVE_SOUL_LIMIIT_OVER);
                        return MakeResultSoulList;
                    }
                    else
                    {
                        return MakePassiveSoulToDB(ref TB, AID, CID, SoulID, creatediv = 2, dbkey);
                    }
                }
                else
                {
                    if (getPassiveLimit.PassiveLimitCost < 1)//생성 초과
                    {
                        MakeResultSoulList.resultcode = System.Convert.ToInt32(Result_Define.eResult.PASSIVE_SOUL_LIMIIT_OVER);
                        return MakeResultSoulList;
                    }
                    else
                    {
                        return MakePassiveSoulToDB(ref TB, AID, CID, SoulID, creatediv, dbkey);
                    }
                }
            }

        }
        private static PassiveSoulLimit PassiveSoulLimitCreateDB(ref TxnBlock TB, long AID, string dbkey = SoulInvenDBName)
        {
            PassiveSoulLimit getLimitInfo = new PassiveSoulLimit();
            float DEF_LIMIT_PASSIVE_SOUL_COST_RUBY = (float)TheSoul.DataManager.SystemData.GetConstValue(ref TB, "DEF_LIMIT_PASSIVE_SOUL_COST_RUBY", dbkey, false);
            string setQuery = string.Format("exec SoulPassiveCreateLimit {0},{1}", AID, DEF_LIMIT_PASSIVE_SOUL_COST_RUBY);
            if (!TB.ExcuteSqlCommand(dbkey, setQuery))
                return getLimitInfo;

            getLimitInfo = FlushPassiveSoulLimitInfo(ref TB, AID, dbkey);

            return getLimitInfo;
        }
        private static PassiveSoulLimit PassiveSoulLimitFetchFromDB(ref TxnBlock TB, long AID, string dbkey = SoulInvenDBName)
        {
            SqlDataReader getDr = null;
            string setQuery = string.Format("SELECT AID, PassiveLimitCost FROM {0} WHERE Regdate=convert(varchar(10),getdate(),21) AND AID = {1}", PassiveSoulLimitDBTableName, AID);
            TB.ExcuteSqlCommand(dbkey, setQuery, ref getDr);

            if (getDr != null)
            {
                var r = SQLtoJson.Serialize(ref getDr);
                string json = mJsonSerializer.ToJsonString(r);
                getDr.Dispose(); getDr.Close();
                PassiveSoulLimit[] retSet = mJsonSerializer.JsonToObject<PassiveSoulLimit[]>(json);

                if (retSet.Length > 0)
                    return retSet[0];
                else
                    return null;
            }
            else
            {
                PassiveSoulLimit getLimitInfo = new PassiveSoulLimit();
                getLimitInfo = PassiveSoulLimitCreateDB(ref TB, AID, dbkey);
                return getLimitInfo;
            }
        }
        private static PassiveSoulLimit PassiveSoulLimitFetchFromRedis(ref TxnBlock TB, long AID, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}_{2}", PassiveSoulLimitPrefix, PassiveSoulLimitDBTableName, AID);
            PassiveSoulLimit getLimitInfo = RedisConst.GetRedisInstance().GetObj<PassiveSoulLimit>(setKey);

            if (getLimitInfo != null && !Flush)
            {
                if (getLimitInfo.AID != AID)
                {
                    getLimitInfo = PassiveSoulLimitFetchFromDB(ref TB, AID, dbkey);
                    RedisConst.GetRedisInstance().SetObj(setKey, getLimitInfo);
                }
            }
            else
            {
                getLimitInfo = PassiveSoulLimitFetchFromDB(ref TB, AID, dbkey);
                if (getLimitInfo == null)
                {
                    getLimitInfo = PassiveSoulLimitCreateDB(ref TB, AID, dbkey);
                    RedisConst.GetRedisInstance().SetObj(setKey, getLimitInfo);
                }
                else
                {
                    RedisConst.GetRedisInstance().SetObj(setKey, getLimitInfo);
                }
            }
            return getLimitInfo;
        }
        public static PassiveSoulLimit FlushPassiveSoulLimitInfo(ref TxnBlock TB, long AID, string dbkey = SoulInvenDBName)
        {
            return PassiveSoulLimitFetchFromRedis(ref TB, AID, dbkey, true);
        }
        public static PassiveSoulLimit GetPassiveSoulLimitInfo(ref TxnBlock TB, long AID, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            return PassiveSoulLimitFetchFromRedis(ref TB, AID, dbkey, Flush);
        }

        private static Dictionary<int, ActiveSoulParts> GetActiveSoulPartsListFetchFromDB(ref TxnBlock TB, long AID, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            if (AID == 0)
                return null;

            SqlDataReader getDr = null;
            string setKey = string.Empty;
            string setQuery = string.Format("SELECT * FROM {0} WHERE AID = {1}", ActiveSoulPartsDBTableName, AID);

            TB.ExcuteSqlCommand(dbkey, setQuery, ref getDr);

            Dictionary<int, ActiveSoulParts> retSet = new Dictionary<int, ActiveSoulParts>();

            if (getDr != null)
            {
                var r = SQLtoJson.Serialize(ref getDr);
                string json = mJsonSerializer.ToJsonString(r);

                getDr.Dispose(); getDr.Close();
                ActiveSoulParts[] itemList = mJsonSerializer.JsonToObject<ActiveSoulParts[]>(json);

                foreach (ActiveSoulParts iteminfo in itemList)
                {
                    retSet.Add(iteminfo.ItemID, iteminfo);
                    setKey = string.Format("{0}_{1}_{2}", ActiveSoulPartsPrefix, ActiveSoulPartsDBTableName, AID);

                    RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, iteminfo.ItemID.ToString(), iteminfo);
                }
            }

            return retSet;
        }
        private static Dictionary<int, ActiveSoulParts> GetActiveSoulPartsListFetchFromRedis(ref TxnBlock TB, long AID, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            string setKey = string.Empty;
            Dictionary<int, ActiveSoulParts> ActiveSoulPartsList = new Dictionary<int, ActiveSoulParts>();

            setKey = string.Format("{0}_{1}_{2}", ActiveSoulPartsPrefix, ActiveSoulPartsDBTableName, AID);

            Dictionary<string, string> getActiveSoulPartsList = RedisConst.GetRedisInstance().GetHashsAll_Item(DataManager_Define.RedisServerAlias_User, setKey);

            if (getActiveSoulPartsList == null || Flush)
                Flush = true;
            else if (getActiveSoulPartsList.Count == 0)
                Flush = true;

            if (Flush)
            {
                RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey);
                Dictionary<int, ActiveSoulParts> charActiveSoulPartsList = GetActiveSoulPartsListFetchFromDB(ref TB, AID, dbkey, Flush);

                foreach (KeyValuePair<int, ActiveSoulParts> setItem in charActiveSoulPartsList)
                {
                    ActiveSoulPartsList[setItem.Key] = setItem.Value;
                }
            }
            else
            {
                foreach (KeyValuePair<string, string> setJson in getActiveSoulPartsList)
                {
                    ActiveSoulParts setItem = mJsonSerializer.JsonToObject<ActiveSoulParts>(setJson.Value);
                    ActiveSoulPartsList[setItem.ItemID] = setItem;
                }
            }
            return ActiveSoulPartsList;
        }
        public static Dictionary<int, ActiveSoulParts> GetActiveSoulPartsList(ref TxnBlock TB, long AID, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            return GetActiveSoulPartsListFetchFromRedis(ref TB, AID, dbkey, Flush);
        }
        public static Dictionary<int, ActiveSoulParts> FlushActiveSoulPartsList(ref TxnBlock TB, long AID, string dbkey = SoulInvenDBName)
        {
            return GetActiveSoulPartsListFetchFromRedis(ref TB, AID, dbkey, true);
        }
        public static ActiveSoulParts GetActiveSoulParts(ref TxnBlock TB, long AID, int ItemID, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            Dictionary<int, ActiveSoulParts> setData = GetActiveSoulPartsList(ref TB, AID, dbkey, Flush);
            if (setData.ContainsKey(ItemID))
                return setData[ItemID];
            else
                return new ActiveSoulParts();
        }
        public static ActiveSoulParts GetActiveSoulPartsIndex(ref TxnBlock TB, long AID, int SoulPartsIndex, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            ActiveSoulParts setActivePartsInfo = new ActiveSoulParts();
            Dictionary<int, ActiveSoulParts> getData = GetActiveSoulPartsList(ref TB, AID, dbkey, Flush);
            foreach (KeyValuePair<int, ActiveSoulParts> setItem in getData)
            {
                if (setItem.Value.SoulPartsIndex == SoulPartsIndex)
                {
                    setActivePartsInfo = setItem.Value;
                }
            }
            return setActivePartsInfo;
        }
        public static ActiveSoulParts AddActiveSoulParts(ref TxnBlock TB, long AID, int ItemID, int ItemEA, string dbkey = SoulInvenDBName)
        {
            ActiveSoulParts getActivePartsInfo = new ActiveSoulParts();
            string setQuery = string.Format("exec UserSoulActivePartsAdd {0},{1},{2}", AID, ItemID, ItemEA);
            if (!TB.ExcuteSqlCommand(dbkey, setQuery))
                return getActivePartsInfo;

            Dictionary<int, ActiveSoulParts> setData = FlushActiveSoulPartsList(ref TB, AID, dbkey);

            if (setData.ContainsKey(ItemID))
                return setData[ItemID];
            else
                return new ActiveSoulParts();
        }
        public static ActiveSoulParts UseActiveSoulParts(ref TxnBlock TB, long AID, int ItemID, int UseItemEA, string dbkey = SoulInvenDBName)
        {
            ActiveSoulParts setActivePartsInfo = new ActiveSoulParts();
            string setQuery = string.Format("UPDATE dbo.SoulActiveParts SET ItemEA=ItemEA-{0} WHERE AID={1} AND ItemID={2}", UseItemEA, AID, ItemID);
            if (!TB.ExcuteSqlCommand(dbkey, setQuery))
                return setActivePartsInfo;

            Dictionary<int, ActiveSoulParts> setData = FlushActiveSoulPartsList(ref TB, AID, dbkey);

            if (setData.ContainsKey(ItemID))
                return setData[ItemID];
            else
                return new ActiveSoulParts();
        }

        // add by manstar - 2015/07/07
        // need change after test
        public static ActiveSoul GetBonusSoul(ref TxnBlock TB, long AID, long CID, long DungeonID, string dbkey = SoulInvenDBName)
        {
            SqlCommand commandGetBonusSoul = new SqlCommand();
            commandGetBonusSoul.CommandText = "GetGuerrillaDungeonBonusSoul2";
            var SoulID = new SqlParameter("@RESULTSOULID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var Grade = new SqlParameter("@RESULTGRADE", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var SpecialBuff1 = new SqlParameter("@RESULTSPECIALBUFF1", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var SpecialBuff2 = new SqlParameter("@RESULTSPECIALBUFF2", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var SpecialBuff3 = new SqlParameter("@RESULTSPECIALBUFF3", SqlDbType.Int) { Direction = ParameterDirection.Output };
            commandGetBonusSoul.Parameters.Add("@aid", SqlDbType.BigInt).Value = AID;
            commandGetBonusSoul.Parameters.Add("@cid", SqlDbType.BigInt).Value = CID;
            commandGetBonusSoul.Parameters.Add("@DungeonID", SqlDbType.BigInt).Value = DungeonID;
            commandGetBonusSoul.Parameters.Add(SoulID);
            commandGetBonusSoul.Parameters.Add(Grade);
            commandGetBonusSoul.Parameters.Add(SpecialBuff1);
            commandGetBonusSoul.Parameters.Add(SpecialBuff2);
            commandGetBonusSoul.Parameters.Add(SpecialBuff3);

            ActiveSoul retObj = new ActiveSoul();
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandGetBonusSoul))
            {
                int retSoulID = int.Parse(SoulID.Value.ToString());
                int retGrade = int.Parse(Grade.Value.ToString());
                int retSpecialBuff1 = int.Parse(SpecialBuff1.Value.ToString());
                int retSpecialBuff2 = int.Parse(SpecialBuff2.Value.ToString());
                int retSpecialBuff3 = int.Parse(SpecialBuff3.Value.ToString());
                retObj.soulid = retSoulID;
                retObj.soulgradelevel = System.Convert.ToByte(retGrade);
                retObj.special_buff1 = retSpecialBuff1;
                retObj.special_buff2 = retSpecialBuff2;
                retObj.special_buff3 = retSpecialBuff3;
            }

            return retObj;
        }

        private const string SoulCreateTableName = "GameData_SoulCreate";
        private const string SoulCreateTablePrefix = "System_SoulCreate";
        public static GameData_SoulCreate GetSystemSoulCreate(ref TxnBlock TB, int SoulPartsIndex, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", SoulCreateTablePrefix, SoulPartsIndex);
            string setQuery = string.Format(@"SELECT * FROM {0} WHERE SoulPartsIndex={1}", SoulCreateTableName, SoulPartsIndex);

            GameData_SoulCreate retObj = GenericFetch.FetchFromRedis_Hash<GameData_SoulCreate>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, SoulPartsIndex.ToString(), setQuery, dbkey, Flush);
            return (retObj != null) ? retObj : new GameData_SoulCreate();
        }
        private const string SystemSoulTableName = "GameData_Soul";
        private const string SystemSoulTablePrefix = "System_Soul";
        public static GameData_Soul GetSystemSoul(ref TxnBlock TB, int SoulID, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", SystemSoulTablePrefix, SoulID);
            string setQuery = string.Format(@"SELECT * FROM {0} WHERE SoulID={1}", SystemSoulTableName, SoulID);

            GameData_Soul retObj = GenericFetch.FetchFromRedis_Hash<GameData_Soul>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, SoulID.ToString(), setQuery, dbkey, Flush);
            return (retObj != null) ? retObj : new GameData_Soul();
        }

        private const string SystemSkillLevelTableName = "GameData_SkillLevel";
        private const string SystemSkillLevelTablePrefix = "System_SkillLevel";
        public static GameData_SkillLevel GetSystemSkillLevel(ref TxnBlock TB, int SkillLevelID, int SkillLevel, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", SystemSkillLevelTablePrefix, SkillLevelID);
            string setQuery = string.Format(@"SELECT * FROM {0} WHERE SkillLevelID={1}", SystemSkillLevelTableName, SkillLevelID);
            List<GameData_SkillLevel> retObj = GenericFetch.FetchFromRedis_MultipleRow_Hash<GameData_SkillLevel>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, SkillLevelID.ToString(), setQuery, dbkey, Flush);
            if (retObj.Count > 0)
                return retObj.Find(item => item.SkillLevel == SkillLevel);
            else
                return new GameData_SkillLevel();
        }

        private const string PassiveSoulProbTableName = "GameData_PassiveSoulProb";
        private const string PassiveSoulProbTablePrefix = "System_PassiveSoulProb";
        public static List<GameData_PassiveSoulProb> GetSystemPassiveSoulProb(ref TxnBlock TB, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", PassiveSoulProbTablePrefix, "Prob");
            string setQuery = string.Format(@"SELECT * FROM {0} WHERE PROB<>0", PassiveSoulProbTableName);
            List<GameData_PassiveSoulProb> retObj = GenericFetch.FetchFromRedis_MultipleRow_Hash<GameData_PassiveSoulProb>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, "Prob", setQuery, dbkey, Flush);
            if (retObj.Count > 0)
                return retObj;
            else
                return new List<GameData_PassiveSoulProb>();
        }
        private static GameData_PassiveSoulProbMin GetSystemPassiveSoulProbMin(ref TxnBlock TB, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", PassiveSoulProbTablePrefix, "ProbMin");
            string setQuery = string.Format(@"SELECT MIN(PROB) AS PROB FROM {0} WHERE PROB<>0", PassiveSoulProbTableName);
            GameData_PassiveSoulProbMin retObj = GenericFetch.FetchFromRedis_Hash<GameData_PassiveSoulProbMin>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, "ProbMin", setQuery, dbkey, Flush);
            return (retObj != null) ? retObj : new GameData_PassiveSoulProbMin();
        }
        private static GameData_PassiveSoulProbTCNT GetSystemPassiveSoulProbTCNT(ref TxnBlock TB, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", PassiveSoulProbTablePrefix, "ProbTotalCount");
            string setQuery = string.Format(@"SELECT COUNT(*) AS totalcount FROM {0} WHERE PROB<>0", PassiveSoulProbTableName);
            GameData_PassiveSoulProbTCNT retObj = GenericFetch.FetchFromRedis_Hash<GameData_PassiveSoulProbTCNT>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, "ProbTotalCount", setQuery, dbkey, Flush);
            return (retObj != null) ? retObj : new GameData_PassiveSoulProbTCNT();
        }

        private static ActiveSoulLevelUP ActiveSoulLevelUPToDB(ref TxnBlock TB, long AID, long CID, long SoulSEQ, int SoulLevel, int UseGold, string dbkey = SoulInvenDBName)
        {
            ActiveSoulLevelUP setLevelUPSoul = new ActiveSoulLevelUP();
            Result_Define.eResult UseUserGold = AccountManager.UseUserGold(ref TB, AID, UseGold, dbkey);//골드 차감
            Account getAccInfo = AccountManager.FlushAccountData(ref TB, AID);
            string setQuery = string.Format("UPDATE dbo.SoulActive SET SoulLevel=SoulLevel+1 WHERE SoulSEQ={0}", SoulSEQ);
            if (TB.ExcuteSqlCommand(dbkey, setQuery))
            {
                FlushActiveSoulList(ref TB, AID, CID, dbkey);
                setLevelUPSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.SUCCESS);
                setLevelUPSoul.soullevel = System.Convert.ToByte(SoulLevel + 1);
                setLevelUPSoul.soulseq = SoulSEQ;
                setLevelUPSoul.gold = getAccInfo.Gold;
                return setLevelUPSoul;
            }
            else
            {
                setLevelUPSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.DB_ERROR);
                return setLevelUPSoul;
            }
        }
        public static ActiveSoulLevelUP LevelUPActiveSoul(ref TxnBlock TB, long AID, long CID, long SoulSEQ, string dbkey = SoulInvenDBName)
        {
            int SkillLevelID = 0;
            ActiveSoulLevelUP setLevelUPSoul = new ActiveSoulLevelUP();
            ActiveSoul getActiveSoulInfo = GetActiveSoul(ref TB, AID, CID, SoulSEQ, dbkey, false);
            Account getAccInfo = TheSoul.DataManager.AccountManager.GetAccountData(ref TB, AID, false, dbkey);
            Character specChar = TheSoul.DataManager.CharacterManager.GetCharacter(ref TB, AID, CID, false, dbkey);
            GameData_Soul getSoulData = GetSystemSoul(ref TB, getActiveSoulInfo.soulid, dbkey, false);
            if (specChar.Class == 1)
                SkillLevelID = getSoulData.PrimarySkillID_1;
            else if (specChar.Class == 2)
                SkillLevelID = getSoulData.PrimarySkillID_2;
            else if (specChar.Class == 3)
                SkillLevelID = getSoulData.PrimarySkillID_3;
            GameData_SkillLevel getSkillLevel = TheSoul.DataManager._DEL_SoulManager.GetSystemSkillLevel(ref TB, SkillLevelID, getActiveSoulInfo.soullevel, dbkey, false);

            if (specChar.level >= getActiveSoulInfo.soullevel)//캐릭터 레벨과 혼 레벨 비교(혼 레벨업은 캐릭터 레벨에 맞춰 레벨업 가능)
            {
                if (specChar.level == getSoulData.PassLevel) //돌파 이후 레벨업 가능 (레벨업 불가)
                {
                    setLevelUPSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.LEVELUP_SOUL_AFTER_LEVELPASS);
                    return setLevelUPSoul;
                }
                else
                {
                    if (getAccInfo.Gold >= getSkillLevel.LevelUp_Gold) // 레벨업 필요 골드 체크
                    {
                        return ActiveSoulLevelUPToDB(ref TB, AID, CID, SoulSEQ, getActiveSoulInfo.soullevel, getSkillLevel.LevelUp_Gold, dbkey);
                    }
                    else
                    {
                        setLevelUPSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.NOT_ENOUGH_LEVELUP_GOLD);
                        return setLevelUPSoul;
                    }
                }
            }
            else
            {
                setLevelUPSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.NO_EXCESS_CHARACTERLV_SOUL_LEVELUP);
                return setLevelUPSoul;
            }

        }
        private const string SystemSoulSkillGroupTableName = "GameData_SoulSkillGroup";
        private const string SystemSoulSkillGroupTablePrefix = "System_SoulSkillGroup";
        public static GameData_SoulSkillGroup GetSystemSoulSkillGroup(ref TxnBlock TB, int GroupID, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            Random rnd = new Random();
            int SkillGroupRND = rnd.Next(1, 5);//1~5 랜덤 그룹 선택
            string setKey = string.Format("{0}_{1}", SystemSoulSkillGroupTablePrefix, GroupID);
            string setQuery = string.Format(@"SELECT * FROM {0} WHERE GroupID={1} and Level=1", SystemSoulSkillGroupTableName, GroupID);
            List<GameData_SoulSkillGroup> retObj = GenericFetch.FetchFromRedis_MultipleRow_Hash<GameData_SoulSkillGroup>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, GroupID.ToString(), setQuery, dbkey, Flush);
            if (retObj.Count > 0)
                return retObj.Find(item => item.Group == SkillGroupRND);
            else
                return new GameData_SoulSkillGroup();
        }
        public static GameData_SoulSkillGroup GetSystemSoulSkillGroupEpic(ref TxnBlock TB, int GroupID, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}_Epic", SystemSoulSkillGroupTablePrefix, GroupID);
            string setQuery = string.Format(@"SELECT TOP 1 * FROM {0} WHERE GroupID={1} and Level=1", SystemSoulSkillGroupTableName, GroupID);
            GameData_SoulSkillGroup retObj = GenericFetch.FetchFromRedis_Hash<GameData_SoulSkillGroup>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, GroupID.ToString(), setQuery, dbkey, Flush);
            return (retObj != null) ? retObj : new GameData_SoulSkillGroup();
        }
        private static ActiveSoulLevelPass ActiveSoulLevelPassToDB(ref TxnBlock TB, long AID, long CID, long SoulSEQ, int MBuff, int RBuff, int EBuff, int ChangeSoulID, int SoulID, int ClassType, int SoulLevel, long RemainItem, long InvenSEQ, string dbkey = SoulInvenDBName)
        {
            ActiveSoulLevelPass setLevelPassSoul = new ActiveSoulLevelPass();
            string setQuery = string.Format("UPDATE dbo.SoulActive SET Special_Buff1={0}, Special_Buff2={1}, Special_Buff3={2}, SoulID={3}, SoulLevel=SoulLevel+1 WHERE AID={4} and SoulID={5} and ClassType={6}", MBuff, RBuff, EBuff, ChangeSoulID, AID, SoulID, ClassType);
            if (TB.ExcuteSqlCommand(dbkey, setQuery))
            {
                FlushActiveSoulList(ref TB, AID, CID, dbkey);

                setLevelPassSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.SUCCESS);

                setLevelPassSoul.soulseq = SoulSEQ;
                setLevelPassSoul.soulid = ChangeSoulID;
                setLevelPassSoul.soullevel = System.Convert.ToByte(SoulLevel + 1);
                setLevelPassSoul.special_buff1 = MBuff;
                setLevelPassSoul.special_buff2 = RBuff;
                setLevelPassSoul.special_buff3 = EBuff;
                setLevelPassSoul.passitem_invenseq = InvenSEQ;
                setLevelPassSoul.passitem_itemea = (int)RemainItem;
                return setLevelPassSoul;
            }
            else
            {
                setLevelPassSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.DB_ERROR);
                return setLevelPassSoul;
            }
        }
        public static ActiveSoulLevelPass LevelPassActiveSoul(ref TxnBlock TB, long AID, long CID, long SoulSEQ, string dbkey = SoulInvenDBName)
        {
            long InvenSEQ = 0;
            int ItemEA = 0;
            long ItemID = 0;
            ActiveSoulLevelPass setLevelPassSoul = new ActiveSoulLevelPass();
            ActiveSoul getActiveSoulInfo = GetActiveSoul(ref TB, AID, CID, SoulSEQ, dbkey, false);
            GameData_Soul getSoulData = GetSystemSoul(ref TB, getActiveSoulInfo.soulid, dbkey, false);
            List<User_Inven> itemList = ItemManager.GetInvenList(ref TB, AID, CID);

            Account getAccInfo = TheSoul.DataManager.AccountManager.GetAccountData(ref TB, AID, false, dbkey);
            Character specChar = TheSoul.DataManager.CharacterManager.GetCharacter(ref TB, AID, CID, false, dbkey);
            List<Character_Detail> userCharList = CharacterManager.GetCharacterListWithEquip(ref TB, AID);

            foreach (User_Inven setItem in itemList)
            {
                if (setItem.itemid == getSoulData.PassItemIndex)
                {
                    InvenSEQ = setItem.invenseq;
                    ItemEA = setItem.itemea;
                    ItemID = setItem.itemid;
                }
            }


            GameData_SoulSkillGroup getSkillLevel = null;
            int MBuff = 0;
            int RBuff = 0;
            int EBuff = 0;
            long RemainItem = 0;

            if (specChar.level >= getSoulData.PassLevel) //캐릭터 레벨이 돌파 레벨보다 크거나 같을 경우 돌파 가능
            {
                if (getSoulData.PassLevel == getActiveSoulInfo.soullevel) //돌파 레벨과 혼레벨 동일해야 돌파 가능
                {
                    if (ItemEA >= getSoulData.PassItemAmout) // 보유 돌파단 갯수 체크
                    {
                        foreach (Character setItem in userCharList)
                        {
                            if (setItem.Class == 1)
                            {
                                if (getSoulData.Special_Buff_M_1 != 0)
                                {
                                    getSkillLevel = GetSystemSoulSkillGroup(ref TB, getSoulData.Special_Buff_M_1, dbkey, false);
                                    MBuff = getSkillLevel.BuffID;
                                }
                                if (getSoulData.Special_Buff_R_1 != 0)
                                {
                                    getSkillLevel = GetSystemSoulSkillGroup(ref TB, getSoulData.Special_Buff_R_1, dbkey, false);
                                    RBuff = getSkillLevel.BuffID;
                                }
                                if (getSoulData.Special_Buff_E_1 != 0)
                                {
                                    getSkillLevel = GetSystemSoulSkillGroupEpic(ref TB, getSoulData.Special_Buff_E_1, dbkey, false);
                                    EBuff = getSkillLevel.BuffID;
                                }
                            }
                            if (setItem.Class == 2)
                            {
                                if (getSoulData.Special_Buff_M_2 != 0)
                                {
                                    getSkillLevel = GetSystemSoulSkillGroup(ref TB, getSoulData.Special_Buff_M_2, dbkey, false);
                                    MBuff = getSkillLevel.BuffID;
                                }
                                if (getSoulData.Special_Buff_R_2 != 0)
                                {
                                    getSkillLevel = GetSystemSoulSkillGroup(ref TB, getSoulData.Special_Buff_R_2, dbkey, false);
                                    RBuff = getSkillLevel.BuffID;
                                }
                                if (getSoulData.Special_Buff_E_2 != 0)
                                {
                                    getSkillLevel = GetSystemSoulSkillGroupEpic(ref TB, getSoulData.Special_Buff_E_2, dbkey, false);
                                    EBuff = getSkillLevel.BuffID;
                                }
                            }
                            if (setItem.Class == 3)
                            {
                                if (getSoulData.Special_Buff_M_3 != 0)
                                {
                                    getSkillLevel = GetSystemSoulSkillGroup(ref TB, getSoulData.Special_Buff_M_3, dbkey, false);
                                    MBuff = getSkillLevel.BuffID;
                                }
                                if (getSoulData.Special_Buff_R_3 != 0)
                                {
                                    getSkillLevel = GetSystemSoulSkillGroup(ref TB, getSoulData.Special_Buff_R_3, dbkey, false);
                                    RBuff = getSkillLevel.BuffID;
                                }
                                if (getSoulData.Special_Buff_E_3 != 0)
                                {
                                    getSkillLevel = GetSystemSoulSkillGroupEpic(ref TB, getSoulData.Special_Buff_E_3, dbkey, false);
                                    EBuff = getSkillLevel.BuffID;
                                }
                            }
                            if (setItem.Class == specChar.Class)//장착 클래스 정보만 리턴
                            {
                                List<Return_DisassableItems_List> retDeletedItem = new List<Return_DisassableItems_List>();
                                Result_Define.eResult UseItem = ItemManager.UseItem(ref TB, AID, ItemID, getSoulData.PassItemAmout, ref retDeletedItem); //돌파단 소진
                                foreach (Return_DisassableItems_List getItems in retDeletedItem)
                                {
                                    if (getItems.itemid == ItemID)
                                    {
                                        RemainItem = getItems.itemea;
                                    }
                                }
                                setLevelPassSoul = ActiveSoulLevelPassToDB(ref TB, AID, CID, SoulSEQ, MBuff, RBuff, EBuff, getSoulData.Next_Hon, getActiveSoulInfo.soulid, setItem.Class, getActiveSoulInfo.soullevel, RemainItem, InvenSEQ, dbkey);
                            }
                            else
                                ActiveSoulLevelPassToDB(ref TB, AID, CID, SoulSEQ, MBuff, RBuff, EBuff, getSoulData.Next_Hon, getActiveSoulInfo.soulid, setItem.Class, getActiveSoulInfo.soullevel, RemainItem, InvenSEQ, dbkey);
                        }
                        return setLevelPassSoul;
                    }
                    else
                    {
                        setLevelPassSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.NOT_ENOUGH_MATERIAL_NEED_SOUL_LEVELPASS);
                        return setLevelPassSoul;
                    }
                }
                else
                {
                    setLevelPassSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.NO_LEVELPASS_PHASE);
                    return setLevelPassSoul;
                }
            }
            else
            {
                setLevelPassSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.CANT_TRY_LEVELPASS_REQUIRE_CHARACTERLV);
                return setLevelPassSoul;
            }
        }
        private const string SystemSoulEvolutionTableName = "GameData_SoulEvolution";
        private const string SystemSoulEvolutionTablePrefix = "System_SoulEvolution";
        public static GameData_SoulEvolution GetSystemSoulEvolution(ref TxnBlock TB, int GroupID, int Soul_EVO_Level, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", SystemSoulEvolutionTablePrefix, GroupID);
            string setQuery = string.Format(@"SELECT * FROM {0} WHERE Group_ID={1}", SystemSoulEvolutionTableName, GroupID);
            List<GameData_SoulEvolution> retObj = GenericFetch.FetchFromRedis_MultipleRow_Hash<GameData_SoulEvolution>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, GroupID.ToString(), setQuery, dbkey, Flush);
            if (retObj.Count > 0)
                return retObj.Find(item => item.Soul_EVO_Level == Soul_EVO_Level);
            else
                return new GameData_SoulEvolution();
        }
        private static ActiveSoulGradeUP ActiveSoulGradeUPToDB(ref TxnBlock TB, long AID, long CID, long SoulSEQ, int ItemID, int NeedParts, int SoulGradeLevel, string dbkey = SoulInvenDBName)
        {
            ActiveSoulGradeUP setGradeUPSoul = new ActiveSoulGradeUP();
            string setQuery = string.Format("UPDATE dbo.SoulActive SET SoulGradeLevel=SoulGradeLevel+1 WHERE SoulSEQ={0}", SoulSEQ);
            if (TB.ExcuteSqlCommand(dbkey, setQuery))
            {
                FlushActiveSoulList(ref TB, AID, CID, dbkey);
                ActiveSoulParts getUserActiveSoulParts = UseActiveSoulParts(ref TB, AID, ItemID, NeedParts, dbkey);//혼 조각 차감
                setGradeUPSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.SUCCESS);
                setGradeUPSoul.soulseq = SoulSEQ;
                setGradeUPSoul.soulgradelevel = System.Convert.ToByte(SoulGradeLevel + 1);
                setGradeUPSoul.gradeupitem_itemea = getUserActiveSoulParts.ItemEA;
                return setGradeUPSoul;
            }
            else
            {
                setGradeUPSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.DB_ERROR);
                return setGradeUPSoul;
            }
        }
        public static ActiveSoulGradeUP GradeUPActiveSoul(ref TxnBlock TB, long AID, long CID, long SoulSEQ, string dbkey = SoulInvenDBName)
        {
            ActiveSoulGradeUP setGradeUPSoul = new ActiveSoulGradeUP();
            ActiveSoul getActiveSoulInfo = GetActiveSoul(ref TB, AID, CID, SoulSEQ, dbkey, false);
            ActiveSoulParts getActivePartsInfo = GetActiveSoulPartsIndex(ref TB, AID, System.Convert.ToInt32(System.Convert.ToString(getActiveSoulInfo.soulid).Substring(0, 4)), dbkey, false);
            GameData_Soul getSoulData = GetSystemSoul(ref TB, getActiveSoulInfo.soulid, dbkey, false);
            GameData_SoulEvolution getSoulEvolution = TheSoul.DataManager._DEL_SoulManager.GetSystemSoulEvolution(ref TB, getSoulData.SoulEvoIndex, getActiveSoulInfo.soulgradelevel, dbkey, false);

            if (getActivePartsInfo.ItemEA >= getSoulEvolution.Need_Soul_Parts)//혼 진화 레벨에 필요한 혼 조각 체크
            {
                return ActiveSoulGradeUPToDB(ref TB, AID, CID, SoulSEQ, getActivePartsInfo.ItemID, getSoulEvolution.Need_Soul_Parts, getActiveSoulInfo.soulgradelevel, dbkey);
            }
            else
            {
                setGradeUPSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.NOT_ENOUGH_MATERIAL_NEED_SOUL_UPGRADE);
                return setGradeUPSoul;
            }
        }
        public static Result_Define.eResult ActiveSoulEquip(ref TxnBlock TB, long AID, long CID, int SlotNum, long EquipSoulSEQ, long ChangeSoulSEQ, string dbkey = Friend_Define.FriendList_DBName)
        {
            if (EquipSoulSEQ > 0 && ChangeSoulSEQ > 0)//친구 신청자 대기열 없음(상수값 없음 요청)
            {
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "UserSoulActiveEquip";
                Cmd.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                Cmd.Parameters.Add("@SlotNum", SqlDbType.Int).Value = SlotNum;
                Cmd.Parameters.Add("@EquipSoulSEQ", SqlDbType.BigInt).Value = EquipSoulSEQ;
                Cmd.Parameters.Add("@ChangeSoulSEQ", SqlDbType.BigInt).Value = ChangeSoulSEQ;
                if (TB.ExcuteSqlStoredProcedure(dbkey, ref Cmd))    // check return by command success
                {
                    FlushActiveSoulList(ref TB, AID, CID, dbkey);
                    return Result_Define.eResult.SUCCESS;
                }
                else
                {
                    return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                }
            }
            else
            {
                return Result_Define.eResult.NO_EQUIP_SOULINFO;
            }
        }
        private static ActiveSoulBuffChange ActiveSoulBuffChangeToDB(ref TxnBlock TB, long AID, long CID, long SoulSEQ, string BuffField, int Buff, string dbkey = SoulInvenDBName)
        {
            ActiveSoulBuffChange setBuffChangeSoul = new ActiveSoulBuffChange();
            string setQuery = string.Format("UPDATE dbo.SoulActive SET {0}={1} WHERE SoulSEQ={2}", BuffField, Buff, SoulSEQ);
            if (TB.ExcuteSqlCommand(dbkey, setQuery))
            {
                FlushActiveSoulList(ref TB, AID, CID, dbkey);
                setBuffChangeSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.SUCCESS);
                setBuffChangeSoul.buff = Buff;
                return setBuffChangeSoul;
            }
            else
            {
                setBuffChangeSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.DB_ERROR);
                return setBuffChangeSoul;
            }
        }
        public static ActiveSoulBuffChange BuffChangeActiveSoul(ref TxnBlock TB, long AID, long CID, long SoulSEQ, int BuffDIV, int Buff, string dbkey = SoulInvenDBName)
        {
            string BuffField = null;
            if (BuffDIV == 1)
                BuffField = "Special_Buff1";
            if (BuffDIV == 2)
                BuffField = "Special_Buff2";
            if (BuffDIV == 3)
                BuffField = "Special_Buff3";
            return ActiveSoulBuffChangeToDB(ref TB, AID, CID, SoulSEQ, BuffField, Buff, dbkey);
        }

        private const string SystemSoulPassiveTableName = "GameData_SoulPassive";
        private const string SystemSoulPassiveTablePrefix = "System_SoulPassive";
        public static GameData_SoulPassive GetSystemSoulPassive(ref TxnBlock TB, int SoulID, string dbkey = SoulInvenDBName, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", SystemSoulPassiveTablePrefix, SoulID);
            string setQuery = string.Format(@"SELECT * FROM {0} WHERE ID={1}", SystemSoulPassiveTableName, SoulID);

            GameData_SoulPassive retObj = GenericFetch.FetchFromRedis_Hash<GameData_SoulPassive>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, SoulID.ToString(), setQuery, dbkey, Flush);
            return (retObj != null) ? retObj : new GameData_SoulPassive();
        }
        private static PassiveSoulGrind PassiveSoulGrindToDB(ref TxnBlock TB, long AID, long CID, long SoulSEQ, int Material_EXP, string dbkey = SoulInvenDBName)
        {
            PassiveSoulGrind setGrindSoul = new PassiveSoulGrind();
            string setQuery = string.Format("UPDATE dbo.SoulPassive SET delflag='Y' WHERE SoulSEQ={0}", SoulSEQ);
            if (TB.ExcuteSqlCommand(dbkey, setQuery))
            {
                FlushPassiveSoulList(ref TB, AID, CID, dbkey);
                Character charInfo = CharacterManager.UpdateCharacterSoulInfo(ref TB, AID, CID, Material_EXP, dbkey);
                setGrindSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.SUCCESS);
                setGrindSoul.passivesoulexp = charInfo.passivesoulexp;
                return setGrindSoul;
            }
            else
            {
                setGrindSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.DB_ERROR);
                return setGrindSoul;
            }
        }
        public static PassiveSoulGrind GrindPassiveSoul(ref TxnBlock TB, long AID, long CID, long SoulSEQ, string dbkey = SoulInvenDBName)
        {
            PassiveSoul getPassiveSoulInfo = GetPassiveSoul(ref TB, AID, CID, SoulSEQ, dbkey, false);
            GameData_SoulPassive getSoulPassiveData = GetSystemSoulPassive(ref TB, getPassiveSoulInfo.soulid, dbkey, false);
            return PassiveSoulGrindToDB(ref TB, AID, CID, SoulSEQ, getSoulPassiveData.Material_EXP, dbkey);
        }
        private static PassiveSoulLevelUP PassiveSoulLevelUPToDB(ref TxnBlock TB, long AID, long CID, long SoulSEQ, int LevelUp_EXP, PassiveSoulLevelUP setLevelUPSoul, string dbkey = SoulInvenDBName)
        {
            string setQuery = string.Format("UPDATE dbo.SoulPassive SET SoulLevel=SoulLevel+1, SoulID={0}, Buff={1} WHERE SoulSEQ={2}", setLevelUPSoul.soulid, setLevelUPSoul.buff, SoulSEQ);
            if (TB.ExcuteSqlCommand(dbkey, setQuery))
            {
                FlushPassiveSoulList(ref TB, AID, CID, dbkey);
                Character charInfo = CharacterManager.UpdateCharacterSoulInfo(ref TB, AID, CID, LevelUp_EXP, dbkey); //패시브 경험치 차감
                setLevelUPSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.SUCCESS);
                setLevelUPSoul.passivesoulexp = charInfo.passivesoulexp;
                return setLevelUPSoul;
            }
            else
            {
                setLevelUPSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.DB_ERROR);
                return setLevelUPSoul;
            }
        }
        public static PassiveSoulLevelUP LevelUPPassiveSoul(ref TxnBlock TB, long AID, long CID, long SoulSEQ, string dbkey = SoulInvenDBName)
        {
            int Buff = 0;
            PassiveSoulLevelUP setLevelUPSoul = new PassiveSoulLevelUP();
            PassiveSoul getPassiveSoulInfo = GetPassiveSoul(ref TB, AID, CID, SoulSEQ, dbkey, false);
            GameData_SoulPassive getSoulPassiveData = GetSystemSoulPassive(ref TB, getPassiveSoulInfo.soulid, dbkey, false);
            GameData_SoulPassive getSoulPassiveLevelUPBuffData = GetSystemSoulPassive(ref TB, getPassiveSoulInfo.soulid + 1, dbkey, false);
            Character charInfo = CharacterManager.GetCharacter(ref TB, AID, CID, false, dbkey);

            if (getSoulPassiveData.LevelUp_EXP > 0) //최종 강화 확인(0일때 최종강화)
            {
                if (charInfo.passivesoulexp >= getSoulPassiveData.LevelUp_EXP)//필요 경험치 체크
                {
                    if (charInfo.Class == 1)
                        Buff = getSoulPassiveLevelUPBuffData.Buff_1;
                    else if (charInfo.Class == 2)
                        Buff = getSoulPassiveLevelUPBuffData.Buff_2;
                    else if (charInfo.Class == 3)
                        Buff = getSoulPassiveLevelUPBuffData.Buff_3;
                    setLevelUPSoul.soulseq = SoulSEQ;
                    setLevelUPSoul.soulid = getPassiveSoulInfo.soulid + 1;
                    setLevelUPSoul.soullevel = getPassiveSoulInfo.soullevel + 1;
                    setLevelUPSoul.buff = Buff;
                    return PassiveSoulLevelUPToDB(ref TB, AID, CID, SoulSEQ, (getSoulPassiveData.LevelUp_EXP * -1), setLevelUPSoul, dbkey);
                }
                else
                {
                    setLevelUPSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.NOT_ENOUGH_EXP_NEED_PASSIVE_SOUL_ENCHANT);
                    return setLevelUPSoul;
                }
            }
            else
            {
                setLevelUPSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.FULL_SOUL_ENCHANT);
                return setLevelUPSoul;
            }
        }
        private static PassiveSoulGrind PassiveSoulChangeToDB(ref TxnBlock TB, long AID, long CID, int SlotNum, long EquipSoulSEQ, long ChangeSoulSEQ, int Material_EXP, string dbkey = SoulInvenDBName)
        {
            PassiveSoulGrind setChangeSoul = new PassiveSoulGrind();
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = "UserSoulPassiveChange";
            Cmd.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            Cmd.Parameters.Add("@SlotNum", SqlDbType.Int).Value = SlotNum;
            Cmd.Parameters.Add("@EquipSoulSEQ", SqlDbType.BigInt).Value = EquipSoulSEQ;
            Cmd.Parameters.Add("@ChangeSoulSEQ", SqlDbType.BigInt).Value = ChangeSoulSEQ;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref Cmd))
            {
                FlushPassiveSoulList(ref TB, AID, CID, dbkey);
                Character charInfo = CharacterManager.UpdateCharacterSoulInfo(ref TB, AID, CID, Material_EXP, dbkey); //패시브 경험치 누적
                setChangeSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.SUCCESS);
                setChangeSoul.passivesoulexp = charInfo.passivesoulexp;
                return setChangeSoul;
            }
            else
            {
                setChangeSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.DB_ERROR);
                return setChangeSoul;
            }
        }
        public static PassiveSoulGrind ChangePassiveSoul(ref TxnBlock TB, long AID, long CID, int SlotNum, long EquipSoulSEQ, long ChangeSoulSEQ, string dbkey = SoulInvenDBName)
        {
            PassiveSoulGrind setChangeSoul = new PassiveSoulGrind();
            PassiveSoul getPassiveSoulInfo = GetPassiveSoul(ref TB, AID, CID, EquipSoulSEQ, dbkey, false);
            GameData_SoulPassive getEquipSoulPassiveData = GetSystemSoulPassive(ref TB, getPassiveSoulInfo.soulid, dbkey, false);
            if (ChangeSoulSEQ > 0) //장착 혼 정보 확인
            {
                return PassiveSoulChangeToDB(ref TB, AID, CID, SlotNum, EquipSoulSEQ, ChangeSoulSEQ, getEquipSoulPassiveData.Material_EXP, dbkey);
            }
            else
            {
                setChangeSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.NO_EQUIP_SOULINFO);
                return setChangeSoul;
            }
        }
        private static PassiveSoulGrind PassiveSoulSlotChangeToDB(ref TxnBlock TB, long AID, long CID, int SlotNum, long EquipSoulSEQ, long ChangeSoulSEQ, string dbkey = SoulInvenDBName)
        {
            PassiveSoulGrind setChangeSoul = new PassiveSoulGrind();
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = "UserSoulPassiveSlotChange";
            Cmd.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            Cmd.Parameters.Add("@SlotNum", SqlDbType.Int).Value = SlotNum;
            Cmd.Parameters.Add("@EquipSoulSEQ", SqlDbType.BigInt).Value = EquipSoulSEQ;
            Cmd.Parameters.Add("@ChangeSoulSEQ", SqlDbType.BigInt).Value = ChangeSoulSEQ;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref Cmd))
            {
                FlushPassiveSoulList(ref TB, AID, CID, dbkey);
                setChangeSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.SUCCESS);
                return setChangeSoul;
            }
            else
            {
                setChangeSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.DB_ERROR);
                return setChangeSoul;
            }
        }
        public static PassiveSoulGrind SlotChangePassiveSoul(ref TxnBlock TB, long AID, long CID, int SlotNum, long EquipSoulSEQ, long ChangeSoulSEQ, string dbkey = SoulInvenDBName)
        {
            PassiveSoulGrind setChangeSoul = new PassiveSoulGrind();
            if (ChangeSoulSEQ > 0) //장착 혼 정보 확인
            {
                return PassiveSoulSlotChangeToDB(ref TB, AID, CID, SlotNum, EquipSoulSEQ, ChangeSoulSEQ, dbkey);
            }
            else
            {
                setChangeSoul.resultcode = System.Convert.ToInt32(Result_Define.eResult.NO_EQUIP_SOULINFO);
                return setChangeSoul;
            }
        }
        private static PassiveSoulMoveStorage PassiveSoulMoveStorageToDB(ref TxnBlock TB, long AID, long CID, long SoulSEQ, string dbkey = SoulInvenDBName)
        {
            PassiveSoulMoveStorage setPassiveSoulMoveStorage = new PassiveSoulMoveStorage();
            List<PassiveSoulEquipSlot> retSet = new List<PassiveSoulEquipSlot>();
            Dictionary<long, PassiveSoul> getPassiveSoulEquipList = GetPassiveSoulEquipList(ref TB, AID, CID, dbkey, false);
            string setQuery = string.Format("UPDATE dbo.SoulPassive SET SlotNum=99 WHERE SoulSEQ={0}", SoulSEQ);
            if (TB.ExcuteSqlCommand(dbkey, setQuery))
            {
                FlushPassiveSoulList(ref TB, AID, CID, dbkey);
                foreach (KeyValuePair<long, PassiveSoul> setItem in getPassiveSoulEquipList)
                {
                    PassiveSoulEquipSlot setPassiveSoulEquipSlot = new PassiveSoulEquipSlot();
                    setPassiveSoulEquipSlot.soulseq = setItem.Value.soulseq;
                    setPassiveSoulEquipSlot.slotnum = setItem.Value.slotnum;
                    retSet.Add(setPassiveSoulEquipSlot);
                }
                setPassiveSoulMoveStorage.resultcode = System.Convert.ToInt32(Result_Define.eResult.SUCCESS);
                setPassiveSoulMoveStorage.passiveslotnumlist = retSet;
                return setPassiveSoulMoveStorage;
            }
            else
            {
                setPassiveSoulMoveStorage.resultcode = System.Convert.ToInt32(Result_Define.eResult.DB_ERROR);
                return setPassiveSoulMoveStorage;
            }
        }
        public static PassiveSoulMoveStorage MoveStoragePassiveSoul(ref TxnBlock TB, long AID, long CID, long SoulSEQ, string dbkey = SoulInvenDBName)
        {
            int PassiveBagCount = 0;
            PassiveSoulMoveStorage setPassiveSoulMoveStorage = new PassiveSoulMoveStorage();
            Dictionary<long, PassiveSoul> getPassiveSoulList = GetPassiveSoulList(ref TB, AID, CID, dbkey, false);
            User_VIP UserVIPInfo = AccountManager.GetUser_VIPInfo(ref TB, AID, false, dbkey);
            System_VIP getSystemVIP = AccountManager.GetSystem_VIP_Level(ref TB, UserVIPInfo.viplevel, false, dbkey);
            foreach (KeyValuePair<long, PassiveSoul> setItem in getPassiveSoulList)
            {
                if (setItem.Value.slotnum == 99)
                    ++PassiveBagCount;
            }
            if (PassiveBagCount < getSystemVIP.BAGSLOT_MAX_PASSIVESOUL) //혼 저장 최대값 체크(VIP레벨에 따라 변경)
            {
                return PassiveSoulMoveStorageToDB(ref TB, AID, CID, SoulSEQ, dbkey);
            }
            else
            {
                setPassiveSoulMoveStorage.resultcode = System.Convert.ToInt32(Result_Define.eResult.FULL_PASSIVE_SOUL_INVEN);
                return setPassiveSoulMoveStorage;
            }
        }
    }
*/
}