using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class Monster : StateOwner, IDisposable// model
    {
        public string ID;
        public string Name;
        public BigInteger HP;
        public BigInteger MAXHP;

        // view
        public GameObject avatar;

        // state
        public StateMachine<Monster> StateMachine;
        string StateOwner.ID { get { return ID; } }
        public string CurrentState { get { return StateMachine.currStateID; } }
        public Animator Animator;

        //size value

        public Vector3 CenterPosition; // 몬스터 중점의 world상 위치.
        public Vector3 Size;           // sprite size, collider size는 이것과 동기화.
        public BoxCollider Collider { get { return avatar.GetComponent<BoxCollider>(); } }

        //hit effect
        GameObject hitparticle = ViewLoader.hitparticle;
        GameObject damagefont = ViewLoader.DamageFont;

        public delegate void monsterChangeEvent_Handler(Monster sender);
        public event monsterChangeEvent_Handler Action_MonsterChangeEvent;

        public MonsterStateMachine.StateChangeHandler Action_StateChangeEvent { set { StateMachine.Action_changeEvent += value; } }

        //TODO: 임시로작성.
        public bool isActive = false;

        internal Monster (MonsterData data)
        {
            ID = data.ID;
            MAXHP = data.MaxHP;
            HP = data.MaxHP;
            Name = data.Name;

            avatar = InitAvatar(data.SpritePath, ViewLoader.monsters.transform, data.LocalPosition); //data.PivotPosition

            Animator = avatar.GetComponent<Animator>();
            StateMachine = new MonsterStateMachine(this);

            //Action_MonsterChangeEvent(this);
            isActive = false;
            avatar.SetActive(false);
        }

        private GameObject InitAvatar(string spritepath, Transform parent, Vector3 localposition)
        {
            GameObject mob = GameObject.Instantiate<GameObject>(Resources.Load("Prefabs/monster") as GameObject);
            mob.name = ID;
            mob.transform.parent = parent; // genposition
            mob.transform.parent.localPosition += localposition;
            mob.transform.localPosition = Vector3.zero;
            mob.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(spritepath);
            mob.GetComponent<MonsterView>().Action_changeEvent += onAnimationFinish;

            Size = mob.GetComponent<SpriteRenderer>().sprite.bounds.size;
            CenterPosition = mob.transform.parent.position; // 피봇으로 옮겨간정도 + 원래의 위치  ( 실제 위치는 옮겨놓지않았기떄문에 monsters(부모)의위치를더함 )                                                                     // pivot으로 몬스터위치조정은힘들어서 collider와 sprite만조정한다.

            mob.GetComponent<BoxCollider>().size = Size;
            mob.GetComponent<BoxCollider>().center = Vector3.zero;

            sexybacklog.Console(Size + " " + localposition + " " + CenterPosition);
            return mob;
        }

        internal void Join() // join the battle
        {
            isActive = true;
            avatar.SetActive(true);
            StateMachine.ChangeState("Appear");
        }

        void onAnimationFinish(string monsterid, string stateID)
        {
            if (stateID == "Appear")
                StateMachine.ChangeState("Ready");
        }

        public void Update()
        {
            if(isActive)
                StateMachine.Update();
        }


        internal bool Hit(Vector3 hitPosition, BigInteger damage, bool isCritical)
        {
            HP -= damage;
            Singleton<GameMoney>.getInstance().ExpGain(damage);
            sexybacklog.Console(damage);
            //particle
            PlayParticle(hitPosition);
            //damagefont
            PlayDamageFont(damage, hitPosition);

            //avatar
            Animator.rootPosition = avatar.transform.position;
            if(isCritical)
                Animator.SetTrigger("Hit_Critical");
            else
                Animator.SetTrigger("Hit");

            Action_MonsterChangeEvent(this);

            if (HP <= 0) // dead check
            {
                if(StateMachine.currStateID == "Ready")
                    StateMachine.ChangeState("Flying");
                return false; // will be destroyed;
            }
            else
                return true;
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

        public void Dispose()
        {
            HP = null; ;
            MAXHP = null;
            avatar.GetComponent<MonsterView>().Dispose();
            StateMachine = null;
            Animator = null;
            hitparticle = null;
            damagefont = null;
            Action_MonsterChangeEvent = null;
            isActive = false;
        }

    ~Monster()
        {
            sexybacklog.Console("몬스터소멸!");
        }
    }
}