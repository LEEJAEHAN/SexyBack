using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    // 몬스터와 관련된 입력을 처리
    public class MonsterManager
    {
        MonsterHpBar hpbar;
        //Dictionary<string, Monster> monsterPool = new Dictionary<string, Monster>();

        MonsterFactory monsterFactory = new MonsterFactory();
        Monster FocusMonster; // TODO: bucket으로수정해야함;

        public delegate void FocusChange_Event(Monster sender);
        public event FocusChange_Event Action_NewFousEvent;

        public delegate void FocusMonsterChange_Event(Monster sender);
        public event FocusMonsterChange_Event Action_TargetMonsterChange;

        bool dispose = false;

        internal void Init()
        {
            hpbar = new MonsterHpBar(this);
            Singleton<ElementalManager>.getInstance().Action_ElementalCreateEvent += onElementalCreate;
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += onHeroCreate;
        }

        public void onChangeMonster(Monster sender)
        {
            if (FocusMonster.ID == sender.ID)
                Action_TargetMonsterChange(sender);
        }
        public void Start()
        {
            // 나중에 이부분은 다 사라져도되어야한다.
            Monster newmonster = monsterFactory.CreateMonster("m02");
            newmonster.Action_MonsterChangeEvent += onChangeMonster;
            //monsterPool.Add(newmonster.ID, newmonster);
            Focus(newmonster);
            FocusMonster.Join();            /// Monster Join battle
        }
        void Focus(Monster monster)
        {
            FocusMonster = monster;
            Action_NewFousEvent(FocusMonster);
            Action_TargetMonsterChange(FocusMonster);
        }

        internal void FixedUpdate()
        {
            hpbar.FixedUpdate();
        }   

        internal void Update()
        { // TODO : all monster update;
            if(FocusMonster != null)
                FocusMonster.Update();

            if(dispose == true)
            {
                DestroyM();
            }
        }

        private void DestroyM()
        {
            FocusMonster.Dispose();
            FocusMonster = null;
            dispose = false;
        }
        internal void DestroyMonster(Monster owner)
        {
            if (dispose)
                return;
            sexybacklog.Console("디스트로이입장.");
            dispose = true;
        }

        internal Monster GetMonster()
        {
            return FocusMonster;
        }
        internal Monster GetMonster(string id)
        {
            return FocusMonster; //TODO: 바꿔야함
        }

        public Vector3 FindPosition(string mosterID)
        {
            return FocusMonster.CenterPosition;
        }

        private void onElementalCreate(Elemental elemental)
        {
            if (FocusMonster == null)
                return;
            FocusMonster.Action_StateChangeEvent = elemental.onTargetStateChange;
        }
        private void onHeroCreate(Hero hero)
        {
            if (FocusMonster == null)
                return;
            FocusMonster.Action_StateChangeEvent = hero.onTargetStateChange;
            //hero.SetDirection(CurrentMonster.CenterPosition);
        }
    }
}