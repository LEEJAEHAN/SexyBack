using System.Collections.Generic;

public class EquipmentData
{
    public string ID;
    internal string iconID;
    public string baseName;
    public string skillID;
    public Equipment.Type type;
    public int grade;
    public int dropStart;
    public int dropEnd;
    internal bool belong;

    //public BaseStat baseStat;
    //public string dropMapID;
    //public int droplevel;

    public List<BonusStat> stats;

    public EquipmentData()
    {
        stats = new List<BonusStat>();
    }
}