﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    enum TalentType
    {
        Attack,
        Element,
        Util
    }
    internal class Talent : IDisposable
    { // base class
        string ID;
        public string GetID { get { return ID; } }
        public int Count;

        // from data
        internal GridItemIcon Icon;
        internal string Description;
        internal Bonus bonus;
        internal TalentType Type;
        internal int Rate;
        bool isConfirm = false;

        public Talent(TalentData data)
        {
            ID = data.id;
            Icon = data.icon;
            Description = data.description;
            bonus = data.bonus;
            Type = data.type;
            Rate = data.rate;
        }
        ~Talent() { sexybacklog.Console("특성소멸"); }

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
            Singleton<TalentManager>.getInstance().UpgradeTalentBonus(Type);
            Singleton<StatManager>.getInstance().Upgrade(bonus, Icon);
            isConfirm = false;
        }

        // function
        private string MakeDescriptionText(bool InstanceBuy)
        {
            return null;
        }

        public void Dispose()
        {

        }

        // update view state
    }
}