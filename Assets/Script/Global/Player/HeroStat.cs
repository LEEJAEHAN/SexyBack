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
    public int Level;
    public double BaseDmg;
    private XmlNode xmlNode;

    internal HeroStat()
    {
        Level = 1;
        BaseDmg = 1;

        BonusLevel = 0;
        DpcX = 1;
        DpcIncreaseXH = 100; // Str과는 다르다.
        AttackSpeedXH = 100; // Spd와는 다르다.
        CriticalRateXH = 20;
        CriticalDamageXH = 425;
        MovespeedXH = 100;
        AttackCapacity = 1;
    }

    public HeroStat(XmlNode xmlNode)
    {
        Level = 1;
        BaseDmg = 1;

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