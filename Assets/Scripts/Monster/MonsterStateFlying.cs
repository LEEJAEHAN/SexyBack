using System;

namespace SexyBackPlayScene
{
    internal class MonsterStateFlying: BaseState<Monster>
    {
        public MonsterStateFlying(Monster owner, MonsterStateMachine statemachine) : base(owner, statemachine)
        {
            sexybacklog.Console("monster flying 생성");
        }

        ~MonsterStateFlying()
        {
            sexybacklog.Console("monster flying 파괴");
        }

        internal override void Begin()
        {
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