using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class StageManager // stage와 monster를 관리한다. 아마도 몬스터 매니져와 합치는게 좋지않을까.
    {
        int GoalFloor;
        int currentFloor = 1;
        float realDistance = 0;
        private BigInteger exp = new BigInteger();
        public List<Stage> Stages = new List<Stage>(); // 보이는 Stage, 몬스터와 배경만 바꿔가며 polling을 한다.        
                                                       // 풀링하지말자. 잦은이동이있는것도아닌데

        public delegate void ExpChange_Event(BigInteger exp);
        public event ExpChange_Event Action_ExpChange;

        public void Init()
        {
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += onHeroCreate;
        }

        private void onHeroCreate(Hero hero)
        {
            hero.Action_DistanceChange += onHeroMove;
        }

        public void onHeroMove(float delta_z)
        {
            realDistance += delta_z;
            foreach (Stage st in Stages)
                st.Move(delta_z);
        }

        public void Start(GameModeData gamemode) //  // stagebuilder
        {
            SetGameMode(gamemode);
            // start

            Stages.Add(CreateStage(currentFloor, 20, 1));
            Stages.Add(CreateStage(currentFloor + 1, 40, 1));

            Stages.Add(CreateStage(3, 60, 1));
            Stages.Add(CreateStage(4, 80, 1));
            Stages.Add(CreateStage(5, 100, 1));


            // stage 1 을 만든다.

            // test command
            //Singleton<ElementalManager>.getInstance().SummonNewElemental("fireball");
            //Singleton<ElementalManager>.getInstance().SummonNewElemental("snowball");
            //Singleton<ElementalManager>.getInstance().SummonNewElemental("airball");
            //Singleton<ElementalManager>.getInstance().SummonNewElemental("earthball");
            //Singleton<ElementalManager>.getInstance().SummonNewElemental("electricball");
            //Singleton<ElementalManager>.getInstance().SummonNewElemental("iceblock");
            //Singleton<ElementalManager>.getInstance().SummonNewElemental("rock");
            //Singleton<ElementalManager>.getInstance().SummonNewElemental("waterball");

        }

        private Stage CreateStage(int floor, int zPosition, int monsterCount)
        {
            Stage abc = new Stage(floor, zPosition);
            abc.InitAvatar();

            for (int i = 0; i < monsterCount; i++)
            {
                abc.CreateMonster();
            }

            return abc;
        }

        private void SetGameMode(GameModeData gamemode)
        {
            GoalFloor = gamemode.GoalFloor;
            ExpGain(gamemode.InitExp);
        }

        public void Update()
        {
            foreach (Stage a in Stages)
            {
                a.Update();
            }
        }

        public void ExpGain(BigInteger e)
        {
            exp += e;
            Action_ExpChange(exp);
        }
        internal bool ExpUse(BigInteger e)
        {
            bool result;

            if (exp - e < 0)
                result = false;
            else
            {
                exp -= e;
                result = true;
            }

            Action_ExpChange(exp);
            return result;
        }


    }
}