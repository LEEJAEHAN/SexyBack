using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

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
        ConsumableManager consumableManager;

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
            consumableManager = Singleton<ConsumableManager>.getInstance();
            stageManager = Singleton<StageManager>.getInstance();
            infoView = Singleton<GameInfoView>.getInstance();
            
            // TODO : menuScene과 연동필요
            statmanager.Init();            // 원래는 menuscene에서 연계된다.
            stageManager.Init(); // 이것역시 마찬가지.
            heroManager.Init();
            monsterManager.Init();
            elementalManager.Init();
            levelUpManager.Init();
            researchManager.Init(); // new PlayerStat.ResearchThread
            consumableManager.Init();
            infoView.Init();

        }
        internal void NewInstance() // 글로벌 히어로 데이터를 받아서 시작한다.
        {
            SceneParmater param = Singleton<SceneParmater>.getInstance();

            HeroStat hStat = new HeroStat();
            PlayerStat pStat = new PlayerStat();
            Dictionary<string, ElementalStat> eStats = StatManager.MakeElementalStats();
            BigInteger exp = new BigInteger(0);

            //load

            sexybacklog.Console(param.StageID);
            stageManager.Start(param.StageID, param.StageBonus);
            heroManager.CreateHero();

            // post event : statup
            statmanager.SetStat(hStat, pStat, eStats);
            // post event : levelup
            heroManager.LevelUp(1);
            // post event : exp gain 
            statmanager.ExpGain(exp, false);

        }
        internal void LoadInstance()
        {
            ElementalManager eData = (ElementalManager)SaveSystem.Load("elementalManager.dat");
            StatManager sData = (StatManager)SaveSystem.Load("statmanager.dat");
            HeroManager hData = (HeroManager)SaveSystem.Load("heroManager.dat");
            ResearchManager rData = (ResearchManager)SaveSystem.Load("researchManager.dat");
            HeroStat hStat = sData.GetHeroStat;
            PlayerStat pStat = sData.GetPlayerStat;
            Dictionary<string, ElementalStat> eStats = sData.GetElementalStats;

            //load
            heroManager.CreateHero();   // post : 스텟
            stageManager.Load((StageManager)SaveSystem.Load("stagemanager.dat"));       // 상관관계 x
            monsterManager.Load((MonsterManager)SaveSystem.Load("monsterManager.dat")); // 상관관계 x
            elementalManager.Load(eData);   //post : 스텟
            researchManager.Load(rData);

            // post event : statup
            statmanager.SetStat(hStat, pStat, eStats);
            researchManager.SetStateNTime(rData);

            // post event : levelup
            heroManager.LevelUp(hData.CurrentHero.LEVEL);            //heroManager.levelup;
            elementalManager.LevelUpAll(eData.elementals);            //elementalManager.levelup;

            // post event : exp gain 
            statmanager.ExpGain(sData.EXP, false);                   // set exp
            
            //GameManager loaddata = null;
            //heroManager.Load(loaddata.heroManager); // and hero is move
        }
        internal void SaveInstance()
        {
            SaveSystem.SaveInstacne();

            SaveSystem.Save(statmanager, "statmanager.dat");
            SaveSystem.Save(stageManager, "stagemanager.dat");
            SaveSystem.Save(monsterManager, "monsterManager.dat");
            SaveSystem.Save(heroManager, "heroManager.dat");
            SaveSystem.Save(elementalManager, "elementalManager.dat");
            SaveSystem.Save(researchManager, "researchManager.dat");
        }
        public void EndGame(bool clear) // 메뉴로 버튼을 눌렀을때,
        {
            // TODO : 게임매니저. 게임클리어에서 보상절차 작업해야함.
            if(clear)
            {
                //rewardmanager.makereward(시간기록,스테이지);
            }

            //SaveInstance(); // TODO : 테스트용으로있다.
            SaveSystem.ClearInstance();

            SceneManager.LoadScene("MenuScene");
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
            consumableManager.Dispose();

            Singleton<StatManager>.Clear();
            Singleton<HeroManager>.Clear();
            Singleton<MonsterManager>.Clear();
            Singleton<ElementalManager>.Clear();
            Singleton<LevelUpManager>.Clear();
            Singleton<ResearchManager>.Clear();
            Singleton<ConsumableManager>.Clear();
            Singleton<StageManager>.Clear();
            Singleton<GameInfoView>.Clear();
            
            statmanager = null;
            heroManager = null;
            monsterManager = null;
            elementalManager = null;
            levelUpManager = null;
            researchManager = null;
            consumableManager = null;
            stageManager = null;
            infoView = null;
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
            consumableManager.Update();
            stageManager.Update();
            statmanager.Update();
        }

    }



}
