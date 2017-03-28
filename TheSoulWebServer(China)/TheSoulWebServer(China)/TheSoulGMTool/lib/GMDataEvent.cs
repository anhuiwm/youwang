using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Tools;
using TheSoul.DataManager.Global;
using TheSoulGMTool.DBClass;


namespace TheSoulGMTool
{
    public partial class GMDataManager
    {
        public static Result_Define.eResult CreateBossRaid(ref TxnBlock TB, string name, System_BOSS_RAID bossRaidInfo, string startDate, string endDate, string dbkey = GMData_Define.CommonDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;

            System_NPC bossInfo = NPC_Manager.GetNPCInfo(ref TB, bossRaidInfo.BossID);
            if (bossInfo.NPCID > 0)
            {
                string setQuery = string.Format(@"INSERT INTO {0} (
                                                                [DungeonID], [NpcID], [BossLevel], [HP], [RemainHP], [CreaterAID], [CreaterNick], [KillerAID], [KillerNick],
                                                                [CreationDate], [PublicDate], [ExpireDate], [Status], [PublicChnnel])
                                                            VALUES (
                                                                {1}, {2}, {3},{4}, {4}, 0, N'{5} ', 0, '', GETDATE(), N'{6}', N'{7}', '{8}', 1)"
                                                        , BossRaid_Define.BossRaid_System_CreationTable
                                                        , bossRaidInfo.DungeonID, bossInfo.NPCID, bossInfo.Level, bossInfo.HP
                                                        , name, startDate, endDate, BossRaid_Define.BossRaidStatus[BossRaid_Define.eRaidStatus.Active]);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;

            }
            return retError;
        }

        public static Result_Define.eResult SetStopBossRaid(ref TxnBlock TB, long bossID, string dbkey = GMData_Define.CommonDBName)
        {
            string setQuery = string.Format(@"Update {0} set ExpireDate = DateAdd(MINUTE, -1, getdate()), Status = '{2}' Where BossRaidID = {1}"
                                                       , BossRaid_Define.BossRaid_System_CreationTable
                                                       , bossID, BossRaid_Define.BossRaidStatus[BossRaid_Define.eRaidStatus.Clear]);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static long GetBossraidCount(ref TxnBlock TB, string dbkey = GMData_Define.CommonDBName)
        {
            string setQuery = string.Format("Select count(*) number From {0} WITH(NOLOCk) Where CreaterAID = 0", BossRaid_Define.BossRaid_System_CreationTable);
            GM_Number retObj = GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.number;
        }

        public static List<BossRaidCreation> GetBossraidList(ref TxnBlock TB, int page, string dbkey = GMData_Define.CommonDBName)
        {

            string setQuery = string.Format(@"SELECT TOP({1}) resultTable.* FROM (
                                                    Select TOP {2} ROW_NUMBER() over (order by BossRaidID Desc) as rownumber, *
                                                    From  {0} with(nolock) 
                                                    Where CreaterAID = 0) as resultTable
                                                WHERE rownumber > {3}"
                                               , BossRaid_Define.BossRaid_System_CreationTable
                                               , GMData_Define.pageSize, (GMData_Define.pageSize * page), (page - 1) * GMData_Define.pageSize);
            List<BossRaidCreation> retObj = GenericFetch.FetchFromDB_MultipleRow<BossRaidCreation>(ref TB, setQuery, dbkey);
            if (retObj == null)
                retObj = new List<BossRaidCreation>();
            return retObj;
        }

        public static long GetSystemAchieveCount(ref TxnBlock TB, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("Select count(*) number From {0} WITH(NOLOCk)", Trigger_Define.System_Achieve_TableName);
            GM_Number retObj = GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.number;
        }

