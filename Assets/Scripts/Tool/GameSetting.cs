using UnityEngine;
using System.Collections;
using System;

namespace SexyBackPlayScene
{
    public class GameSetting
    {
        public static float orthographicSize;
        public static float fieldOfView;// 64
        public static float aspect; // 0.5625
        public static float pixelPerUnit;
        public static Vector3 defaultHeroPosition;
        public static Vector3 EyeLine = new Vector3(0, 1.5f, 10);
        public static Vector3 ECamPosition;

        public readonly float GameWidth = 720;
        public readonly float GameHeight = 1280;
        public float ScreenWidth = Screen.width;
        public float ScreenHeight = Screen.height;

        public void Init()
        {
            SetCameraSetting();
            RemoveTestObject();

            defaultHeroPosition = ViewLoader.HeroPanel.transform.position;
            ECamPosition = ViewLoader.EffectCamera.transform.position;
            // == defaultMonster getPosition 를 effect camera의 z까지만 사영한다.
        }

        private void SetCameraSetting()
        {
            Camera camera = ViewLoader.HeroCamera;
            orthographicSize = camera.orthographicSize; // 6.4
            fieldOfView = camera.GetComponent<Camera>().fieldOfView; // 64
            aspect = camera.aspect; // 0.5625
            pixelPerUnit = 100;
            // TODO : 해상도에 따른 카메라 ViewPort SIZE변화 필요 ( 좀 더 다듬고, ui도 생각해야함)
            // view Port shirink
            ScreenWidth = Screen.width;
            ScreenHeight = Screen.height;
            float ViewPortWidth = (GameWidth * ScreenHeight) / (GameHeight * ScreenWidth);
            float shift = (1 - ViewPortWidth) / 2;
            Rect shrinkScreen = new Rect(shift, 0, ViewPortWidth, 1);
            camera.rect = shrinkScreen;
        }

        internal void RemoveTestObject()
        {
            // remove useless objects; like prefab preview
            ViewLoader.shooter.transform.DestroyChildren();
            ViewLoader.monsterbucket.transform.DestroyChildren();
            ViewLoader.StagePanel.transform.DestroyChildren();
            ViewLoader.Bar_Attack.transform.DestroyChildren();
        }
    }
}