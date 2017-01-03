using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class UIUpdater
    {
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
    }
}