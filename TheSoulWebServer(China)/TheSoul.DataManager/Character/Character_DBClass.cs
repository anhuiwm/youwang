using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager.DBClass;
using ServiceStack.Text;

namespace TheSoul.DataManager.DBClass
{
    public class Character
    {
        public long cid { get; set; }
        public long aid { get; set; }
        public string char_name { get; set; }
        public int Class { get; set; }
        public short level { get; set; }
        public int exp { get; set; }
        public int totalexp { get; set; }
        public int gamemoney { get; set; }
        public int activeslot { get; set; }
        public int passiveslot { get; set; }
        public string delflag { get; set; }
        public string equipflag { get; set; }
        public int passivesoulexp { get; set; }
        public int warpoint { get; set; }
        public List<User_Inven> equiplist { get; set; }
        public List<User_Ultimate_Inven> equip_ultimate { get; set; }

        public Character()
        {
            char_name = "";
            delflag = "N";
            equipflag = "N";
            equiplist = new List<User_Inven>();
            equip_ultimate = new List<User_Ultimate_Inven>();
        }
    }

    public class Character_Detail : Character
    {
        public List<Ret_Equip_Soul_Active> equip_active_soul { get; set; }
        public List<Ret_Soul_Passive> equip_passive_soul { get; set; }

        public Character_Detail()
        {
            char_name = "";
            equip_active_soul = new List<Ret_Equip_Soul_Active>();
            equip_passive_soul = new List<Ret_Soul_Passive>();
            delflag = "N";
            equipflag = "N";
            equiplist = new List<User_Inven>();
        }
        public Character_Detail(Character setInfo)
        {
            cid = setInfo.cid;
            aid = setInfo.aid;
            char_name = setInfo.char_name;
            Class = setInfo.Class;
            level = setInfo.level;
            exp = setInfo.exp;
            totalexp = setInfo.totalexp;
            gamemoney = setInfo.gamemoney;
            activeslot = setInfo.activeslot;
            passiveslot = setInfo.passiveslot;
            passivesoulexp = setInfo.passivesoulexp;
            warpoint = setInfo.warpoint;
            delflag = setInfo.delflag;
            //creation_date = setInfo.creation_date;
            equipflag = setInfo.equipflag;
        }

        public static string makeCharacter_DetailListJson(ref List<Character_Detail> setList)
        {
            JsonArrayObjects json = new JsonArrayObjects();
            const string removeKeyEquip = "equiplist";
            const string removeKeyUltimate = "equip_ultimate";

            setList.ForEach(item =>
            {
                JsonObject setObjJson = JsonObject.Parse(mJsonSerializer.ToJsonString(item));
                if (setObjJson.ContainsKey(removeKeyEquip))
                    setObjJson.Remove(removeKeyEquip);
                if (setObjJson.ContainsKey(removeKeyUltimate))
                    setObjJson.Remove(removeKeyUltimate);

                setObjJson = mJsonSerializer.AddJson(setObjJson, removeKeyEquip, Ret_Inven_Item.makeInvenListJson(item.equiplist));
                setObjJson = mJsonSerializer.AddJson(setObjJson, removeKeyUltimate, User_Ultimate_Inven.ToJsonList(item.equip_ultimate));

                json = mJsonSerializer.AddJsonArray(json, setObjJson);
            }
            );

            return json.ToJson();
        }

        public string ToJson()
        {
            //string setObjJson = "";
            //setObjJson = mJsonSerializer.RemoveJson(mJsonSerializer.ToJsonString(this), "equiplist");
            //setObjJson = mJsonSerializer.AddJson(setObjJson, "equiplist", Ret_Inven_Item.makeInvenListJson(equiplist));

            //return setObjJson;
            const string removeKeyEquip = "equiplist";
            JsonObject setObjJson = JsonObject.Parse(mJsonSerializer.ToJsonString(this));
            if (setObjJson.ContainsKey(removeKeyEquip))
                setObjJson.Remove(removeKeyEquip);

            setObjJson = mJsonSerializer.AddJson(setObjJson, removeKeyEquip, Ret_Inven_Item.makeInvenListJson(equiplist));

            return setObjJson.ToJson();
        }
    }

    public class Character_Detail_With_HP : Character_Detail
    {
        public int curhp { get; set; }
        public int maxhp { get; set; }
        public int killcount { get; set; }

