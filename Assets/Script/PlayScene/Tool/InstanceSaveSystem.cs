using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SexyBackPlayScene
{
    internal class InstanceSaveSystem
    {
        internal static readonly string InstanceDataPath = "InstanceData.xml";

        internal void Save(object target, string filename)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + filename);
            bf.Serialize(file, target);
            file.Close();
        }

        internal object Load(string filename)
        {
            object loaddata = null;
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                FileStream file = File.Open(Application.persistentDataPath + "/" + filename, FileMode.Open);
                if (file != null && file.Length > 0)
                {
                    loaddata = bf.Deserialize(file);
                }
                file.Close();
                return loaddata;
            }
            catch (Exception e)
            {
                sexybacklog.Error("FileLoad Error " + e.Message);
                return null;
            }
        }

        private static void DeleteFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                try
                {
                    System.IO.File.Delete(path);
                }
                catch (System.IO.IOException e)
                {
                    sexybacklog.Error("No SavedFile" + e.Message);
                    return;
                }
            }
        }
        internal static void ClearInstance()
        {
            DeleteFile(InstanceDataPath);
            //DeleteFile(Application.persistentDataPath + "/statmanager.dat");
            //DeleteFile(Application.persistentDataPath + "/stagemanager.dat");
            //DeleteFile(Application.persistentDataPath + "/monsterManager.dat");
            //DeleteFile(Application.persistentDataPath + "/heroManager.dat");
            //DeleteFile(Application.persistentDataPath + "/elementalManager.dat");
            //DeleteFile(Application.persistentDataPath + "/researchManager.dat");
            //PlayerPrefs.DeleteKey("InstanceData");
            //PlayerPrefs.DeleteAll();
        }

        internal static bool InstanceXmlDataExist
        {
            get
            {
                return System.IO.File.Exists(InstanceDataPath);
            }
        }

        //internal static bool InstanceDataExist
        //{
        //    get
        //    {
        //        return System.IO.File.Exists(Application.persistentDataPath + "/statmanager.dat");
        //    }
        //}

        // 로드가 완료되지 않은 시점에서 save가 뜨면 안된다.
        internal static void SaveInstacne()
        {
            XmlWriterSettings setting = new XmlWriterSettings();
            setting.Indent = true;

            XmlWriter writer = XmlWriter.Create(InstanceDataPath, setting);
            writer.WriteStartDocument();

            {
                InstanceStatus stat = Singleton<InstanceStatus>.getInstance();
                writer.WriteStartElement("InstanceStatus");
                writer.WriteAttributeString("Exp", stat.EXP.ToString());
                writer.WriteAttributeString("MapID", stat.InstanceMap.ID.ToString());
                writer.WriteAttributeString("IsBonused", stat.IsBonused.ToString());
                writer.WriteAttributeString("CurrentGameTime", stat.CurrentGameTime.ToString());
                {
                    SaveHero(writer);
                    SaveElements(writer);
                    SaveResearchs(writer);
                    SaveStage(writer);
                    SaveMonster(writer);
                    SaveConsumable(writer);
                }
                writer.WriteEndElement();
            }
            writer.WriteEndDocument();
            writer.Close();
        }



        private static void SaveMonster(XmlWriter writer)
        {
            writer.WriteStartElement("Monsters");
            foreach (Monster data in Singleton<MonsterManager>.getInstance().monsters.ToArray())
            {
                writer.WriteStartElement("Monster");
                writer.WriteAttributeString("id", data.GetID.ToString());
                writer.WriteAttributeString("dataid", data.DataID.ToString());
                writer.WriteAttributeString("level", data.level.ToString());
                writer.WriteAttributeString("type", data.type.ToString());
                writer.WriteAttributeString("hp", data.HP.ToString());
                writer.WriteAttributeString("maxhp", data.MAXHP.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        private static void SaveStage(XmlWriter writer)
        {
            StageManager manager = Singleton<StageManager>.getInstance();
            writer.WriteStartElement("Stages");
            writer.WriteAttributeString("currentfloor", manager.CurrentFloor.ToString());
            writer.WriteAttributeString("lastclearedfloor", manager.lastClearedFloor.ToString());
            writer.WriteAttributeString("nextbattlestage", manager.NextBattleStage.ToString());
            writer.WriteAttributeString("nextmonsterhp", manager.NextMonsterHP.ToString());
            writer.WriteAttributeString("combo", manager.Combo.ToString());
            writer.WriteAttributeString("neednextstage", manager.needNextStage.ToString());
            writer.WriteAttributeString("battletime", manager.BattleTime.ToString());


            foreach (Stage data in manager.Stages)
            {
                writer.WriteStartElement("Stage");
                writer.WriteAttributeString("floor", data.floor.ToString());
                writer.WriteAttributeString("zPosition", data.zPosition.ToString());
                writer.WriteAttributeString("state", data.CurrentState.ToString());
                //writer.WriteAttributeString("monstercount", data.monsterCount.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        private static void SaveResearchs(XmlWriter writer)
        {
            ResearchManager manager = Singleton<ResearchManager>.getInstance();
            writer.WriteStartElement("Researchs");
            writer.WriteAttributeString("currentthread", manager.currentthread.ToString());
            {
                writer.WriteStartElement("FinishResearchs");
                foreach (string id in manager.FinishList.Keys)
                {
                    writer.WriteStartElement("Research");
                    writer.WriteAttributeString("id", id.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteStartElement("RemainResearchs");
                foreach (Research data in manager.researches.Values)
                {
                    writer.WriteStartElement("Research");
                    writer.WriteAttributeString("id", data.GetID.ToString());
                    writer.WriteAttributeString("state", data.CurrentState.ToString());
                    writer.WriteAttributeString("remaintime", data.RemainTime.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        private static void SaveHero(XmlWriter writer)
        {
            Hero data = Singleton<HeroManager>.getInstance().GetHero();
            writer.WriteStartElement("Hero");
            writer.WriteAttributeString("level", data.OriginalLevel.ToString());
            writer.WriteAttributeString("basedmg", data.DamageDensity.ToString());
            writer.WriteAttributeString("state", data.CurrentState.ToString());
            writer.WriteEndElement();
        }

        private static void SaveElements(XmlWriter writer)
        {
            writer.WriteStartElement("Elementals");
            foreach(Elemental data in Singleton<ElementalManager>.getInstance().elementals.Values)
            {
                writer.WriteStartElement("Elemental");
                writer.WriteAttributeString("id", data.GetID.ToString());
                writer.WriteAttributeString("level", data.OriginalLevel.ToString());
                writer.WriteAttributeString("skillactive", data.SkillActive.ToString());
                //writer.WriteAttributeString("skillforcecount", data.SkillForceCount.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        private static void SaveConsumable(XmlWriter writer)
        {
            ConsumableManager manager = Singleton<ConsumableManager>.getInstance();
            writer.WriteStartElement("Consumables");
            {
                foreach (var data in manager.inventory.Values)
                {
                    writer.WriteStartElement("Consumable");
                    writer.WriteAttributeString("id", data.GetID);
                    writer.WriteAttributeString("remaintime", data.RemainTime.ToString());
                    writer.WriteAttributeString("stack", data.Stack.ToString());
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
        }

    }
}
