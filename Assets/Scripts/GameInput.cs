using UnityEngine;

namespace SexyBackPlayScene
{
    internal class GameInput
    {
        public delegate void TouchEvent(Vector3 touchPoint);
        public event TouchEvent noticeTouch;

        Vector3 mouseinputpoint;
        Ray ray;
        RaycastHit hit;
        public void CheckInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseinputpoint = Input.mousePosition;

                ray = UICamera.currentCamera.ScreenPointToRay(mouseinputpoint);
                if (Physics.Raycast(ray, out hit)) // ui 영역
                    return;

                ray = Camera.main.ScreenPointToRay(mouseinputpoint);
                if (Physics.Raycast(ray, out hit)) // 게임 영역
                {
                    noticeTouch(hit.point);
                }
            }
        }

    }
}