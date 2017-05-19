using System.Collections.Generic;

public class EquipmentSkillData
{
    public string ID;
    public string baseSkillName;
    Equipment.Type droptype;
    public string dropMapID;
    public List<BonusStat> baseSkillStat;
    public int dropLevel;

    public EquipmentSkillData(string id, string name, int droplevel, Equipment.Type droptype, List<BonusStat> statList, string dropmapid = null)
    {
        ID = id;
        baseSkillName = name;
        baseSkillStat = statList;
        this.droptype = droptype;
        dropLevel = droplevel;
        dropMapID = dropmapid;
    }

}