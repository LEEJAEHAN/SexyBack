using System;
using System.Runtime.Serialization;

namespace SexyBackPlayScene
{
    [Serializable]
    internal class PlayerStat
    {
        internal int ResearchTimeX; // 곱계수는 X를붙인다.
        internal int ResearchTime; // 보너스 공격스택횟수  6
        internal int ResearchThread;
        internal int ExpIncreaseXH;
        internal int LevelUpPriceXH;
        internal int ResearchPriceXH;

        public PlayerStat()
        {
            ResearchTimeX = 1;
            ResearchTime = 0;
            ResearchThread = 5; // for test
            ExpIncreaseXH = 100;
            LevelUpPriceXH = 100;
            ResearchPriceXH = 100;
        }
    }
    [Serializable]
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
            AttackCount = 3;
            DpcIncreaseXH = 100; // 
            AttackSpeedXH = 100;
            MovespeedXH = 1000;
            CriticalRateXH = 20;
            CriticalDamageXH = 425;
            AttackSpeedXH = 100;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
    [Serializable]
    internal class ElementalStat// 누적배수
    {
        internal BigInteger DpsX;
        internal int DpsIncreaseXH; // 
        internal int CastSpeedXH; //
        internal int skillrateIncreaseXH;
        internal int skilldamageIncreaseXH;

        internal ElementalStat()
        {
            DpsX = new BigInteger(1);
            DpsIncreaseXH = 100;
            CastSpeedXH = 100;
            skillrateIncreaseXH = 100;
            skilldamageIncreaseXH = 100;
        }

    }


}