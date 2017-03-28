using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;

namespace TheSoul.DataManager.DBClass
{
    public class System_Gacha_Level
    {
        public short Character_Level { get; set; }
        public int Level_MatchingID { get; set; }
    }

    public class System_Gacha_Best
    {
        public long GachaIndex { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Gacha_Cash { get; set; }
        public long Main_SoulItemID { get; set; }
        public long Sub_SoulItemID_1 { get; set; }
        public long Sub_SoulItemID_2 { get; set; }
        public long Sub_SoulItemID_3 { get; set; }
        public string delflag { get; set; }
    }

    public class User_Gacha_Info
    {
        public long AID { get; set; }
        public int TotalFree_GachaCount { get; set; }
        public int Free_GachaCount { get; set; }
        public DateTime FreeGacha_regdate { get; set; }
        public int Premium_GachaSpecialCount { get; set; }
        public int Premium_GachaSpecialGetCount { get; set; }
        public int TotalPremium_GachaCount { get; set; }
        public DateTime PremiumGacha_regdate { get; set; }

        public User_Gacha_Info()
        {
            FreeGacha_regdate = PremiumGacha_regdate = DateTime.Now.AddDays(-1);
        }
    }

    public class User_Gacha_Special_Info
    {
        public long AID { get; set; }
        public int Total_Normal_GachaCount { get; set; }
        public int Nomal_GachaSpecialCount { get; set; }
        public int Nomal_GachaSpeciaGetCount { get; set; }
        public DateTime Nomal_Gacha_regdate { get; set; }
        public int Premium_GachaSpecialCount { get; set; }
        public int Premium_GachaSpecialGetCount { get; set; }
        public DateTime Premium_Gacha_regdate { get; set; }
    }


    public class RetGachaInfo
    {
        public int use_freecount { get; set; }
        public long free_left_time { get; set; }
        public long premium_left_time { get; set; }
        public int normal_special_count { get; set; }
        public int normal_special_max_count { get; set; }
        public int premium_special_count { get; set; }
        public int premium_special_max_count { get; set; }

        public RetGachaInfo() { }
        public RetGachaInfo(ref TxnBlock TB, User_Gacha_Info setGachainfo, User_Gacha_Special_Info setSpecialInfo)
        {
            use_freecount = setGachainfo.Free_GachaCount;
            TimeSpan TS = new TimeSpan();

            DateTime freeDate = setGachainfo.FreeGacha_regdate;
            DateTime premiumDate = setGachainfo.PremiumGacha_regdate;
            DateTime curDate = DateTime.Now;

            TS = curDate - freeDate;

            if (TS.Days != 0)
                use_freecount = 0;

            if (TS.TotalMinutes < Shop_Define.Shop_FreeGacha_Minutes)
            {
                TS = freeDate.AddMinutes(Shop_Define.Shop_FreeGacha_Minutes) - curDate;
                free_left_time = System.Convert.ToInt64(TS.TotalSeconds);
            }
            else
                free_left_time = 0;

            TS = curDate - premiumDate;

            if (TS.TotalHours < Shop_Define.Shop_Free_Premium_Hours)
            {
                TS = premiumDate.AddHours(Shop_Define.Shop_Free_Premium_Hours) - curDate;
                premium_left_time = System.Convert.ToInt64(TS.TotalSeconds);
            }
            else
                premium_left_time = 0;

            normal_special_count = setSpecialInfo.Nomal_GachaSpecialCount;

            Shop_Define.eShopConstDef getSpecialConstType = Shop_Define.Shop_Gacha_Special_Normal_Count_Def_List.Count() >= setSpecialInfo.Nomal_GachaSpeciaGetCount
                                                                    ? Shop_Define.Shop_Gacha_Special_Normal_Count_Def_List[setSpecialInfo.Nomal_GachaSpeciaGetCount]
                                                                    : Shop_Define.Shop_Gacha_Special_Normal_Count_Def_List.Last();

            normal_special_max_count = SystemData.GetConstValueInt(ref TB, Shop_Define.Shop_Const_Def_Key_List[getSpecialConstType]);


            premium_special_count = setSpecialInfo.Premium_GachaSpecialCount;

            getSpecialConstType = Shop_Define.Shop_Gacha_Special_Premium_Count_Def_List.Count() >= setSpecialInfo.Premium_GachaSpecialGetCount
                                                                    ? Shop_Define.Shop_Gacha_Special_Premium_Count_Def_List[setSpecialInfo.Premium_GachaSpecialGetCount]
                                                                    : Shop_Define.Shop_Gacha_Special_Premium_Count_Def_List.Last();

            premium_special_max_count = SystemData.GetConstValueInt(ref TB, Shop_Define.Shop_Const_Def_Key_List[getSpecialConstType]);
        }
    }


