using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Text;
using System.Data;

using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Tools;
using TheSoul.DataManager.Global;
using TheSoulGMTool.DBClass;
using TheSoulGMTool;

using OperationTool.Tools;
using OperationTool.DBClass;


namespace OperationTool
{
    public partial class coupon_make : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            WebQueryParam queryFetcher = new WebQueryParam(true);
            queryFetcher.bDBLog = true;
            string retJson = "";
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    tb.IsoLevel = IsolationLevel.ReadCommitted;
                    OperationManager.GetOperationDB(ref tb);
                    
                    Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;

                    if (!Page.IsPostBack)
                        pageInit(ref tb);
                    else
                    {
                        string sdate = queryFetcher.QueryParam_Fetch(startDay.UniqueID, DateTime.Today.ToString("yyyy-MM-dd"));
                        string edate = queryFetcher.QueryParam_Fetch(endDay.UniqueID, DateTime.Today.AddDays(1).ToString("yyyy-MM-dd"));
                        string reqReward1 = queryFetcher.QueryParam_Fetch(all_item.UniqueID, "");
                        string reqReward1cnt = queryFetcher.QueryParam_Fetch(all_itemcnt.UniqueID, "");
                        string reqReward1grade = queryFetcher.QueryParam_Fetch(all_itemgrade.UniqueID, "");
                        string reqReward2 = queryFetcher.QueryParam_Fetch(warrior_item.UniqueID, "");
                        string reqReward2level = queryFetcher.QueryParam_Fetch(warriorlevel.UniqueID, "");
                        string reqReward2grade = queryFetcher.QueryParam_Fetch(warriorgrade.UniqueID, "");
                        string reqReward3 = queryFetcher.QueryParam_Fetch(sword_item.UniqueID, "");
                        string reqReward3level = queryFetcher.QueryParam_Fetch(swordlevel.UniqueID, "");
                        string reqReward3grade = queryFetcher.QueryParam_Fetch(swordgrade.UniqueID, "");
                        string reqReward4 = queryFetcher.QueryParam_Fetch(taoist_item.UniqueID, "");
                        string reqReward4level = queryFetcher.QueryParam_Fetch(taoistlevel.UniqueID, "");
                        string reqReward4grade = queryFetcher.QueryParam_Fetch(taoistgrade.UniqueID, "");

                        System_Coupon_Group groupInfo = new System_Coupon_Group();
                        System_Coupon couponInfo = new System_Coupon();
                        List<System_Coupon_Reward> rewardList1 = new List<System_Coupon_Reward>();
                        List<System_Coupon_Reward> rewardList2 = new List<System_Coupon_Reward>();
                        List<System_Coupon_Reward> rewardList3 = new List<System_Coupon_Reward>();
                        List<System_Coupon_Reward> rewardList4 = new List<System_Coupon_Reward>();
                        
                        groupInfo.Coupon_Active = 1;
                        groupInfo.Coupon_Memo = "test";
                        groupInfo.Coupon_Title = "test";
                        groupInfo.Coupon_Type = 1; //1=> 1:1, 2 1:n
                        groupInfo.Coupon_Num = 10;
                        groupInfo.Reg_id = "tset2";

                        couponInfo.Coupon_Startdate = System.Convert.ToDateTime(string.Format("{0} 00:00:00", sdate));
                        couponInfo.Coupon_Enddate = System.Convert.ToDateTime(string.Format("{0} 23:59:59", edate));
                        couponInfo.Coupon_Type = 1;

                        //reward
                        string[] reward1 = System.Text.RegularExpressions.Regex.Split(reqReward1, ",");
                        string[] reward1cnt = System.Text.RegularExpressions.Regex.Split(reqReward1cnt, ",");
                        string[] reward1grade = System.Text.RegularExpressions.Regex.Split(reqReward1grade, ",");
                        byte itemIndex = 1;
                        for (int i = 0; i < reward1.Length; i++)
                        {
                            long itemid = System.Convert.ToInt64(reward1[i]);
                            if (itemid > 0)
                            {
                                int itemNum = System.Convert.ToInt32(reward1cnt[i]);
                                byte itemgrade = 1;
                                if (!string.IsNullOrEmpty(reward1grade[i]))
                                    itemgrade = System.Convert.ToByte(reward1grade[i]);
                                if (itemid > 0 && itemNum > 0)
                                {

                                    System_Coupon_Reward item = new System_Coupon_Reward();
                                    item.Item_ID = itemid;
                                    item.Item_Num = itemNum;
                                    item.Item_Grade = itemgrade;
                                    item.ItemIndex = itemIndex;
                                    rewardList1.Add(item);
                                    itemIndex++;
                                }
                            }
                        }
                        itemIndex = 1;
                        string[] reward2 = System.Text.RegularExpressions.Regex.Split(reqReward2, ",");
                        string[] reward2level = System.Text.RegularExpressions.Regex.Split(reqReward2level, ",");
                        string[] reward2grade = System.Text.RegularExpressions.Regex.Split(reqReward2grade, ",");
                        for (int i = 0; i < reward2.Length; i++)
                        {
                            long itemid = System.Convert.ToInt64(reward2[i]);
                            if (itemid > 0)
                            {
                                if (string.IsNullOrEmpty(reward2level[i]))
                                    reward2level[i] = "0";
                                if (string.IsNullOrEmpty(reward2grade[i]))
                                    reward2grade[i] = "0";
                                short itemlevel = System.Convert.ToInt16(reward2level[i]);
                                short itemgrade = System.Convert.ToInt16(reward2grade[i]);
                                if (itemgrade > 0)
                                {
                                    System_Coupon_Reward item = new System_Coupon_Reward();
                                    item.Item_ID = itemid;
                                    item.Item_Num = 1;
                                    item.ItemIndex = itemIndex;
                                    item.Item_Level = (byte)itemlevel;
                                    item.Item_Grade = (byte)itemgrade;
                                    rewardList2.Add(item);
                                    itemIndex++;
                                }
                            }
                        }

                        itemIndex = 1;
                        string[] reward3 = System.Text.RegularExpressions.Regex.Split(reqReward3, ",");
                        string[] reward3level = System.Text.RegularExpressions.Regex.Split(reqReward3level, ",");
                        string[] reward3grade = System.Text.RegularExpressions.Regex.Split(reqReward3grade, ",");
                        for (int i = 0; i < reward3.Length; i++)
                        {
                            long itemid = System.Convert.ToInt64(reward3[i]);
                            if (itemid > 0)
                            {
                                if (string.IsNullOrEmpty(reward3level[i]))
                                    reward3level[i] = "0";
                                if (string.IsNullOrEmpty(reward3grade[i]))
                                    reward3grade[i] = "0";
                                short itemlevel = System.Convert.ToInt16(reward3level[i]);
                                short itemgrade = System.Convert.ToInt16(reward3grade[i]);
                                if (itemgrade > 0)
                                {
                                    System_Coupon_Reward item = new System_Coupon_Reward();
                                    item.Item_ID = itemid;
                                    item.Item_Num = 1;
                                    item.ItemIndex = itemIndex;
                                    item.Item_Level = (byte)itemlevel;
                                    item.Item_Grade = (byte)itemgrade;
                                    rewardList3.Add(item);
                                    itemIndex++;
                                }
                            }
                        }

                        string[] reward4 = System.Text.RegularExpressions.Regex.Split(reqReward4, ",");
                        string[] reward4level = System.Text.RegularExpressions.Regex.Split(reqReward4level, ",");
                        string[] reward4grade = System.Text.RegularExpressions.Regex.Split(reqReward4grade, ",");
                        for (int i = 0; i < reward4.Length; i++)
                        {
                            long itemid = System.Convert.ToInt64(reward4[i]);
                            if (itemid > 0)
                            {
                                if (string.IsNullOrEmpty(reward4level[i]))
                                    reward4level[i] = "0";
                                if (string.IsNullOrEmpty(reward4grade[i]))
                                    reward4grade[i] = "0";
                                short itemlevel = System.Convert.ToInt16(reward4level[i]);
                                short itemgrade = System.Convert.ToInt16(reward4grade[i]);
                                if (itemgrade > 0)
                                {
                                    System_Coupon_Reward item = new System_Coupon_Reward();
                                    item.Item_ID = itemid;
                                    item.Item_Num = 1;
                                    item.ItemIndex = itemIndex;
                                    item.Item_Level = (byte)itemlevel;
                                    item.Item_Grade = (byte)itemgrade;
                                    rewardList4.Add(item);
                                    itemIndex++;
                                }
                            }
                        }

                        retError = OperationCouponManager.InsertCoupon(ref tb, groupInfo, couponInfo, rewardList1, rewardList2, rewardList3, rewardList4);

                    }
                    retJson = queryFetcher.Render(retError);
                }
                catch (Exception errorEx)
                {
                    queryFetcher.DBLog("StackTrace" + mJsonSerializer.ToJsonString(errorEx.StackTrace));
                    queryFetcher.DBLog(errorEx.Message);
                    retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(errorEx.Message), Result_Define.eResult.System_Exception);
                }
                finally
                {
                    tb.EndTransaction(queryFetcher.Render_errorFlag);
                    tb.Dispose();
                }

            }

        }
        
        protected void pageInit(ref TxnBlock TB)
        {
            ListItem selectItem = new ListItem("select", "-1");            

            List<Admin_System_Item> allList = GMDataManager.GetNonEquip_Accessory_ItemList(ref TB);
            List<Admin_System_Item> warriorList = GMDataManager.GetEquipItemList(ref TB, "Warrior");
            List<Admin_System_Item> swordList = GMDataManager.GetEquipItemList(ref TB, "Swordmaster");
            List<Admin_System_Item> taoistList = GMDataManager.GetEquipItemList(ref TB, "Taoist");
            all_item.DataSource = allList;
            all_item.DataTextField = "Description";
            all_item.DataValueField = "Item_IndexID";
            all_item.DataBind();
            all_item.Items.Insert(0, selectItem);

            warrior_item.DataSource = warriorList;
            warrior_item.DataTextField = "Description";
            warrior_item.DataValueField = "Item_IndexID";
            warrior_item.DataBind();
            warrior_item.Items.Insert(0, selectItem);

            sword_item.DataSource = swordList;
            sword_item.DataTextField = "Description";
            sword_item.DataValueField = "Item_IndexID";
            sword_item.DataBind();
            sword_item.Items.Insert(0, selectItem);

            taoist_item.DataSource = taoistList;
            taoist_item.DataTextField = "Description";
            taoist_item.DataValueField = "Item_IndexID";
            taoist_item.DataBind();
            taoist_item.Items.Insert(0, selectItem);
        }
    }
}