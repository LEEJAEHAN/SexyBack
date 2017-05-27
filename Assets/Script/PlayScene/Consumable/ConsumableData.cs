using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class ConsumableData
    {
        internal string id;
        internal string name;
        internal Consumable.Type type;
        internal NestedIcon icon;
        internal int value;
        internal string strValue;
        internal int stackPerChest;
        internal int dropLevel;
        internal string description;
        internal int AbsRate;
        internal int Density;
        internal double CoolTime;

        public ConsumableData()
        {
        }
    }
}
