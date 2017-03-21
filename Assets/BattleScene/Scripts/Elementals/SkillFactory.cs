using System;

namespace SexyBackPlayScene
{
    internal class SkillFactory
    {
        internal static Skill Create(string owner, string prefabname, int baseSkillRatio)
        {
            if (owner == "fireball")
            {
                return new ShootSkill(owner, prefabname, DamageType.HitDebuff, baseSkillRatio, "burn", 1.6f, 1, 0, true);
            }
            //if (owner == "rock")
            //{
            //    return new DropSkill(owner, prefabname, DamageType.Hit, baseSkillRatio, null, 15, 0.1f);
            //}
            //if (owner == "airball")
            //{
            //    return new ShootSkill(owner, prefabname, DamageType.Hit, baseSkillRatio, null, 5f, 5, 0.2f);
            //}

            return new EmptySkill();
        }

        internal static Debuff CreateDebuff(string debuffID, BigInteger Damage )
        {
            Debuff debuff = null;
            if(debuffID == "burn")
            {
                int DotRatio = 2;
                int Duration = 10;
                debuff = new Debuff(Damage * DotRatio / Duration, Duration);
            }
            if(debuffID == "poision")
            {
                int DotRatio = 1;
                int Duration = 30;
                debuff = new Debuff(Damage * DotRatio / Duration, Duration);
            }
            return debuff;
        }
    }
}