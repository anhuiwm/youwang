using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Net;

namespace TheSoulCheatServer
{
    public partial class cheat_Ultimate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JsonObjectCollection res = new JsonObjectCollection();
            if (Request["username"] == null || Request["username"] == "")
            {
                res.Add(new JsonNumericValue("resultcode", 1));
                res.Add(new JsonStringValue("message", "Post 값 전달 실패"));
                string json_text = res.ToString();
                Response.Write(json_text);
            }
            else
            {
                string username = Request["username"];

                WebQueryParam queryFetcher = new WebQueryParam();
                string retJson = "";
                queryFetcher.SetDebugMode = true;
                long itemID = queryFetcher.QueryParam_FetchLong("itemid");
                int level = queryFetcher.QueryParam_FetchInt("level");
                int step = queryFetcher.QueryParam_FetchInt("step");
                string disarm = queryFetcher.QueryParam_Fetch("disarm", "");
                string weaponLock = queryFetcher.QueryParam_Fetch("lock", "");
                int loginCount = queryFetcher.QueryParam_FetchInt("login");

                TxnBlock tb = new TxnBlock();
                {
                    long AID = 0;
                    try
                    {

                        queryFetcher.TxnBlockInit(ref tb, ref AID);
                        long serverID = queryFetcher.GlobalDBOpen(ref tb);


                        User_account user_server = cheatData.GetUserAID(ref tb, username);
                        AID = user_server.AID;

                        Result_Define.eResult retErr = Result_Define.eResult.POST_DATA_ERROR;
                        Account userInfo = AccountManager.FlushAccountData(ref tb, AID, ref retErr);
                        if (loginCount > 0)
                        {
                            string setQuery = string.Format("Update {0} Set TotalLoginCount={2} Where Aid={1}", Account_Define.User_LoginCount_TableName, AID, loginCount);
                            retErr = tb.ExcuteSqlCommand(Dungeon_Define.Dungeon_Info_DB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                            if (retErr == Result_Define.eResult.SUCCESS)
                                AccountManager.RemoveLoginCountInfoCache(AID);
                        }
                        
                        if (itemID > 0)
                        {
                            if (level > 0 && step > 0)
                            {
                                System_Ultimate_Enchant oldItemInfo = ItemManager.GetSystem_Ultimate_Enchant(ref tb, itemID);
                                System_Ultimate_Enchant newItemInfo = ItemManager.GetSystem_Ultimate_Enchant(ref tb, oldItemInfo.Ultimate_ID, level, step);
                                string setQuery = string.Format("Update {0} Set item_id={5}, level = {1}, step = {2} Where Aid={3} and item_id={4}", Item_Define.Item_User_Ultimate_Inven_Table, level, step, AID, itemID, newItemInfo.Ultimate_Enchant_Index);
                                retErr = tb.ExcuteSqlCommand(Dungeon_Define.Dungeon_Info_DB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                            }

                        }
                        if (!string.IsNullOrEmpty(disarm))
                        {
                            string setQuery = string.Format("Delete From {0} Where Aid={1}", Item_Define.Item_User_Ultimate_Inven_Table, AID);
                            retErr = tb.ExcuteSqlCommand(Dungeon_Define.Dungeon_Info_DB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        }
                        if (!string.IsNullOrEmpty(weaponLock))
                        {
                            string setQuery = string.Format("Delete From {0} Where Aid={1} and item_id={2}", Item_Define.Item_User_Ultimate_Inven_Table, AID, itemID);
                            retErr = tb.ExcuteSqlCommand(Dungeon_Define.Dungeon_Info_DB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        }
                        if (retErr == Result_Define.eResult.SUCCESS)
                            ItemManager.RemoveUltimateWeaonCache(AID, userInfo.EquipCID);
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
    }
}