    public class System_Shop_Cash_Item
    {
        public int Buy_ID { get; set; }
        public short Buy_GroupID { get; set; }
        public short Buy_Count { get; set; }
        public int Buy_StackPriceValue { get; set; }
        public int Buy_StackPriceValue_Stack { get; set; }
        public int Buy_StackItemNum { get; set; }
        public int Buy_StackItemNum_Stack { get; set; }
    }

    public class System_Shop_Goods : System_Shop_Limit_List
    {
        public string Desc { get; set; }
        public string ItemClass { get; set; }
        public short MinLevel { get; set; }
        public short MaxLevel { get; set; }
        public int VIP_Point { get; set; }

        /// add by lh 2017 03 02, 首充折倍标志，  以及翻倍倍数
        public int DoubleSwitch { get; set; }
        public int Discount { get; set; }
    }

    public class System_Shop_Point : System_Shop_Limit_List
    {
        public string Desc { get; set; }
        public string ItemClass { get; set; }
    }

    public class System_Shop_Guild : System_Shop_Point
    {
    }

    public class System_Shop_Goods_Code
    {
        public string Product_ID { get; set; }
        public int ProductPrice { get; set; }
        public int ProductTier { get; set; }
    }

    public class RetShopGoodsCode
    {
        public string productid { get; set; }
        public long shop_item_id { get; set; }
        public long billing_type { get; set; }

        public RetShopGoodsCode(long itemid, long btype, string setid)
        {
            productid = setid;
            shop_item_id = itemid;
            billing_type = btype;
        }
    }

    public class System_Shop_Limit_List : System_Shop_Goods_Code
    {
        public int Shop_Goods_ID { get; set; }
        public int GroupID { get; set; }
        public string NameCN1 { get; set; }
        public string NameCN2 { get; set; }
        public string ToolTipCN { get; set; }
        public int ItemID { get; set; }
        public int ItemNum { get; set; }
        public int Bonus_ItemID { get; set; }
        public int Bonus_ItemNum { get; set; }
        public string Buy_PriceType { get; set; }
        public int Buy_PriceValue { get; set; }
        public short Type { get; set; }
        public int Sale_Rate { get; set; }
        public long SaleStartTime { get; set; }
        public long SaleEndTime { get; set; }
        public byte SaleType { get; set; }
        public byte DefaultSaleType { get; set; }
        public short Max_Buy { get; set; }
        public short Buy_GroupID { get; set; }
        public short UseGuildLv { get; set; }
    }

    public class System_Shop_BlackMarket
    {
        public int Shop_Goods_ID { get; set; }
        public int GroupID { get; set; }
        public int SlotID { get; set; }
        public int SlotIndex { get; set; }
        public string NameCN1 { get; set; }
        public string NameCN2 { get; set; }
        public string ToolTipCN { get; set; }
        public string ItemClass { get; set; }
        public int ItemID { get; set; }
        public int ItemNum { get; set; }
        public string Buy_PriceType { get; set; }
        public int Buy_PriceValue { get; set; }
        public short Type { get; set; }
        public short Max_Buy { get; set; }
        public int ItemProb { get; set; }
        public short delflag { get; set; }
    }

    public class System_Package_List : System_Shop_Goods_Code
    {
        public long Package_ID { get; set; }
        public byte ActiveType { get; set; }
        public string Buy_PriceType { get; set; }
        public int Buy_PriceValue { get; set; }
        public byte VIP_Level { get; set; }
        public byte Grade { get; set; }
        public byte Max_Buy { get; set; }
        public string NameCN1 { get; set; }
        public string NameCN2 { get; set; }
        public string ToolTipCN { get; set; }
        public string DetailCN { get; set; }
        public long Reward_Box1ID { get; set; }
        public long Reward_Box2ID { get; set; }
        public long Reward_Box3ID { get; set; }
        public long Reward_Box4ID { get; set; }
        public int VIP_Point { get; set; }
        public byte LoopType { get; set; }
        public DateTime SaleStartTime { get; set; }
        public DateTime SaleEndTime { get; set; }
        public string Shop_Icon_Atlas { get; set; } 
    }

