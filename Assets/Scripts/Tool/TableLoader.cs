using System;
using System.Collections.Generic;
namespace SexyBackPlayScene
{
    internal class TableLoader
    {
        public List<MonsterData> monstertablelist = new List<MonsterData>();

        public Dictionary<string, HeroData> herotable = new Dictionary<string, HeroData>();
        public Dictionary<string, MonsterData> monstertable = new Dictionary<string, MonsterData>();
        public Dictionary<string, ElementalData> elementaltable = new Dictionary<string, ElementalData>();
        public Dictionary<string, LevelUpItemData> leveluptable = new Dictionary<string, LevelUpItemData>();

        public List<ResearchData> researchtable = new List<ResearchData>();


        public Dictionary<string, GameModeData> gamemodetable = new Dictionary<string, GameModeData>();

        internal void LoadAll()
        {
            LoadHeroData();
            LoadMonsterData();
            LoadElementData();

            LoadStageData();
            LoadLevelUpData();
            LoadResearchData();

        }

        private void LoadResearchData()
        {
            ResearchData item = new ResearchData();
            item.ID = "R01";
            item.price = new BigIntExpression(10, "zero");
            item.pot = new BigIntExpression(1, "zero");
            item.time = 30;

            item.requireID = "hero";
            item.requeireLevel = 0;

            item.IconName = "SexyBackIcon_FireElemental";
            item.InfoName = "파이어볼 배우기";
            Bonus b = new Bonus();
            b.targetID = "hero";
            b.attribute = "fireball";
            b.value = 1;

            item.bonus.Add(b);

            researchtable.Add(item);
        }

        private void LoadLevelUpData()
        {
            LevelUpItemData heroAttack = new LevelUpItemData("hero", "일반공격", "SexyBackIcon_SWORD2");
            LevelUpItemData item1 = new LevelUpItemData("fireball", "파이어볼", "SexyBackIcon_FireElemental");
            LevelUpItemData item2 = new LevelUpItemData("waterball", "물폭탄", "SexyBackIcon_WaterElemental");
            LevelUpItemData item3 = new LevelUpItemData("rock", "짱돌", "SexyBackIcon_RockElemental");
            LevelUpItemData item4 = new LevelUpItemData("electricball", "지지직", "SexyBackIcon_ElectricElemental");
            LevelUpItemData item5 = new LevelUpItemData("snowball", "눈덩이", "SexyBackIcon_SnowElemental");
            LevelUpItemData item6 = new LevelUpItemData("earthball", "똥", "SexyBackIcon_EarthElemental");
            LevelUpItemData item7 = new LevelUpItemData("airball", "바람바람", "SexyBackIcon_AirElemental");
            LevelUpItemData item8 = new LevelUpItemData("iceblock", "각얼음", "SexyBackIcon_IceElemental");
            LevelUpItemData item9 = new LevelUpItemData("magmaball", "메테오", "SexyBackIcon_MagmaElemental");
            leveluptable.Add(heroAttack.OwnerID, heroAttack);
            leveluptable.Add(item1.OwnerID, item1);
            leveluptable.Add(item2.OwnerID, item2);
            leveluptable.Add(item3.OwnerID, item3);
            leveluptable.Add(item4.OwnerID, item4);
            leveluptable.Add(item5.OwnerID, item5);
            leveluptable.Add(item6.OwnerID, item6);
            leveluptable.Add(item7.OwnerID, item7);
            leveluptable.Add(item8.OwnerID, item8);
            leveluptable.Add(item9.OwnerID, item9);
        }

        private void LoadStageData()
        {
            GameModeData data1 = new GameModeData("TestStage", 20, 0);
            gamemodetable.Add(data1.ID, data1);

        }

        private void LoadElementData()
        {
            ElementalData data1 = new ElementalData("fireball", "Fire Ball", 5200, 1, 100);
            ElementalData data2 = new ElementalData("waterball", "Water Ball", 5300, 5, 500);
            ElementalData data3 = new ElementalData("rock", "Rock", 5500, 25, 4000);
            ElementalData data4 = new ElementalData("electricball", "Plasma", 5700, 150, 37500);
            ElementalData data5 = new ElementalData("snowball", "Snow Ball", 6100, 1150, 450000);
            ElementalData data6 = new ElementalData("earthball", "Mud ball", 6300, 10000, 7250000);
            ElementalData data7 = new ElementalData("airball", "Wind Strike", 6700, 100000, 150000000);
            ElementalData data8 = new ElementalData("iceblock", "Ice Cube", 6900, 1500000, 4000000000);
            ElementalData data9 = new ElementalData("magmaball", "Meteor", 7300, 27500000, 140000000000);

            elementaltable.Add(data1.ID, data1);
            elementaltable.Add(data2.ID, data2);
            elementaltable.Add(data3.ID, data3);
            elementaltable.Add(data4.ID, data4);
            elementaltable.Add(data5.ID, data5);
            elementaltable.Add(data6.ID, data6);
            elementaltable.Add(data7.ID, data7);
            elementaltable.Add(data8.ID, data8);
            elementaltable.Add(data9.ID, data9);
        }

        private void LoadMonsterData()
        {
            monstertable.Add("m01", new MonsterData("m01", "슬라임", 0, 0f ));
            monstertable.Add("m02", new MonsterData("m02", "불멍멍이", 0, 0f ));
            monstertable.Add("m03", new MonsterData("m03", "맹금류", 0, 0f ));
            monstertable.Add("m04", new MonsterData("m04", "나라쿠", 0, 0f ));
            monstertable.Add("m05", new MonsterData("m05", "흔한골렘", 0, 0f ));
            monstertable.Add("m06", new MonsterData("m06", "미녀마녀", 0, 0f ));
            monstertable.Add("m07", new MonsterData("m07", "산적1", 0, 0f ));
            monstertable.Add("m08", new MonsterData("m08", "괴인1", 0, 0f ));
            monstertable.Add("m09", new MonsterData("m09", "괴인2", 0, 0f ));
            monstertable.Add("m10", new MonsterData("m10", "21세기산적", 0, 0f ));
        }

        private void LoadHeroData()
        {
            herotable.Add("hero", new HeroData());
        }
    }
}