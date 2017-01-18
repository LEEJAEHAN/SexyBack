using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    class HeroManager
    {
        Hero Hero;
        #region
        #endregion
        HeroData testHeroData;

        // this class is event publisher
        public delegate void HeroCreate_Event(Hero hero);
        public event HeroCreate_Event noticeHeroCreate;

        public delegate void HeroChange_Event(Hero hero);
        public event HeroChange_Event noticeHeroChange;

        public delegate void ExpChange_Event(BigInteger exp);
        public event ExpChange_Event noticeEXPChange;

        string targetID;

        public void Init()
        {
            LoadData();

            // this class is event listner
            noticeHeroCreate += onHeroChange;
            noticeHeroChange += onHeroChange;
            noticeEXPChange += onExpChange;
            Singleton<GameInput>.getInstance().noticeTouch += onTap;
            Singleton<MonsterManager>.getInstance().noticeMonsterCreate += this.onMonsterCreate;
        }
        public void Start()
        {
            CreateHero();
        }
        private void LoadData()
        {
            testHeroData = new HeroData();
        }
        private void CreateHero()
        {
            Hero = new Hero(testHeroData);
            noticeHeroCreate(Hero);
        }

        internal void Update()
        {
            Hero.Update();
            // 엘레멘탈 매니져
        }
        internal void GainExp(BigInteger damage)
        {
            Hero.GainExp(damage);
            noticeEXPChange(Hero.EXP);
        }

        UILabel label_herodmg = ViewLoader.label_herodmg.GetComponent<UILabel>();
        UILabel label_exp = ViewLoader.label_exp.GetComponent<UILabel>();

        // event reciever
        internal void onTap(Vector3 targetPoint)
        {
            Hero.Attack(targetPoint);
        }
        internal void onMonsterCreate(Monster monster)
        {
            if (Hero == null)
                return;
            Hero.targetID = monster.ID;
        }
        internal void onHeroChange(Hero hero)
        {
            string dpsString = "DPC : " + hero.GetTotalDPC().ToSexyBackString();
            label_herodmg.text = dpsString;
        }
        internal void onExpChange(BigInteger exp)
        {
            string expstring = exp.ToSexyBackString() + " EXP";
            label_exp.text = expstring;
        }

    }
}