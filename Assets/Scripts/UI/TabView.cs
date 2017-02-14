using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene 
{
    public class TabView : MonoBehaviour
    {
        public delegate void EventHandler_HideItem();
        public delegate void EventHandler_ShowItem();

        public event EventHandler_ShowItem Action_ShowList;
        public event EventHandler_ShowItem Action_HideList;

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
                Action_ShowList();
            }
            if (value == false)
            {
                Action_HideList();
            }
        }
    }

}