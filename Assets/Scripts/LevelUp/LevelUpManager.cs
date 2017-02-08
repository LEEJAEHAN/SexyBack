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
            ViewLoader.Tab1Container.GetComponent<UIGrid>().Reposition();
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

        ///  for test
        internal void BuySelected()
        {
            foreach(LevelUpItem a in levelUpItems.Values)
            {
                if (a.Selected)
                    a.Purchase();
            }
        }

    }
}