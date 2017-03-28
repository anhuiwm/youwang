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
    public partial class Item_Soul_Charge : System.Web.UI.Page
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

                short Soul_Equip = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch("Soul_Equip", "0"));
                short Soul_Parts = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch("Soul_Parts", "0"));

                TxnBlock tb = new TxnBlock();
                {
                    long AID = 0;
                    try
                    {
    
    queryFetcher.TxnBlockInit(ref tb, ref AID);
                        queryFetcher.GlobalDBOpen(ref tb);


                        User_account user_server = cheatData.GetUserAID(ref tb, username);
                        AID = user_server.AID;

                        Result_Define.eResult retErr = Result_Define.eResult.POST_DATA_ERROR;
                        Account userInfo = AccountManager.FlushAccountData(ref tb, AID, ref retErr);
                        if (Soul_Equip > 0)
                        {
                            List<User_Inven> itemList = new List<User_Inven>();
                            List<System_Item_Base> soulItemList = ItemManager.GetSystem_Item_BaseList(ref tb, "Soul_Equip");
                            foreach (System_Item_Base item in soulItemList)
                            {
                                retErr = ItemManager.MakeItem(ref tb, ref itemList, AID, item.Item_IndexID, Soul_Equip);
                            }
                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                ItemManager.FlushInvenList(ref tb, AID);
                            }                            
                        }
                        if (Soul_Parts > 0)
                        {
                            List<User_Inven> itemList = new List<User_Inven>();
                            List<System_Item_Base> soulItemList = ItemManager.GetSystem_Item_BaseList(ref tb, "Soul_Parts");
                            foreach (System_Item_Base item in soulItemList)
                            {
                                System_Soul_Parts partsInfo = SoulManager.GetSoul_System_Soul_Parts(ref tb, item.Class_IndexID);
                                System_Soul_Active makeInfo = SoulManager.GetSoul_System_Soul_Active(ref tb, partsInfo.Soul_Group, Soul_Define.Soul_Base_Grade);
                                if (makeInfo.SoulID > 0)
                                {
                                    retErr = ItemManager.MakeItem(ref tb, ref itemList, AID, item.Item_IndexID, Soul_Parts);
                                }
                            }
                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                ItemManager.FlushInvenList(ref tb, AID);
                            }                            
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
    }
}