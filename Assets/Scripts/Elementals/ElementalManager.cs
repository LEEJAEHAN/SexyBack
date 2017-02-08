using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    class ElementalManager
    {
        public Dictionary<string, Elemental> elementals = new Dictionary<string, Elemental>();
        public List<Projectile> projectiles = new List<Projectile>();

        Transform ElementalArea = ViewLoader.area_elemental.transform;

        public delegate void ElementalCreateEvent_Handler(Elemental sender);
        public event ElementalCreateEvent_Handler Action_ElementalCreateEvent;// = delegate (object sender) { };

        internal void Init()
        {
            // this class is event listner
            Singleton<MonsterManager>.getInstance().Action_BeginBattleEvent += this.SetTarget;
        }
        public bool SummonNewElemental(string id)
        {
            if (Singleton<TableLoader>.getInstance().elementaltable.ContainsKey(id) == false)
                return false;
            if (elementals.ContainsKey(id)) // 이미있는경우   
                return false;

            ElementalData data = Singleton<TableLoader>.getInstance().elementaltable[id];
            Elemental temp = new Elemental(data, ElementalArea);
            

            Action_ElementalCreateEvent(temp);
            elementals.Add(temp.GetID, temp);

            temp.LevelUp(1);
            return true;
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
        }


        internal BigInteger GetElementalDamage(string id)
        {
            return elementals[id].DAMAGE;
        }

        // recieve event
        void SetTarget(Monster sender)
        {
            if (elementals == null)
                return;

            foreach(Elemental elemental in elementals.Values)
            {
                elemental.targetID = null;
                sender.StateMachine.Action_changeEvent += elemental.onTargetStateChange;
            }
        }

        internal Elemental GetElemental(string ElementalID)
        {
            return elementals[ElementalID];
        }


        internal bool Upgrade(Bonus b)
        {
            return elementals[b.targetID].Upgrade(b);
        }
    }
}