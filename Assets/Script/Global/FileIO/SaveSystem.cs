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


    internal static XmlDocument LoadXml(string path)
    {
        //SaveSystem.LoadEquipments()
        //TextAsset textasset = Resources.Load(SaveDataPath) as TextAsset;
        //XmlDocument xmldoc = new XmlDocument();
        //xmldoc.LoadXml(SaveDataPath);
        //xmldoc.LoadXml(textasset.text);
        
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.Load(path);
        return xmldoc;
    }

    internal static void SaveGlobalData()
    {
        XmlWriterSettings setting = new XmlWriterSettings();
        setting.Indent = true;

        XmlWriter writer = XmlWriter.Create(SaveDataPath, setting);
        writer.WriteStartDocument();
        writer.WriteStartElement("PlayerStatus");
        SaveEquipments(writer);
        SaveMapInfo(writer);
        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Close();
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
        writer.WriteAttributeString("skillid", e.skillID);
        writer.WriteAttributeString("grade", e.grade.ToString());
        writer.WriteAttributeString("evolution", e.evolution.ToString());
        writer.WriteAttributeString("exp", e.exp.ToString());
        writer.WriteAttributeString("skillLevel", e.skillLevel.ToString());
        writer.WriteAttributeString("isLock", e.isLock.ToString());
        writer.WriteEndElement();
    }
    private static void SaveMapInfo(XmlWriter writer)
    {
        MapManager manager = Singleton<MapManager>.getInstance();
        writer.WriteStartElement("Progress");
        var dictonary = manager.Maps;
        foreach (string id in dictonary.Keys)
        {
            var record = dictonary[id];
            if (record.ClearCount <= 0)
                continue;
            writer.WriteStartElement("Map");
            writer.WriteAttributeString("id", id);
            writer.WriteAttributeString("clearcount", record.ClearCount.ToString());
            writer.WriteAttributeString("besttime", record.BestTime.ToString());
            writer.WriteEndElement();
        }
        writer.WriteEndElement();
    }

        
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