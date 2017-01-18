using UnityEngine;

namespace SexyBackPlayScene
{
    public class DebugPanel : MonoBehaviour
    {

        Vector3 mouseinputpoint;
        Ray ray;
        RaycastHit hit;

        Ray uiray;
        RaycastHit uihit;

        Vector3 moveDirection = new Vector3(0, 1, -3) - new Vector3(0, 0, -10); // 문 - 초기카메라
        //Vector3 moveDirection = Singleton<MonsterManager>.getInstance().GetMonster().Position - new Vector3(0,0,-10); // 몹 - 초기카메라
        //  Vector3 moveDirection = new Vector3(0,3,-1) - new Vector3(0, 0, -10); // 문 - 초기카메라
        // TODO: fixed update 에대해좀더알아야할듯;

        // Use this for initialization
        void Start()
        {

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawRay(ray.origin, ray.direction * 20);
            Gizmos.DrawSphere(hit.point, 0.1f);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(uiray.origin, uiray.direction * 20);
            Gizmos.DrawSphere(uihit.point, 0.05f);

        }
        // Update is called once per frame
        void Update()
        {


            if (Input.GetMouseButtonDown(0))
            {
                mouseinputpoint = Input.mousePosition;

                uiray = UICamera.currentCamera.ScreenPointToRay(mouseinputpoint);
                
                ray = Camera.main.ScreenPointToRay(mouseinputpoint);
                
                if (Physics.Raycast(uiray, out uihit))
                {
                    //sexybacklog.Console("Hit UIArea  : " + uihit.collider.gameObject.name + uihit.point);
                }

                if (Physics.Raycast(ray, out hit))
                {
                    //sexybacklog.Console("Hit GameArea : " + hit.collider.gameObject.name + hit.point);
                }

            }
        }
        private void FixedUpdate()
        {
            FowardCamera();
        }
        void FowardCamera()
        {
            float speed = 0.3f * Time.deltaTime;

            Camera.main.transform.position += (moveDirection * speed);
            ViewLoader.hero.transform.position += (moveDirection * speed);

            if (Camera.main.transform.position.z > -5)
            {
                moveDirection = -moveDirection;
            }

            if(Camera.main.transform.position.z < -10)
            {
                moveDirection = -moveDirection;
            }
        }
    }


}
