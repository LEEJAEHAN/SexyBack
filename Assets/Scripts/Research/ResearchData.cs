using System;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    public class ResearchData
    {
        public static double GrowthRate = 1.148698f; // / 100
        public static double TimeGrothRate = 1.035264924;

        public string ID; // 리서치아이디
        public List<Bonus> bonuses = new List<Bonus>();
        public string requireID;
        public int requeireLevel;

        public string InfoName;
        public string InfoDescription;

        public GridItemIcon icon;

        public int level;
        public int baselevel;
        public int baseprice;
        public int rate;
        public int basetime;

        public ResearchData(string id, string requireid, int requirelevel, GridItemIcon icon, string name, string description, int level, int baselevel, int baseprice, int rate, int basetime, List<Bonus> bonuselist)
        {
            ID = id;
            bonuses = bonuselist;
            requireID = requireid;
            requeireLevel = requirelevel;

            InfoName = name;
            this.icon = icon;

            InfoDescription = description;

            this.level = level;
            this.baselevel = baselevel;
            this.baseprice = baseprice;
            this.rate = rate;
            this.basetime = basetime;

        }

    }

    public class Bonus
    {
        public string targetID;  // both hero and elemental
        public string attribute;
        public int value;
        public string strvalue;

        public Bonus(string targetID, string attribute, int value, string strvalue)
        {
            this.targetID = targetID;
            this.attribute = attribute;
            this.value = value;
            this.strvalue = strvalue;
        }
    }
    //DpsXPer5LV,
    //DpsX,
    //attackspeedXH,

}