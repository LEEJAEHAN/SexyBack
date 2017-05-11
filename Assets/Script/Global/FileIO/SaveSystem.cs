using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

/// <summary>
/// 저장기능으로 사용해본 후보.
/// Unity PlayerPrefs
/// binary serialization - 인스턴스데이터 저장에 사용중.
/// xml text serialization
/// xml not using serialization - 글로벌 데이터 저장에 사용중.
/// 
/// serialization 시 한파일에 담는걸 잘모르겠음 ;ㅅ;
/// </summary>
internal class SaveSystem
{
    internal static readonly string SaveDataPath = "SaveData.xml";

    internal static void Save(object target, string filename)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + filename);
        bf.Serialize(file, target);
        file.Close();
    }

    internal static object Load(string filename)
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

    //internal static void SaveInstacne()
    //{
    //    PlayerPrefs.SetString("InstanceData", "Yes");
    //}
    internal static bool GlobalDataExist
    {
        get
        {
            return System.IO.File.Exists(SaveDataPath);
        }
    }

    internal static bool InstanceDataExist
    {
        get
        {
            return System.IO.File.Exists(Application.persistentDataPath + "/statmanager.dat");
        }
    }

    internal static void ClearInstance()
    {
        DeleteFile(Application.persistentDataPath + "/statmanager.dat");
        DeleteFile(Application.persistentDataPath + "/stagemanager.dat");
        DeleteFile(Application.persistentDataPath + "/monsterManager.dat");
        DeleteFile(Application.persistentDataPath + "/heroManager.dat");
        DeleteFile(Application.persistentDataPath + "/elementalManager.dat");
        DeleteFile(Application.persistentDataPath + "/researchManager.dat");
        //PlayerPrefs.DeleteKey("InstanceData");
        //PlayerPrefs.DeleteAll();
    }

    internal static XmlDocument LoadXml(string saveDataPath)
    {
        //SaveSystem.LoadEquipments()
        //TextAsset textasset = Resources.Load(SaveDataPath) as TextAsset;
        //XmlDocument xmldoc = new XmlDocument();
        //xmldoc.LoadXml(SaveDataPath);
        //xmldoc.LoadXml(textasset.text);

        XmlDocument xmldoc = new XmlDocument();
        xmldoc.Load(SaveDataPath);
        return xmldoc;
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

    internal static void SaveGlobalData()
    {
        EquipmentManager equipManager = Singleton<EquipmentManager>.getInstance();

        XmlWriterSettings setting = new XmlWriterSettings();
        setting.Indent = true;

        XmlWriter writer = XmlWriter.Create(SaveDataPath, setting);
        writer.WriteStartDocument();
        writer.WriteStartElement("PlayerStatus");
        SaveStat(writer);
        SaveEquipments(writer);
        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Close();
    }


    private static void SaveStat(XmlWriter writer)
    {
        PlayerStatus playerStatus = Singleton<PlayerStatus>.getInstance();
        writer.WriteStartElement("Stats");
        {
            writer.WriteStartElement("BaseStat");
            BaseStat baseStat = playerStatus.GetBaseStat;
            writer.WriteAttributeString("Str", baseStat.Str.ToString());
            writer.WriteAttributeString("Int", baseStat.Int.ToString());
            writer.WriteAttributeString("Spd", baseStat.Spd.ToString());
            writer.WriteAttributeString("Luck", baseStat.Luck.ToString());
            writer.WriteEndElement();
        }
        {
            writer.WriteStartElement("UtilStat");
            UtilStat utilStat = playerStatus.GetUtilStat;
            writer.WriteAttributeString("ResearchTime", utilStat.ResearchTime.ToString());
            writer.WriteAttributeString("ResearchThread", utilStat.ResearchThread.ToString());
            writer.WriteAttributeString("ExpIncreaseXH", utilStat.ExpIncreaseXH.ToString());
            writer.WriteAttributeString("LevelUpPriceXH", utilStat.LevelUpPriceXH.ToString());
            writer.WriteAttributeString("ResearchPriceXH", utilStat.ResearchPriceXH.ToString());
            writer.WriteAttributeString("InitExp", utilStat.InitExp.ToString());
            writer.WriteEndElement();
        }
        {
            writer.WriteStartElement("HeroStat");
            HeroStat heroStat = playerStatus.GetHeroStat;
            writer.WriteAttributeString("BonusLevel", heroStat.BonusLevel.ToString());
            writer.WriteAttributeString("DpcX", heroStat.DpcX.ToString());
            writer.WriteAttributeString("AttackCapacity", heroStat.AttackCapacity.ToString());
            writer.WriteAttributeString("DpcIncreaseXH", heroStat.DpcIncreaseXH.ToString());
            writer.WriteAttributeString("AttackSpeedXH", heroStat.AttackSpeedXH.ToString());
            writer.WriteAttributeString("CriticalRateXH", heroStat.CriticalRateXH.ToString());
            writer.WriteAttributeString("CriticalDamageXH", heroStat.CriticalDamageXH.ToString());
            writer.WriteAttributeString("MovespeedXH", heroStat.MovespeedXH.ToString());
            writer.WriteEndElement();
        }
        {
            writer.WriteStartElement("ElementalStats");
            Dictionary<string, ElementalStat> stats = playerStatus.GetElementalStats;
            foreach (string id in stats.Keys)
            {
                writer.WriteStartElement("ElementalStat");
                ElementalStat eStat = playerStatus.GetElementalStat(id);
                writer.WriteAttributeString("id", id);
                writer.WriteAttributeString("BonusLevel", eStat.BonusLevel.ToString());
                writer.WriteAttributeString("DpcX", eStat.DpsX.ToString());
                writer.WriteAttributeString("AttackCapacity", eStat.DpsIncreaseXH.ToString());
                writer.WriteAttributeString("DpcIncreaseXH", eStat.CastSpeedXH.ToString());
                writer.WriteAttributeString("AttackSpeedXH", eStat.SkillRateIncreaseXH.ToString());
                writer.WriteAttributeString("CriticalRateXH", eStat.SkillDmgIncreaseXH.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
        writer.WriteEndElement();
    }
    private static void SaveEquipments(XmlWriter writer)
    {
        var eManager = Singleton<EquipmentManager>.getInstance();
        var equipSets = eManager.equipSets;
        var inventory = eManager.inventory;
        int CurrentSet = equipSets.IndexOf(eManager.currentEquipSet);

        writer.WriteStartElement("Equipments");
        writer.WriteAttributeString("MaxEquipSet", eManager.MaxEquipSet.ToString());
        writer.WriteAttributeString("MaxInventory", eManager.MaxInventory.ToString());
        writer.WriteAttributeString("CurrentSet", CurrentSet.ToString());
        for (int i = 0; i < equipSets.Count; i++)
        {
            writer.WriteStartElement("EquipmentSet");
            foreach (Equipment.Type type in Enum.GetValues(typeof(Equipment.Type)))
            {
                writer.WriteStartElement(type.ToString());
                if (equipSets[i].ContainsKey(type))
                    WriteEquipment(writer, equipSets[i][type]);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
        {
            writer.WriteStartElement("Inventory");
            foreach (var equipment in inventory)
                WriteEquipment(writer, equipment);
            writer.WriteEndElement();
        }
        writer.WriteEndElement();
    }
    private static void WriteEquipment(XmlWriter writer, Equipment e)
    {
        writer.WriteStartElement("Equipment");
        writer.WriteAttributeString("id", e.dataID);
        writer.WriteAttributeString("grade", e.grade.ToString());
        writer.WriteAttributeString("evolution", e.evolution.ToString());
        writer.WriteAttributeString("exp", e.exp.ToString());
        writer.WriteAttributeString("skillLevel", e.skillLevel.ToString());
        writer.WriteAttributeString("isLock", e.isLock.ToString());
        writer.WriteEndElement();
    }



    //    foreach (XmlNode node in StatNodes)
    //    {
    //        ElementalData elemental = new ElementalData();
    //        elemental.ID = node.Attributes["id"].Value;
    //        elemental.Name = node.Attributes["name"].Value;
    //        elemental.BaseCastIntervalXK = int.Parse(node.Attributes["basecastintervalxk"].Value);
    //        elemental.BaseDmgDensity = double.Parse(node.Attributes["basedensity"].Value);
    //        elemental.BasePrice = int.Parse(node.Attributes["baseprice"].Value);
    //        elemental.BaseLevel = int.Parse(node.Attributes["baselevel"].Value);
    //        elemental.PrefabName = node.Attributes["prefab"].Value;
    //        elemental.SkillPrefabName = node.Attributes["skillprefab"].Value;
    //        elemental.BaseSkillRateXK = int.Parse(node.Attributes["baseskillratexk"].Value);
    //        elemental.BaseSkillDamageXH = int.Parse(node.Attributes["baseskilldamagexh"].Value); ;
    //        elementaltable.Add(elemental.ID, elemental);
    //    }



    //public static void ToXMLFile(System.Object obj, string filePath)
    //{
    //    XmlSerializer serializer = new XmlSerializer(typeof(HeroStat));

    //    XmlWriterSettings settings = new XmlWriterSettings();
    //    settings.Indent = true;
    //    settings.NewLineOnAttributes = true;

    //    using (XmlWriter writer = XmlWriter.Create(filePath, settings))
    //    {
    //        serializer.Serialize(writer, obj);
    //    }
    //}

}