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
            hero = new SexyBackHero();
            monster = new SexyBackMonster(2500);
            inputlisteners = new List<InputListener>(); 
        }



        internal void noticeEvent(GameObject sender)
        {
            foreach (InputListener listner in inputlisteners)
                listner.update(sender);
        }

        SexyBackHero hero;
        SexyBackMonster monster;
        public double testTimeTick = 1;
        double gameTime;

        // Update is called once per frame
        public void Update()
        {
            hero.AttackDPS(Time.deltaTime, monster);


            //            SexyBackLog("GamaManager Updating;;");;
            gameTime += Time.deltaTime;
            if(gameTime > testTimeTick)
            {
                gameTime -= testTimeTick;
                hero.IncreaseDPC(3);
                hero.IncreaseDPS(1);
            }

        }

        internal void Tap()
        {
            hero.AttackDPC(monster);
            GameObject.Find("proj").GetComponent<proj>().Throw();
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
                if(value > 10000)// 갓넘었을때는 안줄인다.
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
