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
using TheSoulWebServer.Tools;
using ServiceStack.Text;

namespace TheSoulWebServer
{
    public partial class RequestSoul : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] ops = new string[] {
                // soul 공통
                "getsoullist",
                "getsoullist_all",
                "equipsoul_list",
                "equip_soul",
                "equip_soul_new",

                // active soul
                "soul_activation",
                "active_soul_levelup",
                "active_soul_gradeup",
                "active_soul_auto_gradeup",
                "active_soul_starup",
                "active_soul_skill_set",
                "buy_soul_slot",

                // active soul hide by gm tool
                "active_soul_hide_list",

                // active soul equip
                "soul_equipitemlist",
                "soul_equipitem_to_soul",
                "soul_equip_craft",

                // passive soul
                "passive_soul_create",
                "passive_soul_extract",
                "passive_soul_levelup",
                "passive_soul_reserve",
            };

            WebQueryParam queryFetcher = new WebQueryParam();
            string retJson = "";

            TxnBlock tb = new TxnBlock();
            {
                long AID = 0;
                try
                {
                    queryFetcher.TxnBlockInit(ref tb, ref AID);

                    string requestOp = queryFetcher.QueryParam_Fetch("op");
                    JsonObject json = new JsonObject();

                    if (Array.IndexOf(ops, requestOp) >= 0)
                    {
                        Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                        queryFetcher.operation = requestOp;
                        long SoulSeq = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("soulseq"));
                        long CID = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("cid"));
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.op], requestOp);
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], AID);

                        if (queryFetcher.ReRequestFlag)
                        {
                            retJson = queryFetcher.ReRequestRender();
                        }
                        else if (requestOp.Equals("active_soul_hide_list"))
                        {
                            List<Admin_System_SoulGroup_Active> retList = SoulManager.GetAdmin_System_Soul_ActiveList(ref tb);
                            json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.HideActiveSoulLIst], mJsonSerializer.ToJsonString(retList));
                            retError = Result_Define.eResult.SUCCESS;
                        }
                        else if (requestOp.Equals("getsoullist"))
                        {
                            retError = Result_Define.eResult.SUCCESS;
                            List<Ret_Soul_Active> getActiveSoulList = SoulManager.GetActive_Soul_Ret_List(ref tb, AID, CID);
                            List<Ret_Soul_Passive> getPassiveSoulList = SoulManager.GetPassive_Soul_Ret_List(ref tb, AID, CID);
                            User_Soul_Make_Info makeCountInfo = SoulManager.GetUserSoulMakeInfo(ref tb, AID);

                            json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.ActiveSoulLIst], mJsonSerializer.ToJsonString(getActiveSoulList));
                            json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.PassiveSoulList], mJsonSerializer.ToJsonString(getPassiveSoulList));
                            json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.PassiveRubyMakeCount], mJsonSerializer.ToJsonString(makeCountInfo.Passive_Make_Count));
                        }
                        else if (requestOp.Equals("getsoullist_all"))
                        {
                            retError = Result_Define.eResult.SUCCESS;
                            List<Character> userCharList = CharacterManager.GetCharacterList(ref tb, AID);
                            List<Ret_Soul_Active_Account> allActiveSoulList = new List<Ret_Soul_Active_Account>();
                            userCharList.ForEach(charInfo =>
                            {
                                Ret_Soul_Active_Account addObj = new Ret_Soul_Active_Account();
                                addObj.cid = charInfo.cid;
                                addObj.active_soul_list = SoulManager.GetActive_Soul_Ret_List(ref tb, AID, charInfo.cid, true);
                                addObj.passive_soul_list = SoulManager.GetPassive_Soul_Ret_List(ref tb, AID, charInfo.cid, true);
                                allActiveSoulList.Add(addObj);
                            });
                            json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.AllSoulList], mJsonSerializer.ToJsonString(allActiveSoulList));
                        }
                        else if (requestOp.Equals("equipsoul_list"))
                        {
                            retError = Result_Define.eResult.SUCCESS;

                            List<Ret_Equip_Soul_Active> getEquipActiveSoulList = SoulManager.GetRet_Active_Soul_Equip_List(ref tb, AID, CID);
                            List<Ret_Soul_Passive> getEquipPassiveSoulList = SoulManager.GetRet_Passive_Soul_Equip_List(ref tb, AID, CID);

                            json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.EquipActiveSoulList], mJsonSerializer.ToJsonString(getEquipActiveSoulList));
                            json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.EquipPassiveSoulList], mJsonSerializer.ToJsonString(getEquipPassiveSoulList));
                        }
                        else if (requestOp.Equals("soul_activation"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            retError = SoulManager.SoulActivation(ref tb, AID, SoulSeq);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                //Account UserInfo = AccountManager.GetAccountData(ref tb, AID, true);
                                List<Ret_Soul_Active> getActiveSoulList = SoulManager.GetActive_Soul_Ret_List(ref tb, AID, CID, true);
                                Ret_Soul_Active updateSoul = getActiveSoulList.Find(item => item.soulseq == SoulSeq);

                                //json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetGold], UserInfo.Gold.ToString());
                                //json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetRuby], (UserInfo.Cash + UserInfo.EventCash).ToString());
                                json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.ActiveSoulInfo], mJsonSerializer.ToJsonString(updateSoul));
                            }
                        }
                        else if (requestOp.Equals("soul_equipitemlist"))
                        {
                            retError = Result_Define.eResult.SUCCESS;
                            List<User_Soul_Equip_Inven> getSoulEquipList = SoulManager.GetSoul_Equip_Ret_List(ref tb, AID);

                            json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.SoulEquipItemList], mJsonSerializer.ToJsonString(getSoulEquipList));
                        }
                        else if (requestOp.Equals("soul_equipitem_to_soul"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long SoulEquipSeq = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("soulequipseq"));

                            List<Return_DisassableSoulEquip_List> retDeletedItem = new List<Return_DisassableSoulEquip_List>();

                            if (SoulSeq > 0 && SoulEquipSeq > 0)
                            {
                                retError = SoulManager.Equip_SoulEquipToActiveSoul(ref tb, AID, SoulSeq, SoulEquipSeq, ref retDeletedItem);
                            }
                            else
                                retError = SoulSeq == 0 ? Result_Define.eResult.SOUL_ID_NOT_FOUND : Result_Define.eResult.SOUL_EQUIP_ID_INVALIDE;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<Ret_Soul_Active> getActiveSoulList = SoulManager.GetActive_Soul_Ret_List(ref tb, AID, CID, true);
                                Ret_Soul_Active updateSoul = getActiveSoulList.Find(item => item.soulseq == SoulSeq);

                                json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.ActiveSoulInfo], mJsonSerializer.ToJsonString(updateSoul));
                                json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.DeletedSoulEquip], mJsonSerializer.ToJsonString(retDeletedItem));
                            }
                        }
                        else if (requestOp.Equals("soul_equip_craft"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long CraftItemID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("craft_soul_equip_id"));

                            List<User_Inven> RetGetItems = new List<User_Inven>();
                            List<Return_DisassableSoulEquip_List> retDeletedItem = new List<Return_DisassableSoulEquip_List>();
                            retError = SoulManager.CraftSoulEquip(ref tb, AID, CraftItemID, ref RetGetItems, ref retDeletedItem);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account UserInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError);

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    List<User_Soul_Equip_Inven> getSoulEquipInven = SoulManager.GetUser_Soul_Equip_Inven(ref tb, AID, true);

                                    var getSoulEquip = getSoulEquipInven.Find(item => item.soul_equip_id == CraftItemID);

                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetGold], UserInfo.Gold.ToString());
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], Ret_Inven_Item.makeInvenListJson(ref RetGetItems));
                                    json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.SoulEquipInfo], mJsonSerializer.ToJsonString(getSoulEquip));
                                    json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.DeletedSoulEquip], mJsonSerializer.ToJsonString(retDeletedItem));
                                }
                            }
                        }
                        else if (requestOp.Equals("active_soul_levelup") || requestOp.Equals("active_soul_gradeup") || requestOp.Equals("active_soul_starup")
                                || requestOp.Equals("active_soul_skill_set") || requestOp.Equals("active_soul_auto_gradeup"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            if (requestOp.Equals("active_soul_levelup"))
                            {
                                byte try_count = (byte)queryFetcher.QueryParam_FetchInt("try_count", 1);
                                retError = SoulManager.ActiveSoulLevelUp(ref tb, AID, SoulSeq, try_count);
                            }
                            else if (requestOp.Equals("active_soul_gradeup"))
                                retError = SoulManager.ActiveSoulGradeUp(ref tb, AID, SoulSeq);
                            else if (requestOp.Equals("active_soul_starup"))
                                retError = SoulManager.ActiveSoulStarUp(ref tb, AID, SoulSeq);
                            else if (requestOp.Equals("active_soul_skill_set"))
                            {
                                byte setGroup = System.Convert.ToByte(queryFetcher.QueryParam_Fetch("setgroup"));
                                byte setBuffPos = System.Convert.ToByte(queryFetcher.QueryParam_Fetch("setbuffpos"));

                                retError = SoulManager.SetSpecialBuff(ref tb, AID, CID, SoulSeq, setGroup, setBuffPos);
                            }
                            else if (requestOp.Equals("active_soul_auto_gradeup"))
                            {
                                List<Return_DisassableSoulEquip_List> retDeletedItem = new List<Return_DisassableSoulEquip_List>();
                                retError = SoulManager.AutoActiveSoulGradeUp(ref tb, AID, SoulSeq, ref retDeletedItem);
                                if (retError == Result_Define.eResult.SUCCESS)
                                    json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.DeletedSoulEquip], mJsonSerializer.ToJsonString(retDeletedItem));
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account UserInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    List<Ret_Soul_Active> getActiveSoulList = new List<Ret_Soul_Active>();
                                    List<Character> getCharList = CharacterManager.GetCharacterList(ref tb, AID);
                                    List<Ret_Soul_Active> updateSoul = new List<Ret_Soul_Active>();
                                    getCharList.ForEach(charinfo =>
                                    {
                                        //if (requestOp.Equals("active_soul_starup"))
                                        //    SoulManager.RemoveCacheUser_ActiveSoul_Special_Buff(AID, charinfo.cid);

                                        getActiveSoulList = SoulManager.GetActive_Soul_Ret_List(ref tb, AID, charinfo.cid);
                                        updateSoul.Add(getActiveSoulList.Find(item => item.soulseq == SoulSeq));
                                    });

                                    //Ret_Soul_Active updateSoul = getActiveSoulList.Find(item => item.soulseq == SoulSeq);

                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetGold], UserInfo.Gold.ToString());
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetRuby], (UserInfo.Cash + UserInfo.EventCash).ToString());
                                    json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.ActiveSoulInfo], mJsonSerializer.ToJsonString(updateSoul));
                                }
                            }
                        }
                        else if (requestOp.Equals("passive_soul_create"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            int rubyMakeCount = 0;
                            byte try_count = (byte)queryFetcher.QueryParam_FetchInt("try_count", 1);
                            List<User_PassiveSoul> retMakeSoul = new List<User_PassiveSoul>();
                            retError = SoulManager.MakePassiveSoul(ref tb, AID, CID, try_count, ref retMakeSoul, ref rubyMakeCount);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account UserInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError);

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    List<User_PassiveSoul> getPassiveSoulList = SoulManager.GetUser_PassiveSoul(ref tb, AID, CID);
                                    List<Ret_Soul_Passive> updateSoul = new List<Ret_Soul_Passive>();
                                    retMakeSoul.ForEach(retsoul => { updateSoul.Add(new Ret_Soul_Passive(retsoul)); });

                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetStone], UserInfo.Stone.ToString());
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetRuby], (UserInfo.Cash + UserInfo.EventCash).ToString());
                                    json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.PassiveSoulInfo], mJsonSerializer.ToJsonString(updateSoul));
                                    json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.PassiveRubyMakeCount], mJsonSerializer.ToJsonString(rubyMakeCount));
                                }
                            }
                        }
                        else if (requestOp.Equals("passive_soul_extract") || requestOp.Equals("passive_soul_levelup"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            Character retCharInfo = CharacterManager.GetCharacter(ref tb, AID, CID);
                            int Exp = 0;

                            if (requestOp.Equals("passive_soul_extract"))
                            {
                                string setSoulJson = queryFetcher.QueryParam_Fetch("soulseq_list", "[]");
                                List<long> material_souls = mJsonSerializer.JsonToObject<List<long>>(setSoulJson);

                                if (material_souls.Count > 0)
                                    retError = SoulManager.ExtractPassiveSoul(ref tb, AID, CID, material_souls, ref Exp, ref retCharInfo);
                                else
                                    retError = Result_Define.eResult.SOUL_ID_NOT_FOUND;
                            }
                            else if (requestOp.Equals("passive_soul_levelup"))
                            {
                                byte try_count = (byte)queryFetcher.QueryParam_FetchInt("try_count", 1);
                                retError = SoulManager.PassiveSoulLevelUp(ref tb, AID, CID, SoulSeq, ref Exp, ref retCharInfo, try_count);
                            }
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                if (requestOp.Equals("passive_soul_extract"))
                                {
                                    json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.PassiveSoulGetExp], Exp.ToString());
                                    json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.RetPassiveExp], retCharInfo.passivesoulexp.ToString());
                                }
                                else if (requestOp.Equals("passive_soul_levelup"))
                                {
                                    List<Ret_Soul_Passive> getPassiveSoulList = SoulManager.GetPassive_Soul_Ret_List(ref tb, AID, CID);
                                    Ret_Soul_Passive updateSoul = getPassiveSoulList.Find(item => item.soulseq == SoulSeq);

                                    json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.PassiveSoulUseExp], Exp.ToString());
                                    json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.RetPassiveExp], retCharInfo.passivesoulexp.ToString());
                                    json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.PassiveSoulInfo], mJsonSerializer.ToJsonString(updateSoul));
                                }
                            }
                        }
                        else if (requestOp.Equals("passive_soul_reserve"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            retError = SoulManager.KeepStoragePassiveSoul(ref tb, AID, CID, SoulSeq);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<Ret_Soul_Passive> getPassiveSoulList = SoulManager.GetPassive_Soul_Ret_List(ref tb, AID, CID);
                                Ret_Soul_Passive updateSoul = getPassiveSoulList.Find(item => item.soulseq == SoulSeq);

                                json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.PassiveSoulInfo], mJsonSerializer.ToJsonString(updateSoul));
                            }
                        }
                        else if (requestOp.Equals("equip_soul") || requestOp.Equals("equip_soul_new"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            short soul_type = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch("soul_type"));
                            List<SoulEquipSlot> EquipList = mJsonSerializer.JsonToObject<List<SoulEquipSlot>>(queryFetcher.QueryParam_Fetch("equiplist", "[]"));
                            User_Character_Equip_Soul updateEquipSoul = new User_Character_Equip_Soul();
                            List<User_Character_Equip_Soul> updateEquipSoulList = new List<User_Character_Equip_Soul>();

                            if (EquipList.Count > 0)
                            {
                                retError = soul_type == Soul_Define.Equip_Soul_Type_Acitve ?
                                                            SoulManager.EquipActiveSoulToCharacter(ref tb, AID, CID, ref EquipList, ref updateEquipSoulList, requestOp.Equals("equip_soul_new"))
                                                            : SoulManager.EquipPassiveSoulToCharacter(ref tb, AID, CID, ref EquipList, ref updateEquipSoulList);
                            }
                            else
                            {
                                retError = Result_Define.eResult.SOUL_EQUIPSLOT_INVALIDE;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Character retCharInfo = CharacterManager.GetCharacter(ref tb, AID, CID);

                                json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.PassiveSoulGetExp], "0");
                                json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.RetPassiveExp], retCharInfo.passivesoulexp.ToString());

                                //json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.SoulEquipInfo], mJsonSerializer.ToJsonString(updateEquipSoul));
                            }
                        }
                        else if (requestOp.Equals("buy_soul_slot"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            short soul_type = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch("soul_type"));

                            Character charInfo = CharacterManager.GetCharacter(ref tb, AID, CID);
                            retError = SoulManager.BuySoulSlot(ref tb, AID, CID, soul_type, charInfo.level, charInfo.activeslot, charInfo.passiveslot);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                charInfo = CharacterManager.FlushCharacter(ref tb, AID, CID);
                                Account UserInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError);

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetRuby], (UserInfo.Cash + UserInfo.EventCash).ToString());
                                    json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.RetActiveSlot], charInfo.activeslot.ToString());
                                    json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.RetPassiveSlot], charInfo.passiveslot.ToString());

                                    //json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.SoulEquipInfo], mJsonSerializer.ToJsonString(updateEquipSoul));
                                }
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

                    //error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
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