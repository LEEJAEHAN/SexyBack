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
        // TODO: fixed update 에대해좀더알아야할듯;

        GameObject Hpbar;
        GameObject LateBar1;
        GameObject LateBar2;

        GameObject Bar2;
        GameObject Bar1;

        //float now = 100;
        float instantBarGauge = 1; // 표시되는bar 0 되면올려줘야한다.
        float goal = 100; // 슬로우바의최종목표. 음수도된다.
        float late = 100f;

        float moveamount = 0;
        float addline = 0;

        //            sexybacklog.Console("goal " + goal + " next " + instantBarGauge + " now " + now + " moveamount " + moveamount);

        // Use this for initialization
        void Start()
        {
            Singleton<GameInput>.getInstance().noticeTouchPosition += onTouch;
            Hpbar = GameObject.Find("HPBar");

            LateBar1 = GameObject.Find("HPBar_SlowFill1");
            LateBar2 = GameObject.Find("HPBar_SlowFill2");

            Bar1 = GameObject.Find("HPBar_Fill1");
            Bar2 = GameObject.Find("HPBar_Fill2");

        }
        void onTouch(TapPoint tap)
        {
            instantBarGauge = Hpbar.GetComponent<UIProgressBar>().value - 0.25f;
            if (instantBarGauge <= 0)
            {
                instantBarGauge += 1f;
                ChangeBar();
            }
            Hpbar.GetComponent<UIProgressBar>().value = instantBarGauge;

            // 표시되는 바의 목표와 속도 set
            goal -= 0.25f;
            moveamount = goal - late;
        }

        void ChangeBar()
        {
            Color newColor = Bar2.GetComponent<UISprite>().color;
            Bar1.GetComponent<UISprite>().color = newColor;

            newColor = Color.HSVToRGB(UnityEngine.Random.Range(0, 1f), 1f, 1f);
            Bar2.GetComponent<UISprite>().color = newColor;
        }
        float velocity = 0.3333f; // 1일시 1초에 목적지까지 깎임. 기본 3초
        float accel = 0.00f;

        private void FixedUpdate()
        {
            if (goal >= late)
            {   // 멈춤과 리셋
                LateBar1.GetComponent<UISprite>().fillAmount = Hpbar.GetComponent<UIProgressBar>().value; // 정확한값은 업데이트를타는 HPbar
                late = goal;
                velocity = 0.3333f;
                return;
            }
            // 가속과 이동. 이벤트시 설정도는 goal과 moveamount 는 절대 바꾸지않는다.
            velocity += accel;
            float step = moveamount * velocity * Time.fixedDeltaTime; // 이동은하고있다.

            bool isOver = false;
            if ((int)(late + step) != (int)(late))
                isOver = true;

            late += step;
            DisplayLateGaugeBar(late, goal, isOver);
        }

        void DisplayLateGaugeBar(float late, float goal, bool isover)
        {
            int index = (int)late;
            float fillAmount = late - (int)late;
            int goalindex = (int)goal;

            if (isover) // 오버 이벤트,
                FillAndChangeColor(index);
            if( goalindex < index) // 1차원이상차이날때만 중첩late바를 출력한다.
                LateBar2.SetActive(true);
            else
                LateBar2.SetActive(false);

            LateBar1.GetComponent<UISprite>().fillAmount = fillAmount;
        }

        private void FillAndChangeColor(int index) // latebar
        {
            Color newColor = LateBar2.GetComponent<UISprite>().color;
            LateBar1.GetComponent<UISprite>().color = newColor;

            newColor = Color.HSVToRGB(UnityEngine.Random.Range(0, 1f), 0.7f, 1f);
            LateBar2.GetComponent<UISprite>().color = newColor;
        }

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
        void Update()
        {

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
