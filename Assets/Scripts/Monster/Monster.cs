using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class Monster // model
    {
        public string ID;

        public BigInteger HP;
        public BigInteger MAXHP;

        private GameObject avatar;
        public BoxCollider avatarCollision { get { return avatar.GetComponent<BoxCollider>(); } }

        public Vector3 PivotPosition; // 올라가는정도
        public Vector3 CenterPosition; // its default; readonly.
        public Vector3 Size;

        GameObject hitparticle = ViewLoader.hitparticle;

        public MonsterView.MonsterHit_Event SetHitEvent
        {
            set { avatar.GetComponent<MonsterView>().noticeHit += value; }
        }

        internal Monster (MonsterData data)
        {
            ID = data.ID;
            MAXHP = data.MaxHP;
            HP = data.MaxHP;


            // init avater
            avatar = GameObject.Instantiate<GameObject>(Resources.Load("Prefabs/monster")as GameObject);
            avatar.name = ID;
            avatar.transform.parent = ViewLoader.monsters.transform; // genposition
            avatar.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(data.SpritePath);

            // set position
            Size = avatar.GetComponent<SpriteRenderer>().sprite.bounds.size;
            PivotPosition = avatar.GetComponent<SpriteRenderer>().sprite.bounds.center; // sprite의 center는 피봇이다
            CenterPosition = PivotPosition + GameSetting.defaultMonsterPosition; // 피봇으로 옮겨간정도 + 원래의 위치  ( 실제 위치는 옮겨놓지않았기떄문에 monsters(부모)의위치를더함 )
            // TODO : 아직 실시간 변화를 반영하지못하는구조.

            // pivot으로 몬스터위치조정은힘들어서 collider와 sprite만조정한다.
            avatar.GetComponent<BoxCollider>().size = Size;
            avatar.GetComponent<BoxCollider>().center = PivotPosition;
            // Debug.Log(this.GetComponent<SpriteRenderer>().sprite.bounds);
        }

        public void Update()
        {
        }

        internal void Hit(Vector3 hitPosition, BigInteger damage, bool isCritical)
        {
            HP -= damage;
            Singleton<GameMoney>.getInstance().ExpGain(damage);

            avatar.GetComponent<Animator>().rootPosition = avatar.transform.position;
            if(isCritical)
                avatar.GetComponent<Animator>().SetTrigger("Hit_Critical");
            else
                avatar.GetComponent<Animator>().SetTrigger("Hit");

            //particle
            hitparticle.transform.position = hitPosition;
            hitparticle.GetComponent<ParticleSystem>().Play();
            //damagefont
            Vector3 screenpos = ViewLoader.HeroCamera.WorldToScreenPoint(hitPosition);
            sexybacklog.Console(screenpos);
            GameObject.Find("label_dmgfont").transform.localPosition = screenpos;
            GameObject.Find("label_dmgfont").GetComponent<UILabel>().text = damage.ToSexyBackString();


        }

    }
}