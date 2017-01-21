using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal abstract class HeroState
    {
        protected HeroStateMachine stateMachine;
        protected Hero owner;
        public HeroState(HeroStateMachine stateMachine, Hero owner)
        {
            this.stateMachine = stateMachine;
            this.owner = owner;
        }
        internal abstract void Update();
        internal abstract void End();
        internal abstract void Begin();
        internal abstract void OnTouch(TapPoint pos);
    }
}