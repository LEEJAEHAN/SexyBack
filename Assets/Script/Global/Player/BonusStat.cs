using System;

public enum Attribute
{
    Str,
    Int,
    Luck,
    Spd,
    BonusLevel,
    DpcX,
    DpcIncreaseXH,
    AttackCapacity,
    AttackSpeedXH,
    CriticalRateXH,
    CriticalDamageXH,
    MovespeedXH,
    DpsX,
    DpsIncreaseXH,
    CastSpeedXH,
    SkillRateIncreaseXH,
    SkillDmgIncreaseXH,
    ExpIncreaseXH,
    ResearchTime,
    ResearchTimeX,
    MaxResearchThread,
    LPriceReduceXH,
    RPriceReduceXH,
    InitExp,
    RankBonus,


    // only in instancegame
    Active,
    ActiveSkill,
    Enchant,
    ExpPerFloor,
}
public class BonusStat : ICloneable
{
    public string targetID;  // both hero and elemental
    public Attribute attribute;
    public int value;
    public string description;
    public string strvalue;

    public BonusStat(string targetID, Attribute attribute, int value, string strvalue, string description)
    {
        this.targetID = targetID;
        this.attribute = attribute;
        this.value = value;
        this.strvalue = strvalue;
        this.description = description;
    }

    public BonusStat()
    {
    }

    public object Clone()
    {
        return new BonusStat(this.targetID, this.attribute, this.value, this.strvalue, this.description);
    }



}