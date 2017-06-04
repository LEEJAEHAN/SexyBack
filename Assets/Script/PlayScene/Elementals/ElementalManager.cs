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
            Action_LevelUp = null;
            Singleton<PlayerStatus>.getInstance().Action_ElementalStatChange -= this.onElementalStatChange;
            Singleton<PlayerStatus>.getInstance().Action_BaseStatChange -= onBaseStatChange;

        }

        public Dictionary<string, Elemental> elementals = new Dictionary<string, Elemental>();
        [NonSerialized]
        public Queue<string> readyToCreate = new Queue<string>();


        public delegate void ElementalCreateEvent_Handler(Elemental sender);

        [field:NonSerialized]
        public event ElementalCreateEvent_Handler Action_ElementalCreateEvent;// = delegate (object sender) { };
        public delegate void LevelUp_Event(ICanLevelUp elemental);
        [field: NonSerialized]
        public event LevelUp_Event Action_LevelUp;

        private Elemental SummonNewElemental(string id) // Create == 생성만, Summon ==  스텟과 레벨업설정까지
        {
            ElementalData data = Singleton<TableLoader>.getInstance().elementaltable[id];
            Elemental newElemental = new Elemental(data);
            Action_ElementalCreateEvent(newElemental);
            elementals.Add(id, newElemental);
            return newElemental;
        }

        internal void Init()
        {
            Singleton<PlayerStatus>.getInstance().Action_ElementalStatChange += this.onElementalStatChange;
            Singleton<PlayerStatus>.getInstance().Action_BaseStatChange += onBaseStatChange;
        }

        internal void Load(XmlDocument doc)
        {
            XmlNode eNodes = doc.SelectSingleNode("InstanceStatus/Elementals");
            foreach(XmlNode eNode in eNodes.ChildNodes)
            {
                string id = eNode.Attributes["id"].Value;
                Elemental newElemental = SummonNewElemental(id);
                newElemental.LevelUp(int.Parse(eNode.Attributes["level"].Value)); // no send event
                newElemental.SkillActive = bool.Parse(eNode.Attributes["skillactive"].Value);
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
                string eID = readyToCreate.Dequeue();
                SummonNewElemental(eID);
                LevelUp(eID, Singleton<PlayerStatus>.getInstance().GetElementalStat(eID).InitLevel);
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
            elementals[ElementalID].SkillActive = true;
            elementals[ElementalID].skill.Start(true);
        }

        internal bool LevelUp(string ownerID, int value)
        {
            if (elementals.ContainsKey(ownerID))
            {
                elementals[ownerID].LevelUp(value);
                Action_LevelUp(elementals[ownerID]);
                return true;
            }
            return false;
        }
        public void onBaseStatChange(BaseStat newStat)
        {
            foreach(Elemental elemental in elementals.Values)
            {
                elemental.onStatChange();
            }
        }
        void onElementalStatChange(ElementalStat newStat, string elementalIndex)
        {
            if (elementals.ContainsKey(elementalIndex))
                elementals[elementalIndex].onStatChange();
        }
        public int TotalLevel { get
            {
                int sum = 0;
                foreach (Elemental e in elementals.Values)
                    sum += e.OriginalLevel;
                return sum;
            }
        }


    }
}