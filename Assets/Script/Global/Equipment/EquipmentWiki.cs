using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    internal static string SkillStatToString(List<BonusStat> skillStat)
    {
        string temp = "";
        foreach (BonusStat a in skillStat)
        {
            temp += (a.description.Replace("$d", a.value.ToString()) + "\n");
        }
        return temp.Substring(0, temp.Length - 1);
    }

    internal static double CalEvolCoef(int evolution)
    {
        if (evolution >= 2)
            return 2f;
        else if (evolution == 1)
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
}
