using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    public class GameManager
    { // singleton

        MonsterManager monsterManager;
        HeroManager heroManager;
        ElementalManager elementalManager;
        LevelUpManager levelUpManager;

        GameMoney gameMoney;
        GameProgress gameProgress;

        // Use this for initialization
        public void Init()
        {
            heroManager = Singleton<HeroManager>.getInstance();
            monsterManager = Singleton<MonsterManager>.getInstance();
            elementalManager = Singleton<ElementalManager>.getInstance();
            levelUpManager = Singleton<LevelUpManager>.getInstance();
            gameMoney = Singleton<GameMoney>.getInstance();
            gameProgress = Singleton<GameProgress>.getInstance();


            heroManager.Init();
            monsterManager.Init();
            elementalManager.Init();
            levelUpManager.Init();
            gameMoney.Init();
            gameProgress.Init();


            heroManager.Start();
            monsterManager.Start();
            elementalManager.Start();
            levelUpManager.Start();

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
            gameMoney.Update();
            gameProgress.Update();
        }


    }



}
