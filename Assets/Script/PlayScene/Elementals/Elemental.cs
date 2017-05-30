using System;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace SexyBackPlayScene
{
    [Serializable]
    internal class Elemental : ICanLevelUp, ISerializable// base class of Elementals
    {
        ElementalData baseData;
        public string GetID { get { return baseData.ID; } }
        public int GetLevel { get { return LEVEL; } }
        public string targetID { get { return shooter.targetID; } set { skill.targetID = value; shooter.targetID = value; } }
        // 변수
        BigInteger DpsX = new BigInteger();
        public int DpsXH;
        public int CastSpeedXH;
        public int SkillRatioXH { get { return skill.DAMAGERATIO; } set { skill.SetRatio(value); } }
        public int SkillRateXH;
        int BuffCoef = 1;

        public int LEVEL;
        public BigInteger DPS = new BigInteger();
        public BigInteger DAMAGE = new BigInteger();//  dps * attackinterval
        public BigInteger PRICE = new BigInteger();
        public BigInteger DPSTICK = new BigInteger();
        public double CastInterval { get { return shooter.CastInterval; } set { shooter.SetInterval(value); skill.SetInterval(value); } } // (attackinterval1k / 1000) * ( 100 / attackspeed1h ) 

        // for projectile action;
        private Shooter shooter;
        public Skill skill;
        public List<Skill> instanceSkill;
        public bool SkillActive = false;

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
            instanceSkill = new List<Skill>();
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
            double doubleC = 1 * baseData.BaseDmg * growth * LEVEL * DpsXH * CastSpeedXH * BuffCoef/ 10000;
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

        internal void Buff(bool on, int xtimes)
        {
            if (on)
                BuffCoef = xtimes;
            else
                BuffCoef = 1;
            RefreshStat = true;
        }

        internal void Update()
        {
            if (RefreshStat)
            {
                Refresh();
                RefreshStat = false;
            }

            if (shooter.Enable == false && skill.isEnd)
                JudgeAutoAttack();

            shooter.Update();
            skill.Update();
            skill.PostUpdate();

            //foreach(var s in instanceSkill)
            //{
            //    s.Update();
            //    s.PostUpdate();
            //}

            for (int i = instanceSkill.Count - 1; i >= 0; i--)
            {
                instanceSkill[i].Update();
                instanceSkill[i].PostUpdate();
                if(instanceSkill[i].CheckFinish())
                    instanceSkill.RemoveAt(i);
            }

            sexybacklog.Console("현재 instanceskill Count " + instanceSkill.Count);

            //instanceSkill.RemoveAll(i => i.Finish == true);
            //강제시전

            // 아직구현안함.
            //if (SkillForceCount > 0)
            //{
            //    if (skill.AutoAttack())
            //        SkillForceCount--;
            //}
        }

        public void CastSkillItem()     // 스킬로 사용되는 아이템은 사용 시점의 stat을 계속가지고간다. (중간에 강해지거나 약해지지 않는다.)
        {
            Skill itemskill = SkillFactory.Create(baseData.ID, baseData.SkillPrefabName);
            itemskill.CalDamage(DAMAGE);
            itemskill.SetRatio(SkillRatioXH);
            itemskill.SetInterval(CastInterval);
            itemskill.Start(true);
            itemskill.targetID = this.targetID;
            instanceSkill.Add(itemskill);
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
                    skill.Start(false);
                else
                    shooter.Enable = true;
            }
        }

        public void onTargetStateChange(string monsterID, string stateID)
        {
            if (stateID == "Ready")
            {
                this.targetID = monsterID;
                instanceSkill.ForEach(i => i.targetID = monsterID);
            }
            else
            {
                targetID = null;
                instanceSkill.ForEach(i => i.targetID = null);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {   // 소모성 변수는 저장한다. 영구 스텟은 스텟에서저장한다.
            //info.AddValue("skillForceCount", SkillForceCount);
        }
        public Elemental(SerializationInfo info, StreamingContext context)
        {
            //SkillForceCount = (int)info.GetValue("skillForceCount", typeof(int));
        }
    }
}