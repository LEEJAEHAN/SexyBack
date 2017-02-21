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
        bool flipflop = true;
        public void Printa()
        {
            sexybacklog.Console("a");
        }
        void Update()
        {
            //sexybacklog.InGame(Singleton<MonsterManager>.getInstance().GetMonster().HP.ToString());

            //Vector3 WallMoveVector = GameSetting.defaultHeroPosition - GameSetting.ECamPosition;
            //GameObject.Find("back_image").transform.position += WallMoveVector * Time.deltaTime * 0.2f;
            //GameObject.Find("back_image2").transform.position += WallMoveVector * Time.deltaTime * 0.2f;


            if (flipflop)
            {
                //string str = "-1.231232";

                //float a = 0;
                //if (Single.TryParse(str, out a))
                //{ }

               //// float a = Convert.ToSingle();

               // sexybacklog.Console(a);
                //if (str.Contains("-"))
                //    str.Replace("-", "");

                //BigInteger a = new BigInteger(9999);
                //sexybacklog.Console(a.toLeftDigitString(9, 4));
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
            }
        }


    }


}
