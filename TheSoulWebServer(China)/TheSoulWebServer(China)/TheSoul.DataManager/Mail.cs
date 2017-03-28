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
    public class User_MailBox
    {
        public long mailseq { get; set; }
        public long aid { get; set; }
        public long senderid { get; set; }
        public string sendername { get; set; }
        public string title { get; set; }
        public long item_id_1 { get; set; }
        public int itemea_1 { get; set; }
        public short item_grade_1 { get; set; }
        public short item_level_1 { get; set; }

        public string readflag { get; set; }
    }

    public class User_Mail_Datail : User_MailBox
    {
        public string bodytext { get; set; }
        public long item_id_2 { get; set; }
        public int itemea_2 { get; set; }
        public short item_grade_2 { get; set; }
        public short item_level_2 { get; set; }
        public long item_id_3 { get; set; }
        public int itemea_3 { get; set; }
        public short item_grade_3 { get; set; }
        public short item_level_3 { get; set; }
        public long item_id_4 { get; set; }
        public int itemea_4 { get; set; }
        public short item_grade_4 { get; set; }
        public short item_level_4 { get; set; }
        public long item_id_5 { get; set; }
        public int itemea_5 { get; set; }
        public short item_grade_5 { get; set; }
        public short item_level_5 { get; set; }
        public int remaintime { get; set; }
        public string delflag { get; set; }
    }

    public class Set_Mail_Item
    {
        public long itemid { get; set; }
        public int itemea { get; set; }
        public short grade { get; set; }
        public short level { get; set; }

        public Set_Mail_Item()
        {
            level = 1;
        }

        public Set_Mail_Item(long setID, int setEa, short setGrade = 1, short setLevel = 0)
        {
            itemid = setID;
            itemea = setEa;
            grade = setGrade;
            level = (short)(setLevel < 1 ? 1 : setLevel);
        }
    }

    public class Admin_System_MailNotice
    {
        public long idx { get; set; }
        public byte active { get; set; }
        public byte MailType { get; set; }
        public string senderName { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public DateTime regDate { get; set; }
        public string regID { get; set; }
    }

    public class Admin_System_MailNotice_Reward
    {
        public long IDX { get; set; }
        public long MailIndex { get; set; }
        public int ItemIndex { get; set; }
        public long Item_ID { get; set; }
        public byte Item_Grade { get; set; }
        public byte Item_Level { get; set; }
        public int Item_Num { get; set; }
    }

    public class User_Admin_Mail_Check
    {
        public long AID { get; set; }
        public long last_checked_main_idx { get; set; }
        public DateTime update_date { get; set; }

        public User_Admin_Mail_Check() { }
        public User_Admin_Mail_Check(long setAID) { AID = setAID; }
    }
}
namespace TheSoul.DataManager
{
    public static class Mail_Define
    {
        public const string Mail_Info_DB = "sharding";
        public const string Mail_Info_Surfix = "Info";

        public const string Mail_Box_TableName = "User_MailBox";

        public const long Mail_System_Sender_AID = 0;
        public const string Mail_System_Sender_Name = "system";

        public const string Mail_SystemMailNoticeTableName = "Admin_System_MailNotice";
        public const string Mail_SystemMailNoticeRewardTableName = "Admin_System_MailNotice_Reward";
        public const string Mail_User_SystemMailCheckTableName = "User_Admin_Mail_Check";

        public const int Mail_Max_Count = 50;
        public const int Mail_Close_Min = 10080;
        public const int Mail_MaxItem = 5;
        public const int Mail_VIP_CloseTime_Min = 1438560;

        public const string Coupon_Mail_Title = "#STRING_MSG_MAIL_COUPON_REWARD_HEAD";
        public const string Coupon_Mail_Body = "#STRING_MSG_MAIL_COUPON_REWARD_BODY";

        public enum eMailNoticeType
        {
            MAILNOTICE = 0,
            MAILTITEM = 1,
        }
    }

