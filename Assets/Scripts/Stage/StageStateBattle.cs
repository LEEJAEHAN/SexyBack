using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class StageStateBattle : BaseState<Stage>
    {
        bool DuringBattle = false;
        Monster BattleMonster;

        public StageStateBattle(Stage owner, StateMachine<Stage> statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            owner.zPosition = 0;
            owner.avatar.transform.localPosition = Vector3.zero;
            Singleton<HeroManager>.getInstance().GetHero().ChangeState("Ready");
        }

        public void BattleStart() // 사거리내에 들어옴. battle 시작. 
        {
            DuringBattle = true;
            BattleMonster = Singleton<MonsterManager>.getInstance().GetMonster(owner.monsterID);
            BattleMonster.StateMachine.Action_changeEvent += onTargetStateChange;
            Singleton<ElementalManager>.getInstance().SetTarget(BattleMonster);
            Singleton<HeroManager>.getInstance().SetTarget(BattleMonster);
            Singleton<MonsterManager>.getInstance().JoinBattle(owner.monsterID);
        }

        public void onTargetStateChange(string monsterid, string stateID)
        {
            if (stateID == "Death")
            {
                DuringBattle = false;
                BattleMonster.StateMachine.Action_changeEvent -= onTargetStateChange;
                owner.monsterID = null;
            }
        }

        internal override void End()
        {
            owner.monsterID = null;
            if(BattleMonster.StateMachine != null)
                BattleMonster.StateMachine.Action_changeEvent -= onTargetStateChange;
            BattleMonster = null;
        }

        internal override void Update()
        {
            if(!DuringBattle && owner.monsterID != null)
            {
                BattleStart();
            }

            if(!DuringBattle && owner.monsterID == null)  // battle end
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