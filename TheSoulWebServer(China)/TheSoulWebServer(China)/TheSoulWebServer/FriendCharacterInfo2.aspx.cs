using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Json;

public partial class FriendCharacterInfo2 : System.Web.UI.Page
{
    private SqlConnection DBCon = null;
    private SqlConnection DBCon_Char = null;
    private SqlConnection DBCon_gamedata = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        JsonObjectCollection res = new JsonObjectCollection();
        if (Request["friendaid"] == null || Request["friendaid"] == "")
        {
            res.Add(new JsonNumericValue("resultcode", 1));
            string json_text = res.ToString();
            Response.Write(json_text);
        }
        else
        {
            try
            {
                ulong FriendAID = System.Convert.ToUInt64(Request.Params["friendaid"]);
                int ItemOptionCNT = 0;
				string DBconString = "";
				string savePath = Request.PhysicalApplicationPath;
				TheSoulDBConnect.ReadFile(savePath, ref DBconString);
				DBCon = new SqlConnection(DBconString);
                //DB분산 정보 가져오기
                DBCon.Open();
                var indexcommand = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DBCon, CommandText = "GetFriendAccountDB" };
                var outputRetDBIP = new SqlParameter("@RetDBIP", SqlDbType.VarChar, 16) { Direction = ParameterDirection.Output };
                var outputRetDB_INDEX = new SqlParameter("@RetDB_INDEX", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var outputRetAID = new SqlParameter("@RetAID", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                var outputRetUserName = new SqlParameter("@RetUserName", SqlDbType.NVarChar,32) { Direction = ParameterDirection.Output };
                var outputResultAccount = new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                indexcommand.Parameters.Add("@SNO", SqlDbType.BigInt).Value = FriendAID;
                indexcommand.Parameters.Add(outputRetDBIP);
                indexcommand.Parameters.Add(outputRetDB_INDEX);
                indexcommand.Parameters.Add(outputRetAID);
                indexcommand.Parameters.Add(outputRetUserName);
                indexcommand.Parameters.Add(outputResultAccount);
                indexcommand.ExecuteNonQuery();
                string retRetDBIP = System.Convert.ToString(outputRetDBIP.Value);
                int retDB_INDEX = int.Parse(outputRetDB_INDEX.Value.ToString());
                ulong retAID = System.Convert.ToUInt64(outputRetAID.Value.ToString());
                string retUserName = System.Convert.ToString(outputRetUserName.Value);
                int retResultAccount = int.Parse(outputResultAccount.Value.ToString());
                DBCon.Close();
                if (retResultAccount != 0)
                {
                    res.Add(new JsonNumericValue("resultcode", 47));
                    string json_text = res.ToString();
                    Response.Write(json_text);
                }
                else
                {
                    //DB연결 정의
                    string connChar = "SERVER=" + retRetDBIP + ",9533;DATABASE=AOW_character;UID=SA;PWD=dpaTlemrpdlawm!@#";
                    DBCon_Char = new SqlConnection(connChar);

                    //파라미터 값 확인 로그
                    string reqval = "friendaid=" + Request["friendaid"] + "&username=" + retUserName;
                    string reqURL = System.Convert.ToString(Request.Url);
                    TheSoulWebServerErrorLog.WriteError(savePath, reqURL, reqval);

                    //CID 가져오기
                    DBCon_Char.Open();
                    var command = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DBCon_Char, CommandText = "GetFriendCharacterCheck" };
                    var outputCID = new SqlParameter("@RESULTCID", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                    var outputClass = new SqlParameter("@RESULTCLASS", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var outputLevel = new SqlParameter("@RESULTLEVEL", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var outputEXP = new SqlParameter("@RESULTEXP", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    command.Parameters.Add("@AID", SqlDbType.BigInt).Value = retAID;//AID
                    command.Parameters.Add(outputCID);
                    command.Parameters.Add(outputClass);
                    command.Parameters.Add(outputLevel);
                    command.Parameters.Add(outputEXP);
                    command.ExecuteNonQuery();
                    ulong retCID = ulong.Parse(outputCID.Value.ToString());
                    int retClass = int.Parse(outputClass.Value.ToString());
                    int retLevel = int.Parse(outputLevel.Value.ToString());
                    int retEXP = int.Parse(outputEXP.Value.ToString());
                    DBCon_Char.Close();

                    res.Add(new JsonNumericValue("resultcode", retResultAccount));
                    res.Add(new JsonStringValue("accountname", retUserName));
                    res.Add(new JsonNumericValue("characterclass", retClass));
                    res.Add(new JsonNumericValue("characterlevel", retLevel));
                    res.Add(new JsonNumericValue("characterexp", retEXP));

                    //inventory List
                    JsonArrayCollection MakeJson = new JsonArrayCollection("inventorylist");
                    DBCon_Char.Open();
                    var command2 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DBCon_Char, CommandText = "GetFriendCharacterInven" };
                    command2.Parameters.Add("@AID", SqlDbType.BigInt).Value = retAID;//AID
                    command2.Parameters.Add("@CID", SqlDbType.BigInt).Value = retCID;//CID
                    command2.ExecuteNonQuery();
                    SqlDataReader reader = command2.ExecuteReader();
                    while (reader.Read())
                    {
                        JsonObjectCollection items = new JsonObjectCollection();
                        items.Add(new JsonNumericValue("invenseq", System.Convert.ToUInt64(reader["invenseq"])));
                        items.Add(new JsonNumericValue("cid", System.Convert.ToUInt64(reader["CID"])));
                        items.Add(new JsonNumericValue("itemid", System.Convert.ToUInt32(reader["itemid"])));
                        items.Add(new JsonNumericValue("itemea", System.Convert.ToUInt16(reader["itemea"])));
                        items.Add(new JsonNumericValue("itemtype", System.Convert.ToUInt16(reader["itemtype"])));
                        items.Add(new JsonNumericValue("classtype", System.Convert.ToUInt16(reader["class"])));
                        items.Add(new JsonNumericValue("grade", System.Convert.ToUInt16(reader["enchant_grade"])));
                        items.Add(new JsonStringValue("equipflag", System.Convert.ToString(reader["equipflag"])));
                        items.Add(new JsonStringValue("newyn", System.Convert.ToString(reader["newyn"])));
                        items.Add(new JsonNumericValue("enchant_grade", System.Convert.ToUInt32(reader["enchant_grade"])));
                        items.Add(new JsonNumericValue("enchant_level", System.Convert.ToUInt32(reader["enchant_level"])));
                        items.Add(new JsonNumericValue("enchant_exp", System.Convert.ToUInt64(reader["enchant_exp"])));
                        if (System.Convert.ToUInt64(reader["optionvalue1"]) != 0) { ItemOptionCNT = ItemOptionCNT + 1; }
                        if (System.Convert.ToUInt64(reader["optionvalue2"]) != 0) { ItemOptionCNT = ItemOptionCNT + 1; }
                        if (System.Convert.ToUInt64(reader["optionvalue3"]) != 0) { ItemOptionCNT = ItemOptionCNT + 1; }
                        if (System.Convert.ToUInt64(reader["optionvalue4"]) != 0) { ItemOptionCNT = ItemOptionCNT + 1; }
                        if (System.Convert.ToUInt64(reader["optionvalue5"]) != 0) { ItemOptionCNT = ItemOptionCNT + 1; }
                        items.Add(new JsonNumericValue("optioncount", ItemOptionCNT));
                        items.Add(new JsonStringValue("optiontype1", System.Convert.ToString(reader["optiontype1"])));
                        items.Add(new JsonNumericValue("optionvalue1", System.Convert.ToUInt64(reader["optionvalue1"])));
                        items.Add(new JsonStringValue("optiontype2", System.Convert.ToString(reader["optiontype2"])));
                        items.Add(new JsonNumericValue("optionvalue2", System.Convert.ToUInt64(reader["optionvalue2"])));
                        items.Add(new JsonStringValue("optiontype3", System.Convert.ToString(reader["optiontype3"])));
                        items.Add(new JsonNumericValue("optionvalue3", System.Convert.ToUInt64(reader["optionvalue3"])));
                        items.Add(new JsonStringValue("optiontype4", System.Convert.ToString(reader["optiontype4"])));
                        items.Add(new JsonNumericValue("optionvalue4", System.Convert.ToUInt64(reader["optionvalue4"])));
                        items.Add(new JsonStringValue("optiontype5", System.Convert.ToString(reader["optiontype5"])));
                        items.Add(new JsonNumericValue("optionvalue5", System.Convert.ToUInt64(reader["optionvalue5"])));
                        items.Add(new JsonNumericValue("creation_date", System.Convert.ToUInt64(reader["creation_date"])));
                        ItemOptionCNT = 0;
                        MakeJson.Add(items);
                    }
                    DBCon_Char.Close();
                    res.Add(MakeJson);

                    //Soulinventory List
                    JsonArrayCollection MakeJson2 = new JsonArrayCollection("soulinventorylist");
                    DBCon_Char.Open();
                    var command3 = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DBCon_Char, CommandText = "GetFriendSoulInven" };
                    command3.Parameters.Add("@AID", SqlDbType.BigInt).Value = retAID;//AID
                    command3.Parameters.Add("@CID", SqlDbType.BigInt).Value = retCID;//CID
                    command3.ExecuteNonQuery();
                    SqlDataReader reader2 = command3.ExecuteReader();

                    while (reader2.Read())
                    {
                        JsonObjectCollection items2 = new JsonObjectCollection();
                        items2.Add(new JsonNumericValue("soulseq", System.Convert.ToUInt16(reader2["soulseq"])));
                        items2.Add(new JsonNumericValue("cid", System.Convert.ToUInt16(reader2["CID"])));
                        items2.Add(new JsonNumericValue("soulid", System.Convert.ToUInt32(reader2["SoulID"])));
                        items2.Add(new JsonStringValue("soulname", System.Convert.ToString(reader2["SoulName"])));
                        items2.Add(new JsonNumericValue("soultype", System.Convert.ToUInt16(reader2["SoulType"])));
                        items2.Add(new JsonNumericValue("classtype", System.Convert.ToUInt16(reader2["ClassType"])));
                        items2.Add(new JsonNumericValue("grade", System.Convert.ToUInt16(reader2["Grade"])));
                        items2.Add(new JsonNumericValue("equipflag", System.Convert.ToUInt16(reader2["equipflag"])));
                        items2.Add(new JsonNumericValue("level", System.Convert.ToUInt16(reader2["Level"])));
                        items2.Add(new JsonNumericValue("levelupkillcount", System.Convert.ToUInt32(reader2["LevelUP_Kill_Count"])));
                        items2.Add(new JsonNumericValue("specialbuff1", System.Convert.ToUInt32(reader2["Special_Buff1"])));
                        items2.Add(new JsonNumericValue("specialbuff2", System.Convert.ToUInt32(reader2["Special_Buff2"])));
                        items2.Add(new JsonNumericValue("upgradegauge", System.Convert.ToUInt32(reader2["Upgrade_Gauge"])));
                        items2.Add(new JsonStringValue("newyn", System.Convert.ToString(reader2["newyn"])));
                        items2.Add(new JsonNumericValue("unique_buff", System.Convert.ToUInt32(reader2["unique_buff"])));
                        items2.Add(new JsonStringValue("activeorpassive", System.Convert.ToString(reader2["Active_Or_Passive"])));
                        MakeJson2.Add(items2);
                    }
                    DBCon_Char.Close();
                    res.Add(MakeJson2);

                    string json_text = res.ToString();
                    Response.Write(json_text);
                }
                
            }
            catch (Exception ex)
            {
                if (DBCon != null)
                    DBCon.Close();
                if (DBCon_Char != null)
                    DBCon_Char.Close();
                if (DBCon_gamedata != null)
                    DBCon_gamedata.Close();
                string reqval = "friendaid=" + Request["friendaid"];
                string reqURL = System.Convert.ToString(Request.Url);
                string savePath2 = Request.PhysicalApplicationPath;
                TheSoulWebServerErrorLog2.WriteError(savePath2, ex.Message, reqURL, reqval);
                res.Add(new JsonNumericValue("resultcode", 97));
                res.Add(new JsonStringValue("message", ex.Message));
				res.Add(new JsonStringValue("message", ex.ToString()));
                string json_text = res.ToString();
                Response.Write(json_text);
            }
        }
    }
}