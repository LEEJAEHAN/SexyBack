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
        // 모든 인스턴스화 되어있는 뷰를 로드해놓는다.

        public static GameObject StagePanel;
        // 동적으로 생성되는 것은 ㄴㄴ
        public static GameObject label_debug;
        public static GameObject label_herodmg;
        public static GameObject label_elementaldmg;

        public static GameObject label_minusdps;
        public static GameObject label_exp;

        public static GameObject projectiles;
        public static GameObject hitparticle;
        public static GameObject Effect_Sword;
        public static GameObject hero_sprite;
        public static GameObject area_elemental;
        public static GameObject shooter;

        public static GameObject monsterbucket;

        // NGUI
        public static GameObject TabButton1;
        public static GameObject TabButton2;
        public static GameObject TabButton3;
        public static GameObject TabButton4;

        public static GameObject Tab1Container;
        public static GameObject Tab2Container;
        public static GameObject Tab3Container;
        public static GameObject Tab4Container;


        public static GameObject Info_Context;
        public static GameObject DamageFont;
        public static GameObject Button_Confirm;
        public static GameObject Button_Pause;
        public static GameObject Bar_Attack;
        // info ui
        public static GameObject Info_Icon;
        public static GameObject Info_Description;

        // HPBAR
        public static GameObject HPBar_Fill1;
        public static GameObject HPBar_Fill2;
        public static GameObject HPBar;
        public static GameObject HPBar_SlowFill1;
        public static GameObject HPBar_SlowFill2;
        public static GameObject HPBar_Name;
        public static GameObject HPBar_Unit;
        public static GameObject HPBar_Count;

        public ViewLoader()
        {
            HeroPanel = GameObject.Find("HeroPanel");
            StagePanel = GameObject.Find("StagePanel");

            HeroCamera = GameObject.Find("HeroCamera").GetComponent<Camera>();
            EffectCamera = GameObject.Find("EffectCamera").GetComponent<Camera>();
            label_debug = GameObject.Find("label_debug");
            label_herodmg = GameObject.Find("label_herodmg");
            label_elementaldmg = GameObject.Find("label_elementaldmg");

            label_exp = GameObject.Find("label_exp");
            label_minusdps = GameObject.Find("label_minusdps");
            monsterbucket = GameObject.Find("monsters");

            projectiles = GameObject.Find("projectiles");
            hitparticle = GameObject.Find("hitparticle");

            Effect_Sword = GameObject.Find("Effect_Sword");
            hero_sprite = GameObject.Find("hero_sprite");
            area_elemental = GameObject.Find("area_elemental");
            shooter = GameObject.Find("shooter");

            TabButton1 = GameObject.Find("TabButton1");
            TabButton2 = GameObject.Find("TabButton2");
            TabButton3 = GameObject.Find("TabButton3");
            TabButton4 = GameObject.Find("TabButton4");


            Tab1Container = GameObject.Find("Tab1Container");
            Tab2Container = GameObject.Find("Tab2Container");
            Tab3Container = GameObject.Find("Tab3Container");
            Tab4Container = GameObject.Find("Tab4Container");


            Info_Context = GameObject.Find("Info_Context");
            DamageFont = GameObject.Find("label_dmgfont");
            Button_Confirm = GameObject.Find("Button_Confirm");


            Button_Pause = GameObject.Find("Button_Pause");
            Bar_Attack = GameObject.Find("Bar_Attack");


            Info_Icon = GameObject.Find("Info_Icon");
            Info_Description = GameObject.Find("Info_Description");

            HPBar = GameObject.Find("HPBar");
            HPBar_SlowFill1 = GameObject.Find("HPBar_SlowFill1");
            HPBar_SlowFill2 = GameObject.Find("HPBar_SlowFill2");
            HPBar_Fill1 = GameObject.Find("HPBar_Fill1");
            HPBar_Fill2 = GameObject.Find("HPBar_Fill2");

            HPBar_Name = GameObject.Find("HPBar_Name");
            HPBar_Unit = GameObject.Find("HPBar_Unit");
            HPBar_Count = GameObject.Find("HPBar_Count");


        }

        // prefabs

    }
}