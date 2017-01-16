using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class Monster // model
    {
        public string ID;

        public BigInteger HP;
        public BigInteger MAXHP;

        public GameObject avatar;
        public BoxCollider avatarCollision { get { return avatar.GetComponent<BoxCollider>(); } }

        internal Monster (MonsterData data)
        {
            ID = data.ID;
            MAXHP = data.MaxHP;
            HP = data.MaxHP;

            // 차후 프리펩에서 생성코드로바꾼다.
            avatar = GameObject.Instantiate<GameObject>(Resources.Load("Prefabs/monster")as GameObject);
            avatar.name = ID;
            avatar.transform.parent = ViewLoader.RenderObject.transform;
            avatar.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(data.SpritePath);
            avatar.GetComponent<BoxCollider>().size = avatar.GetComponent<SpriteRenderer>().sprite.bounds.size;
            // Debug.Log(this.GetComponent<SpriteRenderer>().sprite.bounds);

        }

        internal void OnHit(BigInteger damage)
        {
            HP -= damage;
            Singleton<HeroManager>.getInstance().GainExp(damage);
        }

        public void Update()
        {
        }
    }
}