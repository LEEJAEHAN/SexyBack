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
        UILabel TotalDpsLabel = ViewLoader.label_elementaldmg.GetComponent<UILabel>();

        //this class is event publisher 
        public delegate void ElementalChangeEvent_Handler(Elemental sender);
        public event ElementalChangeEvent_Handler noticeElementalChange;// = delegate (object sender) { };

        public delegate void ElementalCreateEvent_Handler(Elemental sender);
        public event ElementalCreateEvent_Handler noticeElementalCreate;// = delegate (object sender) { };

        internal void Init()
        {
            // init data
            LoadData();

            // this class is event listner
            noticeElementalChange += PrintDps; // 내꺼내가받음;
            Singleton<MonsterManager>.getInstance().noticeMonsterCreate += this.onMonsterCreate;
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
            CreateElemental(elementalDatas["fireball"]);
            CreateElemental(elementalDatas["waterball"]);
            CreateElemental(elementalDatas["rock"]);
            CreateElemental(elementalDatas["electricball"]);
            CreateElemental(elementalDatas["snowball"]);
            CreateElemental(elementalDatas["earthball"]);
            CreateElemental(elementalDatas["airball"]);
            CreateElemental(elementalDatas["iceblock"]);
            CreateElemental(elementalDatas["magmaball"]);
        }

        internal void CreateElemental(ElementalData data)
        {
            Elemental temp = new Elemental(data, ElementalArea);
            elementals.Add(temp);


            noticeElementalCreate(temp); // send event
            noticeElementalChange(temp);
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
            noticeElementalChange(target);
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

        internal BigInteger GetTotalDPS()
        {
            BigInteger result = new BigInteger();
            foreach (Elemental elemental in elementals)
            {
                result += elemental.DPS;
            }
            return result;
        }
        internal BigInteger GetElementalDamage(string id)
        {
            return FindElemental(id).DAMAGE;
        }

        // recieve event
        void onMonsterCreate(Monster sender)
        {
            if (elementals == null)
                return;

            foreach(Elemental elemental in elementals)
            {
                elemental.target = sender;
            }
        }
        public void PrintDps(Elemental sender)
        {
            string dpsString = GetTotalDPS().To5String() + " /Sec";
            TotalDpsLabel.GetComponent<UILabel>().text = dpsString;
        }
    }
}