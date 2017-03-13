using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SexyBackPlayScene
{
    internal class SaveSystem
    {
        internal static void Save(object target, string filename)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(filename);
            bf.Serialize(file, target);
            file.Close();
        }

        internal static object Load(string filename)
        {
            object loaddata = null;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filename, FileMode.Open);
            if (file != null && file.Length > 0)
            {
                loaddata = bf.Deserialize(file);
            }
            file.Close();
            return loaddata;
        }
    }
}