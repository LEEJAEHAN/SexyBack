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
    readonly List<BonusStat> damageStat;   // data로부터
    readonly List<BonusStat> baseSkillStat; // data로부터
    public int skillLevel;  // 저장변수
    public string skillName;    // data로부터

    internal static string StatToString(BaseStat stat, int exp, int maxExp)
    {
        BaseStat ExpectedStat = (BaseStat)stat.Clone();
        return ExpectedStat.ToString();
    }

    public double GetMaterialExp(int targetlevel, int targetgrade)   // 대상이 100
    {
        int levelDiff = level - targetlevel;
        // 0 이면 나누기 1 , 1이면 나누기
        double levelDiffCoef = PlayerStatus.CalGlobalGrowth(levelDiff);
        return (50f * levelDiffCoef * (grade + 1) * (evolution + 1) / (targetgrade + 1));
    }

    public List<BonusStat> DmgStat
    {
        get
        {
            return ExpectDmgStat(this.exp, this.evolution);
        }
    }

    public List<BonusStat> ExpectDmgStat(double ExpectedExp, int ExpectedEvolution)
    {
        double BaseDmg = PlayerStatus.CalGlobalGrowth(level) / 4;//Singleton<TableLoader>.getInstance().powertable[level];

        double gradeCoef = EquipmentWiki.CalgradeCoef(grade);
        double evolCoef = EquipmentWiki.CalEvolCoef(ExpectedEvolution);
        double expCoef = EquipmentWiki.CalExpCoef(ExpectedExp);

        List<BonusStat> result = new List<BonusStat>();
        foreach (BonusStat one in damageStat)
        {
            BonusStat temp = (BonusStat)one.Clone();
            temp.dvalue = one.dvalue * BaseDmg * gradeCoef * evolCoef * expCoef;

            if (one.targetID == "elementals")
            {
                int ElementalCount = 0;
                foreach (var e in Singleton<TableLoader>.getInstance().elementaltable.Values)
                {
                    if (level > e.BaseLevel)
                        ElementalCount++;
                }
                temp.dvalue /= ElementalCount;
            }
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

        damageStat = data.stats;
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