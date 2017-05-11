using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Runtime.Serialization;
namespace SexyBackPlayScene
{
    [Serializable]
    class ResearchManager : IDisposable
    {
        ~ResearchManager()
        {
            sexybacklog.Console("ResearchManager 소멸");
        }
        public void Dispose()
        {
            Singleton<PlayerStatus>.getInstance().Action_UtilStatChange -= this.onUtilStatChange;
            ResearchWindow.Clear();
        }

        public int resarchthread = 0;
        public Dictionary<string, Research> researches = new Dictionary<string, Research>();
        public Dictionary<string, string> FinishList = new Dictionary<string, string>();

        [NonSerialized]
        public int maxthread;
        [NonSerialized]
        public Queue<string> idQueue = new Queue<string>();
        public delegate void ResarchThreadChange_Event(bool available);
        [field: NonSerialized]
        public event ResarchThreadChange_Event Action_ThreadChange = delegate { };
        [NonSerialized]
        BigInteger minusExp = new BigInteger(0);
        [NonSerialized]
        ResearchFactory factory = new ResearchFactory();

        public bool CanUseThread { get { return resarchthread < maxthread; } }

        public void Init()
        {
            Singleton<PlayerStatus>.getInstance().Action_UtilStatChange += this.onUtilStatChange;
            Singleton<HeroManager>.getInstance().Action_HeroLevelUp += onHeroLevelUp;
            Singleton<ElementalManager>.getInstance().Action_ElementalLevelUp += onElementalLevelUp;
            ViewLoader.TabButton2.GetComponent<TabView>().Action_ShowList += onShowList;
            ViewLoader.TabButton2.GetComponent<TabView>().Action_HideList += onHideList;
            ViewLoader.Tab2Container.GetComponent<UIGrid>().onCustomSort = myResearchSort;
        }

        public void onUtilStatChange(UtilStat newStat, string eventType)
        {
            switch (eventType)
            {
                case "FinishResearch":
                    FinishFrontOne();
                    break;
                case "ResearchTimeX":
                case "ResearchTime":
                case "ResearchThread":
                case "ResearchPriceXH":
                    SetStat(newStat);
                    break;
            }
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
            SummonResearch(hero.GetID, hero.LEVEL);
        }

        private void onElementalLevelUp(Elemental elemental)
        {
            SummonResearch(elemental.GetID, elemental.LEVEL); // TODO : 바로만들지말고 업데이트에서 만들어야한다.
        }

        private void SummonResearch(string id, int levelcondition)
        {
            foreach (ResearchData item in Singleton<TableLoader>.getInstance().researchtable.Values)
            {
                if (researches.ContainsKey(item.ID))
                    continue;
                if (FinishList.ContainsKey(item.ID))
                    continue;

                if (item.requireID == id && item.requeireLevel <= levelcondition)
                {
                    DrawNewMark();
                    Research research = factory.SummonNewResearch(item);
                    researches.Add(item.ID, research);
                }
            }
        }

        internal void Load(ResearchManager loadedData)
        {
            this.resarchthread = loadedData.resarchthread;
            this.FinishList = loadedData.FinishList;

            foreach (string id in loadedData.researches.Keys)
            {
                ResearchData baseData = Singleton<TableLoader>.getInstance().researchtable[id];
                Research research = factory.SummonNewResearch(baseData);
                researches.Add(id, research);
            }
        }
        internal void SetStateNTime(ResearchManager loadedData)
        {
            foreach (string id in loadedData.researches.Keys)
            {
                Research from = loadedData.researches[id];
                Research to = researches[id];
                to.StateMachine.ChangeState(from.SavedState);
                to.itemView.SetActive(true);
                to.RemainTime = from.RemainTime;
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


        internal void SetStat(UtilStat utilStat)
        {
            if(maxthread != utilStat.ResearchThread)
            {
                maxthread = utilStat.ResearchThread;
                Action_ThreadChange(CanUseThread);
            }
            foreach (Research research in researches.Values)
            {
                research.SetStat(utilStat);
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
            FinishList.Add(id, id);
        }

    }
}
