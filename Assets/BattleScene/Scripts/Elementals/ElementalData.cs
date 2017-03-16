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
        public readonly BigInteger BaseExp;
        public readonly BigInteger BaseDps;
        public readonly int BaseCastIntervalXK; //  elemental 마다 고유하다.
        public readonly int FloatDigit; 

        public ElementalData(string id, string name, int castIntervalXK, BigInteger basedps, BigInteger baseexp)
        {
            ID = id;
            Name = name;
            FloatDigit = 1;
            BaseCastIntervalXK = castIntervalXK;
            BaseDps = basedps;
            BaseExp = baseexp;
        }
        public ElementalData(string id, string name, int castIntervalXK, double basedps, BigInteger baseexp)
        {
            ID = id;
            Name = name;
            BaseCastIntervalXK = castIntervalXK;
            FloatDigit = 10; // 소수점은 1000을 곱한뒤 int로 만든다.
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