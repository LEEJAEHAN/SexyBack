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
        ElementalManager elementalManager;
        ResearchManager researchManager;

        [NonSerialized]
        HeroManager heroManager;
        [NonSerialized]
        LevelUpManager levelUpManager;
        [NonSerialized]
        ConsumableManager consumableManager;
        [NonSerialized]
        InstanceStat instanceStat;
        // singleton - player

        // View
        [NonSerialized]
        GameInfoView infoView;
        // member
        [NonSerialized]
        bool isPause = false;

        // Use this for initialization
        internal void Init()
        {

            instanceStat = Singleton<InstanceStat>.getInstance();
            stageManager = Singleton<StageManager>.getInstance();
            heroManager = Singleton<HeroManager>.getInstance();
            monsterManager = Singleton<MonsterManager>.getInstance();
            elementalManager = Singleton<ElementalManager>.getInstance();
            levelUpManager = Singleton<LevelUpManager>.getInstance();
            researchManager = Singleton<ResearchManager>.getInstance();
            consumableManager = Singleton<ConsumableManager>.getInstance();
            infoView = Singleton<GameInfoView>.getInstance();

            // event listner가 없으면 딱히 init은 필요없다.
            monsterManager.Init();
            levelUpManager.Init();
            researchManager.Init(); // new PlayerStat.ResearchThread
            consumableManager.Init();
            infoView.Init();

        }
        internal void NewInstance() // 글로벌 히어로 데이터를 받아서 시작한다.
        {
            // menuscene에서 연계된다.
            // 인스턴스 스텟은 기본스텟과 별개로 저장한다.
            // 기본스텟은 저장되지 않고, 로드(menuscene)시 1에서부터 특성(저장)과 장비(저장)로부터 항상 새로계산한다.


            // for test  원래라면 meneuscene에서 이미 init되있다.

            instanceStat.Start(Singleton<PlayerStatus>.getInstance());
            heroManager.CreateHero(); // no level and stat
            stageManager.Start(Singleton<PlayerStatus>.getInstance().mapID, Singleton<PlayerStatus>.getInstance().boost);
            instanceStat.ApplyStat(); // post event : statup
            // post event : exp gain 
        }

        internal void LoadInstance()
        {
            //load
            instanceStat.Load((InstanceStat)SaveSystem.Load("statmanager.dat"));
            heroManager.Load((HeroManager)SaveSystem.Load("heroManager.dat"));   // post : 스텟
            stageManager.Load((StageManager)SaveSystem.Load("stagemanager.dat"));       // 상관관계 x
            monsterManager.Load((MonsterManager)SaveSystem.Load("monsterManager.dat")); // 상관관계 x
            elementalManager.Load((ElementalManager)SaveSystem.Load("elementalManager.dat"));   //post : 스텟
            ResearchManager rData = (ResearchManager)SaveSystem.Load("researchManager.dat");
            researchManager.Load(rData);

            // post event : statup, expgain
            instanceStat.ApplyStat();
            researchManager.SetStateNTime(rData);

            //GameManager loaddata = null;
            //heroManager.Load(loaddata.heroManager); // and hero is move
        }
        internal void SaveInstance()
        {
            //SaveSystem.SaveInstacne();
            SaveSystem.Save(instanceStat, "statmanager.dat");
            SaveSystem.Save(stageManager, "stagemanager.dat");
            SaveSystem.Save(monsterManager, "monsterManager.dat");
            SaveSystem.Save(heroManager, "heroManager.dat");
            SaveSystem.Save(elementalManager, "elementalManager.dat");
            SaveSystem.Save(researchManager, "researchManager.dat");
        }
        public void EndGame(bool clear) // 메뉴로 버튼을 눌렀을때,
        {
            int lastStage = Singleton<StageManager>.getInstance().LastStage;
            string map = Singleton<StageManager>.getInstance().MapID;
            int timeRecord = (int)Singleton<StageManager>.getInstance().CurrentGameTime;
            SaveSystem.ClearInstance();
            Singleton<SexyBackRewardScene.RewardManager>.getInstance().GiveReward(map, clear, lastStage, timeRecord, 0);
            SceneManager.LoadScene("RewardScene");
            //SaveInstance(); // TODO : 테스트용으로있다.
        }

        public void Dispose()
        {
            // TODO : 해제작업계속.
            //instanceStat.Dispose();
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

            Singleton<InstanceStat>.Clear();
            Singleton<HeroManager>.Clear();
            Singleton<MonsterManager>.Clear();
            Singleton<ElementalManager>.Clear();
            Singleton<LevelUpManager>.Clear();
            Singleton<ResearchManager>.Clear();
            Singleton<ConsumableManager>.Clear();
            Singleton<StageManager>.Clear();
            Singleton<GameInfoView>.Clear();
            
            instanceStat = null;
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
            instanceStat.Update();
        }

    }



}
