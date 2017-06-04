using System;
using UnityEngine;
using System.Collections.Generic;
namespace SexyBackMenuScene
{
    public class ViewLoader // View Updater 
    { // publisher 
        public static GameObject PopUpPanel;
        public static GameObject Slot1New;
        public static GameObject Slot2New;

        public ViewLoader()
        {
            PopUpPanel = GameObject.Find("UI PopUp");
            Slot1New = GameObject.Find("Slot1New");
            Slot2New = GameObject.Find("Slot2New");
        }

        internal void InitUISetting() // remove and hide test game objects
        {
            PopUpPanel.transform.position = GameObject.Find("UI Root").transform.position;
            PopUpPanel.SetActive(false);
            Slot1New.SetActive(false);
            Slot2New.SetActive(false);
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