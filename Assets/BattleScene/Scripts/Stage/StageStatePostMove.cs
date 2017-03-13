using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class StageStatePostMove : BaseState<Stage>
    {
        // talent를 찍기 전까지.
        bool waiting = false;
        bool flipflop = false;
        double flyingwaitTime = 2;
        double timer = 0;

        public StageStatePostMove(Stage owner, StateMachine<Stage> statemachine) : base(owner, statemachine)
        {
        }

        void onTalentConfirm()
        {
            owner.rewardComplete = true;
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
            owner.avatar.transform.localPosition = GameCameras.EyeLine * (owner.zPosition / 10);
            //owner.Move(delta_z);
        }

        internal override void Update()
        {
            if (waiting == false)
            {
                timer += Time.deltaTime;
                if(flyingwaitTime > 0 && timer > flyingwaitTime)
                {
                    Singleton<HeroManager>.getInstance().GetHero().ChangeState("Move");
                    flyingwaitTime = -1;
                }
                if (!flipflop && owner.zPosition <= GameCameras.HeroCamPosition.z + 1.5f) // change floortext
                {
                    Singleton<StageManager>.getInstance().onStagePass(owner.floor);
                    Singleton<TalentManager>.getInstance().ShowNewTalentWindow(owner.floor);
                    flipflop = true;
                }
                if (owner.zPosition <= GameCameras.HeroCamPosition.z) // talent wait // StageManager.DistancePerFloor - 20
                { //TODO: 계속 여기가 문제... 이거 해결해야함.
                    if (!owner.rewardComplete)
                    {
                        Singleton<HeroManager>.getInstance().GetHero().ChangeState("Ready");
                        waiting = true;
                    }
                }
            }
            else // waiting true
            {
                if (owner.rewardComplete)
                {
                    Singleton<HeroManager>.getInstance().GetHero().ChangeState("Move");
                    owner.ChangeState("Destroy");
                    waiting = false;
                }
            }
        }
    }
}