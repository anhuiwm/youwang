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
    public class Request_Log
    {
        public long log_idx { get; set; }
        public long AID { get; set; }
        public long CID { get; set; }
        public int ErrorCode { get; set; }
        public string RequestURL { get; set; }
        public string Operation { get; set; }
        public string RequestParams { get; set; }
        public string ResponseResult { get; set; }
        public string BaseJson { get; set; }
        public string DetailDBLog { get; set; }
        public DateTime requesttime { get; set; }
        public DateTime regdate { get; set; }
    }

    public partial class errorLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam();
            string retJson = "";
            queryFetcher.SetDebugMode = true;

            TxnBlock tb = new TxnBlock();
            {
                long AID = 0;
                try
                {
                    EncryptKey encryptKey = new EncryptKey();
                    queryFetcher.TxnBlockInit(ref tb, ref AID);
                    queryFetcher.GlobalDBOpen(ref tb);
                    queryFetcher.LogDBOpen(ref tb);
                    string Operation = queryFetcher.QueryParam_Fetch("op", "");
                    int errorCode = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("errorcode", "0"));
                    if (AID > 0 && (!string.IsNullOrEmpty(Operation) || errorCode > 0))
                    {
                        string condition1 = "";
                        string condition2 = "";
                        if(!string.IsNullOrEmpty(Operation))
                            condition1 = string.Format(" And Operation = '{0}'", Operation);
                        
                        if(errorCode >0)
                            condition2 = string.Format(" And ErrorCode = {0}", errorCode);

                        string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Where 1=1{1}{2} order by regdate desc", DataManager_Define.LogTableName, condition1, condition2);
                        List<Request_Log> list = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<Request_Log>(ref tb, setQuery, DataManager_Define.LogDB);
                        list.ForEach(item =>
                        {
                            item.DetailDBLog = item.DetailDBLog.Replace("\\r\\n", "<br/>");
                            item.DetailDBLog = item.DetailDBLog.Replace("<br />\"query[", "<br/><br/><br />\"query[");
                        });
                        loglist.DataSource = list;
                        loglist.DataBind();                        
                    }
                }
                catch (Exception errorEx)
                {
                    retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(errorEx.Message), Result_Define.eResult.System_Exception);
                }
                tb.EndTransaction(queryFetcher.Render_errorFlag);
                tb.Dispose();
            }
        }
    }
}