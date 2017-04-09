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
    public partial class cheat_character_levelup : System.Web.UI.Page
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
                string data = queryFetcher.QueryParam_Fetch("level", "0");
                int classType = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("CLASS", "0"));
                int level = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("level", "0"));
                int exp = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("exp", "0"));
                int passive_exp = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("passive_exp", "0"));

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
                            long curExp = 0;
                            List<Character> characterList = CharacterManager.GetCharacterList(ref tb, AID);
                            foreach (Character character in characterList)
                            {
                                if (character.Class == classType)
                                {
                                    CID = character.cid;
                                    curExp = character.totalexp;
                                }
                            }

                            if (CID > 0 && level > 0)
                            {
                                string CharacterExpTableName = "System_Character_EXP";
                                string setQuery = string.Format("SELECT * FROM {0} Where level = {1} ", CharacterExpTableName, level);
                                System_Character_EXP needExp = TheSoul.DataManager.GenericFetch.FetchFromDB<System_Character_EXP>(ref tb, setQuery, Character_Define.CharacterDBName);
                                int addExp = System.Convert.ToInt32(needExp.ACC_exp-100);
                                int Gold = 0;
                                curExp = curExp * (-1);
                                retErr = CharacterManager.UpdateCharacterInfo(ref tb, CID, AID, (int)curExp, Gold, Character_Define.CharacterDBName, false, true);
                                if (retErr == Result_Define.eResult.SUCCESS)
                                {
                                    retErr = CharacterManager.UpdateCharacterInfo(ref tb, CID, AID, addExp, Gold, Character_Define.CharacterDBName, false, true);
                                    if (retErr == Result_Define.eResult.SUCCESS)
                                    {

                                        Character charaterInfo = CharacterManager.GetCharacter(ref tb, AID, CID);
                                        if (charaterInfo.equipflag == "Y")
                                        {
                                            charaterInfo = CharacterManager.FlushCharacter(ref tb, AID, CID);
                                            string setQuery2 = string.Format("Update {0} Set LV = {2} Where Aid={1}", Account_Define.AccountDBTableName, AID, charaterInfo.level);
                                            retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery2) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                                            if (retErr == Result_Define.eResult.SUCCESS)
                                            {
                                                AccountManager.FlushAccountData(ref tb, AID, ref retErr);
                                            }
                                        }
                                    }
                                }
                            }
                            if (CID > 0 && exp > 0)
                            {
                                retErr = CharacterManager.UpdateCharacterInfo(ref tb, CID, AID, exp, 0);
                                if (retErr == Result_Define.eResult.SUCCESS)
                                {
                                    CharacterManager.FlushCharacter(ref tb, AID, CID);
                                }
                            }
                            if (CID > 0 && passive_exp > 0)
                            {
                                retErr = CharacterManager.UpdateCharacterPassiveSoulExp(ref tb, AID, CID, passive_exp, Character_Define.CharacterDBName, true);
                                if (retErr == Result_Define.eResult.SUCCESS)
                                {
                                    CharacterManager.FlushCharacter(ref tb, AID, CID);
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