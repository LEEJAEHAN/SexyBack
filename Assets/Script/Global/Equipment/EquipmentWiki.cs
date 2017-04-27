using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class EquipmentWiki
{




    public static int GetMaxExp(int grade, int evolution)
    {
        return 100;
    }

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
            temp += (a.description.Replace("%d", a.value.ToString()) + "\n");
        }
        return temp.Substring(0, temp.Length - 1);
    }

}
