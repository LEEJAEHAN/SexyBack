using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class UIUpdater
    { // publisher 
        private static UIUpdater Instance;

        private UIUpdater()
        {

        }

        public static UIUpdater getInstance()
        {
            if (Instance == null)
                Instance = new UIUpdater();
            return Instance;
        }

        // Use this for initialization
        public void Init()
        {
        }


        internal void noticeDamageChanged(double DPC, double DPS)
        {
            string dpsString = "DPS : " + GameManager.SexyBackToInt(DPS) + "\n" + "DPC : " + GameManager.SexyBackToInt(DPC);
            GameObject.Find("label_herodmg").GetComponent<UILabel>().text = dpsString;

        }

        internal void noteiceExpChanged(double EXP)
        {
            string expstring = GameManager.SexyBackToInt(EXP) + " EXP";
            GameObject.Find("label_exp").GetComponent<UILabel>().text = expstring;
        }

        internal void noticeHPChanged(double HP, double MAXHP)
        {
            string dpsString = GameManager.SexyBackToInt(HP) + " / " + GameManager.SexyBackToInt(MAXHP);
            // hp bar
            GameObject.Find("label_monsterhp").GetComponent<UILabel>().text = dpsString;
        }


    }
}