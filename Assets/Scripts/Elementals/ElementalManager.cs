using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    class ElementalManager
    {
        public List<Elemental> elementals = new List<Elemental>();
        public List<Projectile> projectiles = new List<Projectile>();

        Transform ElementalArea = ViewLoader.area_elemental.transform;


        public delegate void ElementalList_ChangeEvent(List<Elemental> list);
        public event ElementalList_ChangeEvent Action_ElementalListChangeEvent;// = delegate (object sender) { };

        public delegate void ElementalCreateEvent_Handler(Elemental sender);
        public event ElementalCreateEvent_Handler Action_ElementalCreateEvent;// = delegate (object sender) { };

        internal void Init()
        {
            // this class is event listner
            Singleton<MonsterManager>.getInstance().Action_NewFousEvent += this.SetTarget;
        }
        public void SummonNewElemental(string id)
        {
            ElementalData data = Singleton<TableLoader>.getInstance().elementaltable[id];
            Elemental temp = new Elemental(data, ElementalArea);

            temp.Action_ElementalChange += this.onElementalChange;
            Action_ElementalCreateEvent(temp);

            temp.LevelUp(1); 

            elementals.Add(temp);
            Action_ElementalListChangeEvent(elementals);
        }

        private void onElementalChange(Elemental sender)
        {
            Action_ElementalListChangeEvent(elementals);
        }

        internal void Update()
        {
            foreach (Elemental elemenatal in elementals)
                elemenatal.Update();
        }

        internal void LevelUp(string ElementalID)
        {
            Elemental target = FindElemental(ElementalID);
            if (target == null)
                return;
            target.LevelUp(1);
        }

        Elemental FindElemental(string elementalid)
        {
            foreach (Elemental elemenetal in elementals)
            {
                if (elemenetal.ID == elementalid)
                    return elemenetal;
            }
            return null;
        }

        internal BigInteger GetElementalDamage(string id)
        {
            return FindElemental(id).DAMAGE;
        }

        // recieve event
        void SetTarget(Monster sender)
        {
            if (elementals == null)
                return;

            foreach(Elemental elemental in elementals)
            {
                elemental.targetID = null;
                sender.Action_StateChangeEvent = elemental.onTargetStateChange;
            }
        }
    }
}