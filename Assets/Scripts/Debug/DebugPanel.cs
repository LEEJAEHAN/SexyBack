using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class TestRate
    {
        internal int density = 10;
        internal bool abs;
        internal string context;
        public TestRate(int d, string c, bool abs)
        {
            density = d;
            context = c;
            this.abs = abs;
        }
        public void Print()
        {
            sexybacklog.Console(context);
        }
    }
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
        List<TestRate> abc;
        int totalcount;
        private void Awake()
        {
            TestRate a = new TestRate(30, "30first", false);
            TestRate b = new TestRate(30, "30second", false);
            TestRate c = new TestRate(30, "30third", false);
            TestRate d = new TestRate(10, "abs10", true);
            TestRate e = new TestRate(30, "abs30", true);

            abc = new List<TestRate>();
            abc.Add(a);
            abc.Add(b);
            abc.Add(c);
            abc.Add(d);
            abc.Add(e);

        }
        void Update()
        {
            //sexybacklog.InGame(Singleton<MonsterManager>.getInstance().GetMonster().HP.ToString());

            //Vector3 WallMoveVector = GameSetting.defaultHeroPosition - GameSetting.ECamPosition;
            //GameObject.Find("back_image").transform.position += WallMoveVector * Time.deltaTime * 0.2f;
            //GameObject.Find("back_image2").transform.position += WallMoveVector * Time.deltaTime * 0.2f;

            //TestRate result = PickRandomOne(abc);
            //if (result == null)
            //    sexybacklog.Console("nonepick.bug");
            //else
            //    result.Print();
            //totalcount++;

            if (flipflop)
            {
                //PickRandomOne;


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

        private TestRate PickRandomOne(List<TestRate> abc)
        {
            TestRate AbsResult = CheckAbsouluteRate(abc);
            if (AbsResult == null)
                return CheckRelativeRate(abc);
            return AbsResult;
        }

        private TestRate CheckAbsouluteRate(List<TestRate> abc)
        {
            int rand = UnityEngine.Random.Range(0, 100);
            foreach (TestRate one in abc)
            {
                if (one.abs == true)
                {
                    rand -= one.density;
                    if (rand < 0)
                        return one;
                }
            }
            return null;
        }

        private TestRate CheckRelativeRate(List<TestRate> abc)
        {
            int sumDensity = GetTotalDensity(abc);
            int rand = UnityEngine.Random.Range(0, sumDensity);

            foreach (TestRate one in abc)
            {
                if (one.abs == false)
                {
                    rand -= one.density;
                    if (rand < 0)
                        return one;
                }
            }
            return null;
        }

        private int GetTotalDensity(List<TestRate> abc)
        {
            int result = 0;
            foreach (TestRate one in abc)
            {
                if (one.abs == false)
                    result += one.density;
            }
            return result;
        }
    }


}
