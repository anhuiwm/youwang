using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager.DBClass;

namespace TheSoul.DataManager.DBClass
{
    public class System_Soul_Active
    {
        public long SoulID { get; set; }
        public string NamingCN { get; set; }
        public string DescCN { get; set; }
        public byte ClassType { get; set; }
        public long SoulGroup { get; set; }
        public byte Grade { get; set; }
        public int GradeUp_Gold { get; set; }
        public long Need_Soul_Equip_1 { get; set; }
        public long Need_Soul_Equip_2 { get; set; }
        public long Need_Soul_Equip_3 { get; set; }
        public long Need_Soul_Equip_4 { get; set; }
        public long Need_Soul_Equip_5 { get; set; }
        public long Need_Soul_Equip_6 { get; set; }
        public long PrimarySkillID_1 { get; set; }
        public long PrimarySkillID_2 { get; set; }
        public long PrimarySkillID_3 { get; set; }
        public long Special_Buff_M_1 { get; set; }
        public long Special_Buff_M_2 { get; set; }
        public long Special_Buff_M_3 { get; set; }
        public long Special_Buff_R_1 { get; set; }
        public long Special_Buff_R_2 { get; set; }
        public long Special_Buff_R_3 { get; set; }
        public long Special_Buff_E_1 { get; set; }
        public long Special_Buff_E_2 { get; set; }
        public long Special_Buff_E_3 { get; set; }
        public long Next_Hon { get; set; }
        public string Card_Face_Atlas { get; set; }
        public string Card_Face_UI { get; set; }
        public string Skill_Icon_Atlas { get; set; }
        public string Card_Face_Btn_A_UI { get; set; }
        public string Card_Face_Btn_X_UI { get; set; }
        public string Skill_Btn_Atlas { get; set; }
        public string Skill_Btn_A_UI_1 { get; set; }
        public string Skill_Btn_A_UI_2 { get; set; }
        public string Skill_Btn_A_UI_3 { get; set; }
        public string Skill_Btn_X_UI_1 { get; set; }
        public string Skill_Btn_X_UI_2 { get; set; }
        public string Skill_Btn_X_UI_3 { get; set; }
        public string Skill_Btn_Y_UI_1 { get; set; }
        public string Skill_Btn_Y_UI_2 { get; set; }
        public string Skill_Btn_Y_UI_3 { get; set; }
        public string Skill_Btn_Z_UI_1 { get; set; }
        public string Skill_Btn_Z_UI_2 { get; set; }
        public string Skill_Btn_Z_UI_3 { get; set; }
        public string SOUL_KIND_1 { get; set; }
        public string SOUL_KIND_2 { get; set; }
        public string SOUL_KIND_3 { get; set; }
    }

    public class System_Soul_Skill_Group
    {
        public long Soul_SkillGroupID { get; set; }
        public long GroupID { get; set; }
        public long Group { get; set; }
        public byte Star_Level { get; set; }
        public long BuffID { get; set; }
    }

    public class System_Soul_Parts
    {
        public long SoulPartsIndex { get; set; }
        public int SoulPartsValue { get; set; }
        public long Soul_Group { get; set; }
        public byte Create_Star_Level { get; set; }
    }

    public class System_Soul_Evolution
    {
        public long Soul_Evolution_ID { get; set; }
        public long Group_ID { get; set; }
        public byte Soul_Star_Level { get; set; }
        public int Need_Soul_Parts { get; set; }
        public long SKill_Level_Group { get; set; }
        public byte Special_Buff_M_Level { get; set; }
        public byte Special_Buff_R_Level { get; set; }
        public byte Special_Buff_E_Level { get; set; }
        public long Buff_1 { get; set; }
        public long Buff_2 { get; set; }
        public long Buff_3 { get; set; }
    }

    public class System_Soul_Equip
    {
        public long Soul_Equip_ID { get; set; }
        public string description { get; set; }
        public byte Grade { get; set; }
        public string Name { get; set; }
        public string Tooltip { get; set; }
        public byte EquipSoulLevel { get; set; }
        public long Craft_Index { get; set; }
        public int AttackPower { get; set; }
        public int Skill_AP { get; set; }
        public float Cooltime { get; set; }
        public int Add_HP { get; set; }
        public int Add_MP { get; set; }
        public float Critical_Probability { get; set; }
        public int Decrease_Damage { get; set; }
    }

