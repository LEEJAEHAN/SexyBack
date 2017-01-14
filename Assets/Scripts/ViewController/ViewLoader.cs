using System;
using UnityEngine;
using System.Collections.Generic;
namespace SexyBackPlayScene
{
    public static class ViewLoader // View Updater 
    { // publisher 

        // 모든 인스턴스화 되어있는 뷰를 로드해놓는다.
        // 동적으로 생성되는 것은 ㄴㄴ

        public static GameObject label_debug = GameObject.Find("label_debug");
        public static GameObject label_herodmg = GameObject.Find("label_herodmg");
        public static GameObject label_elementaldmg = GameObject.Find("label_elementaldmg");


        public static GameObject label_exp = GameObject.Find("label_exp");
        public static GameObject label_monsterhp = GameObject.Find("label_monsterhp");

        public static GameObject slash = GameObject.Find("slash");
        public static GameObject hero = GameObject.Find("hero");
        public static GameObject monster = GameObject.Find("monster");
        public static GameObject hitparticle = GameObject.Find("hitparticle");


        // NGUI event Listner hub
        public static GameObject LevelUpViewController = GameObject.Find("LevelUpViewController");

        // NGUI
        public static GameObject Info_Context = GameObject.Find("Info_Context");

        public static GameObject Button_Confirm = GameObject.Find("Button_Confirm");
        public static GameObject Item_Enable = GameObject.Find("Item_Enable");
        public static GameObject Item_Disable = GameObject.Find("Item_Disable");


        // info ui
        public static GameObject Info_Icon = GameObject.Find("Info_Icon");
        public static GameObject Info_Description = GameObject.Find("Info_Description");


    }
}