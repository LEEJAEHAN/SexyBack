using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class Monster : StateOwner// model
    {
        public string ID;
        public string Name;
        public BigInteger HP;
        public BigInteger MAXHP;

        public GameObject avatar;
        public BoxCollider avatarCollision { get { return avatar.GetComponent<BoxCollider>(); } }

        public StateMachine<Monster> StateMachine;
        public Animator Animator;
        //size value
        public Vector3 PivotPosition; // 올라가는정도
        public Vector3 CenterPosition; // its default; readonly.

        public Vector3 Size;

        GameObject hitparticle = ViewLoader.hitparticle;
        GameObject damagefont = ViewLoader.DamageFont;

        public delegate void monsterChangeEvent_Handler(Monster sender);
        public event monsterChangeEvent_Handler noticeMonsterChange;


        public MonsterStateMachine.StateChangeHandler Action_changeEvent
        { set { avatar.GetComponent<MonsterView>().Action_changeEvent += value;
                StateMachine.Action_changeEvent += value; } }

        string StateOwner.ID {get{ return ID; }}
        public string CurrentState { get { return StateMachine.currStateID; } }

        internal Monster (MonsterData data)
        {
            ID = data.ID;
            MAXHP = data.MaxHP;
            HP = data.MaxHP;
            Name = data.Name;

            avatar = InitAvatar(data.SpritePath, ViewLoader.monsters.transform);
            RecordSize(avatar);
            InitDamageFont();

            Animator = avatar.GetComponent<Animator>();
            StateMachine = new MonsterStateMachine(this);

            noticeMonsterChange(this);
        }
        private void RecordSize(GameObject mob)
        {
            Size = mob.GetComponent<SpriteRenderer>().sprite.bounds.size;
            PivotPosition = mob.GetComponent<SpriteRenderer>().sprite.bounds.center; // sprite의 center는 피봇이다
            CenterPosition = PivotPosition + GameSetting.defaultMonsterPosition; // 피봇으로 옮겨간정도 + 원래의 위치  ( 실제 위치는 옮겨놓지않았기떄문에 monsters(부모)의위치를더함 )                                                                     // pivot으로 몬스터위치조정은힘들어서 collider와 sprite만조정한다.
            mob.GetComponent<BoxCollider>().size = Size;
            mob.GetComponent<BoxCollider>().center = PivotPosition;
        }

        private GameObject InitAvatar(string spritepath, Transform genPosition)
        {   // TODO : 아직 실시간 변화를 반영하지못하는구조.
            GameObject mob = GameObject.Instantiate<GameObject>(Resources.Load("Prefabs/monster") as GameObject);
            mob.name = ID;
            mob.transform.parent = genPosition; // genposition
            mob.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(spritepath);
            return mob;
        }
        public void Update()
        {
            StateMachine.Update();
        }


        internal bool Hit(Vector3 hitPosition, BigInteger damage, bool isCritical)
        {
            HP -= damage;
            Singleton<GameMoney>.getInstance().ExpGain(damage);

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

            noticeMonsterChange(this);

            if (HP <= 0) // dead check
            {
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

        private void InitDamageFont()
        {
            damagefont.GetComponent<UITweener>().AddOnFinished(avatar.GetComponent<MonsterView>().OnDamageFontFinish);
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
    }
}