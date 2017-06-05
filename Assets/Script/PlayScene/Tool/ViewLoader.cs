using System;
using UnityEngine;
using System.Collections.Generic;
namespace SexyBackPlayScene
{
    public class ViewLoader // View Updater 
    { // publisher 

        // 고정 포지션을 가진 인스턴스화 되어있는 뷰를 로드해놓는다.
        // 완전히 한 system에 종속되지 않은 gameobject에 한해. 임시로
        public static GameObject shooter;
        public static GameObject projectiles;
        public static GameObject stagepanel;
        public static GameObject area_elemental;
        public static GameObject monsterbucket;
        public static GameObject objectarea;


        // NGUI

        //NGUI STATIC OBJECT
        public static GameObject PopUpPanel;
        public static GameObject OptionPanel;
        

        public ViewLoader()
        {
            PopUpPanel = GameObject.Find("UI PopUp");
            OptionPanel = GameObject.Find("OptionWindow");
            objectarea = GameObject.Find("Objects");
            area_elemental = GameObject.Find("elementals");
            monsterbucket = GameObject.Find("monsters");
            shooter = GameObject.Find("shooter");
            projectiles = GameObject.Find("projectiles");
            stagepanel = GameObject.Find("StagePanel");
            
            InitUISetting();
        }

        internal void InitUISetting() // remove and hide test game objects
        {
            ViewLoader.shooter.transform.DestroyChildren();
            ViewLoader.monsterbucket.transform.DestroyChildren();
            
            ViewLoader.stagepanel.transform.DestroyChildren();
            ViewLoader.objectarea.transform.DestroyChildren();

            PopUpPanel.transform.position = GameObject.Find("UI Root").transform.position;
            PopUpPanel.SetActive(false);
        }

        public static GameObject InstantiatePrefab(Transform parent, string objectname, string prefabpath)
        {
            GameObject newone = GameObject.Instantiate<GameObject>(Resources.Load(prefabpath) as GameObject);
            newone.name = objectname;
            newone.transform.parent = parent;
            newone.transform.localScale = parent.transform.localScale;
            newone.transform.localPosition = Vector3.zero;
            return newone;
        }

        internal static GameObject MakePopUp(string title, string text, Action yesfun, Action nofun)
        {
            GameObject newone = GameObject.Instantiate<GameObject>(Resources.Load("Prefabs/UI/StandardPopUp") as GameObject);
            newone.name = "YesOrNoWindow";
            newone.transform.parent = PopUpPanel.transform;
            newone.transform.localScale = Vector3.one;
            newone.transform.localPosition = Vector3.zero;
            newone.transform.FindChild("Title").GetComponent<UILabel>().text = title;
            newone.transform.FindChild("Text").GetComponent<UILabel>().text = text;

            newone.GetComponent<StandardPopUpView>().Action_Yes += yesfun;
            newone.GetComponent<StandardPopUpView>().Action_No += nofun;
            return newone;
        }

    }
}
