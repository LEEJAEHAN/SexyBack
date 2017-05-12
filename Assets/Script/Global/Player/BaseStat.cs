using System;
using System.Runtime.Serialization;
using System.Xml;

[Serializable]
public class BaseStat : ICloneable
{
    public int Str;
    public int Int;
    public int Spd;
    public int Luck;

    public BaseStat(int str = 0, int intel = 0, int spd = 0, int luc = 0)
    {
        Str = str;
        Int = intel;
        Spd = spd;
        Luck = luc;
    }

    public BaseStat(XmlNode xmlNode)
    {
        Str = int.Parse(xmlNode.Attributes["Str"].Value);
        Int = int.Parse(xmlNode.Attributes["Int"].Value);
        Spd = int.Parse(xmlNode.Attributes["Spd"].Value);
        Luck = int.Parse(xmlNode.Attributes["Luck"].Value);
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

        return temp.Substring(0, temp.Length - 1);
    }
    public object Clone()
    {
        return new BaseStat(this.Str, this.Int, this.Spd, this.Luck);
    }

    public static BaseStat operator *(BaseStat leftSide, double scalar)
    {
        return new BaseStat((int)(leftSide.Str * scalar),
            (int)(leftSide.Int * scalar),
            (int)(leftSide.Spd * scalar),
            (int)(leftSide.Luck * scalar));
    }
}