    public static class MailManager
    {
        public static Result_Define.eResult SendMail(ref TxnBlock TB, long ReceiverAID, long SenderAID = 0, string SenderName = "", string MailTitle = "", string MailBodyText = ""
                                                        , long item_id_1 = 0, int itemea_1 = 0, long item_grade_1 = 1, int item_level_1 = 0
                                                        , long item_id_2 = 0, int itemea_2 = 0, long item_grade_2 = 1, int item_level_2 = 0
                                                        , long item_id_3 = 0, int itemea_3 = 0, long item_grade_3 = 1, int item_level_3 = 0
                                                        , long item_id_4 = 0, int itemea_4 = 0, long item_grade_4 = 1, int item_level_4 = 0
                                                        , long item_id_5 = 0, int itemea_5 = 0, long item_grade_5 = 1, int item_level_5 = 0
                                                        , int setCloseMin = 0, string dbkey = Mail_Define.Mail_Info_DB)
        {
            string setQuery = string.Format(@"INSERT INTO {0} ( aid, senderid, sendername, title, bodytext,
                                                                item_id_1, itemea_1, item_grade_1, item_level_1,
                                                                item_id_2, itemea_2, item_grade_2, item_level_2,
                                                                item_id_3, itemea_3, item_grade_3, item_level_3,
                                                                item_id_4, itemea_4, item_grade_4, item_level_4,
                                                                item_id_5, itemea_5, item_grade_5, item_level_5,
                                                                closedate, readflag, delflag
                                                              ) VALUES (
                                                                '{1}', N'{2}', N'{3}', N'{4}', N'{5}',
                                                                '{6}', '{7}', '{8}', '{9}',
                                                                '{10}','{11}','{12}','{13}',
                                                                '{14}','{15}','{16}','{17}',
                                                                '{18}','{19}','{20}','{21}',
                                                                '{22}','{23}','{24}','{25}',
                                                                DATEADD(MINUTE, {26}, GETDATE()), '{27}', '{28}'
                                                                )"
                                                        , Mail_Define.Mail_Box_TableName
                                                        , ReceiverAID, SenderAID, SenderName, MailTitle, MailBodyText
                                                        , item_id_1, itemea_1, item_grade_1, item_level_1
                                                        , item_id_2, itemea_2, item_grade_2, item_level_2
                                                        , item_id_3, itemea_3, item_grade_3, item_level_3
                                                        , item_id_4, itemea_4, item_grade_4, item_level_4
                                                        , item_id_5, itemea_5, item_grade_5, item_level_5
                                                        , (setCloseMin > 0 ? setCloseMin : Mail_Define.Mail_Close_Min), "N", "N"
                                                        );
            RemoveMailCache(ReceiverAID);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult SendMail(ref TxnBlock TB, ref List<Set_Mail_Item> setMailItem, long ReceiverAID, long SenderAID = 0, string SenderName = ""
                                                    , string MailTitle = "", string MailBodyText = "", int setCloseMin = 0
                                                    , string dbkey = Mail_Define.Mail_Info_DB)
        {
            long idxID = 0;
            return SendMail(ref TB, ref setMailItem, ReceiverAID, out idxID, SenderAID, SenderName, MailTitle, MailBodyText, setCloseMin, dbkey);
        }

        public static Result_Define.eResult SendMail(ref TxnBlock TB, ref List<Set_Mail_Item> setMailItem, long ReceiverAID, out long idxID,  long SenderAID = 0, string SenderName = ""
                                                            , string MailTitle = "", string MailBodyText = "", int setCloseMin = 0
                                                            , string dbkey = Mail_Define.Mail_Info_DB)
        {
            Set_Mail_Item[] setItem = new Set_Mail_Item[Mail_Define.Mail_MaxItem] {
                new Set_Mail_Item(),
                new Set_Mail_Item(),
                new Set_Mail_Item(),
                new Set_Mail_Item(),
                new Set_Mail_Item()
            };

            int setPos = 0;

            setMailItem.ForEach(mailItem =>
            {
                setItem[setPos].itemid = mailItem.itemid;
                setItem[setPos].itemea = mailItem.itemea;
                setItem[setPos].grade = mailItem.grade;
                setItem[setPos].level = mailItem.level;
                setPos++;
            }
            );

            string setQuery = string.Format(@"INSERT INTO {0} ( aid, senderid, sendername, title, bodytext,
                                                                item_id_1, itemea_1, item_grade_1, item_level_1,
                                                                item_id_2, itemea_2, item_grade_2, item_level_2,
                                                                item_id_3, itemea_3, item_grade_3, item_level_3,
                                                                item_id_4, itemea_4, item_grade_4, item_level_4,
                                                                item_id_5, itemea_5, item_grade_5, item_level_5,
                                                                closedate, readflag, delflag
                                                              ) 
                                                              OUTPUT INSERTED.mailseq 
                                                                VALUES (
                                                                '{1}', N'{2}', N'{3}', N'{4}', N'{5}',
                                                                '{6}', '{7}', '{8}', '{9}',
                                                                '{10}','{11}','{12}','{13}',
                                                                '{14}','{15}','{16}','{17}',
                                                                '{18}','{19}','{20}','{21}',
                                                                '{22}','{23}','{24}','{25}',
                                                                DATEADD(MINUTE, {26}, GETDATE()), '{27}', '{28}'
                                                                );"
                                                        , Mail_Define.Mail_Box_TableName
                                                        , ReceiverAID, SenderAID, SenderName, MailTitle, MailBodyText
                                                        , setItem[0].itemid, setItem[0].itemea, setItem[0].grade, setItem[0].level
                                                        , setItem[1].itemid, setItem[1].itemea, setItem[1].grade, setItem[1].level
                                                        , setItem[2].itemid, setItem[2].itemea, setItem[2].grade, setItem[2].level
                                                        , setItem[3].itemid, setItem[3].itemea, setItem[3].grade, setItem[3].level
                                                        , setItem[4].itemid, setItem[4].itemea, setItem[4].grade, setItem[4].level
                                                        , (setCloseMin > 0 ? setCloseMin : Mail_Define.Mail_Close_Min), "N", "N"
                                                        );

