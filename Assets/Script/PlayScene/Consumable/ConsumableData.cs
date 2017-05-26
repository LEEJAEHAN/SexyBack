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

        public ConsumableData(int num = 1)
        {
            if (num == 1)
            { // 공격스택지급
                id = "C01";
                name = "검술팩";
                type = Consumable.Type.AttackCount;
                icon = new NestedIcon("Icon_11", null, null);
                stackPerChest = 1;
                value = 5;
                strValue = null;
                dropLevel = 0;
                description = "검술의 공격횟수를 5회 즉시 충전합니다. (쿨타임 8초)";
                AbsRate = 0;
                Density = 1;
                CoolTime = 8f;
            }
            else if (num == 2)
            {
                id = "C02";
                name = "스크롤 : 불덩이작렬";
                type = Consumable.Type.Skill;
                icon = new NestedIcon("Icon_20", null, null);
                stackPerChest = 7;
                value = 0;
                strValue = "fireball";
                dropLevel = 0;
                description = "불덩이 작렬을 즉시 시전합니다. (쿨타임 7초)\n화염구 연구완료 후에 사용할 수 있습니다. ";
                AbsRate = 0;
                Density = 1;
                CoolTime = 7f;
            }
            else if (num == 3)
            {
                id = "C03";
                name = "검술 증강물약";
                type = Consumable.Type.HeroBuff;
                icon = new NestedIcon("Icon_14", null, "Icon_11");
                stackPerChest = 1;
                value = 2;
                strValue = null;
                dropLevel = 0;
                description = "30초동안 검술의 공격력이 3배 상승합니다.";
                AbsRate = 0;
                Density = 1;
                CoolTime = 30f;
            }
            else if (num == 4)
            {
                id = "C04";
                name = "화염구 증강물약";
                type = Consumable.Type.ElementalBuff;
                icon = new NestedIcon("Icon_15", null, "Icon_01");
                stackPerChest = 1;
                value = 2;
                strValue = "fireball";
                dropLevel = 0;
                description = "30초동안 화염구의 공격력이 3배 상승합니다.\n화염구 연구완료 후에 사용할 수 있습니다.";
                AbsRate = 0;
                Density = 1;
                CoolTime = 30f;
            }
            else if (num == 5)
            {
                id = "C05";
                name = "경험치 증강물약";
                type = Consumable.Type.ExpBuff;
                icon = new NestedIcon("Icon_17", null, "Icon_18");
                stackPerChest = 1;
                value = 2;
                strValue = null;
                dropLevel = 0;
                description = "30초동안 경험치 획득량이 2배 상승합니다.";
                AbsRate = 0;
                Density = 1;
                CoolTime = 20f;
            }
            else if (num == 6)
            {
                id = "C06";
                name = "가속 시계";
                type = Consumable.Type.ResearchTime;
                icon = new NestedIcon("Icon_35", null, null);
                stackPerChest = 1;
                value = 10;
                strValue = null;
                dropLevel = 0;
                description = "현재 연구중인 가장 짧은 연구의 시간을 10초 단축합니다.\n(시간당 비용은 비싸지지 않습니다.)";
                AbsRate = 0;
                Density = 1;
                CoolTime = 1f;
            }
            else if (num == 7)
            {
                id = "C07";
                name = "검술 보너스";
                type = Consumable.Type.HeroLevelUp;
                icon = new NestedIcon("Icon_11", null, "IconSmall_01");
                stackPerChest = 1;
                value = 1;
                strValue = null;
                dropLevel = 0;
                description = "검술 레벨이 1 증가합니다.";
                AbsRate = 0;
                Density = 1;
                CoolTime = 1f;
            }
            else if (num == 8)
            {
                id = "C08";
                name = "화염구 보너스";
                type = Consumable.Type.ElementalLevelUp;
                icon = new NestedIcon("Icon_01", null, "IconSmall_01");
                stackPerChest = 1;
                value = 1;
                strValue = "fireball";
                dropLevel = 0;
                description = "화염구 레벨이 1 증가합니다.\n화염구 연구완료 후에 사용할 수 있습니다.";
                AbsRate = 0;
                Density = 1;
                CoolTime = 1f;
            }
            else if (num == 9)
            {
                id = "C09";
                name = "경험치";
                type = Consumable.Type.Exp;
                icon = new NestedIcon("Icon_18", null, null);
                stackPerChest = 0;
                value = 30;
                strValue = null;
                dropLevel = 0;
                description = null;
                AbsRate = 49;
                Density = 1;
                CoolTime = 1f;
            }
            else if (num == 10)
            {
                id = "C10";
                name = "보석";
                type = Consumable.Type.Gem;
                icon = new NestedIcon("Icon_34", null, null);
                stackPerChest = 0;
                value = 1;
                strValue = null;
                dropLevel = 0;
                description = null;
                AbsRate = 1;
                Density = 1;
                CoolTime = 1f;
            }
        }
    }
}
