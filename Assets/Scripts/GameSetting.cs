using UnityEngine;
using System.Collections;

namespace SexyBackPlayScene
{
    public static class GameSetting
    {
        public static float orthographicSize = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize; // 6.4
        public static float fieldOfView = GameObject.Find("Main Camera").GetComponent<Camera>().fieldOfView; // 64
        public static float aspect = GameObject.Find("Main Camera").GetComponent<Camera>().aspect; // 0.5625
        public static float pixelPerUnit = 100;

        // game size is -3.6, -6.4 to 3.6 , 6.4  단위:unit // pivot is center 
        // ngui size is 0,0 to 720, 1280 단위:pixel // pivot is left down

        // step1 : nguiposition /= pixerperunit(==100)
        // spet2 :  x -= (aspect * orthographicSize)(==3.2) , y -= orthographicSize(==6.4) (field( x -= 3.6, y -= 6.4 )
    }
}