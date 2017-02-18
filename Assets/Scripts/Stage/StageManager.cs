using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class StageManager // stage와 monster를 관리한다. 아마도 몬스터 매니져와 합치는게 좋지않을까.
    {
        int GoalFloor = 20;
        int currentFloor = 1;
        public double Gametime = 0;
        readonly int DistancePerFloor = 30;
        public static readonly int HeroPosition = -10;

        public List<Stage> Stages = new List<Stage>(); // 보이는 Stage, 몬스터와 배경만 바꿔가며 polling을 한다.        
        public List<Stage> beToDispose = new List<Stage>(); // 풀링하지말자. 잦은이동이있는것도아닌데
        private bool needNextStage = false;

        public void Init(GameModeData gamemode)
        {
            SetGameMode(gamemode);
        }
        private void SetGameMode(GameModeData gamemode)
        {
            GoalFloor = gamemode.GoalFloor;
        }
        public void Start() // start stagebuilder
        {
            Stages.Add(CreateStage(currentFloor, HeroPosition + DistancePerFloor, 1));
            Stages.Add(CreateStage(currentFloor + 1, HeroPosition + 2 * DistancePerFloor, 1));
        }

        private Stage CreateStage(int floor, int zPosition, int monsterCount)
        {
            Stage abc = new Stage(floor, zPosition);
            abc.InitAvatar();
            for (int i = 0; i < monsterCount; i++)
            {
                abc.CreateMonster();
            }
            abc.StateMachine.ChangeState("Move");
            return abc;
        }
        public void onStagePass(int floor)
        {
            currentFloor = floor + 1;
            Singleton<GameInfoView>.getInstance().PrintStage(currentFloor);
        }
        public void onStageClear(Stage stage)
        {
            beToDispose.Add(stage);
            needNextStage = true;
        }

        public void Update()
        {
            Gametime += Time.deltaTime;
            sexybacklog.InGame("총시간 = " + (int)Gametime + "\n빨리감기 +" + Singleton<GameInput>.getInstance().fowardtimefordebug + " 초");

            if (needNextStage)
            {
                Stages.Add(CreateStage(currentFloor + 1, HeroPosition + 2 * DistancePerFloor, 1));
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
    }
}