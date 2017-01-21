﻿using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class HeroStateReady : HeroState
    {
        TapPoint firstTouch;
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
                //owner.SetSwordEffectPosition();
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