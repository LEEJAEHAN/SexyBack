using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    enum ConsumableGroup
    {
        Diamond,
        Exp,
        YellowFever,
        RedFever,
        BlueFever,
        AttackStack,
        LevelUp,
        ResearchTime,
        FinishResearch
    }
    internal class ConsumableData
    {
        internal string id;
        ConsumableGroup group;
        internal string requireID;
        internal GridItemIcon icon;
        internal string description;
        internal Bonus bonus;
        internal int rate;

        public ConsumableData(string id, ConsumableGroup group, GridItemIcon icon, string description, Bonus bonus, string requireID, int rate)
        {
            this.id = id;
            this.icon = icon;
            this.description = description;
            this.bonus = bonus;
            this.rate = rate;
        }

    }
}
