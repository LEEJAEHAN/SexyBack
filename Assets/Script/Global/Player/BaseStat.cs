using System;
using System.Runtime.Serialization;
using System.Xml;

[Serializable]
public class BaseStat : ICloneable
{
    internal int Str;
    internal int Int;
    internal int Spd;
    internal int Luck;

    public BaseStat(int str = 0, int intel = 0, int spd = 0, int luc = 0)
    {
        Str = str;
        Int = intel;
        Spd = spd;
        Luck = luc;
    }

    public string ToStringBox()
    {
        return string.Format("{0}\n{1}\n{2}\n{3}", Str, Int, Spd, Luck);
    }
    public override string ToString()
    {
        string temp = "";
        if (Str > 0)
            temp += ("힘 " + Str.ToString() + "\n");
        if (Int > 0)
            temp += ("지능 " + Int.ToString() + "\n");
        if (Spd > 0)
            temp += ("속도 " + Spd.ToString() + "\n");
        if (Luck > 0)
            temp += ("운 " + Luck.ToString() + "\n");

        if (temp.Length > 1)
            return temp.Substring(0, temp.Length - 1);
        else
            return temp;
    }
    public object Clone()
    {
        return new BaseStat(this.Str, this.Int, this.Spd, this.Luck);
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
            case Attribute.Str:
                Str += bonus.value;
                break;
            case Attribute.Int:
                Int += bonus.value;
                break;
            case Attribute.Luck:
                Luck += bonus.value;
                break;
            case Attribute.Spd:
                Spd += bonus.value;
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
            case Attribute.Str:
                Str -= bonus.value;
                break;
            case Attribute.Int:
                Int -= bonus.value;
                break;
            case Attribute.Luck:
                Luck -= bonus.value;
                break;
            case Attribute.Spd:
                Spd -= bonus.value;
                break;
            default:
                {
                    sexybacklog.Error("noAttribute");
                    break;
                }
        }
    }

    public static BaseStat operator *(BaseStat leftSide, double scalar)
    {
        return new BaseStat((int)(leftSide.Str * scalar),
            (int)(leftSide.Int * scalar),
            (int)(leftSide.Spd * scalar),
            (int)(leftSide.Luck * scalar));
    }
    public static BaseStat operator +(BaseStat leftSide, BaseStat rightSide)
    {
        return new BaseStat((int)(leftSide.Str + rightSide.Str),
            (int)(leftSide.Int + rightSide.Int),
            (int)(leftSide.Spd + rightSide.Spd),
            (int)(leftSide.Luck + rightSide.Luck));
    }
    public static BaseStat operator -(BaseStat leftSide, BaseStat rightSide)
    {
        return new BaseStat((int)(leftSide.Str - rightSide.Str),
            (int)(leftSide.Int - rightSide.Int),
            (int)(leftSide.Spd - rightSide.Spd),
            (int)(leftSide.Luck - rightSide.Luck));
    }
}