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
        public Dictionary<string, Research> researches = new Dictionary<string, Research>();
        public Queue<string> idQueue = new Queue<string>();
        public HashSet<string> FinishList = new HashSet<string>();

        public delegate void ResarchThreadChange_Event(bool available);
        public event ResarchThreadChange_Event Action_ThreadChange = delegate { };
        
                
        public void Init()
        {
            Singleton<HeroManager>.getInstance().Action_HeroLevelUp += onHeroLevelUp;
            Singleton<ElementalManager>.getInstance().Action_ElementalLevelUp += onElementalLevelUp;

            ViewLoader.TabButton2.GetComponent<TabView>().Action_ShowList += onShowList;
            ViewLoader.TabButton2.GetComponent<TabView>().Action_HideList += onHideList;

            ViewLoader.Tab2Container.GetComponent<UIGrid>().onCustomSort = myResearchSort;
        }

        int myResearchSort(Transform a, Transform b)
        {
            return researches[a.gameObject.name].SortOrder - researches[b.gameObject.name].SortOrder;
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

        private void onHeroLevelUp(Hero hero)
        {
            CreateResearch(hero.GetID, hero.LEVEL);
        }

        private void onElementalLevelUp(Elemental elemental)
        {
            CreateResearch(elemental.GetID, elemental.LEVEL); // TODO : 바로만들지말고 업데이트에서 만들어야한다.
        }

        private void CreateResearch(string id, int levelcondition)
        {
            foreach (ResearchData item in Singleton<TableLoader>.getInstance().researchtable)
            {
                if (researches.ContainsKey(item.ID))
                    continue;
                if (FinishList.Contains(item.ID))
                    continue;

                if (item.requireID == id && item.requeireLevel <= levelcondition)
                {
                    Research research = factory.CreateNewResearch(item);
                    researches.Add(item.ID, research);
                }
            }
        }

        public void Update()
        {
            for(int i = 0; i < idQueue.Count; i++)
            {
                string targetid = idQueue.Dequeue();
                researches[targetid].Dispose();
                researches.Remove(targetid);
            }

            foreach (Research research in researches.Values)
            {
                research.Update();
                if (research.CurrentState == "Work")
                    minusExp += research.PricePerSec;
            }
            Singleton<GameInfoView>.getInstance().PrintMinusDps(minusExp);
            minusExp = 0;

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

        internal void FinishFrontOne()
        {
            int min = int.MaxValue;
            Research Front = null;
            foreach (Research research in researches.Values)
            {
                if (research.CurrentState == "Work")
                {
                    if (research.SortOrder < min)
                    {
                        min = research.SortOrder;
                        Front = research;
                    }
                }
            }
            if (Front != null)
                Front.Finish();
        }

        internal void Destory(string id)
        {
            idQueue.Enqueue(id);
            FinishList.Add(id);
        }
    }
}
