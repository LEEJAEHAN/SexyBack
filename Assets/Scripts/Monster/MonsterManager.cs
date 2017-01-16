using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    // 몬스터와 관련된 입력을 처리
    public class MonsterManager
    {
        public delegate void monsterCreateEvent_Handler(SexyBackMonster sender);
        public event monsterCreateEvent_Handler whenMonsterCreate;
         
        Dictionary<string, MonsterData> monsterDatas = new Dictionary<string, MonsterData>();
        SexyBackMonster Monster;

        UILabel label_monsterhp = ViewLoader.label_monsterhp.GetComponent<UILabel>();

        internal void Init()
        {
            // init data
            LoadData();
        }

        private void LoadData()
        {
            MonsterData dummy = new MonsterData(new BigInteger(1000, Digit.b));
            monsterDatas.Add("TestMonster", dummy);
        }

        internal void Start()
        {
            CreateMonster(monsterDatas["TestMonster"]);
        }

        private void CreateMonster(MonsterData data)
        {
            Monster = new SexyBackMonster(data.MaxHP);
            whenMonsterCreate(Monster);
        }

        internal void Update()
        {
            Monster.Update();
        }

        internal SexyBackMonster GetMonster()
        {
            return Monster;
        }

        public void OnHit(Collider collider)
        {

        }

        internal void UpdateHp(SexyBackMonster monster)
        {
            string hp = monster.HP.ToSexyBackString();
            string maxhp = monster.MAXHP.ToSexyBackString();

            label_monsterhp.text = hp + " / " + maxhp;
        }
    }
}