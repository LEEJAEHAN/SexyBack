using System.Collections.Generic;
using System;
using System.Linq;

public class Equipment
{
    string dataID;
    public Type type;
    public int grade; // N, R, SR ;;
    public int Exp;
    public int evolution; // n, +, ++;
    public string iconID;
    public bool Lock;

    public string name;
    BaseStat stat;
    List<BonusStat> skillStat;
    public int skillLevel;
    public string skillName;

    bool isLock;

    internal static string StatToString(BaseStat stat, int exp, int maxExp)
    {
        BaseStat ExpectedStat = (BaseStat)stat.Clone();
        return ExpectedStat.ToString();
    }

    public BaseStat GetStat()
    {
        return GetStat(Exp, evolution);
    }
    public BaseStat GetStat(int ExpectedExp, int ExpectedEvolution)
    {
        double evolCoef;
        double expCoef;

        if (ExpectedEvolution >= 2)
            evolCoef = 2;
        else if (ExpectedEvolution == 1)
            evolCoef = 1.5f;
        else
            evolCoef = 1f;

        expCoef = 1 + (double)ExpectedExp / (double)EquipmentWiki.GetMaxExp(grade, ExpectedEvolution);
        return stat * (evolCoef * expCoef);
    }
    public List<BonusStat> GetSkillStat()
    {
        List<BonusStat> result = new List<BonusStat>();
        foreach (BonusStat b in skillStat)
        {
            BonusStat temp = (BonusStat)b.Clone();
            temp.value = (int)(b.value + b.value * ((skillLevel - 1) * 0.3334f));
            result.Add(temp);
        }
        return result;
    }

    public Equipment(EquipmentData data)
    {
        dataID = data.ID;
        type = data.type;
        grade = data.grade;
        iconID = data.iconID;

        Exp = 20;
        evolution = 0;
        skillLevel = 5;

        stat = data.baseStat;
        skillStat = data.baseSkillStat;

        skillName = data.baseSkillName;
        name = data.baseName;
    }
    public enum Type
    {
        Weapon,
        Staff,
        Ring
    }
}