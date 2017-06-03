using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackMenuScene
{
    public class GameModeWindow : MonoBehaviour
    {
        bool Load = false;
        string selectedMapID = null;
        bool selectedBonus = false;

        private void Awake()
        {
            transform.FindChild("ContentTable").transform.DestroyChildren();
        }
        
        public void Start()
        {
            if(!Load)
            {
                FillWindow(Singleton<MapManager>.getInstance().Maps);
                Load = true;
            }
        }

        private void FillWindow(Dictionary<string, Map> gamemodetable)
        {
            foreach(Map map in gamemodetable.Values)
            {
                GameObject Widget = ViewLoader.InstantiatePrefab(transform.GetChild(1), map.ID, "Prefabs/UI/StageWidget");
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

            gameObject.GetComponentInChildren<UITable>().Reposition();
        }

        void onStartButton(GameObject selectedObject)
        {
            string mapID = selectedObject.name;
            GameObject.Find("Top_Buttons").transform.DestroyChildren();
            transform.DestroyChildren();

            GameObject boosting = ViewLoader.InstantiatePrefab(transform, "BoostingPopUp", "Prefabs/UI/BoostingPopUp");
            boosting.transform.FindChild("Yes").GetComponent<UIButton>().onClick.Add(new EventDelegate(this, "BoostStart"));
            boosting.transform.FindChild("No").GetComponent<UIButton>().onClick.Add(new EventDelegate(this, "NoBoostStart"));

            selectedMapID = mapID;
        }

        void BoostStart()
        {
            selectedBonus = true;
            ViewLoader.Main.GetComponent<Main>().GoPlayScene(selectedMapID, selectedBonus);
        }
        void NoBoostStart()
        {
            selectedBonus = false;
            ViewLoader.Main.GetComponent<Main>().GoPlayScene(selectedMapID, selectedBonus);
        }

    }
}