    public class System_Soul_Craft
    {
        public long Crafted_Item_Index { get; set; }
        public string description { get; set; }
        public long Material1_Index { get; set; }
        public string description_Material1 { get; set; }
        public byte Material1_Value { get; set; }
        public long Material2_Index { get; set; }
        public string description_Material2 { get; set; }
        public byte Material2_Value { get; set; }
        public long Material3_Index { get; set; }
        public string description_Material3 { get; set; }
        public byte Material3_Value { get; set; }
        public long Material4_Index { get; set; }
        public string description_Material4 { get; set; }
        public byte Material4_Value { get; set; }
        public int Craft_Gold { get; set; }
    }

    public class System_Soul_Passive
    {
        public long Soul_PassiveID { get; set; }
        public string NamingCN { get; set; }
        public string DescCN { get; set; }
        public long GroupIndex { get; set; }
        public byte Grade { get; set; }
        public byte Level { get; set; }
        public int LevelUp_EXP { get; set; }
        public int Material_EXP { get; set; }
        public long Buff_1 { get; set; }
        public long Buff_2 { get; set; }
        public long Buff_3 { get; set; }
        public string Grade_UI { get; set; }
        public string Card_Face_Atlas { get; set; }
        public string Card_Face_UI { get; set; }
        public string Skill_Btn_Atlas { get; set; }
        public string Skill_Btn_A_UI { get; set; }
    }

    public class System_Soul_Passive_Prob
    {
        public long Soul_Passive_ProbID { get; set; }
        public float PROB { get; set; }
        public long PassiveID { get; set; }
    }

    public class System_Skill
    {
        public long SkillID { get; set; }
        public string Description { get; set; }
        public string NameCN { get; set; }
        public string SkillTooltipCN { get; set; }
        public string UniqueOptionTooltipCN { get; set; }
        public string SkillGrade { get; set; }
        public long BuffGroup { get; set; }
        public long SkillGroup { get; set; }
        public byte SkillType { get; set; }
        public byte AttackType1 { get; set; }
        public byte AttackType2 { get; set; }
        public string BoolPass { get; set; }
        public float PushDistance { get; set; }
        public byte Check_HitType { get; set; }
        public int AniSequenseNum { get; set; }
        public long AniTimeGroupID { get; set; }
        public string AttackEffectPrefeb { get; set; }
        public string HitEffectPrefeb { get; set; }
        public string RadiusAttackEffectPrefeb { get; set; }
        public byte SummonEffect { get; set; }
        public byte SuperArmorAttack { get; set; }
        public byte SuperArmorDefence { get; set; }
        public string boolAutoTargeting { get; set; }
        public float AutoTargetingRange { get; set; }
        public float AttackAngle { get; set; }
        public float AttackRange { get; set; }
        public float AttackPosition { get; set; }
        public byte RadiusType { get; set; }
        public float RadiusAttackAngle { get; set; }
        public float RadiusAttackRange { get; set; }
        public float RadiusAttackPosition { get; set; }
        public float AttackRange2 { get; set; }
        public float Cooltime { get; set; }
        public string MPType { get; set; }
        public short MPValue { get; set; }
        public string BubbleType { get; set; }
        public byte BubbleValue { get; set; }
        public string APCalType { get; set; }
        public string LoopAnimation { get; set; }
        public float LoopTime { get; set; }
        public byte SkillMoveType { get; set; }
        public float MoveSpeed { get; set; }
        public long Buff_ID1 { get; set; }
        public long Buff_ID2 { get; set; }
        public long Buff_ID3 { get; set; }
        public float ChargingTime { get; set; }
        public byte MaxCharging { get; set; }
        public float Charging0_Multiplied_SkillAP { get; set; }
        public float Charging1_Multiplied_SkillAP { get; set; }
        public float Charging2_Multiplied_SkillAP { get; set; }
        public float Charging3_Multiplied_SkillAP { get; set; }
        public long ProjectileID { get; set; }
        public float ProjectilePosition { get; set; }
        public long LinkSkillID { get; set; }
        public long SkillLevelID { get; set; }
        public long SummonNPCID1 { get; set; }
        public long SummonNum1 { get; set; }
        public long SummonNPCID2 { get; set; }
        public long SummonNum2 { get; set; }
        public long SummonNPCID3 { get; set; }
        public long SummonNum3 { get; set; }
        public string Playskillsound1 { get; set; }
        public string Playskillsound2 { get; set; }
        public string HitSound { get; set; }
        public string CameraShaking { get; set; }
        public string AI { get; set; }
        public int AttackPower { get; set; }
        public float Add_SkillAP { get; set; }
        public int Add_HP { get; set; }
        public int Add_MP { get; set; }
        public int Decrease_Damage { get; set; }
        public float Decrease_Cooltime { get; set; }
        public float Critical_Probability { get; set; }
    }

