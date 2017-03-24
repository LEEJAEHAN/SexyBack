using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    [Serializable]
    internal class StageManager // stage와 monster를 관리한다. 아마도 몬스터 매니져와 합치는게 좋지않을까.
    {
        // 저장되야할 데이터.
        public int GoalFloor;
        public int currentFloor;
        public double Gametime;
        public List<Stage> Stages; // 보이는 Stage, 몬스터와 배경만 바꿔가며 polling을 한다.        

        [NonSerialized]
        public static readonly int DistancePerFloor = 30;
        [NonSerialized]
        public List<Stage> beToDispose = new List<Stage>(); // 풀링하지말자. 잦은이동이있는것도아닌데
        [NonSerialized]
        private bool needNextStage = false;

        public void Init()
        {
        }

        internal void Start(GameModeData gameModeData) // start, no Load
        {
            GoalFloor = gameModeData.GoalFloor;
            currentFloor = 1;
            Gametime = 0;
            Singleton<GameInfoView>.getInstance().PrintStage(currentFloor);
            Stages = new List<Stage>(); // 보이는 Stage, 몬스터와 배경만 바꿔가며 polling을 한다.        
            Stages.Add(new Stage(1, DistancePerFloor, false, false, Singleton<MonsterManager>.getInstance().CreateRandomMonster(1)));
            Stages.Add(new Stage(2, DistancePerFloor + DistancePerFloor, false, false, Singleton<MonsterManager>.getInstance().CreateRandomMonster(2)));
        }
        internal void Load(StageManager data) // 클래스가 로드가 된뒤 셋해야 할것들.
        {
            GoalFloor = data.GoalFloor;
            currentFloor = data.currentFloor;
            Singleton<GameInfoView>.getInstance().PrintStage(currentFloor);
            Gametime = data.Gametime;
            Stages = new List<Stage>(); // 보이는 Stage, 몬스터와 배경만 바꿔가며 polling을 한다. 

            foreach( Stage stagedata in data.Stages)
            {
                Stage stage = new Stage(stagedata.floor, stagedata.zPosition, stagedata.isLastStage, stagedata.rewardComplete, stagedata.monsterID);
                stage.ChangeState(stagedata.savedState);
                Stages.Add(stage);
            }
        }


        private Stage CreateNextStage()
        {
            string monsterID = Singleton<MonsterManager>.getInstance().CreateRandomMonster(currentFloor + 1);
            bool isLast = (currentFloor + 1 == GoalFloor);
            Stage next = new Stage(currentFloor + 1, DistancePerFloor, isLast, false, monsterID);
            return next;
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
            sexybacklog.InGame("총시간 = " + (int)Gametime + " 초");
            sexybacklog.InGame("빨리감기 +" + Singleton<GameInput>.getInstance().fowardtimefordebug + " 초");

            if (needNextStage)
            {
                if(currentFloor < GoalFloor)
                {
                    Stage newStage = CreateNextStage();
                    Stages.Add(newStage);
                }
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