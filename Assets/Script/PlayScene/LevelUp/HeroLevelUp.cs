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
            onHeroChange(hero);
        }

        public override void Update()
        {
            for (int i = 0; i < PurchaseCount; PurchaseCount--)
                if (Singleton<InstanceStatus>.getInstance().ExpUse(PRICE, true))
                    Singleton<HeroManager>.getInstance().LevelUp(1);
            if (ViewRefreshFlag)
                ViewRefresh();
        }

        internal void onHeroChange(Hero hero)
        {
            itemView.DrawLevel(hero.GetLevel.ToString(), hero.BonusLevel > 0);
            OriginalPrice = hero.PRICE;
            PRICE = OriginalPrice * (100 - LPriceReduceXH) / 100;

            Name = OwnerName + " LV." + hero.OriginalLevel;
            if (hero.BonusLevel > 0)
                Name += "+" + hero.BonusLevel;
            StatName = "기본공격력\n공격주기\n강타확률\n강타데미지";
            StatValue = hero.BaseDmg.ToString("N3") + "\n"
                + hero.AttackInterval.ToString("N1") + "초\n"
                + ((double)hero.CriRateXK / 10f).ToString("N1") + "%\n"
                + hero.CriDamageXH.ToString() + "%";
            //+ hero.DpcXH.ToString() + "%\n"
            Damage = hero.DPC.To5String() + " 피해 / 탭";
            PriceName = "다음\n요구";
            PriceValue = hero.DPCTick.To5String() + " 피해 / 탭\n" + PRICE.To5String() + " 경험치";

            ViewRefresh();
        }

    }
}
