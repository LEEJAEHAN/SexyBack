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
            Singleton<MonsterManager>.getInstance().Action_BeginBattleEvent += BeginBattle;
        }
        public void CreateHero()
        {
            CurrentHero = new Hero(Singleton<TableLoader>.getInstance().herotable["hero"]);
            Action_HeroCreateEvent(CurrentHero);
            CurrentHero.ChangeState("Move"); //Ready
            LevelUp(CurrentHero.GetID);
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

            CurrentHero.LevelUp(1);
        }
        internal void BeginBattle(Monster monster)
        {
            if (CurrentHero == null)
                return;

            monster.StateMachine.Action_changeEvent += CurrentHero.onTargetStateChange;
            CurrentHero.ChangeState("Ready");
        }

        internal bool Upgrade(Bonus b)
        {
            return CurrentHero.Upgrade(b);
        }
    }
}