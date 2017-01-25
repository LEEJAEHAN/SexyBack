﻿using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    // 몬스터와 관련된 입력을 처리
    public class MonsterManager
    {
        public delegate void monsterCreateEvent_Handler(Monster sender);
        public event monsterCreateEvent_Handler noticeMonsterCreate;

        public delegate void monsterChangeEvent_Handler(Monster sender);
        public event monsterChangeEvent_Handler noticeMonsterChange;

        MonsterHpBar hpbar;

        Dictionary<string, MonsterData> monsterDatas = new Dictionary<string, MonsterData>();
        Monster CurrentMonster; // TODO: bucket으로수정해야함;

        UILabel label_monsterhp = ViewLoader.label_monsterhp.GetComponent<UILabel>();

        internal void Init()
        {
            LoadData();

            noticeMonsterChange += onMonsterChange;
            Singleton<ElementalManager>.getInstance().noticeElementalCreate += onElementalCreate;
            Singleton<HeroManager>.getInstance().noticeHeroCreate += onHeroCreate;
            ViewLoader.Background.GetComponent<background>().noticeHit += onHitByProjectile;
            
            hpbar = new MonsterHpBar(this);
        }

        public void Start()
        {
            CreateMonster(monsterDatas["m02"]);
        }

        private void LoadData()
        {
            monsterDatas.Add("m01", new MonsterData("m01", "m01", "Sprites/Monster/m01", new BigInteger(999999, Digit.k)));
            monsterDatas.Add("m02", new MonsterData("m02", "m02", "Sprites/Monster/m02", new BigInteger(99999900)));
            monsterDatas.Add("m03", new MonsterData("m03", "m03", "Sprites/Monster/m03", new BigInteger(999999000)));
            monsterDatas.Add("m04", new MonsterData("m04", "m04", "Sprites/Monster/m04", new BigInteger(1000, Digit.b)));
            monsterDatas.Add("m05", new MonsterData("m05", "m05", "Sprites/Monster/m05", new BigInteger(1000, Digit.b)));
            monsterDatas.Add("m06", new MonsterData("m06", "m06", "Sprites/Monster/m06", new BigInteger("123456123456")));
            monsterDatas.Add("m07", new MonsterData("m07", "m07", "Sprites/Monster/m07", new BigInteger(1000, Digit.b)));
            monsterDatas.Add("m08", new MonsterData("m08", "m08", "Sprites/Monster/m08", new BigInteger(1000, Digit.b)));
            monsterDatas.Add("m09", new MonsterData("m09", "m09", "Sprites/Monster/m09", new BigInteger(1000, Digit.b)));
            monsterDatas.Add("m10", new MonsterData("m10", "m00", "Sprites/Monster/m10", new BigInteger(1000, Digit.b)));
        }

        internal void FixedUpdate()
        {
            hpbar.FixedUpdate();
        }

        private void CreateMonster(MonsterData data)
        {
            CurrentMonster = new Monster(data);
            CurrentMonster.SetHitEvent = onHitByProjectile;
            CurrentMonster.SetStateEndEvent = onActionStateEnd;

            // this class is event listner

            noticeMonsterCreate(CurrentMonster);
            noticeMonsterChange(CurrentMonster);
        }

        internal void Update()
        {
            CurrentMonster.Update();
        }

        internal Monster GetMonster()
        {
            return CurrentMonster;
        }
        internal Monster GetMonster(string id)
        {
            return CurrentMonster; //TODO: 바꿔야함
        }

        internal void onMonsterChange(Monster monster)
        {
            string hp = monster.HP.To5String();
            string maxhp = monster.MAXHP.To5String();

            label_monsterhp.text = hp + " / " + maxhp;
        }

        public void Hit(string monsterID, Vector3 hitPosition, BigInteger damage, bool isCritical) // 어차피 한마리라 일단 id는 무시
        {
            if (CurrentMonster == null)
            {
                sexybacklog.Error();
                return;
            }
            CurrentMonster.Hit(hitPosition, damage, isCritical);
            noticeMonsterChange(CurrentMonster);
        }

        public void onHitByProjectile(string monsterID, Vector3 hitposition, string elementalID)
        {
            if (CurrentMonster == null)
            {
                sexybacklog.Error();
                return;
            }
            BigInteger damage = Singleton<ElementalManager>.getInstance().GetElementalDamage(elementalID);
            Hit(monsterID, hitposition, damage, false);
        }

        public Vector3 FindPosition(string mosterID)
        {
            return CurrentMonster.CenterPosition;
        }

        private void onElementalCreate(Elemental sender)
        {
            if (CurrentMonster == null)
                return;
            sender.target = CurrentMonster;
        }
        private void onHeroCreate(Hero hero)
        {
            if (CurrentMonster == null)
                return;
            hero.targetID = CurrentMonster.ID;
            //hero.SetDirection(CurrentMonster.CenterPosition);
        }
        void onActionStateEnd(string monsterid, string stateID)
        {
            if (stateID == "Appear")
                CurrentMonster.stateMachine.ChangeState("Ready");

        }

    }
}