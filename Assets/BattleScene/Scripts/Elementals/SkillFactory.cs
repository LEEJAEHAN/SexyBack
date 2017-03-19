using System;

namespace SexyBackPlayScene
{
    internal class SkillFactory
    {
        internal static Skill Create(string owner, string prefabname, int baseSkillRatio)
        {
            if (owner == "rock")
            {
                return new DropSkill(owner, prefabname, DamageType.Hit, baseSkillRatio, null, 15, 0.1f);
            }
            if (owner == "airball")
            {
                return new ShootSkill(owner, prefabname, DamageType.Hit, baseSkillRatio, null, 4f, 3, 0.2f);
            }


            //int PdmgRatio = baseSkillRatio / 3;
            //int DotRatio = baseSkillRatio * 2 / 3;
            //Debuff burn = new Debuff(DotRatio, 2);



            return new EmptySkill();
        }
    }
}