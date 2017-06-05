using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using System.Xml;

public class GlobalGameManager : IDisposable
{
    ~GlobalGameManager()
    {
        sexybacklog.Console("GlobalGameManager 소멸");
    }

    // View
    //GameInfoView infoView;
    // member
    bool LoadComplete = false;

    // Use this for initialization
    internal bool LoadPlayerData()
    {
        if (LoadComplete)
            return true;

        if (SaveSystem.GlobalDataExist)
        {
            try
            {
                Singleton<EquipmentManager>.getInstance().Load();
                Singleton<TalentManager>.getInstance().Load();
                Singleton<PremiumManager>.getInstance().Load();
                Singleton<MapManager>.getInstance().Load();
            }
            catch( Exception e )
            {
                sexybacklog.Console("글로벌 데이터 로드에 이상이 있었습니다." + e.Message);
                //InstanceSaveSystem.ClearInstance();
                LoadComplete = false;
                return LoadComplete;
            }
        }
        else
        {
            Singleton<EquipmentManager>.getInstance().NewData();
            Singleton<TalentManager>.getInstance().NewData();
            Singleton<PremiumManager>.getInstance().NewData();
            Singleton<MapManager>.getInstance().NewData();
        }

        LoadComplete = true;
        return LoadComplete;
    }
    internal void ReBuildStat()
    {
        Singleton<PlayerStatus>.getInstance().Init();
        Singleton<PlayerStatus>.getInstance().ReCheckStat();
    }


    internal void SaveGlobalData()
    {
        if(LoadComplete)
            SaveSystem.SaveGlobalData();
    }

    public void EndGame() // 메뉴로 버튼을 눌렀을때,
    {
    }

    public void Dispose()
    {
        Singleton<EquipmentManager>.Clear();
        Singleton<TalentManager>.Clear();
        Singleton<PremiumManager>.Clear();
        Singleton<MapManager>.Clear();
        Singleton<PlayerStatus>.Clear();
    }
}

