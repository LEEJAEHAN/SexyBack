using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MonsterStateDeath: BaseState<Monster>
    {
        bool trigger = true;
        public MonsterStateDeath(Monster owner, MonsterStateMachine statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
        }

        internal override void End()
        {
        }

        internal override void Update()
        {
            Singleton<MonsterManager>.getInstance().DestroyMonster(owner);
        }
    }
}