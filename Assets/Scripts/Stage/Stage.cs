using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    [Serializable]
    internal class Stage : IDisposable, IStateOwner
    {
        public int floor = 0;
        public float zPosition = 0;
        public bool isLastStage = false;
        public bool rewardComplete;
        public string CurrentState { get { return StateMachine.currStateID; } }
        public string monsterID;

        [NonSerialized]
        public GameObject avatar;
        internal StageStateMachine StateMachine;

        public string GetID { get { return "F" + floor; } }

        public Stage(int currentFloor, float zPosition, bool isLast, bool rewardComplete, string MonsterID)
        {
            floor = currentFloor;
            this.zPosition = zPosition;
            isLastStage = isLast;
            this.rewardComplete = rewardComplete;

            InitAvatar();

            this.monsterID = MonsterID;
            Monster monster = Singleton<MonsterManager>.getInstance().GetMonster(MonsterID);
            monster.avatar.transform.parent = avatar.transform.FindChild("monster");
            monster.avatar.transform.localPosition = Vector3.zero;

            StateMachine = new StageStateMachine(this);
            StateMachine.ChangeState("Move");
        }

        internal void InitAvatar()
        {
            avatar = ViewLoader.InstantiatePrefab(ViewLoader.stagepanel.transform, "stage" + floor.ToString(), "Prefabs/stage");
            avatar.transform.localPosition = GameCameras.EyeLine * (zPosition / 10);
            // TODO : 배경 스킨 chaange
        }

        internal void Update()
        {
            StateMachine.Update();
        }

        public void Dispose()
        {
            GameObject.Destroy(avatar);
        }

        ~Stage() { sexybacklog.Console("스테이지소멸!"); }
    }
}