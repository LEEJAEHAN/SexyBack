using System.Collections.Generic;

public class EquipmentSkillData
{
    public string ID;
    public string baseSkillName;
    public bool belong;
    public List<BonusStat> baseSkillStat;
    //public int dropLevel;
    internal int dropStart;
    internal int dropEnd;

    public EquipmentSkillData()
    {
        baseSkillStat = new List<BonusStat>();
    }
}