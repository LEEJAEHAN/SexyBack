using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class RockElemental : Elemental
    {
        public RockElemental() : base()
        {
            string name = "rock";
            ShooterName = "shooter_" + name;
            ProjectilePrefabName = "prefabs/" + name;
            ProjectileReadyStateName = name + "_spot";
        }
    }
}