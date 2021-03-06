﻿using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class HeroStateMachine : StateMachine<Hero>
    {
        internal HeroStateMachine(Hero owner) : base(owner)
        {
        }
        protected override BaseState<Hero> CreateState(string stateid)
        {
            switch (stateid)
            {
                case "Ready":
                    return new HeroStateReady(owner, this);
                case "Attack":
                    return new HeroStateAttack(owner, this);
                case "Move":
                    return new HeroStateMove(owner, this);
                case "FastMove":
                    return new HeroStateFastMove(owner, this);
                default:
                    {
                        UnityEngine.Debug.LogError("");
                        return null;
                    }
            }
        }        
    }
}