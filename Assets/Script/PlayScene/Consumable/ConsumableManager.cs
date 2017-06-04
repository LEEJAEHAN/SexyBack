using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class ConsumableManager : IDisposable
    {
        ~ConsumableManager()
        {
            sexybacklog.Console("ConsumableManager 소멸");
        }
        public void Dispose()
        {
        }

        //ConsumableWindow window;
        public Queue<string> idQueue = new Queue<string>();
        public List<ConsumableChest> Chests = new List<ConsumableChest>();
        public Dictionary<string, Consumable> inventory = new Dictionary<string, Consumable>();

        public GameObject TabButton3;
        public GameObject Tab3Container;
        public ConsumableWindow Panel;

        internal void Init()
        {
            TabButton3 = GameObject.Find("TabButton3");
            TabButton3.GetComponent<TabView>().Action_ShowList += onShowList;
            TabButton3.GetComponent<TabView>().Action_HideList += onHideList;

            Tab3Container = GameObject.Find("Tab3Container");
            Tab3Container.transform.DestroyChildren();

            GameObject panelObject = GameObject.Find("Bottom_Window").transform.FindChild("ConsumableWindow").gameObject;
            panelObject.SetActive(true);
            Panel = panelObject.GetComponent<ConsumableWindow>();
        }

        internal void Load(XmlDocument doc)
        {
            XmlNode rootNode = doc.SelectSingleNode("InstanceStatus/Consumables");
            {
                foreach (XmlNode rNode in rootNode.ChildNodes)
                {
                    string id = rNode.Attributes["id"].Value;
                    ConsumableData baseData = Singleton<TableLoader>.getInstance().consumable[id];
                    int stack = int.Parse(rNode.Attributes["stack"].Value);
                    Consumable loadOne = new Consumable(baseData, stack);
                    loadOne.LoadUseState(double.Parse(rNode.Attributes["remaintime"].Value));
                    loadOne.ActiveView();
                    inventory.Add(id, loadOne);
                }
            }
        }

        public void DrawNewMark()
        {
            TabButton3.transform.FindChild("New").gameObject.SetActive(true);
        }
        private void onShowList()
        {
            Tab3Container.SetActive(true);
            Tab3Container.GetComponentInParent<UIScrollView>().ResetPosition();
        }
        private void onHideList()
        {
            Tab3Container.SetActive(false);
        }

        internal void MakeInitialChest()
        {
            Consumable item = ConsumableFactory.PickRandomConsumable(0);
            Chests.Add(new ConsumableChest(item, new Vector3(-1, 1.5f, 0), false, 0));
            Consumable item2 = ConsumableFactory.PickRandomConsumable(0);
            Chests.Add(new ConsumableChest(item2, new Vector3(0, 1.5f, 0), false, 0));
            Consumable item3 = ConsumableFactory.PickRandomConsumable(0);
            Chests.Add(new ConsumableChest(item3, new Vector3(1, 1.5f, 0), false, 0));
        }

        internal static string MakeConstrantText(string target)
        {
            if (Singleton<TableLoader>.getInstance().elementaltable.ContainsKey(target))
                return Singleton<TableLoader>.getInstance().elementaltable[target].Name + " 소환 후에 사용할 수 있습니다.";
            else
                return "";
        }

        public void MakeChest(int ChestCount, int level, Vector3 monsterPosition)
        {
            for(int i = 0; i < ChestCount; i ++ )
            {
                Consumable item = ConsumableFactory.PickRandomConsumable(level);
                Chests.Add(new ConsumableChest(item, monsterPosition, true, level));
            }
        }

        internal void Destory(string id)
        {
            idQueue.Enqueue(id);
        }

        public void Update()
        {
            Tab3Container.GetComponent<UIGrid>().Reposition();
            foreach(var consumable in inventory.Values)
            {
                consumable.Update();
            }

            for (int i = 0; i < idQueue.Count; i++)
            {
                string targetid = idQueue.Dequeue();
                inventory[targetid].Dispose();
                inventory.Remove(targetid);
            }
        }
        internal void Stack(Consumable consumable)
        {
            DrawNewMark();

            if (inventory.ContainsKey(consumable.GetID))
            {
                inventory[consumable.GetID].Merge(consumable);
            }
            else
            {
                inventory.Add(consumable.GetID, consumable);
                inventory[consumable.GetID].ActiveView();
            }
        }


        internal void DestroyChest(ConsumableChest chest)
        {
            Chests.Remove(chest);
        }

        public static BigInteger CalChestExp(int level, int Coef)
        {
            double exp = InstanceStatus.CalGrowthPower(MonsterData.GrowthRate, level) * Coef; // 
            return BigInteger.FromDouble(exp);
        }
        public static bool Buff(bool on, Consumable.Type bufftype, string target, int value)
        {
            switch (bufftype)
            {
                case Consumable.Type.HeroBuff:
                    Singleton<HeroManager>.getInstance().GetHero().Buff(on, value);
                    return true;
                case Consumable.Type.ElementalBuff:
                    if (Singleton<ElementalManager>.getInstance().elementals.ContainsKey(target))
                    {
                        Singleton<ElementalManager>.getInstance().elementals[target].Buff(on, value);
                        return true;
                    }
                    break;
                case Consumable.Type.ExpBuff:
                    Singleton<InstanceStatus>.getInstance().BuffExp(on, value);
                    return true;
            }
            return false;
        }


    }

}
