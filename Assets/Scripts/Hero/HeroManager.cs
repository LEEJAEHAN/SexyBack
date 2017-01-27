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

        public void Init()
        {
            LoadData();
            // this class is event listner
            Singleton<MonsterManager>.getInstance().Action_NewFousEvent += SetTarget;
        }
        private void LoadData()
        {
            testHeroData = new HeroData();
        }
        public void Start()
        {
            CreateHero();
            CurrentHero.ChangeState("Ready");
            LevelUp(CurrentHero.ID);
        }
        private void CreateHero()
        {
            CurrentHero = new Hero(testHeroData);
            Action_HeroCreateEvent(CurrentHero);
        }
        internal void Update()
        {
            CurrentHero.Update();
        }
        internal void LevelUp(string id)
        {
            CurrentHero.AddLevel(1);
        }

        internal void SetTarget(Monster monster)
        {
            if (CurrentHero == null)
                return;

            monster.Action_StateChangeEvent = CurrentHero.onTargetStateChange;
            //CurrentHero.SetDirection(monster.CenterPosition);
        }
    }
}