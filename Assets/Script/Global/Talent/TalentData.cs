using System.Collections.Generic;

internal class TalentData
{
    internal string ID;
    internal string Name;
    internal int MaxLevel;
    internal int PriceCoef;
    internal BonusStat BonusPerLevel;
    internal BonusStat JobBonus;

    public TalentData(int type)
    {
        if (type == 1)
        {
            ID = "T01";
            Name = "모험가";
            MaxLevel = 4;
            PriceCoef = 1;
            BonusPerLevel = new BonusStat("global", Attribute.InitExpCoef, 1);
            JobBonus = new BonusStat("global", Attribute.ReputationXH, 100);
        }
        else if (type == 2)
        {
            ID = "T02";
            Name = "검술전문가";
            MaxLevel = 4;
            PriceCoef = 1;
            BonusPerLevel = new BonusStat("hero", Attribute.AttackCapacity, 1);
            JobBonus = new BonusStat("hero", Attribute.BonusLevel, 1); // 하다말음
        }
        else if (type == 3)
        {
            ID = "T03";
            Name = "파이어메이지";
            MaxLevel = 4;
            PriceCoef = 1;
            BonusPerLevel = new BonusStat("fireball", Attribute.InitLevel, 1);
            JobBonus = new BonusStat("fireball", Attribute.BonusLevel, 3);
        }
        else if (type == 4)
        {
            ID = "T04";
            Name = "아이스메이지";
            MaxLevel = 4;
            PriceCoef = 1;
            BonusPerLevel = new BonusStat("iceblock", Attribute.InitLevel, 1);
            JobBonus = new BonusStat("iceblock", Attribute.BonusLevel, 3);
        }
        else if (type == 5)
        {
            ID = "T05";
            Name = "돌팔매왕";
            MaxLevel = 4;
            PriceCoef = 1;
            BonusPerLevel = new BonusStat("rock", Attribute.InitLevel, 1);
            JobBonus = new BonusStat("rock", Attribute.BonusLevel, 3);
        }
        else if (type == 6)
        {
            ID = "T06";
            Name = "대도";
            MaxLevel = 5;
            PriceCoef = 1;
            BonusPerLevel = new BonusStat("hero", Attribute.MovespeedXH, 20);
            JobBonus = new BonusStat("util", Attribute.BonusConsumable, 1);
        }
        else if (type == 7)
        {
            ID = "T07";
            Name = "학자";
            MaxLevel = 2;
            PriceCoef = 2;
            BonusPerLevel = new BonusStat("util", Attribute.MaxResearchThread, 1);
            JobBonus = new BonusStat("util", Attribute.ResearchTimeX, 2);
        }
        else if (type == 8)
        {
            ID = "T08";
            Name = "던전마스터";
            MaxLevel = 5;
            PriceCoef = 1;
            BonusPerLevel = new BonusStat("global", Attribute.RankBonus, 3);
            JobBonus = new BonusStat("global", Attribute.BonusEquipment, 1); // 확인해봐야함.
        }
    }

}