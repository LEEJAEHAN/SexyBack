using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackMenuScene
{
    public class MapWindow : MonoBehaviour
    {
        Transform Slot;
        string SelectedMapID;
        private void Awake()
        {
            Slot = transform.FindChild("ContentTable");
        }

        public void OnEnable()
        {
            Slot.DestroyChildren();
            SelectedMapID = null;
            FillWindow(Singleton<MapManager>.getInstance().Maps);
        }

        private void FillWindow(Dictionary<string, Map> gamemodetable)
        {
            foreach(Map map in gamemodetable.Values)
            {
                GameObject Widget = ViewLoader.InstantiatePrefab(Slot, map.ID, "Prefabs/UI/StageWidget");
                TimeSpan ts =  TimeSpan.FromSeconds(map.baseData.LimitTime);
                string expresson = ts.Hours + ":" + ts.Minutes.ToString("D2") + ":" + ts.Seconds.ToString("D2");
                Widget.transform.FindChild("BestTime").gameObject.GetComponent<UILabel>().text = "";
                Widget.transform.FindChild("TimeLimit").gameObject.GetComponent<UILabel>().text = expresson;
                Widget.transform.FindChild("StartButton/StageName").gameObject.GetComponent<UILabel>().text = map.baseData.Name;

                EventDelegate ed = new EventDelegate(this, "onStartButton");
                EventDelegate.Parameter param = new EventDelegate.Parameter();
                param.obj = Widget;
                param.field = "selectedObject";
                ed.parameters[0] = param;
                Widget.transform.FindChild("StartButton").GetComponent<UIButton>().onClick.Add(ed);
            }

            Slot.GetComponent<UITable>().Reposition();
        }

        void onStartButton(GameObject selectedObject)
        {
            SelectedMapID  = selectedObject.name;
            GetComponent<TweenAlpha>().enabled = true;
            GameObject.Find("UICamera").GetComponent<UICamera>().eventReceiverMask = 0; // nothing

            //GetComponent<TweenAlpha>().ResetToBeginning();
            //GameObject.Find("Top_Buttons").transform.DestroyChildren();
            //transform.DestroyChildren();
            //GameObject boosting = ViewLoader.InstantiatePrefab(transform, "BoostingPopUp", "Prefabs/UI/BoostingPopUp");
            //boosting.transform.FindChild("Yes").GetComponent<UIButton>().onClick.Add(new EventDelegate(this, "BoostStart"));
            //boosting.transform.FindChild("No").GetComponent<UIButton>().onClick.Add(new EventDelegate(this, "NoBoostStart"));
        }

        public void onTweenFinish()
        {
            Singleton<MapManager>.getInstance().StartMap(SelectedMapID);
        }

    }
}
