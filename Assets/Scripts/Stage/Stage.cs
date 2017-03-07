using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class Stage : IDisposable, IStateOwner
    {
        public int floor = 0;
        public float zPosition = 0;
        public bool isLastStage = false;
        public bool HasReward = true;

        public GameObject avatar;
        public Queue<Monster> monsterQueue = new Queue<Monster>();
        internal StageStateMachine StateMachine;

        public string GetID { get { return "F" + floor; } }
        public string CurrentState { get { return StateMachine.currStateID; } }

        public Stage(int currentFloor, float zPosition)
        {
            floor = currentFloor;
            this.zPosition = zPosition;
            StateMachine = new StageStateMachine(this);
        }

        internal void InitAvatar()
        {
            avatar = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/stage"));
            avatar.transform.parent = ViewLoader.StagePanel.transform;
            avatar.name = "stage" + floor.ToString();
            avatar.transform.localPosition = GameSetting.EyeLine * (zPosition / 10);
            // TODO : 배경 스킨 chaange
        }

        internal void CreateMonster()
        {
            Monster monster = Singleton<MonsterManager>.getInstance().CreateMonster(floor);
            monster.avatar.transform.parent = avatar.transform.FindChild("monster");
            monster.avatar.transform.localPosition = Vector3.zero;
            monsterQueue.Enqueue(monster);
        }

        internal void Update()
        {
            StateMachine.Update();
        }

        public void Dispose()
        {
            GameObject.Destroy(avatar);
            monsterQueue = null;
        }

        ~Stage() { sexybacklog.Console("스테이지소멸!"); }
    }
}