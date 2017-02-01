using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class LevelUpManager
    {
        LevelUpItem currentItem = null;
        Dictionary<string, LevelUpItem> levelUpItems = new Dictionary<string, LevelUpItem>();

        // this class is event publisher
        public delegate void LevelUpCreate_EventHandler(LevelUpItem levelupitem);
        public event LevelUpCreate_EventHandler Action_LevelUpCreate;

        public delegate void LevelUpSelect_EventHandler(LevelUpItem levelupitem);
        public event LevelUpSelect_EventHandler noticeLevelUpSelect;

        internal void Init()
        {
            // this class is event listner
            Singleton<ElementalManager>.getInstance().Action_ElementalCreateEvent += this.onElementalCreate;
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += onHeroCreate;

            ViewLoader.LevelUpViewController.GetComponent<LevelUpViewController>().noticeConfirm += this.onConfirm;
            ViewLoader.LevelUpViewController.GetComponent<LevelUpViewController>().noticeSelect += this.onSelect;
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
            if (Singleton<TableLoader>.getInstance().leveluptable.ContainsKey(sender.ID) == false)
                return;

            LevelUpItemData data = Singleton<TableLoader>.getInstance().leveluptable[sender.ID];

            HeroLevelUpItem levelupItem = new HeroLevelUpItem(data);
            Action_LevelUpCreate(levelupItem);

            sender.Action_HeroChange += levelupItem.UpdateLevelUpItem;
            Singleton<StageManager>.getInstance().Action_ExpChange += levelupItem.onExpChange;

            levelupItem.UpdateLevelUpItem(sender);

            levelUpItems.Add(sender.ID, levelupItem); // sender의 레벨이아닌 data의 레벨
        }
        void onElementalCreate(Elemental sender) // create and bind element item
        {
            if (Singleton<TableLoader>.getInstance().leveluptable.ContainsKey(sender.ID) == false)
                return;

            LevelUpItemData data = Singleton<TableLoader>.getInstance().leveluptable[sender.ID];

            ElementalLevelUpItem levelupItem = new ElementalLevelUpItem(data);
            Action_LevelUpCreate(levelupItem);

            sender.Action_ElementalChange += levelupItem.UpdateLevelUpItem;
            Singleton<StageManager>.getInstance().Action_ExpChange += levelupItem.onExpChange;

            levelupItem.UpdateLevelUpItem(sender);

            levelUpItems.Add(sender.ID, levelupItem); // sender의 레벨이아닌 data의 레벨
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