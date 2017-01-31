﻿using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    class HeroManager
    {
        Hero CurrentHero;

        // this class is event publisher
        public delegate void HeroCreate_Event(Hero hero);
        public event HeroCreate_Event Action_HeroCreateEvent;

        public void Init()
        {
            // this class is event listner
            Singleton<MonsterManager>.getInstance().Action_NewFousEvent += SetTarget;
        }
        public void CreateHero()
        {
            CurrentHero = new Hero(Singleton<TableLoader>.getInstance().herotable["hero"]);
            Action_HeroCreateEvent(CurrentHero);
            CurrentHero.ChangeState("Ready");
            LevelUp(CurrentHero.ID);
        }
        internal void Update()
        {
            if (CurrentHero == null)
                return;
            CurrentHero.Update();
        }
        internal void LevelUp(string id)
        {
            if (CurrentHero == null)
                return;

            CurrentHero.AddLevel(1);
        }
        internal void SetTarget(Monster monster)
        {
            if (CurrentHero == null)
                return;

            monster.Action_StateChangeEvent = CurrentHero.onTargetStateChange;
        }

        
    }
}