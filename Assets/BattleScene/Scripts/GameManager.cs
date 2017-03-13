using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SexyBackPlayScene
{
    [Serializable]
    public class GameManager : IDisposable
    {
        ~GameManager()
        {
            sexybacklog.Console("GameManager 소멸");
        }
        // serialized
        StageManager stageManager; // 일종의 스크립트
        MonsterManager monsterManager;
        HeroManager heroManager;
        ElementalManager elementalManager;
        [NonSerialized]
        LevelUpManager levelUpManager;

        [NonSerialized]
        TalentManager talentManager;

        [NonSerialized]
        StatManager statmanager;
        // singleton - player
        [NonSerialized]
        ResearchManager researchManager;

        // View
        [NonSerialized]
        GameInfoView infoView;
        // member
        [NonSerialized]
        bool isPause = false;

        // Use this for initialization
        internal void Init()
        {
            statmanager = Singleton<StatManager>.getInstance();
            heroManager = Singleton<HeroManager>.getInstance();
            monsterManager = Singleton<MonsterManager>.getInstance();
            elementalManager = Singleton<ElementalManager>.getInstance();
            levelUpManager = Singleton<LevelUpManager>.getInstance();
            researchManager = Singleton<ResearchManager>.getInstance();
            talentManager = Singleton<TalentManager>.getInstance();
            stageManager = Singleton<StageManager>.getInstance();
            infoView = Singleton<GameInfoView>.getInstance();
            
            // TODO : menuScene과 연동필요
            statmanager.Init();            // 원래는 menuscene에서 연계된다.
            stageManager.Init(); // 이것역시 마찬가지.
            heroManager.Init();
            monsterManager.Init();
            elementalManager.Init();
            levelUpManager.Init();
            researchManager.Init(5); // new PlayerStat.ResearchThread
            talentManager.Init();
            infoView.Init();
        }
        internal void NewInstance() // 글로벌 히어로 데이터를 받아서 시작한다.
        {
            statmanager.Start(new HeroStat(), new PlayerStat(), MakeElementalStat(), new BigInteger(0));
            stageManager.Start(Singleton<TableLoader>.getInstance().gamemodetable["TestStage"]);
            heroManager.Start(); // and hero is move

            //elementalmanager.LearnNewElemental("magmaball");
            //elementalmanager.LearnNewElemental("fireball");
            //elementalmanager.LearnNewElemental("waterball");
            //elementalmanager.LearnNewElemental("rock");
            //elementalmanager.LearnNewElemental("electricball");
            //elementalmanager.LearnNewElemental("snowball");
            //elementalmanager.LearnNewElemental("earthball");
            //elementalmanager.LearnNewElemental("airball"); // for test
            //elementalmanager.LearnNewElemental("iceblock");
        }
        internal void LoadInstance()
        {
            //GameManager loaddata = null;

            //statmanager.Start(new HeroStat(), new PlayerStat(), MakeElementalStat(), new BigInteger(0));
            StatManager sData = (StatManager)SaveSystem.Load("statmanager.dat");
            statmanager.Load(sData);              // 만들때
            stageManager.Load((StageManager)SaveSystem.Load("stagemanager.dat"));       // 상관관계 x
            monsterManager.Load((MonsterManager)SaveSystem.Load("monsterManager.dat")); // 상관관계 x
            HeroManager hData = (HeroManager)SaveSystem.Load("heroManager.dat");
            heroManager.Load(hData);   // pre : statamaniger.herostat필요함. post : 스텟
            ElementalManager eData = (ElementalManager)SaveSystem.Load("elementalManager.dat");
            elementalManager.Load(eData);   // pre : statmaiger.elementastst. post : 스텟

            //resaearch 로드
            //talent 로드
            // gameinfo ( 시간 ) 로드

            //statmanager.setallstat;
            heroManager.CurrentHero.SetStat(statmanager.GetHeroStat, true);
            elementalManager.SetStatAll(statmanager.GetElementalStats, true);
            //heroManager.levelup;
            heroManager.LevelUp(hData.CurrentHero.LEVEL);
            //elementalManager.levelup;
            elementalManager.LevelUpAll(eData.elementals);
            statmanager.ExpGain(sData.EXP, false);
            //statmanager.gainexp;

            // 리서치 매니져 // pre : 스텟메니져.research스텟과 post : 스텟메니져.exp와 hero,레벨 elements.level, 스텟
            // 레벨업 매니져 // pre : 스텟매니져.levelup스텟, hero, element creates, post : hero레벨, element레벨, 스텟

            //heroManager.Load(loaddata.heroManager); // and hero is move
        }
        internal void SaveInstance()
        {
            PlayerPrefs.SetString("InstanceData", "Yes");

            SaveSystem.Save(statmanager, "statmanager.dat");
            SaveSystem.Save(stageManager, "stagemanager.dat");
            SaveSystem.Save(monsterManager, "monsterManager.dat");
            SaveSystem.Save(heroManager, "heroManager.dat");
            SaveSystem.Save(elementalManager, "elementalManager.dat");

        }

        private Dictionary<string, ElementalStat> MakeElementalStat()
        {
            Dictionary<string, ElementalStat> elementalStats = new Dictionary<string, ElementalStat>();
            foreach (string elementalid in Singleton<TableLoader>.getInstance().elementaltable.Keys)
                elementalStats.Add(elementalid, new ElementalStat());
            return elementalStats;
        }

        public void Dispose()
        {
            // TODO : 해제작업계속.
            //statmanager.Dispose();
            //heroManager.Dispose();
            //elementalManager.Dispose();
            //levelUpManager.Dispose();
            //researchManager.Dispose();
            //talentManager.Dispose();
            //stageManager.Dispose();
            //infoView.Dispose();

            monsterManager.Dispose();
            levelUpManager.Dispose();
            researchManager.Dispose();
            talentManager.Dispose();

            Singleton<StatManager>.Clear();
            Singleton<HeroManager>.Clear();
            Singleton<MonsterManager>.Clear();
            Singleton<ElementalManager>.Clear();
            Singleton<LevelUpManager>.Clear();
            Singleton<ResearchManager>.Clear();
            Singleton<TalentManager>.Clear();
            Singleton<StageManager>.Clear();
            Singleton<GameInfoView>.Clear();
            
            statmanager = null;
            heroManager = null;
            monsterManager = null;
            elementalManager = null;
            levelUpManager = null;
            researchManager = null;
            talentManager = null;
            stageManager = null;
            infoView = null;
        }

        internal void ClearInstance()
        {
            PlayerPrefs.DeleteKey("InstanceData");
            PlayerPrefs.DeleteAll();
        }


        internal void GameClear()
        {
            // 보상작업
        }



        internal void FixedUpdate()
        {
            if (isPause)
                return;
            monsterManager.FixedUpdate();
        }

        public void Pause(bool value)
        {
            isPause = value;
            if (isPause)
                ViewLoader.PopUpcPanel.SetActive(true);
            else
                ViewLoader.PopUpcPanel.SetActive(false);
        }


        // Update is called once per frame
        public void Update()
        {
            if (isPause)
                return;

            heroManager.Update();
            monsterManager.Update();
            elementalManager.Update();
            levelUpManager.Update();
            researchManager.Update();
            talentManager.Update();
            stageManager.Update();
            statmanager.Update();
        }

    }



}
