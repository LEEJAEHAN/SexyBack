using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene 
{
    public class TabView : MonoBehaviour
    {
        public delegate void EventHandler_HideItem();
        public delegate void EventHandler_ShowItem();

        public event EventHandler_ShowItem Action_ShowList = delegate { };
        public event EventHandler_ShowItem Action_HideList = delegate { };

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void onChangeTap(bool value)
        {
            if (value == true)
            {
                transform.FindChild("New").gameObject.SetActive(false);
                Action_ShowList();
            }
            if (value == false)
            {
                ViewLoader.BottomScrollView.GetComponent<UIScrollView>().ResetPosition();
                Action_HideList();
            }
        }
    }

}