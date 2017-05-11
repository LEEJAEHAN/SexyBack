using System;
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
            Singleton<PlayerStatus>.getInstance().Action_HeroStatChange -= onHeroStatChange;
        }
        Hero CurrentHero;

        // this class is event publisher
        public delegate void HeroCreate_Event(Hero hero);
        [field: NonSerialized]
        public event HeroCreate_Event Action_HeroCreateEvent;

        public delegate void HeroLevelUp_Event(Hero hero);
        [field: NonSerialized]
        public event HeroLevelUp_Event Action_HeroLevelUp;

        public void Init()
        {
            Singleton<PlayerStatus>.getInstance().Action_HeroStatChange += onHeroStatChange;
            // this class is event listner
        }
        internal void Load(HeroManager heroManager)
        {
            CreateHero();
            // 변수 load 
            int temp = heroManager.CurrentHero.AttackCount;
            CurrentHero.AttackManager.SetAttackCount(temp);
        }

        public void onHeroStatChange(HeroStat newStat, string eventType)
        {
            switch (eventType) // 이벤트 처리
            {
                case "Level":
                    SetLevelAndStat(newStat);
                    break;
                case "Enchant":
                case "DpcX":
                case "DpcIncreaseXH":
                    CurrentHero.SetStat(newStat, true, false);
                    break;
                // case "BonusLevel": //미구현
                //case "ActiveElement":
                //    Singleton<ElementalManager>.getInstance().LearnNewElemental(bonus.strvalue);
                //    break;
                default:
                    CurrentHero.SetStat(newStat, false, false);
                    break;
            }
        }
        public void CreateHero()
        {
            CurrentHero = new Hero(Singleton<TableLoader>.getInstance().herotable);
            Action_HeroCreateEvent(CurrentHero);
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

        internal void SetLevelAndStat(HeroStat getHeroStat)
        {
            CurrentHero.SetStat(getHeroStat, true, true);
            Action_HeroLevelUp(CurrentHero);
        }
    }
}