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
        public static Vector3 defaultMonsterPosition;
        public static Vector3 defaultHeroPosition;
        public static Vector3 defaultCameraPosition;

        public void Init()
        {
            Camera camera = ViewLoader.camera;
            orthographicSize = camera.orthographicSize; // 6.4
            fieldOfView = camera.GetComponent<Camera>().fieldOfView; // 64
            aspect = camera.aspect; // 0.5625
            pixelPerUnit = 100;
            defaultMonsterPosition = ViewLoader.monsters.transform.position;
            defaultHeroPosition = ViewLoader.hero.transform.position;
            defaultCameraPosition = camera.transform.position;
        }
        
        internal void RemoveTestObject()
        {
            // remove useless objects; like prefab preview
            ViewLoader.Item_Enable.transform.DestroyChildren();
            ViewLoader.shooter.transform.DestroyChildren();
            ViewLoader.monsters.transform.DestroyChildren();
        }
    }
}