    public class System_Skill_Level
    {
        public long Skill_Level_Index { get; set; }
        public long SkillLevelGroup { get; set; }
        public string Description { get; set; }
        public byte SkillLevel { get; set; }
        public long Buff_ID_PC1 { get; set; }
        public float LevelValue1_PC1 { get; set; }
        public float LevelValue2_PC1 { get; set; }
        public long Buff_ID_PC2 { get; set; }
        public float LevelValue1_PC2 { get; set; }
        public float LevelValue2_PC2 { get; set; }
        public long Buff_ID_PC3 { get; set; }
        public float LevelValue1_PC3 { get; set; }
        public float LevelValue2_PC3 { get; set; }
        public int LevelUp_Gold { get; set; }
    }

    public class System_Skill_Option
    {
        public long Skill_OptionIndex { get; set; }
        public long SkillOptionGroupID { get; set; }
        public byte Count { get; set; }
        public long Buff_GroupID { get; set; }
    }

    public class System_Skill_Animation
    {
        public long Skill_AnimationIndex { get; set; }
        public long AniTimeGroupID { get; set; }
        public byte Count { get; set; }
        public long GroupIndex { get; set; }
        public string AniName { get; set; }
        public string SwitchOnOff { get; set; }
        public float StartAttackTime { get; set; }
        public float StartSubAttackTime { get; set; }
        public float RadiusAttackTime { get; set; }
        public float PreinputTime { get; set; }
        public float StartProjecttileTime { get; set; }
        public float StartCountAttackTime { get; set; }
        public float EndCountAttackTime { get; set; }
        public byte SkillSelector { get; set; }
        public float StartRangeAttackTime { get; set; }
    }

    public class System_Skill_Buff_Group
    {
        public long Skill_Buff_GroupIndex { get; set; }
        public long Buff_GroupID { get; set; }
        public byte Count { get; set; }
        public long BuffID { get; set; }
    }

    public class System_Skill_Buff
    {
        public long Skill_BuffID { get; set; }
        public string BuffName { get; set; }
        public string SPECIAL_BUFF_NAME { get; set; }
        public string SPECIAL_BUFF_ICON { get; set; }
        public string SpecialOptionTooltipCN { get; set; }
        public byte BuffApplyProbability { get; set; }
        public string BuffType { get; set; }
        public long BuffGroup { get; set; }
        public int BuffLevel { get; set; }
        public string BuffOverlap { get; set; }
        public string BuffApplyEventType { get; set; }
        public string StateType { get; set; }
        public int StateArg { get; set; }
        public int ApplyEffectType1 { get; set; }
        public string BuffTarget1 { get; set; }
        public int ApplyEffectType2 { get; set; }
        public string BuffTarget2 { get; set; }
        public int ApplyEffectType3 { get; set; }
        public string BuffTarget3 { get; set; }
    }

    public class System_Skill_Buff_Effect
    {
        public long Skill_Buff_EffectID { get; set; }
        public string EffectPrefeb { get; set; }
        public string EffectType { get; set; }
        public string ValueType { get; set; }
        public float LifeTime { get; set; }
        public float CheckTime { get; set; }
        public float Value1 { get; set; }
        public float Value2 { get; set; }
        public string Description { get; set; }
    }

    public class System_Skill_Projectile
    {
        public long ProjectileID { get; set; }
        public string ProjectilePrefeb { get; set; }
        public float SphereRadius { get; set; }
        public byte ProjectileType { get; set; }
        public byte ProjectileAttackType { get; set; }
        public float ProjectilePushDistance { get; set; }
        public byte ProjectileSuperArmorAttack { get; set; }
        public float ProjectileLifeTime { get; set; }
        public float ProjectileSpeed { get; set; }
        public float ProjectileRange { get; set; }
        public float BomMaxHeight { get; set; }
        public long SkillLevelID { get; set; }
        public float ProjectileRadiusAttackRange { get; set; }
        public float ProjectileAttackPeriod { get; set; }
        public float CollisionRemain { get; set; }
        public string HitEffectPrefeb { get; set; }
        public string RadiusAttackEffectPrefeb { get; set; }
        public string ProjectileTerminateEffect { get; set; }
        public string ProjectileDestroySound { get; set; }
        public short ProjectilePrabola { get; set; }
        public long ApplyBuffID { get; set; }
        public long SkillID { get; set; }
    }

    public class System_Skill_Hon_Effect
    {
        public long Hon_Effect_ID { get; set; }
        public string Description { get; set; }
        public string HonModel { get; set; }
        public string HonAnimation { get; set; }
        public string SummonEffect { get; set; }
        public string FadeoutEffect { get; set; }
        public string TrailEffect { get; set; }
        public string ATKGroundEffect { get; set; }
    }

