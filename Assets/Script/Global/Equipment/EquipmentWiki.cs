using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class EquipmentWiki
{

    internal static string PrintLimit(int limit)
    {
        if (limit == 1)
            return "+";
        else if (limit == 2)
            return "++";
        else
            return "";
    }

    internal static string AttributeBox(List<BonusStat> skillStat)
    {
        string temp = "";
        foreach (BonusStat attr in skillStat)
            temp += StringParser.GetAttributeString(attr) + "\n";

        if (temp.Length > 1)
            return temp.Substring(0, temp.Length - 1);
        else
            return temp;
    }

    internal static double CalLimitCoef(int limit)
    {
        return 1f;
        //if (limit >= 2)
        //    return 4f;
        //else if (limit == 1)
        //    return 2f;
        //else
        //    return 1f;
    }

    internal static double CalgradeCoef(int grade)
    {
        if (grade >= 2)
            return 3f;
        else if (grade == 1)
            return 2f;
        else
            return 1f;

        //if (grade == 2)
        //    return 2.25f;
        //else if (grade == 1)
        //    return 1.5f;
        //else
        //    return 1f;
    }
    internal static double CalExpCoef(int grade, double exp)
    {
        return 1 + (exp / Equipment.MaxExp) * 3; // x1 ~ x4 ( 최대 12배수를맞추기위해 )
        //if (grade == 3 )
        //    return 1 + (exp / Equipment.MaxExp) * 7; // x1 ~ x8 ( 유니크는 unlimit가없기떄문에)
        //else
        //    return 1 + (exp / Equipment.MaxExp); // x1~x2
        // 원래는 x2까지였음.
    }

    //internal static string CalExpPercent(double nextExp)
    //{
    //    if (nextExp == 0)
    //        return "0%";
    //    if (nextExp >= 100)
    //        return "Max";
    //    else
    //        return (nextExp * 100 / maxExp).ToString() + "%";
    //}


    internal static string LockToString(bool isLock)
    {
        if(isLock)
            return "잠금해제";
        else
            return "잠금";
    }

    internal static Color CalNameColor(int grade)
    {
        if (grade == 1)
            return new Color(0, 0.5f, 1);
        else if (grade == 2)
            return new Color(1, 1, 0);
        else if (grade == 3)
            return new Color(0.5f, 0, 1);
        else
            return Color.white;
    }


}
