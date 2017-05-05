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
    public BaseStat baseStat;
    List<BonusStat> baseSkillStat;
    public int skillLevel;
    public string skillName;

    bool isLock;

    internal static string StatToString(BaseStat stat, int exp, int maxExp)
    {
        BaseStat ExpectedStat = (BaseStat)stat.Clone();
        return ExpectedStat.ToString();
    }

    public int MaxExp
    {
        get
        {
            return EquipmentWiki.CalMaxExp(grade, evolution);
        }
    }
    public int MaterialExp
    {
        get
        {
            return 10 * (grade + 1) * (evolution + 1);
        }
    }
    public BaseStat Stat
    {
        get
        {
            double evolCoef = EquipmentWiki.CalEvolCoef(evolution);
            double expCoef = EquipmentWiki.CalExpCoef(Exp, MaxExp);
            return baseStat * (evolCoef * expCoef);
        }
    }
    public BaseStat ExpectStat(int ExpectedExp, int ExpectedEvolution)
    {
        double evolCoef = EquipmentWiki.CalEvolCoef(ExpectedEvolution);
        double expCoef = EquipmentWiki.CalExpCoef(ExpectedExp, EquipmentWiki.CalMaxExp(grade, ExpectedEvolution));
        return baseStat * (evolCoef * expCoef);
    }


    //public static int CalMaxExp(int grade, int evolution)
    //{
    //}
    //internal static int CalMaterialExp(int grade, int evolution)
    //{
    //}

    public List<BonusStat> SkillStat
    {
        get
        {
            List<BonusStat> result = new List<BonusStat>();
            foreach (BonusStat b in baseSkillStat)
            {
                BonusStat temp = (BonusStat)b.Clone();
                temp.value = (int)(b.value + b.value * ((skillLevel - 1) * 0.3334f));
                result.Add(temp);
            }
            return result;
        }
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

        baseStat = data.baseStat;
        baseSkillStat = data.baseSkillStat;

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