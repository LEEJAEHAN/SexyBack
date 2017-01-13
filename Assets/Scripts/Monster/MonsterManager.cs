using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    // 몬스터와 관련된 입력을 처리
    public class MonsterManager
    {
        // 싱글턴 코드 완료

        SexyBackMonster Monster;
        UILabel label_monsterhp = ViewLoader.label_monsterhp.GetComponent<UILabel>();

        internal void Init()
        {
            Monster = new SexyBackMonster(new BigInteger(1000, Digit.b));
        }

        internal void Update()
        {
            Monster.Update();
        }

        internal SexyBackMonster GetMonster()
        {
            return Monster;
        }

        public void OnHit(Collider collider)
        {

        }

        internal void UpdateHp(SexyBackMonster monster)
        {
            string hp = monster.HP.ToSexyBackString();
            string maxhp = monster.MAXHP.ToSexyBackString();

            label_monsterhp.text = hp + " / " + maxhp;
        }
    }
}