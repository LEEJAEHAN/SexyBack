using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class LevelUpManager
    {
        Dictionary<string, LevelUp> levelUpItems = new Dictionary<string, LevelUp>();

        internal void Init()
        {
            // this class is event listner
            Singleton<ElementalManager>.getInstance().Action_ElementalCreateEvent += onElementalCreate;
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += onHeroCreate;
            ViewLoader.TabButton1.GetComponent<TabView>().Action_ShowList += onShowList;
            ViewLoader.TabButton1.GetComponent<TabView>().Action_HideList += onHideList;
        }
        public void DrawNewMark()
        {
            ViewLoader.TabButton1.transform.FindChild("New").gameObject.SetActive(true);
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
            foreach (LevelUp item in levelUpItems.Values)
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

        LevelUp CreateLevelUp(ICanLevelUp root)
        {
            if (Singleton<TableLoader>.getInstance().leveluptable.ContainsKey(root.GetID) == false)
                return null;

            LevelUp levelup = new LevelUp(Singleton<TableLoader>.getInstance().leveluptable[root.GetID], root);
            levelup.onExpChange(Singleton<Player>.getInstance().EXP);
            levelUpItems.Add(root.GetID, levelup);
            DrawNewMark();
            return levelup;
        }

        ///  for test
        internal void BuySelected()
        {
            foreach(LevelUp a in levelUpItems.Values)
            {
                if (a.Selected)
                    a.Purchase();
            }
        }

    }
}