    public class User_ActiveSoul
    {
        public long soulseq { get; set; }
        public long aid { get; set; }
        public long soulid { get; set; }
        public long soulgroup { get; set; }
        public int soulparts_ea { get; set; }
        public byte grade { get; set; }
        public byte level { get; set; }
        public byte starlevel { get; set; }
        public long special_buffid1 { get; set; }
        public long special_buffid2 { get; set; }
        public long special_buffid3 { get; set; }
        public DateTime creation_date { get; set; }
        public string delflag { get; set; }
        //public byte hide { get; set; }
        public List<User_ActiveSoul_Equip> soulequiplist { get; set; }

        public User_ActiveSoul() { soulequiplist = new List<User_ActiveSoul_Equip>(); }
    }

    public class User_PassiveSoul
    {
        public long soulseq { get; set; }
        public long aid { get; set; }
        public long cid { get; set; }
        public long soulid { get; set; }
        public long soulgroup { get; set; }
        public byte class_type { get; set; }
        public byte level { get; set; }
        //public int soulexp { get; set; }
        public string stateflag { get; set; }       // passivesoul state define 3 state ( "C" : create, "E" : Equip, "S" : Store )
        public string delflag { get; set; }
        public DateTime creation_date { get; set; }
    }

    public class User_Soul_Make_Info
    {
        public long AID { get; set; }
        public int Total_Passive_Make_Count { get; set; }
        public int Passive_Make_Count { get; set; }
        public DateTime Passive_Make_regdate { get; set; }
    }

    public class User_Character_Equip_Soul
    {
        public long aid { get; set; }
        public long cid { get; set; }
        public long soulseq { get; set; }
        public byte soul_type { get; set; }
        public byte class_type { get; set; }
        public byte slot_num { get; set; }
    }

    public class User_Soul_Equip_Inven
    {
        public long equipinvenseq { get; set; }
        public long aid { get; set; }
        public long soul_equip_id { get; set; }
        public int soul_equip_ea { get; set; }
    }

    public class User_ActiveSoul_Equip
    {
        public long soulequipseq { get; set; }
        public long aid { get; set; }
        public long soulseq { get; set; }
        public long soul_equip_id { get; set; }
        public string delflag { get; set; }
    }

    public class User_ActiveSoul_Special_Buff
    {
        public long aid { get; set; }
        public long cid { get; set; }
        public long soulseq { get; set; }
        public long special_buffid1 { get; set; }
        public long special_buffid2 { get; set; }
        public long special_buffid3 { get; set; }
    }

    public class Ret_Soul_Active_Account
    {
        public long cid { get; set; }
        public List<Ret_Soul_Active> active_soul_list { get; set; }
        public List<Ret_Soul_Passive> passive_soul_list { get; set; }
    }

    public class Ret_Soul_Active
    {
        public long soulseq { get; set; }
        public long aid { get; set; }
        public long cid { get; set; }
        public long soulid { get; set; }
        public int soulparts_ea { get; set; }
        public byte grade { get; set; }
        public byte level { get; set; }
        public byte starlevel { get; set; }
        public long special_buffid1 { get; set; }
        public long special_buffid2 { get; set; }
        public long special_buffid3 { get; set; }
        public List<long> soulequip_id_list { get; set; }

        public Ret_Soul_Active() { }
        public Ret_Soul_Active(User_ActiveSoul setSoul)
        {
            soulseq = setSoul.soulseq;
            aid = setSoul.aid;
            cid = 0;
            soulid = setSoul.soulid;
            soulparts_ea = setSoul.soulparts_ea;
            grade = setSoul.grade;
            level = setSoul.level;
            starlevel = setSoul.starlevel;
            special_buffid1 = setSoul.special_buffid1;
            special_buffid2 = setSoul.special_buffid2;
            special_buffid3 = setSoul.special_buffid3;
            soulequip_id_list = new List<long>();
        }

