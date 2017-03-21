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
        public BigInteger TickDamage;
        public int duration;

        double duriationTimer = 0;
        double tickTimer = 0;
        double tick = 0.2f;

        public Debuff(BigInteger tickDamage, int duration)
        {
            TickDamage = tickDamage;
            this.duration = duration;
        }
        //internal void Update()
        //{
        //    duriationTimer += Time.deltaTime;

        //    if(duriationTimer <= duration)
        //    {
        //        tickTimer += Time.deltaTime;
        //        while (tickTimer > tick)
        //        {
        //            tickTimer -= tick;
        //        }

        //    }   
        //}

        internal BigInteger PopOneTickDamage()
        {
            duration -= 1;
            return TickDamage;
        }

        internal bool CheckEnd()
        {
            if (duration <= 0)
                return true;
            return false;
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
        public string debuff;
        public int DAMAGERATIO;
        public BigInteger DAMAGE;


        public Skill(string ownerID, string prefab, DamageType ability, int baseDamageRatio, string debuffID)
        {
            this.ownerID = ownerID;
            prefabname = prefab;
            isTargetEnemy = true;
            this.ability = ability;
            this.debuff = debuffID;
            baseRatio = baseDamageRatio;
        }

        abstract internal void ReLoad(double timer);
        abstract internal bool Shoot(double timer, string targetID);
        virtual internal void Update()
        {

        }

        internal void SetStat(int skilldamageIncreaseXH)
        {
            DAMAGERATIO = baseRatio * skilldamageIncreaseXH / 100;
        }
        virtual internal void SetInterval(double interval)
        {
            this.CASTINTERVAL = interval;
        }
        virtual internal void CalDamage(BigInteger elementaldmg)
        {
            DAMAGE = elementaldmg * DAMAGERATIO / 100;
        }
    }

}