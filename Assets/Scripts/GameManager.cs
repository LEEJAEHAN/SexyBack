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
            heroManager = Singleton<HeroManager>.getInstance();
            monsterManager = Singleton<MonsterManager>.getInstance();
            elementalManager = Singleton<ElementalManager>.getInstance();
            levelUpManager = Singleton<LevelUpManager>.getInstance();


            heroManager.Init();
            monsterManager.Init();
            elementalManager.Init();
            levelUpManager.Init();

            heroManager.Start();
            monsterManager.Start();
            elementalManager.Start();
            levelUpManager.Start();

        }
        // Update is called once per frame
        public void Update()
        {
            heroManager.Update();
            monsterManager.Update();
            elementalManager.Update();
            levelUpManager.Update();
        }


    }



}