        public static List<Ret_Soul_Active> makeActiveSoulRetList(ref TxnBlock TB, long AID, long CID, ref List<User_ActiveSoul> setList, ref List<User_ActiveSoul_Equip> equipList, ref List<User_ActiveSoul_Special_Buff> buffList)
        {
            List<Ret_Soul_Active> retObj = new List<Ret_Soul_Active>();

            foreach (User_ActiveSoul item in setList)
            {
                if (item.soulseq > 0 && item.soulid > 0)
                {
                    Ret_Soul_Active setObj = new Ret_Soul_Active(item);
                    List<long> setEquip = new List<long>();
                    equipList.FindAll(soul => soul.soulseq == item.soulseq).ForEach(equip =>
                    {
                        setEquip.Add(equip.soul_equip_id);
                    });
                    setObj.soulequip_id_list = setEquip;

                    var setBuff = buffList.Find(soul => soul.soulseq == item.soulseq);
                    if (setBuff == null)
                        setBuff = new User_ActiveSoul_Special_Buff();

                    DataManager_Define.eCountryCode serviceArea = SystemData.GetServiceArea(ref TB);
                    // korea and china version use diff limit grade.
                    byte checkNeedGrade = serviceArea == DataManager_Define.eCountryCode.China ?
                                    Soul_Define.Soul_Special_Buff_Need_Grade_1_CN :
                                    Soul_Define.Soul_Special_Buff_Need_Grade_1_KR;

                    // korea version didn't auto set buff
                    if (serviceArea == DataManager_Define.eCountryCode.China || serviceArea == DataManager_Define.eCountryCode.Taiwan)
                    {
                        if (
                        (item.grade >= checkNeedGrade && setBuff.special_buffid1 <= 0)
                        || (item.grade >= Soul_Define.Soul_Special_Buff_Need_Grade_2 && setBuff.special_buffid2 <= 0)
                        || (item.grade >= Soul_Define.Soul_Special_Buff_Need_Grade_3 && setBuff.special_buffid3 <= 0)
                        )
                        {
                            SoulManager.MakeSpecialBuff(ref TB, AID, CID, item.soulseq, ref setBuff);
                        }
                    }
                    else
                    {
                        if (item.grade >= Soul_Define.Soul_Special_Buff_Need_Grade_3 && setBuff.special_buffid3 <= 0)
                        {
                            SoulManager.MakeSpecialBuff(ref TB, AID, CID, item.soulseq, ref setBuff);
                        }
                    }

                    setObj.cid = CID;
                    setObj.special_buffid1 = setBuff.special_buffid1;
                    setObj.special_buffid2 = setBuff.special_buffid2;
                    setObj.special_buffid3 = setBuff.special_buffid3;

                    retObj.Add(setObj);
                }
            }

            return retObj;
        }
    }

    public class Ret_Equip_Soul_Active
    {
        public long soulseq { get; set; }
        public long aid { get; set; }
        public long cid { get; set; }
        public int slot_num { get; set; }
        public long soulid { get; set; }
        public byte grade { get; set; }
        public byte level { get; set; }
        public byte starlevel { get; set; }
        public long special_buffid1 { get; set; }
        public long special_buffid2 { get; set; }
        public long special_buffid3 { get; set; }
        public List<long> soulequip_id_list { get; set; }

        public Ret_Equip_Soul_Active() { }
        public Ret_Equip_Soul_Active(User_ActiveSoul setSoul, int set_slot = 0, long CID = 0)
        {
            soulseq = setSoul.soulseq;
            aid = setSoul.aid;
            soulid = setSoul.soulid;
            grade = setSoul.grade;
            level = setSoul.level;
            starlevel = setSoul.starlevel;
            special_buffid1 = setSoul.special_buffid1;
            special_buffid2 = setSoul.special_buffid2;
            special_buffid3 = setSoul.special_buffid3;
            slot_num = set_slot;
            cid = slot_num > 0 ? CID : 0;
            soulequip_id_list = new List<long>();
        }

        public static List<Ret_Equip_Soul_Active> makeActiveSoulRetEquipList(ref TxnBlock TB, long AID, long CID, ref List<User_ActiveSoul> setList, ref List<User_Character_Equip_Soul> setEquipList, ref List<User_ActiveSoul_Equip> equipList, ref List<User_ActiveSoul_Special_Buff> buffList)
        {
            List<Ret_Equip_Soul_Active> retObj = new List<Ret_Equip_Soul_Active>();

            foreach (User_ActiveSoul item in setList)
            {
                if (item.soulseq > 0)
                {
                    var findEquip = setEquipList.Find(equip => equip.soul_type == Soul_Define.Equip_Soul_Type_Acitve && equip.soulseq == item.soulseq);
                    int setSlot = findEquip != null ? findEquip.slot_num : 0;
                    if (setSlot > 0)
                    {
                        Ret_Equip_Soul_Active setObj = new Ret_Equip_Soul_Active(item, setSlot, CID);
                        List<long> setEquip = new List<long>();
                        equipList.FindAll(soul => soul.soulseq == item.soulseq).ForEach(equip =>
                        {
                            setEquip.Add(equip.soul_equip_id);
                        });
                        setObj.soulequip_id_list = setEquip;

                        var setBuff = buffList.Find(soul => soul.soulseq == item.soulseq);
                        if (setBuff == null)
                            setBuff = new User_ActiveSoul_Special_Buff();

                        // korea and china version use diff limit grade.
                        byte checkNeedGrade = SystemData.GetServiceArea(ref TB) == DataManager_Define.eCountryCode.China ?
                            Soul_Define.Soul_Special_Buff_Need_Grade_1_CN :
                            Soul_Define.Soul_Special_Buff_Need_Grade_1_KR;
            
                        if (
                            (item.grade >= checkNeedGrade && setBuff.special_buffid1 <= 0)
                            || (item.grade >= Soul_Define.Soul_Special_Buff_Need_Grade_2 && setBuff.special_buffid2 <= 0)
                            || (item.grade >= Soul_Define.Soul_Special_Buff_Need_Grade_3 && setBuff.special_buffid3 <= 0)
                            )
                        {
                            SoulManager.MakeSpecialBuff(ref TB, AID, CID, item.soulseq, ref setBuff);
                        }

                        setObj.special_buffid1 = setBuff.special_buffid1;
                        setObj.special_buffid2 = setBuff.special_buffid2;
                        setObj.special_buffid3 = setBuff.special_buffid3;

                        retObj.Add(setObj);
                    }
                }
            }
            return retObj;
        }
    }

