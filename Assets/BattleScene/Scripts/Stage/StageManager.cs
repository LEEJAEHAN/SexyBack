using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    [Serializable]
    internal class StageManager // stage와 monster를 관리한다. 아마도 몬스터 매니져와 합치는게 좋지않을까.
    {
        // 저장되야할 데이터.
        public string MapID;
        public bool isBonused;

        public double CurrentGameTime;
        public int CurrentFloor;
        public List<Stage> Stages; // 보이는 Stage, 몬스터와 배경만 바꿔가며 polling을 한다.        

        [NonSerialized]
        public static readonly int DistancePerFloor = 30;
        [NonSerialized]
        public int MaxFloor;
        [NonSerialized]
        public int LimitGameTime;

        [NonSerialized]
        public List<Stage> beToDispose = new List<Stage>(); // 풀링하지말자. 잦은이동이있는것도아닌데
        [NonSerialized]
        private bool needNextStage = false;

        public void Init()
        {
        }

        internal void Start(string mapID, bool isBonused) // start, no Load
        {
            MapData map = Singleton<TableLoader>.getInstance().gamemodetable[mapID];
            MapID = map.ID;
            this.isBonused = isBonused;
            MaxFloor = map.MaxFloor;
            LimitGameTime = map.LimitTime;

            CurrentFloor = 1;
            CurrentGameTime = 0;
            Singleton<GameInfoView>.getInstance().PrintStage(CurrentFloor);
            Stages = new List<Stage>(); // 보이는 Stage, 몬스터와 배경만 바꿔가며 polling을 한다.        
            Stages.Add(new Stage(1, DistancePerFloor, false, false, Singleton<MonsterManager>.getInstance().CreateRandomMonster(1)));
            Stages.Add(new Stage(2, DistancePerFloor + DistancePerFloor, false, false, Singleton<MonsterManager>.getInstance().CreateRandomMonster(2)));
        }
        internal void Load(StageManager data) // 클래스가 로드가 된뒤 셋해야 할것들.
        {
            MapData map = Singleton<TableLoader>.getInstance().gamemodetable[data.MapID];
            MapID = map.ID;
            isBonused = data.isBonused;
            MaxFloor = map.MaxFloor;
            LimitGameTime = map.LimitTime;

            Singleton<GameInfoView>.getInstance().PrintStage(CurrentFloor);
            CurrentGameTime = data.CurrentGameTime;
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
            string monsterID = Singleton<MonsterManager>.getInstance().CreateRandomMonster(CurrentFloor + 1);
            bool isLast = (CurrentFloor + 1 == MaxFloor);
            Stage next = new Stage(CurrentFloor + 1, DistancePerFloor, isLast, false, monsterID);
            return next;
        }

        public void onStagePass(int floor)
        {
            CurrentFloor = floor + 1;
            Singleton<GameInfoView>.getInstance().PrintStage(CurrentFloor);
        }
        public void onStageClear(Stage stage)
        {
            beToDispose.Add(stage);
            needNextStage = true;
        }

        public void Update()
        {
            CurrentGameTime += Time.deltaTime;
            sexybacklog.InGame("총시간 = " + (int)CurrentGameTime + " 초");
            sexybacklog.InGame("빨리감기 +" + Singleton<GameInput>.getInstance().fowardtimefordebug + " 초");

            if (needNextStage)
            {
                if(CurrentFloor < MaxFloor)
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