            SqlCommand mySqlCommand = new SqlCommand(setQuery);            
            SqlDataReader getDr = null;
            idxID = 0;
            mySqlCommand.Prepare();
            if (TB.ExcuteSqlCommand(dbkey, ref mySqlCommand, ref getDr))
            {
                if (getDr != null)
                {
                    while (getDr.Read())
                    {
                        if (getDr.GetInt64(0) > -1)
                            idxID = getDr.GetInt64(0);
                    }
                    getDr.Dispose(); getDr.Close();                    
                }

                RemoveMailCache(ReceiverAID);
                return Result_Define.eResult.SUCCESS;
            }
            else
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }


        public static bool RemoveMailCache(long AID)
        {
            return true;
            //string setKey = string.Format("{0}_{1}_{2}", Mail_Define.Mail_Box_TableName, AID, Mail_Define.Mail_Info_Surfix);
            //return RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey);
            //return RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.REDIS_COMMAND_FAIL;
        }

        public static List<User_Mail_Datail> GetUser_Mail_List(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Mail_Define.Mail_Info_DB)
        {
            string setQuery = string.Format(@"SELECT TOP {2} *, (DATEDIFF(SS,GETDATE(), closedate)) as remaintime FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND closedate > GETDATE() AND delflag = 'N' ORDER BY mailseq DESC", Mail_Define.Mail_Box_TableName, AID, Mail_Define.Mail_Max_Count);
            List<User_Mail_Datail> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_Mail_Datail>(ref TB, setQuery, dbkey);


            //string setKey = string.Format("{0}_{1}_{2}", Mail_Define.Mail_Box_TableName, AID, Mail_Define.Mail_Info_Surfix);
            //List<User_Mail_Datail> retObj = GenericFetch.FetchFromOnly_Redis_Hash_All<User_Mail_Datail>(DataManager_Define.RedisServerAlias_User, setKey);

            //if (retObj.Count < 1 || Flush)
            //{
            //    string setQuery = string.Format(@"SELECT TOP {2} *, (DATEDIFF(SS,GETDATE(), closedate)) as remaintime FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND closedate > GETDATE() AND delflag = 'N' ORDER BY mailseq DESC", Mail_Define.Mail_Box_TableName, AID, Mail_Define.Mail_Max_Count);
            //    List<User_Mail_Datail> listObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_Mail_Datail>(ref TB, setQuery, dbkey);
            //    foreach (User_Mail_Datail setObj in listObj)
            //    {
            //        RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, setObj.mailseq.ToString(), setObj);
            //        retObj.Add(setObj);
            //    }
            //    RedisConst.GetRedisInstance().SetExpireTimeHash(DataManager_Define.RedisServerAlias_User, setKey);
            //}
            return retObj;
        }

