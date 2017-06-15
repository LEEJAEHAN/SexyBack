using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MonsterFactory
    {
        //double GrowthRatePerFloor;

        //readonly int[] HpType;
        //readonly int LevelPerFloor;
        //public readonly int MonsterPerStage;
        //public readonly int BossTerm;
        public MapMonsterData info;

        public MonsterFactory()
        {
            info = Singleton<InstanceStatus>.getInstance().InstanceMap.baseData.MapMonster;
        }
        public Monster CreateRandomMonster(string ID, int floor, int type)
        {
            return CreateMonster(ID, FindRandomMonsterDataID(floor), floor, type);
        }

        public string FindRandomMonsterDataID(int level)
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

        internal int CreateMonsters(int floor, Queue<Monster> queue)
        {
            int count = 0;
            bool isbossStage = floor % info.BossTerm == 0;
            if (Singleton<PlayerStatus>.getInstance().GetGlobalStat.FastStage >= floor)
            {
                if (isbossStage)
                {
                    string id = string.Format("F{0}M{1}", floor, count);
                    Monster newmonster = CreateRandomMonster(id, floor, 2);
                    queue.Enqueue(newmonster);
                    count++;
                }
            }
            else
            {
                for (int i = 0; i < info.MonsterPerStage; i++)
                {
                    string id = string.Format("F{0}M{1}", floor, i);
                    Monster newmonster = CreateRandomMonster(id, floor, isbossStage ? 1 : 0);
                    queue.Enqueue(newmonster);
                    count++;
                }
            }
            return count;
        }

        public Monster CreateMonster(string instanceID, string dataID, int floor, int type)
        {
            //TODO : 보스따로구분해야함.
            // floor - 1; // 1층이면 1레벨 몬스터, 10층이면 10레벨(2의10승) 몬스터, 
            MonsterData data = Singleton<TableLoader>.getInstance().monstertable[dataID];
            Monster monster = new Monster(instanceID, dataID);
            monster.level = floor * info.LevelPerFloor;
            monster.Name = data.Name;
            monster.type = type;
            
            double growth = InstanceStatus.CalInstanceGrowth(monster.level); // 2, level
            monster.MAXHP = BigInteger.FromDouble(growth * info.HP[type]);
            monster.HP = BigInteger.FromDouble(growth * info.HP[type]);
            monster.chestCount = info.Chest[type] * Singleton<PlayerStatus>.getInstance().GetUtilStat.ConsumableX;
            monster.avatar = InitAvatar(monster.GetID, ViewLoader.monsterbucket.transform, data.LocalPosition, out monster.CenterPosition); //data.PivotPosition
            monster.sprite = InitSprite(monster.avatar, data.SpritePath, out monster.Size);
            
            SetCollider(monster.avatar, monster.Size, Vector3.zero);
            monster.StateMachine = new MonsterStateMachine(monster);

            //Action_MonsterChangeEvent(this);
            monster.avatar.SetActive(false);
            return monster;
        }


        private Monster LoadMonster(string instanceID, string dataID, int level, BigInteger hp, int type)
        {
            MonsterData data = Singleton<TableLoader>.getInstance().monstertable[dataID];
            Monster monster = new Monster(instanceID, dataID);
            monster.level = level;
            monster.Name = data.Name;
            double growth = InstanceStatus.CalInstanceGrowth(monster.level);
            monster.type = type;
            monster.MAXHP = BigInteger.FromDouble(growth * info.HP[type]);
            monster.HP = hp;
            monster.chestCount = info.Chest[type] * Singleton<PlayerStatus>.getInstance().GetUtilStat.ConsumableX;

            monster.avatar = InitAvatar(monster.GetID, ViewLoader.monsterbucket.transform, data.LocalPosition, out monster.CenterPosition); //data.PivotPosition
            monster.sprite = InitSprite(monster.avatar, data.SpritePath, out monster.Size);
            SetCollider(monster.avatar, monster.Size, Vector3.zero);
            monster.StateMachine = new MonsterStateMachine(monster);

            //Action_MonsterChangeEvent(this);
            monster.avatar.SetActive(false);
            return monster;
        }

        internal Monster LoadMonster(XmlNode node)
        {
            string dataID = node.Attributes["dataid"].Value;
            string id = node.Attributes["id"].Value;
            int level = int.Parse(node.Attributes["level"].Value);
            int type = int.Parse(node.Attributes["type"].Value);
            BigInteger hp = new BigInteger(node.Attributes["hp"].Value);

            if (hp <= 0)
                return null;
            else
                return LoadMonster(id, dataID, level, hp, type);
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