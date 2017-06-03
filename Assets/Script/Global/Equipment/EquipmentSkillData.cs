using System.Collections.Generic;

public class EquipmentSkillData
{
    public string ID;
    public string baseSkillName;
    public bool belong;
    public List<BonusStat> baseSkillStat;
    public int dropLevel;

    public EquipmentSkillData()
    {
        baseSkillStat = new List<BonusStat>();
    }
}