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
    ConsumableX,
    ReputationXH,
    BaseDensityAdd,
    FastStage,

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
    public double dvalue;
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
    public BonusStat(string targetID, Attribute attribute, double dvalue)
    {
        this.targetID = targetID;
        this.attribute = attribute;
        this.dvalue = dvalue;
    }

    public BonusStat()
    {
    }

    public object Clone()
    {
        var clone = new BonusStat(this.targetID, this.attribute, this.value);
        clone.strvalue = this.strvalue;
        clone.dvalue = this.dvalue;
        return clone;
    }


}