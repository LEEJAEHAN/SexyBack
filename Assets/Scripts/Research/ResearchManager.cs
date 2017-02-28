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

        public int resarchthread = 0;
        public int maxthread = Singleton<StatManager>.getInstance().GetPlayerStat.ResearchThread;
        public bool CanUseThread { get { return resarchthread < maxthread; } }

        BigInteger minusExp = new BigInteger(0);
        public delegate void ResarchThreadChange_Event(bool available);
        public event ResarchThreadChange_Event Action_ThreadChange = delegate { };

        public Dictionary<string, Research> researches = new Dictionary<string, Research>();
        public List<Research> beToDispose = new List<Research>();

        public void Init()
        {
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += onHeroCreate;
            Singleton<ElementalManager>.getInstance().Action_ElementalCreateEvent += onElementalCreate;

            ViewLoader.TabButton2.GetComponent<TabView>().Action_ShowList += onShowList;
            ViewLoader.TabButton2.GetComponent<TabView>().Action_HideList += onHideList;

            ViewLoader.Tab2Container.GetComponent<UIGrid>().onCustomSort = myResearchSort;
        }

        int myResearchSort(Transform a, Transform b)
        {
            return researches[a.gameObject.name].SortOrder -  researches[b.gameObject.name].SortOrder;
        }

        public void DrawNewMark()
        {
            ViewLoader.TabButton2.transform.FindChild("New").gameObject.SetActive(true);
        }

        private void onHideList()
        {
            ViewLoader.Tab2Container.SetActive(false);
            // ViewLoader.Info_Context.SetActive(false);
        }

        private void onShowList()
        {
            ViewLoader.Tab2Container.SetActive(true);
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
            minusExp = 0;
            foreach (Research research in researches.Values)
            {
                research.Update();
                if (research.CurrentState == "Work")
                    minusExp += research.PricePerSec;
                Singleton<GameInfoView>.getInstance().PrintMinusDps(minusExp);
            }

            foreach (Research research in beToDispose)
            {
                researches.Remove(research.GetID);
                research.Dispose();
            }
            beToDispose.Clear(); // TODO : 이거 찜찜함
            ViewLoader.Tab2Container.GetComponent<UIGrid>().Reposition();
        }

        internal void SetThread(PlayerStat playerStat)
        {
            maxthread = playerStat.ResearchThread;
            Action_ThreadChange(CanUseThread);
        }
        public void UseThread(bool start)
        {
            if (start)
            {
                if (CanUseThread)
                    resarchthread++;
            }
            else
            {
                if (resarchthread > 0)
                    resarchthread--;
            }
            Action_ThreadChange(CanUseThread);
        }

        internal void SetStat(PlayerStat researchStat)
        {
            foreach (Research research in researches.Values)
            {
                research.SetStat(researchStat);
            }
        }
        internal void Destroy(string iD)
        {
            beToDispose.Add(researches[iD]);
        }
    }
}
