using System;

namespace SexyBackPlayScene
{
    public class ElementalData // 엘레멘탈의 콘스턴트 데이타 ( 변하지않는 )
    {
        public static double GrowthRate = 1.148698f; // / 100
        public int MaxLevel = 400;

        // from Excel
        public string ID;
        public string Name;
        public string SkillName;
        public int BaseLevel;
        public int BasePrice;
        public double BaseDmg;
        public int BaseCastIntervalXK; //  elemental 마다 고유하다.
        public string PrefabName;
        public string SkillPrefabName;
        public int BaseSkillRateXK;
        public int BaseSkillDamageXH;

        public ElementalData()
        {

        }
    }
}