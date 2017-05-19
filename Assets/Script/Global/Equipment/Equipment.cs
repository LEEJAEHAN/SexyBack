using System.Collections.Generic;
using System;
using System.Linq;

public class Equipment
{
    public string dataID;
    internal string skillID;
    public Type type;
    public int grade; // N, R, SR ;;    // 저장변수
    public int exp; // 저장변수
    public int evolution; // n, +, ++;  // 저장변수
    public string iconID;
    public bool isLock;   // 저장변수

    public string name;
    public BaseStat baseStat;   // data로부터
    List<BonusStat> baseSkillStat; // data로부터
    public int skillLevel;  // 저장변수
    public string skillName;    // data로부터

    internal static string StatToString(BaseStat stat, int exp, int maxExp)
    {
        BaseStat ExpectedStat = (BaseStat)stat.Clone();
        return ExpectedStat.ToString();
    }
    public bool isMaxExp
    {
        get
        {
            return exp == MaxExp;
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
            double expCoef = EquipmentWiki.CalExpCoef(exp, MaxExp);
            return baseStat * (evolCoef * expCoef);
        }
    }
    public BaseStat ExpectStat(int ExpectedExp, int ExpectedEvolution)
    {
        double evolCoef = EquipmentWiki.CalEvolCoef(ExpectedEvolution);
        double expCoef = EquipmentWiki.CalExpCoef(ExpectedExp, EquipmentWiki.CalMaxExp(grade, ExpectedEvolution));
        return baseStat * (evolCoef * expCoef);
    }

    public void DrawIconView(UISprite icon, UILabel name)
    {
        icon.spriteName = iconID;
        name.text = this.name;
        name.color = EquipmentWiki.CalNameColor(grade);
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

    public Equipment(EquipmentData data, EquipmentSkillData skilldata)
    {
        dataID = data.ID;
        skillID = skilldata.ID;
        type = data.type;
        grade = data.grade;
        iconID = data.iconID;

        exp = 0;
        evolution = 0;
        skillLevel = 1;
        isLock = false;

        baseStat = data.baseStat;
        baseSkillStat = skilldata.baseSkillStat;

        skillName = skilldata.baseSkillName;
        name = data.baseName;
    }

    public enum Type
    {
        Weapon = 0,
        Staff,
        Ring
    }

    internal bool Compare(Equipment equipment)
    {
        return (this.grade == equipment.grade && this.evolution == equipment.evolution && this.dataID.Equals(equipment.dataID));
    }

    internal void AddExp(int amount)
    {
        exp += amount;
        if (exp > MaxExp)
            exp = MaxExp;
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