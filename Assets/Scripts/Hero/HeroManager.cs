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

        UILabel label_herodmg = ViewLoader.label_herodmg.GetComponent<UILabel>();

        public void Init()
        {
            LoadData();

            // this class is event listner
            noticeHeroChange += PrintDpc;
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

      

        // event reciever
        internal void onTouch(TapPoint pos)
        {
            //currhero
            CurrentHero.onTouch(pos);
        }
        internal void onMonsterCreate(Monster monster)
        {
            if (CurrentHero == null)
                return;
            CurrentHero.targetID = monster.ID;
            //CurrentHero.SetDirection(monster.CenterPosition);
        }
        void PrintDpc(Hero hero)
        {
            string dpsString = hero.DPC.ToSexyBackString() + " /Tap";
            label_herodmg.text = dpsString;
        }

    }
}