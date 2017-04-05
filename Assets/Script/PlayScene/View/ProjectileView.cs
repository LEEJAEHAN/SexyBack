using UnityEngine;
using System.Collections;

namespace SexyBackPlayScene
{
    public class ProjectileView : MonoBehaviour // view와 동일하다
    {
        // projectile data
        // animation info

        // TODO: statemachine을 view가 가진다 ㅠㅠ.. 이것도 리팩토링해야함.
        Animator anim;
        public bool floorOnly;
        public int Timeout = 5;
        public double TimeoutTimer = 0;

        void Awake()
        {
            anim = gameObject.GetComponent<Animator>();
        }

        private void Update()
        {            
        }
        void FixedUpdate()
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base.Destroy"))
            {
                Destroy(this.gameObject);
                return;
            }
            if (anim.GetBool("Shoot") == true)
            {
                float xVec = GetComponent<Rigidbody>().velocity.x;
                float yVec = GetComponent<Rigidbody>().velocity.y;
                
                float rot = UnityEngine.Mathf.Atan2(yVec, xVec) * UnityEngine.Mathf.Rad2Deg;
                //GameManager.SexyBackDebug(rot + " " + xVec + " " + yVec);

                transform.eulerAngles = new Vector3(0, 0, rot + 180);

                TimeoutTimer += Time.deltaTime;
                if (TimeoutTimer > Timeout)
                    Destroy(this.gameObject);
            }

        }

        void Init()
        {

        }

        private void OnTriggerEnter(Collider collider)
        {
            if(floorOnly)
            {
                if (collider.gameObject.tag == "Floor")
                {
                    this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    this.gameObject.GetComponent<SphereCollider>().enabled = false;
                    anim.SetBool("Shoot", false);
                    anim.SetBool("Hit", true);
                }
                return;
            }

            if (collider.gameObject.tag == "Floor" || collider.gameObject.tag == "Monster")
            {
                this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                this.gameObject.GetComponent<SphereCollider>().enabled = false;
                anim.SetBool("Shoot", false);
                anim.SetBool("Hit", true);
            }
        }
    }
}
