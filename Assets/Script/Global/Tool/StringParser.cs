﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class StringParser
{
    internal static string GetTimeText(int second)
    {
        int remain = second;
        int h = remain / 3600;
        remain -= h * 3600;
        int m = remain / 60;
        remain -= m * 60;
        int s = remain;

        return String.Format("{0}시간 {1}분 {2}초", h, m, s);
    }

    public static string GetStackText(int stack)
    {
        if (stack <= 0)
            return "";
        else
            return "x" + stack.ToString();
    }

    internal static string ReplaceString(string text, string arg1)
    {
        string temp = text.Replace("$1$", arg1);
        return temp;
    }
    internal static string ReplaceString(string text, string arg1, string arg2)
    {
        string temp = text.Replace("$1$", arg1);
        temp = temp.Replace("$2$", arg2);
        return temp;
    }


    internal static string GetAttributeString(BonusStat bonus)
    {
        if (bonus == null)
            return "";

        string AttackName = null;
        string SkillName = null;
        int SkillRateXK = 0;
        //int SkillRatio = 0;

        var eTable = Singleton<TableLoader>.getInstance().elementaltable;
        if (eTable.ContainsKey(bonus.targetID))
        {
            var elemental = eTable[bonus.targetID];
            AttackName = elemental.Name;
            SkillName = elemental.SkillName;
            SkillRateXK = elemental.BaseSkillRateXK;
            //SkillRatio = elemental.BaseSkillDamageXH;
        }
        else if (bonus.targetID == "hero")
        {
            var hero = Singleton<TableLoader>.getInstance().herotable;
            AttackName = hero.Name;
            SkillName = hero.SkillName;
            SkillRateXK = hero.BaseSkillRateXK;
            //SkillRatio = hero.BaseSkillDamageXH;
        }
        else if ( bonus.targetID == "elementals")
            AttackName = "모든마법";

        switch (bonus.attribute)
        {
            case Attribute.Str:
                return string.Format("힘 +{0}", bonus.value);
            case Attribute.Int:
                return string.Format("지능 +{0}", bonus.value);
            case Attribute.Luck:
                return string.Format("운 +{0}", bonus.value);
            case Attribute.Spd:
                return string.Format("속도 +{0}", bonus.value);
            case Attribute.BonusLevel:
                return string.Format("{0} 레벨 +{1}", AttackName, bonus.value);
            case Attribute.DpcX:
                return string.Format("{0} 피해량 {1}배", AttackName, bonus.value);
            case Attribute.DpcIncreaseXH:
                return string.Format("{0} 피해량 +{1}%", AttackName, bonus.value);
            case Attribute.AttackCapacity:
                return string.Format("최대공격 스택 +{0}", bonus.value);
            case Attribute.AttackSpeedXH:
                return string.Format("{0} 공격속도 +{1}%", AttackName, bonus.value);
            case Attribute.CriticalRateXH:
                return string.Format("{0} 확률 +{1:N1}%", SkillName, (double)(SkillRateXK * bonus.value / 1000f)); // 버림
            case Attribute.CriticalDamageXH:
                return string.Format("{0} 피해량 +{1}%", SkillName, bonus.value);
            case Attribute.MovespeedXH:
                return string.Format("이동속도 +{0}%", bonus.value);
            case Attribute.DpsX:
                return string.Format("{0} 피해량 {1}배", AttackName, bonus.value);
            case Attribute.DpsIncreaseXH:
                return string.Format("{0} 피해량 +{1}%", AttackName, bonus.value);
            case Attribute.CastSpeedXH:
                return string.Format("{0} 시전속도 +{1}%", AttackName, bonus.value);
            case Attribute.SkillRateIncreaseXH:
                return string.Format("{0} 발동확률 +{1:N1}%", SkillName, (double)(SkillRateXK * bonus.value / 1000f)); // 버림
            case Attribute.SkillDmgIncreaseXH:
                return string.Format("{0} 피해량 +{1}%", SkillName, bonus.value);
            case Attribute.ExpIncreaseXH:
                return string.Format("경험치 획득량 +{0}%", bonus.value);
            case Attribute.ResearchTime:
                return string.Format("연구시간 -{0}초", bonus.value);
            case Attribute.ResearchTimeX:
                return string.Format("연구속도 {0}배", bonus.value);
            case Attribute.MaxResearchThread:
                return string.Format("동시연구개수 +{0}", bonus.value);
            case Attribute.LPriceReduceXH:
                return string.Format("레벨업비용 -{0}%", bonus.value);
            case Attribute.RPriceReduceXH:
                return string.Format("연구비용 -{0}%", bonus.value);
            case Attribute.InitExpCoef:
                return string.Format("시작경험치 {0}배", bonus.value);
            case Attribute.RankBonus:
                return string.Format("랭크보너스 +{0}", bonus.value);
            case Attribute.BonusEquipment:
                return string.Format("장비보상 +{0}", bonus.value);
            case Attribute.ConsumableX:
                return string.Format("소모품드랍 {0}배", bonus.value);
            case Attribute.InitLevel:
                return string.Format("{0} 초기레벨 +{1}", AttackName, bonus.value);
            case Attribute.ReputationXH:
                return string.Format("명성획득 +{0}%", bonus.value);
            case Attribute.BaseDensityAdd:
                    return string.Format((bonus.dvalue >= 10)? "{0} 피해량 {1:N0}배 증가" : "{0} 피해량 {1:N2}배 증가", AttackName, bonus.dvalue);
        }
        return "Can't Parse Attribute";
    }

    internal static string GetRequireText(string targetID, int requireLevel)
    {
        string targetName = null;

        if (Singleton<TableLoader>.getInstance().leveluptable.ContainsKey(targetID))
            targetName = Singleton<TableLoader>.getInstance().leveluptable[targetID].OwnerName;

        if (targetName == null)
            return "";

        return string.Format("{0} {1}레벨부터 연구가능합니다.", targetName, requireLevel);
    }
}
