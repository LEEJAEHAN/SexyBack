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
    public const double MaxExp = 100f;
    public int limit; // n, +, ++;  // 저장변수
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

    public double GetMaterialExp(Equipment target)   // 대상이 100
    {
        int levelDiff = level - target.level;
        // 0 이면 나누기 1 , 1이면 나누기
        double levelDiffCoef = PlayerStatus.CalGlobalGrowth(levelDiff);
        int sourceGradeCoef = Math.Min(3,(grade + 1)); // 노멀1배,매직2배,레어3배,유니크3배
        int targetGradeCoef = Math.Min(3, (target.grade + 1)); // 노멀1배,매직2배,레어3배,유니크3배
        //int targetLimitCoef = target.grade == 3 ? 7 : (int)Math.Pow(2, target.limit);
        // None == 1,+ ==2, ++ ==4, 유니크 ==7
        int targetLimitCoef = 7;
        return (100f) * levelDiffCoef * sourceGradeCoef / targetGradeCoef / targetLimitCoef;
    }

    public List<BonusStat> DmgStat
    {
        get
        {
            return ExpectDmgStat(exp, limit, grade);
        }
    }



    public List<BonusStat> ExpectDmgStat(double ExpectedExp, int ExpectedLimit, int ExpectedGrade)
    {
        // 장통계
        double BaseDmg = PlayerStatus.CalGlobalGrowth(level) / 16;
        //Singleton<TableLoader>.getInstance().powertable[level];
        double gradeCoef = EquipmentWiki.CalgradeCoef(ExpectedGrade);
        double CalLimitCoef = EquipmentWiki.CalLimitCoef(ExpectedLimit);
        double expCoef = EquipmentWiki.CalExpCoef(ExpectedGrade, ExpectedExp);

        List<BonusStat> result = new List<BonusStat>();
        foreach (BonusStat one in damageStat)
        {
            BonusStat temp = (BonusStat)one.Clone();
            temp.dvalue = one.dvalue * BaseDmg * gradeCoef * CalLimitCoef * expCoef;

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

    public void DrawIconView(UISprite icon, UILabel name, int expectedgrade, int expectedlimit, int itemlevel)
    {
        icon.spriteName = iconID;
        name.text = string.Format("{0}{1} LV.{2}", this.name, EquipmentWiki.PrintLimit(expectedlimit), itemlevel);
        name.color = EquipmentWiki.CalNameColor(expectedgrade);
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
        limit = 0;
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

    internal bool isSameItem(Equipment equipment)
    {
        //return (this.grade == equipment.grade && this.limit == equipment.limit && this.dataID.Equals(equipment.dataID));
        return (this.grade == equipment.grade && this.dataID.Equals(equipment.dataID));
    }

    internal void AddExp(double amount)
    {
        exp += amount;
        if (exp > MaxExp)
            exp = MaxExp;
    }
    internal bool isMaxExp()
    {
        return exp >= MaxExp;
    }


    internal void UnLimit()
    {
        limit++;
        if (limit > 2)
            limit = 2;
    }
    internal bool CanUnLimit
    {
        get
        {
            return isMaxExp() && limit < 2;
        }
    }
    internal bool CanGradeUp
    {
        get
        {
            return isMaxExp() && grade < 2;
        }
    }
    internal void GradeUp()
    {
        grade++;
        if (grade > 2)
            grade = 2;
    }
}