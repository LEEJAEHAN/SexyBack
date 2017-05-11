using System;
using System.Runtime.Serialization;
using System.Xml;

[Serializable]
internal class ElementalStat// 누적배수
{
    internal int BonusLevel;
    internal BigInteger DpsX;
    internal int DpsIncreaseXH; // 
    internal int CastSpeedXH; //
    internal int SkillRateIncreaseXH;
    internal int SkillDmgIncreaseXH;

    // only use battleScene
    internal int Level;
    internal bool SkillLaunch;

    internal ElementalStat()
    {
        Level = 1;
        SkillLaunch = false;
        BonusLevel = 0;
        DpsX = new BigInteger(1);
        DpsIncreaseXH = 100;
        CastSpeedXH = 100;
        SkillRateIncreaseXH = 100;
        SkillDmgIncreaseXH = 100;
    }
    internal void LoadStat(XmlNode xmlNode)
    {
        Level = 1;
        SkillLaunch = false;
        BonusLevel = int.Parse(xmlNode.Attributes["BonusLevel"].Value);
        DpsX = new BigInteger(xmlNode.Attributes["DpsX"].Value);
        DpsIncreaseXH = int.Parse(xmlNode.Attributes["DpsIncreaseXH"].Value);
        CastSpeedXH = int.Parse(xmlNode.Attributes["CastSpeedXH"].Value);
        SkillRateIncreaseXH = int.Parse(xmlNode.Attributes["SkillRateIncreaseXH"].Value);
        SkillDmgIncreaseXH = int.Parse(xmlNode.Attributes["SkillDmgIncreaseXH"].Value);
    }


    internal void Add(BonusStat bonus)
    {
        switch (bonus.attribute)
        {
            case "Level":
                Level += bonus.value;
                break;
            case "Active":
                break;
            case "ActiveSkill":
                SkillLaunch = true;
                break;
            case "BonusLevel":
                BonusLevel += bonus.value;
                break;
            case "DpsX":
                DpsX *= bonus.value;
                break;
            case "DpsIncreaseXH":
                DpsIncreaseXH += bonus.value;
                break;
            case "CastSpeedXH":
                CastSpeedXH += bonus.value;
                break;
            case "SkillRateIncreaseXH":
                SkillRateIncreaseXH += bonus.value;
                break;
            case "SkillDmgIncreaseXH":
                SkillDmgIncreaseXH += bonus.value;
                break;
            default:
                    sexybacklog.Error("noAttribute");
                    break;
        }
    }
    internal void Remove(BonusStat bonus)
    {
        switch (bonus.attribute)
        {
            case "BonusLevel":
                BonusLevel -= bonus.value;
                break;
            case "DpsX":
                DpsX /= bonus.value;
                break;
            case "DpsIncreaseXH":
                DpsIncreaseXH -= bonus.value;
                break;
            case "CastSpeedXH":
                CastSpeedXH -= bonus.value;
                break;
            case "SkillRateIncreaseXH":
                SkillRateIncreaseXH -= bonus.value;
                break;
            case "SkillDmgIncreaseXH":
                SkillDmgIncreaseXH -= bonus.value;
                break;
            default:
                sexybacklog.Error("noAttribute");
                break;
        }
    }

}

