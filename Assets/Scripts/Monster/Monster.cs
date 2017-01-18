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

        public Vector3 PivotPosition; // 올라가는정도
        public Vector3 CenterPosition; // 
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
            PivotPosition = avatar.GetComponent<SpriteRenderer>().sprite.bounds.center; // sprite의 center는 피봇이다
            CenterPosition = PivotPosition + GameSetting.defaultMonsterPosition; // 피봇으로 옮겨간정도 + 원래의 위치  ( 실제 위치는 옮겨놓지않았기떄문에 monsters(부모)의위치를더함 )

            // pivot으로 몬스터위치조정은힘들어서 collider와 sprite만조정한다.
            avatar.GetComponent<BoxCollider>().size = Size;
            avatar.GetComponent<BoxCollider>().center = PivotPosition;
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