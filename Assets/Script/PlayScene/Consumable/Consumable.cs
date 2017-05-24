using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class Consumable : IDisposable     // stack 항목.
    {
        internal enum Type
        {
            AttackCount,        // int value
            Skill,              // string ID
            Buff,               // buff id or 버프능력세부
            ResearchTime,       // int value
            LevelUp,            // int value
            Exp                 // 아예 획득시부터 루틴이다름.
        }

        public string GetID { get { return baseData.id; } }
        public readonly ConsumableData baseData;
        public int Stack;
        bool Confirm = false;

        // GridView Class;

        public Consumable(ConsumableData data, int Stack = 0)
        {
            baseData = data;
            this.Stack = Stack;
            if (Stack == 0)
                Stack = baseData.stackPerChest;
        }
        
        public string Description
        {
            get
            {
                //return StringParser.ReplaceString(description, bonus.strvalue);
                //return StringParser.ReplaceString(description, bonus.value.ToString());
                return null;
            }
        }

        public void Update()
        {   // state machine
            if(Confirm)
            {
                Apply();
            }
        }
        internal void Use()
        {
            Stack--;
            Confirm = true;
        }

        public void Apply()
        {
            //Singleton<InstanceStatus>.getInstance().ApplyBonusWithIcon(bonus, Icon);
            Confirm = false;
        }

        // function
        public void Dispose()
        {

        }
        // update view state
    }
}