using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class TopWindow : MonoBehaviour {

    public void Awake()
    {
        if (SceneManager.GetActiveScene().name == "MenuScene")
        {
            Singleton<EquipmentManager>.getInstance().BindTopView(this);
            GameObject.Find("SP").GetComponent<UILabel>().text = "";
            GameObject.Find("GEM").GetComponent<UILabel>().text = "";
        }
    }

    public void Start()
    {
        DrawEquipments(Singleton<EquipmentManager>.getInstance().currentEquipSet);
    }

    public void DrawEquipments(Dictionary<Equipment.Type, Equipment> equipments)
    {
        Transform equipIcons = transform.FindChild("Slot2/Table");       
        for (int i = 0; i < equipIcons.childCount; i++)
        {
            Transform part = equipIcons.GetChild(i);
            Equipment.Type t = (Equipment.Type)Enum.Parse(typeof(Equipment.Type), part.name);
            if (equipments.ContainsKey(t))
            {
                part.gameObject.SetActive(true);
                part.GetComponent<UISprite>().spriteName = equipments[t].iconID;
            }
            else
            {
                part.gameObject.SetActive(false);
            }
        }
        equipIcons.GetComponent<UITable>().Reposition();
    }

	// Update is called once per frame
	void Update () {
		
	}
}
