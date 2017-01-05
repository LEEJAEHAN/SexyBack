using UnityEngine;
using System.Collections;

public class Shooter_Example : MonoBehaviour {
	
	GameObject projectile;
	Animator anim;
	Rigidbody2D r_body;
	Vector3 projectile_position;
	
	public string projectile_name;
	
	void Update () {
		
		if(Input.mousePosition.x <= Screen.width*0.75f){
		
				 projectile_position = Input.mousePosition;		 
				 projectile_position.z = 10.0f;
				 projectile_position = Camera.main.ScreenToWorldPoint(projectile_position);
				 projectile_position.y = projectile_position.y + 1;

			if(Input.GetMouseButtonDown(0)){			
				projectile = Instantiate(Resources.Load(projectile_name)) as GameObject;
				projectile.layer = 8;
				projectile.transform.position = projectile_position;
				anim = projectile.GetComponent<Animator>();
				r_body = projectile.GetComponent<Rigidbody2D>();	
			}
			
			if(Input.GetMouseButton(0) && projectile && anim){
				projectile.transform.position = projectile_position;
			}
			
			if(Input.GetMouseButtonUp(0) && anim){
				anim.SetBool("Shoot", true);
				r_body.isKinematic = false;
				r_body.velocity = new Vector2(-15, 0); 
			}
		}
		else{
			if(anim){
				anim.SetBool("Shoot", true);
				r_body.isKinematic = false;
				r_body.velocity = new Vector2(-15, 0);
				anim = null;
			}
		}

	}
	
	void OnGUI () {
		if (GUI.RepeatButton(new Rect(Screen.width - 160, 10, 150, 20), "fireball"))
				projectile_name = "fireball";
			
		if (GUI.RepeatButton(new Rect(Screen.width - 160, 40, 150, 20), "waterball"))
				projectile_name = "waterball";
				
		if (GUI.RepeatButton(new Rect(Screen.width - 160, 70, 150, 20), "snowball"))
				projectile_name = "snowball";
				
		if (GUI.RepeatButton(new Rect(Screen.width - 160, 100, 150, 20), "iceblock"))
				projectile_name = "iceblock";
				
		if (GUI.RepeatButton(new Rect(Screen.width - 160, 130, 150, 20), "airball"))
				projectile_name = "airball";
				
		if (GUI.RepeatButton(new Rect(Screen.width - 160, 160, 150, 20), "magmaball"))
				projectile_name = "magmaball";
				
		if (GUI.RepeatButton(new Rect(Screen.width - 160, 190, 150, 20), "earthball"))
				projectile_name = "earthball";
				
		if (GUI.RepeatButton(new Rect(Screen.width - 160, 220, 150, 20), "electricball"))
				projectile_name = "electricball";
				
		if (GUI.RepeatButton(new Rect(Screen.width - 160, 250, 150, 20), "rock"))
				projectile_name = "rock";
        
	}
}
