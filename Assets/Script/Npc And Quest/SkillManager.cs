using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public C1 c1Skill;
    public C2 c2Skill;
    public C3 c3Skill;
    public C4 c4Skill;
    public Uiti ultiSkill;

    public void LockAllSkills()
    {
        if (c1Skill != null) c1Skill.canUseSkill = false;
        if (c2Skill != null) c2Skill.canUseSkill = false;
        if (c3Skill != null) c3Skill.canUseSkill = false;
        if (c4Skill != null) c4Skill.canUseSkill = false;
        if (ultiSkill != null) ultiSkill.canUseSkill = false;
    }

    public void UnlockAllSkills()
    {
        if (c1Skill != null) c1Skill.canUseSkill = true;
        if (c2Skill != null) c2Skill.canUseSkill = true;
        if (c3Skill != null) c3Skill.canUseSkill = true;
        if (c4Skill != null) c4Skill.canUseSkill = true;
        if (ultiSkill != null) ultiSkill.canUseSkill = true;
    }
}