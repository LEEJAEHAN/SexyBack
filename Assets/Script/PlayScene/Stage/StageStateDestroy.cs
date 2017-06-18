using System;

namespace SexyBackPlayScene
{
    internal class StageStateDestroy : BaseState<Stage>
    {
        bool flipflop = true;

        public StageStateDestroy(Stage owner, StateMachine<Stage> statemachine) : base(owner, statemachine)
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
            if(owner.type == StageType.LastPortal)
            {
                if (flipflop)
                {
                    Singleton<HeroManager>.getInstance().GetHero().Action_DistanceChange -= onHeroMove;
                    Singleton<StageManager>.getInstance().onStageDestroy(owner);
                    flipflop = false;
                    return;
                }
            }

            if (owner.zPosition <= -20f) // remove stage
            {
                if(flipflop)
                {
                    Singleton<HeroManager>.getInstance().GetHero().Action_DistanceChange -= onHeroMove;
                    Singleton<StageManager>.getInstance().onStageDestroy(owner);
                    flipflop = false;
                }
            }
        }
    }
}