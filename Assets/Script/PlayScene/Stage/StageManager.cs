using System;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Linq;

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
        public int lastClearedFloor;
        [NonSerialized]
        public static int MaxFloor;

        public List<Stage> Stages; // 보이는 Stage, 몬스터와 배경만 바꿔가며 polling을 한다.        

        [NonSerialized]
        public static readonly int DistancePerFloor = 30;
        [NonSerialized]
        public List<Stage> beToDispose = new List<Stage>(); // 풀링하지말자. 잦은이동이있는것도아닌데
        [NonSerialized]
        private bool needNextStage = false;

        [NonSerialized]
        StageFactory Factory = new StageFactory();

        public void Init()
        {
        }

        internal void Start(Map map) // start, no Load
        {
            // playscene 에서 시작할때를 위한 Test
            MaxFloor = map.baseData.MaxFloor;

            CurrentFloor = 1;       // 1층부터 세기시작.

            Stages = new List<Stage>();
            Stages.Add(Factory.CreateStage(0, 0));     // portal;
            Stages.Add(Factory.CreateStage(CurrentFloor, 0 + DistancePerFloor));   // firststage

            //Stages.Add(Factory.CreateStage(CurrentFloor, DistancePerFloor));
            //Stages.Add(Factory.CreateStage(CurrentFloor+1, DistancePerFloor * 2));
        }

        internal void Load(XmlDocument doc)
        {
            string mapID = doc.SelectSingleNode("InstanceStatus").Attributes["MapID"].Value;
            Map map = Singleton<MapManager>.getInstance().Maps[mapID];
            
            XmlNode stagesNode = doc.SelectSingleNode("InstanceStatus/Stages");

            MaxFloor = map.baseData.MaxFloor;
            CurrentFloor = int.Parse(stagesNode.Attributes["currentfloor"].Value);
            lastClearedFloor = int.Parse(stagesNode.Attributes["lastclearedfloor"].Value);

            Stages = new List<Stage>();
            foreach (XmlNode stageNode in stagesNode.ChildNodes)
            {
                Stages.Add(Factory.LoadStage(stageNode));
            }
        }

        public void onStagePass(Stage stage)
        {


            if (stage.type == StageType.FirstPortal)
            {
                //Singleton<ConsumableManager>.getInstance().MakeInitialChest();
            }
            if (stage.type == StageType.Normal)
            {
                CurrentFloor = stage.floor + 1;
            }
            if (stage.type == StageType.LastPortal)
            {
                Singleton<InstanceGameManager>.getInstance().EndGame(true);
            }

        }
        public void onStageClear(Stage stage)
        {
            beToDispose.Add(stage);
            lastClearedFloor = stage.floor;
            needNextStage = true;
        }

        public void Update()
        {
            if (needNextStage)
            {
                if (CurrentFloor <= MaxFloor)
                    Stages.Add(Factory.CreateStage(lastClearedFloor + 2, Stages.Max(x => x.zPosition) + DistancePerFloor));
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