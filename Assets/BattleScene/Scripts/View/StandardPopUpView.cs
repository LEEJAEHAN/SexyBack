using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SexyBackPlayScene
{
    public class StandardPopUpView : MonoBehaviour
    {
        public Action Action_Yes; // Action == delegat void
        public Action Action_No;

        public void OnYes()
        {
            Action_Yes();
            Finish();
        }
        public void OnNo()
        {
            Action_No();
            Finish();
        }
        void Finish()
        {
            Action_Yes = null;
            Action_No = null;
            this.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
