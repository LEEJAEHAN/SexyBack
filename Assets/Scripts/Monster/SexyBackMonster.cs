using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class SexyBackMonster // model
    {
        public BigInteger HP;
        public BigInteger MAXHP;

        public GameObject avatar;
        GameObject hitparticle;

        public Vector3 Position
        {
            get { return avatar.transform.position;}
            private set { }
        }

        internal SexyBackMonster (BigInteger maxhp)
        {
            MAXHP = maxhp;
            HP = maxhp;

            // 차후 프리펩에서 생성코드로바꾼다.
            avatar = ViewLoader.monster;

            avatar.GetComponent<MonsterBehaviour>().monster = this;
            hitparticle = ViewLoader.hitparticle;
        }

        internal void OnHit(BigInteger damage)
        {
            HP -= damage;
            Singleton<HeroManager>.getInstance().GainExp(damage);

            avatar.GetComponent<Animator>().SetTrigger("Hit");
            hitparticle.transform.position = new Vector3(avatar.transform.position.x + UnityEngine.Random.Range(-1f, 1f), avatar.transform.position.y + UnityEngine.Random.Range(-1f, 1f), 0);
            hitparticle.GetComponent<ParticleSystem>().Play();

            // 업데이트를 manager 의 업데이트주기에맞춰서할꺼냐, 이벤트시 갱신형식으로할꺼냐
            Singleton<MonsterManager>.getInstance().UpdateHp(this);
        }


        public void Update()
        {
        }
    }
}