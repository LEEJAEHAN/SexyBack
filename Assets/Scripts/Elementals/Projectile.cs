using UnityEngine;

namespace SexyBackPlayScene
{
    internal class Projectile // TODO : 리팩토링해야함.
    {
        public string ownerID;
        //string ID; // useless, projectile은 elemental에 종속된다.
        public GameObject view; // projectile View

        //~Projectile(){ sexybacklog.Console("projectile 소멸!"); }

        public Projectile(Elemental owner, string prefabpath, Vector3 genPosition)
        {
            ownerID = owner.GetID;
            view = ViewLoader.InstantiatePrefab(ViewLoader.shooter.transform, this.ownerID, prefabpath);
            view.transform.position = genPosition; // not local position
            view.transform.localScale = ViewLoader.projectiles.transform.lossyScale;
            // position 은 월드포지션. localpositoin은 부모에서 얼마떨어져있냐, localScale은 그 객체클릭했을때 나오는 사이즈값. lossyscale은 최고 root로빠졌을때의 사이즈값.
            view.GetComponent<ProjectileView>().noticeDestroy += owner.onDestroyProjectile;
            view.GetComponent<SphereCollider>().enabled = false;
            //TODO : 아직 리팩토링할곳이많은부분, 바로윗줄은 shooter object의 scale을 world에서 조정해야함
            view.SetActive(true);
        }

        private bool isReadyState
        {
            get
            {
                string ready = ElementalData.ProjectileReadyStateName(ownerID);
                return view.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(ready);
            }
        }

        internal bool Shoot(Vector3 target, float speed)
        {
            if (view != null && isReadyState)
            {
                view.transform.parent = ViewLoader.projectiles.transform; // 슈터에서 빠진다.
                // Shootfunc
                view.GetComponent<Animator>().SetBool("Shoot", true);
                view.GetComponent<SphereCollider>().enabled = true;
                view.GetComponent<Rigidbody>().useGravity = true;


                float xDistance = target.x - view.transform.position.x;
                float yDistance = target.y - view.transform.position.y;
                float zDistance = target.z - view.transform.position.z;

                float throwangle_xy;

                throwangle_xy = Mathf.Atan((yDistance + (-Physics.gravity.y * Mathf.Pow(speed, 2) / 2)) / xDistance);

                //float totalVelo = xDistance / Mathf.Cos(throwangle_xy);

                float xVelo, yVelo, zVelo;
                xVelo = xDistance / speed;
                yVelo = xDistance * Mathf.Tan(throwangle_xy) / speed;
                zVelo = zDistance / speed;

                view.GetComponent<Rigidbody>().velocity = new Vector3(xVelo, yVelo, zVelo);
                return true;
            }
            return false;
        }


    }
}