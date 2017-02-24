using System;

namespace SexyBackPlayScene
{
    internal class StageStateMachine : StateMachine<Stage>
    {
        public StageStateMachine(Stage owner) : base(owner)
        {
        }

        protected override BaseState<Stage> CreateState(string stateid)
        {
            switch (stateid)
            {
                case "Move":
                    return new StageStateMove(owner, this);
                case "Battle":
                    return new StageStateBattle(owner, this);
                case "PostMove":
                    return new StageStatePostMove(owner, this);
                case "Destroy":
                    return new StateStateDestroy(owner, this);
                default:
                    {
                        UnityEngine.Debug.LogError("");
                        return null; ;
                    }
            }
        }
    }
}