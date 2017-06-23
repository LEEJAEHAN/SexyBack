using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using System.Xml;

namespace SexyBackPlayScene
{
    [Serializable]
    public class InstanceGameManager : IDisposable
    {
        ~InstanceGameManager()
        {
            sexybacklog.Console("InstanceGameManager 소멸");
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
        //[NonSerialized]
        ConsumableManager consumableManager;
        [NonSerialized]
        InstanceStatus instanceStat;
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

            // event listner가 없으면 딱히 init은 필요없다.
            monsterManager.Init();
            levelUpManager.Init();
            researchManager.Init(); // new PlayerStat.ResearchThread
            consumableManager.Init();
            heroManager.Init();
            elementalManager.Init();
            instanceStat.Init();
            infoView.Init();
        }
        internal void NewInstance() // 글로벌 히어로 데이터를 받아서 시작한다.
        {
            instanceStat.Start();
            researchManager.Start();
            heroManager.Start(); // no level and stat
            
            stageManager.Start(instanceStat.InstanceMap);

            GameObject.Find("Middle_Window").GetComponent<TweenAlpha>().ResetToBeginning();
            LoadComplete = true;
        }

        internal bool LoadInstanceData()
        {
            try
            {
                XmlDocument doc = SaveSystem.LoadXml(InstanceSaveSystem.InstanceDataPath);
                instanceStat.Load(doc);
                heroManager.Load(doc);
                stageManager.Load(doc); // 상관관계 x //일단 수정하고난담에.
                monsterManager.Load(doc); // 상관관계 x
                elementalManager.Load(doc);
                consumableManager.Load(doc);
                researchManager.Load(doc);
            }
            catch (Exception e)
            {
                sexybacklog.Console("인스턴스 데이터 로드에 이상이 있었습니다." + e.Message);
                //InstanceSaveSystem.ClearInstance();
                LoadComplete = false;
                return LoadComplete;
            }
            LoadComplete = true;
            return LoadComplete;
        }
        internal void SaveInstance()
        {
            //if(LoadComplete)
                InstanceSaveSystem.SaveInstacne();
            SaveSystem.SaveGlobalData();
        }
        public void EndGame(bool clear) // 메뉴로 버튼을 눌렀을때,
        {
            SaveInstance(); // TODO : 테스트용으로있다.
            GameClear(clear);
            InstanceSaveSystem.ClearInstance();
            SceneManager.LoadScene("RewardScene");
        }

        private void GameClear(bool clear)
        {
            Map currentmap = Singleton<InstanceStatus>.getInstance().InstanceMap;
            int lastStage = Singleton<StageManager>.getInstance().CurrentFloor - 1;
            int timeRecord = (int)Singleton<InstanceStatus>.getInstance().CurrentGameTime;
            int totalLevel = Singleton<HeroManager>.getInstance().GetHero().OriginalLevel + Singleton<ElementalManager>.getInstance().TotalLevel;
            int finishResearchCount = Singleton<ResearchManager>.getInstance().FinishList.Count;

            Singleton<SexyBackRewardScene.RewardManager>.getInstance().RecordResult
                (currentmap, clear, lastStage, timeRecord, totalLevel, finishResearchCount);
        }

        public void Dispose()
        {
            // TODO : 해제작업계속.
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
