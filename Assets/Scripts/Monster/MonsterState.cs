namespace SexyBackPlayScene
{
    internal abstract class BaseState<T> where T : Statable
    {
        protected T owner;
        protected StateMachine<T> stateMachine;
        public BaseState(T owner, StateMachine<T> statemachine)
        {
            this.owner = owner;
            stateMachine = statemachine;
        }
        internal abstract void Update();
        internal abstract void End();
        internal abstract void Begin();
    }
}