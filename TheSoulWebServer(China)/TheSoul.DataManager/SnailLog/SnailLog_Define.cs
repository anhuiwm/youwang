using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSoul.DataManager
{
    public static class SnailLog_Define
    {
        public const string _CurrentUser_Log_tablename = "_CurrentUser_Log";
        public const string game_player_action_log_tablename = "_snail_game_player_action_log";
        public const string role_log_tablename = "_snail_role_log";
        public const string role_additional_log_tablename = "_snail_role_additional_log";
        public const string task_log_tablename = "_snail_task_log";
        public const string money_log_tablename = "_snail_money_log";
        public const string item_log_tablename = "_snail_item_log";
        public const string role_upgrade_log_tablename = "_snail_role_upgrade_log";
        public const string instance_log_tablename = "_snail_instance_log";
        public const string program_log_tablename = "_snail_program_log";
        public const string scene_log_tablename = "_snail_scene_log";
        public const string mseed_item_log_tablename = "mseed_item_log";

        public const long Snail_s_id_Seperator_Operator         = 1000000000;       // Operator
        public const long Snail_s_id_Seperator_event            = 0000000000;       // achiveid + value or event_id
        public const long Snail_s_id_Seperator_item_idx         = 0000000000;       // item_base id
        public const long Snail_s_id_Seperator_achive           = 1100000000;       // achiveid + value or event_id
        public const long Snail_s_id_Seperator_achive_pvp       = 1200000000;       // achiveid + value or event_id
        public const long Snail_s_id_Seperator_pve_stage        = 1310000000;       // pve_mission_stage
        public const long Snail_s_id_Seperator_pve_dark         = 1320000000;       // pve_dark_passgae_dungeon_id
        public const long Snail_s_id_Seperator_pve_elite        = 1330000000;       // pve_elite_dungeon_id
        public const long Snail_s_id_Seperator_pve_boss         = 1340000000;       // pve_boss_raid_dungeon_id
        public const long Snail_s_id_Seperator_pve_party        = 1350000000;       // pve_party_dungeon_id
        public const long Snail_s_id_Seperator_shop_item        = 1400000000;       // shop_goods_id
        public const long Snail_s_id_Seperator_shop_package     = 1500000000;       // shop_package_id
        //public const long Snail_s_id_Seperator_daily_event      = 1800000000;       // + daily count
        //public const long Snail_s_id_Seperator_firstpay_event   = 1900000000;       // first pay event
        //public const long Snail_s_id_Seperator_shop_package     = 3100000000;       // shop_package_id
        //public const long Snail_s_id_Seperator_shop_gacha       = 3200000000;       // shop_gacha_id
        //public const long Snail_s_id_Seperator_shop_free_gacha  = 3210000000;       // shop_free_gacha_id
        //public const long Snail_s_id_Seperator_shop_reset       = 3300000000;       // shop_reset
        //public const long Snail_s_id_Seperator_shop_soul_diss   = 3400000000;       // shop_soul_diss
        //public const long Snail_s_id_Seperator_pve_rank_reward  = 4200000000;       // pve_rank_reward
        //public const long Snail_s_id_Seperator_pve_revival      = 4300000000;       // pve_revival
        //public const long Snail_s_id_Seperator_pvp_reset_stage  = 4410000000;       // pve_count_reset
        //public const long Snail_s_id_Seperator_pvp_reset_dark   = 4420000000;       // pve_count_reset
        ////public const long Snail_s_id_Seperator_pvp_reset_elite  = 4430000000;       // pve_count_reset
        //public const long Snail_s_id_Seperator_pvp_type         = 5000000000;       // pvp_play type
        
        public enum Log_Rotate_Type
        {
            None = 0,
            Day = 1,
            Week = 2,
            Month = 3,
        }

        public enum Snail_Task_Act_type
        {
            start = 0,
            success = 1,
            fail = 2,
        }

        public enum Snail_Money_type
        {
            ruby = 0,
            gold = 1,
        }
        
        public enum Snail_Money_Event_type
        {
            add = 0,
            use = 1,
        }

        public enum SnailLogKey
        {
            //set write/update op
            write_game_player_action_log,
            write_role_log,
            update_role_log,
            renew_role_log,
            write_task_log,
            write_money_log,
            write_item_log,
            write_role_upgrade_log,
            write_instance_log,
            write_program_log,
            write_scene_log,
            mseed_item_log,

            // in game param
            op,
            aid,
            cid,
            add_time,
            trigger_id_list,
            money_log_list,
            item_log_list,

            // snail param
            s_account,
            s_create_acc,
            n_vip_level,
            s_role,
            s_ip,
            d_logintime,
            d_logouttime,
            s_mac,
            s_os_type,
            s_comment,
            s_task_id,
            s_role_name,
            n_act_type,
            d_create,
            s_buy_account,
            s_buy_role,
            n_money_type,
            s_event_id,
            n_event_type,
            s_item_id,
            s_instance_id,
            n_count,
            n_money,
            s_recv_account,
            s_recv_role,
            s_scene_id,
            s_guild_id,
            s_state,
            n_role_level,
            n_total_logtime,
            n_before,
            n_after,
            n_id,
            d_time,
            n_logtype,
            n_level_before,
            n_level_after,
            s_skill_id,
            s_instanceid,
            s_instancetypeid,
            d_starttime,
            d_endtime,
            n_duration,
            n_troop_count1,
            n_troop_count2,
            s_programid,
            s_programname,
            d_opentime,
            d_date,
            n_enter,
            n_totaltime,
            s_act_id,
        }

        public static readonly Dictionary<SnailLogKey, string> SetLogKey = new Dictionary<SnailLogKey, string>()
        {
            { SnailLogKey.write_game_player_action_log, "write_game_player_action_log" },
            { SnailLogKey.write_role_log, "write_role_log" },
            { SnailLogKey.update_role_log,"update_role_log" },
            { SnailLogKey.renew_role_log,"renew_role_log" },
            { SnailLogKey.write_task_log,"write_task_log" },
            { SnailLogKey.write_money_log,"write_money_log" },
            { SnailLogKey.write_item_log,"write_item_log" },
            { SnailLogKey.mseed_item_log,"mseed_item_log" },
            
            { SnailLogKey.write_role_upgrade_log,"write_role_upgrade_log" },
            { SnailLogKey.write_instance_log,"write_instance_log" },
            { SnailLogKey.write_program_log,"write_program_log" },
            { SnailLogKey.write_scene_log,"write_scene_log" },

            { SnailLogKey.op, "op" },
            { SnailLogKey.aid, "aid" },
            { SnailLogKey.cid, "cid" },
            { SnailLogKey.add_time, "add_time" },
            { SnailLogKey.trigger_id_list, "trigger_id_list" },
            { SnailLogKey.money_log_list, "money_log_list" },
            { SnailLogKey.item_log_list, "item_log_list" },

            { SnailLogKey.s_account,"s_account" },
            { SnailLogKey.s_create_acc,"s_create_acc" },
            { SnailLogKey.n_vip_level,"n_vip_level" },            
            { SnailLogKey.s_role,"s_role" },
            { SnailLogKey.s_ip,"s_ip" },
            { SnailLogKey.d_logintime,"d_logintime" },
            { SnailLogKey.d_logouttime,"d_logouttime" },
            { SnailLogKey.s_mac,"s_mac" },
            { SnailLogKey.s_os_type,"s_os_type" },
            { SnailLogKey.s_comment,"s_comment" },
            { SnailLogKey.s_task_id,"s_task_id" },
            { SnailLogKey.s_role_name,"s_role_name" },
            { SnailLogKey.n_act_type,"n_act_type" },
            { SnailLogKey.d_create,"d_create" },
            { SnailLogKey.s_buy_account,"s_buy_account" },
            { SnailLogKey.s_buy_role,"s_buy_role" },
            { SnailLogKey.n_money_type,"n_money_type" },
            { SnailLogKey.s_event_id,"s_event_id" },
            { SnailLogKey.n_event_type,"n_event_type" },
            { SnailLogKey.s_item_id,"s_item_id" },
            { SnailLogKey.s_instance_id,"s_instance_id" },
            { SnailLogKey.n_count,"n_count" },
            { SnailLogKey.n_money,"n_money" },
            { SnailLogKey.s_recv_account,"s_recv_account" },
            { SnailLogKey.s_recv_role,"s_recv_role" },
            { SnailLogKey.s_scene_id,"s_scene_id" },
            { SnailLogKey.s_guild_id,"s_guild_id" },
            { SnailLogKey.s_state,"s_state" },
            { SnailLogKey.n_role_level,"n_role_level" },
            { SnailLogKey.n_total_logtime,"n_total_logtime" },
            { SnailLogKey.n_before,"n_before" },
            { SnailLogKey.n_after,"n_after" },
            { SnailLogKey.n_id,"n_id" },
            { SnailLogKey.d_time,"d_time" },
            { SnailLogKey.n_logtype,"n_logtype" },
            { SnailLogKey.n_level_before,"n_level_before" },
            { SnailLogKey.n_level_after,"n_level_after" },
            { SnailLogKey.s_skill_id,"s_skill_id" },
            { SnailLogKey.s_instanceid,"s_instanceid" },
            { SnailLogKey.s_instancetypeid,"s_instancetypeid" },
            { SnailLogKey.d_starttime,"d_starttime" },
            { SnailLogKey.d_endtime,"d_endtime" },
            { SnailLogKey.n_duration,"n_duration" },
            { SnailLogKey.n_troop_count1,"n_troop_count1" },
            { SnailLogKey.n_troop_count2,"n_troop_count2" },
            { SnailLogKey.s_programid,"s_programid" },
            { SnailLogKey.s_programname,"s_programname" },
            { SnailLogKey.d_opentime,"d_opentime" },
            { SnailLogKey.d_date,"d_date" },
            { SnailLogKey.n_enter,"n_enter" },
            { SnailLogKey.n_totaltime,"n_totaltime" },
            { SnailLogKey.s_act_id,"s_act_id" },
        };

        public static readonly Dictionary<string, SnailLogKey> GetLogKey = new Dictionary<string, SnailLogKey>()
        {
            { "write_game_player_action_log" ,SnailLogKey.write_game_player_action_log },
            { "write_role_log", SnailLogKey.write_role_log },
            { "update_role_log", SnailLogKey.update_role_log },
            { "write_task_log", SnailLogKey.write_task_log },
            { "write_money_log", SnailLogKey.write_money_log },
            { "write_item_log", SnailLogKey.write_item_log },
            { "write_role_upgrade_log", SnailLogKey.write_role_upgrade_log },
            { "write_instance_log", SnailLogKey.write_instance_log },
            { "write_program_log", SnailLogKey.write_program_log },
            { "write_scene_log", SnailLogKey.write_scene_log },
            { "mseed_item_log", SnailLogKey.mseed_item_log },

            { "op", SnailLogKey.op },
            { "aid", SnailLogKey.aid },
            { "cid", SnailLogKey.cid },
            { "add_time", SnailLogKey.add_time },
            { "trigger_id_list", SnailLogKey.trigger_id_list },
            { "money_log_list", SnailLogKey.money_log_list },
            { "item_log_list", SnailLogKey.item_log_list },

            { "s_account", SnailLogKey.s_account },
            { "s_create_acc", SnailLogKey.s_create_acc },
            { "n_vip_level", SnailLogKey.n_vip_level },
            
            { "s_role", SnailLogKey.s_role },
            { "s_ip", SnailLogKey.s_ip },
            { "d_logintime", SnailLogKey.d_logintime },
            { "d_logouttime", SnailLogKey.d_logouttime },
            { "s_mac", SnailLogKey.s_mac },
            { "s_os_type", SnailLogKey.s_os_type },
            { "s_comment", SnailLogKey.s_comment },
            { "s_task_id", SnailLogKey.s_task_id },
            { "s_role_name", SnailLogKey.s_role_name },
            { "n_act_type", SnailLogKey.n_act_type },
            { "d_create", SnailLogKey.d_create },
            { "s_buy_account", SnailLogKey.s_buy_account },
            { "s_buy_role", SnailLogKey.s_buy_role },
            { "n_money_type", SnailLogKey.n_money_type },
            { "s_event_id", SnailLogKey.s_event_id },
            { "n_event_type", SnailLogKey.n_event_type },
            { "s_item_id", SnailLogKey.s_item_id },
            { "s_instance_id", SnailLogKey.s_instance_id },
            { "n_count", SnailLogKey.n_count },
            { "n_money", SnailLogKey.n_money },
            { "s_recv_account", SnailLogKey.s_recv_account },
            { "s_recv_role", SnailLogKey.s_recv_role },
            { "s_scene_id", SnailLogKey.s_scene_id },
            { "s_guild_id", SnailLogKey.s_guild_id },
            { "s_state", SnailLogKey.s_state },
            { "n_role_level", SnailLogKey.n_role_level },
            { "n_total_logtime", SnailLogKey.n_total_logtime },
            { "n_before", SnailLogKey.n_before },
            { "n_after", SnailLogKey.n_after },
            { "n_id", SnailLogKey.n_id },
            { "d_time", SnailLogKey.d_time },
            { "n_logtype", SnailLogKey.n_logtype },
            { "n_level_before", SnailLogKey.n_level_before },
            { "n_level_after", SnailLogKey.n_level_after },
            { "s_skill_id", SnailLogKey.s_skill_id },
            { "s_instanceid", SnailLogKey.s_instanceid },
            { "s_instancetypeid", SnailLogKey.s_instancetypeid },
            { "d_starttime", SnailLogKey.d_starttime },
            { "d_endtime", SnailLogKey.d_endtime },
            { "n_duration", SnailLogKey.n_duration },
            { "n_troop_count1", SnailLogKey.n_troop_count1 },
            { "n_troop_count2", SnailLogKey.n_troop_count2 },
            { "s_programid", SnailLogKey.s_programid },
            { "s_programname", SnailLogKey.s_programname },
            { "d_opentime", SnailLogKey.d_opentime },
            { "d_date", SnailLogKey.d_date },
            { "n_enter", SnailLogKey.n_enter },
            { "n_totaltime", SnailLogKey.n_totaltime },
            { "s_act_id", SnailLogKey.s_act_id },
        };

        public static readonly Dictionary<string, Operation_SID> GetOperationSID = new Dictionary<string, Operation_SID>()
        {
            { "createaccount", Operation_SID.createaccount },
            { "createcharacter", Operation_SID.createcharacter },
            { "login", Operation_SID.login },
            { "create_new_character", Operation_SID.create_new_character },
            { "refresh_account", Operation_SID.refresh_account },
            { "change_character_group", Operation_SID.change_character_group },
            { "chat_ignore_list", Operation_SID.chat_ignore_list },
            { "chat_ignore_add", Operation_SID.chat_ignore_add },
            { "chat_ignore_remove", Operation_SID.chat_ignore_remove },
            { "check_refresh", Operation_SID.check_refresh },
            { "townuser_list", Operation_SID.townuser_list },
            { "tutorial_set", Operation_SID.tutorial_set },
            { "tutorial_end", Operation_SID.tutorial_end },
            { "check", Operation_SID.check },
            { "activecheck", Operation_SID.activecheck },
            { "list", Operation_SID.list },
            { "detail", Operation_SID.detail },
            { "enter", Operation_SID.enter },
            { "result", Operation_SID.result },
            { "reward", Operation_SID.reward },
            { "event_type_list", Operation_SID.event_type_list },
            { "event_list", Operation_SID.event_list },
            { "event_daily_count", Operation_SID.event_daily_count },
            { "event_daily_count_buy", Operation_SID.event_daily_count_buy },
            { "achieve_list", Operation_SID.achieve_list },
            { "get_event_reward", Operation_SID.get_event_reward },
            { "get_achive_reward", Operation_SID.get_achive_reward },
            { "get_first_pay_reward", Operation_SID.get_first_pay_reward },
            { "get_user_event_info", Operation_SID.get_user_event_info },
            { "pvp_achieve_list", Operation_SID.pvp_achieve_list },
            { "get_pvp_achive_reward", Operation_SID.get_pvp_achive_reward },
            { "event_7day_list", Operation_SID.event_7day_list },
            { "get_event_7day_reward", Operation_SID.get_event_7day_reward },
            { "buy_event_7day_package", Operation_SID.buy_event_7day_package },
            { "my_list", Operation_SID.my_list },
            { "delete", Operation_SID.delete },
            { "sendgift", Operation_SID.sendgift },
            { "sendgift_all", Operation_SID.sendgift_all },
            { "detail_info", Operation_SID.detail_info },
            { "firend_set_pvp", Operation_SID.firend_set_pvp },
            { "friend_request_list", Operation_SID.friend_request_list },
            { "friend_recommand_list", Operation_SID.friend_recommand_list },
            { "friend_recommand_list_refresh", Operation_SID.friend_recommand_list_refresh },
            { "accept_friend", Operation_SID.accept_friend },
            { "decline_friend", Operation_SID.decline_friend },
            { "invite_friend", Operation_SID.invite_friend },
            { "search_friend", Operation_SID.search_friend },
            { "lobby", Operation_SID.lobby },
            { "stage", Operation_SID.stage },
            { "resetge", Operation_SID.resetge },
            { "playstart", Operation_SID.playstart },
            { "playresult", Operation_SID.playresult },
            { "set_ge_group", Operation_SID.set_ge_group },
            { "shoplist", Operation_SID.shoplist },
            { "shopreset", Operation_SID.shopreset },
            { "guild_create", Operation_SID.guild_create },
            { "guild_name_check", Operation_SID.guild_name_check },
            { "guild_info", Operation_SID.guild_info },
            { "guild_managed", Operation_SID.guild_managed },
            { "guild_join", Operation_SID.guild_join },
            { "guild_recommend", Operation_SID.guild_recommend },
            { "guild_recommend_refresh", Operation_SID.guild_recommend_refresh },
            { "guild_joiner_state_change", Operation_SID.guild_joiner_state_change },
            { "guild_state_change", Operation_SID.guild_state_change },
            { "guild_entrust_ask", Operation_SID.guild_entrust_ask },
            { "guild_entrust_check", Operation_SID.guild_entrust_check },
            { "guild_entrust_reply", Operation_SID.guild_entrust_reply },
            { "guild_daily_mission_reward", Operation_SID.guild_daily_mission_reward },
            { "getinven", Operation_SID.getinven },
            { "equipitem", Operation_SID.equipitem },
            { "unequipitem", Operation_SID.unequipitem },
            { "sellitem", Operation_SID.sellitem },
            { "enchantarmor", Operation_SID.enchantarmor },
            { "evolutionarmor", Operation_SID.evolutionarmor },
            { "metalworkarmor", Operation_SID.metalworkarmor },
            { "enchantweapon", Operation_SID.enchantweapon },
            { "refiningoption", Operation_SID.refiningoption },
            { "getoptiondetail", Operation_SID.getoptiondetail },
            { "disassemble", Operation_SID.disassemble },
            { "disassemble_info", Operation_SID.disassemble_info },
            { "optionadd", Operation_SID.optionadd },
            { "optionadd_reroll", Operation_SID.optionadd_reroll },
            { "option_allchange", Operation_SID.option_allchange },
            { "useitem", Operation_SID.useitem },
            { "enchantcape", Operation_SID.enchantcape },
            { "mail_list", Operation_SID.mail_list },
            { "mail_detail", Operation_SID.mail_detail },
            { "mail_open", Operation_SID.mail_open },
            { "mission_modeinfo", Operation_SID.mission_modeinfo },
            { "mission_taskinfo", Operation_SID.mission_taskinfo },
            { "mission_start", Operation_SID.mission_start },
            { "mission_result_sweep", Operation_SID.mission_result_sweep },
            { "mission_result", Operation_SID.mission_result },
            { "mission_rank_reward", Operation_SID.mission_rank_reward },
            { "pve_revival", Operation_SID.pve_revival },
            { "pve_count_reset", Operation_SID.pve_count_reset },
            { "dark_passage_start", Operation_SID.dark_passage_start },
            { "dark_passage_result", Operation_SID.dark_passage_result },
            { "elite_modeinfo", Operation_SID.elite_modeinfo },
            { "elite_start", Operation_SID.elite_start },
            { "elite_result", Operation_SID.elite_result },
            { "overlord_list", Operation_SID.overlord_list },
            { "overlord_top_rank", Operation_SID.overlord_top_rank },
            { "get_targetinfo", Operation_SID.get_targetinfo },
            { "server_config", Operation_SID.server_config },
            { "get_user_aid", Operation_SID.get_user_aid },
            { "billing_progress", Operation_SID.billing_progress },
            { "change_billing_id", Operation_SID.change_billing_id },
            { "use_account_ticket", Operation_SID.use_account_ticket },
            { "use_account_ruby", Operation_SID.use_account_ruby },
            { "add_account_point", Operation_SID.add_account_point },
            { "use_account_point", Operation_SID.use_account_point },
            { "party_dungeon_result", Operation_SID.party_dungeon_result },
            { "add_guild_point", Operation_SID.add_guild_point },
            { "overlord_result", Operation_SID.overlord_result },
            { "account_info", Operation_SID.account_info },
            { "character_list", Operation_SID.character_list },
            { "character_info", Operation_SID.character_info },
            { "character_group", Operation_SID.character_group },
            { "equip_item_list", Operation_SID.equip_item_list },
            { "equip_soul_list", Operation_SID.equip_soul_list },
            { "equip_active_soul_list", Operation_SID.equip_active_soul_list },
            { "equip_passive_soul_list", Operation_SID.equip_passive_soul_list },
            { "pvp_info", Operation_SID.pvp_info },
            { "use_contributionpoint", Operation_SID.use_contributionpoint },
            { "addcontributionpoint", Operation_SID.addcontributionpoint },
            { "pvp_count_reset", Operation_SID.pvp_count_reset },
            { "pvp_count_info", Operation_SID.pvp_count_info },
            { "pvp_open_time", Operation_SID.pvp_open_time },
            { "trigger_progress", Operation_SID.trigger_progress },
            { "item_make", Operation_SID.item_make },
            { "send_mail", Operation_SID.send_mail },
            { "reg_snail_id", Operation_SID.reg_snail_id },
            { "get_warpoint", Operation_SID.get_warpoint },
            { "set_warpoint", Operation_SID.set_warpoint },
            { "get_system_data", Operation_SID.get_system_data },
            { "get_pvp_record", Operation_SID.get_pvp_record },
            { "get_pvp_record_all", Operation_SID.get_pvp_record_all },
            { "set_pvp_count", Operation_SID.set_pvp_count },
            { "set_pvp_record", Operation_SID.set_pvp_record },
            { "pvp_guildwar_join", Operation_SID.pvp_guildwar_join },
            { "get_ruby_pvp_player_info", Operation_SID.get_ruby_pvp_player_info },
            { "redis_flush", Operation_SID.redis_flush },
            { "set_chatchannel", Operation_SID.set_chatchannel },
            { "pvp_rank", Operation_SID.pvp_rank },
            { "pvp_rank_lastweek", Operation_SID.pvp_rank_lastweek },
            { "friend_rank", Operation_SID.friend_rank },
            { "guildwarpvp_rank", Operation_SID.guildwarpvp_rank },
            { "guildwarpvp_rank_lastweek", Operation_SID.guildwarpvp_rank_lastweek },
            { "character_rank", Operation_SID.character_rank },
            { "guild_rank", Operation_SID.guild_rank },
            { "draw_gacha", Operation_SID.draw_gacha },
            { "free_gacha", Operation_SID.free_gacha },
            { "free_premium_gacha", Operation_SID.free_premium_gacha },
            { "shop_list", Operation_SID.shop_list },
            { "shop_package_list", Operation_SID.shop_package_list },
            { "buy_package", Operation_SID.buy_package },
            { "buy_shop_item", Operation_SID.buy_shop_item },
            { "buy_billing_cash", Operation_SID.buy_billing_cash },
            { "shop_buy_count_reset", Operation_SID.shop_buy_count_reset },
            { "blackmarket_soul_disassemble", Operation_SID.blackmarket_soul_disassemble },
            { "get_vip_reward", Operation_SID.get_vip_reward },
            { "aosbilling_success", Operation_SID.aosbilling_success },
            { "getsoullist", Operation_SID.getsoullist },
            { "equipsoul_list", Operation_SID.equipsoul_list },
            { "equip_soul", Operation_SID.equip_soul },
            { "equip_soul_new", Operation_SID.equip_soul_new },
            { "soul_activation", Operation_SID.soul_activation },
            { "active_soul_levelup", Operation_SID.active_soul_levelup },
            { "active_soul_gradeup", Operation_SID.active_soul_gradeup },
            { "active_soul_starup", Operation_SID.active_soul_starup },
            { "active_soul_skill_set", Operation_SID.active_soul_skill_set },
            { "buy_soul_slot", Operation_SID.buy_soul_slot },
            { "soul_equipitemlist", Operation_SID.soul_equipitemlist },
            { "soul_equipitem_to_soul", Operation_SID.soul_equipitem_to_soul },
            { "soul_equip_craft", Operation_SID.soul_equip_craft },
            { "passive_soul_create", Operation_SID.passive_soul_create },
            { "passive_soul_extract", Operation_SID.passive_soul_extract },
            { "passive_soul_levelup", Operation_SID.passive_soul_levelup },
            { "passive_soul_reserve", Operation_SID.passive_soul_reserve },
        };

        public static long Operation_To_S_Event_ID(Operation_SID setID)
        {
            return (long)setID + SnailLog_Define.Snail_s_id_Seperator_Operator;
        }

        public enum Operation_SID
        {
//RequestAccount 
                createaccount = 1,
                createcharacter,
                login,
                create_new_character,
                refresh_account,
                change_character_group,

                // chat method
                chat_ignore_list,
                chat_ignore_add,
                chat_ignore_remove,

//RequestBackground 
                check_refresh,
                townuser_list,
                tutorial_set,
                tutorial_end,                

//RequestBossRaid                 
                check,
                activecheck,
                list,
                detail,
                enter,
                result,
                reward,
//RequestEvent
                event_type_list,
                event_list,
                event_daily_count,
                event_daily_count_buy,
                achieve_list,
                get_event_reward,
                get_achive_reward,
                get_first_pay_reward,

                get_user_event_info,

                pvp_achieve_list,
                get_pvp_achive_reward,

                event_7day_list,
                get_event_7day_reward,
                buy_event_7day_package,
//RequestFriends 
                // 1 depth my friend list view op
                my_list,
                delete,
                sendgift,
                sendgift_all,
                detail_info,
                firend_set_pvp,

                // 2 depth find friend view op
                friend_request_list,
                friend_recommand_list,
                friend_recommand_list_refresh,
                accept_friend,
                decline_friend,
                invite_friend,
                search_friend,

//RequestGE
                lobby,                // 황금원정단 로비 : 나의 황금원정단, 상대방 매칭 정보
                stage,                // 황금원정단 스테이지
                resetge,             // 원정단 리셋 (다시 하기)
                playstart,            // 원정단 시작,진입
                playresult,           // 원정단 결과,보상
                set_ge_group,           // 원정단 캐릭터 순서 

                shoplist,             // 원정단 상점 정보 표시
                shopreset,            // 원정단 상점 초기화

//RequestGuild 
                guild_create,
                guild_name_check,
                guild_info,
                guild_managed,
                guild_join,
                guild_recommend,
                guild_recommend_refresh,
                guild_joiner_state_change,
                guild_state_change,
                guild_entrust_ask,
                guild_entrust_check,
                guild_entrust_reply,
                guild_daily_mission_reward,

//RequestItem
                getinven,
                equipitem,
                unequipitem,
                sellitem,

                // armor
                enchantarmor,
                evolutionarmor,
                metalworkarmor,

                // weapon
                enchantweapon,
                refiningoption,
                getoptiondetail,
                disassemble,
                disassemble_info,

                // accessory
                optionadd,
                optionadd_reroll,
                option_allchange,

                // cape
                useitem,
                enchantcape,

//RequestMail
                mail_list,
                mail_detail,
                mail_open,

//RequestMission
                mission_modeinfo,
                mission_taskinfo,
                mission_start,
                mission_result_sweep,
                mission_result,
                mission_rank_reward,
                pve_revival,
                pve_count_reset,

                // dark passage
                dark_passage_start,
                dark_passage_result,
                //dark_passage_result_sweep,      // not use yet (contents closed)

                // elite pve
                elite_modeinfo,
                elite_start,
                elite_result,

//RequestOverlord
                overlord_list,
                overlord_top_rank,
                get_targetinfo, //대전 상대 리스트를 선택하여 대전이 가능한 경우, 상대의 소지 캐릭터 정보, 장착된 아이템을 클라이언트가 요청함.
                overlord_result,


//RequestPrivateServer 
                server_config,
                get_user_aid,

                billing_progress,
                change_billing_id,

                use_account_ticket,
                use_account_ruby,
                add_account_point,
                use_account_point,
                party_dungeon_result,
                add_guild_point,                

                account_info,
                character_list,
                character_info,
                character_group,
                equip_item_list,
                equip_soul_list,
                equip_active_soul_list,
                equip_passive_soul_list,
                pvp_info,
                use_contributionpoint,
                addcontributionpoint,
                
                //reset count
                pvp_count_reset,
                pvp_count_info,

                // pvp opentime
                pvp_open_time,

                // trigger_progress
                trigger_progress,

                // make item or mail requeset
                item_make,
                send_mail,

                // regist snail UID
                reg_snail_id,

                // chracter warpoint
                get_warpoint,
                set_warpoint,

                // load system data 
                get_system_data,

                // pvp operation
                get_pvp_record,
                get_pvp_record_all,
                set_pvp_count,
                set_pvp_record,

                pvp_guildwar_join,

                get_ruby_pvp_player_info,
                redis_flush,
                set_chatchannel,                

//RequestPvP                
                // all PvP
                pvp_rank,
                pvp_rank_lastweek,
                friend_rank,
                
                
                //// 1vs1 PvP   // not use yet. instead all pvp rank use op=pvp_rank
                //1vs1pvp_rank,
                //1vs1pvp_rank_lastweek,

                //guildwar PvP
                guildwarpvp_rank,
                guildwarpvp_rank_lastweek,

                // charater ranking - not pvp
                character_rank,

                // guild ranking - not pvp
                guild_rank,

                // pvp open time list
                get_pvp_open,
//RequestShop
                draw_gacha,
                free_gacha,
                free_premium_gacha,

                shop_list,
                shop_package_list,
                buy_package,
                buy_shop_item,    

                buy_billing_cash,     // billing

                shop_buy_count_reset,
                blackmarket_soul_disassemble,

                get_vip_reward,
                aosbilling_success,

//RequestSoul                
                // soul 공통
                getsoullist,
                equipsoul_list,
                equip_soul,
                equip_soul_new,

                // active soul
                soul_activation,
                active_soul_levelup,
                active_soul_gradeup,
                active_soul_starup,
                active_soul_skill_set,
                buy_soul_slot,

                // active soul equip
                soul_equipitemlist,
                soul_equipitem_to_soul,
                soul_equip_craft,

                // passive soul
                passive_soul_create,
                passive_soul_extract,
                passive_soul_levelup,
                passive_soul_reserve,
        }

        public enum PvPOperationSID : long
        {
            MATCH_FREE = 110001 + Snail_s_id_Seperator_Operator,
            MATCH_1VS1 = 110002 + Snail_s_id_Seperator_Operator,
            MATCH_GUILD_3VS3 = 110003 + Snail_s_id_Seperator_Operator,
            MATCH_RUBY_PVP = 110004 + Snail_s_id_Seperator_Operator,
            MATCH_PARTY = 110006 + Snail_s_id_Seperator_Operator,
            MATCH_OVERLORD = 110007 + Snail_s_id_Seperator_Operator,

            // result sid
            MATCH_FREE_RESULT = 111001 + Snail_s_id_Seperator_Operator,
            MATCH_1VS1_RESULT = 111002 + Snail_s_id_Seperator_Operator,
            MATCH_GUILD_3VS3_RESULT = 111003 + Snail_s_id_Seperator_Operator,
            MATCH_RUBY_PVP_RESULT = 111004 + Snail_s_id_Seperator_Operator,
            MATCH_PARTY_RESULT = 111006 + Snail_s_id_Seperator_Operator,
            MATCH_OVERLORD_RESULT = 111007 + Snail_s_id_Seperator_Operator,
        }
        
        public enum GachaOperationSID : long
        {
            NORMAL_TRY_ONE = 210151 + Snail_s_id_Seperator_Operator,
            NORMAL_TRY_TEN = 210152 + Snail_s_id_Seperator_Operator,
            PREMIUM_TRY_ONE = 210153 + Snail_s_id_Seperator_Operator,
            PREMIUM_TRY_TEN = 210154 + Snail_s_id_Seperator_Operator,
            BEST_TRY_ONE = 210155 + Snail_s_id_Seperator_Operator,

            FREE_NORMAL_TRY_ONE = 220151 + Snail_s_id_Seperator_Operator,
            FREE_PREMIUM_TRY_ONE = 220153 + Snail_s_id_Seperator_Operator,

            TREASURE_BOX_GOLD = 220154 + Snail_s_id_Seperator_Operator,
            TREASURE_BOX_CASH = 220155 + Snail_s_id_Seperator_Operator,
            TREASURE_BOX_SPECIAL = 220156 + Snail_s_id_Seperator_Operator,
        }

        public enum ShopResetOperationSID : long
        {
//            RubyShop = 310151 + Snail_s_id_Seperator_Operator,
            Guild = 310152 + Snail_s_id_Seperator_Operator,
            Expedition = 310153 + Snail_s_id_Seperator_Operator,
            PvP_1vs1 = 310154 + Snail_s_id_Seperator_Operator,
            PvP_FreeForAll = 310155 + Snail_s_id_Seperator_Operator,
            Party = 310156 + Snail_s_id_Seperator_Operator,
            Ranking = 310157 + Snail_s_id_Seperator_Operator,
            BlackMarket = 310158 + Snail_s_id_Seperator_Operator,
        }

    }
}
