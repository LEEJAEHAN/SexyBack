using System;

namespace SexyBackPlayScene
{
    internal abstract class BaseState<T> where T : class, IStateOwner
    {

        protected T owner;
        protected StateMachine<T> stateMachine;
        public BaseState(T owner, StateMachine<T> statemachine)
        {
            this.owner = owner;
            stateMachine = statemachine;
            sexybacklog.Console(this.ToString() + " State 생성");
        }
        internal abstract void Update();
        internal abstract void End();
        internal abstract void Begin();

        ~BaseState() { sexybacklog.Console(this.ToString() + " State 소멸"); }

    }

}