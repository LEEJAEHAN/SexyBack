using System;

namespace SexyBackPlayScene
{
    public class ElementalData // 엘레멘탈의 콘스턴트 데이타 ( 변하지않는 )
    {
        public string ID;
        public string Name;
        public double CreateActionTime;
        public double GrowthRate = 1.148698f; // / 100
        public int MaxLevel = 999;
        // from Excel
        public BigInteger BaseExp;
        public BigInteger BaseDps;
        public int BaseCastIntervalXK; //  elemental 마다 고유하다.
        public int FloatDigit = 1;
        public string PrefabName;
        public string SkillPrefabName;
        public int BaseSkillRate;
        public int BaseSkillDamageXH;

        public ElementalData()
        {

        }
    }
}