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
            monsterManager.Init();

            heroManager = Singleton<HeroManager>.getInstance();
            heroManager.Init();

            elementalManager = Singleton<ElementalManager>.getInstance();
            elementalManager.Init();


            levelUpManager = Singleton<LevelUpManager>.getInstance();
            levelUpManager.Init();
        }   

        double gameTime;
        public double testTimeTick = 1;

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
