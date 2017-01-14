using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    class ElementalManager
    {
        public List<Elemental> elementals;
        SexyBackMonster target;

        UILabel totaldpslabel = ViewLoader.label_elementaldmg.GetComponent<UILabel>();


        internal void Init()
        {
            target = Singleton<MonsterManager>.getInstance().GetMonster();
            //test elementals
            elementals = new List<Elemental>();

            Load();
        }

        private void Load()
        {
            ElementalData fireball = new ElementalData("fireball", 5200, 1);
            ElementalData waterball = new ElementalData("waterball", 5300, 5);
            ElementalData rock = new ElementalData("rock", 5500, 25);
            ElementalData electricball = new ElementalData("electricball", 5700, 150);
            ElementalData snowball = new ElementalData("snowball", 6100, 1150);
            ElementalData earthball = new ElementalData("earthball", 6300, 10000);
            ElementalData airball = new ElementalData("airball", 6700, 100000);
            ElementalData iceblock = new ElementalData("iceblock", 6900, 1500000);
            ElementalData magmaball = new ElementalData("magmaball", 7300, 27500000);

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

        internal void Update()
        {
            foreach (Elemental elemenatal in elementals)
            {
                {
                    elemenatal.AttackTimer += Time.deltaTime;

                    // 만들어진다.
                    if (elemenatal.AttackTimer > elemenatal.AttackInterval - 1 && elemenatal.NoProjectile)
                    {
                        elemenatal.CreateProjectile();
                    }

                    if (elemenatal.AttackTimer > elemenatal.AttackInterval)
                    {
                        elemenatal.Shoot(target.Position);
                    }

                }
                //    elemenatal.Update();
            }
        }


        public void onDpsChanged(Elemental sender)
        {
            string dpsString = "DPS : " + GetTotalDPS().ToSexyBackString();
            totaldpslabel.GetComponent<UILabel>().text = dpsString;
        }

        internal BigInteger GetTotalDPS()
        {
            BigInteger result = new BigInteger();
            foreach (Elemental elemental in elementals)
            {
                result += elemental.Dps;
            }
            return result;
        }

    }
}