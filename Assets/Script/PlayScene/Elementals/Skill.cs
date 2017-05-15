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
        internal enum Type
        {
            Burn = 20,
            Poison = 10,
            None = 0
        }

        public BigInteger TickDamage;
        public int duration;        
        public Type type;

        public Debuff(Type type, BigInteger tickDamage, int duration)
        {
            TickDamage = tickDamage;
            this.duration = duration;
            this.type = type;
        }
        internal BigInteger PopOneTickDamage()
        {
            duration -= 1;
            return TickDamage;
        }
        public static Color GetMask(Type t)
        {
            switch(t)
            {
                case Type.Burn:
                    {
                        return Color.red;
                    }
                case Type.Poison:
                    {
                        return Color.green;
                    }
                default:
                    return Color.white;
            }
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
        public Debuff.Type debuff;
        public int DAMAGERATIO;
        public BigInteger DAMAGE;

        public Skill(string ownerID, string prefab, DamageType ability, int baseDamageRatio, Debuff.Type debuff)
        {
            this.ownerID = ownerID;
            prefabname = prefab;
            isTargetEnemy = true;
            this.ability = ability;
            this.debuff = debuff;
            baseRatio = baseDamageRatio;
        }

        abstract internal void ReLoad(double timer);
        abstract internal bool Shoot(double timer, string targetID);
        virtual internal void Update()
        {

        }

        internal void SetRatio(int damageRatio)
        {
            DAMAGERATIO = damageRatio;
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