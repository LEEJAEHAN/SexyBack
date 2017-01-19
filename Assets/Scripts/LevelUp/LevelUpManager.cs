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
        public event LevelUpCreate_EventHandler noticeLevelUpCreate;

        public delegate void LevelUpSelect_EventHandler(LevelUpItem levelupitem);
        public event LevelUpSelect_EventHandler noticeLevelUpSelect;

        public delegate void LevelUpChange_EventHandler(LevelUpItem levelupitem);
        public event LevelUpChange_EventHandler noticeLevelUpChange;

        // view controller class ( 일단은 동적생성하지 않는다. )
        LevelUpViewController viewController = ViewLoader.LevelUpViewController.GetComponent<LevelUpViewController>();



        public void Init()
        {
            // init data
            LoadData();

            // this class is event listner
            Singleton<ElementalManager>.getInstance().noticeElementalCreate += this.onElementalCreate;
            Singleton<ElementalManager>.getInstance().noticeElementalChange += onElementalChange;
            Singleton<HeroManager>.getInstance().noticeHeroCreate += onHeroCreate;
            Singleton<HeroManager>.getInstance().noticeHeroChange += onHeroChange;

            viewController.Confirm += this.Confirm;
            viewController.Select += this.Select;
        }

        void LoadData()
        {
            LevelUpItemData heroAttack = new LevelUpItemData("hero", "Normal Attack", "SexyBackIcon_MagmaElemental");
            
            LevelUpItemData item1 = new LevelUpItemData("fireball","파이어볼","SexyBackIcon_FireElemental");
            LevelUpItemData item2 = new LevelUpItemData("waterball", "물폭탄", "SexyBackIcon_WaterElemental");
            LevelUpItemData item3 = new LevelUpItemData("rock", "짱돌", "SexyBackIcon_RockElemental");
            LevelUpItemData item4 = new LevelUpItemData("electricball", "지지직", "SexyBackIcon_ElectricElemental");
            LevelUpItemData item5 = new LevelUpItemData("snowball", "눈덩이", "SexyBackIcon_SnowElemental");
            LevelUpItemData item6 = new LevelUpItemData("earthball", "똥", "SexyBackIcon_EarthElemental");
            LevelUpItemData item7 = new LevelUpItemData("airball", "바람바람", "SexyBackIcon_AirElemental");
            LevelUpItemData item8 = new LevelUpItemData("iceblock", "각얼음", "SexyBackIcon_IceElemental");
            LevelUpItemData item9 = new LevelUpItemData("magmaball", "메테오", "SexyBackIcon_MagmaElemental");

            itemDatas.Add(heroAttack.OwnerID, heroAttack);
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

        private void CreateLevelUpItem(CanLevelUp owner, LevelUpItemData data, OwnerType type)
        {
            LevelUpItem levelupItem = new LevelUpItem(owner, data, type);
            items.Add(levelupItem); // sender의 레벨이아닌 data의 레벨

            noticeLevelUpCreate(levelupItem);
        }

        internal void Update()
        {
            /// ui에서 confirm 클릭이들어오면 구매count를 변경해놓을것이다.
            foreach (LevelUpItem item in items)
            {
                item.Update();
            }
        }

        public List<LevelUpItem> GetItemList()
        {
            return items;
        }
        private LevelUpItem FindItemFromID(string id)
        {
            foreach (LevelUpItem item in items)
            {
                if (item.ID == id)
                    return item;
            }
            return null;
        }

        internal void onExpChanged()
        {
            // TODO: 구현해야함
            // 업글할 수 있는것만 것만 표시;
            // confirm 버튼 활성화 비활성화;
        }

        ///  이하는 view 표시 관련, view 단의 작업. 절대 data를 변경해선 안된다. ( 데이터는 update에서 변경되어야 한다.)

        void Confirm()
        { // 모델을 변경시키는 행위기 때문에 실제 변경은 업데이트에서 수행되어야함;
            if (currentItem == null)
                return;
            currentItem.Purchase();
        }

        public void Select(string selecteItemViewID)
        {
            if (selecteItemViewID == null) // off event
            {
                currentItem = null;
                return;
            }

            currentItem = FindItemFromID(selecteItemViewID);
            noticeLevelUpSelect(currentItem);
        }

        // recieve event
        // 데이터 체인지로부터 인한 이벤트
        void onElementalCreate(Elemental sender)
        {
            CreateLevelUpItem(sender, itemDatas[sender.ID], OwnerType.elemental);
            // TODO: LevelUpItem을상속해서 두개로나눠야 한다;
        }
        void onHeroCreate(Hero hero)
        {
            CreateLevelUpItem(hero, itemDatas[hero.ID], OwnerType.hero);
        }

        internal void onHeroChange(Hero hero)
        {
            LevelUpItem result = FindItemFromID(hero.ID);
            result.UpdateLevelUpItem(hero);
            noticeLevelUpChange(result);
        }
        internal void onElementalChange(Elemental elemental)
        {
            LevelUpItem result = FindItemFromID(elemental.ID);
            result.UpdateLevelUpItem(elemental);
            noticeLevelUpChange(result);
        }

    }
}