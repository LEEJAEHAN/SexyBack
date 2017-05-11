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
        bool LoadComplete = false;

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
        InstanceStatus instanceStat;
        PlayerStatus outGameStat;

        InstanceSaveSystem saveTool;
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
            instanceStat = Singleton<InstanceStatus>.getInstance();
            stageManager = Singleton<StageManager>.getInstance();
            heroManager = Singleton<HeroManager>.getInstance();
            monsterManager = Singleton<MonsterManager>.getInstance();
            elementalManager = Singleton<ElementalManager>.getInstance();
            levelUpManager = Singleton<LevelUpManager>.getInstance();
            researchManager = Singleton<ResearchManager>.getInstance();
            consumableManager = Singleton<ConsumableManager>.getInstance();
            infoView = Singleton<GameInfoView>.getInstance();
            outGameStat = Singleton<PlayerStatus>.getInstance();

            // event listner가 없으면 딱히 init은 필요없다.
            outGameStat.Init();     // menu단계에서 이미 load되있으면 init을안할테고, 만약 playscene실행이면 init을함.
            monsterManager.Init();
            levelUpManager.Init();
            researchManager.Init(); // new PlayerStat.ResearchThread
            consumableManager.Init();
            heroManager.Init();
            elementalManager.Init();
            instanceStat.Init();
            infoView.Init();

            saveTool = new InstanceSaveSystem();
        }
        internal void NewInstance() // 글로벌 히어로 데이터를 받아서 시작한다.
        {
            // menuscene에서 연계된다.
            // 인스턴스 스텟은 기본스텟과 별개로 저장한다.
            // 기본스텟은 저장되지 않고, 로드(menuscene)시 1에서부터 특성(저장)과 장비(저장)로부터 항상 새로계산한다.

            // for test  원래라면 meneuscene에서 이미 init되있다.
            instanceStat.Start();
            heroManager.CreateHero(); // no level and stat
            stageManager.Start(instanceStat.GetMapID);

//            PlayerStatus stat
            ApplyStat(); // post event : statup
            // post event : exp gain 
            LoadComplete = true;
        }

        internal void ApplyStat() // 스텟 적용해야하는놈들, 히어로, 엘리멘탈, 리서치, 레벨업, 인스턴스스텟(exp)
        {
            researchManager.SetStat(outGameStat.GetUtilStat); //첨엔 없고, herocreate시 서먼된다.
            levelUpManager.SetStat(outGameStat.GetUtilStat); //첨엔 없고, herocreate시 서먼된다.
            elementalManager.SetLevelAndStat(outGameStat.GetElementalStats); // 첨엔 없고, 서먼된다.
            heroManager.SetLevelAndStat(outGameStat.GetHeroStat);
            instanceStat.SetStat(outGameStat.GetUtilStat);
            
        }


        internal void LoadInstance()
        {
            //스텟은 instance manager 파일에 저장또는 로드되지않는다.
            //load
            instanceStat.Load((InstanceStatus)saveTool.Load("statmanager.dat"));
            heroManager.Load((HeroManager)saveTool.Load("heroManager.dat"));   // post : 스텟

            //stageManager.Load(); 일단 수정하고난담에.
            stageManager.Load(instanceStat.GetMapID, (StageManager)saveTool.Load("stagemanager.dat"));       // 상관관계 x
            monsterManager.Load((MonsterManager)saveTool.Load("monsterManager.dat")); // 상관관계 x

            elementalManager.Load((ElementalManager)saveTool.Load("elementalManager.dat"));   //post : 스텟
            ResearchManager rData = (ResearchManager)saveTool.Load("researchManager.dat");
            researchManager.Load(rData);

            // post event : statup, expgain
            ApplyStat();

            researchManager.SetStateNTime(rData);

            //GameManager loaddata = null;
            //heroManager.Load(loaddata.heroManager); // and hero is move
            LoadComplete = true;
        }
        internal void SaveInstance()
        {
            if(LoadComplete)
            {
                saveTool.SaveInstacne();
            }
            saveTool.Save(instanceStat, "statmanager.dat");
            saveTool.Save(stageManager, "stagemanager.dat");
            saveTool.Save(monsterManager, "monsterManager.dat");
            saveTool.Save(heroManager, "heroManager.dat");
            saveTool.Save(elementalManager, "elementalManager.dat");
            saveTool.Save(researchManager, "researchManager.dat");
        }
        public void EndGame(bool clear) // 메뉴로 버튼을 눌렀을때,
        {
            int lastStage = Singleton<StageManager>.getInstance().CurrentFloor -1;
            string map = Singleton<InstanceStatus>.getInstance().GetMapID;
            int timeRecord = (int)Singleton<InstanceStatus>.getInstance().CurrentGameTime;
            InstanceSaveSystem.ClearInstance();
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
            elementalManager.Dispose();
            heroManager.Dispose();
            instanceStat.Dispose();

            Singleton<InstanceStatus>.Clear();
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
