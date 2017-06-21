using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class StageStateBattle : BaseState<Stage>
    {
        MonsterManager mManager;
        // 이개념을 몬스터에 넣을것인가 스테이지에 넣을것인가.

        public StageStateBattle(Stage owner, StateMachine<Stage> statemachine) : base(owner, statemachine)
        {
            mManager = Singleton<MonsterManager>.getInstance();
        }

        internal override void Begin()
        {
            Singleton<StageManager>.getInstance().MakeOrLoadMonster(owner.floor);// 미리 로드되있으면 있고, 없으면 만든다.
            owner.zPosition = 0;
            owner.avatar.transform.localPosition = Vector3.zero;
            Singleton<StageManager>.getInstance().BattleStart(owner);
        }

        internal override void End()
        {
        }

        internal override void Update()
        {
            if (mManager.GetBattleMonster() == null)  // 첨부터 없었거나 혹은 battle end
            {
                Singleton<StageManager>.getInstance().BattleEnd(owner);
                owner.ChangeState("PostMove");
            }

        }

    }
}