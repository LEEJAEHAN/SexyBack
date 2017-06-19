using System;
using System.Xml;
using SexyBackRewardScene;
using System.Collections.Generic;

internal static class EquipFactory
{

    //public static Equipment CraftEquipment(string equipID, string skillID)
    //{
    //    Equipment newOne = new Equipment(Singleton<TableLoader>.getInstance().equipmenttable[equipID],
    //        Singleton<TableLoader>.getInstance().equipskilltable[skillID]);
    //    return newOne;
    //}

    public static Equipment LoadEquipment(XmlNode equipNode)
    {
        string tableID = equipNode.Attributes["id"].Value;
        string skillID = equipNode.Attributes["skillid"].Value;
        int level = int.Parse(equipNode.Attributes["level"].Value);
        //int grade = int.Parse(equipNode.Attributes["grade"].Value);
        Equipment e = new Equipment(Singleton<TableLoader>.getInstance().equipmenttable[tableID],
            Singleton<TableLoader>.getInstance().equipskilltable[skillID], level);
        e.exp = double.Parse(equipNode.Attributes["exp"].Value);
        e.limit = int.Parse(equipNode.Attributes["limit"].Value);
        e.skillLevel = int.Parse(equipNode.Attributes["skillLevel"].Value);
        e.isLock = bool.Parse(equipNode.Attributes["isLock"].Value);
        return e;
    }

    internal static Equipment LotteryEquipment(MapRewardData rewardInfo, RewardRank rank, int level)
    {
        int resultGrade = LotteryGrade(rank); // = 로터리;

        // TODO : 랭크에따라 리워드레벨이나 grade변화를 줘야한다.
        //int resultLevel = rewardInfo.level; 
        string resultEquipID = PopRandomEquipment(rewardInfo, resultGrade, level);//"E01"; // = 로터리(맵드랍레벨, 랭크)

        EquipmentData equipment = Singleton<TableLoader>.getInstance().equipmenttable[resultEquipID];
        string resultSkillID = PopRandomSkill(equipment, level); //"ES01";// = 로터리

        sexybacklog.Console("정해진 등급 : " + resultGrade);
        sexybacklog.Console("정해진 아이템아이디 : " + resultEquipID);

        var skill = Singleton<TableLoader>.getInstance().equipskilltable[resultSkillID];

        int itemlevel = (level / 10) * 10; // Math.Max(50, level); // 10단위로짜르며, 최소 50레벨이후부터 환생할수있게한다.
        Equipment newOne = new Equipment(equipment, skill, itemlevel);
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
            if (sData.belong == false && (sData.dropStart <= rewardLevel && rewardLevel <= sData.dropEnd))
                Candidates.Add(sData.ID);
        }

        if (Candidates.Count <= 0)
            return "ES00";

        return Candidates[UnityEngine.Random.Range(0, Candidates.Count)];
    }


    private static string PopRandomEquipment(MapRewardData rewardData, int resultGrade, int level)
    {
        List<string> Candidates = new List<string>();

        var eTable = Singleton<TableLoader>.getInstance().equipmenttable;
        foreach (var eData in eTable.Values)
        {
            if (resultGrade != eData.grade)
                continue;

            if (eData.belong)   // 특정던젼 귀속템
            {
                if (rewardData.FixCandidates.Contains(eData.ID))
                    Candidates.Add(eData.ID);
            }
            else if (eData.dropStart <= level && level <= eData.dropEnd)
                Candidates.Add(eData.ID);
        }

        if (Candidates.Count <= 0)      // 해당등급의 아이템이 후보목록에 아예없을땐 등급을 낮춰 재귀뽑기를한다.
        {
            if (resultGrade >= 0)
            {
                sexybacklog.Console("재귀뽑기를합니다.");
                return PopRandomEquipment(rewardData, resultGrade-1, level);
            }
            else
                return "E00";           // 정말아무것도없을땐 더미.
        }

        return Candidates[UnityEngine.Random.Range(0, Candidates.Count)];
    }

    private static int LotteryGrade(RewardRank rank)
    {
        int rand = UnityEngine.Random.Range(0, 85); // 1+12+24+48,  0~84 
        if (rand < 1) return 3;
        if (rand < 1 + 12) return 2;
        if (rand < 1 + 12 + 24) return 1;
        return 0;

        //rand = UnityEngine.Random.Range(0, 144); // 0~99 0~7
        //switch (rank)
        //{
        //    case RewardRank.SSS:
        //        if (rand < 48) return 2;
        //        if (rand < 48 + 72) return 1;
        //        break;
        //    case RewardRank.SS:
        //        if (rand < 24) return 2;
        //        if (rand < 24 + 36) return 1;
        //        break;
        //    case RewardRank.S:
        //        if (rand < 16) return 2;
        //        if (rand < 16 + 24) return 1;
        //        break;
        //    case RewardRank.A:
        //        if (rand < 12 + 18) return 1;
        //        break;
        //    case RewardRank.B:
        //        if (rand < 8 + 12) return 1;
        //        break;
        //    case RewardRank.C:
        //        if (rand < 6 + 9) return 1;
        //        break;
        //    case RewardRank.D:
        //        if (rand < 4 + 6) return 1;
        //        break;
        //    case RewardRank.E:
        //        if (rand < 2 + 3) return 1;
        //        break;
        //    case RewardRank.F:
        //        break;
        //    default:
        //        break;
        //}
        //return 0;
    }
    

}