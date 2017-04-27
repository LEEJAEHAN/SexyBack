using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SexyBackMenuScene;

public class EquipmentWindow : MonoBehaviour
{
    public void OnEnable()
    {
        setPosition();
        ClearWindow();
        FillInventory(Singleton<EquipmentManager>.getInstance().inventory);
    }

    public void FillInventory(List<Equipment> equipments)
    {
        GameObject inventory = transform.FindChild("인벤토리/Grid").gameObject;

        for(int i = 0; i < equipments.Count; i++)
        {
            if (equipments[i] == null)
                continue;

            GameObject EquipView = ViewLoader.InstantiatePrefab(inventory.transform, i.ToString(), "Prefabs/UI/Equipment");

            EventDelegate Ehandler = new EventDelegate(this, "onInvenTouch");
            EventDelegate.Parameter Eparam1 = new EventDelegate.Parameter();
            Eparam1.obj = EquipView;
            Eparam1.field = "name";
            EventDelegate.Parameter Eparam2 = new EventDelegate.Parameter();
            Eparam2.obj = EquipView.GetComponent<UIToggle>();
            Eparam2.field = "value";

            Ehandler.parameters[0] = Eparam1;
            Ehandler.parameters[1] = Eparam2;

            EquipView.GetComponent<UIToggle>().onChange.Add(Ehandler);
       }

        //foreach (Equipment a in equipments)
        //{
        //    ViewLoader.InstantiatePrefab(inventory.transform, a., "Prefabs/UI/Equipment");
        //}

        inventory.GetComponent<UIGrid>().Reposition();
    }
    public void onEquipTouch()
    {

    }
    public void onInvenTouch(string index, bool toggle)
    {
        if(toggle)
        {
            sexybacklog.Console("아이콘눌렀다." + toggle + index);
            transform.FindChild("ButtonSet/Set1").gameObject.SetActive(true);
        }
        else
        {
            sexybacklog.Console("아이콘뗏다." + toggle + index);
            transform.FindChild("ButtonSet/Set1").gameObject.SetActive(false);
        }
    }

    internal void setPosition()
    {
        transform.localPosition = Vector3.zero;
    }


    private void ClearWindow()
    {
        transform.FindChild("아이템정보").gameObject.SetActive(false);

        //transform.FindChild("장비슬롯/Slot1/Equip1").gameObject.SetActive(false);
        transform.FindChild("장비슬롯/Slot2/Equip2").gameObject.SetActive(false);
        transform.FindChild("장비슬롯/Slot3/Equip3").gameObject.SetActive(false);
        transform.FindChild("장비슬롯/Mask").gameObject.SetActive(false);

        transform.FindChild("Text").GetComponent<UILabel>().text = "";

        transform.FindChild("ButtonSet/Set1").gameObject.SetActive(false);
        transform.FindChild("ButtonSet/Set2").gameObject.SetActive(false);
        transform.FindChild("인벤토리/Grid").DestroyChildren();

    }



    //private void FillWindow(Dictionary<string, MapData> gamemodetable)
    //{
    //    foreach (MapData data in Singleton<TableLoader>.getInstance().mapTable.Values)
    //    {
    //        GameObject Widget = ViewLoader.InstantiatePrefab(transform.GetChild(1), data.ID, "Prefabs/UI/StageWidget");
    //        TimeSpan ts = TimeSpan.FromSeconds(data.LimitTime);
    //        string expresson = ts.Hours + ":" + ts.Minutes.ToString("D2") + ":" + ts.Seconds.ToString("D2");
    //        Widget.transform.FindChild("BestTime").gameObject.GetComponent<UILabel>().text = "";
    //        Widget.transform.FindChild("TimeLimit").gameObject.GetComponent<UILabel>().text = expresson;
    //        Widget.transform.FindChild("StartButton/StageName").gameObject.GetComponent<UILabel>().text = data.Name;

    //        EventDelegate ed = new EventDelegate(this, "onStartButton");
    //        EventDelegate.Parameter param = new EventDelegate.Parameter();
    //        param.obj = Widget;
    //        param.field = "selectedObject";
    //        ed.parameters[0] = param;
    //        Widget.transform.FindChild("StartButton").GetComponent<UIButton>().onClick.Add(ed);
    //    }

    //    gameObject.GetComponentInChildren<UITable>().Reposition();
    //}

}
