using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class SexyBackMonster
    {
        public double HP;
        public double MAXHP;

        public SexyBackMonster (double maxhp)
        {
            MAXHP = maxhp;
            HP = maxhp;
            noticeHPChanged();
        }
        public void Hit(double damage, bool HitMotion)
        {
            HP -= damage;
            noticeHPChanged();
            if(HitMotion)
            {
                GameManager.SexyBackLog("hitmotion");
                GameObject.Find("monster").GetComponent<Animator>().SetTrigger("Hit");
            }
        }

        void noticeHPChanged()
        {
            string dpsString = GameManager.SexyBackToInt(HP) + " / " + GameManager.SexyBackToInt(MAXHP);
            // hp bar
            GameObject.Find("label_monsterhp").GetComponent<UILabel>().text = dpsString;
        }


    }
}