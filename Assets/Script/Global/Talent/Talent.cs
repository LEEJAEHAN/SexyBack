using System;

internal class Talent
{
    readonly TalentData baseData;

    public int Level;
    internal string ID {  get { return baseData.ID; } }
    internal BonusStat BonusPerLevel { get { return baseData.BonusPerLevel; } }
    internal BonusStat TotalBonus { get { return baseData.BonusPerLevel * Level; } }
    internal BonusStat JobBonus { get { return baseData.JobBonus; } }
    public string Name { get { return baseData.Name; } }
    public int PriceCoef { get { return baseData.PriceCoef; } }

    internal Talent(TalentData data, int level)
    {
        baseData = data;
        Level = level;
    }

}