using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class HeroData
    {
        public static double GrowthRate = 1.148698f;

        public readonly string ID;
        public readonly string Name;
        public readonly double AttackInterval;
        public readonly float MoveSpeed;
        public readonly int BaseLevel;
        public readonly int BasePrice;
        public readonly double BaseDmg;
        public readonly int BaseSkillRateXK;
        public readonly int BaseSkillDamageXH;

        public HeroData()
        {
            ID = "hero";
            Name = "이재한";
            AttackInterval = 5;
            MoveSpeed = 3f;
            BaseLevel = 0;
            BasePrice = 60;
            BaseDmg = 1.025f;       // fireball's dmg
            BaseSkillRateXK = 200;
            BaseSkillDamageXH = 425;
        }
    }
}