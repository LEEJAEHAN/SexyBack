using System;
using UnityEngine;

namespace SexyBackPlayScene
{

    internal class HeroStateMachine
    {

        HeroState CurrState;

        internal HeroStateMachine(Hero owner)
        {
            CurrState = new HeroStateReady(this,owner);
        }

        internal void Update()
        {
            CurrState.Update();
        }

        internal void ChangeState(HeroState newState)
        {
            CurrState.End();
            CurrState = newState;
            CurrState.Begin();
        }

        internal void onTouch(TapPoint pos)
        {
            CurrState.OnTouch(pos);
        }
    }
}