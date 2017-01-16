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

        // Use this for initialization
        public void Init()
        {
            monsterManager = Singleton<MonsterManager>.getInstance();
            heroManager = Singleton<HeroManager>.getInstance();
            elementalManager = Singleton<ElementalManager>.getInstance();
            levelUpManager = Singleton<LevelUpManager>.getInstance();

            monsterManager.Init();
            heroManager.Init();
            elementalManager.Init();
            levelUpManager.Init();

            monsterManager.Start();
            heroManager.Start();
            elementalManager.Start();
            levelUpManager.Start();

        }
        // Update is called once per frame
        public void Update()
        {
            monsterManager.Update();
            heroManager.Update();
            elementalManager.Update();
            levelUpManager.Update();
        }


    }



}
