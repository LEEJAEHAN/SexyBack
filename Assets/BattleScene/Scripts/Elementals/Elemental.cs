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
        int level = 0;
        BigInteger dpsX = new BigInteger();
        int dpsIncreaseXH;
        int castSpeedXH;
        int skillrateIncreaseXH;
        int skilldamageIncreaseXH;

        public BigInteger DPS = new BigInteger();
        public BigInteger DPSTICK = new BigInteger();
        public BigInteger DAMAGE = new BigInteger();//  dps * attackinterval
        public int SKILLRATIO;
        public int SKILLRATE;
        public BigInteger SKILLDAMAGE = new BigInteger();
        public double CASTINTERVAL { get { return shooter.CASTINTERVAL; }  set { shooter.SetInterval(value); skill.SetInterval(value); } } // (attackinterval1k / 1000) * ( 100 / attackspeed1h ) 

        // 고정수
        readonly string ID;
        readonly int DpsShiftDigit;
        readonly int BaseCastIntervalXK;
        readonly BigInteger BaseDps;
        readonly BigInteger BaseExp;
        readonly int BaseSkillRate;
        readonly int BaseSkillRatio;

        readonly double GrowthRate;

        // for projectile action;
        private Shooter shooter;
        public Skill skill;
        private double AttackTimer = 0;

        // ICanLevelUp
        public int LEVEL { get { return level; } }
        public BigInteger LevelUpPrice { get { return BigInteger.PowerByGrowth(BaseExp, level, GrowthRate); } }
        // event
        public delegate void ElementalChange_EventHandler(Elemental elemental);
        public event ElementalChange_EventHandler Action_Change = delegate { };

        public Elemental(ElementalData data)
        {
            ID = data.ID;
            BaseCastIntervalXK = data.BaseCastIntervalXK;
            BaseDps = data.BaseDps;
            DpsShiftDigit = data.FloatDigit;
            BaseExp = data.BaseExp;
            GrowthRate = data.GrowthRate;
            BaseSkillRate = data.BaseSkillRate;
            BaseSkillRatio = data.BaseSkillDamageXH;

            shooter = new Shooter(ID, data.PrefabName );
            skill = SkillFactory.Create(ID, data.SkillPrefabName, BaseSkillRatio);
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("level", level);
        }
        public Elemental(SerializationInfo info, StreamingContext context)
        {
            level = (int)info.GetValue("level", typeof(int));
        }

        public void LevelUp(int amount)
        {
            level += amount;
            CalDPS();
            Action_Change(this);
        }
        internal void SetStat(ElementalStat elementalstat, bool CalDamage) // total
        {
            dpsX = elementalstat.DpsX;
            dpsIncreaseXH = elementalstat.DpsIncreaseXH;
            castSpeedXH = elementalstat.CastSpeedXH;

            // CASTINTERVAL이 0.5보다 낮아져선 안된다. ( 실제는 0.8 )
            CASTINTERVAL = UnityEngine.Mathf.Max(0.8f,((float)BaseCastIntervalXK / (float)(castSpeedXH * 10)));
            SKILLRATE = BaseSkillRate * elementalstat.skillrateIncreaseXH / 100;
            SKILLRATIO = BaseSkillRatio* elementalstat.skilldamageIncreaseXH/ 100;
            skill.SetStat(elementalstat.skilldamageIncreaseXH);

            if (CalDamage)
                CalDPS();

            Action_Change(this);
        }
        void CalDPS()
        {
            DPS = BaseDps * dpsX * (level * dpsIncreaseXH * castSpeedXH) / (DpsShiftDigit * 10000);
            DPSTICK = BaseDps * dpsX * (dpsIncreaseXH * castSpeedXH) / (DpsShiftDigit * 10000);
            DAMAGE = DPS * BaseCastIntervalXK / (castSpeedXH * 10); //  dps * CASTINTERVAL]
            skill.CalDamage(DAMAGE);
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
                    EndAttack();
            }

            skill.Update(targetID); // cast 이후의 post업데이트.
            AttackTimer += Time.deltaTime;
        }

        bool isSkillAttack = false; // 이번공격이 스킬인지 
        private bool JudgeSkill { get { return SKILLRATE > UnityEngine.Random.Range(0, 100); } }


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