        public static List<System_Achieve> GetSystemAchieveList(ref TxnBlock TB, int page, string dbkey = GMData_Define.ShardingDBName)
        {

            string setQuery = string.Format(@"SELECT TOP({1}) resultTable.* FROM (
                                                    Select TOP {2} ROW_NUMBER() over (order by AchieveID ) as rownumber, *
                                                    From  {0} with(nolock)) as resultTable
                                                WHERE rownumber > {3}"
                                               , Trigger_Define.System_Achieve_TableName
                                               , GMData_Define.pageSize, (GMData_Define.pageSize * page), (page - 1) * GMData_Define.pageSize);
            List<System_Achieve> retObj = GenericFetch.FetchFromDB_MultipleRow<System_Achieve>(ref TB, setQuery, dbkey);
            if (retObj == null)
                retObj = new List<System_Achieve>();
            return retObj;
        }

        public static long GetUserAchieveCount(ref TxnBlock TB, long AID, int event_loopType = 2, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format(@"SELECT Count(*) as number From  {0} se with(nolock) inner join {1} ue with(nolock) on se.AchieveID = ue.Event_ID
                                                    Where AID = {2} And se.Event_LoopType = {3}"
                                                , Trigger_Define.System_Achieve_TableName, Trigger_Define.User_Achieve_Data_TableName
                                                , AID, event_loopType);
            GM_Number retObj = GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            return (retObj == null) ? 0 : retObj.number;
        }

        public static List<GM_User_Event> GetUserAchieveList(ref TxnBlock TB, long AID, int page, int event_loopType = 2, string dbkey = GMData_Define.ShardingDBName)
        {
            int pageSize = 20;
            string setLanguage = GMDataManager.GetGmToolLanguage();
            
            string setQuery = string.Format(@"SELECT TOP({4}) resultTable.* FROM (
                                                    Select TOP {5} ROW_NUMBER() over (order by se.AchieveID Desc) as rownumber,  se.[Description] as Event_Dev_Name, se.TaskCN as NameCN
                                                    , ue.User_Event_ID, ue.StartTime, ue.EndTime, ue.CurrentValue1, ue.ClearFlag, ue.RewardFlag, se.ClearTriggerType1_Value3, ue.Event_ID
                                                    From  {0} se with(nolock) inner join {1} ue with(nolock) on se.AchieveID = ue.Event_ID
                                                    Where AID = {2} And se.Event_LoopType = {3}) as resultTable
                                                WHERE rownumber > {6}"
                                                , Trigger_Define.System_Achieve_TableName, Trigger_Define.User_Achieve_Data_TableName
                                                , AID, event_loopType
                                                , pageSize, (pageSize * page), (page - 1) * pageSize);
            List<GM_User_Event> retObj = GenericFetch.FetchFromDB_MultipleRow<GM_User_Event>(ref TB, setQuery, dbkey);
            if (retObj == null || retObj.Count == 0)
                new List<GM_User_Event>();
            else
            {
                foreach (GM_User_Event item in retObj)
                {
                    string eventName = GetItmeName(ref TB, item.NameCN);
                    item.Event_Dev_Name = string.IsNullOrEmpty(eventName) ? item.Event_Dev_Name : eventName;
                }
            }

            return retObj;
        }

        public static long GetUserEventCount(ref TxnBlock TB, long AID, string sdate, string edate, string dbkey = GMData_Define.ShardingDBName)
        {
            string condition1 = (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate)) ? string.Format("and se.Event_EndTime > '{0}' and se.Event_EndTime <= '{1}'", sdate, edate) : "and se.Event_EndTime >= GETDATE()";
            string setQuery = string.Format(@"SELECT Count(*) as number From  {0} se with(nolock) inner join {1} ue with(nolock) on se.Event_ID = ue.Event_ID
                                                        inner join {2} seg with(nolock) on se.Event_Type = seg.Event_Type
                                                    Where AID = {3} {4}"
                                                , Trigger_Define.System_Event_TableName, Trigger_Define.User_Event_Data_TableName, Trigger_Define.System_EventGroup_Admin_TableName
                                                , AID, condition1);
            GM_Number retObj = GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            return (retObj == null) ? 0 : retObj.number;
        }

        public static List<GM_User_Event> GetUserEventList(ref TxnBlock TB, long AID, int page, string sdate, string edate, string dbkey = GMData_Define.ShardingDBName)
        {
            string condition1 = (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate)) ? string.Format("and se.Event_EndTime > '{0}' and se.Event_EndTime <= '{1}'", sdate, edate) : "and se.Event_EndTime >= GETDATE()";
            string setQuery = string.Format(@"SELECT TOP({5}) resultTable.* FROM (
                                                    Select TOP {6} ROW_NUMBER() over (order by se.Event_Type, ue.Event_ID Desc) as rownumber, se.Event_Dev_Name, se.Event_StartTime, se.Event_EndTime, ue.User_Event_ID, ue.StartTime, ue.ClearFlag, ue.RewardFlag, seg.Event_Title as Event_Type
                                                    From  {0} se with(nolock) inner join {1} ue with(nolock) on se.Event_ID = ue.Event_ID
                                                        inner join {2} seg with(nolock) on se.Event_Type = seg.Event_Type
                                                    Where AID = {3} {4}) as resultTable
                                                WHERE rownumber > {7}"
                                                , Trigger_Define.System_Event_TableName, Trigger_Define.User_Event_Data_TableName, Trigger_Define.System_EventGroup_Admin_TableName
                                                , AID, condition1
                                                ,GMData_Define.pageSize, (GMData_Define.pageSize * page), (page - 1) * GMData_Define.pageSize);
            List<GM_User_Event> retObj = GenericFetch.FetchFromDB_MultipleRow<GM_User_Event>(ref TB, setQuery, dbkey);
            return (retObj == null || retObj.Count == 0) ? new List<GM_User_Event>() : retObj;
        }

        public static List<System_TriggerType> GetSystemTriggerTypeList(ref TxnBlock TB, string dbKey = GMData_Define.GmDBName)
        {
            string triggerLang = GMDataManager.GetGmToolLanguage();

            string setQuery = string.Format(@"Select TriggerID, Trigger_{1} as [Trigger], TriggerType, value1, value2, value3, etc From {0} WITH(NOLOCK)", GMData_Define.SystemTriggerTypeTable, triggerLang);

            List<System_TriggerType> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<System_TriggerType>(ref TB, setQuery, dbKey);
            if (retObj == null)
                retObj = new List<System_TriggerType>();
            return retObj;
        }

        public static System_TriggerType GetSystemTriggerType(ref TxnBlock TB, string TriggerType, string dbKey = GMData_Define.GmDBName)
        {
            string triggerLang = GMDataManager.GetGmToolLanguage();

            string setQuery = string.Format(@"Select TriggerID, Trigger_{2} as [Trigger], TriggerType From {0} WITH(NOLOCK) Where TriggerType = '{1}'", GMData_Define.SystemTriggerTypeTable, TriggerType, triggerLang);

            System_TriggerType retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<System_TriggerType>(ref TB, setQuery, dbKey);
            if (retObj == null)
                retObj = new System_TriggerType();
            return retObj;
        }

        public static List<GM_EventGroup_Admin> GetEventAdminList(ref TxnBlock TB, int GroupType, string dbKey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format(@"Select Event_Group_Type, Event_Index, Event_Title, Event_Intro, Event_Type, order_index, (Select Case When ISNULL(SUM(ActiveType),0) = 0 then 'X' else 'O' end from {1} WITH(NOLOCK) Where Event_Type=sea.Event_Type) as ActiveState
                                            From {0} as sea WITH(NOLOCK, INDEX(IDX_GM_SystemEvanteGroupList)) Where Event_Group_Type = {2}  Order By order_index", Trigger_Define.System_EventGroup_Admin_TableName, Trigger_Define.System_Event_TableName, GroupType);

            List<GM_EventGroup_Admin> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GM_EventGroup_Admin>(ref TB, setQuery, dbKey);
            if (retObj == null)
                new List<GM_EventGroup_Admin>();
            return retObj;
        }

        public static GM_EventGroup_Admin GetEventAdminData(ref TxnBlock TB, int GroupType, int index, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format(@"Select Event_Group_Type, Event_Index, Event_Title, Event_Intro, Event_Type, order_index, (Select Case When ISNULL(SUM(ActiveType),0) = 0 then 'X' else 'O' end from {1} WITH(NOLOCK) Where Event_Type=sea.Event_Type) as ActiveState
                                            From {0} as sea WITH(NOLOCK, INDEX(IDX_GM_SystemEventGroup)) Where Event_Index = {2} And Event_Group_Type = {3} ", Trigger_Define.System_EventGroup_Admin_TableName, Trigger_Define.System_Event_TableName, index, GroupType);

            GM_EventGroup_Admin retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_EventGroup_Admin>(ref TB, setQuery, dbkey);
            if (retObj == null)
                new GM_EventGroup_Admin();
            return retObj;
        }
        
        public static long GetMaxEventAdminIndex(ref TxnBlock TB, int groupType, string dbkey = GMData_Define.GmDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            string setQuery = string.Format("Select ISNULL(MAX(Event_Index),0) + 1 as number From {0} WITH(NOLOCK) Where Event_Group_Type = {1}", GMData_Define.AdminSystemEventGroupIndexTable, groupType);
            GM_Number maxIndexValue = GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey);
            if (maxIndexValue == null || maxIndexValue.number == 1)
            {
                //인덱스가 없을때 설정되어 있는 db의 시스템 정보를 가져와 설정
                string countQuery = string.Format("Select ISNULL(MAX(Event_Index),0) as number From {0} WITH(NOLOCK) Where Event_Group_Type = {1}", Trigger_Define.System_EventGroup_Admin_TableName, groupType);
                GM_Number eventCount = GenericFetch.FetchFromDB<GM_Number>(ref TB, countQuery, GMData_Define.ShardingDBName);
                if (eventCount.number == 0)
                    maxIndexValue.number = 1;
                else
                    maxIndexValue.number = eventCount.number+1;
            }
            setQuery = string.Format("Insert into {0} (Event_Group_Type, Event_Index) Values ({1}, {2})", GMData_Define.AdminSystemEventGroupIndexTable, groupType, maxIndexValue.number);
            retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            if (retError != Result_Define.eResult.SUCCESS)
            {
                maxIndexValue.number = 0;
            }

            return maxIndexValue.number;
        }
        public static Result_Define.eResult SetEventAdmin(ref Dictionary<long, TxnBlock> server, System_EventGroup_Admin setData, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                string setQuery = string.Format(@"MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON Event_Group_Type = {1} And Event_Index = {2}
                                                WHEN MATCHED THEN
                                                   Update Set 
                                                        order_index = {3}, Event_Title = N'{4}', Event_Intro = N'{5}'
                                                WHEN NOT MATCHED THEN
                                                    INSERT (Event_Group_Type, Event_Index, Event_Title, Event_Intro, Event_Type, Order_Index)
                                                    Values ({1}, {2}, N'{4}', N'{5}', N'{6}', {3});"
                                                , Trigger_Define.System_EventGroup_Admin_TableName, setData.Event_Group_Type, setData.Event_Index, setData.Order_Index
                                                , setData.Event_Title, setData.Event_Intro, setData.Event_Type);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }
            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            return retError;
        }

        public static Result_Define.eResult InsertEventAdmin(ref Dictionary<long, TxnBlock> server, System_EventGroup_Admin setData, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            TxnBlock TB = server.First().Value;
            setData.Event_Index = System.Convert.ToInt32(GetMaxEventAdminIndex(ref TB, setData.Event_Group_Type));
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TB = tb.Value;
                string setQuery = string.Format(@"Insert Into {0} (Event_Group_Type, Event_Index, Event_Title, Event_Intro, Event_Type, Order_Index) 
                                                                Values ({1}, {2}, N'{4}', N'{5}', N'{6}', {3})"
                                                , Trigger_Define.System_EventGroup_Admin_TableName, setData.Event_Group_Type, setData.Event_Index, setData.Order_Index
                                                , setData.Event_Title, setData.Event_Intro, setData.Event_Type);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }
            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            return retError;
        }

        public static Result_Define.eResult SetEventOnOfF(ref Dictionary<long, TxnBlock> server, string active, string eventtype, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;

            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                string setQuery = "";
                if (active == "O")
                {
                    setQuery = string.Format(@"Update {0} set Event_StartTime = getdate(), Event_EndTime = Dateadd(d, 7, getdate()), ActiveType = 1
                                                    Where Event_Type in (Select Event_Type from {1} Where Event_Type = '{2}')"
                                                , Trigger_Define.System_Event_TableName, Trigger_Define.System_EventGroup_Admin_TableName, eventtype);
                }
                else
                {
                    setQuery = string.Format(@"Update {0} set Event_StartTime = Dateadd(d, -2, getdate()), Event_EndTime = Dateadd(d, -1, getdate()), ActiveType = 0
                                                    Where Event_Type in (Select Event_Type from {1} Where Event_Type = '{2}')"
                                                , Trigger_Define.System_Event_TableName, Trigger_Define.System_EventGroup_Admin_TableName, eventtype);
                }
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                retError = SetUserEventOnOff(ref server, active, eventtype);
            }
            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            return retError;
        }

        public static Result_Define.eResult SetEventDate(ref Dictionary<long, TxnBlock> server, long eventID, string title, string tooltip, string startDate, string endDate, System_Event setData, List<System_Event_Reward_Box> itemlist1, List<System_Event_Reward_Box> itemlist2, List<System_Event_Reward_Box> itemlist3, List<System_Event_Reward_Box> itemlist4, string dbkey = GMData_Define.ShardingDBName)
        {//선물이벤트
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            long rewardID1 = setData.Reward_Box1ID;
            long rewardID2 = setData.Reward_Box2ID;
            long rewardID3 = setData.Reward_Box3ID;
            long rewardID4 = setData.Reward_Box4ID;

            TxnBlock TB = server.First().Value;
            if (itemlist1.Count > 0 && setData.Reward_Box1ID == 0)
            {
                rewardID1 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.EVENT);
                retError = UpdateEventRewardBox(ref server, rewardID1, itemlist1);
            }
            else if (itemlist1.Count > 0 && setData.Reward_Box1ID > 0)
            {
                retError = UpdateEventRewardBox(ref server, rewardID1, itemlist1);
            }
            else
            {
                retError = DeleteEventRewardBox(ref server, rewardID1);
                rewardID1 = 0;
            }
            if (itemlist2.Count > 0 && setData.Reward_Box2ID == 0)
            {
                rewardID2 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.EVENT);
                retError = UpdateEventRewardBox(ref server, rewardID2, itemlist2);
            }
            else if (itemlist2.Count > 0 && setData.Reward_Box2ID > 0)
            {
                retError = UpdateEventRewardBox(ref server, rewardID2, itemlist2);
            }
            else
            {
                retError = DeleteEventRewardBox(ref server, rewardID2);
                rewardID2 = 0;
            }
            if (itemlist3.Count > 0 && setData.Reward_Box3ID == 0)
            {
                rewardID3 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.EVENT);
                retError = UpdateEventRewardBox(ref server, rewardID3, itemlist3);
            }
            else if (itemlist3.Count > 0 && setData.Reward_Box3ID > 0)
            {
                retError = UpdateEventRewardBox(ref server, rewardID3, itemlist3);
            }
            else
            {
                retError = DeleteEventRewardBox(ref server, rewardID3);
                rewardID3 = 0;
            }
            if (itemlist4.Count > 0 && setData.Reward_Box4ID == 0)
            {
                rewardID4 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.EVENT);
                retError = UpdateEventRewardBox(ref server, rewardID4, itemlist4);
            }
            else if (itemlist4.Count > 0 && setData.Reward_Box4ID > 0)
            {
                retError = UpdateEventRewardBox(ref server, rewardID4, itemlist4);
            }
            else
            {
                retError = DeleteEventRewardBox(ref server, rewardID4);
                rewardID4 = 0;
            }

            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TB = tb.Value;
                string setQuery = string.Format(@"Update {0} set Event_Dev_Name=N'{8}', Event_Tooltip=N'{9}', Event_StartTime = '{2}', Event_EndTime = '{3}', Reward_Box1ID='{4}', Reward_Box2ID='{5}', Reward_Box3ID='{6}', Reward_Box4ID='{7}' Where Event_ID = {1}"
                                                    , Trigger_Define.System_Event_TableName, eventID, startDate, endDate, rewardID1, rewardID2, rewardID3, rewardID4, title, tooltip);

                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
                //if (retError == Result_Define.eResult.SUCCESS)
                //{
                //    setData.Event_StartTime = DateTime.Parse(startDate);
                //    setData.Event_EndTime = DateTime.Parse(endDate);
                //    string gmid = HttpContext.Current.Request.Cookies.Count > 0 ? HttpContext.Current.Request.Cookies["mseedadmin"]["userid"] : "admin";
                //    long logCount = SnailLogManager.GetSnailLog_program_log(ref TB);
                //    if (logCount > 0)
                //        SnailLogManager.SnailLog_write_program_log(ref TB, gmid, ref setData);
                //    else
                //    {
                //        List<System_Event> eventList = TriggerManager.GetSystem_Event_All_List(ref TB, dbkey);
                //        foreach (System_Event item in eventList)
                //        {
                //            System_Event itemData = item;
                //            SnailLogManager.SnailLog_write_program_log(ref TB, "admin", ref itemData);
                //        }
                //        SnailLogManager.SnailLog_write_program_log(ref TB, gmid, ref setData);
                //    }
                //}
                //else
                //    break;
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                retError = SetUserEventOnOff(ref server, setData.ActiveType.ToString(), setData.Event_Type, eventID, startDate, endDate);
            }
            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            return retError;
        }

        private static Result_Define.eResult SetUserEventOnOff(ref Dictionary<long, TxnBlock> server, string active, string eventtype = "", long eventID = 0, string startDate = "", string endDate = "", string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            if (!string.IsNullOrEmpty(startDate))
                startDate = "'"+startDate+"'";
            if (!string.IsNullOrEmpty(endDate))
                endDate = "'" + endDate + "'";
            if (eventID > 0)
            {
                foreach (KeyValuePair<long, TxnBlock> tb in server)
                {
                    TxnBlock TB = tb.Value;
                    string setQuery = "";
                    if (active == "O")
                    {
                        if (string.IsNullOrEmpty(startDate))
                            startDate = "getdate()";
                        if (string.IsNullOrEmpty(endDate))
                            endDate = "Dateadd(d, 7, getdate())";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(startDate))
                            startDate = "Dateadd(d, -2, getdate())";
                        if (string.IsNullOrEmpty(endDate))
                            endDate = "Dateadd(d, -1, getdate())";
                    }
                    setQuery = string.Format(@"Update {0} set StartTime = {3}, EndTime = {4}
                                                    Where Event_ID in (SELECT Event_ID FROM {1} WITH(NOLOCK) Where Event_Type = '{2}')"
                                                    , Trigger_Define.User_Event_Data_TableName, Trigger_Define.System_Event_TableName, eventtype, startDate, endDate);
                    retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;

                    if (retError != Result_Define.eResult.SUCCESS)
                        break;
                }
            }
            else
            {
                foreach (KeyValuePair<long, TxnBlock> tb in server)
                {
                    TxnBlock TB = tb.Value;
                    string setQuery = "";
                    if (active == "O")
                    {
                        if (string.IsNullOrEmpty(startDate))
                            startDate = "getdate()";
                        if (string.IsNullOrEmpty(endDate))
                            endDate = "Dateadd(d, 7, getdate())";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(startDate))
                            startDate = "Dateadd(d, -2, getdate())";
                        if (string.IsNullOrEmpty(endDate))
                            endDate = "Dateadd(d, -1, getdate())";                        
                    }
                    setQuery = string.Format(@"Update {0} set StartTime = {2}, EndTime = {3} Where Event_ID = {1}"
                                                    , Trigger_Define.User_Event_Data_TableName, eventID, startDate, endDate);
                    retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                    if (retError != Result_Define.eResult.SUCCESS)
                        break;
                }
            }
            return retError;
        }

        public static List<System_Event> GetSystem_Event_List(ref TxnBlock TB, int group, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK, INDEX(IDX_GM_SystemEventList)) Where Event_Type in (Select Event_Type from {1} WITH(NOLOCK, INDEX(IDX_GM_SystemEvanteGroupList)) Where Event_Group_Type = {2}) order by Event_ID desc", Trigger_Define.System_Event_TableName, Trigger_Define.System_EventGroup_Admin_TableName, group);
            List<System_Event> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<System_Event>(ref TB, setQuery, dbkey);
            return retObj;
        }

        public static System_Event GetSystem_EventData(ref TxnBlock TB, long idx, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) Where Event_ID = {1}", Trigger_Define.System_Event_TableName, idx);
            System_Event retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<System_Event>(ref TB, setQuery, dbkey);
            if (retObj == null)
                retObj = new System_Event();
            return retObj;
        }

        public static List<Admin_LineNotice> GetLineNoticeList(ref TxnBlock TB, string dbkey = GMData_Define.CommonDBName)
        {
            string setQuery = string.Format(@"SELECT *, case when sdate > GETDATE() then N'" + Resources.languageResource.lang_noticeWait + "' when sdate<= GETDATE() and edate >= GETDATE() then N'"+Resources.languageResource.lang_noticeIng+"' else N'"+Resources.languageResource.lang_noticeEnd+"' end as flrag FROM {0} WITH(NOLOCK) Where notice_type = 2 order by regdate desc", GMData_Define.NoticeTable);
            List<Admin_LineNotice> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<Admin_LineNotice>(ref TB, setQuery, dbkey);
            if (retObj.Count == 0)
                retObj = new List<Admin_LineNotice>();
            return retObj;
        }

        public static List<TheSoulGMTool.DBClass.Admin_Notice> GetNoticeList(ref TxnBlock TB, string dbkey = GMData_Define.CommonDBName)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) Where notice_type = 1 order by regdate desc", GMData_Define.NoticeTable);
            List<TheSoulGMTool.DBClass.Admin_Notice> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<TheSoulGMTool.DBClass.Admin_Notice>(ref TB, setQuery, dbkey);
            if (retObj.Count == 0)
                retObj = new List<TheSoulGMTool.DBClass.Admin_Notice>();
            return retObj;
        }

        public static TheSoulGMTool.DBClass.Admin_Notice GetNotice(ref TxnBlock TB, int idx, string dbkey = GMData_Define.CommonDBName)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) Where idx = {1}", GMData_Define.NoticeTable, idx);
            TheSoulGMTool.DBClass.Admin_Notice retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<TheSoulGMTool.DBClass.Admin_Notice>(ref TB, setQuery, dbkey);
            if (retObj == null)
                retObj = new TheSoulGMTool.DBClass.Admin_Notice();
            return retObj;
        }

        public static Result_Define.eResult DeleteNotice(ref Dictionary<long, TxnBlock> server, long idx, int noticeType, string dbkey = GMData_Define.CommonDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            string setQuery = string.Format("Delete From {0} Where idx = {1}", GMData_Define.NoticeTable, idx);
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }
            if (retError == Result_Define.eResult.SUCCESS)
            {
                TxnBlock TB = server.First().Value;
                if(noticeType == 1)
                    retError = InsertGMControlLog(ref TB, GMResult_Define.TargetType.GAME_SYSTEM, idx, "", GMResult_Define.ControlType.NITICE_DELETE, setQuery, string.Join(",", server.Keys.Select(x=>x.ToString())));
                else
                    retError = InsertGMControlLog(ref TB, GMResult_Define.TargetType.GAME_SYSTEM, idx, "", GMResult_Define.ControlType.LINE_NOTICE_DELETE, setQuery, string.Join(",", server.Keys.Select(x => x.ToString())));
            }

            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            return retError;
        }

        public static Result_Define.eResult InsertLineNotice(ref Dictionary<long, TxnBlock> server, string sdate, string edate, short type, string contents, string regid, int time = 0, string dbkey = GMData_Define.CommonDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            long idx = 0;
            TxnBlock TB = server.First().Value;
            retError = CreateNoticeIndex(ref TB, ref idx, 2);
            contents = contents.Replace("'", "''");
            string setQuery = string.Format("Insert into {0} (idx, notice_type, sdate, edate, type, repeatTime, title, regid) Values ({1}, 2, '{2}','{3}','{4}','{5}',N'{6}','{7}')", GMData_Define.NoticeTable, idx, sdate, edate, type, time, contents, regid);
            if (retError == Result_Define.eResult.SUCCESS)
            {
                foreach (KeyValuePair<long, TxnBlock> tb in server)
                {
                    TB = tb.Value;
                     retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                    if (retError != Result_Define.eResult.SUCCESS)
                        break;
                }
            }
            if (retError == Result_Define.eResult.SUCCESS)
            {
                TB = server.First().Value;
                retError = InsertGMControlLog(ref TB, GMResult_Define.TargetType.GAME_SYSTEM, idx, "", GMResult_Define.ControlType.LINE_NOITCE_ADD, setQuery, string.Join(",", server.Keys.Select(x => x.ToString())));
            }

            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);

            return retError;
        }

        public static Result_Define.eResult UpdateLineNotice(ref Dictionary<long, TxnBlock> server, long idx, string sdate, string edate, short type, string contents, string regid, int time = 0, string dbkey = GMData_Define.CommonDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            contents = contents.Replace("'", "''");
            long newIndex = 0;
            TxnBlock TB = server.First().Value;
            retError = CreateNoticeIndex(ref TB, ref newIndex, 2);

            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TB = tb.Value;
                string regdate = GetNotice(ref TB, (int)idx, dbkey).regdate.ToString("yyyy-MM-dd HH:mm:ss");
                string setQuery = string.Format(@"Insert into {0} (idx, notice_type, sdate, edate, type, repeatTime, title, regid, regdate, editeID, editeDate) 
                                                        Values ({1}, 2, '{2}','{3}','{4}','{5}',N'{6}','{7}', '{8}', '{7}',getdate())"
                                            , GMData_Define.NoticeTable, newIndex, sdate, edate, type, time, contents, regid, regdate);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }
            if (retError == Result_Define.eResult.SUCCESS)
                retError = DeleteNotice(ref server, idx, 2);
            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            return retError;
        }



        public static Result_Define.eResult UpdateDailyEvent(ref Dictionary<long, TxnBlock> server, long idx, int vip1, int vip2, int vip3, int vip4, int vip5, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            string setQuery = string.Format(@"Update {0} Set Reward1_VIP_Level = '{2}', Reward2_VIP_Level = '{3}', Reward3_VIP_Level = '{4}', Reward4_VIP_Level = '{5}', Reward5_VIP_Level = '{6}'
                                              Where Event_Daily_ID={1}", Trigger_Define.System_Event_Daily_TableName, idx, vip1, vip2, vip3, vip4, vip5);
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }
            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            return retError;
        }

        public static Result_Define.eResult UpdateFirstPaymentEvent(ref Dictionary<long, TxnBlock> server, int active, string title, string msg, string dbkey = GMData_Define.ShardingDBName)

        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            title = title.Replace("'", "''");
            msg = msg.Replace("'", "''");
            string setQuery = string.Format(@"Update {0} Set Reward_Mail_Subject_CN = N'{1}', Reward_Mail_Text_CN = N'{2}'", Trigger_Define.System_Event_First_Payment_TableName, title, msg);
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }
            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            return retError;
        }

        public static Result_Define.eResult UpdateEventRewardBox(ref Dictionary<long, TxnBlock> server, long idx, List<System_Event_Reward_Box> itemList, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            retError = DeleteEventRewardBox(ref server, idx);

            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                foreach (System_Event_Reward_Box item in itemList)
                {
                    item.EventItem_TargetType = string.IsNullOrEmpty(item.EventItem_TargetType) ? "Item" : item.EventItem_TargetType;
                    string setQuery = string.Format(@"Insert Into {0} (EventBoxID, VIP_Level, BoxItemIndex, EventItem_TargetType, EventItem_ID, EventItem_Level, EventItem_Grade, EventItem_Rnd1Type, EventItem_Rnd1Value, EventItem_Rnd2Type, EventItem_Rnd2Value, EventItem_Rnd3Type, EventItem_Rnd3Value, EventItem_Num)
                                                                Values ({1}, {2},{3},'{4}',{5},{6},{7}, 0, 0, 0, 0, 0, 0, {8})", Trigger_Define.System_Event_Reward_Box_TableName
                                                                , idx, item.VIP_Level, item.BoxItemIndex, item.EventItem_TargetType, item.EventItem_ID, item.EventItem_Level, item.EventItem_Grade, item.EventItem_Num);
                    retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                    if (retError != Result_Define.eResult.SUCCESS)
                        break;
                }
            }
            return retError;
        }

        private static Result_Define.eResult DeleteEventRewardBox(ref Dictionary<long, TxnBlock> server, long idx, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            string setQuery = string.Format("Delete From {0} Where EventBoxID={1}", Trigger_Define.System_Event_Reward_Box_TableName, idx);
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }
            return retError;
        }

        public static List<GM_System_Package_List> GetGM_Package_List(ref TxnBlock TB, bool Flush = false, string dbkey = GMData_Define.ShardingDBName)
        {
            //string setKey = string.Format("{0}_{1}", Trigger_Define.Trigger_Prefix, Shop_Define.Shop_System_Package_List_TableName);
            if (Flush)
                ShopManager.RemoveShop_System_Package_List();

            string setQuery = string.Format(@"SELECT A.*, ISNULL(B.SaleStartTime, GETDATE()-1) AS SaleStartTime, ISNULL(B.SaleEndTime, getdate()+1) AS SaleEndTime 
                                                        FROM {0} AS A WITH(NOLOCK) LEFT OUTER JOIN {1} AS B WITH(NOLOCK) ON A.Package_ID = B.Shop_Goods_ID Order by A.Package_ID Desc", Shop_Define.Shop_System_Package_List_TableName, Shop_Define.Shop_Limit_TableName);
            return GenericFetch.FetchFromDB_MultipleRow<GM_System_Package_List>(ref TB, setQuery, dbkey);
        }

        public static List<GM_Number> GetShopGoodsCodeList(ref TxnBlock TB, long shopID, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("Select Billing_Platform_Type as number From {0} With(nolock) Where Shop_Goods_ID = {1}", Shop_Define.Shop_GoodsCode_TableName, shopID);
            List<GM_Number> retObj = GenericFetch.FetchFromDB_MultipleRow<GM_Number>(ref TB, setQuery, dbkey);
            return (retObj == null) ? new List<GM_Number>() : retObj;
        }

        public static GM_System_Package_List GetGM_Package_Data(ref TxnBlock TB, long idx, string dbkey = GMData_Define.ShardingDBName, bool package = true)
        {
           
            string setQuery = string.Format(@"SELECT A.*, ISNULL(B.SaleStartTime, GETDATE()-1) AS SaleStartTime, ISNULL(B.SaleEndTime, getdate()+1) AS SaleEndTime 
                                                        FROM {0} AS A WITH(NOLOCK) LEFT OUTER JOIN {1} AS B WITH(NOLOCK) ON A.Package_ID = B.Shop_Goods_ID Where Package_ID={2} "
                                            , package ? Shop_Define.Shop_System_Package_List_TableName : Shop_Define.Shop_System_Package_Cheap_TableName, Shop_Define.Shop_Limit_TableName, idx);
            GM_System_Package_List retObj = GenericFetch.FetchFromDB<GM_System_Package_List>(ref TB, setQuery, dbkey);
            if(retObj == null)
                retObj = new GM_System_Package_List();
            return retObj;
        }

        public static Result_Define.eResult InsertEvent(ref Dictionary<long, TxnBlock> server, string startDate, string endDate, System_Event setData, List<System_Event_Reward_Box> itemlist1, List<System_Event_Reward_Box> itemlist2, List<System_Event_Reward_Box> itemlist3, List<System_Event_Reward_Box> itemlist4, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            TxnBlock TB = server.First().Value;
            long rewardID1 = 0;
            long rewardID2 = 0;
            long rewardID3 = 0;
            long rewardID4 = 0;

            if (setData.Event_ID > 0)
            {
                if (itemlist1.Count > 0)
                {
                    rewardID1 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.EVENT);
                    retError = UpdateEventRewardBox(ref server, rewardID1, itemlist1);
                }
                if (itemlist2.Count > 0)
                {
                    rewardID2 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.EVENT);
                    retError = UpdateEventRewardBox(ref server, rewardID2, itemlist2);
                }
                if (itemlist3.Count > 0)
                {
                    rewardID3 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.EVENT);
                    retError = UpdateEventRewardBox(ref server, rewardID3, itemlist3);
                }
                if (itemlist4.Count > 0)
                {
                    rewardID4 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.EVENT);
                    retError = UpdateEventRewardBox(ref server, rewardID4, itemlist4);
                }
            }
            if (retError == Result_Define.eResult.SUCCESS)
            {
                foreach (KeyValuePair<long, TxnBlock> tb in server)
                {
                    TB = tb.Value;

                    string setQuery = string.Format(@"Insert Into {0} (Event_ID, Event_Type, Event_Dev_Name, Event_Tooltip, ActiveType, Event_StartTime, Event_EndTime, Event_Loop, Event_LoopType
                                                                , ActiveTriggerType1, ActiveTriggerType1_Value1, ActiveTriggerType1_Value2, ActiveTriggerType1_Value3
                                                                , ActiveTriggerType2, ActiveTriggerType2_Value1, ActiveTriggerType2_Value2, ActiveTriggerType2_Value3
                                                                , ClearTriggerType1, ClearTriggerType1_Value1, ClearTriggerType1_Value2, ClearTriggerType1_Value3
                                                                , ClearTriggerType2, ClearTriggerType2_Value1, ClearTriggerType2_Value2, ClearTriggerType2_Value3
                                                                , Reward_Box1ID, Reward_Box2ID, Reward_Box3ID, Reward_Box4ID
                                                                , Event_Price_Ruby, Reward_Mail_Subject_CN, Reward_Mail_Text_CN, OrderID)
                                                            Values ('{1}', '{2}', N'{3}', N'{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}'
                                                                    , '{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}', '{27}', '{28}', '{29}', '{30}', N'{31}', N'{32}', {33})", Trigger_Define.System_Event_TableName
                                                                , setData.Event_ID, setData.Event_Type, setData.Event_Dev_Name, setData.Event_Tooltip, setData.ActiveType, startDate, endDate, setData.Event_Loop, setData.Event_LoopType
                                                                , setData.ActiveTriggerType1, setData.ActiveTriggerType1_Value1, setData.ActiveTriggerType1_Value2, setData.ActiveTriggerType1_Value3
                                                                , setData.ActiveTriggerType2, setData.ActiveTriggerType2_Value1, setData.ActiveTriggerType2_Value2, setData.ActiveTriggerType2_Value3
                                                                , setData.ClearTriggerType1, setData.ClearTriggerType1_Value1, setData.ClearTriggerType1_Value2, setData.ClearTriggerType1_Value3
                                                                , setData.ClearTriggerType2, setData.ClearTriggerType2_Value1, setData.ClearTriggerType2_Value2, setData.ClearTriggerType2_Value3
                                                                , rewardID1, rewardID2, rewardID3, rewardID4, setData.Event_Price_Ruby, setData.Reward_Mail_Subject_CN, setData.Reward_Mail_Text_CN, setData.OrderID);
                    retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                    if (retError != Result_Define.eResult.SUCCESS)
                        break;
                    //if (retError == Result_Define.eResult.SUCCESS)
                    //{
                    //    setData.Event_StartTime = DateTime.Parse(startDate);
                    //    setData.Event_EndTime = DateTime.Parse(endDate);
                    //    string gmid = HttpContext.Current.Request.Cookies.Count > 0 ? HttpContext.Current.Request.Cookies["mseedadmin"]["userid"] : "admin";
                    //    long logCount = SnailLogManager.GetSnailLog_program_log(ref TB);
                    //    if (logCount > 0)
                    //        SnailLogManager.SnailLog_write_program_log(ref TB, gmid, ref setData);
                    //    else
                    //    {
                    //        List<System_Event> eventList = TriggerManager.GetSystem_Event_All_List(ref TB, dbkey);
                    //        foreach (System_Event item in eventList)
                    //        {
                    //            System_Event itemData = item;
                    //            SnailLogManager.SnailLog_write_program_log(ref TB, "admin", ref itemData);
                    //        }
                    //        SnailLogManager.SnailLog_write_program_log(ref TB, gmid, ref setData);
                    //    }
                    //}
                    //else
                    //    break;
                }
            }

            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            return retError;
        }

        public static Result_Define.eResult DeleteEvent(ref Dictionary<long, TxnBlock> server, long eventID, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;

            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;

                string setQuery = string.Format(@"Delete From {0} Where Event_ID = {1}", Trigger_Define.System_Event_TableName, eventID);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError == Result_Define.eResult.SUCCESS)
                {
                    TriggerManager.RemoveEventDataFromRedis(0, true);
                }
                else
                    break;
            }
            return retError;
        }

        public static string GetEventInofJson(ref Dictionary<long, TxnBlock> server, long idx, string dbkey = GMData_Define.ShardingDBName)
        {
            string json = "";
            TxnBlock TB = server.First().Value;
            List<server_group_config> serverGourpList = GlobalManager.GetServerGroupList(ref TB);
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TB = tb.Value;
                System_Event dataInfo = GMDataManager.GetSystem_EventData(ref TB, idx);
                string serverName = serverGourpList.Find(item=>item.server_group_id == tb.Key)==null?"none": serverGourpList.Find(item=>item.server_group_id == tb.Key).server_group_name;

                json = mJsonSerializer.AddJson(json,string.Format("{0}_{1}",serverName, tb.Key), mJsonSerializer.ToJsonString(dataInfo));
            }
            return json;
        }

        public static Result_Define.eResult UpdateEvent(ref Dictionary<long, TxnBlock> server, long idx, string startDate, string endDate, System_Event setData, List<System_Event_Reward_Box> itemlist1, List<System_Event_Reward_Box> itemlist2, List<System_Event_Reward_Box> itemlist3, List<System_Event_Reward_Box> itemlist4, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            long rewardID1 = setData.Reward_Box1ID;
            long rewardID2 = setData.Reward_Box2ID;
            long rewardID3 = setData.Reward_Box3ID;
            long rewardID4 = setData.Reward_Box4ID;
            TxnBlock TB = server.First().Value;
            if (itemlist1.Count > 0 && setData.Reward_Box1ID == 0)
            {
                rewardID1 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.EVENT);
                retError = UpdateEventRewardBox(ref server, rewardID1, itemlist1);
            }
            else if (itemlist1.Count > 0 && setData.Reward_Box1ID > 0)
            {
                retError = UpdateEventRewardBox(ref server, rewardID1, itemlist1);
            }
            else
            {
                retError = DeleteEventRewardBox(ref server, rewardID1);
                rewardID1 = 0;
            }
            if (itemlist2.Count > 0 && setData.Reward_Box2ID == 0)
            {
                rewardID2 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.EVENT);
                retError = UpdateEventRewardBox(ref server, rewardID2, itemlist2);
            }
            else if (itemlist2.Count > 0 && setData.Reward_Box2ID > 0)
            {
                retError = UpdateEventRewardBox(ref server, rewardID2, itemlist2);
            }
            else
            {
                retError = DeleteEventRewardBox(ref server, rewardID2);
                rewardID2 = 0;
            }
            if (itemlist3.Count > 0 && setData.Reward_Box3ID == 0)
            {
                rewardID3 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.EVENT);
                retError = UpdateEventRewardBox(ref server, rewardID3, itemlist3);
            }
            else if (itemlist3.Count > 0 && setData.Reward_Box3ID > 0)
            {
                retError = UpdateEventRewardBox(ref server, rewardID3, itemlist3);
            }
            else
            {
                retError = DeleteEventRewardBox(ref server, rewardID3);
                rewardID3 = 0;
            }
            if (itemlist4.Count > 0 && setData.Reward_Box4ID == 0)
            {
                rewardID4 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.EVENT);
                retError = UpdateEventRewardBox(ref server, rewardID4, itemlist4);
            }
            else if (itemlist4.Count > 0 && setData.Reward_Box4ID > 0)
            {
                retError = UpdateEventRewardBox(ref server, rewardID4, itemlist4);
            }
            else
            {
                retError = DeleteEventRewardBox(ref server, rewardID4);
                rewardID4 = 0;
            }
            if (idx != setData.Event_ID && retError == Result_Define.eResult.SUCCESS)
            {
                retError = DeleteEvent(ref server, setData.Event_ID);
            }
            if (retError == Result_Define.eResult.SUCCESS)
            {
                foreach (KeyValuePair<long, TxnBlock> tb in server)
                {
                    TB = tb.Value;
                    if (retError == Result_Define.eResult.SUCCESS)
                    {
                        string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON Event_ID = {1}
                                                WHEN MATCHED THEN
                                                   Update Set 
                                                        Event_ID= {34}, Event_Type='{2}', Event_Dev_Name=N'{3}', Event_Tooltip=N'{4}', ActiveType='{5}', Event_StartTime='{6}', Event_EndTime='{7}', Event_Loop='{8}', Event_LoopType='{9}'
                                                        , ActiveTriggerType1='{10}', ActiveTriggerType1_Value1='{11}', ActiveTriggerType1_Value2='{12}', ActiveTriggerType1_Value3='{13}'
                                                        , ActiveTriggerType2='{14}', ActiveTriggerType2_Value1='{15}', ActiveTriggerType2_Value2='{16}', ActiveTriggerType2_Value3='{17}'
                                                        , ClearTriggerType1='{18}', ClearTriggerType1_Value1='{19}', ClearTriggerType1_Value2='{20}', ClearTriggerType1_Value3='{21}'
                                                        , ClearTriggerType2='{22}', ClearTriggerType2_Value1='{23}', ClearTriggerType2_Value2='{24}', ClearTriggerType2_Value3='{25}'
                                                        , Reward_Box1ID='{26}', Reward_Box2ID='{27}', Reward_Box3ID='{28}', Reward_Box4ID='{29}'
                                                        , Event_Price_Ruby='{30}', Reward_Mail_Subject_CN=N'{31}', Reward_Mail_Text_CN=N'{32}', OrderID = {33}
                                                WHEN NOT MATCHED THEN
                                                    INSERT (
                                                            Event_ID, Event_Type, Event_Dev_Name, Event_Tooltip, ActiveType, Event_StartTime, Event_EndTime, Event_Loop, Event_LoopType
                                                            , ActiveTriggerType1, ActiveTriggerType1_Value1, ActiveTriggerType1_Value2, ActiveTriggerType1_Value3
                                                            , ActiveTriggerType2, ActiveTriggerType2_Value1, ActiveTriggerType2_Value2, ActiveTriggerType2_Value3
                                                            , ClearTriggerType1, ClearTriggerType1_Value1, ClearTriggerType1_Value2, ClearTriggerType1_Value3
                                                            , ClearTriggerType2, ClearTriggerType2_Value1, ClearTriggerType2_Value2, ClearTriggerType2_Value3
                                                            , Reward_Box1ID, Reward_Box2ID, Reward_Box3ID, Reward_Box4ID
                                                            , Event_Price_Ruby, Reward_Mail_Subject_CN, Reward_Mail_Text_CN, OrderID
                                                    )
                                                    Values ('{34}', '{2}', N'{3}', N'{4}', '{5}', '{6}', '{7}', '{8}', '{9}'
                                                            , '{10}', '{11}', '{12}', '{13}'
                                                            , '{14}', '{15}', '{16}', '{17}'
                                                            , '{18}', '{19}', '{20}', '{21}'
                                                            , '{22}', '{23}', '{24}', '{25}'
                                                            , '{26}', '{27}', '{28}', '{29}'
                                                            , '{30}', N'{31}', N'{32}', {33});
                                                ", Trigger_Define.System_Event_TableName
                                                    , idx, setData.Event_Type, setData.Event_Dev_Name, setData.Event_Tooltip, setData.ActiveType, startDate, endDate, setData.Event_Loop, setData.Event_LoopType
                                                    , setData.ActiveTriggerType1, setData.ActiveTriggerType1_Value1, setData.ActiveTriggerType1_Value2, setData.ActiveTriggerType1_Value3
                                                    , setData.ActiveTriggerType2, setData.ActiveTriggerType2_Value1, setData.ActiveTriggerType2_Value2, setData.ActiveTriggerType2_Value3
                                                    , setData.ClearTriggerType1, setData.ClearTriggerType1_Value1, setData.ClearTriggerType1_Value2, setData.ClearTriggerType1_Value3
                                                    , setData.ClearTriggerType2, setData.ClearTriggerType2_Value1, setData.ClearTriggerType2_Value2, setData.ClearTriggerType2_Value3
                                                    , rewardID1, rewardID2, rewardID3, rewardID4, setData.Event_Price_Ruby, setData.Reward_Mail_Subject_CN, setData.Reward_Mail_Text_CN, setData.OrderID, setData.Event_ID);
                        retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retError != Result_Define.eResult.SUCCESS)
                            break;
                        
                    }
                }
            }
            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            return retError;
        }

        public static System_Product_ID GetSystemProduct(ref TxnBlock TB, int billingType, int price, string dbkey = GMData_Define.GmDBName)
        {
            string setQuery = string.Format(@"Select * From {0} with(nolock) Where Billing_Platform_Type = {1} And PriceValue = {2}", GMData_Define.SystemProductIDTable, billingType, price);
            System_Product_ID retObj = GenericFetch.FetchFromDB<System_Product_ID>(ref TB, setQuery, dbkey);
            return retObj == null ? new System_Product_ID() : retObj;
        }

        public static System_Product_ID GetSystemProductBYRealPrice(ref TxnBlock TB, int price, string dbkey = GMData_Define.GmDBName)
        {
            string setQuery = string.Format(@"Select top 1 * From {0} with(nolock) Where Real_PriceValue = {1}", GMData_Define.SystemProductIDTable,  price);
            System_Product_ID retObj = GenericFetch.FetchFromDB<System_Product_ID>(ref TB, setQuery, dbkey);
            return retObj == null ? new System_Product_ID() : retObj;
        }

        public static Result_Define.eResult InsertPackageList(ref Dictionary<long, TxnBlock> server, string startDate, string endDate, System_Package_List package, List<System_Package_RewardBox> itemlist1, List<System_Package_RewardBox> itemlist2, List<System_Package_RewardBox> itemlist3, List<System_Package_RewardBox> itemlist4, List<int> billingList, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            TxnBlock TB = server.First().Value;
            TxnBlock TB2 = server.First().Value;
            long packageID = GetMaxIndexValue(ref TB, 1, GMData_Define.eSystemType.PACKAGE);
            if (packageID < 1000000)
                packageID = 1000000;
            int realPrice = package.Buy_PriceValue;
            if (package.Buy_PriceType == "PriceType_PayReal")
            {
                foreach (KeyValuePair<long, TxnBlock> tb in server)
                {
                    TB = tb.Value;
                    foreach (int item in billingList)
                    {
                        System_Product_ID productInfo = GetSystemProduct(ref TB2, item, package.Buy_PriceValue);
                        retError = ShopManager.SetProductID(ref TB, packageID, Shop_Define.eShopType.Billing, (Shop_Define.eBillingType)item, productInfo.Product_ID, productInfo.Real_PriceValue, productInfo.PriceTier);
                        realPrice = productInfo.Real_PriceValue;
                        if (retError != Result_Define.eResult.SUCCESS)
                            break;
                    }
                    if (retError != Result_Define.eResult.SUCCESS)
                        break;
                }
            }
            if (packageID > 0 && retError == Result_Define.eResult.SUCCESS)
            {
                package.Buy_PriceValue = realPrice;
                long rewardID1 = 0;
                long rewardID2 = 0;
                long rewardID3 = 0;
                long rewardID4 = 0;

                if (itemlist1.Count > 0)
                {
                    rewardID1 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.PACKAGE);
                    retError = UpdatePackageRewardBox(ref server, rewardID1, itemlist1);
                }
                if (itemlist2.Count > 0)
                {
                    rewardID2 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.PACKAGE);
                    retError = UpdatePackageRewardBox(ref server, rewardID2, itemlist2);
                }
                if (itemlist3.Count > 0)
                {
                    rewardID3 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.PACKAGE);
                    retError = UpdatePackageRewardBox(ref server, rewardID3, itemlist3);
                }
                if (itemlist4.Count > 0)
                {
                    rewardID4 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.PACKAGE);
                    retError = UpdatePackageRewardBox(ref server, rewardID4, itemlist4);
                }

                foreach (KeyValuePair<long, TxnBlock> tb in server)
                {
                    TB = tb.Value;

                    string setQuery = string.Format(@"Insert Into {0} (Package_ID, ActiveType, Buy_PriceType, Buy_PriceValue, VIP_Level, Grade, Max_Buy, NameCN1, NameCN2, ToolTipCN, DetailCN, Reward_Box1ID, Reward_Box2ID, Reward_Box3ID, Reward_Box4ID, VIP_Point, LoopType)
                                                                Values ({1}, {2}, '{3}', {4}, {5}, {6}, {7}, N'{8}', N'{9}', N'{10}', N'{11}', {12}, {13}, {14}, {15}, {16}, {17})", Shop_Define.Shop_System_Package_List_TableName
                                                                , packageID, package.ActiveType, package.Buy_PriceType, package.Buy_PriceValue, package.VIP_Level
                                                                , package.Grade, package.Max_Buy, package.NameCN1, package.NameCN2, package.ToolTipCN, package.DetailCN, rewardID1, rewardID2, rewardID3, rewardID4, package.VIP_Point, package.LoopType);
                    retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                    if (retError == Result_Define.eResult.SUCCESS)
                    {
                        string setQuery2 = string.Format(@"Insert Into {0} (Shop_Goods_ID, Sale_Rate, SaleStartTime, SaleEndTime, SaleType, DefaultSaleType) Values ({1},0,'{2}','{3}', 0, 0)", Shop_Define.Shop_Limit_TableName, packageID, startDate, endDate);
                        retError = TB.ExcuteSqlCommand(dbkey, setQuery2) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                    }
                    if (retError != Result_Define.eResult.SUCCESS)
                        break;
                }
            }
            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            return retError;
        }

        public static Result_Define.eResult UpdatePackageList(ref Dictionary<long, TxnBlock> server, string startDate, string endDate, System_Package_List package, List<System_Package_RewardBox> itemlist1, List<System_Package_RewardBox> itemlist2, List<System_Package_RewardBox> itemlist3, List<System_Package_RewardBox> itemlist4, List<int> billingList, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            long rewardID1 = package.Reward_Box1ID;
            long rewardID2 = package.Reward_Box2ID;
            long rewardID3 = package.Reward_Box3ID;
            long rewardID4 = package.Reward_Box4ID;
            TxnBlock TB = server.First().Value;
            TxnBlock TB2 = server.First().Value;

            if (package.Buy_PriceType == "PriceType_PayReal")
            {
                foreach (KeyValuePair<long, TxnBlock> tb in server)
                {
                    TB = tb.Value;
                    foreach (int item in billingList)
                    {
                        System_Product_ID productInfo = GetSystemProduct(ref TB2, item, package.Buy_PriceValue);
                        retError = ShopManager.SetProductID(ref TB, package.Package_ID, Shop_Define.eShopType.Billing, (Shop_Define.eBillingType)item, productInfo.Product_ID, productInfo.Real_PriceValue, productInfo.PriceTier);

                        if (retError != Result_Define.eResult.SUCCESS)
                            break;
                    }
                    if (retError != Result_Define.eResult.SUCCESS)
                        break;
                }
            }
            if (retError == Result_Define.eResult.SUCCESS)
            {
                if (itemlist1.Count > 0 && package.Reward_Box1ID == 0)
                {
                    rewardID1 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.PACKAGE);
                    retError = UpdatePackageRewardBox(ref server, rewardID1, itemlist1);
                }
                else if (itemlist1.Count > 0 && package.Reward_Box1ID > 0)
                {
                    retError = UpdatePackageRewardBox(ref server, rewardID1, itemlist1);
                }
                else
                {
                    retError = DeletePackageRewardBox(ref server, rewardID1);
                    rewardID1 = 0;
                }
                if (itemlist2.Count > 0 && package.Reward_Box2ID == 0)
                {
                    rewardID2 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.PACKAGE);
                    retError = UpdatePackageRewardBox(ref server, rewardID2, itemlist2);
                }
                else if (itemlist2.Count > 0 && package.Reward_Box2ID > 0)
                {
                    retError = UpdatePackageRewardBox(ref server, rewardID2, itemlist2);
                }
                else
                {
                    retError = DeletePackageRewardBox(ref server, rewardID2);
                    rewardID2 = 0;
                }
                if (itemlist3.Count > 0 && package.Reward_Box3ID == 0)
                {
                    rewardID3 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.PACKAGE);
                    retError = UpdatePackageRewardBox(ref server, rewardID3, itemlist3);
                }
                else if (itemlist3.Count > 0 && package.Reward_Box3ID > 0)
                {
                    retError = UpdatePackageRewardBox(ref server, rewardID3, itemlist3);
                }
                else
                {
                    retError = DeletePackageRewardBox(ref server, rewardID3);
                    rewardID3 = 0;
                }
                if (itemlist4.Count > 0 && package.Reward_Box4ID == 0)
                {
                    rewardID4 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.PACKAGE);
                    retError = UpdatePackageRewardBox(ref server, rewardID4, itemlist4);
                }
                else if (itemlist4.Count > 0 && package.Reward_Box4ID > 0)
                {
                    retError = UpdatePackageRewardBox(ref server, rewardID4, itemlist4);
                }
                else
                {
                    retError = DeletePackageRewardBox(ref server, rewardID4);
                    rewardID4 = 0;
                }
            }
            if (retError == Result_Define.eResult.SUCCESS)
            {
                foreach (KeyValuePair<long, TxnBlock> tb in server)
                {
                    TB = tb.Value;

                    string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON Package_ID = {1}
                                                WHEN MATCHED THEN
                                                    UPDATE SET 
                                                    ActiveType = {2}, Buy_PriceType = '{3}', Buy_PriceValue = {4}
                                                    , VIP_Level = {5}, Grade = {6}, Max_Buy = {7}, NameCN1 = N'{8}', NameCN2 = N'{9}'
                                                    , ToolTipCN = N'{10}', DetailCN = N'{11}'
                                                    , Reward_Box1ID = {12}, Reward_Box2ID = {13}, Reward_Box3ID = {14}, Reward_Box4ID = {15}
                                                    , VIP_Point = {16}, LoopType = {17}
                                                WHEN NOT MATCHED THEN
                                                    INSERT (
                                                            Package_ID, ActiveType, Buy_PriceType, Buy_PriceValue
                                                            , VIP_Level, Grade, Max_Buy, NameCN1, NameCN2, ToolTipCN
                                                            , DetailCN, Reward_Box1ID, Reward_Box2ID, Reward_Box3ID
                                                            , Reward_Box4ID, VIP_Point, LoopType
                                                    )
                                                    Values ({1}, {2}, '{3}', {4}, {5}, {6}, {7}, N'{8}', N'{9}', N'{10}', N'{11}', {12}, {13}, {14}, {15}, {16}, {17});
                                                ", Shop_Define.Shop_System_Package_List_TableName
                                                    , package.Package_ID, package.ActiveType, package.Buy_PriceType, package.Buy_PriceValue, package.VIP_Level
                                                    , package.Grade, package.Max_Buy, package.NameCN1, package.NameCN2, package.ToolTipCN, package.DetailCN, rewardID1, rewardID2, rewardID3, rewardID4
                                                    , package.VIP_Point, package.LoopType);
                    retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                    if (retError == Result_Define.eResult.SUCCESS)
                    {
                        string setQuery2 = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON Shop_Goods_ID = {1}
                                                WHEN MATCHED THEN
                                                    UPDATE SET 
                                                    SaleStartTime='{2}', SaleEndTime='{3}'
                                                WHEN NOT MATCHED THEN
                                                   INSERT (
                                                        [Shop_Goods_ID]
                                                       ,[Sale_Rate]
                                                       ,[SaleStartTime]
                                                       ,[SaleEndTime]
                                                       ,[SaleType]
                                                       ,[DefaultSaleType]
                                                    )
                                                   VALUES (
                                                         '{1}'
                                                        ,0
                                                        ,'{2}'
                                                        ,'{3}'
                                                        ,0
                                                        ,0
                                                    );
                                                "
                                                , Shop_Define.Shop_Limit_TableName, package.Package_ID, startDate, endDate);
                        retError = TB.ExcuteSqlCommand(dbkey, setQuery2) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                    }
                    if (retError != Result_Define.eResult.SUCCESS)
                        break;
                }
            }
            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            return retError;
        }

        public static List<GM_System_Package_List> GetPackageCheapList(ref TxnBlock TB, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format(@"Select A.Package_ID, ISNULL(B.SaleStartTime, GETDATE()-1) AS SaleStartTime, ISNULL(B.SaleEndTime, getdate()+1) AS SaleEndTime 
                                                From {0} AS A WITH(NOLOCK) inner join {1} as B WITH(NOLOCK) ON A.Package_ID = B.Shop_Goods_ID 
                                                Where A.ActiveType=1 Order by B.SaleStartTime Desc, A.Package_ID DESC", Shop_Define.Shop_System_Package_Cheap_TableName, Shop_Define.Shop_Limit_TableName);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<GM_System_Package_List>(ref TB, setQuery, dbkey);
        }

        public static long GetPackageCheapIndex1Yuan(ref TxnBlock TB, long index, string startDate, string endDate, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format(@"Select top 1 * From {0} WITH(NOLOCK, INDEX(IDX_GM_LIMITDATE)) 
                                                Where CONVERT(nvarchar(19),SaleStartTime,121) = '{1}' And CONVERT(nvarchar(19),SaleEndTime,121) = '{2}' And Shop_Goods_ID < {3} order by Shop_Goods_ID desc", Shop_Define.Shop_Limit_TableName, startDate, endDate, index);
            return TheSoul.DataManager.GenericFetch.FetchFromDB<System_Shop_Limit_List>(ref TB, setQuery, dbkey).Shop_Goods_ID;
        }

        public static List<string> GetPackageCheapMaxDateServerList(ref Dictionary<long, TxnBlock> server, long index, long index2, string setDate, string endDate)
        {            
            string setQuery = string.Format(@"Select * From {0} WITH(NOLOCK, INDEX(IDX_GM_LIMITDATE)) 
                                                Where (SaleStartTime between '{2}' And '{3}' Or SaleEndTime between '{2}' And '{3}')
                                                    And Shop_Goods_ID in (Select Package_ID From {1} WITH(NOLOCK) Where ActiveType = 1 And Package_ID not in ({4},{5}))"
                                                , Shop_Define.Shop_Limit_TableName, Shop_Define.Shop_System_Package_Cheap_TableName, setDate, endDate, index, index2);
            List<string> resultServerList = new List<string>();
            TxnBlock TB = server.First().Value;
            List<server_group_config> serverList = GlobalManager.GetServerGroupList(ref TB).FindAll(item => item.server_group_id > 0);
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TB = tb.Value;
                string dbkey = GMData_Define.ShardingDBName;
                List<System_Gacha_Best> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<System_Gacha_Best>(ref TB, setQuery, dbkey);
                if (retObj.Count > 0)
                {
                    resultServerList.Add(serverList.Find(item => item.server_group_id.ToString() == tb.Key.ToString()).server_group_name);
                }
            }
            return resultServerList;
        }

        public static Result_Define.eResult InsertPackageCheap(ref Dictionary<long, TxnBlock> server, string startDate, string endDate, List<System_Package_List> pagckgeList, List<System_Package_RewardBox> itemlist1, List<System_Package_RewardBox> itemlist2, List<int> billingList, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            int loopCount = 0;
            foreach (System_Package_List package in pagckgeList)
            {
                TxnBlock TB = server.First().Value;
                TxnBlock TB2 = server.First().Value;
                long packageID = GetMaxIndexValue(ref TB, 1, GMData_Define.eSystemType.PACKAGE);

                if (packageID > 0)
                {
                    //goods code add
                    int realPrice = package.Buy_PriceValue;
                    foreach (KeyValuePair<long, TxnBlock> tb in server)
                    {
                        TB = tb.Value;
                        foreach (int item in billingList)
                        {
                            System_Product_ID productInfo = GetSystemProduct(ref TB2, item, package.Buy_PriceValue);
                            retError = ShopManager.SetProductID(ref TB, packageID, Shop_Define.eShopType.Billing, (Shop_Define.eBillingType)item, productInfo.Product_ID, productInfo.Real_PriceValue, productInfo.PriceTier);
                            realPrice = productInfo.Real_PriceValue;
                            if (retError != Result_Define.eResult.SUCCESS)
                                break;
                        }
                        if (retError != Result_Define.eResult.SUCCESS)
                            break;
                    }
                    
                    if (retError == Result_Define.eResult.SUCCESS)
                    {
                        package.Buy_PriceValue = realPrice;
                        long rewardID1 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.PACKAGE);
                        if (loopCount > 0)
                        {
                            if (rewardID1 > 0)
                                retError = UpdatePackageCheapRewardBox(ref server, rewardID1, itemlist1);
                        }
                        else
                        {
                            if (rewardID1 > 0)
                                retError = UpdatePackageCheapRewardBox(ref server, rewardID1, itemlist2);
                        }
                        if (retError == Result_Define.eResult.SUCCESS)
                        {
                            foreach (KeyValuePair<long, TxnBlock> tb in server)
                            {
                                TB = tb.Value;

                                string setQuery = string.Format(@"Insert Into {0} (Package_ID, ActiveType, Buy_PriceType, Buy_PriceValue, VIP_Level, Grade, Max_Buy, NameCN1, NameCN2, ToolTipCN, DetailCN, Reward_Box1ID, Reward_Box2ID, Reward_Box3ID, Reward_Box4ID, VIP_Point, LoopType)
                                                                Values ({1}, 1, 'PriceType_PayReal', {2}, 0, 0, {3}, N'{4}', N'xxx', N'xxx', N'xxx', {5}, 0, 0, 0, {6}, {7})", Shop_Define.Shop_System_Package_Cheap_TableName
                                                                            , packageID, package.Buy_PriceValue, package.Max_Buy, package.NameCN1, rewardID1, package.VIP_Point, package.LoopType);
                                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    string setQuery2 = string.Format(@"Insert Into {0} (Shop_Goods_ID, Sale_Rate, SaleStartTime, SaleEndTime, SaleType, DefaultSaleType) Values ({1},0,'{2}','{3}', 0, 0)", Shop_Define.Shop_Limit_TableName, packageID, startDate, endDate);
                                    retError = TB.ExcuteSqlCommand(dbkey, setQuery2) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                                }
                                if (retError != Result_Define.eResult.SUCCESS)
                                    break;
                            }
                        }
                    }
                }
                loopCount++;
            }
            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            return retError;
        }

        public static Result_Define.eResult UpdatePackageCheap(ref Dictionary<long, TxnBlock> server, string startDate, string endDate, List<System_Package_List> pagckgeList, List<System_Package_RewardBox> itemlist1, List<System_Package_RewardBox> itemlist2, List<int> billingList, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            int loopCount = 0;
            TxnBlock TB2 = server.First().Value;
            foreach (System_Package_List package in pagckgeList)
            {
                long rewardID1 = package.Reward_Box1ID;
                if (loopCount > 0)
                {
                    retError = UpdatePackageCheapRewardBox(ref server, rewardID1, itemlist1);
                }
                else
                {
                    retError = UpdatePackageCheapRewardBox(ref server, rewardID1, itemlist2);
                }

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    foreach (KeyValuePair<long, TxnBlock> tb in server)
                    {
                        TxnBlock TB = tb.Value;
                        foreach (int item in billingList)
                        {
                            System_Product_ID productInfo = GetSystemProduct(ref TB2, item, package.Buy_PriceValue);
                            retError = ShopManager.SetProductID(ref TB, package.Package_ID, Shop_Define.eShopType.Billing, (Shop_Define.eBillingType)item, productInfo.Product_ID, productInfo.Real_PriceValue, productInfo.PriceTier);

                            if (retError != Result_Define.eResult.SUCCESS)
                                break;
                        }
                        if (retError != Result_Define.eResult.SUCCESS)
                            break;
                    }
                }

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    foreach (KeyValuePair<long, TxnBlock> tb in server)
                    {
                        TxnBlock TB = tb.Value;

                        string setQuery = string.Format(@"MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON Package_ID = {1}
                                                WHEN MATCHED THEN
                                                    UPDATE SET ActiveType=0
                                                WHEN NOT MATCHED THEN
                                                   Insert (Package_ID, ActiveType, Buy_PriceType, Buy_PriceValue, VIP_Level, Grade, Max_Buy, NameCN1, NameCN2, ToolTipCN, DetailCN, Reward_Box1ID, Reward_Box2ID, Reward_Box3ID, Reward_Box4ID, VIP_Point, LoopType)
                                                            Values ({1}, 0, 'PriceType_PayReal', {2}, 0, 0, {3}, N'{4}', N'xxx', N'xxx', N'xxx', {5}, 0, 0, 0, {6}, {7});"
                                                    , Shop_Define.Shop_System_Package_Cheap_TableName, package.Package_ID, package.Buy_PriceValue, package.Max_Buy, package.NameCN1, rewardID1, package.VIP_Point, package.LoopType);
                        retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retError != Result_Define.eResult.SUCCESS)
                            break;
                    }
                }
                loopCount++;
            }
            if (retError == Result_Define.eResult.SUCCESS)
            {
                retError = InsertPackageCheap(ref server, startDate, endDate, pagckgeList, itemlist1, itemlist2, billingList);
            }
            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            return retError;
        }

        public static Result_Define.eResult Update_Event_7Day(ref Dictionary<long, TxnBlock> server, System_Event_7Day setData, List<System_Event_7Day_Reward> itemlist1, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            long rewardID1 = setData.Reward_Box1ID;
            TxnBlock TB = server.First().Value;
            if (itemlist1.Count > 0 && setData.Reward_Box1ID == 0)
            {
                rewardID1 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.SEVEN_EVENT);
                retError = Update_7Day_Event_Package_RewardBox(ref server, rewardID1, itemlist1);
            }
            else if (itemlist1.Count > 0 && setData.Reward_Box1ID > 0)
            {
                retError = Update_7Day_Event_Package_RewardBox(ref server, rewardID1, itemlist1);
            }
            else
            {
                retError = Delete_7Day_Event_Package_RewardBox(ref server, rewardID1);
                rewardID1 = 0;
            }
            
            if (retError == Result_Define.eResult.SUCCESS)
            {
                foreach (KeyValuePair<long, TxnBlock> tb in server)
                {
                    TB = tb.Value;
                    if (retError == Result_Define.eResult.SUCCESS)
                    {
                        string setQuery = string.Format(@"Update {0} Set Event_Dev_Name=N'{2}', Event_Tooltip=N'{3}'
                                                                , ClearTriggerType1='{4}', ClearTriggerType1_Value1='{5}', ClearTriggerType1_Value2='{6}', ClearTriggerType1_Value3='{7}'
                                                                , ClearTriggerType2='{8}', ClearTriggerType2_Value1='{9}', ClearTriggerType2_Value2='{10}', ClearTriggerType2_Value3='{11}'
                                                                , Reward_Box1ID='{12}'
                                                                Where Event_ID = {1}", Trigger_Define.System_7Day_Event_TableName
                                                                    , setData.Event_ID, setData.Event_Dev_Name, setData.Event_Tooltip
                                                                    , setData.ClearTriggerType1, setData.ClearTriggerType1_Value1, setData.ClearTriggerType1_Value2, setData.ClearTriggerType1_Value3
                                                                    , setData.ClearTriggerType2, setData.ClearTriggerType2_Value1, setData.ClearTriggerType2_Value2, setData.ClearTriggerType2_Value3
                                                                    , rewardID1);
                        retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retError != Result_Define.eResult.SUCCESS)
                            break;
                    }
                }
            }
            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            return retError;
        }

        public static Result_Define.eResult Update_Event_7Day_Package(ref Dictionary<long, TxnBlock> server, System_Event_7Day_Package_List package, List<System_Event_7Day_Reward> itemlist1, List<System_Event_7Day_Reward> itemlist2, List<System_Event_7Day_Reward> itemlist3, List<System_Event_7Day_Reward> itemlist4, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            long rewardID1 = package.Reward_Box1ID;
            long rewardID2 = package.Reward_Box2ID;
            long rewardID3 = package.Reward_Box3ID;
            long rewardID4 = package.Reward_Box4ID;

            TxnBlock TB = server.First().Value;
            if (itemlist1.Count > 0 && package.Reward_Box1ID == 0)
            {
                rewardID1 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.SEVEN_EVENT);
                retError = Update_7Day_Event_Package_RewardBox(ref server, rewardID1, itemlist1);
            }
            else if (itemlist1.Count > 0 && package.Reward_Box1ID > 0)
            {
                retError = Update_7Day_Event_Package_RewardBox(ref server, rewardID1, itemlist1);
            }
            else
            {
                retError = Delete_7Day_Event_Package_RewardBox(ref server, rewardID1);
                rewardID1 = 0;
            }
            if (itemlist2.Count > 0 && package.Reward_Box2ID == 0)
            {
                rewardID2 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.PACKAGE);
                retError = Update_7Day_Event_Package_RewardBox(ref server, rewardID2, itemlist2);
            }
            else if (itemlist2.Count > 0 && package.Reward_Box2ID > 0)
            {
                retError = Update_7Day_Event_Package_RewardBox(ref server, rewardID2, itemlist2);
            }
            else
            {
                retError = Delete_7Day_Event_Package_RewardBox(ref server, rewardID2);
                rewardID2 = 0;
            }
            if (itemlist3.Count > 0 && package.Reward_Box3ID == 0)
            {
                rewardID3 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.PACKAGE);
                retError = Update_7Day_Event_Package_RewardBox(ref server, rewardID3, itemlist3);
            }
            else if (itemlist3.Count > 0 && package.Reward_Box3ID > 0)
            {
                retError = Update_7Day_Event_Package_RewardBox(ref server, rewardID3, itemlist3);
            }
            else
            {
                retError = Delete_7Day_Event_Package_RewardBox(ref server, rewardID3);
                rewardID3 = 0;
            }
            if (itemlist4.Count > 0 && package.Reward_Box4ID == 0)
            {
                rewardID4 = GetMaxIndexValue(ref TB, 2, GMData_Define.eSystemType.PACKAGE);
                retError = Update_7Day_Event_Package_RewardBox(ref server, rewardID4, itemlist4);
            }
            else if (itemlist4.Count > 0 && package.Reward_Box4ID > 0)
            {
                retError = Update_7Day_Event_Package_RewardBox(ref server, rewardID4, itemlist4);
            }
            else
            {
                retError = Delete_7Day_Event_Package_RewardBox(ref server, rewardID4);
                rewardID4 = 0;
            }

            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TB = tb.Value;
                string setQuery = string.Format(@"Update {0} Set ActiveType = {2}, Buy_PriceType = '{3}', Buy_PriceValue = {4}, Grade = {5}, Max_Buy = {6}, NameCN1 = N'{7}', NameCN2 = N'{8}'
                                                            , ToolTipCN = N'{9}', DetailCN = N'{10}', Reward_Box1ID = {11}, Reward_Box2ID = {12}, Reward_Box3ID = {13}, Reward_Box4ID = {14}, VIP_Point = {15}
                                                            Where Package_ID = {1}", Trigger_Define.System_7Day_Event_Package_TableName
                                                            , package.Package_ID, package.ActiveType, package.Buy_PriceType, package.Buy_PriceValue
                                                            , package.Grade, package.Max_Buy, package.NameCN1, package.NameCN2, package.ToolTipCN, package.DetailCN, rewardID1, rewardID2, rewardID3, rewardID4, package.VIP_Point);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;

                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }
            if (retError == Result_Define.eResult.SUCCESS)
                GMDataManager.SetRedisDataInit(ref server);
            return retError;
        }

        private static Result_Define.eResult UpdatePackageRewardBox(ref Dictionary<long, TxnBlock> server, long idx, List<System_Package_RewardBox> itemList, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;


            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                string setQuery = string.Format("Delete From {0} Where RewardBoxID = {1}", Shop_Define.Shop_System_Package_RewardBox_TableName, idx);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError == Result_Define.eResult.SUCCESS)
                {
                    foreach (System_Package_RewardBox item in itemList)
                    {
                        setQuery = string.Format(@"Insert Into {0} (RewardBoxID, ItemIndex, Item_TargetType, Item_ID, Item_Level, Item_Grade, Item_Rnd1Type, Item_Rnd1Value, Item_Rnd2Type, Item_Rnd2Value, Item_Rnd3Type, Item_Rnd3Value, Item_Num)
                                                                Values ({1}, {2},'{3}',{4},{5},{6},0, 0, 0, 0, 0, 0, {7})", Shop_Define.Shop_System_Package_RewardBox_TableName
                                                                    , idx, item.ItemIndex, item.Item_TargetType, item.Item_ID, item.Item_Level, item.Item_Grade, item.Item_Num);
                        retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retError != Result_Define.eResult.SUCCESS)
                            break;

                    }
                }
            }
            return retError;
        }

        private static Result_Define.eResult DeletePackageRewardBox(ref Dictionary<long, TxnBlock> server, long idx, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;


            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;

                string setQuery = string.Format("Delete From {0} Where RewardBoxID = {1}", Shop_Define.Shop_System_Package_RewardBox_TableName, idx);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }
            return retError;
        }

        private static Result_Define.eResult UpdatePackageCheapRewardBox(ref Dictionary<long, TxnBlock> server, long idx, List<System_Package_RewardBox> itemList, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;


            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                string setQuery = string.Format("Delete From {0} Where RewardBoxID = {1}", Shop_Define.Shop_System_Package_Cheap_RewardBox_TableName, idx);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError == Result_Define.eResult.SUCCESS)
                {
                    foreach (System_Package_RewardBox item in itemList)
                    {
                        item.Item_TargetType = string.IsNullOrEmpty(item.Item_TargetType) ? "Item" : item.Item_TargetType;
                        setQuery = string.Format(@"Insert Into {0} (RewardBoxID, ItemIndex, Item_TargetType, Item_ID, Item_Level, Item_Grade, Item_Rnd1Type, Item_Rnd1Value, Item_Rnd2Type, Item_Rnd2Value, Item_Rnd3Type, Item_Rnd3Value, Item_Num)
                                                                Values ({1}, {2},'{3}',{4},{5},{6},0, 0, 0, 0, 0, 0, {7})", Shop_Define.Shop_System_Package_Cheap_RewardBox_TableName
                                                                    , idx, item.ItemIndex, item.Item_TargetType, item.Item_ID, item.Item_Level, item.Item_Grade, item.Item_Num);
                        retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retError != Result_Define.eResult.SUCCESS)
                            break;
                    }
                }
            }
            return retError;
        }

        private static Result_Define.eResult Update_7Day_Event_Package_RewardBox(ref Dictionary<long, TxnBlock> server, long idx, List<System_Event_7Day_Reward> itemList, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;

            retError = Delete_7Day_Event_Package_RewardBox(ref server, idx);
            if (retError == Result_Define.eResult.SUCCESS)
            {
                foreach (KeyValuePair<long, TxnBlock> tb in server)
                {
                    TxnBlock TB = tb.Value;

                    foreach (System_Package_RewardBox item in itemList)
                    {
                        string setQuery = string.Format(@"Insert Into {0} (RewardBoxID, ItemIndex, Item_TargetType, Item_ID, Item_Level, Item_Grade, Item_Rnd1Type, Item_Rnd1Value, Item_Rnd2Type, Item_Rnd2Value, Item_Rnd3Type, Item_Rnd3Value, Item_Num)
                                                                Values ({1}, {2},'{3}',{4},{5},{6},0, 0, 0, 0, 0, 0, {7})", Trigger_Define.System_7Day_Event_Reward_TableName
                                                                    , idx, item.ItemIndex, item.Item_TargetType, item.Item_ID, item.Item_Level, item.Item_Grade, item.Item_Num);
                        retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        if (retError != Result_Define.eResult.SUCCESS)
                            break;
                    }
                }
            }
            return retError;
        }

        private static Result_Define.eResult Delete_7Day_Event_Package_RewardBox(ref Dictionary<long, TxnBlock> server, long idx, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;


            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;

                string setQuery = string.Format("Delete From {0} Where RewardBoxID = {1}", Trigger_Define.System_7Day_Event_Reward_TableName, idx);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }
            return retError;
        }

        public static Admin_System_MailNotice GetAminMailInfo(ref TxnBlock TB, long index, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Where idx = {1}", Mail_Define.Mail_SystemMailNoticeTableName, index);
            Admin_System_MailNotice retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<Admin_System_MailNotice>(ref TB, setQuery, dbkey);
            return retObj == null ? new Admin_System_MailNotice() : retObj;
        }
        
        public static List<Admin_System_MailNotice> GetAminMailList(ref TxnBlock TB, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Order by idx desc", Mail_Define.Mail_SystemMailNoticeTableName);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<Admin_System_MailNotice>(ref TB, setQuery, dbkey);
        }

        public static Result_Define.eResult InsertAdminMail(ref Dictionary<long, TxnBlock> server, string sedate, string edate, Admin_System_MailNotice setInfo, List<Admin_System_MailNotice_Reward> reward, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            string gmId = "";
            if (HttpContext.Current.Request.Cookies.Count == 0)
            {
                gmId = "test2";
            }
            else
            {
                gmId = HttpContext.Current.Request.Cookies["mseedadmin"]["userid"];
            }
            long index = 0;
            retError = GetAdminMailIndex(ref server, ref index);
            if (retError == Result_Define.eResult.SUCCESS)
            {
                foreach (KeyValuePair<long, TxnBlock> tb in server)
                {
                    TxnBlock TB = tb.Value;
                    string setQuery = string.Format(@"Insert Into {0} (idx, active, MailType, senderName, title, message, startDate, endDate, regDate, regID)
                                                        Values ({1}, 1, {2}, N'{3}', N'{4}', N'{5}', N'{6}', N'{7}', getdate() , N'{8}')",
                                                                Mail_Define.Mail_SystemMailNoticeTableName, index, setInfo.MailType, setInfo.senderName, setInfo.title
                                                                , setInfo.message, sedate, edate, gmId);
                    retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                    if (retError != Result_Define.eResult.SUCCESS)
                        break;
                }
                if (retError == Result_Define.eResult.SUCCESS && setInfo.MailType == 1 && reward.Count > 0)
                {
                    retError = InsertAdminMailReward(ref server, index, reward);
                }
            }
            return retError;
        }

        private static Result_Define.eResult GetAdminMailIndex(ref Dictionary<long, TxnBlock> server, ref long index, string dbkey = GMData_Define.GmDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            TxnBlock TB = server.First().Value;
            string setQuery = string.Format("Select ISNULL(MAX(idx),0) as number From {0} WITH(NOLOCK)", GMData_Define.AdminSystemMailNoticeIndexTable);
            long maxNum = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey).number;
            long mailMaxNum = 0;
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TB = tb.Value;
                setQuery = string.Format("Select ISNULL(MAX(idx), 0) as number From {0} WITH(NOLOCK)", Mail_Define.Mail_SystemMailNoticeTableName);
                long tableMaxNum = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, GMData_Define.ShardingDBName).number;
                if (tableMaxNum > mailMaxNum)
                    mailMaxNum = tableMaxNum;
            }

            TB = server.First().Value;

            if (mailMaxNum > maxNum)
            {
                string setQuery3 = string.Format("Insert into {0} Values ({1}, getdate())", GMData_Define.AdminSystemMailNoticeIndexTable, mailMaxNum);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery3) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                setQuery = string.Format("Select ISNULL(MAX(idx),0) + 1 as number From {0} WITH(NOLOCK)", GMData_Define.AdminSystemMailNoticeIndexTable);
                index = TheSoul.DataManager.GenericFetch.FetchFromDB<GM_Number>(ref TB, setQuery, dbkey).number;

                string setQuery2 = string.Format("Insert into {0} Values ({1}, getdate())", GMData_Define.AdminSystemMailNoticeIndexTable, index);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery2) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            }
            return retError;
        }

        public static Result_Define.eResult InsertAdminMailReward(ref Dictionary<long, TxnBlock> server, long index, List<Admin_System_MailNotice_Reward> reward, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            foreach (KeyValuePair<long, TxnBlock> tb in server)
            {
                TxnBlock TB = tb.Value;
                foreach (Admin_System_MailNotice_Reward item in reward)
                {
                    string setQuery = string.Format(@"Insert Into {0} (MailIndex, ItemIndex, Item_ID, Item_Grade, Item_Level, Item_Num)
                                                                Values({1}, {2}, {3}, {4}, {5}, {6})", Mail_Define.Mail_SystemMailNoticeRewardTableName
                                                                    , index, item.ItemIndex, item.Item_ID, item.Item_Grade, item.Item_Level, item.Item_Num);
                    retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                    if (retError != Result_Define.eResult.SUCCESS)
                        break;
                }
            }
            return retError;
        }

        public static Result_Define.eResult DeleteAdminMail(ref TxnBlock TB, long index, string dbkey = GMData_Define.ShardingDBName)
        {
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            byte mailType = GetAminMailInfo(ref TB, index).MailType;            
            string setQuery = string.Format("Delete From {0} Where idx = {1}", Mail_Define.Mail_SystemMailNoticeTableName, index);
            retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            if (retError == Result_Define.eResult.SUCCESS && mailType == 1)
                retError = DeleteAdminMailReward(ref TB, index);
            return retError;
        }

        public static Result_Define.eResult DeleteAdminMailReward(ref TxnBlock TB, long index, string dbkey = GMData_Define.ShardingDBName)
        {
            string setQuery = string.Format("Delete From {0} Where MailIndex = {1}", Mail_Define.Mail_SystemMailNoticeRewardTableName, index);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
        }

        public static string GetActiveType(string active)
        {
            if (active.Equals("1"))
            {
                return Resources.languageResource.lang_apply;
            }
            else
            {
                return Resources.languageResource.lang_notapply;
            }
        }
    }
}