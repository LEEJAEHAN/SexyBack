using System;

namespace SexyBackPlayScene
{
    public class ElementalData // 엘레멘탈의 콘스턴트 데이타 ( 변하지않는 )
    {
        public readonly string ID;
        public readonly string Name;
        public readonly double CreateActionTime;
        public readonly double GrowthRate = 1.148698f; // / 100
        public readonly int MaxLevel = 999;
        // from Excel
        public readonly BigInteger BaseExp; // 베이스exp로부터 level과 GrowthRate를 통해 ExpForNthLevel을 구함.                          // from Excel
        public readonly BigInteger BaseDps; // baseExp / expForBaseUnitDps(exp per dps) = dps; 레벨이 올라가면 항상베이스dps 만큼 올라간다// 계산되는값.
        public readonly int FloatDigit; 

        public readonly int AttackIntervalK; //  elemental 마다 고유하다.                                                     // from excel
        //public int ExpForBaseUnitDps; // 베이스기준 dps 1올리기위해 들어가는 exp ( 높을수록 가성비가 구리다는 것 )      // from Excel


        public ElementalData(string id, string name, int attackIntervalK, BigInteger basedps, BigInteger baseexp)
        {
            ID = id;
            Name = name;
            FloatDigit = 1;
            AttackIntervalK = attackIntervalK;
            BaseDps = basedps; // 원래는 basedps만입력받으면되는데, 값이너무더러워서 exp랑 rdps로 계산
            BaseExp = baseexp;

            //    ExpForBaseUnitDps = expperdps;
            //    BaseDps = baseexp / ExpForBaseUnitDps; // 원래는 basedps만입력받으면되는데, 값이너무더러워서 exp랑 rdps로 계산
        }
        public ElementalData(string id, string name, int attackIntervalK, double basedps, BigInteger baseexp)
        {
            ID = id;
            Name = name;
            AttackIntervalK = attackIntervalK;
            FloatDigit = 10000; // 소수점은 1000을 곱한뒤 int로 만든다.
            basedps *= FloatDigit;
            BaseDps = new BigInteger((int)basedps);
            BaseExp = baseexp;
        }

        public static string ProjectilePrefabName(string elementalid)
        {
            return "prefabs/Projectile/" + elementalid;
        }

        public static string ProjectileReadyStateName(string elementalid)
        {
            return elementalid + "_spot";
        }


    }
}