        public static User_Mail_Datail GetUser_Mail_Detail(ref TxnBlock TB, long AID, long Mail_Seq, bool Flush = false, string dbkey = Mail_Define.Mail_Info_DB)
        {
            string setQuery = string.Format(@"SELECT *, (DATEDIFF(SS,GETDATE(), closedate)) as remaintime FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND mailseq = {2} AND closedate > GETDATE() AND delflag = 'N'", Mail_Define.Mail_Box_TableName, AID, Mail_Seq);
            User_Mail_Datail retObj = GenericFetch.FetchFromDB<User_Mail_Datail>(ref TB, setQuery, dbkey);

            //string setKey = string.Format("{0}_{1}_{2}", Mail_Define.Mail_Box_TableName, AID, Mail_Define.Mail_Info_Surfix);
            //string setQuery = string.Format(@"SELECT *, (DATEDIFF(SS,GETDATE(), closedate)) as remaintime FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND mailseq = {2} AND closedate > GETDATE() AND delflag = 'N'", Mail_Define.Mail_Box_TableName, AID, Mail_Seq);
            //User_Mail_Datail retObj = GenericFetch.FetchFromRedis_Hash<User_Mail_Datail>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, Mail_Seq.ToString(), setQuery, dbkey, Flush);
            return (retObj != null) ? retObj : new User_Mail_Datail();
        }

        public static Result_Define.eResult Update_MailReadFlag(ref TxnBlock TB, long AID, long Mail_Seq, bool isSet = true, string dbkey = Account_Define.AccountShardingDB)
        {
            string setQuery = string.Format("UPDATE {0} SET readflag = '{3}' WHERE aid = {1} AND mailseq = {2} ", Mail_Define.Mail_Box_TableName, AID, Mail_Seq, isSet ? "Y" : "N");
            return (TB.ExcuteSqlCommand(dbkey, setQuery)) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult Update_MailOpenFlag(ref TxnBlock TB, long AID, long Mail_Seq, bool isSet = true, string dbkey = Account_Define.AccountShardingDB)
        {
            string setQuery = string.Format("UPDATE {0} SET delflag = '{3}' WHERE aid = {1} AND mailseq = {2} ", Mail_Define.Mail_Box_TableName, AID, Mail_Seq, isSet ? "Y" : "N");
            return (TB.ExcuteSqlCommand(dbkey, setQuery)) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static List<Admin_System_MailNotice> GetAdmin_NoticeMailList(ref TxnBlock TB, long Check_Seq, string dbkey = Mail_Define.Mail_Info_DB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK, INDEX(IX_User_SendMail_List))  WHERE idx > {1} AND (GETDATE() BETWEEN startDate AND endDate) AND active > 0", Mail_Define.Mail_SystemMailNoticeTableName, Check_Seq);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<Admin_System_MailNotice>(ref TB, setQuery, dbkey);
        }

        public static List<Admin_System_MailNotice_Reward> GetAdmin_NoticeMailReward(ref TxnBlock TB, long Check_Seq, string dbkey = Mail_Define.Mail_Info_DB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE MailIndex = {1}", Mail_Define.Mail_SystemMailNoticeRewardTableName, Check_Seq);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<Admin_System_MailNotice_Reward>(ref TB, setQuery, dbkey);
        }

        public static List<Set_Mail_Item> GetAdminSendMailItemList(ref TxnBlock TB, long Check_Seq, string dbkey = Mail_Define.Mail_Info_DB)
        {
            List<Admin_System_MailNotice_Reward> setList = GetAdmin_NoticeMailReward(ref TB, Check_Seq);
            List<Set_Mail_Item> retList = new List<Set_Mail_Item>();
            setList.ForEach(setItem => { retList.Add(new Set_Mail_Item(setItem.Item_ID, setItem.Item_Num, setItem.Item_Grade, setItem.Item_Level)); });
            return retList;
        }

        public static User_Admin_Mail_Check GetUser_Admin_Mail_Check(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Mail_Define.Mail_Info_DB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1}", Mail_Define.Mail_User_SystemMailCheckTableName, AID);
            User_Admin_Mail_Check retObj = GenericFetch.FetchFromDB<User_Admin_Mail_Check>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new User_Admin_Mail_Check(AID);
        }

        public static Result_Define.eResult SetUser_Admin_Mail_Check(ref TxnBlock TB, User_Admin_Mail_Check setInfo, string dbkey = Mail_Define.Mail_Info_DB)
        {
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON AID = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                last_checked_main_idx = {2},
	                                                update_date = GETDATE()
                                                WHEN NOT MATCHED THEN
                                                   INSERT VALUES ({1}, {2}, GETDATE());
                                    ", Mail_Define.Mail_User_SystemMailCheckTableName, setInfo.AID, setInfo.last_checked_main_idx
                 );
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }
    }
}