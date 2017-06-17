using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class TopWindow : MonoBehaviour {

    public void Awake()
    {
        Singleton<EquipmentManager>.getInstance().BindTopView(this);
        Singleton<TalentManager>.getInstance().BindTopView(this);
        Singleton<PremiumManager>.getInstance().BindTopView(this);

        if (SceneManager.GetActiveScene().name == "MenuScene")
        {
            transform.FindChild("Slot1/Title").GetComponent<UILabel>().text = "명성";
            transform.FindChild("Slot1/Value").GetComponent<UILabel>().text = Singleton<TalentManager>.getInstance().Reputation.ToString();
            transform.FindChild("Slot2/Title/LeftArrow").gameObject.SetActive(false);
            transform.FindChild("Slot2/Title/RightArrow").gameObject.SetActive(false);
            transform.FindChild("Slot3/Title").GetComponent<UILabel>().text = "보석";
            transform.FindChild("Slot3/Value").GetComponent<UILabel>().text = Singleton<PremiumManager>.getInstance().Gem.ToString();
        }
        else if (SceneManager.GetActiveScene().name == "PlayScene")
        {
            transform.FindChild("Slot1/Title").GetComponent<UILabel>().text = "진행시간";
            transform.FindChild("Slot1/Value").GetComponent<UILabel>().text = "1:00:00";
            transform.FindChild("Slot2/Title/LeftArrow").gameObject.SetActive(true);
            transform.FindChild("Slot2/Title/RightArrow").gameObject.SetActive(true);
            transform.FindChild("Slot3/Title").GetComponent<UILabel>().text = "보석";
            transform.FindChild("Slot3/Value").GetComponent<UILabel>().text = Singleton<PremiumManager>.getInstance().Gem.ToString();
        }
    }
    internal void PrintSlot1String(string value)
    {
        transform.FindChild("Slot1/Value").GetComponent<UILabel>().text = value;
    }
    internal void PrintSlot3String(string value)
    {
        transform.FindChild("Slot3/Value").GetComponent<UILabel>().text = value;

    }

    public void onRightButton()
    {
        Singleton<EquipmentManager>.getInstance().SwapEquipSet(true);
    }
    public void onLeftButton()
    {
        Singleton<EquipmentManager>.getInstance().SwapEquipSet(false);
    }

    public void Start()
    {
        DrawEquipments(Singleton<EquipmentManager>.getInstance().currentEquipSet);
    }

    public void DrawEquipments(Dictionary<Equipment.Slot, Equipment> equipments)
    {
        Transform equipIcons = transform.FindChild("Slot2/Table");       
        for (int i = 0; i < equipIcons.childCount; i++)
        {
            Transform part = equipIcons.GetChild(i);
            Equipment.Slot t = (Equipment.Slot)Enum.Parse(typeof(Equipment.Slot), part.name);
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
