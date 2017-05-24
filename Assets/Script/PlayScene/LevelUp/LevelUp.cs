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
        internal NestedIcon Icon;
        protected GridItem itemView;
        LevelUpWindow Panel;

        protected BigInteger PRICE;
        protected BigInteger OriginalPrice = new BigInteger(0); // original price
        protected int LPriceReduceXH; // stat
        protected int PurchaseCount = 0; // 구매횟수 == level;

        public bool Selected = false;
        bool Learn = false;
        protected bool ViewRefreshFlag = true;

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
            Icon = new NestedIcon(data.IconName);

            Singleton<InstanceStatus>.getInstance().Action_ExpChange += onExpChange;
            itemView = new GridItem(GridItem.Type.LevelUp, ID, Icon, this);
            Panel = LevelUpWindow.getInstance;
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
            ViewRefresh();
        }

        public void onConfirm()     // purchase
        {   // 중복입력 막는다.
            if(PurchaseCount == 0 && Singleton<InstanceStatus>.getInstance().EXP > PRICE)
                PurchaseCount++;
        }

        public void onExpChange(BigInteger exp)
        {
            ViewRefreshFlag = true;
        }
        internal void SetStat()
        {
            UtilStat utilStat = Singleton<PlayerStatus>.getInstance().GetUtilStat;
            LPriceReduceXH = Mathf.Min(50, utilStat.LPriceReduceXH);// * (200 + baseStat.Luck) / 200;
            PRICE = OriginalPrice * (100 - LPriceReduceXH) / 100;
            ViewRefreshFlag = true;
        }
        public void ViewRefresh()
        {
            bool CanBuy = Singleton<InstanceStatus>.getInstance().EXP > PRICE;
            if (CanBuy)
                itemView.Enable();
            else
                itemView.Disable();

            if (!Learn)
            {
                itemView.SetActive(true);
                Learn = true;
            }

            if (Selected)
            {
                if (CanBuy)
                    Panel.SetButton1(Selected, true);
                else
                    Panel.SetButton1(Selected, false);
                Panel.Show(Selected, Icon, Name, StatName, StatValue, PriceName, PriceValue, Damage);
            }
        }
        // function

        public abstract void Update();
    }

}