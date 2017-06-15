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
        e.evolution = int.Parse(equipNode.Attributes["evolution"].Value);
        e.skillLevel = int.Parse(equipNode.Attributes["skillLevel"].Value);
        e.isLock = bool.Parse(equipNode.Attributes["isLock"].Value);
        return e;
    }

    internal static Equipment LotteryEquipment(string mapid, MapRewardData rewardInfo, RewardRank rank)
    {
        int resultGrade = LotteryGrade(rank); // = 로터리;

        // TODO : 랭크에따라 리워드레벨이나 grade변화를 줘야한다.
        //int resultLevel = rewardInfo.level; 
        string resultEquipID = PopRandomEquipment(mapid, rewardInfo, resultGrade);//"E01"; // = 로터리(맵드랍레벨, 랭크)

        EquipmentData equipment = Singleton<TableLoader>.getInstance().equipmenttable[resultEquipID];
        string resultSkillID = PopRandomSkill(equipment, rewardInfo.Level); //"ES01";// = 로터리

        sexybacklog.Console("정해진 등급 : " + resultGrade);
        sexybacklog.Console("정해진 아이템아이디 : " + resultEquipID);

        var skill = Singleton<TableLoader>.getInstance().equipskilltable[resultSkillID];

        int itemlevel = Math.Max(50, rewardInfo.Level); // 튜토리얼 스테이지는 40이하의 드랍레벨이기때문에 40으로상향.
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


    private static string PopRandomEquipment(string mapid, MapRewardData rewardData, int resultGrade)
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
            else if (eData.dropStart <= rewardData.Level && rewardData.Level <= eData.dropEnd)
                Candidates.Add(eData.ID);
        }

        if (Candidates.Count <= 0)      // 해당등급의 아이템이 후보목록에 아예없을땐 등급을 낮춰 재귀뽑기를한다.
        {
            if (resultGrade >= 0)
            {
                sexybacklog.Console("재귀뽑기를합니다.");
                return PopRandomEquipment(mapid, rewardData, resultGrade-1);
            }
            else
                return "E00";           // 정말아무것도없을땐 더미.
        }

        return Candidates[UnityEngine.Random.Range(0, Candidates.Count)];
    }

    private static int LotteryGrade(RewardRank rank)
    {
        int rand = UnityEngine.Random.Range(0, 144); // 0~99 0~7
        switch (rank)
        {
            case RewardRank.SSS:
                if (rand < 48) return 2;
                if (rand < 48 + 72) return 1;
                break;
            case RewardRank.SS:
                if (rand < 24) return 2;
                if (rand < 24 + 36) return 1;
                break;
            case RewardRank.S:
                if (rand < 16) return 2;
                if (rand < 16 + 24) return 1;
                break;
            case RewardRank.A:
                if (rand < 12 + 18) return 1;
                break;
            case RewardRank.B:
                if (rand < 8 + 12) return 1;
                break;
            case RewardRank.C:
                if (rand < 6 + 9) return 1;
                break;
            case RewardRank.D:
                if (rand < 4 + 6) return 1;
                break;
            case RewardRank.E:
                if (rand < 2 + 3) return 1;
                break;
            case RewardRank.F:
                break;
            default:
                break;
        }
        return 0;
    }


    //private static int LotteryGrade(RewardRank rank)
    //{
    //    int rand = UnityEngine.Random.Range(0, 100); // 0~99
    //    switch (rank)
    //    {
    //        case RewardRank.SSS:
    //            if (rand < 4) return 3;
    //            if (rand < 4 + 28) return 2;
    //            return 1;
    //        case RewardRank.SS:
    //            if (rand < 3) return 3;
    //            if (rand < 3 + 21) return 2;
    //            return 1;
    //        case RewardRank.S:
    //            if (rand < 2) return 3;
    //            if (rand < 2 + 14) return 2;
    //            return 1;
    //        case RewardRank.A:
    //            if (rand < 1) return 3;
    //            if (rand < 1 + 7) return 2;
    //            if (rand < 1 + 7 + 84) return 1;
    //            return 0;
    //        case RewardRank.B:
    //            if (rand < 70) return 1;
    //            return 0;
    //        case RewardRank.C:
    //            if (rand < 56) return 1;
    //            return 0;
    //        case RewardRank.D:
    //            if (rand < 42) return 1;
    //            return 0;
    //        case RewardRank.E:
    //            if (rand < 28) return 1;
    //            return 0;
    //        case RewardRank.F:
    //            return 0;
    //    }
    //    return 0;
    //}


}