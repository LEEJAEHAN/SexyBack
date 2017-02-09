using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class HeroData
    {
        public readonly string ID = "hero";
        public readonly string Name = "이재한";

        public readonly double GrowthRate = 1.148698f; // / 100
        public readonly double AttackInterval = 5;
        public readonly double CriRate = 0.15;
        public readonly int CriDamage = 200;
        public readonly float MoveSpeed = 0.05f;  // TODO : for test
        public readonly int AttackCount = 1;

        public readonly BigInteger BaseExp = new BigInteger(50);
        public readonly BigInteger BaseDpc = new BigInteger(5);


        public HeroData()
        {

        }
    }
}