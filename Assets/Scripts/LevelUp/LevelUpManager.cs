using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class LevelUpManager
    {
        List <LevelUpItem> items = new List<LevelUpItem>();
        LevelUpItem currentItem = null;

        Dictionary<string, LevelUpItemData> itemDatas = new Dictionary<string, LevelUpItemData>();
        Queue<LevelUpItem> processqueue = new Queue<LevelUpItem>();

        // this class is event publisher
        public delegate void LevelUpCreate_EventHandler (LevelUpItem levelupitem);
        public event LevelUpCreate_EventHandler onLevelUpCreate;

        public delegate void LevelUpSelect_EventHandler(LevelUpItem levelupitem);
        public event LevelUpSelect_EventHandler onLevelUpSelect;

        public delegate void LevelUpChange_EventHandler(LevelUpItem levelupitem);
        public event LevelUpChange_EventHandler onLevelUpChange;

        // view controller class ( 일단은 동적생성하지 않는다. )
        LevelUpViewController viewController = ViewLoader.LevelUpViewController.GetComponent<LevelUpViewController>();



        public void Init()
        {
            // init data
            LoadData();

            // this class is event listner
            Singleton<ElementalManager>.getInstance().onElementalCreate += this.onElementalCreate;
            Singleton<ElementalManager>.getInstance().onElementalChange += this.onElementalChange;
            viewController.Confirm += this.Confirm;
            viewController.Select += this.Select;
        }

        void LoadData()
        {
            LevelUpItemData item1 = new LevelUpItemData("fireball","파이어볼","SexyBackIcon_FireElemental", 1);
            LevelUpItemData item2 = new LevelUpItemData("waterball", "물폭탄", "SexyBackIcon_WaterElemental", 1);
            LevelUpItemData item3 = new LevelUpItemData("rock", "짱돌", "SexyBackIcon_RockElemental", 1);
            LevelUpItemData item4 = new LevelUpItemData("electricball", "지지직", "SexyBackIcon_ElectricElemental", 1);
            LevelUpItemData item5 = new LevelUpItemData("snowball", "눈덩이", "SexyBackIcon_SnowElemental", 1);
            LevelUpItemData item6 = new LevelUpItemData("earthball", "똥", "SexyBackIcon_EarthElemental", 1);
            LevelUpItemData item7 = new LevelUpItemData("airball", "바람바람", "SexyBackIcon_AirElemental", 1);
            LevelUpItemData item8 = new LevelUpItemData("iceblock", "각얼음", "SexyBackIcon_IceElemental", 1);
            LevelUpItemData item9 = new LevelUpItemData("magmaball", "메테오", "SexyBackIcon_MagmaElemental", 1);

            itemDatas.Add(item1.OwnerID, item1);
            itemDatas.Add(item2.OwnerID, item2);
            itemDatas.Add(item3.OwnerID, item3);
            itemDatas.Add(item4.OwnerID, item4);
            itemDatas.Add(item5.OwnerID, item5);
            itemDatas.Add(item6.OwnerID, item6);
            itemDatas.Add(item7.OwnerID, item7);
            itemDatas.Add(item8.OwnerID, item8);
            itemDatas.Add(item9.OwnerID, item9);
        }
        internal void Start()
        {
        }

        private void CreateLevelUpItem(Elemental owner, LevelUpItemData data)
        {
            LevelUpItem levelupItem = new LevelUpItem(owner, data);
            items.Add(levelupItem); // sender의 레벨이아닌 data의 레벨

            onLevelUpCreate(levelupItem);
        }

        internal void Update()
        {
            /// ui에서 confirm 클릭이들어오면 flag를 변경해놓을것이다.
            if(processqueue.Count > 0)
            {
                processqueue.Dequeue().Purchase();
            }

            // load해놓은 데이터엔 level이 높은게있을수도있다.
            foreach (LevelUpItem item in items)
            {
                if (item.PurchaseCount > item.target.LevelCount)
                {
                    Singleton<ElementalManager>.getInstance().SetLevel(item.target.ID, item.PurchaseCount);
                }
            }
        }

        public List<LevelUpItem> GetItemList()
        {
            return items;
        }
        private LevelUpItem FindItem(string id)
        {
            foreach (LevelUpItem item in items)
            {
                if (item.OwnerID == id)
                    return item;
            }
            return null;
        }


        internal void onExpChanged()
        {
            // 구현해야함
            // 업글할 수 있는것만 것만 표시;
            // confirm 버튼 활성화 비활성화;
        }

        ///  이하는 view 표시 관련, view 단의 작업. 절대 data를 변경해선 안된다. ( 데이터는 update에서 변경되어야 한다.)

        void Confirm()
        { // 모델을 변경시키는 행위기 때문에 실제 변경은 업데이트에서 수행되어야함;
            if (currentItem == null)
                return;
            processqueue.Enqueue(currentItem);
        }

        public void Select(string selecteItemID)
        {
            if (selecteItemID == null) // off event
            {
                currentItem = null;
                return;
            }

            currentItem = FindItem(selecteItemID);
            onLevelUpSelect(currentItem);
        }

        // recieve event
        // 데이터 체인지로부터 인한 이벤트
        void onElementalCreate(Elemental sender)
        {
            CreateLevelUpItem(sender, itemDatas[sender.ID]);
        }

        internal void onElementalChange(Elemental sender)
        {
            // 레벨업아이템을 갱신한다.
            onLevelUpChange(sender.levelupItem);
        }

    }
}