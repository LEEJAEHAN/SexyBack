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
            Clear();
        }
        internal void Clear()
        {
            transform.FindChild("ContentTable").transform.DestroyChildren();
        }
        
        public void OnEnable()
        {
            if(!Load)
            {
                FillWindow(Singleton<TableLoader>.getInstance().mapTable);
                Load = true;
            }
        }

        private void FillWindow(Dictionary<string, MapData> gamemodetable)
        {
            foreach(MapData data in Singleton<TableLoader>.getInstance().mapTable.Values)
            {
                GameObject Widget = ViewLoader.InstantiatePrefab(transform.GetChild(1), data.ID, "Prefabs/UI/StageWidget");
                TimeSpan ts =  TimeSpan.FromSeconds(data.LimitTime);
                string expresson = ts.Hours + ":" + ts.Minutes.ToString("D2") + ":" + ts.Seconds.ToString("D2");
                Widget.transform.FindChild("BestTime").gameObject.GetComponent<UILabel>().text = "";
                Widget.transform.FindChild("TimeLimit").gameObject.GetComponent<UILabel>().text = expresson;
                Widget.transform.FindChild("StartButton/StageName").gameObject.GetComponent<UILabel>().text = data.Name;

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
