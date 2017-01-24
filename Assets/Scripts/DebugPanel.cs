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
        GameObject LateBar;

        GameObject Bar2;
        GameObject Bar1;

        //float now = 100;
        float instantBarGauge = 1; // 표시되는bar 0 되면올려줘야한다.
        float goal = 100; // 슬로우바의최종목표. 음수도된다.
        float moveamount = 0;
        float addline = 0;

        //            sexybacklog.Console("goal " + goal + " next " + instantBarGauge + " now " + now + " moveamount " + moveamount);

        // Use this for initialization
        void Start()
        {
            Singleton<GameInput>.getInstance().noticeTouchPosition += onTouch;
            Hpbar = GameObject.Find("HPBar");

            LateBar = GameObject.Find("HPBar_SlowFill");

            Bar1 = GameObject.Find("HPBar_Fill1");
            Bar2 = GameObject.Find("HPBar_Fill2");

        }
        void onTouch(TapPoint tap)
        {
            instantBarGauge = Hpbar.GetComponent<UIProgressBar>().value - 0.25f;
            if (instantBarGauge <= 0)
            {
                instantBarGauge += 1f;
                //ChangeBar();
            }
            Hpbar.GetComponent<UIProgressBar>().value = instantBarGauge;

            // 표시되는 바의 목표와 속도 set
            goal -= 0.25f;            
            moveamount = goal - now;
        }
        bool flipflop = true;

        void ChangeBar()
        {
            Color newColor = Bar2.GetComponent<UISprite>().color;
            Bar1.GetComponent<UISprite>().color = newColor;

            newColor = Color.HSVToRGB(Random.Range(0, 1f), 1f, 1f);
            Bar2.GetComponent<UISprite>().color = newColor;
        }
        public float now {  get { return 99 - addline + LateBar.GetComponent<UISprite>().fillAmount; } }
        float velocity = 0.3333f; // 1일시 1초에 목적지까지 깎임. 기본 3초
        float accel = 0.00f;

        private void FixedUpdate()
        {
            if (goal >= now)
            {   // 멈춤과 리셋
                sexybacklog.Console("CHECK");
                LateBar.GetComponent<UISprite>().fillAmount = Hpbar.GetComponent<UIProgressBar>().value; // 정확한값은 업데이트를타는 HPbar
                goal = now;
                velocity = 0.3333f;
                return;
            }

            // 가속과 이동. 이벤트시 설정도는 goal과 moveamount 는 절대 바꾸지않는다.
            velocity += accel;
            float step = moveamount * velocity * Time.fixedDeltaTime;
            LateBar.GetComponent<UISprite>().fillAmount += step;

            // 표시안되는바 set
            if (LateBar.GetComponent<UISprite>().fillAmount <= 0)
            {
                LateBar.GetComponent<UISprite>().fillAmount = 1;
                addline += 1;
            }
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
