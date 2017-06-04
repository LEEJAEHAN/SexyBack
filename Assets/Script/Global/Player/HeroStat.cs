using System;
using System.Runtime.Serialization;
using System.Xml;

[Serializable]
public class HeroStat
{
    public int BonusLevel;
    public BigInteger DpcX; // 곱계수는 X를붙인다.
    public int AttackCapacity; // 공격스택횟수
    public int DpcIncreaseXH; // 
    public int AttackSpeedXH;
    public int CriticalRateXH;
    public int CriticalDamageXH;
    public int MovespeedXH;

    // only use battleScene
    //public int Level;

    internal HeroStat()
    {
        BonusLevel = 0;
        DpcX = 1;
        DpcIncreaseXH = 0; // Str과는 다르다.
        AttackSpeedXH = 0; // Spd와는 다르다.
        CriticalRateXH = 0; // 20;
        CriticalDamageXH = 0; // 425;
        MovespeedXH = 0;
        AttackCapacity = 1;
    }
    
    internal void ApplyBonus(BonusStat bonus, bool signPositive)
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
            case Attribute.DpcX:
                DpcX *= bonus.value;
                break;
            case Attribute.DpcIncreaseXH:
                DpcIncreaseXH += bonus.value;
                break;
            case Attribute.AttackCapacity:
                AttackCapacity += bonus.value;
                break;
            case Attribute.AttackSpeedXH:
                AttackSpeedXH += bonus.value;
                break;
            case Attribute.CriticalRateXH:
                CriticalRateXH += bonus.value;
                break;
            case Attribute.CriticalDamageXH:
                CriticalDamageXH += bonus.value;
                break;
            case Attribute.MovespeedXH:
                MovespeedXH += bonus.value;
                break;
            default:
                {
                    sexybacklog.Error("noAttribute");
                    break;
                }
        }
    }
    internal void Remove(BonusStat bonus)
    {
        switch (bonus.attribute)
        {
            case Attribute.BonusLevel:
                BonusLevel -= bonus.value;
                break;
            case Attribute.DpcX:
                DpcX /= bonus.value;
                break;
            case Attribute.DpcIncreaseXH:
                DpcIncreaseXH -= bonus.value;
                break;
            case Attribute.AttackCapacity:
                AttackCapacity -= bonus.value;
                break;
            case Attribute.AttackSpeedXH:
                AttackSpeedXH -= bonus.value;
                break;
            case Attribute.CriticalRateXH:
                CriticalRateXH -= bonus.value;
                break;
            case Attribute.CriticalDamageXH:
                CriticalDamageXH -= bonus.value;
                break;
            case Attribute.MovespeedXH:
                MovespeedXH -= bonus.value;
                break;
            default:
                {
                    sexybacklog.Error("noAttribute");
                    break;
                }
        }
    }
}