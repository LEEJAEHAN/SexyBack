using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class StageStateMove : BaseState<Stage>
    {
        public StageStateMove(Stage owner, StateMachine<Stage> statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            Singleton<HeroManager>.getInstance().GetHero().Action_DistanceChange += onHeroMove;
        }

        internal override void End()
        {
            Singleton<HeroManager>.getInstance().GetHero().Action_DistanceChange -= onHeroMove;
        }

        public void onHeroMove(double delta_z)
        {
            owner.zPosition -= (float)delta_z;  // 벽이 다가온다
            owner.avatar.transform.localPosition = GameCameras.EyeLine * (owner.zPosition / 10);
        }

        internal override void Update()
        {
            if (owner.zPosition < 0)
            {
                if (owner.type == StageType.Normal && owner.floor == Singleton<StageManager>.getInstance().NextBattleStage)
                    owner.ChangeState("Battle");
                else
                    owner.ChangeState("PostMove");
            }
        }
    }
}