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

        // 동적으로 생성되는 것은 ㄴㄴ
        public static GameObject label_debug;
        public static GameObject label_herodmg;
        public static GameObject label_elementaldmg;

        public static GameObject label_exp;
        public static GameObject label_monsterhp;

        public static GameObject projectiles;
        public static GameObject hitparticle;
        public static GameObject Effect_Sword;
        public static GameObject hero_sprite;
        public static GameObject area_elemental;
        public static GameObject shooter;
        public static GameObject monsters;

        // NGUI event Listner hub
        public static GameObject LevelUpViewController;

        // NGUI
        public static GameObject Info_Context;
        public static GameObject DamageFont;
        public static GameObject Button_Confirm;
        public static GameObject Item_Enable;
        public static GameObject Item_Disable;
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

            HeroCamera = GameObject.Find("HeroCamera").GetComponent<Camera>();
            EffectCamera = GameObject.Find("EffectCamera").GetComponent<Camera>();
            label_debug = GameObject.Find("label_debug");
            label_herodmg = GameObject.Find("label_herodmg");
            label_elementaldmg = GameObject.Find("label_elementaldmg");

            label_exp = GameObject.Find("label_exp");
            label_monsterhp = GameObject.Find("label_monsterhp");


            projectiles = GameObject.Find("projectiles");
            hitparticle = GameObject.Find("hitparticle");

            Effect_Sword = GameObject.Find("Effect_Sword");
            hero_sprite = GameObject.Find("hero_sprite");
            area_elemental = GameObject.Find("area_elemental");
            shooter = GameObject.Find("shooter");
            monsters = GameObject.Find("monsters");

            LevelUpViewController = GameObject.Find("LevelUpViewController");

            Info_Context = GameObject.Find("Info_Context");
            DamageFont = GameObject.Find("label_dmgfont");
            Button_Confirm = GameObject.Find("Button_Confirm");
            Item_Enable = GameObject.Find("Item_Enable");
            Item_Disable = GameObject.Find("Item_Disable");
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