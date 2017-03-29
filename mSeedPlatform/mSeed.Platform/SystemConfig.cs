using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mSeed.Common;

namespace mSeed.Platform
{
    public class auth_platform_info
    {
        public ePlatformType auth_type { get; set; }
        public string package_name { get; set; }
        public string app_id { get; set; }
        public string app_secret { get; set; }

        public auth_platform_info()
        {
            auth_type = ePlatformType.EPlatformType_UnityEditer;
            app_id = app_secret = "";
        }
        public auth_platform_info(ePlatformType setType, string setName, string setID, string setSecret)
        {
            package_name = setName;
            auth_type = setType;
            app_id = setID;
            app_secret = setSecret;
        }
    }

    public class SystemConfig
    {
        // singleton SystemConfig instance
        public static SystemConfig _sysconfig = new SystemConfig();
        /// <summary> get _sysconfig Instance </summary>
        public static SystemConfig GetSystemConfigInstance()
        {
            return _sysconfig;
        }

        private IniParser parser = null;
        public const string configInIFileName = @"ini\platform.ini";
        public string GoogleServiceAccount = string.Empty;
        public string GoogleServiceKeyFilePath = string.Empty;
        public string PhysicPath = string.Empty;
        public mDBTxnBlock.DBEndpoint platformDB = new mDBTxnBlock.DBEndpoint();
        public mDBTxnBlock.DBEndpoint couponDB = new mDBTxnBlock.DBEndpoint();

        //private Dictionary<string, List<string>> iniCategory = new Dictionary<string, List<string>>()
        //{
        //    { "Google", new List<string>() {"service_account", "p12_filepath"} },
        //};

        private SystemConfig()
        {
            PhysicPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
            string iniFile = System.IO.Path.Combine(PhysicPath, configInIFileName);
            parser = new IniParser(iniFile);

            GoogleServiceAccount = parser.GetSetting("Google", "service_account");
            GoogleServiceKeyFilePath = parser.GetSetting("Google", "p12_filepath");
            GoogleServiceKeyFilePath = System.IO.Path.Combine(PhysicPath, GoogleServiceKeyFilePath);

            couponDB.Host = parser.GetSetting("CouponDB", "host");
            couponDB.Database = parser.GetSetting("CouponDB", "db");
            couponDB.UserID = parser.GetSetting("CouponDB", "id");
            couponDB.UserPW = parser.GetSetting("CouponDB", "pw");
            couponDB.SetDBAlias = parser.GetSetting("CouponDB", "db_alias");

            platformDB.Host = parser.GetSetting("PlatformDB", "host");
            platformDB.Database = parser.GetSetting("PlatformDB", "db");
            platformDB.UserID = parser.GetSetting("PlatformDB", "id");
            platformDB.UserPW = parser.GetSetting("PlatformDB", "pw");
            platformDB.SetDBAlias = parser.GetSetting("PlatformDB", "db_alias");

            mSeed.Common.mLogger.mLogger.GetLoggerInstance().LogfileName = @"log\platform_log";
        }
    }
}
