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
            for (int i = 0; i < PurchaseCount; PurchaseCount--)
            {
                if (Singleton<StageManager>.getInstance().ExpUse(Price))
                    Singleton<HeroManager>.getInstance().LevelUp(OwnerID);
            }
        }


        internal void onHeroChange(Hero hero)
        {
            originalprice = new BigInteger(hero.NEXTEXPSTR);

            Button_Text = hero.LEVEL.ToString();
            //Button_Text = hero.DPC.ToSexyBackString();

            string description = Info_Name + " LV" + hero.LEVEL + "\n";
            description += "Damage : " + hero.BASEDPC + "/tap\n";
            description += "Next : +" + hero.NEXTDPC + "/tap\n";
            description += "Cost : " + Price.To5String() + " EXP";

            Info_Text = description;

            itemView.FillItemContents(Button_Text);
            itemView.FillInfo(Selected, Icon, Info_Text);

        }
    }
}