using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class SexyBackMonster
    {
        public BigInteger HP;
        public BigInteger MAXHP;

        GameObject hitparticle;
        GameObject avatar;

        public SexyBackMonster (BigInteger maxhp)
        {
            MAXHP = maxhp;
            HP = maxhp;
            hitparticle = GameObject.Find("hitparticle") as GameObject;
            avatar = GameObject.Find("monster");
            avatar.GetComponent<MonsterAvatar>().monster = this;
        }
        
        // mvc 어기고있음
        public void Hit(BigInteger damage, bool HitMotion)
        {
            HP -= damage;
            GameManager.getInstance().GainExp(damage);
            UIUpdater.getInstance().noticeHPChanged();
            if(HitMotion)
            {
//                GameManager.SexyBackLog("hitmotion");
                avatar.GetComponent<Animator>().SetTrigger("Hit");
                hitparticle.transform.position = new Vector3(avatar.transform.position.x + UnityEngine.Random.Range(-1f,1f), avatar.transform.position.y + UnityEngine.Random.Range(-1f, 1f),0);
                hitparticle.GetComponent<ParticleSystem>().Play();
            }


        }



    }
}