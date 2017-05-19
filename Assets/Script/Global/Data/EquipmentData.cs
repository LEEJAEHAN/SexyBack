using System.Collections.Generic;

public class EquipmentData
{
    public string ID;
    internal string iconID;
    public string baseName;
    public string dropMapID;

    public Equipment.Type type;
    public int grade;
    public int droplevel;
    public BaseStat baseStat;
    private Equipment.Type weapon;

    public EquipmentData(string id, string iconid, string name, Equipment.Type type, int droplevel, int grade, BaseStat stat, string dropmapid = null)
    {
        ID = id;
        iconID = iconid;
        this.type = type;
        baseName = name;
        baseStat = stat;
        this.grade = grade;
        this.droplevel = droplevel;
        dropMapID = dropmapid;
    }
}