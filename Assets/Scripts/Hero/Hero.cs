using System;
using System.Collections.Generic;
using UnityEngine;
namespace SexyBackPlayScene
{
    internal class Hero : CanLevelUp
    {
        private HeroData baseData;
        private BigInteger exp = new BigInteger();
        private BigInteger baseDpc = new BigInteger();
        private BigInteger dpcX = new BigInteger(); // 곱계수는 X를붙인다.
        private int level;

        int attackspeedXH = 100; // speed 와 interval은 역수관계

        // 최종적으로 나가는 값은 모두 대문자이다. 중간과정은 앞에만대문자;
        // data property
        public string ID { get { return baseData.ID; } }
        public string NAME { get { return baseData.Name; } }
        public int LEVEL { get {return level; } }
        public BigInteger DPC { get { return baseDpc * dpcX; } }
        public string BASEDPC { get { return baseDpc.ToSexyBackString(); } }
        public string NEXTDPC { get { return baseData.BaseDpcPool[level].ToSexyBackString(); } } 
        public BigIntExpression NEXTEXPSTR { get { return baseData.BaseExpPool[level]; } } 
        public double ATTACKINTERVAL { get { return baseData.ATTACKINTERVAL * 100 / attackspeedXH; } }   // 공속공식
        public double CRIRATE { get { return baseData.CRIRATE ; } }
        public int CRIDAMAGE { get { return baseData.CRIDAMAGE ; } }
        public int MOVESPEED { get { return baseData.MOVESPEED; } }

        // for view action, state
        public HeroStateMachine stateMachine;
        public string targetID;
        public Vector3 AttackMoveVector;
        public Vector3 LastTapPosition;
        public Vector3 LastEffectPosition;
        public double ATTACKSPEED { get { return attackspeedXH / 100; } }

        // flag property
        private bool isCritical { get { return CRIRATE > UnityEngine.Random.Range(0.0f, 1.0f); } }


        public Hero(HeroData data)
        {
            baseData = data;
            exp = 0;
            AddLevel(1);
            dpcX = new BigInteger(1);

            stateMachine = new HeroStateMachine(this);

            //
            AttackMoveVector = new Vector3(0, 1.5f, -3) - GameSetting.defaultCameraPosition;

        }

        internal void Move(Vector3 step)
        {
            ViewLoader.camera.transform.position += step;
            ViewLoader.hero.transform.position += step;
        }


        internal void Stop()
        {
            ViewLoader.camera.transform.position = GameSetting.defaultCameraPosition;
            ViewLoader.hero.transform.position = GameSetting.defaultHeroPosition;
        }
        internal void Update()
        {
            stateMachine.Update();
        }

        public void Attack()
        {

            if (isCritical)
            {
                BigInteger totaldamage = DPC * CRIDAMAGE / 100;
                Singleton<MonsterManager>.getInstance().onHit(targetID, totaldamage, LastTapPosition);
                // 크리티컬 글자 필요 
            }
            else
            {
                Singleton<MonsterManager>.getInstance().onHit(targetID, DPC, LastTapPosition);
            }
        }
        public void onTouch(Vector3 tapposition, Vector3 effectposition)
        {
            LastTapPosition = tapposition;
            LastEffectPosition = effectposition;

            stateMachine.onTouch();
        }

        internal void SetSwordEffectPosition()
        {
            //TODO: 아예 EFFECT 카메라를 따로둬서 EFFECT PANEL을 히어로에 붙이지 않는걸 고려해봐야할듯
            Vector3 directionVector;
            Vector3 currMonCenter = Singleton<MonsterManager>.getInstance().GetMonster().CenterPosition;

            directionVector = currMonCenter - LastTapPosition;
            float rot = UnityEngine.Mathf.Atan2(directionVector.y, directionVector.x) * UnityEngine.Mathf.Rad2Deg;

            ViewLoader.hero_sword.transform.eulerAngles = new Vector3(0, 0, rot);
            ViewLoader.hero_sword.transform.position = new Vector3(LastEffectPosition.x, LastEffectPosition.y, ViewLoader.hero_sword.transform.position.z);
        }

        // get set private
        public BigInteger GainExp(BigInteger damage)
        {
            exp += damage;
            return exp;
        }
        internal BigInteger UseExp(BigInteger price)
        {
            exp -= price;
            return exp;
        }

        internal void AddLevel(int amount) // 레벨이 10이면 9까지더해야한다;
        {
            if (level + amount > baseData.MaxLevel)
                return;

            for (int i = level; i < level + amount; i++)
                baseDpc += new BigInteger(baseData.BaseDpcPool[i]);
            level += amount;
        }
    }
}
