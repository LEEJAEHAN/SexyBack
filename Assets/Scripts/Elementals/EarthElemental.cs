using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class EarthElemental : Elemental
    {
        public EarthElemental() : base()
        {
            string name = "earthball";
            ShooterName = "shooter_" + name;
            ProjectilePrefabName = "prefabs/" + name;
            ProjectileReadyStateName = name + "_spot";
        }
    }
}