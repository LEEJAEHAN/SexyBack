using UnityEngine;
using System.Collections;
using System;

namespace SexyBackPlayScene
{
    public class GameCameras
    {
        // gamescale setting;
        public static Camera HeroCamera;
        public static Camera EffectCamera;
        public static Camera UICamera;

        //static float orthographicSize;
        //static float fieldOfView;// 64
        //static float pixelPerUnit; // 100
        public static Vector3 HeroCamPosition;
        public static Vector3 EyeLine = new Vector3(0, 1.5f, 10);
        public static Vector3 ECamPosition;

        static float GameAspect; // 0.5625
        static readonly float GameWidth = 720;
        static readonly float GameHeight = 1280;
        static float ScreenWidth = Screen.width;
        static float ScreenHeight = Screen.height;

        static float shrinkScreenWidth { get { return ScreenHeight * GameAspect; } } // 세로를 기준으로 가로가 줄어들때 가로길이.
        static float shrinkScreenHeight { get { return ScreenWidth / GameAspect; } } // 가로를 기준으로 세로가 줄어들때 세로길이.
        static float UIScaleWidth { get { return GameWidth / ScreenWidth; } }
        static float UIScaleHeight { get { return GameHeight / ScreenHeight; } }

        public static bool WideScreen = true;

        static float ViewPortWidthShift;
        static float ViewPortHeightShift;
        internal static bool DamageFontFlag = true;

        public GameCameras()
        {
            InitCamera();
            SetResolution();

            HeroCamPosition = HeroCamera.transform.position;
            ECamPosition = EffectCamera.transform.position;
            // == defaultMonster getPosition 를 effect camera의 z까지만 사영한다.
        }
        private void SetResolution()
        {
            ScreenWidth = Screen.width;
            ScreenHeight = Screen.height;

            Rect shrinkScreen = HeroCamera.rect;
            if (ScreenWidth / ScreenHeight >= GameAspect ) // 가로가 넓다. 1280을 세로길이까지 맞춤. 가로는 계산값으로, 가로여백 남김.
            {
                WideScreen = true;
                // 카메라는 좌 우 검은여백만큼 을 제한 중앙만 그린다. 즉 카메라가 비추는 가로 뷰포트가 좁아짐
                float ViewPortWidth = shrinkScreenWidth / ScreenWidth;
                ViewPortWidthShift = (1 - ViewPortWidth) / 2;
                shrinkScreen = new Rect(ViewPortWidthShift, 0, ViewPortWidth, 1);
            }
            else if (ScreenWidth / ScreenHeight < GameAspect) // 세로로 길다. 720을 가로넓이까지 맞춤, 세로는 계산값으로, 세로여백 남김.
            {
                WideScreen = false;
                // 카메라는 상 하 검은여백만큼 을 제한 중앙만 그린다. 즉 카메라가 비추는 세로 뷰포트가 좁아짐
                float ViewPortHeight = shrinkScreenHeight / ScreenHeight;
                ViewPortHeightShift = (1 - ViewPortHeight) / 2;
                shrinkScreen = new Rect(0, ViewPortHeightShift, 1, ViewPortHeight);
            }

            HeroCamera.rect = shrinkScreen;
            EffectCamera.rect = shrinkScreen;
            UICamera.rect = shrinkScreen;
        }
        private void InitCamera()
        {
            HeroCamera = GameObject.Find("HeroCamera").GetComponent<Camera>();
            EffectCamera = GameObject.Find("EffectCamera").GetComponent<Camera>();
            UICamera = GameObject.Find("UICamera").GetComponent<Camera>();

            //orthographicSize = HeroCamera.orthographicSize; // 6.4
            //fieldOfView = HeroCamera.GetComponent<Camera>().fieldOfView; // 64
            //pixelPerUnit = 100;

            GameAspect = GameWidth / GameHeight;// 
        }

        internal static Vector3 ScreenPosToUIPos(Vector3 screenposition)
        {
            Vector3 result = new Vector3();
            if (WideScreen)
            {
                float shiftx = ScreenWidth * ViewPortWidthShift; // == (ScreenWidth - shrinkScreenWidth) / 2; 
                float x = screenposition.x - shiftx; // shift , 왼쪽으로 붙임.
                float y = screenposition.y;
                x *= UIScaleHeight; // ui의 세로크기가 1로, ui세로크기 증가가 곧 ui배율;
                y *= UIScaleHeight;

                result = new Vector3(x, y, screenposition.z);
            }
            else if(!WideScreen)
            {
                float shifty = ScreenHeight * ViewPortHeightShift; // == (ScreenHeight - shrinkScreenHeight) / 2;
                float x = screenposition.x; // shift , 왼쪽으로 붙임.
                float y = screenposition.y - shifty;
                x *= UIScaleWidth; // ui의 가로크기가 1로, ui가로크기 증가가 곧 ui배율;
                y *= UIScaleWidth;

                result = new Vector3(x, y, screenposition.z);
            }
            return result;
        }
    }
}