using System;
using System.Collections.Generic;
namespace SexyBackPlayScene
{
    internal class TableLoader
    {
        public Dictionary<string, HeroData> herotable = new Dictionary<string, HeroData>();
        public Dictionary<string, MonsterData> monstertable = new Dictionary<string, MonsterData>();
        public Dictionary<string, ElementalData> elementaltable = new Dictionary<string, ElementalData>();
        // public Dictionary<string, HeroData> stagetable = new Dictionary<string, HeroData>();
        public Dictionary<string, HeroData> leveluptable = new Dictionary<string, HeroData>();


        internal void LoadAll()
        {
            LoadHeroData();
            LoadMonsterData();
            LoadElementData();

            //  LoadStageData();
            LoadLevelUpData();

        }

        private void LoadLevelUpData()
        {
        }

        private void LoadStageData()
        {
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
            monstertable.Add("m01", new MonsterData("m01", "몬스터이름", "Sprites/Monster/m01", 0, 0, new BigInteger(100, Digit.m)));
            monstertable.Add("m02", new MonsterData("m02", "몬스터이름", "Sprites/Monster/m02", 0, 0, new BigInteger(4444440000)));
            monstertable.Add("m03", new MonsterData("m03", "몬스터이름", "Sprites/Monster/m03", 0, 0f, new BigInteger(999999000)));
            monstertable.Add("m04", new MonsterData("m04", "몬스터이름", "Sprites/Monster/m04", 0, 0.5f, new BigInteger(1, Digit.zero)));
            monstertable.Add("m05", new MonsterData("m05", "몬스터이름", "Sprites/Monster/m05", 0, 0, new BigInteger(999999, Digit.k)));
            monstertable.Add("m06", new MonsterData("m06", "몬스터이름", "Sprites/Monster/m06", 0, 0, new BigInteger("1")));
            monstertable.Add("m07", new MonsterData("m07", "몬스터이름", "Sprites/Monster/m07", 0, 0, new BigInteger(999999, Digit.k)));
            monstertable.Add("m08", new MonsterData("m08", "몬스터이름", "Sprites/Monster/m08", 0, 0, new BigInteger(1000, Digit.b)));
            monstertable.Add("m09", new MonsterData("m09", "몬스터이름", "Sprites/Monster/m09", 0, 0, new BigInteger(1000, Digit.b)));
            monstertable.Add("m10", new MonsterData("m10", "몬스터이름", "Sprites/Monster/m10", 0, 0, new BigInteger(1000, Digit.b)));
        }

        private void LoadHeroData()
        {
            herotable.Add("hero", new HeroData());
        }
    }
}