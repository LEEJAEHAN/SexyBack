using System.Collections.Generic;

public class EquipmentData
{
    public string ID;
    public Equipment.Type type;
    public string baseName;
    public int grade;
    public BaseStat baseStat;
    public string baseSkillName;
    public List<BonusStat> baseSkillStat;
    internal string iconID;

    public EquipmentData(string id, string iconid, string name, Equipment.Type type, int grade,
        BaseStat stat, string skillname, List<BonusStat> skillstat)
    {
        ID = id;
        iconID = iconid;
        this.type = type;
        baseName = name;
        baseStat = stat;
        baseSkillName = skillname;
        baseSkillStat = skillstat;
        this.grade = grade;
    }
}