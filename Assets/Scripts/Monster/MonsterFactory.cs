using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MonsterFactory
    {
        int TotalProductCount = 0;

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
            TotalProductCount++;
            string InstanceID = TotalProductCount + "/" + data.ID + "/" + level.ToString();
            Monster monster = new Monster(InstanceID);

            monster.Name = data.Name;
            monster.level = level;
            monster.MAXHP = BigInteger.PowerByGrowth(data.baseHP, level - 1, data.GrowthRate);
            monster.HP = BigInteger.PowerByGrowth(data.baseHP, level - 1, data.GrowthRate);

            monster.avatar = InitAvatar(data.ID, ViewLoader.monsterbucket.transform, data.LocalPosition, out monster.CenterPosition); //data.PivotPosition
            monster.sprite = InitSprite(monster.avatar, data.SpritePath, out monster.Size);
            SetCollider(monster.avatar, monster.Size, Vector3.zero);
            monster.StateMachine = new MonsterStateMachine(monster);

            //Action_MonsterChangeEvent(this);
            monster.avatar.SetActive(false);
            return monster;
        }

        private GameObject InitAvatar(string name, Transform parent, Vector3 localposition, out Vector3 realposition)
        {
            GameObject temp = GameObject.Instantiate<GameObject>(Resources.Load("Prefabs/monster") as GameObject);
            temp.name = name;
            temp.transform.parent = parent; // genposition
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


        //private List<E> ShuffleList<E>(List<E> inputList)
        //{
        //    List<E> randomList = new List<E>();
        //    int randomIndex = 0;
        //    while (inputList.Count > 0)
        //    {
        //        randomIndex = UnityEngine.Random.Range(0, inputList.Count); //Choose a random object in the list
        //        randomList.Add(inputList[randomIndex]); //add it to the new, random list
        //        inputList.RemoveAt(randomIndex); //remove to avoid duplicates
        //    }
        //    return randomList; //return the new random list
        //}

    }
}