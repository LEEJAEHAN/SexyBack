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
            owner.sprite.GetComponent<Animator>().speed = 5f;
            owner.sprite.GetComponent<Animator>().SetTrigger("Appear");
        }

        internal override void End()
        {
            owner.sprite.GetComponent<Animator>().SetTrigger("Ready");
        }

        internal override void Update()
        {
            timer += Time.deltaTime;
            if (timer >= AppearTime)
                stateMachine.ChangeState("Ready");

        }
    }
}