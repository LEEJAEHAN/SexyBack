using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class HeroData
    {
        public readonly string ID = "hero";
        public readonly string Name = "이재한";

        public readonly double GrowthRate = 1.148698f;
        public readonly double AttackInterval = 5;
        public readonly double CriRate = 0.10;
        public readonly int CriDamage = 200;
        public readonly float MoveSpeed = 2f;
        public readonly int AttackCount = 1;

        public readonly BigInteger BaseExp = new BigInteger(100);
        public readonly BigInteger BaseDpc = new BigInteger(10);

        public HeroData()
        {

        }
    }
}