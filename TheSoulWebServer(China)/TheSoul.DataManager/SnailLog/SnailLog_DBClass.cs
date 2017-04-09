using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager.DBClass;

namespace TheSoul.DataManager.DBClass
{
    public class _snail_game_player_action_log
    {
        public long log_idx { get; set; }
        public long aid { get; set; }
        public string s_account { get; set; }
        public string s_role_name { get; set; }
        public DateTime d_create { get; set; }
        public string s_act_id { get; set; }
        public int n_type { get; set; }
        public string s_act_value { get; set; }
    }

    public class _snail_instance_log
    {
        public long log_idx { get; set; }
        public long aid { get; set; }
        public int n_id { get; set; }
        public string s_instanceid { get; set; }
        public string s_instancetypeid { get; set; }
        public DateTime d_starttime { get; set; }
        public DateTime d_endtime { get; set; }
        public int n_duration { get; set; }
        public int n_troop_count1 { get; set; }
        public int n_troop_count2 { get; set; }
    }

    public class _snail_item_log
    {
        public long log_idx { get; set; }
        public long aid { get; set; }
        public string s_account { get; set; }
        public string s_role_name { get; set; }
        public DateTime d_create { get; set; }
        public string s_event_id { get; set; }
        public int n_event_type { get; set; }
        public string s_item_id { get; set; }
        public int n_count { get; set; }
        public string s_scene_id { get; set; }
        public string s_guild_id { get; set; }
        public string s_state { get; set; }
        public string s_comment { get; set; }
        public int n_before { get; set; }
        public int n_after { get; set; }
    }

    public class _snail_money_log
    {
        public long log_idx { get; set; }
        public long aid { get; set; }
        public string s_buy_account { get; set; }
        public string s_buy_role { get; set; }
        public DateTime d_create { get; set; }
        public int n_money_type { get; set; }
        public string s_event_id { get; set; }
        public int n_event_type { get; set; }
        public string s_item_id { get; set; }
        public string s_instance_id { get; set; }
        public int n_count { get; set; }
        public int n_money { get; set; }
        public string s_recv_account { get; set; }
        public string s_recv_role { get; set; }
        public string s_scene_id { get; set; }
        public string s_guild_id { get; set; }
        public string s_state { get; set; }
        public string s_comment { get; set; }
        public int n_role_level { get; set; }
        public int n_total_logtime { get; set; }
        public int n_before { get; set; }
        public int n_after { get; set; }
    }

    public class _snail_program_log
    {
        public int n_id { get; set; }
        public string gm_id { get; set; }
        public string s_programid { get; set; }
        public string s_programname { get; set; }
        public DateTime d_opentime { get; set; }
        public DateTime d_endtime { get; set; }
    }

    public class SnailLogCount
    {
        public long count { get; set; }
    }

    public class _snail_role_log
    {
        public long log_idx { get; set; }
        public long aid { get; set; }
        public int n_id { get; set; }
        public string s_account { get; set; }
        public string s_role { get; set; }
        public string s_ip { get; set; }
        public DateTime d_logintime { get; set; }
        public DateTime d_logouttime { get; set; }
        public string s_mac { get; set; }
        public string s_os_type { get; set; }
        public string s_comment { get; set; }
    }

    public class _snail_role_additional_log
    {
        public long aid { get; set; }
        public string s_role_name { get; set; }
        public string s_uid { get; set; }
        public string s_create_acc { get; set; }
        public DateTime d_create { get; set; }
        public DateTime d_delete { get; set; }
        public int n_deleted { get; set; }
        public int n_total_sec { get; set; }
        public DateTime d_save { get; set; }
        public DateTime d_insert { get; set; }
        public int n_role_num { get; set; }
        public int n_role_level { get; set; }
        public int n_vip_level { get; set; }
        public long n_role_effectiveness { get; set; }
    }

    public class _snail_role_upgrade_log
    {
        public long log_idx { get; set; }
        public long aid { get; set; }
        public int n_id { get; set; }
        public DateTime d_time { get; set; }
        public string s_account { get; set; }
        public string s_role { get; set; }
        public int n_logtype { get; set; }
        public int n_level_before { get; set; }
        public int n_level_after { get; set; }
        public string s_skill_id { get; set; }
    }

