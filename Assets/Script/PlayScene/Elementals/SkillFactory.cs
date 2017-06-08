using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class SkillFactory
    {
        internal static Skill Create(string owner, string prefabname)
        {
            switch (owner)
            {
                case "fireball":
                    {
                        return new ShootSkill(owner, prefabname, DamageType.HitDebuff, Debuff.Type.Burn, 0.75f, 1, 0, true, true);
                    }
                case "iceblock":
                    {
                        return new ShootSkill(owner, prefabname, DamageType.Hit, Debuff.Type.None, 5f, 10, 0.1f, true, true);
                    }
                case "rock":
                    {
                        return new DropSkill(owner, prefabname, DamageType.Hit, Debuff.Type.None, 25, 0.1f);
                    }
                case "electricball":
                    {
                        return new ShootSkill(owner, prefabname, DamageType.HitPerHPHigh, Debuff.Type.None, 5f, 5, 0.2f, true, false);
                    }
                case "waterball":
                    {
                        return new ShootSkill(owner, prefabname, DamageType.HitDebuff, Debuff.Type.Poison, 0.75f, 1, 0, true, true);
                    }
                case "earthball":
                    {
                        return new CrashSkill(owner, prefabname, DamageType.HitPerHPLow, Debuff.Type.None, 1.3f, true);
                    }
                case "airball":
                    {
                        return new ShootSkill(owner, prefabname, DamageType.Hit, Debuff.Type.None, 5f, 5, 0.2f, false, false);
                    }
                case "snowball":
                    {
                        return new DropSkill(owner, prefabname, DamageType.Hit, Debuff.Type.None, 28, 0.1f);
                    }
                case "magmaball":
                    {
                        return new CrashSkill(owner, prefabname, DamageType.Hit, Debuff.Type.None, 1.6f, false);
                    }
                default:
                    return new EmptySkill();
            }
        }

        internal static Debuff CreateDebuff(Debuff.Type type, BigInteger Damage)
        {
            Debuff debuff = null;
            if (type == Debuff.Type.Burn)
            {
                int Duration = 2;
                debuff = new Debuff(Debuff.Type.Burn, Damage, Duration);
            }
            if (type == Debuff.Type.Poison)
            {
                int Duration = 10;
                debuff = new Debuff(Debuff.Type.Poison, Damage, Duration); //Damage * DotRatio / Duration
            }
            return debuff;
        }
    }
}