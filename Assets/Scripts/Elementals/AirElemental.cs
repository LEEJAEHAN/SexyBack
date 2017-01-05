using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class AirElemental : Elemental
    {
        public AirElemental() : base()
        {
            string name = "airball";
            ShooterName = "shooter_" + name;
            ProjectilePrefabName = "prefabs/"+ name;
            ProjectileReadyStateName = name + "_spot";
        }
    }
}