    public class _snail_scene_log
    {
        public long log_idx { get; set; }
        public long aid { get; set; }
        public int n_id { get; set; }
        public string s_sceneid { get; set; }
        public DateTime d_date { get; set; }
        public int n_enter { get; set; }
        public decimal n_totaltime { get; set; }
    }

    public class _snail_task_log
    {
        public long log_idx { get; set; }
        public long aid { get; set; }
        public string s_task_id { get; set; }
        public string s_account { get; set; }
        public string s_role_name { get; set; }
        public int n_act_type { get; set; }
        public DateTime d_create { get; set; }
    }

    public class TaskLogInfo
    {
        public long taskid { get; set; }
        public int act_type { get; set; }       // 0 : start, 1: success, 2: fail

        public TaskLogInfo(long EventID, int ActType, Trigger_Define.eEventListType bEvent)
        {
            taskid = bEvent == Trigger_Define.eEventListType.Event ? EventID + SnailLog_Define.Snail_s_id_Seperator_event :
                bEvent == Trigger_Define.eEventListType.Achive ? EventID + SnailLog_Define.Snail_s_id_Seperator_achive :
                bEvent == Trigger_Define.eEventListType.PvP_Achive ? EventID + SnailLog_Define.Snail_s_id_Seperator_achive_pvp : EventID;
            act_type = ActType;
        }

        public static long GetS_TaskID(long EventID, Trigger_Define.eEventListType bEvent)
        {
            return bEvent == Trigger_Define.eEventListType.Event ? EventID + SnailLog_Define.Snail_s_id_Seperator_event :
                bEvent == Trigger_Define.eEventListType.Achive ? EventID + SnailLog_Define.Snail_s_id_Seperator_achive :
                bEvent == Trigger_Define.eEventListType.PvP_Achive ? EventID + SnailLog_Define.Snail_s_id_Seperator_achive_pvp : EventID;
        }
    }

    public class MoneyLogInfo
    {
        public int money_type { get; set; }     // 0 : ruby, 1: gold
        public int event_type { get; set; }     // 0 : add, 1: use
        public int n_money { get; set; }
        public int n_before { get; set; }
        public int n_after { get; set; }
        public string d_create { get; set; }
        public MoneyLogInfo(int setMoneyType, int setEventType, int setMoney, int setBefore, int setAfter)
        {
            money_type = setMoneyType;
            event_type = setEventType;
            n_money = setMoney;
            n_before = setBefore;
            n_after = setAfter;
            d_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }

    public class ItemLogInfo
    {
        public long item_id { get; set; }
        public int event_type { get; set; }     // 0 : add, 1: use
        public int n_count { get; set; }
        public int n_before { get; set; }
        public int n_after { get; set; }
        public string d_create { get; set; }

        public long inven_seq { get; set; }     // for mseed
        public short class_type { get; set; }
        public short grade { get; set; }
        public short level { get; set; }
        public string equipposition { get; set; }

        public ItemLogInfo(long setItemID, int setEventType, int setMoney, int setBefore, int setAfter)
        {
            item_id = setItemID + SnailLog_Define.Snail_s_id_Seperator_item_idx;
            event_type = setEventType;
            n_count = setMoney;
            n_before = setBefore;
            n_after = setAfter;
            d_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public ItemLogInfo(long setItemID, int setEventType, int setMoney, int setBefore, int setAfter, long setSeq, short setClass, short setGrade, short setLevel, string setEquipPosition)
        {
            item_id = setItemID + SnailLog_Define.Snail_s_id_Seperator_item_idx;
            event_type = setEventType;
            n_count = setMoney;
            n_before = setBefore;
            n_after = setAfter;
            d_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            inven_seq = setSeq;
            class_type = setClass;
            grade = setGrade;
            level = setLevel;
            equipposition = setEquipPosition;
        }
    }

    public class log_rotate_rule
    {
        public string ObjectName { get; set; }
        public byte RuleType { get; set; }
    }

    public class mseed_item_log
    {
        public long log_idx { get; set; }
        public long aid { get; set; }
        public short class_type { get; set; }
        public long inven_seq { get; set; }
        public long item_id { get; set; }
        public long deleted_ea { get; set; }
        public short grade { get; set; }
        public short level { get; set; }
        public string equipposition { get; set; }
        public DateTime reg_date { get; set; }
    }
}
