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

        public delegate void ConfirmTalent_Event();
        public event ConfirmTalent_Event Action_ConfirmTalent;

        internal void Init()
        {
        }

        public void MakeChest(int ChestCount, int level, Vector3 monsterPosition)
        {
            for(int i = 0; i < ChestCount; i ++ )
            {
                Consumable item = ConsumableFactory.PickRandomConsumable(level);
                Chests.Add(new ConsumableChest(item, monsterPosition));
            }
        }
        internal void Refresh()
        {
        }
        public void Update()
        {
        }

        internal void Confirm()
        {
        }
    }

}
