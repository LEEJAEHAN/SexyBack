using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MonsterStateFlying: BaseState<Monster>
    {
        float flyingTime = 3.75f;
        float DropTickTime = 0.5f;
        public int chestCount;

        public MonsterStateFlying(Monster owner, MonsterStateMachine statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            //if (Singleton<StageManager>.getInstance().Combo < 3)
            //    chestCount = Singleton<InstanceStatus>.getInstance().InstanceMap.baseData.MapMonster.Chest[(int)owner.type];
            //else
            //    chestCount = 0;
            chestCount = 1 * Singleton<PlayerStatus>.getInstance().GetUtilStat.ConsumableX;
            owner.avatar.GetComponent<MonsterView>().Fly();
        }

        internal override void End()
        {
        }

        internal override void Update()
        {
            flyingTime -= Time.deltaTime;
            DropTickTime -= Time.deltaTime;

            if(DropTickTime <= 0 && chestCount > 0)
            {
                bool isOpen = Singleton<HeroManager>.getInstance().AutoChestOpen;
                //sexybacklog.Console(owner.avatar.transform.position);
                Singleton<ConsumableManager>.getInstance().MakeChest(1, owner.level, owner.avatar.transform.position, isOpen);
                chestCount--;
                DropTickTime = 0.5f;
            }

            if (flyingTime <= 0)
                stateMachine.ChangeState("Death");
        }
    }
}