using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class HeroAttackManager : IDisposable // 쿨타임관리, 이펙트생성 등
    {
        ~HeroAttackManager()
        {
            sexybacklog.Console("HeroAttackManager 소멸");
        }
        public void Dispose()
        {
            owner = null;
            AttackPlan = null;
            CoolTimeBar = null;
            SwordIcons = null;
        }

        private Hero owner;
        private Queue<TapPoint> AttackPlan = new Queue<TapPoint>();

        // 상태값만 있지 히어로데이터는없어야한다.
        private double attackTimer = 0; // 공격쿨타임카운터

        public int atkCount {  get { return owner.AttackCount; } set { owner.AttackCount = value; } }
        public bool CanMakePlan { get { return atkCount > 0; } }
        public bool CanAttack { get { return AttackPlan.Count > 0; } }

        private GameObject CoolTimeBar = GameObject.Find("Bar_Attack");
        private GameObject[] SwordIcons = new GameObject[10];
        private int[] iconangle = { 0, 30, -30, 60, -60, 90, -90 };


        public HeroAttackManager(Hero hero)
        {
            owner = hero;
            CoolTimeBar.transform.DestroyChildren();
        }
        public void SetAttackCount(int count)
        {
            atkCount = count;
            DrawIcon(atkCount);
        }
        void DrawIcon(int count)
        {
            for (int i = 1; i <= count; i++)
            {
                if (i > iconangle.Length)
                    return;
                AddSwordIcon(i);
            }
        }
        internal void Update()
        {
            CheckCoolDown();
        }

        internal void MakeAttackPlan(TapPoint point)
        {
            if (atkCount <= 0)
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
            atkCount++;
            if (atkCount > iconangle.Length)
                return;
            AddSwordIcon(atkCount);
        }
        private void ReduceAttackCount()
        {
            atkCount--;
            if (atkCount < 0 || atkCount > iconangle.Length)
                return;

            RemoveSwordIcon(atkCount);
        }
        private void RemoveSwordIcon(int seuqnece)
        {
            if (SwordIcons[seuqnece] != null)
                GameObject.Destroy(SwordIcons[seuqnece]);
        }

        private void CheckCoolDown()
        {
            if (atkCount < owner.MaxAttackCount)
            {
                attackTimer += Time.deltaTime;
                CoolTimeBar.GetComponent<UISlider>().value = (float)(attackTimer / owner.AttackInterval);
                if (attackTimer > owner.AttackInterval)
                {
                    AddAttackCount();
                    attackTimer -= owner.AttackInterval;
                }
            }
            else // staic is max
                CoolTimeBar.GetComponent<UISlider>().value = 1;
        }

        public void MakeSlashEffect(TapPoint Tap, Vector3 monsterPos, bool isCritical)
        {
            // rotate
            Vector3 directionVector = monsterPos - Tap.PosInHeroCam;
            float rot = UnityEngine.Mathf.Atan2(directionVector.y, directionVector.x) * UnityEngine.Mathf.Rad2Deg;
            EffectController.getInstance.PlaySwordEffect(Tap.PosInEffectCam, new Vector3(0, 0, rot), isCritical);
        }

        private void AddSwordIcon(int sequence)
        {
            GameObject swordicon = ViewLoader.InstantiatePrefab(CoolTimeBar.transform, "attackcount" + sequence, "prefabs/UI/attackcount");
            swordicon.transform.localScale = Vector3.one;
            swordicon.transform.rotation = Quaternion.Euler(0, 0, 45 + iconangle[sequence - 1]);
            SwordIcons[sequence - 1] = swordicon;
        }


    }
}