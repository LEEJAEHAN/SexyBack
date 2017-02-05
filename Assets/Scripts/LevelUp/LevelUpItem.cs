using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public abstract class LevelUpItem : IHasGridItem
        // 레벨업을 하기 위해 구매해야하는 객체, canlevelup 이만들어지면 기생으로 붙는다. 저장된 게임 능력치와는 관계없다
    {
        protected GridItem itemView;

        public delegate void LevelUpChange_EventHandler(LevelUpItem levelupitem);
        public event LevelUpChange_EventHandler Action_LevelUpChange;

        // 돈관련, protected
        protected BigInteger originalprice = new BigInteger(0); // original price
        private int priceXK = 1000;
        protected int PurchaseCount = 0; // 구매횟수 == level;
        protected BigInteger Price { get { return originalprice * priceXK / 1000; } }

        internal string ID;// 해당객체view의 name과같다 // id로 이름을바꿔야할듯
        internal string OwnerID;
        internal string Icon;
        internal string Info_Name; 

        internal string Button_Text; // 아이템버튼 우하단 텍스트
        internal string Info_Text; // 아이템버튼 인포창 텍스트

        internal bool CanBuy = false;
        public bool Selected = false;
        bool Learn = false;


        public LevelUpItem(LevelUpItemData data)
        {
            ID = data.ID;
            OwnerID = data.OwnerID;
            Icon = data.IconName;
            Info_Name = data.InfoName;

            Singleton<StageManager>.getInstance().Action_ExpChange += onExpChange;
            itemView = new GridItem("LevelUp", ID, Icon, ViewLoader.Tab1Container, this);

            itemView.SetActive(false);
            Refresh();
        }

        internal abstract void Update();

        internal void Purchase()
        {
            if (CanBuy)
                PurchaseCount++;
        }
        
        public void onExpChange(BigInteger exp)
        {
            CanBuy = exp > Price;
            Refresh();

        }

        public void Refresh()
        {
            if (CanBuy)
                itemView.ConfirmEnable(Selected);
            else
                itemView.ConfirmDisable(Selected);

            if (CanBuy)
                itemView.Enable();
            else
                itemView.Disable();

            if (!Learn)
            {
                itemView.SetActive(true);
                Learn = true;
            }
        }

        public void onSelect(string id)
        {
            if (id == null)
            {
                Selected = false;
                itemView.ClearInfo();
                return;
            }

            Selected = true;
            itemView.FillInfo(Selected, Icon, Info_Text);
            Refresh();
        }

        public void onConfirm(string id)
        {
            Purchase();
        }

    }

}