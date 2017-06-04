using System;
using System.Xml;

internal class GlobalStat
{
    internal int RankBonus;
    internal int InitExpCoef;
    internal int BonusEquipment;
    internal int ReputationXH;
    public GlobalStat()
    {
        RankBonus = 0;
        InitExpCoef = 0;
        BonusEquipment = 0;
        ReputationXH = 0;
    }

    //public int GetInitExp(int temp)
    //{
    //    return (int)Math.Pow(500, temp);
    //    if (temp < 1)
    //        return 0;
    //    else if (temp == 1)
    //        return 500;
    //    else if (temp == 2)
    //        return 1000;
    //    else if (temp == 3)
    //        return 2000;
    //    else if (temp == 4)
    //        return 4000;
    //    else
    //        return 10000;
    //}


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
            case Attribute.InitExpCoef:
                InitExpCoef += bonus.value;
                break;
            case Attribute.RankBonus:
                RankBonus += bonus.value;
                break;
            case Attribute.BonusEquipment:
                BonusEquipment += bonus.value;
                break;
            case Attribute.ReputationXH:
                ReputationXH += bonus.value;
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
            case Attribute.InitExpCoef:
                InitExpCoef -= bonus.value;
                break;
            case Attribute.RankBonus:
                RankBonus -= bonus.value;
                break;
            case Attribute.BonusEquipment:
                BonusEquipment -= bonus.value;
                break;
            case Attribute.ReputationXH:
                ReputationXH -= bonus.value;
                break;
            default:
                {
                    sexybacklog.Error("noAttribute");
                    break;
                }
        }
    }

}
