namespace SexyBackPlayScene
{
    internal abstract class StateMachine<T> where T : Statable
    {
        protected T owner;
        private BaseState<T> CurrState;
        string currStateID;

        internal StateMachine(T owner)
        {
            this.owner = owner;
        }
        public void Update()
        {
            CurrState.Update();
        }
        private void ChangeState(BaseState<T> newState)
        {
            if (CurrState != null)
                CurrState.End();

            CurrState = newState;
            CurrState.Begin();
        }
        protected abstract BaseState<T> CreateState(string stateid);

        // public
        public void ChangeState(string stateid)
        {
            ChangeState(CreateState(stateid));
        }




    }
}