using System;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class HeroLevelUp : LevelUp
    {
        // ICanLevelUp
        //public string LevelUpDamageText { get { return DPC.To5String() + " /Tap"; } }
        //public string LevelUpNextText { get { return (BaseDpc * dpcX * dpcIncreaseXH / 100).To5String() + " /Tap"; } }

        public HeroLevelUp(LevelUpData data, Hero hero) : base(data)
        {
            OwnerID = hero.GetID;
            hero.Action_Change += onHeroChange;
        }

        public override void Update()
        {
            for (int i = 0; i < PurchaseCount; PurchaseCount--)
            {
                if (Singleton<StatManager>.getInstance().ExpUse(PRICE, true))
                    Singleton<HeroManager>.getInstance().LevelUp(1);
            }
        }

        internal void onHeroChange(Hero hero)
        {
            itemView.FillItemContents(hero.LEVEL.ToString());
            originalPrice = hero.PRICE;
            CalPrice();

            Name = OwnerName + " LV." + hero.LEVEL.ToString();
            HeroStat stat = Singleton<StatManager>.getInstance().GetHeroStat;
            StatName = "피해량\n공격속도\n크리티컬\n크리티컬데미지";
            StatValue = stat.DpcIncreaseXH.ToString() + "%\n" + stat.AttackSpeedXH.ToString() + "%\n" + stat.CriticalRateXH.ToString() + "%\n" + stat.CriticalDamageXH.ToString() + "%";
            Damage = hero.DPC.To5String() + " 피해 / 탭";
            PriceName = "다음\n요구";
            PriceValue = hero.DPCTick.To5String() + " 피해 / 탭\n" + PRICE.To5String() + " 경험치";

            Refresh();
        }

    }
}
