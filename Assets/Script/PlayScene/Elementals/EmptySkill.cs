using System;

namespace SexyBackPlayScene
{
    internal class EmptySkill : Skill
    {
        public EmptySkill() : base("none", "none", DamageType.Hit, Debuff.Type.None)
        {

        }

        internal override void ReLoad()
        {
        }

        internal override void FirstShoot()
        {
        }

        internal override bool CheckFinish()
        {
            return true;
        }

        internal override void Start(bool NoReloadTime)
        {
        }
    }
}