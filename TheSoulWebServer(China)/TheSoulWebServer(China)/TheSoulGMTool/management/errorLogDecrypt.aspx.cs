using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Tools;
using TheSoul.DataManager.Global;
using TheSoulWebServer.Tools;
using TheSoulGMTool.DBClass;

namespace TheSoulGMTool.management
{
    public partial class errorLogDecrypt : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

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
                if (jsonData["result"] == "1")
                    TheSoulEncrypt.CompressionDecrypt(jsonData["returndata"], setKey, ref decryptString);
                else
                    TheSoulEncrypt.DecryptData(jsonData["returndata"], setKey, ref decryptString);
                decrypt.Text = decryptString;
            }

        }
    }
}