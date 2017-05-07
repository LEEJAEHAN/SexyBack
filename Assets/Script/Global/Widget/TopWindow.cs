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

    public void DrawEquipments(Dictionary<string, Equipment> equipments)
    {
        Transform equipIcons = transform.FindChild("Slot2/Table");       
        for (int i = 0; i < equipIcons.childCount; i++)
        {
            Transform part = equipIcons.GetChild(i);
            if (equipments.ContainsKey(part.name))
            {
                part.gameObject.SetActive(true);
                part.GetComponent<UISprite>().spriteName = equipments[part.name].iconID;
            }
            else
            {
                part.gameObject.SetActive(false);
            }
        }
        equipIcons.GetComponent<UITable>().Reposition();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
