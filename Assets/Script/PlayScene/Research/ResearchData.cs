using System;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    public class ResearchData
    {
        public string ID; // 리서치아이디
        public BonusStat bonus;
        public string requireID;
        public int requireLevel;

        public string Name;
        public string Description;

        public NestedIcon icon;
        public int showLevel; //
        public int baselevel; // baselevel of target level for coef
        public int baseprice;
        public int rate;
        public int basetime;

        public ResearchData()
        {
        }

    }
}

