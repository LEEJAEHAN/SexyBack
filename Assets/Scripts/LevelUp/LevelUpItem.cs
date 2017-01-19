using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public abstract class LevelUpItem// 레벨업을 하기 위해 구매해야하는 객체, canlevelup 이만들어지면 기생으로 붙는다. 저장된 게임 능력치와는 관계없다
    {
        private LevelUpItemData data;
        
        protected BigInteger price; // original price
        protected BigInteger Price { get { return price * priceXK / 1000; } }
        private int priceXK;
        protected int PurchaseCount; // 구매횟수 == level;

        public string ID { get { return data.OwnerID; } } // 해당객체view의 name과같다 // id로 이름을바꿔야할듯
        public string Icon { get { return data.IconName; } }
        public string Info_Name { get { return data.InfoName; } } // owner.name과는다르다.
        public string Button_Text; // 아이템버튼 우하단 텍스트
        public string Info_Text; // 아이템버튼 인포창 텍스트

        protected LevelUpItem(LevelUpItemData data)
        {
            PurchaseCount = 0;
            this.data = data;
            priceXK = 1000;
        }

        internal void Purchase()
        {
            // 돈-면못사게해야함
            PurchaseCount++;
        }
        internal abstract void Update();
        internal abstract void UpdateLevelUpItem(CanLevelUp owner);

    }


}