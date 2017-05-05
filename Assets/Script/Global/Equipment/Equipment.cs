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
    public bool isMaxExp
    {
        get
        {
            return Exp == MaxExp;
        }
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

        Exp = 100;
        evolution = 0;
        skillLevel = 1;

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

    internal bool Compare(Equipment equipment)
    {
        return (this.grade == equipment.grade && this.evolution == equipment.evolution && this.dataID.Equals(equipment.dataID));
    }

    internal void AddExp(int amount)
    {
        Exp += amount;
        if (Exp > MaxExp)
            Exp = MaxExp;
    }

    internal void Evolution()
    {
        evolution++;
        if (evolution > 2)
            evolution = 2;
    }
    internal bool CanEvolution
    {
        get
        {
            return isMaxExp && evolution < 2;
        }

    }

}