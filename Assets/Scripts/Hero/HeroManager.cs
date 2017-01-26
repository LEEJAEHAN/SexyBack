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
        public event HeroCreate_Event Action_HeroCreateEvent;

        public delegate void HeroChange_Event(Hero hero);
        public event HeroChange_Event noticeHeroChange;

        UILabel label_herodmg = ViewLoader.label_herodmg.GetComponent<UILabel>();

        public void Init()
        {
            LoadData();
            // this class is event listner
            noticeHeroChange += PrintDpc;
            Singleton<MonsterManager>.getInstance().Action_NewFousEvent += SetTarget;
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
            Action_HeroCreateEvent(CurrentHero);
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

        internal void SetTarget(Monster monster)
        {
            if (CurrentHero == null)
                return;

            monster.Action_StateChangeEvent = CurrentHero.onTargetStateChange;
            //CurrentHero.SetDirection(monster.CenterPosition);
        }

        void PrintDpc(Hero hero)
        {
            string dpsString = hero.DPC.To5String() + " /Tap";
            label_herodmg.text = dpsString;
        }

    }
}