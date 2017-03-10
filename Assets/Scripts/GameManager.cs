using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


namespace SexyBackPlayScene
{
    public class GameManager : IDisposable
    {
        ~GameManager()
        {
            sexybacklog.Console("GameManager 소멸");
        }

        // singleton - stage
        StageManager stageManager; // 일종의 스크립트
        MonsterManager monsterManager;
        HeroManager heroManager;
        ElementalManager elementalManager;
        TalentManager talentManager;

        // singleton - player
        StatManager statmanager;
        LevelUpManager levelUpManager;
        ResearchManager researchManager;

        // View
        GameInfoView infoView;
        // member

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
            statmanager.Init(new HeroStat(), new PlayerStat(), MakeElementalStat(), new BigInteger(0));            // 원래는 menuscene에서 연계된다.
            stageManager.Init(Singleton<TableLoader>.getInstance().gamemodetable["TestStage"]); // 이것역시 마찬가지.
            heroManager.Init();
            monsterManager.Init();
            elementalManager.Init();
            levelUpManager.Init();
            researchManager.Init(5); // new PlayerStat.ResearchThread
            talentManager.Init();
            infoView.Init();
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
            statmanager.Dispose();
            //heroManager.Dispose();
            monsterManager.Dispose();
            //elementalManager.Dispose();
            //levelUpManager.Dispose();
            //researchManager.Dispose();
            //talentManager.Dispose();
            //stageManager.Dispose();
            //infoView.Dispose();

            Singleton<StatManager>.Clear();
            //Singleton<HeroManager>.Clear();
            Singleton<MonsterManager>.Clear();
            //Singleton<ElementalManager>.Clear();

            statmanager = null;
            //heroManager = null;
            monsterManager = null;
            //elementalManager = null;

        }

        internal void GameClear()
        {
        }
        internal void ExitGame()
        {
            PlayerPrefs.SetString("게임중저장데이터", "게임상태저장");
            Application.Quit();
        }

        internal void Start()
        {
            heroManager.CreateHero(); // and hero is move
            stageManager.Start();
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
