using System;
using System.Collections;
using System.Collections.Generic;
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

        public List<ConsumableChest> Chests = new List<ConsumableChest>();
        public Dictionary<string, Consumable> inventory = new Dictionary<string, Consumable>();

        public GameObject TabButton3;
        public GameObject Tab3Container;
        
        ConsumableWindow window;

        internal void Init()
        {
            TabButton3 = GameObject.Find("TabButton3");
            TabButton3.GetComponent<TabView>().Action_ShowList += onShowList;
            TabButton3.GetComponent<TabView>().Action_HideList += onHideList;

            Tab3Container = GameObject.Find("Tab3Container");
            Tab3Container.transform.DestroyChildren();
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

        public void MakeChest(int ChestCount, int level, Vector3 monsterPosition)
        {
            for(int i = 0; i < ChestCount; i ++ )
            {
                Consumable item = ConsumableFactory.PickRandomConsumable(level);
                Chests.Add(new ConsumableChest(item, monsterPosition));
            }
        }
        public void Update()
        {
            Tab3Container.GetComponent<UIGrid>().Reposition();
        }
        internal void Stack(Consumable consumable)
        {
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

        internal void Confirm()
        {
        }
    }

}
