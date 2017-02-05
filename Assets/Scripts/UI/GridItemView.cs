using UnityEngine;

namespace SexyBackPlayScene
{
    public class GridItemView : MonoBehaviour
    {
        // researchview 가 아닌 iconview 범용 스크립트로 만들각오.
        public delegate void GridItemSelect_Event(string name);
        public event GridItemSelect_Event Action_SelectGridItem;

        public delegate void GridItemConfirm_Event(string name);
        public event GridItemConfirm_Event Action_ConfirmGridItem;

        bool selected = false;

        public void onItemSelect() //, string ItemButtonName, bool toggleState
        {
            if (GetComponent<UIToggle>().value == false) //// toggle off
            {
                selected = false;
                ViewLoader.Button_Confirm.GetComponent<UIButton>().onClick.Clear();
                Action_SelectGridItem(null);
            }

            else if (GetComponent<UIToggle>().value == true) // toggle on
            {
                selected = true;
                ViewLoader.Button_Confirm.GetComponent<UIButton>().onClick.Add(new EventDelegate(this, "onConfirm"));
                Action_SelectGridItem(this.name);
            }
        }

        public void onConfirm()
        {
            Action_ConfirmGridItem(this.name);
        }

    }
}