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
         
        Dictionary<string, MonsterData> monsterDatas = new Dictionary<string, MonsterData>();

        Monster FocusMonster; // TODO: bucket으로수정해야함;

        public delegate void FocusChange_Event(Monster sender);
        public event FocusChange_Event Action_NewFousEvent;

        public delegate void FocusMonsterChange_Event(Monster sender);
        public event FocusMonsterChange_Event Action_TargetMonsterChange;


        internal void Init()
        {
            LoadData();
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
            Monster newmonster = CreateMonster(monsterDatas["m06"]);
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

        private void LoadData()
        {
            monsterDatas.Add("m01", new MonsterData("m01", "다람쥐 Lv.1", "Sprites/Monster/m01", new BigInteger(99999, Digit.k)));
            monsterDatas.Add("m02", new MonsterData("m02", "다람쥐 Lv.1", "Sprites/Monster/m02", new BigInteger(4444440000)));
            monsterDatas.Add("m03", new MonsterData("m03", "다람쥐 Lv.1", "Sprites/Monster/m03", new BigInteger(999999000)));
            monsterDatas.Add("m04", new MonsterData("m04", "다람쥐 Lv.1", "Sprites/Monster/m04", new BigInteger(1000, Digit.b)));
            monsterDatas.Add("m05", new MonsterData("m05", "다람쥐 Lv.1", "Sprites/Monster/m05", new BigInteger(999999, Digit.k)));
            monsterDatas.Add("m06", new MonsterData("m06", "다람쥐 Lv.1", "Sprites/Monster/m06", new BigInteger("777777777")));
            monsterDatas.Add("m07", new MonsterData("m07", "다람쥐 Lv.1", "Sprites/Monster/m07", new BigInteger(999999, Digit.k)));
            monsterDatas.Add("m08", new MonsterData("m08", "다람쥐 Lv.1", "Sprites/Monster/m08", new BigInteger(1000, Digit.b)));
            monsterDatas.Add("m09", new MonsterData("m09", "다람쥐 Lv.1", "Sprites/Monster/m09", new BigInteger(1000, Digit.b)));
            monsterDatas.Add("m10", new MonsterData("m10", "다람쥐 Lv.1", "Sprites/Monster/m10", new BigInteger(1000, Digit.b)));
        }

        internal void FixedUpdate()
        {
            hpbar.FixedUpdate();
        }

        private Monster CreateMonster(MonsterData data)
        {
            Monster TempMonster = new Monster(data);

            TempMonster.Action_MonsterChangeEvent += onChangeMonster;

            return TempMonster;
            // this class is event listner
        }

        internal void Update()
        { // TODO : all monster update;
            if(FocusMonster != null)
                FocusMonster.Update();

            if(deleteplan == true)
            {
                DestroyM();
            }
        }

        private void DestroyM()
        {
            FocusMonster.Dispose();
            FocusMonster = null;
            deleteplan = false;
        }

        internal void DestroyMonster(Monster owner)
        {
            sexybacklog.Console("디스트로이입장.");
            deleteplan = true;
        }
        bool deleteplan = false;


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