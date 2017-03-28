using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using mSeed.mDBTxnBlock;
using mSeed.RedisManager;
using TheSoulWebServer.Tools;
using TheSoul.DataManager.Global;
using TheSoul.DataManager.DBClass;
using TheSoulGMTool.DBClass;

namespace TheSoulGMTool
{
    /// <summary>
    /// WebService의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {

        [WebMethod]
        public void HelloWorld()
        {
            //return "Hello World";
            Context.Response.Output.Write("Hello World");
        }

        [WebMethod]
        public string SetLanguage(string data_lang)
        {
            string result = "";
            bool queryResult = false;
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, 1);
                    string web_lang = GMDataManager.GetGMToolWebLanguage(ref tb, data_lang);
                    GMDataManager.SetUserCookiesLanguage(data_lang, web_lang);
                    GMDataManager.SetLanguage(web_lang);
                }
                catch (Exception e)
                {
                    queryResult = false;
                    result = e.Message;
                }
                tb.EndTransaction(queryResult);
                tb.Dispose();
            }
            return result;
        }

        [WebMethod]
        public System_TriggerType GetTrigerDesc(string trigerType)
        {
            System_TriggerType result = null;
            bool queryResult = false;
            WebQueryParam queryFetcher = new WebQueryParam(true);
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, 1);
                    result =  GMDataManager.GetSystemTriggerType(ref tb, trigerType);
                    queryResult = true;
                }
                catch (Exception e)
                {
                    Console.Write(e);
                    queryResult = false;
                    result = new System_TriggerType();
                }
                tb.EndTransaction(queryResult);
                tb.Dispose();
            }
            return result;
        }

        [WebMethod]
        public String GetEventServerCheck(long EventID, string checked_Server)
        {
            string result = "";
            bool queryResult = false;
            Dictionary<long, TxnBlock> TxnBlackServer = new Dictionary<long, TxnBlock>();
            string[] reqServerList = System.Text.RegularExpressions.Regex.Split(checked_Server, ",");
            foreach (string Key in reqServerList)
            {
                long ServerKey = System.Convert.ToInt64(Key);
                if (!TxnBlackServer.ContainsKey(ServerKey))
                {
                    TxnBlock tb = new TxnBlock();
                    TheSoulDBcon.GetInstance().TheSoulDBInitFromGlobal(ref tb, (int)ServerKey, true);
                    TxnBlackServer.Add(ServerKey, tb);
                }
            }

            try
            {
                TxnBlock tb = TxnBlackServer.First().Value;
                List<server_group_config> serverGourpList = GlobalManager.GetServerGroupList(ref tb);
                int eventCount = 0;
                foreach (KeyValuePair<long, TxnBlock> TB in TxnBlackServer)
                {
                    tb = TB.Value;
                    System_Event dataInfo = GMDataManager.GetSystem_EventData(ref tb, EventID);
                    if (dataInfo.Event_ID == EventID)
                    {
                        string serverName = serverGourpList.Find(item => item.server_group_id == TB.Key) == null ? TB.Key.ToString() : serverGourpList.Find(item => item.server_group_id == TB.Key).server_group_name;
                        result = string.IsNullOrEmpty(result) ? serverName : string.Format("{0}, {1}", result, serverName);
                        eventCount++;
                    }
                }
                if (eventCount == 0)
                    result = "이벤트 ID로 진행하시겠습니까?";
                else
                    result = "[" + result + "] 서버에 " + EventID + " 아이디가 있습니다.\n계속 진행 시 " + EventID + "이벤트가 수정됩니다.\n진행 하시겠습니까?";
                queryResult = true;
            }
            catch (Exception e)
            {
                Console.Write(e);
                queryResult = false;
                result = "Error";
            }
            finally
            {
                foreach (KeyValuePair<long, TxnBlock> setItem in TxnBlackServer)
                {
                    setItem.Value.EndTransaction(queryResult);
                    setItem.Value.Dispose();
                }
            }
            return result;
        }
    }
}
