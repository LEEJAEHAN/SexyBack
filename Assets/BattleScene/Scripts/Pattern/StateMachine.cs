using System;

namespace SexyBackPlayScene
{
    internal abstract class StateMachine<T> where T : class, IStateOwner
    {
        protected T owner;
        private BaseState<T> CurrState;
        public string newStateID = null;
        public string currStateID;

        public delegate void StateChangeHandler(string id, string stateid);
        public event StateChangeHandler Action_changeEvent = delegate { };

        internal StateMachine(T owner)
        {
            this.owner = owner;
        }
        public void Update()
        {
            if (newStateID != null)
                ChangeState(CreateState(newStateID));

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
            currStateID = newStateID;

            Action_changeEvent(owner.GetID, currStateID);
            newStateID = null;
            CurrState.Begin();
        }
        protected abstract BaseState<T> CreateState(string stateid);

        // public
        public void ChangeState(string stateid)
        {
            newStateID = stateid;
        }

    }

}