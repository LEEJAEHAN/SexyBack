using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class GameInput : IDisposable
    {
        ~GameInput()
        {
            sexybacklog.Console("GameInput 소멸");
        }
        public delegate void Touch_Event(TapPoint tap);
        public event Touch_Event Action_TouchEvent = delegate { }; // back viewport 에서의 터치포지션

        Vector3 mouseinputpoint;
        Ray ray;
        RaycastHit hit;
        RaycastHit effecthit;

        public int fowardtimefordebug = 0;

        public void Dispose()
        {
            Action_TouchEvent = null;
        }

        void TouchGameScreen(Vector3 position)
        {
            mouseinputpoint = position;

            if (UICamera.Raycast(position))
                return;

//            sexybacklog.Console(mouseinputpoint);

            ray = GameCameras.EffectCamera.ScreenPointToRay(mouseinputpoint);

            Physics.Raycast(ray, out effecthit, 100, 1000000000); // 이펙트영역  1<<9

            ray = GameCameras.HeroCamera.ScreenPointToRay(mouseinputpoint);

            if (Physics.Raycast(ray, out hit, 100, 0100000000)) // 게임영역 1<< 8
            {
                TapPoint pos = new TapPoint(hit.point, effecthit.point, UICamera.lastEventPosition);
                Action_TouchEvent(pos);
            }
        }

        public void CheckInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TouchGameScreen(Input.mousePosition);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                Singleton<LevelUpManager>.getInstance().BuySelected();
            }
            if (Input.GetKey(KeyCode.Space))
            {
                TouchGameScreen(new Vector3(360,800,0));
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                Singleton<StatManager>.getInstance().Upgrade(new Bonus("player", "ResearchTimeX", 2, ""), new GridItemIcon());
            }
            if(Input.GetKey(KeyCode.M))
            {
                Singleton<StatManager>.getInstance().ExpGain(new BigInteger(new BigIntExpression(100, "Z")), false);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                bool result = false;
                result = Singleton<MonsterManager>.getInstance().TestDeal(Singleton<HeroManager>.getInstance().GetHero().DPC);
                result = Singleton<MonsterManager>.getInstance().TestDeal(Singleton<ElementalManager>.getInstance().GetTotalDps() * 5);

                //Singleton<StatManager>.getInstance().ExpGain(Singleton<HeroManager>.getInstance().GetHero().DPC);
                //Singleton<StatManager>.getInstance().ExpGain(Singleton<ElementalManager>.getInstance().GetTotalDps() * 5);
                if(result)
                    fowardtimefordebug +=5;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
            }

        }

    }
    internal struct TapPoint
    {
        public Vector3 PosInHeroCam;  // world unit, default 메인 캠에서 본 위치
        public Vector3 PosInEffectCam;   // world unit, effect cam position 에서 본 위치
        public Vector3 UiPos;   // pixel unit, 1280,720 기준의 좌표이다.

        internal TapPoint(Vector3 gamepos, Vector3 effectpos, Vector3 uipos)
        {
            PosInHeroCam = gamepos;
            PosInEffectCam = effectpos;
            UiPos = uipos;
        }
    }
}