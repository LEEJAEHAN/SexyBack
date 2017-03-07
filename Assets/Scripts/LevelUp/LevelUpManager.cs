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

        void onHeroCreate(Hero hero) // create and bind heroitem
        {
            if (Singleton<TableLoader>.getInstance().leveluptable.ContainsKey(hero.GetID) == false)
                return;

            LevelUp levelup = new HeroLevelUp(Singleton<TableLoader>.getInstance().leveluptable[hero.GetID], hero);
            levelup.SetStat(Singleton<StatManager>.getInstance().GetPlayerStat);
            levelUpItems.Add(hero.GetID, levelup);
            DrawNewMark();
        }

        void onElementalCreate(Elemental elemental) // create and bind element item
        {
            if (Singleton<TableLoader>.getInstance().leveluptable.ContainsKey(elemental.GetID) == false)
                return;

            LevelUp levelup = new ElementalLevelUp(Singleton<TableLoader>.getInstance().leveluptable[elemental.GetID], elemental);
            levelup.SetStat(Singleton<StatManager>.getInstance().GetPlayerStat);
            levelUpItems.Add(elemental.GetID, levelup);
            DrawNewMark();
            return;
        }


        ///  for test
        internal void BuySelected()
        {
            foreach(LevelUp item in levelUpItems.Values)
            {
                if (item.Selected)
                    item.Purchase();
            }
        }

        internal void SetStat(PlayerStat stat)
        {
            foreach (LevelUp item in levelUpItems.Values)
            {
                if (item.Selected)
                    item.SetStat(stat);
            }
        }
    }
}