        public Character_Detail_With_HP()
        {
            char_name = "";
            equip_active_soul = new List<Ret_Equip_Soul_Active>();
            equip_passive_soul = new List<Ret_Soul_Passive>();
            delflag = "N";
            equipflag = "N";
            equiplist = new List<User_Inven>();
        }

        public Character_Detail_With_HP(Character setInfo, int setCurHP = 0, int setMaxHP = 0, int setKillCount = 0)
        {
            cid = setInfo.cid;
            aid = setInfo.aid;
            char_name = setInfo.char_name;
            Class = setInfo.Class;
            level = setInfo.level;
            exp = setInfo.exp;
            totalexp = setInfo.totalexp;
            gamemoney = setInfo.gamemoney;
            activeslot = setInfo.activeslot;
            passiveslot = setInfo.passiveslot;
            delflag = setInfo.delflag;
            //creation_date = setInfo.creation_date;
            equipflag = setInfo.equipflag;
            curhp = setCurHP;
            maxhp = setMaxHP;
            killcount = setKillCount;
            warpoint = setInfo.warpoint;
        }


        public static string makeCharacter_DetailListJson(ref List<Character_Detail_With_HP> setList)
        {
            JsonArrayObjects json = new JsonArrayObjects();
            const string removeKeyEquip = "equiplist";

            setList.ForEach(item =>
            {
                JsonObject setObjJson = JsonObject.Parse(mJsonSerializer.ToJsonString(item));
                if (setObjJson.ContainsKey(removeKeyEquip))
                    setObjJson.Remove(removeKeyEquip);

                setObjJson = mJsonSerializer.AddJson(setObjJson, removeKeyEquip, Ret_Inven_Item.makeInvenListJson(item.equiplist));
                json = mJsonSerializer.AddJsonArray(json, setObjJson);
            }
            );

            return json.ToJson();
            //string json = "";

            //setList.ForEach(item =>
            //{
            //    string setObjJson = "";
            //    setObjJson = mJsonSerializer.RemoveJson(mJsonSerializer.ToJsonString(item), "equiplist");
            //    setObjJson = mJsonSerializer.AddJson(setObjJson, "equiplist", Ret_Inven_Item.makeInvenListJson(item.equiplist));

            //    json = mJsonSerializer.AddJsonArray(json, setObjJson);
            //}
            //);

            //return json;
        }

        public static string makeCharacter_DetailListJson(List<Character_Detail_With_HP> setList)
        {
            JsonArrayObjects json = new JsonArrayObjects();
            const string removeKeyEquip = "equiplist";

            setList.ForEach(item =>
            {
                JsonObject setObjJson = JsonObject.Parse(mJsonSerializer.ToJsonString(item));
                if (setObjJson.ContainsKey(removeKeyEquip))
                    setObjJson.Remove(removeKeyEquip);

                setObjJson = mJsonSerializer.AddJson(setObjJson, removeKeyEquip, Ret_Inven_Item.makeInvenListJson(item.equiplist));
                json = mJsonSerializer.AddJsonArray(json, setObjJson);
            }
            );

            return json.ToJson();

            //string json = "";

            //setList.ForEach(item =>
            //{
            //    string setObjJson = "";
            //    setObjJson = mJsonSerializer.RemoveJson(mJsonSerializer.ToJsonString(item), "equiplist");
            //    setObjJson = mJsonSerializer.AddJson(setObjJson, "equiplist", Ret_Inven_Item.makeInvenListJson(item.equiplist));

            //    json = mJsonSerializer.AddJsonArray(json, setObjJson);
            //}
            //);

            //return json;
        }
    }

    public class Character_Simple
    {
        public long cid { get; set; }
        public long aid { get; set; }
        public int Class { get; set; }
        public short level { get; set; }
        public string equipflag { get; set; }

        public Character_Simple() { }

        public Character_Simple(Character setInfo)
        {
            if (setInfo != null)
            {
                cid = setInfo.cid;
                aid = setInfo.aid;
                Class = setInfo.Class;
                level = setInfo.level;
                equipflag = setInfo.equipflag;
            }
        }
    }

