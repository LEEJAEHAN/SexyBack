using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public abstract class LevelUpItem// 레벨업을 하기 위해 구매해야하는 객체, canlevelup 이만들어지면 기생으로 붙는다. 저장된 게임 능력치와는 관계없다
    {
        private LevelUpItemData data;

        protected BigInteger originalprice; // original price
        protected BigInteger Price { get { return originalprice * priceXK / 1000; } }
        private int priceXK;
        private bool canbuy;

        protected int PurchaseCount; // 구매횟수 == level;

        internal string ID { get { return data.OwnerID; } } // 해당객체view의 name과같다 // id로 이름을바꿔야할듯
        internal string Icon { get { return data.IconName; } }
        internal string Info_Name { get { return data.InfoName; } } // owner.name과는다르다.
        internal string Button_Text; // 아이템버튼 우하단 텍스트
        internal string Info_Text; // 아이템버튼 인포창 텍스트

        protected LevelUpItem(LevelUpItemData data)
        {
            PurchaseCount = 0;
            this.data = data;
            priceXK = 1000;
        }
        internal void Purchase()
        {
            if(canbuy)
                PurchaseCount++;
        }
        internal bool Toggle(BigInteger exp)
        {
            bool pastCanbuy = canbuy;
            if (Price >= exp)
                canbuy = true;
            else
                canbuy = false;

            return pastCanbuy != canbuy; // has toggled
        }

        internal abstract void Update();
    }

}