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
            StatName = "공격배수\n공격주기\n강타확률\n강타데미지";
            StatValue = string.Format("{0:N1}\n{1:N1}초\n{2:N1}%\n{3}%",
                    hero.DamageDensity,
                    hero.AttackInterval,
                    (double)hero.CriRateXK / 10f,
                    hero.CriDamageXH);
            //+ hero.DpcXH.ToString() + "%\n"
            Damage = hero.DPC.To5String() + " / 탭";
            PriceValue = string.Format("{0} / 탭\n[F9DB11]{1} 경험치[-]",
                hero.DPCTick.To5String(),
                PRICE.To5String());

            ViewRefresh();
        }

    }
}
