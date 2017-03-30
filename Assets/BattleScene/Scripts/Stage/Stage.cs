using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    [Serializable]
    internal class Stage : IDisposable, IStateOwner
    {
        public int floor;
        public float zPosition;
        public bool isLastStage = false;
        public bool rewardComplete;
        public string savedState; // TODO : 이거빼는게좋을듯
        public List<string> monsters;

        [NonSerialized]
        public GameObject avatar;
        [NonSerialized]
        internal StageStateMachine StateMachine;

        public string GetID { get { return "F" + floor; } }
        public Stage(int currentFloor, float zPosition, bool isLast, bool rewardComplete, List<string> monsters)
        {
            floor = currentFloor;
            this.zPosition = zPosition;
            isLastStage = isLast;
            this.rewardComplete = rewardComplete;

            InitAvatar();
            this.monsters = monsters;
            StateMachine = new StageStateMachine(this);
            ChangeState("Move");
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

        public void ChangeState(string stateid)
        {
            savedState = stateid;
            StateMachine.ChangeState(stateid);
        }
        ~Stage() { sexybacklog.Console("스테이지소멸!"); }
    }
}