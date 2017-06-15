using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MonsterStateFlying: BaseState<Monster>
    {
        float flyingTime = 3.75f;
        float DropTickTime = 0.5f;

        public MonsterStateFlying(Monster owner, MonsterStateMachine statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            owner.avatar.GetComponent<MonsterView>().Fly();
        }

        internal override void End()
        {
        }

        internal override void Update()
        {
            flyingTime -= Time.deltaTime;
            DropTickTime -= Time.deltaTime;

            if(DropTickTime <= 0 && owner.chestCount > 0)
            {
                //sexybacklog.Console(owner.avatar.transform.position);
                Singleton<ConsumableManager>.getInstance().MakeChest(1, owner.level, owner.avatar.transform.position);
                owner.chestCount--;
                DropTickTime = 0.5f;
            }

            if (flyingTime <= 0)
                stateMachine.ChangeState("Death");
        }
    }
}