using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Runtime.Serialization;
using System.Xml;

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

        public int currentthread = 0;
        public int maxthread;
        public Dictionary<string, Research> researches = new Dictionary<string, Research>();
        public Dictionary<string, string> FinishList = new Dictionary<string, string>();

        [NonSerialized]
        public Queue<string> idQueue = new Queue<string>();
        public delegate void ResarchThreadChange_Event(bool available);
        [field: NonSerialized]
        public event ResarchThreadChange_Event Action_ThreadChange = delegate { };
        [NonSerialized]
        BigInteger minusExp = new BigInteger(0);
        [NonSerialized]
        ResearchFactory factory = new ResearchFactory();

        public bool CanUseThread { get { return currentthread < maxthread; } }

        public void Init()
        {
            Singleton<PlayerStatus>.getInstance().Action_UtilStatChange += this.onUtilStatChange;
            Singleton<HeroManager>.getInstance().Action_HeroLevelUp += onHeroLevelUp;
            Singleton<ElementalManager>.getInstance().Action_ElementalLevelUp += onElementalLevelUp;
            ViewLoader.TabButton2.GetComponent<TabView>().Action_ShowList += onShowList;
            ViewLoader.TabButton2.GetComponent<TabView>().Action_HideList += onHideList;
            ViewLoader.Tab2Container.GetComponent<UIGrid>().onCustomSort = myResearchSort;
        }
        public void Start()
        {
            currentthread = 0;
            maxthread = Singleton<PlayerStatus>.getInstance().GetUtilStat.MaxResearchThread;
        }

        public void onUtilStatChange(UtilStat newStat)
        {
            SetStatAll(newStat);
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

        internal void Load(XmlDocument doc)
        {
            XmlNode rootNode = doc.SelectSingleNode("InstanceStatus/Researchs");
            currentthread = int.Parse(rootNode.Attributes["currentthread"].Value);
            maxthread = Singleton<PlayerStatus>.getInstance().GetUtilStat.MaxResearchThread;
            {
                XmlNode FinishNode = rootNode.SelectSingleNode("FinishResearchs");
                foreach (XmlNode rNode in FinishNode.ChildNodes)
                {
                    string id = rNode.Attributes["id"].Value;
                    FinishList.Add(id, id);
                }
            }
            {
                XmlNode remainNode = rootNode.SelectSingleNode("RemainResearchs");
                foreach (XmlNode rNode in remainNode.ChildNodes)
                {
                    string id = rNode.Attributes["id"].Value;
                    ResearchData baseData = Singleton<TableLoader>.getInstance().researchtable[id];
                    Research research = factory.SummonNewResearch(baseData);
                    researches.Add(id, research);

                    research.StateMachine.ChangeState(rNode.Attributes["state"].Value);
                    research.itemView.SetActive(true);
                    research.RemainTime = double.Parse(rNode.Attributes["remaintime"].Value);
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


        public void UseThread(bool start)
        {
            if (start)
            {
                if (CanUseThread)
                    currentthread++;
            }
            else
            {
                if (currentthread > 0)
                    currentthread--;
            }
            Action_ThreadChange(CanUseThread);
        }
        

        internal void SetStatAll(UtilStat utilStat)
        {
            foreach (Research research in researches.Values)
                research.SetStat(utilStat);

            maxthread = utilStat.MaxResearchThread;
            Action_ThreadChange(CanUseThread);
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
