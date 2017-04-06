﻿using System;
using UnityEngine;
using System.Collections.Generic;
namespace SexyBackPlayScene
{
    public class ViewLoader // View Updater 
    { // publisher 

        public static GameObject shooter;
        public static GameObject projectiles;
        public static GameObject stagepanel;
        public static GameObject area_elemental;
        public static GameObject monsterbucket;

        // 고정 포지션을 가진 인스턴스화 되어있는 뷰를 로드해놓는다.
        // 주로 프리펩이 생길 곳들.
        // 완전히 한 system에 종속되지 않은 gameobject에 한해. 임시로

        // NGUI

        //NGUI STATIC OBJECT
        public static GameObject PopUpPanel;
        public static GameObject OptionPanel;

        public static GameObject BottomScrollView;

        public static GameObject TabButton1;
        public static GameObject TabButton2;
        public static GameObject TabButton3;
        public static GameObject TabButton4;

        public static GameObject Tab1Container;
        public static GameObject Tab2Container;
        public static GameObject Tab3Container;
        public static GameObject Tab4Container;

        public ViewLoader()
        {
            PopUpPanel = GameObject.Find("UI PopUp");
            OptionPanel = GameObject.Find("OptionWindow");

            area_elemental = GameObject.Find("elementals");
            monsterbucket = GameObject.Find("monsters");
            shooter = GameObject.Find("shooter");
            projectiles = GameObject.Find("projectiles");
            stagepanel = GameObject.Find("StagePanel");

            BottomScrollView = GameObject.Find("BottomScrollView");
            TabButton1 = GameObject.Find("TabButton1");
            TabButton2 = GameObject.Find("TabButton2");
            TabButton3 = GameObject.Find("TabButton3");
            TabButton4 = GameObject.Find("TabButton4");
            Tab1Container = GameObject.Find("Tab1Container");
            Tab2Container = GameObject.Find("Tab2Container");
            Tab3Container = GameObject.Find("Tab3Container");
            Tab4Container = GameObject.Find("Tab4Container");

            InitUISetting();
        }

        internal void InitUISetting() // remove and hide test game objects
        {
            ViewLoader.shooter.transform.DestroyChildren();
            ViewLoader.monsterbucket.transform.DestroyChildren();
            ViewLoader.Tab1Container.transform.DestroyChildren();
            ViewLoader.stagepanel.transform.DestroyChildren();

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

        internal static GameObject MakePopUp(string v, string context, Action yesfun, Action nofun)
        {
            GameObject newone = GameObject.Instantiate<GameObject>(Resources.Load("Prefabs/UI/StandardPopUp") as GameObject);
            newone.name = "YesOrNoWindow";
            newone.transform.parent = PopUpPanel.transform;
            newone.transform.localScale = Vector3.one;
            newone.transform.localPosition = Vector3.zero;

            newone.GetComponent<StandardPopUpView>().Action_Yes += yesfun;
            newone.GetComponent<StandardPopUpView>().Action_No += nofun;
            return newone;
        }

    }
}