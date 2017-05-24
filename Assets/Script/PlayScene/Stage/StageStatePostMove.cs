using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class StageStatePostMove : BaseState<Stage>
    {
        // talent를 찍기 전까지.
        bool waiting = false;
        bool PassDoor = false;

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
            //Singleton<ConsumableManager>.getInstance().Action_ConfirmTalent += onTalentConfirm;
        }

        internal override void End()
        {
            Singleton<HeroManager>.getInstance().GetHero().Action_DistanceChange -= onHeroMove;
            //Singleton<ConsumableManager>.getInstance().Action_ConfirmTalent -= onTalentConfirm;
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
                if (!PassDoor && owner.zPosition <= GameCameras.HeroCamPosition.z + 1.5f) // 영웅이 문을 지나칠때 change floortext
                {
                    Singleton<StageManager>.getInstance().onStagePass(owner);
                    PassDoor = true;
                }
                if (owner.zPosition <= GameCameras.HeroCamPosition.z) // 문이 완전히 화면에서 지나칠때 talent wait // StageManager.DistancePerFloor - 20
                { //TODO: 계속 여기가 문제... 이거 해결해야함.
                    waiting = true;
                    if (owner.rewardComplete == false)
                    {
//                        Singleton<ConsumableManager>.getInstance().ShowNewTalentWindow(owner.floor);
                        Singleton<HeroManager>.getInstance().GetHero().ChangeState("Ready");
                    }
                }
            }
            else if (waiting == true) // waiting true
            {
                // TODO : 테스트위해서
                owner.rewardComplete = true;
                if (owner.rewardComplete == true)
                {
                    Singleton<HeroManager>.getInstance().GetHero().ChangeState("Move");
                    owner.ChangeState("Destroy");
                    waiting = false;
                }
            }
        }
    }
}