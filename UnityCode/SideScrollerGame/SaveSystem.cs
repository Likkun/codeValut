using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{

    public static void SaveData( GameState gs )
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/data.hpg";

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, gs);
        stream.Close();

    }

    public static GameState LoadData()
    {
        string path = Application.persistentDataPath + "/data.hpg";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameState data = formatter.Deserialize(stream) as GameState;

            stream.Close();

            return data;

        }
        else
        {
            Debug.LogError("SaveFile Not Found");
            return null;
        }

    }

}

