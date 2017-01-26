using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    // 몬스터와 관련된 입력을 처리
    public class MonsterManager
    {
        MonsterHpBar hpbar;
        Dictionary<string, Monster> monsterPool = new Dictionary<string, Monster>();
         
        Dictionary<string, MonsterData> monsterDatas = new Dictionary<string, MonsterData>();
        Monster CurrentMonster; // TODO: bucket으로수정해야함;

        UILabel label_monsterhp = ViewLoader.label_monsterhp.GetComponent<UILabel>();

        public delegate void FocusChange_Event(Monster sender);
        public event FocusChange_Event Action_ChangeFocusEvent;

        public delegate void FocusMonsterChange_Event(Monster sender);
        public event FocusMonsterChange_Event Action_FocusMonsterChange;


        internal void Init()
        {
            LoadData();
            hpbar = new MonsterHpBar(this);

            Singleton<ElementalManager>.getInstance().noticeElementalCreate += onElementalCreate;
            Singleton<HeroManager>.getInstance().noticeHeroCreate += onHeroCreate;
        }

        public void onChangeMonster(Monster sender)
        {
            if (CurrentMonster.ID == sender.ID)
                Action_FocusMonsterChange(sender);
        }
        public void Start()
        {
            Monster newmonster = CreateMonster(monsterDatas["m02"]);
            monsterPool.Add(newmonster.ID, newmonster);
            Focus("m02");
        }
        void Focus(string Monsterid)
        {
            CurrentMonster = monsterPool[Monsterid];
            Action_ChangeFocusEvent(CurrentMonster);
            Action_FocusMonsterChange(CurrentMonster);
            CurrentMonster.StateMachine.ChangeState("Appear");
            CurrentMonster.isActive = true;
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

        private Monster CreateMonster(MonsterData data)
        {
            Monster TempMonster = new Monster(data);

            TempMonster.Action_MonsterChangeEvent += onChangeMonster;
            TempMonster.Action_StateChangeEvent = onMonsterStateChange;

            return TempMonster;
            // this class is event listner
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

        //public void Hit(string monsterID, Vector3 hitPosition, BigInteger damage, bool isCritical)
        //{    // 어차피 한마리라 일단 id는 무시 // TODO : 바꺼야함
        //    if (CurrentMonster == null)
        //    {
        //        sexybacklog.Error();
        //        return;
        //    }
        //    CurrentMonster.Hit(hitPosition, damage, isCritical);
        //    noticeMonsterListChange(CurrentMonster);
        //}

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
        void onMonsterStateChange(string monsterid, string stateID)
        {
            Monster a = FindMonster(monsterid);
            if (stateID == "Appear")
                a.StateMachine.ChangeState("Ready");
        }

        private Monster FindMonster(string id)
        {
            return CurrentMonster;
        }
    }
}