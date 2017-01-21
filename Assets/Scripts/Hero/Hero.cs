using System;
using System.Collections.Generic;
using UnityEngine;
namespace SexyBackPlayScene
{

    internal class Hero
    {


        private HeroData baseData;
        private BigInteger baseDpc = new BigInteger();
        private BigInteger dpcX = new BigInteger(); // 곱계수는 X를붙인다.
        private int level;

        int attackcount;
        double attackTimer; // 공격쿨타임카운터
        int bounsAttackCount; // 보너스 공격스택횟수

        int attackspeedXH = 100; // speed 와 interval은 역수관계

        // 최종적으로 나가는 값은 모두 대문자이다. 중간과정은 앞에만대문자;
        // data property
        public int MAXATTACKCOUNT {  get { return baseData.ATTACKCOUNT + bounsAttackCount; } }
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
        public Queue<TapPoint> AttackPlan;
        public HeroStateMachine StateMachine;
        public string targetID;
        public Vector3 AttackMoveVector;
        public double ATTACKSPEED { get { return attackspeedXH / 100; } }

        // flag property
        private bool isCritical { get { return CRIRATE > UnityEngine.Random.Range(0.0f, 1.0f); } }

        public bool CanAttack { get { return attackcount > 0; } }

        public Hero(HeroData data)
        {
            baseData = data;
            AddLevel(1);
            dpcX = new BigInteger(1);
            attackTimer = 0;
            attackcount = 100;
            bounsAttackCount = 4; // 나중에 설정

            AttackPlan = new Queue<TapPoint>();

            StateMachine = new HeroStateMachine(this);
            //
            AttackMoveVector = new Vector3(0, 1.5f, -3) - GameSetting.defaultCameraPosition;

        }

        public void MakeAttackPlan(TapPoint point)
        {
            AttackPlan.Enqueue(point);
            attackcount--;
            //AttackCount++; AttackPlan 의 사이즈가된다.
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
            CheckCoolDown();
            StateMachine.Update();
        }

        private void CheckCoolDown()
        {
            if(attackcount < MAXATTACKCOUNT)
            {
                attackTimer += Time.deltaTime;
                ViewLoader.Bar_Attack.GetComponent<UISlider>().value = (float)(attackTimer / ATTACKINTERVAL);
                if (attackTimer > ATTACKINTERVAL)
                {
                    attackcount++;
                    attackTimer -= ATTACKINTERVAL;
                }
            }
            else // staic is max
                ViewLoader.Bar_Attack.GetComponent<UISlider>().value = 1;

            sexybacklog.InGame(attackcount);
        }

        internal bool Attack(float attackSpeed)
        {
            ViewLoader.hero_sprite.GetComponent<Animator>().speed = attackSpeed;

            if (isCritical)
            {
                ViewLoader.hero_sprite.GetComponent<Animator>().SetTrigger("Attack");
                ViewLoader.hero_sword.GetComponent<Animator>().SetTrigger("Play");

                MoveCriticalEffect();


                BigInteger totaldamage = DPC * CRIDAMAGE / 100;
                Singleton<MonsterManager>.getInstance().onHit(targetID, totaldamage, LastTapPosition);
                // 크리티컬 글자 필요 
            }
            else
            {
                ViewLoader.hero_sprite.GetComponent<Animator>().SetTrigger("Attack");
                ViewLoader.hero_sword.GetComponent<Animator>().SetTrigger("Play");

                Singleton<MonsterManager>.getInstance().onHit(targetID, DPC, LastTapPosition);
            }
            return true;
        }

        public void Attack()
        {
        }
        public void onTouch(TapPoint pos)
        {
            //LastTapPosition = tapposition;
            //LastEffectPosition = effectposition;

            StateMachine.onTouch(pos);
        }

        public void SetSwordEffectPosition()
        {
            //TODO: 아예 EFFECT 카메라를 따로둬서 EFFECT PANEL을 히어로에 붙이지 않는걸 고려해봐야할듯
            Vector3 directionVector;
            Vector3 currMonCenter = Singleton<MonsterManager>.getInstance().GetMonster().CenterPosition;

            directionVector = currMonCenter - LastTapPosition;
            float rot = UnityEngine.Mathf.Atan2(directionVector.y, directionVector.x) * UnityEngine.Mathf.Rad2Deg;

            ViewLoader.hero_sword.transform.eulerAngles = new Vector3(0, 0, rot);
            ViewLoader.hero_sword.transform.position = new Vector3(LastEffectPosition.x, LastEffectPosition.y, ViewLoader.hero_sword.transform.position.z);
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
