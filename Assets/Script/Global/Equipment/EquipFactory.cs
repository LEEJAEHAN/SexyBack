using System;
using System.Xml;
using SexyBackRewardScene;

internal static class EquipFactory
{
    public static Equipment CraftEquipment(string equipID, string skillID)
    {
        Equipment newOne = new Equipment(Singleton<TableLoader>.getInstance().equipmenttable[equipID],
            Singleton<TableLoader>.getInstance().equipskilltable[skillID]);
        return newOne;
    }
    public static Equipment LoadEquipment(XmlNode equipNode)
    {
        string tableID = equipNode.Attributes["id"].Value;
        string skillID = equipNode.Attributes["skillid"].Value;
        Equipment e = new Equipment(Singleton<TableLoader>.getInstance().equipmenttable[tableID],
            Singleton<TableLoader>.getInstance().equipskilltable[skillID]);

        e.grade = int.Parse(equipNode.Attributes["grade"].Value);
        e.exp = int.Parse(equipNode.Attributes["exp"].Value);
        e.evolution = int.Parse(equipNode.Attributes["evolution"].Value);
        e.skillLevel = int.Parse(equipNode.Attributes["skillLevel"].Value);
        e.isLock = bool.Parse(equipNode.Attributes["isLock"].Value);
        return e;
    }

    internal static Equipment LotteryEquipment(MapRewardData rewardInfo, RewardRank rank)
    {
        // rank랑 reward level이랑 
        string resultBaseItemID = "E01"; // = 로터리(맵드랍레벨, 랭크)
        string resultSkillID = "ES01";// = 로터리
        int resultGrade = 2; // = 로터리;

        Equipment newOne = new Equipment(Singleton<TableLoader>.getInstance().equipmenttable[resultBaseItemID],
            Singleton<TableLoader>.getInstance().equipskilltable[resultSkillID]);
        newOne.grade = resultGrade; // = 로터리


        return newOne;
    }
}