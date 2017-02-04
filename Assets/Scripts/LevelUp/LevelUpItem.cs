using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public abstract class LevelUpItem : IHasGridItem
        // 레벨업을 하기 위해 구매해야하는 객체, canlevelup 이만들어지면 기생으로 붙는다. 저장된 게임 능력치와는 관계없다
    {
        private LevelUpItemData data;
        protected GridItem itemView;
        protected bool Selected = false;

        public delegate void LevelUpChange_EventHandler(LevelUpItem levelupitem);
        public event LevelUpChange_EventHandler Action_LevelUpChange;

        // 돈관련, protected
        protected BigInteger originalprice; // original price
        private int priceXK;
        protected int PurchaseCount; // 구매횟수 == level;
        protected BigInteger Price { get { return originalprice * priceXK / 1000; } }

        internal string ID { get { return data.OwnerID; } } // 해당객체view의 name과같다 // id로 이름을바꿔야할듯
        internal string ViewName {  get { return "levelup_" + data.OwnerID; } }
        internal string Icon { get { return data.IconName; } }
        internal string Info_Name { get { return data.InfoName; } } // owner.name과는다르다.
        internal bool CanBuy = false;
        internal string Button_Text; // 아이템버튼 우하단 텍스트
        internal string Info_Text; // 아이템버튼 인포창 텍스트

        public LevelUpItem(LevelUpItemData data)
        {
            originalprice = new BigInteger(0);
            PurchaseCount = 0;
            this.data = data;
            priceXK = 1000;

            Singleton<StageManager>.getInstance().Action_ExpChange += onExpChange;
            itemView = new GridItem("LevelUp", ID, Icon, this);

            itemView.Show();
        }

        internal void Purchase()
        {
            if (CanBuy)
                PurchaseCount++;
        }

        internal abstract void Update();

        internal static string NameToID(string itemViewName)
        {
            return itemViewName.Substring("levelup_".Length);
        }

        public void onExpChange(BigInteger exp)
        {
            CanBuy = exp > Price;
            UpdateInfoView();
            UpdateItemView();
        }

        private void UpdateInfoView()
        {
            if (CanBuy)
                itemView.ConfirmEnable(Selected);
            else
                itemView.ConfirmDisable(Selected);
        }
        private void UpdateItemView()
        {
            if (CanBuy)
                itemView.Enable();
            else
                itemView.Disable();
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
        }

        public void onConfirm(string id)
        {
            Purchase();
        }
    }

}