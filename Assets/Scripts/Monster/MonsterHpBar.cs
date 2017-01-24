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

        MonsterManager manager;

        float maxstack = 100; // 변수
        float goal = 100; // 슬로우바의최종목표. 음수도된다.
        float late = 100;
        float currentLstack = 0;
        float currentIstack = 0;

        float moveamount = 0; // 게이지 이동량
        float velocity = 0.5f; // 이동속도. 1일시 1초에 목적지까지 깎임. 기본 3초
        float accel = 0.00f;  // 이동가속도.

        float InitHue = 1f;

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
            manager.noticeMonsterCreate += onMonsterCreate;
            manager.noticeMonsterChange += onMonsterChange;

            currentLstack = maxstack - 1;
            currentIstack = maxstack - 1;
            goal = maxstack;
            late = maxstack;

            Hpbar = ViewLoader.HPBar;
            LateBar1 = ViewLoader.HPBar_SlowFill1;
            LateBar2 = ViewLoader.HPBar_SlowFill2;
            Bar1 = ViewLoader.HPBar_Fill1;
            Bar2 = ViewLoader.HPBar_Fill2;
            HPBar_Name = ViewLoader.HPBar_Name;
            HPBar_Unit = ViewLoader.HPBar_Unit;
            HPBar_Count = ViewLoader.HPBar_Count;

            BigInteger a = 10;
            
        }

        // Use this for initialization
        void onMonsterCreate(Monster sender)
        {
            Hpbar.SetActive(true);

            InitHue = UnityEngine.Random.Range(0, 1f);
            Bar1.GetComponent<UISprite>().color = Color.HSVToRGB(InitHue, 1, 1);
            LateBar1.GetComponent<UISprite>().color = Color.HSVToRGB(InitHue, 1, 0.8f);

            setColor(Bar1, Bar2, (int)currentIstack); // max 100이면 99단 풀차지부터.
            setColor(LateBar1, LateBar2, (int)currentLstack);
            IGauge = 1;
            LGauge = 1;
        }

        void onMonsterChange(Monster monster)
        {
            // 표시되는 바의 목표와 속도 set
            goal -= 0.25f;
            moveamount = goal - late;

            int stackindex = (int)goal;
            IGauge = goal - stackindex;
            if (stackindex < currentIstack)
            {
                currentIstack = stackindex;
                setColor(Bar1, Bar2, (int)goal); // 나중에 점프카운트 바꿔야한다.
            }
        }

        private void setColor(GameObject current, GameObject next, int index)
        {
            float hue = 0;
            float saturation = 0;
            float brightness = 0;
            Color currColor = current.GetComponent<UISprite>().color;
            Color.RGBToHSV(currColor, out hue, out saturation, out brightness);

            hue = InitHue + (0.13f) * index;
            hue = hue - (int)hue;

            current.GetComponent<UISprite>().color = Color.HSVToRGB(hue, saturation, brightness);
            next.GetComponent<UISprite>().color = Color.HSVToRGB(hue + 0.07f, saturation, brightness);
        }

        public void FixedUpdate()
        {
            // 가속과 이동. 이벤트시 설정도는 goal과 moveamount 는 절대 바꾸지않는다.
            velocity += accel;
            float step = moveamount * velocity * Time.fixedDeltaTime; // 이동은하고있다.

            late += step;
            DisplayLateGaugeBar(late, goal);

            if (goal >= late)
            {   // 멈춤과 리셋
                LateBar1.GetComponent<UISprite>().fillAmount = Hpbar.GetComponent<UIProgressBar>().value; // 정확한값은 업데이트를타는 HPbar
                late = goal;
                velocity = 0.5f;
                return;
            }
        }

        void DisplayLateGaugeBar(float late, float goal)
        {
            int stackindex = (int)late;
            float fillAmount = late - (int)late;
            int goalindex = (int)goal;

            if (stackindex < currentLstack)
            {
                currentLstack = stackindex;
                setColor(LateBar1, LateBar2, stackindex);
            }

            if (currentIstack < currentLstack) // 1차원이상차이날때만 중첩late바를 출력한다.
            {
                LateBar2.SetActive(true);
            }
            else
            {
                LateBar2.SetActive(false);
            }
            LateBar1.GetComponent<UISprite>().fillAmount = fillAmount;
        }

    }
}