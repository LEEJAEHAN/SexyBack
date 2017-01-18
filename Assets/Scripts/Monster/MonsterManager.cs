using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    // 몬스터와 관련된 입력을 처리
    public class MonsterManager
    {
        public delegate void monsterCreateEvent_Handler(Monster sender);
        public event monsterCreateEvent_Handler noticeMonsterCreate;

        public delegate void monsterChangeEvent_Handler(Monster sender);
        public event monsterChangeEvent_Handler noticeMonsterChange;

        GameObject hitparticle = ViewLoader.hitparticle;

        Dictionary<string, MonsterData> monsterDatas = new Dictionary<string, MonsterData>();
        Monster CurrentMonster; // TODO: bucket으로수정해야함;

        UILabel label_monsterhp = ViewLoader.label_monsterhp.GetComponent<UILabel>();

        internal void Init()
        {
            LoadData();

            noticeMonsterChange += onMonsterChange;
            Singleton<ElementalManager>.getInstance().onElementalCreate += onElementalCreate;
            Singleton<HeroManager>.getInstance().noticeHeroCreate += onHeroCreate;
        }

        public void Start()
        {
            CreateMonster(monsterDatas["m06"]);
        }

        private void LoadData()
        {
            monsterDatas.Add("m01", new MonsterData("m01", "Sprites/Monster/m01", new BigInteger(1000, Digit.b)));
            monsterDatas.Add("m02", new MonsterData("m02", "Sprites/Monster/m02", new BigInteger(1000, Digit.b)));
            monsterDatas.Add("m03", new MonsterData("m03", "Sprites/Monster/m03", new BigInteger(1000, Digit.b)));
            monsterDatas.Add("m04", new MonsterData("m04", "Sprites/Monster/m04", new BigInteger(1000, Digit.b)));
            monsterDatas.Add("m05", new MonsterData("m05", "Sprites/Monster/m05", new BigInteger(1000, Digit.b)));
            monsterDatas.Add("m06", new MonsterData("m06", "Sprites/Monster/m06", new BigInteger(1000, Digit.b)));
            monsterDatas.Add("m07", new MonsterData("m07", "Sprites/Monster/m07", new BigInteger(1000, Digit.b)));
            monsterDatas.Add("m08", new MonsterData("m08", "Sprites/Monster/m08", new BigInteger(1000, Digit.b)));
            monsterDatas.Add("m09", new MonsterData("m09", "Sprites/Monster/m09", new BigInteger(1000, Digit.b)));
            monsterDatas.Add("m10", new MonsterData("m10", "Sprites/Monster/m10", new BigInteger(1000, Digit.b)));
        }

        private void CreateMonster(MonsterData data)
        {
            CurrentMonster = new Monster(data);
            // this class is event listner
            CurrentMonster.avatar.GetComponent<MonsterView>().noticeHit += onHit;

            noticeMonsterCreate(CurrentMonster);
        }

        internal void Update()
        {
            CurrentMonster.Update();
        }

        internal Monster GetMonster()
        {
            return CurrentMonster;
        }
        internal Monster GetMonster(string id)
        {
            return CurrentMonster; //TODO: 바꿔야함
        }

        internal void onMonsterChange(Monster monster)
        {
            string hp = monster.HP.ToSexyBackString();
            string maxhp = monster.MAXHP.ToSexyBackString();

            label_monsterhp.text = hp + " / " + maxhp;
        }

        public void onHit(string monsterID, BigInteger damage, Vector3 hitPosition) // 어차피 한마리라 일단 id는 무시
        {
            CurrentMonster.Hit(damage);

            CurrentMonster.avatar.GetComponent<Animator>().rootPosition = CurrentMonster.avatar.transform.position;
            CurrentMonster.avatar.GetComponent<Animator>().SetTrigger("Hit");
          
            hitparticle.transform.position = hitPosition;
            hitparticle.GetComponent<ParticleSystem>().Play();
            noticeMonsterChange(CurrentMonster);
        }

        private void onElementalCreate(Elemental sender)
        {
            sender.target = CurrentMonster;
        }
        private void onHeroCreate(Hero hero)
        {
            hero.targetID = CurrentMonster.ID;
            //hero.SetDirection(CurrentMonster.CenterPosition);
        }
    }
}