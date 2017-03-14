﻿using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    [Serializable]
    internal class HeroManager : IDisposable
    {
        ~HeroManager()
        {
            sexybacklog.Console("HeroManager 소멸");
        }
        public void Dispose()
        {
            Action_HeroCreateEvent = null;
            Action_HeroLevelUp = null;
            CurrentHero.Dispose();
            CurrentHero = null;
        }

        public Hero CurrentHero;

        // this class is event publisher
        public delegate void HeroCreate_Event(Hero hero);
        [field: NonSerialized]
        public event HeroCreate_Event Action_HeroCreateEvent;

        public delegate void HeroLevelUp_Event(Hero hero);
        [field: NonSerialized]
        public event HeroLevelUp_Event Action_HeroLevelUp;

        public void Init()
        {
            // this class is event listner
        }
        public void CreateHero()
        {
            CurrentHero = new Hero(Singleton<TableLoader>.getInstance().herotable);
            Action_HeroCreateEvent(CurrentHero);
            //CurrentHero.SetStat(Singleton<StatManager>.getInstance().GetHeroStat, true);
            //LevelUp(1);
            CurrentHero.ChangeState("Move"); //Init state is move
        }

        internal void Update()
        {
            if (CurrentHero == null)
                return;
            CurrentHero.Update();
        }
        internal Hero GetHero()
        {
            return CurrentHero;
        }
        internal void SetTarget(Monster monster)
        {
            if (CurrentHero == null)
                return;

            monster.StateMachine.Action_changeEvent += CurrentHero.onTargetStateChange;
        }

        internal void LevelUp(int amount)
        {
            CurrentHero.LevelUp(amount);
            Action_HeroLevelUp(CurrentHero);
        }

    }
}