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
        public int CurrentFloor;
        public int lastClearedFloor;
        public int Combo;
        public int NextBattleStage;
        public bool needNextStage = false;
        public double BattleTime;
        StageFactory Factory = new StageFactory();

        public BigInteger NextMonsterHP = new BigInteger(0);
        public List<Stage> Stages; // 보이는 Stage, 몬스터와 배경만 바꿔가며 polling을 한다.        

        public List<Stage> beToDispose = new List<Stage>(); // 풀링하지말자. 잦은이동이있는것도아닌데
        public const int DistancePerFloor = 30;

        bool TimerOn = false;

        internal void MakeOrLoadMonster(int floor)
        {   // 로드된게 있으면 두고 없으면 만든다.
            if (Singleton<MonsterManager>.getInstance().monsters.Count == 0 && NextMonsterHP > 0)
            {
                Singleton<MonsterManager>.getInstance().CreateStageMonster(floor, NextMonsterHP);
                NextMonsterHP = new BigInteger(0);
            }
        }
        public void Init()
        {
        }

        internal void Start(Map map) // start, no Load
        {// playscene 에서 시작할때를 위한 Test
            CurrentFloor = 1;       // 1층부터 세기시작.
            NextBattleStage = 1;
            NextMonsterHP = map.baseData.MapMonster.HP[0];
            Stages = new List<Stage>();
            Stages.Add(Factory.CreateStage(0, 0));     // portal;
            Stages.Add(Factory.CreateStage(CurrentFloor, 0 + DistancePerFloor));   // firststage
            //Stages.Add(Factory.CreateStage(CurrentFloor, DistancePerFloor));
            //Stages.Add(Factory.CreateStage(CurrentFloor+1, DistancePerFloor * 2));
        }

        internal void Load(XmlDocument doc)
        {
            string mapID = doc.SelectSingleNode("InstanceStatus").Attributes["MapID"].Value;
            XmlNode stagesNode = doc.SelectSingleNode("InstanceStatus/Stages");
            CurrentFloor = int.Parse(stagesNode.Attributes["currentfloor"].Value);
            lastClearedFloor = int.Parse(stagesNode.Attributes["lastclearedfloor"].Value);
            NextBattleStage = int.Parse(stagesNode.Attributes["nextbattlestage"].Value);
            NextMonsterHP = new BigInteger(stagesNode.Attributes["nextmonsterhp"].Value);
            Combo = int.Parse(stagesNode.Attributes["combo"].Value);
            needNextStage = bool.Parse(stagesNode.Attributes["neednextstage"].Value);
            BattleTime = double.Parse(stagesNode.Attributes["battletime"].Value);

            Stages = new List<Stage>();
            foreach (XmlNode stageNode in stagesNode.ChildNodes)
            {
                Stages.Add(Factory.LoadStage(stageNode));
            }
        }


        public void onStagePass(Stage stage)
        {
            lastClearedFloor = stage.floor;
            if (stage.type == StageType.FirstPortal)
            {
                needNextStage = true;
                //Singleton<ConsumableManager>.getInstance().MakeInitialChest();
            }
            if (stage.type == StageType.Normal)
            {
                CurrentFloor = stage.floor + 1;
                needNextStage = true;

                MapMonsterData info = Singleton<InstanceStatus>.getInstance().InstanceMap.baseData.MapMonster;
                int hpCoef = (CurrentFloor % info.BossTerm == 0) ? 1 : 0;
                int level = CurrentFloor * info.LevelPerFloor;
                double growth = InstanceStatus.CalInstanceGrowth(level); // 2, level
                NextMonsterHP += BigInteger.FromDouble(growth * info.HP[hpCoef]);
            }
            if (stage.type == StageType.LastPortal)
            {
                Singleton<InstanceGameManager>.getInstance().EndGame(true);
            }
        }

        internal void BattleStart(Stage stage)
        {
            Singleton<HeroManager>.getInstance().GetHero().ChangeState("Ready");
            Singleton<MonsterManager>.getInstance().JoinBattle(stage.floor, stage.avatar.transform.FindChild("monster"));
            //BattleMonster = mManager.GetBattleMonster();
            Monster battlemonster = Singleton<MonsterManager>.getInstance().GetBattleMonster();
            if (battlemonster == null)
                return;

            battlemonster.StateMachine.Action_changeEvent += onTargetStateChange;
            Singleton<ElementalManager>.getInstance().SetTarget(battlemonster);
            Singleton<HeroManager>.getInstance().SetTarget(battlemonster);
        }
        internal void BattleEnd()
        {
            if (BattleTime <= 5)
            {
                // Combo 상승.
                Combo++;
                Singleton<HeroManager>.getInstance().GetHero().ChangeState("FastMove");
            }
            else if( BattleTime <= 50)
            {
                Combo = (Combo >= 6 ? 6 : (Combo >= 3 ? 3 : 0));
                // Combo 변함없음.
                Singleton<HeroManager>.getInstance().GetHero().ChangeState("FastMove");
            }
            else
            {
                //Combo 전단계로.
                Combo = Combo >= 6 ? 3 : 0;
                Singleton<HeroManager>.getInstance().GetHero().ChangeState("Move");
            }

            if (Combo < 3)
                NextBattleStage = CurrentFloor + 1;
            else if (Combo < 6)
                NextBattleStage = ((CurrentFloor + 10) / 10) * 10;
            else
                NextBattleStage = ((CurrentFloor + 100) / 100) * 100;

            BattleTime = 0;
            TimerOn = false;
        }

        public void onTargetStateChange(string monsterid, string stateID)
        {
            if (stateID == "Ready")
            {
                TimerOn = true;
            }
            if (stateID == "Flying")
            {
                TimerOn = false;
            }
            if (stateID == "Death")
            {
                Singleton<MonsterManager>.getInstance().GetBattleMonster().StateMachine.Action_changeEvent -= onTargetStateChange;
                Singleton<MonsterManager>.getInstance().EndBattle();
            }
        }

        public void onStageDestroy(Stage stage)
        {
            beToDispose.Add(stage);
        }

        public void Update()
        {
            if (TimerOn)
            {
                BattleTime += Time.deltaTime;
                sexybacklog.InGame(string.Format("전투시간 {0:N3}", BattleTime));
            }

            if (needNextStage)
            {
                //if (CurrentFloor <= Singleton<InstanceStatus>.getInstance().InstanceMap.baseData.MaxFloor)
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