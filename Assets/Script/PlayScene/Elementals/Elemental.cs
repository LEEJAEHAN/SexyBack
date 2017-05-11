using System;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace SexyBackPlayScene
{
    [Serializable]
    internal class Elemental : ISerializable// base class of Elementals
    {
        public string GetID { get { return ID; } }
        public string targetID;

        // 변수
        BigInteger dpsX = new BigInteger();
        int dpsIncreaseXH;
        int castSpeedXH;
        int skillrateIncreaseXH;
        int skilldamageIncreaseXH;

        public int LEVEL = 1;
        public BigInteger DPS = new BigInteger();
        public BigInteger DAMAGE = new BigInteger();//  dps * attackinterval
        public BigInteger PRICE = new BigInteger();
        public BigInteger DPSTICK = new BigInteger();
        public int SKILLRATIO;
        public int SKILLRATEXK;
        public BigInteger SKILLDAMAGE = new BigInteger();
        public double CASTINTERVAL { get { return shooter.CASTINTERVAL; }  set { shooter.SetInterval(value); skill.SetInterval(value); } } // (attackinterval1k / 1000) * ( 100 / attackspeed1h ) 

        // 고정수
        readonly string ID;
        readonly int BaseCastIntervalXK;
        public int BaseLevel;
        public int BasePrice;
        public double BaseDmgDensity;
        readonly int BaseSkillRateXK;
        readonly int BaseSkillRatio;

        // for projectile action;
        private Shooter shooter;
        public Skill skill;
        public bool skillActive = false;
        public int skillForceCount = 0;
        private double AttackTimer = 0;

        // ICanLevelUp
        // event
        public delegate void ElementalChange_EventHandler(Elemental elemental);
        public event ElementalChange_EventHandler Action_Change = delegate { };

        public Elemental(ElementalData data)
        {
            ID = data.ID;
            BaseCastIntervalXK = data.BaseCastIntervalXK;
            BaseDmgDensity = data.BaseDmgDensity;
            BaseLevel = data.BaseLevel;
            BasePrice = data.BasePrice;
            BaseSkillRateXK = data.BaseSkillRateXK;
            BaseSkillRatio = data.BaseSkillDamageXH;

            shooter = new Shooter(ID, data.PrefabName );
            skill = SkillFactory.Create(ID, data.SkillPrefabName, BaseSkillRatio);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {   // 소모성 변수는 저장한다. 영구 스텟은 스텟에서저장한다.
            info.AddValue("skillForceCount", skillForceCount);
        }
        public Elemental(SerializationInfo info, StreamingContext context)
        {
            skillForceCount = (int)info.GetValue("skillForceCount", typeof(int));
        }

        internal void SetStat(ElementalStat elementalstat, bool NeedCalDamage, bool NeedCalPrice) // total
        {
            LEVEL = elementalstat.Level;
            skillActive = elementalstat.SkillLaunch;
            dpsX = elementalstat.DpsX;
            skillActive = elementalstat.SkillLaunch;
            dpsIncreaseXH = elementalstat.DpsIncreaseXH;
            castSpeedXH = elementalstat.CastSpeedXH;
            skillrateIncreaseXH = elementalstat.SkillRateIncreaseXH;
            skilldamageIncreaseXH = elementalstat.SkillDmgIncreaseXH;

            // CASTINTERVAL이 0.5보다 낮아져선 안된다. ( 실제는 0.8 )
            CASTINTERVAL = UnityEngine.Mathf.Max(0.8f,((float)BaseCastIntervalXK / (float)(castSpeedXH * 10)));
            SKILLRATEXK = BaseSkillRateXK * skillrateIncreaseXH / 100;
            SKILLRATIO = BaseSkillRatio* skilldamageIncreaseXH / 100;
            skill.SetStat(skilldamageIncreaseXH);

            if (NeedCalDamage)
                CalDPS();
            if(NeedCalPrice)
                CalPrice();
            Action_Change(this);
        }
        void CalDPS()
        {
            double growth = InstanceStatus.CalGrowthPower(ElementalData.GrowthRate, BaseLevel); // 
            double doubleC = 1 * BaseDmgDensity * growth * LEVEL * dpsIncreaseXH * castSpeedXH / 10000;
            BigInteger Coefficient = BigInteger.FromDouble(doubleC);
            DPS = dpsX * Coefficient;
            DPSTICK = DPS / LEVEL;
            DAMAGE = DPS * BaseCastIntervalXK / (castSpeedXH * 10); //  dps * CASTINTERVAL]
            skill.CalDamage(DAMAGE);
        }
        private void CalPrice()
        {
            double BasePriceDensity = InstanceStatus.GetTotalDensityPerLevel(BaseLevel + LEVEL);
            // cal price
            double growth = InstanceStatus.CalGrowthPower(ElementalData.GrowthRate, BaseLevel + LEVEL);
            double doubleC = BasePrice * BasePriceDensity * growth;
            PRICE = BigInteger.FromDouble(doubleC); // 60(랩업비기본) * 2.08(비중) * power수
        }
        // TODO : 여기도 언젠간 statemachine작업을 해야할듯 ㅠㅠ
        internal void Update()
        {
            //shooter 리로드
            //shooter 발사.

            if(!isSkillAttack)
            {
                shooter.ReLoad(AttackTimer);
                if (shooter.Shoot(AttackTimer, targetID))
                    EndAttack();
            }

            if (isSkillAttack)
            {
                skill.ReLoad(AttackTimer);
                if (skill.Shoot(AttackTimer, targetID))
                {
                    EndAttack();
                }
            }

            skill.Update(); // cast 이후의 post업데이트.
            AttackTimer += Time.deltaTime;
        }

        bool isSkillAttack = false; // 이번공격이 스킬인지 
        private bool JudgeSkill {
            get
            {
                if (skillForceCount > 0)
                {
                    skillForceCount--;
                    return true;
                }
                if (!skillActive)
                    return false;
                return SKILLRATEXK > UnityEngine.Random.Range(0, 1000);
            }
        }

        void EndAttack()
        {
            AttackTimer = 0; // 정상적으로 발사 완료 후 타이머리셋
            isSkillAttack = JudgeSkill;
        }

        public void onTargetStateChange(string monsterID, string stateID)
        {
            if (stateID == "Ready")
                this.targetID = monsterID;
            else
                targetID = null;
        }
    }
}