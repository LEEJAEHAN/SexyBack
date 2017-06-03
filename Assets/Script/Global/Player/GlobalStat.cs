using System;
using System.Xml;

internal class GlobalStat
{
    internal int RankBonus;
    internal BigInteger InitExp;
    internal int BonusEquipment;

    public GlobalStat()
    {
        // 게임 전후, recal을 하므로 저장할 필요가 없다.
        RankBonus = 0;
        InitExp = new BigInteger(0);
    }

    public GlobalStat(XmlNode xmlNode)
    {
        RankBonus = int.Parse(xmlNode.Attributes["RankBonus"].Value);
        InitExp = new BigInteger(xmlNode.Attributes["InitExp"].Value);
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
            case Attribute.InitExp:
                InitExp += new BigInteger(bonus.strvalue);
                break;
            case Attribute.RankBonus:
                RankBonus += bonus.value;
                break;
            case Attribute.BonusEquipment:
                BonusEquipment += bonus.value;
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
            case Attribute.InitExp:
                InitExp -= new BigInteger(bonus.strvalue);
                break;
            case Attribute.RankBonus:
                RankBonus -= bonus.value;
                break;
            case Attribute.BonusEquipment:
                BonusEquipment -= bonus.value;
                break;
            default:
                {
                    sexybacklog.Error("noAttribute");
                    break;
                }
        }
    }

}
