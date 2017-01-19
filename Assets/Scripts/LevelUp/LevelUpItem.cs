using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class LevelUpItem // 레벨업을 하기 위해 구매해야하는 객체, canlevelup 이만들어지면 기생으로 붙는다. 저장된 게임 능력치와는 관계없다
    {
        private LevelUpItemData data;
        private OwnerType Type; // TODO: 사라져야함

        private BigInteger price; // original price
        private int priceXK;

        private BigInteger Price { get { return price * priceXK / 1000; } }
        private int PurchaseCount; // 구매횟수 == level;

        public string ID { get { return data.OwnerID; } } // 해당객체view의 name과같다 // id로 이름을바꿔야할듯
        public string Icon { get { return data.IconName; } }
        public string Info_Name { get { return data.InfoName; } } // owner.name과는다르다.
        public string Button_Text; // 아이템버튼 우하단 텍스트
        public string Info_Text; // 아이템버튼 인포창 텍스트

        public LevelUpItem(CanLevelUp owner, LevelUpItemData data, OwnerType type)
        {
            PurchaseCount = 0;
            this.data = data;
            Type = type;
            priceXK = 1000;

            if (type == OwnerType.hero)
            {
                UpdateLevelUpItem((Hero)owner);
            }
            else
            {
                UpdateLevelUpItem((Elemental)owner);
            }
        }

        internal void Purchase()
        {
            PurchaseCount++;
        }

        internal void Update()
        {
            if (PurchaseCount > 0)
            {
                if (Type == OwnerType.elemental)
                {
                    Singleton<ElementalManager>.getInstance().LevelUp(ID, PurchaseCount);
                    Singleton<HeroManager>.getInstance().UseExp(Price);
                }
                if (Type == OwnerType.hero)
                {
                    Singleton<HeroManager>.getInstance().LevelUp(ID, PurchaseCount);
                    Singleton<HeroManager>.getInstance().UseExp(Price);
                }
                PurchaseCount = 0;
            }
        }

        internal void UpdateLevelUpItem(Elemental elemental)
        {
            // 오리지널 price가 조정되고, 출력은 계산된 Price가된다
            price = elemental.NEXTEXP;

            // 버튼창에는 곱적용된 데미지 보여준다.
            Button_Text = elemental.DPS.ToSexyBackString();

            // 인포창에는 base 데미지만 보여준다.
            string description = Info_Name + " LV" + elemental.LEVEL + "\n";
            description += "Damage : " + elemental.BASEDPS.ToSexyBackString() + "/tap\n";
            description += "Next : +" + elemental.NEXTDPS.ToSexyBackString() + "/tap\n";
            description += "Cost : " + Price.ToSexyBackString() + " EXP";

            Info_Text = description;
        }

        internal void UpdateLevelUpItem(Hero hero)
        {
            price = new BigInteger(hero.NEXTEXP);
            Button_Text = hero.DPC.ToSexyBackString();

            string description = Info_Name + " LV" + hero.LEVEL+ "\n";
            description += "Damage : " + hero.BASEDPC + "/tap\n";
            description += "Next : +" + hero.NEXTDPC + "/tap\n";
            description += "Cost : " + Price.ToSexyBackString() + " EXP";

            Info_Text = description;
        }

    }

    public enum OwnerType
    {
        hero = 0,
        elemental = 1
    }

}