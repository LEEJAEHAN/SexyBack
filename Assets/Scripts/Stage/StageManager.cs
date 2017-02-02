using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class StageManager // stage와 monster를 관리한다. 아마도 몬스터 매니져와 합치는게 좋지않을까.
    {
        int GoalFloor = 20;
        int currentFloor = 1;
        int DistancePerFloor = 10;
        int InitDistance = 0;
        int distance { get { return InitDistance + currentFloor * DistancePerFloor; } }

        private BigInteger exp = new BigInteger();//new BigInteger(10, Digit.E);
        public List<Stage> Stages = new List<Stage>(); // 보이는 Stage, 몬스터와 배경만 바꿔가며 polling을 한다.        
        public List<Stage> beToDispose = new List<Stage>(); // 풀링하지말자. 잦은이동이있는것도아닌데
        private bool needNextStage = false;

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
            foreach (Stage st in Stages)
                st.Move(delta_z);

            sexybacklog.InGame(distance + " " + currentFloor);
        }

        public void Start(GameModeData gamemode) //  // stagebuilder
        {
            SetGameMode(gamemode);
            // start
            Stages.Add(CreateStage(currentFloor, InitDistance + DistancePerFloor, 1));
            Stages.Add(CreateStage(currentFloor + 1, InitDistance + 2 * DistancePerFloor, 1));
        }

        private Stage CreateStage(int floor, int zPosition, int monsterCount)
        {
            Stage abc = new Stage(floor, zPosition);
            abc.Action_StageClear += onStageClear;
            abc.InitAvatar();
            for (int i = 0; i < monsterCount; i++)
            {
                abc.CreateMonster();
            }
            return abc;
        }

        private void onStageClear(Stage stage)
        {
            currentFloor = stage.floor + 1;
            beToDispose.Add(stage);
            needNextStage = true;
        }

        private void SetGameMode(GameModeData gamemode)
        {
            GoalFloor = gamemode.GoalFloor;
            ExpGain(gamemode.InitExp);
        }

        public void Update()
        {
            if(needNextStage)
            {
                Stages.Add(CreateStage(currentFloor + 1, DistancePerFloor, 1));
                needNextStage = false;
            }

            foreach (Stage a in Stages)
            {
                a.Update();
            }

            foreach (Stage b in beToDispose)
            {
                Stages.Remove(b);
                b.Dispose();
            }
            beToDispose.Clear();
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