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
        Monster CurrentMonster;



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
            CreateMonster(monsterDatas["TestMonster"]);
        }

        private void LoadData()
        {
            MonsterData dummy = new MonsterData("testmonster", "Sprites/Monster/m04", new BigInteger(1000, Digit.b));
            monsterDatas.Add("TestMonster", dummy);
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

        internal void onMonsterChange(Monster monster)
        {
            string hp = monster.HP.ToSexyBackString();
            string maxhp = monster.MAXHP.ToSexyBackString();

            label_monsterhp.text = hp + " / " + maxhp;
        }

        private void onHit(string monsterID, Projectile projectile) // 어차피 한마리라 일단 id는 무시
        {
            CurrentMonster.OnHit(projectile.Damage);
            CurrentMonster.avatar.GetComponent<Animator>().SetTrigger("Hit");

            hitparticle.transform.position = projectile.gameObject.transform.position;
            hitparticle.GetComponent<ParticleSystem>().Play();
            noticeMonsterChange(CurrentMonster);
        }
        public void onHitByHero(string monsterID, BigInteger damage)
        {
            CurrentMonster.OnHit(damage);
            CurrentMonster.avatar.GetComponent<Animator>().SetTrigger("Hit");
            
            noticeMonsterChange(CurrentMonster);
        }

        private void onElementalCreate(Elemental sender)
        {
            sender.target = CurrentMonster;
        }
        private void onHeroCreate(Hero hero)
        {
            hero.targetID = CurrentMonster.ID;
        }
    }
}