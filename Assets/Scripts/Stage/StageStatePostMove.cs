using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class StageStatePostMove : BaseState<Stage>
    {
        bool talentChoice = false;
        bool waiting = false;

        public StageStatePostMove(Stage owner, StateMachine<Stage> statemachine) : base(owner, statemachine)
        {
        }

        void onTalentConfirm()
        {
            talentChoice = true;
        }

        internal override void Begin()
        {
            Singleton<HeroManager>.getInstance().GetHero().Action_DistanceChange += onHeroMove;
            Singleton<TalentManager>.getInstance().Action_ConfirmTalent += onTalentConfirm;
        }

        internal override void End()
        {
            Singleton<HeroManager>.getInstance().GetHero().Action_DistanceChange -= onHeroMove;
            Singleton<TalentManager>.getInstance().Action_ConfirmTalent -= onTalentConfirm;

        }

        public void onHeroMove(double delta_z)
        {
            owner.zPosition -= (float)delta_z;  // 벽이 다가온다
            owner.avatar.transform.localPosition = GameSetting.EyeLine * (owner.zPosition / 10);
            //owner.Move(delta_z);
        }

        internal override void Update()
        {
            if (waiting == false)
            {
                if (owner.zPosition <= GameSetting.HeroCamPosition.z + 1.5f) // change floortext
                {
                    Singleton<StageManager>.getInstance().onStagePass(owner.floor);
                }
                if (owner.zPosition <= GameSetting.HeroCamPosition.z) // talent wait // StageManager.DistancePerFloor - 20
                { //TODO: 계속 여기가 문제... 이거 해결해야함.
                    if (!talentChoice)
                    {
                        Singleton<HeroManager>.getInstance().GetHero().ChangeState("Ready");
                        waiting = true;
                    }
                }
                if (owner.zPosition <= -(StageManager.DistancePerFloor)) // remove stage
                {
                    stateMachine.ChangeState("Destroy");
                }
            }
            else // waiting true
            {
                if (talentChoice)
                {
                    Singleton<HeroManager>.getInstance().GetHero().ChangeState("Move");
                    waiting = false;
                }
            }
        }
    }
}