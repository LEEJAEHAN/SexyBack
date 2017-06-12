using System;

internal class Talent
{
    readonly TalentData baseData;

    public int Level;
    internal string ID { get { return baseData.ID; } }
    internal BonusStat BonusPerLevel { get { return baseData.BonusPerLevel; } }
    internal BonusStat TotalBonus {
        get
        {
            BonusStat temp = (BonusStat)BonusPerLevel.Clone();
            temp.value *= Level;
            return temp;
        }
    }
    internal BonusStat JobBonus { get { return baseData.JobBonus; } }
    public string Name { get { return baseData.Name; } }
    public int PriceCoef { get { return baseData.PriceCoef; } }
    public bool IsMaxLevel { get { return Level >= baseData.MaxLevel; } }
    internal Talent(TalentData data, int level)
    {
        baseData = data;
        Level = level;
    }

}