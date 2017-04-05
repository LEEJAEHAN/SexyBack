using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    [Serializable]
    internal class StageManager // stage와 monster를 관리한다. 아마도 몬스터 매니져와 합치는게 좋지않을까.
    {
        ~StageManager()
        {
            sexybacklog.Console("StageManager 소멸");
        }

        // 저장되야할 데이터.
        public string MapID = "Map01";
        public bool isBonused = false;
        public double CurrentGameTime;
        public int CurrentFloor;
        public List<Stage> Stages; // 보이는 Stage, 몬스터와 배경만 바꿔가며 polling을 한다.        

        [NonSerialized]
        public static readonly int DistancePerFloor = 30;
        [NonSerialized]
        public static readonly int MonsterCountPerFloor= 10;
        [NonSerialized]
        public static int MaxFloor;
        [NonSerialized]
        public static int LimitGameTime;
        [NonSerialized]
        public List<Stage> beToDispose = new List<Stage>(); // 풀링하지말자. 잦은이동이있는것도아닌데
        [NonSerialized]
        private bool needNextStage = false;
        [NonSerialized]
        StageFactory Factory = new StageFactory();

        public void Init(string mapid, bool bonus)
        {
            MapID = mapid;
            isBonused = bonus;
        }

        internal void Start() // start, no Load
        {
            sexybacklog.Console(MapID);
            MapData map = Singleton<TableLoader>.getInstance().mapTable[MapID];
            MaxFloor = map.MaxFloor;
            LimitGameTime = map.LimitTime;

            CurrentFloor = 0;
            CurrentGameTime = 0;

            Stages = new List<Stage>();
            Stages.Add(Factory.CreateStage(CurrentFloor, DistancePerFloor));
            Stages.Add(Factory.CreateStage(CurrentFloor+1, DistancePerFloor * 2));
        }

        internal void Load(StageManager data) // 클래스가 로드가 된뒤 셋해야 할것들.
        {
            MapID = data.MapID;
            isBonused = data.isBonused;
            sexybacklog.Console(data.MapID);
            MapData map = Singleton<TableLoader>.getInstance().mapTable[data.MapID];
            MaxFloor = map.MaxFloor;
            LimitGameTime = map.LimitTime;

            CurrentFloor = data.CurrentFloor;
            CurrentGameTime = data.CurrentGameTime;

            Stages = new List<Stage>(); // 보이는 Stage, 몬스터와 배경만 바꿔가며 polling을 한다. 
            foreach( Stage stagedata in data.Stages)
            {
                Stages.Add(Factory.LoadStage(stagedata));
            }
        }

        public void onStagePass(int floor)
        {
            CurrentFloor = floor + 1;
        }
        public void onStageClear(Stage stage)
        {
            beToDispose.Add(stage);
            needNextStage = true;
            if(stage.type == StageType.LastPortal)
            {
                Singleton<GameManager>.getInstance().EndGame(true);
            }
        }

        public void Update()
        {
            CurrentGameTime += Time.deltaTime;
            sexybacklog.InGame("총시간 = " + (int)CurrentGameTime + " 초");
            sexybacklog.InGame("빨리감기 +" + Singleton<GameInput>.getInstance().fowardtimefordebug + " 초");

            if (needNextStage)
            {
                if(CurrentFloor <= MaxFloor)
                    Stages.Add(Factory.CreateStage(CurrentFloor + 1 , DistancePerFloor));
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