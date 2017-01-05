using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MagmaElemental : Elemental
    {
        public MagmaElemental() : base()
        {
            string name = "magmaball";
            ShooterName = "shooter_" + name;
            ProjectilePrefabName = "prefabs/" + name;
            ProjectileReadyStateName = name + "_spot";
        }
    }
}