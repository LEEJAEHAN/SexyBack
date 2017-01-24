using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class HeroStateAttack : HeroState
    {
        double timer;
        double swingtimer;
        double ActionTime;
        double AttackSpeed;
        double additionalattackcount;

        int DashRate = 25;
        int SwingRate = 25;
        int BackRate = 25;

        double DashTime;
        double SwingTime;
        double BackTime;

        bool BeginDash = false;
        bool BeginSwing = false;
        bool BeginBack = false;

        double ForwardSpeed { get { return 100 / (ActionTime * DashRate); } }
        double BackwardSpeed { get { return 100 / (ActionTime * BackRate); } }

        double SwingActionTime { get { return 0.15f * AttackSpeed; } } // effect 재생이 0.15초로 가장김

        public Vector3 AttackMoveVector;


        public HeroStateAttack(HeroStateMachine stateMachine, Hero owner) : base(stateMachine, owner)
        {
            timer = 0;
            additionalattackcount = 0;
            swingtimer = 0;
            ActionTime = owner.ATTACKINTERVAL;  // 중간에 값이 업데이트되도 무시하기위해
            AttackSpeed = owner.ATTACKSPEED;

            DashRate = 25;
            SwingRate = 25; // 1초 0.15초히어로 공격을 8번까지 할수있다.
            BackRate = 50;

            DashTime = ActionTime * DashRate / 100;
            SwingTime = ActionTime * SwingRate / 100;
            BackTime = ActionTime * BackRate / 100;

            AttackMoveVector = GameSetting.ECamPosition - GameSetting.defaultHeroPosition;
        }

        internal override void Begin()
        {
            hero.Warp(GameSetting.defaultHeroPosition);
        }

        internal override void End()
        {
            hero.Warp(GameSetting.defaultHeroPosition);
        }

        internal override void OnTouch(TapPoint pos)
        {
            if(!BeginBack && hero.AttackManager.CanMakePlan) // 후퇴전까지는 추가타 이벤트를 받는다;
            {
                hero.AttackManager.MakeAttackPlan(pos);
                additionalattackcount++;
                //sexybacklog.Console("Tap:" + pos.EffectPos);//WorldPos
            }
        }

        void ChangeState() // 한번만힐행된다. begin each state
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
                hero.Warp(GameSetting.ECamPosition);
                ViewLoader.hero_sprite.GetComponent<Animator>().SetBool("Move", false);
            }
            else if (!BeginBack && timer > DashTime + SwingTime)
            {
                BeginBack = true;
                ViewLoader.hero_sprite.GetComponent<Animator>().speed = (float)AttackSpeed;
                ViewLoader.hero_sprite.GetComponent<Animator>().SetBool("Move", true);
            }
            else if (timer > DashTime + SwingTime + BackTime)
                stateMachine.ChangeState(new HeroStateReady(stateMachine, hero));

        }

        internal override void Update()
        {
            timer += Time.deltaTime;
            swingtimer += Time.deltaTime;

            ChangeState();

            if (BeginDash && !BeginSwing)  // during dash
                hero.Move(AttackMoveVector * (float)ForwardSpeed * Time.deltaTime);
            else if (BeginSwing && !BeginBack)  // during attack
            {
                if (swingtimer > SwingActionTime && hero.AttackManager.CanAttack) // 0.1로마다 들어와야한다. timer를 0.1f만큼빼준다.
                {
                    if (hero.Attack((float)AttackSpeed))
                    {
                        timer -= SwingActionTime; // 공격을 한번더할수있게 타이머카운터를 빼준다.
                        swingtimer = 0;
                    }
                }
            }
            else if (BeginBack) // during back
                hero.Move(-(AttackMoveVector * (float)BackwardSpeed * Time.deltaTime));
        }
        // AttackTimer(총시간) * 전진Ratio = 총 전진시간
        // 1초에가는벡터 * 1/전진시간  = 1초에가는벡터 * 전진스피드 = 총 이동량(고정) 
        // Vector3 step = owner.MoveDirection * (float)ForwardSpeed;
        // 전진스피드 = 1 / (총시간 * 전진Ratio)
        // 한업데이트당 총이동량 * time.deltatime 만큼 이동하면됨;
    }
}
