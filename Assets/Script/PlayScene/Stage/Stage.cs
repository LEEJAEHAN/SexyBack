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
        public bool rewardComplete;
        public string savedState; // TODO : 이거빼는게좋을듯
        public int monsterCount = 0;

        [NonSerialized]
        public StageType type;
        [NonSerialized]
        public GameObject avatar;
        [NonSerialized]
        internal StageStateMachine StateMachine;

        public string GetID { get { return "F" + floor; } }
        public string CurrentState { get { return StateMachine.currStateID; } }

        public Stage()
        {
            StateMachine = new StageStateMachine(this);
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

        public void MakeMonsters()
        {
            monsterCount = Singleton<MonsterManager>.getInstance().CreateStageMonster(floor);
        }

        ~Stage() { sexybacklog.Console("스테이지소멸!"); }
    }

    public enum StageType
    {
        Normal,
        FirstPortal,
        LastPortal
    }

}