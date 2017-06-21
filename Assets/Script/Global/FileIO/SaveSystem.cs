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
        SaveTalents(writer);
        SaveStat(writer);       // for test, 로드는 하지 않는다.
        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Close();
    }


    private static void SaveStat(XmlWriter writer)
    {
        PlayerStatus playerStatus = Singleton<PlayerStatus>.getInstance();
        writer.WriteStartElement("Stats");
        {
            writer.WriteStartElement("GlobalStat");
            GlobalStat globalStat = playerStatus.GetGlobalStat;
            writer.WriteAttributeString("InitExp", globalStat.InitExpCoef.ToString());
            writer.WriteAttributeString("RankBonus", globalStat.RankBonus.ToString());
            writer.WriteEndElement();
        }
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
            writer.WriteAttributeString("ResearchTimeX", utilStat.ResearchTimeX.ToString());
            writer.WriteAttributeString("ResearchTime", utilStat.ResearchTime.ToString());
            writer.WriteAttributeString("MaxResearchThread", utilStat.MaxResearchThread.ToString());
            writer.WriteAttributeString("ExpIncreaseXH", utilStat.ExpIncreaseXH.ToString());
            writer.WriteAttributeString("LPriceReduceXH", utilStat.LPriceReduceXH.ToString());
            writer.WriteAttributeString("RPriceReduceXH", utilStat.RPriceReduceXH.ToString());
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
                writer.WriteAttributeString("DpsX", eStat.DpsX.ToString());
                writer.WriteAttributeString("DpsIncreaseXH", eStat.DpsIncreaseXH.ToString());
                writer.WriteAttributeString("CastSpeedXH", eStat.CastSpeedXH.ToString());
                writer.WriteAttributeString("SkillRateIncreaseXH", eStat.SkillRateIncreaseXH.ToString());
                writer.WriteAttributeString("SkillDmgIncreaseXH", eStat.SkillDmgIncreaseXH.ToString());
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
            foreach (Equipment.Slot type in Enum.GetValues(typeof(Equipment.Slot)))
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
        writer.WriteAttributeString("level", e.level.ToString());
        writer.WriteAttributeString("grade", e.grade.ToString());
        writer.WriteAttributeString("limit", e.limit.ToString());
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
            writer.WriteAttributeString("bestrank", record.BestRank.ToString());
            writer.WriteEndElement();
        }
        writer.WriteEndElement();
    }
    private static void SaveTalents(XmlWriter writer)
    {
        TalentManager manager = Singleton<TalentManager>.getInstance();
        writer.WriteStartElement("Talents");
        writer.WriteAttributeString("Reputation", manager.Reputation.ToString());
        writer.WriteAttributeString("SpendReputation", manager.SpendReputation.ToString());
        writer.WriteAttributeString("CurrentTalent", manager.CurrentTalent.ID);
        var dictonary = manager.Talents;
        foreach (KeyValuePair<string, Talent> data  in dictonary)
        {
            writer.WriteStartElement("Talent");
            writer.WriteAttributeString("id", data.Key);
            writer.WriteAttributeString("level", data.Value.Level.ToString());
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