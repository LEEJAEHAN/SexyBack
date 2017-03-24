using System;

namespace SexyBackPlayScene
{
    internal class EmptySkill : Skill
    {
        public EmptySkill() : base("none", "none", DamageType.Hit, 0, Debuff.Type.None)
        {

        }

        internal override void ReLoad(double timer)
        {
        }

        internal override bool Shoot(double timer, string targetID)
        {
            return true;
        }
    }
}