    public class Character_Simple_With_Equip : Character_Simple
    {
        public List<User_Inven> equiplist { get; set; }
        public List<User_Ultimate_Inven> equip_ultimate { get; set; }
        public Character_Simple_With_Equip(Character setInfo, List<User_Inven> setEquip)
        {
            if (setInfo != null && setEquip != null)
            {
                cid = setInfo.cid;
                aid = setInfo.aid;
                Class = setInfo.Class;
                level = setInfo.level;
                equipflag = setInfo.equipflag;
                equiplist = setEquip;
                equip_ultimate = setInfo.equip_ultimate;
            }
        }

        public string ToJson()
        {
            const string removeKeyEquip = "equiplist";
            JsonObject setObjJson = JsonObject.Parse(mJsonSerializer.ToJsonString(this));

            if (setObjJson.ContainsKey(removeKeyEquip))
                setObjJson.Remove(removeKeyEquip);

            setObjJson = mJsonSerializer.AddJson(setObjJson, removeKeyEquip, Ret_Inven_Item.makeInvenListJson(equiplist));

            return setObjJson.ToJson();

            //string setObjJson = "";
            //setObjJson = mJsonSerializer.RemoveJson(mJsonSerializer.ToJsonString(this), "equiplist");
            //setObjJson = mJsonSerializer.AddJson(setObjJson, "equiplist", Ret_Inven_Item.makeInvenListJson(equiplist));

            //return setObjJson;
        }
    }

    public class System_PC_BASE
    {
        public int PC_Code { get; set; }
        public int Base_Gold { get; set; }
        public int Base_Cash { get; set; }
        public int Base_Weapon { get; set; }
        public int Base_Helmet { get; set; }
        public int Base_Armor { get; set; }
        public int Base_Glove { get; set; }
        public int Base_Shoes { get; set; }
        public int Base_Active_Hon1 { get; set; }
        public int Base_Active_Hon2 { get; set; }
        public int Base_Active_Hon3 { get; set; }
        public int Base_Active_Hon4 { get; set; }
        public int Base_Passive_Hon1 { get; set; }
        public int Base_Passive_Hon2 { get; set; }
        public int Base_Passive_Hon3 { get; set; }
        public int Base_Passive_Hon4 { get; set; }
        public int Base_Passive_Hon5 { get; set; }
        public int Base_Passive_Hon6 { get; set; }
        public int Base_Hon1 { get; set; }
        public int Base_Hon2 { get; set; }
        public int Base_Hon3 { get; set; }
        public int Base_Hon4 { get; set; }
        public int Base_Hon5 { get; set; }
        public int Base_HPPOTION_NUM { get; set; }
        public int Base_MPPOTION_NUM { get; set; }
        public int Base_RevivalStone_NUM { get; set; }
        public byte MaxLevel { get; set; }
    }

    public class System_Character_EXP
    {
        public short Level { get; set; }
        public int PCLevelUpEXP { get; set; }
        public int ACC_exp { get; set; }
        public int LevelEXP { get; set; }
    }

    public class User_CharacterGroup
    {
        public long aid { get; set; }
        public long cid1 { get; set; }
        public long cid2 { get; set; }
        public long cid3 { get; set; }
    }

    public class User_GE_CharacterGroup
    {
        public long aid { get; set; }
        public long cid1 { get; set; }
        public long cid2 { get; set; }
        public long cid3 { get; set; }
        public long cid4 { get; set; }
    }

    public class Character_Stat // calc by CS server
    {
        public long CID { get; set; }
        public int BaseHP { get; set; }
        public int BaseMP { get; set; }
        public int HP { get; set; }
        public int HPMax { get; set; }
        public int MP { get; set; }
        public int MPMax { get; set; }
        public int ATTACK_MIN { get; set; }
        public int ATTACK_MAX { get; set; }
        public int EXP { get; set; }
        public int LEVEL { get; set; }

        public int DEFENCE_POWER { get; set; }
        public float CPR { get; set; }
        public float CRITICAL { get; set; }
        public float CRITICAL_PROTECTION { get; set; }
        public float CRITICAL_RATING { get; set; }
        public float MAX_CRITICAL_PROP { get; set; }
        public float DEFENCE_CRITICAL_RATING { get; set; }
        public int DEFENCE_POINT { get; set; }
        public int WAR_POINT { get; set; }
        public int ACTIVE_SOUL_WAR_POINT { get; set; }
        public int PASSIVE_SOUL_WAR_POINT { get; set; }

        public int MAX_WAR_POINT { get; set; }
    }
}
