﻿using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class SexyBackMonster
    {
        public double HP;
        public double MAXHP;

        GameObject hitparticle;
        GameObject avatar;

        public SexyBackMonster (double maxhp)
        {
            MAXHP = maxhp;
            HP = maxhp;
            UIUpdater.getInstance().noticeHPChanged(HP, MAXHP);
            hitparticle = GameObject.Find("hitparticle") as GameObject;
            avatar = GameObject.Find("monster");
        }
        public void Hit(double damage, bool HitMotion)
        {
            HP -= damage;
            UIUpdater.getInstance().noticeHPChanged(HP,MAXHP);
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