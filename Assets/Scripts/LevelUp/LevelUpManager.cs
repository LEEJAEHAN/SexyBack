using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class LevelUpManager
    {
        LevelUpItem currentItem = null;
        Dictionary<string, LevelUpItemData> levelUpDatas = new Dictionary<string, LevelUpItemData>();
        Dictionary<string, LevelUpItem> levelUpItems = new Dictionary<string, LevelUpItem>();

        // this class is event publisher
        public delegate void LevelUpCreate_EventHandler(LevelUpItem levelupitem);
        public event LevelUpCreate_EventHandler noticeLevelUpCreate;

        public delegate void LevelUpSelect_EventHandler(LevelUpItem levelupitem);
        public event LevelUpSelect_EventHandler noticeLevelUpSelect;

        public delegate void LevelUpChange_EventHandler(LevelUpItem levelupitem);
        public event LevelUpChange_EventHandler noticeLevelUpChange;


        // view controller class ( 일단은 동적생성하지 않는다. )
        LevelUpViewController viewController = ViewLoader.LevelUpViewController.GetComponent<LevelUpViewController>();

        internal void Init()
        {
            // init data
            LoadData();

            // this class is event listner
            Singleton<ElementalManager>.getInstance().Action_ElementalCreateEvent += this.onElementalCreate;
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += onHeroCreate;
            Singleton<GameMoney>.getInstance().noticeEXPChange += onExpChange;

            viewController.noticeConfirm += this.onConfirm;
            viewController.noticeSelect += this.onSelect;
        }
        void LoadData()
        {
            LevelUpItemData heroAttack = new LevelUpItemData("hero", "일반공격", "SexyBackIcon_SWORD2");

            LevelUpItemData item1 = new LevelUpItemData("fireball", "파이어볼", "SexyBackIcon_FireElemental");
            LevelUpItemData item2 = new LevelUpItemData("waterball", "물폭탄", "SexyBackIcon_WaterElemental");
            LevelUpItemData item3 = new LevelUpItemData("rock", "짱돌", "SexyBackIcon_RockElemental");
            LevelUpItemData item4 = new LevelUpItemData("electricball", "지지직", "SexyBackIcon_ElectricElemental");
            LevelUpItemData item5 = new LevelUpItemData("snowball", "눈덩이", "SexyBackIcon_SnowElemental");
            LevelUpItemData item6 = new LevelUpItemData("earthball", "똥", "SexyBackIcon_EarthElemental");
            LevelUpItemData item7 = new LevelUpItemData("airball", "바람바람", "SexyBackIcon_AirElemental");
            LevelUpItemData item8 = new LevelUpItemData("iceblock", "각얼음", "SexyBackIcon_IceElemental");
            LevelUpItemData item9 = new LevelUpItemData("magmaball", "메테오", "SexyBackIcon_MagmaElemental");

            levelUpDatas.Add(heroAttack.OwnerID, heroAttack);

            levelUpDatas.Add(item1.OwnerID, item1);
            levelUpDatas.Add(item2.OwnerID, item2);
            levelUpDatas.Add(item3.OwnerID, item3);
            levelUpDatas.Add(item4.OwnerID, item4);
            levelUpDatas.Add(item5.OwnerID, item5);
            levelUpDatas.Add(item6.OwnerID, item6);
            levelUpDatas.Add(item7.OwnerID, item7);
            levelUpDatas.Add(item8.OwnerID, item8);
            levelUpDatas.Add(item9.OwnerID, item9);
        }
        internal void Start()
        {
        }

        internal void Update()
        {
            foreach (LevelUpItem item in levelUpItems.Values)
                item.Update();
        }
        // recieve event
        // 데이터 체인지로부터 인한 이벤트
        void onHeroCreate(Hero sender) // create and bind heroitem
        {
            HeroLevelUpItem levelupItem = new HeroLevelUpItem(levelUpDatas[sender.ID]);
            levelUpItems.Add(sender.ID, levelupItem); // sender의 레벨이아닌 data의 레벨
            sender.Action_HeroChange += onHeroChange;
            noticeLevelUpCreate(levelupItem);

            noticeLevelUpChange(levelupItem);
        }
        void onElementalCreate(Elemental sender) // create and bind element item
        {
            ElementalLevelUpItem levelupItem = new ElementalLevelUpItem(levelUpDatas[sender.ID]);
            levelUpItems.Add(sender.ID, levelupItem); // sender의 레벨이아닌 data의 레벨
            sender.noticeElementalChange += onElementalChange;
            noticeLevelUpCreate(levelupItem);

            noticeLevelUpChange(levelupItem);
        }
        void onHeroChange(Hero sender) //update item
        {
            HeroLevelUpItem result = (HeroLevelUpItem)levelUpItems[sender.ID];
            result.UpdateLevelUpItem(sender);
            noticeLevelUpChange(result);
        }
        void onElementalChange(Elemental sender) //update item
        {
            ElementalLevelUpItem result = (ElementalLevelUpItem)levelUpItems[sender.ID];
            result.UpdateLevelUpItem(sender);
            noticeLevelUpChange(result);
        }
        void onExpChange(BigInteger exp)
        {
            foreach (LevelUpItem item in levelUpItems.Values)
            {
                noticeLevelUpChange(item);
            }
        }
        ///  view로부터의 이벤트 . 해당 메서드 런타임에서 절대 data를 변경해선 안된다. ( 데이터는 update에서 변경되어야 한다.)
        void onConfirm() // 모델을 변경시키는 행위기 때문에 실제 변경은 업데이트에서 수행되어야함;
        {
            if (currentItem == null)
                return;
            currentItem.Purchase();
        }
        void onSelect(string itemViewName) // selected item' id from view
        {
            if (itemViewName == null)
            {
                currentItem = null;
            }
            else if (itemViewName != null)
            {
                string itemID = LevelUpItem.NameToID(itemViewName);
                if (levelUpItems.ContainsKey(itemID))
                    currentItem = levelUpItems[itemID];
            }
            noticeLevelUpSelect(currentItem);
        }
    }
}