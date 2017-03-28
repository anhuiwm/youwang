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
    public class System_Item_Base
    {
        public long Item_IndexID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ItemClass { get; set; }
        public short ClassNo { get; set; }
        public long Class_IndexID { get; set; }
        public string Buy_PriceType { get; set; }///无用
        public int Buy_PriceValue { get; set; }///无用
        public int Sell_Money { get; set; }
        public string Icon_Resource { get; set; }
        public string Item_Icon_Atlas { get; set; }
        public int StackMAX { get; set; }
    }
    
    public class System_Item_Equip : System_Item_Base
    {
        public long EquipItem_IndexID { get; set; }
        public string ItemType { get; set; }
        public string EquipPosition { get; set; }
        public string GrowthType { get; set; }
        public long GrowthTableID1 { get; set; }
        public long GrowthTableID2 { get; set; }
        public string EquipClass { get; set; }
        public short EquipLevel { get; set; }
        public short Tier { get; set; }
    }

    public class System_Item_Use : System_Item_Base
    {
        public long UseItem_IndexID { get; set; }
        public string ItemType { get; set; }
        public short TierGroup { get; set; }
        public long Count { get; set; }
        public string TargetCondition { get; set; }
        public long Target_IndexID { get; set; }
        public string UsingMethod { get; set; }
        public short UseLevel { get; set; }
        public string UseCondition { get; set; }
        public int ConditionValue { get; set; }
    }


    public class System_Item_Info : System_Item_Base
    {
        public long InfoItem_IndexID { get; set; }
        public string DEV_Description { get; set; }
        public string ItemType { get; set; }
        public int ItemType_Value { get; set; }
        public string BagPosition { get; set; }
    }

    public class System_Item_Costume : System_Item_Base
    {
        public long CostumeItem_IndexID { get; set; }
        public string StringCN_Tooltip { get; set; }
        public string ItemType { get; set; }
        public string EquipPosition { get; set; }
        public long FixOptionID1 { get; set; }
        public long FixOptionID2 { get; set; }
        public long FixOptionID3 { get; set; }
        public string EquipClass { get; set; }
        public string Resource_Model_Wear { get; set; }
        public string Resource_Texture_Wear { get; set; }
        public string Resource_Model_Weapon { get; set; }
        public string Resource_Texture_Weapon { get; set; }
        public string Item_Resource { get; set; }
        public string DEV_Description { get; set; }
    }


    public class System_Item_Band
    {
        public long BindItem_IndexID { get; set; }
        public string DEV_Description { get; set; }
        public long Item_IndexID1 { get; set; }
        public long Item_IndexID2 { get; set; }
        public long Item_IndexID3 { get; set; }
        public long Item_IndexID4 { get; set; }
        public long Item_IndexID5 { get; set; }
        public long Item_IndexID6 { get; set; }
        public long Item_IndexID7 { get; set; }
        public long Item_IndexID8 { get; set; }
    }

    public class System_Item_Option
    {
        public long Option_IndexID { get; set; }
        public string OptionType { get; set; }
        public string OptionUse { get; set; }
        public string ParameterType { get; set; }
        public short OptionValue_Tier { get; set; }
        public short OptionValue_Grade { get; set; }
        public string OptionClass { get; set; }
        public string OptionEquip { get; set; }
        public int OptionValue1 { get; set; }
        public int OptionValue2 { get; set; }
    }

    public class System_Item_Option_UseInfo
    {
        public long Option_Useinfo_IndexID { get; set; }
        public string DEV_Description { get; set; }
        public string ItemType { get; set; }
        public string ParameterType { get; set; }
        public string FixOption { get; set; }
        public string UseAvailability { get; set; }
    }

    public class System_Item_TierInfo
    {
        public int Level { get; set; }
        public int Tier { get; set; }
    }

    public class System_Item_Enchant
    {
        public long Enchant_IndexID { get; set; }
        public int Count { get; set; }
        public string EnchantType { get; set; }
        public int EnchantGroup { get; set; }
        public short Tier { get; set; }
        public int EnchantLevel { get; set; }
        public int Enchant_Rate { get; set; }
        public int GradeUP_NeedGold { get; set; }
        public long EnchantUP_NeedItemID1 { get; set; }
        public int EnchantUP_NeedItemCount1 { get; set; }
        public long EnchantUP_NeedItemID2 { get; set; }
        public int EnchantUP_NeedItemCount2 { get; set; }
        public int EnchantUP_SuccessRate { get; set; }
        public int AbilityLevel { get; set; }
        public string DEV_Description { get; set; }
    }

    public class System_Item_Enchant_Armor
    {
        public long Enchant_IndexID { get; set; }
        public short Count { get; set; }
        public int EnchantGroup { get; set; }
        public short EnchantGrade { get; set; }
        public short EnchantLevel { get; set; }
        public string MainParameter_Type { get; set; }
        public int MainParameter_Value1 { get; set; }
        public string SubParameter_Type { get; set; }
        public int SubParameter_Value1 { get; set; }
        public int NeedLevel { get; set; }
        public int NeedGold { get; set; }
        public long NeedItem { get; set; }
        public int ItemCount { get; set; }
        public long NextItem { get; set; }
        public int AbilityLevel { get; set; }
        public string Resource_Model { get; set; }
        public string Resource_Texture { get; set; }
        public string Item_Icon_Atlas { get; set; }
        public string Item_Resource { get; set; }
        public string DEV_Description { get; set; }
    }

    public class System_Item_Grade_Accessory
    {
        public long Accessory_IndexID { get; set; }
        public string DEV_Description { get; set; }
        public string ItemType { get; set; }
        public long Count { get; set; }
        public long AcceGroup { get; set; }
        public short Tier { get; set; }
        public short Grade { get; set; }
        public long FixOptionID1 { get; set; }
        public int OptionGold_One { get; set; }
        //public int OptionGold_All { get; set; }
        public string Item_Icon_Atlas { get; set; }
        public string Item_Resource { get; set; }
    }

    public class System_Item_Grade_Weapon
    {
        public long Weapon_IndexID { get; set; }
        public string DEV_Description { get; set; }
        public int Count { get; set; }
        public int WeaponGroup { get; set; }
        public short Tier { get; set; }
        public short Grade { get; set; }
        public int minAP_Fix { get; set; }
        public int maxAP_Fix { get; set; }
        public long Disassemble_MakeItemID { get; set; }
        public int Disassemble_MakeItemCount { get; set; }
        public string Resource_Model { get; set; }
        public string Resource_Texture { get; set; }
        public string Item_Icon_Atlas { get; set; }
        public string Item_Resource { get; set; }
    }

    public class System_Item_Refining_Weapon
    {
        public long Refining_IndexID { get; set; }
        public string DEV_Description { get; set; }
        public string RefiningType { get; set; }
        public short Tier { get; set; }
        public short RefiningLevel { get; set; }
        public int NeedEXP { get; set; }
        public int GetEXP_min { get; set; }
        public int GetEXP_max { get; set; }
        public long NeedItem1 { get; set; }
        public int NeedItem1_Count { get; set; }
        public long NeedItem2 { get; set; }
        public int NeedItem2_Count { get; set; }
        public int RefiningUP_Value { get; set; }
    }

    public class System_Item_Cape
    {
        public long Cape_IndexID { get; set; }
        public string DEV_Description { get; set; }
        public short Tier { get; set; }
        public int DFP_Fix { get; set; }
        public string Resource_Model { get; set; }
        public string Resource_Texture { get; set; }
        public string Item_Icon_Atlas { get; set; }
        public string Item_Resource { get; set; }
        public long Disassemble_MakeItemID { get; set; }
        public int Disassemble_MakeItemCount { get; set; }
    }

    public class System_Item_Tier_SetInfo
    {
        public long TierSet_IndexID { get; set; }
        public string DEV_Description { get; set; }
        public string SetType { get; set; }
        public short Tier { get; set; }
        public short Grade { get; set; }
        public short Count { get; set; }
        public long Option_ID { get; set; }
    }

    public class User_Inven_Count
    {
        public long Account_Inven_Count { get; set; }
        public long Character_Inven_Count { get; set; }
    }

    public class User_Item_Enchant
    {
        public long invenseq { get; set; }
        public long use_itemid { get; set; }
        public int use_count { get; set; }
    }
    
    public class User_Inven
    {
        public long invenseq { get; set; }
        public short inventory_type { get; set; }
        public long aid { get; set; }
        public long cid { get; set; }
        public byte class_type { get; set; }
        public long itemid { get; set; }
        public int itemea { get; set; }
        public short item_type { get; set; }
        public short grade { get; set; }
        public short level { get; set; }
        public string equipflag { get; set; }
        public string newyn { get; set; }
        public string delflag { get; set; }
        public string equipposition { get; set; }
        public DateTime creation_date { get; set; }
        public List<User_Inven_Option> base_option { get; set; }
        public List<User_Inven_Option> random_option { get; set; }

        
        public User_Inven() {
            equipflag = "N";
            newyn = "N";
            delflag = "N";
            equipposition = "";
            creation_date = DateTime.Now;
            base_option = new List<User_Inven_Option>();
            random_option = new List<User_Inven_Option>();
        }

        public User_Inven(long setItemID, int setItemEa)
        {
            itemid = setItemID;
            itemea = setItemEa;
            equipflag = "N";
            newyn = "N";
            delflag = "N";
            equipposition = "";
            creation_date = DateTime.Now;
            base_option = new List<User_Inven_Option>();
            random_option = new List<User_Inven_Option>();
        }

        public User_Inven(long setItemID, int setItemEa, short setGrade, short setLevel)
        {
            itemid = setItemID;
            itemea = setItemEa;
            grade = setGrade;
            level = setLevel;
            equipflag = "N";
            newyn = "N";
            delflag = "N";
            equipposition = "";
            creation_date = DateTime.Now;
            base_option = new List<User_Inven_Option>();
            random_option = new List<User_Inven_Option>();
        }

        public User_Inven(User_Orb_Inven setItem)
        {
            invenseq = setItem.orb_inven_seq;
            itemid = setItem.orb_id;
            itemea = 1;
            equipflag = "N";
            newyn = "N";
            delflag = "N";
            equipposition = "";
            creation_date = setItem.creation_date;
            base_option = setItem.orb_option;
            random_option = new List<User_Inven_Option>();
        }
    }

    public class User_Inven_Option
    {
        public long optionseq { get; set; }
        public long invenseq { get; set; }
        public string isbase { get; set; }
        public string optiontype { get; set; }
        public int option_value { get; set; }
        public int option_add_value { get; set; }
        public byte option_grade { get; set; }
        public byte option_level { get; set; }
        public int option_exp { get; set; }
        public string delflag { get; set; }

        public User_Inven_Option() { }
        public User_Inven_Option(System_Item_Option setOption, int setValue, bool isBaseOption = false)
        {
            isbase = isBaseOption ? "Y" : "N";
            optiontype = setOption.OptionType;
            option_value = setValue;
            delflag = "N";
        }

        public User_Inven_Option(string setOption, int setValue, bool isBaseOption = false)
        {
            isbase = isBaseOption ? "Y" : "N";
            optiontype = setOption;
            option_value = setValue;
            delflag = "N";
        }

        public static void CopyOption(ref User_Inven_Option baseOption, ref User_Inven_Option SourceOption)
        {
            baseOption.optiontype = SourceOption.optiontype;
            baseOption.option_value = SourceOption.option_value;
            baseOption.option_add_value = SourceOption.option_add_value;
            baseOption.option_grade = SourceOption.option_grade;
            baseOption.option_level = SourceOption.option_level;
            baseOption.option_exp = SourceOption.option_exp;
        }

        //public static string SetOptionJson(User_Inven_Option item)
        //{
        //    string setOptionjson = "";
        //    setOptionjson = mJsonSerializer.AddJson(setOptionjson, "optiontype", item.optiontype.ToString());
        //    setOptionjson = mJsonSerializer.AddJson(setOptionjson, "option_value", item.option_value.ToString());
        //    return setOptionjson;
        //}
        
        public static JsonObject SetOptionJson(User_Inven_Option item)
        {
            JsonObject setOptionjson = new JsonObject();
            setOptionjson.Add("optiontype", item.optiontype.ToString());
            setOptionjson.Add("option_value", item.option_value.ToString());
            //string setOptionjson = "";
            //setOptionjson = mJsonSerializer.AddJson(setOptionjson, "optiontype", item.optiontype.ToString());
            //setOptionjson = mJsonSerializer.AddJson(setOptionjson, "option_value", item.option_value.ToString());
            return setOptionjson;
        }
    }

    public class Ret_Inven_Item
    {
        public long invenseq { get; set; }
        public short inventory_type { get; set; }
        public long aid { get; set; }
        public long cid { get; set; }
        public byte class_type { get; set; }
        public long itemid { get; set; }
        public int itemea { get; set; }
        public short item_type { get; set; }
        public short grade { get; set; }
        public short level { get; set; }
        public string equipflag { get; set; }
        public string newyn { get; set; }
        public string equipposition { get; set; }
        public List<User_Inven_Option> base_option { get; set; }
        public List<User_Inven_Option> random_option { get; set; }


        public Ret_Inven_Item(User_Inven setitem, bool bWithOptionLevel = false)
        {
            invenseq = setitem.invenseq;
            inventory_type = setitem.inventory_type;
            aid = setitem.aid;
            cid = setitem.cid;
            class_type = setitem.class_type;
            itemid = setitem.itemid;
            itemea = setitem.itemea;
            item_type = setitem.item_type;
            grade = setitem.grade;
            level = setitem.level;
            equipflag = setitem.equipflag != null ? setitem.equipflag : "N";
            newyn = setitem.newyn != null ? setitem.newyn : "N";
            equipposition = setitem.equipposition != null ? setitem.equipposition : "";
            base_option = setitem.base_option != null ? setitem.base_option : new List<User_Inven_Option>();
            random_option = setitem.random_option != null ? setitem.random_option : new List<User_Inven_Option>();

            //if (setitem.base_option != null)
            //    base_option = setitem.base_option;
            //else
            //    base_option = new List<User_Inven_Option>();

            //if (setitem.random_option != null)
            //    random_option = setitem.random_option;
            //else
            //    random_option = new List<User_Inven_Option>();
        }

        public string ToJson(bool bWithOptionLevel = false)
        {
            JsonObject setJson = new JsonObject();
            setJson = mJsonSerializer.AddJson(setJson, "invenseq", invenseq.ToString());
            setJson = mJsonSerializer.AddJson(setJson, "inventory_type", inventory_type.ToString());
            setJson = mJsonSerializer.AddJson(setJson, "aid", aid.ToString());
            setJson = mJsonSerializer.AddJson(setJson, "cid", cid.ToString());
            setJson = mJsonSerializer.AddJson(setJson, "class_type", class_type.ToString());
            setJson = mJsonSerializer.AddJson(setJson, "itemid", itemid.ToString());
            setJson = mJsonSerializer.AddJson(setJson, "itemea", itemea.ToString());
            setJson = mJsonSerializer.AddJson(setJson, "item_type", item_type.ToString());
            setJson = mJsonSerializer.AddJson(setJson, "grade", grade.ToString());
            setJson = mJsonSerializer.AddJson(setJson, "level", level.ToString());
            setJson = mJsonSerializer.AddJson(setJson, "equipflag", equipflag.ToString());
            setJson = mJsonSerializer.AddJson(setJson, "newyn", newyn.ToString());
            setJson = mJsonSerializer.AddJson(setJson, "equipposition", equipposition.ToString());

            JsonArrayObjects baseOptionjson = new JsonArrayObjects();
            base_option.ForEach(item =>
            {
                baseOptionjson = mJsonSerializer.AddJsonArray(baseOptionjson, SetOptionJson(item, bWithOptionLevel));
            }
            );

            JsonArrayObjects randomOptionjson = new JsonArrayObjects();
            random_option.ForEach(item =>
            {
                randomOptionjson = mJsonSerializer.AddJsonArray(randomOptionjson, SetOptionJson(item, bWithOptionLevel));
            }
            );

            setJson = mJsonSerializer.AddJson(setJson, "base_option", baseOptionjson.ToJson());
            setJson = mJsonSerializer.AddJson(setJson, "random_option", randomOptionjson.ToJson());

            return setJson.ToJson();
        }

        private string SetOptionJson(User_Inven_Option item, bool bWithOptionLevel = false)
        {
            JsonObject setOptionjson = new JsonObject();
            setOptionjson = mJsonSerializer.AddJson(setOptionjson, "optionseq", item.optionseq.ToString());
            setOptionjson = mJsonSerializer.AddJson(setOptionjson, "optiontype", item.optiontype.ToString());
            setOptionjson = mJsonSerializer.AddJson(setOptionjson, "option_value", item.option_value.ToString());

            if (item.option_grade > 0 || bWithOptionLevel)
                setOptionjson = mJsonSerializer.AddJson(setOptionjson, "option_grade", item.option_grade.ToString());

            if (item.option_add_value > 0 || bWithOptionLevel)
                setOptionjson = mJsonSerializer.AddJson(setOptionjson, "option_add_value", item.option_add_value.ToString());

            if (bWithOptionLevel)
            {
                setOptionjson = mJsonSerializer.AddJson(setOptionjson, "option_level", item.option_level.ToString());
                setOptionjson = mJsonSerializer.AddJson(setOptionjson, "option_exp", item.option_exp.ToString());
            }

            return setOptionjson.ToJson();
        }

        public static string makeInvenListJson(ref List<User_Inven> setList)
        {
            string json = "";

            setList.ForEach(item =>
            {
                json = mJsonSerializer.AddJsonArray(json, new Ret_Inven_Item(item).ToJson());
            }
            );

            return json;
        }

        public static string makeInvenListJson(List<User_Inven> setList)
        {
            JsonArrayObjects json = new JsonArrayObjects();

            setList.ForEach(item =>
            {
                json = mJsonSerializer.AddJsonArray(json, new Ret_Inven_Item(item).ToJson());
            }
            );

            return json.ToJson();
        }
    }


    public class Return_DisassableItems
    {
        public long makeitemid { get; set; }
        public long makeitemcount { get; set; }
        public Return_DisassableItems_List[] makeitemlist { get; set; }
    }

    public class Return_DisassableItems_List
    {
        public long itemseq { get; set; }
        public long itemid { get; set; }
        public int itemea { get; set; }

        public Return_DisassableItems_List() { }
        public Return_DisassableItems_List(User_Inven setItem)
        {
            itemseq = setItem.invenseq;
            itemid = setItem.itemid;
            itemea = setItem.itemea;
        }
    }

    // for drop table
    public class System_Drop_Box_Group
    {
        public long DropBoxGroupID { get; set; }
        public string Description { get; set; }
        public string DropBoxType { get; set; }
        public long DropBox1ID { get; set; }
        public long DropBox2ID { get; set; }
        public long DropBox3ID { get; set; }
        public long DropBox4ID { get; set; }
    }

    public class System_Drop_Box
    {
        public int DropBoxID { get; set; }
        public string Description { get; set; }
        public string DropGroup1Type { get; set; }
        public int DropGroup1ID { get; set; }
        public short DropGroup1_Dropnum { get; set; }
        public string DropGroup2Type { get; set; }
        public int DropGroup2ID { get; set; }
        public short DropGroup2_Dropnum { get; set; }
        public string DropGroup3Type { get; set; }
        public int DropGroup3ID { get; set; }
        public short DropGroup3_Dropnum { get; set; }
        public string DropGroup4Type { get; set; }
        public int DropGroup4ID { get; set; }
        public short DropGroup4_Dropnum { get; set; }
        public string DropGroup5Type { get; set; }
        public int DropGroup5ID { get; set; }
        public short DropGroup5_Dropnum { get; set; }
    }

    public class System_Drop_Group
    {
        public long DropGroupIndex { get; set; }
        public long DropGroupID { get; set; }
        public int DropIndex { get; set; }
        public string DropTargetType { get; set; }
        public long DropItemID { get; set; }
        public int DropItemLevel { get; set; }
        public int DropItemGrade { get; set; }
        public int DropMinNum { get; set; }
        public int DropMaxNum { get; set; }
        public int DropProb { get; set; }
        public int AddProb { get; set; }
        public int MaxProb { get; set; }
        public int ProbID { get; set; }

        public System_Drop_Group() { }
        public System_Drop_Group(RetShopTreasureBoxItem setItem)
        {
            DropTargetType = "Item";
            DropItemID = setItem.itemid;
            DropItemLevel = setItem.level;
            DropItemGrade = setItem.grade;
            DropMinNum = setItem.amount;
            DropMaxNum = setItem.amount;
        }
    }

    // user drop rate compensate info
    public class User_Drop_AddProb_Info
    {
        public long AID { get; set; }
        public long ProbID { get; set; }
        public int AddProb { get; set; }
    }

    public class User_Character_VIP_Costume
    {
        public long aid { get; set; }
        public long cid { get; set; }
        public string equipflag { get; set; }
    }

    // ultimate weapon contents
    public class System_Ultimate_Weapon : System_Item_Base
    {
        public long Ultimate_ID { get; set; }
        //public string Description { get; set; }
        public string ItemType { get; set; }
        public short EquipClass { get; set; }
        public short Tier { get; set; }
        public string Trigger1_Name { get; set; }
        public string Trigger2_Name { get; set; }
        public string Trigger3_Name { get; set; }
        public string Trigger4_Name { get; set; }
        public string VIPTrigger_Name { get; set; }
        public string ActiveTriggerType1 { get; set; }
        public int ActiveTriggerType1_Value1 { get; set; }
        public int ActiveTriggerType1_Value2 { get; set; }
        public int ActiveTriggerType1_Value3 { get; set; }
        public string ActiveTriggerType2 { get; set; }
        public int ActiveTriggerType2_Value1 { get; set; }
        public int ActiveTriggerType2_Value2 { get; set; }
        public int ActiveTriggerType2_Value3 { get; set; }
        public string ActiveTriggerType3 { get; set; }
        public int ActiveTriggerType3_Value1 { get; set; }
        public int ActiveTriggerType3_Value2 { get; set; }
        public int ActiveTriggerType3_Value3 { get; set; }
        public string ActiveTriggerType4 { get; set; }
        public int ActiveTriggerType4_Value1 { get; set; }
        public int ActiveTriggerType4_Value2 { get; set; }
        public int ActiveTriggerType4_Value3 { get; set; }
        public string Beyond_ActiveTriggerType { get; set; }
        public int Beyond_ActiveTriggerType_Value1 { get; set; }
        public int Beyond_ActiveTriggerType_Value2 { get; set; }
        public int Beyond_ActiveTriggerType_Value3 { get; set; }
        public int OrbSlot_Lv1 { get; set; }
        public int OrbSlot_Lv2 { get; set; }
        public int OrbSlot_Lv3 { get; set; }
        public int OrbSlot_Lv4 { get; set; }
        public int OrbSlot_Lv5 { get; set; }
        public int OrbSlot_Lv6 { get; set; }
        public string Resource_Model { get; set; }
        public string Resource_Texture { get; set; }
        //public string Item_Icon_Atlas { get; set; }
        public string Item_Resource { get; set; }

        public System_Ultimate_Weapon() { ItemType = Item_Define.UltimateWeaponItemType; }
    }

    public class System_Ultimate_Enchant
    {
        public long Ultimate_Enchant_Index { get; set; }
        public long Ultimate_ID { get; set; }
        public string Description { get; set; }
        public short DEV_Class { get; set; }
        public short DEV_Tier { get; set; }
        public short EnchantLevel { get; set; }
        public short Step { get; set; }
        public long Option_ID1 { get; set; }
        public long Option_ID2 { get; set; }
        public long Option_ID3 { get; set; }
        public long Option_ID4 { get; set; }
        public int Require_NeedGold { get; set; }
        public int Require_NeedRuby { get; set; }
        public int Require_NeedCLv { get; set; }
        public long Require_NeedItemID { get; set; }
        public int Require_NeedItemNum { get; set; }
        public long NextItemID { get; set; }
    }

    public class System_Ultimate_Orb : System_Item_Base
    {
        public long Orb_Item_ID { get; set; }
        //public string Description { get; set; }
        public string ItemType { get; set; }
        public short OrbLevel { get; set; }
        public int Base_EXP { get; set; }
        public int LevelUp_EXP { get; set; }
        public long NextItemID { get; set; }
        public long Option_ID1 { get; set; }
        public long Option_ID2 { get; set; }
        public long Option_ID3 { get; set; }
        public byte Orb_Type { get; set; }
        public System_Ultimate_Orb() { ItemType = Item_Define.UltimateOrbItemType; }
    }

    public class User_Ultimate_Inven
    {
        public long ultimate_inven_seq { get; set; }
        public long aid { get; set; }
        public long cid { get; set; }
        public byte class_type { get; set; }
        public long item_id { get; set; }
        public short level { get; set; }
        public short step { get; set; }
        public string equipflag { get; set; }
        public DateTime creation_date { get; set; }
        public List<User_Inven_Option> option_list { get; set; }
        public List<User_Orb_Inven> orb_slot { get; set; }

        public User_Ultimate_Inven()
        {
            creation_date = DateTime.Now;
            equipflag = "N";
            option_list = new List<User_Inven_Option>();
            orb_slot = new List<User_Orb_Inven>();
        }

        public string ToJson()
        {
            JsonObject setJson = JsonObject.Parse(mJsonSerializer.ToJsonString(this));
            setJson.Remove("creation_date");
            setJson.Remove("option_list");
            setJson.Remove("orb_slot");
            //string setJson = mJsonSerializer.ToJsonString(this);
            //setJson = mJsonSerializer.RemoveJson(setJson, "creation_date");
            //setJson = mJsonSerializer.RemoveJson(setJson, "option_list");
            //setJson = mJsonSerializer.RemoveJson(setJson, "orb_slot");

            JsonArrayObjects optionjson = new JsonArrayObjects(); 
            option_list.ForEach(item =>
            {
                optionjson.Add(User_Inven_Option.SetOptionJson(item));
            }
            );

            JsonArrayObjects orbjson = new JsonArrayObjects(); 

            orb_slot.ForEach(item =>
            {
                orbjson.Add(item.ToJsonObj());
            }
            );
            setJson.Add("option_list", optionjson.ToJson());
            setJson.Add("orb_slot", orbjson.ToJson());

            //setJson = mJsonSerializer.AddJson(setJson, "option_list", optionjson);
            //setJson = mJsonSerializer.AddJson(setJson, "orb_slot", orbjson);

            return setJson.ToJson();
        }

        public static string ToJsonList(List<User_Ultimate_Inven> setList)
        {
            string json = "[]";
            setList.ForEach(item =>
            {
                json = mJsonSerializer.AddJsonArray(json, item.ToJson());
            }
            );
            return json;
        }
    }
    
    public class User_Orb_Inven
    {
        public long orb_inven_seq { get; set; }
        public long aid { get; set; }
        public long orb_id { get; set; }
        public int exp { get; set; }
        public long ultimate_inven_seq { get; set; }
        public byte slot_num { get; set; }
        public string delflag { get; set; }
        public DateTime creation_date { get; set; }
        public List<User_Inven_Option> orb_option { get; set; }

        public User_Orb_Inven()
        {
            creation_date = DateTime.Now;
            orb_option = new List<User_Inven_Option>();
        }

        public JsonObject ToJsonObj()
        {
            //string setJson = mJsonSerializer.ToJsonString(this);
            //setJson = mJsonSerializer.RemoveJson(setJson, "delflag");
            //setJson = mJsonSerializer.RemoveJson(setJson, "creation_date");
            //setJson = mJsonSerializer.RemoveJson(setJson, "delflag");
            //setJson = mJsonSerializer.RemoveJson(setJson, "orb_option");

            JsonObject setJson = JsonObject.Parse(mJsonSerializer.ToJsonString(this));
            setJson.Remove("delflag");
            setJson.Remove("creation_date");
            setJson.Remove("delflag");
            setJson.Remove("orb_option");

            JsonArrayObjects optionjson = new JsonArrayObjects();
            orb_option.ForEach(item =>
            {
                optionjson.Add(User_Inven_Option.SetOptionJson(item));
            }
            );
            setJson.Add("orb_option", optionjson.ToJson());
            return setJson;
        }

        public static string makeOrbListJson(ref List<User_Orb_Inven> setList)
        {
            JsonArrayObjects setJson = new JsonArrayObjects();
            //string setJson = "[]";

            setList.ForEach(item =>
            {
                setJson.Add(item.ToJsonObj());
                //setJson = mJsonSerializer.AddJsonArray(setJson, item.ToJson());
            }
            );

            return setJson.ToJson();
        }
    }

    public class User_Equip_Orb_Request
    {
        public long orb_seq { get; set; }
        public byte slot_num { get; set; }
    }

    /*
    public class System_ITEM_OPTION_GRADERATE
    {
        public int OptionRate_IndexID { get; set; }
        public string Description { get; set; }
        public string OptionRateType { get; set; }
        public short OptionRate_Tier { get; set; }
        public float OptionRate_Grade1 { get; set; }
        public float OptionRate_Grade2 { get; set; }
        public float OptionRate_Grade3 { get; set; }
        public float OptionRate_Grade4 { get; set; }
        public float OptionRate_Grade5 { get; set; }

        public int GetGrade()
        {
            List<float> checkGradeRate = new List<float>()
            {
                OptionRate_Grade1,
                OptionRate_Grade2,
                OptionRate_Grade3,
                OptionRate_Grade4,
                OptionRate_Grade5,
            };

            double Max = 0;
            foreach (float gradeRate in checkGradeRate)
            {
                Max += gradeRate;
            }

            double curRate = TheSoul.DataManager.Math.GetRandomDouble(0, Max);

            eItemGrade setGrade = eItemGrade.None;

            double checkRate = 0;
            foreach (float gradeRate in checkGradeRate)
            {
                setGrade++;
                checkRate += gradeRate;
                if (checkRate >= curRate)
                    return (int)setGrade;
            }

            return (int)eItemGrade.None;
        }

        private enum eItemGrade
        {
            None = 0,
            Grade1 = 1,
            Grade2,
            Grade3,
            Grade4,
            Grade5,
        }
    }

    public class System_ITEM_GETEXP
    {
        public int GetEXP_IndexID { get; set; }
        public string Description { get; set; }
        public string GetEXPType { get; set; }
        public short GetEXP_Tier { get; set; }
        public short GetEXP_Grade { get; set; }
        public int GetEXP { get; set; }
    }

    public class System_ITEM_NEEDEXP
    {
        public int EXP_IndexID { get; set; }
        public string Description { get; set; }
        public short Count { get; set; }
        public short EXPGroup { get; set; }
        public short EXPGrade { get; set; }
        public short EnchantLevel { get; set; }
        public int NeedEXP { get; set; }
        public int EnchantPrice { get; set; }
        public int GradeUPPrice { get; set; }
        public long GradeUP_NeedItemID { get; set; }
        public int GradeUP_NeedItemCount { get; set; }
        public long OptionChange_NeedItemID { get; set; }
        public int OptionChange_NeedItemCount { get; set; }
    }


    public class User_Inven
    {
        public long cid { get; set; }
        public long aid { get; set; }
        public short inventory_type { get; set; }
        public long itemid { get; set; }
        public int itemea { get; set; }
        public short itemtype { get; set; }
        public short Class { get; set; }
        public short grade { get; set; }
        public DateTime creation_date { get; set; }
        public long invenseq { get; set; }
        public string equipflag { get; set; }
        public string newyn { get; set; }
        public string delflag { get; set; }
        public string equipposition { get; set; }
        public short enchant_grade { get; set; }
        public short enchant_level { get; set; }
        public short fixoption { get; set; }
        public int enchant_exp { get; set; }
        public string optiontype1 { get; set; }
        public int optionvalue1 { get; set; }
        public string optiontype2 { get; set; }
        public int optionvalue2 { get; set; }
        public string optiontype3 { get; set; }
        public int optionvalue3 { get; set; }
        public string optiontype4 { get; set; }
        public int optionvalue4 { get; set; }
        public string optiontype5 { get; set; }
        public int optionvalue5 { get; set; }

        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }

        public User_Inven()
        {
            cid = 0;
            aid = 0;
            inventory_type = 0;
            itemid = 0;
            itemea = 0;
            itemtype = 0;
            Class = 0;
            grade = 0;
            creation_date = DateTime.Now;
            inventory_type = 0;
            equipflag = "";
            newyn = "";
            delflag = "";
            equipposition = "";
            enchant_grade = 0;
            enchant_exp = 0;
            enchant_level = 0;
            fixoption = 0;
            optiontype1 = "";
            optionvalue1 = 0;
            optiontype2 = "";
            optionvalue2 = 0;
            optiontype3 = "";
            optionvalue3 = 0;
            optiontype4 = "";
            optionvalue4 = 0;
            optiontype5 = "";
            optionvalue5 = 0;
        }
    }

    public class User_Inven_Option_type
    {
        public string optiontype { get; set; }
        public int optionvalue { get; set; }

        public User_Inven_Option_type()
        {
            optiontype = string.Empty;
            optionvalue = default(int);
        }

        public User_Inven_Option_type(string type, int value)
        {
            optiontype = type;
            optionvalue = value;
        }
    }

    public class System_ITEM_ENCHANT_WPN
    {
        public long Enchant_IndexID { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public int EnchantGroup { get; set; }
        public short Tier { get; set; }
        public int EnchantLevel { get; set; }
        public int AP_Rate { get; set; }
        public int GradeUP_NeedGold { get; set; }
        public long EnchantUP_NeedItemID1 { get; set; }
        public int EnchantUP_NeedItemCount1 { get; set; }
        public long EnchantUP_NeedItemID2 { get; set; }
        public int EnchantUP_NeedItemCount2 { get; set; }
        public int EnchantUP_SuccessRate { get; set; }
        public int ProbabilityUpCash { get; set; }
        public int AbilityLevel { get; set; }
    }

    public class System_ITEM_EVOL_WEAPON
    {
        public int Evol_IndexID { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public int EvolGroup { get; set; }
        public short Tier { get; set; }
        public short Grade { get; set; }
        public int AP_Fix { get; set; }
        public int GradeUP_NeedGold { get; set; }
        public int GradeUP_NeedItemID { get; set; }
        public int GradeUP_NeedItemCount { get; set; }
        public int Disassemble_MakeItemID { get; set; }
        public int Disassemble_MakeItemCount { get; set; }
        public long OptionChange_NeedItemID { get; set; }
        public int OptionChange_NeedItemCount { get; set; }
        public string Resource_Model { get; set; }
        public string Resource_Texture { get; set; }
        public string Resource_EffectPrefab { get; set; }
        public string Item_Icon_Atlas { get; set; }
        public string Item_Resource { get; set; }
        public string Dev_Description { get; set; }
    }

    public class System_ITEM_EVOL_ACCESSORY
    {
        public int Evol_IndexID { get; set; }
        public string Description { get; set; }
        public string ItemType { get; set; }
        public int Count { get; set; }
        public int EvolGroup { get; set; }
        public short Tier { get; set; }
        public short Grade { get; set; }
        public int FixOptionID1 { get; set; }
        public int FixOptionID2 { get; set; }
        public short ProbabilityValue { get; set; }
        public int ProbabilityUpCash { get; set; }
        public int GradeUP_NeedGold { get; set; }
        public int GradeUP_NeedItemID { get; set; }
        public int GradeUP_NeedItemCount { get; set; }
        public int OptionChange_NeedItemID { get; set; }
        public int OptionChange_NeedItemCount { get; set; }
        public int ValueChange_NeedItemID { get; set; }
        public int ValueChange_NeedItemCount { get; set; }
        public int Disassemble_MakeItemID { get; set; }
        public int Disassemble_MakeItemCount { get; set; }
        public string Item_Icon_Atlas { get; set; }
        public string Item_Resource { get; set; }
    }
    */
}
