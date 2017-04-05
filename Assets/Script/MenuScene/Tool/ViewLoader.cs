using System;
using UnityEngine;
using System.Collections.Generic;
namespace SexyBackMenuScene
{
    public class ViewLoader // View Updater 
    { // publisher 
        public static GameObject Main;
        public static GameObject Mask;
        public static GameObject PopUps;
        public static GameObject Middle_Window;
        public static GameObject Slot1New;
        public static GameObject Slot2New;

        public ViewLoader()
        {
            Main = GameObject.Find("Main");
            Mask = GameObject.Find("Mask");
            PopUps = GameObject.Find("PopUps");
            Middle_Window = GameObject.Find("Middle_Window");
            Slot1New = GameObject.Find("Slot1New");
            Slot2New = GameObject.Find("Slot2New");
        }

        internal void InitUISetting() // remove and hide test game objects
        {
            Mask.transform.position = Vector3.zero;
            PopUps.transform.position = Vector3.zero;

            Mask.SetActive(false);
            PopUps.SetActive(false);

            Slot1New.SetActive(false);
            Slot2New.SetActive(false);

            Middle_Window.GetComponent<GameModeWindow>().Clear();
            GameObject.Find("SP").GetComponent<UILabel>().text = "";
            GameObject.Find("GEM").GetComponent<UILabel>().text = "";
            GameObject.Find("IconTable").transform.DestroyChildren();
        }

        internal void Clear()
        {

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
            newone.transform.parent = PopUps.transform;
            newone.transform.localScale = Vector3.one;
            newone.transform.localPosition = Vector3.zero;

            newone.GetComponent<StandardPopUpView>().Action_Yes += yesfun;
            newone.GetComponent<StandardPopUpView>().Action_No += nofun;
            return newone;
        }

    }
}