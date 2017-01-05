using UnityEngine;
using System.Collections;

public class projectile_collision : MonoBehaviour {

	Animator anim;
	
	void Start () {
		anim = this.gameObject.GetComponent<Animator>();
		Physics2D.IgnoreLayerCollision(8,8);
	}
	
	void Update () {
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("Base.Destroy")){
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
        if(coll.gameObject.name != null)
        {
			anim.SetBool("Hit", true);
			this.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        }
	}
}
