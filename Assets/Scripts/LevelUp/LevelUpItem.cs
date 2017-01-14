using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class LevelUpItem
    {
        public CanLevelUp target;
        public int level; // 조정될 레벨
        public const float GrowthRate = 1.17f;                                                                         // from Excel
        public BigInteger BaseExp; // 베이스exp로부터 level과 GrowthRate를 통해 ExpForNthLevel을 구함.                          // from Excel
        public string IconName;
        public string ItemViewID; // 해당객체view의 name과같다

        public LevelUpItem(CanLevelUp elemental, string iconName, int initLevel, BigInteger baseexp)
        {
            target = elemental;
            IconName = iconName;
            level = initLevel;
            BaseExp = baseexp;
            ItemViewID = elemental.ItemViewID;
        }

        public void Buy()
        {
            level++;
        }

        internal void ApplyProcess()
        {
            if (level > target.level)
                target.LevelUp(level);
        }
        
        internal string GetTargetDamage()
        {
            return target.GetDamageString();
        }

        public BigInteger PriceToNextLv
        {
            get
            {
                double growth = Mathf.Pow(GrowthRate, level);
                int intgrowth = 0;
                BigInteger result;

                if ((int)growth < int.MaxValue / 10000)
                {
                    intgrowth = (int)(growth * 10000);
                    result = BaseExp * intgrowth / 10000;
                }
                else
                {
                    intgrowth = (int)growth;
                    result = BaseExp * intgrowth;
                }
                return result;
            }
        }

    }
}