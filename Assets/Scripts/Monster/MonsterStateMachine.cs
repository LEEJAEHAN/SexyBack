using System;

namespace SexyBackPlayScene
{
    internal class MonsterStateMachine : StateMachine<Monster>
    {
        internal MonsterStateMachine(Monster owner) : base(owner)
        {   
            //ChangeState("Appear");
        }
        protected override BaseState<Monster> CreateState(string stateid)
        {
            switch (stateid)
            {
                case "Appear":
                    return new MonsterStateAppear(owner, this);
                case "Ready":
                    return new MonsterStateReady(owner, this);
                case "Flying":
                    return new MonsterStateFlying(owner, this);
                case "Death":
                    return new MonsterStateReady(owner, this);
                default:
                    {
                        sexybacklog.Error();
                        return null;
                    }
            }
        }
    }
    internal enum MonsterStateEnum
    {
        Appear,
        Ready,
        Flying,
        Death
    }

}