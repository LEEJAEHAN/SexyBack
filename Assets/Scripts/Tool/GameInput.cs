using UnityEngine;

namespace SexyBackPlayScene
{
    internal class GameInput
    {
        public delegate void Touch_Event(TapPoint tap);
        public event Touch_Event Action_TouchEvent = delegate { }; // back viewport 에서의 터치포지션

        Vector3 mouseinputpoint;
        Ray ray;
        RaycastHit hit;
        RaycastHit effecthit;

        public void CheckInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseinputpoint = Input.mousePosition;

                if (UICamera.Raycast(Input.mousePosition))
                    return;

                ray = ViewLoader.EffectCamera.ScreenPointToRay(mouseinputpoint);

                Physics.Raycast(ray, out effecthit, 100, 1000000000); // 이펙트영역  1<<9

                ray = ViewLoader.HeroCamera.ScreenPointToRay(mouseinputpoint);

                if (Physics.Raycast(ray, out hit, 100, 0100000000)) // 게임영역 1<< 8
                {
                    TapPoint pos = new TapPoint(hit.point, effecthit.point, UICamera.lastEventPosition);
                    Action_TouchEvent(pos);
                }
            }

            if (Input.GetKey(KeyCode.Space))
            {
                Singleton<LevelUpManager>.getInstance().BuySelected();
            }

        }
        void CalculateEffectPoint(Vector3 position)
        {
            //ray = ViewLoader.camera.sc
        }

        


    }
    internal struct TapPoint
    {
        public Vector3 GamePos;  // world unit
        public Vector3 EffectPos;   // world unit
        public Vector3 UiPos;   // pixel unit

        internal TapPoint(Vector3 gamepos, Vector3 effectpos, Vector3 uipos)
        {
            GamePos = gamepos;
            EffectPos = effectpos;
            UiPos = uipos;
        }
    }
}