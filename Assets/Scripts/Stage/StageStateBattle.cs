using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class StageStateBattle : BaseState<Stage>
    {
        Monster BattleMonster;
        bool DuringBattle = false;

        public StageStateBattle(Stage owner, StateMachine<Stage> statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            owner.zPosition = 0;
            owner.avatar.transform.localPosition = Vector3.zero;
        }

        public void BattleStart() // 사거리내에 들어옴. battle 시작. 
        {   // TODO : 몬스터매니져가 왜 배틀을 주관하는지? 다른곳으로빠져야할듯. 마찬가지로 몬스터 죽음을 이용하여 너무 많은 컨트롤을 함.
            DuringBattle = true;

            BattleMonster = owner.monsterQueue.Dequeue();
            BattleMonster.StateMachine.Action_changeEvent += onTargetStateChange;

            Singleton<ElementalManager>.getInstance().SetTarget(BattleMonster);
            Singleton<HeroManager>.getInstance().SetTarget(BattleMonster);
            Singleton<MonsterManager>.getInstance().JoinBattle(BattleMonster);

            // 몬스터죽음이벤트받음
            // 배틀엔드 스테이트가됨,
            // hero 언포커스몬스터
            // elemental언포커스 몬스터

        }

        public void onTargetStateChange(string monsterid, string stateID)
        {
            if (stateID == "Death")
            {
                BattleMonster.StateMachine.Action_changeEvent -= onTargetStateChange;
                BattleMonster = null;
                DuringBattle = false;
            }
        }

        internal override void End()
        {
            while(owner.monsterQueue.Count > 0)
                owner.monsterQueue.Dequeue().StateMachine.Action_changeEvent -= onTargetStateChange;

        }

        internal override void Update()
        {
            if(!DuringBattle && owner.monsterQueue.Count > 0)
            {
                BattleStart();
            }

            if(!DuringBattle && owner.monsterQueue.Count == 0)
            {
                //ViewLoader.Reward_PopUp.SetActive(true);
                stateMachine.ChangeState("Move");
                Singleton<HeroManager>.getInstance().GetHero().ChangeState("Move");
            }
        }
        
    }
}