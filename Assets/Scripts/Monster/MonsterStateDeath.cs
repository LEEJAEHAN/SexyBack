using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MonsterStateDeath: BaseState<Monster>
    {
        public MonsterStateDeath(Monster owner, MonsterStateMachine statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
        }

        internal override void End()
        {
        }

        float destroytime = 1;
        internal override void Update()
        {
            destroytime -= Time.deltaTime;
            if(destroytime<=0)
            {
                Singleton<MonsterManager>.getInstance().DestroyMonster(owner);
            }
        }
    }
}