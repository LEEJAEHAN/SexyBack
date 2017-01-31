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
        Stage stage; // 일종의 스크립트

        // Use this for initialization
        public void Init()
        {
            heroManager = Singleton<HeroManager>.getInstance();
            monsterManager = Singleton<MonsterManager>.getInstance();
            elementalManager = Singleton<ElementalManager>.getInstance();
            levelUpManager = Singleton<LevelUpManager>.getInstance();
            infoView = Singleton<GameInfoView>.getInstance();
            stage = Singleton<Stage>.getInstance();

            heroManager.Init();
            monsterManager.Init();
            elementalManager.Init();
            levelUpManager.Init();
            infoView.Init();
            stage.Init();
        }

        internal void Start(object args)
        {
           stage.Start(new StageData());
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
            
            stage.Update();
        }


    }



}
