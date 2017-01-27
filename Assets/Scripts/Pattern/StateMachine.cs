using System;

namespace SexyBackPlayScene
{
    public abstract class StateMachine<T> where T : class, StateOwner
    {
        protected T owner;
        private BaseState<T> CurrState;
        public string currStateID;
        public delegate void StateChangeHandler(string id, string stateid);
        public event StateChangeHandler Action_changeEvent = delegate { };

        internal StateMachine(T owner)
        {
            this.owner = owner;
        }
        public void Update()
        {
            if (CurrState == null)
                return;
            CurrState.Update();
        }
        private void ChangeState(BaseState<T> newState)
        {
            if (CurrState != null)
            {
                CurrState.End();
            }

            CurrState = newState;
            CurrState.Begin();
        }
        protected abstract BaseState<T> CreateState(string stateid);

        // public
        public void ChangeState(string stateid)
        {
            ChangeState(CreateState(stateid));
            currStateID = stateid;
            Action_changeEvent(owner.ID, stateid);
        }

    }

}