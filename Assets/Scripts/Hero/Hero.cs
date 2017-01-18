using System;
using System.Collections.Generic;
using UnityEngine;
namespace SexyBackPlayScene
{
    internal class Hero : CanLevelUp
    {
        private HeroData baseData;
        public BigInteger DPC;
        public BigInteger EXP;
        public double ATTACKINTERVAL;
        public double CRIRATE;
        public int CRIDAMAGE;

        public bool ISCRITICAL { get { return CRIRATE > UnityEngine.Random.Range(0.0f, 1.0f); } }

        public HeroStateMachine stateMachine;

        public string targetID;

        public Vector3 MoveDirection;
        public int MOVESPEED;

        // 수정해야함. 임시
        public override string ID { get { return "Hero01"; } }
        public override string Name { get { return "Hero"; } }
        public override string Item_Text { get { return DPC.ToSexyBackString(); } } // 아이템버튼 우하단 텍스트
        public override string Info_Description { get { return "Damage : " + DPC.ToSexyBackString() + "/tap\n" + "Next : +" + DPC.ToSexyBackString() + "/tap"; } }
        public override BigInteger PriceToNextLv { get { return new BigInteger(10); } }
        // 임시

        public Vector3 lastTapPosition;
        public Vector3 SwordPosition;

        public Hero(HeroData data)
        {
            DPC = data.DPC;
            EXP = data.EXP;

            ATTACKINTERVAL = data.ATTACKINTERVAL;
            CRIRATE = data.CRIRATE;
            CRIDAMAGE = data.CRIDAMAGE;
            MOVESPEED = data.MOVESPEED;

            stateMachine = new HeroStateMachine(this);
            SetDirection(GameSetting.defaultMonsterPosition);
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
        public void SetDirection(Vector3 monsterPosition)
        {
            MoveDirection = new Vector3(0, 1.5f, -3) - GameSetting.defaultCameraPosition;
        }

        internal void Update()
        {
            stateMachine.Update();
        }

        public void Attack()
        {
            ViewLoader.hero_sword.GetComponent<Animator>().SetTrigger("Play");
            ViewLoader.hero_sprite.GetComponent<Animator>().SetTrigger("Attack");

            if (ISCRITICAL)
            {
                BigInteger totaldamage = DPC * CRIDAMAGE / 100;
                Singleton<MonsterManager>.getInstance().onHit(targetID, totaldamage, lastTapPosition);
                // 크리티컬 글자 필요 
            }
            else
            {
                Singleton<MonsterManager>.getInstance().onHit(targetID, DPC, lastTapPosition);
            }
        }
        public void onTouch(Vector3 tapposition, Vector3 effectposition)
        {
            lastTapPosition = tapposition;
            SwordPosition = effectposition;

            stateMachine.onTouch();
        }

        internal void GainExp(BigInteger damage)
        {
            EXP += damage;
        }

        public void IncreaseDPC(BigInteger amount)
        {
            DPC += amount;
        }

        internal BigInteger GetTotalDPC()
        {
            return DPC;
        }

        internal void SetSwordEffectPosition()
        {
            Vector3 directionVector;
            Vector3 currMonCenter = Singleton<MonsterManager>.getInstance().GetMonster().CenterPosition;

            sexybacklog.Console(currMonCenter + " center");

            directionVector = currMonCenter - lastTapPosition;
            float rot = UnityEngine.Mathf.Atan2(directionVector.y, directionVector.x) * UnityEngine.Mathf.Rad2Deg;

            ViewLoader.hero_sword.transform.eulerAngles = new Vector3(0, 0, rot);
            ViewLoader.hero_sword.transform.position = new Vector3(SwordPosition.x, SwordPosition.y, ViewLoader.hero_sword.transform.position.z);

        }
    }
}
