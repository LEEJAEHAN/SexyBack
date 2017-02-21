using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class LevelUp : IHasGridItem
    // 레벨업을 하기 위해 구매해야하는 객체, canlevelup 이만들어지면 기생으로 붙는다. 저장된 게임 능력치와는 관계없다
    {
        protected GridItem itemView;
        internal GridItemIcon Icon;

        InfoPanel infoPanel;

        WeakReference owner;
        // 돈관련, protected
        protected BigInteger originalprice = new BigInteger(0); // original price
        private int priceXK = 1000;
        protected int PurchaseCount = 0; // 구매횟수 == level;
        protected BigInteger Price { get { return originalprice * priceXK / 1000; } }

        internal string ID;// 해당객체view의 name과같다 // id로 이름을바꿔야할듯
        internal string OwnerID;
        internal string Info_Name;

        internal string Button_Text; // 아이템버튼 우하단 텍스트
        internal string Info_Text; // 아이템버튼 인포창 텍스트

        internal bool CanBuy = false;
        public bool Selected = false;
        bool Learn = false;
        
        public LevelUp(LevelUpData data, ICanLevelUp root)
        {
            owner = new WeakReference(root);
            root.Action_LevelUpInfoChange += onLevelChange;

            ID = data.ID;
            OwnerID = data.OwnerID;
            Icon = new GridItemIcon(data.IconName, null);
            Info_Name = data.InfoName;

            Singleton<Player>.getInstance().Action_ExpChange += onExpChange;
            itemView = new GridItem("LevelUp", ID, Icon, ViewLoader.Tab1Container);
            itemView.AttachEventListner(this);
            infoPanel = Singleton<InfoPanel>.getInstance();
        }

        internal void Update()
        {
            for (int i = 0; i < PurchaseCount; PurchaseCount--)
            {
                if (Singleton<Player>.getInstance().ExpUse(Price))
                    (owner.Target as ICanLevelUp).LevelUp(1);
            }
        }

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
                infoPanel.SetConfirmButton(Selected, true);
            else
                infoPanel.SetConfirmButton(Selected, false);

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
                infoPanel.Hide();
                return;
            }

            Selected = true;
            infoPanel.Show(Selected, Icon, Info_Text);
            Refresh();
        }

        public void onConfirm(string id)
        {   // 중복입력 막는다.
            if(PurchaseCount == 0)
                Purchase();
        }

        public void onPause(string id)
        {
        }


        internal void onLevelChange(ICanLevelUp sender)
        {
            originalprice = sender.LevelUpPrice;
            Button_Text = sender.LEVEL.ToString();

            Info_Text = Info_Name + " LV " + sender.LEVEL + "\n";
            Info_Text += "데미지 : " + sender.LevelUpDamageText + "\n";
            Info_Text += "다음레벨 : +" + sender.LevelUpNextText + "\n";
            Info_Text += "비용 : " + Price.To5String() + " ";

            itemView.FillItemContents(Button_Text);
            infoPanel.Show(Selected, Icon, Info_Text);
        }

    }

}