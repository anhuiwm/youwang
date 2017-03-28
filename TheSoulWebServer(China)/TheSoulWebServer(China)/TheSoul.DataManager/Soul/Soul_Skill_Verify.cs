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

namespace TheSoul.DataManager
{
    public static partial class SoulManager
    {
        public static bool CheckSkillVerify(ref TxnBlock TB, long AID, List<Client_Use_SkillInfo> useSkill, ref Result_Define.eResult retError, out Error_Skill_Info setSkill)
        {
            Dictionary<long, Character_Stat> checkStat = new Dictionary<long, Character_Stat>();
            Dictionary<long, Character> checkClass = new Dictionary<long, Character>();
            setSkill = new Error_Skill_Info();
            foreach (Client_Use_SkillInfo chkSkill in useSkill)
            {
                if (chkSkill.groupid > 0)
                {
                    System_Skill_Level skillInfo = SoulManager.GetSoul_System_Skill_Level(ref TB, chkSkill.groupid, chkSkill.level);
                    if (skillInfo.SkillLevelGroup == chkSkill.groupid)
                    {
                        if (!checkStat.ContainsKey(chkSkill.cid))
                            checkStat[chkSkill.cid] = CharacterManager.GetCharacterStat(ref TB, chkSkill.cid);
                        if (!checkClass.ContainsKey(chkSkill.cid))
                            checkClass[chkSkill.cid] = CharacterManager.GetCharacter(ref TB, AID, chkSkill.cid);

                        if (chkSkill.cid != checkClass[chkSkill.cid].cid || chkSkill.cid != checkStat[chkSkill.cid].CID)
                        {
                            retError = Result_Define.eResult.CHARACTER_NOT_FOUND;
                            return true;
                        }
                                                
                        float Att_SkillValue = 
                            checkClass[chkSkill.cid].Class == (int)Character_Define.SystemClassType.Class_Warrior ? skillInfo.LevelValue1_PC1 :
                            (checkClass[chkSkill.cid].Class == (int)Character_Define.SystemClassType.Class_Swordmaster ? skillInfo.LevelValue1_PC2 + skillInfo.LevelValue2_PC2 * 5 :
                            (checkClass[chkSkill.cid].Class == (int)Character_Define.SystemClassType.Class_Taoist ? skillInfo.LevelValue1_PC3 : 0));

                        float calcMaxDamage = checkStat[chkSkill.cid].ATTACK_MAX * ((Att_SkillValue / 100 ) * 2) * checkStat[chkSkill.cid].CRITICAL_RATING * Soul_Define.Soul_Skill_Passive_Max_Damage_Facter;

                        if (chkSkill.maxdmg > calcMaxDamage)
                        {
                            setSkill.errskill = chkSkill;
                            setSkill.charstat = checkStat[chkSkill.cid];
                            setSkill.skillinfo = skillInfo;
                            setSkill.calc_max_damage = calcMaxDamage;
                            //retError = Result_Define.eResult.System_Hack_Detected;
                            //retError = Result_Define.eResult.SUCCESS;
                            retError = SoulManager.SetUserHackDetect(ref TB, AID);
                            TB.SetLogData("hack_skill", 1);                            
                            return true;
                        }
                    }
                    else
                    {
                        retError = Result_Define.eResult.SOUL_SYSTEM_INFO_NOT_FOUND;
                        return true;
                    }
                }
            }
            return true;
        }
    }
}
