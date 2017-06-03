using System;

public enum Attribute
{
    Str,
    Int,
    Luck,
    Spd,
    BonusLevel,
    InitLevel,
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
    BonusEquipment,
    BonusConsumable,

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
    public string strvalue;

    public BonusStat(string targetID, Attribute attribute, int value, string strvalue)
    {
        this.targetID = targetID;
        this.attribute = attribute;
        this.value = value;
        this.strvalue = strvalue;
    }

    public BonusStat()
    {
    }

    public static BonusStat operator *(BonusStat origin, int scalar)
    {
        return new BonusStat(origin.targetID, origin.attribute, origin.value * scalar, origin.strvalue);
    }

    public object Clone()
    {
        return new BonusStat(this.targetID, this.attribute, this.value, this.strvalue);
    }



}