using System;
using UnityEngine;
using System.Collections.Generic;
using System.Xml;

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

        public Queue<Monster> monsters = new Queue<Monster>();

        [NonSerialized]
        int DeadCount;      // ready to pop;
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
        internal void Load(XmlDocument doc)
        {
            XmlNode rootNode = doc.SelectSingleNode("InstanceStatus/Monsters");
            foreach (XmlNode node in rootNode.ChildNodes)
            {
                Monster monster = monsterFactory.LoadMonster(node);
                if (monster != null)
                    monsters.Enqueue(monster);
            }
        }

        internal int CreateStageMonster(int floor)
        {
            int count = 0;
            for (int i = 0; i < monsterFactory.MonsterPerStage; i++)
            {
                string id = string.Format("F{0}M{1}", floor, i);
                bool isBoss = (floor % monsterFactory.BossTerm  == 0);
                Monster newmonster = monsterFactory.CreateRandomMonster(id, floor, isBoss);
                monsters.Enqueue(newmonster);
                count++;
            }
            return count;
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


        public void JoinBattle(int floor, int remainMonsterCount, Transform genTransform) // 사거리내에 들어옴. battle 시작. 
        {
            BattleMonster = monsters.Peek();
            BattleMonster.Spawn(genTransform);            // 여기가 실제 monstermanager의 기능.
            string HpBarName;

            int sequence = monsterFactory.MonsterPerStage - remainMonsterCount + 1;
            
            if (BattleMonster.isBoss)
                HpBarName = string.Format("[{0}층 보스] {1}", floor, BattleMonster.Name);
            else
                HpBarName = string.Format("[{0}층 {1}단계] {2}", floor, sequence, BattleMonster.Name);
            HpBar.FillNewBar(HpBarName, BattleMonster);
        }
        internal void EndBattle()
        {
            BattleMonster = null;
        }

        internal void FixedUpdate()
        {
            HpBar.FixedUpdate();
        }

        internal void Update()
        {
            foreach (Monster m in monsters)
                m.Update();

            while (DeadCount > 0)
            {
                Monster DeadMonster = monsters.Dequeue();
                DeadMonster.Dispose();
                DeadCount--;
            }
        }

        internal bool TestDeal(BigInteger damage)
        {
            if (BattleMonster != null)
                return BattleMonster.Hit(new Vector3(360, 800, 0), damage, false);
            return false;
        }

        internal void DestroyFirstMonster()
        {
            DeadCount++;
        }
        internal Monster GetMonster()
        {
            return monsters.Peek();
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