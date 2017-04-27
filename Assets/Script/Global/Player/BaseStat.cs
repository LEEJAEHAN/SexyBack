using System;

public class BaseStat : ICloneable
{
    public int Str;
    public int Int;
    public int Spd;
    public int Luck;

    public BaseStat()
    {
        Str = 0;
        Int = 0;
        Spd = 0;
        Luck = 0;
    }
    public BaseStat(int str, int intel, int spd, int luc)
    {
        Str = str;
        Int = intel;
        Spd = spd;
        Luck = luc;
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