using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class Stage : IDisposable
    {
        public int floor = 0;
        public float zPosition = 0;
        public GameObject avatar;
        List<String> monsters = new List<string>();

        public delegate void StageClear_EventHandler(Stage stage);
        public event StageClear_EventHandler Action_StageClear;

        public Stage(int currentFloor, float zPosition)
        {
            floor = currentFloor;
            this.zPosition = zPosition;
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
            monster.StateMachine.Action_changeEvent += onTargetStateChange;
            monster.avatar.transform.parent = avatar.transform.FindChild("monster");
            monster.avatar.transform.localPosition = Vector3.zero;
            monsters.Add(monster.ID);
        }

        public void onTargetStateChange(string monsterid, string stateID)
        {
            if (stateID == "Death")
                monsters.Remove(monsterid);
        }

        public void Move(float delta_z)
        {
            zPosition -= delta_z;
            avatar.transform.localPosition -= GameSetting.EyeLine * (delta_z / 10); // 벽이 다가온다
            if (zPosition < 0 && monsters.Count > 0)
            {
                zPosition = 0;
                avatar.transform.localPosition = Vector3.zero;
                Singleton<MonsterManager>.getInstance().Battle(monsters[0]);
            }
            if(zPosition <= -10 && monsters.Count == 0) // clear stage
            {
                Action_StageClear(this);
            }
        }

        internal void Update()
        {
        }

        public void Dispose()
        {
            GameObject.Destroy(avatar);
            if (monsters.Count > 0)
                foreach (string monsterid in monsters)
                    Singleton<MonsterManager>.getInstance().GetMonster(monsterid).StateMachine.Action_changeEvent -= onTargetStateChange;
            monsters = null;
        }

        ~Stage()
        {
            //sexybacklog.Console("stage소멸!");
        }
    }
}