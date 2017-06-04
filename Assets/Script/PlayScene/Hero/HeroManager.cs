using System;
using System.Xml;
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
            Action_LevelUp = null;
            CurrentHero.Dispose();
            CurrentHero = null;
            Singleton<PlayerStatus>.getInstance().Action_HeroStatChange -= onHeroStatChange;
            Singleton<PlayerStatus>.getInstance().Action_BaseStatChange -= onBaseStatChange;
        }
        Hero CurrentHero;

        // this class is event publisher
        public delegate void HeroCreate_Event(Hero hero);
        [field: NonSerialized]
        public event HeroCreate_Event Action_HeroCreateEvent;

        public delegate void LevelUp_Event(ICanLevelUp hero);
        [field: NonSerialized]
        public event LevelUp_Event Action_LevelUp;
                
        public void Init()
        {
            Singleton<PlayerStatus>.getInstance().Action_HeroStatChange += onHeroStatChange;
            Singleton<PlayerStatus>.getInstance().Action_BaseStatChange += onBaseStatChange;

            // this class is event listner
        }
        internal void Load(XmlDocument doc)
        {
            XmlNode rootNode = doc.SelectSingleNode("InstanceStatus/Hero");
            CreateNewHero();
            CurrentHero.BaseDmg = double.Parse(rootNode.Attributes["basedmg"].Value);
            // 레벨업 이벤트를 안줘서 research를 생성하지 않는다.
            CurrentHero.LevelUp(int.Parse(rootNode.Attributes["level"].Value));
        }

        internal void LevelUp(int value)
        {
            CurrentHero.LevelUp(value);
            Action_LevelUp(CurrentHero);
        }
        internal void Enchant(string elementID)
        {
            CurrentHero.Enchant(elementID);
        }
        public void onBaseStatChange(BaseStat newStat)
        {
            CurrentHero.onStatChange();
        }
        public void onHeroStatChange(HeroStat newStat)
        {
            CurrentHero.onStatChange();
        }
        public void CreateNewHero()
        {
            CurrentHero = new Hero(Singleton<TableLoader>.getInstance().herotable);
            Action_HeroCreateEvent(CurrentHero);
            CurrentHero.ChangeState("Move"); //Init state is move
            //CurrentHero.onHeroStatChange(Singleton<PlayerStatus>.getInstance().GetHeroStat);
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