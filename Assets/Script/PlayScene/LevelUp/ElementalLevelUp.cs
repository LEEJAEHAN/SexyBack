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
            StatName = "공격배수\n공격주기\n스킬확률\n스킬데미지";
            StatValue = string.Format("{0:N1}\n{1:N1}초\n{2:N1}%\n{3}%",
                elemental.DamageDensity,
                elemental.CastInterval,
                (double)elemental.SkillRateXH / 10f,
                elemental.SkillRatioXH);
            //+ elemental.DpsXH.ToString() + "%\n"
            PriceValue = PRICE.To5String();
            Damage = string.Format("+{0} / 초\n{1} / 초", elemental.DPSTICK.To5String(), elemental.DPS.To5String());
            ViewRefresh();
        }

    }
}
