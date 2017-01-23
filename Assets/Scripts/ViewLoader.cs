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

        public static GameObject Button_Confirm;
        public static GameObject Item_Enable;
        public static GameObject Item_Disable;
        public static GameObject Bar_Attack;
        // info ui
        public static GameObject Info_Icon;
        public static GameObject Info_Description;


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

            Button_Confirm = GameObject.Find("Button_Confirm");
            Item_Enable = GameObject.Find("Item_Enable");
            Item_Disable = GameObject.Find("Item_Disable");
            Bar_Attack = GameObject.Find("Bar_Attack");


            Info_Icon = GameObject.Find("Info_Icon");
            Info_Description = GameObject.Find("Info_Description");
        }



        // prefabs

    }
}