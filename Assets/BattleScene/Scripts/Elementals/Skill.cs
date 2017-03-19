using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public enum DamageType
    {
        Hit,
        DeBuff,
        HitDebuff,
        HitPerHPLow,
        HitPerHPHigh,
    }

    internal class Debuff
    {
        public int DAMAGERATIO;
        public int baseRatio;
        public BigInteger DAMAGE;

        public Debuff(int baseDamageRatio, int duration)
        {
            baseRatio = baseDamageRatio;
        }
        internal void Update()
        {
        }
    }

    //Type; // "shoot", "drop", "fastshoot" "cast"; // dot,4  childtype
    internal abstract class Skill // case drop
    {
        // skilldata
        protected string ownerID;
        protected bool ReLoaded = false;
        protected double CASTINTERVAL;
        protected bool isTargetEnemy;
        protected string prefabname;
        public DamageType ability;
        public int baseRatio; // base
        public int DAMAGERATIO;
        public BigInteger DAMAGE;

        public Debuff debuff;

        public Skill(string ownerID, string prefab, DamageType ability, int baseDamageRatio, Debuff debuff)
        {
            this.ownerID = ownerID;
            prefabname = prefab;
            isTargetEnemy = true;
            baseRatio = baseDamageRatio;
            this.debuff = debuff;
        }

        abstract internal void ReLoad(double timer);
        abstract internal bool Shoot(double timer, string targetID);
        virtual internal void Update(string targetID)
        {

        }

        internal void SetStat(int skilldamageIncreaseXH)
        {
            DAMAGERATIO = baseRatio * skilldamageIncreaseXH / 100;
            if(debuff != null)
                debuff.DAMAGERATIO = baseRatio * skilldamageIncreaseXH / 100;
        }
        virtual internal void SetInterval(double interval)
        {
            this.CASTINTERVAL = interval;
        }
        virtual internal void CalDamage(BigInteger elementaldmg)
        {
            DAMAGE = elementaldmg * DAMAGERATIO / 100;
            if (debuff != null)
                debuff.DAMAGE = elementaldmg * debuff.DAMAGERATIO / 100;
        }
    }

}