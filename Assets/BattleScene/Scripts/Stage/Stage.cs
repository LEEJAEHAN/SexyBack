﻿using System;
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
        public List<string> monsters;

        [NonSerialized]
        public StageType type;
        [NonSerialized]
        public GameObject avatar;
        [NonSerialized]
        internal StageStateMachine StateMachine;

        public string GetID { get { return "F" + floor; } }

        public Stage()
        {
            StateMachine = new StageStateMachine(this);
            monsters = new List<string>();
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

        public void MakeMonsters(int count)
        {
            MonsterManager mManager = Singleton<MonsterManager>.getInstance();

            for (int i = 0; i < count; i++)
            {
                if (i == count - 1)
                    monsters.Add(mManager.CreateRandomMonster("F" + floor + "M" + i, floor, true));
                else
                    monsters.Add(mManager.CreateRandomMonster("F" + floor + "M" + i, floor, false));
            }
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