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
            itemView.DrawLevel(elemental.LEVEL.ToString());
            OriginalPrice = elemental.PRICE;
            PRICE = OriginalPrice * (100 - LPriceReduceXH) / 100;

            Name = OwnerName + " LV." + elemental.LEVEL.ToString();
            StatName = "피해량\n공격속도\n스킬확률\n스킬데미지";
            StatValue = elemental.DpsXH.ToString() + "%\n"
                + elemental.CastSpeedXH.ToString() + "%\n"
                + ((double)elemental.SkillRateXH / 10f).ToString("N1") + "%\n"
                + elemental.SkillRatioXH.ToString() + "%";

            Damage = elemental.DPS.To5String() + " 피해 / 초";
            PriceName = "다음\n요구";
            PriceValue = elemental.DPSTICK.To5String() + " 피해 / 초\n" + PRICE.To5String() + " 경험치";

            ViewRefresh();
        }

    }
}
