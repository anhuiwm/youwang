using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using mSeed.mDBTxnBlock;
using mSeed.RedisManager;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Global;
using TheSoulWebServer.Tools;
using ServiceStack.Text;

using NAMU;

using System.Net;
using System.Net.Sockets;
using System.IO;

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml;
using System.Globalization;

namespace TheSoulWebServer
{
    public class testSt
    {
        public Account userAcc { get; set; }
        public Dictionary<long, Character> userChar { get; set; }
    }

    public class teststring
    {
        public string returndata { get; set; }
        public string value { get; set; }
    }

    public static class DateTimeExtensions
    {
        
    }

    public partial class WebTest : System.Web.UI.Page
    {
        bool setDebug = false;

        public void ErrorLog(string e)
        {
            if (setDebug)
                System.Web.HttpContext.Current.Response.Write(e + "\n<br>");
        }

        public void Render<T>(T obj)
        {
            //var jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //string json = jsonSerializer.Serialize(mJsonSerializer.ToJsonString(obj));
            string json = mJsonSerializer.ToJsonString(obj);
            Response.Write(json);
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<string> SerialNumberGenerator(int keyLength, int getCount)
        {
            Dictionary<string, string> keyList = new Dictionary<string, string>();
            int breakCount = 0;
            while (keyList.Count < getCount && breakCount < getCount)
            {
                string newSerialNumber = "";
                string SerialNumber = Guid.NewGuid().ToString("N").Substring(0, (int)keyLength).ToUpper();
                for (int iCount = 0; iCount < (int)keyLength; iCount += 4)
                    newSerialNumber = newSerialNumber + SerialNumber.Substring(iCount, 4) + "-";
                newSerialNumber = newSerialNumber.Substring(0, newSerialNumber.Length - 1);

                if (!keyList.ContainsKey(newSerialNumber))
                    keyList.Add(newSerialNumber, SerialNumber);
                else
                    breakCount++;
            }

            return keyList.Keys.ToList<string>();
        }

        public static string GetReqeustURL(string Url, string dataParams, bool doPost = true, string contentType = "application/x-www-form-urlencoded")
        {
            try
            {
                HttpWebRequest request;
                if (doPost)
                {
                    /* POST */
                    request = (HttpWebRequest)WebRequest.Create(Url);
                    request.Method = "POST";    // 기본값 "GET"
                    request.ContentType = contentType;

                    // request param to byte array for IO stream
                    byte[] byteDataParams = UTF8Encoding.UTF8.GetBytes(dataParams);
                    request.ContentLength = byteDataParams.Length;

                    // reqesut byte array write to IO stream
                    Stream stDataParams = request.GetRequestStream();
                    stDataParams.Write(byteDataParams, 0, byteDataParams.Length);
                    stDataParams.Close();
                }
                else
                {
                    /* GET */
                    request = (HttpWebRequest)WebRequest.Create(Url + "?" + dataParams);
                    request.Method = "GET";
                }

                // 요청, 응답 받기
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // 응답 Stream 읽기
                Stream stReadData = response.GetResponseStream();
                StreamReader srReadData = new StreamReader(stReadData, Encoding.Default);
                return srReadData.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        /*
08-11 11:42:51.174$ <color=#df7b02>[WEB>] Request/<b>SERVER_LIST</b></color> (op=serverlist, reqid=0)
http://211.253.28.132:20000/RequestGlobal.aspx?op=serverlist&platform_type=100&billing_type=1000&version=0&Debug=1
{
  "op" : "serverlist",
  "platform_type" : 100,
  "billing_type" : 1000,
  "version" : 0
}

08-11 11:43:03.508$ <color=#df7b02>[WEB>] Request/<b>LOGIN</b></color> (op=login, reqid=0)
http://211.253.28.132:11000/RequestAccount.aspx?op=login&aid=103&platform_type=100&billing_type=1000&version=0&s_mac=&s_os_type=0&reqid=0&Debug=1
{
  "op" : "login",
  "aid" : 103,
  "platform_type" : 100,
  "billing_type" : 1000,
  "version" : 0,
  "s_mac" : "",
  "s_os_type" : 0,
  "reqid" : 0
}

         */

        private static string SerialGenerate(int keyLength)
        {
            string newSerialNumber = "";
            string SerialNumber = Guid.NewGuid().ToString("N").Substring(0, keyLength).ToUpper();
            for (int iCount = 0; iCount < keyLength; iCount += 4)
                newSerialNumber = newSerialNumber + SerialNumber.Substring(iCount, 4) + "-";
            newSerialNumber = newSerialNumber.Substring(0, newSerialNumber.Length - 1);
            return newSerialNumber;
        }

        struct testStruct
        {
            public long tLong;
            public float tFloat;
            public string tString;

            public testStruct(string setString = "")
            {
                this.tLong = 0;
                this.tFloat = 0;
                this.tString = setString;
            }

            public override string ToString()
            {
                long mem = GC.GetTotalMemory(false);
                JsonObject retJson = new JsonObject();
                retJson.Add("tLong", tLong.ToString());
                retJson.Add("tFloat", tFloat.ToString());
                retJson.Add("tString", string.IsNullOrEmpty(tString) ? string.Empty : tString);
                long mem2= GC.GetTotalMemory(false);
                return retJson.ToJson();
            }

            public static testStruct Parse(string json)
            {
                while (json.Contains("\\"))
                {
                    json = json.Replace("\\", "");
                }

                JsonObject setJson = JsonObject.Parse(json);
                testStruct retObj = new testStruct();
                retObj.tLong = setJson.ContainsKey("tLong") ? long.Parse(setJson["tLong"]) : 0;
                retObj.tFloat = setJson.ContainsKey("tFloat") ? float.Parse(setJson["tFloat"]) : 0;
                retObj.tString = setJson.ContainsKey("tString") ? setJson["tString"] : string.Empty;
                return retObj;
            }
        }

        T testT<T>()
        {
            T retObj = default(T);
            return retObj;
        }

        public struct Size
        {
            public double Width { get; set; }
            public double Height { get; set; }

            public override string ToString()
            {
                return Width + "x" + Height;
            }

            public static Size Parse(string json)
            {
                var size = json.Split('x');
                return new Size
                {
                    Width = double.Parse(size[0]),
                    Height = double.Parse(size[1])
                };
            }
        }

        public static XmlDocument MakeRequest(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return (xmlDoc);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                Console.Read();
                return null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime startDate = DateTime.Parse("2016-01-01");
            DateTime checkDate = DateTime.Parse("2016-10-31");
            DateTime dbDate = DateTime.Parse("2016-11-01");

            int fristWeek = PvPManager.GetWeeks(startDate, checkDate);
            int secondWeek = PvPManager.GetWeeks(startDate, dbDate);

            bool bLoop = false;
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            System.Globalization.Calendar cal = dfi.Calendar;

            //int startWeek = cal.GetWeekOfYear(startDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            //int currentWeek = cal.GetWeekOfYear(dbDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            // firstweek set monday;
            int startWeek = cal.GetWeekOfYear(startDate, dfi.CalendarWeekRule, DayOfWeek.Monday);
            int currentWeek = cal.GetWeekOfYear(dbDate, dfi.CalendarWeekRule, DayOfWeek.Monday);

            if (startWeek != currentWeek || startDate.Year != dbDate.Year || startDate.Month != dbDate.Month)
                bLoop = true;
            return;

            List<String> pw = SerialNumberGenerator(16, 5);
            Response.Write(mJsonSerializer.ToJsonString(pw));
            return;

            //DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            //System.Globalization.Calendar cal = dfi.Calendar;

            //DateTime startDate = DateTime.Parse("2016-09-24");
            //DateTime dbDate = DateTime.Parse("2016-09-25");

            //int startWeek = cal.GetWeekOfYear(startDate, dfi.CalendarWeekRule, DayOfWeek.Monday);
            //int currentWeek = cal.GetWeekOfYear(dbDate, dfi.CalendarWeekRule, DayOfWeek.Monday);


            //string timeString = GenericFetch.GetServerTimeString();
            //Response.Write(timeString + "<br>");

            //DateTime ConvertTime = GenericFetch.GetTimeStringToDateTime(timeString);
            //Response.Write(ConvertTime + "<br>");

            //TimeZone zone = TimeZone.CurrentTimeZone;
            //TimeSpan offset = zone.GetUtcOffset(DateTime.Now);

            //// Demonstrate ToLocalTime and ToUniversalTime.
            //DateTime local = zone.ToLocalTime(DateTime.Now);
            //DateTime universal = zone.ToUniversalTime(DateTime.Now);
            //Response.Write(zone.DaylightName + "," + zone.StandardName + "<br>");
            //Response.Write(offset + "<br>");
            //Response.Write(DateTime.Now + "<br>");
            //Response.Write(local + "<br>");
            //Response.Write(universal + "<br>");
            //return;

            //            TxnBlock tb = new TxnBlock();

            //TheSoulDBcon.GetInstance().TheSoulDBInitFromGlobal(ref tb);

            //TriggerManager.CheckClearTrigger(ref tb, 103, Trigger_Define.eTriggerType.Soul_Lv, 1, 0, 1);

            //return;
            List<string> setURL = new List<string>();
            setURL.Add("http://211.253.28.52:11000/RequestPrivateServer.aspx");
            setURL.Add("http://211.253.28.38:11000/RequestPrivateServer.aspx");
            setURL.Add("http://211.253.28.237:11000/RequestPrivateServer.aspx");
            setURL.Add("http://211.253.28.167:11000/RequestPrivateServer.aspx");

            setURL.Add("http://211.253.30.40:11000/RequestPrivateServer.aspx");
            setURL.Add("http://211.253.30.37:11000/RequestPrivateServer.aspx");
            setURL.Add("http://211.253.28.56:11000/RequestPrivateServer.aspx");
            setURL.Add("http://211.253.28.51:11000/RequestPrivateServer.aspx");

            StringBuilder dataParams = new StringBuilder();
            dataParams.Append("op=redis_flush");
            dataParams.Append("&aid=3");
            dataParams.Append("&serverkey=ranking");

            foreach (string setURI in setURL)
            {
                string ret = "";
                ret = GetReqeustURL(setURI, dataParams.ToString(), false);
                Response.Write(ret + "<br>");
            }

            return;
            //long mem = GC.GetTotalMemory(false);
            //testStruct temp = testT<testStruct>();
            //temp.tLong = 1;
            //temp.tFloat = 0.5f;
            //temp.tString = "ts";
            //string tempjson = mJsonSerializer.ToJsonString(temp);
            //long mem2 = GC.GetTotalMemory(false);
            //JsonObject testJson = JsonObject.Parse(tempjson);
            //testStruct tempDe = mJsonSerializer.JsonToObject<testStruct>(tempjson);
            //if (temp.tLong != 0)
            //    Console.WriteLine("what");
            //else
            //    Console.WriteLine("fine");
            //long mem3 = GC.GetTotalMemory(false);

            //Size testSize = new Size();
            //testSize.Width = 1;
            //string json = mJsonSerializer.ToJsonString(testSize);

            //long mem4 = GC.GetTotalMemory(false);
            //XmlDocument xmlDoc = MakeRequest("google.com");

            //XmlDocument doc = new XmlDocument();
            //doc.PreserveWhitespace = true;
            //try { doc.Load("booksData.xml"); }
            //catch (System.IO.FileNotFoundException)
            //{
            //    doc.LoadXml("<?xml version=\"1.0\"?> \n" +
            //    "<books xmlns=\"http://www.contoso.com/books\"> \n" +
            //    "  <book genre=\"novel\" ISBN=\"1-861001-57-8\" publicationdate=\"1823-01-28\"> \n" +
            //    "    <title>Pride And Prejudice</title> \n" +
            //    "    <price>24.95</price> \n" +
            //    "  </book> \n" +
            //    "  <book genre=\"novel\" ISBN=\"1-861002-30-1\" publicationdate=\"1985-01-01\"> \n" +
            //    "    <title>The Handmaid's Tale</title> \n" +
            //    "    <price>29.95</price> \n" +
            //    "  </book> \n" +
            //    "</books>");
            //}

            //XmlNodeList nodes = doc.GetElementsByTagName("book", "http://www.contoso.com/books");

            //foreach (XmlNode node in nodes)
            //{
            //    Response.Write(string.Format("{0}: {1} <br>",
            //                        node.Attributes["genre"].InnerText,
            //                        node.Attributes["ISBN"].InnerText));
            //    foreach (XmlNode child in node.ChildNodes)
            //    {
            //        Response.Write(string.Format("{0} <br>",
            //                            child.InnerText
            //                            ));
            //    }

            //}  

            //// Create a new XmlDocument  
            //XmlDocument doc = new XmlDocument();

            //// Load data  
            //doc.Load("d:\booksData.xml");

            //// Set up namespace manager for XPath  
            //XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            //ns.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

            //// Get forecast with XPath  
            //XmlNodeList nodes = doc.SelectNodes("/rss/channel/item/yweather:forecast", ns);

            //// You can also get elements based on their tag name and namespace,  
            //// though this isn't recommended  
            ////XmlNodeList nodes = doc.GetElementsByTagName("forecast",   
            ////                          "http://xml.weather.yahoo.com/ns/rss/1.0");  

            //foreach (XmlNode node in nodes)
            //{
            //    Console.WriteLine("{0}: {1}, {2}F - {3}F",
            //                        node.Attributes["day"].InnerText,
            //                        node.Attributes["text"].InnerText,
            //                        node.Attributes["low"].InnerText,
            //                        node.Attributes["high"].InnerText);
            //}  
            //return;
            //TheSoul.DataManager.TCP_Packet.TCP_GM_Operation.LogOutAllUser("10.1.1.23", 10200, TheSoul.DataManager.TCP_Packet.TCP_GM_Operation.GM_CS_OPERATION.GM_CS_OP_LOGOUT_ALL);
            //string responseBody = "";
            //const string global_get_aid_url = "http://211.253.14.138:11000/RequestPrivateServer.aspx";
            //const string game_create_account_url = "http://211.253.14.138:11000/RequestAccount.aspx";
            ////const string game_create_character_url = "http://localhost:2680/RequestAccount.aspx";
            //const string game_create_character_url = "http://211.253.14.138:11000/RequestAccount.aspx";
            //long AID = 0;
            //JsonObject getJson;
            //int retError = 0;

            //for (int i = 0; i < 3500; i++)
            //{
            //    string user_id = string.Format("{0}_{1}", SerialGenerate(16), i);
            //    StringBuilder dataParams = new StringBuilder();
            //    dataParams.Append("op=get_user_aid");
            //    dataParams.Append("&platform_type=");
            //    dataParams.Append((int)Global_Define.ePlatformType.EPlatformType_UnityEditer);
            //    dataParams.Append("&billing_type=");
            //    dataParams.Append((int)Shop_Define.eBillingType.UnityDebug);
            //    dataParams.Append("&user_id=");
            //    dataParams.Append(user_id);
            //    dataParams.Append("&version=0");
            //    responseBody = GetReqeustURL(global_get_aid_url, dataParams.ToString());
            //    getJson = JsonObject.Parse(responseBody);

            //    if (long.TryParse(getJson["aid"], out AID))
            //    {
            //        if (AID > 0)
            //        {
            //            dataParams.Clear();
            //            dataParams.Append("op=createaccount");
            //            dataParams.Append("&aid=");
            //            dataParams.Append(AID);
            //            dataParams.Append("&platform_type=");
            //            dataParams.Append((int)Global_Define.ePlatformType.EPlatformType_UnityEditer);
            //            dataParams.Append("&billing_type=");
            //            dataParams.Append((int)Shop_Define.eBillingType.UnityDebug);
            //            dataParams.Append("&userid=");
            //            dataParams.Append(user_id);
            //            dataParams.Append("&username=");
            //            dataParams.Append("bot_" + AID);
            //            dataParams.Append("&cc=kr");
            //            dataParams.Append("&languagecode=0");
            //            dataParams.Append("&Debug=1");
            //            responseBody = GetReqeustURL(game_create_account_url, dataParams.ToString());
            //            getJson = JsonObject.Parse(responseBody);
            //            if (int.TryParse(getJson["resultcode"], out retError))
            //            {
            //                if (retError == 0)
            //                {
            //                    dataParams.Clear();
            //                    dataParams.Append("op=create_test_character");
            //                    dataParams.Append("&aid=");
            //                    dataParams.Append(AID);
            //                    dataParams.Append("&class=");
            //                    dataParams.Append(TheSoul.DataManager.Math.GetRandomInt(1,2));
            //                    dataParams.Append("&level=");
            //                    dataParams.Append(TheSoul.DataManager.Math.GetRandomInt(88, 90));
            //                    dataParams.Append("&Debug=1");
            //                    responseBody = GetReqeustURL(game_create_character_url, dataParams.ToString());
            //                    //Response.Write(responseBody);
            //                }
            //            }
            //        }
            //    }
            //}

            /*
            string enc = "jVquLlvx4FhQpg1Qmj7HUe6BkrHSpQirOHkkEM5I6j4p4jO+L3kBdtFpjlHMNInRDQK//9lvDTjF9Ubk9ooW9QmWAjESVYADjWcvKVwYSFlkDyJgyNpEa2irw8qFfNYWW91clDfIVBKm1juh4eTJurvhreqXKAwUjlQcbBvJWS0=";
            string setKey = "Sy4yS4erGZz4gyr053rtznBhQP9z1Im/xmsl78ydO7g=";

KgALN8vcZ4Jzcd+MSuNsAmDJcKzS91tlwZt4HjH7XRk=
v500QbMjCzn/6R+40cfLkqVmOMu3bWB79p6T0g5+0oA=
Sy4yS4erGZz4gyr053rtznBhQP9z1Im/xmsl78ydO7g=

             08-09 19:49:18.281$ EncryptJson() key=v500QbMjCzn/6R+40cfLkqVmOMu3bWB79p6T0g5+0oA=, iv=ToN80QS/DWc/EkzT8AUapg==
{"op":"login","aid":13,"platform_type":100,"billing_type":1000,"version":0,"s_mac":"","s_os_type":0,"reqid":0}

vhz7+igO5qCiVrQtUsVlbBaEUckw5SvzQ5ieN2uQut2MflZ5rNQ4/WE8b8JnNYow2XDW9ERnxfsHTQF7xZvLjpB255wGjRTiiL/eHD86gU3xUNBttxmh6soq6725BXs6EKYCtLcObjKbId1iNdaOug==

UnityEngine.Debug:Log(Object, Object)
Log:Internal_Log(Int32, Object, Object) (at Assets/Script/UTIL/Log.cs:224)
Log:Format(String, Object[]) (at Assets/Script/UTIL/Log.cs:292)
HttpManager:EncryptJson(JSONObject, Boolean) (at Assets/Script/NET/WWW/HttpManager.cs:676)
HttpManager:CreatePOST(WebRequest) (at Assets/Script/NET/WWW/HttpManager.cs:370)
<CoSendWithRetryCheck>c__Iterator100:MoveNext() (at Assets/Script/NET/WWW/HttpManager.cs:431)
UnityEngine.MonoBehaviour:StartCoroutine(IEnumerator)
HttpManager:send2(WebRequest) (at Assets/Script/NET/WWW/HttpManager.cs:253)
UINetManager:RequestLogin(Int64) (at Assets/Script/NET/WWW/UINetManager.cs:176)
LoginControl:FinishSDKLogin() (at Assets/Script/UI/Login/LoginControl.cs:344)
NetClient:OnScMseedLogin(scMSEEDLogin) (at Assets/Script/NET/Session/NetClient.cs:6106)
NetClient:ProcCommand(Packet) (at Assets/Script/NET/Session/NetClient.cs:1164)
NetClient:ProcessChunk(Chunk) (at Assets/Script/NET/Session/NetClient.cs:827)
NetClient:FixedUpdate() (at Assets/Script/NET/Session/NetClient.cs:742)

08-09 19:49:18.337$ <color=#df7b02>[WEB] <b>Error</<b>CryptographicException</b></color>: <color=red>Bad PKCS7 padding. Invalid length 183.</color> (op=login, reqid=0, elapsed=0.057)
{"returndata":"jVquLlvx4FhQpg1Qmj7HUe6BkrHSpQirOHkkEM5I6j4p4jO+L3kBdtFpjlHMNInRDQK//9lvDTjF9Ubk9ooW9QmWAjESVYADjWcvKVwYSFlkDyJgyNpEa2irw8qFfNYWW91clDfIVBKm1juh4eTJurvhreqXKAwUjlQcbBvJWS0=","result":0}

UnityEngine.Debug:Log(Object, Object)
Log:Internal_Log(Int32, Object, Object) (at Assets/Script/UTIL/Log.cs:224)
Log:Format(String, Object[]) (at Assets/Script/UTIL/Log.cs:292)
HttpManager:LogHttpError(InternalWebRequest, String, String, String) (at Assets/Script/NET/WWW/HttpManager.cs:229)
HttpManager:LogHttpError(InternalWebRequest, Exception, String) (at Assets/Script/NET/WWW/HttpManager.cs:219)
HttpManager:UnpackHttpResult(InternalWebRequest) (at Assets/Script/NET/WWW/HttpManager.cs:565)
<CoSendWithRetryCheck>c__Iterator100:MoveNext() (at Assets/Script/NET/WWW/HttpManager.cs:460)

            string DecryptString = "";
            TheSoulEncrypt.DecryptData(enc, setKey, ref DecryptString);
             */

            //if (isCompress)
            //    TheSoulEncrypt.CompressionDecrypt(setVal, setKey, ref DecryptString);
            //else
            //    TheSoulEncrypt.DecryptData(setVal, setKey, ref DecryptString);

            //WebQueryParam queryFetcher = new WebQueryParam(true);

            //string username = queryFetcher.QueryParam_Fetch("nickname");


        //    Response.Write(SerialNumberGenerator(32, 1000000).Count);
        //    //Response.Write(mJsonSerializer.ToJsonString(keyList));

            //return;
            //WebQueryParam queryFetcher = new WebQueryParam(true);
            //TxnBlock tb = new TxnBlock();

            //TheSoulDBcon.GetInstance().TheSoulDBInitFromGlobal(ref tb);
            //string authKey = queryFetcher.QueryParam_Fetch("authkey");
            //string userlogin = queryFetcher.QueryParam_Fetch("userlogin");

            //if (!string.IsNullOrEmpty(authKey))
            //{
            //    string id = GlobalManager.GetUserPlatformID(ref tb, authKey, Global_Define.ePlatformType.EPlatformType_Google);
            //    Response.Write(id);
            //}
            //else if (!string.IsNullOrEmpty(userlogin))
            //{
            //    string webBody = GlobalManager.GetGooglePlusAccessToken();
            //    Response.Write(webBody);
            //    GlobalManager._GooglePlusAccessToken = null;

            //}
            //else
            //{
            //    string serviceAccountID = "817162163610-compute@developer.gserviceaccount.com";
            //    string keyFilePath = "D:\\Work\\1_Dark_Blaze\\google_auth_key\\Google Play Android Developer-27d86138bd13.p12";
            //    string[] setScope = { GoogleJsonWebToken.SCOPE_USER_EMAIL, GoogleJsonWebToken.SCOPE_USER_INFO, GoogleJsonWebToken.SCOPE_openid, "https://www.googleapis.com/auth/androidpublisher" };
            //    var auth = GoogleJsonWebToken.GetAccessToken(
            //                    serviceAccountID,
            //                    keyFilePath,
            //                    setScope);
            //    Response.Write(mJsonSerializer.ToJsonString(auth));
            //}

            //string purchaseToken = "{\"orderId\":\"1299912121692121212121.1320812112121\",\"packageName\":\"com.test.runrunrun\",\"productId\":\"com.test.runrunrun.item_001\",\"purchaseTime\":1393377621833,\"purchaseState\":0,\"purchaseToken\":\"sekadakadaaxgjhsrLn61wGoyAl..._8-l8zsUhVpY7o7Zq08s\"}";
            //GoogleJsonWebToken.GoogleIABVerify(purchaseToken);

            //return;

            //for (int i = 0; i < 100; i++)
            //{                
            //    string token1 = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            //    string token2 = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            //    using (MD5 md5Hash = MD5.Create())
            //    {
            //        string source = token1 + token2;
            //        string hash = GetMd5Hash(md5Hash, source);
            //        string result = "The MD5 hash of " + source + " is: " + hash + ".";

            //        if (VerifyMd5Hash(md5Hash, source, hash))
            //        {
            //            result = "The hashes are the same.";
            //        }
            //        else
            //        {
            //            result = "The hashes are not same.";
            //        }
            //    }
            //}
            //return;
            //SocketServer setServer = new SocketServer();
            //setServer.ListenBind();

            //TCPTestServer setTestServer = new TCPTestServer();
            //setTestServer.ListenBind();

            //TCTSocketTestClient setTestClient = new TCTSocketTestClient();
            //setTestClient.Connect();
            //setTestClient.SendMsg("test11$");
            //setTestClient.SendMsg("longtest 1231587a9sdfjl$");
            //Socket client;
            //client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //client.Connect(IPAddress.Parse("210.122.11.197"), 10200);

            //byte[] buffer = 
            //{
            //    0x70, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            //    0xcc, 0x00, 0x00, 0x00, 0x34, 0x12, 0x00, 0x00,
            //    0x17, 0x12, 0x12, 0x12, 0x7f, 0x12, 0x66, 0x12,
            //    0x77, 0x12, 0x61, 0x12, 0x66, 0x12, 0x27, 0x12,
            //    0x12, 0x12, 0x8b, 0x33, 0xd2, 0x8b, 0xc7, 0x35,
            //    0x13, 0x12, 0x12, 0x12, 0x12, 0x12, 0x12, 0x12,
            //    0x12, 0x12, 0x12, 0x12, 0xfa, 0x11, 0x12, 0x12,
            //    0x12, 0x12, 0x12, 0x12, 0x47, 0x12, 0x5c, 0x12,
            //    0x5b, 0x12, 0x46, 0x12, 0x4b, 0x12, 0x4d, 0x12,
            //    0x27, 0x12, 0x4d, 0x12, 0x23, 0x12, 0x20, 0x12,
            //    0x21, 0x12, 0x20, 0x12, 0x22, 0x12, 0x22, 0x12,
            //    0x27, 0x12, 0x26, 0x12, 0x12, 0x12, 0x1b, 0x12,
            //    0x41, 0x12, 0x77, 0x12, 0x7e, 0x12, 0x74, 0x12,
            //    0xce, 0xd9, 0xdd, 0x0c, 0x12, 0x12, 0x12, 0x12,
            //    0x03, 0x12, 0x12, 0x12, 0x83, 0xa5, 0x83, 0xa5,
            //    0x32, 0x12, 0x27, 0x12, 0x3a, 0xde, 0x32, 0x12,
            //    0x7a, 0x12, 0x73, 0x12, 0x7c, 0x12, 0x76, 0x12,
            //    0x32, 0x12, 0x23, 0x12, 0x23, 0xd3, 0x32, 0x12,
            //    0x23, 0x12, 0x1a, 0xaa, 0xba, 0xae, 0x12, 0x12,
            //    0xce, 0xd9, 0xdd, 0x0c, 0x12, 0x12, 0x12, 0x12,
            //    0xce, 0xd9, 0xdd, 0x0c, 0x12, 0x12, 0x12, 0x12,
            //    0x01, 0x12, 0x12, 0x12, 0x7b, 0x12, 0x7c, 0x12,
            //    0x74, 0x12, 0x7d, 0x12, 0x4d, 0x12, 0x71, 0x12,
            //    0x7a, 0x12, 0x73, 0x12, 0x60, 0x12, 0x73, 0x12,
            //    0x71, 0x12, 0x66, 0x12, 0x77, 0x12, 0x60, 0x12,
            //    0x4d, 0x12, 0x7f, 0x12, 0x73, 0x12, 0x7b, 0x12,
            //    0x7c, 0x12, 0x12, 0x12, 0x5b, 0x12, 0x42, 0x12,
            //    0xce, 0xd9, 0xdd, 0x0c
            //};

            ////byte[] buffer2 = 
            ////{
            ////    0x1b, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            ////    0x04, 0x00, 0x00, 0x00, 0x34, 0x12, 0x00, 0x00,
            ////    0x2a, 0x12, 0x12, 0x12  
            ////};
            //client.Send(buffer);

            //buffer = new byte[1024];
            //while (true)
            //{
            //    int len;
            //    try
            //    {
            //        len = client.Receive(buffer);
            //        if (len > 0)
            //        {
            //            string received = System.Text.Encoding.ASCII.GetString(buffer);
            //            break;
            //        }
            //    }
            //    catch (SocketException exc)
            //    {
            //        break;
            //    }
            //}

            //client.Disconnect(false);

            //DateTime chkTime = GenericFetch.StartOfWeek(DateTime.Now, DayOfWeek.Monday);
            //string days = chkTime.ToString("yyyyMM01");
            //Render<string>(days);

            //WebQueryParam queryFetcher = new WebQueryParam(true);
            //string setVal = "Z4lbtqbhn+wIG1slWRRf4F0KBy9oXvCUdMEYM0NFmggX7DYlfsaU4n/wLwt0dMIes9goR4munH0LrLW8SACy93Bqnck3HrFKVRGan6qgDxtpwuAhhg7E9sQass3MId7siKSjAvw6s2U+09OvtKgBtouVFu7PuiphBfUoXKOXbaBeQmoTUqHYulFWysI002EjIVssmCrsbFr11NMnoNV/6n2KCgYIma1gfv9gciuy1reBPwksTBMsFDHnr4Rkc46eh0OyhBT7yEcHub2aB9CXg1k6EFD/c22VI3ql258CVvl1lyKLrSCYk21u+L738bBzHhYkPB/Lt8Y+77CNA5Otkguy2nL0YPREOrLTPsaimlHtu9Q1FQ5Ax3BhDZBs95dRul9WGk5bJnSyuFLQehYvWpJqi0PG4oEmO7GVGoB0FVAxDZ0RsMnxEHTaKOa7sWX4uvwddH64hCOqz+Gv+Gcz/ho/s68Rgr6KLcyoaKdwPoInnIseriRT005mPeksJ3vacU+Qvx+eoyzzXJOCsq+i0JnQW+SF+vw2aUSvAYQlVpxpS3FoHSChMU+0spQ541aG2FdWOEAg/I5z2U1wWFm1gtmJnfTV9k8GJKSPgCgyAbY+0RsLusfbLyN1lqylerIT7wuiDZ/E/5h8XpUElOCPh0phiC1RJ+x2C3MhAC79i6XWnbNqNz2Voclqy9j8/eXIkPwpS3bRvAvH55OPrcUsjrczsnHqIplZ8xi5w4aoGHMuZVIOZYLqyBIOe2OOJHBGxBHwZJFYbWbYFtwpPXavFntrVDyuFh4hlzAocCxAENCQd3EtF1dtLbisIoO7dCsM1KW2Vyf4czM6haKvst+WLuXNa1xl90zi7HMx0LCL4AD3O1qWvPbXTA6AamUtYRzfHgymrUQdegD7FzIAb7bSAGDx01x0yg4y+D5iddQN2I5dl8p+raNFvPviEHPGn0l84LdXplxc7l0l3FQCDLQ6Uo0jsnW+HKjSXfds1eJQpnqqGW4lgtniu/1Y1t2qEq0FV/ZhqDqW59wsDme8DYWrXk+72h8xTnkGxNeSfqGxV3fzh/6YagU5L4N8qLudrCaOOneG0ApmRvYVzuav9ozTXXxc8HP/dHWxbkJnfspgILywWxOf8glUJvWrs7er3YABffIjRfSxDan1wIff2F1OP/XTzCzdX8JsXjdsrKv/JsI=";
            //string setKey = "PqWuPXw0q3kEa/VE9JIF/lEc1MZVJikLUNdiwNRY/JM=";
            //bool isCompress = true;

            //string DecryptString = "";

            //if (isCompress)
            //    TheSoulEncrypt.CompressionDecrypt(setVal, setKey, ref DecryptString);
            //else
            //    TheSoulEncrypt.DecryptData(setVal, setKey, ref DecryptString);

            //JsonObject setJson = JsonObject.Parse(DecryptString);
            //string getitemJson = "";
            //setJson.TryGetValue("getitem", out getitemJson);
            //setJson = JsonObject.Parse(getitemJson);
            //List<User_Inven> setItems = mJsonSerializer.JsonToObject <List<User_Inven>>(getitemJson);

            //Response.Write(DecryptString);


            
            //WebQueryParam queryFetcher = new WebQueryParam(true);
            ////Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            //TxnBlock tb = new TxnBlock();
            
            //TheSoulDBcon.GetInstance().TheSoulDBInitFromGlobal(ref tb);
            //long brank = queryFetcher.QueryParam_FetchLong("brank", 0);
            //long arank = queryFetcher.QueryParam_FetchLong("arank", 0);

            //long reward = PvPManager.CaclHighGradeReward(ref tb, brank, arank);

            //JsonObject json = new JsonObject();
            //json = mJsonSerializer.AddJson(json, "beforeRank", mJsonSerializer.ToJsonString(brank));
            //json = mJsonSerializer.AddJson(json, "afterRank", mJsonSerializer.ToJsonString(arank));
            //json = mJsonSerializer.AddJson(json, "reward", mJsonSerializer.ToJsonString(reward));
            //Response.Write(json.ToJson());
            //tb.EndTransaction();
            //tb.Dispose();
        }
    }
}