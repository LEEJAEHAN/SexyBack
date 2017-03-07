using System;

namespace SexyBackPlayScene
{
    internal class ResearchStateMachine : StateMachine<Research>
    {
        public ResearchStateMachine(Research owner) : base(owner)
        {
        }

        protected override BaseState<Research> CreateState(string stateid)
        {
            switch (stateid)
            {
                case "Ready":
                    return new ResearchStateReady(owner, this);
                case "Work":
                    return new ResearchStateWork(owner, this);
                case "Pause":
                    return new ResearchStatePause(owner, this);
                case "Destroy":
                    return new ResearchStateDestroy(owner, this);

                default:
                    {
                        UnityEngine.Debug.LogError("");
                        return null;
                    }
            }
        }
    }
}