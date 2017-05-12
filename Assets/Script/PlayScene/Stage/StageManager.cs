using System;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

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
        //public string MapID;
        public int CurrentFloor;
        [NonSerialized]
        public static int MaxFloor;
        public List<Stage> Stages; // 보이는 Stage, 몬스터와 배경만 바꿔가며 polling을 한다.        

        [NonSerialized]
        public static readonly int DistancePerFloor = 30;
        [NonSerialized]
        public static readonly int MonsterCountPerFloor= 3;
        [NonSerialized]
        public List<Stage> beToDispose = new List<Stage>(); // 풀링하지말자. 잦은이동이있는것도아닌데
        [NonSerialized]
        private bool needNextStage = false;
        [NonSerialized]
        StageFactory Factory = new StageFactory();

        public void Init()
        {
        }

        internal void Start(string MapID) // start, no Load
        {
            // playscene 에서 시작할때를 위한 Test
            MaxFloor = Singleton<TableLoader>.getInstance().mapTable[MapID].MaxFloor;
            CurrentFloor = 0;

            Stages = new List<Stage>();
            Stages.Add(Factory.CreateStage(CurrentFloor, DistancePerFloor));
            Stages.Add(Factory.CreateStage(CurrentFloor+1, DistancePerFloor * 2));
        }

        internal void Load(XmlDocument doc)
        {
            string mapID = doc.SelectSingleNode("InstanceStatus").Attributes["MapID"].Value;
            XmlNode stagesNode = doc.SelectSingleNode("InstanceStatus/Stages");

            MaxFloor = Singleton<TableLoader>.getInstance().mapTable[mapID].MaxFloor;
            CurrentFloor = int.Parse(stagesNode.Attributes["currentfloor"].Value);
            Stages = new List<Stage>();
            foreach (XmlNode stageNode in stagesNode.ChildNodes)
            {
                Stages.Add(Factory.LoadStage(stageNode));
            }
        }

        public void onStagePass(Stage stage)
        {
            CurrentFloor = stage.floor + 1;
            if (stage.type == StageType.LastPortal)
            {
                Singleton<GameManager>.getInstance().EndGame(true);
            }

        }
        public void onStageClear(Stage stage)
        {
            beToDispose.Add(stage);
            needNextStage = true;
        }

        public void Update()
        {
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