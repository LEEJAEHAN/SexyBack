using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class LevelUpManager : IDisposable
    {
        ~LevelUpManager()
        {
            sexybacklog.Console("LevelUpManager 소멸");
        }
        public void Dispose()
        {
            Singleton<PlayerStatus>.getInstance().Action_UtilStatChange -= this.onUtilStatChange;
            LevelUpWindow.Clear();
            Panel = null;
        }
        LevelUpWindow Panel;
        Dictionary<string, LevelUp> levelUpItems = new Dictionary<string, LevelUp>();

        internal void Init()
        {
            Panel = LevelUpWindow.getInstance;
            Singleton<PlayerStatus>.getInstance().Action_UtilStatChange += this.onUtilStatChange;
            // this class is event listner
            Singleton<ElementalManager>.getInstance().Action_ElementalCreateEvent += SummonElemetalLevelUpItem;
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += SummonHeroLevelUpItem;
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
        public void onUtilStatChange(UtilStat newStat, string eventType)
        {
            switch (eventType)
            {
                case "LevelUpPriceXH":
                    SetStat(newStat);
                    break;
            }
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

        void SummonHeroLevelUpItem(Hero hero) // create and bind heroitem
        {
            if (Singleton<TableLoader>.getInstance().leveluptable.ContainsKey(hero.GetID) == false)
                return;

            LevelUp levelup = new HeroLevelUp(Singleton<TableLoader>.getInstance().leveluptable[hero.GetID], hero);
            levelup.SetStat(Singleton<PlayerStatus>.getInstance().GetUtilStat);
            levelUpItems.Add(hero.GetID, levelup);
            DrawNewMark();
        }

        void SummonElemetalLevelUpItem(Elemental elemental) // create and bind element item
        {
            if (Singleton<TableLoader>.getInstance().leveluptable.ContainsKey(elemental.GetID) == false)
                return;

            LevelUp levelup = new ElementalLevelUp(Singleton<TableLoader>.getInstance().leveluptable[elemental.GetID], elemental);
            levelup.SetStat(Singleton<PlayerStatus>.getInstance().GetUtilStat);
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

        internal void SetStat(UtilStat stat)
        {
            foreach (LevelUp item in levelUpItems.Values)
            {
                if (item.Selected)
                    item.SetStat(stat);
            }
        }

    }
}