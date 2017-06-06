using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class ElementalLevelUp : LevelUp
    {
        public ElementalLevelUp(LevelUpData data, Elemental elemental) : base(data)
        {
            OwnerID = elemental.GetID;
            elemental.Action_Change += onElementalChange;
        }

        public override void Update()
        {
            for (int i = 0; i < PurchaseCount; PurchaseCount--)
                if (Singleton<InstanceStatus>.getInstance().ExpUse(PRICE, true))
                    Singleton<ElementalManager>.getInstance().LevelUp(OwnerID, 1);
            if (ViewRefreshFlag)
                ViewRefresh();
        }

        internal void onElementalChange(Elemental elemental)
        {
            itemView.DrawLevel(elemental.GetLevel.ToString(), elemental.BonusLevel > 0);
            OriginalPrice = elemental.PRICE;
            PRICE = OriginalPrice * (100 - LPriceReduceXH) / 100;

            Name = OwnerName + " LV." + elemental.OriginalLevel;
            if (elemental.BonusLevel > 0)
                Name += "+" + elemental.BonusLevel;
            StatName = "기본공격력\n공격주기\n스킬확률\n스킬데미지";
            StatValue = elemental.BaseDmg.ToString("N3") + "\n"
                + elemental.CastInterval.ToString("N1") + "초\n"
                + ((double)elemental.SkillRateXH / 10f).ToString("N1") + "%\n"
                + elemental.SkillRatioXH.ToString() + "%";
            //+ elemental.DpsXH.ToString() + "%\n"
            Damage = elemental.DPS.To5String() + " 피해 / 초";
            PriceName = "다음\n요구";
            PriceValue = elemental.DPSTICK.To5String() + " 피해 / 초\n" + PRICE.To5String() + " 경험치";

            ViewRefresh();
        }

    }
}