    public class System_Package_RewardBox
    {
        public long Package_RewardBox_ID { get; set; }
        public long RewardBoxID { get; set; }
        public byte ItemIndex { get; set; }
        public string Item_TargetType { get; set; }
        public long Item_ID { get; set; }
        public byte Item_Level { get; set; }
        public byte Item_Grade { get; set; }
        public byte Item_Rnd1Type { get; set; }
        public int Item_Rnd1Value { get; set; }
        public byte Item_Rnd2Type { get; set; }
        public int Item_Rnd2Value { get; set; }
        public byte Item_Rnd3Type { get; set; }
        public int Item_Rnd3Value { get; set; }
        public int Item_Num { get; set; }

        public System_Package_RewardBox() { }
        public System_Package_RewardBox(long setID, int setCount, byte setLevel = 0, byte setGrade = 1)
        {
            Item_ID = setID;
            Item_Num = setCount;
            Item_Level = setLevel;
            Item_Grade = setGrade;
        }
    }

    public class RetPackageList
    {
        public long package_id { get; set; }
        public byte buy_price_type { get; set; }
        public int buy_price_value { get; set; }
        public byte grade { get; set; }
        public int buy_count { get; set; }
        public int max_buy_count { get; set; }
        public string name_cn1 { get; set; }
        public string name_cn2 { get; set; }
        public string tooltip_cn { get; set; }
        public string detail_cn { get; set; }
        public byte vip_level { get; set; }
        public string productid { get; set; }
        public string Shop_Icon_Atlas { get; set; } 
        public List<RetPackageReward> item_list { get; set; }

        public RetPackageList() { item_list = new List<RetPackageReward>(); }
        public RetPackageList(System_Package_List setList, int buyCount = 0)
        {
            package_id = setList.Package_ID;
            buy_price_type = (byte)Item_Define.Item_BuyType_List[setList.Buy_PriceType];
            buy_price_value = setList.Buy_PriceValue;
            grade = setList.Grade;
            buy_count = buyCount;
            vip_level = setList.VIP_Level;
            max_buy_count = setList.Max_Buy;
            name_cn1 = setList.NameCN1;
            name_cn2 = setList.NameCN2;
            tooltip_cn = setList.ToolTipCN;
            detail_cn = setList.DetailCN;
            item_list = new List<RetPackageReward>();
            productid = setList.Product_ID;
            Shop_Icon_Atlas = setList.Shop_Icon_Atlas;

        }
    }

    public class RetPackageReward
    {
        public long item_id { get; set; }
        public int item_num { get; set; }
        public short item_grade { get; set; }
        public short item_level { get; set; }

        public RetPackageReward() { }
        public RetPackageReward(System_Package_RewardBox setItem)
        {
            item_id = setItem.Item_ID;
            item_num = setItem.Item_Num;
            item_grade = setItem.Item_Grade;
            item_level = setItem.Item_Level;
        }
    }

    public class RetShopItem
    {
        public int shop_item_id { get; set; }
        public int group_id { get; set; }
        public int buy_count { get; set; }
        public int max_buy_count { get; set; }
        public int sale_rate { get; set; }
        public string name_cn1 { get; set; }
        public string name_cn2 { get; set; }
        public string tooltip_cn { get; set; }
        public long item_id { get; set; }
        public long item_num { get; set; }
        public long bonus_item_id { get; set; }
        public long bonus_item_num { get; set; }
        public byte buy_price_type { get; set; }
        public long buy_price_value { get; set; }
        public long sale_starttime { get; set; }
        public long sale_endtime { get; set; }
        public byte sale_type { get; set; }
        public int buy_group_id { get; set; }
        public int use_guild_lv { get; set; }
        public string productid { get; set; }

