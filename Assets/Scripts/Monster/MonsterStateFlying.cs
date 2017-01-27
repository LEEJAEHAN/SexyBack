using System;

namespace SexyBackPlayScene
{
    internal class MonsterStateFlying: BaseState<Monster>
    {
        public MonsterStateFlying(Monster owner, MonsterStateMachine statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            Singleton<MonsterManager>.getInstance().DestroyMonster(owner);
        }

        internal override void End()
        {
        }

        internal override void Update()
        {
            //
            //            sexybacklog.Console("레디까지무사히옴 ㅠㅠ");
        }
    }
}