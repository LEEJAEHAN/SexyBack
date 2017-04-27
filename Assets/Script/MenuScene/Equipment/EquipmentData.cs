using System.Collections.Generic;

public class EquipmentData
{
    public string ID;
    public Equipment.Type type;
    public string baseName;
    public int grade;
    public BaseStat baseStat;
    public string baseSkillName;
    public List<Bonus> baseSkillStat;

    public EquipmentData()
    {
        ID = "E01";
        type = Equipment.Type.Weapon;
        baseName = "롱소드";
        baseStat = new BaseStat(30, 0, 0, 0);
        baseSkillName = "자와자와";
        baseSkillStat = new List<Bonus>();
        baseSkillStat.Add(new Bonus("fireball", "DpsX", 2, null));

        grade = 0;


    }
}