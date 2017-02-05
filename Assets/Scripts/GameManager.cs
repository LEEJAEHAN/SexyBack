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
        ResearchManager researchManager;

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
            researchManager = Singleton<ResearchManager>.getInstance();
            infoView = Singleton<GameInfoView>.getInstance();


            stageManager = Singleton<StageManager>.getInstance();

            heroManager.Init();
            monsterManager.Init();
            elementalManager.Init();
            levelUpManager.Init();
            researchManager.Init();

            infoView.Init();
            stageManager.Init();
        }

        internal void Start(GameModeData args)
        {
            stageManager.Start(args);
            heroManager.CreateHero(); // and hero is move

            ////elementalManager.SummonNewElemental("fireball");
            //elementalManager.SummonNewElemental("waterball");
            //elementalManager.SummonNewElemental("rock");
            //elementalManager.SummonNewElemental("electricball");
            //elementalManager.SummonNewElemental("snowball");
            //elementalManager.SummonNewElemental("earthball");
            //elementalManager.SummonNewElemental("airball"); // for test
            //elementalManager.SummonNewElemental("iceblock");
            elementalManager.SummonNewElemental("magmaball");


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
            stageManager.Update();
        }


    }



}
