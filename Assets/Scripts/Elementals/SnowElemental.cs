using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class SnowElemental : Elemental
    {
        public SnowElemental() : base()
        {
            string name = "snowball";
            ShooterName = "shooter_" + name;
            ProjectilePrefabName = "prefabs/" + name;
            ProjectileReadyStateName = name + "_spot";
        }
    }
}