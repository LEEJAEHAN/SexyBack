using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace SexyBackPlayScene 
{
    [Serializable]
    class ElementalManager : IDisposable
    {
        ~ElementalManager()
        {
            sexybacklog.Console("ElementalManager 소멸");
        }

        public void Dispose()
        {
            elementals = null;
            readyToCreate = null;
            Action_ElementalCreateEvent = null;// = delegate (object sender) { };
            Action_ElementalLevelUp = null;
            Singleton<PlayerStatus>.getInstance().Action_ElementalStatChange -= this.onElementalStatChange;
        }

        public Dictionary<string, Elemental> elementals = new Dictionary<string, Elemental>();
        [NonSerialized]
        public Queue<string> readyToCreate = new Queue<string>();
        public delegate void ElementalCreateEvent_Handler(Elemental sender);


        [field:NonSerialized]
        public event ElementalCreateEvent_Handler Action_ElementalCreateEvent;// = delegate (object sender) { };
        public delegate void ElementalLevelUp_Event(Elemental elemental);
        [field: NonSerialized]
        public event ElementalLevelUp_Event Action_ElementalLevelUp;

        private Elemental SummonNewElemental(string id) // Create == 생성만, Summon ==  스텟과 레벨업설정까지
        {
            ElementalData data = Singleton<TableLoader>.getInstance().elementaltable[id];
            Elemental newElemental = new Elemental(data);
            Action_ElementalCreateEvent(newElemental);
            elementals.Add(id, newElemental);
            ElementalStat stat = Singleton<PlayerStatus>.getInstance().GetElementalStat(id);
            SetStat(stat, id);
            return newElemental;
        }

        internal void Init()
        {
            Singleton<PlayerStatus>.getInstance().Action_ElementalStatChange += this.onElementalStatChange;
        }

        internal void Load(XmlDocument doc)
        {
            XmlNode eNodes = doc.SelectSingleNode("InstanceStatus/Elementals");
            foreach(XmlNode eNode in eNodes.ChildNodes)
            {
                string id = eNode.Attributes["id"].Value;
                Elemental newElemental= SummonNewElemental(id);
                LevelUp(id, int.Parse(eNode.Attributes["level"].Value));
                newElemental.skillForceCount = int.Parse(eNode.Attributes["skillforcecount"].Value);
                newElemental.skillActive = bool.Parse(eNode.Attributes["skillactive"].Value);
                ElementalData data = Singleton<TableLoader>.getInstance().elementaltable[id];
            }
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
            {
                SummonNewElemental(readyToCreate.Peek());
                LevelUp(readyToCreate.Dequeue(), 1);
            }
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

        internal void LearnNewElemental(string id)
        {
            if (Singleton<TableLoader>.getInstance().elementaltable.ContainsKey(id) == false)
                return;
            if (elementals.ContainsKey(id)) // 이미있는경우   
                return;
            readyToCreate.Enqueue(id);
        }

        internal void ActiveSkill(string ElementalID)
        {
            elementals[ElementalID].skillActive = true;
            elementals[ElementalID].skillForceCount++;
            // no post events
        }

        internal void LevelUp(string ownerID, int value)
        {
            elementals[ownerID].LevelUp(value);
            Action_ElementalLevelUp(elementals[ownerID]);
        }
        internal void SetStatAll(Dictionary<string, ElementalStat> statList)
        {
            foreach (string id in elementals.Keys)
                elementals[id].SetStat(statList[id]);
        }


        void onElementalStatChange(ElementalStat newStat, string elementalIndex)
        {
            SetStat(newStat, elementalIndex);
        }
        internal void SetStat(ElementalStat stat, string ElementalID)
        {
            if (elementals.ContainsKey(ElementalID))
                elementals[ElementalID].SetStat(stat);
        }


    }
}