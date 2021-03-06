﻿using System;
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

            //tranform의 위치를 바로 설정할땐 OverlayPosition 메서드를 쓰면되지만
            //터치의경우 터치했을당시의 카메라 설정에 의한 postion을 기억해놔야하기때문에, ray를써서구했다.
            ray = GameCameras.EffectCamera.ScreenPointToRay(mouseinputpoint);
            Physics.Raycast(ray, out effecthit, 100, 1000000000); // 이펙트영역  1<<9
            ray = GameCameras.HeroCamera.ScreenPointToRay(mouseinputpoint);
            if (Physics.Raycast(ray, out hit, 100, 0100000000)) // 게임영역 1<< 8
            {
                TapPoint pos = new TapPoint(hit.point, effecthit.point);
                //TapPoint pos = new TapPoint(hit.point, effecthit.point, UICamera.lastEventPosition);
                Action_TouchEvent(pos);
            }
        }

        // 치트
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
                Singleton<PlayerStatus>.getInstance().ApplySpecialStat(new BonusStat("util", Attribute.ResearchTimeX, 2), true);
                EffectController.getInstance.AddBuffEffect(new NestedIcon("Icon_19", "치트속도2배", "IconSmall_02"));
            }
            if(Input.GetKey(KeyCode.M))
            {
                Singleton<InstanceStatus>.getInstance().ExpGain(new BigInteger(new BigIntExpression(100, "F")), false);
            }
            if (Input.GetKey(KeyCode.S))
            {
                Singleton<InstanceGameManager>.getInstance().SaveInstance(); // 인스턴스세이브
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                bool result = false;
                bool temp;
                result = Singleton<MonsterManager>.getInstance().TestDeal(Singleton<HeroManager>.getInstance().GetHero().DPC);
                result = Singleton<MonsterManager>.getInstance().TestDeal(Singleton<ElementalManager>.getInstance().GetTotalDps(out temp) * 5);
                if (result)
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
        public Vector3 PosInEffectCam;   // 터치찍었을당시의 HeroCam기준의 world unit을, effect cam position 으로 변환한값.
                                         //public Vector3 UiPos;   // pixel unit, 1280,720 기준의 좌표이다.

        internal TapPoint(Vector3 gamepos, Vector3 effectpos)
        {
            PosInHeroCam = gamepos;
            PosInEffectCam = effectpos;
            //UiPos = uipos;
        }
    }
}