        /// add by lh 2017 03 02, 首充折倍标志，  以及翻倍倍数
        public int DoubleSwitch { get; set; }   /// 是否开启首充标志
        public int Discount { get; set; }        /// 翻倍倍数

        public RetShopItem() { }
        public RetShopItem(System_Shop_Limit_List setItem, int buyCount)
        {
            shop_item_id = setItem.Shop_Goods_ID;
            group_id = setItem.GroupID;
            name_cn1 = setItem.NameCN1;
            name_cn2 = setItem.NameCN2;
            tooltip_cn = setItem.ToolTipCN;
            max_buy_count = setItem.Max_Buy;
            buy_count = buyCount;
            sale_rate = setItem.Sale_Rate;
            sale_starttime = setItem.SaleStartTime;
            sale_endtime = setItem.SaleEndTime;
            sale_type = (setItem.SaleEndTime > 0) ? setItem.SaleType : setItem.DefaultSaleType;
            item_id = setItem.ItemID;
            item_num = setItem.ItemNum;
            bonus_item_id = setItem.Bonus_ItemID;
            bonus_item_num = setItem.Bonus_ItemNum;
            buy_price_type = (byte)Item_Define.Item_BuyType_List[setItem.Buy_PriceType];
            buy_price_value = setItem.Buy_PriceValue;
            use_guild_lv = setItem.UseGuildLv;
            buy_group_id = setItem.Buy_GroupID;
            productid = setItem.Product_ID;
        }

        public RetShopItem(System_Shop_BlackMarket setItem, int buyCount)
        {
            shop_item_id = setItem.Shop_Goods_ID;
            group_id = setItem.GroupID;
            name_cn1 = setItem.NameCN1;
            name_cn2 = setItem.NameCN2;
            tooltip_cn = setItem.ToolTipCN;
            max_buy_count = setItem.Max_Buy;
            buy_count = buyCount;
            sale_rate = 0;
            sale_starttime = 0;
            sale_endtime = 0;
            sale_type = (byte)Shop_Define.eShopSaleType.RegularSale;
            item_id = setItem.ItemID;
            item_num = setItem.ItemNum;
            bonus_item_id = 0;
            bonus_item_num = 0;
            buy_price_type = (byte)Item_Define.Item_BuyType_List[setItem.Buy_PriceType];
            buy_price_value = setItem.Buy_PriceValue;
            use_guild_lv = 0;
            buy_group_id = 0;
        }
    }

    public class User_Shop_Buy
    {
        public long AID { get; set; }
        public long Shop_Goods_ID { get; set; }
        public int TotalBuy_Count { get; set; }
        public int Buy_Count { get; set; }
        public int GoodsType { get; set; }
        public DateTime regdate { get; set; }
    }

    public class User_Shop_BlackMarket
    {
        public long AID { get; set; }
        public string ShopItemList { get; set; }
        public DateTime regdate { get; set; }

        public User_Shop_BlackMarket()
        {
            ShopItemList = string.Empty;
            regdate = DateTime.Now.AddDays(-1);
        }
    }
    
    public class User_Shop_TreasureBox
    {
        public long AID { get; set; }
        public string ShopItemList { get; set; }
        public int GoldBuyCount { get; set; }
        public int RubyBuyCount { get; set; }
        public DateTime regdate { get; set; }

        public User_Shop_TreasureBox()
        {
            ShopItemList = string.Empty;
            regdate = DateTime.Now.AddDays(-1);
        }
    }

    public class User_Shop_TreasureBox_List
    {
        public List<RetShopTreasureBoxItem> gold_treasure { get; set; }
        public int gold_box_need_gold { get; set; }
        public int gold_box_need_ruby { get; set; }
        public List<RetShopTreasureBoxItem> ruby_treasure { get; set; }
        public int ruby_box_need_gold { get; set; }
        public int ruby_box_need_ruby { get; set; }

    }

    public class RetShopTreasureBoxItem
    {
        public long itemid { get; set; }
        public int amount { get; set; }
        public int level { get; set; }
        public int grade { get; set; }
        public int buyslot { get; set; }
        public byte buyflag { get; set; }

