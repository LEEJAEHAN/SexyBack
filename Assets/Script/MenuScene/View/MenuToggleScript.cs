using UnityEngine;

namespace SexyBackMenuScene
{
    class MenuToggleScript : MonoBehaviour
    {
        GameObject MapWindow;
        GameObject EquipmentWindow;
        GameObject JobWindow;

        private void Awake()
        {
            MapWindow = transform.FindChild("MapWindow").gameObject;
            //MapWindow.transform.localPosition = Vector3.zero;
            MapWindow.SetActive(false);

            EquipmentWindow = transform.FindChild("EquipmentWindow").gameObject;
            //EquipmentWindow.transform.localPosition = Vector3.zero;
            EquipmentWindow.SetActive(false);

            JobWindow = transform.FindChild("JobWindow").gameObject;
            //JobWindow.transform.localPosition = Vector3.zero;
            JobWindow.SetActive(false);

        }

        public void onToggle(string ButtonName, bool togglevalue)
        {
            if(ButtonName =="게임시작")
            {
                MapWindow.SetActive(togglevalue);
            }
            if (ButtonName == "장비교체")
            {
                EquipmentWindow.SetActive(togglevalue);
            }
            if (ButtonName == "직업전문화")
            {
                JobWindow.SetActive(togglevalue);
            }

        }


    }
}
