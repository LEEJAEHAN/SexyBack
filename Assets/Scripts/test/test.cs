using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace test
{

public class test : MonoBehaviour {

        List<TestItem> testitemlist = new List<TestItem>();
        int count;
        int value;
        double timer = 0;
        int refreshtime = 5;
        GameObject grid;
        GameObject gridItem;


        // Use this for initialization
        void Start () {

            count = 0;
            value = 0;
            grid =  GameObject.Find("Grid");
            gridItem = Resources.Load("Prefabs/itemsprite") as GameObject;
        }
	
	// Update is called once per frame
	void Update () {

            timer += Time.deltaTime;
            if(timer > refreshtime)
            {
                Debug.Log("item REfresh");
                timer -= refreshtime;
                DrawItems();
            }
        }
        public void Additem()
        {
            Debug.Log("AddItem");
            testitemlist.Add(new TestItem(value++));
        }

        public void DrawItems()
        {
            ClearGrid();
            foreach (TestItem a in testitemlist)
            {
                MakeITemUI();
                int b = a.value;
                grid.GetComponent<UIGrid>().Reposition();

            }
        }
        public void ClearGrid()
        {
            grid.transform.DestroyChildren();
            count = 0;
        }

        public void MakeITemUI()
        {
            GameObject newitemcomponent = GameObject.Instantiate<GameObject>(gridItem);
            newitemcomponent.name = "item" + count.ToString();
            newitemcomponent.transform.parent = grid.transform;
            newitemcomponent.transform.localScale = grid.transform.localScale;

            //            grid.GetComponent<UIGrid>().AddChild(newitemcomponent.transform);

            count++;
            //;
        }
}

}
