using System;
using System.Runtime.Serialization;
using System.Xml;

[Serializable]
internal class ElementalStat// 누적배수
{
    // dynamic stat
    internal int BonusLevel;
    internal BigInteger DpsX;
    internal int DpsIncreaseXH; // 
    internal int CastSpeedXH; //
    internal int SkillRateIncreaseXH;
    internal int SkillDmgIncreaseXH;

    // static stat
    internal int InitLevel;

    internal ElementalStat()
    {
        BonusLevel = 0;
        DpsX = new BigInteger(1);
        DpsIncreaseXH = 0;
        CastSpeedXH = 0;
        SkillRateIncreaseXH = 0;
        SkillDmgIncreaseXH = 0;
        InitLevel = 1;
    }

    internal void ApplyElementStat(BonusStat bonus, bool signPositive)
    {
        if (signPositive)
            Add(bonus);
        else
            Remove(bonus);
    }

    internal void Add(BonusStat bonus)
    {
        switch (bonus.attribute)
        {
            case Attribute.BonusLevel:
                BonusLevel += bonus.value;
                break;
            case Attribute.InitLevel:
                InitLevel += bonus.value;
                break;
            case Attribute.DpsX:
                DpsX *= bonus.value;
                break;
            case Attribute.DpsIncreaseXH:
                DpsIncreaseXH += bonus.value;
                break;
            case Attribute.CastSpeedXH:
                CastSpeedXH += bonus.value;
                break;
            case Attribute.SkillRateIncreaseXH:
                SkillRateIncreaseXH += bonus.value;
                break;
            case Attribute.SkillDmgIncreaseXH:
                SkillDmgIncreaseXH += bonus.value;
                break;
            default:
                    sexybacklog.Error("noAttribute");
                    break;
        }
    }
    internal void Remove(BonusStat bonus)
    {
        switch (bonus.attribute)
        {
            case Attribute.BonusLevel:
                BonusLevel -= bonus.value;
                break;
            case Attribute.InitLevel:
                InitLevel -= bonus.value;
                break;
            case Attribute.DpsX:
                DpsX /= bonus.value;
                break;
            case Attribute.DpsIncreaseXH:
                DpsIncreaseXH -= bonus.value;
                break;
            case Attribute.CastSpeedXH:
                CastSpeedXH -= bonus.value;
                break;
            case Attribute.SkillRateIncreaseXH:
                SkillRateIncreaseXH -= bonus.value;
                break;
            case Attribute.SkillDmgIncreaseXH:
                SkillDmgIncreaseXH -= bonus.value;
                break;
            default:
                sexybacklog.Error("noAttribute");
                break;
        }
    }

    internal void ApplyBonus(BonusStat bonus, bool signPositive)
    {
        if (signPositive)
            Add(bonus);
        else
            Remove(bonus);
    }
}

