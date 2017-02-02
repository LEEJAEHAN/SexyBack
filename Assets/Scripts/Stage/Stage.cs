using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class Stage
    {
        public int floor = 0;
        float zPosition = 0;
        public GameObject avatar;
        List<String> monsters = new List<string>();

        public Stage(int currentFloor, float zPosition)
        {
            floor = currentFloor;
            this.zPosition = zPosition;
        }

        internal void InitAvatar()
        {
            avatar = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/stage"));
            avatar.transform.parent = ViewLoader.StagePanel.transform;
            avatar.transform.localPosition = GameSetting.EyeLine * (zPosition / 10);
            // TODO : 배경 스킨 chaange
        }

        internal void CreateMonster()
        {
            Monster monster = Singleton<MonsterManager>.getInstance().CreateMonster(floor);
            monster.Action_StateChangeEvent = onTargetStateChange;
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
        }

        internal void Update()
        {

        }
    }
}