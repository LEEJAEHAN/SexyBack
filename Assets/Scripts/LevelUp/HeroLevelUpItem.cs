using System;

namespace SexyBackPlayScene
{
    internal class HeroLevelUpItem : LevelUpItem
    {
        public HeroLevelUpItem(LevelUpItemData data) : base(data)
        {
        }

        internal override void Update()
        {
            if (PurchaseCount > 0)
            {
                Singleton<HeroManager>.getInstance().LevelUp(ID, PurchaseCount);
                Singleton<HeroManager>.getInstance().UseExp(Price);
                PurchaseCount = 0;
            }
        }

        internal override void UpdateLevelUpItem(CanLevelUp hero)
        {
            price = new BigInteger(hero.NEXTEXPSTR);

            Button_Text = hero.DPC.ToSexyBackString();

            string description = Info_Name + " LV" + hero.LEVEL + "\n";
            description += "Damage : " + hero.BASEDPC + "/tap\n";
            description += "Next : +" + hero.NEXTDPC + "/tap\n";
            description += "Cost : " + Price.ToSexyBackString() + " EXP";

            Info_Text = description;

        }
    }
}