    public class Ret_Soul_Passive
    {
        public long soulseq { get; set; }
        public long aid { get; set; }
        public long cid { get; set; }
        public long soulid { get; set; }
        public byte class_type { get; set; }
        public byte level { get; set; }
        //public string stateflag { get; set; }       // passivesoul state define 3 state ( "C" : create, "E" : Equip, "S" : Store )    // not use yet
        public int slot_num { get; set; }

        public Ret_Soul_Passive() { }
        public Ret_Soul_Passive(User_PassiveSoul setSoul, int set_slot = 0)
        {
            soulseq = setSoul.soulseq;
            aid = setSoul.aid;
            cid = setSoul.cid;
            soulid = setSoul.soulid;
            level = setSoul.level;
            class_type = setSoul.class_type;
            //stateflag = setSoul.stateflag;
            slot_num = set_slot;
        }

        public static List<Ret_Soul_Passive> makePassiveSoulRetList(ref List<User_PassiveSoul> setList, List<User_Character_Equip_Soul> setEquipList, bool bEquip)
        {
            List<Ret_Soul_Passive> retObj = new List<Ret_Soul_Passive>();

            setList.ForEach(item =>
            {
                if (item.soulseq > 0)
                {
                    var findEquip = setEquipList.Find(equip => equip.soul_type == Soul_Define.Equip_Soul_Type_Passive && equip.soulseq == item.soulseq);
                    int setSlot = findEquip != null ? findEquip.slot_num : 0;

                    if (bEquip)
                    {
                        if (setSlot > 0)
                            retObj.Add(new Ret_Soul_Passive(item, setSlot));
                    }
                    else
                        retObj.Add(new Ret_Soul_Passive(item, setSlot));
                }
            }
            );

            return retObj;
        }
    }

    public class User_Soul_Passive_Store_Count
    {
        public long Passive_Store_Count { get; set; }
    }

    public class Return_DisassableSoulEquip_List
    {
        public long equipinvenseq { get; set; }
        public long soul_equip_id { get; set; }
        public int soul_equip_ea { get; set; }

        public Return_DisassableSoulEquip_List() { }
        public Return_DisassableSoulEquip_List(User_Soul_Equip_Inven setItem)
        {
            equipinvenseq = setItem.equipinvenseq;
            soul_equip_id = setItem.soul_equip_id;
            soul_equip_ea = setItem.soul_equip_ea;
        }
    }

    public class SoulEquipSlot
    {
        public long cid { get; set; }
        public long soulseq { get; set; }
        public short slotnum { get; set; }
    }

    // old code : not use yet
    public class ActiveSoul
    {
        public long soulseq { get; set; }
        public long aid { get; set; }
        public long cid { get; set; }
        public int soulid { get; set; }
        public string soulname { get; set; }
        public byte classtype { get; set; }
        public byte soullevel { get; set; }
        public byte soulgradelevel { get; set; }
        public int special_buff1 { get; set; }
        public int special_buff2 { get; set; }
        public int special_buff3 { get; set; }
        public byte slotnum { get; set; }
    }

    public class ActiveSoulMake
    {
        public long soulseq { get; set; }
        public long aid { get; set; }
        public long cid { get; set; }
        public int soulid { get; set; }
        public string soulname { get; set; }
        public byte classtype { get; set; }
        public byte soullevel { get; set; }
        public byte soulgradelevel { get; set; }
        public int special_buff1 { get; set; }
        public int special_buff2 { get; set; }
        public int special_buff3 { get; set; }
        public byte slotnum { get; set; }
        public int resultcode { get; set; }
    }

