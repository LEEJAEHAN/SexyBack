﻿using System;

namespace SexyBackPlayScene
{
    internal class StageStateDestroy : BaseState<Stage>
    {
        public StageStateDestroy(Stage owner, StateMachine<Stage> statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            Singleton<StageManager>.getInstance().onStageClear(owner);
        }

        internal override void End()
        {
        }

        internal override void Update()
        {
        }
    }
}