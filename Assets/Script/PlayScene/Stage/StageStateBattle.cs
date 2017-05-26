using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class StageStateBattle : BaseState<Stage>
    {
        bool DuringBattle = false;
        MonsterManager mManager;

        public StageStateBattle(Stage owner, StateMachine<Stage> statemachine) : base(owner, statemachine)
        {
            mManager = Singleton<MonsterManager>.getInstance();
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
            int sequence = StageManager.MonsterPerStage - owner.monsterCount + 1;
            bool isBoss = StageManager.MonsterPerStage == sequence;

            mManager.JoinBattle(owner.floor, sequence, isBoss, owner.avatar.transform.FindChild("monster"));

            Monster BattleMonster = mManager.GetMonster();
            BattleMonster.StateMachine.Action_changeEvent += onTargetStateChange;
            Singleton<ElementalManager>.getInstance().SetTarget(BattleMonster);
            Singleton<HeroManager>.getInstance().SetTarget(BattleMonster);
        }

        public void onTargetStateChange(string monsterid, string stateID)
        {
            if (stateID == "Flying")
            {
                owner.monsterCount--;
            }
            if (stateID == "Death")
            {
                mManager.GetMonster().StateMachine.Action_changeEvent -= onTargetStateChange;
                mManager.EndBattle();
                DuringBattle = false;
            }
        }

        internal override void End()
        {
        }

        internal override void Update()
        {
            if(!DuringBattle && owner.monsterCount > 0)
            {
                BattleStart();
            }

            if(!DuringBattle && owner.monsterCount <= 0)  // battle end
            {
                NextState();
            }
        }

        void NextState()
        {
            if (owner.floor == StageManager.MaxFloor)
            {
                sexybacklog.Console("마지막층도달!");
            }

            Singleton<HeroManager>.getInstance().GetHero().ChangeState("Move");
            owner.ChangeState("PostMove");
        }

    }
}