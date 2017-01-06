using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    public class GameManager
    { // singleton

        private static GameManager GameManagerInstance;
        List<InputListener> inputlisteners;

        private GameManager()
        {

        }

        public static GameManager getInstance()
        {
            if (GameManagerInstance == null)
                GameManagerInstance = new GameManager();
            return GameManagerInstance;
        }



        // Use this for initialization
        public void Init()
        {
            Physics.gravity = new Vector3(0, -20.0f, 0);


            inputlisteners = new List<InputListener>();

            hero = new SexyBackHero();
            monster = new SexyBackMonster(2500);
            elementals = new List<Elemental>();

            elementals.Add(new Elemental("airball", new ElementalData("airball"), GameObject.Find("shooter_airball")));
            //elementals.Add(new EarthElemental());
            //elementals.Add(new ElectricElemental());
            //elementals.Add(new IceElemental());
            //elementals.Add(new FireElemental());
            //elementals.Add(new MagmaElemental());
            //elementals.Add(new RockElemental());
            //elementals.Add(new SnowElemental());
            //elementals.Add(new WaterElemental());


        }



        internal void noticeEvent(GameObject sender)
        {
            foreach (InputListener listner in inputlisteners)
                listner.update(sender);
        }

        SexyBackHero hero;
        SexyBackMonster monster;
        List<Elemental> elementals;

        public double testTimeTick = 1;
        double gameTime;

        // Update is called once per frame
        public void Update()
        {
            foreach (Elemental elemenatal in elementals)
            {
                elemenatal.Update();
            }


            //hero.AttackDPS(Time.deltaTime, monster);

            gameTime += Time.deltaTime;
            if (gameTime > testTimeTick)
            {
                gameTime -= testTimeTick;
                hero.IncreaseDPC(3);
            }

        }

        internal void Tap()
        {
            hero.AttackDPC(monster);

            foreach (Elemental elemenatal in elementals)
            {
                elemenatal.Shoot();
            }

        }

        public static void SexyBackLog(object msg)
        {
            Debug.Log(msg);
        }

        internal static void SexyBackDebug(object msg)
        {
            GameObject.Find("label_debug").GetComponent<UILabel>().text = msg.ToString();
        }

        public static string SexyBackToInt(double value)
        {
            // 소수 세째짜리까지
            if (value > 1000)
            {
                if (value > 10000)// 갓넘었을때는 안줄인다.
                {
                    value = value / 1000;
                    return value.ToString("N1") + "K";

                }
                else
                {
                    return value.ToString("N0");
                }
            }
            else
            {
                return value.ToString("N1");
            }


        }
    }



}
