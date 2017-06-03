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
            Name = "검술전문가";
            MaxLevel = 4;
            PriceCoef = 1;
            BonusPerLevel = new BonusStat("hero", Attribute.AttackCapacity, 1, null);
            JobBonus = new BonusStat("hero", Attribute.BonusLevel, 1, null); // 하다말음
        }
        else if (type == 2)
        {
            ID = "T02";
            Name = "던전마스터";
            MaxLevel = 5;
            PriceCoef = 1;
            BonusPerLevel = new BonusStat("global", Attribute.RankBonus, 3, null);
            JobBonus = new BonusStat("global", Attribute.BonusEquipment, 1, null); // 하다말음
        }
        else if (type == 3)
        {
            ID = "T03";
            Name = "대도";
            MaxLevel = 5;
            PriceCoef = 1;
            BonusPerLevel = new BonusStat("hero", Attribute.MovespeedXH, 20, null);
            JobBonus = new BonusStat("util", Attribute.BonusConsumable, 1, null);
        }
        else if (type == 4)
        {
            ID = "T04";
            Name = "파이어메이지";
            MaxLevel = 4;
            PriceCoef = 1;
            BonusPerLevel = new BonusStat("fireball", Attribute.InitLevel, 1, null);    // 저장쪽 ㅏㅎ다말음
            JobBonus = new BonusStat("fireball", Attribute.BonusLevel, 3, null);    // 하다말음
        }
    }

}