        public RetShopTreasureBoxItem() { }
        public RetShopTreasureBoxItem(System_Drop_Group setItem, int setSlot)
        {
            itemid = setItem.DropItemID;
            amount = TheSoul.DataManager.Math.GetRandomInt(setItem.DropMinNum, setItem.DropMaxNum);
            level = setItem.DropItemLevel;
            grade = setItem.DropItemGrade;
            buyslot = setSlot;
        }
    }

    public class User_Shop_Reset
    {
        public long aid { get; set; }
        public int shoptype { get; set; }
        public int total_reset_count { get; set; }
        public int reset_count { get; set; }
        public DateTime regdate { get; set; }
    }

    public class System_Gacha_Box_Group : System_Drop_Box_Group
    {
        public int Level_MatchingID { get; set; }
        public int Gacha_Gold { get; set; }
        public int Gacha_Cash { get; set; }
    }

    public class System_Gacha_Best_DropGrop : System_Drop_Group
    {
    }

    public class User_Billing_List
    {
        public long BillingIndex { get; set; }
        public long AID { get; set; }
        public long CID { get; set; }
        public int Shop_Goods_ID { get; set; }
        public int Buy_PriceValue { get; set; }
        public string Billing_ID { get; set; }
        public string Billing_Token { get; set; }
        public string Platform_UID { get; set; }
        public int Buy_Platform { get; set; }
        public int Billing_Status { get; set; }
        public DateTime regdate { get; set; }
    }


    public class Ret_User_Billing_List
    {
        public string billing_id { get; set; }
        public string billing_token { get; set; }
        public int billing_status { get; set; }
        public string product_id { get; set; }
        public int price_value { get; set; }
        public string buy_date { get; set; }

        public Ret_User_Billing_List(User_Billing_List setObj, string setProductID)
        {
            billing_id = setObj.Billing_ID;
            billing_token = setObj.Billing_Token;
            billing_status = setObj.Billing_Status;
            product_id = setProductID;
            price_value = setObj.Buy_PriceValue;
            buy_date = setObj.regdate.ToString("yyyy-MM-dd");
        }
    }

    public class User_Shop_Subscription
    {
        public long AID { get; set; }
        public long Shop_Goods_ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public User_Shop_Subscription()
        {
            StartDate = EndDate = DateTime.Now;
        }
    }

    public class RetShopSubscription
    {
        public long shop_item_id { get; set; }
        public int left_day { get; set; }

        public RetShopSubscription() { }
        public RetShopSubscription(long ShopID, int LeftCount)
        {
            shop_item_id = ShopID;
            left_day = LeftCount;
        }
    }

    public class User_BlackMarketSoulSeq
    {
        public int soulseq { get; set; }
        public int count { get; set; }
    }

    public class User_TotalBuyPrice
    {
        public long totalbuy { get; set; }
    }

    public class Ret_Event_7DayPackage_List : RetPackageList
    {
        public long Buy_Day { get; set; }
        public Ret_Event_7DayPackage_List() { item_list = new List<RetPackageReward>(); }
        public Ret_Event_7DayPackage_List(System_Event_7Day_Package_List setList, int buyCount = 0)
        {
            package_id = setList.Package_ID;
            Buy_Day = setList.Buy_Day;
            buy_price_type = (byte)Item_Define.Item_BuyType_List[setList.Buy_PriceType];
            buy_price_value = setList.Buy_PriceValue;
            grade = setList.Grade;
            buy_count = buyCount;
            max_buy_count = setList.Max_Buy;
            name_cn1 = setList.NameCN1;
            name_cn2 = setList.NameCN2;
            tooltip_cn = setList.ToolTipCN;
            detail_cn = setList.DetailCN;
            item_list = new List<RetPackageReward>();
        }
    }

    public class User_GetShopCount
    {
        public int count { get; set; }
    }

    //public class System_Package_Cheap : System_Package_List
    //{
    //    public long ScheduleID { get; set; }
    //}

    //public class System_Package_Cheap_Reward : System_Package_RewardBox
    //{
    //    public long ScheduleID { get; set; }
    //}

    public class User_Billing_AuthKey
    {
        public string BillingAuthKey { get; set; }
        public long AID { get; set; }
        public long Shop_Goods_ID { get; set; }
        public long Billing_Platform_Type { get; set; }
        public string Product_ID { get; set; }
        public DateTime regdate { get; set; }
    }
}