using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class HeroData
    {
        public readonly string ID;
        public readonly string Name;
        public readonly double GrowthRate;
        public readonly double AttackInterval;
        public readonly float MoveSpeed;
        public readonly BigInteger BaseExp;
        public readonly BigInteger BaseDpc;
        public HeroData()
        {
            ID = "hero";
            Name = "이재한";
            GrowthRate = 1.148698f;
            AttackInterval = 5;
            MoveSpeed = 1f;
            BaseExp = new BigInteger(60);
            BaseDpc = new BigInteger(50000); // 60 / 5
        }
    }
}