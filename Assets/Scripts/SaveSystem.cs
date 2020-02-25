using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/levelData.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveAndExit data = new SaveAndExit();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveAndExit LoadData()
    {
        string path = Application.persistentDataPath + "/levelData.fun";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveAndExit data = formatter.Deserialize(stream) as SaveAndExit;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }

}
