using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mSeed.Common;

namespace mSeed.Platform
{
    public static class DB_Define
    {
        public static readonly Dictionary<eDBTables, string> DBTables = new Dictionary<eDBTables, string>()
        {
            { eDBTables.game_access_auth,           "game_access_auth" },
            { eDBTables.game_service,               "game_service" },
            { eDBTables.game_service_info,          "game_service_info" },
            { eDBTables.game_service_product_id,    "game_service_product_id" },
            { eDBTables.user_account,               "user_account" },
            { eDBTables.user_account_restrict,      "user_account_restrict" },
            { eDBTables.user_billing_authkey,       "user_billing_authkey" },
            { eDBTables.user_billing_list,          "user_billing_list" },
            { eDBTables.user_guest_auth_id,         "user_guest_auth_id" },
            { eDBTables.user_play_game,             "user_play_game" },
            { eDBTables.user_push_token,            "user_push_token" },
            { eDBTables.system_error_code,          "system_error_code" },
            { eDBTables.system_push_service,          "system_push_service" },
        };

        public enum eDBTables
        {
            game_access_auth = 100,
            game_service,
            game_service_info,
            game_service_product_id,
            system_push_service,

            user_account = 200,
            user_account_restrict,
            user_billing_authkey,
            user_billing_list,
            user_guest_auth_id,
            user_play_game,
            user_push_token,

            system_error_code = 999,
        }
    }

    public class game_access_auth
    {
        public long api_access_id { get; set; }
        public long game_service_id { get; set; }
        public int access_level { get; set; }
        public string access_auth_key { get; set; }
        public DateTime reg_date { get; set; }
    }

    public class GameAccessAuth : game_access_auth
    {
        public DateTime ExpireTime;

        public GameAccessAuth()
        {
            ExpireTime = DateTime.Now;
        }

        public GameAccessAuth(game_access_auth setinfo)
        {
            if (setinfo != null)
            {
                api_access_id = setinfo.api_access_id;
                game_service_id = setinfo.game_service_id;
                access_level = setinfo.access_level;
                access_auth_key = setinfo.access_auth_key;
                reg_date = setinfo.reg_date;
                ExpireTime = DateTime.Now;
            }            
        }
    }

    public class game_service
    {
        public long game_service_id { get; set; }
        public string service_name { get; set; }
        public string ssl_certificate_path { get; set; }
        public string push_certificate_path { get; set; }
        public DateTime reg_date { get; set; }
    }

    public class game_service_info
    {
        public long game_service_id { get; set; }
        public int service_type { get; set; }
        public byte service_status { get; set; }
        public string service_app_id { get; set; }
        public string service_secret { get; set; }
        public DateTime reg_date { get; set; }
    }

    public class GameServiceInfoStruct
    {
        public long game_service_id;
        public Dictionary<int, game_service_info> service_info_list;
        public DateTime ExpireTime;

        public GameServiceInfoStruct()
        {
        }

        public GameServiceInfoStruct(long setid)
        {
            game_service_id = setid;
            service_info_list = new Dictionary<int, game_service_info>();
        }
    }

    public class game_service_product_id
    {
        public long product_index { get; set; }
        public long game_service_id { get; set; }
        public int billing_platform_type { get; set; }
        public string product_id { get; set; }
        public int price_value { get; set; }
        public int price_tier { get; set; }
    }

    public class system_push_service
    {
        public long push_id { get; set; }
        public long game_service_id { get; set; }
        public byte push_type { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public byte push_status { get; set; }
        public string push_reason { get; set; }
        public DateTime send_reserv_date { get; set; }
        public string register { get; set; }
        public DateTime reg_date { get; set; }
    }

    public class ret_push_msg
    {
        public long push_id { get; set; }
        public long game_service_id { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public long token_count { get; set; }
        public byte push_type { get; set; }
        public string fcm_key { get; set; }
        public string apns_cert { get; set; }
    }

    public class push_msg_result
    {
        public List<ret_push_msg> push_msg_list { get; set; }
        public int error { get; set; }
    }

    public class user_account
    {
        public long user_id { get; set; }
        public int platform_type { get; set; }
        public string platform_user_id { get; set; }
        public int user_account_status { get; set; }
        public DateTime reg_date { get; set; }
    }

    public class user_account_restrict
    {
        public long user_id { get; set; }
        public DateTime login_restrict_enddate { get; set; }
        public DateTime login_restrict_reg_date { get; set; }
        public DateTime chat_restrict_endate { get; set; }
        public DateTime chat_restrict_reg_date { get; set; }
    }

    public class user_billing_authkey
    {
        public string billing_authkey { get; set; }
        public long user_id { get; set; }
        public int platform_type { get; set; }
        public string product_id { get; set; }
        public string payload_info { get; set; }
        public DateTime regdate { get; set; }
    }

    public class user_billing_list
    {
        public long billind_idx { get; set; }
        public long user_id { get; set; }
        public long game_service_id { get; set; }
        public string product_id { get; set; }
        public int price_value { get; set; }
        public int price_tier { get; set; }
        public string billing_authkey { get; set; }
        public string billing_token { get; set; }
        public int billing_platform_type { get; set; }
        public byte billing_status { get; set; }
        public DateTime regdate { get; set; }
    }

    public class user_guest_auth_id
    {
        public string auth_md5_id { get; set; }
        public string server_auth_token { get; set; }
        public string client_auth_token { get; set; }
        public string server_auth_md5 { get; set; }
        public string client_auth_md5 { get; set; }
        public DateTime reg_date { get; set; }

        public user_guest_auth_id() { }
        public user_guest_auth_id(string set_auth, string set_server, string set_client, string set_server_md5, string set_client_md5)
        {
            auth_md5_id = set_auth;
            server_auth_token = set_server;
            client_auth_token = set_client;
            server_auth_md5 = set_server_md5;
            client_auth_md5 = set_client_md5;
        }
    }

    public class user_play_game
    {
        public long user_id { get; set; }
        public long game_service_id { get; set; }
        public DateTime reg_date { get; set; }
    }

    public class user_push_token
    {
        public long token_idx { get; set; }
        public long user_id { get; set; }
        public long game_service_id { get; set; }
        public int service_type { get; set; }
        public string push_token { get; set; }
        public byte push_status { get; set; }
        public DateTime reg_date { get; set; }
    }

    public class ret_user_platform_id
    {
        public long platform_idx { get; set; }
        public string platform_user_id { get; set; }
        public int error { get; set; }
        public ret_user_platform_id() { error = (int)Result_Define.eResult.SUCCESS; }
        public ret_user_platform_id(long setidx, string setid)
        {
            platform_idx = setidx;
            platform_user_id = setid;
            error = (int)Result_Define.eResult.SUCCESS;
        }
    }

    public class ret_push_token
    {
        public long token_idx { get; set; }
        public long user_id { get; set; }
        public int service_type { get; set; }
        public string push_token { get; set; }
    }

    public class push_token_result
    {
        public List<ret_push_token> push_token_list { get; set; }
        public int error { get; set; }
    }
}
