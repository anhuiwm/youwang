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
using ServiceStack.Text;


namespace TheSoulWebServer
{
    public partial class RequestFriendsGlobal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] ops = new string[] {
                "detail_info",
            };

            WebQueryParam queryFetcher = new WebQueryParam();
            string retJson = "";
            TxnBlock tb = new TxnBlock();
            {
                long AID = 0;
                try
                {
                    queryFetcher.TxnBlockInit(ref tb, ref AID, true);
                    string requestOp = queryFetcher.QueryParam_Fetch("op");
                    JsonObject json = new JsonObject();
                    if (Array.IndexOf(ops, requestOp) >= 0)
                    {
                        queryFetcher.operation = requestOp;
                        Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;

                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.op], requestOp);
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], AID);
                        if (requestOp.Equals("detail_info"))
                        {
                            long FAID = queryFetcher.QueryParam_FetchLong("friend_aid");
                            long FCID = queryFetcher.QueryParam_FetchLong("friend_cid");
                            List<Character> userCharList = CharacterManager.GetCharacterList(ref tb, FAID, true);

                            var findChar = userCharList.Find(charinfo => charinfo.cid == FCID);
                            if (findChar == null && FCID != 0)
                                retError = Result_Define.eResult.CHARACTER_NOT_FOUND;
                            else if (userCharList.Count < 1)
                                retError = Result_Define.eResult.CHARACTER_NOT_FOUND;
                            else
                                retError = Result_Define.eResult.SUCCESS;

                            if (FCID == 0 && retError == Result_Define.eResult.SUCCESS)
                            {
                                findChar = userCharList.FirstOrDefault();
                                if (findChar != null)
                                    FCID = findChar.cid;
                                else
                                    retError = Result_Define.eResult.CHARACTER_NOT_FOUND;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Character_Detail charInfo = new Character_Detail(findChar);
                                // use only db and not set cache. cause user info confused (not matched)
                                charInfo.equiplist = ItemManager.GetEquipList(ref tb, FAID, FCID, true);
                                charInfo.equip_active_soul = SoulManager.GetRet_Active_Soul_Equip_List(ref tb, FAID, FCID);
                                charInfo.equip_passive_soul = SoulManager.GetRet_Passive_Soul_Equip_List(ref tb, FAID, FCID);
                                charInfo.equip_ultimate = ItemManager.GetEquipUltimate(ref tb, FAID, FCID);


                                List<Character_Simple> cidList = new List<Character_Simple>();
                                foreach (Character setInfo in userCharList)
                                {
                                    cidList.Add(new Character_Simple(setInfo));
                                }

                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.CharacterDetailInfo], charInfo.ToJson());
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.CharacterSimpleInfoList], mJsonSerializer.ToJsonString(cidList));
                            }
                        }
                        retJson = queryFetcher.Render(json.ToJson(), retError);
                    }
                    else
                    {
                        retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(DefineError.System_Unknown_Operation), Result_Define.eResult.System_Unknown_Operation);
                    }
                }
                catch (Exception errorEx)
                {
                    string error = "";
#if DEBUG
                    error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
#else
                    if (queryFetcher.SetDebugMode)
                        error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
#endif

                    error = mJsonSerializer.AddJson(error, "Message", mJsonSerializer.ToJsonString(errorEx.Message));
                    retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(error), Result_Define.eResult.System_Exception);
                }
                finally
                {
                    //if (AID > 0)
                    //    queryFetcher.CheckSnail_ID(ref tb, AID);
                    queryFetcher.SetShowLogMode = tb.EndTransaction(queryFetcher.Render_errorFlag);
                    queryFetcher.ErrorLogWrite(retJson, ref tb);
                    tb.Dispose();
                } 
            }
        }
    }
}