using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SexyBackMenuScene
{
    public enum EquipmentState
    {
        None = 1,
        EquipSelected,
        InvenSelected,
        EnchantMode,
        EvolutionMode,

        Working = 10,
    }

    public class EquipmentStateMachine
    {
        public EquipmentState Mode;
        Transform transform;
        EquipmentWindow script;
        EquipmentManager manager;

        public EquipmentStateMachine(Transform owner, EquipmentWindow component)
        {
            transform = owner;
            script = component;
            manager = Singleton<EquipmentManager>.getInstance();
        }
        public void ChangeMode(EquipmentState state)
        {
            EndMode(Mode);
            Mode = state;
            StartMode(Mode);
        }
        private void StartMode(EquipmentState state)
        {
            if (state == EquipmentState.None)
            {
                transform.FindChild("아이템정보").gameObject.SetActive(false);
                transform.FindChild("ButtonSet/Set1").gameObject.SetActive(false);
                transform.FindChild("ButtonSet/Set2").gameObject.SetActive(false);
            }
            else if (state == EquipmentState.InvenSelected)
            {
                script.FillSelected(manager.Focused);
                transform.FindChild("ButtonSet/Set1").gameObject.SetActive(true);
                transform.FindChild("ButtonSet/Set1/Table/Button1/Label").GetComponent<UILabel>().text = "장착";
                transform.FindChild("ButtonSet/Set1/Table/Button1").GetComponent<UIButton>().onClick.Add(new EventDelegate(script, "onEquipButton"));
                transform.FindChild("ButtonSet/Set1/Table/Button2").GetComponent<UIButton>().isEnabled = manager.Focused.CanEvolution;
                transform.FindChild("ButtonSet/Set1/Table/Button4/Label").GetComponent<UILabel>().text = EquipmentWiki.LockToString(manager.Focused.isLock);
            }
            else if (state == EquipmentState.EquipSelected)
            {
                script.FillSelected(manager.Focused);
                transform.FindChild("ButtonSet/Set1").gameObject.SetActive(true);
                transform.FindChild("ButtonSet/Set1/Table/Button1/Label").GetComponent<UILabel>().text = "해제";
                transform.FindChild("ButtonSet/Set1/Table/Button1").GetComponent<UIButton>().onClick.Add(new EventDelegate(script, "onUnEquipButton"));
                transform.FindChild("ButtonSet/Set1/Table/Button2").GetComponent<UIButton>().isEnabled = manager.Focused.CanEvolution;
                transform.FindChild("ButtonSet/Set1/Table/Button4/Label").GetComponent<UILabel>().text = EquipmentWiki.LockToString(manager.Focused.isLock);
            }
            else if (state == EquipmentState.EnchantMode)
            {
                script.FillSelected(manager.Focused);
                transform.FindChild("Text").GetComponent<UILabel>().text = "재료로 사용할 장비를 선택해 주세요.\n<주의> 재료로 사용되는 장비는 사라지며 복구되지 않습니다.";
                transform.FindChild("ButtonSet/Set2").gameObject.SetActive(true);
                transform.FindChild("ButtonSet/Set2/Button1").GetComponent<UIButton>().onClick.Add(new EventDelegate(script, "onEnchantConfirm"));
                transform.FindChild("ButtonSet/Set2/Button1").GetComponent<UIButton>().isEnabled = false;
                transform.FindChild("ButtonSet/Set2/Button2").GetComponent<UIButton>().onClick.Add(new EventDelegate(script, "onCancel"));
                transform.FindChild("장비슬롯/Mask").gameObject.SetActive(true);
                script.CheckList.Clear();
                Transform inventory = transform.FindChild("인벤토리/ScrollView/Grid");
                for (int i = 0; i < inventory.childCount; i++)
                {
                    Transform view = inventory.GetChild(i);
                    if (i == manager.SelectIndex)
                    {
                        view.FindChild("Mask").gameObject.SetActive(true);
                        // 마스크가나 락이 생기는데 uitoggle을끈다.
                        view.GetComponent<UIToggle>().Set(true, false);
                        view.GetComponent<UIToggle>().enabled = false;
                    }
                    else if (manager.inventory[i].isLock)
                    {
                        view.FindChild("Lock").gameObject.SetActive(true);
                        view.FindChild("Mask").gameObject.SetActive(true);
                        view.GetComponent<UIToggle>().enabled = false;
                    }
                    else
                    {
                        // change select event and toggle event
                        view.GetComponent<UIToggle>().group = 0;
                        view.GetComponent<UIToggle>().Set(false, false);
                        view.GetComponent<UIToggle>().onChange.Clear();
                        view.GetComponent<UIToggle>().activeSprite = view.FindChild("Check").GetComponent<UISprite>();
                        script.SetToggleEvent(view.gameObject, "onInvenCheck");
                    }
                }
            }
            else if (state == EquipmentState.EvolutionMode)
            {
                script.FillSelected(manager.Focused);
                transform.FindChild("Text").GetComponent<UILabel>().text = "MAX 강화된 동일장비 1개를 재료로 선택해주세요.\n<주의> 재료로 사용되는 장비는 사라지며 복구되지 않습니다.";
                transform.FindChild("ButtonSet/Set2").gameObject.SetActive(true);
                transform.FindChild("ButtonSet/Set2/Button1").GetComponent<UIButton>().onClick.Add(new EventDelegate(script, "onEvolutionConfirm"));
                transform.FindChild("ButtonSet/Set2/Button1").GetComponent<UIButton>().isEnabled = false;
                transform.FindChild("ButtonSet/Set2/Button2").GetComponent<UIButton>().onClick.Add(new EventDelegate(script, "onCancel"));
                transform.FindChild("장비슬롯/Mask").gameObject.SetActive(true);
                script.CheckList.Clear();
                Transform inventory = transform.FindChild("인벤토리/ScrollView/Grid");
                for (int i = 0; i < inventory.childCount; i++)
                {
                    Transform view = inventory.GetChild(i);
                    if (i == manager.SelectIndex)
                    {
                        view.FindChild("Mask").gameObject.SetActive(true);
                        view.GetComponent<UIToggle>().Set(true, false);
                        view.GetComponent<UIToggle>().enabled = false;
                    }
                    else if (manager.inventory[i].isLock)
                    {
                        view.FindChild("Lock").gameObject.SetActive(true);
                        view.FindChild("Mask").gameObject.SetActive(true);
                        view.GetComponent<UIToggle>().enabled = false;
                    }
                    else if (manager.isEvolutionMaterial(i) == false)
                    {
                        view.FindChild("Mask").gameObject.SetActive(true);
                        view.GetComponent<UIToggle>().enabled = false;
                    }
                    else
                    {
                        // change select event and toggle event
                        view.GetComponent<UIToggle>().group = 3;
                        view.GetComponent<UIToggle>().optionCanBeNone = true;
                        view.GetComponent<UIToggle>().Set(false, false);
                        view.GetComponent<UIToggle>().onChange.Clear();
                        view.GetComponent<UIToggle>().activeSprite = view.FindChild("Check").GetComponent<UISprite>();
                        script.SetToggleEvent(view.gameObject, "onInvenCheck");
                    }
                }
            }
        }

        internal void SubMode(EquipmentState enchanting, bool enable)
        {
            if(enable)
            {
                script.FillSelected(manager.Focused);
                transform.FindChild("아이템정보배경/Effect").gameObject.SetActive(true);
                transform.FindChild("아이템정보배경/Effect").GetComponent<TweenAlpha>().PlayForward();
                GameObject.Find("UICamera").GetComponent<UICamera>().eventReceiverMask = 0; // nothing
            }
            else
            {
                transform.FindChild("아이템정보배경/Effect").gameObject.SetActive(false);
                transform.FindChild("아이템정보배경/Effect").GetComponent<TweenAlpha>().ResetToBeginning();
                GameObject.Find("UICamera").GetComponent<UICamera>().eventReceiverMask = -1; // everything
            }
        }

        internal void UndoMode()
        {
            EndMode(Mode);
            Mode = manager.GetPrevMode();
            StartMode(Mode);
        }

        private void EndMode(EquipmentState state)
        {
            if (state == EquipmentState.None)
            {
                transform.FindChild("아이템정보").gameObject.SetActive(true);
            }
            else if (state == EquipmentState.InvenSelected || state == EquipmentState.EquipSelected)
            {
                transform.FindChild("ButtonSet/Set1").gameObject.SetActive(false);
                transform.FindChild("ButtonSet/Set1/Table/Button1").GetComponent<UIButton>().onClick.Clear();
            }
            else if (state == EquipmentState.EnchantMode || state == EquipmentState.EvolutionMode)
            {
                transform.FindChild("Text").GetComponent<UILabel>().text = "";
                transform.FindChild("ButtonSet/Set2").gameObject.SetActive(false);
                transform.FindChild("ButtonSet/Set2/Button1").GetComponent<UIButton>().onClick.Clear();
                transform.FindChild("ButtonSet/Set2/Button2").GetComponent<UIButton>().onClick.Clear();
                transform.FindChild("장비슬롯/Mask").gameObject.SetActive(false);
                script.CheckList.Clear();

                Transform inventory = transform.FindChild("인벤토리/ScrollView/Grid");
                for (int i = 0; i < inventory.childCount; i++)
                {
                    Transform view = inventory.GetChild(i);
                    view.GetComponent<UIToggle>().enabled = true;
                    view.GetComponent<UIToggle>().group = 2;
                    view.GetComponent<UIToggle>().optionCanBeNone = false;

                    if (i == manager.SelectIndex)
                    {
                        view.GetComponent<UIToggle>().Set(true, false);
                    }
                    else
                    {
                        // change select event and toggle event
                        view.GetComponent<UIToggle>().Set(false, false);
                        view.GetComponent<UIToggle>().onChange.Clear();
                        view.GetComponent<UIToggle>().activeSprite = view.FindChild("Selected").GetComponent<UISprite>();
                        script.SetToggleEvent(view.gameObject, "onInvenToggle");
                    }
                    view.FindChild("Mask").gameObject.SetActive(false);
                    view.FindChild("Lock").gameObject.SetActive(false);
                }
            }
        }

    }
}
