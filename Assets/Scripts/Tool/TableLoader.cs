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