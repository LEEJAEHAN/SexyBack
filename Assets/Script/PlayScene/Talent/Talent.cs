using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    enum TalentType
    {
        Attack = 1,
        Elemental,
        Util
    }
    internal class Talent : IDisposable
    { // base class
        string ID;
        public string GetID { get { return ID; } }

        public int Count;

        // from data
        internal GridItemIcon Icon;
        string description;
        internal BonusStat bonus;
        internal TalentType Type;

        internal bool AbsRate;
        internal int Rate;

        bool isConfirm = false;

        public Talent(TalentData data)
        {
            ID = data.id;
            Icon = data.icon;
            description = data.description;
            bonus = data.bonus;
            Type = data.type;
            Rate = data.rate;
            AbsRate = data.absrate;
        }
        Talent() { sexybacklog.Console("특성소멸"); }

        internal void SetFloor(int floor)
        {
            if (bonus.attribute == "ExpPerFloor")
            {
//                bonus.bigvalue = BigInteger.PowerByGrowth(bonus.value, floor - 1, MonsterData.GrowthRate);
                Icon.IconText = bonus.strvalue;
            }
        }
        public string Description
        {
            get
            {
                if (bonus.attribute == "ExpPerFloor")
                {
                    return StringParser.ReplaceString(description, bonus.strvalue);
                }
                else
                {
                    return StringParser.ReplaceString(description, bonus.value.ToString());
                }
            }
        }

        public void Update()
        {   // state machine
            if(isConfirm)
            {
                DoUpgrade();
            }
        }
        internal void Confirm()
        {
            Count++;
            isConfirm = true;
        }

        public void DoUpgrade()
        {
            //Singleton<InstanceStatus>.getInstance().ApplyBonusWithIcon(bonus, Icon);
            isConfirm = false;
        }

        // function
        public void Dispose()
        {

        }
        // update view state
    }
}