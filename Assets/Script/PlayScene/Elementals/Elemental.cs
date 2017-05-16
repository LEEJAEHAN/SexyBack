using System;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace SexyBackPlayScene
{
    [Serializable]
    internal class Elemental : ISerializable// base class of Elementals
    {
        ElementalData baseData;
        public string GetID { get { return baseData.ID; } }
        public string targetID { set { skill.targetID = value; shooter.targetID = value; } }
        // 변수
        BigInteger DpsX = new BigInteger();
        public int DpsXH;
        public int CastSpeedXH;
        public int SkillRatioXH { get { return skill.DAMAGERATIO; } set { skill.SetRatio(value); } }
        public int SkillRateXH;

        public int LEVEL;
        public BigInteger DPS = new BigInteger();
        public BigInteger DAMAGE = new BigInteger();//  dps * attackinterval
        public BigInteger PRICE = new BigInteger();
        public BigInteger DPSTICK = new BigInteger();
        public double CastInterval { get { return shooter.CastInterval; } set { shooter.SetInterval(value); skill.SetInterval(value); } } // (attackinterval1k / 1000) * ( 100 / attackspeed1h ) 

        // for projectile action;
        private Shooter shooter;
        public Skill skill;
        public bool SkillActive = false;
        public int SkillForceCount = 0;

        // ICanLevelUp
        // event
        public delegate void ElementalChange_EventHandler(Elemental elemental);
        public event ElementalChange_EventHandler Action_Change = delegate { };

        bool RefreshStat = true;

        public Elemental(ElementalData data)
        {
            baseData = data;
            shooter = new Shooter(data.ID, data.PrefabName);
            skill = SkillFactory.Create(data.ID, data.SkillPrefabName);
        }

        internal void LevelUp(int value)
        {
            LEVEL += value;
            RefreshStat = true;
        }
        internal void onStatChange()
        {
            RefreshStat = true;
        }

        private void CalStat()
        {
            ElementalStat elementalstat = Singleton<PlayerStatus>.getInstance().GetElementalStat(GetID);
            BaseStat basestat = Singleton<PlayerStatus>.getInstance().GetBaseStat;

            DpsX = elementalstat.DpsX;
            DpsXH = (100 + elementalstat.DpsIncreaseXH) * (100 + basestat.Int) / 100;
            CastSpeedXH = (100 + elementalstat.CastSpeedXH) * (200 + basestat.Spd) / 200;
            SkillRateXH = baseData.BaseSkillRateXK * (100 + elementalstat.SkillRateIncreaseXH) / 100;
            SkillRatioXH = baseData.BaseSkillDamageXH * (100 + elementalstat.SkillDmgIncreaseXH) / 100;
            // CASTINTERVAL이 0.5보다 낮아져선 안된다. ( 실제는 0.8 )
            CastInterval = UnityEngine.Mathf.Max(0.8f, ((float)baseData.BaseCastIntervalXK / (float)(CastSpeedXH * 10)));
        }

        void CalDps()
        {
            double growth = InstanceStatus.CalGrowthPower(ElementalData.GrowthRate, baseData.BaseLevel); // 
            double doubleC = 1 * baseData.BaseDmg * growth * LEVEL * DpsXH * CastSpeedXH / 10000;
            BigInteger Coefficient = BigInteger.FromDouble(doubleC);
            DPS = DpsX * Coefficient;
            if (LEVEL > 0)
                DPSTICK = DPS / LEVEL;
            DAMAGE = DPS * baseData.BaseCastIntervalXK / (CastSpeedXH * 10); //  dps * CASTINTERVAL]
            skill.CalDamage(DAMAGE);
        }
        private void CalPrice()
        {
            double BasePriceDensity = InstanceStatus.GetTotalDensityPerLevel(baseData.BaseLevel + LEVEL);
            // cal price
            double growth = InstanceStatus.CalGrowthPower(ElementalData.GrowthRate, baseData.BaseLevel + LEVEL);
            double doubleC = baseData.BasePrice * BasePriceDensity * growth;
            PRICE = BigInteger.FromDouble(doubleC); // 60(랩업비기본) * 2.08(비중) * power수
        }

        internal void Update()
        {
            if (RefreshStat)
            {
                Refresh();
                RefreshStat = false;
            }

            if (shooter.Enable == false && skill.Enable == false)
                JudgeAutoAttack();

            shooter.AutoAttack();
            skill.AutoAttack();
            skill.PostUpdate();

            // 아직구현안함.
            //if (SkillForceCount > 0)
            //{
            //    if (skill.AutoAttack())
            //        SkillForceCount--;
            //}
        }

        private void Refresh()
        {
            CalStat();
            CalDps();
            CalPrice();
            Action_Change(this);
        }

        private void JudgeAutoAttack()
        {
            if (!SkillActive)
                shooter.Enable = true;
            else
            {
                if (SkillRateXH > UnityEngine.Random.Range(0, 1000))
                    skill.Enable = true;
                else
                    shooter.Enable = true;
            }
        }

        public void onTargetStateChange(string monsterID, string stateID)
        {
            if (stateID == "Ready")
                this.targetID = monsterID;
            else
                targetID = null;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {   // 소모성 변수는 저장한다. 영구 스텟은 스텟에서저장한다.
            info.AddValue("skillForceCount", SkillForceCount);
        }
        public Elemental(SerializationInfo info, StreamingContext context)
        {
            SkillForceCount = (int)info.GetValue("skillForceCount", typeof(int));
        }
    }
}