using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class HeroStateReady : HeroState
    {
        bool TouchTrigger = false;
        public HeroStateReady(HeroStateMachine stateMachine, Hero owner) : base(stateMachine, owner)
        {
            TouchTrigger = false;
        }

        internal override void Begin()
        {
        }

        internal override void End()
        {
        }

        internal override void OnTouch(TapPoint pos)
        {
            if (!TouchTrigger && owner.CanAttack)
            {
                owner.MakeAttackPlan(pos);
                TouchTrigger = true;

                sexybacklog.Console("tap worldpos:"+pos.UiPos);
            }
        }

        internal override void Update()
        {
            if (TouchTrigger)
            {
                TouchTrigger = false;
                //CheckMonster
                if (owner.targetID != null)
                {
                    stateMachine.ChangeState(new HeroStateAttack(stateMachine, owner));
                }
            }

        }
    }
}