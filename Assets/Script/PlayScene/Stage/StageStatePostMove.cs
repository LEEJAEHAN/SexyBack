﻿using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class StageStatePostMove : BaseState<Stage>
    {
        // talent를 찍기 전까지.
        bool PassDoor = false;

        public StageStatePostMove(Stage owner, StateMachine<Stage> statemachine) : base(owner, statemachine)
        {
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
            if (!PassDoor && owner.zPosition <= GameCameras.HeroCamPosition.z + 1.5f) // 영웅이 문을 지나칠때 change floortext
            {
                Singleton<StageManager>.getInstance().onStagePass(owner);
                PassDoor = true;
            }
            if (owner.zPosition <= GameCameras.HeroCamPosition.z) // 문이 완전히 화면에서 지나칠때 talent wait // StageManager.DistancePerFloor - 20
                owner.ChangeState("Destroy");
        }
    }
}