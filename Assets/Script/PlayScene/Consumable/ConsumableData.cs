using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class ConsumableData
    {
        internal string id;
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

        public ConsumableData(int num = 1)
        {
            if (num == 1)
            { // 공격스택지급
                id = "C01";
                type = Consumable.Type.AttackCount;
                icon = new NestedIcon("Icon_11", null, null);
                value = 5;
                stackPerChest = 6;
                strValue = null;
                description = "검술의 공격횟수를 5회 즉시 충전합니다.";
                AbsRate = 0;
                Density = 1;
                CoolTime = 1f;
            }
        }
    }
}
