using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using TheSoul.DataManager.DBClass;


namespace TheSoul.DataManager.DBClass
{
    public class System_NPC
    {
        public int NPCID { get; set; }
        public string NamingCN { get; set; }
        public string DescCN { get; set; }
        public string Description { get; set; }
        public int NPCType { get; set; }
        public int NPCGrade { get; set; }
        public int Level { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public int MaxAP { get; set; }
        public int MinAp { get; set; }
        public int DP { get; set; }
        public int CRT { get; set; }
        public int CP { get; set; }
        public float MaxCriticalProb { get; set; }
        public float MaxDefenseRating { get; set; }
        public int EXPWeight { get; set; }
        public int DropBoxGroupID { get; set; }
        public string DefenseType { get; set; }
        public string NPCRes { get; set; }
        public string Res_Variation { get; set; }
        public string WeaponRes1 { get; set; }
        public string WeaponRes2 { get; set; }
        public string Animator_Type { get; set; }
        public string WeaponType { get; set; }
        public float? dummy_must { get; set; }
        public float? Collider_radius { get; set; }
        public float? Scale { get; set; }
        public int? DefalutSuperArmor { get; set; }
        public string DieSound { get; set; }
    }
}

namespace TheSoul.DataManager
{
    public static partial class NPC_Manager
    {
        // NPC System info
        const string NPC_DBName = "sharding";
        const string NPC_TableName = "System_NPC";
        const string NPC_Prefix = "System_NPC";
        const string NPC_Surfix = "Info";

        public static System_NPC GetNPCInfo(ref TxnBlock TB, long npcID, string dbkey = NPC_DBName, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", NPC_Prefix, NPC_Surfix);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE NPCID = {1}", NPC_TableName, npcID);
            System_NPC retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_NPC>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, npcID.ToString(), setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_NPC();
            return retObj;
        }
    }
}
