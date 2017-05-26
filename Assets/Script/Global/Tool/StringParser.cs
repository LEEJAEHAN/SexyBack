using System;
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
            AttackName = "검술";
            SkillName = "강타";
            SkillRateXK = hero.BaseSkillRateXK;
            //SkillRatio = hero.BaseSkillDamageXH;
        }

        switch (bonus.attribute)
        {
            case Attribute.Str:
                return string.Format("힘 {0}", bonus.value);
            case Attribute.Int:
                return string.Format("지능 {0}", bonus.value);
            case Attribute.Luck:
                return string.Format("운 {0}", bonus.value);
            case Attribute.Spd:
                return string.Format("속도 {0}", bonus.value);
            case Attribute.BonusLevel:
                return string.Format("{0} 레벨 +{1}", AttackName, bonus.value);
            case Attribute.DpcX:
                return string.Format("{0} 데미지 {1}배", AttackName, bonus.value);
            case Attribute.DpcIncreaseXH:
                return string.Format("{0} 데미지 {1}%증가", AttackName, bonus.value);
            case Attribute.AttackCapacity:
                return string.Format("최대공격 스택 +{0}", bonus.value);
            case Attribute.AttackSpeedXH:
                return string.Format("{0} 공격속도 {1}%증가", AttackName, bonus.value);
            case Attribute.CriticalRateXH:
                return string.Format("{0} 확률 {1:N1}%증가", SkillName, (double)(SkillRateXK * bonus.value / 1000f) - 0.05f); // 버림
            case Attribute.CriticalDamageXH:
                return string.Format("{0} 데미지 {1}%증가", SkillName, bonus.value);
            case Attribute.MovespeedXH:
                return string.Format("이동속도 {0}%증가", bonus.value);
            case Attribute.DpsX:
                return string.Format("{0} 데미지 {1}배", AttackName, bonus.value);
            case Attribute.DpsIncreaseXH:
                return string.Format("{0} 데미지 {1}%증가", AttackName, bonus.value);
            case Attribute.CastSpeedXH:
                return string.Format("{0} 시전속도 {1}%증가", AttackName, bonus.value);
            case Attribute.SkillRateIncreaseXH:
                return string.Format("{0} 발동확률 {1:N1}%증가", SkillName, (double)(SkillRateXK * bonus.value / 1000f) - 0.05f); // 버림
            case Attribute.SkillDmgIncreaseXH:
                return string.Format("{0} 데미지 {0}%증가", SkillName, bonus.value);
            case Attribute.ExpIncreaseXH:
                return string.Format("경험치 획득량 {0}%증가", bonus.value);
            case Attribute.ResearchTime:
                return string.Format("연구시간 {0}초 감소", bonus.value);
            case Attribute.ResearchTimeX:
                return string.Format("연구시간 {0}배 단축", bonus.value);
            case Attribute.MaxResearchThread:
                return string.Format("동시연구개수 +{0}", bonus.value);
            case Attribute.LPriceReduceXH:
                return string.Format("레벨업비용 {0}% 감소", bonus.value);
            case Attribute.RPriceReduceXH:
                return string.Format("연구비용 {0}% 감소", bonus.value);
            case Attribute.InitExp:
                return string.Format("시작경험치 +{0}", bonus.value);
            case Attribute.RankBonus:
                return string.Format("랭크보너스 +{0}", bonus.value);
        }
        return "Can't Parse Attribute";
    }
}
