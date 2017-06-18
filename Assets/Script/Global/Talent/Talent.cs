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
            if (BonusPerLevel == null)
                return null;
            BonusStat temp = (BonusStat)BonusPerLevel.Clone();
            if(temp.attribute == Attribute.ResearchTimeX || temp.attribute == Attribute.InitExpCoef)
                temp.value = (int)Math.Pow(temp.value, Level);
            else
                temp.value *= Level;
            return temp;
        }
    }
    internal BonusStat JobBonus { get { return baseData.JobBonus; } }
    public string Name { get { return baseData.Name; } }
    public int NextPrice
    {
        get
        {
            int talentlevel = baseData.BaseLevel + (int)Math.Round(baseData.BaseLevelPerLevel * Level, 0);
            return (int)Math.Round(TalentManager.ReputationUnit * PlayerStatus.CalGlobalGrowth(talentlevel), 0);
        }
    }
    public bool IsMaxLevel { get { return Level >= baseData.MaxLevel; } }
    internal Talent(TalentData data, int level)
    {
        baseData = data;
        Level = level;
    }

}