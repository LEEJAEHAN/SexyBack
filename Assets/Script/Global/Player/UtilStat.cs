using System;
using System.Runtime.Serialization;
using System.Xml;

[Serializable]
internal class UtilStat
{
    internal int ResearchTime; 
    internal int MaxResearchThread;
    internal int ExpIncreaseXH;
    internal int LevelUpPriceXH;
    internal int ResearchPriceXH;
    internal BigInteger InitExp;

    // only use battleScene
    internal int ResearchTimeX;

    public UtilStat()
    {
        ResearchTimeX = 1;
        ResearchTime = 0;
        MaxResearchThread = 5; // for test
        ExpIncreaseXH = 100;
        LevelUpPriceXH = 100;
        ResearchPriceXH = 100;
        InitExp = new BigInteger(0);
    }

    public UtilStat(XmlNode xmlNode)
    {
        ResearchTimeX = int.Parse(xmlNode.Attributes["ResearchTimeX"].Value);
        ResearchTime = int.Parse(xmlNode.Attributes["ResearchTime"].Value);
        MaxResearchThread = int.Parse(xmlNode.Attributes["MaxResearchThread"].Value);
        ExpIncreaseXH = int.Parse(xmlNode.Attributes["ExpIncreaseXH"].Value);
        LevelUpPriceXH = int.Parse(xmlNode.Attributes["LevelUpPriceXH"].Value);
        ResearchPriceXH = int.Parse(xmlNode.Attributes["ResearchPriceXH"].Value);
        InitExp = new BigInteger(xmlNode.Attributes["ResearchPriceXH"].Value);
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
            case "LevelUpPriceXH":
                    LevelUpPriceXH -= bonus.value;
                    break;
            case "ResearchPriceXH":
                    ResearchPriceXH -= bonus.value;
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
            case "LevelUpPriceXH":
                LevelUpPriceXH += bonus.value;
                break;
            case "ResearchPriceXH":
                ResearchPriceXH += bonus.value;
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