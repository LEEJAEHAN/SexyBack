using System;

namespace SexyBackPlayScene
{
    internal class SkillFactory
    {
        internal static Skill Create(string owner, string prefabname, int baseSkillRatio)
        {
            if (owner=="rock")
            {
                return new DropSkill(owner, prefabname, DamageType.Hit, baseSkillRatio, null, 15, 0.1f);
            }

            return new EmptySkill();
        }
    }
}