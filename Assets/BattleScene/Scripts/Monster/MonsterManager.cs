using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    [Serializable]
    // 몬스터와 관련된 입력을 처리
    internal class MonsterManager : IDisposable
    {
        ~MonsterManager()
        {
            sexybacklog.Console("MonsterManager 소멸");
        }

        Dictionary<string, Monster> monsters = new Dictionary<string, Monster>();

        [NonSerialized]
        Queue<string> disposeIDs= new Queue<string>();
        [NonSerialized]
        Monster BattleMonster; // TODO: bucket으로수정해야함;
        [NonSerialized]
        MonsterHpBar HpBar;
        [NonSerialized]
        MonsterFactory monsterFactory;

        internal void Init()
        {
            HpBar = new MonsterHpBar();
            monsterFactory = new MonsterFactory();
            Singleton<ElementalManager>.getInstance().Action_ElementalCreateEvent += onElementalCreate;
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += onHeroCreate;
        }
        internal void Load(MonsterManager monsterManager)
        {
            foreach (string id in monsterManager.monsters.Keys)
            {
                Monster data = monsterManager.monsters[id];
                if (data.HP <= 0)
                    continue;
                Monster monster = monsterFactory.LoadMonster(data.ID, data.DataID, data.level, data.HP);
                monsters.Add(id, monster);
            }
        }

        public void Dispose()
        {
            HpBar.Dispose();
            HpBar = null;
            Singleton<MonsterHpBar>.Clear();
            EffectController.Clear();
        }

        public void onHit(Monster sender)
        {
            if (BattleMonster.GetID == sender.GetID)
                HpBar.UpdateBar(sender);
        }
        //internal string CreateRandomMonster(int floor)
        //{
        //    Monster newmonster = monsterFactory.CreateRandomMonster(floor);
        //    monsters.Add(newmonster.GetID, newmonster);
        //    return newmonster.GetID;
        //}
        internal string CreateRandomMonster(string monsterID, int floor, bool isBoss)
        {
            Monster newmonster = monsterFactory.CreateRandomMonster(monsterID, floor, isBoss);
            monsters.Add(newmonster.GetID, newmonster);
            return newmonster.GetID;
        }

        public void JoinBattle(string monsterid, int floor, int sequence, bool isboss, Transform genTransform) // 사거리내에 들어옴. battle 시작. 
        {   // TODO : 몬스터매니져가 왜 배틀을 주관하는지? 다른곳으로빠져야할듯. 마찬가지로 몬스터 죽음을 이용하여 너무 많은 컨트롤을 함.
            BattleMonster = monsters[monsterid];
            BattleMonster.Spawn(genTransform);            // 여기가 실제 monstermanager의 기능.
            string HpBarName;
            if (isboss)
                HpBarName = "[" + floor.ToString() + "층 보스]" + BattleMonster.Name;
            else
                HpBarName = "[" + floor.ToString() + "층 " + sequence.ToString() + "단계] " + BattleMonster.Name;

            HpBar.FillNewBar(HpBarName, BattleMonster);
            //HpBar.UpdateBar(BattleMonster);
        }

        internal void FixedUpdate()
        {
            HpBar.FixedUpdate();
        }

        internal void Update()
        {
            foreach( Monster m in monsters.Values)
                m.Update();

            while (disposeIDs.Count!=0)
            {
                string id = disposeIDs.Dequeue();
                if (id == BattleMonster.GetID)
                    BattleMonster = null;
                monsters[id].Dispose();
                monsters.Remove(id);
            }
        }

        internal bool TestDeal(BigInteger damage)
        {
            if (BattleMonster != null)
                return BattleMonster.Hit(new Vector3(360, 800, 0), damage, false);
            return false;
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

        public int GetHPPercent()
        {
            return HpBar.GetPercent();
        }

    }
}