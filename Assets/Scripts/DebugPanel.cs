using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class DebugPanel : MonoBehaviour
    {

        Vector3 mouseinputpoint;
        Ray ray;
        RaycastHit hit;

        Ray uiray;
        RaycastHit uihit;

        Ray effectray;
        RaycastHit effecthit;


        Ray a;
        RaycastHit ahit;

        Vector3 moveDirection = new Vector3(0, 1, -3) - new Vector3(0, 0, -10);
        // 문 - 초기카메라
        //Vector3 moveDirection = Singleton<MonsterManager>.getInstance().GetMonster().Position - new Vector3(0,0,-10); // 몹 - 초기카메라
        //  Vector3 moveDirection = new Vector3(0,3,-1) - new Vector3(0, 0, -10); // 문 - 초기카메라

   

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

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(a.origin, a.direction * 20);
            Gizmos.DrawSphere(ahit.point, 0.1f);

        }
        // Update is called once per frame
        bool flipflop = true;
        void Update()
        {
            if (flipflop)
            {
                BigInteger a = new BigInteger("123456123456");
                sexybacklog.Console(a.To5String());
                flipflop = !flipflop;
            }

            if (Input.GetMouseButtonDown(0))
            {
                mouseinputpoint = Input.mousePosition;

                uiray = UICamera.currentCamera.ScreenPointToRay(mouseinputpoint);

                ray = ViewLoader.HeroCamera.ScreenPointToRay(mouseinputpoint);

                effectray = ViewLoader.EffectCamera.ScreenPointToRay(mouseinputpoint);


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
                //a = new Ray(Singleton<MonsterManager>.getInstance().GetMonster().CenterPosition, directionVector);
                //Physics.Raycast(a, out ahit, 100, 110000000);
            }
        }


    }


}
