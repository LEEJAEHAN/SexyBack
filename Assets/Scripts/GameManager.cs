using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    public class GameManager
    { // singleton
        private static GameManager GameManagerInstance;

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

            hero = new SexyBackHero();
            monster = new SexyBackMonster(new BigInteger(1000,Digit.b));
            elementals = new List<Elemental>();

            ElementalData fireball = new ElementalData("fireball", 3200, 10, 100);
            ElementalData waterball = new ElementalData("waterball", 3300, 50, 120);
            ElementalData rock = new ElementalData("rock", 3500, 400, 168);
            ElementalData electricball = new ElementalData("electricball", 3700, 3750, 237);
            ElementalData snowball = new ElementalData("snowball", 4100, 45000, 400);
            ElementalData earthball = new ElementalData("earthball", 4300, 725000, 720);
            ElementalData airball = new ElementalData("airball", 4700, 15000000, 1450);
            ElementalData iceblock = new ElementalData("iceblock", 4900, 400000000, 2785);
            ElementalData magmaball = new ElementalData("magmaball", 5300, 14000000000, 5100);



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

        internal SexyBackMonster GetMonster()
        {
            return monster;
        }
        internal BigInteger GetTotalDPC()
        {
            return hero.DPC;
        }

        internal BigInteger GetTotalDPS()
        {
            BigInteger result = new BigInteger();
            foreach(Elemental elemental in elementals)
            {
                result += elemental.Dps;
            }
            return result;
        }

        SexyBackHero hero;
        SexyBackMonster monster;
        List<Elemental> elementals;
        public BigInteger EXP = 0;


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
        internal void GainExp(BigInteger damage)
        {
            EXP += damage;
            UIUpdater.getInstance().noteiceExpChanged();
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
