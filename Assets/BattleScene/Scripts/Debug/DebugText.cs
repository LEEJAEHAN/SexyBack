using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class DebugText : MonoBehaviour
    {
        private static DebugText instance;
        public static DebugText getInstance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.Find("DebugText").AddComponent<DebugText>();

                return instance;
            }
        }

        UILabel debugTextObject;
        public string debugText = "";
        bool flipflop = true;

        private void Awake()
        {
            debugTextObject = gameObject.GetComponent<UILabel>();
        }

        Vector3 mouseinputpoint;
        Ray ray;
        RaycastHit hit;
        Ray uiray;
        RaycastHit uihit;
        Ray effectray;
        RaycastHit effecthit;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawRay(ray.origin, ray.direction * 20);
            Gizmos.DrawSphere(hit.point, 0.1f);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(uiray.origin, uiray.direction * 20);
            Gizmos.DrawSphere(uihit.point, 0.05f);

            Gizmos.color = Color.green;
            Gizmos.DrawRay(effectray.origin, effectray.direction * 20);
            Gizmos.DrawSphere(effecthit.point, 0.1f);
        }
        // Update is called once per frame

        internal void Append(string v)
        {
            debugText += v;
        }

        void Update()
        {
            debugTextObject.text = debugText;
            debugText = "";

            if (flipflop)
            {
                flipflop = !flipflop;
            }

            if (Input.GetMouseButtonDown(0))
            {
                CheckClick();
            }
        }

        private void CheckClick()
        {
            mouseinputpoint = Input.mousePosition;

            uiray = UICamera.currentCamera.ScreenPointToRay(mouseinputpoint);

            ray = GameCameras.HeroCamera.ScreenPointToRay(mouseinputpoint);

            effectray = GameCameras.EffectCamera.ScreenPointToRay(mouseinputpoint);


            if (Physics.Raycast(uiray, out uihit, 100, 000100000))
            {
                //sexybacklog.Console("Hit UIArea  : " + uihit.collider.gameObject.name + uihit.point);
            }

            if (Physics.Raycast(ray, out hit, 100, 0100000000))
            {
                //sexybacklog.Console("Hit BACK : " + hit.collider.gameObject.tag + " " + hit.collider.gameObject.name + hit.point);
            }

            if (Physics.Raycast(effectray, out effecthit, 100, 1000000000))
            {
                //sexybacklog.Console("Hit EFFECT: " + effecthit.collider.gameObject.tag + " " + effecthit.collider.gameObject.name + effecthit.point);
            }
        }

    }


}
