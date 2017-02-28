﻿namespace SexyBackPlayScene
{
    internal class PlayerStat
    {
        internal int ResearchTimeX; // 곱계수는 X를붙인다.
        internal int ResearchTime; // 보너스 공격스택횟수  6
        internal int ResearchThread;
        internal int ExpIncreaseXH;

        public PlayerStat()
        {
            ResearchTimeX = 1;
            ResearchTime = 0;
            ResearchThread = 5; // for test
            ExpIncreaseXH = 100;
        }
    }
    internal class HeroStat
    {
        internal BigInteger DpcX; // 곱계수는 X를붙인다.
        internal int AttackCount; // 보너스 공격스택횟수  6
        internal int DpcIncreaseXH; // 
        internal int AttackSpeedXH;
        internal int CriticalRateXH;
        internal int CriticalDamageXH;
        internal int MovespeedXH;

        internal HeroStat()
        {
            DpcX = 1;
            AttackCount = 10;
            DpcIncreaseXH = 100; // 
            AttackSpeedXH = 100;
            MovespeedXH = 1000;
            CriticalRateXH = 10;
            CriticalDamageXH = 200;
            AttackSpeedXH = 100;
        }
    }
    internal class ElementalStat // 누적배수
    {
        internal BigInteger DpsX;
        internal int DpsIncreaseXH; // 
        internal int CastSpeedXH; //

        internal ElementalStat()
        {
            DpsX = new BigInteger(1);
            DpsIncreaseXH = 100;
            CastSpeedXH = 100;
        }
    }


}