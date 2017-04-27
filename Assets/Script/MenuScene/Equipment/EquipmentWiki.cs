using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class EquipmentWiki
{




    public static int getMaxExp(int grade, int evolution)
    {

        return 100;
    }

    internal static string ToString(int evolution)
    {
        if (evolution == 1)
            return "+";
        else if (evolution == 2)
            return "++";
        else
            return "";
    }
}
