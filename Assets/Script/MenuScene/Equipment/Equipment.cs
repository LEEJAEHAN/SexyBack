using System.Collections.Generic;
public class Equipment
{
    string dataID;
    Type type;
    int grade; // N, R, SR ;;
    int Exp;
    int evolution; // n, +, ++;

    public string name;
    BaseStat stat;

    List<Bonus> skillStat;
    int skillLevel;
    string skillName;

    bool isLock;
    
    public Equipment(EquipmentData data)
    {
        dataID = data.ID;
        type = data.type;
        grade = data.grade;

        Exp = 0;
        evolution = 0;
        skillLevel = 1;

        stat = data.baseStat;
        skillStat = data.baseSkillStat;

        skillName = data.baseSkillName + "Lv."+skillLevel.ToString();
        name = data.baseName + EquipmentWiki.ToString(evolution);
    }
    public enum Type
    {
        Weapon

    }
}