using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class LevelUpManager : IDisposable
    {
        Dictionary<string, LevelUp> levelUpItems = new Dictionary<string, LevelUp>();
        bool RefreshStat = true;
        public GameObject TabButton1;
        public GameObject Tab1Container;
        public LevelUpWindow Panel;

        ~LevelUpManager()
        {
            sexybacklog.Console("LevelUpManager 소멸");
        }
        public void Dispose()
        {
            Singleton<PlayerStatus>.getInstance().Action_UtilStatChange -= this.onUtilStatChange;
            //LevelUpWindow.Clear();
        }

        internal void Init()
        {
            // this class is event listner
            Singleton<PlayerStatus>.getInstance().Action_UtilStatChange += this.onUtilStatChange;
            Singleton<ElementalManager>.getInstance().Action_ElementalCreateEvent += SummonElemetalLevelUpItem;
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += SummonHeroLevelUpItem;

            TabButton1 = GameObject.Find("TabButton1");
            TabButton1.GetComponent<TabView>().Action_ShowList += onShowList;
            TabButton1.GetComponent<TabView>().Action_HideList += onHideList;

            Tab1Container = GameObject.Find("Tab1Container");
            Tab1Container.transform.DestroyChildren();

            //LevelUpWindow Panel = LevelUpWindow.getInstance;
            GameObject panelObject = GameObject.Find("Bottom_Window").transform.FindChild("LevelUpWindow").gameObject;
            panelObject.SetActive(true);
            Panel = panelObject.GetComponent<LevelUpWindow>();
        }
        public void DrawNewMark()
        {
            TabButton1.transform.FindChild("New").gameObject.SetActive(true);
        }
        private void onShowList()
        {
            Tab1Container.SetActive(true);
            Tab1Container.GetComponentInParent<UIScrollView>().ResetPosition();
        }
        private void onHideList()
        {
            Tab1Container.SetActive(false);
        }
        public void onUtilStatChange(UtilStat newStat)
        {
            RefreshStat = true;
        }

        internal void Update()
        {
            if (RefreshStat)
            {
                foreach (LevelUp item in levelUpItems.Values)
                    item.SetStat();
                RefreshStat = false;
            }

            foreach (LevelUp item in levelUpItems.Values)
                item.Update();
            Tab1Container.GetComponent<UIGrid>().Reposition();
        }

        void SummonHeroLevelUpItem(Hero hero) // create and bind heroitem
        {
            if (Singleton<TableLoader>.getInstance().leveluptable.ContainsKey(hero.GetID) == false)
                return;

            LevelUp levelup = new HeroLevelUp(Singleton<TableLoader>.getInstance().leveluptable[hero.GetID], hero);
            levelup.SetStat();
            levelUpItems.Add(hero.GetID, levelup);
            DrawNewMark();
        }

        void SummonElemetalLevelUpItem(Elemental elemental) // create and bind element item
        {
            if (Singleton<TableLoader>.getInstance().leveluptable.ContainsKey(elemental.GetID) == false)
                return;

            LevelUp levelup = new ElementalLevelUp(Singleton<TableLoader>.getInstance().leveluptable[elemental.GetID], elemental);
            levelup.SetStat();
            levelUpItems.Add(elemental.GetID, levelup);
            DrawNewMark();
            return;
        }

        ///  for test
        internal void BuySelected()
        {
            foreach (LevelUp item in levelUpItems.Values)
            {
                if (item.Selected)
                    item.onConfirm();
            }
        }

    }
}