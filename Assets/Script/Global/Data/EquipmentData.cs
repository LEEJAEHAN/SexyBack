using System.Collections.Generic;

public class EquipmentData
{
    public string ID;
    internal string iconID;
    public string baseName;
    public string dropMapID;
    public string skillID;

    public Equipment.Type type;
    public int grade;
    public int droplevel;
    public BaseStat baseStat;

    public EquipmentData()
    {
        baseStat = new BaseStat();
    }
}