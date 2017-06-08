using System.Collections.Generic;
using System;
using System.Linq;

public class Equipment
{
    public int level;
    public string dataID;
    internal string skillID;
    public Type type;
    public int grade; // N, R, SR ;;    // 글자색깔; 저장변수
    public double exp; // 저장변수
    public int evolution; // n, +, ++;  // 저장변수
    public string iconID;
    public bool isLock;   // 저장변수

    public string name;
    readonly List<BonusStat> baseStat;   // data로부터
    readonly List<BonusStat> baseSkillStat; // data로부터
    public int skillLevel;  // 저장변수
    public string skillName;    // data로부터

    internal static string StatToString(BaseStat stat, int exp, int maxExp)
    {
        BaseStat ExpectedStat = (BaseStat)stat.Clone();
        return ExpectedStat.ToString();
    }

    public double GetMaterialExp(int targetlevel)   // 대상이 100
    {
        int levelDiff = targetlevel - level;
        double levelDiffCoef = Math.Pow(EquipmentManager.CoefPerLevelUnit, levelDiff / EquipmentManager.LevelUnit);

        return (50f * levelDiffCoef * (grade + 1) * (evolution + 1));
    }
    //            double 레벨계수 = Math.Pow( powerPerDifficulty, level / 100);

    public List<BonusStat> Stat
    {
        get
        {
            int statCount = baseStat.Count;
            int PowerCoef = Singleton<TableLoader>.getInstance().powertable[level];
            double gradeCoef = EquipmentWiki.CalgradeCoef(grade);
            double evolCoef = EquipmentWiki.CalEvolCoef(evolution);
            double expCoef = EquipmentWiki.CalExpCoef(exp);

            List<BonusStat> result = new List<BonusStat>();
            foreach (BonusStat one in baseStat)
            {
                BonusStat temp = (BonusStat)one.Clone();
                temp.value = (int)(one.value * PowerCoef * gradeCoef * evolCoef * expCoef / statCount);
                result.Add(temp);
            }
            return result;
        }
    }

    public List<BonusStat> ExpectStat(double ExpectedExp, int ExpectedEvolution)
    {
        int statCount = baseStat.Count;
        int PowerCoef = Singleton<TableLoader>.getInstance().powertable[level];
        double gradeCoef = EquipmentWiki.CalgradeCoef(grade);
        double evolCoef = EquipmentWiki.CalEvolCoef(ExpectedEvolution);
        double expCoef = EquipmentWiki.CalExpCoef(ExpectedExp);

        List<BonusStat> result = new List<BonusStat>();
        foreach (BonusStat one in baseStat)
        {
            BonusStat temp = (BonusStat)one.Clone();
            temp.value = (int)(one.value * PowerCoef * gradeCoef * evolCoef * expCoef / statCount);
            result.Add(temp);
        }
        return result;
    }

    public void DrawIconView(UISprite icon, UILabel name, int expectedEV)
    {
        icon.spriteName = iconID;
        name.text = this.name + EquipmentWiki.EvToString(expectedEV);
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
                temp.value = (int)(b.value * (skillLevel + 2) / 3); // 1x ~ 4x
                result.Add(temp);
            }
            return result;
        }
    }

    public Equipment(EquipmentData data, EquipmentSkillData skilldata, int level)
    {
        dataID = data.ID;
        skillID = skilldata.ID;
        type = data.type;
        iconID = data.iconID;

        exp = 0;
        evolution = 0;
        grade = data.grade;
        this.level = level;
        skillLevel = 1;
        isLock = false;

        baseStat = data.stats;
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
    public enum Slot
    {
        Weapon = 0,
        Staff,
        Ring,
        Ring2
    }

    internal bool Compare(Equipment equipment)
    {
        return (this.grade == equipment.grade && this.evolution == equipment.evolution && this.dataID.Equals(equipment.dataID));
    }

    internal void AddExp(double amount)
    {
        exp += amount;
        if (exp > 100f)
            exp = 100f;
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
            return exp == 100 && evolution < 2;
        }

    }

}