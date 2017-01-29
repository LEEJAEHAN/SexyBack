using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MonsterStateAppear : BaseState<Monster>
    {
        public MonsterStateAppear(Monster owner, MonsterStateMachine statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            owner.sprite.GetComponent<Animator>().SetTrigger("Appear");
        }

        internal override void End()
        {
            owner.sprite.GetComponent<Animator>().SetTrigger("Ready");
        }

        internal override void Update()
        {
        }
    }
}