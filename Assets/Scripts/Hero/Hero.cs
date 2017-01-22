using System;
using System.Collections.Generic;
using UnityEngine;
namespace SexyBackPlayScene
{
    internal class Hero
    {
        private HeroData baseData;
        private GameObject avatar;
        private BigInteger baseDpc = new BigInteger();
        private BigInteger dpcX = new BigInteger(); // 곱계수는 X를붙인다.
        private int level;

        int attackcount;
        double attackTimer; // 공격쿨타임카운터
        int bounsAttackCount; // 보너스 공격스택횟수

        int attackspeedXH = 100; // speed 와 interval은 역수관계

        // 최종적으로 나가는 값은 모두 대문자이다. 중간과정은 앞에만대문자;
        // data property
        public int MAXATTACKCOUNT { get { return baseData.ATTACKCOUNT + bounsAttackCount; } }
        public string ID { get { return baseData.ID; } }
        public string NAME { get { return baseData.Name; } }
        public int LEVEL { get { return level; } }
        public BigInteger DPC { get { return baseDpc * dpcX; } }
        public string BASEDPC { get { return baseDpc.ToSexyBackString(); } }
        public string NEXTDPC { get { return baseData.BaseDpcPool[level].ToSexyBackString(); } }
        public BigIntExpression NEXTEXPSTR { get { return baseData.BaseExpPool[level]; } }
        public double ATTACKINTERVAL { get { return baseData.ATTACKINTERVAL * 100 / attackspeedXH; } }   // 공속공식
        public double CRIRATE { get { return baseData.CRIRATE; } }
        public int CRIDAMAGE { get { return baseData.CRIDAMAGE; } }
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
            avatar = ViewLoader.HeroPanel;
            AddLevel(1);
            dpcX = new BigInteger(1);
            attackTimer = 0;
            attackcount = 0;
            bounsAttackCount = 6; // 나중에 설정

            AddAttackCount();
            AddAttackCount();
            AddAttackCount();
            AddAttackCount();
            AddAttackCount();
            AddAttackCount();
            AddAttackCount();

            AttackPlan = new Queue<TapPoint>();

            StateMachine = new HeroStateMachine(this);
            //
            AttackMoveVector = GameSetting.ECamPosition - GameSetting.defaultHeroPosition;

        }

        public void MakeAttackPlan(TapPoint point)
        {
            if (!CanAttack)
                return;
            AttackPlan.Enqueue(point);
            ReduceAttackCount();
            //AttackCount++; AttackPlan 의 사이즈가된다.
        }

        internal void Move(Vector3 step)
        {
            avatar.transform.position += step;
        }
        internal void Warp(Vector3 position)
        {
            //avatar.transform.position = GameSetting.defaultHeroPosition;
            avatar.transform.position = position;
        }

        internal void Update()
        {
            CheckCoolDown();
            StateMachine.Update();
        }

        private void CheckCoolDown()
        {
            if (attackcount < MAXATTACKCOUNT)
            {
                attackTimer += Time.deltaTime;
                ViewLoader.Bar_Attack.GetComponent<UISlider>().value = (float)(attackTimer / ATTACKINTERVAL);
                if (attackTimer > ATTACKINTERVAL)
                {
                    AddAttackCount();
                    attackTimer -= ATTACKINTERVAL;
                }
            }
            else // staic is max
                ViewLoader.Bar_Attack.GetComponent<UISlider>().value = 1;

            sexybacklog.InGame(attackcount);
        }


        //TODO : 어택판넬을 아예 다른 클래스로 빼는게 어떨가 싶음.
        GameObject[] SwordIcons = new GameObject[10];
        GameObject SwordIcon = Resources.Load<GameObject>("prefabs/UI/attackcount");
        int[] iconangle = { 0, 30, -30, 60, -60, 90, -90};

        void AddAttackCount()
        {
            attackcount++;

            if (attackcount > 7)
                return;
            GameObject swordicon = GameObject.Instantiate(SwordIcon) as GameObject;
            swordicon.name = "attackcount" + attackcount;
            swordicon.transform.parent = ViewLoader.Bar_Attack.transform;
            swordicon.transform.localScale = Vector3.one;
            swordicon.transform.localPosition = Vector3.zero;
            swordicon.transform.rotation = Quaternion.Euler(0, 0, 45 + iconangle[attackcount-1]);
            SwordIcons[attackcount - 1] = swordicon;
        }

        void ReduceAttackCount()
        {
            attackcount--;
            if (attackcount < 0)
                return;

            if (SwordIcons[attackcount] != null)
                GameObject.Destroy(SwordIcons[attackcount]);
        }


        internal bool Attack(float attackSpeed)
        {
            if (AttackPlan.Count == 0 || targetID == null)
                return false;

            TapPoint p = AttackPlan.Dequeue();
            
            ViewLoader.hero_sprite.GetComponent<Animator>().speed = attackSpeed;
            BigInteger damage;
            if (isCritical)
            {
                damage = DPC * CRIDAMAGE / 100;
                MoveAndMakeEffect(p, true);
                // TODO : 크리티컬 글자 필요 
            }
            else
            {
                damage = DPC;
                MoveAndMakeEffect(p, false);
            }
            ViewLoader.hero_sprite.GetComponent<Animator>().SetTrigger("Attack");
            ViewLoader.hero_sword.GetComponent<Animator>().SetTrigger("Play");
            Singleton<MonsterManager>.getInstance().onHit(targetID, damage, p.GamePos);
            return true;
        }

        private void MoveAndMakeEffect(TapPoint Tap, bool isCritical)
        {
            //TODO: Instanstiate 로바꾸고, 크리티컬모션 따로 구현해야함
            Vector3 currMonCenter = Singleton<MonsterManager>.getInstance().GetMonster().CenterPosition;
            Vector3 directionVector = currMonCenter - Tap.GamePos;
            float rot = UnityEngine.Mathf.Atan2(directionVector.y, directionVector.x) * UnityEngine.Mathf.Rad2Deg;

            ViewLoader.hero_sword.transform.eulerAngles = new Vector3(0, 0, rot);
            Vector3 b = ViewLoader.EffectCamera.ViewportToWorldPoint(Tap.UiPos);
            ViewLoader.hero_sword.transform.position = Tap.UiPos;
            sexybacklog.Console("Hit:"+ViewLoader.hero_sword.transform.position);
        }

        public void Attack()
        {
        }
        public void onTouch(TapPoint pos)
        {
            StateMachine.onTouch(pos);
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
