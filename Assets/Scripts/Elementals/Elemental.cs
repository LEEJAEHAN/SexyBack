using UnityEngine;

namespace SexyBackPlayScene
{
    public abstract class Elemental // base class of Elementals
    {

        public Elemental()
        {
        }
        public abstract void Update();
        public abstract void Shoot();
        public abstract void Cast();
    }
}