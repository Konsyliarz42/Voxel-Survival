using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveGame(Ui_menu_control canvas)
    {
        string path = Application.persistentDataPath + "/Save.vsf";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(canvas);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadGame()
    {
        string path = Application.persistentDataPath + "/Save.vsf";

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save not found in " + path);
            FileStream stream = new FileStream(path, FileMode.Create);
            stream.Close();
            return null;
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------

    public static void SaveStatus(Ui_game_control canvas)
    {
        string path = Application.persistentDataPath + "/Score.vsf";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        GameStatus data = new GameStatus(canvas);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameStatus LoadStatus()
    {
        string path = Application.persistentDataPath + "/Score.vsf";

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameStatus data = formatter.Deserialize(stream) as GameStatus;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Score not found in " + path);
            return null;
        }
    }
}