    public class ActiveSoulParts
    {
        public long AID { get; set; }
        public int ItemID { get; set; }
        public int SoulPartsIndex { get; set; }
        public int ItemEA { get; set; }
    }

    public class PassiveSoul
    {
        public long soulseq { get; set; }
        public int soulid { get; set; }
        public long aid { get; set; }
        public long cid { get; set; }
        public string soulname { get; set; }
        public byte sclass { get; set; }
        public byte soullevel { get; set; }
        public int buff { get; set; }
        public byte slotnum { get; set; }
    }
    public class PassiveSoulMake
    {
        public long soulseq { get; set; }
        public int soulid { get; set; }
        public long aid { get; set; }
        public long cid { get; set; }
        public string soulname { get; set; }
        public byte sclass { get; set; }
        public byte soullevel { get; set; }
        public int buff { get; set; }
        public int slotnum { get; set; }
        public int passivepoint { get; set; }
        public int remaincreatecnt { get; set; }
        public int cash { get; set; }
        public int resultcode { get; set; }
    }
    public class PassiveSoulLimit
    {
        public long AID { get; set; }
        public int PassiveLimitCost { get; set; }
    }
    public class GameData_SoulCreate
    {
        public int SoulPartsIndex { get; set; }

        public short SoulPartsValue { get; set; }

        public int CreateSoul { get; set; }

        public byte EVO_Level { get; set; }

    }
    public class GameData_PassiveSoulProb
    {
        public int ID { get; set; }
        public double PROB { get; set; }
        public int PassiveID { get; set; }
    }
    public class GameData_PassiveSoulProbMin
    {
        public double PROB { get; set; }
    }
    public class GameData_PassiveSoulProbTCNT
    {
        public int totalcount { get; set; }
    }
    public class GameData_Soul
    {
        public int SoulID { get; set; }

        public string NamingCN { get; set; }

        public string DescCN { get; set; }

        public byte ClassType { get; set; }

        public int SoulEvoIndex { get; set; }

        public byte Grade { get; set; }

        public short PassLevel { get; set; }

        public int PassItemIndex { get; set; }

        public short PassItemAmout { get; set; }

        public int PrimarySkillID_1 { get; set; }

        public int PrimarySkillID_2 { get; set; }

        public int PrimarySkillID_3 { get; set; }

        public int Special_Buff_M_1 { get; set; }

        public int Special_Buff_M_2 { get; set; }

        public int Special_Buff_M_3 { get; set; }

        public int Special_Buff_R_1 { get; set; }

        public int Special_Buff_R_2 { get; set; }

        public int Special_Buff_R_3 { get; set; }

        public int Special_Buff_E_1 { get; set; }

        public int Special_Buff_E_2 { get; set; }

        public int Special_Buff_E_3 { get; set; }

        public int Next_Hon { get; set; }

        public string Card_Face_Atlas { get; set; }

        public string Card_Face_UI { get; set; }

        public string Card_Name_Atlas { get; set; }

        public string Card_Name_UI { get; set; }

        public string Skill_Icon_Atlas { get; set; }

        public string Card_Face_Btn_A_UI { get; set; }

        public string Card_Face_Btn_X_UI { get; set; }

        public string Skill_Btn_Atlas { get; set; }

        public string Skill_Btn_A_UI_1 { get; set; }

        public string Skill_Btn_A_UI_2 { get; set; }

        public string Skill_Btn_A_UI_3 { get; set; }

        public string Skill_Btn_X_UI_1 { get; set; }

        public string Skill_Btn_X_UI_2 { get; set; }

        public string Skill_Btn_X_UI_3 { get; set; }

        public string Skill_Btn_Y_UI_1 { get; set; }

        public string Skill_Btn_Y_UI_2 { get; set; }

        public string Skill_Btn_Y_UI_3 { get; set; }

        public string Skill_Btn_Z_UI_1 { get; set; }

        public string Skill_Btn_Z_UI_2 { get; set; }

        public string Skill_Btn_Z_UI_3 { get; set; }

        public string SOUL_KIND_1 { get; set; }

        public string SOUL_KIND_2 { get; set; }

        public string SOUL_KIND_3 { get; set; }

    }
    public class ActiveSoulListClass
    {
        public long SoulSEQ { get; set; }
        public int SoulID { get; set; }
        public int SoulLevel { get; set; }
        public int SoulGradeLevel { get; set; }
        public int Buff1 { get; set; }
        public int Buff2 { get; set; }
        public int Buff3 { get; set; }
        public int SlotNum { get; set; }
        public int SoulPartsItemID { get; set; }
        public int SoulPartsItemEA { get; set; }
        public int ClassType { get; set; }
    }

