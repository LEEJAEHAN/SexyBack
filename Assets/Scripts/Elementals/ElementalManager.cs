using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    class ElementalManager
    {
        public Dictionary<string, Elemental> elementals = new Dictionary<string, Elemental>();
        public Queue<string> readyToCreate = new Queue<string>();
        public delegate void ElementalCreateEvent_Handler(Elemental sender);
        public event ElementalCreateEvent_Handler Action_ElementalCreateEvent;// = delegate (object sender) { };

        internal void Init()
        {
            // this class is event listner
        }
        private bool SummonNewElemental(string id)
        {
            ElementalData data = Singleton<TableLoader>.getInstance().elementaltable[id];
            ElementalStat stat = Singleton<StatManager>.getInstance().GetElementalStat(id);
            Elemental newElemental = new Elemental(data, stat, ViewLoader.area_elemental.transform);
            Action_ElementalCreateEvent(newElemental);
            elementals.Add(newElemental.GetID, newElemental);
            newElemental.LevelUp(1);
            return true;
        }

        internal void LearnNewElemental(string id)
        {
            if (Singleton<TableLoader>.getInstance().elementaltable.ContainsKey(id) == false)
                return;
            if (elementals.ContainsKey(id)) // 이미있는경우   
                return;

            readyToCreate.Enqueue(id);
        }

        public BigInteger GetTotalDps()
        {
            BigInteger result = new BigInteger(0);
            foreach (Elemental elemental in elementals.Values)
                result += elemental.DPS;
            return result;
        }

        internal void Update()
        {
            foreach (Elemental elemenatal in elementals.Values)
                elemenatal.Update();

            while (readyToCreate.Count > 0)
                SummonNewElemental(readyToCreate.Dequeue());
        }


        internal BigInteger GetElementalDamage(string id)
        {
            return elementals[id].DAMAGE;
        }

        // recieve event
        public void SetTarget(Monster sender)
        {
            if (elementals == null)
                return;

            foreach (Elemental elemental in elementals.Values)
            {
                elemental.targetID = null;
                sender.StateMachine.Action_changeEvent += elemental.onTargetStateChange;
            }
        }

        internal Elemental GetElemental(string ElementalID)
        {
            return elementals[ElementalID];
        }

        internal void SetStat(ElementalStat stat, string ElementalID)
        {
            if (elementals.ContainsKey(ElementalID))
                elementals[ElementalID].SetStat(stat);
        }
        internal void SetStatAll(Dictionary<string, ElementalStat> statList)
        {
            foreach (string id in elementals.Keys)
                elementals[id].SetStat(statList[id]);
        }
    }
}