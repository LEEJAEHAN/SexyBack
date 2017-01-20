using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    class HeroManager
    {
        Hero CurrentHero;
        HeroData testHeroData;

        // this class is event publisher
        public delegate void HeroCreate_Event(Hero hero);
        public event HeroCreate_Event noticeHeroCreate;

        public delegate void HeroChange_Event(Hero hero);
        public event HeroChange_Event noticeHeroChange;

        public void Init()
        {
            LoadData();

            // this class is event listner
            noticeHeroChange += onHeroChange;
            Singleton<GameMoney>.getInstance().noticeEXPChange += onExpChange;
            Singleton<GameInput>.getInstance().noticeTouchPosition += onTouch;
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
            CurrentHero = new Hero(testHeroData);
            noticeHeroCreate(CurrentHero);
            noticeHeroChange(CurrentHero);
        }

        internal void Update()
        {
            CurrentHero.Update();
        }


        internal void LevelUp(string id)
        {
            CurrentHero.AddLevel(1);
            noticeHeroChange(CurrentHero);
        }

        UILabel label_herodmg = ViewLoader.label_herodmg.GetComponent<UILabel>();
        UILabel label_exp = ViewLoader.label_exp.GetComponent<UILabel>();

        // event reciever
        internal void onTouch(Vector3 touchPoint, Vector3 effectPoint)
        {
            //currhero
            CurrentHero.onTouch(touchPoint, effectPoint);
        }
        internal void onMonsterCreate(Monster monster)
        {
            if (CurrentHero == null)
                return;
            CurrentHero.targetID = monster.ID;
            //CurrentHero.SetDirection(monster.CenterPosition);
        }
        void onHeroChange(Hero hero)
        {
            string dpsString = "DPC : " + hero.DPC.ToSexyBackString();
            label_herodmg.text = dpsString;
        }
        void onExpChange(BigInteger exp)
        {
            string expstring = exp.ToSexyBackString() + " EXP";
            label_exp.text = expstring;
        }

    }
}