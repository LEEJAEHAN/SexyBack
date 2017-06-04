using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MonsterStateFlying: BaseState<Monster>
    {
        float flyingTime = 3.75f;
        float DropTickTime = 0.5f;
        int DropCount = 0;

        public MonsterStateFlying(Monster owner, MonsterStateMachine statemachine) : base(owner, statemachine)
        {
            if(owner.isBoss)
                DropCount = Singleton<InstanceStatus>.getInstance().InstanceMap.baseData.ChestPerBossMonster;
            else
                DropCount = Singleton<InstanceStatus>.getInstance().InstanceMap.baseData.ChestPerMonster;

            DropCount += Singleton<PlayerStatus>.getInstance().GetUtilStat.BonusConsumable;
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

            if(DropTickTime <= 0 && DropCount > 0)
            {
                //sexybacklog.Console(owner.avatar.transform.position);
                Singleton<ConsumableManager>.getInstance().MakeChest(1, owner.level, owner.avatar.transform.position);
                DropCount--;
                DropTickTime = 0.5f;
            }

            if (flyingTime <= 0)
                stateMachine.ChangeState("Death");
        }
    }
}