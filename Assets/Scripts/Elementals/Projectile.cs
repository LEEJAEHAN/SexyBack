using UnityEngine;
using System.Collections;

namespace SexyBackPlayScene
{

    public class Projectile : MonoBehaviour
    {
        // projectile data
        public BigInteger Damage;

        
        
        // animation info
        Animator anim;
        float rot;

        void Start()
        {
            anim = this.gameObject.GetComponent<Animator>();
//            Physics2D.IgnoreLayerCollision(8, 8);
        }

        void Update()
        {
            if(anim.GetBool("Shoot") == true)
            {
                float xVec = GetComponent<Rigidbody>().velocity.x;
                float yVec = GetComponent<Rigidbody>().velocity.y;
                float rad = Mathf.Atan2(yVec, xVec);

                rot = UnityEngine.Mathf.Atan2(yVec, xVec) * UnityEngine.Mathf.Rad2Deg;

//                GameManager.SexyBackDebug(rot + " " + xVec + " " + yVec);

                transform.eulerAngles = new Vector3(0, 0, rot + 180);
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base.Destroy"))
            {
//                GameManager.SexyBackLog("RemoveFire");
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
