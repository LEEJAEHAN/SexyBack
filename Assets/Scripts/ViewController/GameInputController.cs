using UnityEngine;
using System.Collections;

namespace SexyBackPlayScene
{
    public class GameInputController : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnTapDown()
        {
            // publish
            Singleton<HeroManager>.getInstance().OnKeyEvent();
        }
    }

}