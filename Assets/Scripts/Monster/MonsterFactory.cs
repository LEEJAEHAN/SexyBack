using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MonsterFactory
    {


        public MonsterFactory()
        {


        }

        public Monster CreateMonster(string id)
        {
            MonsterData data = Singleton<TableLoader>.getInstance().monstertable[id];
            Monster monster = new Monster();

            monster.ID = data.ID;
            monster.MAXHP = data.MaxHP;
            monster.HP = data.MaxHP;
            monster.Name = data.Name;

            monster.avatar = InitAvatar(data.ID, ViewLoader.monsters.transform, data.LocalPosition, out monster.CenterPosition); //data.PivotPosition
            monster.sprite = InitSprite(monster.avatar, data.SpritePath, out monster.Size);
            SetCollider(monster.avatar, monster.Size, Vector3.zero);
            monster.StateMachine = new MonsterStateMachine(monster);

            //Action_MonsterChangeEvent(this);
            monster.avatar.SetActive(false);

            return monster;
        }

        private GameObject InitAvatar(string name, Transform parent, Vector3 localposition, out Vector3 realposition)
        {
            GameObject mob = GameObject.Instantiate<GameObject>(Resources.Load("Prefabs/monster") as GameObject);
            mob.name = name;
            mob.transform.parent = parent; // genposition
            mob.transform.localPosition = localposition;
            realposition = mob.transform.position; // 피봇으로 옮겨간정도 + 원래의 위치  ( 실제 위치는 옮겨놓지않았기떄문에 monsters(부모)의위치를더함 // pivot으로 몬스터위치조정은힘들어서 collider와 sprite만조정한다.
            return mob;
        }
        private void SetCollider(GameObject mob, Vector3 size, Vector3 center)
        {
            mob.GetComponent<BoxCollider>().size = size;
            mob.GetComponent<BoxCollider>().center = center;
        }

        private GameObject InitSprite(GameObject monster, string spritepath, out Vector3 size)
        {
            GameObject sprobj = monster.transform.GetChild(0).gameObject;
            sprobj.transform.localPosition = Vector3.zero;
            sprobj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(spritepath);
            size = sprobj.GetComponent<SpriteRenderer>().sprite.bounds.size;
            return sprobj;
        }



    }
}