using System;

namespace SexyBackPlayScene
{
    internal class ElementalLevelUpItem : LevelUpItem
    {
        public ElementalLevelUpItem(LevelUpItemData data) : base(data)
        {
        }

        internal override void Update()
        {
            if (PurchaseCount > 0)
            {
                Singleton<ElementalManager>.getInstance().LevelUp(ID, PurchaseCount);
                Singleton<HeroManager>.getInstance().UseExp(Price);
                PurchaseCount = 0;
            }
        }

        internal override void UpdateLevelUpItem(CanLevelUp owner)
        {
            price = owner.NEXTEXP;

            // 버튼창에는 곱적용된 데미지 보여준다.
            Button_Text = owner.DPS.ToSexyBackString();

            // 인포창에는 base 데미지만 보여준다.
            string description = Info_Name + " LV" + owner.LEVEL + "\n";
            description += "Damage : " + owner.BASEDPS.ToSexyBackString() + "/tap\n";
            description += "Next : +" + owner.NEXTDPS.ToSexyBackString() + "/tap\n";
            description += "Cost : " + Price.ToSexyBackString() + " EXP";

            Info_Text = description;
        }
    }
}
