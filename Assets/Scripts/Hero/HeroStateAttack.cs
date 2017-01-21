using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class HeroStateAttack : HeroState
    {
        double timer;
        double swingtimer;
        double AttackCount;
        double AttackSpeed;
        double additionalattackcount;

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

        double ForwardSpeed { get { return 100 / (AttackCount * DashRate); } }
        double BackwardSpeed { get { return 100 / (AttackCount * BackRate); } }

        double SwingActionTime { get { return 0.15f * AttackSpeed; } } // effect 재생이 0.15초로 가장김

        public HeroStateAttack(HeroStateMachine stateMachine, Hero owner) : base(stateMachine, owner)
        {
            timer = 0;
            additionalattackcount = 0;
            swingtimer = 0;
            AttackCount = owner.ATTACKINTERVAL;  // 중간에 값이 업데이트되도 무시하기위해
            AttackSpeed = owner.ATTACKSPEED;

            DashRate = 8;
            SwingRate = 12; // 1초 0.15초히어로 공격을 8번까지 할수있다.
            BackRate = 24;
            WaitRate = 100 - DashRate - SwingRate - BackRate;

            DashTime = AttackCount * DashRate / 100;
            SwingTime = AttackCount * SwingRate / 100;
            BackTime = AttackCount * BackRate / 100;
            waitTime = AttackCount * BackRate / 100;
        }

        internal override void Begin()
        {
            owner.Stop();
        }

        internal override void End()
        {
            owner.Stop();
        }

        internal override void OnTouch(TapPoint pos)
        {
            if(!BeginBack && owner.CanAttack) // 후퇴전까지는 추가타 이벤트를 받는다;
            {
                owner.MakeAttackPlan(pos);
                additionalattackcount++;
            }
        }

        void ChangeState() // 한번만힐행된다
        {
            if (!BeginDash)
            {
                BeginDash = true;
                ViewLoader.hero_sprite.GetComponent<Animator>().speed = (float)(AttackSpeed * BackRate / DashRate);
                ViewLoader.hero_sprite.GetComponent<Animator>().SetBool("Move", true);
            }
            else if (!BeginSwing && timer > DashTime)
            {
                BeginSwing = true;
            }
            else if (!BeginBack && timer > DashTime + SwingTime)
            {
                BeginBack = true;
                ViewLoader.hero_sprite.GetComponent<Animator>().speed = (float)AttackSpeed;
                ViewLoader.hero_sprite.GetComponent<Animator>().SetBool("Move", true);
            }
            else if (!BeginWait && timer > DashTime + SwingTime + BackTime)
            {
                BeginWait = true;
                ViewLoader.hero_sprite.GetComponent<Animator>().SetBool("Move", false);
                owner.Stop();
            }
        }

        internal override void Update()
        {
            timer += Time.deltaTime;
            swingtimer += Time.deltaTime;

            ChangeState();

            if (BeginDash && !BeginSwing)  // during dash
            {
                owner.Move(owner.AttackMoveVector * (float)ForwardSpeed * Time.deltaTime);
            }
            if (BeginSwing && !BeginBack)  // during attack
            {
                if(swingtimer > SwingActionTime && owner.AttackPlan.Count > 0) // 0.1로마다 들어와야한다. timer를 0.1f만큼빼준다.
                {
                    if (owner.Attack((float)AttackSpeed))
                    {
                        timer -= SwingActionTime; // 공격을 한번더할수있게 타이머카운터를 빼준다.
                        waitTime -= SwingActionTime; // 대기시간에서빠진다.
                        swingtimer -= SwingActionTime;
                    }
                }
            }
            else if (BeginBack && !BeginWait) // during back
            {
                owner.Move(-(owner.AttackMoveVector * (float)BackwardSpeed * Time.deltaTime));
            }
            else if (BeginWait)
            {
                waitTime -= Time.deltaTime;
                if(waitTime <= 0)
                    stateMachine.ChangeState(new HeroStateReady(stateMachine, owner));
            }
        }

        // AttackTimer(총시간) * 전진Ratio = 총 전진시간
        // 1초에가는벡터 * 1/전진시간  = 1초에가는벡터 * 전진스피드 = 총 이동량(고정) 
        // Vector3 step = owner.MoveDirection * (float)ForwardSpeed;
        // 전진스피드 = 1 / (총시간 * 전진Ratio)
        // 한업데이트당 총이동량 * time.deltatime 만큼 이동하면됨;
    }
}