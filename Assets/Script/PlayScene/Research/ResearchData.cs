using System;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    public class ResearchData
    {
        //public static double GrowthRate = 1.148698f;            // / 100 // 2의 5분의1승( 5층마다 2배 )
        //public static double TimeGrothRate = 1.035264924;       // 2의 의 20분의 1승 ( 20층마다 2배 )

        public string ID; // 리서치아이디
        public BonusStat bonus;
        public string requireID;

        public string Name;
        public string Description;

        public NestedIcon icon;

        public int Level;// currentlevel of target 
        public int showlevel; //
        public int baselevel; // baselevel of target
        public int baseprice;
        public int rate;
        public int basetime;

        public ResearchData(string id, string requireid, int showlevel, NestedIcon icon, string name, string description, int level, int baselevel, int baseprice, int rate, int basetime, BonusStat bonus)
        {
            ID = id;
            this.bonus = bonus;
            requireID = requireid;
            this.showlevel = showlevel;
            Name = name;
            this.icon = icon;

            Description = description;
            this.Level = level;
            this.baselevel = baselevel;
            this.baseprice = baseprice;
            this.rate = rate;
            this.basetime = basetime;
        }

    }
}

