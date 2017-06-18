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
        public bool AutoAttack = true;
        public bool AutoChestOpen = true;

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
            CreateHero();
            XmlNode rootNode = doc.SelectSingleNode("InstanceStatus/Hero");
            if(rootNode.Attributes["state"].Value == "FastMove")
                CurrentHero.ChangeState("FastMove");
            else
                CurrentHero.ChangeState("Move");
            CurrentHero.DamageDensity = double.Parse(rootNode.Attributes["basedmg"].Value);
            CurrentHero.LevelUp(int.Parse(rootNode.Attributes["level"].Value));// 레벨업 이벤트를 안줘서 research를 생성하지 않는다.
        }
        public void Start()
        {
            CreateHero();
            CurrentHero.ChangeState("Move"); //Init state is move
            LevelUp(1);
            //CurrentHero.onHeroStatChange(Singleton<PlayerStatus>.getInstance().GetHeroStat);
        }
        private void CreateHero()
        {
            CurrentHero = new Hero(Singleton<TableLoader>.getInstance().herotable);
            Action_HeroCreateEvent(CurrentHero);
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