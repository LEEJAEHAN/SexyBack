using System;
using System.Runtime.Serialization;

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
    public int Level;
    public double BaseDmg;

    internal HeroStat()
    {
        Level = 1;
        BonusLevel = 0;
        BaseDmg = 1;

        DpcX = 1;
        DpcIncreaseXH = 100; // Str과는 다르다.
        AttackSpeedXH = 100; // Spd와는 다르다.
        CriticalRateXH = 20;
        CriticalDamageXH = 425;

        MovespeedXH = 1000;
        AttackCapacity = 3;
    }
    internal void Add(BonusStat bonus)
    {
        switch (bonus.attribute)
        {
            case "Level":
                Level += bonus.value;
                break;
            case "Enchant":
                BaseDmg += Singleton<TableLoader>.getInstance().elementaltable[bonus.strvalue].BaseDmgDensity;
                break;
            case "BonusLevel":
                BonusLevel += bonus.value;
                break;
            case "DpcX":
                DpcX *= bonus.value;
                break;
            case "DpcIncreaseXH":
                DpcIncreaseXH += bonus.value;
                break;
            case "AttackCapacity":
                AttackCapacity += bonus.value;
                break;
            case "AttackSpeedXH":
                AttackSpeedXH += bonus.value;
                break;
            case "CriticalRateXH":
                CriticalRateXH += bonus.value;
                break;
            case "CriticalDamageXH":
                CriticalDamageXH += bonus.value;
                break;
            case "MovespeedXH":
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
            case "BonusLevel":
                BonusLevel -= bonus.value;
                break;
            case "DpcX":
                DpcX /= bonus.value;
                break;
            case "DpcIncreaseXH":
                DpcIncreaseXH -= bonus.value;
                break;
            case "AttackCapacity":
                AttackCapacity -= bonus.value;
                break;
            case "AttackSpeedXH":
                AttackSpeedXH -= bonus.value;
                break;
            case "CriticalRateXH":
                CriticalRateXH -= bonus.value;
                break;
            case "CriticalDamageXH":
                CriticalDamageXH -= bonus.value;
                break;
            case "MovespeedXH":
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