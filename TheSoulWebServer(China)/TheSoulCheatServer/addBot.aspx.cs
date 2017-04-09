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
    public partial class addBot : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam();
            string retJson = "";
            queryFetcher.SetDebugMode = true;
            for (int i = 0; i <= 200; i++)
            {
                Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                TxnBlock tb = new TxnBlock();
                {
                    long AID = 0;
                    try
                    {
                        queryFetcher.TxnBlockInit(ref tb, ref AID);
                        tb.IsoLevel = IsolationLevel.ReadCommitted;
                        long server_group_id = queryFetcher.GlobalDBOpen(ref tb);
                        long server_id = 6;

                        SqlCommand commandCreateAccount = new SqlCommand();
                        commandCreateAccount.CommandText = "System_Get_UAID";
                        var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        var result_id = new SqlParameter("@ret_platform_user_id", SqlDbType.NVarChar, 128) { Direction = ParameterDirection.Output };
                        commandCreateAccount.Parameters.Add("@platform_type", SqlDbType.Int).Value = 0;
                        commandCreateAccount.Parameters.Add("@platform_user_id", SqlDbType.NVarChar, 128).Value = "";
                        commandCreateAccount.Parameters.Add("@user_account_status", SqlDbType.Int).Value = 0;
                        commandCreateAccount.Parameters.Add(result);
                        commandCreateAccount.Parameters.Add(result_id);

                        if (tb.ExcuteSqlStoredProcedure("global", ref commandCreateAccount))
                        {
                            if (System.Convert.ToInt32(result.Value) < 0)
                            {
                                retError = Result_Define.eResult.DB_ERROR;
                            }
                            else
                                retError = Result_Define.eResult.SUCCESS;
                        }
                        else
                            retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                        commandCreateAccount.Dispose();

                        if (retError == Result_Define.eResult.SUCCESS)
                        {
                            string userID = result_id.Value.ToString();
                            string userName = "bot_" + i;
                            string CountryCode = "kr";
                            int LanguageCode = 0;
                            AID = System.Convert.ToInt64(result.Value);
                            if (LanguageCode < 1)
                            {
                                switch (CountryCode.ToLower())
                                {
                                    case "kr":
                                        LanguageCode = 0; // 한국어
                                        break;
                                    case "jp":
                                        LanguageCode = 2; // 일본어
                                        break;
                                    case "cn":
                                        LanguageCode = 3; // 중국어
                                        break;
                                    default:
                                        LanguageCode = 1; // 영어
                                        break;
                                }
                            }

                            retError = AccountManager.CreateAccountCommon(ref tb, AID, userID, userName);

                            if (retError == Result_Define.eResult.ACCOUNT_ID_ALREAD_CREATED)
                            {
                                if (CharacterManager.GetCharacterList(ref tb, AID).Count > 0)
                                    retError = Result_Define.eResult.ACCOUNT_ID_LOGIN_TO_PLAY;
                                else
                                    retError = Result_Define.eResult.ACCOUNT_ID_ALREAD_CREATED_BUT_NEED_CHARACTER_CREATE;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = AccountManager.RegistAccountGlobal(ref tb, AID, userName, server_id);

                            if (retError == Result_Define.eResult.SUCCESS || retError == Result_Define.eResult.ACCOUNT_ID_GLOBAL_REGIST_ALREADY)
                                retError = AccountManager.CreateAccountSharding(ref tb, AID, userName, CountryCode, LanguageCode);
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                short setClass = 1;
                                long CID = 0;
                                if ((i % 2) == 0)
                                    setClass = 2;

                                List<Character> CharacterList = CharacterManager.FlushCharacterList(ref tb, AID);
                                if (CharacterList.Count > 0)
                                    retError = Result_Define.eResult.CHARACTER_ID_ALREAD_CREATED;

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    if (setClass < (short)Character_Define.SystemClassType.Class_First || setClass > (short)Character_Define.SystemClassType.Class_Last)
                                        retError = Result_Define.eResult.CHARACTER_CLASS_INVALIDE;
                                }

                                if (retError == Result_Define.eResult.SUCCESS)
                                    retError = CharacterManager.RegistCharacterGlobal(ref tb, ref CID, AID, server_id, false);

                                if (retError == Result_Define.eResult.SUCCESS || retError == Result_Define.eResult.CHARACTER_ID_GLOBAL_REGIST_ALREADY)
                                    retError = CharacterManager.CreateCharacterSharding(ref tb, AID, CID, setClass);

                                if (retError == Result_Define.eResult.SUCCESS)
                                    retError = CharacterManager.UpdateCharacterInfo(ref tb, CID, AID, 0, 0);

                                System_PC_BASE PcBaseInfo = null;
                                if (retError == Result_Define.eResult.SUCCESS)
                                    PcBaseInfo = CharacterManager.GetPCbaseInfo(ref tb, System.Convert.ToInt32(setClass));

                                if (PcBaseInfo != null)
                                {
                                    CharacterList = CharacterManager.FlushCharacterList(ref tb, AID);

                                    List<User_Inven> setMakeItem = new List<User_Inven>();

                                    setMakeItem.Add(new User_Inven(PcBaseInfo.Base_Weapon, 1, 2, 0));
                                    setMakeItem.Add(new User_Inven(PcBaseInfo.Base_Armor, 1));
                                    setMakeItem.Add(new User_Inven(PcBaseInfo.Base_Helmet, 1));
                                    setMakeItem.Add(new User_Inven(PcBaseInfo.Base_Glove, 1));
                                    setMakeItem.Add(new User_Inven(PcBaseInfo.Base_Shoes, 1));

                                    List<User_Inven> setEquipItem = new List<User_Inven>();

                                    retError = ItemManager.MakeEquipArray(ref tb, ref setEquipItem, AID, setMakeItem, CID);

                                    if (setEquipItem.Count < 0 || retError != Result_Define.eResult.SUCCESS)
                                        retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

                                    setEquipItem.ForEach(item => { ItemManager.EquipItemToCharacter(ref tb, AID, item.invenseq, CID); });
                                }
                            }
                        }
                        retJson = queryFetcher.Render("", retError);
                    }
                    catch (Exception errorEx)
                    {
                        retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(errorEx.Message), Result_Define.eResult.System_Exception);
                    }
                    tb.EndTransaction(queryFetcher.Render_errorFlag);
                    tb.Dispose();
                }
                Response.Write(retError+"<br>");
            }
        }
    }
}