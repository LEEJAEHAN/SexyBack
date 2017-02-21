using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class Monster : IStateOwner, IDisposable// model
    {
        readonly string ID;
        public string GetID { get { return ID; } }
        public string Name;
        public int level;   
        public BigInteger HP;
        public BigInteger MAXHP;

        // view
        public GameObject avatar;
        public GameObject sprite;

        // state
        public StateMachine<Monster> StateMachine;
        public string CurrentState { get { return StateMachine.currStateID; } }

        //size value
        public Vector3 CenterPosition; // 몬스터 중점의 world상 위치.
        public Vector3 Size;           // sprite size, collider size는 이것과 동기화.

        public MonsterStateMachine.StateChangeHandler Action_StateChangeEvent { set { StateMachine.Action_changeEvent += value; } }

        //TODO: 임시로작성.
        public bool isActive = false;

        internal Monster(string InstsanceID)
        {
            ID = InstsanceID;
        }

        internal void Join() // join the battle
        {
            isActive = true;
            avatar.SetActive(true);
            StateMachine.ChangeState("Appear");
        }

        public void Update()
        {
            if (isActive)
                StateMachine.Update();
        }

        internal bool Hit(Vector3 hitPosition, BigInteger damage, bool isCritical)
        {
            HP -= damage;
            Singleton<Player>.getInstance().ExpGain(damage);
            sexybacklog.Console(damage);
            //particle
            EffectController.getInstance.PlayParticle(hitPosition);
            //damagefont
            EffectController.getInstance.PlayDamageFont(damage, hitPosition);

            //avatar
            sprite.GetComponent<Animator>().rootPosition = avatar.transform.position;
            if (isCritical)
                sprite.GetComponent<Animator>().SetTrigger("Hit_Critical");
            else
                sprite.GetComponent<Animator>().SetTrigger("Hit");

            if (HP < 0)
                HP = 0;

            Singleton<MonsterManager>.getInstance().onHit(this);

            if (HP <= 0) // dead check
            {
                if (StateMachine.currStateID == "Ready")
                    StateMachine.ChangeState("Flying");
                return false; // will be destroyed;
            }
            else
                return true;
        }

        public void Dispose()
        {
            HP = null;
            MAXHP = null;
            avatar.GetComponent<MonsterView>().Dispose();
            avatar = null;
            sprite = null;
            StateMachine = null;
            isActive = false;
        }

        ~Monster()
        {
            sexybacklog.Console("몬스터소멸!");
        }
    }
}