using System;
using System.Collections.Generic;

public class ResearchData
{
    public static double GrowthRate = 1.148698f; // / 100
    public static double TimeGrothRate = 1.035264924;

    public string ID; // 리서치아이디
    public BonusStat bonus;
    public string requireID;
    public int requeireLevel;

    public string InfoName;
    public string InfoDescription;

    public NestedIcon icon;

    public int level;
    public int baselevel;
    public int baseprice;
    public int rate;
    public int basetime;

    public ResearchData(string id, string requireid, int requirelevel, NestedIcon icon, string name, string description, int level, int baselevel, int baseprice, int rate, int basetime, BonusStat bonus)
    {
        ID = id;
        this.bonus = bonus;
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
