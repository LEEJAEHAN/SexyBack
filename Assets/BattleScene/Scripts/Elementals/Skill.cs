//using System;
//using UnityEngine;

//namespace SexyBackPlayScene
//{
//    internal class Skill
//    {
//        internal int DamageXH = 300;
//        internal readonly int DamageXHPerLV = 20;

//        internal int Duration = 30;

//        internal int ProjectileAmount = 15;
//        internal double scale = 1;
//        internal double speed = 1;

//        internal string buffID = "";
//        internal string buffTarget = "";

//        internal Color color; // overray color
//        internal string ability; //  "instancedmg", "buff", "dotdmg", , ;
//        internal string casttype; // "shoot", "drop", "fastshoot" "cast"; // dot,4 



//        bool ready;
//        bool attack;


//        internal void Update(Elemental elemental)
//        {     
//            // case shoot
//            if (ready)
//            {
//                elemental.currentporjectile = CreateProjectile(슈터위치, 량, 스케일, 속도, 칼라, 프리펩네임));
//            }
//            if (attack)
//            {
//                if (CastShoot(this))
//                    EndAttack();
//            }

//            // case drop
//            if (ready)
//            {
//                elemental.currentporjectile = CreateProjectile(CurrentProjectile);
//            }
//            if (attack)
//            {
//                if (CastShoot(this))
//                    EndAttack();
//            }

//        }

//        internal void CastShoot(Elemental elemental)
//        {
//            if (elemental.targetID != null)
//            {
//                Vector3 destination = elemental.calDestination(elemental.targetID);
//                if (elemental.CurrentProjectile.Shoot(destination, 0.8f))
//                    EndAttack();
//            }
//            else if (targetID == null) { } //타겟이생길떄까지 대기한다. 

//        }

//        internal void CastDrop(Elemental elemental)
//        {
//            if (elemental.targetID != null)
//            {
//                Vector3 destination = elemental.calDestination(targetID);
//                if (CurrentProjectile.Shoot(destination, 0.8f))
//                    EndAttack();
//            }
//            else if (targetID == null) { } //타겟이생길떄까지 대기한다. 

//        }

//        internal void CastCast(Elemental elemental)
//        {
//            if (elemental.targetID != null)
//            {
//                Vector3 destination = elemental.calDestination(targetID);
//                if (CurrentProjectile.Shoot(destination, 0.8f))
//                    EndAttack();
//            }
//            else if (targetID == null) { } //타겟이생길떄까지 대기한다. 

//        }


//        internal void Apply(Monster owner, MonsterStateReady monsterStateReady)
//        {
//            if (skill.ability == "Dot")
//            {

//            }

//            dobplans.Add
//            //debuffplans.add(  
//        }

//    }

//}