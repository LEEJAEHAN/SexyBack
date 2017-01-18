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

        public Vector3 Position;
        public Vector3 Size;

        internal Monster (MonsterData data)
        {
            ID = data.ID;
            MAXHP = data.MaxHP;
            HP = data.MaxHP;

            avatar = GameObject.Instantiate<GameObject>(Resources.Load("Prefabs/monster")as GameObject);
            avatar.name = ID;
            avatar.transform.parent = ViewLoader.monsters.transform; // genposition
            avatar.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(data.SpritePath);

            Size = avatar.GetComponent<SpriteRenderer>().sprite.bounds.size;
            Position = avatar.GetComponent<SpriteRenderer>().sprite.bounds.center;
            // pivot으로 몬스터위치조정은힘들어서 collider와 sprite만조정한다.
            avatar.GetComponent<BoxCollider>().size = Size;
            avatar.GetComponent<BoxCollider>().center = Position;
            // Debug.Log(this.GetComponent<SpriteRenderer>().sprite.bounds);
        }

        internal void Hit(BigInteger damage)
        {
            HP -= damage;
            Singleton<HeroManager>.getInstance().GainExp(damage);
        }

        public void Update()
        {
        }
    }
}