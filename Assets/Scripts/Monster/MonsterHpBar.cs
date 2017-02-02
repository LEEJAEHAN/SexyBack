using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MonsterHpBar
    {
        GameObject Hpbar;
        GameObject LateBar1;
        GameObject LateBar2;

        GameObject Bar2;
        GameObject Bar1;
        GameObject HPBar_Name;
        GameObject HPBar_Unit;
        GameObject HPBar_Count;

        float maxstack = 100; // 변수
        float goal = 100; // 슬로우바의최종목표. 음수도된다.
        float late = 100;
        int currentLstack = 0;
        int currentIstack = 0;
        int maxdigit = 0;

        float velocity = -0.3334f; // 이동속도. 1일시 1초에 목적지까지 깎임. 기본 3초
        //float accel = 0.01f;  // 이동가속도. 현재 사용하지않음.

        float InitHue = 1f;
        private bool Decreasing = false;

        public float IGauge
        {
            get { return Hpbar.GetComponent<UIProgressBar>().value; }
            set { Hpbar.GetComponent<UIProgressBar>().value = value; }
        }
        public float LGauge
        {
            get { return LateBar1.GetComponent<UISprite>().fillAmount; }
            set { LateBar1.GetComponent<UISprite>().fillAmount = value; }
        }

        public MonsterHpBar(MonsterManager manager)
        {
            manager.Action_BeginBattleEvent += FillNewBar;
            manager.Action_TargetMonsterChange += UpdateBar;

            Hpbar = ViewLoader.HPBar;
            LateBar1 = ViewLoader.HPBar_SlowFill1;
            LateBar2 = ViewLoader.HPBar_SlowFill2;
            Bar1 = ViewLoader.HPBar_Fill1;
            Bar2 = ViewLoader.HPBar_Fill2;
            HPBar_Name = ViewLoader.HPBar_Name;
            HPBar_Unit = ViewLoader.HPBar_Unit;
            HPBar_Count = ViewLoader.HPBar_Count;

            Hpbar.SetActive(false);
        }

        // Use this for initialization
        void FillNewBar(Monster sender)
        {
            sender.Action_StateChangeEvent = onTargetStateChange;
            // 123,456,123,456
            BigInteger hp = sender.MAXHP;
            maxdigit = 0;
            string floatstring = hp.toLeftDigitString(out maxdigit, 2, 4); // "12.3456"
            string unitstring = BigInteger.CalDigitOtherN(maxdigit, 3); // 10b
            float totalgauge = Convert.ToSingle(floatstring);
            // set monster info text

            maxstack = totalgauge;  // 게이지가 만땅일시 0가되버려서
            currentLstack = (int)(totalgauge);
            currentIstack = (int)(totalgauge);
            goal = maxstack;
            late = maxstack;

            Bar2.SetActive(true);
            Decreasing = false;
            setColor(Bar1, Bar2, (int)currentIstack); // max 100이면 99단 풀차지부터.
            setColor(LateBar1, LateBar2, (int)currentLstack);
            IGauge = maxstack - (int)maxstack;
            LGauge = maxstack - (int)maxstack;
            Bar2.GetComponent<UISprite>().fillAmount = 1;


            // set text
            HPBar_Count.GetComponent<UILabel>().text = "x" + currentIstack;
            HPBar_Unit.GetComponent<UILabel>().text = unitstring;
            HPBar_Name.GetComponent<UILabel>().text = sender.Name;

            // set Color
            InitHue = UnityEngine.Random.Range(0, 1f);
            Bar1.GetComponent<UISprite>().color = Color.HSVToRGB(InitHue, 1, 1);
            LateBar1.GetComponent<UISprite>().color = Color.HSVToRGB(InitHue, 1, 0.7f);

            //attach
            UpdateBar(sender);
            Hpbar.SetActive(false);
        }

        private void onTargetStateChange(string id, string stateid)
        {
            if (stateid == "Appear") //stateid == "Death" || 
                Hpbar.SetActive(false);
            else
                Hpbar.SetActive(true);
        }

        void UpdateBar(Monster monster)
        {
            if (goal <=0 )
            {
                IGauge = 0;
                currentIstack = 0;
                return;
            }

            float prevgoal = goal;
            string floatstring = monster.HP.toLeftDigitString(maxdigit, 4);

            // 표시되는 바의 목표와 속도 set
            goal = Convert.ToSingle(floatstring);
            if (prevgoal == goal)
                return;

            Decreasing = true;
            //moveamount = goal - late; /// 음수
            if (goal < 0)
                goal = 0; // 속도 지정 후에 골은 0으로만든다.

            int stackindex = (int)goal; // 골이 조금도 안깎이면. 정수부에서 게이지가 0이 될 가능성이있다.
            IGauge = goal - stackindex;
            if (stackindex < currentIstack)
            {
                currentIstack = stackindex;
                setColor(Bar1, Bar2, (int)goal); // 나중에 점프카운트 바꿔야한다.
                HPBar_Count.GetComponent<UILabel>().text = "x" + currentIstack;
            }
            if (currentIstack == 0)
                Bar2.SetActive(false);
            //sexybacklog.Console(goal + " currstack:" + currentIstack);
        }

        private void setColor(GameObject current, GameObject next, int index)
        {
            float hue = 0;
            float nexthue = 0;
            float saturation = 0;
            float brightness = 0;
            Color currColor = current.GetComponent<UISprite>().color;
            Color.RGBToHSV(currColor, out hue, out saturation, out brightness);

            hue = InitHue + (0.7f) * index;
            nexthue = hue + (0.7f);
            hue -= (int)hue;
            nexthue -= (int)nexthue;

            current.GetComponent<UISprite>().color = Color.HSVToRGB(hue, saturation, brightness);
            next.GetComponent<UISprite>().color = Color.HSVToRGB(nexthue, saturation, brightness);
        }

        public void FixedUpdate()
        {
            if (!Decreasing)
                return;
            // 가속과 이동. 이벤트시 설정도는 goal과 moveamount 는 절대 바꾸지않는다.
            //velocity += accel;
            float vel = velocity * (float)Math.Pow((currentLstack - currentIstack + 1), 2);
            if (vel > velocity)
                vel = velocity;

            float step = vel * Time.fixedDeltaTime; // 이동은하고있다.
            late += step;

            DisplayLateGaugeBar(late, goal);

            if (goal >= late)
            {   // 멈춤과 리셋
                LateBar1.GetComponent<UISprite>().fillAmount = Hpbar.GetComponent<UIProgressBar>().value; // 정확한값은 업데이트를타는 HPbar
                late = goal;
                vel = velocity;
                Decreasing = false;
            }
            if (late <= 0.001f) // TODO : 이부분이 젤찜찜함
            {   // detach
                late = 0;
                Decreasing = false;
                Hpbar.SetActive(false);
            }
        }

        void DisplayLateGaugeBar(float late, float goal)
        {
            int stackindex = (int)late;
            float fillAmount = late - (int)late;

            if (stackindex < currentLstack)
            {
                currentLstack = stackindex;
                setColor(LateBar1, LateBar2, stackindex);
            }

            if (currentIstack < currentLstack) // 1차원이상차이날때만 중첩late바를 출력한다.
                LateBar2.SetActive(true);
            else
                LateBar2.SetActive(false);

            LateBar1.GetComponent<UISprite>().fillAmount = fillAmount;
        }

    }
}