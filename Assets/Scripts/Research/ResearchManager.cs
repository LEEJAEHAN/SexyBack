using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SexyBackPlayScene
{
    class ResearchManager
    {
        public Dictionary<string, Research> researches = new Dictionary<string, Research>();
        public List<Research> beToDispose = new List<Research>();

        public void Init()
        {
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += onHeroCreate;
            Singleton<ElementalManager>.getInstance().Action_ElementalCreateEvent += onElementalCreate;

            ViewLoader.TabButton3.GetComponent<TabView>().Action_ShowList += onShowList;
            ViewLoader.TabButton3.GetComponent<TabView>().Action_HideList += onHideList;
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
            CreateResearch(elemental);
        }

        private void CreateResearch(ICanLevelUp root)
        {
            foreach (ResearchData item in Singleton<TableLoader>.getInstance().researchtable)
            {
                if (item.requireID == root.GetID)
                {
                    Research research = new Research(item, root);
                    researches.Add(item.ID, research);
                }
            }
        }  

        public void Update()
        {
            foreach (Research research in researches.Values)
            {
                research.Update();
            }

            foreach (Research research in beToDispose)
            {
                researches.Remove(research.ID);
                research.Dispose();
            }
            beToDispose.Clear(); // TODO : 이거 찜찜함
        }

        internal void Destroy(string iD)
        {
            beToDispose.Add(researches[iD]);
        }
    }
}
