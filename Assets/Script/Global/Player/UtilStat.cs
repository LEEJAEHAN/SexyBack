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
    internal int ConsumableX;
    public UtilStat()
    {
        ResearchTimeX = 1;
        ResearchTime = 0;
        MaxResearchThread = 1; // for test
        ExpIncreaseXH = 0;
        LPriceReduceXH = 0;
        RPriceReduceXH = 0;
        ConsumableX = 1;
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
                LPriceReduceXH += bonus.value;
                break;
            case Attribute.RPriceReduceXH:
                RPriceReduceXH += bonus.value;
                break;
            case Attribute.ConsumableX:
                ConsumableX *= bonus.value;
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
                LPriceReduceXH -= bonus.value;
                break;
            case Attribute.RPriceReduceXH:
                RPriceReduceXH -= bonus.value;
                break;
            case Attribute.ConsumableX:
                ConsumableX /= bonus.value;
                break;
            default:
                {
                    sexybacklog.Error("noAttribute");
                    break;
                }
        }
    }

}