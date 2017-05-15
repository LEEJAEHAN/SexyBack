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

    public HeroStat(XmlNode xmlNode)
    {
        BonusLevel = int.Parse(xmlNode.Attributes["BonusLevel"].Value);
        DpcX = new BigInteger(xmlNode.Attributes["DpcX"].Value);
        DpcIncreaseXH = int.Parse(xmlNode.Attributes["DpcIncreaseXH"].Value);
        AttackSpeedXH = int.Parse(xmlNode.Attributes["AttackSpeedXH"].Value);
        CriticalRateXH = int.Parse(xmlNode.Attributes["CriticalRateXH"].Value);
        CriticalDamageXH = int.Parse(xmlNode.Attributes["CriticalDamageXH"].Value);
        MovespeedXH = int.Parse(xmlNode.Attributes["MovespeedXH"].Value);
        AttackCapacity = int.Parse(xmlNode.Attributes["AttackCapacity"].Value);
    }

    internal void Add(BonusStat bonus)
    {
        switch (bonus.attribute)
        {
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