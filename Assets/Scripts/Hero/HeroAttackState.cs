using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class HeroAttackState : HeroState
    {
        double AttackTimer;
        double AttackInterval;
        double AttackCount;

        int ForwardRate = 300;
        int BackwardRate = 500;
        int AttackDelayRate = 200;
        bool AttackFinish = false;

        double ForwardMoveTime { get { return AttackInterval * ForwardRate / 1000; } }
        double BackwardMoveTime { get { return AttackInterval * BackwardRate / 1000; } }
        double ForwardSpeed { get { return 1000 / (AttackInterval * ForwardRate); } }
        double BackwardSpeed { get { return 1000 / (AttackInterval * BackwardRate); } }

        public HeroAttackState(HeroStateMachine stateMachine, Hero owner) : base(stateMachine, owner)
        {
            AttackTimer = 0;
            AttackInterval = owner.ATTACKINTERVAL;  // 중간에 값이 업데이트되도 무시하기위해
            AttackCount = 1;
            AttackFinish = false;

            ForwardRate = 75;
            BackwardRate = 250;
            AttackDelayRate = 1000 - ForwardRate - BackwardRate;
        }

        internal override void Begin()
        {
            owner.Stop();
            ViewLoader.hero_sprite.GetComponent<Animator>().SetTrigger("Move");
        }

        internal override void End()
        {
            owner.Stop();
        }

        internal override void OnTouch()
        {
            if (AttackCount > 0)
                owner.SetSwordEffectPosition();
        }

        internal override void Update()
        {
            AttackTimer += Time.deltaTime;

            if (AttackTimer < ForwardMoveTime) // go front
            {
                //                owner.Move();
                // AttackTimer(총시간) * 전진Ratio = 총 전진시간
                // 1초에가는벡터 * 1/전진시간  = 1초에가는벡터 * 전진스피드 = 총 이동량(고정) 
                // Vector3 step = owner.MoveDirection * (float)ForwardSpeed;
                // 전진스피드 = 1 / (총시간 * 전진Ratio)
                // 한업데이트당 총이동량 * time.deltatime 만큼 이동하면됨;

                owner.Move(owner.MoveDirection * (float)ForwardSpeed * Time.deltaTime);
            }
            else if (AttackTimer >= ForwardMoveTime  && AttackTimer < (ForwardMoveTime + BackwardMoveTime)) // go back
            {
                if(AttackCount > 0 ) // attack;
                {
                    AttackCount--;
                    owner.Attack();
                }
                owner.Move(owner.MoveDirection * (float)BackwardSpeed * -Time.deltaTime);
            }
            else if( AttackTimer >= ForwardMoveTime + BackwardMoveTime && AttackTimer < AttackInterval)
            {
                if (AttackFinish == false)
                {
                    AttackFinish = true;
                    ViewLoader.hero_sprite.GetComponent<Animator>().SetTrigger("Stop");
                    owner.Stop();
                }
            }
            else // AttackTimer >= AttackInterval
            {
                stateMachine.ChangeState(new HeroStateReady(stateMachine, owner));
            }

        }
    }
}