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
        {
            DuringBattle = true;

            BattleMonster = owner.monsterQueue.Dequeue();
            BattleMonster.StateMachine.Action_changeEvent += onTargetStateChange;

            Singleton<ElementalManager>.getInstance().SetTarget(BattleMonster);
            Singleton<HeroManager>.getInstance().SetTarget(BattleMonster);
            Singleton<MonsterManager>.getInstance().JoinBattle(BattleMonster);
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

            if(!DuringBattle && owner.monsterQueue.Count == 0)  // battle end
            {
                NextState();

            }
        }
        void NextState()
        {
            if (owner.isLastStage)
            {
                return;
            }

            Singleton<HeroManager>.getInstance().GetHero().ChangeState("Move");
            Singleton<TalentManager>.getInstance().ShowNewTalentWindow(owner.floor);
            stateMachine.ChangeState("PostMove");
        }

    }
}