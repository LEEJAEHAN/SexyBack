using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

internal class SaveSystem
    {
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
            FileStream file = File.Open(Application.persistentDataPath + "/"+ filename, FileMode.Open);
            if (file != null && file.Length > 0)
            {
                loaddata = bf.Deserialize(file);
            }
            file.Close();
            return loaddata;
        }

        //internal static void SaveInstacne()
        //{
        //    PlayerPrefs.SetString("InstanceData", "Yes");
        //}
        internal static bool CanLoad()
        {
            if (System.IO.File.Exists(Application.persistentDataPath + "/statmanager.dat"))
                return true;
            else
                return false;
            // return PlayerPrefs.HasKey("InstanceData");

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

    }