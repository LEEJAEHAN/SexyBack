using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class LevelUpManager
    {
        Dictionary<string, LevelUpItem> levelUpItems = new Dictionary<string, LevelUpItem>();

        internal void Init()
        {
            // this class is event listner
            Singleton<ElementalManager>.getInstance().Action_ElementalCreateEvent += onElementalCreate;
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += onHeroCreate;
            ViewLoader.TabButton1.GetComponent<TabView>().Action_ShowList += onShowList;
            ViewLoader.TabButton1.GetComponent<TabView>().Action_HideList += onHideList;
        }

        private void onHideList()
        {
            ViewLoader.Tab1Container.SetActive(false);
           // ViewLoader.Info_Context.SetActive(false);
        }

        private void onShowList()
        {
            ViewLoader.Tab1Container.SetActive(true);
        }

        internal void Update()
        {
            foreach (LevelUpItem item in levelUpItems.Values)
                item.Update();
        }

        void onHeroCreate(Hero sender) // create and bind heroitem
        {
            CreateLevelUp(sender);
        }
        void onElementalCreate(Elemental sender) // create and bind element item
        {
            CreateLevelUp(sender);
        }

        LevelUpItem CreateLevelUp(ICanLevelUp root)
        {
            if (Singleton<TableLoader>.getInstance().leveluptable.ContainsKey(root.GetID) == false)
                return null;

            LevelUpItem levelupItem = new LevelUpItem(Singleton<TableLoader>.getInstance().leveluptable[root.GetID], root);
            levelUpItems.Add(root.GetID, levelupItem);
            return levelupItem;
        }

        ///  view로부터의 이벤트 . 해당 메서드 런타임에서 절대 data를 변경해선 안된다. ( 데이터는 update에서 변경되어야 한다.)
    }
}