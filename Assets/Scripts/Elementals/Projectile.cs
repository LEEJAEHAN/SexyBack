using UnityEngine;
using System.Collections;

namespace SexyBackPlayScene
{

    public class Projectile : MonoBehaviour // view와 동일하다
    {
        // projectile data
        public BigInteger Damage;

        // animation info
        Animator anim;

        void Awake()
        {
            anim = this.gameObject.GetComponent<Animator>();
            //Physics2D.IgnoreLayerCollision(8, 8);
        }

        void Start()
        {
            
        }

        void FixedUpdate()
        {
            if(anim.GetBool("Shoot") == true)
            {
                float xVec = GetComponent<Rigidbody>().velocity.x;
                float yVec = GetComponent<Rigidbody>().velocity.y;
                
                float rot = UnityEngine.Mathf.Atan2(yVec, xVec) * UnityEngine.Mathf.Rad2Deg;
                //GameManager.SexyBackDebug(rot + " " + xVec + " " + yVec);

                transform.eulerAngles = new Vector3(0, 0, rot + 180);
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base.Destroy"))
            {
                Destroy(this.gameObject);
            }
        }

        void Init()
        {

        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Monster")
            {
                anim.SetBool("Shoot", false);
                anim.SetBool("Hit", true);
                this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}
