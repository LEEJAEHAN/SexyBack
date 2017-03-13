using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MonsterFactory
    {
        public MonsterFactory()
        {
        }
        public Monster CreateRandomMonster(int level)
        {
            return CreateMonster(FindRandomMonsterID(level), level);
        }

        public string FindRandomMonsterID(int level)
        {
            List<MonsterData> monsterListInLevel = new List<MonsterData>();
            foreach (MonsterData mdata in Singleton<TableLoader>.getInstance().monstertable.Values)
            {
                if (mdata.MinFloor <= level && mdata.MaxFloor >= level)
                    monsterListInLevel.Add(mdata);
            }
            if (monsterListInLevel.Count <= 0)
            {
                sexybacklog.Error("no monster at this level");
                return monsterListInLevel[0].ID;
            }

            int randIndex = UnityEngine.Random.Range(0, monsterListInLevel.Count - 1);

            return monsterListInLevel[randIndex].ID;
        }

        public Monster CreateMonster(string id, int level)
        {
            MonsterData data = Singleton<TableLoader>.getInstance().monstertable[id];
            Monster monster = new Monster(id);
            monster.level = level;
            monster.Name = data.Name;
            monster.MAXHP = BigInteger.PowerByGrowth(data.baseHP, level - 1, MonsterData.GrowthRate);
            monster.HP = BigInteger.PowerByGrowth(data.baseHP, level - 1, MonsterData.GrowthRate);

            monster.avatar = InitAvatar(monster.GetID, ViewLoader.monsterbucket.transform, data.LocalPosition, out monster.CenterPosition); //data.PivotPosition
            monster.sprite = InitSprite(monster.avatar, data.SpritePath, out monster.Size);
            SetCollider(monster.avatar, monster.Size, Vector3.zero);
            monster.StateMachine = new MonsterStateMachine(monster);

            //Action_MonsterChangeEvent(this);
            monster.avatar.SetActive(false);
            return monster;
        }

        public Monster LoadMonster(string monsterdataid, int level, BigInteger hp)
        {
            MonsterData data = Singleton<TableLoader>.getInstance().monstertable[monsterdataid];
            Monster monster = new Monster(monsterdataid);
            monster.level = level;
            monster.Name = data.Name;
            monster.MAXHP = BigInteger.PowerByGrowth(data.baseHP, level - 1, MonsterData.GrowthRate);
            monster.HP = hp;

            monster.avatar = InitAvatar(monster.GetID, ViewLoader.monsterbucket.transform, data.LocalPosition, out monster.CenterPosition); //data.PivotPosition
            monster.sprite = InitSprite(monster.avatar, data.SpritePath, out monster.Size);
            SetCollider(monster.avatar, monster.Size, Vector3.zero);
            monster.StateMachine = new MonsterStateMachine(monster);

            //Action_MonsterChangeEvent(this);
            monster.avatar.SetActive(false);
            return monster;
        }


        private GameObject InitAvatar(string name, Transform parent, Vector3 localposition, out Vector3 realposition)
        {
            GameObject temp = ViewLoader.InstantiatePrefab(parent, name, "Prefabs/monster");
            temp.transform.localPosition = localposition;
            realposition = temp.transform.position; // 피봇으로 옮겨간정도 + 원래의 위치  ( 실제 위치는 옮겨놓지않았기떄문에 monsters(부모)의위치를더함 // pivot으로 몬스터위치조정은힘들어서 collider와 sprite만조정한다.
            return temp;
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