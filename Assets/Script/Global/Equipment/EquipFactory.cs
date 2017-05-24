using System;
using System.Xml;
using SexyBackRewardScene;
using System.Collections.Generic;

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

    internal static Equipment LotteryEquipment(string mapid, MapRewardData rewardInfo, RewardRank rank)
    {
        int resultGrade = LotteryGrade(rank); // = 로터리;
        // rank랑 reward level이랑 
        string resultEquipID = PopRandomEquipment(mapid, rewardInfo.RewardLevel, resultGrade);//"E01"; // = 로터리(맵드랍레벨, 랭크)

        sexybacklog.Console(resultEquipID);

        var equipment = Singleton<TableLoader>.getInstance().equipmenttable[resultEquipID];
        string resultSkillID = PopRandomSkill(equipment, rewardInfo.RewardLevel); //"ES01";// = 로터리

        sexybacklog.Console(resultSkillID);

        var skill = Singleton<TableLoader>.getInstance().equipskilltable[resultSkillID];
        Equipment newOne = new Equipment(equipment, skill);
        
        if (resultGrade <= 2)
            newOne.grade = resultGrade; // = 로터리

        return newOne;
    }

    private static string PopRandomSkill(EquipmentData item, int rewardLevel)
    {
        if (item.skillID != null)
            return item.skillID;

        List<string> Candidates = new List<string>();
        var sTable = Singleton<TableLoader>.getInstance().equipskilltable;
        foreach (var sData in sTable.Values)
        {
            if (sData.belong == false && sData.dropLevel == rewardLevel)
            {
                Candidates.Add(sData.ID);
            }
        }

        if (Candidates.Count <= 0)
            return "ES00";

        return Candidates[UnityEngine.Random.Range(0, Candidates.Count)];
    }


    private static string PopRandomEquipment(string mapid, int rewardLevel, int resultGrade)
    {
        List<string> Candidates = new List<string>();

        var eTable = Singleton<TableLoader>.getInstance().equipmenttable;
        foreach (var eData in eTable.Values)
        {
            if ((eData.dropMapID == null || eData.dropMapID == mapid ) && eData.droplevel == rewardLevel)
            {
                if( resultGrade >= 3 )
                {
                    if (eData.grade == resultGrade)
                        Candidates.Add(eData.ID);
                }
                else // grade is 0~2
                {
                    if(eData.grade == 0)
                        Candidates.Add(eData.ID);
                }
            }
        }
        if (Candidates.Count <= 0)
            return "E00";

        return Candidates[UnityEngine.Random.Range(0, Candidates.Count)];
    }

    private static int LotteryGrade(RewardRank rank)
    {
        int rand = UnityEngine.Random.Range(0, 100); // 0~99
        switch (rank)
        {
            case RewardRank.SSS:
                if (rand < 4) return 3;
                if (rand < 4 + 28) return 2;
                return 1;
            case RewardRank.SS:
                if (rand < 3) return 3;
                if (rand < 3 + 21) return 2;
                return 1;
            case RewardRank.S:
                if (rand < 2) return 3;
                if (rand < 2 + 14) return 2;
                return 1;
            case RewardRank.A:
                if (rand < 1) return 3;
                if (rand < 1 + 7) return 2;
                if (rand < 1 + 7 + 84) return 1;
                return 0;
            case RewardRank.B:
                if (rand < 70) return 1;
                return 0;
            case RewardRank.C:
                if (rand < 56) return 1;
                return 0;
            case RewardRank.D:
                if (rand < 42) return 1;
                return 0;
            case RewardRank.E:
                if (rand < 28) return 1;
                return 0;
            case RewardRank.F:
                return 0;
        }
        return 0;
    }
}