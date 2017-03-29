using System;
using System.Collections.Generic;
using SexyBackPlayScene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SexyBackMenuScene
{
    public class GameModeWindow : MonoBehaviour
    {
        bool Load = false;
        private void Awake()
        {
        }

        public void Select(bool toggle)
        {
            this.gameObject.SetActive(toggle);
            if(!Load)
            {
                FillWindow(Singleton<TableLoader>.getInstance().gamemodetable);
                Load = true;
            }
        }

        private void FillWindow(Dictionary<string, MapData> gamemodetable)
        {
            foreach(MapData data in Singleton<TableLoader>.getInstance().gamemodetable.Values)
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
            string stageid = selectedObject.name;

            GameObject.Find("Bottom_Window").transform.DestroyChildren();
            GameObject.Find("Middle_Area").transform.DestroyChildren();
            GameObject.Find("Middle_Window").transform.DestroyChildren();

            GameObject boosting = ViewLoader.InstantiatePrefab(transform, "BoostingPopUp", "Prefabs/UI/BoostingPopUp");
            boosting.transform.FindChild("Yes").GetComponent<UIButton>().onClick.Add(new EventDelegate(this, "BoostStart"));
            boosting.transform.FindChild("No").GetComponent<UIButton>().onClick.Add(new EventDelegate(this, "NoBoostStart"));
            Singleton<SceneParmater>.getInstance().StageID = stageid;
        }

        void BoostStart()
        {
            Singleton<SceneParmater>.getInstance().StageBonus = true;
            SceneManager.LoadScene("PlayScene");
        }
        void NoBoostStart()
        {
            Singleton<SceneParmater>.getInstance().StageBonus = false;
            SceneManager.LoadScene("PlayScene");
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        internal void Clear()
        {
            this.transform.FindChild("ContentTable").transform.DestroyChildren();

        }
    }
}