    public class GameData_SkillLevel
    {
        public int Index { get; set; }

        public int SkillLevelID { get; set; }

        public string Description { get; set; }

        public int SkillLevel { get; set; }

        public int Buff_ID { get; set; }

        public int LevelValue1 { get; set; }

        public int LevelValue2 { get; set; }

        public int LevelUp_Gold { get; set; }

    }
    public class ActiveSoulLevelUP
    {
        public int resultcode { get; set; }
        public long soulseq { get; set; }
        public byte soullevel { get; set; }
        public int gold { get; set; }
    }
    public class ActiveSoulLevelPass
    {
        public int resultcode { get; set; }
        public long soulseq { get; set; }
        public int soulid { get; set; }
        public byte soullevel { get; set; }
        public int special_buff1 { get; set; }
        public int special_buff2 { get; set; }
        public int special_buff3 { get; set; }
        public long passitem_invenseq { get; set; }
        public int passitem_itemea { get; set; }
    }
    public class GameData_SoulSkillGroup
    {
        public int Index { get; set; }
        public int GroupID { get; set; }
        public byte Group { get; set; }
        public byte Level { get; set; }
        public int BuffID { get; set; }
    }
    public class ActiveSoulGradeUP
    {
        public int resultcode { get; set; }
        public long soulseq { get; set; }
        public byte soulgradelevel { get; set; }
        public int gradeupitem_itemea { get; set; }
    }
    public class GameData_SoulEvolution
    {
        public int ID { get; set; }
        public int Group_ID { get; set; }
        public short Soul_EVO_Level { get; set; }
        public short Need_Soul_Parts { get; set; }
        public short Add_Skill_AP_1 { get; set; }
        public short Add_Skill_AP_2 { get; set; }
        public short Add_Skill_AP_3 { get; set; }
        public short Special_Buff_M_Level { get; set; }
        public short Special_Buff_R_Level { get; set; }
        public short Special_Buff_E_Level { get; set; }
        public int Buff_1 { get; set; }
        public int Buff_2 { get; set; }
        public int Buff_3 { get; set; }
        public string Tooltip_1 { get; set; }
        public string Tooltip_2 { get; set; }
        public string Tooltip_3 { get; set; }
    }
    public class ActiveSoulBuffChange
    {
        public int resultcode { get; set; }
        public int buff { get; set; }
    }
    public class PassiveSoulGrind
    {
        public int resultcode { get; set; }
        public int passivesoulexp { get; set; }
    }
    public class GameData_SoulPassive
    {
        public int ID { get; set; }
        public string NamingCN { get; set; }
        public string DescCN { get; set; }
        public int GroupIndex { get; set; }
        public int Level { get; set; }
        public int LevelUp_EXP { get; set; }
        public int Material_EXP { get; set; }
        public int Buff_1 { get; set; }
        public int Buff_2 { get; set; }
        public int Buff_3 { get; set; }
        public string Grade { get; set; }
        public string Card_Face_Atlas { get; set; }
        public string Card_Face_UI { get; set; }
        public string Skill_Btn_Atlas { get; set; }
        public string Skill_Btn_A_UI { get; set; }
    }
    public class PassiveSoulLevelUP
    {
        public int resultcode { get; set; }
        public long soulseq { get; set; }
        public int soulid { get; set; }
        public int soullevel { get; set; }
        public int buff { get; set; }
        public int passivesoulexp { get; set; }
    }
    public class PassiveSoulMoveStorage
    {
        public int resultcode { get; set; }
        public List<SoulEquipSlot> passiveslotnumlist { get; set; }
    }

    public class Client_Use_SkillInfo
    {
        public long cid { get; set; }
        public long groupid { get; set; }
        public byte level { get; set; }
        public long maxdmg { get; set; }
    }

    public class Error_Skill_Info
    {
        public Client_Use_SkillInfo errskill { get; set; }
        public Character_Stat charstat { get; set; }
        public System_Skill_Level skillinfo { get; set; }
        public float calc_max_damage { get; set; }
    }

    public class Admin_System_SoulGroup_Active
    {
        public long soulgroup { get; set; }
        public byte hide { get; set; }
    }

    public class User_HackDetect
    {
        public long aid { get; set; }
        public string username { get; set; }
        public int total_detect_count { get; set; }
        public int today_detect_count { get; set; }
        public DateTime reg_date { get; set; }
    }
}