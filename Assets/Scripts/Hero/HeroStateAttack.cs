using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class HeroStateAttack : BaseState<Hero>
    {
        double timer = 0;
        double swingtimer = 0;
        double ActionTime;
        double AttackSpeed;

        int DashRate = 15;
        int SwingRate = 20; // 1.25초 0.15초히어로 공격을 여러번 할수있다.
        int BackRate = 25;

        double DashTime { get { return ActionTime * DashRate / 100; } }
        double SwingTime { get { return ActionTime * SwingRate / 100; } }
        double BackTime { get { return ActionTime * BackRate / 100; } }

        double ForwardSpeed { get { return 100 / (ActionTime * DashRate); } }
        double BackwardSpeed { get { return 100 / (ActionTime * BackRate); } }
        // effect 재생이 0.15초로 가장김
        double SwingActionTime { get { return 0.5f / AttackSpeed; } }

        HeroMiniState State;
        public Vector3 AttackMoveVector = GameSetting.ECamPosition - GameSetting.HeroCamPosition;

        public HeroStateAttack(Hero owner, HeroStateMachine stateMachine) : base(owner, stateMachine)
        {
        }
        internal override void Begin()
        {
            ActionTime = owner.ATTACKINTERVAL;  // 중간에 값이 업데이트되도 무시하기위해
            AttackSpeed = owner.ATTACKSPEED;
            State = HeroMiniState.None;
            Singleton<GameInput>.getInstance().Action_TouchEvent += onTouch;
            owner.Warp(GameSetting.HeroCamPosition);
        }

        internal override void End()
        {
            Singleton<GameInput>.getInstance().Action_TouchEvent -= onTouch;
            owner.Warp(GameSetting.HeroCamPosition);
        }

        internal void onTouch(TapPoint pos)
        {
            if (State != HeroMiniState.Dash && State != HeroMiniState.Swing)
                return;
            if (owner.AttackManager.CanMakePlan) // 후퇴전까지는 추가타 이벤트를 받는다;
                owner.AttackManager.MakeAttackPlan(pos);
        }

        void ChangeState() // 한번만힐행된다. begin each state
        {
            if (State == HeroMiniState.None && timer <= DashTime)
            {
                State = HeroMiniState.Dash;
                owner.Animator.speed = (float)(AttackSpeed * BackRate / DashRate);
                owner.Animator.SetBool("Move", true);
                return;
            }
            else if (State == HeroMiniState.Dash && timer > DashTime)
            {
                State = HeroMiniState.Swing;
                owner.Warp(GameSetting.ECamPosition);
                owner.Animator.SetBool("Move", false);
                return;
            }
            else if (State == HeroMiniState.Swing && timer > DashTime + SwingTime)
            {
                State = HeroMiniState.Back;
                owner.Animator.speed = (float)AttackSpeed;
                owner.Animator.SetBool("Move", true);
                return;
            }
            else if (timer > DashTime + SwingTime + BackTime)
            {
                stateMachine.ChangeState("Ready");
            }
        }

        internal override void Update()
        {
            timer += Time.deltaTime;
            swingtimer += Time.deltaTime;

            ChangeState();

            switch (State)
            {
                case HeroMiniState.Dash:
                    {
                        owner.Move(AttackMoveVector * (float)ForwardSpeed * Time.deltaTime);
                        break;
                    }
                case HeroMiniState.Swing:
                    {
                        if (swingtimer > SwingActionTime) // 0.1로마다 들어와야한다. timer를 0.1f만큼빼준다.
                        {
                            if (owner.Attack((float)AttackSpeed))
                            {
                                timer -= SwingActionTime; // 공격을 한번더할수있게 타이머카운터를 빼준다.
                                swingtimer = 0;
                            }
                        }
                        break;
                    }
                case HeroMiniState.Back:
                    {
                        owner.Move(-(AttackMoveVector * (float)BackwardSpeed * Time.deltaTime));
                        break;
                    }
            }
        }
        enum HeroMiniState
        {
            None,
            Dash,
            Swing,
            Back,
        };
        // AttackTimer(총시간) * 전진Ratio = 총 전진시간
        // 1초에가는벡터 * 1/전진시간  = 1초에가는벡터 * 전진스피드 = 총 이동량(고정) 
        // Vector3 step = owner.MoveDirection * (float)ForwardSpeed;
        // 전진스피드 = 1 / (총시간 * 전진Ratio)
        // 한업데이트당 총이동량 * time.deltatime 만큼 이동하면됨;
    }
}
