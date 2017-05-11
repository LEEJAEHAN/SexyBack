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

        public HeroData()
        {
            ID = "hero";
            Name = "이재한";
            AttackInterval = 5;
            MoveSpeed = 3f;
            BaseLevel = 0;
            BasePrice = 60;
        }
    }
}