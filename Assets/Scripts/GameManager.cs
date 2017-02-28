using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    public class GameManager
    {
        // singleton - stage
        StageManager stageManager; // 일종의 스크립트
        MonsterManager monsterManager;
        HeroManager heroManager;
        ElementalManager elementalManager;
        TalentManager talentManager;

        // singleton - player
        StatManager player;
        LevelUpManager levelUpManager;
        ResearchManager researchManager;

        // View
        GameInfoView infoView;
        // member

        // Use this for initialization
        internal void Init(GameModeData args)
        {
            player = Singleton<StatManager>.getInstance();
            heroManager = Singleton<HeroManager>.getInstance();
            monsterManager = Singleton<MonsterManager>.getInstance();
            elementalManager = Singleton<ElementalManager>.getInstance();
            levelUpManager = Singleton<LevelUpManager>.getInstance();
            researchManager = Singleton<ResearchManager>.getInstance();
            talentManager = Singleton<TalentManager>.getInstance();
            stageManager = Singleton<StageManager>.getInstance();

            infoView = Singleton<GameInfoView>.getInstance();


            player.Init(args);
            stageManager.Init(args);
            heroManager.Init();
            monsterManager.Init();
            elementalManager.Init();
            levelUpManager.Init();
            researchManager.Init();
            talentManager.Init();
            infoView.Init();
        }

        internal void Start()
        {
            player.Start();
            stageManager.Start();
        }
        internal void FixedUpdate()
        {
            monsterManager.FixedUpdate();
        }

        // Update is called once per frame
        public void Update()
        {
            heroManager.Update();
            monsterManager.Update();
            elementalManager.Update();
            levelUpManager.Update();
            researchManager.Update();
            talentManager.Update();
            stageManager.Update();
        }

    }



}
