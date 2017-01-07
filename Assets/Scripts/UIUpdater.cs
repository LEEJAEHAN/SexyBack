using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class UIUpdater // View Updater 
    { // publisher 
        private static UIUpdater Instance;
        GameManager DataModel;
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
        public void Init(GameManager model)
        {
            DataModel = model;
        }


        internal void noticeDamageChanged()
        {
            string dpsString = "DPS : " + DataModel.GetTotalDPS().ToSexyBackString()+ "\n" + "DPC : " + DataModel.GetTotalDPC().ToSexyBackString();
            GameObject.Find("label_herodmg").GetComponent<UILabel>().text = dpsString;
        }
        internal void noteiceExpChanged()
        {
            string expstring = DataModel.EXP.ToSexyBackString() + " EXP";
            GameObject.Find("label_exp").GetComponent<UILabel>().text = expstring;
        }
        internal void noticeHPChanged()
        {
            string hp = DataModel.GetMonster().HP.ToSexyBackString();
            string maxhp = DataModel.GetMonster().MAXHP.ToSexyBackString();

            GameObject.Find("label_monsterhp").GetComponent<UILabel>().text = hp + " / " + maxhp;
        }
    }
}