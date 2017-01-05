using UnityEngine;
using System.Collections;

namespace SexyBackPlayScene
{
    public class MonsterAvatar : MonoBehaviour {

        public SexyBackMonster monster;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

        private void OnTriggerEnter(Collider source)
        {
            if(source.tag == "Projectile")
            {
                double damage = source.gameObject.GetComponent<Projectile>().damage;

                monster.Hit(damage, true);
            }
        }
    }

}
