using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    // 몬스터와 관련된 입력을 처리
    internal class MonsterManager
    {
        Dictionary<string, Monster> monsters = new Dictionary<string, Monster>();
        Queue<string> disposeIDs= new Queue<string>();

        Monster BattleMonster; // TODO: bucket으로수정해야함;

        MonsterFactory monsterFactory = new MonsterFactory();
        MonsterHpBar hpbar;

        internal void Init()
        {
            hpbar = new MonsterHpBar(this);
            Singleton<ElementalManager>.getInstance().Action_ElementalCreateEvent += onElementalCreate;
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += onHeroCreate;
        }

        public void onChangeMonster(Monster sender)
        {
            if (BattleMonster.GetID == sender.GetID)
                hpbar.UpdateBar(sender);
        }
        internal Monster CreateMonster(int floor)
        {
            Monster newmonster = monsterFactory.CreateRandomMonster(floor);

            newmonster.Action_MonsterChangeEvent += onChangeMonster;
            monsters.Add(newmonster.GetID, newmonster);

            return newmonster;
            //Focus(newmonster);
        }

        public void JoinBattle(Monster monster) // 사거리내에 들어옴. battle 시작. 
        {   // TODO : 몬스터매니져가 왜 배틀을 주관하는지? 다른곳으로빠져야할듯. 마찬가지로 몬스터 죽음을 이용하여 너무 많은 컨트롤을 함.
            BattleMonster = monster;
            hpbar.FillNewBar(BattleMonster);
            hpbar.UpdateBar(BattleMonster);
            BattleMonster.Join();            // 여기가 실제 monstermanager의 기능.
        }

        internal void FixedUpdate()
        {
            hpbar.FixedUpdate();
        }

        internal void Update()
        {
            foreach( Monster m in monsters.Values)
                m.Update();

            while(disposeIDs.Count!=0)
            {
                string id = disposeIDs.Dequeue();
                if (id == BattleMonster.GetID)
                    BattleMonster = null;
                monsters[id].Dispose();
                monsters.Remove(id);
            }
        }
        internal void DestroyMonster(Monster owner)
        {
            disposeIDs.Enqueue(owner.GetID);
        }
        internal Monster GetMonster(string id)
        {
            if (!monsters.ContainsKey(id))
                return null;
            return monsters[id];
        }
        private void onElementalCreate(Elemental elemental)
        {
            if (BattleMonster == null)
                return;
            BattleMonster.StateMachine.Action_changeEvent += elemental.onTargetStateChange;
            if (BattleMonster.CurrentState == "Ready")
                elemental.targetID = BattleMonster.GetID;
        }
        private void onHeroCreate(Hero hero)
        {
            if (BattleMonster == null)
                return;
            BattleMonster.StateMachine.Action_changeEvent += hero.onTargetStateChange;
            //hero.SetDirection(CurrentMonster.CenterPosition);
        }
    }
}