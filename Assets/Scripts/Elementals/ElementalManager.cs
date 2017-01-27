using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    class ElementalManager
    {
        public List<Elemental> elementals = new List<Elemental>();
        public List<Projectile> projectiles = new List<Projectile>();

        public Dictionary<string, ElementalData> elementalDatas = new Dictionary<string, ElementalData>();

        Transform ElementalArea = ViewLoader.area_elemental.transform;


        public delegate void ElementalList_ChangeEvent(List<Elemental> list);
        public event ElementalList_ChangeEvent Action_ElementalListChangeEvent;// = delegate (object sender) { };

        public delegate void ElementalCreateEvent_Handler(Elemental sender);
        public event ElementalCreateEvent_Handler Action_ElementalCreateEvent;// = delegate (object sender) { };

        internal void Init()
        {
            // init data
            LoadData();

            // this class is event listner
            Singleton<MonsterManager>.getInstance().Action_NewFousEvent += this.SetTarget;
        }
        private void LoadData()
        {
            ElementalData data1 = new ElementalData("fireball", "Fire Ball", 5200, 1, 100);
            ElementalData data2 = new ElementalData("waterball", "Water Ball", 5300, 5, 500);
            ElementalData data3 = new ElementalData("rock", "Rock", 5500, 25, 4000);
            ElementalData data4 = new ElementalData("electricball", "Plasma", 5700, 150, 37500);
            ElementalData data5 = new ElementalData("snowball", "Snow Ball", 6100, 1150, 450000);
            ElementalData data6 = new ElementalData("earthball", "Mud ball", 6300, 10000, 7250000);
            ElementalData data7 = new ElementalData("airball", "Wind Strike", 6700, 100000, 150000000);
            ElementalData data8 = new ElementalData("iceblock", "Ice Cube", 6900, 1500000, 4000000000);
            ElementalData data9 = new ElementalData("magmaball", "Meteor", 7300, 27500000, 140000000000);

            elementalDatas.Add(data1.ID, data1);
            elementalDatas.Add(data2.ID, data2);
            elementalDatas.Add(data3.ID, data3);
            elementalDatas.Add(data4.ID, data4);
            elementalDatas.Add(data5.ID, data5);
            elementalDatas.Add(data6.ID, data6);
            elementalDatas.Add(data7.ID, data7);
            elementalDatas.Add(data8.ID, data8);
            elementalDatas.Add(data9.ID, data9);
        }

        public void Start()
        {
            elementals.Add(CreateElemental(elementalDatas["fireball"]));
            elementals.Add(CreateElemental(elementalDatas["waterball"]));
            elementals.Add(CreateElemental(elementalDatas["rock"]));
            elementals.Add(CreateElemental(elementalDatas["electricball"]));
            elementals.Add(CreateElemental(elementalDatas["snowball"]));
            elementals.Add(CreateElemental(elementalDatas["earthball"]));
            elementals.Add(CreateElemental(elementalDatas["airball"]));
            elementals.Add(CreateElemental(elementalDatas["iceblock"]));
            elementals.Add(CreateElemental(elementalDatas["magmaball"]));
            Action_ElementalListChangeEvent(elementals);
        }
        internal Elemental CreateElemental(ElementalData data)
        {
            Elemental temp = new Elemental(data, ElementalArea);
            temp.Action_ElementalChange += this.onElementalChange;
            Action_ElementalCreateEvent(temp); // send event

            temp.LevelUp(1);
            return temp;
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