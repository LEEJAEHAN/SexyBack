using System;
using System.Runtime.Serialization;
using System.Xml;

[Serializable]
internal class UtilStat
{
    internal int ResearchTime; 
    internal int MaxResearchThread;
    internal int ExpIncreaseXH;
    internal int LPriceReduceXH;
    internal int RPriceReduceXH;
    internal BigInteger InitExp;

    // only use battleScene
    internal int ResearchTimeX;

    public UtilStat()
    {
        ResearchTimeX = 1;
        ResearchTime = 0;
        MaxResearchThread = 1; // for test
        ExpIncreaseXH = 0;
        LPriceReduceXH = 0;
        RPriceReduceXH = 0;
        InitExp = new BigInteger(0);
    }

    public UtilStat(XmlNode xmlNode)
    {
        ResearchTimeX = int.Parse(xmlNode.Attributes["ResearchTimeX"].Value);
        ResearchTime = int.Parse(xmlNode.Attributes["ResearchTime"].Value);
        MaxResearchThread = int.Parse(xmlNode.Attributes["MaxResearchThread"].Value);
        ExpIncreaseXH = int.Parse(xmlNode.Attributes["ExpIncreaseXH"].Value);
        LPriceReduceXH = int.Parse(xmlNode.Attributes["LPriceReduceXH"].Value);
        RPriceReduceXH = int.Parse(xmlNode.Attributes["RPriceReduceXH"].Value);
        InitExp = new BigInteger(xmlNode.Attributes["InitExp"].Value);
    }

    internal void Add(BonusStat bonus)
    {
        switch (bonus.attribute)
        {
            case "ExpIncreaseXH":
                    ExpIncreaseXH += bonus.value;
                    break;
            case "ResearchTime":
                    ResearchTime += bonus.value;
                    break;
            case "ResearchTimeX":
                this.ResearchTimeX *= bonus.value;
                break;
            case "MaxResearchThread":
                    MaxResearchThread += bonus.value;
                    break;
            case "LPriceReduceXH":
                    LPriceReduceXH -= bonus.value;
                    break;
            case "RPriceReduceXH":
                    RPriceReduceXH -= bonus.value;
                    break;
            case "InitExp":
                    InitExp += new BigInteger(bonus.strvalue);
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
            case "ExpIncreaseXH":
                ExpIncreaseXH -= bonus.value;
                break;
            case "ResearchTime":
                ResearchTime -= bonus.value;
                break;
            case "ResearchTimeX":
                this.ResearchTimeX /= bonus.value;
                break;
            case "MaxResearchThread":
                MaxResearchThread -= bonus.value;
                break;
            case "LPriceReduceXH":
                LPriceReduceXH += bonus.value;
                break;
            case "RPriceReduceXH":
                RPriceReduceXH += bonus.value;
                break;
            case "InitExp":
                InitExp -= new BigInteger(bonus.strvalue);
                break;
            default:
                {
                    sexybacklog.Error("noAttribute");
                    break;
                }
        }
    }

}