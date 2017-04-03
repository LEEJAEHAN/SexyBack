using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    abstract internal class LevelUp : IHasGridItem
    // 레벨업을 하기 위해 구매해야하는 객체, canlevelup 이만들어지면 기생으로 붙는다. 저장된 게임 능력치와는 관계없다
    {
        ~LevelUp()
        {
            sexybacklog.Console("LevelUp 소멸");
        }
        internal string ID;// 해당객체view의 name
        internal GridItemIcon Icon;
        protected GridItem itemView;
        LevelUpWindow Panel;

        protected BigInteger PRICE;
        protected BigInteger originalPrice = new BigInteger(0); // original price
        int priceXH; // stat

        protected int PurchaseCount = 0; // 구매횟수 == level;

        internal bool CanBuy = false;
        public bool Selected = false;
        bool Learn = false;

        protected string OwnerID;
        protected string OwnerName;
 
        protected string Name;
        protected string StatName; // 아이템버튼 인포창 텍스트
        protected string StatValue; // 아이템버튼 인포창 텍스트최하단
        protected string PriceName; // 아이템버튼 인포창 텍스트최하단
        protected string PriceValue; // 아이템버튼 인포창 텍스트최하단
        protected string Damage; // 아이템버튼 인포창 텍스트최하단

        public LevelUp(LevelUpData data)
        {
            ID = data.ID;
            OwnerID = data.OwnerID;
            OwnerName = data.OwnerName;
            Icon = new GridItemIcon(data.IconName);

            Singleton<StatManager>.getInstance().Action_ExpChange += onExpChange;
            itemView = new GridItem("LevelUp", ID, Icon, ViewLoader.Tab1Container);
            itemView.AttachEventListner(this);
            Panel = LevelUpWindow.getInstance;
        }

        internal void Purchase()
        {
            if (CanBuy)
                PurchaseCount++;
        }

        public void onExpChange(BigInteger exp)
        {
            CanBuy = exp > PRICE;
            Refresh();
        }

        public void onSelect(string id)
        {
            if (id == null)
            {
                Selected = false;
                Panel.Action_Confirm -= this.onConfirm;
                Panel.Hide();
                return;
            }

            Selected = true;
            Panel.Action_Confirm += this.onConfirm;
            Refresh();
        }

        public void onConfirm()
        {   // 중복입력 막는다.
            if(PurchaseCount == 0)
                Purchase();
        }

        internal void SetStat(PlayerStat stat)
        {
            priceXH = stat.LevelUpPriceXH;
            CalPrice();
            Refresh();
        }
        protected void CalPrice()
        {
            PRICE = originalPrice * priceXH / 100;
            CanBuy = Singleton<StatManager>.getInstance().EXP > PRICE;
        }

        public void Refresh()
        {
            if (CanBuy)
                Panel.SetButton1(Selected, true);
            else
                Panel.SetButton1(Selected, false);

            if (CanBuy)
                itemView.Enable();
            else
                itemView.Disable();

            if (!Learn)
            {
                itemView.SetActive(true);
                Learn = true;
            }
            
            Panel.Show(Selected, Icon, Name, StatName, StatValue, PriceName, PriceValue, Damage);
        }
        // function

        public abstract void Update();
    }

}