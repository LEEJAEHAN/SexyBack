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
                Singleton<GameMoney>.getInstance().ExpUse(Price);
                Singleton<HeroManager>.getInstance().LevelUp(ID);
            }
        }

        internal void UpdateLevelUpItem(Hero hero)
        {
            originalprice = new BigInteger(hero.NEXTEXPSTR);

            Button_Text = hero.LEVEL.ToString();
            //Button_Text = hero.DPC.ToSexyBackString();

            string description = Info_Name + " LV" + hero.LEVEL + "\n";
            description += "Damage : " + hero.BASEDPC + "/tap\n";
            description += "Next : +" + hero.NEXTDPC + "/tap\n";
            description += "Cost : " + Price.To5String() + " EXP";

            Info_Text = description;
            Notice(this);
        }
    }
}