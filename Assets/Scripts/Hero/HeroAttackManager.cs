using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class HeroAttackManager // 쿨타임관리, 이펙트생성 등
    {
        private Hero owner;
        private Queue<TapPoint> AttackPlan = new Queue<TapPoint>();

        // 상태값만 있지 히어로데이터는없어야한다.
        private double attackTimer = 0; // 공격쿨타임카운터
        private int currentAttackCount = 0; // 누를시 바로 소모된다.

        public bool CanMakePlan { get { return currentAttackCount > 0; } }
        public bool CanAttack { get { return AttackPlan.Count > 0; } }

        private GameObject[] SwordIcons = new GameObject[10];
        private int[] iconangle = { 0, 30, -30, 60, -60, 90, -90 };

        private GameObject SwordIcon = Resources.Load<GameObject>("prefabs/UI/attackcount");
        private GameObject SwordEffect = ViewLoader.Effect_Sword;
        private GameObject CoolTimeBar = ViewLoader.Bar_Attack;

        public HeroAttackManager(Hero hero)
        {
            owner = hero;
            for (int i = 0; i < 7; i++) // test
                AddAttackCount();
            // Instantiate GameObject.
        }
        public void ViewOn()
        {

        }
        public void ViewOff()
        {

        }

        internal void Update()
        {
            CheckCoolDown();
        }

        internal void MakeAttackPlan(TapPoint point)
        {
            if (currentAttackCount <= 0)
                return;
            AttackPlan.Enqueue(point); // 사이즈는 attackcount
            ReduceAttackCount();
        }
        internal TapPoint NextAttackPlan()
        {
            return AttackPlan.Dequeue();
        }

        private void AddAttackCount()
        {
            currentAttackCount++;

            if (currentAttackCount > iconangle.Length)
                return;

            AddSwordIcon(currentAttackCount);
        }
        private void ReduceAttackCount()
        {
            currentAttackCount--;
            if (currentAttackCount < 0 || currentAttackCount > iconangle.Length)
                return;

            RemoveSwordIcon(currentAttackCount);
        }

        private void CheckCoolDown()
        {
            if (currentAttackCount < owner.MAXATTACKCOUNT)
            {
                attackTimer += Time.deltaTime;
                ViewLoader.Bar_Attack.GetComponent<UISlider>().value = (float)(attackTimer / owner.ATTACKINTERVAL);
                if (attackTimer > owner.ATTACKINTERVAL)
                {
                    AddAttackCount();
                    attackTimer -= owner.ATTACKINTERVAL;
                }
            }
            else // staic is max
                ViewLoader.Bar_Attack.GetComponent<UISlider>().value = 1;
        }

        public void MoveMakePlayEffect(TapPoint Tap, Vector3 monsterPos, bool isCritical)
        {
            // rotate
            Vector3 directionVector = monsterPos - Tap.GamePos;
            float rot = UnityEngine.Mathf.Atan2(directionVector.y, directionVector.x) * UnityEngine.Mathf.Rad2Deg;
            SwordEffect.transform.eulerAngles = new Vector3(0, 0, rot);

            // translation
            SwordEffect.transform.position = Tap.EffectPos;

            // play
            if(isCritical)
                SwordEffect.GetComponent<Animator>().SetTrigger("Play_Critical");
            else
                SwordEffect.GetComponent<Animator>().SetTrigger("Play");

            // debug
            //sexybacklog.Console("Hit:"+SwordEffect.transform.position);
        }

        private void AddSwordIcon(int sequence)
        {   
            GameObject swordicon = GameObject.Instantiate(SwordIcon) as GameObject;
            swordicon.name = "attackcount" + sequence;
            swordicon.transform.parent = ViewLoader.Bar_Attack.transform;
            swordicon.transform.localScale = Vector3.one;
            swordicon.transform.localPosition = Vector3.zero;
            swordicon.transform.rotation = Quaternion.Euler(0, 0, 45 + iconangle[sequence - 1]);
            SwordIcons[sequence - 1] = swordicon;
        }
        // TODO : 한번 생겼으면 pooling하자..
        private void RemoveSwordIcon(int seuqnece)
        {
            if (SwordIcons[seuqnece] != null)
                GameObject.Destroy(SwordIcons[seuqnece]);
        }

    }
}