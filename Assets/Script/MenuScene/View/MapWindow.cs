using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackMenuScene
{
    public class MapWindow : MonoBehaviour
    {
        Transform Slot;
        UIScrollView Scroll;
        string SelectedMapID;
        private void Awake()
        {
            Slot = transform.FindChild("ScrollView/ContentTable");
            Scroll = transform.FindChild("ScrollView").GetComponent<UIScrollView>();
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
                TimeSpan lTime =  TimeSpan.FromSeconds(map.baseData.LimitTime);
                TimeSpan bTime = TimeSpan.FromSeconds(map.BestTime);
                //string expresson = ts.Hours + ":" + ts.Minutes.ToString("D2") + ":" + ts.Seconds.ToString("D2");
                Widget.transform.FindChild("BestTime").gameObject.GetComponent<UILabel>().text = map.BestTime > 0 ?
                    string.Format("{0:D2}:{1:D2}:{2:D2} {3}", bTime.Hours, bTime.Minutes, bTime.Seconds, map.BestRank) : "";
                Widget.transform.FindChild("TimeLimit").gameObject.GetComponent<UILabel>().text =
                    string.Format("{0:D2}:{1:D2}:{2:D2}", lTime.Hours, lTime.Minutes, lTime.Seconds);
                Widget.transform.FindChild("StartButton/StageName").gameObject.GetComponent<UILabel>().text = map.baseData.Name;

                if(map.baseData.RequireMap != null)
                {
                    if (gamemodetable[map.baseData.RequireMap].ClearCount == 0)
                    {
                        Widget.transform.FindChild("Back").GetComponent<UISprite>().color = new Color(0, 0, 0, 0.33f);
                        Widget.transform.FindChild("StartButton").GetComponent<UIButton>().isEnabled = false;
                    }
                }
                EventDelegate ed = new EventDelegate(this, "onStartButton");
                EventDelegate.Parameter param = new EventDelegate.Parameter();
                param.obj = Widget;
                param.field = "selectedObject";
                ed.parameters[0] = param;
                Widget.transform.FindChild("StartButton").GetComponent<UIButton>().onClick.Add(ed);
            }

            Slot.GetComponent<UITable>().Reposition();
            Scroll.ResetPosition();
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
