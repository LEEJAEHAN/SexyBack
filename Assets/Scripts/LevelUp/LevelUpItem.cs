using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class LevelUpItem // 레벨업을 하기 위해 구매해야하는 객체
    {
        public string ID; // 해당객체view의 name과같다 // id로 이름을바꿔야할듯
        public string OwnerID; // 해당객체view의 name과같다 // id로 이름을바꿔야할듯
        public CanLevelUp target;

        public int PurchaseCount; // 구매횟수 == level;

        public string IconName;

        public string Item_Text { get { return target.Item_Text; } } // 아이템버튼 우하단 텍스트

        public string Info_Name; // owner.name과는다르다.
        public string Info_Description { get { return target.Info_Description; } }
        public BigInteger Info_Price { get { return target.PriceToNextLv; } }

        public LevelUpItem(CanLevelUp owner, LevelUpItemData data)
        {
            target = owner;
            target.levelupItem = this;

            ID = owner.ID;    // owner는 어차피 levelup 아이템을 하나만가지니까 통일해보자;
            OwnerID = owner.ID;
            Info_Name = data.InfoName;
            IconName = data.IconName;
            PurchaseCount = data.PurchasedCount;
        }

        internal void Purchase()
        {
            PurchaseCount++;
        }


    }
}