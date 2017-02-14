using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SexyBackPlayScene
{
    class ResearchManager
    {
        ResearchFactory factory = new ResearchFactory();

        public Dictionary<string, Research> researches = new Dictionary<string, Research>();
        public List<Research> beToDispose = new List<Research>();

        public void Init()
        {
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += onHeroCreate;
            Singleton<ElementalManager>.getInstance().Action_ElementalCreateEvent += onElementalCreate;

            ViewLoader.TabButton3.GetComponent<TabView>().Action_ShowList += onShowList;
            ViewLoader.TabButton3.GetComponent<TabView>().Action_HideList += onHideList;

            ViewLoader.Tab3Container.GetComponent<UIGrid>().onCustomSort = myResearchSort;
        }

        int myResearchSort(Transform a, Transform b)
        {
            return researches[a.gameObject.name].SortOrder -  researches[b.gameObject.name].SortOrder;
        }

        private void onHideList()
        {
            ViewLoader.Tab3Container.SetActive(false);
            // ViewLoader.Info_Context.SetActive(false);
        }

        private void onShowList()
        {
            ViewLoader.Tab3Container.SetActive(true);
        }

        private void onHeroCreate(Hero hero)
        {
            CreateResearch(hero);
        }

        private void onElementalCreate(Elemental elemental)
        {
            CreateResearch(elemental); // TODO : 바로만들지말고 업데이트에서 만들어야한다.
        }

        private void CreateResearch(ICanLevelUp root)
        {
            foreach (ResearchData item in Singleton<TableLoader>.getInstance().researchtable)
            {
                if (item.requireID == root.GetID)
                {
                    Research research = factory.CreateNewResearch(item, root);
                    researches.Add(item.ID, research);
                }
            }
        }  

        public void Update()
        {
            try
            {
                foreach (Research research in researches.Values)
                {
                    research.Update();
                }
            }
            catch (InvalidOperationException e )
            {
                sexybacklog.Error(e.Message);
            }

            foreach (Research research in beToDispose)
            {
                researches.Remove(research.GetID);
                research.Dispose();
            }
            beToDispose.Clear(); // TODO : 이거 찜찜함
            ViewLoader.Tab3Container.GetComponent<UIGrid>().Reposition();
        }

        internal void Destroy(string iD)
        {
            beToDispose.Add(researches[iD]);
        }

        internal void ReduceTime(ResearchUpgradeStat researchStat)
        {
            foreach (Research research in researches.Values)
            {
                research.SetStat(researchStat);
            }
        }
    }
}
