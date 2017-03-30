using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MonsterHpBar : IDisposable
    {
        ~MonsterHpBar()
        {
            sexybacklog.Console("MonsterHpBar 소멸");
        }
        public void Dispose()
        {
            Hpbar = null;
            LateBar1 = null;
            LateBar2 = null;
            Bar2 = null;
            Bar1 = null;
            HPBar_Name = null;
            HPBar_Unit = null;
        }

        GameObject Hpbar;
        GameObject LateBar1;
        GameObject LateBar2;

        GameObject Bar2;
        GameObject Bar1;
        GameObject HPBar_Name;
        GameObject HPBar_Unit;
//        GameObject HPBar_Count;

        float goal = 100; // 슬로우바의최종목표. 음수도된다.
        float late = 100;
        int MaxHpDigit = 0;
        float MaxHp;
        float Hp;
        int currentLstack = 1;
        int currentIstack = 1;


        float velocity = -0.3334f; // 이동속도. 1일시 1초에 목적지까지 깎임. 기본 3초
        //float accel = 0.01f;  // 이동가속도. 현재 사용하지않음.

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

        public MonsterHpBar()
        {
            Hpbar = GameObject.Find("HPBar");
            LateBar1 = GameObject.Find("HPBar_SlowFill1");
            LateBar2 = GameObject.Find("HPBar_SlowFill2");
            Bar1 = GameObject.Find("HPBar_Fill1");
            Bar2 = GameObject.Find("HPBar_Fill2");
            HPBar_Name = GameObject.Find("HPBar_Name");
            HPBar_Unit = GameObject.Find("HPBar_Unit");
            Hpbar.SetActive(false);
        }

        // Use this for initialization
        public void FillNewBar(string BarTitle, Monster monster)
        {
            monster.StateMachine.Action_changeEvent += onTargetStateChange;
            // 123,456,123,456
            string maxhpstring = monster.MAXHP.ToStringAndMakeDigit(out MaxHpDigit, 3, 2); // 123.45
            if (!Single.TryParse(maxhpstring, out MaxHp))
                sexybacklog.Error("HPBAR Error" + MaxHp);

            string hpstring = monster.HP.ToStringAsDigit(MaxHpDigit, 2);
            if (!Single.TryParse(hpstring, out Hp))
                sexybacklog.Error("HPBAR Error" + Hp);

            // fill gauge
            goal = (Hp / MaxHp * 10) - 0.001f; // // 9.999f // 정수일경우. .0일시 게이지가 바닥으로시작해버린다. -0.001 ~ 9.999f;
            late = goal; // 9.999f
            currentLstack = (int)(goal) + 1;
            currentIstack = (int)(goal) + 1;
            DisplayLateGaugeBar(late, goal);
            IGauge = goal - (int)goal;// 0 ~ 0.9999f
            LGauge = late - (int)late;

            if (currentIstack > 1)
            {
                Bar2.GetComponent<UISprite>().fillAmount = 1;
                Bar2.SetActive(true);
            }
            Decreasing = false;

            // set text
            HPBar_Unit.GetComponent<UILabel>().text = monster.HP.ToCommaString() + " (" + GetPercent().ToString() + "%)";
            HPBar_Name.GetComponent<UILabel>().text = BarTitle;

            //            HPBar_Count.GetComponent<UILabel>().text = "x" + currentIstack;

            // set Color
            setColor(Bar1, Bar2, currentIstack, 1); // max 100이면 99단 풀차지부터.
            setColor(LateBar1, LateBar2, currentLstack, 0.7f);
            
            //attach
            Hpbar.SetActive(false);
        }

        private void onTargetStateChange(string id, string stateid)
        {
            if (stateid == "Appear" || stateid == "Death") //stateid == "Death" || 
                Hpbar.SetActive(false);
            else if (stateid == "Ready" || stateid == "Flying")
                Hpbar.SetActive(true);
        }

        public void UpdateBar(Monster monster)
        {
            if (goal < 0)
            {
                IGauge = 0;
                currentIstack = 1;
                return;
            }

            float prevgoal = goal; // 9.999f
            string hpstring = monster.HP.ToStringAsDigit(MaxHpDigit, 2);
            if (!Single.TryParse(hpstring, out Hp))
                sexybacklog.Error("HPBAR Error" + Hp);

            goal = (Hp / MaxHp * 10) - 0.001f; // -0.001 ~ 9.999f;

            // 표시되는 바의 목표와 속도 set

            if (prevgoal == goal)
                return;

            Decreasing = true;
            HPBar_Unit.GetComponent<UILabel>().text = monster.HP.ToCommaString() + " (" + GetPercent().ToString() + "%)";

            //moveamount = goal - late; /// 음수
            if (goal < 0) // <= -0.001f 와 같다.
                goal = -0.001f; // 속도 지정 후에 골은 0으로만든다.

            int stackindex = (int)goal + 1; // 골이 조금도 안깎이면. 정수부에서 게이지가 0이 될 가능성이있다.
            IGauge = goal - (int)goal;
            if (stackindex < currentIstack)
            {
                currentIstack = stackindex;
                setColor(Bar1, Bar2, stackindex, 1); // 나중에 점프카운트 바꿔야한다.
                //HPBar_Count.GetComponent<UILabel>().text = "x" + stackindex;
            }
            if (currentIstack == 1)
                Bar2.SetActive(false);

        }

        private void setColor(GameObject current, GameObject next, int index, float brightness)
        {
            // index가 1일때 레드. hue = 0

            // 10의 색깔.
            float HueUnit = 60f / 360f; // = UnityEngine.Random.Range(0, 1f);

            float hue = 0 + HueUnit * (index - 1);
            float nexthue = 0 + HueUnit * (index - 2);
            hue -= (int)hue;
            nexthue -= (int)nexthue;

            current.GetComponent<UISprite>().color = Color.HSVToRGB(hue, 1, brightness);
            next.GetComponent<UISprite>().color = Color.HSVToRGB(nexthue, 1, brightness);
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
                LGauge = Hpbar.GetComponent<UIProgressBar>().value; // 정확한값은 업데이트를타는 HPbar
                late = goal;
                vel = velocity;
                Decreasing = false;
            }
            if (late <= 0f) // TODO : 이부분이 젤찜찜함
            {   // detach
                late = -0.0001f;
                Decreasing = false;
                Hpbar.SetActive(false);
            }
        }

        void DisplayLateGaugeBar(float late, float goal)
        {
            int stackindex = (int)late + 1;

            if (stackindex < currentLstack)
            {
                currentLstack = stackindex;
                setColor(LateBar1, LateBar2, stackindex, 0.7f);
            }

            if (currentIstack < currentLstack) // 1차원이상차이날때만 중첩late바를 출력한다.
                LateBar2.SetActive(true);
            else
                LateBar2.SetActive(false);

            LGauge = late - (int)late;
        }

        public int GetPercent()
        {
            return (int)Math.Ceiling(goal * 10f);
        }
    }
}