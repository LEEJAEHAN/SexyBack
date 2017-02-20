using System;
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
        }
        public void CreateHero()
        {
            CurrentHero = new Hero(Singleton<TableLoader>.getInstance().herotable["hero"]);
            Action_HeroCreateEvent(CurrentHero);
            CurrentHero.ChangeState("Move"); //Ready
            CurrentHero.LevelUp(1);
            CurrentHero.SetDamageX(Singleton<Player>.getInstance().GetHeroStat.DpcX);
            CurrentHero.SetStat(Singleton<Player>.getInstance().GetHeroStat);
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
    }
}