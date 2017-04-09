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
    public partial class cheat_Achieve : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            //GetSystem_Achieve_List

            if (Request["username"] != null)
            {
                WebQueryParam queryFetcher = new WebQueryParam();
                queryFetcher.SetDebugMode = true;
                string username = queryFetcher.QueryParam_Fetch("username", "");
                string retJson = "";
                
                TxnBlock tb = new TxnBlock();
                {
                    long AID = 0;
                    try
                    {
    
    queryFetcher.TxnBlockInit(ref tb, ref AID);
                        queryFetcher.GlobalDBOpen(ref tb);

                        User_account user_server = cheatData.GetUserAID(ref tb, username);
                        AID = user_server.AID;
                        List<User_Event_Data> userAchieveList = TriggerManager.Check_Achieve_Data_List(ref tb, AID);

                        string setQuery = string.Format(@"SELECT uad.User_Event_ID, uad.Event_ID, sa.AchieveID, sa.Description FROM {0} as uad inner join {2} as sa on uad.Event_ID = sa.AchieveID WHERE AID = {1} AND (GETDATE() BETWEEN StartTime AND EndTime)", Trigger_Define.User_Achieve_Data_TableName, AID, Trigger_Define.System_Achieve_TableName);
                        DataSet ds = new DataSet();
                        tb.ExcuteSqlCommand(Trigger_Define.Trigger_Info_DB, setQuery, ref ds);
                        DataGrid1.DataSource = ds;
                        DataGrid1.DataBind();
                        Result_Define.eResult retErr = Result_Define.eResult.POST_DATA_ERROR;

                        foreach (User_Event_Data achieve in userAchieveList)
                        {
                            string data = queryFetcher.QueryParam_Fetch(achieve.Event_ID.ToString(), "");
                            if (!string.IsNullOrEmpty(data))
                            {
                                long eventID = System.Convert.ToInt64(data);
                                string ClearFlag = achieve.ClearFlag;
                                if (System.Convert.ToInt32(data) >= achieve.ClearTriggerType1_Value3)
                                {
                                    achieve.ClearFlag = "C";
                                }
                                achieve.CurrentValue1 = System.Convert.ToInt64(data);
                                bool timecheck = TriggerManager.IsSetMask(achieve.ClearTriggerType1_Value3, achieve.Event_LoopType);
                                retErr = TriggerManager.UpdateUserEvent(ref tb, achieve, timecheck, Trigger_Define.eEventListType.Achive, false);
                                TriggerManager.GetUser_Achieve_Data(ref tb, AID, achieve.User_Event_ID, true);

                                //string setQuery2 = string.Format(@"UPDATE {0} SET CurrentValue1 = {1}, ClearFlag = '{2}' WHERE User_Event_ID = {3}", Trigger_Define.User_Achieve_Data_TableName, data, ClearFlag, achieve.User_Event_ID);
                                //retErr = tb.ExcuteSqlCommand(Trigger_Define.Trigger_Info_DB, setQuery2) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                                //if (retErr == Result_Define.eResult.SUCCESS)
                                //{
                                //    TriggerManager.Check_Achieve_Data_Info(ref tb, AID, eventID, true);
                                //}
                            }
                        }
                        if (retErr == Result_Define.eResult.SUCCESS)
                        {
                            TriggerManager.Check_Achieve_Data_List(ref tb, AID, true);
                        }

                        retJson = queryFetcher.Render("", retErr);
                        
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

        private void achievementList(ref TxnBlock TB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0}", Trigger_Define.System_Achieve_TableName);
            
        }
    }
}