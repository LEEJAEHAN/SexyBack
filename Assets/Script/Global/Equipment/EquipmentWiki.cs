using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class EquipmentWiki
{

    internal static string EvToString(int evolution)
    {
        if (evolution == 1)
            return "+";
        else if (evolution == 2)
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

    internal static double CalEvolCoef(int evolution)
    {
        if (evolution >= 2)
            return 2f;
        else if (evolution == 1)
            return 1.414213562f;
        else
            return 1f;
    }

    internal static double CalgradeCoef(int grade)
    {
        if (grade == 2)
            return 2.25f;
        else if (grade == 1)
            return 1.5f;
        else
            return 1f;
    }
    internal static double CalExpCoef(int exp, int maxExp)
    {
        return 1 + (double)exp / (double)maxExp;
    }

    public static int CalMaxExp(int grade, int evolution)
    {
        return 100 * (grade + 1) * (evolution + 1);
    }

    internal static string CalExpPercent(int nextExp, int maxExp)
    {
        if (nextExp == 0)
            return "0%";
        if (nextExp == maxExp)
            return "Max";
        else
            return (nextExp * 100 / maxExp).ToString() + "%";
    }


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
