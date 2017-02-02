using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    public class GameManager
    { 
        // singleton - player
        LevelUpManager levelUpManager;

        // singleton - stage
        MonsterManager monsterManager;
        HeroManager heroManager;
        ElementalManager elementalManager;

        // View
        GameInfoView infoView;

        // member
        StageManager stageManager; // 일종의 스크립트

        // Use this for initialization
        public void Init()
        {
            heroManager = Singleton<HeroManager>.getInstance();
            monsterManager = Singleton<MonsterManager>.getInstance();
            elementalManager = Singleton<ElementalManager>.getInstance();
            levelUpManager = Singleton<LevelUpManager>.getInstance();
            infoView = Singleton<GameInfoView>.getInstance();
            stageManager = Singleton<StageManager>.getInstance();

            heroManager.Init();
            monsterManager.Init();
            elementalManager.Init();
            levelUpManager.Init();
            infoView.Init();
            stageManager.Init();
        }

        internal void Start(GameModeData args)
        {
            stageManager.Start(args);
            heroManager.CreateHero(); // and hero is move
//            elementalManager.SummonNewElemental("airball"); // for test


            //            monsterManager.CreateMonster(currentFloor);



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
            
            stageManager.Update();
        }


    }



}
