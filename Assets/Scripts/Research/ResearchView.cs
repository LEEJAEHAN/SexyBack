using UnityEngine;

namespace SexyBackPlayScene
{
    public class ResearchView : MonoBehaviour
    {
        // researchview 가 아닌 iconview 범용 스크립트로 만들각오.
        public delegate void ResearchSelect_Event(string name);
        public event ResearchSelect_Event Action_ResearchSelect;

        public delegate void ResearchStart_Event(string name);
        public event ResearchStart_Event Action_ResearchStart;

        bool selected = false;

        public void onItemSelect() //, string ItemButtonName, bool toggleState
        {
            if (GetComponent<UIToggle>().value == false) //// toggle off
            {
                selected = false;
                ViewLoader.Button_Confirm.GetComponent<UIButton>().onClick.Clear();
                Action_ResearchSelect(null);
            }

            else if (GetComponent<UIToggle>().value == true) // toggle on
            {
                selected = true;
                ViewLoader.Button_Confirm.GetComponent<UIButton>().onClick.Add(new EventDelegate(this, "onConfirm"));
                Action_ResearchSelect(this.name);
            }
        }

        public void onConfirm()
        {
            Action_ResearchStart(this.name);
        }

    }
}