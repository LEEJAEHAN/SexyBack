using System;
using System.Runtime.Serialization;
using System.Xml;

[Serializable]
internal class UtilStat
{
    internal int ResearchTime;
    internal int ResearchTimeX;
    internal int MaxResearchThread;
    internal int ExpIncreaseXH;
    internal int LPriceReduceXH;
    internal int RPriceReduceXH;

    public UtilStat()
    {
        ResearchTimeX = 1;
        ResearchTime = 0;
        MaxResearchThread = 1; // for test
        ExpIncreaseXH = 0;
        LPriceReduceXH = 0;
        RPriceReduceXH = 0;
    }

    public UtilStat(XmlNode xmlNode)
    {
        ResearchTimeX = int.Parse(xmlNode.Attributes["ResearchTimeX"].Value);
        ResearchTime = int.Parse(xmlNode.Attributes["ResearchTime"].Value);
        MaxResearchThread = int.Parse(xmlNode.Attributes["MaxResearchThread"].Value);
        ExpIncreaseXH = int.Parse(xmlNode.Attributes["ExpIncreaseXH"].Value);
        LPriceReduceXH = int.Parse(xmlNode.Attributes["LPriceReduceXH"].Value);
        RPriceReduceXH = int.Parse(xmlNode.Attributes["RPriceReduceXH"].Value);
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
            case Attribute.ExpIncreaseXH:
                ExpIncreaseXH += bonus.value;
                break;
            case Attribute.ResearchTime:
                ResearchTime += bonus.value;
                break;
            case Attribute.ResearchTimeX:
                this.ResearchTimeX *= bonus.value;
                break;
            case Attribute.MaxResearchThread:
                MaxResearchThread += bonus.value;
                break;
            case Attribute.LPriceReduceXH:
                LPriceReduceXH -= bonus.value;
                break;
            case Attribute.RPriceReduceXH:
                RPriceReduceXH -= bonus.value;
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
            case Attribute.ExpIncreaseXH:
                ExpIncreaseXH -= bonus.value;
                break;
            case Attribute.ResearchTime:
                ResearchTime -= bonus.value;
                break;
            case Attribute.ResearchTimeX:
                this.ResearchTimeX /= bonus.value;
                break;
            case Attribute.MaxResearchThread:
                MaxResearchThread -= bonus.value;
                break;
            case Attribute.LPriceReduceXH:
                LPriceReduceXH += bonus.value;
                break;
            case Attribute.RPriceReduceXH:
                RPriceReduceXH += bonus.value;
                break;
            default:
                {
                    sexybacklog.Error("noAttribute");
                    break;
                }
        }
    }

}