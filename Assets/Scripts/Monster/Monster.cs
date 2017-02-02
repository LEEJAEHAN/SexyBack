using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class Monster : StateOwner, IDisposable// model
    {
        public string ID;
        public string Name;
        public int level;   
        public BigInteger HP;
        public BigInteger MAXHP;

        // view
        public GameObject avatar;
        public GameObject sprite;
        GameObject hitparticle = ViewLoader.hitparticle;
        GameObject damagefont = ViewLoader.DamageFont;

        // state
        public StateMachine<Monster> StateMachine;
        string StateOwner.ID { get { return ID; } }
        public string CurrentState { get { return StateMachine.currStateID; } }

        //size value
        public Vector3 CenterPosition; // 몬스터 중점의 world상 위치.
        public Vector3 Size;           // sprite size, collider size는 이것과 동기화.

        public delegate void monsterChangeEvent_Handler(Monster sender);
        public event monsterChangeEvent_Handler Action_MonsterChangeEvent;

        public MonsterStateMachine.StateChangeHandler Action_StateChangeEvent { set { StateMachine.Action_changeEvent += value; } }

        //TODO: 임시로작성.
        public bool isActive = false;

        internal Monster()
        {
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
            Singleton<StageManager>.getInstance().ExpGain(damage);
            sexybacklog.Console(damage);
            //particle
            PlayParticle(hitPosition);
            //damagefont
            PlayDamageFont(damage, hitPosition);

            //avatar
            sprite.GetComponent<Animator>().rootPosition = avatar.transform.position;
            if (isCritical)
                sprite.GetComponent<Animator>().SetTrigger("Hit_Critical");
            else
                sprite.GetComponent<Animator>().SetTrigger("Hit");

            Action_MonsterChangeEvent(this);

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
            hitparticle = null;
            damagefont = null;
            Action_MonsterChangeEvent = null;
            isActive = false;
        }

        void PlayParticle(Vector3 position)
        {
            hitparticle.transform.position = position;
            hitparticle.GetComponent<ParticleSystem>().Play();
        }

        void PlayDamageFont(BigInteger dmg, Vector3 position)
        {
            Vector3 screenpos = ViewLoader.HeroCamera.WorldToScreenPoint(position);
            damagefont.SetActive(true);
            damagefont.GetComponent<UITweener>().PlayForward();
            damagefont.GetComponent<UITweener>().ResetToBeginning();
            damagefont.transform.localPosition = screenpos;
            damagefont.GetComponent<UILabel>().text = dmg.To5String();
            damagefont.GetComponent<UILabel>().fontSize = (int)((30 + 10 * (10 - screenpos.z)));
        }

        ~Monster()
        {
            sexybacklog.Console("몬스터소멸!");
        }
    }
}