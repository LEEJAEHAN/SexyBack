using UnityEngine;

namespace SexyBackPlayScene
{
    internal class GameInput
    {
        public delegate void TouchPositionEvent(TapPoint tap);
        public event TouchPositionEvent noticeTouchPosition; // back viewport 에서의 터치포지션

        Vector3 mouseinputpoint;
        Ray ray;
        RaycastHit uihit;
        RaycastHit hit;
        RaycastHit effecthit;

        public void CheckInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseinputpoint = Input.mousePosition;

                ray = UICamera.currentCamera.ScreenPointToRay(mouseinputpoint);
                if (Physics.Raycast(ray, out uihit)) // ui 영역
                    return;

                ray = ViewLoader.camera.ScreenPointToRay(mouseinputpoint);

                if (Physics.Raycast(ray, out hit, 100, 0100000000)
                    &&
                    Physics.Raycast(ray, out effecthit, 100, 1000000000)) // 게임영역  1<<8
                {
                    noticeTouchPosition(new TapPoint(hit.point, effecthit.point));
                }
            }
        }

        void CalculateEffectPoint(Vector3 position)
        {
            //              ray = ViewLoader.camera.sc
        }

    }
    internal struct TapPoint
    {
        public Vector3 GamePos;
        public Vector3 UiPos;
        internal TapPoint(Vector3 gamepos, Vector3 uipos)
        {
            GamePos = gamepos;
            UiPos = uipos;
        }
    }
}