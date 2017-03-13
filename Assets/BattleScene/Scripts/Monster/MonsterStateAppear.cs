using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MonsterStateAppear : BaseState<Monster>
    {
        float AppearTime = 2;
        float timer = 0;

        public MonsterStateAppear(Monster owner, MonsterStateMachine statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            owner.sprite.GetComponent<Animator>().speed = 10 / AppearTime;
            owner.sprite.GetComponent<Animator>().SetTrigger("Appear");
        }

        internal override void End()
        {
        }

        internal override void Update()
        {
            timer += Time.deltaTime;
            if (timer >= AppearTime)
            {
                owner.sprite.GetComponent<Animator>().SetTrigger("Ready");
                stateMachine.ChangeState("Ready");
            }

        }
    }
}