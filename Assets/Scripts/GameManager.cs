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
            monster = new SexyBackMonster(100);
            elementals = new List<Elemental>();



            ElementalData fireball = new ElementalData("fireball", 2, 10, 100);
            ElementalData waterball = new ElementalData("waterball", 3, 50, 120);
            ElementalData rock = new ElementalData("rock", 5, 400, 168);
            ElementalData electricball = new ElementalData("electricball", 7, 3750, 237);
            ElementalData snowball = new ElementalData("snowball", 11, 45000, 400);
            ElementalData earthball = new ElementalData("earthball", 13, 725000, 720);
            ElementalData airball = new ElementalData("airball", 17, 15000000, 1450);
            ElementalData iceblock = new ElementalData("iceblock", 19, 400000000, 2785);
            ElementalData magmaball = new ElementalData("magmaball", 23, 0, 5100);




            elementals.Add(new Elemental(fireball.ShooterName, fireball, Resources.Load(fireball.ProjectilePrefabName) as GameObject, GameObject.Find(fireball.ShooterName)));
            elementals.Add(new Elemental(waterball.ShooterName, waterball, Resources.Load(waterball.ProjectilePrefabName) as GameObject, GameObject.Find(waterball.ShooterName)));
            elementals.Add(new Elemental(rock.ShooterName, rock, Resources.Load(rock.ProjectilePrefabName) as GameObject, GameObject.Find(rock.ShooterName)));
            elementals.Add(new Elemental(electricball.ShooterName, electricball, Resources.Load(electricball.ProjectilePrefabName) as GameObject, GameObject.Find(electricball.ShooterName)));
            elementals.Add(new Elemental(snowball.ShooterName, snowball, Resources.Load(snowball.ProjectilePrefabName) as GameObject, GameObject.Find(snowball.ShooterName)));
            elementals.Add(new Elemental(earthball.ShooterName, earthball, Resources.Load(earthball.ProjectilePrefabName) as GameObject, GameObject.Find(earthball.ShooterName)));
            elementals.Add(new Elemental(airball.ShooterName, airball, Resources.Load(airball.ProjectilePrefabName) as GameObject, GameObject.Find(airball.ShooterName)));
            elementals.Add(new Elemental(iceblock.ShooterName, iceblock, Resources.Load(iceblock.ProjectilePrefabName) as GameObject, GameObject.Find(iceblock.ShooterName)));
            elementals.Add(new Elemental(magmaball.ShooterName, magmaball, Resources.Load(magmaball.ProjectilePrefabName) as GameObject, GameObject.Find(magmaball.ShooterName)));


        }

        internal void GainExp(double damage)
        {
            EXP += damage;
            UIUpdater.getInstance().noteiceExpChanged(EXP);

        }

        internal void noticeEvent(GameObject sender)
        {
            foreach (InputListener listner in inputlisteners)
                listner.update(sender);
        }

        SexyBackHero hero;
        SexyBackMonster monster;
        List<Elemental> elementals;
        public double EXP = 0;


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
                elemenatal.LevelUp();


                //   elemenatal.ShootForDebug();
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
