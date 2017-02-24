using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class StageStateMove : BaseState<Stage>
    {
        Hero bindHero;

        public StageStateMove(Stage owner, StateMachine<Stage> statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            bindHero = Singleton<HeroManager>.getInstance().GetHero();
            if (bindHero == null) // 히어로가없으면,
                Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += onHeroCreate;
            else
            {
                bindHero.Action_DistanceChange += onHeroMove;
            }
        }

        void onHeroCreate(Hero newHero)
        {
            bindHero = newHero;
            bindHero.Action_DistanceChange += onHeroMove;
        }

        internal override void End()
        {
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent -= onHeroCreate;
            bindHero.Action_DistanceChange -= onHeroMove;
            bindHero = null;
        }

        public void onHeroMove(double delta_z)
        {
            owner.zPosition -= (float)delta_z;  // 벽이 다가온다
            owner.avatar.transform.localPosition = GameSetting.EyeLine * (owner.zPosition / 10);
        }

        internal override void Update()
        {
            if (owner.zPosition < 0 && owner.monsterQueue.Count > 0)
            {
                bindHero.ChangeState("Ready");
                owner.StateMachine.ChangeState("Battle");
            }
        }
    }
}