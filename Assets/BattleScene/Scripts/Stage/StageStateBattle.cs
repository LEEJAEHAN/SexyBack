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

        public void BattleStart(int index) // 사거리내에 들어옴. battle 시작. 
        {
            DuringBattle = true;

            int sequence = StageManager.MonsterCountPerFloor - owner.monsters.Count + 1;
            bool isBoss = StageManager.MonsterCountPerFloor == sequence;

            mManager.JoinBattle(owner.monsters[index], owner.floor, sequence, isBoss, owner.avatar.transform.FindChild("monster"));

            Monster BattleMonster = mManager.GetMonster(owner.monsters[index]);
            BattleMonster.StateMachine.Action_changeEvent += onTargetStateChange;
            Singleton<ElementalManager>.getInstance().SetTarget(BattleMonster);
            Singleton<HeroManager>.getInstance().SetTarget(BattleMonster);
        }

        public void onTargetStateChange(string monsterid, string stateID)
        {
            if (stateID == "Flying")
            {
                owner.monsters.Remove(monsterid);
            }
            if(stateID == "Death")
            {
                mManager.GetMonster(monsterid).StateMachine.Action_changeEvent -= onTargetStateChange;
                DuringBattle = false;
            }
        }

        internal override void End()
        {
        }

        internal override void Update()
        {
            if(!DuringBattle && owner.monsters.Count > 0)
            {
                BattleStart(0);
            }

            if(!DuringBattle && owner.monsters.Count <= 0)  // battle end
            {
                NextState();
            }
        }

        void NextState()
        {
            if (owner.isLastStage)
            {
                sexybacklog.Console("마지막도달!");
                return;
            }

            owner.ChangeState("PostMove");
        }

    }
}