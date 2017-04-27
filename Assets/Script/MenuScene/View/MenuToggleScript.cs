using UnityEngine;

namespace SexyBackMenuScene
{
    class MenuToggleScript : MonoBehaviour
    {
        GameObject GameModeWindow;
        GameObject EquipmentWindow;

        private void Awake()
        {
            GameModeWindow = transform.FindChild("GameModeWindow").gameObject;
            GameModeWindow.transform.localPosition = Vector3.zero;
            GameModeWindow.SetActive(false);

            EquipmentWindow = transform.FindChild("EquipmentWindow").gameObject;
            EquipmentWindow.transform.localPosition = Vector3.zero;
            EquipmentWindow.SetActive(false);


        }

        public void onToggle(string ButtonName, bool togglevalue)
        {
            if(ButtonName =="게임시작")
            {
                GameModeWindow.SetActive(togglevalue);
            }
            if (ButtonName == "장비교체")
            {
                EquipmentWindow.SetActive(togglevalue);
            }


        }


    }
}
