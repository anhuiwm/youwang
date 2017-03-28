using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

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
using ServiceStack.Text;

namespace TheSoulCheatServer
{
    public class test
    {
        long a1 { get; set; }
        long a2 { get; set; }
        string a3 { get; set; }
        string a4 { get; set; }
        long a5 { get; set; }
        long a6 { get; set; }
        string data { get; set; }
        string result { get; set; }
        string setkey { get; set; }
        DateTime regdate { get; set; }
        string json { get; set; }

    }
    public partial class csvLogDecrypt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] allLines = File.ReadAllLines(@"D:\gamedb1.csv");
            List<test> list = new List<test>();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (string line in allLines)
            {
                string[] data = line.Split(',');

                string encryptData = data[6] +","+ data[7]; // 6 : json data, 7 : result data
                string setKey = data[8].Replace("encryptKey=", ""); // key data
                Dictionary<string, string> jsonData = mJsonSerializer.JsonToDictionary(encryptData);
                string decryptString = "";
                if (jsonData["result"] == "1")
                    TheSoulEncrypt.CompressionDecrypt(jsonData["returndata"], setKey, ref decryptString);
                else
                    TheSoulEncrypt.DecryptData(jsonData["returndata"], setKey, ref decryptString);
                //getitem
                Dictionary<string, string> decryptJson = mJsonSerializer.JsonToDictionary(decryptString);
                //JsonObject setJson = JsonObject.Parse(decryptString);
                string getitemJson = "";
                //setJson.TryGetValue("getitem", out getitemJson);
                //setJson = JsonObject.Parse(getitemJson);
                List<User_Inven> setItems = mJsonSerializer.JsonToObject<List<User_Inven>>(getitemJson);
                sb.Append(line);
                sb.Append(",");
                foreach (User_Inven item in setItems)
                {
                    sb.Append("," + item.itemid);
                }
                sb.Append("\r\n");
                            
            }
            
            string lang = System.Web.HttpContext.Current.Request.UserLanguages[0];
            string charSet = "utf-8";
            lang = lang.Split(new char[] { ';' })[0];
            if (lang == "zh-CN")
                charSet = "GB2312";
            else
                charSet = "euc-kr";

            string filename = "attachment;filename=gamedb1_de.csv";
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = charSet;
            Response.ContentEncoding = System.Text.Encoding.GetEncoding(charSet);
            Response.AddHeader("content-disposition", filename);
            Response.ContentType = "text/csv";

            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();

            Response.Write("aa");
        }
    }
}