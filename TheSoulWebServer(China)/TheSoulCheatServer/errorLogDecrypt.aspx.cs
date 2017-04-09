using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using mSeed.mDBTxnBlock;
using mSeed.RedisManager;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Global;
using TheSoulWebServer.Tools;
using TheSoulCheatServer.lib;
using System.Net.Json;

namespace TheSoulCheatServer
{
    public partial class errorLogDecrypt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam();
            queryFetcher.SetDebugMode = true;
            string encryptString = queryFetcher.QueryParam_Fetch(encryptdata.UniqueID, "");
            if (!string.IsNullOrEmpty(encryptString))
            {
                string[] encrypt = System.Text.RegularExpressions.Regex.Split(encryptString, ",encryptKey=");
                string setKey = encrypt[1];
                Dictionary<string, string> jsonData = mJsonSerializer.JsonToDictionary(encrypt[0]);
                string decryptString = "";
                if(jsonData["result"] == "1")
                    TheSoulEncrypt.CompressionDecrypt(jsonData["returndata"], setKey, ref decryptString);
                else
                    TheSoulEncrypt.DecryptData(jsonData["returndata"], setKey, ref decryptString);
                decrypt.Text = decryptString;
            }
        }
    }
}