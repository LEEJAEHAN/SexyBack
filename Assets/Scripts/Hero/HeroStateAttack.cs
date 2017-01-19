using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class HeroStateAttack : HeroState
    {
        double timer;
        double AttackInterval;
        double AttackSpeed;

        int DashRate = 25;
        int SwingRate = 25;
        int BackRate = 25;
        int WaitRate = 25;

        double DashTime;
        double SwingTime;
        double BackTime;
        double waitTime;

        bool BeginDash = false;
        bool BeginSwing = false;
        bool BeginBack = false;
        bool BeginWait = false;

        double ForwardSpeed { get { return 100 / (AttackInterval * DashRate); } }
        double BackwardSpeed { get { return 100 / (AttackInterval * BackRate); } }

        public HeroStateAttack(HeroStateMachine stateMachine, Hero owner) : base(stateMachine, owner)
        {
            timer = 0;
            AttackInterval = owner.ATTACKINTERVAL;  // 중간에 값이 업데이트되도 무시하기위해
            AttackSpeed = owner.ATTACKSPEED;

            DashRate = 8;
            SwingRate = 10;
            BackRate = 24;
            WaitRate = 100 - DashRate - SwingRate - BackRate;

            DashTime = AttackInterval * DashRate / 100;
            SwingTime = AttackInterval * SwingRate / 100;
            BackTime = AttackInterval * BackRate / 100;
            waitTime = AttackInterval * BackRate / 100;
        }

        internal override void Begin()
        {
            owner.Stop();
        }

        internal override void End()
        {
            owner.Stop();
        }

        internal override void OnTouch()
        {

        }

        void ChangeState() // 한번만힐행된다
        {
            if (!BeginDash)
            {
                BeginDash = true;
                ViewLoader.hero_sprite.GetComponent<Animator>().speed = (float)(AttackSpeed * BackRate / DashRate);
                ViewLoader.hero_sprite.GetComponent<Animator>().SetTrigger("Move");
            }
            else if (!BeginSwing && timer > DashTime)
            {
                BeginSwing = true;
                ViewLoader.hero_sprite.GetComponent<Animator>().speed = (float)AttackSpeed;
                ViewLoader.hero_sprite.GetComponent<Animator>().SetTrigger("Attack");
                ViewLoader.hero_sword.GetComponent<Animator>().SetTrigger("Play");
                owner.Attack();  // TODO: 공격애니메이션 시간동안 잠깐 대기가 필요할수도
            }
            else if (!BeginBack && timer > DashTime + SwingTime)
            {
                BeginBack = true;
                ViewLoader.hero_sprite.GetComponent<Animator>().speed = (float)AttackSpeed;
                ViewLoader.hero_sprite.GetComponent<Animator>().SetTrigger("Move");
            }
            else if (!BeginWait && timer > DashTime + SwingTime + BackTime)
            {
                BeginWait = true;
                ViewLoader.hero_sprite.GetComponent<Animator>().SetTrigger("Stop");
                owner.Stop();
            }
            else if (timer > AttackInterval)
            {
                stateMachine.ChangeState(new HeroStateReady(stateMachine, owner));
            }
        }

        internal override void Update()
        {
            timer += Time.deltaTime;

            ChangeState();

            if (BeginDash && !BeginSwing)  // during dash
            {
                owner.Move(owner.AttackMoveVector * (float)ForwardSpeed * Time.deltaTime);
            }
            else if (BeginBack && !BeginWait) // during back
            {
                owner.Move(-(owner.AttackMoveVector * (float)BackwardSpeed * Time.deltaTime));
            }
        }

        // AttackTimer(총시간) * 전진Ratio = 총 전진시간
        // 1초에가는벡터 * 1/전진시간  = 1초에가는벡터 * 전진스피드 = 총 이동량(고정) 
        // Vector3 step = owner.MoveDirection * (float)ForwardSpeed;
        // 전진스피드 = 1 / (총시간 * 전진Ratio)
        // 한업데이트당 총이동량 * time.deltatime 만큼 이동하면됨;
    }
}