﻿using System;
using System.Collections.Generic;
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

        internal void Init()
        {
            // this class is event listner
        }

        private bool SummonNewElemental(string id) // Create == 생성만, Summon ==  스텟과 레벨업설정까지
        {
            ElementalData data = Singleton<TableLoader>.getInstance().elementaltable[id];
            Elemental newElemental = new Elemental(data);
            Action_ElementalCreateEvent(newElemental);

            elementals.Add(id, newElemental);

            ElementalStat stat = Singleton<StatManager>.getInstance().GetElementalStat(id);
            SetLevelAndStat(stat, id);
            return true;
        }

        internal void Load(ElementalManager elementalManager) // Create
        {
            foreach(string saveEID in elementalManager.elementals.Keys)
            {
                ElementalData data = Singleton<TableLoader>.getInstance().elementaltable[saveEID];
                Elemental newElemental = new Elemental(data);
                Action_ElementalCreateEvent(newElemental);
                elementals.Add(saveEID, newElemental);

                // 변수 load 
                newElemental.skillForceCount = elementalManager.elementals[saveEID].skillForceCount;
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
                SummonNewElemental(readyToCreate.Dequeue());
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

        internal void ActiveSkill(ElementalStat stat, string ElementalID)
        {
            SetStat(stat, ElementalID, false, false);
            elementals[ElementalID].skillForceCount++;
        }

        internal void SetLevelAndStat(Dictionary<string, ElementalStat> statList)
        {
            foreach (string id in elementals.Keys)
                SetLevelAndStat(statList[id], id);
        }
        internal void SetLevelAndStat(ElementalStat stat, string ElementalID)
        {
            SetStat(stat, ElementalID, true, true);
            Action_ElementalLevelUp(elementals[ElementalID]);
        }
        internal void SetStatAll(Dictionary<string, ElementalStat> statList, bool CalDps, bool CalPrice)
        {
            foreach (string id in elementals.Keys)
                elementals[id].SetStat(statList[id], CalDps, CalPrice);
        }
        internal void SetStat(ElementalStat stat, string ElementalID, bool CalDps, bool CalPrice)
        {
            if (elementals.ContainsKey(ElementalID))
                elementals[ElementalID].SetStat(stat, CalDps, CalPrice);
        }


    }
}