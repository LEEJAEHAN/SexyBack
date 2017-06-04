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
    InitExpCoef,
    RankBonus,
    BonusEquipment,
    BonusConsumable,
    ReputationXH,

    // only in instancegame "ingame"
    Active,
    ActiveSkill,
    Enchant,
}
public class BonusStat : ICloneable
{
    public string targetID;  // both hero and elemental
    public Attribute attribute;
    public int value;
    public string strvalue;

    public BonusStat(string targetID, Attribute attribute, int value)
    {
        this.targetID = targetID;
        this.attribute = attribute;
        this.value = value;
    }
    public BonusStat(string targetID, Attribute attribute, string strvalue)
    {
        this.targetID = targetID;
        this.attribute = attribute;
        this.strvalue = strvalue;
    }

    public BonusStat()
    {
    }

    public static BonusStat operator *(BonusStat origin, int scalar)
    {
        return new BonusStat(origin.targetID, origin.attribute, origin.value * scalar);
    }

    public object Clone()
    {
        var clone = new BonusStat(this.targetID, this.attribute, this.value);
        clone.strvalue = this.strvalue;
        return clone;
    }



}