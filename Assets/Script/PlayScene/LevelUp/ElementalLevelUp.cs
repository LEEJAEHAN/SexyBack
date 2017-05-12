using System;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class ElementalLevelUp : LevelUp
    {
        // ICanLevelUp
        //public string LevelUpDamageText { get { return DPC.To5String() + " /Tap"; } }
        //public string LevelUpNextText { get { return (BaseDpc * dpcX * dpcIncreaseXH / 100).To5String() + " /Tap"; } }

        public ElementalLevelUp(LevelUpData data, Elemental elemental) : base(data)
        {
            OwnerID = elemental.GetID;
            elemental.Action_Change += onElementalChange;
        }

        public override void Update()
        {
            for (int i = 0; i < PurchaseCount; PurchaseCount--)
            {
                if (Singleton<InstanceStatus>.getInstance().ExpUse(PRICE, true))
                    Singleton<ElementalManager>.getInstance().LevelUp(OwnerID, 1);
            }
        }

        internal void onElementalChange(Elemental elemental)
        {
            itemView.FillItemContents(elemental.LEVEL.ToString());
            originalPrice = elemental.PRICE;
            CalPrice();

            Name = OwnerName + " LV." + elemental.LEVEL.ToString();
            ElementalStat stat = Singleton<PlayerStatus>.getInstance().GetElementalStat(elemental.GetID);
            StatName = "피해량\n공격속도";
            StatValue = stat.DpsIncreaseXH.ToString() + "%\n" + stat.CastSpeedXH.ToString() + "%";
            Damage = elemental.DPS.To5String() + " 피해 / 초";
            PriceName = "다음\n요구";
            PriceValue = elemental.DPSTICK.To5String() + " 피해 / 초\n" + PRICE.To5String() + " 경험치";

            Refresh();
        }

    }
}
