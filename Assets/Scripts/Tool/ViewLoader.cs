using System;
using UnityEngine;
using System.Collections.Generic;
namespace SexyBackPlayScene
{
    public class ViewLoader // View Updater 
    { // publisher 

        public static GameObject HeroPanel;
        public static Camera HeroCamera;
        public static Camera EffectCamera;

        // 인스턴스화 되어있는 뷰를 로드해놓는다.
        // 완전히 한 system에 종속되지 않은 gameobject에 한해. 임시로
        public static GameObject StagePanel;
        public static GameObject label_debug;
        public static GameObject projectiles;
        public static GameObject hero_sprite;
        public static GameObject area_elemental;
        public static GameObject shooter;
        public static GameObject monsterbucket;

        // NGUI
        public static GameObject BottomScrollView;

        public static GameObject TabButton1;
        public static GameObject TabButton2;
        public static GameObject TabButton3;
        public static GameObject TabButton4;

        public static GameObject Tab1Container;
        public static GameObject Tab2Container;
        public static GameObject Tab3Container;
        public static GameObject Tab4Container;


        public static GameObject Tab1Info;

        public static GameObject Bar_Attack;

        public static GameObject Reward_PopUp;


        public ViewLoader()
        {
            HeroPanel = GameObject.Find("HeroPanel");
            StagePanel = GameObject.Find("StagePanel");

            HeroCamera = GameObject.Find("HeroCamera").GetComponent<Camera>();
            EffectCamera = GameObject.Find("EffectCamera").GetComponent<Camera>();
            label_debug = GameObject.Find("label_debug");
            monsterbucket = GameObject.Find("monsters");

            projectiles = GameObject.Find("projectiles");

            hero_sprite = GameObject.Find("hero_sprite");
            area_elemental = GameObject.Find("area_elemental");
            shooter = GameObject.Find("shooter");

            BottomScrollView = GameObject.Find("BottomScrollView");
            TabButton1 = GameObject.Find("TabButton1");
            TabButton2 = GameObject.Find("TabButton2");
            TabButton3 = GameObject.Find("TabButton3");
            TabButton4 = GameObject.Find("TabButton4");
            Tab1Container = GameObject.Find("Tab1Container");
            Tab2Container = GameObject.Find("Tab2Container");
            Tab3Container = GameObject.Find("Tab3Container");
            Tab4Container = GameObject.Find("Tab4Container");
            Tab1Info = GameObject.Find("Tab1Info");

            Bar_Attack = GameObject.Find("Bar_Attack");

            Reward_PopUp = GameObject.Find("Reward_PopUp");
        }
        
        public static GameObject InstantiatePrefab(Transform parent, string objectname, string prefabpath )
        {
            GameObject newone = GameObject.Instantiate<GameObject>(Resources.Load(prefabpath) as GameObject);
            newone.name = objectname;
            newone.transform.parent = parent;
            newone.transform.localScale = parent.transform.localScale;
            newone.transform.localPosition = Vector3.zero;
            return newone;
        }
    }
}