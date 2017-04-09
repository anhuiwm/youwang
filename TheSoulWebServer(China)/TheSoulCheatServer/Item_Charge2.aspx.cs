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
    public partial class Item_Charge2 : System.Web.UI.Page
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
                WebQueryParam queryFetcher = new WebQueryParam();
                string retJson = "";
                queryFetcher.SetDebugMode = true;
                string username = queryFetcher.QueryParam_Fetch("username", "");
                int classType = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("CLASS", "0"));
                long ItemID = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("ItemID", "0"));
                long ItemID2 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("ItemID2", "0"));
                long ItemID3 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("ItemID3", "0"));
                long ItemID4 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("ItemID4", "0"));
                long ItemID5 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("ItemID5", "0"));
                long ItemID6 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("ItemID6", "0"));
                short grade = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch("Grade", "0"));
                short grade2 = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch("Grade2", "0"));
                short grade3 = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch("Grade3", "0"));
                short grade4 = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch("Grade4", "0"));
                short grade5 = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch("Grade5", "0"));
                short grade6 = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch("Grade6", "0"));
                short ItemEA = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch("ItemEA", "1"));
                short ItemEA2 = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch("ItemEA2", "1"));
                short ItemEA3 = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch("ItemEA3", "1"));
                short ItemEA4 = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch("ItemEA4", "1"));
                short ItemEA5 = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch("ItemEA5", "1"));
                short ItemEA6 = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch("ItemEA6", "1"));
                string passive = queryFetcher.QueryParam_Fetch("passive", "");

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
                        if (classType > 0)
                        {
                            long CID = 0;
                            List<Character> characterList = CharacterManager.GetCharacterList(ref tb, AID);
                            foreach (Character character in characterList)
                            {
                                if (character.Class == classType)
                                {
                                    CID = character.cid;
                                }
                            }

                            if (CID > 0)
                            {
                                List<User_Inven> itemList = new List<User_Inven>();
                                if (ItemID > 0 && ItemEA > 0)
                                {
                                    if (!string.IsNullOrEmpty(passive))
                                        retErr = SoulManager.MakePassiveSoulID(ref tb, ItemID, AID, CID);
                                    else
                                        retErr = ItemManager.MakeItem(ref tb, ref itemList, AID, ItemID, ItemEA, CID, 0, grade);
                                }
                                if (ItemID2 > 0 && ItemEA2 > 0)
                                {
                                    if (!string.IsNullOrEmpty(passive))
                                        retErr = SoulManager.MakePassiveSoulID(ref tb, ItemID2, AID, CID);
                                    else
                                        retErr = ItemManager.MakeItem(ref tb, ref itemList, AID, ItemID2, ItemEA2, CID, 0, grade2);
                                }
                                if (ItemID3 > 0 && ItemEA3 > 0)
                                {
                                    if (!string.IsNullOrEmpty(passive))
                                        retErr = SoulManager.MakePassiveSoulID(ref tb, ItemID3, AID, CID);
                                    else
                                        retErr = ItemManager.MakeItem(ref tb, ref itemList, AID, ItemID3, ItemEA3, CID, 0, grade3);
                                }
                                if (ItemID4 > 0 && ItemEA4 > 0)
                                {
                                    if (!string.IsNullOrEmpty(passive))
                                        retErr = SoulManager.MakePassiveSoulID(ref tb, ItemID4, AID, CID);
                                    else
                                        retErr = ItemManager.MakeItem(ref tb, ref itemList, AID, ItemID4, ItemEA4, CID, 0, grade);
                                }
                                if (ItemID5 > 0 && ItemEA5 > 0)
                                {
                                    if (!string.IsNullOrEmpty(passive))
                                        retErr = SoulManager.MakePassiveSoulID(ref tb, ItemID5, AID, CID);
                                    else
                                        retErr = ItemManager.MakeItem(ref tb, ref itemList, AID, ItemID5, ItemEA5, CID, 0, grade5);
                                }
                                if (ItemID6 > 0 && ItemEA6 > 0)
                                {
                                    if (!string.IsNullOrEmpty(passive))
                                        retErr = SoulManager.MakePassiveSoulID(ref tb, ItemID6, AID, CID);
                                    else
                                        retErr = ItemManager.MakeItem(ref tb, ref itemList, AID, ItemID6, ItemEA6, CID, 0, grade6);
                                }

                                if (retErr == Result_Define.eResult.SUCCESS)
                                {
                                    ItemManager.FlushInvenList(ref tb, AID);
                                }
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