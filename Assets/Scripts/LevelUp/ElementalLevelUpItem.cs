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
            for (int i = 0; i < PurchaseCount; PurchaseCount--)
            {
                Singleton<GameMoney>.getInstance().ExpUse(Price);
                Singleton<ElementalManager>.getInstance().LevelUp(ID);
            }
        }

        internal  void UpdateLevelUpItem(Elemental owner)
        {
            originalprice = owner.NEXTEXP;

            // 버튼창에는 곱적용된 데미지 보여준다.
            Button_Text = owner.LEVEL.ToString();
            //Button_Text = "[FFFF00]" + owner.DPS.ToSexyBackString() + "[-]";

            // 인포창에는 base 데미지만 보여준다.
            string description = Info_Name + " LV" + owner.LEVEL + "\n";
            description += "Damage : " + owner.BASEDPS.ToSexyBackString() + "/sec\n";
            description += "Next : +" + owner.NEXTDPS.ToSexyBackString() + "/sec\n";
            description += "Cost : " + Price.ToSexyBackString() + " EXP";

            Info_Text = description;
        }
    }
}
